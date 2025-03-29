using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using AutoServis.Model;
using AutoServis.Components.Templates;
using AutoServis.Services;

namespace AutoServis.Repository
{
    public class RepairsRepository
    {
        private List<Repair> repairs = new List<Repair>();
        private List<CarRepair> carRepairsTemplate = new List<CarRepair>();
        public bool dataChange { get; set; }
        public bool addNewRepair { get; set; }

        public RepairsRepository() 
        {
            dataChange = false;
            addNewRepair = false;
        }

        public RepairsRepository(List<Repair> repairs): this()
        {
            this.repairs = repairs;
        }

        public List<Repair> Repairs { 
            get { return repairs; 
            } 
        }

        public Repair? GetRepair(int index)
        {
            try
            {
                return repairs.First(c => c.id == index);
            }
            catch (InvalidOperationException ex)
            {
                
            }
            return null;
        }

        public void NewList(List<Repair> repairs)
        {
            //this.repairs = new List<Repair>(repairs);
            this.repairs = repairs;
        }

        public void AddRepair(Repair repair)
        {
            if (repair != null)
            {
                repairs.Add(repair);
            }
        }

        public void RemoveRepair(int index)
        {
            repairs.RemoveAll(c => c.id == index);
        }

        public void UpdateRepair(Repair repair)
        {
            int index = repairs.FindIndex(repairList => repairList.id == repair.id);
            if (index != -1)
            {
                repairs[index] = repair;
            }
        }

        public void AddCarRepairTemplate(CarRepair carRepair)
        {
            this.carRepairsTemplate.Add(carRepair);
        }

        public void AddCarRepairTemplate(Repair repair, RepairsRepository repairsRepository, RepairService repairService)
        {            
            CarRepair carRepair = new CarRepair()
            {
                Margin = 3,
                MaximumWidthRequest = 1000,
                RepairId = repair.id,
                RepairName = repair.name,
                RepairDate = repair.date.ToShortDateString(),
                RepairMileage = repair.mileage + " Km",
                RepairPrice = repair.price + " Kč",
                decription = repair.description,
                part_name = repair.part_name,
                url = repair.url,
                car_id = repair.car_id,
                RepairsRepository = repairsRepository,
            };
            this.carRepairsTemplate.Add(carRepair);
        }

        public CarRepair? GetCarRepairTemplate(int repairId)
        {
            int index = repairs.FindIndex(repairList => repairList.id == repairId);
            if (index != -1)
            {
                return carRepairsTemplate[index];
            }
            return null;
        }

        public List<CarRepair> GetCarRepairsTemplates()
        {
            return this.carRepairsTemplate;
        }

        public CarRepair GetLastCarRepairTemplate()
        {
            return carRepairsTemplate.Last();
        }

        public void UpdateCarRepairTemplate(Repair repair)
        {
            int index = repairs.FindIndex(repairList => repairList.id == repair.id);
            if (index != -1)
            {
                CarRepair carRepair = this.carRepairsTemplate[index];
                if (carRepair != null)
                {
                    carRepair.RepairId = repair.id;
                    carRepair.RepairName = repair.name;
                    carRepair.RepairDate = repair.date.ToShortDateString();
                    carRepair.RepairMileage = repair.mileage + " Km";
                    carRepair.RepairPrice = repair.price + " Kč";
                    carRepair.decription = repair.description;
                    carRepair.part_name = repair.part_name;
                    carRepair.url = repair.url;
                }
            }
        }

        public void RemoveCarRepairTemplate(int repairId)
        {
            carRepairsTemplate.RemoveAll(list => list.RepairId == repairId);
        }

        public void ClearCarRepairTemplateList()
        {
            carRepairsTemplate.Clear();
        }
    }
}
