namespace AutoServis.Views.All.Pages.CarDetail;

using AutoServis.Components.Templates;
using AutoServis.Model;
using AutoServis.Model.JSON;
using AutoServis.Repository;
using AutoServis.Services;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Text.Json;

public partial class AllCarDetailTabbedPage : TabbedPage
{
    public ObservableCollection<CarRepair> CarRepairs { get; set; } = new ObservableCollection<CarRepair>();


    private Car car;
	private CarInfo carInfo = null;
    private RepairsRepository repairsRepository = new RepairsRepository();
    private RepairService repairService;

    public AllCarDetailTabbedPage(Car car)
    {
        InitializeComponent();
        RepairCarpage.BindingContext = this;

        repairService = new RepairService();

        var repairPage = new AllCarDetailFormRepair(car.id, repairsRepository)
        {
            Title = "Fomuláø", // Nastavíme titulek tabulky
            IconImageSource = "form.png"
        };
        this.Children.Add(repairPage);

        this.car = car;
        ShowCarInfo(car);
        
        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        await LoadCarRepairs(car, repairService);        
    }

    private async Task LoadCarRepairs(Car car, RepairService repairService)
    {
        List<Repair>? repairs = null;
        repairs = await repairService.GetRepairs(car.id);
        if (repairs != null || !repairs.Any())
        {
            repairsRepository.NewList(repairs);
            repairsRepository.ClearCarRepairTemplateList();
            ShowCarRepair(repairsRepository.Repairs);
        }
    }

    public void ShowCarInfo(Car car)
	{
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

    private void ShowCarRepair(List<Repair> repairs)
	{
        CarRepairs.Clear();
        foreach (var repair in repairs)
        {
            CarRepairs.Add(CreateNewRepairForView(repair));
        }
    }

    private CarRepair CreateNewRepairForView(Repair repair)
    {
        CarRepair carRepair = new CarRepair()
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
            RepairsRepository = this.repairsRepository,
        };
        repairsRepository.AddCarRepairTemplate(carRepair);
        return carRepair;
    }

    public void DeleteCarRepair(int repairId)
    {
        var repairToRemove = CarRepairs.FirstOrDefault(r => r.RepairId == repairId);
        if (repairToRemove != null)
        {
            CarRepairs.Remove(repairToRemove);
        }
    }

    private async void OnCurrentPageChange(object sender, EventArgs e)
    {

#if WINDOWS
			if (this.CurrentPage == AboutCarPage)
			{
				if (this.car != null) ShowCarInfo(this.car);
				return;
			}
#endif
        if (this.CurrentPage == RepairCarpage && repairsRepository.addNewRepair)
        {
            repairsRepository.addNewRepair = false;
            InitializeAsync();
        }
    }
}