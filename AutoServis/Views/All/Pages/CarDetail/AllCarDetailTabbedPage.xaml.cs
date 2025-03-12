namespace AutoServis.Views.All.Pages.CarDetail;

using AutoServis.Components.Templates;
using AutoServis.Model;
using AutoServis.Model.JSON;
using AutoServis.Services;
using System.Text.Json;

public partial class AllCarDetailTabbedPage : TabbedPage
{
	private Car car;
	private CarInfo carInfo = null;
    public AllCarDetailTabbedPage(Car car)
    {
        InitializeComponent();
		this.car = car;
        ShowCarInfo(car);
        AllCarDetailFormRepair.CarId = car.id;
        //Task.Run(() => neco());
        List<Repair> repairs = null;
        RepairService repairService = new RepairService();
        Task.Run(async () =>
        {
            repairs = await repairService.GetRepairs(car.id);
            if (repairs != null || !repairs.Any())
            {
                ShowCarRepair(repairs);
            }
        });
            
    }

    private async void neco()
    {
        for(int i = 0; i < 3; i++)
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await DisplayAlert("Ahoj", "ahoj", "Ok");
            });
            await Task.Delay(5000);
        }
    }

    public void ShowCarInfo(Car car)
	{
		//Car car = LoadCarFromDatabase(carId);
		verticalViewCarInfo.Children.Clear();
        string carImage = "fuel_icon.png";
        if (car.fuel == "Elektro" || car.fuel == "Hybrid") carImage = "ecofuel_icon.png";
		this.carInfo = new CarInfo
		{
			CarName = car.nickname == "" ? $"{car.brand} {car.model}" : $"{car.brand} {car.model}\n({car.nickname})",
			CarManufacture = car.manufacture.ToShortDateString(),
			CarSPZ = car.spz,
			CarMileage = car.mileage.ToString() + " Km",
			CarFuel = car.fuel,
			CarFuelImage = carImage,
			CarGearbox = car.transmition,
			CarDisplacement = car.displacement == "" ? car.power + " kw" : car.displacement + " cm3\n" + car.power + " kw",
			CarVIN = car.vin,
			CarAirCondition = car.aircondition ? "Ano" : "Ne",
			CarSeatDoor = car.seats.ToString() + " / " + car.doors.ToString(),
			CarBody = car.body,
			CarDrive = car.drive4x4 ? "Ano" : "Ne",
			CarColor = car.color == "" ? " - " : car.color,
			CarEngine = car.name_engine,
			CarCode = car.code == "" ? " - " : car.code,
			CarDisplacement2 = car.displacement == "" ? " - " : car.displacement + " cm3",
			CarPower = car.power + " kw",
			CarTorque = car.torque == "" ? " - " : car.torque + " nm",
			CarOil = car.oil_capacity == "" ? " - " : car.oil_capacity + " l",
			CarNickname = car.nickname == "" ? " - " : car.nickname,
        };
        verticalViewCarInfo.Children.Add(carInfo);
	}

    public async void LoadRepairsFromDatabase()
    {
        API api = new API();

        if (api.checkConnectivity())
        {
            await DisplayAlert("Oznámení", "Není pøipojení k internetu. Proto nemohli být naèteny data", "Zavøít");
            return;
        }
        List<Repair> repairs = new List<Repair>();
        try
        {
            HttpResponseMessage responseMessage = await api.client.GetAsync($"repair/list?id={car.id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                string getResponseString = await responseMessage.Content.ReadAsStringAsync();
                repairs = JsonSerializer.Deserialize<List<Repair>>(getResponseString);

                if (!repairs.Any())
                {
                    Label label = new Label { Text = "Nebyly nalezeny žádné opravy.", HorizontalTextAlignment = TextAlignment.Center, FontSize = 24 };
                    verticalViewCarRepair.Children.Clear();
                    verticalViewCarRepair.Children.Add(label);
                    return;
                }

                ShowCarRepair(repairs);
            }
            else
            {
                await DisplayAlert("Oznámení", "Pøi naèítání dat z databáze došlo k chybì.", "Zavøít");
            }
        }
        catch (HttpRequestException ex)
        {
            await App.Current.MainPage.DisplayAlert("Chyba", "Chyba se spojením.", "Ok");
        }
        catch (Exception ex)
        {
            await App.Current.MainPage.DisplayAlert("Chyba", "Nenámá chyba nastala.", "Ok");
        }
    }

    private async void ShowCarRepair(List<Repair> repairs)
	{
        await Task.Delay(200);

        verticalViewCarRepair.Children.Clear();
        foreach (Repair repair in repairs)
        {
            verticalViewCarRepair.Children.Add(CreateNewRepairForView(repair));
        }
    }

    private CarRepair CreateNewRepairForView(Repair repair)
    {
        string carImage = "gas_car.png";
        if (car.fuel == "Elektro" || car.fuel == "Hybrid") carImage = "electro_car.png";
        CarRepair carRepair = new CarRepair
        {
            Margin = 3,
            MaximumWidthRequest = 1000,
            RepairId = repair.id,
            RepairName = repair.name,
            RepairDate = repair.date.ToShortDateString(),
            RepairMileage = repair.mileage + " Km",
            RepairPrice = repair.price + " Kè",
            decription = repair.description,
            part_name = repair.part_name,
            url = repair.url,
            car_id = car.id,
            AllCarDetailTabbedPage = this,
        };
        return carRepair;
    }

    private void OnCurrentPageChange(object sender, EventArgs e)
    {
        // TODO doøešit zobrazování tabulky pro Windows
		#if WINDOWS
			if (this.CurrentPage == AboutCarPage)
			{
				if (this.car != null) ShowCarInfo(this.car);
				return;
			}
		#endif
        /*
		if (this.CurrentPage == RepairCarpage)
		{
            //LoadRepairsFromDatabase();
        }*/
    }
}