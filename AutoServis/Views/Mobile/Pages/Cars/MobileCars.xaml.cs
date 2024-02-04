namespace AutoServis.Views.Mobile.Pages.Cars;
using AutoServis.Model;
using AutoServis.Model.API;
using AutoServis.Model.JSON;
using System.Text.Json;

public partial class MobileCars : ContentPage
{
	User user;
	public MobileCars(Object userO)
	{
		InitializeComponent();
        user = (User)userO;
		zobraz();
	}

	private async void zobraz()
	{
        verticalLayout.Children.Add(new Label { Text="Táhni" });
        API api = new API();

        HttpResponseMessage responseMessage = await api.client.GetAsync($"car/listcaruser?id={user.id}");
        if (responseMessage.IsSuccessStatusCode)
		{
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                Converters = { new BoolConverter() },
            };
            string getResponseString = await responseMessage.Content.ReadAsStringAsync();
            List<Car> cars = JsonSerializer.Deserialize<List<Car>>(getResponseString, options);
            foreach (Car car in cars)
            {
                Label label = new Label
                {
                    Text=car.id.ToString(),
                };
                verticalLayout.Children.Add(label);
            }
        }
    }

    private async void addNewCarClick(object sender, EventArgs e)
    {
		await Navigation.PushAsync(new MobileNewCar());
    }

    private void OnSwipe(object sender, SwipedEventArgs e)
    {
        DisplayAlert("Ahoj", "Toto bylo swipe :D", "ok");
    }
}