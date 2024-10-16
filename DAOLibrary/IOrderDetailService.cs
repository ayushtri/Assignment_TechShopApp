using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAOLibrary
{
    internal interface IOrderDetailService
    {
        public decimal CalculateSubtotal(int OrderDetailID);
        public Tuple<int, int, int, int> GetOrderDetailInfo(int OrderDetailID);
        public bool UpdateQuantity(int OrderDetailID, int NewQuanity);
        public bool AddDiscount(int OrderDetailID, int DiscountPercent);
        public int GetOrderDetailID(int OrderID);
    }
}
