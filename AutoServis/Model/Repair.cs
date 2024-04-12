using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoServis.Model
{
    public class Repair
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime date { get; set; }
        public double mileage { get; set; }
        public string description { get; set; }
        public string price { get; set; }
        public string part_name {  get; set; }
        public string url {  get; set; }
        public int car_id {  get; set; }

        public Repair(int id, string name, DateTime date, double mileage, string description, string price, string part_name, string url, int car_id)
        {
            this.id = id;
            this.name = name;
            this.date = date;
            this.mileage = mileage;
            this.description = description;
            this.price = price;
            this.part_name = part_name;
            this.url = url;
            this.car_id = car_id;
        }
    }
}
