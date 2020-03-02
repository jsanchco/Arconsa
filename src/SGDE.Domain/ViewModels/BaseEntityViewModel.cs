// ReSharper disable InconsistentNaming
namespace SGDE.Domain.ViewModels
{
    #region Using

    using System;

    #endregion

    public class BaseEntityViewModel
    {
        public int? id { get; set; }
        public DateTime? addedDate { get; set; }
        public DateTime? modifiedDate { get; set; }
        public string iPAddress { get; set; }

        #region Constructor

        public BaseEntityViewModel()
        {
            addedDate = DateTime.Now;
            modifiedDate = DateTime.Now;
        }

        #endregion
    }
}
