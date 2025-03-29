namespace AutoServis.Views.All.Pages.CarDetail;
using AutoServis.Components.Forms;
using AutoServis.Model;
using AutoServis.Repository;

public partial class AllCarDetailFormRepair : ContentPage
{
	public AllCarDetailFormRepair()
	{
		InitializeComponent();        
    }

	public AllCarDetailFormRepair(int carId, RepairsRepository repairsRepository)
	{
        InitializeComponent();
		RepairForm repairForm = new RepairForm(carId, repairsRepository);
		formAddToView(repairForm);
    }

    public AllCarDetailFormRepair(Repair repair, RepairsRepository repairsRepository)
    {
        InitializeComponent();
        RepairForm repairForm = new RepairForm(repair.car_id, repairsRepository);
        formAddToView(repairForm);
		fillRepairForm(repairForm, repair);
    }

	private void formAddToView(RepairForm repairForm)
	{

#if ANDROID || IOS
        MobileFormView.Children.Clear();
        MobileFormView.Children.Add(repairForm);
#else
		WindowsFormView.Children.Clear();
        WindowsFormView.Children.Add(repairForm);
#endif
    }

    private void fillRepairForm(RepairForm repairForm, Repair repair)
	{
		Label title = (Label)repairForm.FindByName("titleName");
		Label repairId = (Label)repairForm.FindByName("idRepair");
        Entry nameInput = (Entry)repairForm.FindByName("nameInput");
		DatePicker dateInput = (DatePicker)repairForm.FindByName("repairDate");
		Entry mileageinput = (Entry)repairForm.FindByName("mileageInput");
		Editor descriptionInput = (Editor)repairForm.FindByName("descriptionInput");
        Entry priceInput = (Entry)repairForm.FindByName("priceInput");
        Entry namepartInput = (Entry)repairForm.FindByName("namepartInput");
        Entry urlInput = (Entry)repairForm.FindByName("urlInput");
		Button button = (Button)repairForm.FindByName("BtnRepairForm");

		repairId.Text = repair.id.ToString();
		title.Text = "Editace opravy vozidla";
		nameInput.Text = repair.name;
		dateInput.Date = repair.date;
		mileageinput.Text = repair.mileage.ToString();
		descriptionInput.Text = repair.description;
		priceInput.Text = repair.price;
		namepartInput.Text = repair.part_name;
		urlInput.Text = repair.url;
		button.Text = "Uložit zmìny";
    }
}