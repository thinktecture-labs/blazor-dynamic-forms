using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using Blazor.DynamicForms.Client.Models.FromModels;
using Blazor.DynamicForms.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Blazor.DynamicForms.Client.Components
{
    public partial class FormTwo
    {
        private Root _root;

        [Inject] public DynamicControlDataService DynamicControlDataService { get; set; }
        [Inject] public DynamicFormGeneratorService DynamicFormGeneratorService { get; set; }

        private ExpandoObject model = new();
        private EditForm _form;
        

        protected override async Task OnInitializedAsync()
        {
            _root = await DynamicControlDataService.LoadFormTwoData();
            var dict = (IDictionary<string, Object>) model;
            var data = await DynamicControlDataService.LoadFormData(2);
            if (data != null)
            {
                foreach (var keyValuePair in data)
                {
                    dict.Add(keyValuePair);
                }
            }
            else
            {
                Console.WriteLine("Data could not be parsed...");
            }

            await base.OnInitializedAsync();
        }

        private RenderFragment CreateForm()
        {
            return DynamicFormGeneratorService.CreateComponentByFormData(_root, model);
        }
    }
}