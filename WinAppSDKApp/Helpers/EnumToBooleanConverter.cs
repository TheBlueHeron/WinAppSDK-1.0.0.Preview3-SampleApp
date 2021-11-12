using Microsoft.UI.Xaml.Data;
using System;

namespace GoogleMapper.Helpers
{
    /// <summary>
    /// Converts <see cref="Enum"/> values to <see cref="bool"/> values.
    /// </summary>
    public class EnumToBooleanConverter
        : IValueConverter
    {
        #region Objects and variables

        private const string _errParam = "parameter must be the name of an Enum";
        private const string _errValue = "value must be an Enum";

        #endregion

        /// <summary>
        /// Gets or sets the <see cref="Type"/> of the <see cref="Enum"/> to convert.
        /// </summary>
        public Type EnumType { get; set; }

        /// <summary>
        /// Converts the <see cref="Enum"/> value to a boolean value.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (parameter is string)
            {
                if (!Enum.IsDefined(EnumType, value))
                {
                    throw new ArgumentException(_errValue);
                }

                object enumValue = Enum.Parse(EnumType, parameter.ToString());

                return enumValue.Equals(value);
            }
            throw new ArgumentException(_errParam);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            string enumString = parameter as string;

            if (enumString is not null)
            {
                return Enum.Parse(EnumType, enumString);
            }
            throw new ArgumentException(_errParam);
        }
    }
}
