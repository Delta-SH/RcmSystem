using System;

namespace Rcm.Site.Models {
    [Serializable]
    public class StationModel {
        public int id { get; set; }

        public string name { get; set; }

        public string type { get; set; }

        public string county { get; set; }

        public string city { get; set; }

        public string province { get; set; }
    }
}