namespace AutoServis.Components.Forms;
using AutoServis.Model;
using AutoServis.Views.All.Pages.CarDetail;
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
            App.Current.MainPage.DisplayAlert("Ozn�men�", "N�jak� z povinn�ch �daj� nebyl vypln�n.", "Ok");
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
            App.Current.MainPage.DisplayAlert("Chyba", "Nejste p�ipojeni k internetu.\n\n" +
                "Je pot�eba internetov� p�ipojen�!", "Ok");
            BtnRepairForm.IsVisible = true;
            LoadingIndicator.IsVisible = false;
            return;
        }

        int id = -1;
        if (idRepair.Text != "")
        {
            id = Convert.ToInt32(idRepair.Text);
        }

        // Vytvo�en� opravy
        Repair repair = new Repair(id, name, date, mileage, description, price, part_name, url, CarId);

        HttpResponseMessage response = null;
        if (id == -1) response = await api.client.PostAsJsonAsync("repair/create", repair);
        else response = await api.client.PutAsJsonAsync("repair/update", repair);

        if (response.IsSuccessStatusCode)
        {
            App.Current.MainPage.DisplayAlert("Ozn�men�", "Ulo�en� opravy prob�hlo �sp�n�.", "OK");
            // Vy�i�t�n� vstupn�ch pol�
            nameInput.Text = "";
            repairDate.Date = DateTime.Now;
            mileageInput.Text = "";
            descriptionInput.Text = "";
            priceInput.Text = "";
            namepartInput.Text = "";
            urlInput.Text = "";

            if (id != -1)
            {
                AllCarDetailFormRepair parentPage = FindParentMobileNewCar(this);
                if (parentPage != null)
                {
                    parentPage?.AllCarDetailTabbedPage.LoadRepairsFromDatabase();
                }
                await Navigation.PopAsync();
            }
        }
        else
        {
            App.Current.MainPage.DisplayAlert("Chyba", "Nastala neo�k�van� chyba. Zkus se to znovu", "Ok");
        }
        BtnRepairForm.IsVisible = true;
        LoadingIndicator.IsVisible = false;        
    }

    private AllCarDetailFormRepair FindParentMobileNewCar(Element element)
    {
        // Rekurzivn� hled� rodi�ovskou str�nku allCarDetailFormRepair
        if (element.Parent is AllCarDetailFormRepair allCarDetailFormRepair)
        {
            return allCarDetailFormRepair;
        }

        if (element.Parent != null)
        {
            return FindParentMobileNewCar(element.Parent);
        }

        return null;
    }


}