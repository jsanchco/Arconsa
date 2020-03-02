namespace SGDE.API.Util
{
    #region Using

    using Domain.Helpers;

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
    }
}
