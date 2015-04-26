using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC_FinalProject.Models
{
    public class CustomerAddedEventArgs : EventArgs
    {
        public CustomerAddedEventArgs(Final_Core.Customer newCustomer)
        {
            Customer = newCustomer;
        }

        public Final_Core.Customer Customer { get; set; }
    }
}
