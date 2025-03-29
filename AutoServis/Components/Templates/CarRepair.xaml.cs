namespace AutoServis.Components.Templates;
using AutoServis.Model;
using AutoServis.Repository;
using AutoServis.Services;
using AutoServis.Views.All.Pages.CarDetail;
using AutoServis.Views.All.Pages.RepairDetail;
using System.Security.Cryptography;

public partial class CarRepair : ContentView
{
    //private RepairsRepository repairsRepository;
    private RepairService repairService = new RepairService();

    public event Action<CarRepair>? OnDeleteRequested;

    public CarRepair()
	{
		InitializeComponent();
	}

    public static readonly BindableProperty RepairsRepositoryProperty =
    BindableProperty.Create(
        nameof(RepairsRepository),
        typeof(RepairsRepository),
        typeof(CarRepair),
        null);    

    public static readonly BindableProperty RepairIdProperty = BindableProperty.Create(nameof(RepairId), typeof(int), typeof(CarRepair), -1);
    public static readonly BindableProperty RepairNameProperty = BindableProperty.Create(nameof(RepairName), typeof(string), typeof(CarRepair), string.Empty);
    public static readonly BindableProperty RepairDateProperty = BindableProperty.Create(nameof(RepairDate), typeof(string), typeof(CarRepair), string.Empty);
    public static readonly BindableProperty RepairMileageProperty = BindableProperty.Create(nameof(RepairMileage), typeof(string), typeof(CarRepair), string.Empty);
    public static readonly BindableProperty RepairPriceProperty = BindableProperty.Create(nameof(RepairPrice), typeof(string), typeof(CarRepair), string.Empty);
    public string decription { get; set; }
    public string part_name { get; set; }
    public string url { get; set; }
    public int car_id { get; set; }

    public AllCarDetailTabbedPage AllCarDetailTabbedPage { get; set; }

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

    public RepairsRepository RepairsRepository
    {
        get => (RepairsRepository)GetValue(RepairsRepositoryProperty);
        set => SetValue(RepairsRepositoryProperty, value);
    }


    private async void OnEditSwipeItemInvoked(object sender, EventArgs e)
    {
        Repair? repair = RepairsRepository.GetRepair(RepairId);
        if (repair != null)
        {
            await Navigation.PushAsync(new AllCarDetailFormRepair(repair, RepairsRepository));
            return;
        }
        await App.Current.MainPage.DisplayAlert("Chyba", "Oprava nebyla nalezena.", "Ok");
    }

    private async void OnDeleteSwipeItemInvoked(object sender, EventArgs e)
    {
        // Kontrola zda chce opravdu daný záznam smazat
        bool answer = await App.Current.MainPage.DisplayAlert("Smazat", $"Opravdu si přejete opravu {RepairName}?", "Ano", "Ne");
        if (!answer) return;

        (bool? success, string message) = await repairService.RemoveRepair(RepairId);

        if (success == null || success == false)
        {
            await App.Current.MainPage.DisplayAlert("Chyba", message, "Ok");
            return;
        }

        RepairsRepository.RemoveRepair(RepairId);
        RepairsRepository.dataChange = true;
        await App.Current.MainPage.DisplayAlert("Oznámení", message, "Ok");

        CarRepair? carRepair = RepairsRepository.GetCarRepairTemplate(RepairId);
        if (carRepair != null)
        {
            RepairsRepository.RemoveCarRepairTemplate(RepairId);
        }

        AllCarDetailTabbedPage parentPage = FindParentMobileCars(this);
        parentPage?.DeleteCarRepair(RepairId);
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


    private async void ClickMoreInfo(object sender, EventArgs e)
    {
        Repair? repair = RepairsRepository.GetRepair(RepairId);
        if (repair != null)
        {
            await Navigation.PushAsync(new AllRepairDetail(repair, RepairsRepository));
            return;
        }
        await App.Current.MainPage.DisplayAlert("Chyba", "Oprava nebyla nalezena.", "Ok");
    }
}