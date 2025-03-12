using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AutoServis.Model;

namespace AutoServis.Services
{
    public class RepairService
    {
        private API api = new API();
        public RepairService() { }

        public async Task<List<Repair>?> GetRepairs(int carId) 
        {
            if (api.checkConnectivity()) return null;
            try
            {                
                HttpResponseMessage responseMessage = await api.client.GetAsync($"repair/list?id={carId}");
                if (responseMessage.IsSuccessStatusCode)
                {
                    List<Repair> repairs = new List<Repair>();
                    string getResponseString = await responseMessage.Content.ReadAsStringAsync();
                    repairs = JsonSerializer.Deserialize<List<Repair>>(getResponseString);

                    return repairs;
                }
                else return null;
            }
            catch (HttpRequestException ex)
            {
                
            }
            catch (Exception ex)
            {
                
            }

            return null; 
        }
    }
}
