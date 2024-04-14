using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AutoServis.Model
{
    public class API
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
            // Zabezpečený API klíč pro komunikaci
            client.DefaultRequestHeaders.Add("KEYAPI", "$2a$11$rKbwa1GLzIh74MOo4YQTuu7TP.VOj7t9GKJtj6EhpRUN/cjNI5OYy");
        }

        public bool checkConnectivity()
        {
            return Connectivity.Current.NetworkAccess != NetworkAccess.Internet;
        }

        public string[] GetConnectionMessage()
        {
            string[] message = { "Chyba","Nejste připojeni k internetu.\n\nJe potřeba internetové připojení!", "Ok" };
            return message;
        }
    }
}
