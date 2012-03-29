using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using ActiveDirectoryForCloud.Proxy;
using System.ServiceModel.Description;

namespace ActiveDirectoryProxy
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new ServiceHost(typeof(ProxyService));
            host.Open();

            foreach (ServiceEndpoint endpoint in host.Description.Endpoints)
            {
                Console.WriteLine("Listening on {0}", endpoint.Address.Uri);
            }

            Console.WriteLine("Press enter to stop the host");
            Console.ReadLine();

            host.Close();
        }
    }
}
