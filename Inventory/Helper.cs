using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inventory
{
    public class HelperClass
    {

        public static string RetrieveConnectionString()
        {
            System.Configuration.Configuration rootWebConfig =
                System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("/MyWebSiteRoot");
            System.Configuration.ConnectionStringSettings connString;
            if (rootWebConfig.ConnectionStrings.ConnectionStrings.Count > 0)
            {
                connString =
                    rootWebConfig.ConnectionStrings.ConnectionStrings["Inventory"];
                if (connString != null)
                    return connString.ConnectionString;
                else
                    return "No Inventory connection string" + rootWebConfig.ConnectionStrings.ConnectionStrings[0].Name;
            }

            return "No Inventory connection string " + rootWebConfig.ConnectionStrings.ConnectionStrings.Count.ToString() ;
        }
    }
}
