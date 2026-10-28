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
        private readonly string? _loadedModelPath;

        public JJMChatService(IWebHostEnvironment env)
        {
            _env = env ?? throw new ArgumentNullException(nameof(env));
            _mlContext = new MLContext();

            string webRoot = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");

            // Candidate model paths in order of preference
            string[] candidatePaths = new[]
            {
                Path.Combine(_env.ContentRootPath, "AImodel", "digitalgrowth_model.zip"),
                Path.Combine(webRoot, "AImodel", "digitalgrowth_model.zip"),
                Path.Combine(_env.ContentRootPath, "AImodel", "jjm_up_model.zip"),
                Path.Combine(webRoot, "AImodel", "jjm_up_model.zip")
            };

            foreach (var candidate in candidatePaths)
            {
                if (File.Exists(candidate))
                {
                    try
                    {
                        _model = _mlContext.Model.Load(candidate, out _);
                        _predictionEngine = _mlContext.Model.CreatePredictionEngine<JJMModel.Models.JJMInfo, JJMModel.Models.ModelOutput>(_model);
                        _modelLoaded = true;
                        _loadedModelPath = candidate;
                        break;
                    }
                    catch
                    {
                        // Continue checking next candidate
                    }
                }
            }

            if (!_modelLoaded)
            {
                _model = null;
                _predictionEngine = null;
            }
        }

        public bool IsModelLoaded => _modelLoaded;
        public string? LoadedModelPath => _loadedModelPath;

        public string Predict(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return string.Empty;

            if (!_modelLoaded || _predictionEngine == null)
            {
                throw new InvalidOperationException(
                    "ML model is not available. Please ensure 'digitalgrowth_model.zip' or 'jjm_up_model.zip' exists in 'AImodel' or 'wwwroot/AImodel'."
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
