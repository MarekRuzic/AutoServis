using Microsoft.Maui.Controls.Internals;
using AutoServis.Model;
using AutoServis.Model.API;
using System.Net.Http.Json;
using AutoServis.Views.Mobile.Pages.Cars;
namespace AutoServis.Components.Forms;

public partial class NewCarForm : ContentView
{
    public NewCarForm()
    {
        InitializeComponent();
    }
    public Int32 id { get; set;}
    private char doors = '5';
    private string transmition = "Manuální";

    public void setDoors(char doors)
    {
        this.doors = doors;
    }

    public void setTransmition(string transmition)
    {
        this.transmition = transmition;
    }

    private bool IsInputEmpty(String text)
    {
        if (text == null || text.Trim() == "") return true;
        return false;
    }

    private void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
    {
        double value = Math.Round(e.NewValue);
        sliderSeat.Value = value;
        displayLabel.Text = $"Poèet míst k sezení: {value}";
    }

    private void seatButtonClick(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        string text = btn.Text;
        double value = sliderSeat.Value;
        if (text.ToLower().Trim() == "pøidat")
        {
            value += 1;
            if (value > 9) value = 9;
        }
        else
        {
            value -= 1;
            if (value < 2) value = 2;
        }
        sliderSeat.Value = value;
    }

    private void SwitchChange(object sender, ToggledEventArgs e)
    {
        if (airConditioningSwitch.IsToggled) airConditioningLabel.Text = "Ano";
        else airConditioningLabel.Text = "Ne";
    }

    private async void newCar(object sender, EventArgs e)
    {
        if (IsInputEmpty(brandInput.Text) &&
            IsInputEmpty(modelInput.Text) &&
            IsInputEmpty(vinInput.Text) &&
            IsInputEmpty(spzInput.Text) &&
            IsInputEmpty(nameEngineInput.Text) &&
            IsInputEmpty(powerInput.Text))
        {
            App.Current.MainPage.DisplayAlert("Oznámení", "Nìjaký z povinných údajù nebyl vyplnìn.", "Ok");
            return;
        }

        double mileage = 0;
        if (!Double.TryParse(mileageInput.Text, out mileage))
        {
            App.Current.MainPage.DisplayAlert("Oznámení", "Údaj stav kilometrù je špatnì vyplnìn", "Ok");
            return;
        }

        if (fuelPicker.SelectedIndex == -1 || bodyPicker.SelectedIndex == -1)
        {
            App.Current.MainPage.DisplayAlert("Oznámení", "Nebyly vyplnìny všechny údaje", "Ok");
            return;
        }

        string brand = brandInput.Text.Trim();
        string model = modelInput.Text.Trim();
        DateTime manufature = manufactureDate.Date;
        string fuel = fuelPicker.ItemsSource[fuelPicker.SelectedIndex].ToString();
        string body = bodyPicker.ItemsSource[bodyPicker.SelectedIndex].ToString();
        string color = "";
        if (!IsInputEmpty(colorInput.Text)) color = colorInput.Text.Trim();
        bool drive4x4 = checkbox_4x4.IsChecked;
        char seats = sliderSeat.Value.ToString()[0];
        bool airCondition = airConditioningSwitch.IsToggled;
        string vin = vinInput.Text.Trim();
        string spz = spzInput.Text.Trim();
        string nickname = "";
        if (!IsInputEmpty(nicknameInput.Text)) nickname = nicknameInput.Text.Trim();
        string name_engine = nameEngineInput.Text.Trim();
        string code = "";
        if (!IsInputEmpty(codeInput.Text)) code = codeInput.Text.Trim();
        string displacement = "";
        if (!IsInputEmpty(displacementInput.Text)) displacement = displacementInput.Text.Trim();
        string power = powerInput.Text.Trim();
        string torque = "";
        if (!IsInputEmpty(torqueInput.Text)) torque = torqueInput.Text.Trim();
        string oil_capacity = "";
        if (!IsInputEmpty(oilInput.Text)) oil_capacity = oilInput.Text.Trim();

        if (id.Equals(null))
        {
            return;
        }

        Car car = new Car(-1, brand, model, manufature, mileage, fuel, body,
            color, drive4x4, doors, seats, airCondition, vin, spz, nickname, 
            name_engine, code, displacement, power, torque, oil_capacity, transmition,
            id);

        API api = new API();
        if (api.checkConnectivity())
        {
            App.Current.MainPage.DisplayAlert("Chyba", "Nejste pøipojeni k internetu.\n\n" +
                "Je potøeba internetové pøipojení!", "Ok");
            return;
        }

        HttpResponseMessage response;

        // Nalezne rodièe a zavolá metodu SaveToList
        MobileNewCar parentPage = FindParentMobileNewCar(this);        

        if (BtnEndForm.Text.Trim() == "Uložit zmìny")
        {
            car.id = Convert.ToInt32(idCar.Text);
            response = await api.client.PutAsJsonAsync("car/update", car);

            if (response.IsSuccessStatusCode)
            {
                // Pøidání do hlavního listu upravené vozidlo
                if (parentPage != null)
                {
                    parentPage?.SaveToList(car, false);
                }
                await App.Current.MainPage.DisplayAlert("Oznámení", "U vozidla byla úspìšnì zmìnìna data", "OK");
                await Navigation.PopAsync();
            }
            else App.Current.MainPage.DisplayAlert("Chyba", "Nastala neoèkávaná chyba. Zkus se to znovu", "Ok");
            return;
        }

        response = await api.client.PostAsJsonAsync("car/create", car);

        if (response.IsSuccessStatusCode)
        {
            // Pøidání nového vozidla do listu
            if (parentPage != null)
            {
                parentPage?.SaveToList(null, true);
            }
            await App.Current.MainPage.DisplayAlert("Oznámení", "Vozidlo bylo úspìšnì vytvoøeno.", "OK");
            await Navigation.PopAsync();
        }
        else App.Current.MainPage.DisplayAlert("Chyba", "Nastala neoèkávaná chyba. Zkus se to znovu", "Ok");
    }

    private void doorsChange(object sender, CheckedChangedEventArgs e)
    {
        if (sender is RadioButton radioButton && e.Value)
        {
            // Získání obsahu (Content) zvoleného RadioButtonu
            string selectedContent = radioButton.Content?.ToString();
            doors = selectedContent[0];
        }
    }

    private void gearboxChange(object sender, CheckedChangedEventArgs e)
    {
        if (sender is RadioButton radioButton && e.Value)
        {
            transmition = radioButton.Content?.ToString();
        }
    }

    private MobileNewCar FindParentMobileNewCar(Element element)
    {
        // Rekurzivnì hledá rodièovskou stránku MobileCars
        if (element.Parent is MobileNewCar mobileNewCar)
        {
            return mobileNewCar;
        }

        if (element.Parent != null)
        {
            // Pokraèuj v hledání v rodièovském prvku
            return FindParentMobileNewCar(element.Parent);
        }

        // Nenalezena žádná rodièovská stránka MobileCars
        return null;
    }
}