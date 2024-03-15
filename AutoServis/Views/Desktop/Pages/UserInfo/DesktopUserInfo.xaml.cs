namespace AutoServis.Views.Desktop.Pages.UserInfo;
using AutoServis.Model;
using AutoServis.Components.Forms;


public partial class DesktopUserInfo : ContentPage
{
	public DesktopUserInfo(User user)
	{
		InitializeComponent();
		SetValuesInForm(user);
    }

	private void SetValuesInForm(User user)
	{
		userInfo.id = user.id;
		userInfo.password = user.password;

		Entry firstnameEntry = (Entry)userInfo.FindByName("FirstnameInput");
		Entry lastnameEntry = (Entry)userInfo.FindByName("lastnameInput");
		Entry emailEntry = (Entry)userInfo.FindByName("emailInput");

		firstnameEntry.Text = user.firstname;
		lastnameEntry.Text = user.lastname;
		emailEntry.Text = user.email;
	}
}