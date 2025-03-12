namespace AutoServis.Views.Desktop.Pages.UserInfo;
using AutoServis.Views.Mobile.Pages.Cars;
using AutoServis.Model;
using AutoServis.Components.Forms;


public partial class DesktopUserInfo : ContentPage
{
	public DesktopUserInfo(User user)
	{        
        InitializeComponent();
        userInfo.user = user;
		userInfo.SetEntryValues(user);    
    }
}