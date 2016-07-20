using iPem.Data.Common;
using Rcm.Core.Data;
using Rcm.Core.Domain;
using Rcm.Core.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Rcm.Data {
    public partial class PointRepository {

        #region Fields

        private readonly string _databaseConnectionString;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public PointRepository() {
            this._databaseConnectionString = SqlHelper.RcmsCfgConnection;
        }

        #endregion

        #region Methods

        public virtual List<Point> GetAI() {
            var entities = new List<Point>();
            using(var rdr = SqlHelper.ExecuteReader(this._databaseConnectionString, CommandType.Text, SqlCommands_Cfg.Sql_Point_Repository_GetAI, null)) {
                while(rdr.Read()) {
                    var entity = new Point();
                    entity.Id = SqlTypeConverter.DBNullInt32Handler(rdr["Id"]);
                    entity.Type = EnmScType.Aic;
                    entity.Name = SqlTypeConverter.DBNullStringHandler(rdr["Name"]);
                    entity.Comment = SqlTypeConverter.DBNullStringHandler(rdr["Comment"]);
                    entity.AuxSet = SqlTypeConverter.DBNullStringHandler(rdr["AuxSet"]);
                    entity.PId = SqlTypeConverter.DBNullInt32Handler(rdr["PId"]);
                    entity.Enabled = SqlTypeConverter.DBNullBooleanHandler(rdr["Enabled"]);
                    entities.Add(entity);
                }
            }
            return entities;
        }

        public virtual List<Point> GetAIP(int pid) {
            SqlParameter[] parms = { new SqlParameter("@PId", SqlDbType.Int) };
            parms[0].Value = pid;

            var entities = new List<Point>();
            using(var rdr = SqlHelper.ExecuteReader(this._databaseConnectionString, CommandType.Text, SqlCommands_Cfg.Sql_Point_Repository_GetAIP, parms)) {
                while(rdr.Read()) {
                    var entity = new Point();
                    entity.Id = SqlTypeConverter.DBNullInt32Handler(rdr["Id"]);
                    entity.Type = EnmScType.Aic;
                    entity.Name = SqlTypeConverter.DBNullStringHandler(rdr["Name"]);
                    entity.Comment = SqlTypeConverter.DBNullStringHandler(rdr["Comment"]);
                    entity.AuxSet = SqlTypeConverter.DBNullStringHandler(rdr["AuxSet"]);
                    entity.PId = SqlTypeConverter.DBNullInt32Handler(rdr["PId"]);
                    entity.Enabled = SqlTypeConverter.DBNullBooleanHandler(rdr["Enabled"]);
                    entities.Add(entity);
                }
            }
            return entities;
        }

        public virtual List<Point> GetAO() {
            var entities = new List<Point>();
            using(var rdr = SqlHelper.ExecuteReader(this._databaseConnectionString, CommandType.Text, SqlCommands_Cfg.Sql_Point_Repository_GetAO, null)) {
                while(rdr.Read()) {
                    var entity = new Point();
                    entity.Id = SqlTypeConverter.DBNullInt32Handler(rdr["Id"]);
                    entity.Type = EnmScType.Aoc;
                    entity.Name = SqlTypeConverter.DBNullStringHandler(rdr["Name"]);
                    entity.Comment = SqlTypeConverter.DBNullStringHandler(rdr["Comment"]);
                    entity.AuxSet = SqlTypeConverter.DBNullStringHandler(rdr["AuxSet"]);
                    entity.PId = SqlTypeConverter.DBNullInt32Handler(rdr["PId"]);
                    entity.Enabled = SqlTypeConverter.DBNullBooleanHandler(rdr["Enabled"]);
                    entities.Add(entity);
                }
            }
            return entities;
        }

        public virtual List<Point> GetAOP(int pid) {
            SqlParameter[] parms = { new SqlParameter("@PId", SqlDbType.Int) };
            parms[0].Value = pid;

            var entities = new List<Point>();
            using(var rdr = SqlHelper.ExecuteReader(this._databaseConnectionString, CommandType.Text, SqlCommands_Cfg.Sql_Point_Repository_GetAOP, parms)) {
                while(rdr.Read()) {
                    var entity = new Point();
                    entity.Id = SqlTypeConverter.DBNullInt32Handler(rdr["Id"]);
                    entity.Type = EnmScType.Aoc;
                    entity.Name = SqlTypeConverter.DBNullStringHandler(rdr["Name"]);
                    entity.Comment = SqlTypeConverter.DBNullStringHandler(rdr["Comment"]);
                    entity.AuxSet = SqlTypeConverter.DBNullStringHandler(rdr["AuxSet"]);
                    entity.PId = SqlTypeConverter.DBNullInt32Handler(rdr["PId"]);
                    entity.Enabled = SqlTypeConverter.DBNullBooleanHandler(rdr["Enabled"]);
                    entities.Add(entity);
                }
            }
            return entities;
        }

        public virtual List<Point> GetDI() {
            var entities = new List<Point>();
            using(var rdr = SqlHelper.ExecuteReader(this._databaseConnectionString, CommandType.Text, SqlCommands_Cfg.Sql_Point_Repository_GetDI, null)) {
                while(rdr.Read()) {
                    var entity = new Point();
                    entity.Id = SqlTypeConverter.DBNullInt32Handler(rdr["Id"]);
                    entity.Type = EnmScType.Dic;
                    entity.Name = SqlTypeConverter.DBNullStringHandler(rdr["Name"]);
                    entity.Comment = SqlTypeConverter.DBNullStringHandler(rdr["Comment"]);
                    entity.AuxSet = SqlTypeConverter.DBNullStringHandler(rdr["AuxSet"]);
                    entity.PId = SqlTypeConverter.DBNullInt32Handler(rdr["PId"]);
                    entity.Enabled = SqlTypeConverter.DBNullBooleanHandler(rdr["Enabled"]);
                    entities.Add(entity);
                }
            }
            return entities;
        }

        public virtual List<Point> GetDIP(int pid) {
            SqlParameter[] parms = { new SqlParameter("@PId", SqlDbType.Int) };
            parms[0].Value = pid;

            var entities = new List<Point>();
            using(var rdr = SqlHelper.ExecuteReader(this._databaseConnectionString, CommandType.Text, SqlCommands_Cfg.Sql_Point_Repository_GetDIP, parms)) {
                while(rdr.Read()) {
                    var entity = new Point();
                    entity.Id = SqlTypeConverter.DBNullInt32Handler(rdr["Id"]);
                    entity.Type = EnmScType.Dic;
                    entity.Name = SqlTypeConverter.DBNullStringHandler(rdr["Name"]);
                    entity.Comment = SqlTypeConverter.DBNullStringHandler(rdr["Comment"]);
                    entity.AuxSet = SqlTypeConverter.DBNullStringHandler(rdr["AuxSet"]);
                    entity.PId = SqlTypeConverter.DBNullInt32Handler(rdr["PId"]);
                    entity.Enabled = SqlTypeConverter.DBNullBooleanHandler(rdr["Enabled"]);
                    entities.Add(entity);
                }
            }
            return entities;
        }

        public virtual List<Point> GetDO() {
            var entities = new List<Point>();
            using(var rdr = SqlHelper.ExecuteReader(this._databaseConnectionString, CommandType.Text, SqlCommands_Cfg.Sql_Point_Repository_GetDO, null)) {
                while(rdr.Read()) {
                    var entity = new Point();
                    entity.Id = SqlTypeConverter.DBNullInt32Handler(rdr["Id"]);
                    entity.Type = EnmScType.Doc;
                    entity.Name = SqlTypeConverter.DBNullStringHandler(rdr["Name"]);
                    entity.Comment = SqlTypeConverter.DBNullStringHandler(rdr["Comment"]);
                    entity.AuxSet = SqlTypeConverter.DBNullStringHandler(rdr["AuxSet"]);
                    entity.PId = SqlTypeConverter.DBNullInt32Handler(rdr["PId"]);
                    entity.Enabled = SqlTypeConverter.DBNullBooleanHandler(rdr["Enabled"]);
                    entities.Add(entity);
                }
            }
            return entities;
        }

        public virtual List<Point> GetDOP(int pid) {
            SqlParameter[] parms = { new SqlParameter("@PId", SqlDbType.Int) };
            parms[0].Value = pid;

            var entities = new List<Point>();
            using(var rdr = SqlHelper.ExecuteReader(this._databaseConnectionString, CommandType.Text, SqlCommands_Cfg.Sql_Point_Repository_GetDOP, parms)) {
                while(rdr.Read()) {
                    var entity = new Point();
                    entity.Id = SqlTypeConverter.DBNullInt32Handler(rdr["Id"]);
                    entity.Type = EnmScType.Doc;
                    entity.Name = SqlTypeConverter.DBNullStringHandler(rdr["Name"]);
                    entity.Comment = SqlTypeConverter.DBNullStringHandler(rdr["Comment"]);
                    entity.AuxSet = SqlTypeConverter.DBNullStringHandler(rdr["AuxSet"]);
                    entity.PId = SqlTypeConverter.DBNullInt32Handler(rdr["PId"]);
                    entity.Enabled = SqlTypeConverter.DBNullBooleanHandler(rdr["Enabled"]);
                    entities.Add(entity);
                }
            }
            return entities;
        }

        #endregion

    }
}
