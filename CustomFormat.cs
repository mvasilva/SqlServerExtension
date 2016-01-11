using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class UserDefinedFunctions
{
    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlString FN_GER_DateFormat(SqlString _cultura, SqlString _format, SqlDateTime _date)
    {
        // Put your code here
        return _date.IsNull || _format.IsNull ? SqlString.Null : string.Format(new System.Globalization.CultureInfo(_cultura.IsNull ? "pt-BR" : _cultura.Value), _format.ToString(), _date.Value);
    }


    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlString FN_GER_StringFormat(SqlString _cultura, SqlString _format, SqlString _text)
    {
        // Put your code here
        return string.Format(new System.Globalization.CultureInfo(_cultura.IsNull ? "pt-BR" : _cultura.Value), _format.Value, _text.ToString().Split(','));
    }

    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlString FN_GER_NumberFormat(SqlString _cultura, SqlString _format, SqlDouble _number)
    {
        // Put your code here
        return string.Format(new System.Globalization.CultureInfo(_cultura.IsNull ? "pt-BR" : _cultura.Value), _format.Value, _number.IsNull ? 0 : _number.Value);
    }

}
