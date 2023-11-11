using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

using DevExpress.CodeParser;

using VNCPhidgets21Explorer.Resources;

namespace VNCPhidgets21Explorer.Presentation.Converters
{
    
    /// <summary>
    /// Converts to and from a MultiItem Select ComboBox Control
    /// </summary>
    public class SelectedItemsToListOfAdvancedServoPerformanceConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return new List<object>((IEnumerable<AdvancedServoPerformance>)value);
            }

            return null;
        }

        //object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        //{
        //    //var huh = value.GetType();

        //    //Dictionary<string, Resources.AdvancedServoPerformance> result = new Dictionary<string, Resources.AdvancedServoPerformance>();
        //    ////var enumerable = (Dictionary<string, Resources.AdvancedServoPerformance>)value;
        //    //var enumerable = (List<object>)value;

        //    //if (enumerable != null)
        //    //{
        //    //    foreach (object item in enumerable)
        //    //    {
        //    //        result.Add(item.);
        //    //        //result.Add(item.Key, item.Value)
        //    //        //result.Add(item);
        //    //        //result.Add((Dictionary<string, Resources.AdvancedServoPerformance>)item);
        //    //    }
        //    //}
        //return DependencyProperty.UnsetValue;
        //    //return result;
        //}

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<AdvancedServoPerformance> result = new List<AdvancedServoPerformance>();
            var enumerable = (List<object>)value;

            if (enumerable != null)
            {
                foreach (object item in enumerable)
                {
                    result.Add((AdvancedServoPerformance)item);
                }
            }

            return result;
        }
    }
}
