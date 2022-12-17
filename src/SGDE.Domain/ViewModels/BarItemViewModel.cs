using System.Collections.Generic;

namespace SGDE.Domain.ViewModels
{
    public class BarItemViewModel
    {
        public string name { get; set; }
        public string[] labels { get; set; }
        public List<Dataset> datasets { get; set; }
    }

    public class Dataset
    {
        public string label { get; set; }
        public string backgroundColor { get; set; }
        public string borderColor { get; set; }
        public double borderWidth { get; set; } = 1;
        public string hoverBackgroundColor => backgroundColor;
        public string hoverBorderColor => borderColor;
        public List<double> data { get; set; } 
    }
}
