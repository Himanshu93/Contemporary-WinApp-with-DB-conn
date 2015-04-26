using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Core
{
    public class Customer
    {
        /*
        [CustomerID] ,[NameStyle],[Title],[FirstName],[MiddleName],[LastName]
        ,[Suffix],[CompanyName],[SalesPerson],[EmailAddress],[Phone],[PasswordHash]
        ,[PasswordSalt],[rowguid],[ModifiedDate]
        */
        public int CustomerID { get; set; }

        public string Title { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Suffix { get; set; }

        public string CompanyName { get; set; }

        public string SalesPerson { get; set; }

        public string EmailAddress { get; set; }

        public string Phone { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }

        public Guid rowguid { get; set; }
        
        //public string Password { get; set; }

    }
}
