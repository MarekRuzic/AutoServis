namespace AutoServis.Views.All.Pages.CarForm;

using AutoServis.Components.Forms;
using AutoServis.Model;
using AutoServis.Repository;
using AutoServis.Services;

public partial class AllCarForm : ContentPage
{
    // Pro editaci stávajícího vozidla
    public AllCarForm(Car car, int user_id, ICarsRepository carsRepository, ICarService carService, string title)
    {
        InitializeComponent();
        NewCarForm carForm = new NewCarForm(car, carService, carsRepository);
        Content = carForm;
        Title = title;
    }

    // Bez id pro pøidání nového vozidla
    public AllCarForm(int user_id, ICarService carService, ICarsRepository carsRepository)
    {
        InitializeComponent();
        Content = new NewCarForm(user_id, carService, carsRepository);
    }
}