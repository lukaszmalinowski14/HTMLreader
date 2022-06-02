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
    public class towary 
    {
            public string ttw_klucz {get; set;}
            public int ttw_idtowaru {get; set;}

            public static void GetTowary(List<towary> listaTowarow)
        {
             //List<towary> listaTowarow = new();
             setPimstalnew connection = new setPimstalnew();
              connection.Open();

               try{
        
                NpgsqlDataReader myReader = null;
                NpgsqlCommand myCommand = new NpgsqlCommand("select ttw_idtowaru, ttw_klucz from tg_towary where tgr_idgrupy=107", connection.connection);
                myReader = myCommand.ExecuteReader();
                while(myReader.Read())
                    {
                         listaTowarow.Add(new towary()
                            {
                                ttw_klucz=myReader["ttw_klucz"].ToString(),
                                ttw_idtowaru=int.Parse(myReader["ttw_idtowaru"].ToString()),
                            });
                    }
               }
                catch (NpgsqlException ex)
                {
                Console.WriteLine(ex);
                }

                connection.Close();
            
        }
    }
    }
}