namespace SGDE.Domain.Helpers
{
    #region Using

    using System.Globalization;


    #endregion

    public static class ExtensionsMethods
    {
        public static string ToFormatSpain(this double value)
        {
            var result = value.ToString("N", CultureInfo.CreateSpecificCulture("es-ES"));

            //var find = result.IndexOf(",00");
            //if (find > 0)
            //    result = result.Substring(0, find);

            return result;
        }
    }
}
