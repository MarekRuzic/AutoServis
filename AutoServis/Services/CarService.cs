using AutoServis.Model;
using AutoServis.Model.JSON;
using Microsoft.Maui.Layouts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AutoServis.Services
{
    public class CarService : ICarService
    {
        private API api = new API();

        private bool isApiConnected()
        {
            if (api.checkConnectivity())
            {
                DisplayAlert("Oznámení", "Není připojení k internetu. Proto nemohli být načteny data", "Zavřít", false);
                return false;
            }
            return true;
        }

        public async void DisplayAlert(string title, string message, string buttons, bool isAwait)
        {
            if (isAwait)
            {
                await App.Current.MainPage.DisplayAlert(title, message, buttons); 
                return;
            }
            App.Current.MainPage.DisplayAlert(title, message, buttons);
        }


        public async Task<List<Car>> LoadUserCars(int userId, API api)
        {
            if (!isApiConnected()) return null;            

            try
            {
                HttpResponseMessage responseMessage = await api.client.GetAsync($"car/listcaruser?id={userId}");
                if (responseMessage.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        Converters = { new BoolConverter() },
                    };
                    string getResponseString = await responseMessage.Content.ReadAsStringAsync();
                    List<Car> cars = JsonSerializer.Deserialize<List<Car>>(getResponseString, options);

                    if (cars != null && cars.Count > 0)
                    {
                        return cars;
                    }
                }
                else
                {
                    DisplayAlert("Oznámení", "Při načítání dat z databáze došlo k chybě.", "Zavřít", true);
                }
            }
            catch (HttpRequestException ex)
            {
                DisplayAlert("Chyba", "Chyba se spojením.", "Ok", true);
            }
            catch (Exception ex)
            {
                DisplayAlert("Chyba", "Neznámá chyba nastala.", "Ok", true);
            }
            return null;
        }

        public async Task<bool> DeleteCar(int carId)
        {
            if (!isApiConnected()) return false;

            try
            {
                HttpResponseMessage response = await api.client.DeleteAsync($"car/delete?id={carId}");
                if (response.IsSuccessStatusCode)
                {
                    await App.Current.MainPage.DisplayAlert("Oznámení", $"Auto bylo uspěšně smazáno.", "Ok");
                    return true;
                }
                await App.Current.MainPage.DisplayAlert("Oznámení", $"Došlo k chybě při mazání vozidla.", "Ok");
            }
            catch (HttpRequestException ex)
            {
                await App.Current.MainPage.DisplayAlert("Chyba", "Chyba se spojením.", "Ok");
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Chyba", "Nenámá chyba nastala.", "Ok");
            }
            return false;
        }


        public async Task<bool> InsertNewCar(Car car)
        {
            if (!isApiConnected()) return false;

            try
            {
                HttpResponseMessage response = await api.client.PostAsJsonAsync("car/create", car);
                if (response.IsSuccessStatusCode)
                {
                    DisplayAlert("Oznámení", "Vozidlo bylo úspěšně vytvořeno." +
                    "\n\nPo stisku tlačítka budete přesměrování na úvodní stránku 😊", "OK", false);
                    return true;
                }
                else DisplayAlert("Chyba", "Nastala neočkávaná chyba. Zkus se to znovu", "Ok", false);
            }
            catch (HttpRequestException ex)
            {
                DisplayAlert("Chyba", "Chyba se spojením.", "Ok", true);
            }
            catch (Exception ex)
            {
                DisplayAlert("Chyba", "Neznámá chyba nastala.", "Ok", true);
            }

            return false;
        }


        public async Task<bool> UpdateCar(Car car)
        {
            if (!isApiConnected()) return false;

            try
            {
                HttpResponseMessage response = await api.client.PutAsJsonAsync("car/update", car);
                if (response.IsSuccessStatusCode) 
                {
                    DisplayAlert("Oznámení", "U vozidla byla úspěšně změněna data 😊", "OK", false);
                    return true;
                }
                else DisplayAlert("Chyba", "Nastala neočkávaná chyba. Zkus se to znovu", "Ok", false);
            }
            catch (HttpRequestException ex)
            {
                DisplayAlert("Chyba", "Chyba se spojením.", "Ok", true);
            }
            catch (Exception ex)
            {
                DisplayAlert("Chyba", "Neznámá chyba nastala.", "Ok", true);
            }

            return false;
        }


    }
}
