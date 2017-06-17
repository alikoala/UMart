using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UMart {
    public partial class Form1 : Form {

        private List<Good> goods;
        private List<Good> market;

        public Form1() {
            InitializeComponent();
            JsonSerializerSettings settings = new JsonSerializerSettings {
                TypeNameHandling = TypeNameHandling.All
            };
            
            goods = JsonConvert.DeserializeObject<List<Good>>(System.IO.File.ReadAllText("goods.json"), settings);

            initGoods();
            initMarket();
            
        }

        private void initGoods() {
            foreach (Good g in goods) {
                ListViewItem item = new ListViewItem(g.Article.ToString());
                item.SubItems.Add(g.Name);
                if (g is WeightGood)
                    item.SubItems.Add((g as WeightGood).PricePerKG.ToString());
                else if (g is PieceGood)
                    item.SubItems.Add((g as PieceGood).PricePerPiece.ToString());
                listView1.Items.Add(item);
            }
        }

        private void initMarket() {
            market = new List<Good>();
            ListViewItem item = new ListViewItem("");
            item.SubItems.Add("Итого");
            item.SubItems.Add("");
            item.SubItems.Add("0");
            item.SubItems.Add("0");
            listView2.Items.Add(item);
        }



        /// <summary>
        /// Удаление из корзины
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e) {
            if (listView2.SelectedItems.Count == 0)
                return;
            for (int i = 0; i < listView2.SelectedItems.Count; i++) {
                int index = listView2.SelectedItems[i].Index;
                if (index >= market.Count)
                    continue;
                Good good = market[index];
                if (good is PieceGood) {
                    (good as PieceGood).Count--;
                } else if (good is WeightGood) {
                    (good as WeightGood).Weight -= 1;
                }
                if (good.GetCost() <= 0) {
                    button3_Click(sender, e);
                } else {
                    updateMarketItem(index);
                    updateSum();
                }
            }
        }

        private void updateMarketItem(int index) {
            listView2.Items[index].SubItems[3].Text = market[index].ToString();
            listView2.Items[index].SubItems[4].Text = market[index].GetCost().ToString();
        }

        /// <summary>
        /// Добавление в корзину
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e) {
            if (listView1.SelectedItems.Count == 0)
                return;
            for (int i = 0; i < listView1.SelectedItems.Count; i++) {
                Good g = goods[listView1.SelectedItems[i].Index];
                if (market.Contains(g)) {
                    Good goodInMarket = market[market.IndexOf(g)];
                    if (goodInMarket is WeightGood) {
                        ((WeightGood)goodInMarket).Weight +=1;
                    } else if (goodInMarket is PieceGood) {
                        ((PieceGood)goodInMarket).Count++;
                    }
                    updateMarketItem(market.IndexOf(g));
                } else {
                    addMarketItem(g);
                }
            }
            updateSum();
        }

        private void updateSum() {
            double sumCost = 0;
            foreach (Good g in market) {
                sumCost += g.GetCost();
            }
            listView2.Items[listView2.Items.Count - 1].SubItems[4].Text = sumCost.ToString();
            listView2.Items[listView2.Items.Count - 1].SubItems[3].Text = market.Count.ToString();

        }

        private void addMarketItem(Good g) {
            Good good = (Good)g.Clone();
            if (good is WeightGood) {
                ((WeightGood)good).Weight = 1;
            } else if (good is PieceGood) {
                ((PieceGood)good).Count = 1;
            }
            market.Add(good);
            ListViewItem item = new ListViewItem(good.Article.ToString());
            item.SubItems.Add(good.Name);
            item.SubItems.Add(good.GetPrice().ToString());
            item.SubItems.Add(good.ToString());
            item.SubItems.Add(good.GetCost().ToString());
            listView2.Items.Insert(listView2.Items.Count-1, item);


        }

        private void button3_Click(object sender, EventArgs e) {
            if (listView2.SelectedItems.Count == 0)
                return;
            for (int i = 0; i < listView2.SelectedItems.Count; i++) {
                int index = listView2.SelectedItems[i].Index;
                if (index >= market.Count)
                    continue;
                listView2.Items.RemoveAt(index);
                market.RemoveAt(index);
                updateSum();
                if (listView2.Items.Count > 1 && index < listView2.Items.Count - 1) {
                    listView2.Items[index].Selected = true;
                }
            } 
        }

        private void listView2_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Delete) {
                button3_Click(sender, null);
            } else if (e.KeyCode == Keys.Back) {
                button2_Click(sender, null);
            }
        }

        private void listView1_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                button1_Click(sender, null);
            }
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e) {
            if (listView2.SelectedItems.Count != 1 || listView2.SelectedItems[0].Index == listView2.Items.Count - 1)
                return;

            Good g = market[listView2.SelectedItems[0].Index];
            if (g is WeightGood) {
                numericUpDown1.Value = (decimal)(g as WeightGood).Weight;
                numericUpDown1.DecimalPlaces = 3;
                numericUpDown1.Increment = 0.1M;
                numericUpDown1.Minimum = 0.001M;
                label3.Text = "Масса";
            } else if (g is PieceGood) {
                numericUpDown1.Value = (g as PieceGood).Count;
                numericUpDown1.DecimalPlaces = 0;
                numericUpDown1.Increment = 1M;
                numericUpDown1.Minimum = 1;
                label3.Text = "Количество";
            }            
        }

        private void button4_Click(object sender, EventArgs e) {
            if (listView2.SelectedItems.Count != 1 || listView2.SelectedItems[0].Index == listView2.Items.Count - 1)
                return;

            Good g = market[listView2.SelectedItems[0].Index];
            if (g is WeightGood)
                (g as WeightGood).Weight = (double)numericUpDown1.Value;
            else if (g is PieceGood) {
                (g as PieceGood).Count = (int)numericUpDown1.Value;
            }

                listView2.SelectedItems[0].SubItems[3].Text = g.ToString();

            updateMarketItem(listView2.SelectedItems[0].Index);
            updateSum();
            
        }
    }
}
