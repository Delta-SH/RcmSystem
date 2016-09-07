using System;

namespace Rcm.Site.Models {
    public class PointModel {
        public int id { get; set; }

        public string name { get; set; }

        public int type { get; set; }

        public string typeDisplay { get; set; }

        public float value { get; set; }

        public string valueDisplay { get; set; }

        public int status { get; set; }

        public string statusDisplay { get; set; }

        public string timestamp { get; set; }

        public string shortTime { get; set; }
    }
}