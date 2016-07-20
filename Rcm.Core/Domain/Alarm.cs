using Rcm.Core.Enum;
using System;

namespace Rcm.Core.Domain {
    [Serializable]
    public partial class Alarm {
        public int Id { get; set; }

        public int PointId { get; set; }

        public EnmScType PointType { get; set; }

        public string PointName { get; set; }

        public int DeviceId { get; set; }

        public string DeviceName { get; set; }

        public string StationName { get; set; }

        public string AreaName { get; set; }

        public string DriverName { get; set; }

        public EnmAlarmLevel AlarmLevel { get; set; }

        public double AlarmValue { get; set; }

        public string AlarmDesc { get; set; }

        public int AlarmClassId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime ConfirmTime { get; set; }

        public string ConfirmName { get; set; }

        public DateTime EndTime { get; set; }

        public double EndValue { get; set; }

        public EnmAlarmEnd EndType { get; set; }

        public string AuxSet { get; set; }
    }
}