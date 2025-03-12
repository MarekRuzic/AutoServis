namespace AutoServis.Views.Mobile.Pages.Cars;
using AutoServis.Views.Desktop.Pages.Login;
using AutoServis.Views.Desktop.Pages.UserInfo;
using AutoServis.Views.Mobile.Pages.Login;
using AutoServis.Views.Mobile.Pages.UserInfo;
using AutoServis.Views.All.Pages.CarForm;
using AutoServis.Components.Templates;
using AutoServis.Model;
using AutoServis.Model.JSON;
using System.Text.Json;
using Microsoft.Maui.Controls;
using AutoServis.Views.All.Pages.AboutPage;
using AutoServis.Repository;
using AutoServis.Services;
using System.Collections.Generic;

public partial class MobileCars : ContentPage
{
    private API api;
	private User user;
    private ICarService carService;
    private ICarsRepository carsRepository;

    public MobileCars(User user)
    {
        InitializeComponent();
        this.user = user;
        this.carService = new CarService();
        this.api = new API();
        InitializeAsyncFunction();
    }

    private async void InitializeAsyncFunction()
    {
        carsRepository = new CarsRepository(await carService.LoadUserCars(user.id, api));
        ShowUserCars(carsRepository.GetCars());
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (carsRepository != null && carsRepository.dataChange)
        {
            carsRepository.dataChange = false;
            ShowUserCars(carsRepository.GetCars());
        }

    }

    public void TriggerOnAppearing() => OnAppearing();

    public void SaveNewCredits(string firstname, string lastname, string email)
    {
        user.firstname = firstname; 
        user.lastname = lastname; 
        user.email = email;
    }

    public void SaveNewPassword(string password)
    {
        user.password = password;
    }    

    // Zobrazení jednotlivých vozidel uživatele
    public void ShowUserCars(List<Car> cars)
    {
        if (cars == null || !cars.Any())
        {
            LoadingIndicator.IsVisible = false;
            return;
        }
        verticalLayout.Children.Clear();
        int margin = 0;
        if (DevicePlatform.WinUI == DeviceInfo.Platform) margin = 10;
        foreach (Car car in cars)
        {
            string carImage = "gas_car.png";
            if (car.fuel == "Elektro" || car.fuel == "Hybrid") carImage = "electro_car.png";
            var carView = new CarDetail(carsRepository, carService)
            {
                Padding = margin,
                MaximumWidthRequest = 1000,
                CarId = car.id,
                CarName = $"{car.brand} {car.model}",
                CarManufacture = car.manufacture.ToShortDateString(),
                CarSPZ = car.spz,
                CarColor = car.color,
                CarMileage = car.mileage.ToString(),
                CarFuel = car.fuel,
                CarEngineName = car.name_engine,
                CarPower = car.power,
                CarGearbox = car.transmition,
                CarImage = carImage,
            };
            verticalLayout.Children.Add(carView);
        }
        verticalLayout.Children.Add(addNewCarButton);
    }

    private async void addNewCarClick(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AllCarForm(user.id, carService, carsRepository));
    }

    private async void LogOut(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Odhlásit se", "Opravdu se chcete odhlásit?", "Ano", "Ne");
        if (answer)
        {
            #if ANDROID || IOS
                App.Current.MainPage = new NavigationPage(new MobileLogin());
            #else
                App.Current.MainPage = new NavigationPage(new DesktopLogin());
            #endif
        }
    }

    private async void userInfoClick(object sender, EventArgs e)
    {
#if ANDROID || IOS
        await Navigation.PushAsync(new MobileUserInfo(user));
#else
        await Navigation.PushAsync(new DesktopUserInfo(user));
#endif
    }

    private async void AboutAppClick(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AllAboutPage());
    }

    private async void refreshContentClick(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Aktualizovat seznam", "Opravdu si pøejete aktualizovat seznam vozidel?\n\n" +
            "Tato akce mùže chvíli trvat.\n\n" +
            "Doporuèuje se, pokud jste pøihlášeni souèasnì na dvou zaøízeních a na jednom z nich provedete zmìny.", "Ano", "Ne");
        if (answer)
        {
            verticalLayout.Children.Clear();
            verticalLayout.Children.Add(LoadingIndicator);
            InitializeAsyncFunction();
        }
    }
}