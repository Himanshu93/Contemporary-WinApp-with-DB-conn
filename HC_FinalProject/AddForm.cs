using HC_FinalProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Final_Core;

namespace HC_FinalProject
{
    public partial class AddForm : Form
    {
        public event EventHandler<CustomerAddedEventArgs> CustomerAdded;
        public AddForm()
        {
            InitializeComponent();
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            //Validation
            if (string.IsNullOrEmpty(tbx_FirstName.Text))
            {
                MessageBox.Show("First Name is Required!");
                this.Close();
            }

            if (string.IsNullOrEmpty(tbx_LastName.Text))
            {
                MessageBox.Show("Last Name is Required!");
                this.Close();
            }

            if (string.IsNullOrEmpty(tbx_CompanyName.Text))
            {
                MessageBox.Show("Company Name is Required!");
                this.Close();
            }

            if (string.IsNullOrEmpty(tbx_SalesPerson.Text))
            {
                MessageBox.Show("Name of Sales Person is Required!");
                this.Close();
            }

            if (string.IsNullOrEmpty(tbx_EmailAddress.Text))
            {
                MessageBox.Show("Email Address is Required!");
                this.Close();
            }

            if (string.IsNullOrEmpty(tbx_Phone.Text))
            {
                MessageBox.Show("Phone Number is Required!");
                this.Close();
            }

            //Pack into Customer Object
            Final_Core.Customer newCustomer = new Final_Core.Customer()
            {
                Title = tbx_Title.Text,
                FirstName = tbx_FirstName.Text,
                LastName = tbx_LastName.Text,
                MiddleName = tbx_MiddleName.Text,
                Suffix = tbx_Suffix.Text,
                CompanyName = tbx_CompanyName.Text,
                SalesPerson = tbx_SalesPerson.Text,
                EmailAddress = tbx_EmailAddress.Text,
                Phone = tbx_Phone.Text,
            };

            //Raise the event
            if(CustomerAdded != null)
            {
                CustomerAdded(this, new CustomerAddedEventArgs(newCustomer));
            }

            this.Close();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
