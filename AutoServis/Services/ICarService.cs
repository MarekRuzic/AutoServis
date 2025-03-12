using AutoServis.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoServis.Services
{
    public interface ICarService
    {
        public Task<List<Car>> LoadUserCars(int userId, API api);
        public Task<bool> DeleteCar(int carId);
        public Task<bool> UpdateCar(Car car);
        public Task<bool> InsertNewCar(Car car);
    }
}
