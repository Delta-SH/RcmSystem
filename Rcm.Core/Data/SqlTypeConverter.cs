using Rcm.Core.Enum;
using System;
using System.Collections.Generic;

namespace iPem.Data.Common {
    public partial class SqlTypeConverter {

        #region base handler 

        /// <summary>
        /// DBNull String Handler
        /// </summary>
        /// <param name="val">val</param>
        public static string DBNullStringHandler(object val) {
            if (val == DBNull.Value) { return default(String); }
            return val.ToString();
        }

        /// <summary>
        /// DBNull String Checker
        /// </summary>
        /// <param name="val">val</param>
        public static object DBNullStringChecker(string val) {
            if (val == default(String)) { return DBNull.Value; }
            return val;
        }

        /// <summary>
        /// DBNull Int32 Handler
        /// </summary>
        /// <param name="val">val</param>
        public static int DBNullInt32Handler(object val) {
            if (val == DBNull.Value) { return int.MinValue; }
            return (Int32)val;
        }

        /// <summary>
        /// DBNull Int32 Checker
        /// </summary>
        /// <param name="val">val</param>
        public static object DBNullInt32Checker(int val) {
            if(val == int.MinValue) { return DBNull.Value; }
            return val;
        }

        /// <summary>
        /// DBNull Int64 Handler
        /// </summary>
        /// <param name="val">val</param>
        public static long DBNullInt64Handler(object val) {
            if (val == DBNull.Value) { return long.MinValue; }
            return (Int64)val;
        }

        /// <summary>
        /// DBNull Int64 Checker
        /// </summary>
        /// <param name="val">val</param>
        public static object DBNullInt64Checker(long val) {
            if(val == long.MinValue) { return DBNull.Value; }
            return val;
        }

        /// <summary>
        /// DBNull Float Handler
        /// </summary>
        /// <param name="val">val</param>
        public static float DBNullFloatHandler(object val) {
            if (val == DBNull.Value) { return float.MinValue; }
            return (Single)val;
        }

        /// <summary>
        /// DBNull Float Checker
        /// </summary>
        /// <param name="val">val</param>
        public static object DBNullFloatChecker(float val) {
            if(val == float.MinValue) { return DBNull.Value; }
            return val;
        }

        /// <summary>
        /// DBNull Double Handler
        /// </summary>
        /// <param name="val">val</param>
        public static double DBNullDoubleHandler(object val) {
            if (val == DBNull.Value) { return double.MinValue; }
            return (Double)val;
        }

        /// <summary>
        /// DBNull Double Checker
        /// </summary>
        /// <param name="val">val</param>
        public static object DBNullDoubleChecker(double val) {
            if(val == double.MinValue) { return DBNull.Value; }
            return val;
        }

        /// <summary>
        /// DBNull Decimal Handler
        /// </summary>
        /// <param name="val">val</param>
        public static decimal DBNullDecimalHandler(object val) {
            if(val == DBNull.Value) { return decimal.MinValue; }
            return (Decimal)val;
        }

        /// <summary>
        /// DBNull Double Checker
        /// </summary>
        /// <param name="val">val</param>
        public static object DBNullDecimalChecker(decimal val) {
            if(val == decimal.MinValue) { return DBNull.Value; }
            return val;
        }

        /// <summary>
        /// DBNull DateTime Handler
        /// </summary>
        /// <param name="val">val</param>
        public static DateTime DBNullDateTimeHandler(object val) {
            if (val == DBNull.Value) { return default(DateTime); }
            return (DateTime)val;
        }

        /// <summary>
        /// DBNull DateTime Checker
        /// </summary>
        /// <param name="val">val</param>
        public static object DBNullDateTimeChecker(DateTime val) {
            if (val == default(DateTime)) { return DBNull.Value; }
            return val;
        }

        /// <summary>
        /// DBNull DateTime Nullable Handler
        /// </summary>
        /// <param name="val">val</param>
        public static DateTime? DBNullDateTimeNullableHandler(object val) {
            if(val == DBNull.Value) { return null; }
            return (DateTime)val;
        }

        /// <summary>
        /// DBNull DateTime Nullable Checker
        /// </summary>
        /// <param name="val">val</param>
        public static object DBNullDateTimeNullableChecker(DateTime? val) {
            if(!val.HasValue) { return DBNull.Value; }
            return val.Value;
        }

        /// <summary>
        /// DBNull Boolean Handler
        /// </summary>
        /// <param name="val">val</param>
        public static bool DBNullBooleanHandler(object val) {
            if (val == DBNull.Value) { return default(Boolean); }
            return (Boolean)val;
        }

        /// <summary>
        /// DBNull Guid Handler
        /// </summary>
        /// <param name="val">val</param>
        public static Guid DBNullGuidHandler(object val) {
            if (val == DBNull.Value) { return default(Guid); }
            return new Guid(val.ToString());
        }

        /// <summary>
        /// DBNull Guid Checker
        /// </summary>
        /// <param name="val">val</param>
        public static object DBNullGuidChecker(Guid val) {
            if (val == default(Guid)) { return DBNull.Value; }
            return val;
        }

        /// <summary>
        /// DBNull Bytes Handler
        /// </summary>
        /// <param name="val">val</param>
        public static byte[] DBNullBytesHandler(object val) {
            if(val == DBNull.Value) { return null; }
            return (byte[])val;
        }

        /// <summary>
        /// DBNull Bytes Checker
        /// </summary>
        /// <param name="val">val</param>
        public static object DBNullBytesChecker(object val) {
            if(val == null) { return DBNull.Value; }
            return val;
        }

        #endregion

        #region enum handler

        public static EnmScType DBNullEnmScTypeHandler(object val) {
            if(val == DBNull.Value) { return EnmScType.None; }

            var v = (Int32)val;
            return Enum.IsDefined(typeof(EnmScType), v) ? (EnmScType)v : EnmScType.None;
        }

        public static EnmPointStatus DBNullEnmPointStatusHandler(object val) {
            if(val == DBNull.Value) { return EnmPointStatus.Invalid; }

            var v = (Int32)val;
            return Enum.IsDefined(typeof(EnmPointStatus), v) ? (EnmPointStatus)v : EnmPointStatus.Invalid;
        }

        public static EnmAlarmLevel DBNullEnmAlarmLevelHandler(object val) {
            if(val == DBNull.Value) { return EnmAlarmLevel.NoAlarm; }

            var v = (Int32)val;
            return Enum.IsDefined(typeof(EnmAlarmLevel), v) ? (EnmAlarmLevel)v : EnmAlarmLevel.NoAlarm;
        }

        public static EnmAlarmEnd DBNullEnmAlarmEndHandler(object val) {
            if(val == DBNull.Value) { return EnmAlarmEnd.Normal; }

            var v = (Int32)val;
            return Enum.IsDefined(typeof(EnmAlarmEnd), v) ? (EnmAlarmEnd)v : EnmAlarmEnd.Normal;
        }

        public static EnmSetValue DBNullEnmSetValueHandler(object val) {
            if(val == DBNull.Value) { return EnmSetValue.OnlyRead; }

            var v = (Int32)val;
            return Enum.IsDefined(typeof(EnmSetValue), v) ? (EnmSetValue)v : EnmSetValue.OnlyRead;
        }

        #endregion

    }
}
