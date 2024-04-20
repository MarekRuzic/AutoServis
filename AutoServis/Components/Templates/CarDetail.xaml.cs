namespace AutoServis.Components.Templates;
using AutoServis.Model;
using AutoServis.Views.Mobile.Pages.Cars;
using AutoServis.Views.All.Pages.CarDetail;
using Microsoft.VisualBasic.FileIO;

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

    private async void MoreCarInfoClicked(object sender, EventArgs e)
    {
        // Získání id daného
        //await Navigation.PushAsync(new AllCarDetailTabbedPage(CarId));


        MobileCars parentPage = FindParentMobileCars(this);

        await Navigation.PushAsync(new AllCarDetailTabbedPage(parentPage.LoadCar(CarId)));
    }

    private MobileCars FindParentMobileCars(Element element)
    {
        // Rekurzivnì hledá rodièovskou stránku MobileCars
        if (element.Parent is MobileCars mobileCars)
        {
            return mobileCars;
        }

        if (element.Parent != null)
        {
            // Pokraèuj v hledání v rodièovském prvku
            return FindParentMobileCars(element.Parent);
        }

        // Nenalezena žádná rodièovská stránka MobileCars
        return null;
    }

    private async void OnDeleteSwipeItemInvoked(object sender, EventArgs e)
    {
        bool answer = await App.Current.MainPage.DisplayAlert("Smazat", $"Opravdu si pøejete smazat {CarName}?", "Ano", "Ne");
        if (!answer)
        {
            return;
        }

        API api = new API();

        if (api.checkConnectivity())
        {
            await App.Current.MainPage.DisplayAlert("Varování", "Nemáte pøipojení k internetu, je potøeba pøipojení", "Ok");
            return;
        }

        try
        {
            HttpResponseMessage response = await api.client.DeleteAsync($"car/delete?id={CarId}");
            if (response.IsSuccessStatusCode)
            {
                await App.Current.MainPage.DisplayAlert("Oznámení", $"Auto bylo uspìšnì smazáno.", "Ok");

                // Získání reference na rodièovskou stránku
                MobileCars parentPage = FindParentMobileCars(this);

                // Kontrola nalezení rodièovské stránky
                if (parentPage == null)
                {
                    App.Current.MainPage.DisplayAlert("Oznámení", $"V souèasné chvíli nelze smazat požadovéné vozidlo.", "Ok");
                    return;
                }
                // Zavolání metody pro smazání auta z List<Car>
                parentPage?.DeleteCar(CarId);
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Oznámení", $"Došlo k chybì pøi mazání vozidla.", "Ok");
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

    private async void OnEditSwipeItemInvoked(object sender, EventArgs e)
    {
        MobileCars parentPage = FindParentMobileCars(this);
        // Kontrola nalezení rodièovské stránky
        if (parentPage == null)
        {
            App.Current.MainPage.DisplayAlert("Oznámení", $"V souèasné chvíli nelze smazat požadovéné vozidlo.", "Ok");
            return;
        }
        parentPage?.EditCar(CarId);        
    }
}