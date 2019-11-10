//*****************************************************************************************
//*                                                                                       *
//* This is an auto-generated file by Microsoft ML.NET CLI (Command-Line Interface) tool. *
//*                                                                                       *
//*****************************************************************************************

using Microsoft.ML.Data;

namespace Walterlv.ML.WordGeneratorML.Model.DataModels
{
    public class ModelInput
    {
        [ColumnName("Label"), LoadColumn(0)]
        public bool Label { get; set; }


        [ColumnName("Value"), LoadColumn(1)]
        public string Value { get; set; }


    }
}
