namespace AutoServis.Components.Templates;
using AutoServis.Model;
using AutoServis.Views.Mobile.Pages.Cars;
using AutoServis.Views.All.Pages.CarDetail;
using AutoServis.Views.All.Pages.CarForm;
using Microsoft.VisualBasic.FileIO;
using AutoServis.Services;
using AutoServis.Repository;

public partial class CarDetail : ContentView
{
    private readonly ICarsRepository carsRepository;
    private readonly ICarService carService;

    public CarDetail()
	{
		InitializeComponent();
	}

    public CarDetail(ICarsRepository carsRepository, ICarService carService)
    {
        InitializeComponent();
        this.carsRepository = carsRepository;
        this.carService = carService;
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

    private async void MoreCarInfoClicked(object sender, EventArgs e)
    {
        // Otevření nové stránky s detailem vozidla
        Car car = carsRepository.GetCar(CarId);
        if (car != null)
            await Navigation.PushAsync(new AllCarDetailTabbedPage(car));    
    }   

    private async void OnDeleteSwipeItemInvoked(object sender, EventArgs e)
    {
        bool answer = await App.Current.MainPage.DisplayAlert("Smazat", $"Opravdu si přejete smazat {CarName}?", "Ano", "Ne");
        if (!answer)
        {
            return;
        }        

        if(await carService.DeleteCar(CarId))
        {
            carsRepository.RemoveCar(CarId);
            carsRepository.dataChange = true;

            if (FindParentPage() is MobileCars parentPage)
            {
                parentPage.TriggerOnAppearing(); // Zavolá metodu na stránce MobileCars
            }
            else App.Current.MainPage.DisplayAlert("Chyba", $"Při mazání došlo k chybě! 😅\n\n" +
                $"Aktualizujte seznam vozidel pomocí načítačího tlačítka v horním menu (Aktualizovat seznam)", "Ano", "Ne");
        }
    }

    private Page FindParentPage()
    {
        Element parent = this.Parent; // Začneme od aktuálního rodiče

        while (parent != null)
        {
            if (parent is Page page)
            {
                return page; // Jakmile najdeme ContentPage, vrátíme ji
            }
            parent = parent.Parent; // Pokračujeme nahoru ve stromu
        }

        return null; // Pokud nenalezeno, vrátí null
    }

    private async void OnEditSwipeItemInvoked(object sender, EventArgs e)
    {
        Car car = carsRepository.GetCar(CarId);
        if (car != null)
            await Navigation.PushAsync(new AllCarForm(car, car.user_id, carsRepository, carService, "Editace vozidla"));
    }
}