using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Text.Json;
using Blazor.DynamicForms.Client.Extensions;
using Blazor.DynamicForms.Client.Models.FromModels;
using Blazor.DynamicForms.Client.Shared.ResourceFiles;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.CompilerServices;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.Localization;

namespace Blazor.DynamicForms.Client.Services
{
    public class DynamicFormGeneratorService
    {
        private readonly IStringLocalizer<Resource> _localizer;

        public DynamicFormGeneratorService(IStringLocalizer<Resource> localizer)
        {
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        }

        public RenderFragment CreateComponentByFormData(Root controlData, ExpandoObject data) => builder =>
        {
            RenderControls(controlData.Controls, data, builder);
        };

        private void RenderControls(List<DynamicControl> controls, ExpandoObject data, RenderTreeBuilder builder)
        {
            var sectionIndex = 1;
            foreach (var control in controls)
            {
                if (control.Id != "root" && control.Id != "mainGroup")
                {
                    if (control.Id == "tmlInfoGroup")
                    {
                        sectionIndex = RenderTable(data, builder, control, sectionIndex);
                    }
                    else
                    {
                        sectionIndex = RenderSection(data, builder, control, sectionIndex);
                    }
                }

                if (control.Controls?.Count > 0)
                {
                    RenderControls(control.Controls, data, builder);
                }
            }
        }

        private int RenderSection(ExpandoObject data, RenderTreeBuilder builder, DynamicControl control,
            int sectionIndex)
        {
            if (control.Type == "regularLayout")
            {
                builder.OpenElement(0, "div");
                builder.AddAttribute(1, "class", "section");
                builder.OpenElement(2, "span");
                builder.AddContent(3, sectionIndex);
                builder.CloseElement();
                builder.AddContent(2, $"{control.Caption}: ");
                builder.CloseElement();
                sectionIndex++;
            }

            if (control.Controls?.Count > 0)
            {
                builder.OpenElement(0, "div");
                builder.AddAttribute(1, "class", "inner-wrap");
                var itemIndex = 2;
                foreach (var item in control.Controls)
                {
                    builder.AddContent(itemIndex++, builder2 => RenderControl(data, builder2, item));
                }

                builder.CloseElement();
            }

            return sectionIndex;
        }

        private int RenderTable(ExpandoObject data, RenderTreeBuilder builder, DynamicControl control, int sectionIndex)
        {
            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "class", "section");
            builder.OpenElement(2, "span");
            builder.AddContent(3, sectionIndex);
            builder.CloseElement();
            builder.AddContent(2, $"{control.Caption}: ");
            builder.CloseElement();
            sectionIndex++;


            if (control.Controls?.Count > 0)
            {
                builder.OpenElement(0, "div");
                builder.AddAttribute(1, "class", "inner-wrap");
                builder.OpenElement(3, "div");
                builder.AddAttribute(3, "style", "display: grid");
                foreach (var item in control.Controls)
                {
                    builder.AddContent(4, builder2 =>
                    {
                        builder2.OpenElement(0, "div");
                        var col = item.Position.ColumnIndex + 1;
                        builder2.AddAttribute(1, "style", $"grid-column: {col}; grid-row: {item.Position.RowIndex}");
                        builder2.AddContent(2, builder3 => RenderControl(data, builder3, item));
                        builder2.CloseElement();
                    });
                }

                builder.CloseElement();
                builder.CloseElement();
            }

            return sectionIndex;
        }

        private void RenderControl(ExpandoObject data, RenderTreeBuilder builder, DynamicControl control)
        {
            builder.OpenElement(0, "label");
            builder.AddContent(1, control.Caption);
            builder.AddContent(2,
                new RenderFragment(builder2 => RenderInputComponent(data, control, builder2)));
            builder.CloseElement();
        }

        private void RenderInputComponent(ExpandoObject data, DynamicControl control,
            RenderTreeBuilder builder2)
        {
            switch (control.Type)
            {
                case "textBox":
                case "multiLineTextBox":
                    builder2.OpenComponent(0, typeof(InputText));
                    // Create the handler for ValueChanged. I use reflection to the value.

                    BindDataValue<string>(data, control.Id, builder2, control);

                    if (control.Type == "multiLineTextBox")
                    {
                        builder2.AddAttribute(5, "Multiline", true);
                    }

                    builder2.CloseComponent();
                    break;
                case "datePicker":
                    builder2.OpenComponent(0, typeof(InputDate<DateTime>));
                    BindDataValue<DateTime>(data, control.Id, builder2, control);
                    builder2.CloseComponent();
                    break;
                case "dropdown":
                    builder2.OpenComponent(0, typeof(InputSelect<string>));
                    BindDataValue<string>(data, control.Id, builder2, control);
                    builder2.AddAttribute(5, "ChildContent", (RenderFragment) ((builder2) =>
                    {
                        var index = 1;
                        foreach (var item in control.Items)
                        {
                            builder2.OpenElement(6, "option");
                            builder2.AddAttribute(7, "Value", $"Value-{index}");
                            builder2.AddContent(8, _localizer[item]);
                            builder2.CloseElement();
                            builder2.AddMarkupContent(9, "\r\n");
                            index++;
                        }
                    }));
                    builder2.CloseComponent();
                    break;

                case "checkBox":
                    builder2.OpenComponent(0, typeof(InputCheckbox));
                    BindDataValue<bool>(data, control.Id, builder2, control);
                    builder2.CloseComponent();
                    break;
                default:
                    builder2.OpenElement(0, "p");
                    builder2.AddAttribute(1, "style", "padding: 8px 12px  24px");
                    builder2.AddContent(2, "Control is to be implemented :-)");
                    builder2.CloseElement();
                    break;
            }
        }

        private void BindDataValue<T>(ExpandoObject data, string key, RenderTreeBuilder builder, DynamicControl control)
        {
            var accessor = ((IDictionary<string, object>) data);

            object value = default(T);
            accessor.TryGetValue(key, out value);

            value = GetValue<T>(accessor, key);

            var valueChanged = RuntimeHelpers.TypeCheck(
                EventCallback.Factory.Create<T>(
                    this, EventCallback.Factory.CreateInferred(this, __value => accessor[key] = __value,
                        GetValue<T>(accessor, key))));

            var formElementReference = new ValueReference<T>()
            {
                Value = (T) value,
                ValueChanged = valueChanged
            };

            var constantDropDown = Expression.Constant(formElementReference, typeof(ValueReference<T>));
            var exp = Expression.Property(constantDropDown, nameof(ValueReference<T>.Value));
            builder.AddAttribute(1, "Value", formElementReference.Value);
            builder.AddAttribute(3, "ValueChanged", formElementReference.ValueChanged);
            builder.AddAttribute(4, "ValueExpression", Expression.Lambda<Func<T>>(exp));
            builder.AddAttribute(6, "Placeholder", control.EmptyText ?? String.Empty);
        }

        private T GetValue<T>(IDictionary<string, object> data, string key)
        {
            if (data[key] is JsonElement)
            {
                if (typeof(T) == typeof(DateTime))
                {
                    var dateString = ((JsonElement) data[key]).ToTestObject<string>();
                    var date = Convert.ToDateTime(dateString, CultureInfo.InvariantCulture);
                    var jsonString = JsonSerializer.Serialize(date);
                    return JsonSerializer.Deserialize<T>(jsonString);
                }

                if (typeof(T) == typeof(bool))
                {
                    var boolString = ((JsonElement) data[key]).GetRawText();
                    Boolean.TryParse(boolString, out var result);
                    var jsonString = JsonSerializer.Serialize(result);
                    return JsonSerializer.Deserialize<T>(jsonString);
                }

                return ((JsonElement) data[key]).ToTestObject<T>();
            }

            return (T) data[key];
        }
    }
}