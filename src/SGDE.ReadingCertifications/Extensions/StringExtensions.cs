using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Globalization;

namespace SGDE.ReadingCertifications.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveValueStart(this string source, string value)
        {
            if (!source.StartsWith(value))
                return source;

            return source.Substring(value.Length);
        }

        public static string RemoveValueEnd(this string source, string value)
        {
            if (!source.EndsWith(value))
                return source;

            return source.Remove(source.LastIndexOf(value));
        }

        public static double? ToDouble(this string source)
        {
            source = source
                .Replace(".", string.Empty)
                .Replace(",", ".")
                .Replace("%", string.Empty)
                .Replace("(", string.Empty)
                .Replace(")", string.Empty)
                .Trim();

            if (double.TryParse(source, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
            {
                result = Math.Round(result, 2);
                return result;
            }

            return null;
        }
    }
}
