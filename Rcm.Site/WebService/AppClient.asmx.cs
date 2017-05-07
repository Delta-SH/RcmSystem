using Newtonsoft.Json;
using Rcm.Core;
using Rcm.Core.Caching;
using Rcm.Core.Domain;
using Rcm.Core.Enum;
using Rcm.Service;
using Rcm.Site.Extensions;
using Rcm.Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Rcm.Site.WebService {
    [WebService(Namespace = "http://rcms.com.cn/", Description="Rcms API")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class AppClient : System.Web.Services.WebService {

        #region Fields

        private readonly UserService _userService;
        private readonly RoleService _roleService;
        private readonly AlarmService _alarmService;
        private readonly AreaService _areaService;
        private readonly StationService _stationService;
        private readonly DeviceService _deviceService;
        private readonly PointService _pointService;
        private readonly OrderService _orderService;
        private readonly ValueService _valueService;
        private readonly ICacheManager _cacheManager;

        #endregion

        #region Ctor

        public AppClient() {
            this._userService = new UserService();
            this._roleService = new RoleService();
            this._alarmService = new AlarmService();
            this._areaService = new AreaService();
            this._stationService = new StationService();
            this._deviceService = new DeviceService();
            this._pointService = new PointService();
            this._orderService = new OrderService();
            this._valueService = new ValueService();
            this._cacheManager = new CacheManager();
        }

        #endregion

        #region Methods

        [WebMethod(Description = "<b>用户权限验证</b><br><i>输入参数：</i><br><font color='blue'>string</font> uid:用户名<br><font color='blue'>string</font> password:密码</br><i>输出结果：</i><br/><font color='blue'>json</font> 登录成功，返回用户令牌token。调用其它API接口，需传入获得的令牌验证身份。")]
        public string Validate(string uid, string password) {
            var result = new AjaxResultModel { code = 400, success = false, message = "登录失败" };
            try {
                if(String.IsNullOrWhiteSpace(uid))
                    throw new Exception("用户名不能为空。");

                uid = uid.Trim();
                if(String.IsNullOrWhiteSpace(password))
                    throw new Exception("密码不能为空。");

                var uResult = _userService.Validate(uid, password);
                switch(uResult) {
                    case LoginResult.Successful:
                        result.code = 300;
                        result.message = "用户验证通过，角色待验证。";
                        break;
                    case LoginResult.NotExist:
                        throw new Exception("用户名不存在。");
                    case LoginResult.NotEnabled:
                        throw new Exception("用户已禁用，请与管理员联系。");
                    case LoginResult.Expired:
                        throw new Exception("用户已过期，请与管理员联系。");
                    case LoginResult.WrongPassword:
                    default:
                        throw new Exception("密码错误，登录失败。");
                }

                if(result.code == 300) {
                    var current = _userService.GetUser(uid);
                    var rResult = _roleService.Validate(current.GroupId);
                    switch(rResult) {
                        case LoginResult.Successful:
                            result.code = 200;
                            result.success = true;
                            result.message = Guid.NewGuid().ToString("N");
                            if(current.LastId == 0) current.GroupId = 10078;
                            _cacheManager.Set<User>(result.message, current, TimeSpan.FromMinutes(10));
                            break;
                        case LoginResult.RoleNotExist:
                            throw new Exception("角色不存在。");
                        case LoginResult.RoleNotEnabled:
                            throw new Exception("角色已禁用，请与管理员联系。");
                        default:
                            throw new Exception("角色错误。");
                    }
                }
            } catch(Exception exc) {
                result.message = exc.Message;
            }

            return JsonConvert.SerializeObject(result, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Include }); 
        }

        [WebMethod(Description = "<b>获取监控树列表</b><br><i>输入参数：</i><br><font color='blue'>string</font> token:用户令牌<br><font color='blue'>string</font> node:请求节点编号，获取根节点传入root</br><i>输出结果：</i><br/><font color='blue'>json</font> 返回请求节点的一级子节点列表")]
        public string GetOrganization(string token, string node) {
            var data = new AjaxDataModel<List<TreeModel>> {
                success = false,
                message = "无数据",
                total = 0,
                data = new List<TreeModel>()
            };

            try {
                if(string.IsNullOrWhiteSpace(token))
                    throw new Exception("无效的令牌");

                if(!_cacheManager.IsSet(token))
                    throw new Exception("无效的令牌");

                if(string.IsNullOrWhiteSpace(node))
                    throw new Exception("无效的节点");

                var user = _cacheManager.Get<User>(token);
                if(node == "root") {
                    #region root
                    var roots = _areaService.GetAreas(user.GroupId).FindAll(a => a.LastId == 0);
                    data.success = true;
                    data.message = "200 Ok";
                    data.total = roots.Count;
                    for(var i = 0; i < roots.Count; i++) {
                        var root = new TreeModel {
                            id = roots[i].Id.ToString(),
                            text = roots[i].Name,
                            selected = false,
                            icon = "Area",
                            expanded = false,
                            leaf = false
                        };

                        data.data.Add(root);
                    }
                    #endregion
                } else {
                    data.message = "无效的节点";
                    var keys = Common.SplitKeys(node);
                    if(keys.Length == 2) {
                        var type = int.Parse(keys[0]);
                        var id = int.Parse(keys[1]);
                        var gid = user.GroupId;
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
                                        id = children[i].Id.ToString(),
                                        text = children[i].Name,
                                        selected = false,
                                        icon = "Area",
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
                                            id = stations[i].Id.ToString(),
                                            text = stations[i].Name,
                                            selected = false,
                                            icon = "Station",
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
                                        id = devices[i].Id.ToString(),
                                        text = devices[i].Name,
                                        selected = false,
                                        icon = "Device",
                                        expanded = false,
                                        leaf = true
                                    };

                                    data.data.Add(root);
                                }
                            }
                            #endregion
                        } else if(nodeType == EnmScType.Device) {
                            #region device organization

                            data.success = true;
                            data.message = "No Data";
                            data.total = 0;

                            #endregion
                        }
                    }
                }
            } catch(Exception exc) {
                data.message = exc.Message;
            }

            return JsonConvert.SerializeObject(data, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Include });
        }

        [WebMethod(Description = "<b>获取站点信息</b><br><i>输入参数：</i><br><font color='blue'>string</font> token:用户令牌<br><font color='blue'>int</font> node:区域编号(-1表示获取所有站点)<br><font color='blue'>int</font> start:需要获取数据的起始位置（初始为0）</br><font color='blue'>int</font> limit:需要获取数据的条数</br><i>输出结果：</i><br/><font color='blue'>json</font> 返回符合条件的站点列表")]
        public string GetStations(string token, int node, int start = 0, int limit = int.MaxValue) {
            var data = new AjaxDataModel<List<StationModel>> {
                success = false,
                message = "无数据",
                total = 0,
                data = new List<StationModel>()
            };

            try {
                if(string.IsNullOrWhiteSpace(token))
                    throw new Exception("无效的令牌");

                if(!_cacheManager.IsSet(token))
                    throw new Exception("无效的令牌");

                var user = _cacheManager.Get<User>(token);
                var stations = node == -1 ? _stationService.GetStations(user.GroupId) : _stationService.GetStations(user.GroupId, node);
                var areas = _areaService.GetAreas(user.GroupId);
                data.success = true;
                data.message = "200 Ok";
                data.total = stations.Count;

                var end = start + limit;
                if(end > stations.Count)
                    end = stations.Count;

                for(int i = start; i < end; i++) {
                    var current = stations[i];
                    var county = string.Empty;
                    var city = string.Empty;
                    var province = string.Empty;

                    var area = areas.Find(a => a.Id == current.AreaId);
                    if(area != null) {
                        county = area.Name;
                        var parea = areas.Find(a => a.Id == area.LastId);
                        if(parea != null) {
                            city = parea.Name;
                            var pparea = areas.Find(a => a.Id == parea.LastId);
                            if(pparea != null)
                                province = pparea.Name;
                        }
                    }

                    data.data.Add(new StationModel {
                        id = current.Id,
                        name = current.Name,
                        type = current.StationTypeName,
                        county = county,
                        city = city,
                        province = province
                    });
                }
            } catch(Exception exc) {
                data.message = exc.Message;
            }

            return JsonConvert.SerializeObject(data, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Include });
        }

        [WebMethod(Description = "<b>获取设备信息</b><br><i>输入参数：</i><br><font color='blue'>string</font> token:用户令牌<br><font color='blue'>int</font> node:站点编号(-1表示获取所有设备)<br><font color='blue'>int</font> start:需要获取数据的起始位置（初始为0）</br><font color='blue'>int</font> limit:需要获取数据的条数</br><i>输出结果：</i><br/><font color='blue'>json</font> 返回符合条件的设备列表")]
        public string GetDevices(string token, int node, int start = 0, int limit = int.MaxValue) {
            var data = new AjaxDataModel<List<DeviceModel>> {
                success = false,
                message = "无数据",
                total = 0,
                data = new List<DeviceModel>()
            };

            try {
                if(string.IsNullOrWhiteSpace(token))
                    throw new Exception("无效的令牌");

                if(!_cacheManager.IsSet(token))
                    throw new Exception("无效的令牌");

                var user = _cacheManager.Get<User>(token);
                var devices = node == -1 ? _deviceService.GetAllDevices(user.GroupId) : _deviceService.GetDevices(node);
                data.success = true;
                data.message = "200 Ok";
                data.total = devices.Count;

                var end = start + limit;
                if(end > devices.Count)
                    end = devices.Count;

                for(int i = start; i < end; i++) {
                    data.data.Add(new DeviceModel {
                        id = devices[i].Id,
                        name = devices[i].Name,
                        type = devices[i].DeviceTypeName
                    });
                }
            } catch(Exception exc) {
                data.message = exc.Message;
            }

            return JsonConvert.SerializeObject(data, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Include });
        }

        [WebMethod(Description = "<b>获取信号测值</b><br><i>输入参数：</i><br><font color='blue'>string</font> token:用户令牌<br><font color='blue'>int</font> node:设备编号</br><font color='blue'>int[]</font> types:信号类型数组（Dic：2，Aic：3，Doc：4，Aoc：5）</br><i>输出结果：</i><br/><font color='blue'>json</font> 返回指定设备下所有信号测点")]
        public string GetPoints(string token, int node, int[] types = null) {
            var data = new AjaxDataModel<List<PointModel>> {
                success = true,
                message = "无数据",
                total = 0,
                data = new List<PointModel>()
            };

            try {
                if(string.IsNullOrWhiteSpace(token))
                    throw new Exception("无效的令牌");

                if(!_cacheManager.IsSet(token))
                    throw new Exception("无效的令牌");

                var user = _cacheManager.Get<User>(token);

                if(types == null || types.Length == 0) 
                    types = new int[] { (int)EnmScType.Aic, (int)EnmScType.Aoc, (int)EnmScType.Dic, (int)EnmScType.Doc };

                var gid = user.GroupId;
                var points = _pointService.GetPoints(node, types.Contains((int)EnmScType.Aic), types.Contains((int)EnmScType.Aoc), types.Contains((int)EnmScType.Dic), types.Contains((int)EnmScType.Doc));
                var values = _valueService.GetValues(node, types.Contains((int)EnmScType.Aic), types.Contains((int)EnmScType.Aoc), types.Contains((int)EnmScType.Dic), types.Contains((int)EnmScType.Doc));
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
                        value = pv.Value != null ? pv.Value.Value : 0f,
                        valueDisplay = pv.Value != null ? Common.GetValueDisplay(pv.Point, pv.Value.Value) : Common.GetValueDisplay(pv.Point, 0),
                        status = pv.Value != null ? (int)pv.Value.State : (int)EnmPointStatus.Invalid,
                        statusDisplay = pv.Value != null ? Common.GetPointStatusDisplay(pv.Value.State) : Common.GetPointStatusDisplay(EnmPointStatus.Invalid),
                        timestamp = pv.Value != null ? CommonHelper.DateTimeConverter(pv.Value.UpdateTime) : CommonHelper.DateTimeConverter(DateTime.Now),
                        shortTime = pv.Value != null ? CommonHelper.ShortTimeConverter(pv.Value.UpdateTime) : CommonHelper.ShortTimeConverter(DateTime.Now)
                    });
                }

                data.total = data.data.Count;
                data.message = "200 Ok";
            } catch(Exception exc) {
                data.success = false;
                data.message = exc.Message;
            }

            return JsonConvert.SerializeObject(data, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Include });
        }

        [WebMethod(Description = "<b>请求信号测值</b><br><i>输入参数：</i><br><font color='blue'>string</font> token:用户令牌<br><font color='blue'>int</font> node:设备编号</br><i>输出结果：</i><br/><font color='blue'>json</font> 返回指定设备下信号测点的数据是否请求成功")]
        public string RequestPoints(string token, int node) {
            var data = new AjaxResultModel {
                code = 400,
                success = false,
                message = ""
            };

            try {
                if(string.IsNullOrWhiteSpace(token))
                    throw new Exception("无效的令牌");

                if(!_cacheManager.IsSet(token))
                    throw new Exception("无效的令牌");

                _orderService.SetOrder(new Order {
                    NodeId = node,
                    NodeType = EnmScType.Device,
                    OpType = EnmSetValue.OnlyRead,
                    OpValue = double.MinValue
                });

                data.code = 200;
                data.success = true;
                data.message = "请求命令已下发";
            } catch(Exception exc) {
                data.message = exc.Message;
            }

            return JsonConvert.SerializeObject(data, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Include });
        }

        [WebMethod(Description = "<b>信号遥控</b><br><i>输入参数：</i><br><font color='blue'>string</font> token:用户令牌<br><font color='blue'>int</font> point:信号编号</br><font color='blue'>int</font> type:信号类型（Dic：2，Aic：3，Doc：4，Aoc：5）</br><font color='blue'>int</font> ctrl:遥控值（常开控制：0，常闭控制：1，脉冲控制：2）</br><i>输出结果：</i><br/><font color='blue'>json</font> 返回信号遥控是否成功")]
        public string ControlPoint(string token, int point, int type, int ctrl) {
            try {
                if(string.IsNullOrWhiteSpace(token))
                    throw new Exception("无效的令牌");

                if(!_cacheManager.IsSet(token))
                    throw new Exception("无效的令牌");

                if(type != (int)EnmScType.Doc)
                    throw new Exception("信号类型错误");

                _orderService.SetOrder(new Order {
                    NodeId = point,
                    NodeType = EnmScType.Doc,
                    OpType = EnmSetValue.ReadWrite,
                    OpValue = ctrl
                });

                return JsonConvert.SerializeObject(new AjaxResultModel { success = true, code = 200, message = "参数设置成功" }, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Include });
            } catch(Exception exc) {
                return JsonConvert.SerializeObject(new AjaxResultModel { success = false, code = 400, message = exc.Message }, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Include });
            }
        }

        [WebMethod(Description = "<b>信号遥调</b><br><i>输入参数：</i><br><font color='blue'>string</font> token:用户令牌<br><font color='blue'>int</font> point:信号编号</br><font color='blue'>int</font> type:信号类型（Dic：2，Aic：3，Doc：4，Aoc：5）</br><font color='blue'>double</font> adjust:遥调值</br><i>输出结果：</i><br/><font color='blue'>json</font> 返回信号遥调是否成功")]
        public string AdjustPoint(string token, int point, int type, double adjust) {
            try {
                if(string.IsNullOrWhiteSpace(token))
                    throw new Exception("无效的令牌");

                if(!_cacheManager.IsSet(token))
                    throw new Exception("无效的令牌");

                if(type != (int)EnmScType.Aoc)
                    throw new Exception("信号类型错误");

                _orderService.SetOrder(new Order {
                    NodeId = point,
                    NodeType = EnmScType.Aoc,
                    OpType = EnmSetValue.ReadWrite,
                    OpValue = adjust
                });

                return JsonConvert.SerializeObject(new AjaxResultModel { success = true, code = 200, message = "参数设置成功" }, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Include });
            } catch(Exception exc) {
                return JsonConvert.SerializeObject(new AjaxResultModel { success = false, code = 400, message = exc.Message }, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Include });
            }
        }

        [WebMethod(Description = "<b>获取活动告警</b><br><i>输入参数：</i><br><font color='blue'>string</font> token:用户令牌<br><font color='blue'>int</font> node:节点编号(-1表示获取所有告警)</br><font color='blue'>int</font> type:节点类型（Area：-1，Station：0，Device：1）</br><font color='blue'>int[]</font> level:告警级别数组（一级告警：1，二级告警：2，三级告警：3，四级告警：4）</br><font color='blue'>int</font> start:需要获取数据的起始位置（初始为0）</br><font color='blue'>int</font> limit:需要获取数据的条数</br><i>输出结果：</i><br/><font color='blue'>json</font> 返回符合条件的活动告警列表")]
        public string GetActAlarms(string token, int node, int type, int[] level, int start = 0, int limit = int.MaxValue) {
            var data = new AjaxDataModel<List<ActAlarmModel>> {
                success = true,
                message = "无数据",
                total = 0,
                data = new List<ActAlarmModel>()
            };

            try {
                if(string.IsNullOrWhiteSpace(token))
                    throw new Exception("无效的令牌");

                if(!_cacheManager.IsSet(token))
                    throw new Exception("无效的令牌");

                var user = _cacheManager.Get<User>(token);
                var models = RequestActAlarms(user.GroupId, node, type, level);
                if(models.Count > 0) {
                    data.message = "200 Ok";
                    data.total = models.Count;

                    var end = start + limit;
                    if(end > models.Count)
                        end = models.Count;

                    for(int i = start; i < end; i++) {
                        data.data.Add(new ActAlarmModel {
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
            } catch(Exception exc) {
                data.success = false;
                data.message = exc.Message;
            }

            return JsonConvert.SerializeObject(data, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Include });
        }

        [WebMethod(Description = "<b>获取历史告警</b><br><i>输入参数：</i><br><font color='blue'>string</font> token:用户令牌<br><font color='blue'>int</font> node:节点编号(-1表示获取所有告警)</br><font color='blue'>int</font> type:节点类型（Area：-1，Station：0，Device：1）</br><font color='blue'>int[]</font> level:告警级别数组（一级告警：1，二级告警：2，三级告警：3，四级告警：4）</br><font color='blue'>datetime</font> startDate:开始时间</br><font color='blue'>datetime</font> endDate:结束时间</br><font color='blue'>int</font> start:需要获取数据的起始位置（初始为0）</br><font color='blue'>int</font> limit:需要获取数据的条数</br><i>输出结果：</i><br/><font color='blue'>json</font> 返回符合条件的历史告警列表")]
        public string GetHisAlarms(string token, int node, int type, int[] level, DateTime startDate, DateTime endDate, int start = 0, int limit = int.MaxValue) {
            var data = new AjaxDataModel<List<HisAlarmModel>> {
                success = true,
                message = "无数据",
                total = 0,
                data = new List<HisAlarmModel>()
            };

            try {
                if(string.IsNullOrWhiteSpace(token))
                    throw new Exception("无效的令牌");

                if(!_cacheManager.IsSet(token))
                    throw new Exception("无效的令牌");

                var user = _cacheManager.Get<User>(token);
                var models = RequestHisAlarms(user.GroupId, node, type, level, startDate, endDate);
                if(models.Count > 0) {
                    data.message = "200 Ok";
                    data.total = models.Count;

                    var end = start + limit;
                    if(end > models.Count)
                        end = models.Count;

                    for(int i = start; i < end; i++) {
                        data.data.Add(new HisAlarmModel {
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
            } catch(Exception exc) {
                data.success = false;
                data.message = exc.Message;
            }

            return JsonConvert.SerializeObject(data, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Include });
        }

        private List<Alarm> RequestActAlarms(int gid, int node, int type, int[] level) {
            var result = new List<Alarm>();
            if(node == -1) {
                #region root
                result = _alarmService.GetAllActAlarms();
                if(level != null && level.Length > 0)
                    result = result.FindAll(a => level.Contains((int)a.AlarmLevel));
                #endregion
            } else {
                var nodeType = Enum.IsDefined(typeof(EnmScType), type) ? (EnmScType)type : EnmScType.None;
                if(nodeType == EnmScType.Area) {
                    #region area organization
                    var areas = _areaService.GetAreas(gid);
                    var current = areas.Find(a => a.Id == node);
                    if(current == null) return result;

                    var children = new List<Area>();
                    children.Add(current);
                    GetChildArea(areas, current.Id, children);

                    var matchs = children.Select(c => c.Name);
                    result = _alarmService.GetAllActAlarms().FindAll(a => matchs.Contains(a.AreaName));
                    if(level != null && level.Length > 0)
                        result = result.FindAll(a => level.Contains((int)a.AlarmLevel));
                    #endregion
                } else if(nodeType == EnmScType.Station) {
                    #region station organization
                    var station = _stationService.GetStation(node);
                    if(station == null) return result;

                    result = _alarmService.GetAllActAlarms().FindAll(a => a.StationName == station.Name);
                    if(level != null && level.Length > 0)
                        result = result.FindAll(a => level.Contains((int)a.AlarmLevel));
                    #endregion
                } else if(nodeType == EnmScType.Device) {
                    #region device organization
                    var device = _deviceService.GetDevice(node);
                    if(device == null) return result;

                    result = _alarmService.GetAllActAlarms().FindAll(a => a.DeviceId == device.Id);
                    if(level != null && level.Length > 0)
                        result = result.FindAll(a => level.Contains((int)a.AlarmLevel));
                    #endregion
                }
            }

            return result;
        }

        private List<Alarm> RequestHisAlarms(int gid, int node, int type, int[] level, DateTime startDate, DateTime endDate) {
            endDate = endDate.AddSeconds(86399);
            var result = new List<Alarm>();
            if(node == -1) {
                #region root
                result = _alarmService.GetHisAlarms(startDate, endDate);
                if(level != null && level.Length > 0)
                    result = result.FindAll(a => level.Contains((int)a.AlarmLevel));
                #endregion
            } else {
                var nodeType = Enum.IsDefined(typeof(EnmScType), type) ? (EnmScType)type : EnmScType.None;
                if(nodeType == EnmScType.Area) {
                    #region area organization
                    var areas = _areaService.GetAreas(gid);
                    var current = areas.Find(a => a.Id == node);
                    if(current == null) return result;

                    var children = new List<Area>();
                    children.Add(current);
                    GetChildArea(areas, current.Id, children);

                    var matchs = children.Select(c => c.Name);
                    result = _alarmService.GetHisAlarms(startDate, endDate).FindAll(a=>matchs.Contains(a.AreaName));
                    if(level != null && level.Length > 0)
                        result = result.FindAll(a => level.Contains((int)a.AlarmLevel));
                    #endregion
                } else if(nodeType == EnmScType.Station) {
                    #region station organization
                    var station = _stationService.GetStation(node);
                    if(station == null) return result;

                    result = _alarmService.GetHisAlarms(startDate, endDate).FindAll(a => a.StationName == station.Name);
                    if(level != null && level.Length > 0)
                        result = result.FindAll(a => level.Contains((int)a.AlarmLevel));
                    #endregion
                } else if(nodeType == EnmScType.Device) {
                    #region device organization
                    var device = _deviceService.GetDevice(node);
                    if(device == null) return result;

                    result = _alarmService.GetHisAlarms(startDate, endDate).FindAll(a => a.DeviceId == device.Id);
                    if(level != null && level.Length > 0)
                        result = result.FindAll(a => level.Contains((int)a.AlarmLevel));
                    #endregion
                }
            }

            return result;

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
