using System;
using Npgsql;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace HTMLreader
{
    partial class Program
    {
    abstract class Connect
    {

        public NpgsqlConnection connection;

        public  Connect(String SERVER, string PORT, String USER, String PASSWORD, String DATABASE, String SslMode)
        {
                connection = new NpgsqlConnection(
                "Server=" + SERVER + ";" +
                "Port=" + PORT + ";" +
                "User Id=" + USER + ";" +
                "Password=" + PASSWORD + ";" +
                "Timeout=1024;"+

                "Database=" + DATABASE + ";"+
                "SslMode=" +SslMode+ ";"
            );
        }

        public NpgsqlConnection Connection 
        {

            get {return connection;}
            set{ }
         }

        public virtual void Open()
        {
            try
            {
            connection.Open();
            }
            catch (NpgsqlException ex)
            {
            Console.WriteLine(ex);
            }
        }

        public virtual void Close()
        {
             try
            {
               connection.Close();
            }
            catch (NpgsqlException ex)
            {
            Console.WriteLine(ex);
            }

        }
    }
        class setPimstalnew:Connect
        {
            public setPimstalnew (String SERVER= "10.1.5.30", string PORT= "5432", String USER= "testdbuser", String PASSWORD= "Xai7aer7pu", String DATABASE="pimstalnew", String SslMode= "Disable")
            :base (SERVER,PORT,USER,PASSWORD,DATABASE,SslMode)
            {}
        // public void Pimstalnew()
        // { 
        //     String SERVER = "10.1.5.30";
        //     string PORT = "5432";
        //     String USER = "testdbuser";
        //     String PASSWORD = "Xai7aer7pu";
        //     String DATABASE = "pimstalnew";
        //     String SslMode = "Disable";
        //     //Connect(SERVER,PORT,USER,PASSWORD,DATABASE,SslMode,SslMode);
        // }
        }

        class setPraporty:Connect
        {
            public setPraporty (String SERVER="10.1.5.30", string PORT = "5432", String USER = "testdbuser", String PASSWORD = "Xai7aer7pu", String DATABASE = "praporty", String SslMode = "Disable")
            :base (SERVER,PORT,USER,PASSWORD,DATABASE,SslMode)
            {}
        // public void Praporty()
        // {
        //     String SERVER = "10.1.5.30";
        //     string PORT = "5432";
        //     String USER = "testdbuser";
        //     String PASSWORD = "Xai7aer7pu";
        //     String DATABASE = "praporty";
        //     String SslMode = "Disable";

          
        // }
        }
    }
}
