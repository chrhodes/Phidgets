using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

using VNCPhidgets21Explorer.Resources;

namespace VNCPhidgets21Explorer.Presentation.Converters
{

    /// <summary>
    /// Converts to and from a MultiItem Select ComboBox Control
    /// </summary>
    public class SelectedItemsToListOfInterfaceKitSequenceConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return new List<object>((IEnumerable<InterfaceKitSequence>)value);
            }

            return null;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<InterfaceKitSequence> result = new List<InterfaceKitSequence>();
            var enumerable = (List<object>)value;

            if (enumerable != null)
            {
                foreach (object item in enumerable)
                {
                    result.Add((InterfaceKitSequence)item);
                }
            }

            return result;
        }
    }
}
