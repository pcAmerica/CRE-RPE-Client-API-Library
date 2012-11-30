using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pcAmerica.DesktopPOS.API.Client.CompanyInformationService;
using System.Runtime.InteropServices;

namespace pcAmerica.DesktopPOS.API.Client
{
    public class CompanyInformationAPI
    {
        public List<String> GetStoreIDs()
        {
            using (CompanyInformationServiceClient client = new CompanyInformationServiceClient())
            {
               client.Open();
               return client.GetStoreIDs();
            }
            
        }

        public List<String> GetStationIDs(String StoreID)
        {
            using (CompanyInformationServiceClient client = new CompanyInformationServiceClient())
            {
                client.Open();
                return client.GetStationIDs(StoreID);
            }

        }

        public void GetDBInfo(ref String DBName, ref String InstanceName)
        {
            using (CompanyInformationServiceClient client = new CompanyInformationServiceClient())
            {
                client.Open();
                client.GetDBInfo(ref DBName,ref InstanceName);
            }
        }
    }
}
