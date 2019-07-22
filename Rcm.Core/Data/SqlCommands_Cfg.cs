namespace Rcm.Core.Data {
    /// <summary>
    /// The SqlText class is intended to encapsulate high performance, 
    /// scalable best practices for common uses of SqlClient.
    /// </summary>
    public class SqlCommands_Cfg {
        //user repository
        public const string Sql_User_Repository_GetEntity = @"SELECT [Id],[Name],[Uid],[Pwd] AS [Password],[GroupId],[DeptId],[LastId],[EmpNO],[DutyType],[LimitTime],[Enabled] FROM [dbo].[UUser] WHERE [Uid] =@Uid;";
        public const string Sql_User_Repository_GetEntities = @"SELECT [Id],[Name],[Uid],[Pwd] AS [Password],[GroupId],[DeptId],[LastId],[EmpNO],[DutyType],[LimitTime],[Enabled] FROM [dbo].[UUser] ORDER BY [Id];";

        //role repository
        public const string Sql_Role_Repository_GetEntity = @"SELECT [Id],[Name],[LastId],[Enabled] FROM [dbo].[UGroup] WHERE [Id]=@Id;";
        public const string Sql_Role_Repository_GetEntities = @"SELECT [Id],[Name],[LastId],[Enabled] FROM [dbo].[UGroup] ORDER BY [Id];";
        public const string Sql_Role_Repository_GetValues = @"SELECT [ID] AS [Value] FROM [dbo].[MStation];";        
        public const string Sql_Role_Repository_GetValuesById = @"SELECT [OID] AS [Value] FROM [dbo].[UMGroup] WHERE [Id]=@Id;";

        //area repository
        public const string Sql_Area_Repository_GetEntity = @"SELECT [ID],[LastID],[Name],[Desc],[MID],[ExpSet] FROM [dbo].[MArea] WHERE [ID]=@ID;";
        public const string Sql_Area_Repository_GetEntities = @"SELECT [ID],[LastID],[Name],[Desc],[MID],[ExpSet] FROM [dbo].[MArea] ORDER BY [Name];";
        public const string Sql_Area_Repository_GetGroupKeys = @"
        ;WITH Stations AS
        (
            SELECT MS.ID,MS.AreaID FROM [dbo].[MStation] MS 
            INNER JOIN [dbo].[UMGroup] UM ON MS.[ID]=UM.[OID] 
            WHERE UM.[ID]=@GroupId
        )
        SELECT AreaID FROM Stations GROUP BY AreaID;";

        //station repository
        public const string Sql_Station_Repository_GetEntity = @"SELECT MS.*,ST.[Type] AS [StationTypeName] FROM [dbo].[MStation] MS LEFT OUTER JOIN [dbo].[CStationType] ST ON MS.[StationType]=ST.[ID] WHERE MS.[ID]=@Id;";
        public const string Sql_Station_Repository_GetEntities = @"SELECT MS.*,ST.[Type] AS [StationTypeName] FROM [dbo].[MStation] MS LEFT OUTER JOIN [dbo].[CStationType] ST ON MS.[StationType]=ST.[ID] ORDER BY MS.[Name];";
        public const string Sql_Station_Repository_GetEntitiesByPId = @"SELECT MS.*,ST.[Type] AS [StationTypeName] FROM [dbo].[MStation] MS LEFT OUTER JOIN [dbo].[CStationType] ST ON MS.[StationType]=ST.[ID] WHERE MS.[AreaID]=@PId ORDER BY MS.[Name];";
        public const string Sql_Station_Repository_GetGroupEntities = @"SELECT MS.*,ST.[Type] AS [StationTypeName] FROM [dbo].[MStation] MS INNER JOIN [dbo].[UMGroup] UMG ON MS.ID = UMG.OID LEFT OUTER JOIN [dbo].[CStationType] ST ON MS.[StationType]=ST.[ID] WHERE UMG.[ID]=@GroupId ORDER BY MS.[Name];";
        public const string Sql_Station_Repository_GetGroupEntitiesByPId = @"
        ;WITH Keys AS (
	        SELECT [OID] FROM [dbo].[UMGroup] WHERE [ID]=@GroupId
        ),
        Stations AS (
	        SELECT * FROM [dbo].[MStation] WHERE AreaID=@PId
        )
        SELECT S.*,ST.[Type] AS [StationTypeName] FROM Stations S 
        INNER JOIN Keys K ON S.ID = K.OID 
        LEFT OUTER JOIN [dbo].[CStationType] ST ON S.[StationType]=ST.[ID]
        ORDER BY S.[Name];";
        public const string Sql_Station_Repository_GetTypes = @"SELECT [ID],[Type] AS [Name] FROM [dbo].[CStationType] ORDER BY [ID];";

        //device repository
        public const string Sql_Device_Repository_GetEntity = @"SELECT MD.*,DT.[Type] AS [DeviceTypeName] FROM [dbo].[MDevice] MD LEFT OUTER JOIN [dbo].[CDeviceType] DT ON MD.DeviceType=DT.ID WHERE MD.[ID]=@Id;";
        public const string Sql_Device_Repository_GetEntities = @"SELECT MD.*,DT.[Type] AS [DeviceTypeName] FROM [dbo].[MDevice] MD LEFT OUTER JOIN [dbo].[CDeviceType] DT ON MD.DeviceType=DT.ID ORDER BY MD.[ID];";
        public const string Sql_Device_Repository_GetChildEntities = @"SELECT MD.*,DT.[Type] AS [DeviceTypeName] FROM [dbo].[MDevice] MD LEFT OUTER JOIN [dbo].[CDeviceType] DT ON MD.DeviceType=DT.ID WHERE MD.[PID]=@PId ORDER BY MD.[ID];";
        public const string Sql_Device_Repository_GetGroupEntities = @"
        ;WITH Keys AS (
	        SELECT [OID] FROM [dbo].[UMGroup] WHERE [ID] = @GroupId
        ),
        Devices AS (
	        SELECT MD.*,DT.[Type] AS [DeviceTypeName] FROM [dbo].[MDevice] MD 
	        LEFT OUTER JOIN [dbo].[CDeviceType] DT ON MD.DeviceType=DT.ID
        )
        SELECT D.* FROM Devices D INNER JOIN Keys K ON D.PID = K.OID ORDER BY D.[ID];";
        public const string Sql_Device_Repository_GetTypes = @"SELECT [ID],[Type] AS [Name] FROM [dbo].[CDeviceType] ORDER BY [ID];";

        //point repository
        public const string Sql_Point_Repository_GetAI = @"SELECT [Id],[Name],[Unit] AS [Comment],[AuxSet],[PID],[Enabled] FROM [dbo].[MAic] ORDER BY [Id];";
        public const string Sql_Point_Repository_GetAIP = @"SELECT [Id],[Name],[Unit] AS [Comment],[AuxSet],[PID],[Enabled] FROM [dbo].[MAic] WHERE [PID]=@PID  ORDER BY [Id];";
        public const string Sql_Point_Repository_GetAO = @"SELECT [Id],[Name],[Unit] AS [Comment],[AuxSet],[PID],[Enabled] FROM [dbo].[MAoc] ORDER BY [Id];";
        public const string Sql_Point_Repository_GetAOP = @"SELECT [Id],[Name],[Unit] AS [Comment],[AuxSet],[PID],[Enabled] FROM [dbo].[MAoc] WHERE [PID]=@PID ORDER BY [Id];";
        public const string Sql_Point_Repository_GetDI = @"SELECT [Id],[Name],[StateDesc] AS [Comment],[AuxSet],[PID],[Enabled] FROM [dbo].[MDic] ORDER BY [Id];";
        public const string Sql_Point_Repository_GetDIP = @"SELECT [Id],[Name],[StateDesc] AS [Comment],[AuxSet],[PID],[Enabled] FROM [dbo].[MDic] WHERE [PID]=@PID ORDER BY [Id];";
        public const string Sql_Point_Repository_GetDO = @"SELECT [Id],[Name],[StateDesc] AS [Comment],[AuxSet],[PID],[Enabled] FROM [dbo].[MDoc] ORDER BY [Id];";
        public const string Sql_Point_Repository_GetDOP = @"SELECT [Id],[Name],[StateDesc] AS [Comment],[AuxSet],[PID],[Enabled] FROM [dbo].[MDoc] WHERE [PID]=@PID ORDER BY [Id];";

        //video repository
        public const string Sql_Gvideo_Repository_GetEntities = @"SELECT GV.[ID],GV.[Enabled],MV.[Name],GV.[Type],GV.[IP],GV.[Port],GV.[Uid],GV.[Pwd],GV.[AuxSet],GV.[ImgPort],MV.[PortId] FROM [dbo].[GVideo] GV INNER JOIN [dbo].[MVic] MV ON GV.[ID] = MV.[VideoID];";
    }
}
