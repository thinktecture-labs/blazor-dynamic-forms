using System.Collections.Generic;

namespace Blazor.DynamicForms.Client.Models.DataModels
{
    public class TestModelFormTwo
    {
        public string PrimaryContact { get; set; }
        public string SecondaryContact { get; set; }
        public List<string> PurposeOfAnalysis { get; set; }
        public string OtherPurposeOfAnalysis { get; set; }
        public bool IsMoreSampleAvailable { get; set; }
        public string SolubilityInformation { get; set; }
        public bool Priority { get; set; }
        public string CustomerIssue { get; set; }
        public string CustomerIssueProduct { get; set; }
        public string DeformulationComposition { get; set; }
        public string DeformulationCompositionConfirmation { get; set; }
        public string ResearchAndDevelopmentMaterialType { get; set; }
        public List<string> FormOfOutput { get; set; }
        public List<string> SamplesTestedInOtherLabs { get; set; }
        public string OtherSamplesTestedInOtherLabs { get; set; }
        public string SampleCompositionAndPurity { get; set; }
        public string ReactionScheme { get; set; }
    }
}