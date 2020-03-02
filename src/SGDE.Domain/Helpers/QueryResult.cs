namespace SGDE.Domain.Helpers
{
    #region Using

    using System.Collections.Generic;

    #endregion

    public class QueryResult <T>
    {
        public List <T> Data { get; set; }
        public long Count { get; set; }
    }
}
