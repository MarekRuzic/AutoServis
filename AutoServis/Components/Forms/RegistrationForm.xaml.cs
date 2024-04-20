using AutoServis.Model;
using System.Net.Http.Json;
using System.Text.Json;
using BCrypt.Net;

namespace AutoServis.Components.Forms;

public partial class RegistrationForm : ContentView
{
	public RegistrationForm()
	{
		InitializeComponent();
	}

    private async void showDialog(string title, string message, string buttons)
    {
        await App.Current.MainPage.DisplayAlert(title, message, buttons);
    }

    private async void Restration(object sender, EventArgs e)
    {
        if (passwordInput.Text != passwordRepeatInput.Text)
        {
            showDialog("Oznámení", "Hesla se neshodují", "Ok");
            return;
        }

        if (FirstnameInput.Text == null ||
            lastnameInput.Text == null ||
            emailInput.Text == null)
        {
            showDialog("Oznámení", "Nebyli vyplnìny všechny údaje!", "Ok");
            return;
        }

        string firstname = FirstnameInput.Text.Trim();
        string lastname = lastnameInput.Text.Trim();
        string email = emailInput.Text.Trim();
        string password = passwordInput.Text.Trim();

        API api = new API();


        if (api.checkConnectivity())
        {
            showDialog("Chyba", "Nejste pøipojeni k internetu.\n\n" +
                "Je potøeba internetové pøipojení!", "Ok");
            return;
        }

        RegisterButton.IsVisible = false;
        LoadingIndicator.IsVisible = true;

        try
        {
            // Nahrání všech uživatelù pro kontrolu jedineènosti emailu
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
                    showDialog("Oznámení", "Tento email už nìkdo využívá zkuste prosím jiný.", "Ok");
                    RegisterButton.IsVisible = true;
                    LoadingIndicator.IsVisible = false;
                    return;
                }
            }
            else
            {
                showDialog("Chyba", "Nastala neoèkávaná chyba. Zkus se to znovu", "Ok");
                RegisterButton.IsVisible = true;
                LoadingIndicator.IsVisible = false;
                return;
            }

            string hashPassword = BCrypt.Net.BCrypt.HashPassword(password);

            // Vytvoøení nového uživatele
            User user = new User(firstname, lastname, email, hashPassword);

            if (api.checkConnectivity())
            {
                showDialog("Chyba", "Nejste pøipojeni k internetu.\n\n" +
                    "Je potøeba internetové pøipojení!", "Ok");
                RegisterButton.IsVisible = true;
                LoadingIndicator.IsVisible = false;
                return;
            }

            response = await api.client.
                PostAsJsonAsync("user/create", user);

            if (response.IsSuccessStatusCode)
            {
                showDialog("Úspìch", "Uživatel byl úspìšnì vytvoøen.\n\n" +
                    "Mùžete se nyní pøihlásit", "OK");
                // Pøesun zpìt na pøihlašovací stránku
                await Navigation.PopAsync();
            }
            else
            {
                showDialog("Chyba", "Nastala neoèkávaná chyba. Zkus se to znovu", "Ok");
            }
        }
        catch (HttpRequestException ex)
        {
            await App.Current.MainPage.DisplayAlert("Chyba", "Chyba se spojením.", "Ok");
        }
        catch (Exception ex)
        {
            await App.Current.MainPage.DisplayAlert("Chyba", "Neznámá chyba nastala.", "Ok");
        }
        finally
        {
            RegisterButton.IsVisible = true;
            LoadingIndicator.IsVisible = false;
        }
    }

    private void InputComplete(object sender, EventArgs e)
    {
        Entry entry = (Entry)sender;
        if (entry == null) return;
        if (entry.ReturnType == ReturnType.Done)
        {
            entry.Unfocus();
        }
        else if (entry.ReturnType == ReturnType.Next)
        {

        }
    }
}