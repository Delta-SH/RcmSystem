using iPem.Data.Common;
using Rcm.Core.Data;
using Rcm.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Rcm.Data {
    public partial class DeviceRepository {

        #region Fields

        private readonly string _databaseConnectionString;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public DeviceRepository() {
            this._databaseConnectionString = SqlHelper.RcmsCfgConnection;
        }

        #endregion

        #region Methods

        public virtual Device GetEntity(int id) {
            SqlParameter[] parms = { new SqlParameter("@Id", SqlDbType.Int) };
            parms[0].Value = id;

            Device entity = null;
            using(var rdr = SqlHelper.ExecuteReader(this._databaseConnectionString, CommandType.Text, SqlCommands_Cfg.Sql_Device_Repository_GetEntity, parms)) {
                if(rdr.Read()) {
                    entity = new Device();
                    entity.Id = SqlTypeConverter.DBNullInt32Handler(rdr["Id"]);
                    entity.Name = SqlTypeConverter.DBNullStringHandler(rdr["Name"]);
                    entity.Desc = SqlTypeConverter.DBNullStringHandler(rdr["Desc"]);
                    entity.DeviceType = SqlTypeConverter.DBNullInt32Handler(rdr["DeviceType"]);
                    entity.DeviceTypeName = SqlTypeConverter.DBNullStringHandler(rdr["DeviceTypeName"]);
                    entity.VendorId = SqlTypeConverter.DBNullInt32Handler(rdr["VendorId"]);
                    entity.Version = SqlTypeConverter.DBNullStringHandler(rdr["Version"]);
                    entity.DeviceModel = SqlTypeConverter.DBNullStringHandler(rdr["DeviceModel"]);
                    entity.BeginRunTime = SqlTypeConverter.DBNullDateTimeHandler(rdr["BeginRunTime"]);
                    entity.MID = SqlTypeConverter.DBNullStringHandler(rdr["MID"]);
                    entity.ExpSet = SqlTypeConverter.DBNullStringHandler(rdr["ExpSet"]);
                    entity.TID = SqlTypeConverter.DBNullInt32Handler(rdr["TID"]);
                    entity.Bind = SqlTypeConverter.DBNullBooleanHandler(rdr["Bind"]);
                    entity.PId = SqlTypeConverter.DBNullInt32Handler(rdr["PId"]);
                    entity.Enabled = SqlTypeConverter.DBNullBooleanHandler(rdr["Enabled"]);
                }
            }
            return entity;
        }

        public virtual List<Device> GetEntities() {
            var entities = new List<Device>();
            using(var rdr = SqlHelper.ExecuteReader(this._databaseConnectionString, CommandType.Text, SqlCommands_Cfg.Sql_Device_Repository_GetEntities, null)) {
                while(rdr.Read()) {
                    var entity = new Device();
                    entity.Id = SqlTypeConverter.DBNullInt32Handler(rdr["Id"]);
                    entity.Name = SqlTypeConverter.DBNullStringHandler(rdr["Name"]);
                    entity.Desc = SqlTypeConverter.DBNullStringHandler(rdr["Desc"]);
                    entity.DeviceType = SqlTypeConverter.DBNullInt32Handler(rdr["DeviceType"]);
                    entity.DeviceTypeName = SqlTypeConverter.DBNullStringHandler(rdr["DeviceTypeName"]);
                    entity.VendorId = SqlTypeConverter.DBNullInt32Handler(rdr["VendorId"]);
                    entity.Version = SqlTypeConverter.DBNullStringHandler(rdr["Version"]);
                    entity.DeviceModel = SqlTypeConverter.DBNullStringHandler(rdr["DeviceModel"]);
                    entity.BeginRunTime = SqlTypeConverter.DBNullDateTimeHandler(rdr["BeginRunTime"]);
                    entity.MID = SqlTypeConverter.DBNullStringHandler(rdr["MID"]);
                    entity.ExpSet = SqlTypeConverter.DBNullStringHandler(rdr["ExpSet"]);
                    entity.TID = SqlTypeConverter.DBNullInt32Handler(rdr["TID"]);
                    entity.Bind = SqlTypeConverter.DBNullBooleanHandler(rdr["Bind"]);
                    entity.PId = SqlTypeConverter.DBNullInt32Handler(rdr["PId"]);
                    entity.Enabled = SqlTypeConverter.DBNullBooleanHandler(rdr["Enabled"]);
                    entities.Add(entity);
                }
            }
            return entities;
        }

        public virtual List<Device> GetGroupEntities(int gid) {
            SqlParameter[] parms = { new SqlParameter("@GroupId", SqlDbType.Int) };
            parms[0].Value = gid;

            var entities = new List<Device>();
            using(var rdr = SqlHelper.ExecuteReader(this._databaseConnectionString, CommandType.Text, SqlCommands_Cfg.Sql_Device_Repository_GetGroupEntities, parms)) {
                while(rdr.Read()) {
                    var entity = new Device();
                    entity.Id = SqlTypeConverter.DBNullInt32Handler(rdr["Id"]);
                    entity.Name = SqlTypeConverter.DBNullStringHandler(rdr["Name"]);
                    entity.Desc = SqlTypeConverter.DBNullStringHandler(rdr["Desc"]);
                    entity.DeviceType = SqlTypeConverter.DBNullInt32Handler(rdr["DeviceType"]);
                    entity.DeviceTypeName = SqlTypeConverter.DBNullStringHandler(rdr["DeviceTypeName"]);
                    entity.VendorId = SqlTypeConverter.DBNullInt32Handler(rdr["VendorId"]);
                    entity.Version = SqlTypeConverter.DBNullStringHandler(rdr["Version"]);
                    entity.DeviceModel = SqlTypeConverter.DBNullStringHandler(rdr["DeviceModel"]);
                    entity.BeginRunTime = SqlTypeConverter.DBNullDateTimeHandler(rdr["BeginRunTime"]);
                    entity.MID = SqlTypeConverter.DBNullStringHandler(rdr["MID"]);
                    entity.ExpSet = SqlTypeConverter.DBNullStringHandler(rdr["ExpSet"]);
                    entity.TID = SqlTypeConverter.DBNullInt32Handler(rdr["TID"]);
                    entity.Bind = SqlTypeConverter.DBNullBooleanHandler(rdr["Bind"]);
                    entity.PId = SqlTypeConverter.DBNullInt32Handler(rdr["PId"]);
                    entity.Enabled = SqlTypeConverter.DBNullBooleanHandler(rdr["Enabled"]);
                    entities.Add(entity);
                }
            }
            return entities;
        }

        public virtual List<Device> GetChildEntities(int pid) {
            SqlParameter[] parms = { new SqlParameter("@PId", SqlDbType.Int) };
            parms[0].Value = pid;

            var entities = new List<Device>();
            using(var rdr = SqlHelper.ExecuteReader(this._databaseConnectionString, CommandType.Text, SqlCommands_Cfg.Sql_Device_Repository_GetChildEntities, parms)) {
                while(rdr.Read()) {
                    var entity = new Device();
                    entity.Id = SqlTypeConverter.DBNullInt32Handler(rdr["Id"]);
                    entity.Name = SqlTypeConverter.DBNullStringHandler(rdr["Name"]);
                    entity.Desc = SqlTypeConverter.DBNullStringHandler(rdr["Desc"]);
                    entity.DeviceType = SqlTypeConverter.DBNullInt32Handler(rdr["DeviceType"]);
                    entity.DeviceTypeName = SqlTypeConverter.DBNullStringHandler(rdr["DeviceTypeName"]);
                    entity.VendorId = SqlTypeConverter.DBNullInt32Handler(rdr["VendorId"]);
                    entity.Version = SqlTypeConverter.DBNullStringHandler(rdr["Version"]);
                    entity.DeviceModel = SqlTypeConverter.DBNullStringHandler(rdr["DeviceModel"]);
                    entity.BeginRunTime = SqlTypeConverter.DBNullDateTimeHandler(rdr["BeginRunTime"]);
                    entity.MID = SqlTypeConverter.DBNullStringHandler(rdr["MID"]);
                    entity.ExpSet = SqlTypeConverter.DBNullStringHandler(rdr["ExpSet"]);
                    entity.TID = SqlTypeConverter.DBNullInt32Handler(rdr["TID"]);
                    entity.Bind = SqlTypeConverter.DBNullBooleanHandler(rdr["Bind"]);
                    entity.PId = SqlTypeConverter.DBNullInt32Handler(rdr["PId"]);
                    entity.Enabled = SqlTypeConverter.DBNullBooleanHandler(rdr["Enabled"]);
                    entities.Add(entity);
                }
            }
            return entities;
        }

        public virtual Dictionary<int, string> GetTypes() {
            var entities = new Dictionary<int, string>();
            using(var rdr = SqlHelper.ExecuteReader(this._databaseConnectionString, CommandType.Text, SqlCommands_Cfg.Sql_Device_Repository_GetTypes, null)) {
                while(rdr.Read()) {
                    var id = SqlTypeConverter.DBNullInt32Handler(rdr["Id"]);
                    var name = SqlTypeConverter.DBNullStringHandler(rdr["Name"]);
                    entities[id] = name;
                }
            }
            return entities;
        }

        #endregion

    }
}
