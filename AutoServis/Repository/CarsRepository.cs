using AutoServis.Model;
using AutoServis.Model.JSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AutoServis.Repository
{
    public class CarsRepository : ICarsRepository
    {
        private List<Car> cars = new List<Car>();
        public bool dataChange { get; set; }

        public CarsRepository() { }

        public CarsRepository(List<Car> cars) 
        { 
            this.cars = cars;
        }

        public List<Car> GetCars() 
        { 
            return cars; 
        }

        public Car GetCar(int carId)
        {
            try
            {
                return cars.First(c => c.id == carId);
            }
            catch(InvalidOperationException ex)
            {
                App.Current.MainPage.DisplayAlert("Chyba", "Auto nebylo nalezeno", "Ok");
            }
            return null;
        }

        public void AddCar(Car car)
        {
            if (car != null)
            {
                cars.Add(car);
            }
        }

        public void RemoveCar(int carId)
        {
            cars.RemoveAll(c => c.id == carId);
        }        

        public void UpdateCar(Car car)
        {
            int index = cars.FindIndex(carList => carList.id == car.id);
            if (index != -1)
            {
                cars[index] = car;
            }
        }
    }
}
