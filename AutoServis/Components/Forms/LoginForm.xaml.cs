namespace AutoServis.Components.Forms;
using AutoServis.Views.Desktop.Pages.Registration;
using AutoServis.Views.Mobile.Pages.Registration;
using System.Text.Json;
using AutoServis.Model;
using AutoServis.Views.Mobile.Pages.Cars;
using AutoServis.Repository;
using AutoServis.Services;

public partial class LoginForm : ContentView
{
    private UserService _userService = new UserService();
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
        // Kontrola inputù
        if (this.email.Text == "" || this.email.Text == null)
        {
            showDialog("Oznámení", "Nebyl zadán email", "Ok");
            return;
        }

        if (this.password.Text == "" || this.password.Text == null)
        {
            showDialog("Oznámení", "Nebylo zadáno heslo", "Ok");
            return;
        }

        LoginButton.IsVisible = false;
        LoadingIndicator.IsVisible = true;

        // Naètení inputù do promìnných
        string email = this.email.Text.Trim();
        string password = this.password.Text.Trim();

        (User? user, string? message) = await _userService.UserLogin(email, password);

        if (user == null)
        {
            showDialog("Oznámení", message, "Ok");
            LoginButton.IsVisible = true;
            LoadingIndicator.IsVisible = false;
            return;
        }

        // Pøesmìrování na novou stránku
        App.Current.MainPage = new NavigationPage(new MobileCars(user));

        LoginButton.IsVisible = true;
        LoadingIndicator.IsVisible = false;  
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