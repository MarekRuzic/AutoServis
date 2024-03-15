namespace AutoServis.Components.Forms;
using AutoServis.Model;
using Microsoft.Maui.ApplicationModel.Communication;
using System.Net.Http.Json;

public partial class UserInfoForm : ContentView
{
    private User user;
	public UserInfoForm()
	{
		InitializeComponent();         
	}

    public int id { get; set; }
    public string password { get; set; }

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

        string firstname = FirstnameInput.Text.Trim();
        string lastname = lastnameInput.Text.Trim();
        string email = emailInput.Text.Trim();

        User user = new User(id, firstname, lastname, email);

        API api = new API();
        if (api.checkConnectivity())
        {
            string[] message = api.GetConnectionMessage();
            App.Current.MainPage.DisplayAlert(message[0], message[1], message[2]);
            return;
        }

        HttpResponseMessage response = await api.client.PutAsJsonAsync("user/updateuser", user);
        if (response.IsSuccessStatusCode)
        {
            await App.Current.MainPage.DisplayAlert("Oznámení", "Data o Vás byla úspìšnì zmìnìna", "OK");
        }
        else
        {
            await App.Current.MainPage.DisplayAlert("Chyba", "Nastala neoèkávaná chyba. Zkus se to znovu", "Ok");
        }
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

        if (this.password != oldPassword)
        {
            App.Current.MainPage.DisplayAlert("Oznámení", "Pùvodní hesla se neshodují", "Ok");
            return;
        }

        if (newPassword != newPasswordRepeat)
        {
            App.Current.MainPage.DisplayAlert("Oznámení", "Nová hesla se neshodují", "Ok");
            return;
        }

        User user = new User(id, newPassword);

        API api = new API();
        if (api.checkConnectivity())
        {
            string[] message = api.GetConnectionMessage();
            App.Current.MainPage.DisplayAlert(message[0], message[1], message[2]);
            return;
        }

        HttpResponseMessage response = await api.client.PutAsJsonAsync("user/updatepassworduser", user);
        if (response.IsSuccessStatusCode)
        {
            await App.Current.MainPage.DisplayAlert("Oznámení", "Heslo bylo úspìšnì zmìnìno", "OK");
        }
        else
        {
            await App.Current.MainPage.DisplayAlert("Chyba", "Nastala neoèkávaná chyba. Zkus se to znovu", "Ok");
        }
    }

    private void SwitchOnChange(object sender, ToggledEventArgs e)
    {
        if (switchView.IsToggled)
        {
            userInfo.IsVisible = false;
            userPassword.IsVisible = true;
            labelTitle.Text = "Zmìna hesla";
            labelSwitch.Text = "Zmìnit uživateslké údaje";
            return;
        }
        userInfo.IsVisible = true;
        userPassword.IsVisible = false;
        labelTitle.Text = "Uživatelské údaje";
        labelSwitch.Text = "Zmìnit heslo";
    }
}