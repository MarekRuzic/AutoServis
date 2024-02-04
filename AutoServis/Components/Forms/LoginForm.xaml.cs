namespace AutoServis.Components.Forms;
using AutoServis.Views.Desktop.Pages.Registration;
using AutoServis.Views.Mobile.Pages.Registration;
using System.Text.Json;
using AutoServis.Model.API;
using AutoServis.Model;
using AutoServis.Views.Mobile.Pages.Cars;

public partial class LoginForm : ContentView
{
	public LoginForm()
	{
		InitializeComponent();
	}

    private async void showDialog(string title, string message, string buttons)
    {
        await App.Current.MainPage.DisplayAlert(title, message, buttons);
    }

    private async void ClickLogin(object sender, EventArgs e)
    {
        // Kontrola input�
        if (email.Text == "" || email.Text == null)
        {
            showDialog("Ozn�men�", "Nebyl zad�n email", "Ok");
            return;
        }
        string emailInput = email.Text.Trim();

        if (password.Text == "" || password.Text == null)
        {
            showDialog("Ozn�men�", "Nebylo zad�no heslo", "Ok");
            return;
        }
        string passwordInput = password.Text.Trim();

        //Na�ten� usera podle emailu
        API api = new API();

        if (api.checkConnectivity())
        {
            showDialog("Varov�n�", "Nem�te p�ipojen� k internetu, je pot�eba p�ipojen�", "Ok");
            return;
        }

        HttpResponseMessage response = await api.client.
            GetAsync($"user/getuser?email={emailInput}");

        if (response.IsSuccessStatusCode)
        {
            // Na�ten� dat do string s n�sledn� p�evod na Usera
            string getResponsestring = await response.Content.ReadAsStringAsync();
            User user = JsonSerializer.Deserialize<List<User>>(getResponsestring).FirstOrDefault();

            if (!user.checkPassword(passwordInput))
            {
                showDialog("Ozn�men�", "Neplatn� heslo", "Ok");
                return;
            }

            #if ANDROID || IOS
                App.Current.MainPage = new NavigationPage(new MobileCars(user));
            #else
               App.Current.MainPage = new NavigationPage(new MainPage());
            #endif
        }
        else
        {
            await App.Current.MainPage.DisplayAlert("Chyba", "Chyba p�i na��t�n� dat. Zkus te to znovu.", "Ok");
        }
    }

    private void OnEmailCompleted(object sender, EventArgs e)
    {
        password.Focus();
    }

    private void OnPasswordCompleted(object sender, EventArgs e)
    {
        password.Unfocus();
    }

    private async void OpenRegistration(object sender, EventArgs e)
    {
#if ANDROID || IOS
        await Navigation.PushAsync(new MobileRegistration());
#else
        await Navigation.PushAsync(new DesktopRegistration());
#endif

    }
}