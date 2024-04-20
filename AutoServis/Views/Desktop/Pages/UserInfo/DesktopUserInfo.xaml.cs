namespace AutoServis.Views.Desktop.Pages.UserInfo;
using AutoServis.Views.Mobile.Pages.Cars;
using AutoServis.Model;
using AutoServis.Components.Forms;


public partial class DesktopUserInfo : ContentPage
{
	public DesktopUserInfo(User user, MobileCars mobileCars)
	{
		InitializeComponent();
		SetValuesInForm(user, mobileCars);
    }

	private void SetValuesInForm(User user, MobileCars mobileCars)
	{
		userInfo.id = user.id;
		userInfo.password = user.password;
        userInfo.mobileCars = mobileCars;

        Entry firstnameEntry = (Entry)userInfo.FindByName("FirstnameInput");
		Entry lastnameEntry = (Entry)userInfo.FindByName("lastnameInput");
		Entry emailEntry = (Entry)userInfo.FindByName("emailInput");

		firstnameEntry.Text = user.firstname;
		lastnameEntry.Text = user.lastname;
		emailEntry.Text = user.email;
	}
}