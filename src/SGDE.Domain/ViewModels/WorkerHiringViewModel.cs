namespace SGDE.Domain.ViewModels
{
    #region Using

    using System;

    #endregion

    public class WorkerHiringViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string dni { get; set; }
        public int? workId { get; set; }
        public string workName { get; set; }
        public string dateStart { get; set; }
        public int state { get; set; }
    }
}
