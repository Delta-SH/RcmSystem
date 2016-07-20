using System;

namespace Rcm.Core.Enum {
    public enum ResultStatus {
        Failure,
        Success
    }

    public enum LicenseStatus {
        Invalid,
        Expired,
        Evaluation,
        Licensed
    }

    public enum LoginResult {
        /// <summary>
        /// Login successful
        /// </summary>
        Successful = 1,
        /// <summary>
        /// Account dies not exist
        /// </summary>
        NotExist = 2,
        /// <summary>
        /// Wrong password
        /// </summary>
        WrongPassword = 3,
        /// <summary>
        /// Account have not been enabled
        /// </summary>
        NotEnabled = 4,
        /// <summary>
        /// Account has been expired 
        /// </summary>
        Expired = 5,
        /// <summary>
        /// Account has been locked 
        /// </summary>
        Locked = 6,
        /// <summary>
        /// Role dies not exist
        /// </summary>
        RoleNotExist = 7,
        /// <summary>
        /// Role have not been enabled
        /// </summary>
        RoleNotEnabled = 8
    }

    public enum EnmScType {
        None = -3,
        /// <summary>
        /// 监控中心
        /// </summary> 
        LSC = -2,
        /// <summary>
        /// 区域
        /// </summary> 
        Area = -1,
        /// <summary>
        /// 局站
        /// </summary> 
        Station = 0,
        /// <summary>
        /// 设备
        /// </summary> 
        Device = 1,
        /// <summary>
        /// 遥信量
        /// </summary> 
        Dic = 2,
        /// <summary>
        /// 遥测量
        /// </summary> 
        Aic = 3,
        /// <summary>
        /// 遥控量
        /// </summary> 
        Doc = 4,
        /// <summary>
        /// 遥调量
        /// </summary> 
        Aoc = 5,
        /// <summary>
        /// 字符输入量 
        /// </summary> 
        Sic,
        /// <summary>
        /// 字符输出量 
        /// </summary> 
        Soc,
        /// <summary>
        /// 图片输入量 
        /// </summary> 
        Pic,
        /// <summary>
        /// 图片输出量 
        /// </summary> 
        Poc,
        /// <summary>
        /// 视频输入量 
        /// </summary> 
        Vic,
        /// <summary>
        /// 视频输出量 
        /// </summary> 
        Voc,
        /// <summary>
        /// 音频输入量 
        /// </summary> 
        ADic,
        /// <summary>
        /// 音频输出量 
        /// </summary> 
        ADoc,
        /// <summary>
        /// 字符 
        /// </summary> 
        Str,
        /// <summary>
        /// 图片
        /// </summary> 
        Img,
        /// <summary>
        /// 前置机 
        /// </summary> 
        FS,
        /// <summary>
        /// 总线控制器
        /// </summary> 
        Bus,
        /// <summary>
        /// 驱动器
        /// </summary> 
        Driver,
        /// <summary>
        /// 扩展对象
        /// </summary>
        ExObj = 99
    }

    public enum EnmPointStatus {
        /// <summary>
        /// 正常数据
        /// </summary>
        Normal,
        /// <summary>
        /// 一级告警
        /// </summary>
        Level1,
        /// <summary>
        /// 二级告警
        /// </summary>
        Level2,
        /// <summary>
        /// 三级告警
        /// </summary>
        Level3,
        /// <summary>
        /// 四级告警
        /// </summary>
        Level4,
        /// <summary>
        /// 操作事件
        /// </summary>
        Opevent,
        /// <summary>
        /// 无效数据
        /// </summary>
        Invalid,
        /// <summary>
        /// 通信中断
        /// </summary>
        NullValue
    }

    public enum EnmAlarmLevel {
        /// <summary>
        /// 正常
        /// </summary>
        NoAlarm,
        /// <summary>
        /// 一级告警
        /// </summary>
        Level1,
        /// <summary>
        /// 二级告警
        /// </summary>
        Level2,
        /// <summary>
        /// 三级告警
        /// </summary>
        Level3,
        /// <summary>
        /// 四级告警
        /// </summary>
        Level4
    }

    public enum EnmAlarmEnd {
        /// <summary>
        /// 正常结束
        /// </summary>
        Normal,
        /// <summary>
        /// 升级结束
        /// </summary>
        UpLevel,
        /// <summary>
        /// 过滤结束
        /// </summary>
        Filter,
        /// <summary>
        /// 手动屏蔽结束
        /// </summary>
        Mask,
        /// <summary>
        /// 节点删除
        /// </summary>
        NodeRemove,
        /// <summary>
        /// 设备删除
        /// </summary>
        DeviceRemove
    }

    public enum EnmSetValue {
        /// <summary>
        /// 请求数据
        /// </summary>
        OnlyRead,
        /// <summary>
        /// 设置数据
        /// </summary>
        ReadWrite,
        /// <summary>
        /// 仿真数据
        /// </summary>
        Emulate
    }
}
