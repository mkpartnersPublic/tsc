using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServiceTutorial
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
                    return connString.ConnectionString ;
                else
                    return "No Inventory connection string";
            }

            return "No Inventory connection string";
        }
    }
}
