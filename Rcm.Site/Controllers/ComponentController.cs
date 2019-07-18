using Rcm.Core.Enum;
using Rcm.Service;
using Rcm.Site.Extensions;
using Rcm.Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Rcm.Site.Controllers {
    public class ComponentController : Controller {

        #region Fields

        private readonly AreaService _areaService;
        private readonly StationService _stationService;
        private readonly DeviceService _deviceService;
        private readonly PointService _pointService;

        #endregion

        #region Ctor

        public ComponentController() {
            this._areaService = new AreaService();
            this._stationService = new StationService();
            this._deviceService = new DeviceService();
            this._pointService = new PointService();
        }

        #endregion

        #region Action

        [AjaxAuthorize]
        public JsonResult GetStationTypes(int start, int limit) {
            var data = new AjaxDataModel<List<ComboItem<int, string>>> {
                success = true,
                message = "No data",
                total = 0,
                data = new List<ComboItem<int, string>>()
            };

            try {
                var models = _stationService.GetTypes();
                if(models.Count > 0) {
                    data.message = "200 Ok";
                    data.total = models.Count;
                    data.data.AddRange(models.Select(d => new ComboItem<int, string> { id = d.Key, text = d.Value }));
                }
            } catch(Exception exc) {
                data.success = false;
                data.message = exc.Message;
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [AjaxAuthorize]
        public JsonResult GetDeviceTypes(int start, int limit) {
            var data = new AjaxDataModel<List<ComboItem<int, string>>> {
                success = true,
                message = "No data",
                total = 0,
                data = new List<ComboItem<int, string>>()
            };

            try {
                var models = _deviceService.GetTypes();
                if(models.Count > 0) {
                    data.message = "200 Ok";
                    data.total = models.Count;
                    data.data.AddRange(models.Select(d => new ComboItem<int, string> { id = d.Key, text = d.Value }));
                }
            } catch(Exception exc) {
                data.success = false;
                data.message = exc.Message;
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [AjaxAuthorize]
        public JsonResult GetAlarmLevels(int start, int limit) {
            var data = new AjaxDataModel<List<ComboItem<int, string>>> {
                success = true,
                message = "No data",
                total = 0,
                data = new List<ComboItem<int, string>>()
            };

            try {
                foreach(EnmAlarmLevel level in Enum.GetValues(typeof(EnmAlarmLevel))) {
                    if(level == EnmAlarmLevel.NoAlarm) continue;
                    data.data.Add(new ComboItem<int, string>() { id = (int)level, text = Common.GetAlarmDisplay(level) });
                }

                if(data.data.Count > 0) {
                    data.total = data.data.Count;
                    data.message = "200 Ok";
                }
            } catch(Exception exc) {
                data.success = false;
                data.message = exc.Message;
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [AjaxAuthorize]
        public JsonResult GetAreas(string node, bool? multiselect) {
            var data = new AjaxDataModel<List<TreeModel>> {
                success = true,
                message = "No data",
                total = 0,
                data = new List<TreeModel>()
            };

            try {
                if (node == "root") {
                    #region root organization
                    var roots = _areaService.GetAreas(Common.GroupId).FindAll(a => a.LastId == 0);
                    if (roots.Count > 0) {
                        data.success = true;
                        data.message = "200 Ok";
                        data.total = roots.Count;
                        for (var i = 0; i < roots.Count; i++) {
                            var root = new TreeModel {
                                id = roots[i].Id.ToString(),
                                text = roots[i].Name,
                                icon = Icons.Diqiu,
                                expanded = false,
                                leaf = false
                            };

                            if (multiselect.HasValue && multiselect.Value)
                                root.selected = false;

                            data.data.Add(root);
                        }
                    }
                    #endregion
                } else if (!string.IsNullOrWhiteSpace(node)) {
                    #region area organization
                    var areas = _areaService.GetAreas(Common.GroupId);
                    var children = areas.FindAll(a => a.LastId.ToString().Equals(node));
                    if (children.Count > 0) {
                        data.success = true;
                        data.message = "200 Ok";
                        data.total = children.Count;
                        foreach (var child in children) {
                            var root = new TreeModel {
                                id = child.Id.ToString(),
                                text = child.Name,
                                icon = Icons.Diqiu,
                                expanded = false,
                                leaf = false
                            };

                            if (multiselect.HasValue && multiselect.Value)
                                root.selected = false;

                            data.data.Add(root);
                        }
                    }
                    #endregion
                }
            } catch (Exception exc) {
                data.success = false;
                data.message = exc.Message;
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [AjaxAuthorize]
        public JsonResult GetAreaPath(string[] nodes) {
            var data = new AjaxDataModel<List<string[]>> {
                success = true,
                message = "No data",
                total = 0,
                data = new List<string[]>()
            };

            try {
                var areas = _areaService.GetAreas(Common.GroupId);
                foreach (var node in nodes) {
                    var current = areas.Find(a => a.Id.ToString() == node);
                    if (current != null)
                        data.data.Add(Common.GetAreaPaths(areas, current));
                }

                if (data.data.Count > 0) {
                    data.total = data.data.Count;
                    data.message = "200 Ok";
                }
            } catch (Exception exc) {
                data.success = false;
                data.message = exc.Message;
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [AjaxAuthorize]
        public JsonResult FilterAreaPath(string text) {
            var data = new AjaxDataModel<List<string[]>> {
                success = true,
                message = "No data",
                total = 0,
                data = new List<string[]>()
            };

            try {
                if (!string.IsNullOrWhiteSpace(text)) {
                    text = text.Trim().ToLower();

                    var areas = _areaService.GetAreas(Common.GroupId);
                    var matchs = areas.FindAll(a => a.Name.ToLower().Contains(text));
                    foreach (var match in matchs)
                        data.data.Add(Common.GetAreaPaths(areas, match));
                }

                if (data.data.Count > 0) {
                    data.total = data.data.Count;
                    data.message = "200 Ok";
                }
            } catch (Exception exc) {
                data.success = false;
                data.message = exc.Message;
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [AjaxAuthorize]
        public JsonResult GetStations(string node, bool? multiselect, bool? leafselect) {
            var data = new AjaxDataModel<List<TreeModel>> {
                success = true,
                message = "No data",
                total = 0,
                data = new List<TreeModel>()
            };

            try {
                if (node == "root") {
                    #region root organization
                    var roots = _areaService.GetAreas(Common.GroupId).FindAll(a => a.LastId == 0);
                    if (roots.Count > 0) {
                        data.success = true;
                        data.message = "200 Ok";
                        data.total = roots.Count;
                        for (var i = 0; i < roots.Count; i++) {
                            var root = new TreeModel {
                                id = Common.JoinKeys((int)EnmScType.Area, roots[i].Id),
                                text = roots[i].Name,
                                icon = Icons.Diqiu,
                                expanded = false,
                                leaf = false
                            };

                            if (multiselect.HasValue && multiselect.Value) {
                                if (!leafselect.HasValue || !leafselect.Value)
                                    root.selected = false;
                            }

                            data.data.Add(root);
                        }
                    }
                    #endregion
                } else if (!string.IsNullOrWhiteSpace(node)) {
                    var keys = Common.SplitKeys(node);
                    if (keys.Length == 2) {
                        var type = int.Parse(keys[0]);
                        var id = int.Parse(keys[1]);
                        var nodeType = Enum.IsDefined(typeof(EnmScType), type) ? (EnmScType)type : EnmScType.None;
                        if (nodeType == EnmScType.Area) {
                            #region area organization
                            var areas = _areaService.GetAreas(Common.GroupId);
                            var children = areas.FindAll(a => a.LastId == id);
                            if (children.Count > 0) {
                                data.success = true;
                                data.message = "200 Ok";
                                data.total = children.Count;
                                for (var i = 0; i < children.Count; i++) {
                                    var root = new TreeModel {
                                        id = Common.JoinKeys((int)EnmScType.Area, children[i].Id),
                                        text = children[i].Name,
                                        icon = Icons.Diqiu,
                                        expanded = false,
                                        leaf = false
                                    };

                                    if (multiselect.HasValue && multiselect.Value) {
                                        if (!leafselect.HasValue || !leafselect.Value)
                                            root.selected = false;
                                    }

                                    data.data.Add(root);
                                }
                            } else {
                                var stations = _stationService.GetStations(Common.GroupId, id);
                                if (stations.Count > 0) {
                                    data.success = true;
                                    data.message = "200 Ok";
                                    data.total = stations.Count;
                                    for (var i = 0; i < stations.Count; i++) {
                                        var root = new TreeModel {
                                            id = Common.JoinKeys((int)EnmScType.Station, stations[i].Id),
                                            text = stations[i].Name,
                                            icon = Icons.Juzhan,
                                            expanded = false,
                                            leaf = true
                                        };

                                        if (multiselect.HasValue && multiselect.Value)
                                            root.selected = false;

                                        data.data.Add(root);
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                }
            } catch (Exception exc) {
                data.success = false;
                data.message = exc.Message;
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [AjaxAuthorize]
        public JsonResult GetStationPath(string[] nodes) {
            var data = new AjaxDataModel<List<string[]>> {
                success = true,
                message = "No data",
                total = 0,
                data = new List<string[]>()
            };

            try {
                foreach (var node in nodes) {
                    var keys = Common.SplitKeys(node);
                    if (keys.Length == 2) {
                        var type = int.Parse(keys[0]);
                        var id = int.Parse(keys[1]);
                        var nodeType = Enum.IsDefined(typeof(EnmScType), type) ? (EnmScType)type : EnmScType.Area;
                        if (nodeType == EnmScType.Area) {
                            #region area organization
                            var areas = _areaService.GetAreas(Common.GroupId);
                            var current = areas.Find(a => a.Id == id);
                            if (current != null) {
                                var paths = Common.GetAreaPaths(areas, current);
                                for (var i = 0; i < paths.Length; i++) {
                                    paths[i] = Common.JoinKeys((int)EnmScType.Area, paths[i]);
                                }

                                data.data.Add(paths);
                            }
                            #endregion
                        } else if (nodeType == EnmScType.Station) {
                            #region station organization
                            var current = _stationService.GetStation(id);
                            if (current != null) {
                                var paths = new List<string>();
                                var areas = _areaService.GetAreas(Common.GroupId);
                                var parent = areas.Find(a => a.Id == current.AreaId);
                                if (parent != null) {
                                    var parentPaths = Common.GetAreaPaths(areas, parent);
                                    foreach (var pp in parentPaths) {
                                        paths.Add(Common.JoinKeys((int)EnmScType.Area, pp));
                                    }
                                }

                                paths.Add(Common.JoinKeys((int)EnmScType.Station, current.Id));
                                data.data.Add(paths.ToArray());
                            }
                            #endregion
                        }
                    }
                }

                if (data.data.Count > 0) {
                    data.total = data.data.Count;
                    data.message = "200 Ok";
                }
            } catch (Exception exc) {
                data.success = false;
                data.message = exc.Message;
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [AjaxAuthorize]
        public JsonResult FilterStationPath(string text) {
            var data = new AjaxDataModel<List<string[]>> {
                success = true,
                message = "No data",
                total = 0,
                data = new List<string[]>()
            };

            try {
                if (!string.IsNullOrWhiteSpace(text)) {
                    text = text.Trim().ToLower();
                    var areas = _areaService.GetAreas(Common.GroupId);
                    var areaMatchs = areas.FindAll(a => a.Name.ToLower().Contains(text));
                    foreach (var current in areaMatchs) {
                        var paths = Common.GetAreaPaths(areas, current);
                        for (var i = 0; i < paths.Length; i++) {
                            paths[i] = Common.JoinKeys((int)EnmScType.Area, paths[i]);
                        }

                        data.data.Add(paths);
                    }

                    var stations = _stationService.GetStations(Common.GroupId);
                    var staMatchs = stations.FindAll(s => s.Name.ToLower().Contains(text));
                    foreach (var current in staMatchs) {
                        var paths = new List<string>();
                        var parent = areas.Find(a => a.Id == current.AreaId);
                        if (parent != null) {
                            var parentPaths = Common.GetAreaPaths(areas, parent);
                            foreach (var pp in parentPaths) {
                                paths.Add(Common.JoinKeys((int)EnmScType.Area, pp));
                            }
                        }

                        paths.Add(Common.JoinKeys((int)EnmScType.Station, current.Id));
                        data.data.Add(paths.ToArray());
                    }
                }

                if (data.data.Count > 0) {
                    data.total = data.data.Count;
                    data.message = "200 Ok";
                }
            } catch (Exception exc) {
                data.success = false;
                data.message = exc.Message;
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [AjaxAuthorize]
        public JsonResult GetDevices(string node, bool? multiselect, bool? leafselect) {
            var data = new AjaxDataModel<List<TreeModel>> {
                success = true,
                message = "No data",
                total = 0,
                data = new List<TreeModel>()
            };

            try {
                if (node == "root") {
                    #region root organization
                    var roots = _areaService.GetAreas(Common.GroupId).FindAll(a => a.LastId == 0);
                    if (roots.Count > 0) {
                        data.success = true;
                        data.message = "200 Ok";
                        data.total = roots.Count;
                        for (var i = 0; i < roots.Count; i++) {
                            var root = new TreeModel {
                                id = Common.JoinKeys((int)EnmScType.Area, roots[i].Id),
                                text = roots[i].Name,
                                icon = Icons.Diqiu,
                                expanded = false,
                                leaf = false
                            };

                            if (multiselect.HasValue && multiselect.Value) {
                                if (!leafselect.HasValue || !leafselect.Value)
                                    root.selected = false;
                            }

                            data.data.Add(root);
                        }
                    }
                    #endregion
                } else if (!string.IsNullOrWhiteSpace(node)) {
                    var keys = Common.SplitKeys(node);
                    if (keys.Length == 2) {
                        var type = int.Parse(keys[0]);
                        var id = int.Parse(keys[1]);
                        var nodeType = Enum.IsDefined(typeof(EnmScType), type) ? (EnmScType)type : EnmScType.None;
                        if (nodeType == EnmScType.Area) {
                            #region area organization
                            var areas = _areaService.GetAreas(Common.GroupId);
                            var children = areas.FindAll(a => a.LastId == id);
                            if (children.Count > 0) {
                                data.success = true;
                                data.message = "200 Ok";
                                data.total = children.Count;
                                for (var i = 0; i < children.Count; i++) {
                                    var root = new TreeModel {
                                        id = Common.JoinKeys((int)EnmScType.Area, children[i].Id),
                                        text = children[i].Name,
                                        icon = Icons.Diqiu,
                                        expanded = false,
                                        leaf = false
                                    };

                                    if (multiselect.HasValue && multiselect.Value) {
                                        if (!leafselect.HasValue || !leafselect.Value)
                                            root.selected = false;
                                    }

                                    data.data.Add(root);
                                }
                            } else {
                                var stations = _stationService.GetStations(Common.GroupId, id);
                                if (stations.Count > 0) {
                                    data.success = true;
                                    data.message = "200 Ok";
                                    data.total = stations.Count;
                                    for (var i = 0; i < stations.Count; i++) {
                                        var root = new TreeModel {
                                            id = Common.JoinKeys((int)EnmScType.Station, stations[i].Id),
                                            text = stations[i].Name,
                                            icon = Icons.Juzhan,
                                            expanded = false,
                                            leaf = false
                                        };

                                        if (multiselect.HasValue && multiselect.Value)
                                            root.selected = false;

                                        data.data.Add(root);
                                    }
                                }
                            }
                            #endregion
                        } else if (nodeType == EnmScType.Station) {
                            #region station organization
                            var devices = _deviceService.GetDevices(id);
                            if (devices != null && devices.Count > 0) {
                                data.success = true;
                                data.message = "200 Ok";
                                data.total = devices.Count;
                                for (var i = 0; i < devices.Count; i++) {
                                    var root = new TreeModel {
                                        id = Common.JoinKeys((int)EnmScType.Device, devices[i].Id),
                                        text = devices[i].Name,
                                        icon = Icons.Shebei,
                                        expanded = false,
                                        leaf = true
                                    };

                                    if (multiselect.HasValue && multiselect.Value)
                                        root.selected = false;

                                    data.data.Add(root);
                                }
                            }
                            #endregion
                        }
                    }
                }
            } catch (Exception exc) {
                data.success = false;
                data.message = exc.Message;
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [AjaxAuthorize]
        public JsonResult GetDevicePath(string[] nodes) {
            var data = new AjaxDataModel<List<string[]>> {
                success = true,
                message = "No data",
                total = 0,
                data = new List<string[]>()
            };

            try {
                foreach (var node in nodes) {
                    var keys = Common.SplitKeys(node);
                    if (keys.Length == 2) {
                        var type = int.Parse(keys[0]);
                        var id = int.Parse(keys[1]);
                        var nodeType = Enum.IsDefined(typeof(EnmScType), type) ? (EnmScType)type : EnmScType.Area;
                        if (nodeType == EnmScType.Area) {
                            #region area organization
                            var areas = _areaService.GetAreas(Common.GroupId);
                            var current = areas.Find(a => a.Id == id);
                            if (current != null) {
                                var paths = Common.GetAreaPaths(areas, current);
                                for (var i = 0; i < paths.Length; i++) {
                                    paths[i] = Common.JoinKeys((int)EnmScType.Area, paths[i]);
                                }

                                data.data.Add(paths);
                            }
                            #endregion
                        } else if (nodeType == EnmScType.Station) {
                            #region station organization
                            var current = _stationService.GetStation(id);
                            if (current != null) {
                                var paths = new List<string>();
                                var areas = _areaService.GetAreas(Common.GroupId);
                                var parent = areas.Find(a => a.Id == current.AreaId);
                                if (parent != null) {
                                    var parentPaths = Common.GetAreaPaths(areas, parent);
                                    foreach (var pp in parentPaths) {
                                        paths.Add(Common.JoinKeys((int)EnmScType.Area, pp));
                                    }
                                }

                                paths.Add(Common.JoinKeys((int)EnmScType.Station, current.Id));
                                data.data.Add(paths.ToArray());
                            }
                            #endregion
                        } else if (nodeType == EnmScType.Device) {
                            #region device organization
                            var current = _deviceService.GetDevice(id);
                            if (current != null) {
                                var paths = new List<string>();
                                var parent = _stationService.GetStation(current.PId);
                                if (parent != null) {
                                    var areas = _areaService.GetAreas(Common.GroupId);
                                    var pparent = areas.Find(a => a.Id == parent.AreaId);
                                    if (pparent != null) {
                                        var pparentPaths = Common.GetAreaPaths(areas, pparent);
                                        foreach (var pp in pparentPaths) {
                                            paths.Add(Common.JoinKeys((int)EnmScType.Area, pp));
                                        }
                                    }

                                    paths.Add(Common.JoinKeys((int)EnmScType.Station, parent.Id));
                                }

                                paths.Add(Common.JoinKeys((int)EnmScType.Device, current.Id));
                                data.data.Add(paths.ToArray());
                            }
                            #endregion
                        }
                    }
                }

                if (data.data.Count > 0) {
                    data.total = data.data.Count;
                    data.message = "200 Ok";
                }
            } catch (Exception exc) {
                data.success = false;
                data.message = exc.Message;
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [AjaxAuthorize]
        public JsonResult FilterDevicePath(string text) {
            var data = new AjaxDataModel<List<string[]>> {
                success = true,
                message = "No data",
                total = 0,
                data = new List<string[]>()
            };

            try {
                if (!string.IsNullOrWhiteSpace(text)) {
                    text = text.Trim().ToLower();
                    var areas = _areaService.GetAreas(Common.GroupId);
                    var areaMatchs = areas.FindAll(a => a.Name.ToLower().Contains(text));
                    foreach (var current in areaMatchs) {
                        var paths = Common.GetAreaPaths(areas, current);
                        for (var i = 0; i < paths.Length; i++) {
                            paths[i] = Common.JoinKeys((int)EnmScType.Area, paths[i]);
                        }

                        data.data.Add(paths);
                    }

                    var stations = _stationService.GetStations(Common.GroupId);
                    var staMatchs = stations.FindAll(s => s.Name.ToLower().Contains(text));
                    foreach (var current in staMatchs) {
                        var paths = new List<string>();
                        var parent = areas.Find(a => a.Id == current.AreaId);
                        if (parent != null) {
                            var parentPaths = Common.GetAreaPaths(areas, parent);
                            foreach (var pp in parentPaths) {
                                paths.Add(Common.JoinKeys((int)EnmScType.Area, pp));
                            }
                        }

                        paths.Add(Common.JoinKeys((int)EnmScType.Station, current.Id));
                        data.data.Add(paths.ToArray());
                    }

                    var devices = _deviceService.GetAllDevices(Common.GroupId);
                    var devMatchs = devices.FindAll(s => s.Name.ToLower().Contains(text));
                    foreach (var current in devMatchs) {
                        var paths = new List<string>();
                        var parent = stations.Find(s => s.Id == current.PId);
                        if (parent != null) {
                            var pparent = areas.Find(a => a.Id == parent.AreaId);
                            if (pparent != null) {
                                var pparentPaths = Common.GetAreaPaths(areas, pparent);
                                foreach (var pp in pparentPaths) {
                                    paths.Add(Common.JoinKeys((int)EnmScType.Area, pp));
                                }
                            }

                            paths.Add(Common.JoinKeys((int)EnmScType.Station, parent.Id));
                        }

                        paths.Add(Common.JoinKeys((int)EnmScType.Device, current.Id));
                        data.data.Add(paths.ToArray());
                    }
                }

                if (data.data.Count > 0) {
                    data.total = data.data.Count;
                    data.message = "200 Ok";
                }
            } catch (Exception exc) {
                data.success = false;
                data.message = exc.Message;
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [AjaxAuthorize]
        public JsonResult GetPoints(int device, bool _ai = true, bool _ao = true, bool _di = true, bool _do = true, bool _al = true) {
            var data = new AjaxDataModel<List<ComboItem<int, string>>> {
                success = true,
                message = "No data",
                total = 0,
                data = new List<ComboItem<int, string>>()
            };

            try {
                var points = _pointService.GetPoints(device, _ai, _ao, _di, _do);
                if (points.Count > 0) {
                    data.message = "200 Ok";
                    data.total = points.Count;
                    data.data.AddRange(points.Select(s => new ComboItem<int, string> { id = s.Id, text = s.Name }));
                }
            } catch (Exception exc) {
                data.success = false;
                data.message = exc.Message;
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}