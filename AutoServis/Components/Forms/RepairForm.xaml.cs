namespace AutoServis.Components.Forms;
using AutoServis.Model;
using Microsoft.Maui.ApplicationModel.Communication;
using System.Net.Http.Json;
using System.Text.Json;

public partial class RepairForm : ContentView
{
    public int CarId { get; set; }
	public RepairForm()
	{
        InitializeComponent();
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

        API api = new API();

        if (api.checkConnectivity())
        {
            App.Current.MainPage.DisplayAlert("Chyba", "Nejste pøipojeni k internetu.\n\n" +
                "Je potøeba internetové pøipojení!", "Ok");
            BtnRepairForm.IsVisible = true;
            LoadingIndicator.IsVisible = false;
            return;
        }

        // Vytvoøení opravy
        Repair repair = new Repair(-1, name, date, mileage, description, price, part_name, url, CarId);
                
        HttpResponseMessage response = await api.client.PostAsJsonAsync("repair/create", repair);

        if (response.IsSuccessStatusCode)
        {
            App.Current.MainPage.DisplayAlert("Oznámení", "Uložení opravy probìhlo úspìšnì.", "OK");
            // Vyèištìní vstupních polí
            nameInput.Text = "";
            repairDate.Date = DateTime.Now;
            mileageInput.Text = "";
            descriptionInput.Text = "";
            priceInput.Text = "";
            namepartInput.Text = "";
            urlInput.Text = "";
        }
        else
        {
            App.Current.MainPage.DisplayAlert("Chyba", "Nastala neoèkávaná chyba. Zkus se to znovu", "Ok");
        }
        BtnRepairForm.IsVisible = true;
        LoadingIndicator.IsVisible = false;        
    }


}