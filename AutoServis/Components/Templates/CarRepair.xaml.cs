namespace AutoServis.Components.Templates;

public partial class CarRepair : ContentView
{
	public CarRepair()
	{
		InitializeComponent();
	}

    public static readonly BindableProperty RepairIdProperty = BindableProperty.Create(nameof(RepairId), typeof(int), typeof(CarRepair), -1);
    public static readonly BindableProperty RepairNameProperty = BindableProperty.Create(nameof(RepairName), typeof(string), typeof(CarRepair), string.Empty);
    public static readonly BindableProperty RepairDateProperty = BindableProperty.Create(nameof(RepairDate), typeof(string), typeof(CarRepair), string.Empty);
    public static readonly BindableProperty RepairMileageProperty = BindableProperty.Create(nameof(RepairMileage), typeof(string), typeof(CarRepair), string.Empty);
    public static readonly BindableProperty RepairPriceProperty = BindableProperty.Create(nameof(RepairPrice), typeof(string), typeof(CarRepair), string.Empty);

    public int RepairId
    {
        get => (int)GetValue(RepairIdProperty);
        set => SetValue(RepairIdProperty, value);
    }

    public string RepairName
    {
        get => (string)GetValue(RepairNameProperty);
        set => SetValue(RepairNameProperty, value);
    }

    public string RepairDate
    {
        get => (string)GetValue(RepairDateProperty);
        set => SetValue(RepairDateProperty, value);
    }

    public string RepairMileage
    {
        get => (string)GetValue(RepairMileageProperty);
        set => SetValue(RepairMileageProperty, value);
    }

    public string RepairPrice
    {
        get => (string)GetValue(RepairPriceProperty);
        set => SetValue(RepairPriceProperty, value);
    }


    private void OnEditSwipeItemInvoked(object sender, EventArgs e)
    {

    }

    private void OnDeleteSwipeItemInvoked(object sender, EventArgs e)
    {

    }
}