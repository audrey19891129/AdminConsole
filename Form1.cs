using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web.Helpers;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Ajax.Utilities;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading;

namespace ConsoleAdmin
{
    public partial class AdminConsole : Form
    {
        public AdminConsole()
        {
            InitializeComponent();
        }

        List<Product> ProductsList;
        List<Sale> SalesList;

        private HttpClient getClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44333/BrookeAndCoWebService.asmx/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }


        private void pRODUCTSToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (pnlLogIn.Visible == false)
            {
                pnlTopTen.Visible = false;
                pnlLogIn.Visible = false;
                pnlSales.Visible = false;
                pnlProducts.Visible = true;
                pnlProducts.BringToFront();
                ProductsTable.BackgroundColor = Color.WhiteSmoke;

                HttpClient client = getClient();
                string url = "getProducts";
                var response = client.GetAsync(url);
                var results = response.Result;
                if (results.IsSuccessStatusCode)
                {
                    var textResponse = results.Content.ReadAsStringAsync();
                    List<Product> list = JsonSerializer.Deserialize<List<Product>>(textResponse.Result);
                    ProductsTable.DataSource = null;
                    ProductsTable.AutoGenerateColumns = false;
                    ProductsTable.ColumnCount = 5;

                    ProductsTable.Columns[0].Name = "pcode";
                    ProductsTable.Columns[0].HeaderText = "Product code";
                    ProductsTable.Columns[0].DataPropertyName = "pcode";
                    ProductsTable.Columns[0].Width = 200;

                    ProductsTable.Columns[1].HeaderText = "Type";
                    ProductsTable.Columns[1].Name = "type";
                    ProductsTable.Columns[1].DataPropertyName = "type";
                    ProductsTable.Columns[1].Width = 100;

                    ProductsTable.Columns[2].Name = "title";
                    ProductsTable.Columns[2].HeaderText = "Product title";
                    ProductsTable.Columns[2].DataPropertyName = "title";
                    ProductsTable.Columns[2].Width = 650;

                    ProductsTable.Columns[3].Name = "price";
                    ProductsTable.Columns[3].HeaderText = "Current Price";
                    ProductsTable.Columns[3].DataPropertyName = "price";

                    ProductsTable.Columns[4].Name = "inventory";
                    ProductsTable.Columns[4].HeaderText = "Sold";
                    ProductsTable.Columns[4].DataPropertyName = "left";

                    ProductsTable.DataSource = list;
                }
                else
                {
                    Console.WriteLine("error");
                }
            }
        }
   
        public class Product
        {
            public string pcode { get; set; }
            public string type { get; set; }
            public string title { get; set; }
            public string category { get; set; }
            public string genre { get; set; }
            public double price { get; set; }
            public string picture { get; set; }
            public int bought { get; set; }
            public int left { get; set; }
            public int warehouse { get; set; }

            public Product(string pcode, string type, string title, string category, string genre, double price, string picture, int bought, int left, int warehouse)
            {
                this.pcode = pcode;
                this.picture = picture;
                this.type = type;
                this.title = title;
                this.category = category;
                this.genre = genre;
                this.price = price;
                this.bought = bought;
                this.left = left;
                this.warehouse = warehouse;
            }
        }
        public class Employee
        {
            public string title { get; set; }
            public string username { get; set; }
            public string password { get; set; }
            public string firstname { get; set; }
            public string lastname { get; set; }
            public Employee() { }
        }

        private void tOPTENToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pnlLogIn.Visible == false)
            {
                pnlProducts.Visible = false;
                pnlLogIn.Visible = false;
                pnlSales.Visible = true;
                pnlTopTen.Visible = true;
                pnlTopTen.BringToFront();

                HttpClient client = getClient();
                string url = "getTopTen";
                var response = client.GetAsync(url);
                var results = response.Result;
                if (results.IsSuccessStatusCode)
                {
                    var textResponse = results.Content.ReadAsStringAsync();
                    List<Product> list = JsonSerializer.Deserialize<List<Product>>(textResponse.Result);
                    ProductsList = list;
                    System.Collections.IList list1 = list;
                    for (int i = 0; i < list1.Count; i++)
                    {
                        Product pr = (Product)list1[i];
                        lvTopTen.Items.Add(new ListViewItem(new string[] { (i + 1).ToString(), pr.pcode, (pr.left).ToString() }));
                    }
                    lvTopTen.Items[0].Selected = true;
                    setFieldsInfos(0);
                }
                else
                {
                    Console.WriteLine("error");
                }
            }
        }

        public void setFieldsInfos(int index)
        {
            Product product = ProductsList[index];
            tbTitle.Text = product.title;
            tbType.Text = product.type;
            tbGen.Text = product.genre;
            tbCat.Text = product.category;
            tbWh.Text = product.warehouse.ToString();
            tbSold.Text = product.left.ToString();
            tbBought.Text = product.bought.ToString();
            ProductImage.Load(product.picture);
            tbPromo.Text = "UNAVAILABLE";
        }



        private void ProductsTable_CellContentClick(object sender, DataGridViewCellEventArgs e){}

        private void lvTopTen_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = 0;
            if (lvTopTen.SelectedItems.Count > 0)
                index = lvTopTen.SelectedIndices[0];
            setFieldsInfos(index);
        }
        public class Sale
        {
            public string pcode { get; set; }
            public string type { get; set; }
            public string title { get; set; }
            public string category { get; set; }
            public string picture { get; set; }
            public string genre { get; set; }
            public int quantity { get; set; }
            public double price_bought { get; set; }
            public double price_sold { get; set; }
            public string order_date { get; set; }
            public double profits { get; set; }

            public Sale(string pcode, string type, string title, string category, string picture, string genre, int quantity, double price_bought, double price_sold, string order_date, double profits)
            {
                this.pcode = pcode;
                this.type = type;
                this.title = title;
                this.category = category;
                this.genre = genre;
                this.quantity = quantity;
                this.order_date = order_date;
                this.picture = picture;
                this.price_bought = Math.Round(price_bought, 2);
                this.price_sold = Math.Round(price_sold, 2);
                this.profits = Math.Round(profits, 2);
            }
        }

        private void getSalesList()
        {
            HttpClient client = getClient();
            string url = "getSalesList";
            var response = client.GetAsync(url);
            var results = response.Result;

            if (results.IsSuccessStatusCode)
            {
                var textResponse = results.Content.ReadAsStringAsync();
                List<Sale> list = JsonSerializer.Deserialize<List<Sale>>(textResponse.Result);
                SalesList = list;
            }
        }


        private List<Sale> getListByDate()
        {
            DateTime date = dateTimePicker1.Value;
            string d = date.ToString("yyyy-MM-dd");
            List<Sale> list = new List<Sale>();

            foreach (Sale sale in SalesList)
            {
                if (sale.order_date == d)
                    list.Add(sale);
            }
            return list;
        }

        private List<Sale> getListByInterval()
        {
            DateTime d1 = date1.Value.Date;
            DateTime d2 = date2.Value.Date;
            List<Sale> list = new List<Sale>();

            foreach (Sale sale in SalesList)
            {
                DateTime Date = DateTime.Parse(sale.order_date);
                if (Date >= d1 & Date <= d2)
                    list.Add(sale);
                else if(Date >= d2 & Date <= d1)
                    list.Add(sale);
            }
            return list;
        }

        public double calculateSalesTotal(List<Sale> list)
        {
            double sales = 0;
            foreach (Sale sale in list)
            {
                sales += sale.price_sold;
            }
            return sales;
        }

        public double calculateProfitsTotal(List<Sale> list)
        {
            double profits = 0;
            foreach (Sale sale in list)
            {
                profits += sale.profits;
            }
            return profits;
        }

        public int calculateProductsTotal(List<Sale> list)
        {
            int sales = 0;
            foreach (Sale sale in list)
            {
                sales += sale.quantity;
            }
            return sales;
        }

        private List<Sale> getListByMonth()
        {
            int month = cbMonths.SelectedIndex + 1;
            List<Sale> list = new List<Sale>();

            foreach (Sale sale in SalesList)
            {
                DateTime Date = DateTime.Parse(sale.order_date);
                if (Date.Month == month)
                    list.Add(sale);
            }
            return list;
        }

        private void dAYLYToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pnlLogIn.Visible == false)
            {
                pnlProducts.Visible = false;
                pnlTopTen.Visible = false;

                gbMonthly.Visible = false;
                gbInterval.Visible = false;
                gbDaily.BringToFront();
                gbDaily.Visible = true;
                pnlSales.BringToFront();
                pnlSales.Visible = true;
                getSalesList();
                SalesTable.DataSource = null;

                List<Sale> list = getListByDate();
                if (list.Count > 0)
                {
                    ShowResult(list);
                    double sales = calculateSalesTotal(list);
                    tbMoney.Text = sales.ToString() + " $  CAD";
                    int prod = calculateProductsTotal(list);
                    tbProdQ.Text = prod.ToString();
                    double profits = calculateProfitsTotal(list);
                    tbProfits.Text = profits.ToString() + " $  CAD";
                }
                else
                    MessageBox.Show("There are no sales on this date.");
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            List<Sale> list = getListByDate();
            ShowResult(list);
            double sales = calculateSalesTotal(list);
            tbMoney.Text = sales.ToString() + " $  CAD";
            int prod = calculateProductsTotal(list);
            tbProdQ.Text = prod.ToString();
            double profits = calculateProfitsTotal(list);
            tbProfits.Text = profits.ToString() + " $  CAD";
            if (list.Count == 0)
            {
                DateTime date = dateTimePicker1.Value;
                string d = date.ToString("yyyy-MM-dd");
                MessageBox.Show("There are no sales on the date of : " + d);
            }  
        }

        private void wEEKLYToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pnlLogIn.Visible == false)
            {
                pnlProducts.Visible = false;
                pnlTopTen.Visible = false;
                pnlLogIn.Visible = false;
                pnlSales.Visible = true;
                pnlSales.BringToFront();
                gbDaily.Visible = false;
                gbMonthly.Visible = false;
                gbInterval.Visible = true;
                gbInterval.BringToFront();
                getSalesList();
                SalesTable.DataSource = null;
                List<Sale> list = getListByInterval();
                setTable(list);
            }
        }
        private void setTable(List<Sale> list)
        {
            ShowResult(list);
            double sales = calculateSalesTotal(list);
            tbMoney.Text = sales.ToString() + " $  CAD";
            int prod = calculateProductsTotal(list);
            tbProdQ.Text = prod.ToString();
            double profits = calculateProfitsTotal(list);
            tbProfits.Text = profits.ToString() + " $  CAD";
            if (list.Count == 0)
                SalesTable.DataSource = "There are no sales on that day.";
        }

        private void ShowResult(List<Sale> list)
        {
            SalesTable.DataSource = null;
            SalesTable.AutoGenerateColumns = false;
            SalesTable.ColumnCount = 8;

            SalesTable.Columns[0].Name = "pcode";
            SalesTable.Columns[0].HeaderText = "Product code";
            SalesTable.Columns[0].DataPropertyName = "pcode";
            SalesTable.Columns[0].Width = 200;

            SalesTable.Columns[1].HeaderText = "Type";
            SalesTable.Columns[1].Name = "type";
            SalesTable.Columns[1].DataPropertyName = "type";
            SalesTable.Columns[1].Width = 100;

            SalesTable.Columns[2].Name = "quant";
            SalesTable.Columns[2].HeaderText = "Quantity";
            SalesTable.Columns[2].DataPropertyName = "quantity";

            SalesTable.Columns[3].Name = "price_b";
            SalesTable.Columns[3].HeaderText = "Price Bought";
            SalesTable.Columns[3].DataPropertyName = "price_bought";

            SalesTable.Columns[4].Name = "price_s";
            SalesTable.Columns[4].HeaderText = "Price Sold";
            SalesTable.Columns[4].DataPropertyName = "price_sold";

            SalesTable.Columns[5].Name = "diff";
            SalesTable.Columns[5].HeaderText = "Profits";
            SalesTable.Columns[5].DataPropertyName = "profits";

            SalesTable.Columns[6].Name = "diff";
            SalesTable.Columns[6].HeaderText = "Profits";
            SalesTable.Columns[6].DataPropertyName = "profits";

            SalesTable.Columns[7].Name = "date";
            SalesTable.Columns[7].HeaderText = "Sold on";
            SalesTable.Columns[7].DataPropertyName = "order_date";

            SalesTable.DataSource = list;
            if(list.Count > 0)
            {
                SalesTable.Rows[0].Selected = true;
                setGbSalesInfos(0);
            }
            
        }

        private void mONTHLYToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (pnlLogIn.Visible == false)
            {
                pnlProducts.Visible = false;
                pnlTopTen.Visible = false;
                pnlLogIn.Visible = false;
                pnlSales.Visible = true;
                pnlSales.BringToFront();
                gbDaily.Visible = false;
                gbInterval.Visible = false;
                gbMonthly.Visible = true;
                gbMonthly.BringToFront();
                getSalesList();
                SalesTable.DataSource = null;
                cbMonths.SelectedIndex = 0;
                List<Sale> list = getListByMonth();
                setTable(list);
            }
        }

        private void cbMonths_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Sale> list = getListByMonth();
            setTable(list);
        }

        private void date1_ValueChanged(object sender, EventArgs e)
        {
            List<Sale> list = getListByInterval();
            setTable(list);
        }

        private void date2_ValueChanged(object sender, EventArgs e)
        {
            List<Sale> list = getListByInterval();
            setTable(list);
        }

        private void SalesTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = SalesTable.CurrentCell.RowIndex;
            setGbSalesInfos(index);
        }
        private void setGbSalesInfos(int index)
        {
            Sale sale = SalesList[index];
            pBSales.Load(sale.picture);
            pBSales.SizeMode = PictureBoxSizeMode.StretchImage;
            lblCategory.Text = "Category: " + sale.category;
            lblTitle.Text = sale.title;
            lblGenre.Text = "Genre: " + sale.genre;
        }

        private void pnlProducts_Paint(object sender, PaintEventArgs e) {}

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            asyncResponse();
        }

        private async Task asyncResponse()
        {
            HttpClient client = getClient();

            string username = tbUsername.Text;
            string password = tbPassword.Text;
            var parameters = new Dictionary<string, string> { { "username", username }, { "password", password } };
            var encodedContent = new FormUrlEncodedContent(parameters);

            var url = "getEmployee";
            var response = await client.PostAsync(url, encodedContent).ConfigureAwait(false);
            
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var textResponse = response.Content.ReadAsStringAsync();
                Employee employee = JsonSerializer.Deserialize<Employee>(textResponse.Result);
                if (!(employee.username == null & employee.password == null))
                {
                    if ((employee.title).Equals("administrator"))
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            pnlLogIn.Visible = false;
                            ProductsTable.BackgroundColor = SystemColors.ControlDarkDark;
                            MessageBox.Show("Welcome " + employee.firstname.ToString() + " "  + employee.lastname.ToString());
                        });
                    }
                    else
                        MessageBox.Show("Sorry, you are not authorized to use this program.");
                }
                else
                {
                    MessageBox.Show("This username and password combination is not valid.");
                }
            }
        }

        private void sIGNOUTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlLogIn.Visible = true;
            ProductsTable.BackgroundColor = SystemColors.ControlDarkDark;
            pnlProducts.BringToFront();
            tbUsername.Text = "";
            tbPassword.Text = "";
        }
    }
}
