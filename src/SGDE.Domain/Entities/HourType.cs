namespace SGDE.Domain.Entities
{
    #region Using

    using System.Collections.Generic;

    #endregion

    public class HourType : BaseEntity
    {
        public string Name { get; set; }

        public virtual ICollection<DailySigning> DailySignings { get; set; } = new HashSet<DailySigning>();
    }
}
