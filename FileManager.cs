using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.IO;
using System.Collections.Generic;

public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void PR_GER_FileInfo(SqlString nom_Caminho)
    {

        FileInfo _file = new FileInfo(nom_Caminho.Value);

        List<SqlMetaData> colunas = new List<SqlMetaData>();
        //new SqlMetaData("stringcol", SqlDbType.NVarChar, 128)

        colunas.Add(new SqlMetaData("dat_Criacao", SqlDbType.DateTime));
        colunas.Add(new SqlMetaData("dat_UltimoAcesso", SqlDbType.DateTime));
        colunas.Add(new SqlMetaData("dat_UltimaEscrita", SqlDbType.DateTime));
        colunas.Add(new SqlMetaData("ind_Existe", SqlDbType.Bit));
        colunas.Add(new SqlMetaData("nom_Arquivo", SqlDbType.VarChar, 700));
        colunas.Add(new SqlMetaData("nom_Diretorio", SqlDbType.VarChar, 700));
        colunas.Add(new SqlMetaData("nom_Extensao", SqlDbType.VarChar, 700));

        colunas.Add(new SqlMetaData("val_Tamanho", SqlDbType.BigInt));

        SqlDataRecord record = new SqlDataRecord(colunas.ToArray());

        BindFile(_file, record);


        SqlContext.Pipe.Send(record);
    }

    private static void BindFile(FileInfo _file, SqlDataRecord record)
    {
        record.SetSqlDateTime(0, _file.Exists ? _file.CreationTime : SqlDateTime.Null);
        record.SetSqlDateTime(1, _file.Exists ? _file.LastAccessTime : SqlDateTime.Null);
        record.SetSqlDateTime(2, _file.Exists ? _file.LastWriteTime : SqlDateTime.Null);
        record.SetSqlBoolean(3, _file.Exists);
        record.SetString(4, _file.Name);
        record.SetString(5, _file.DirectoryName);
        record.SetString(6, _file.Extension);
        record.SetSqlInt64(7, _file.Exists ? _file.Length : SqlInt64.Null);
    }


    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void PR_GER_FileCopy(SqlString nom_ArquivoOrigem, SqlString nom_ArquivoDestino, SqlBoolean ind_Substituir)
    {

        List<SqlMetaData> colunas = new List<SqlMetaData>();

        colunas.Add(new SqlMetaData("nom_ArquivoOrigem", SqlDbType.VarChar, 700));
        colunas.Add(new SqlMetaData("nom_ArquivoDestino", SqlDbType.VarChar, 700));
        colunas.Add(new SqlMetaData("ind_Status", SqlDbType.Int));
        colunas.Add(new SqlMetaData("nom_Msg", SqlDbType.VarChar, 1000));

        SqlDataRecord record = new SqlDataRecord(colunas.ToArray());

        try
        {
            if (File.Exists(nom_ArquivoOrigem.Value))
            {
                FileInfo fileDestino = new FileInfo(nom_ArquivoDestino.Value);

                if (Directory.Exists(fileDestino.DirectoryName))
                {
                    File.Copy(nom_ArquivoOrigem.Value, nom_ArquivoDestino.Value, ind_Substituir.Value);

                    record.SetString(0, nom_ArquivoOrigem.Value);
                    record.SetString(1, nom_ArquivoDestino.Value);
                    record.SetInt32(2, 1);
                    record.SetString(3, "Operação executada com sucesso");

                }
                else
                {
                    record.SetString(0, nom_ArquivoOrigem.Value);
                    record.SetString(1, nom_ArquivoDestino.Value);
                    record.SetInt32(2, -2);
                    record.SetString(3, "Diretório destino não existe");
                }

            }
            else
            {

                record.SetString(0, nom_ArquivoOrigem.Value);
                record.SetString(1, nom_ArquivoDestino.Value);
                record.SetInt32(2, -1);
                record.SetString(3, "Arquivo origem não existe");

            }


        }
        catch (Exception e)
        {

            record.SetString(0, nom_ArquivoOrigem.Value);
            record.SetString(1, nom_ArquivoDestino.Value);
            record.SetInt32(2, 0);
            record.SetString(3, e.Message);

        }


        // Send the record to the client.
        SqlContext.Pipe.Send(record);
    }


    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void PR_GER_FileMove(SqlString nom_ArquivoOrigem, SqlString nom_ArquivoDestino)
    {

        List<SqlMetaData> colunas = new List<SqlMetaData>();

        colunas.Add(new SqlMetaData("nom_ArquivoOrigem", SqlDbType.VarChar, 700));
        colunas.Add(new SqlMetaData("nom_ArquivoDestino", SqlDbType.VarChar, 700));
        colunas.Add(new SqlMetaData("ind_Status", SqlDbType.Int));
        colunas.Add(new SqlMetaData("nom_Msg", SqlDbType.VarChar, 1000));

        SqlDataRecord record = new SqlDataRecord(colunas.ToArray());

        try
        {
            if (File.Exists(nom_ArquivoOrigem.Value))
            {
                FileInfo fileDestino = new FileInfo(nom_ArquivoDestino.Value);

                if (Directory.Exists(fileDestino.DirectoryName))
                {
                    File.Move(nom_ArquivoOrigem.Value, nom_ArquivoDestino.Value);

                    record.SetString(0, nom_ArquivoOrigem.Value);
                    record.SetString(1, nom_ArquivoDestino.Value);
                    record.SetInt32(2, 1);
                    record.SetString(3, "Operação executada com sucesso");

                }
                else
                {
                    record.SetString(0, nom_ArquivoOrigem.Value);
                    record.SetString(1, nom_ArquivoDestino.Value);
                    record.SetInt32(2, -2);
                    record.SetString(3, "Diretório destino não existe");
                }

            }
            else
            {

                record.SetString(0, nom_ArquivoOrigem.Value);
                record.SetString(1, nom_ArquivoDestino.Value);
                record.SetInt32(2, -1);
                record.SetString(3, "Arquivo origem não existe");

            }


        }
        catch (Exception e)
        {

            record.SetString(0, nom_ArquivoOrigem.Value);
            record.SetString(1, nom_ArquivoDestino.Value);
            record.SetInt32(2, 0);
            record.SetString(3, e.Message);

        }


        // Send the record to the client.
        SqlContext.Pipe.Send(record);
    }


    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void PR_GER_FileDelete(SqlString nom_ArquivoOrigem)
    {

        List<SqlMetaData> colunas = new List<SqlMetaData>();

        colunas.Add(new SqlMetaData("nom_ArquivoOrigem", SqlDbType.VarChar, 700));

        colunas.Add(new SqlMetaData("ind_Status", SqlDbType.Int));
        colunas.Add(new SqlMetaData("nom_Msg", SqlDbType.VarChar, 1000));

        SqlDataRecord record = new SqlDataRecord(colunas.ToArray());

        try
        {
            if (File.Exists(nom_ArquivoOrigem.Value))
            {

                File.Delete(nom_ArquivoOrigem.Value);

                record.SetString(0, nom_ArquivoOrigem.Value);
                record.SetInt32(1, 1);
                record.SetString(2, "Operação executada com sucesso");



            }
            else
            {

                record.SetString(0, nom_ArquivoOrigem.Value);
                record.SetInt32(1, -1);
                record.SetString(2, "Arquivo origem não existe");

            }


        }
        catch (Exception e)
        {

            record.SetString(0, nom_ArquivoOrigem.Value);
            record.SetInt32(1, 0);
            record.SetString(2, e.Message);

        }


        // Send the record to the client.
        SqlContext.Pipe.Send(record);
    }
}
