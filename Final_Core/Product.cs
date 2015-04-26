using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Core
{
    public class Product
    {
        public int ProductID { get; set; }

        public string Name { get; set; }

        public string ProductNumber { get; set; }

        public string Color { get; set; }//null

        public decimal StandardCost { get; set; }

        public decimal ListPrice { get; set; }

        public string Size { get; set; }//null

        public decimal Weight { get; set; }//null

        public int ProductCategoryID { get; set; }//null

        public int ProductModeID { get; set; }//null

        public DateTime SellStartDate { get; set; }

        public DateTime SellEndDate { get; set; }//Null

        public DateTime DiscontinuedDate { get; set; }//NUll

        ////public Byte ThumbNailPhoto { get; set; }

        public string ThumbNailPhotoFileName { get; set; }

        public Guid rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }
        
    }
}
