using AutoServis.Model;
using AutoServis.Model.API;
using System.Net.Http.Json;
using System.Text.Json;

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
            showDialog("Ozn�men�", "Hesla se neshoduj�", "Ok");
            return;
        }

        if (FirstnameInput.Text == null ||
            lastnameInput.Text == null ||
            emailInput.Text == null)
        {
            showDialog("Ozn�men�", "Nebyli vypln�ny v�echny �daje!", "Ok");
            return;
        }

        string firstname = FirstnameInput.Text.Trim();
        string lastname = lastnameInput.Text.Trim();
        string email = emailInput.Text.Trim();
        string password = passwordInput.Text.Trim();

        API api = new API();


        if (api.checkConnectivity())
        {
            showDialog("Chyba", "Nejste p�ipojeni k internetu.\n\n" +
                "Je pot�eba internetov� p�ipojen�!", "Ok");
            return;
        }

        // Nahr�n� v�ech u�ivatel� pro kontrolu jedine�nosti emailu
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
                showDialog("Ozn�men�", "Tento email u� n�kdo vyu��v� zkuste pros�m jin�.", "Ok");
                return;
            }
        }
        else
        {
            showDialog("Chyba", "Nastala neo�k�van� chyba. Zkus se to znovu", "Ok");
            return;
        }

        // Vytvo�en� nov�ho u�ivatele
        User user = new User(firstname, lastname, email, password);

        if (api.checkConnectivity())
        {
            showDialog("Chyba", "Nejste p�ipojeni k internetu.\n\n" +
                "Je pot�eba internetov� p�ipojen�!", "Ok");
            return;
        }

        response = await api.client.
            PostAsJsonAsync("user/create", user);

        if (response.IsSuccessStatusCode)
        {
            showDialog("�sp�ch", "U�ivatel byl �sp�n� vytvo�en.\n\n" +
                "M��ete se nyn� p�ihl�sit", "OK");
            // P�esun zp�t na p�ihla�ovac� str�nku
            await Navigation.PopAsync();
        }
        else
        {
            showDialog("Chyba", "Nastala neo�k�van� chyba. Zkus se to znovu", "Ok");
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