using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace lamsef.Modules
{
    public class DBModules
    {
        public SqlConnection DB_Connection;

        public bool DB_Connect()
        {
            DB_Connection = new SqlConnection();
            try
            {
                DB_Connection.ConnectionString = "Data Source=" + ConstModules.constDB_Source + ";"
                                                 + "Initial Catalog=" + ConstModules.constDB_Name + ";"
                                                 + "Integrated Security=True;MultipleActiveResultSets=True";
                if(DB_Connection.State == System.Data.ConnectionState.Closed)
                {
                    DB_Connection.Open();
                }
                return true;
            }
            catch(Exception ex)
            {
                {
                    if(DB_Connection != null)
                    {
                        DB_Connection.Close();
                    }
                    return false;
                }
            }
        }

        public bool DB_Close()
        {
            try
            {
                if(DB_Connection.State == System.Data.ConnectionState.Open)
                {
                    DB_Connection.Close();
                }
                DB_Connection.Dispose();
                DB_Connection = null;
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public bool DB_SqlReader(string strSql, ref SqlDataReader sqlRdr)
        {
            SqlCommand sqlCmn = new SqlCommand();

            try
            {
                sqlCmn = new SqlCommand(strSql, DB_Connection);
                sqlCmn.Connection = DB_Connection;
                sqlRdr = sqlCmn.ExecuteReader();
                sqlCmn.Dispose();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}