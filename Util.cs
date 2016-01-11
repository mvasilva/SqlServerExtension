using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Net;
using System.IO;

public partial class UserDefinedFunctions
{
    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlInt32 FN_GER_GetWeekOfYear(SqlDateTime _date)
    {

        System.Globalization.GregorianCalendar gc = new System.Globalization.GregorianCalendar();


        return _date.IsNull ? SqlInt32.Null : gc.GetWeekOfYear(_date.Value, System.Globalization.CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
    }
}


public partial class StoredProcedures
{

    /// <summary>
    /// You may call a get method  from rest service
    /// </summary>
    /// <param name="_webUrl"></param>
    /// <param name="_returnVal"></param>
    [Microsoft.SqlServer.Server.SqlProcedure]    
    public static void PR_DDP_WebRequest(SqlString _webUrl, out SqlInt32 _returnVal)
    {
        SqlInt32 objReturn = 0;

        if (!_webUrl.IsNull)
        {
            try
            {
                HttpWebRequest request = null;
                HttpWebResponse response = null;
                Stream stream = null;
                StreamReader streamReader = null;

                request = (HttpWebRequest)WebRequest.Create(_webUrl.Value);
                request.Method = "GET";
                request.ContentLength = 0;

                response = (HttpWebResponse)request.GetResponse();
                stream = response.GetResponseStream();
                streamReader = new StreamReader(stream);
                objReturn = Convert.ToInt32(streamReader.ReadToEnd());

                response.Close();
                stream.Dispose();
                streamReader.Dispose();
            }

            catch (Exception ex)
            {
                SqlContext.Pipe.Send(ex.Message.ToString());
                objReturn = 0;
            }
        }
        else
        {
            objReturn = 0;
        }

        _returnVal = objReturn;
    }

}
