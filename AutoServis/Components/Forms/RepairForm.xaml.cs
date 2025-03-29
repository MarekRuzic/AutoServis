namespace AutoServis.Components.Forms;
using AutoServis.Model;
using AutoServis.Repository;
using AutoServis.Services;
using AutoServis.Views.All.Pages.CarDetail;
using Microsoft.Maui.ApplicationModel.Communication;
using System.Net.Http.Json;
using System.Text.Json;

public partial class RepairForm : ContentView
{
    RepairService repairService = new RepairService();
    public RepairsRepository repairsRepository { get; set; }
    public int CarId { get; set; }
	public RepairForm()
	{
        InitializeComponent();
	}

    public RepairForm(int carId, RepairsRepository repairsRepository)
    {
        InitializeComponent();
        this.repairsRepository = repairsRepository;
        this.CarId = carId;
    }

    private void OnEntryTextChange(object sender, TextChangedEventArgs e)
    {
        string text = ((Entry)sender).Text;
        string result = "";
        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] >= '0' && text[i] <= '9'
            || text[i] == ' ') result += text[i];
        }
        ((Entry)sender).Text = result;
    }

    private bool IsInputEmpty(String text)
    {
        if (text == null || text.Trim() == "") return true;
        return false;
    }

    private async void saveRepairCar_Clicked(object sender, EventArgs e)
    {
        if (IsInputEmpty(nameInput.Text) &&
           IsInputEmpty(mileageInput.Text) &&
           IsInputEmpty(descriptionInput.Text))
        {
            App.Current.MainPage.DisplayAlert("Oznámení", "Nìjaký z povinných údajù nebyl vyplnìn.", "Ok");
            return;
        }        

        string name = nameInput.Text;
        DateTime date = repairDate.Date;
        double mileage = Convert.ToDouble(mileageInput.Text);
        string description = descriptionInput.Text;
        string price = priceInput.Text;
        string part_name = namepartInput.Text;
        string url = urlInput.Text;

        BtnRepairForm.IsVisible = false;
        LoadingIndicator.IsVisible = true;        

        int id = -1;
        if (idRepair.Text != "")
        {
            id = Convert.ToInt32(idRepair.Text);
        }

        // Vytvoøení opravy
        Repair repair = new Repair(id, name, date, mileage, description, price, part_name, url, CarId);
        bool? success;
        string message;

        if (id == -1) (success, message) = await repairService.InsertRepair(repair);
        else (success, message) = await repairService.UpdateRepair(repair);

        if (success == null || success == false)
        {
            App.Current.MainPage.DisplayAlert("Chyba", message, "OK");
            return;
        }        

        // Vyèištìní vstupních polí
        nameInput.Text = "";
        repairDate.Date = DateTime.Now;
        mileageInput.Text = "";
        descriptionInput.Text = "";
        priceInput.Text = "";
        namepartInput.Text = "";
        urlInput.Text = "";

        BtnRepairForm.IsVisible = true;
        LoadingIndicator.IsVisible = false;

        repairsRepository.dataChange = true;
        await App.Current.MainPage.DisplayAlert("Oznámení", message, "OK");
        if (id != -1)
        {
            repairsRepository.UpdateRepair(repair);    
            repairsRepository.UpdateCarRepairTemplate(repair);
            await Navigation.PopAsync();
            return;
        }
        repairsRepository.AddRepair(repair);
        repairsRepository.AddCarRepairTemplate(repair, repairsRepository, repairService);
        repairsRepository.addNewRepair = true;
    }
}