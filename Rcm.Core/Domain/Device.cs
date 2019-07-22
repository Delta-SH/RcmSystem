using System;

namespace Rcm.Core.Domain {
    [Serializable]
    public class Device {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Desc { get; set; }

        public int DeviceType { get; set; }

        public string DeviceTypeName { get; set; }

        public int VendorId { get; set; }

        public string Version { get; set; }

        public string DeviceModel { get; set; }

        public DateTime BeginRunTime { get; set; }

        public string MID { get; set; }

        public string ExpSet { get; set; }

        public int TID { get; set; }

        public bool Bind { get; set; }

        public int PId { get; set; }

        public bool Enabled { get; set; }
    }
}
