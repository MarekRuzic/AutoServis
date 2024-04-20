namespace AutoServis.Components.Forms;
using AutoServis.Model;
using Microsoft.Maui.ApplicationModel.Communication;
using System.Net.Http.Json;
using BCrypt.Net;

public partial class UserInfoForm : ContentView
{
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
            App.Current.MainPage.DisplayAlert("Ozn�men�", "N�jak� z u�ivatelsk�ch �daj� nebyl vypln�n!", "Ok");
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

        UpdateUserButton.IsVisible = false;
        LoadingIndicator.IsVisible = true;

        try
        {
            HttpResponseMessage response = await api.client.PutAsJsonAsync("user/updateuser", user);
            if (response.IsSuccessStatusCode)
            {
                App.Current.MainPage.DisplayAlert("Ozn�men�", "Data o V�s byla �sp�n� zm�n�na", "OK");
            }
            else
            {
                App.Current.MainPage.DisplayAlert("Chyba", "Nastala neo�k�van� chyba. Zkus se to znovu", "Ok");
            }
        }
        catch (HttpRequestException ex)
        {
            await App.Current.MainPage.DisplayAlert("Chyba", "Chyba se spojen�m.", "Ok");
        }
        catch (Exception ex)
        {
            await App.Current.MainPage.DisplayAlert("Chyba", "Nen�m� chyba nastala.", "Ok");
        }
        finally
        {
            UpdateUserButton.IsVisible = true;
            LoadingIndicator.IsVisible = false;
        }
    }

    private async void UpdatePasswordClick(object sender, EventArgs e)
    {
        if (IsInputEmpty(lastPasswordInput.Text) &&
            IsInputEmpty(passwordInput.Text) &&
            IsInputEmpty(passwordRepeatInput.Text))
        {
            App.Current.MainPage.DisplayAlert("Ozn�men�", "N�jak� z u�ivatelsk�ch �daj� nebyl vypln�n!", "Ok");
            return;
        }

        string oldPassword = lastPasswordInput.Text;
        string newPassword = passwordInput.Text;
        string newPasswordRepeat = passwordRepeatInput.Text;

        if (!BCrypt.Verify(oldPassword, this.password))
        {
            App.Current.MainPage.DisplayAlert("Ozn�men�", "P�vodn� hesla se neshoduj�", "Ok");
            return;
        }

        if (newPassword != newPasswordRepeat)
        {
            App.Current.MainPage.DisplayAlert("Ozn�men�", "Nov� hesla se neshoduj�", "Ok");
            return;
        }        

        string newPasswordHash = BCrypt.HashPassword(newPassword);

        User user = new User(id, newPasswordHash);

        API api = new API();
        if (api.checkConnectivity())
        {
            string[] message = api.GetConnectionMessage();
            App.Current.MainPage.DisplayAlert(message[0], message[1], message[2]);
            return;
        }

        UpdatePasswordButton.IsVisible = false;
        LoadingIndicator2.IsVisible = true;

        try
        {

            HttpResponseMessage response = await api.client.PutAsJsonAsync("user/updatepassworduser", user);
            if (response.IsSuccessStatusCode)
            {
                App.Current.MainPage.DisplayAlert("Ozn�men�", "Heslo bylo �sp�n� zm�n�no", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                App.Current.MainPage.DisplayAlert("Chyba", "Nastala neo�k�van� chyba. Zkus se to znovu", "Ok");
            }
        }
        catch (HttpRequestException ex)
        {
            await App.Current.MainPage.DisplayAlert("Chyba", "Chyba se spojen�m.", "Ok");
        }
        catch (Exception ex)
        {
            await App.Current.MainPage.DisplayAlert("Chyba", "Nen�m� chyba nastala.", "Ok");
        }
        finally
        {
            UpdatePasswordButton.IsVisible = true;
            LoadingIndicator2.IsVisible = false;
        }
    }

    private void SwitchOnChange(object sender, ToggledEventArgs e)
    {
        if (switchView.IsToggled)
        {
            userInfo.IsVisible = false;
            userPassword.IsVisible = true;
            labelTitle.Text = "Zm�na hesla";
            return;
        }
        userInfo.IsVisible = true;
        userPassword.IsVisible = false;
        labelTitle.Text = "U�ivatelsk� �daje";
    }
}