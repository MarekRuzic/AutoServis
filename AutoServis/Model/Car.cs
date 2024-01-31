using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoServis.Model
{
    internal class Car
    {
        public int id { get; set; }
        public string brand { get; set; }
        public string model { get; set; }
        public DateTime manufacture { get; set; }
        public double mileage { get; set; }
        public string fuel { get; set; }
        public string body { get; set; }
        public string color { get; set; }
        public bool drive4x4 { get; set; }
        public char doors { get; set; }
        public char seats { get; set; }
        public bool aircondition { get; set; }
        public string vin { get; set; }
        public string spz { get; set; }
        public string nickname { get; set; }
        public string name_engine { get; set; }
        public string code { get; set; }
        public string displacement { get; set; }
        public string power { get; set; }
        public string torque { get; set; }
        public string oil_capacity { get; set; }
        public string transmition { get; set; }
        public int user_id { get; set; }

        public Car(int id, string brand, string model, DateTime manufacture, double mileage, string fuel, string body, string color, bool drive4x4, char doors, char seats, bool aircondition, string vin, string spz, string nickname, string name_engine, string code, string displacement, string power, string torque, string oil_capacity, string transmition, int user_id)
        {
            this.id = id;
            this.brand = brand;
            this.model = model;
            this.manufacture = manufacture;
            this.mileage = mileage;
            this.fuel = fuel;
            this.body = body;
            this.color = color;
            this.drive4x4 = drive4x4;
            this.doors = doors;
            this.seats = seats;
            this.aircondition = aircondition;
            this.vin = vin;
            this.spz = spz;
            this.nickname = nickname;
            this.name_engine = name_engine;
            this.code = code;
            this.displacement = displacement;
            this.power = power;
            this.torque = torque;
            this.oil_capacity = oil_capacity;
            this.transmition = transmition;
            this.user_id = user_id;
        }
    }
}
