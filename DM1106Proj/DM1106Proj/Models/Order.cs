using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DM1106Proj.Models
{
    public class Order
    {
        public Order()
        {
            this.OrderItems = new HashSet<OrderItem>();
        }
        public int Id { get; set; }
        public string userEmail { get; set; }
        public DateTime deliveryDate { get; set; }
        public DateTime creationDate { get; set; }
        public string status { get; set; }

        public decimal totalPrice { get; set; }
        public decimal freightRate { get; set; }
        public float totalWeight { get; set; }
        public virtual ICollection<OrderItem> OrderItems
        {
            get;
            set;
        }
    }
}