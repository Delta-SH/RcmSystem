using Rcm.Core.NPOI;
using System;
using System.Drawing;
using System.Web.Script.Serialization;

namespace Rcm.Site.Models {
    [Serializable]
    public class HisAlarmModel {
        [ExcelDisplayName("序号")]
        public int index { get; set; }

        [ExcelIgnore]
        public int id { get; set; }

        [ExcelIgnore]
        public int level { get; set; }

        [ExcelColor]
        [ExcelDisplayName("告警级别")]
        public string levelDisplay { get; set; }

        [ExcelDisplayName("所属区域")]
        public string area { get; set; }

        [ExcelDisplayName("所属站点")]
        public string station { get; set; }

        [ExcelDisplayName("设备名称")]
        public string device { get; set; }

        [ExcelDisplayName("信号名称")]
        public string point { get; set; }

        [ExcelDisplayName("告警描述")]
        public string comment { get; set; }

        [ExcelDisplayName("触发值")]
        public double alarmValue { get; set; }

        [ExcelDisplayName("恢复值")]
        public double endValue { get; set; }

        [ExcelDisplayName("告警时间")]
        public string startTime { get; set; }

        [ExcelDisplayName("结束时间")]
        public string endTime { get; set; }

        [ExcelDisplayName("确认人员")]
        public string confirmer { get; set; }

        [ExcelDisplayName("确认时间")]
        public string confirmedTime { get; set; }

        [ExcelDisplayName("告警历时")]
        public string interval { get; set; }

        [ExcelDisplayName("结束类型")]
        public string endType { get; set; }

        [ScriptIgnore]
        [ExcelBackground]
        public Color background { get; set; }
    }
}