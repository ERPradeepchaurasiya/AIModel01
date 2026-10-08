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
        private readonly ITransformer _model;
        private readonly PredictionEngine<JJMModel.Models.JJMInfo, JJMModel.Models.ModelOutput> _predictionEngine;

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

            if (!File.Exists(modelPath))
            {
                throw new FileNotFoundException(
                    "JJM ML model not found.",
                    modelPath
                );
            }

            _model = _mlContext.Model.Load(
                modelPath,
                out _
            );

            _predictionEngine =
                _mlContext.Model.CreatePredictionEngine<JJMModel.Models.JJMInfo, JJMModel.Models.ModelOutput>(
                    _model
                );
        }

        public string Predict(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return string.Empty;

            var input = new JJMModel.Models.JJMInfo
            {
                User = message
            };

            var prediction = _predictionEngine.Predict(input);

            return prediction?.PredictedLabel ?? string.Empty;
        }
    }
}