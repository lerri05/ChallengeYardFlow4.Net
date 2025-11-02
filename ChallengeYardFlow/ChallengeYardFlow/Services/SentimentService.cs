using Microsoft.ML;
using Microsoft.ML.Data;

namespace ChallengeYardFlow.Services
{
    public sealed class SentimentService
    {
        private readonly MLContext _mlContext;
        private readonly ITransformer _model;
        private readonly PredictionEngine<SentimentData, SentimentPrediction> _engine;

        public SentimentService()
        {
            _mlContext = new MLContext(seed: 1);

            var trainingData = _mlContext.Data.LoadFromEnumerable(new[]
            {
                new SentimentData { Text = "amo esta moto", Label = true },
                new SentimentData { Text = "excelente experiência", Label = true },
                new SentimentData { Text = "péssimo atendimento", Label = false },
                new SentimentData { Text = "horrível e caro", Label = false },
                new SentimentData { Text = "ótimo custo benefício", Label = true },
                new SentimentData { Text = "não recomendo", Label = false }
            });

            var pipeline = _mlContext.Transforms.Text.FeaturizeText("Features", nameof(SentimentData.Text))
                .Append(_mlContext.BinaryClassification.Trainers.SdcaLogisticRegression());

            _model = pipeline.Fit(trainingData);
            _engine = _mlContext.Model.CreatePredictionEngine<SentimentData, SentimentPrediction>(_model);
        }

        public SentimentPrediction Predict(string text)
        {
            return _engine.Predict(new SentimentData { Text = text });
        }
    }

    public sealed class SentimentData
    {
        public string Text { get; set; } = string.Empty;

        [ColumnName("Label")]
        public bool Label { get; set; }
    }

    public sealed class SentimentPrediction
    {
        [ColumnName("PredictedLabel")]
        public bool Prediction { get; set; }

        public float Probability { get; set; }
        public float Score { get; set; }
    }
}


