namespace AutoServis.Components.Forms;

public partial class NewCarForm : ContentView
{
	public NewCarForm()
	{
		InitializeComponent();
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
}