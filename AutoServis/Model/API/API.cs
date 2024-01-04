using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AutoServis.Model.API
{
    class API
    {
        public HttpClient client { get; set; }

        public API()
        {
            client = new HttpClient();
            //client.BaseAddress = new Uri("http://localhost/projects/API_carService/index.php/");
            //client.BaseAddress = new Uri("http://192.168.1.186/projects/API_carService/index.php/");
            client.BaseAddress = new Uri("http://ruzicka-marek.4fan.cz/API_carService/index.php/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("appliaciton/json"));
            byte[] encryptedApiKey = Encoding.ASCII.GetBytes("ahoj");
            client.DefaultRequestHeaders.Add("KEYAPI", "ahoj");
        }

        public bool checkConnectivity()
        {
            return Connectivity.Current.NetworkAccess != NetworkAccess.Internet;
        }
    }
}
