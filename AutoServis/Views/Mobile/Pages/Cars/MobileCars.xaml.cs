namespace AutoServis.Views.Mobile.Pages.Cars;
using AutoServis.Views.Desktop.Pages.Login;
using AutoServis.Views.Mobile.Pages.Login;
using AutoServis.Components.Templates;
using AutoServis.Model;
using AutoServis.Model.API;
using AutoServis.Model.JSON;
using System.Text.Json;

public partial class MobileCars : ContentPage
{
	User user;
	public MobileCars(Object userO)
	{
		InitializeComponent();
        user = (User)userO;
        showUserCars();
	}

    private async void showUserCars()
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
            List<Car> cars = JsonSerializer.Deserialize<List<Car>>(getResponseString, options);
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
    }

    private async void addNewCarClick(object sender, EventArgs e)
    {
		await Navigation.PushAsync(new MobileNewCar(user.id));
    }

    private void OnSwipe(object sender, SwipedEventArgs e)
    {
        DisplayAlert("Ahoj", "Toto bylo swipe :D", "ok");
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