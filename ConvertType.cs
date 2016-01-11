using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class UserDefinedFunctions
{
    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlDateTime FN_GER_ConvertToDatetime(SqlString _cultura, SqlString _strData)
    {
        return _strData.IsNull ? SqlDateTime.Null : Convert.ToDateTime(_strData.Value, new System.Globalization.CultureInfo(_cultura.IsNull ? "pt-BR" : _cultura.Value));
    }


    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlDateTime FN_GER_ConvertToDatetimeByFormat(SqlString _strFormat, SqlString _strData)
    {

        return _strFormat.IsNull || _strData.IsNull ? SqlDateTime.Null : DateTime.ParseExact(_strData.Value, _strFormat.Value, System.Globalization.CultureInfo.InvariantCulture);
    }

    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlInt64 FN_GER_ConvertDatetimeToTimestamp(SqlDateTime _date)
    {
        long objReturn = 0;

        if (!_date.IsNull)
        {

            DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0);

            TimeSpan elapsedTime = _date.Value.ToUniversalTime() - Epoch;
            objReturn = (long)elapsedTime.TotalSeconds;
        }
        return _date.IsNull ? SqlInt64.Null : objReturn;
    }

    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlDateTime FN_GER_ConvertTimestampToDatetime(SqlInt64 _val)
    {
        DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0);

        return _val.IsNull ? SqlDateTime.Null : origin.AddSeconds(_val.Value).ToLocalTime();
    }
}
