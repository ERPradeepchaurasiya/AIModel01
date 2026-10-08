using Microsoft.ML.Data;

namespace JJMModel.Models
{

    public class ChatRequest
    {
        public string Message { get; set; }
    }


    public class JJMInfo
    {
        [LoadColumn(0)] public string User { get; set; }
        [LoadColumn(1)] public string Assistant { get; set; }
    }

    public class ModelOutput
    {
        [ColumnName("PredictedLabel")]
        public string PredictedLabel { get; set; }
    }
}