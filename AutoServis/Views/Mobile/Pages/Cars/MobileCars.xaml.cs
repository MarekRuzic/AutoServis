namespace AutoServis.Views.Mobile.Pages.Cars;
using AutoServis.Views.Desktop.Pages.Login;
using AutoServis.Views.Mobile.Pages.Login;
using AutoServis.Components.Templates;
using AutoServis.Model;
using AutoServis.Model.API;
using AutoServis.Model.JSON;
using System.Text.Json;
using Microsoft.Maui.Controls;

public partial class MobileCars : ContentPage
{
	User user;
    List<Car> cars = new List<Car>();
	public MobileCars(Object user)
	{
		InitializeComponent();
        this.user = (User)user;
        LoadUserCars();
	}

    // Naète data z databáze pomocí API
    private async void LoadUserCars()
    {
        API api = new API();

        HttpResponseMessage responseMessage = await api.client.GetAsync($"car/listcaruser?id={user.id}");
        if (responseMessage.IsSuccessStatusCode)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                Converters = { new BoolConverter() },
            };
            string getResponseString = await responseMessage.Content.ReadAsStringAsync();
            cars = JsonSerializer.Deserialize<List<Car>>(getResponseString, options);

            ShowUserCars();
        }
        else
        {
            await DisplayAlert("Oznámení", "Pøi naèítání dat z databáze došlo k chybì.", "Zavøít");
        }
    }

    // Zobrazení jednotlivých vozidel uživatele
    private void ShowUserCars()
    {
        verticalLayout.Children.Clear();
        foreach (Car car in cars)
        {
            string carImage = "gas_car.png";
            if (car.fuel == "Elektro" || car.fuel == "Hybrid") carImage = "electro_car.png";
            var carView = new CarDetail
            {
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

    public void DeleteCar(int carId)
    {
        //Odstraní všechny prvky s daným ID za pomoci lambda výrazu
        cars.RemoveAll(c => c.id == carId);
        // Znovu zobrazení List<Car>
        ShowUserCars();
    }

    public async void EditCar(int carId)
    {
        Car car = cars.FirstOrDefault(c => c.id == carId);
        MobileCars mobileCars = this;
        await Navigation.PushAsync(new MobileNewCar(car, user.id, mobileCars));
    }

    public void SaveCarToList(Car car, bool isAdd)
    {
        if (isAdd)
        {
            LoadUserCars();
        }
        else
        {
            int index = cars.FindIndex(carList => carList.id == car.id);
            if (index != -1)
            {
                cars[index] = car;
            }
        }        
        ShowUserCars();
    }

    private async void addNewCarClick(object sender, EventArgs e)
    {
        MobileCars mobileCars = this;
        await Navigation.PushAsync(new MobileNewCar(user.id, mobileCars));
    }

    private async void LogOut(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Odlásit se", "", "Ano", "Ne");
        if (answer)
        {
            #if ANDROID || IOS
                App.Current.MainPage = new NavigationPage(new MobileLogin());
            #else
                App.Current.MainPage = new NavigationPage(new DesktopLogin());
            #endif
        }
    }
}