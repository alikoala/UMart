using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMart {
    class WeightGood : Good {

        /// <summary>
        /// Вес товара в килограммах
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// Цена за килограмм
        /// </summary>
        public double PricePerKG { get; set; }

        public override string UnitName {
            get {
                return "КГ";
            }
        }

        public WeightGood() : base() {
            Weight = 0;
            PricePerKG = 0;
        }

        public WeightGood(String name, long article) : base(name, article) {
            Weight = 0;
            PricePerKG = 0;
        }

        public WeightGood(String name, long article, double weight): base(name, article) {
            Weight = weight;
            PricePerKG = 0;
        }

        public WeightGood(String name, long article, double weight, double pricePerKG) : base(name, article) {
            Weight = weight;
            PricePerKG = pricePerKG;
        }

        /// <summary>
        /// Получает итоговую стоимость товара
        /// </summary>
        /// <returns>Стоимость товара в рублях</returns>
        public override double GetCost() {
            return Weight * PricePerKG;
        }

        public override double GetPrice() {
            return PricePerKG;
        }

        public override string ToString() {
            return String.Format("{0} {1}", Weight, UnitName);
        }
    }
}
