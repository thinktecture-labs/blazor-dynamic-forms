using System.Collections.Generic;
using MudBlazor;

namespace Blazor.DynamicForms.Client.Models.FromModels
{
    public class DynamicControl
    {
        public string Type { get; set; }
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string Caption { get; set; }
        public bool Validate { get; set; }
        public bool Printable { get; set; }
        public string ErrorText { get; set; }
        public string EmptyText { get; set; }
        public List<string> Items { get; set; }
        public string CheckState { get; set; }
        public Position Position { get; set; }
        public bool? IgnoreLayout { get; set; }
        public bool ShowBorder { get; set; }
        public List<DynamicControl> Controls { get; set; } = new();
        public List<ColumnDefinition> ColumnDefinitions { get; set; }
        public List<RowDefinition> RowDefinitions { get; set; }
        public int BestFitWeight { get; set; }
    }
}