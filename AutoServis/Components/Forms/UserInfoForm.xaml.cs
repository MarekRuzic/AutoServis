namespace AutoServis.Components.Forms;
using AutoServis.Model;
using Microsoft.Maui.ApplicationModel.Communication;
using System.Net.Http.Json;
using BCrypt.Net;
using AutoServis.Views.Mobile.Pages.Cars;
using AutoServis.Components.Templates;
using System.ComponentModel;
using AutoServis.Services;

public partial class UserInfoForm : ContentView
{
    private UserService _userService = new UserService();

    public UserInfoForm()
	{
		InitializeComponent();
	}

    public User user { get; set; }

    public void SetEntryValues(User user)
    {
        FirstnameInput.Text = user.firstname;
        lastnameInput.Text = user.lastname;
        emailInput.Text = user.email;
    }

    private bool IsInputEmpty(String text)
    {
        if (text == null || text.Trim() == "") return true;
        return false;
    }

    private void InputComplete(object sender, EventArgs e)
    {

    }

    private async void UpdateUserDetailClick(object sender, EventArgs e)
    {
        if (IsInputEmpty(FirstnameInput.Text) &&
            IsInputEmpty(lastnameInput.Text) &&
            IsInputEmpty(emailInput.Text))
        {
            App.Current.MainPage.DisplayAlert("Oznámení", "Nìjaký z uživatelských údajù nebyl vyplnìn!", "Ok");
            return;
        }

        UpdateUserButton.IsVisible = false;
        LoadingIndicator.IsVisible = true;

        string firstname = FirstnameInput.Text.Trim();
        string lastname = lastnameInput.Text.Trim();
        string email = emailInput.Text.Trim();

        (bool? success, string message) = await _userService.ExistUserInDatabase(email);

        if (success == null || success == true)
        {
            App.Current.MainPage.DisplayAlert("Chyba", message, "Ok");
            UpdateUserButton.IsVisible = true;
            LoadingIndicator.IsVisible = false;
            return;
        }

        this.user.UpdateUserCredits(firstname, lastname, email);
        (success, message) = await _userService.UpdateUserDetail(user);


        if (success == false)
        {
            App.Current.MainPage.DisplayAlert("Chyba", message, "Ok");
            UpdateUserButton.IsVisible = true;
            LoadingIndicator.IsVisible = false;
            return;
        }

        if (success == true)
        {
            App.Current.MainPage.DisplayAlert("Oznámení", message, "Ok");
            UpdateUserButton.IsVisible = true;
            LoadingIndicator.IsVisible = false;
            await Navigation.PopAsync();
            return;
        }

        UpdateUserButton.IsVisible = true;
        LoadingIndicator.IsVisible = false;        
    }

    private async void UpdatePasswordClick(object sender, EventArgs e)
    {
        if (IsInputEmpty(lastPasswordInput.Text) &&
            IsInputEmpty(passwordInput.Text) &&
            IsInputEmpty(passwordRepeatInput.Text))
        {
            App.Current.MainPage.DisplayAlert("Oznámení", "Nìjaký z uživatelských údajù nebyl vyplnìn!", "Ok");
            return;
        }

        string oldPassword = lastPasswordInput.Text;
        string newPassword = passwordInput.Text;
        string newPasswordRepeat = passwordRepeatInput.Text;

        if (!BCrypt.Verify(oldPassword, this.user.password))
        {
            App.Current.MainPage.DisplayAlert("Oznámení", "Pùvodní hesla se neshodují", "Ok");
            return;
        }

        if (newPassword != newPasswordRepeat)
        {
            App.Current.MainPage.DisplayAlert("Oznámení", "Nová hesla se neshodují", "Ok");
            return;
        }

        UpdatePasswordButton.IsVisible = false;
        LoadingIndicator2.IsVisible = true;

        string newPasswordHash = BCrypt.HashPassword(newPassword);
        User user = new User(this.user.id, newPasswordHash);

        (bool success, string message) = await _userService.UpdateUserPassword(user);

        if (success == false)
        {
            App.Current.MainPage.DisplayAlert("Chyba", message, "Ok");
            UpdateUserButton.IsVisible = true;
            LoadingIndicator.IsVisible = false;
            return;
        }

        if (success == true)
        {
            App.Current.MainPage.DisplayAlert("Oznámení", message, "Ok");
            UpdateUserButton.IsVisible = true;
            LoadingIndicator.IsVisible = false;
            this.user.password = newPasswordHash;
            await Navigation.PopAsync();
            return;
        }

        UpdateUserButton.IsVisible = true;
        LoadingIndicator.IsVisible = false;
    }

    private void SwitchOnChange(object sender, ToggledEventArgs e)
    {
        if (switchView.IsToggled)
        {
            userInfo.IsVisible = false;
            userPassword.IsVisible = true;
            labelTitle.Text = "Zmìna hesla";
            return;
        }
        userInfo.IsVisible = true;
        userPassword.IsVisible = false;
        labelTitle.Text = "Uživatelské údaje";
    }
}