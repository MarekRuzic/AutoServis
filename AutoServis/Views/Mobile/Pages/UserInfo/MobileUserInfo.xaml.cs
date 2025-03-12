namespace AutoServis.Views.Mobile.Pages.UserInfo;
using AutoServis.Model;
using AutoServis.Components.Forms;
using AutoServis.Views.Mobile.Pages.Cars;

public partial class MobileUserInfo : ContentPage
{
	public MobileUserInfo(User user)
	{
		InitializeComponent();
        userInfo.user = user;
        userInfo.SetEntryValues(user);        
    }   
}