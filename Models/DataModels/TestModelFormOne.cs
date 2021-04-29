using System;
using System.Collections.Generic;

namespace Blazor.DynamicForms.Client.Models.DataModels
{
    public class TestModelFormOne
    {
        public DateTime OrderDate { get; set; }
        public string OrderType { get; set; }
        public string OrderCategory { get; set; }
        public string OrderPriority { get; set; }
        public string OrderPriorityReason { get; set; }
        public string SlabType1 { get; set; }
        public string SlabType2 { get; set; }
        public string SampleStorage { get; set; }
        public string Bore { get; set; }
        public List<TableModel> SubModels { get; set; }
    }
}