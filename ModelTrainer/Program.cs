using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.ML;

namespace ModelTrainer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("===============================================================");
            Console.WriteLine("  Digital Growth IT Software Solution - ML Model Trainer       ");
            Console.WriteLine("  Company: https://digitalgrowthitsoftwaresolution.com/       ");
            Console.WriteLine("===============================================================\n");

            // Define directories
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            // Go up to solution/project directory
            string projectDir = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", ".."));
            string dataDir = Path.Combine(projectDir, "Data");
            string excelPath = Path.Combine(dataDir, "DigitalGrowth_TrainingData.xlsx");
            string csvPath = Path.Combine(dataDir, "DigitalGrowth_TrainingData.csv");

            // Ensure data directory exists
            if (!Directory.Exists(dataDir))
            {
                Directory.CreateDirectory(dataDir);
            }

            // STEP 1: Generate Excel & CSV datasets
            Console.WriteLine("[1/5] Generating training dataset for Digital Growth IT...");
            var rawDataset = Dataset.GetDigitalGrowthDataset();
            Console.WriteLine($"      Generated {rawDataset.Count} training samples.");

            Console.WriteLine($"[2/5] Creating Excel spreadsheet at:\n      {excelPath}");
            ExcelHelper.SaveToExcel(excelPath, rawDataset);
            ExcelHelper.SaveToCsv(csvPath, rawDataset);
            Console.WriteLine("      Excel (.xlsx) and CSV (.csv) generated successfully.\n");

            // STEP 2: Read training data FROM the Excel file
            Console.WriteLine($"[3/5] Loading training data directly from Excel file...");
            List<JJMInfo> trainingData = ExcelHelper.LoadFromExcel(excelPath);
            Console.WriteLine($"      Successfully read {trainingData.Count} records from {Path.GetFileName(excelPath)}.");

            var distinctLabels = trainingData.Select(x => x.Assistant).Distinct().Count();
            Console.WriteLine($"      Distinct response categories (classes): {distinctLabels}\n");

            // STEP 3: Train the ML.NET model
            Console.WriteLine("[4/5] Training ML.NET Multiclass Classification Pipeline...");
            var mlContext = new MLContext(seed: 42);

            IDataView dataView = mlContext.Data.LoadFromEnumerable(trainingData);

            // Pipeline:
            // 1. Map string Assistant (Label) to Key
            // 2. Featurize User query string into numerical feature vectors
            // 3. Train using SDCA Maximum Entropy classifier
            // 4. Map predicted Key back to string PredictedLabel
            var trainingPipeline = mlContext.Transforms.Conversion.MapValueToKey(
                    outputColumnName: "Label",
                    inputColumnName: nameof(JJMInfo.Assistant))
                .Append(mlContext.Transforms.Text.FeaturizeText(
                    outputColumnName: "Features",
                    inputColumnName: nameof(JJMInfo.User)))
                .Append(mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy(
                    labelColumnName: "Label",
                    featureColumnName: "Features"))
                .Append(mlContext.Transforms.Conversion.MapKeyToValue(
                    outputColumnName: "PredictedLabel",
                    inputColumnName: "PredictedLabel"));

            Console.WriteLine("      Fitting model on training data...");
            var trainedModel = trainingPipeline.Fit(dataView);
            Console.WriteLine("      Model training complete!\n");

            // STEP 4: Test & Verify the Model
            Console.WriteLine("[5/5] Running verification tests on sample queries:");
            var predictionEngine = mlContext.Model.CreatePredictionEngine<JJMInfo, ModelOutput>(trainedModel);

            string[] testQueries = new[]
            {
                "Hello, who are you?",
                "How can I contact your support team?",
                "Where is Digital Growth IT office located?",
                "Do you build Binary MLM software?",
                "Can you create an Android mobile app for my business?",
                "What features are included in your School ERP?",
                "How much does website development cost?"
            };

            foreach (var query in testQueries)
            {
                var input = new JJMInfo { User = query };
                var result = predictionEngine.Predict(input);
                Console.WriteLine($"\n  [User Query] : \"{query}\"");
                string preview = result.PredictedLabel;
                if (preview.Length > 120) preview = preview.Substring(0, 117) + "...";
                Console.WriteLine($"  [AI Answer]  : {preview}");
            }

            // STEP 5: Save Model to Target Locations
            Console.WriteLine("\n---------------------------------------------------------------");
            Console.WriteLine("Saving model files...");

            // Output targets:
            var targets = new List<string>
            {
                Path.Combine(projectDir, "AImodel", "digitalgrowth_model.zip"),
                Path.Combine(projectDir, "wwwroot", "AImodel", "digitalgrowth_model.zip"),
                Path.Combine(projectDir, "wwwroot", "AImodel", "jjm_up_model.zip"), // Replace legacy model
                Path.Combine(baseDir, "digitalgrowth_model.zip")
            };

            foreach (var targetPath in targets)
            {
                var dir = Path.GetDirectoryName(targetPath);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                mlContext.Model.Save(trainedModel, dataView.Schema, targetPath);
                Console.WriteLine($"  -> Saved: {targetPath}");
            }

            Console.WriteLine("\n[SUCCESS] Model generation and training finished successfully!");
            Console.WriteLine("===============================================================");
        }
    }
}
