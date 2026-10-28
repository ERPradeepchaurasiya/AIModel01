using Microsoft.ML.Data;

namespace ModelTrainer
{
    public class JJMInfo
    {
        [LoadColumn(0)]
        public string User { get; set; } = string.Empty;

        [LoadColumn(1)]
        public string Assistant { get; set; } = string.Empty;
    }

    public class ModelOutput
    {
        [ColumnName("PredictedLabel")]
        public string PredictedLabel { get; set; } = string.Empty;

        public float[]? Score { get; set; }
    }
}
