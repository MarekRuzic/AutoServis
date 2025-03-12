using AutoServis.Model;
using AutoServis.Model.JSON;
using Microsoft.Maui.ApplicationModel.Communication;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace AutoServis.Services
{
    class UserService
    {
        private API api = new API();

        /// <summary>
        /// Check if User Exist in database throught API connection
        /// </summary>
        /// <param name="email">Email is universal identificator</param>
        /// <returns>If user exist method return true, null for connectivity and false if not</returns>
        public async Task<(bool?, string)> ExistUserInDatabase(string email)
        {
            if (api.checkConnectivity()) return (null, "Nepodařilo se připojit k databázi");

            try
            {
                // Nahrání všech uživatelů pro kontrolu jedinečnosti emailu
                HttpResponseMessage response = await api.client.GetAsync("user/list?limit=99999");
                if (response.IsSuccessStatusCode)
                {
                    string getResponsestring = await response.Content.ReadAsStringAsync();
                    List<User> users = JsonSerializer.Deserialize<List<User>>(getResponsestring);
                    bool uniqueEmail = true;
                    foreach (User userDb in users)
                    {
                        if (userDb.email == email)
                        {
                            uniqueEmail = false;
                            break;
                        }
                    }

                    if (!uniqueEmail)
                    {
                        return (true, "Tento email už někdo využívá zkuste prosím jiný.");
                    }
                }
                else
                {
                    return (true, "Nastala neočkávaná chyba. Zkus se to znovu");
                }
            }
            catch (HttpRequestException ex)
            {
                return (null, "Chyba se spojením. Zkuste to prosím znovu.");
            }
            catch (Exception ex)
            {
                return (null, "Neznámá chyba nastala. Zkuste to prosím znovu.");
            }


            return (false, "Uživatel v databázi ještě neexistuje");
        }

        public async Task<(bool?, string)> Registration(User user)
        {
            if (api.checkConnectivity()) return (null, "Nepodařilo se připojit k databázi");
            string message = "Chyba";
            try
            {             
                HttpResponseMessage response = await api.client.PostAsJsonAsync("user/create", user);

                if (response.IsSuccessStatusCode) return (true, "Uživatel byl úspěšně vytvořen.\n\n" +
                        "Můžete se nyní přihlásit");
                else return(null, "Nastala neočkávaná chyba. Zkus se to znovu");
            }
            catch (HttpRequestException ex)
            {
                message = "Chyba se spojením.";                
            }
            catch (Exception ex)
            {
                message = "Neznámá chyba nastala.";
            }
            return (null, message);
        }

        public async Task<(User?, string?)> UserLogin(string email, string password)
        {
            if (api.checkConnectivity()) return (null, "Nepodařilo se připojit k databázi");
            string message = "Chyba";
            try
            {
                HttpResponseMessage response = await api.client.
                    GetAsync($"user/getuser?email={email}");

                if (response.IsSuccessStatusCode)
                {
                    // Načtení dat do string s následný převod na Usera
                    string getResponsestring = await response.Content.ReadAsStringAsync();
                    User user = JsonSerializer.Deserialize<List<User>>(getResponsestring).FirstOrDefault();

                    // Kontrola uživatele a hesla
                    if (user == null || !user.checkPassword(password))
                    {
                        return (null, "Neplatné uživatelské údaje");
                    }

                    return (user, null);
                }
                else return (null, "Chyba při načítání dat. Zkus te to znovu.");

            }
            catch (HttpRequestException ex)
            {
                return (null, "Chyba se spojením.");
            }
            catch (Exception ex)
            {
                return (null, "Neznámá chyba nastala.");
            }
        }

        public async Task<(bool, string)> UpdateUserDetail(User user)
        {
            if (api.checkConnectivity()) return (false, "Nepodařilo se připojit k databázi");

            try
            {
                HttpResponseMessage response = await api.client.PutAsJsonAsync("user/updateuser", user);
                // Pokud aktualizace proběhla v pohodě uloží se nové udáje do Usera a uživatel je vrácen na domovskou obrazovku
                if (response.IsSuccessStatusCode)
                {
                    return (true, "Data o Vás byla úspěšně změněna");                    
                }
                else return (false, "Nastala neočkávaná chyba. Zkus se to znovu");
                
            }
            catch (HttpRequestException ex)
            {
                return (false, "Chyba se spojením.");
            }
            catch (Exception ex)
            {
                return (false, "Nenámá chyba nastala.");
            }
        }

        public async Task<(bool, string)> UpdateUserPassword(User user)
        {
            if (api.checkConnectivity()) return (false, "Nepodařilo se připojit k databázi");

            try
            {
                HttpResponseMessage response = await api.client.PutAsJsonAsync("user/updatepassworduser", user);
                if (response.IsSuccessStatusCode)
                {
                    return (true, "Heslo bylo úspěšně změněno");
                }
                else return (false, "Nastala neočkávaná chyba. Zkus se to znovu");                
            }
            catch (HttpRequestException ex)
            {
                return (false, "Chyba se spojením.");
            }
            catch (Exception ex)
            {
                return (false, "Nenámá chyba nastala.");
            }
        }
    }
}
