namespace SGDE.API.Util
{
    #region Using

    using Domain.Helpers;
    using System;

    #endregion

    public static class Helper
    {
        public static string getSearch(string filter)
        {
            if (filter != null)
            {
                var newfiltersplits = filter;
                var filtersplits = newfiltersplits.Split('(', ')', ' ');
                var filterfield = filtersplits[1];

                if (filtersplits.Length == 5)
                {
                    if (filtersplits[1] == "tolower")
                    {
                        filterfield = filter.Split('(', ')', '\'')[2];
                    }
                }
                if (filtersplits.Length != 5)
                {
                    filterfield = filter.Split('(', ')', '\'')[3];
                }

                return Searcher.RemoveAccentsWithNormalization(filterfield);
            }

            return null;
        }

        public static string getSearchLite(string filter)
        {
            if (filter != null)
            {
                var newfiltersplits = filter;
                var filtersplits = newfiltersplits.Split('(', ')', ' ');

                if (filtersplits.Length == 5 && filtersplits[1].Equals("id", StringComparison.CurrentCultureIgnoreCase))
                    return filtersplits[3];

                if (filtersplits.Length == 5 && filtersplits[2].Equals("name", StringComparison.CurrentCultureIgnoreCase))
                {
                    var filterNameSplit = filtersplits[1].Split(",");
                    if (filterNameSplit.Length == 2)
                        return filterNameSplit[0].Replace("'", string.Empty);
                }
            }

            return null;
        }
    }
}
