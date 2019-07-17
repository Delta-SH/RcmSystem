namespace Rcm.Core.Data {
    /// <summary>
    /// The SqlText class is intended to encapsulate high performance, 
    /// scalable best practices for common uses of SqlClient.
    /// </summary>
    public partial class SqlCommands_His {
        /// <summary>
        /// alarm repository
        /// </summary>
        public const string Sql_Alarm_Repository_GetActEntities = @"SELECT * FROM [dbo].[AAlarm] ORDER BY [StartTime] DESC;";
        public const string Sql_Alarm_Repository_GetActEntitiesByTime = @"SELECT * FROM [dbo].[AAlarm] WHERE [StartTime] BETWEEN @Start AND @End  ORDER BY [StartTime] DESC;";
        public const string Sql_Alarm_Repository_GetHisEntities = @"
        DECLARE @tpDate DATETIME, 
                @tbName NVARCHAR(255),
                @tableCnt INT = 0,
                @SQL NVARCHAR(MAX) = N'';

        SET @tpDate = @Start;
        WHILE(DATEDIFF(MM,@tpDate,@End)>=0)
        BEGIN
            SET @tbName = N'[dbo].[HAlarm'+CONVERT(VARCHAR(6),@tpDate,112)+ N']';
            IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(@tbName) AND type in (N'U'))
            BEGIN
                IF(@tableCnt>0)
                BEGIN
                SET @SQL += N' 
                UNION ALL 
                ';
                END
        			
                SET @SQL += N'SELECT * FROM ' + @tbName + N' WHERE [StartTime] BETWEEN ''' + CONVERT(NVARCHAR,@Start,120) + N''' AND ''' + CONVERT(NVARCHAR,@End,120) + N'''';
                SET @tableCnt += 1;
            END
            SET @tpDate = DATEADD(MM,1,@tpDate);
        END

        IF(@tableCnt>0)
        BEGIN
	        SET @SQL = N';WITH Alarms AS
	        (
		        ' + @SQL + N'
	        )
	        SELECT * FROM Alarms ORDER BY [StartTime];'
        END
        EXECUTE sp_executesql @SQL;";

        /// <summary>
        /// order repository
        /// </summary>
        public const string Sql_Order_Repository_SetEntities = @"
        UPDATE [dbo].[MNode] SET [OpValue]=@OpValue WHERE [NodeID]=@NodeID AND [NodeType]=@NodeType AND [OpType]=@OpType;
        IF(@@ROWCOUNT = 0)
        BEGIN
	        INSERT INTO [dbo].[MNode]([NodeID],[NodeType],[OpType],[OpValue]) VALUES(@NodeID,@NodeType,@OpType,@OpValue);
        END";

        /// <summary>
        /// value repository
        /// </summary>
        public const string Sql_Value_Repository_GetAI = @"SELECT [DevID],[NodeID],[NodeType],[Value],[State],[UpdateTime] FROM [dbo].[HNode] WHERE [NodeType]=@AI;";
        public const string Sql_Value_Repository_GetAIP = @"SELECT [DevID],[NodeID],[NodeType],[Value],[State],[UpdateTime] FROM [dbo].[HNode] WHERE [DevID]=@PId AND [NodeType]=@AI;";
        public const string Sql_Value_Repository_GetAO = @"SELECT [DevID],[NodeID],[NodeType],[Value],[State],[UpdateTime] FROM [dbo].[HNode] WHERE [NodeType]=@AO;";
        public const string Sql_Value_Repository_GetAOP = @"SELECT [DevID],[NodeID],[NodeType],[Value],[State],[UpdateTime] FROM [dbo].[HNode] WHERE [DevID]=@PId AND [NodeType]=@AO;";
        public const string Sql_Value_Repository_GetDI = @"SELECT [DevID],[NodeID],[NodeType],[Value],[State],[UpdateTime] FROM [dbo].[HNode] WHERE [NodeType]=@DI;";
        public const string Sql_Value_Repository_GetDIP = @"SELECT [DevID],[NodeID],[NodeType],[Value],[State],[UpdateTime] FROM [dbo].[HNode] WHERE [DevID]=@PId AND [NodeType]=@DI;";
        public const string Sql_Value_Repository_GetDO = @"SELECT [DevID],[NodeID],[NodeType],[Value],[State],[UpdateTime] FROM [dbo].[HNode] WHERE [NodeType]=@DO;";
        public const string Sql_Value_Repository_GetDOP = @"SELECT [DevID],[NodeID],[NodeType],[Value],[State],[UpdateTime] FROM [dbo].[HNode] WHERE [DevID]=@PId AND [NodeType]=@DO;";
        public const string Sql_Value_Repository_GetValues = @"
        DECLARE @tpDate DATETIME, 
                @tbName NVARCHAR(255),
                @tableCnt INT = 0,
                @SQL NVARCHAR(MAX) = N'';

        SET @tpDate = @Start;
        WHILE(DATEDIFF(MM,@tpDate,@End)>=0)
        BEGIN
            SET @tbName = N'[dbo].[HRealCurve'+CONVERT(VARCHAR(6),@tpDate,112)+ N']';
            IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(@tbName) AND type in (N'U'))
            BEGIN
                IF(@tableCnt>0)
                BEGIN
                SET @SQL += N' 
                UNION ALL 
                ';
                END
        		
                SET @SQL += N'SELECT * FROM ' + @tbName + N' WHERE [UpdateTime] BETWEEN ''' + CONVERT(NVARCHAR,@Start,120) + N''' AND ''' + CONVERT(NVARCHAR,@End,120) + N'''';
                SET @tableCnt += 1;
            END
            SET @tpDate = DATEADD(MM,1,@tpDate);
        END

        IF(@tableCnt>0)
        BEGIN
	        SET @SQL = N';WITH TResult AS
	        (
		        ' + @SQL + N'
	        )
	        SELECT * FROM TResult ORDER BY [UpdateTime];'
        END

        EXECUTE sp_executesql @SQL;";

    }
}
