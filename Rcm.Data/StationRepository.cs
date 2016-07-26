using iPem.Data.Common;
using Rcm.Core.Data;
using Rcm.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Rcm.Data {
    public partial class StationRepository {

        #region Fields

        private readonly string _databaseConnectionString;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public StationRepository() {
            this._databaseConnectionString = SqlHelper.RcmsCfgConnection;
        }

        #endregion

        #region Methods

        public virtual Station GetEntity(int id) {
            SqlParameter[] parms = { new SqlParameter("@Id", SqlDbType.Int) };
            parms[0].Value = id;

            Station entity = null;
            using(var rdr = SqlHelper.ExecuteReader(this._databaseConnectionString, CommandType.Text, SqlCommands_Cfg.Sql_Station_Repository_GetEntity, parms)) {
                if(rdr.Read()) {
                    entity = new Station();
                    entity.Id = SqlTypeConverter.DBNullInt32Handler(rdr["Id"]);
                    entity.Name = SqlTypeConverter.DBNullStringHandler(rdr["Name"]);
                    entity.Desc = SqlTypeConverter.DBNullStringHandler(rdr["Desc"]);
                    entity.MID = SqlTypeConverter.DBNullStringHandler(rdr["MID"]);
                    entity.ExpSet = SqlTypeConverter.DBNullStringHandler(rdr["ExpSet"]);
                    entity.StationType = SqlTypeConverter.DBNullInt32Handler(rdr["StationType"]);
                    entity.StationTypeName = SqlTypeConverter.DBNullStringHandler(rdr["StationTypeName"]);
                    entity.Longitude = SqlTypeConverter.DBNullDoubleHandler(rdr["Longitude"]);
                    entity.Latitude = SqlTypeConverter.DBNullDoubleHandler(rdr["Latitude"]);
                    entity.AreaId = SqlTypeConverter.DBNullInt32Handler(rdr["AreaId"]);
                    entity.Enabled = SqlTypeConverter.DBNullBooleanHandler(rdr["Enabled"]);
                }
            }
            return entity;
        }

        public virtual List<Station> GetEntities() {
            var entities = new List<Station>();
            using(var rdr = SqlHelper.ExecuteReader(this._databaseConnectionString, CommandType.Text, SqlCommands_Cfg.Sql_Station_Repository_GetEntities, null)) {
                while(rdr.Read()) {
                    var entity = new Station();
                    entity.Id = SqlTypeConverter.DBNullInt32Handler(rdr["Id"]);
                    entity.Name = SqlTypeConverter.DBNullStringHandler(rdr["Name"]);
                    entity.Desc = SqlTypeConverter.DBNullStringHandler(rdr["Desc"]);
                    entity.MID = SqlTypeConverter.DBNullStringHandler(rdr["MID"]);
                    entity.ExpSet = SqlTypeConverter.DBNullStringHandler(rdr["ExpSet"]);
                    entity.StationType = SqlTypeConverter.DBNullInt32Handler(rdr["StationType"]);
                    entity.StationTypeName = SqlTypeConverter.DBNullStringHandler(rdr["StationTypeName"]);
                    entity.Longitude = SqlTypeConverter.DBNullDoubleHandler(rdr["Longitude"]);
                    entity.Latitude = SqlTypeConverter.DBNullDoubleHandler(rdr["Latitude"]);
                    entity.AreaId = SqlTypeConverter.DBNullInt32Handler(rdr["AreaId"]);
                    entity.Enabled = SqlTypeConverter.DBNullBooleanHandler(rdr["Enabled"]);
                    entities.Add(entity);
                }
            }
            return entities;
        }

        public virtual List<Station> GetEntities(int pid) {
            SqlParameter[] parms = { new SqlParameter("@PId", SqlDbType.Int) };
            parms[0].Value = pid;

            var entities = new List<Station>();
            using(var rdr = SqlHelper.ExecuteReader(this._databaseConnectionString, CommandType.Text, SqlCommands_Cfg.Sql_Station_Repository_GetEntitiesByPId, parms)) {
                while(rdr.Read()) {
                    var entity = new Station();
                    entity.Id = SqlTypeConverter.DBNullInt32Handler(rdr["Id"]);
                    entity.Name = SqlTypeConverter.DBNullStringHandler(rdr["Name"]);
                    entity.Desc = SqlTypeConverter.DBNullStringHandler(rdr["Desc"]);
                    entity.MID = SqlTypeConverter.DBNullStringHandler(rdr["MID"]);
                    entity.ExpSet = SqlTypeConverter.DBNullStringHandler(rdr["ExpSet"]);
                    entity.StationType = SqlTypeConverter.DBNullInt32Handler(rdr["StationType"]);
                    entity.StationTypeName = SqlTypeConverter.DBNullStringHandler(rdr["StationTypeName"]);
                    entity.Longitude = SqlTypeConverter.DBNullDoubleHandler(rdr["Longitude"]);
                    entity.Latitude = SqlTypeConverter.DBNullDoubleHandler(rdr["Latitude"]);
                    entity.AreaId = SqlTypeConverter.DBNullInt32Handler(rdr["AreaId"]);
                    entity.Enabled = SqlTypeConverter.DBNullBooleanHandler(rdr["Enabled"]);
                    entities.Add(entity);
                }
            }
            return entities;
        }

        public virtual List<Station> GetGroupEntities(int gid) {
            SqlParameter[] parms = { new SqlParameter("@GroupId", SqlDbType.Int) };
            parms[0].Value = gid;

            var entities = new List<Station>();
            using(var rdr = SqlHelper.ExecuteReader(this._databaseConnectionString, CommandType.Text, SqlCommands_Cfg.Sql_Station_Repository_GetGroupEntities, parms)) {
                while(rdr.Read()) {
                    var entity = new Station();
                    entity.Id = SqlTypeConverter.DBNullInt32Handler(rdr["Id"]);
                    entity.Name = SqlTypeConverter.DBNullStringHandler(rdr["Name"]);
                    entity.Desc = SqlTypeConverter.DBNullStringHandler(rdr["Desc"]);
                    entity.MID = SqlTypeConverter.DBNullStringHandler(rdr["MID"]);
                    entity.ExpSet = SqlTypeConverter.DBNullStringHandler(rdr["ExpSet"]);
                    entity.StationType = SqlTypeConverter.DBNullInt32Handler(rdr["StationType"]);
                    entity.StationTypeName = SqlTypeConverter.DBNullStringHandler(rdr["StationTypeName"]);
                    entity.Longitude = SqlTypeConverter.DBNullDoubleHandler(rdr["Longitude"]);
                    entity.Latitude = SqlTypeConverter.DBNullDoubleHandler(rdr["Latitude"]);
                    entity.AreaId = SqlTypeConverter.DBNullInt32Handler(rdr["AreaId"]);
                    entity.Enabled = SqlTypeConverter.DBNullBooleanHandler(rdr["Enabled"]);
                    entities.Add(entity);
                }
            }
            return entities;
        }

        public virtual List<Station> GetGroupEntities(int gid, int pid) {
            SqlParameter[] parms = { new SqlParameter("@GroupId", SqlDbType.Int),
                                     new SqlParameter("@PId", SqlDbType.Int) };
            parms[0].Value = gid;
            parms[1].Value = pid;

            var entities = new List<Station>();
            using(var rdr = SqlHelper.ExecuteReader(this._databaseConnectionString, CommandType.Text, SqlCommands_Cfg.Sql_Station_Repository_GetGroupEntitiesByPId, parms)) {
                while(rdr.Read()) {
                    var entity = new Station();
                    entity.Id = SqlTypeConverter.DBNullInt32Handler(rdr["Id"]);
                    entity.Name = SqlTypeConverter.DBNullStringHandler(rdr["Name"]);
                    entity.Desc = SqlTypeConverter.DBNullStringHandler(rdr["Desc"]);
                    entity.MID = SqlTypeConverter.DBNullStringHandler(rdr["MID"]);
                    entity.ExpSet = SqlTypeConverter.DBNullStringHandler(rdr["ExpSet"]);
                    entity.StationType = SqlTypeConverter.DBNullInt32Handler(rdr["StationType"]);
                    entity.StationTypeName = SqlTypeConverter.DBNullStringHandler(rdr["StationTypeName"]);
                    entity.Longitude = SqlTypeConverter.DBNullDoubleHandler(rdr["Longitude"]);
                    entity.Latitude = SqlTypeConverter.DBNullDoubleHandler(rdr["Latitude"]);
                    entity.AreaId = SqlTypeConverter.DBNullInt32Handler(rdr["AreaId"]);
                    entity.Enabled = SqlTypeConverter.DBNullBooleanHandler(rdr["Enabled"]);
                    entities.Add(entity);
                }
            }
            return entities;
        }

        public virtual Dictionary<int, string> GetTypes() {
            var entities = new Dictionary<int, string>();
            using(var rdr = SqlHelper.ExecuteReader(this._databaseConnectionString, CommandType.Text, SqlCommands_Cfg.Sql_Station_Repository_GetTypes, null)) {
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
