using Final_SqlUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HC_FinalProject
{
    public partial class Customer : Form
    {
        public const string SqlConnStringName = "SqlCustomerConnection";
        public Customer()
        {
            InitializeComponent();
        }

        public void ExitApplication(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void tsb_exit_Click(object sender, EventArgs e)
        {
            ExitApplication(sender, e);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExitApplication(sender, e);
        }

        private void Customer_Load(object sender, EventArgs e)
        {
            LoadCustomers();

        }

        private void LoadCustomers()
        {
            SqlCustomerUtility customerutil = new SqlCustomerUtility(SqlConnStringName);
            List<Final_Core.Customer> customer = customerutil.GetAllCustomers();

            dataGridView1.DataSource = customer;

        }

        private void tsb_Add_Click(object sender, EventArgs e)
        {
            AddForm addCustomerForm = new AddForm();
            addCustomerForm.CustomerAdded += addCustomerForm_CustomerAdded;
            addCustomerForm.ShowDialog();
        }

        private void addCustomerForm_CustomerAdded(object sender, Models.CustomerAddedEventArgs e)
        {
            SqlCustomerUtility customerUtil = new SqlCustomerUtility(SqlConnStringName);
            if (e.Customer == null)
            {
                MessageBox.Show("Customer entry not found!");
                return;
            }
            try
            {
                customerUtil.AddCustomer(e.Customer);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            LoadCustomers();

        }

        private void tsb_delete_Click(object sender, EventArgs e)
        {
            //Find Column
            int IdColumnIndex = -1;
            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                if (col.Name == "CustomerID")
                {
                    IdColumnIndex = col.Index;
                    break;
                }
            }

            //Find Row to Delete
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.ColumnIndex == IdColumnIndex)
                    {
                        //Get Customer
                        int IdofCustomer = (int)cell.Value;

                        //Capture Customer Object
                        SqlCustomerUtility util = new SqlCustomerUtility(SqlConnStringName);
                        Final_Core.Customer customerToDelete = util.GetAllCustomers().FirstOrDefault(x => x.CustomerID == IdofCustomer);
                        
                        //Delete Row
                        util.DeleteCustomer(customerToDelete);
                    }
                }
            }

            //Refresh
            LoadCustomers();
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Capture the row edit
            DataGridViewRow matchingRow=null;
            foreach (DataGridViewRow dgvr in dataGridView1.Rows)
	        {
                if (dgvr.Index == e.RowIndex)
                {
                    matchingRow = dgvr;
                    break;
                }
	        }

            // Pass to Build User
            Final_Core.Customer customerDTO = BuildCustomer(matchingRow);

            //Update User in SQL Utility
            SqlCustomerUtility util = new SqlCustomerUtility(SqlConnStringName);
            util.UpdateCustomer(customerDTO);
        }

        private Final_Core.Customer BuildCustomer(DataGridViewRow row)
        {
            //Map Column Indexes
            Final_Core.Customer newCustomer = new Final_Core.Customer();
            Dictionary<string, int> dgvlookup = new Dictionary<string, int>();

            foreach(DataGridViewColumn col in dataGridView1.Columns)
            {
                dgvlookup.Add(col.Name, col.Index);

            }

            foreach(DataGridViewCell cell in row.Cells)
            {
                if (cell.ColumnIndex == dgvlookup["CustomerID"])
                {

                    newCustomer.CustomerID = (int)cell.Value;
                }

                if (cell.ColumnIndex == dgvlookup["Title"])
                {

                    newCustomer.Title = (string)cell.Value;
                }
                
                if(cell.ColumnIndex==dgvlookup["FirstName"])
                {

                    newCustomer.FirstName = (string)cell.Value;
                }

                if (cell.ColumnIndex == dgvlookup["MiddleName"])
                {

                    newCustomer.MiddleName = (string)cell.Value;
                }

                if (cell.ColumnIndex == dgvlookup["LastName"])
                {

                    newCustomer.LastName = (string)cell.Value;
                }

                if (cell.ColumnIndex == dgvlookup["Suffix"])
                {

                    newCustomer.Suffix = (string)cell.Value;
                }

                if (cell.ColumnIndex == dgvlookup["CompanyName"])
                {

                    newCustomer.CompanyName = (string)cell.Value;
                }

                if (cell.ColumnIndex == dgvlookup["SalesPerson"])
                {

                    newCustomer.SalesPerson = (string)cell.Value;
                }

                if (cell.ColumnIndex == dgvlookup["EmailAddress"])
                {

                    newCustomer.EmailAddress = (string)cell.Value;
                }

                if (cell.ColumnIndex == dgvlookup["Phone"])
                {

                    newCustomer.Phone = (string)cell.Value;
                }

                if (cell.ColumnIndex == dgvlookup["PasswordHash"])
                {

                    newCustomer.PasswordHash = (string)cell.Value;
                }

                if (cell.ColumnIndex == dgvlookup["PasswordSalt"])
                {

                    newCustomer.PasswordSalt = (string)cell.Value;
                }

                if (cell.ColumnIndex == dgvlookup["ModifiedDate"])
                {

                    newCustomer.ModifiedDate = (DateTime)cell.Value;
                }

                if (cell.ColumnIndex == dgvlookup["rowguid"])
                {

                    newCustomer.rowguid = (Guid)cell.Value;
                }
            }

            return newCustomer;


        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btn_ViewOrders_Click(object sender, EventArgs e)
        {
            DataGridViewRow matchingRow = null;

            int customerID;
            int SalesOrderID;
            int ProductID;
            foreach (DataGridViewRow dgvr in dataGridView1.Rows)
            {
                if (dgvr.Index == dataGridView1.CurrentCell.RowIndex)
                {
                    matchingRow = dgvr;
                    break;
                }
            }

            if (dataGridView1.Columns[0].Name== "CustomerID")
            {
                customerID = GetCustomerID(matchingRow);

                LoadOrdersForCustomer(customerID);

            }
            else if (dataGridView1.Columns[2].Name == "OrderDate")
            {

                SalesOrderID = GetSalesOrderID(matchingRow);
                LoadSalesOrderDetails(SalesOrderID);

            }
            else if (dataGridView1.Columns[3].Name == "ProductID")
            {

                ProductID = GetProductID(matchingRow);
                LoadProductDetails(ProductID);

            }
      
        }


        private int GetProductID(DataGridViewRow row)
        {
            Dictionary<string, int> dgvlookup = new Dictionary<string, int>();
            int ProductID = 0;
            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                dgvlookup.Add(col.Name, col.Index);

            }

            foreach (DataGridViewCell cell in row.Cells)
            {
                if (cell.ColumnIndex == dgvlookup["ProductID"])
                {

                    ProductID = (int)cell.Value;
                }

            }

            return ProductID;
        }

        private void LoadProductDetails(int ProductID)
        {
            SqlCustomerUtility customerutil = new SqlCustomerUtility(SqlConnStringName);
            List<Final_Core.Product> Productdetails = customerutil.GetProductDetails(ProductID);

            dataGridView1.DataSource = Productdetails;
        }

        private void LoadSalesOrderDetails(int SalesOrderID)
        {
            SqlCustomerUtility customerutil = new SqlCustomerUtility(SqlConnStringName);
            List<Final_Core.OrderDetails> salesorderdetails = customerutil.GetOrderDetails(SalesOrderID);

            dataGridView1.DataSource = salesorderdetails;
        }

        private int GetSalesOrderID(DataGridViewRow row)
        {
            Dictionary<string, int> dgvlookup = new Dictionary<string, int>();
            int SalesOrderID = 0;
            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                dgvlookup.Add(col.Name, col.Index);

            }

            foreach (DataGridViewCell cell in row.Cells)
            {
                if (cell.ColumnIndex == dgvlookup["SalesOrderID"])
                {

                    SalesOrderID = (int)cell.Value;
                }

            }

            return SalesOrderID;
        }

        private void LoadOrdersForCustomer(int CustomerID)
        {
            SqlCustomerUtility customerutil = new SqlCustomerUtility(SqlConnStringName);
            List<Final_Core.Orders> orders = customerutil.GetAllOrdersforCustomer(CustomerID);

            dataGridView1.DataSource = orders;

        }

        private int GetCustomerID( DataGridViewRow row)
        {
            Dictionary<string, int> dgvlookup = new Dictionary<string, int>();
            int CustomerID=0;
            foreach(DataGridViewColumn col in dataGridView1.Columns)
            {
                dgvlookup.Add(col.Name, col.Index);

            }

            foreach (DataGridViewCell cell in row.Cells)
            {
                if (cell.ColumnIndex == dgvlookup["CustomerID"])
                {

                    CustomerID = (int)cell.Value;
                }

            }

            return CustomerID;
        }
        
    }
}
