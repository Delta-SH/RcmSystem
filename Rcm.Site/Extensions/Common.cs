using Rcm.Core.Domain;
using Rcm.Core.Enum;
using System;
using System.Drawing;
using System.Web;
using System.Web.Security;

namespace Rcm.Site.Extensions {
    public abstract class Common {
        public static int GroupId {
            get {
                var httpContext = HttpContext.Current;
                if(httpContext == null ||
                   httpContext.Request == null ||
                   !httpContext.Request.IsAuthenticated ||
                   !(httpContext.User.Identity is FormsIdentity)) {
                    throw new Exception("Unauthorized");
                }

                var authCookie = httpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
                if(authCookie == null)
                    throw new Exception("Cookie not found.");

                var ticket = FormsAuthentication.Decrypt(authCookie.Value);
                if(ticket == null)
                    throw new Exception("Encrypted ticket is invalid.");

                return int.Parse(ticket.UserData);
            }
        }

        public static string GlobalSeparator {
            get { return "┆"; }
        }

        public static string JoinKeys(params object[] keys) {
            if(keys == null || keys.Length == 0)
                return string.Empty;

            return string.Join(GlobalSeparator, keys);
        }

        public static string[] SplitKeys(string key) {
            if(string.IsNullOrWhiteSpace(key))
                return new string[] { };

            return key.Split(new string[] { GlobalSeparator }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string GetScTypeDisplay(EnmScType type) {
            switch(type) {
                case EnmScType.None:
                    return "未知";
                case EnmScType.LSC:
                    return "监控中心";
                case EnmScType.Area:
                    return "区域";
                case EnmScType.Station:
                    return "局站";
                case EnmScType.Device:
                    return "设备";
                case EnmScType.Aic:
                    return "遥测";
                case EnmScType.Aoc:
                    return "遥调";
                case EnmScType.Dic:
                    return "遥信";
                case EnmScType.Doc:
                    return "遥控";
                case EnmScType.Sic:
                    return "字符输入量";
                case EnmScType.Soc:
                    return "字符输出量";
                case EnmScType.Pic:
                    return "图片输入量";
                case EnmScType.Poc:
                    return "图片输出量";
                case EnmScType.Vic:
                    return "视频输入量";
                case EnmScType.Voc:
                    return "视频输出量";
                case EnmScType.ADic:
                    return "音频输入量";
                case EnmScType.ADoc:
                    return "音频输出量";
                case EnmScType.Str:
                    return "字符";
                case EnmScType.Img:
                    return "图片";
                case EnmScType.FS:
                    return "前置机";
                case EnmScType.Bus:
                    return "总线控制器";
                case EnmScType.Driver:
                    return "总线控制器";
                default:
                    return "未定义";
            }
        }

        public static string GetPointStatusDisplay(EnmPointStatus status) {
            switch(status) {
                case EnmPointStatus.Normal:
                    return "正常数据";
                case EnmPointStatus.Level1:
                    return "一级告警";
                case EnmPointStatus.Level2:
                    return "二级告警";
                case EnmPointStatus.Level3:
                    return "三级告警";
                case EnmPointStatus.Level4:
                    return "四级告警";
                case EnmPointStatus.Opevent:
                    return "操作事件";
                case EnmPointStatus.Invalid:
                    return "无效数据";
                case EnmPointStatus.NullValue:
                    return "通信中断";
                default:
                    return "Undefined";
            }
        }

        public static string GetValueDisplay(Rcm.Core.Domain.Point current, float value) {
            switch(current.Type) {
                case EnmScType.Dic:
                case EnmScType.Doc:
                    var result = string.Empty;
                    var units = (current.Comment ?? string.Empty).Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach(var u in units) {
                        var vs = u.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                        if(vs.Length != 2) continue;

                        var flag = ((int)value).ToString();
                        if(vs[0].Trim() == flag) {
                            result = vs[1].Trim();
                            break;
                        }
                    }
                    return result;
                case EnmScType.Aic:
                case EnmScType.Aoc:
                    return current.Comment ?? string.Empty;
                default:
                    return "";
            }
        }

        public static string GetAlarmDisplay(EnmAlarmLevel level) {
            switch(level) {
                case EnmAlarmLevel.NoAlarm:
                    return "无告警";
                case EnmAlarmLevel.Level1:
                    return "一级告警";
                case EnmAlarmLevel.Level2:
                    return "二级告警";
                case EnmAlarmLevel.Level3:
                    return "三级告警";
                case EnmAlarmLevel.Level4:
                    return "四级告警";
                default:
                    return "Undefined";
            }
        }

        public static string GetAlarmEndDisplay(EnmAlarmEnd type) {
            switch(type) {
                case EnmAlarmEnd.Normal:
                    return "正常结束";
                case EnmAlarmEnd.UpLevel:
                    return "升级结束";
                case EnmAlarmEnd.Filter:
                    return "过滤结束";
                case EnmAlarmEnd.Mask:
                    return "手动屏蔽结束";
                case EnmAlarmEnd.NodeRemove:
                    return "节点删除";
                case EnmAlarmEnd.DeviceRemove:
                    return "设备删除";
                default:
                    return "Undefined";
            }
        }

        public static Color GetAlarmColor(EnmAlarmLevel level) {
            switch(level) {
                case EnmAlarmLevel.Level1:
                    return Color.Red;
                case EnmAlarmLevel.Level2:
                    return Color.Orange;
                case EnmAlarmLevel.Level3:
                    return Color.Yellow;
                case EnmAlarmLevel.Level4:
                    return Color.SkyBlue;
                default:
                    return Color.White;
            }
        }
    }
}