using Rcm.Core.NPOI;
using System;
using System.Drawing;
using System.Web.Script.Serialization;

namespace Rcm.Site.Models {
    [Serializable]
    public class HisDataModel {
        [ExcelDisplayName("序号")]
        public int index { get; set; }

        [ExcelIgnore]
        public int id { get; set; }

        [ExcelDisplayName("所属区域")]
        public string area { get; set; }

        [ExcelDisplayName("所属站点")]
        public string station { get; set; }

        [ExcelDisplayName("设备名称")]
        public string device { get; set; }

        [ExcelDisplayName("信号名称")]
        public string point { get; set; }

        [ExcelDisplayName("信号类型")]
        public string type { get; set; }

        [ExcelDisplayName("信号测值")]
        public float value { get; set; }

        [ExcelDisplayName("单位/描述")]
        public string unit { get; set; }

        [ExcelDisplayName("测值时间")]
        public string time { get; set; }
    }
}