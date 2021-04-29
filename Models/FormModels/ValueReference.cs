using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;

namespace Blazor.DynamicForms.Client.Models.FromModels
{
    public class ValueReference<T>
    {
        private T _value;

        public T Value
        {
            get { return _value; }
            set
            {
                _value = value;
                if (ValueChanged.HasDelegate)
                    ValueChanged.InvokeAsync(_value);
            }
        }

        public EventCallback<T> ValueChanged { get; set; }
        public Expression<Func<T>> ValueExpression { get; internal set; }

        public static void SetValue(object model, string key, T value)
        {
            var modelType = model.GetType();

            if (modelType == typeof(ExpandoObject))
            {
                var accessor = ((IDictionary<string, object>) model);
                accessor[key] = value;
            }
            else
            {
                var propertyInfo = modelType.GetProperty(key);
                propertyInfo.SetValue(model, value);
            }
        }

        public static T GetValue(object model, string key)
        {
            var modelType = model.GetType();

            if (modelType == typeof(ExpandoObject))
            {
                var accessor = ((IDictionary<string, object>) model);
                return (T) accessor[key];
            }
            else
            {
                var propertyInfo = modelType.GetProperty(key);
                return (T) propertyInfo.GetValue(model);
            }
        }
    }
}