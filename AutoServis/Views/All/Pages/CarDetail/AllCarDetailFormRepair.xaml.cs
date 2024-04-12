namespace AutoServis.Views.All.Pages.CarDetail;
using AutoServis.Components.Forms;
using AutoServis.Model;

public partial class AllCarDetailFormRepair : ContentPage
{
	public int CarId { get; set; }
	public AllCarDetailFormRepair()
	{
		InitializeComponent();
    }

    public AllCarDetailFormRepair(Repair repair)
    {
        InitializeComponent();
		RepairForm repairForm;
		#if ANDROID || IOS
			repairForm = RepairFormMobile;
		#else
			repairForm = RepairFormWindows;
		#endif
		fillRepairForm(repairForm, repair);
    }

	private void fillRepairForm(RepairForm repairForm, Repair repair)
	{
		Entry nameInput = (Entry)repairForm.FindByName("nameInput");
		DatePicker dateInput = (DatePicker)repairForm.FindByName("repairDate");
		Entry mileageinput = (Entry)repairForm.FindByName("mileageInput");
		Editor descriptionInput = (Editor)repairForm.FindByName("descriptionInput");
        Entry priceInput = (Entry)repairForm.FindByName("priceInput");
        Entry namepartInput = (Entry)repairForm.FindByName("namepartInput");
        Entry urlInput = (Entry)repairForm.FindByName("urlInput");

		nameInput.Text = repair.name;
		dateInput.Date = repair.date;
		mileageinput.Text = repair.mileage.ToString();
		descriptionInput.Text = repair.description;
		priceInput.Text = repair.price;
		namepartInput.Text = repair.part_name;
		urlInput.Text = repair.url;
    }

    protected override void OnAppearing()
	{
		base.OnAppearing();											
		#if ANDROID || IOS
			RepairFormMobile.CarId = CarId;
		#else
			RepairFormWindows.CarId = CarId;
		#endif
    }
}