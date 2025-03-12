using AutoServis.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoServis.Repository
{
    public interface ICarsRepository
    {
        bool dataChange { get; set; }
        List<Car> GetCars();
        Car GetCar(int id);
        void RemoveCar(int id);
        void UpdateCar(Car car);
        void AddCar(Car car);

    }
}
