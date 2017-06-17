using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMart {
    class PieceGood : Good {

        /// <summary>
        /// Количество штук
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Цена за штуку
        /// </summary>
        public double PricePerPiece { get; set; }

        public override string UnitName {
            get {
                return "Шт";
            }
        }

        public PieceGood() : base() {
            Count = 0;
            PricePerPiece = 0;
        }

        public PieceGood(String name, long article) : base(name, article) {
            Count = 0;
            PricePerPiece = 0;
        }

        public PieceGood(String name, long article, int count): base(name, article) {
            Count = count;
            PricePerPiece = 0;
        }

        public PieceGood(String name, long article, int count, double pricePerPiece) : base(name, article) {
            Count = count;
            PricePerPiece = pricePerPiece;
        }

        public override double GetCost() {
            return Count * PricePerPiece;
        }

        public override string ToString() {
            return String.Format("{0} {1}", Count, UnitName);
        }

        public override double GetPrice() {
            return PricePerPiece;
        }
    }
}
