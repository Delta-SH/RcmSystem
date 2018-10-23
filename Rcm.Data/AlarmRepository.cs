using Rcm.Core.Data;
using Rcm.Core.Domain;
using Rcm.Data.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Rcm.Data {
    public partial class AlarmRepository {

        #region Fields

        private readonly string _databaseConnectionString;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public AlarmRepository() {
            this._databaseConnectionString = SqlHelper.RcmsHisConnection;
        }

        #endregion

        #region Methods

        public virtual List<Alarm> GetActEntities() {
            var entities = new List<Alarm>();
            using(var rdr = SqlHelper.ExecuteReader(this._databaseConnectionString, CommandType.Text, SqlCommands_His.Sql_Alarm_Repository_GetActEntities, null)) {
                while(rdr.Read()) {
                    var entity = new Alarm();
                    entity.Id = SqlTypeConverter.DBNullInt32Handler(rdr["Id"]);
                    entity.PointId = SqlTypeConverter.DBNullInt32Handler(rdr["PointId"]);
                    entity.PointType = SqlTypeConverter.DBNullEnmScTypeHandler(rdr["PointType"]);
                    entity.PointName = SqlTypeConverter.DBNullStringHandler(rdr["PointName"]);
                    entity.DeviceId = SqlTypeConverter.DBNullInt32Handler(rdr["DeviceId"]);
                    entity.DeviceName = SqlTypeConverter.DBNullStringHandler(rdr["DeviceName"]);
                    entity.StationName = SqlTypeConverter.DBNullStringHandler(rdr["StationName"]);
                    entity.AreaName = SqlTypeConverter.DBNullStringHandler(rdr["AreaName"]);
                    entity.DriverName = SqlTypeConverter.DBNullStringHandler(rdr["DriverName"]);
                    entity.AlarmLevel = SqlTypeConverter.DBNullEnmAlarmLevelHandler(rdr["AlarmLevel"]);
                    entity.AlarmValue = SqlTypeConverter.DBNullFloatHandler(rdr["AlarmValue"]);
                    entity.AlarmDesc = SqlTypeConverter.DBNullStringHandler(rdr["AlarmDesc"]);
                    entity.AlarmClassId = SqlTypeConverter.DBNullInt32Handler(rdr["AlarmClassId"]);
                    entity.StartTime = SqlTypeConverter.DBNullDateTimeHandler(rdr["StartTime"]);
                    var confirmTime = SqlTypeConverter.DBNullStringHandler(rdr["ConfirmTime"]);
                    if(!string.IsNullOrWhiteSpace(confirmTime)) entity.ConfirmTime = DateTime.Parse(confirmTime);
                    entity.ConfirmName = SqlTypeConverter.DBNullStringHandler(rdr["ConfirmName"]);
                    var endTime = SqlTypeConverter.DBNullStringHandler(rdr["EndTime"]);
                    if(!string.IsNullOrWhiteSpace(endTime)) entity.EndTime = DateTime.Parse(endTime);
                    entity.EndValue = SqlTypeConverter.DBNullFloatHandler(rdr["EndValue"]);
                    entity.EndType = SqlTypeConverter.DBNullEnmAlarmEndHandler(rdr["EndType"]);
                    entity.AuxSet = SqlTypeConverter.DBNullStringHandler(rdr["AuxSet"]);
                    entities.Add(entity);
                }
            }
            return entities;
        }

        public virtual List<Alarm> GetActEntities(DateTime start, DateTime end) {
            SqlParameter[] parms = { new SqlParameter("@Start", SqlDbType.DateTime),
                                     new SqlParameter("@End", SqlDbType.DateTime) };
            parms[0].Value = start;
            parms[1].Value = end;

            var entities = new List<Alarm>();
            using(var rdr = SqlHelper.ExecuteReader(this._databaseConnectionString, CommandType.Text, SqlCommands_His.Sql_Alarm_Repository_GetActEntitiesByTime, parms)) {
                while(rdr.Read()) {
                    var entity = new Alarm();
                    entity.Id = SqlTypeConverter.DBNullInt32Handler(rdr["Id"]);
                    entity.PointId = SqlTypeConverter.DBNullInt32Handler(rdr["PointId"]);
                    entity.PointType = SqlTypeConverter.DBNullEnmScTypeHandler(rdr["PointType"]);
                    entity.PointName = SqlTypeConverter.DBNullStringHandler(rdr["PointName"]);
                    entity.DeviceId = SqlTypeConverter.DBNullInt32Handler(rdr["DeviceId"]);
                    entity.DeviceName = SqlTypeConverter.DBNullStringHandler(rdr["DeviceName"]);
                    entity.StationName = SqlTypeConverter.DBNullStringHandler(rdr["StationName"]);
                    entity.AreaName = SqlTypeConverter.DBNullStringHandler(rdr["AreaName"]);
                    entity.DriverName = SqlTypeConverter.DBNullStringHandler(rdr["DriverName"]);
                    entity.AlarmLevel = SqlTypeConverter.DBNullEnmAlarmLevelHandler(rdr["AlarmLevel"]);
                    entity.AlarmValue = SqlTypeConverter.DBNullFloatHandler(rdr["AlarmValue"]);
                    entity.AlarmDesc = SqlTypeConverter.DBNullStringHandler(rdr["AlarmDesc"]);
                    entity.AlarmClassId = SqlTypeConverter.DBNullInt32Handler(rdr["AlarmClassId"]);
                    entity.StartTime = SqlTypeConverter.DBNullDateTimeHandler(rdr["StartTime"]);
                    var confirmTime = SqlTypeConverter.DBNullStringHandler(rdr["ConfirmTime"]);
                    if(!string.IsNullOrWhiteSpace(confirmTime)) entity.ConfirmTime = DateTime.Parse(confirmTime);
                    entity.ConfirmName = SqlTypeConverter.DBNullStringHandler(rdr["ConfirmName"]);
                    var endTime = SqlTypeConverter.DBNullStringHandler(rdr["EndTime"]);
                    if(!string.IsNullOrWhiteSpace(endTime)) entity.EndTime = DateTime.Parse(endTime);
                    entity.EndValue = SqlTypeConverter.DBNullFloatHandler(rdr["EndValue"]);
                    entity.EndType = SqlTypeConverter.DBNullEnmAlarmEndHandler(rdr["EndType"]);
                    entity.AuxSet = SqlTypeConverter.DBNullStringHandler(rdr["AuxSet"]);
                    entities.Add(entity);
                }
            }
            return entities;
        }

        public virtual List<Alarm> GetHisEntities(DateTime start, DateTime end) {
            SqlParameter[] parms = { new SqlParameter("@Start", SqlDbType.DateTime),
                                     new SqlParameter("@End", SqlDbType.DateTime) };
            parms[0].Value = start;
            parms[1].Value = end;

            var entities = new List<Alarm>();
            using(var rdr = SqlHelper.ExecuteReader(this._databaseConnectionString, CommandType.Text, SqlCommands_His.Sql_Alarm_Repository_GetHisEntities, parms)) {
                while(rdr.Read()) {
                    var entity = new Alarm();
                    entity.Id = SqlTypeConverter.DBNullInt32Handler(rdr["Id"]);
                    entity.PointId = SqlTypeConverter.DBNullInt32Handler(rdr["PointId"]);
                    entity.PointType = SqlTypeConverter.DBNullEnmScTypeHandler(rdr["PointType"]);
                    entity.PointName = SqlTypeConverter.DBNullStringHandler(rdr["PointName"]);
                    entity.DeviceId = SqlTypeConverter.DBNullInt32Handler(rdr["DeviceId"]);
                    entity.DeviceName = SqlTypeConverter.DBNullStringHandler(rdr["DeviceName"]);
                    entity.StationName = SqlTypeConverter.DBNullStringHandler(rdr["StationName"]);
                    entity.AreaName = SqlTypeConverter.DBNullStringHandler(rdr["AreaName"]);
                    entity.DriverName = SqlTypeConverter.DBNullStringHandler(rdr["DriverName"]);
                    entity.AlarmLevel = SqlTypeConverter.DBNullEnmAlarmLevelHandler(rdr["AlarmLevel"]);
                    entity.AlarmValue = SqlTypeConverter.DBNullFloatHandler(rdr["AlarmValue"]);
                    entity.AlarmDesc = SqlTypeConverter.DBNullStringHandler(rdr["AlarmDesc"]);
                    entity.AlarmClassId = SqlTypeConverter.DBNullInt32Handler(rdr["AlarmClassId"]);
                    entity.StartTime = SqlTypeConverter.DBNullDateTimeHandler(rdr["StartTime"]);
                    var confirmTime = SqlTypeConverter.DBNullStringHandler(rdr["ConfirmTime"]);
                    if(!string.IsNullOrWhiteSpace(confirmTime)) entity.ConfirmTime = DateTime.Parse(confirmTime);
                    entity.ConfirmName = SqlTypeConverter.DBNullStringHandler(rdr["ConfirmName"]);
                    var endTime = SqlTypeConverter.DBNullStringHandler(rdr["EndTime"]);
                    if(!string.IsNullOrWhiteSpace(endTime)) entity.EndTime = DateTime.Parse(endTime);
                    entity.EndValue = SqlTypeConverter.DBNullFloatHandler(rdr["EndValue"]);
                    entity.EndType = SqlTypeConverter.DBNullEnmAlarmEndHandler(rdr["EndType"]);
                    entities.Add(entity);
                }
            }
            return entities;
        }

        #endregion

    }
}
