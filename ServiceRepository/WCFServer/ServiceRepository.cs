using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Runtime.Serialization;
using log4net;

namespace NServiceRepository
{
    /**
     * Klasa modułu ServiceRepository
     * */
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServiceRepository : IServiceRepository
    {
        /**
         * Lista z serwisami
         * */
        private List<Service> Services;

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ServiceRepository()
        {
            Services = new List<Service>();
        }

        /**
         *  Rejestrowanie serwisu
         * */
        public void RegisterService(String Name, String Address)
        {
            if (Name == String.Empty || Address == String.Empty)
                throw new EmptyAddressOrNameException();

            if (FindService(Name) != null)
                throw new ServiceAlreadyExistsException();

            var NewService = new Service();
            NewService.Adress = Address;
            NewService.Name = Name;
            log.Info("Zarejestrowano serwis: " + Name + " pod adresem: " + Address);
            Services.Add(NewService);
        }

        /**
         * Pobieranie adresu serwisu
         * */
        public String GetServiceLocation(String Name)
        {
            var Service = FindService(Name);

            if (Service == null) throw new ServiceNotFoundException();

            return Service.Adress;
        }

        /**
         * Wyrejestrowywanie serwisu
         * */
        public void Unregister(String Name)
        {
            var Service = FindService(Name);

            if (Service == null) throw new ServiceNotFoundException();
            log.Info("Wyrejestrowano serwis: " + Name);
            Services.Remove(Service);
        }

        /**
         * Tu będzie odnowa połączenia
         * */
        public void Alive(String Name)
        {
            Console.WriteLine("Still Alive");
        }

        /**
         * Odszukiwanie serwisu w liście Services
         * */
        private Service FindService(String Name)
        {
            return Services.Find(Element => Element.Name == Name);
        }

    }
}
