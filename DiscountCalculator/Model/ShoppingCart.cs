using System.Collections.Generic;
using System.Linq;

namespace Calculator
{
    public class ShoppingCart
    {
        /// <summary>
        /// Customer linked to this Shopping Cart
        /// </summary>
        public Customer Customer { get; set; }

        /// <summary>
        /// Items in the Shopping Cart
        /// </summary>
        public List<Item> Items { get; set; }



        /// <summary>
        /// Return the Grand Total of all the Items in the Shopping Cart
        /// </summary>
        /// <returns>Grand Total</returns>
        public decimal GetSubTotal( )
        {
            return Items.Sum( el => el.Value );
        }


        /// <summary>
        /// Return the Total of the Items in the Shopping Cart that are not Groceries
        /// </summary>
        /// <returns>Total without Groceries</returns>
        public decimal GetTotalWithoutGroceries( )
        {
            return Items.Where( el => el.ItemType != ItemType.Groceries ).Sum( el => el.Value );
        }

    }
}
