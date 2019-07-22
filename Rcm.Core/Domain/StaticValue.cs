using System;

namespace Rcm.Core.Domain {
    [Serializable]
    public class StaticValue {
        public int PointID { get; set; }

        public DateTime BeginTime { get; set; }

        public DateTime EndTime { get; set; }

        public double AvgValue { get; set; }

        public double MaxValue { get; set; }

        public DateTime MaxTime { get; set; }

        public double MinValue { get; set; }

        public DateTime MinTime { get; set; }
    }
}
