using System.Web.Configuration;

namespace ServiceChat
{
    public class Database
    {
        public static string ConnectionString { get; } = WebConfigurationManager
            .OpenWebConfiguration("/Web")
            .ConnectionStrings.ConnectionStrings["SQL_Connection"].ConnectionString;
    }
}