using System;

namespace Rcm.Site.Models {
    [Serializable]
    public partial class GVideoModel {
        public int id { get; set; }

        public bool enabled { get; set; }

        public string name { get; set; }

        public int type { get; set; }

        public string ip { get; set; }

        public int port { get; set; }

        public string uid { get; set; }

        public string pwd { get; set; }

        public string auxSet { get; set; }

        //public string imgIp { get; set; }

        public int imgPort { get; set; }

        public int portId { get; set; }
    }
}