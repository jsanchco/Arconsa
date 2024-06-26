﻿using System;

namespace SGDE.Domain.ViewModels
{
    public class ReportQueryViewModel
    {
        public int enterpriseId { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public int? workerId { get; set; }
        public int? workId { get; set; }
        public int? clientId { get; set; }
        public bool showCeros { get; set; }
    }
}
