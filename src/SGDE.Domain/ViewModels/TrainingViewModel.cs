namespace SGDE.Domain.ViewModels
{
    #region Using

    using System;

    #endregion

    public class TrainingViewModel : BaseEntityViewModel
    {
        public string name { get; set; }
        public double hours { get; set; }
        public string center { get; set; }
        public string address { get; set; }
        public byte[] file { get; set; }

        public int userId { get; set; }
        public string userName { get; set; }
    }
}
