using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Core
{
    public class Orders
    {
        /*
       [SalesOrderID] ,[RevisionNumber],[OrderDate],[DueDate],[ShipDate],[Status],[OnlineOrderFlag],[SalesOrderNumber],[PurchaseOrderNumber],[AccountNumber]
      ,[CustomerID],[ShipToAddressID],[BillToAddressID],[ShipMethod],[CreditCardApprovalCode],[SubTotal],[TaxAmt],[Freight],[TotalDue],[Comment],[rowguid]
      ,[ModifiedDate]
       */

        public int SalesOrderID { get; set; }
        public int RevisionNumber { get; set; }//tinyint
        public DateTime OrderDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime ShipDate { get; set; } //null
        public int Status { get; set; } //tinyint
        public bool OnlineOrderFlag { get; set; }
        public string SalesOrderNumber { get; set; }
        public string PurchaseOrderNumber { get; set; } //null
        public string AccountNumber { get; set; } //null
        public int CustomerID_Order { get; set; }
        public int ShipToAddressID { get; set; } //null
        public int BillToAddressID { get; set; } //null
        public string ShipMethod { get; set; }
        public string CreditCardApprovalCode { get; set; } //null
        public decimal SubTotal { get; set; }
        public decimal TaxAmt { get; set; }
        public decimal Freight { get; set; }
        public decimal TotalDue { get; set; }
        public string Comment { get; set; } //null
        public Guid rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }




    }
}
