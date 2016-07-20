using Rcm.Core;
using Rcm.Core.Domain;
using Rcm.Core.Enum;
using Rcm.Core.NPOI;
using Rcm.Service;
using Rcm.Site.Extensions;
using Rcm.Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Rcm.Site.Controllers {
    [Authorize]
    public class HomeController : Controller {

        #region Fields

        private readonly AlarmService _alarmService;
        private readonly AreaService _areaService;
        private readonly StationService _stationService;
        private readonly DeviceService _deviceService;
        private readonly PointService _pointService;
        private readonly OrderService _orderService;
        private readonly ValueService _valueService;
        private readonly IExcelManager _excelManager;

        #endregion

        #region Ctor

        public HomeController() {
            this._alarmService = new AlarmService();
            this._areaService = new AreaService();
            this._stationService = new StationService();
            this._deviceService = new DeviceService();
            this._pointService = new PointService();
            this._orderService = new OrderService();
            this._valueService = new ValueService();
            this._excelManager = new ExcelManager();
        }

        #endregion

        #region Action

        public ActionResult Index() {
            ViewBag.BarIndex = 0;
            return View();
        }

        public ActionResult ActiveData() {
            ViewBag.BarIndex = 1;
            return View();
        }

        public ActionResult ActiveAlarm() {
            ViewBag.BarIndex = 2;
            return View();
        }

        public ActionResult HistoryAlarm() {
            ViewBag.BarIndex = 3;
            return View();
        }

        public ActionResult Map() {
            ViewBag.BarIndex = 4;
            return View();
        }

        public ActionResult MapIFrame() {
            return View();
        }

        [AjaxAuthorize]
        public JsonResult GetMarkers(double minlng, double minlat, double maxlng, double maxlat) {
            var data = new AjaxDataModel<List<MarkerModel>> {
                success = true,
                message = "No data",
                total = 0,
                data = new List<MarkerModel>()
            };

            try {
                var currentGroupId = Common.GroupId;
                var stations = _stationService.GetStations(currentGroupId).FindAll(s => {
                    return s.Longitude >= minlng && s.Longitude <= maxlng && s.Latitude >= minlat && s.Latitude <= maxlat;
                });

                if(stations.Count > 0) {
                    if(stations.Count > 100) 
                        stations = stations.Take(100).ToList();

                    data.message = "200 Ok";
                    data.total = stations.Count;

                    var alarms = _alarmService.GetAllActAlarms();
                    var devices = _deviceService.GetAllDevices(currentGroupId);
                    var almWDev = from alarm in alarms
                                  join device in devices on alarm.DeviceId equals device.Id
                                  select new {
                                      Alarm = alarm,
                                      Device = device
                                  };

                    var almsInSta = from awd in almWDev
                                    group awd by awd.Device.PId into g
                                    select new {
                                        Id = g.Key,
                                        Alarms = g.Select(a => a.Alarm)
                                    };

                    foreach(var station in stations) {
                        var model = new MarkerModel() {
                            id = station.Id.ToString(),
                            name = station.Name,
                            type = station.StationTypeName,
                            lng = station.Longitude,
                            lat = station.Latitude
                        };

                        var eachSta = almsInSta.FirstOrDefault(s => s.Id == station.Id);
                        if(eachSta != null) {
                            model.alm1 = eachSta.Alarms.Count(a => a.AlarmLevel == EnmAlarmLevel.Level1);
                            model.alm2 = eachSta.Alarms.Count(a => a.AlarmLevel == EnmAlarmLevel.Level2);
                            model.alm3 = eachSta.Alarms.Count(a => a.AlarmLevel == EnmAlarmLevel.Level3);
                            model.alm4 = eachSta.Alarms.Count(a => a.AlarmLevel == EnmAlarmLevel.Level4);

                            if(model.alm1 > 0)
                                model.level = (int)EnmAlarmLevel.Level1;
                            else if(model.alm2 > 0)
                                model.level = (int)EnmAlarmLevel.Level2;
                            else if(model.alm3 > 0)
                                model.level = (int)EnmAlarmLevel.Level3;
                            else if(model.alm4 > 0)
                                model.level = (int)EnmAlarmLevel.Level4;
                            else
                                model.level = (int)EnmAlarmLevel.NoAlarm;
                        } else {
                            model.level = (int)EnmAlarmLevel.NoAlarm;
                            model.alm1 = 0;
                            model.alm2 = 0;
                            model.alm3 = 0;
                            model.alm4 = 0;
                        }

                        data.data.Add(model);
                    }
                }
            } catch(Exception exc) {
                data.success = false;
                data.message = exc.Message;
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [AjaxAuthorize]
        public JsonResult GetOrganization(string node) {
            var data = new AjaxDataModel<List<TreeModel>> {
                success = true,
                message = "No data",
                total = 0,
                data = new List<TreeModel>()
            };

            try {
                if(!string.IsNullOrWhiteSpace(node)) {
                    if(node == "root") {
                        #region root

                        var roots = _areaService.GetAreas(Common.GroupId).FindAll(a => a.LastId == 0);
                        for(var i = 0; i < roots.Count; i++) {
                            var root = new TreeModel {
                                id = Common.JoinKeys((int)EnmScType.Area, roots[i].Id),
                                text = roots[i].Name,
                                selected = false,
                                icon = Icons.Diqiu,
                                expanded = false,
                                leaf = false
                            };

                            data.data.Add(root);
                        }

                        #endregion
                    } else {
                        var keys = Common.SplitKeys(node);
                        if(keys.Length == 2) {
                            var type = int.Parse(keys[0]);
                            var id = int.Parse(keys[1]);
                            var gid = Common.GroupId;
                            var nodeType = Enum.IsDefined(typeof(EnmScType), type) ? (EnmScType)type : EnmScType.None;
                            if(nodeType == EnmScType.Area) {
                                #region area organization
                                var areas = _areaService.GetAreas(gid);
                                var children = areas.FindAll(a => a.LastId == id);
                                if(children.Count > 0) {
                                    data.success = true;
                                    data.message = "200 Ok";
                                    data.total = children.Count;
                                    for(var i = 0; i < children.Count; i++) {
                                        var root = new TreeModel {
                                            id = Common.JoinKeys((int)EnmScType.Area, children[i].Id),
                                            text = children[i].Name,
                                            selected = false,
                                            icon = Icons.Diqiu,
                                            expanded = false,
                                            leaf = false
                                        };

                                        data.data.Add(root);
                                    }
                                } else {
                                    var stations = _stationService.GetStations(gid, id);
                                    if(stations.Count > 0) {
                                        data.success = true;
                                        data.message = "200 Ok";
                                        data.total = stations.Count;
                                        for(var i = 0; i < stations.Count; i++) {
                                            var root = new TreeModel {
                                                id = Common.JoinKeys((int)EnmScType.Station, stations[i].Id),
                                                text = stations[i].Name,
                                                selected = false,
                                                icon = Icons.Juzhan,
                                                expanded = false,
                                                leaf = false
                                            };

                                            data.data.Add(root);
                                        }
                                    }
                                }
                                #endregion
                            } else if(nodeType == EnmScType.Station) {
                                #region station organization
                                var devices = _deviceService.GetDevices(id);
                                if(devices.Count > 0) {
                                    data.success = true;
                                    data.message = "200 Ok";
                                    data.total = devices.Count;
                                    for(var i = 0; i < devices.Count; i++) {
                                        var root = new TreeModel {
                                            id = Common.JoinKeys((int)EnmScType.Device, devices[i].Id),
                                            text = devices[i].Name,
                                            selected = false,
                                            icon = Icons.Shebei,
                                            expanded = false,
                                            leaf = true
                                        };

                                        data.data.Add(root);
                                    }
                                }
                                #endregion
                            }
                        }
                    }
                }
            } catch(Exception exc) {
                data.success = false;
                data.message = exc.Message;
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [AjaxAuthorize]
        public JsonResult SearchOrganization(string text) {
            var data = new AjaxDataModel<List<string[]>> {
                success = true,
                message = "No data",
                total = 0,
                data = new List<string[]>()
            };

            try {
                if(!string.IsNullOrWhiteSpace(text)) {
                    text = text.Trim().ToLower();

                    var gid = Common.GroupId;
                    var areas = _areaService.GetAreas(gid);
                    var areaMatchs = areas.FindAll(a => a.Name.ToLower().Contains(text));
                    foreach(var match in areaMatchs) {
                        var paths = GetAreaPath(areas, match);
                        if(paths.Count > 0)
                            data.data.Add(paths.ToArray());
                    }

                    var stations = _stationService.GetStations(gid);
                    var staMatchs = stations.FindAll(s => s.Name.ToLower().Contains(text));
                    foreach(var match in staMatchs) {
                        var paths = GetAreaPath(areas, match.AreaId);
                        paths.Add(Common.JoinKeys((int)EnmScType.Station, match.Id));
                        data.data.Add(paths.ToArray());
                    }

                    data.message = "200 Ok";
                    data.total = data.data.Count;
                }
            } catch(Exception exc) {
                data.success = false;
                data.message = exc.Message;
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [AjaxAuthorize]
        public JsonResult RequestPoints(string node, int[] types) {
            var data = new AjaxDataModel<List<PointModel>> {
                success = true,
                message = "无数据",
                total = 0,
                data = new List<PointModel>()
            };

            try {
                if(types != null && types.Length > 0) {
                    var keys = Common.SplitKeys(node);
                    if(keys.Length == 2) {
                        var type = int.Parse(keys[0]);
                        var id = int.Parse(keys[1]);
                        var gid = Common.GroupId;
                        var nodeType = Enum.IsDefined(typeof(EnmScType), type) ? (EnmScType)type : EnmScType.None;
                        if(nodeType == EnmScType.Device) {
                            var points = _pointService.GetPoints(id, types.Contains((int)EnmScType.Aic), types.Contains((int)EnmScType.Aoc), types.Contains((int)EnmScType.Dic), types.Contains((int)EnmScType.Doc));
                            var values = _valueService.GetValues(id, types.Contains((int)EnmScType.Aic), types.Contains((int)EnmScType.Aoc), types.Contains((int)EnmScType.Dic), types.Contains((int)EnmScType.Doc));
                            var pWv = from point in points
                                      join value in values on new { Id = point.Id, Type = point.Type } equals new { Id = value.NodeId, Type = value.NodeType } into temp
                                      from pv in temp.DefaultIfEmpty()
                                      select new {
                                          Point = point,
                                          Value = pv
                                      };

                            foreach(var pv in pWv) {
                                data.data.Add(new PointModel {
                                    id = pv.Point.Id,
                                    name = pv.Point.Name,
                                    type = (int)pv.Point.Type,
                                    typeDisplay = Common.GetScTypeDisplay(pv.Point.Type),
                                    value = pv.Value != null ? pv.Value.Value : 0d,
                                    valueDisplay = pv.Value != null ? Common.GetValueDisplay(pv.Point, pv.Value.Value) : Common.GetValueDisplay(pv.Point, 0),
                                    status = pv.Value != null ? (int)pv.Value.State : (int)EnmPointStatus.Invalid,
                                    statusDisplay = pv.Value != null ? Common.GetPointStatusDisplay(pv.Value.State) : Common.GetPointStatusDisplay(EnmPointStatus.Invalid),
                                    timestamp = pv.Value != null ? CommonHelper.DateTimeConverter(pv.Value.UpdateTime) : CommonHelper.DateTimeConverter(DateTime.Now),
                                    shortTime = pv.Value != null ? CommonHelper.ShortTimeConverter(pv.Value.UpdateTime) : CommonHelper.ShortTimeConverter(DateTime.Now)
                                });
                            }

                            data.total = data.data.Count;
                            data.message = "200 Ok";
                        }
                    }
                }
            } catch(Exception exc) {
                data.success = false;
                data.message = exc.Message;
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [AjaxAuthorize]
        public JsonResult SetOrders(string node) {
            var data = new AjaxResultModel {
                code = 400,
                success = false,
                message = ""
            };

            try {
                var keys = Common.SplitKeys(node);
                if(keys.Length == 2) {
                    var type = int.Parse(keys[0]);
                    var id = int.Parse(keys[1]);
                    var gid = Common.GroupId;
                    var nodeType = Enum.IsDefined(typeof(EnmScType), type) ? (EnmScType)type : EnmScType.None;
                    if(nodeType == EnmScType.Device) {
                        _orderService.SetOrder(new Order {
                            NodeId = id,
                            NodeType = EnmScType.Device,
                            OpType = EnmSetValue.OnlyRead,
                            OpValue = double.MinValue
                        });

                        data.code = 200;
                        data.success = true;
                        data.message = "Ok";
                    }
                }
            } catch(Exception exc) {
                data.message = exc.Message;
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AjaxAuthorize]
        public JsonResult ControlPoint(int point, EnmScType type, int ctrl) {
            try {
                if(type != EnmScType.Doc)
                    throw new Exception("信号类型错误");

                _orderService.SetOrder(new Order {
                    NodeId = point,
                    NodeType = type,
                    OpType = EnmSetValue.ReadWrite,
                    OpValue = ctrl
                });

                return Json(new AjaxResultModel { success = true, code = 200, message = "参数设置成功" });
            } catch(Exception exc) {
                return Json(new AjaxResultModel { success = false, code = 400, message = exc.Message });
            }
        }

        [HttpPost]
        [AjaxAuthorize]
        public JsonResult AdjustPoint(int point, EnmScType type, double adjust) {
            try {
                if(type != EnmScType.Aoc)
                    throw new Exception("信号类型错误");

                _orderService.SetOrder(new Order {
                    NodeId = point,
                    NodeType = type,
                    OpType = EnmSetValue.ReadWrite,
                    OpValue = adjust
                });

                return Json(new AjaxResultModel { success = true, code = 200, message = "参数设置成功" });
            } catch(Exception exc) {
                return Json(new AjaxResultModel { success = false, code = 400, message = exc.Message });
            }
        }

        [AjaxAuthorize]
        public JsonResult RequestActAlarms(string node, int[] stationtypes, int[] devicetypes, int[] alarmlevels, int start, int limit) {
            var data = new AjaxDataModel<List<ActAlarmModel>> {
                success = true,
                message = "无数据",
                total = 0,
                data = new List<ActAlarmModel>()
            };

            try {
                var models = this.GetActAlarms(node, stationtypes, devicetypes, alarmlevels);
                if(models.Count > 0) {
                    data.message = "200 Ok";
                    data.total = models.Count;

                    var end = start + limit;
                    if(end > models.Count)
                        end = models.Count;

                    for(int i = start; i < end; i++) {
                        data.data.Add(new ActAlarmModel {
                            index = start + i + 1,
                            id = models[i].Id,
                            level = (int)models[i].AlarmLevel,
                            levelDisplay = Common.GetAlarmDisplay(models[i].AlarmLevel),
                            time = CommonHelper.DateTimeConverter(models[i].StartTime),
                            area = models[i].AreaName,
                            station = models[i].StationName,
                            device = models[i].DeviceName,
                            point = models[i].PointName,
                            comment = models[i].AlarmDesc,
                            value = models[i].AlarmValue,
                            confirmer = models[i].ConfirmName,
                            confirmedTime = CommonHelper.DateTimeConverter(models[i].ConfirmTime),
                            interval = CommonHelper.IntervalConverter(models[i].StartTime),
                            background = Common.GetAlarmColor(models[i].AlarmLevel)
                        });
                    }
                }
            } catch(Exception exc) {
                data.success = false;
                data.message = exc.Message;
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public ActionResult DownloadActAlarms(string node, int[] stationtypes, int[] devicetypes, int[] alarmlevels) {
            try {
                var result = new List<ActAlarmModel>();
                var models = this.GetActAlarms(node, stationtypes, devicetypes, alarmlevels);
                if(models != null && models.Count > 0) {
                    for(int i = 0; i < models.Count; i++) {
                        result.Add(new ActAlarmModel {
                            index = i + 1,
                            id = models[i].Id,
                            level = (int)models[i].AlarmLevel,
                            levelDisplay = Common.GetAlarmDisplay(models[i].AlarmLevel),
                            time = CommonHelper.DateTimeConverter(models[i].StartTime),
                            area = models[i].AreaName,
                            station = models[i].StationName,
                            device = models[i].DeviceName,
                            point = models[i].PointName,
                            comment = models[i].AlarmDesc,
                            value = models[i].AlarmValue,
                            confirmer = models[i].ConfirmName,
                            confirmedTime = CommonHelper.DateTimeConverter(models[i].ConfirmTime),
                            interval = CommonHelper.IntervalConverter(models[i].StartTime),
                            background = Common.GetAlarmColor(models[i].AlarmLevel)
                        });
                    }
                }

                using(var ms = _excelManager.Export<ActAlarmModel>(result, "实时告警列表", string.Format("操作人员：{0}  操作日期：{1}", User.Identity.Name, CommonHelper.DateTimeConverter(DateTime.Now)))) {
                    return File(ms.ToArray(), _excelManager.ContentType, _excelManager.RandomFileName);
                }
            } catch(Exception exc) {
                return Json(new AjaxResultModel { success = false, code = 400, message = exc.Message });
            }
        }

        [AjaxAuthorize]
        public JsonResult RequestHisAlarms(string node, int[] stationtypes, int[] devicetypes, int[] alarmlevels, DateTime startdate, DateTime enddate, double interval, int start, int limit) {
            var data = new AjaxDataModel<List<HisAlarmModel>> {
                success = true,
                message = "无数据",
                total = 0,
                data = new List<HisAlarmModel>()
            };

            try {
                var models = this.GetHisAlarms(node, stationtypes, devicetypes, alarmlevels, startdate, enddate, interval);
                if(models.Count > 0) {
                    data.message = "200 Ok";
                    data.total = models.Count;

                    var end = start + limit;
                    if(end > models.Count)
                        end = models.Count;

                    for(int i = start; i < end; i++) {
                        data.data.Add(new HisAlarmModel {
                            index = start + i + 1,
                            id = models[i].Id,
                            level = (int)models[i].AlarmLevel,
                            levelDisplay = Common.GetAlarmDisplay(models[i].AlarmLevel),
                            area = models[i].AreaName,
                            station = models[i].StationName,
                            device = models[i].DeviceName,
                            point = models[i].PointName,
                            comment = models[i].AlarmDesc,
                            alarmValue = models[i].AlarmValue,
                            endValue = models[i].EndValue,
                            startTime = CommonHelper.DateTimeConverter(models[i].StartTime),
                            endTime = CommonHelper.DateTimeConverter(models[i].EndTime),
                            confirmer = models[i].ConfirmName,
                            confirmedTime = CommonHelper.DateTimeConverter(models[i].ConfirmTime),
                            interval = CommonHelper.IntervalConverter(models[i].StartTime, models[i].EndTime),
                            endType = Common.GetAlarmEndDisplay(models[i].EndType),
                            background = Common.GetAlarmColor(models[i].AlarmLevel)
                        });
                    }
                }
            } catch(Exception exc) {
                data.success = false;
                data.message = exc.Message;
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public ActionResult DownloadHisAlarms(string node, int[] stationtypes, int[] devicetypes, int[] alarmlevels, DateTime startdate, DateTime enddate, double interval) {
            try {
                var result = new List<HisAlarmModel>();
                var models = this.GetHisAlarms(node, stationtypes, devicetypes, alarmlevels, startdate, enddate, interval);
                if(models != null && models.Count > 0) {
                    for(int i = 0; i < models.Count; i++) {
                        result.Add(new HisAlarmModel {
                            index = i + 1,
                            id = models[i].Id,
                            level = (int)models[i].AlarmLevel,
                            levelDisplay = Common.GetAlarmDisplay(models[i].AlarmLevel),
                            area = models[i].AreaName,
                            station = models[i].StationName,
                            device = models[i].DeviceName,
                            point = models[i].PointName,
                            comment = models[i].AlarmDesc,
                            alarmValue = models[i].AlarmValue,
                            endValue = models[i].EndValue,
                            startTime = CommonHelper.DateTimeConverter(models[i].StartTime),
                            endTime = CommonHelper.DateTimeConverter(models[i].EndTime),
                            confirmer = models[i].ConfirmName,
                            confirmedTime = CommonHelper.DateTimeConverter(models[i].ConfirmTime),
                            interval = CommonHelper.IntervalConverter(models[i].StartTime, models[i].EndTime),
                            endType = Common.GetAlarmEndDisplay(models[i].EndType),
                            background = Common.GetAlarmColor(models[i].AlarmLevel)
                        });
                    }
                }

                using(var ms = _excelManager.Export<HisAlarmModel>(result, "历时告警列表", string.Format("操作人员：{0}  操作日期：{1}", User.Identity.Name, CommonHelper.DateTimeConverter(DateTime.Now)))) {
                    return File(ms.ToArray(), _excelManager.ContentType, _excelManager.RandomFileName);
                }
            } catch(Exception exc) {
                return Json(new AjaxResultModel { success = false, code = 400, message = exc.Message });
            }
        }

        private List<Alarm> GetActAlarms(string node, int[] stationtypes, int[] devicetypes, int[] alarmlevels) {
            var result = new List<Alarm>();
            if(!string.IsNullOrWhiteSpace(node)) {
                var gid = Common.GroupId;
                if(node == "root") {
                    #region root
                    var devices = _deviceService.GetAllDevices(gid);
                    if(devicetypes != null && devicetypes.Length > 0)
                        devices = devices.FindAll(d => devicetypes.Contains(d.DeviceType));

                    var stations = _stationService.GetStations(gid);
                    if(stationtypes != null && stationtypes.Length > 0)
                        stations = stations.FindAll(s => stationtypes.Contains(s.StationType));

                    var alarms = _alarmService.GetAllActAlarms();
                    if(alarmlevels != null && alarmlevels.Length > 0)
                        alarms = alarms.FindAll(a => alarmlevels.Contains((int)a.AlarmLevel));

                    var models = from alarm in alarms
                                 join device in devices on alarm.DeviceId equals device.Id
                                 join station in stations on device.PId equals station.Id
                                 select alarm;

                    result.AddRange(models);
                    #endregion
                } else {
                    var keys = Common.SplitKeys(node);
                    if(keys.Length == 2) {
                        var type = int.Parse(keys[0]);
                        var id = int.Parse(keys[1]);
                        var nodeType = Enum.IsDefined(typeof(EnmScType), type) ? (EnmScType)type : EnmScType.None;
                        if(nodeType == EnmScType.Area) {
                            #region area organization
                            var areas = _areaService.GetAreas(gid);
                            var current = areas.Find(a => a.Id == id);
                            if(current == null) return result;

                            var children = new List<Area>();
                            children.Add(current);
                            GetChildArea(areas, current.Id, children);

                            var devices = _deviceService.GetAllDevices(gid);
                            if(devicetypes != null && devicetypes.Length > 0)
                                devices = devices.FindAll(d => devicetypes.Contains(d.DeviceType));

                            var matchs = children.Select(c => c.Id);
                            var stations = _stationService.GetStations(gid).FindAll(s => matchs.Contains(s.AreaId));
                            if(stationtypes != null && stationtypes.Length > 0)
                                stations = stations.FindAll(s => stationtypes.Contains(s.StationType));

                            var alarms = _alarmService.GetAllActAlarms();
                            if(alarmlevels != null && alarmlevels.Length > 0)
                                alarms = alarms.FindAll(a => alarmlevels.Contains((int)a.AlarmLevel));

                            var models = from alarm in alarms
                                         join device in devices on alarm.DeviceId equals device.Id
                                         join station in stations on device.PId equals station.Id
                                         select alarm;

                            result.AddRange(models);
                            #endregion
                        } else if(nodeType == EnmScType.Station) {
                            #region station organization
                            var station = _stationService.GetStation(id);
                            if(station == null) return result;
                            if(stationtypes != null && stationtypes.Length > 0 && !stationtypes.Contains(station.StationType)) return result;

                            var devices = _deviceService.GetDevices(id);
                            if(devicetypes != null && devicetypes.Length > 0)
                                devices = devices.FindAll(d => devicetypes.Contains(d.DeviceType));

                            var alarms = _alarmService.GetAllActAlarms();
                            if(alarmlevels != null && alarmlevels.Length > 0)
                                alarms = alarms.FindAll(a => alarmlevels.Contains((int)a.AlarmLevel));

                            var models = from alarm in alarms
                                         join device in devices on alarm.DeviceId equals device.Id
                                         select alarm;

                            result.AddRange(models);
                            #endregion
                        } else if(nodeType == EnmScType.Device) {
                            #region device organization
                            var device = _deviceService.GetDevice(id);
                            if(device == null) return result;
                            if(devicetypes != null && devicetypes.Length > 0 && !devicetypes.Contains(device.DeviceType)) return result;
                            
                            var station = _stationService.GetStation(device.PId);
                            if(station == null) return result;
                            if(stationtypes != null && stationtypes.Length > 0 && !stationtypes.Contains(station.StationType)) return result;

                            var alarms = _alarmService.GetAllActAlarms().FindAll(a => a.DeviceId == device.Id);
                            if(alarmlevels != null && alarmlevels.Length > 0)
                                alarms = alarms.FindAll(a => alarmlevels.Contains((int)a.AlarmLevel));

                            result.AddRange(alarms);
                            #endregion
                        }
                    }
                }
            }

            return result;
        }

        private List<Alarm> GetHisAlarms(string node, int[] stationtypes, int[] devicetypes, int[] alarmlevels, DateTime startdate, DateTime enddate, double interval) {
            enddate = enddate.AddSeconds(86399);
            var result = new List<Alarm>();
            if(!string.IsNullOrWhiteSpace(node)) {
                var gid = Common.GroupId;
                if(node == "root") {
                    #region root
                    var devices = _deviceService.GetAllDevices(gid);
                    if(devicetypes != null && devicetypes.Length > 0)
                        devices = devices.FindAll(d => devicetypes.Contains(d.DeviceType));

                    var stations = _stationService.GetStations(gid);
                    if(stationtypes != null && stationtypes.Length > 0)
                        stations = stations.FindAll(s => stationtypes.Contains(s.StationType));

                    var alarms = _alarmService.GetHisAlarms(startdate, enddate);
                    if(alarmlevels != null && alarmlevels.Length > 0)
                        alarms = alarms.FindAll(a => alarmlevels.Contains((int)a.AlarmLevel));

                    if(interval > 0) {
                        interval = interval * 60;
                        alarms = alarms.FindAll(a => a.EndTime.Subtract(a.StartTime).TotalSeconds >= interval);
                    }

                    var models = from alarm in alarms
                                 join device in devices on alarm.DeviceId equals device.Id
                                 join station in stations on device.PId equals station.Id
                                 select alarm;

                    result.AddRange(models);
                    #endregion
                } else {
                    var keys = Common.SplitKeys(node);
                    if(keys.Length == 2) {
                        var type = int.Parse(keys[0]);
                        var id = int.Parse(keys[1]);
                        var nodeType = Enum.IsDefined(typeof(EnmScType), type) ? (EnmScType)type : EnmScType.None;
                        if(nodeType == EnmScType.Area) {
                            #region area organization
                            var areas = _areaService.GetAreas(gid);
                            var current = areas.Find(a => a.Id == id);
                            if(current == null) return result;

                            var children = new List<Area>();
                            children.Add(current);
                            GetChildArea(areas, current.Id, children);

                            var devices = _deviceService.GetAllDevices(gid);
                            if(devicetypes != null && devicetypes.Length > 0)
                                devices = devices.FindAll(d => devicetypes.Contains(d.DeviceType));

                            var matchs = children.Select(c => c.Id);
                            var stations = _stationService.GetStations(gid).FindAll(s => matchs.Contains(s.AreaId));
                            if(stationtypes != null && stationtypes.Length > 0)
                                stations = stations.FindAll(s => stationtypes.Contains(s.StationType));

                            var alarms = _alarmService.GetHisAlarms(startdate, enddate);
                            if(alarmlevels != null && alarmlevels.Length > 0)
                                alarms = alarms.FindAll(a => alarmlevels.Contains((int)a.AlarmLevel));

                            if(interval > 0) {
                                interval = interval * 60;
                                alarms = alarms.FindAll(a => a.EndTime.Subtract(a.StartTime).TotalSeconds >= interval);
                            }

                            var models = from alarm in alarms
                                         join device in devices on alarm.DeviceId equals device.Id
                                         join station in stations on device.PId equals station.Id
                                         select alarm;

                            result.AddRange(models);
                            #endregion
                        } else if(nodeType == EnmScType.Station) {
                            #region station organization
                            var station = _stationService.GetStation(id);
                            if(station == null) return result;
                            if(stationtypes != null && stationtypes.Length > 0 && !stationtypes.Contains(station.StationType)) return result;

                            var devices = _deviceService.GetDevices(id);
                            if(devicetypes != null && devicetypes.Length > 0)
                                devices = devices.FindAll(d => devicetypes.Contains(d.DeviceType));

                            var alarms = _alarmService.GetHisAlarms(startdate, enddate);
                            if(alarmlevels != null && alarmlevels.Length > 0)
                                alarms = alarms.FindAll(a => alarmlevels.Contains((int)a.AlarmLevel));

                            if(interval > 0) {
                                interval = interval * 60;
                                alarms = alarms.FindAll(a => a.EndTime.Subtract(a.StartTime).TotalSeconds >= interval);
                            }

                            var models = from alarm in alarms
                                         join device in devices on alarm.DeviceId equals device.Id
                                         select alarm;

                            result.AddRange(models);
                            #endregion
                        } else if(nodeType == EnmScType.Device) {
                            #region device organization
                            var device = _deviceService.GetDevice(id);
                            if(device == null) return result;
                            if(devicetypes != null && devicetypes.Length > 0 && !devicetypes.Contains(device.DeviceType)) return result;

                            var station = _stationService.GetStation(device.PId);
                            if(station == null) return result;
                            if(stationtypes != null && stationtypes.Length > 0 && !stationtypes.Contains(station.StationType)) return result;

                            var alarms = _alarmService.GetHisAlarms(startdate, enddate).FindAll(a => a.DeviceId == device.Id);
                            if(alarmlevels != null && alarmlevels.Length > 0)
                                alarms = alarms.FindAll(a => alarmlevels.Contains((int)a.AlarmLevel));

                            if(interval > 0) {
                                interval = interval * 60;
                                alarms = alarms.FindAll(a => a.EndTime.Subtract(a.StartTime).TotalSeconds >= interval);
                            }

                            result.AddRange(alarms);
                            #endregion
                        }
                    }
                }
            }

            return result;
        }

        private List<string> GetAreaPath(List<Area> areas, int id) {
            var paths = new List<string>();
            var current = areas.Find(a => a.Id == id);
            if(current == null) return paths;
            return GetAreaPath(areas, current);
        }

        private List<string> GetAreaPath(List<Area> areas, Area current) {
            var paths = new List<string>();
            if(current == null) return paths;

            var pid = current.LastId;
            var index = 1;
            paths.Add(Common.JoinKeys((int)EnmScType.Area, current.Id));
            while(true) {
                if(index > 10) break;
                var parent = areas.Find(a => a.Id == pid);
                if(parent == null) break;

                paths.Add(Common.JoinKeys((int)EnmScType.Area, parent.Id));
                pid = parent.LastId;
                index++;
            }

            paths.Reverse();
            return paths;
        }

        private void GetChildArea(List<Area> areas, int pid, List<Area> result) {
            var children = areas.FindAll(a => a.LastId == pid);
            if(children.Count == 0) return;
            
            foreach(var child in children) {
                result.Add(child);
                GetChildArea(areas, child.Id, result);
            }
        }

        #endregion

    }
}