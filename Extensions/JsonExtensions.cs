using System;
using System.Data;
using System.Text.Json;

namespace Blazor.DynamicForms.Client.Extensions
{
    public static class JsonExtensions
    {
        public static T ToTestObject<T>(this JsonElement element)
        {
            var json = element.GetRawText();
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}