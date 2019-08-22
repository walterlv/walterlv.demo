using dotnetCampus.Cli;
using Microsoft.ML;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Walterlv.ML.WordGeneratorML.Model.DataModels;

namespace Walterlv.Demo
{
    class GenerateWordOptions
    {
        [Option('m', "ModelFile")]
        public string ModelFile { get; set; }

        public async Task RunAsync()
        {
            var predEngine = GetPredictionEngine();
            while (true)
            {
                var line = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(line))
                {
                    Console.CursorTop--;
                    continue;
                }
                else if (File.Exists(line))
                {
                    var lines = File.ReadAllLines(line);
                    PrintPrediction(predEngine, lines);
                    continue;
                }

                PrintPrediction(predEngine, line);

                if (line.Equals("exit", StringComparison.CurrentCultureIgnoreCase))
                {
                    break;
                }
            }
        }

        private void PrintPrediction(PredictionEngine<ModelInput, ModelOutput> predEngine, params string[] words)
        {
            Console.CursorTop--;
            foreach (var word in words)
            {
                var prediction = Predict(predEngine, word);

                Console.ForegroundColor = prediction ? ConsoleColor.Green : ConsoleColor.DarkRed;
                Console.WriteLine($"{(prediction ? "✔" : "❌")} {word}");
                Console.ResetColor();
            }
        }

        private PredictionEngine<ModelInput, ModelOutput> GetPredictionEngine()
        {
            // Load the model
            var mlContext = new MLContext();
            ITransformer mlModel = mlContext.Model.Load("MLModel.zip", out var modelInputSchema);
            var predEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);
            return predEngine;
        }

        private bool Predict(PredictionEngine<ModelInput, ModelOutput> predEngine, string value)
        {
            // Use the code below to add input data
            var input = new ModelInput();

            // input.
            input.Value = value;

            // Try model on sample data
            ModelOutput result = predEngine.Predict(input);
            return result.Prediction;
        }
    }
}
