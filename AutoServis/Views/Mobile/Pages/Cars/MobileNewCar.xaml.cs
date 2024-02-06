namespace AutoServis.Views.Mobile.Pages.Cars;

public partial class MobileNewCar : ContentPage
{
	public MobileNewCar(int id)
	{
		InitializeComponent();
		NewCarForm.id = id;
	}
}