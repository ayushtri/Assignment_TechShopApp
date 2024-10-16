using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAOLibrary
{
    internal interface IInventoryService
    {
        public int GetInventoryID(int ProductID);
        public bool AddToInventory(int ProductID, int Quantity);
        public bool RemoveFromInventory(int ProductID, int Quantity);
        public bool UpdateStockQuantity(int ProductID, int NewQuantity);
        public bool IsProductAvailable(int ProductID, int QuantityToCheck);
        public int GetInventoryValue(int ProductID);
        public List<Tuple<int, string, int>> ListLowStockProducts(int threshold);
        public List<Tuple<int, string, int>> ListAllProducts();

    }
}
