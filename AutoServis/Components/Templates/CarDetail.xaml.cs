namespace AutoServis.Components.Templates;
using AutoServis.Model;

public partial class CarDetail : ContentView
{
	public CarDetail()
	{
		InitializeComponent();
	}

    public static readonly BindableProperty CarIdProperty = BindableProperty.Create(nameof(CarId), typeof(int), typeof(CarDetail), -1);
    public static readonly BindableProperty CarNameProperty = BindableProperty.Create(nameof(CarName), typeof(string), typeof(CarDetail), string.Empty);
    public static readonly BindableProperty CarManufactureProperty = BindableProperty.Create(nameof(CarManufacture), typeof(string), typeof(CarDetail), string.Empty);
    public static readonly BindableProperty CarSPZProperty = BindableProperty.Create(nameof(CarSPZ), typeof(string), typeof(CarDetail), string.Empty);
    public static readonly BindableProperty CarColorProperty = BindableProperty.Create(nameof(CarColor), typeof(string), typeof(CarDetail), string.Empty);
    public static readonly BindableProperty CarMileageProperty = BindableProperty.Create(nameof(CarMileage), typeof(string), typeof(CarDetail), string.Empty);
    public static readonly BindableProperty CarFuelProperty = BindableProperty.Create(nameof(CarFuel), typeof(string), typeof(CarDetail), string.Empty);
    public static readonly BindableProperty CarEngineNameProperty = BindableProperty.Create(nameof(CarEngineName), typeof(string), typeof(CarDetail), string.Empty);
    public static readonly BindableProperty CarPowerProperty = BindableProperty.Create(nameof(CarPower), typeof(string), typeof(CarDetail), string.Empty);
    public static readonly BindableProperty CarGearboxProperty = BindableProperty.Create(nameof(CarGearbox), typeof(string), typeof(CarDetail), string.Empty);
    public static readonly BindableProperty CarImageProperty = BindableProperty.Create(nameof(CarImage), typeof(string), typeof(CarDetail), string.Empty);

    public int CarId
    {
        get => (int)GetValue(CarIdProperty);
        set => SetValue(CarIdProperty, value);
    }

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

    public string CarColor
    {
        get => (string)GetValue(CarColorProperty);
        set => SetValue (CarColorProperty, value);
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

    public string CarEngineName
    {
        get => (string)GetValue(CarEngineNameProperty);
        set => SetValue(CarEngineNameProperty, value);
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

    public string CarImage
    {
        get => (string)GetValue(CarImageProperty);
        set => SetValue(CarImageProperty, value);
    }

    private void MoreInfoClicked(object sender, EventArgs e)
    {

        //App.Current.MainPage.DisplayAlert("Ahoj", $"Id je: {carDetail.Id}", "ok");
    }

    private void MoreCarInfoClicked(object sender, EventArgs e)
    {
        // Z�sk�n� id dan�ho
        App.Current.MainPage.DisplayAlert("Ahoj", $"Id je: {CarId}", "ok");
    }

    private async void OnDeleteSwipeItemInvoked(object sender, EventArgs e)
    {
        bool answer = await App.Current.MainPage.DisplayAlert("Smazat", $"Opravdu si p�ejete smazat {CarName}?", "Ano", "Ne");
        if (!answer)
        {
            return;
        }
        await App.Current.MainPage.DisplayAlert("Ozn�men�", $"Auto bylo usp�n� smaz�no.", "Ok");
    }
}