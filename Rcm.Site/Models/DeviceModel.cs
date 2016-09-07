using System;

namespace Rcm.Site.Models {
    [Serializable]
    public class DeviceModel {
        public int id { get; set; }

        public string name { get; set; }

        public string type { get; set; }
    }
}