using AutoServis.Views.Desktop.Pages.Login;
using AutoServis.Views.Mobile.Pages.Login;
namespace AutoServis
{
    public partial class App : Application
    {      
        public App()
        {
            InitializeComponent();

            //MainPage = new AppShell();            

#if ANDROID || IOS
            MainPage = new NavigationPage(new MobileLogin());
#else
            MainPage = new NavigationPage(new DesktopLogin());
#endif
        }
    }
}
