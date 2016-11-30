using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core.Framework.Provisioning.Providers.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace PnP_LocalTestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string userName = "sp-install@Dev.com";
            Console.WriteLine("Enter your password.");

            OfficeDevPnP.Core.AuthenticationManager aManager = new OfficeDevPnP.Core.AuthenticationManager();
            using (var clientContext = aManager.GetWebLoginClientContext("https://sharepoint.dev.com/sites/testsitecollection/"))
            {
                // Get the SharePoint web  
                Web web = clientContext.Web;
                // Load the Web properties  
                clientContext.Load(web);
                // Execute the query to the server.  
                clientContext.ExecuteQuery();
                // Web properties - Display the Title and URL for the web  
                Console.WriteLine("Title: " + web.Title + "; URL: " + web.Url);


                // Read the XML file
                XMLTemplateProvider provider = new XMLFileSystemTemplateProvider(String.Format(@"{0}\Templates\", Environment.CurrentDirectory), "");


                if (provider != null)
                {
                    // Get the template
                    var template = provider.GetTemplate("RoleDefinition_Template.xml");
                    template.Connector = provider.Connector;

                    if (template != null)
                    {
                        clientContext.Web.ApplyProvisioningTemplate(template);
                    }
                }

                Console.ReadLine();

            }

        }

    }
}
