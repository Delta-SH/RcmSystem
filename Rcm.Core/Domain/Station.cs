using System;

namespace Rcm.Core.Domain {
    [Serializable]
    public class Station {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Desc { get; set; }

        public string MID { get; set; }

        public string ExpSet { get; set; }

        public int StationType { get; set; }

        public string StationTypeName { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public int AreaId { get; set; }

        public bool Enabled { get; set; }
    }
}
