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
        // Kontrola input�
        if (this.email.Text == "" || this.email.Text == null)
        {
            showDialog("Ozn�men�", "Nebyl zad�n email", "Ok");
            return;
        }

        if (this.password.Text == "" || this.password.Text == null)
        {
            showDialog("Ozn�men�", "Nebylo zad�no heslo", "Ok");
            return;
        }

        LoginButton.IsVisible = false;
        LoadingIndicator.IsVisible = true;

        // Na�ten� input� do prom�nn�ch
        string email = this.email.Text.Trim();
        string password = this.password.Text.Trim();

        (User? user, string? message) = await _userService.UserLogin(email, password);

        if (user == null)
        {
            showDialog("Ozn�men�", message, "Ok");
            LoginButton.IsVisible = true;
            LoadingIndicator.IsVisible = false;
            return;
        }

        // P�esm�rov�n� na novou str�nku
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