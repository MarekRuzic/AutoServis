namespace AutoServis.Components.Templates;

public partial class CarInfo : ContentView
{
	public CarInfo()
	{
		InitializeComponent();
	}

    public static readonly BindableProperty CarNameProperty = BindableProperty.Create(nameof(CarName), typeof(string), typeof(CarInfo), string.Empty);
    public static readonly BindableProperty CarManufactureProperty = BindableProperty.Create(nameof(CarManufacture), typeof(string), typeof(CarInfo), string.Empty);
    public static readonly BindableProperty CarSPZProperty = BindableProperty.Create(nameof(CarSPZ), typeof(string), typeof(CarInfo), string.Empty);
    public static readonly BindableProperty CarMileageProperty = BindableProperty.Create(nameof(CarMileage), typeof(string), typeof(CarInfo), string.Empty);
    public static readonly BindableProperty CarFuelProperty = BindableProperty.Create(nameof(CarFuel), typeof(string), typeof(CarInfo), string.Empty);
    public static readonly BindableProperty CarFuelImageProperty = BindableProperty.Create(nameof(CarFuelImage), typeof(string), typeof(CarInfo), string.Empty);
    public static readonly BindableProperty CarPowerProperty = BindableProperty.Create(nameof(CarPower), typeof(string), typeof(CarInfo), string.Empty);
    public static readonly BindableProperty CarGearboxProperty = BindableProperty.Create(nameof(CarGearbox), typeof(string), typeof(CarInfo), string.Empty);
    public static readonly BindableProperty CarDisplacementProperty = BindableProperty.Create(nameof(CarDisplacement), typeof(string), typeof(CarInfo), string.Empty);
    public static readonly BindableProperty CarVINProperty = BindableProperty.Create(nameof(CarVIN), typeof(string), typeof(CarInfo), string.Empty);
    public static readonly BindableProperty CarAirConditionProperty = BindableProperty.Create(nameof(CarAirCondition), typeof(string), typeof(CarInfo), string.Empty);
    public static readonly BindableProperty CarSeatDoorProperty = BindableProperty.Create(nameof(CarSeatDoor), typeof(string), typeof(CarInfo), string.Empty);
    public static readonly BindableProperty CarBodyProperty = BindableProperty.Create(nameof(CarBody), typeof(string), typeof(CarInfo), string.Empty);
    public static readonly BindableProperty CarDriveProperty = BindableProperty.Create(nameof(CarDrive), typeof(string), typeof(CarInfo), string.Empty);
    public static readonly BindableProperty CarColorProperty = BindableProperty.Create(nameof(CarColor), typeof(string), typeof(CarInfo), string.Empty);
    public static readonly BindableProperty CarEngineProperty = BindableProperty.Create(nameof(CarEngine), typeof(string), typeof(CarInfo), string.Empty);
    public static readonly BindableProperty CarCodeProperty = BindableProperty.Create(nameof(CarCode), typeof(string), typeof(CarInfo), string.Empty);
    public static readonly BindableProperty CarDisplacement2Property = BindableProperty.Create(nameof(CarDisplacement2), typeof(string), typeof(CarInfo), string.Empty);
    public static readonly BindableProperty CarTorqueProperty = BindableProperty.Create(nameof(CarTorque), typeof(string), typeof(CarInfo), string.Empty);
    public static readonly BindableProperty CarOilProperty = BindableProperty.Create(nameof(CarOil), typeof(string), typeof(CarInfo), string.Empty);
    public static readonly BindableProperty CarNicknameProperty = BindableProperty.Create(nameof(CarNickname), typeof(string), typeof(CarInfo), string.Empty);


    public string CarName
    {
        get => (string)GetValue(CarNameProperty);
        set => SetValue(CarNameProperty, value);
    }

    public string CarManufacture
    {
        get => (string)GetValue(CarManufactureProperty);
        set => SetValue(CarManufactureProperty, value);
    }

    public string CarSPZ
    {
        get => (string)GetValue(CarSPZProperty);
        set => SetValue(CarSPZProperty, value);
    }

    public string CarMileage
    {
        get => (string)GetValue(CarMileageProperty);
        set => SetValue(CarMileageProperty, value);
    }

    public string CarFuel
    {
        get => (string)GetValue(CarFuelProperty);
        set => SetValue(CarFuelProperty, value);
    }

    public string CarFuelImage
    {
        get => (string)GetValue(CarFuelImageProperty);
        set => SetValue(CarFuelImageProperty, value);
    }

    public string CarPower
    {
        get => (string)GetValue(CarPowerProperty);
        set => SetValue(CarPowerProperty, value);
    }

    public string CarGearbox
    {
        get => (string)GetValue(CarGearboxProperty);
        set => SetValue(CarGearboxProperty, value);
    }

    public string CarDisplacement
    {
        get => (string)GetValue(CarDisplacementProperty);
        set => SetValue(CarDisplacementProperty, value);
    }

    public string CarDisplacement2
    {
        get => (string)GetValue(CarDisplacement2Property);
        set => SetValue(CarDisplacement2Property, value);
    }

    public string CarVIN
    {
        get => (string)GetValue(CarVINProperty);
        set => SetValue(CarVINProperty, value);
    }

    public string CarAirCondition
    {
        get => (string)GetValue(CarAirConditionProperty);
        set => SetValue(CarAirConditionProperty, value);
    }

    public string CarSeatDoor
    {
        get => (string)GetValue(CarSeatDoorProperty);
        set => SetValue(CarSeatDoorProperty, value);
    }

    public string CarBody
    {
        get => (string)GetValue(CarBodyProperty);
        set => SetValue(CarBodyProperty, value);
    }

    public string CarDrive
    {
        get => (string)GetValue(CarDriveProperty);
        set => SetValue(CarDriveProperty, value);
    }

    public string CarColor
    {
        get => (string)GetValue(CarColorProperty);
        set => SetValue(CarColorProperty, value);
    }

    public string CarEngine
    {
        get => (string)GetValue(CarEngineProperty);
        set => SetValue(CarEngineProperty, value);
    }

    public string CarCode
    {
        get => (string)GetValue(CarCodeProperty);
        set => SetValue(CarCodeProperty, value);
    }

    public string CarTorque
    {
        get => (string)GetValue(CarTorqueProperty);
        set => SetValue(CarTorqueProperty, value);
    }

    public string CarOil
    {
        get => (string)GetValue(CarOilProperty);
        set => SetValue(CarOilProperty, value);
    }

    public string CarNickname
    {
        get => (string)GetValue(CarNicknameProperty);
        set => SetValue(CarNicknameProperty, value);
    }
}