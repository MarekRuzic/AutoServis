using Microsoft.Maui.Controls.Internals;
using AutoServis.Model;
using System.Net.Http.Json;
using AutoServis.Views.Mobile.Pages.Cars;
using AutoServis.Services;
using AutoServis.Repository;
namespace AutoServis.Components.Forms;

public partial class NewCarForm : ContentView
{
    private Int32 userId;
    private ICarService carService;
    private ICarsRepository carsRepository;
    private char doors = '5';
    private string transmition = "Manuální";

    public NewCarForm()
    {
        InitializeComponent();
    }

    public NewCarForm(int userId, ICarService carService, ICarsRepository carsRepository)
    {
        InitializeComponent();
        this.userId = userId;
        this.carService = carService;
        this.carsRepository = carsRepository;
    }

    public NewCarForm(Car car, ICarService carService,  ICarsRepository carsRepository)
    {
        InitializeComponent();
        // Incializace proměnných
        this.carService = carService;
        this.carsRepository = carsRepository;
        this.userId = car.user_id;
        SetAllInputs(car);        
    }

    private void SetAllInputs(Car car)
    {
        headerLabel.Text = "Editace vozidla";
        BtnEndForm.Text = "Uložit změny";

        // Nastavení hodnot inputům
        idCar.Text = car.id.ToString();
        brandInput.Text = car.brand;
        modelInput.Text = car.model;
        spzInput.Text = car.spz;
        manufactureDate.Date = car.manufacture;
        mileageInput.Text = car.mileage.ToString();
        colorInput.Text = car.color;
        sliderSeat.Value = car.seats - 48;
        displayLabel.Text = $"Počet míst k sezení: {car.seats}";
        airConditioningSwitch.IsToggled = car.aircondition;
        checkbox_4x4.IsChecked = car.drive4x4;
        nicknameInput.Text = car.nickname;
        vinInput.Text = car.vin;
        nameEngineInput.Text = car.name_engine;
        powerInput.Text = car.power;
        codeInput.Text = car.code;
        displacementInput.Text = car.displacement;
        torqueInput.Text = car.torque;
        oilInput.Text = car.oil_capacity;
        // RadioButton dveře
        doors = (car.doors - 48).ToString()[0];
        findAndSelectRadioButton(doorRBFlex, (car.doors - 48).ToString());

        // RadioButton převodovka
        transmition = car.transmition;
        findAndSelectRadioButton(gearboxRBFlex, car.transmition);

        //Pickery
        SetPicker(fuelPicker, car.fuel);
        SetPicker(bodyPicker, car.body);
    }


    private void findAndSelectRadioButton(FlexLayout flex, string content)
    {
        foreach (View child in flex.Children)
        {
            if (child is RadioButton radioButton)
            {
                if (radioButton.Content.ToString() == content)
                {
                    radioButton.IsChecked = true;
                    if (FindByName("doorsRBFlex") == flex) this.doors = content[0];
                    else this.transmition = content;
                    break;
                }
            }
        }
    }

    // Vybrání správné hodnoty v Pickeru
    private void SetPicker(Picker picker, string item)
    {
        if (picker.ItemsSource is IList<string> fuelItems)
        {
            int index = fuelItems.IndexOf(item);

            // Zkontrolujte, zda hodnota byla nalezena ve zdroji dat
            if (index != -1)
            {
                picker.SelectedIndex = index;
            }
        }
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
        displayLabel.Text = $"Počet míst k sezení: {value}";
    }

    private void seatButtonClick(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        string text = btn.Text;
        double value = sliderSeat.Value;
        if (text.ToLower().Trim() == "přidat")
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
        if (airConditioningSwitch.IsToggled) airConditioningLabel.Text = "Klimatizace - Ano";
        else airConditioningLabel.Text = "Klimatizace - Ne";
    }

    private async void newCarClick(object sender, EventArgs e)
    {
        if (IsInputEmpty(brandInput.Text) &&
            IsInputEmpty(modelInput.Text) &&
            IsInputEmpty(vinInput.Text) &&
            IsInputEmpty(spzInput.Text) &&
            IsInputEmpty(nameEngineInput.Text) &&
            IsInputEmpty(powerInput.Text))
        {
            App.Current.MainPage.DisplayAlert("Oznámení", "Nějaký z povinných údajů nebyl vyplněn.", "Ok");
            return;
        }
        // Kontrola zda bylo do vstupního pole zadané pouze číslo
        double mileage = 0;
        if (!Double.TryParse(mileageInput.Text, out mileage))
        {
            App.Current.MainPage.DisplayAlert("Oznámení", "Údaj stav kilometrů je špatně vyplněn", "Ok");
            return;
        }

        // Kontrola zda byl zvolen prvek v komponentě picker
        if (fuelPicker.SelectedIndex == -1 || bodyPicker.SelectedIndex == -1)
        {
            App.Current.MainPage.DisplayAlert("Oznámení", "Nebyly vyplněny všechny údaje", "Ok");
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

        if (userId.Equals(null))
        {
            return;
        }

        // C# kód zobrazení ActivityIndicator s Label
        LoadingIndicator.IsVisible = true;
        BtnEndForm.IsVisible = false;

        Car car = new Car(-1, brand, model, manufature, mileage, fuel, body,
                color, drive4x4, doors, seats, airCondition, vin, spz, nickname,
                name_engine, code, displacement, power, torque, oil_capacity, transmition,
                userId);

        if (BtnEndForm.Text.Trim() == "Uložit změny")
        {
            car.id = Convert.ToInt32(idCar.Text);
            if (await carService.UpdateCar(car))
            {
                carsRepository.UpdateCar(car);
                carsRepository.dataChange = true;
                await Navigation.PopAsync();
            }
        }
        else
        {
            if (await carService.InsertNewCar(car))
            {
                carsRepository.AddCar(car);
                carsRepository.dataChange = true;
                await Navigation.PopAsync();
            }
        }

        LoadingIndicator.IsVisible = false;
        BtnEndForm.IsVisible = true;        
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

    private void OnEntryTextChange(object sender, TextChangedEventArgs e)
    {
        string text = ((Entry)sender).Text;
        string result = "";
        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] >= '0' && text[i] <= '9' || text[i] == ' ') 
                result += text[i];
        }
        ((Entry)sender).Text = result;    
    }
}