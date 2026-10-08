using Microsoft.AspNetCore.Hosting;
using Microsoft.ML;
using System;
using System.IO;

namespace JJMModel.Services
{
    public class JJMChatService
    {
        private readonly IWebHostEnvironment _env;
        private readonly MLContext _mlContext;
        private ITransformer? _model;
        private PredictionEngine<JJMModel.Models.JJMInfo, JJMModel.Models.ModelOutput>? _predictionEngine;
        private readonly bool _modelLoaded;

        // Accept IWebHostEnvironment via DI
        public JJMChatService(IWebHostEnvironment env)
        {
            _env = env ?? throw new ArgumentNullException(nameof(env));

            _mlContext = new MLContext();

            string modelPath = Path.Combine(
                _env.ContentRootPath,
                "AImodel",
                "jjm_up_model.zip"
            );

            if (File.Exists(modelPath))
            {
                try
                {
                    _model = _mlContext.Model.Load(
                        modelPath,
                        out _
                    );

                    _predictionEngine =
                        _mlContext.Model.CreatePredictionEngine<JJMModel.Models.JJMInfo, JJMModel.Models.ModelOutput>(
                            _model
                        );

                    _modelLoaded = true;
                }
                catch (Exception)
                {
                    // Loading failed - keep service available but mark model as not loaded
                    _modelLoaded = false;
                    _model = null;
                    _predictionEngine = null;
                }
            }
            else
            {
                // Model file not present at startup. Do not throw here so app can start.
                _modelLoaded = false;
                _model = null;
                _predictionEngine = null;
            }
        }

        public string Predict(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return string.Empty;

            if (!_modelLoaded || _predictionEngine == null)
            {
                throw new InvalidOperationException(
                    "ML model is not available. Ensure the model file 'AImodel/jjm_up_model.zip' exists in the application's content root."
                );
            }

            var input = new JJMModel.Models.JJMInfo
            {
                User = message
            };

            var prediction = _predictionEngine.Predict(input);

            return prediction?.PredictedLabel ?? string.Empty;
        }
    }
}