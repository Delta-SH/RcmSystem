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

        private readonly DeviceService _deviceService;
        private readonly StationService _stationService;

        #endregion

        #region Ctor

        public ComponentController() {
            this._deviceService = new DeviceService();
            this._stationService = new StationService();
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

        #endregion

    }
}