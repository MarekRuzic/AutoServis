namespace AutoServis.Components.Forms;

public partial class RepairForm : ContentView
{
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

    private void saveRepairCar_Clicked(object sender, EventArgs e)
    {
        if (IsInputEmpty(nameInput.Text) &&
           IsInputEmpty(mileageInput.Text) &&
           IsInputEmpty(descriptionInput.Text))
        {
            App.Current.MainPage.DisplayAlert("Oznámení", "Nìjaký z povinných údajù nebyl vyplnìn.", "Ok");
            return;
        }

        string name = nameInput.Text;
        string mileage = mileageInput.Text;
        string description = descriptionInput.Text;
        DateTime date = repairDate.Date;
        string partName = namepartInput.Text;
        string urlweb = urlInput.Text;


    }
}