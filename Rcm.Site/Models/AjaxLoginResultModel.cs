using System;

namespace Rcm.Site.Models {
    [Serializable]
    public partial class AjaxLoginResultModel {

        public bool success { get; set; }

        public int code { get; set; }

        public string message { get; set; }

        public int roleId { get; set; }
    }
}