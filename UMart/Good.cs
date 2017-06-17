using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMart {
    abstract class Good : IComparable, ICloneable {

        /// <summary>
        /// Название товара
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Название единицы измерения
        /// </summary>
        public abstract String UnitName { get; }

        /// <summary>
        /// Номер артикула
        /// </summary>
        public long Article { get; set; }

        public Good() {
            Article = 0;
            Name = "";
        }

        public Good(String name) {
            Name = name;
            Article = 0;
        }

        public Good(String name, long article) {
            Name = name;
            Article = article;
        }

        public override bool Equals(object obj) {
            return (obj is Good) && ((obj as Good).Article == Article) && ((obj as Good).Name == Name);
        }

        /// <summary>
        /// Получает итоговую стоимость товара
        /// </summary>
        /// <returns>Стоимость товара в рублях</returns>
        public abstract double GetCost();

        public abstract double GetPrice();

        public int CompareTo(object obj) {
            return (obj is Good) ? Name.CompareTo((obj as Good).Name) : -1;
        }

        public object Clone() {
            return MemberwiseClone();
        }
    }
}
