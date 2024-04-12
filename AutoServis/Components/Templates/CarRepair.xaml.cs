namespace AutoServis.Components.Templates;
using AutoServis.Model;
using AutoServis.Views.All.Pages.CarDetail;

public partial class CarRepair : ContentView
{
	public CarRepair()
	{
		InitializeComponent();
	}

    public static readonly BindableProperty RepairIdProperty = BindableProperty.Create(nameof(RepairId), typeof(int), typeof(CarRepair), -1);
    public static readonly BindableProperty RepairNameProperty = BindableProperty.Create(nameof(RepairName), typeof(string), typeof(CarRepair), string.Empty);
    public static readonly BindableProperty RepairDateProperty = BindableProperty.Create(nameof(RepairDate), typeof(string), typeof(CarRepair), string.Empty);
    public static readonly BindableProperty RepairMileageProperty = BindableProperty.Create(nameof(RepairMileage), typeof(string), typeof(CarRepair), string.Empty);
    public static readonly BindableProperty RepairPriceProperty = BindableProperty.Create(nameof(RepairPrice), typeof(string), typeof(CarRepair), string.Empty);
    public string decription { get; set; }
    public string part_name { get; set; }
    public string url { get; set; }
    public int car_id { get; set; }

    public int RepairId
    {
        get => (int)GetValue(RepairIdProperty);
        set => SetValue(RepairIdProperty, value);
    }

    public string RepairName
    {
        get => (string)GetValue(RepairNameProperty);
        set => SetValue(RepairNameProperty, value);
    }

    public string RepairDate
    {
        get => (string)GetValue(RepairDateProperty);
        set => SetValue(RepairDateProperty, value);
    }

    public string RepairMileage
    {
        get => (string)GetValue(RepairMileageProperty);
        set => SetValue(RepairMileageProperty, value);
    }

    public string RepairPrice
    {
        get => (string)GetValue(RepairPriceProperty);
        set => SetValue(RepairPriceProperty, value);
    }


    private async void OnEditSwipeItemInvoked(object sender, EventArgs e)
    {
        Repair repair = new Repair(RepairId, RepairName, Convert.ToDateTime(RepairDate), Convert.ToDouble(RepairMileage.Substring(0, RepairMileage.IndexOf(' '))),
            decription, RepairPrice.Substring(0, RepairPrice.IndexOf(' ')), part_name, url, car_id);
        await Navigation.PushAsync(new AllCarDetailFormRepair(repair));
    }

    private async void OnDeleteSwipeItemInvoked(object sender, EventArgs e)
    {
        bool answer = await App.Current.MainPage.DisplayAlert("Smazat", $"Opravdu si pøejete opravu {RepairName}?", "Ano", "Ne");
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

        HttpResponseMessage response = await api.client.DeleteAsync($"repair/delete?id={RepairId}");
        if (response.IsSuccessStatusCode)
        {
            await App.Current.MainPage.DisplayAlert("Oznámení", $"Oprava byla uspìšnì smazán.", "Ok");

            // Získání reference na rodièovskou stránku
            AllCarDetailTabbedPage parentPage = FindParentMobileCars(this);

            // Kontrola nalezení rodièovské stránky
            if (parentPage == null)
            {
                App.Current.MainPage.DisplayAlert("Oznámení", $"V souèasné chvíli nelze smazat požadovéné vozidlo.", "Ok");
                return;
            }
            parentPage?.LoadRepairsFromDatabase();

        }
        else
        {
            await App.Current.MainPage.DisplayAlert("Oznámení", $"Došlo k chybì pøi mazání opravy.", "Ok");
        }
    }

    private AllCarDetailTabbedPage FindParentMobileCars(Element element)
    {
        if (element.Parent is AllCarDetailTabbedPage allCarDetailTabbedPage)
        {
            return allCarDetailTabbedPage;
        }

        if (element.Parent != null)
        {
            // Pokraèuj v hledání v rodièovském prvku
            return FindParentMobileCars(element.Parent);
        }

        return null;
    }
}