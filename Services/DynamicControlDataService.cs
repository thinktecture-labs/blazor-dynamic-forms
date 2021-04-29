using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blazor.DynamicForms.Client.Models.FromModels;

namespace Blazor.DynamicForms.Client.Services
{
    public class DynamicControlDataService
    {
        private readonly HttpClient _httpClient;

        public DynamicControlDataService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public Task<Root> LoadFormOneData()
        {
            return _httpClient.GetFromJsonAsync<Root>("sample-data/form1.json");
        }

        public Task<Dictionary<string, Object>> LoadFormData(int index = 1)
        {
            return _httpClient.GetFromJsonAsync<Dictionary<string, Object>>($"sample-data/form{index}-values.json");
        }

        public Task<Root> LoadFormTwoData()
        {
            return _httpClient.GetFromJsonAsync<Root>("sample-data/form2.json");
        }
    }
}