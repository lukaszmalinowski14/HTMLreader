using System;
using Npgsql;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Drawing;
using System.Reflection;
using System.Text;

namespace HTMLreader
{
    partial class Program
    {
    public class METODY
    {
        public static void InsertRaportToDB(List<raport> Raport)
        {
            setPimstalnew connection = new setPimstalnew();
            NpgsqlCommand cmd = null;
            connection.Open();
            
             try{ 
                    NpgsqlDataReader myReader = null;
                    NpgsqlCommand myCommand = new NpgsqlCommand("DELETE FROM meteurosystem.lma_ks_Czasy_lki", connection.connection);
                    myReader = myCommand.ExecuteReader();
                }
            catch(NpgsqlException ex)
                {
                Console.WriteLine(ex);
                }

                  connection.Close();
                  
                foreach(var mvt in Raport)
        {
            try
        {
           //  conn.Open();
            String sql = "INSERT INTO meteurosystem.lma_ks_Czasy_lki(ttw_idtowaru, kwh_idheadu, duration)"+
                "VALUES (:ttw_idtowaru, :kwh_idheadu, :duration);";

            connection.Open();
            cmd = new NpgsqlCommand(sql, connection.connection);
            cmd.Parameters.Add(new NpgsqlParameter("ttw_idtowaru", NpgsqlTypes.NpgsqlDbType.Integer)); 
            cmd.Parameters[0].Value = mvt.ttw_idtowaru;
            cmd.Parameters.Add(new NpgsqlParameter("kwh_idheadu", NpgsqlTypes.NpgsqlDbType.Integer)); 
            cmd.Parameters[1].Value = mvt.kwh_idheadu;
            cmd.Parameters.Add(new NpgsqlParameter("duration", NpgsqlTypes.NpgsqlDbType.Integer)); 
            cmd.Parameters[2].Value = mvt.CzasLki;
            cmd.Prepare();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if(cmd!=null) cmd.Dispose();
            if(connection!=null) connection.Close();
        }
        }

        }
        
    }
    }
}