namespace AutoServis.Views.All.Pages.CarDetail;

using AutoServis.Components.Templates;
using AutoServis.Model;

public partial class AllCarDetailTabbedPage : TabbedPage
{
	private Car car;
	private CarInfo carInfo = null;
    public AllCarDetailTabbedPage(Car car)
    {
        InitializeComponent();
		this.car = car;
        ShowCarInfo(car);
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

    private void OnCurrentPageChange(object sender, EventArgs e)
    {
#if WINDOWS
        if (this.CurrentPage == PrvniStrana)
		{
			if (this.car != null)
			{
				ShowCarInfo(this.car);
			}
		}
#endif
    }
}