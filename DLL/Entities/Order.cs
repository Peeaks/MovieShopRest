﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.Entities {
    public class Order : AbstractEntity {
        public virtual List<Movie> Movies { get; set; }
        public Customer Customer { get; set; }
        public DateTime Time { get; set; }
        public PromoCode PromoCode { get; set; }
        public double SubTotalPrice {
            get {
                double price = 0;
                foreach (var movie in Movies) {
                    price += movie.Price;
                }
                return price;
            }
        }
        public double TotalPrice {
            get {
                double price = 0;
                if (this.PromoCode != null && this.PromoCode.Code != null) {
                    foreach (var movie in Movies) {
                        double discount = movie.Price*this.PromoCode.Discount*0.01;
                        price += movie.Price - discount;
                    }
                } else {
                    foreach (var movie in Movies) {
                        price += movie.Price;
                    }
                }
                return price;
            }
        }
    }
}