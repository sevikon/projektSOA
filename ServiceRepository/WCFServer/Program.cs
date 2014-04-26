using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Runtime.Serialization;
using NServiceRepository;
using System.Configuration;

/**
 * ######## ServiceRepository #########
 * 
 * Authors: Mateusz Ścirka, Konrad Seweryn
 * 
 * */

namespace NServiceRepository
{ 
    /**
     * Klasa główna programu
     * */
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ServiceRepository Repository = new ServiceRepository();
                //Repository.Register("yoyo", "yyyy");
                string serviceRepoAddress = ConfigurationSettings.AppSettings["serviceRepoAddress"];
                var Server = new ServiceRepositoryHost(Repository, serviceRepoAddress);
                Server.AddDefaultEndpoint("net.tcp://localhost:41234/IServiceRepository");

                ServiceDebugBehavior debug = Server.Description.Behaviors.Find<ServiceDebugBehavior>();

                // if not found - add behavior with setting turned on 
                if (debug == null)
                {
                    Server.Description.Behaviors.Add(
                         new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });
                }
                else
                {
                    // make sure setting is turned ON
                    if (!debug.IncludeExceptionDetailInFaults)
                    {
                        debug.IncludeExceptionDetailInFaults = true;
                    }
                }
                Server.Open();

                Console.WriteLine("Chyba działa...");
            }
            catch (ServiceRepositoryException Ex)
            {
                Console.WriteLine(Ex.Message);
            }

            Console.ReadLine();
        }
    }
}
