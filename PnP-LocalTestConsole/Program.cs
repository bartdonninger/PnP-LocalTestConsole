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
                    var template = provider.GetTemplate("PropertyBag_Template.xml");
                    template.Connector = provider.Connector;

                    if (template != null)
                    {
                        using (var clientContext2 = clientContext.Clone("https://sharepoint.dev.com/sites/testsitecollection/subsite1"))
                        {
                            //Web createdWeb = clientContext.Site.OpenWeb(new Uri("https://sharepoint.dev.com/sites/testsitecollection/subsite1").PathAndQuery);



                            clientContext2.Web.ApplyProvisioningTemplate(template);

                            // Todo: Should be added as tokens to the template, so the propertybag entries can be
                            //  set in the PnP Template it self.
                            // Check if tokens are null, insert them into the property bag
                            //if (tokens != null)
                            //{
                            //foreach (var token in tokens)
                            //{
                            //// Create the propertybag entry and add the keys as indexed propertybag
                            //createdWeb.SetPropertyBagValue(token.Key, token.Value);
                            //createdWeb.AddIndexedPropertyBagKey(token.Key);
                            //}
                            //}
                        }
                    }
                }

                Console.ReadLine();

            }

        }

    }
}
