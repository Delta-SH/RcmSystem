using Rcm.Core.Data;
using Rcm.Core.Domain;
using Rcm.Core.Enum;
using Rcm.Data.Common;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Rcm.Data {
    public partial class ValueRepository {

        #region Fields

        private readonly string _databaseConnectionString;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public ValueRepository() {
            this._databaseConnectionString = SqlHelper.RcmsHisConnection;
        }

        #endregion

        #region Methods

        public virtual List<ActValue> GetAI() {
            SqlParameter[] parms = { new SqlParameter("@AI", SqlDbType.Int) };
            parms[0].Value = (int)EnmScType.Aic;

            var entities = new List<ActValue>();
            using(var rdr = SqlHelper.ExecuteReader(this._databaseConnectionString, CommandType.Text, SqlCommands_His.Sql_Value_Repository_GetAI, parms)) {
                while(rdr.Read()) {
                    entities.Add(new ActValue {
                        DevId = SqlTypeConverter.DBNullInt32Handler(rdr["DevId"]),
                        NodeId = SqlTypeConverter.DBNullInt32Handler(rdr["NodeId"]),
                        NodeType = SqlTypeConverter.DBNullEnmScTypeHandler(rdr["NodeType"]),
                        Value = SqlTypeConverter.DBNullDoubleHandler(rdr["Value"]),
                        State = SqlTypeConverter.DBNullEnmPointStatusHandler(rdr["State"]),
                        UpdateTime = SqlTypeConverter.DBNullDateTimeHandler(rdr["UpdateTime"])
                    });
                }
            }
            return entities;
        }

        public virtual List<ActValue> GetAIP(int pid) {
            SqlParameter[] parms = { new SqlParameter("@PId", SqlDbType.Int),
                                     new SqlParameter("@AI", SqlDbType.Int) };
            parms[0].Value = pid;
            parms[1].Value = (int)EnmScType.Aic;

            var entities = new List<ActValue>();
            using(var rdr = SqlHelper.ExecuteReader(this._databaseConnectionString, CommandType.Text, SqlCommands_His.Sql_Value_Repository_GetAIP, parms)) {
                while(rdr.Read()) {
                    entities.Add(new ActValue {
                        DevId = SqlTypeConverter.DBNullInt32Handler(rdr["DevId"]),
                        NodeId = SqlTypeConverter.DBNullInt32Handler(rdr["NodeId"]),
                        NodeType = SqlTypeConverter.DBNullEnmScTypeHandler(rdr["NodeType"]),
                        Value = SqlTypeConverter.DBNullDoubleHandler(rdr["Value"]),
                        State = SqlTypeConverter.DBNullEnmPointStatusHandler(rdr["State"]),
                        UpdateTime = SqlTypeConverter.DBNullDateTimeHandler(rdr["UpdateTime"])
                    });
                }
            }
            return entities;
        }

        public virtual List<ActValue> GetAO() {
            SqlParameter[] parms = { new SqlParameter("@AO", SqlDbType.Int) };
            parms[0].Value = (int)EnmScType.Aoc;

            var entities = new List<ActValue>();
            using(var rdr = SqlHelper.ExecuteReader(this._databaseConnectionString, CommandType.Text, SqlCommands_His.Sql_Value_Repository_GetAO, parms)) {
                while(rdr.Read()) {
                    entities.Add(new ActValue {
                        DevId = SqlTypeConverter.DBNullInt32Handler(rdr["DevId"]),
                        NodeId = SqlTypeConverter.DBNullInt32Handler(rdr["NodeId"]),
                        NodeType = SqlTypeConverter.DBNullEnmScTypeHandler(rdr["NodeType"]),
                        Value = SqlTypeConverter.DBNullDoubleHandler(rdr["Value"]),
                        State = SqlTypeConverter.DBNullEnmPointStatusHandler(rdr["State"]),
                        UpdateTime = SqlTypeConverter.DBNullDateTimeHandler(rdr["UpdateTime"])
                    });
                }
            }
            return entities;
        }

        public virtual List<ActValue> GetAOP(int pid) {
            SqlParameter[] parms = { new SqlParameter("@PId", SqlDbType.Int),
                                     new SqlParameter("@AO", SqlDbType.Int) };
            parms[0].Value = pid;
            parms[1].Value = (int)EnmScType.Aoc;

            var entities = new List<ActValue>();
            using(var rdr = SqlHelper.ExecuteReader(this._databaseConnectionString, CommandType.Text, SqlCommands_His.Sql_Value_Repository_GetAOP, parms)) {
                while(rdr.Read()) {
                    entities.Add(new ActValue {
                        DevId = SqlTypeConverter.DBNullInt32Handler(rdr["DevId"]),
                        NodeId = SqlTypeConverter.DBNullInt32Handler(rdr["NodeId"]),
                        NodeType = SqlTypeConverter.DBNullEnmScTypeHandler(rdr["NodeType"]),
                        Value = SqlTypeConverter.DBNullDoubleHandler(rdr["Value"]),
                        State = SqlTypeConverter.DBNullEnmPointStatusHandler(rdr["State"]),
                        UpdateTime = SqlTypeConverter.DBNullDateTimeHandler(rdr["UpdateTime"])
                    });
                }
            }
            return entities;
        }

        public virtual List<ActValue> GetDI() {
            SqlParameter[] parms = { new SqlParameter("@DI", SqlDbType.Int) };
            parms[0].Value = (int)EnmScType.Dic;

            var entities = new List<ActValue>();
            using(var rdr = SqlHelper.ExecuteReader(this._databaseConnectionString, CommandType.Text, SqlCommands_His.Sql_Value_Repository_GetDI, parms)) {
                while(rdr.Read()) {
                    entities.Add(new ActValue {
                        DevId = SqlTypeConverter.DBNullInt32Handler(rdr["DevId"]),
                        NodeId = SqlTypeConverter.DBNullInt32Handler(rdr["NodeId"]),
                        NodeType = SqlTypeConverter.DBNullEnmScTypeHandler(rdr["NodeType"]),
                        Value = SqlTypeConverter.DBNullDoubleHandler(rdr["Value"]),
                        State = SqlTypeConverter.DBNullEnmPointStatusHandler(rdr["State"]),
                        UpdateTime = SqlTypeConverter.DBNullDateTimeHandler(rdr["UpdateTime"])
                    });
                }
            }
            return entities;
        }

        public virtual List<ActValue> GetDIP(int pid) {
            SqlParameter[] parms = { new SqlParameter("@PId", SqlDbType.Int),
                                     new SqlParameter("@DI", SqlDbType.Int) };
            parms[0].Value = pid;
            parms[1].Value = (int)EnmScType.Dic;

            var entities = new List<ActValue>();
            using(var rdr = SqlHelper.ExecuteReader(this._databaseConnectionString, CommandType.Text, SqlCommands_His.Sql_Value_Repository_GetDIP, parms)) {
                while(rdr.Read()) {
                    entities.Add(new ActValue {
                        DevId = SqlTypeConverter.DBNullInt32Handler(rdr["DevId"]),
                        NodeId = SqlTypeConverter.DBNullInt32Handler(rdr["NodeId"]),
                        NodeType = SqlTypeConverter.DBNullEnmScTypeHandler(rdr["NodeType"]),
                        Value = SqlTypeConverter.DBNullDoubleHandler(rdr["Value"]),
                        State = SqlTypeConverter.DBNullEnmPointStatusHandler(rdr["State"]),
                        UpdateTime = SqlTypeConverter.DBNullDateTimeHandler(rdr["UpdateTime"])
                    });
                }
            }
            return entities;
        }

        public virtual List<ActValue> GetDO() {
            SqlParameter[] parms = { new SqlParameter("@DO", SqlDbType.Int) };
            parms[0].Value = (int)EnmScType.Doc;

            var entities = new List<ActValue>();
            using(var rdr = SqlHelper.ExecuteReader(this._databaseConnectionString, CommandType.Text, SqlCommands_His.Sql_Value_Repository_GetDO, parms)) {
                while(rdr.Read()) {
                    entities.Add(new ActValue {
                        DevId = SqlTypeConverter.DBNullInt32Handler(rdr["DevId"]),
                        NodeId = SqlTypeConverter.DBNullInt32Handler(rdr["NodeId"]),
                        NodeType = SqlTypeConverter.DBNullEnmScTypeHandler(rdr["NodeType"]),
                        Value = SqlTypeConverter.DBNullDoubleHandler(rdr["Value"]),
                        State = SqlTypeConverter.DBNullEnmPointStatusHandler(rdr["State"]),
                        UpdateTime = SqlTypeConverter.DBNullDateTimeHandler(rdr["UpdateTime"])
                    });
                }
            }
            return entities;
        }

        public virtual List<ActValue> GetDOP(int pid) {
            SqlParameter[] parms = { new SqlParameter("@PId", SqlDbType.Int),
                                     new SqlParameter("@DO", SqlDbType.Int) };
            parms[0].Value = pid;
            parms[1].Value = (int)EnmScType.Doc;

            var entities = new List<ActValue>();
            using(var rdr = SqlHelper.ExecuteReader(this._databaseConnectionString, CommandType.Text, SqlCommands_His.Sql_Value_Repository_GetDOP, parms)) {
                while(rdr.Read()) {
                    entities.Add(new ActValue {
                        DevId = SqlTypeConverter.DBNullInt32Handler(rdr["DevId"]),
                        NodeId = SqlTypeConverter.DBNullInt32Handler(rdr["NodeId"]),
                        NodeType = SqlTypeConverter.DBNullEnmScTypeHandler(rdr["NodeType"]),
                        Value = SqlTypeConverter.DBNullDoubleHandler(rdr["Value"]),
                        State = SqlTypeConverter.DBNullEnmPointStatusHandler(rdr["State"]),
                        UpdateTime = SqlTypeConverter.DBNullDateTimeHandler(rdr["UpdateTime"])
                    });
                }
            }
            return entities;
        }

        #endregion

    }
}
