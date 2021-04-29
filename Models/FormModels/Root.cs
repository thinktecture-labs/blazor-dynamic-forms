using System.Collections.Generic;

namespace Blazor.DynamicForms.Client.Models.FromModels
{
    public class Root
    {
        public string Type { get; set; }
        public string Id { get; set; }
        public bool ShowBorder { get; set; }
        public string LayoutType { get; set; }
        public List<DynamicControl> Controls { get; set; }
    }
}