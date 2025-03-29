namespace AutoServis.Views.All.Pages.RepairDetail;
using AutoServis.Model;
using AutoServis.Repository;
using AutoServis.Views.All.Pages.CarDetail;
using AutoServis.Components.Templates;

public partial class AllRepairDetail : ContentPage
{
    private Repair repair { get; set; }
    private string url = "";
    private RepairsRepository repairsRepository;

	public AllRepairDetail(Repair repair, RepairsRepository repairsRepository)
	{
        InitializeComponent();
        LoadDataToComponents(repair);
        this.repair = repair;
        this.repairsRepository = repairsRepository;
	}

    private void LoadDataToComponents(Repair repair)
    {
        nameRepair.Text = repair.name;
        dateRepair.Text = repair.date.ToShortDateString();
        mileageRepair.Text = repair.mileage.ToString() + " Km";
        priceRepair.Text = repair.price + " Kè";
        descriptionRepair.Text = repair.description;
        if (repair.part_name != "")
        {
            partnameVisual.IsVisible = true;
            partnameRepair.Text = repair.part_name;
        }
        if (repair.url != "")
        {
            lineVisual.IsVisible = true;
            urlVisual.IsVisible = true;
            urlRepair.Text = repair.url;
            url = repair.url;
        }
    }

    private async void OpenLink(object sender, EventArgs e)
    {
        try
        {
            if (url != "")
            {
                Uri uri = new Uri(url);
                await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
            }
            else
            {
                throw new Exception();
            }
        }
        catch (Exception ex)
        { // An unexpected error occurred. No browser may be installed on the device.
            await DisplayAlert("Oznámení", "Pøi otvírání prohlížeèe došlo k chybì", "ok");            
        }
    }

    private async void EditRepair_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AllCarDetailFormRepair(repair, repairsRepository));
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Repair? repair = repairsRepository.GetRepair(this.repair.id);
        if (repair != null)
        {
            this.repair = repair;
            LoadDataToComponents(repair);
        }
        
    }
}