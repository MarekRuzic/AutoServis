using AutoServis.Model;
using System.Net.Http.Json;
using System.Text.Json;
using BCrypt.Net;
using AutoServis.Services;

namespace AutoServis.Components.Forms;

public partial class RegistrationForm : ContentView
{
    private UserService _userService = new UserService();
	public RegistrationForm()
	{
		InitializeComponent();
	}

    private async void DisplayAlert(string title, string message, string buttons, bool isAwait)
    {
        if (isAwait)
        {
            await App.Current.MainPage.DisplayAlert(title, message, buttons);
            return;
        }
        App.Current.MainPage.DisplayAlert(title, message, buttons);
    }

    private async void Restration(object sender, EventArgs e)
    {
        if (passwordInput.Text != passwordRepeatInput.Text)
        {
            DisplayAlert("Oznámení", "Hesla se neshodují", "Ok", false);
            return;
        }

        if (FirstnameInput.Text == null ||
            lastnameInput.Text == null ||
            emailInput.Text == null)
        {
            DisplayAlert("Oznámení", "Nebyli vyplnìny všechny údaje!", "Ok", false);
            return;
        }

        RegisterButton.IsVisible = false;
        LoadingIndicator.IsVisible = true;

        string firstname = FirstnameInput.Text.Trim();
        string lastname = lastnameInput.Text.Trim();
        string email = emailInput.Text.Trim();
        string password = passwordInput.Text.Trim();

        // Kontrola zda uživatel s daným email již není registrován
        (bool ? exists, string message) = await _userService.ExistUserInDatabase(email);
       
        if (exists == null)
        {
            DisplayAlert("Chyba", message, "Ok", false);
            RegisterButton.IsVisible = true;
            LoadingIndicator.IsVisible = false;
            return;
        }

        if (exists == true)
        {
            DisplayAlert("Oznámení", message, "Ok", false);
            RegisterButton.IsVisible = true;
            LoadingIndicator.IsVisible = false;
            return;
        }

        // Pokraèování registrace pokud uživatel nebyl nalezen
        // HashPassword a vytoøení usera a poslání data na API
        string hashPassword = BCrypt.Net.BCrypt.HashPassword(password);
        User user = new User(firstname, lastname, email, hashPassword);

        (exists, message) = await _userService.Registration(user);
        if (exists == null)
        {
            DisplayAlert("Chyba", message, "Ok", false);
            RegisterButton.IsVisible = true;
            LoadingIndicator.IsVisible = false;
            return;
        }

        if (exists == true)
        {
            DisplayAlert("Oznámení", message, "Ok", false);
            await Navigation.PopAsync();
            RegisterButton.IsVisible = true;
            LoadingIndicator.IsVisible = false;
            return;
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