using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
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

        public async Task<(bool?, string)> RemoveRepair(int repairId)
        {
            if (api.checkConnectivity()) return (null, "Chyba se spojením. Zkuste to znovu");
            try
            {
                HttpResponseMessage response = await api.client.DeleteAsync($"repair/delete?id={repairId}");
                if (response.IsSuccessStatusCode)
                {
                    return (true, "Oprava byla uspěšně smazána.");
                }
                else
                {
                    return (false, $"Došlo k chybě při mazání opravy.");
                }
            }
            catch (HttpRequestException ex)
            {
                return (false, "Chyba se spojením.");
            }
            catch (Exception ex)
            {
                return (false, "Nenámá chyba nastala.");
            }
            return (null, "Nastala neznámá chyba");
        }

        public async Task<(bool?, string)> InsertRepair(Repair repair)
        {
            if (api.checkConnectivity()) return (null, "Chyba se spojením. Zkuste to znovu");
            try
            {
                HttpResponseMessage response = await api.client.PostAsJsonAsync("repair/create", repair);
                if (response.IsSuccessStatusCode)
                {
                    return (true, "Oprava byla uspěšně vytvořena.");
                }
                else
                {
                    return (false, $"Došlo k chybě při vytváření opravy.");
                }
            }
            catch (HttpRequestException ex)
            {
                return (false, "Chyba se spojením.");
            }
            catch (Exception ex)
            {
                return (false, "Nenámá chyba nastala.");
            }

            return (null, "Nastala neznámá chyba");
        }

        public async Task<(bool?, string)> UpdateRepair(Repair repair)
        {
            if (api.checkConnectivity()) return (null, "Chyba se spojením. Zkuste to znovu");
            try
            {
                HttpResponseMessage response = await api.client.PutAsJsonAsync("repair/update", repair);
                if (response.IsSuccessStatusCode)
                {
                    return (true, "Oprava byla uspěšně aktualizována.");
                }
                else
                {
                    return (false, $"Došlo k chybě při aktualizaci opravy.");
                }
            }
            catch (HttpRequestException ex)
            {
                return (false, "Chyba se spojením.");
            }
            catch (Exception ex)
            {
                return (false, "Nenámá chyba nastala.");
            }

            return (null, "Nastala neznámá chyba");
        }
    }
}
