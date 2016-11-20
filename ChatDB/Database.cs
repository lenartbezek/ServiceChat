using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ChatDB
{
    public class Database
    {
        public static string ConnectionString = System.Web.Configuration
            .WebConfigurationManager
            .OpenWebConfiguration("/Web")
            .ConnectionStrings.ConnectionStrings["SQL_Connection"].ConnectionString;
    }
}