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
    public class KKW 
    {
        public string kwh_numer {get; set;}
        public int kwh_idheadu {get; set;}
        public string towary {get; set;}
        public int Licznik_Elementow {get; set;}
    

      public static void GetKKW(List<KKW> listaKKW)
        {
             //List<towary> listaTowarow = new();
             setPimstalnew connection = new setPimstalnew();
              connection.Open();

               try{
        
                NpgsqlDataReader myReader = null;
                NpgsqlCommand myCommand = new NpgsqlCommand("select * from meteurosystem.lma_ks_KKW_L", connection.connection);
                myReader = myCommand.ExecuteReader();
                while(myReader.Read())
                    {
                         listaKKW.Add(new KKW()
                            {
                                kwh_numer=myReader["kwh_numer"].ToString(),
                                kwh_idheadu=int.Parse(myReader["kwh_idheadu"].ToString()),
                                towary=myReader["towary"].ToString(),
                                Licznik_Elementow=int.Parse(myReader["Licznik_Elementow"].ToString()),
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