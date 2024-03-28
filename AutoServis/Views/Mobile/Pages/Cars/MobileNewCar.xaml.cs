namespace AutoServis.Views.Mobile.Pages.Cars;
using AutoServis.Model;

public partial class MobileNewCar : ContentPage
{
    private MobileCars mobileCars;
    // Pro editaci stávajícího vozidla
	public MobileNewCar(Car car, int user_id, MobileCars mobileCars)
	{
		InitializeComponent();
        Button btnEndForm = (Button)NewCarForm.FindByName("BtnEndForm");
        if (btnEndForm != null)
        {
            btnEndForm.Text = "Uložit zmìny";
        }    
        this.mobileCars = mobileCars;
        NewCarForm.id = user_id;
        SetValuesInForm(car);
    }

    private void SetValuesInForm(Car car)
    {
        // Naètení všech inputù
        Entry brandInput = (Entry)NewCarForm.FindByName("brandInput");
        Entry modelInput = (Entry)NewCarForm.FindByName("modelInput");
        Entry spzInput = (Entry)NewCarForm.FindByName("spzInput");
        DatePicker manufactureDate = (DatePicker)NewCarForm.FindByName("manufactureDate");
        Entry mileageInput = (Entry)NewCarForm.FindByName("mileageInput");
        Picker fuelPicker = (Picker)NewCarForm.FindByName("fuelPicker");
        Picker bodyPicker = (Picker)NewCarForm.FindByName("bodyPicker");
        // RadioButtons doors
        FlexLayout doorRBFlex = (FlexLayout)NewCarForm.FindByName("doorRBFlex");
        Entry colorInput = (Entry)NewCarForm.FindByName("colorInput");
        Slider sliderSeat = (Slider)NewCarForm.FindByName("sliderSeat");
        Label displaySeat = (Label)NewCarForm.FindByName("displayLabel");
        Switch airConditioningSwitch = (Switch)NewCarForm.FindByName("airConditioningSwitch");
        CheckBox checkbox_4x4 = (CheckBox)NewCarForm.FindByName("checkbox_4x4");
        Entry nicknameInput = (Entry)NewCarForm.FindByName("nicknameInput");
        Entry vinInput = (Entry)NewCarForm.FindByName("vinInput");
        Entry nameEngineInput = (Entry)NewCarForm.FindByName("nameEngineInput");
        Entry powerInput = (Entry)NewCarForm.FindByName("powerInput");
        // Radio buttons pøevodovka
        FlexLayout gearboxRBFlex = (FlexLayout)NewCarForm.FindByName("gearboxRBFlex");
        Entry codeInput = (Entry)NewCarForm.FindByName("codeInput");
        Entry displacementInput = (Entry)NewCarForm.FindByName("displacementInput");
        Entry torqueInput = (Entry)NewCarForm.FindByName("torqueInput");
        Entry oilInput = (Entry)NewCarForm.FindByName("oilInput");
        Label idCarLabel = (Label)NewCarForm.FindByName("idCar");


        // Nastavení hodnot inputùm
        idCarLabel.Text = car.id.ToString();
        brandInput.Text = car.brand;
        modelInput.Text = car.model;
        spzInput.Text = car.spz;
        manufactureDate.Date = car.manufacture;
        mileageInput.Text = car.mileage.ToString();
        //Pickery

        colorInput.Text = car.color;
        sliderSeat.Value = car.seats - 48;
        displaySeat.Text = $"Poèet míst k sezení: {car.seats}";
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
        // RadioButton dveøe
        findAndSelectRadioButton(doorRBFlex, (car.doors - 48).ToString());
        // RadioButton pøevodovka
        findAndSelectRadioButton(gearboxRBFlex, car.transmition);
        SetPicker(fuelPicker, car.fuel);
        SetPicker(bodyPicker, car.body);
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

    // Nalezne radio button a ten nastaví na danou hodnotu
    private void findAndSelectRadioButton(FlexLayout flex, string content)
    {
        foreach (View child in flex.Children)
        {
            if (child is RadioButton radioButton)
            {
                if (radioButton.Content.ToString() == content)
                {
                    radioButton.IsChecked = true;
                    if (NewCarForm.FindByName("doorsRBFlex") == flex) NewCarForm.setDoors(content[0]);
                    else NewCarForm.setTransmition(content);
                    break;
                }
            }
        }
    }

    // Bez id pro pøidání nového vozidla
    public MobileNewCar(int user_id, MobileCars mobileCars)
    {
        InitializeComponent();
        NewCarForm.id = user_id;
        this.mobileCars = mobileCars;
    }

    // Posílá do hlavní stránky mobileCars auto, které aktualizuje a znovu naète stránku
    public void SaveToList(Car car, bool isAdd)
    {
        mobileCars.SaveCarToList(car, isAdd);
    }
}