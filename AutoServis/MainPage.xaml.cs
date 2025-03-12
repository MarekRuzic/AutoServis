using AutoServis.Model;
using AutoServis.Model.JSON;
using AutoServis.Views.Mobile.Pages.Login;
using AutoServis.Views.Desktop.Pages.Login;
using Microsoft.Maui.Controls.Compatibility;
using System.Text.Json;
using AutoServis.Views.Mobile.Pages.Cars;

namespace AutoServis
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            //zobraz();
            zobraz2();
        }

        private async void zobraz2()
        {
            API api = new API();

            //HttpResponseMessage responseMessage = await api.client.GetAsync("car/list?limit=99999");
            HttpResponseMessage responseMessage = await api.client.GetAsync($"car/listcaruser?id=1");
            if (responseMessage.IsSuccessStatusCode)
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    Converters = { new BoolConverter() },
                };
                string getResponseString = await responseMessage.Content.ReadAsStringAsync();
                List<Car> cars = JsonSerializer.Deserialize<List<Car>>(getResponseString, options);
                allUsers.Children.Clear();
                foreach (Car car in cars)
                {
                    // Vytvoření borderu s červenou čárou o tloušťce 3
                    Frame borderFrame = new Frame
                    {
                        StyleId = car.id.ToString(),
                        BorderColor = Color.FromRgb(255, 0, 0),
                        CornerRadius = 5,
                        Padding = new Thickness(10),
                        HasShadow = false,
                        Content = new Microsoft.Maui.Controls.StackLayout
                        {
                            Children =
                            {
                                new Label { Text = $"Id uživatele: {car.id}" },
                                new Label { Text = car.brand },
                                new Label { Text = car.model },
                                new Label { Text = car.manufacture.ToString() },
                                // Další labely můžete přidat podle potřeby
                            }
                        }
                    };

                    // Přidání vytvořeného borderu do existujícího kontejneru (např. StackLayout)
                    // Zde předpokládám, že máte StackLayout s názvem "myStackLayout"
                    allUsers.Children.Add(borderFrame);
                }
                allUsers.Children.Add(form2);
                Button button = new Button
                {
                    Text = "new page"
                };
                //button.Clicked += async (sender, args) => await Navigation.PushAsync(new MobileCars());
                allUsers.Children.Add(button);

                
            }
            else
            {
                await DisplayAlert("Fuj", "No fuj co se to stalo", "Yes", "No");
            }
        }

        private async void zobraz()
        {
            API api = new API();

            HttpResponseMessage response = await api.client.GetAsync("user/list?limit=99999");
            if (response.IsSuccessStatusCode)
            {
                string getResponsestring = await response.Content.ReadAsStringAsync();
                List<User> users = JsonSerializer.Deserialize<List<User>>(getResponsestring);
                allUsers.Children.Clear();
                foreach (User user in users)
                {
                    // Vytvoření borderu s červenou čárou o tloušťce 3
                    Frame borderFrame = new Frame
                    {
                        StyleId = user.id.ToString(),
                        BorderColor = Color.FromRgb(255, 0, 0),
                        CornerRadius = 5,
                        Padding = new Thickness(10),
                        HasShadow = false,
                        Content = new Microsoft.Maui.Controls.StackLayout
                        {
                            Children =
                            {
                                new Label { Text = $"Id uživatele: {user.id}" },
                                new Label { Text = user.firstname },
                                new Label { Text = user.lastname },
                                new Label { Text = user.email },
                                // Další labely můžete přidat podle potřeby
                            }
                        }
                    };

                    // Přidání vytvořeného borderu do existujícího kontejneru (např. StackLayout)
                    // Zde předpokládám, že máte StackLayout s názvem "myStackLayout"
                    allUsers.Children.Add(borderFrame);                    
                }
                allUsers.Children.Add(form2);
            }
            else
            {
                await DisplayAlert("Fuj", "No fuj co se to stalo", "Yes", "No");
            }
        }

        private void LogOut(object sender, EventArgs e)
        {
            #if ANDROID || IOS
                App.Current.MainPage = new NavigationPage(new MobileLogin());
            #else
                App.Current.MainPage = new NavigationPage(new DesktopLogin());
            #endif
        }
    }

}
