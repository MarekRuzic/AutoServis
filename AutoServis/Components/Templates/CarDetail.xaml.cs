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
        // Z�sk�n� id dan�ho
        //await Navigation.PushAsync(new AllCarDetailTabbedPage(CarId));


        MobileCars parentPage = FindParentMobileCars(this);

        await Navigation.PushAsync(new AllCarDetailTabbedPage(parentPage.LoadCar(CarId)));
    }

    private MobileCars FindParentMobileCars(Element element)
    {
        // Rekurzivn� hled� rodi�ovskou str�nku MobileCars
        if (element.Parent is MobileCars mobileCars)
        {
            return mobileCars;
        }

        if (element.Parent != null)
        {
            // Pokra�uj v hled�n� v rodi�ovsk�m prvku
            return FindParentMobileCars(element.Parent);
        }

        // Nenalezena ��dn� rodi�ovsk� str�nka MobileCars
        return null;
    }

    private async void OnDeleteSwipeItemInvoked(object sender, EventArgs e)
    {
        bool answer = await App.Current.MainPage.DisplayAlert("Smazat", $"Opravdu si p�ejete smazat {CarName}?", "Ano", "Ne");
        if (!answer)
        {
            return;
        }

        API api = new API();

        if (api.checkConnectivity())
        {
            await App.Current.MainPage.DisplayAlert("Varov�n�", "Nem�te p�ipojen� k internetu, je pot�eba p�ipojen�", "Ok");
            return;
        }

        try
        {
            HttpResponseMessage response = await api.client.DeleteAsync($"car/delete?id={CarId}");
            if (response.IsSuccessStatusCode)
            {
                await App.Current.MainPage.DisplayAlert("Ozn�men�", $"Auto bylo usp�n� smaz�no.", "Ok");

                // Z�sk�n� reference na rodi�ovskou str�nku
                MobileCars parentPage = FindParentMobileCars(this);

                // Kontrola nalezen� rodi�ovsk� str�nky
                if (parentPage == null)
                {
                    App.Current.MainPage.DisplayAlert("Ozn�men�", $"V sou�asn� chv�li nelze smazat po�adov�n� vozidlo.", "Ok");
                    return;
                }
                // Zavol�n� metody pro smaz�n� auta z List<Car>
                parentPage?.DeleteCar(CarId);
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Ozn�men�", $"Do�lo k chyb� p�i maz�n� vozidla.", "Ok");
            }
        }
        catch (HttpRequestException ex)
        {
            await App.Current.MainPage.DisplayAlert("Chyba", "Chyba se spojen�m.", "Ok");
        }
        catch (Exception ex)
        {
            await App.Current.MainPage.DisplayAlert("Chyba", "Nen�m� chyba nastala.", "Ok");
        }
    }

    private async void OnEditSwipeItemInvoked(object sender, EventArgs e)
    {
        MobileCars parentPage = FindParentMobileCars(this);
        // Kontrola nalezen� rodi�ovsk� str�nky
        if (parentPage == null)
        {
            App.Current.MainPage.DisplayAlert("Ozn�men�", $"V sou�asn� chv�li nelze smazat po�adov�n� vozidlo.", "Ok");
            return;
        }
        parentPage?.EditCar(CarId);        
    }
}