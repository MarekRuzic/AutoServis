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

    // Na�te data z datab�ze pomoc� API
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
            await DisplayAlert("Ozn�men�", "P�i na��t�n� dat z datab�ze do�lo k chyb�.", "Zav��t");
        }
    }

    // Zobrazen� jednotliv�ch vozidel u�ivatele
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
        //Odstran� v�echny prvky s dan�m ID za pomoci lambda v�razu
        cars.RemoveAll(c => c.id == carId);
        // Znovu zobrazen� List<Car>
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
        bool answer = await DisplayAlert("Odl�sit se", "", "Ano", "Ne");
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