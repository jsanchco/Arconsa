namespace SGDE.Domain.Helpers
{
    #region Using

    using System.Globalization;
    using System.Text;

    #endregion

    public static class Searcher
    {
        public static string RemoveAccentsWithNormalization(string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
                return string.Empty;

            var normalizedString = inputString.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            for (int i = 0; i < normalizedString.Length; i++)
            {
                var uc = CharUnicodeInfo.GetUnicodeCategory(normalizedString[i]);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(normalizedString[i]);
                }
            }
            return (sb.ToString().Normalize(NormalizationForm.FormC));
        }
    }
}
