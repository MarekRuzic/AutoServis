using AutoServis.Model;
using AutoServis.Model.API;
using Microsoft.Maui.Controls.Compatibility;
using System.Text.Json;

namespace AutoServis
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
            zobraz();
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

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }

}
