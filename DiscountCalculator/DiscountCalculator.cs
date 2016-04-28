namespace Calculator
{
    public class DiscountCalculator
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public DiscountCalculator( )
        {
        }


        /// <summary>
        /// Calculate the Discount the Customer gets and return the owed amount
        /// Discount Rules:
        ///   1. If the user is an employee of the store, he gets a 30% discount 
        ///   2. If the user is an affiliate of the store, he gets a 10% discount 
        ///   3. If the user has been a customer for over 2 years, he gets a 5% discount. 
        ///   4. For every $100 on the bill, there would be a $5 discount
        ///   5. The percentage based discounts do not apply on groceries. 
        ///   6. A user can get only one of the percentage based discounts on a bill.
        /// </summary>
        /// <param name="shoppingCart">Shopping cart containing customer and items</param>
        /// <returns>Grand Total</returns>
        public decimal CalculateDiscount(
            ShoppingCart shoppingCart )
        {
            // Calculate the totals for the item types in the 'basket'
            decimal totalNoGroceries = shoppingCart.GetTotalWithoutGroceries( );
            decimal subTotal = shoppingCart.GetSubTotal( );

            // Get the Discount Amount for the given Customer
            var discPrcnt = shoppingCart.Customer.GetDiscountPercent( ) * totalNoGroceries;

            // Calculate possible discount from amounts over 100
            // - Not percentage based so use sub total
            var bulkDisc = 0M;
            if ( subTotal >= 100M )
                bulkDisc = CalculateBulkDiscount( subTotal );

            // Calculate and return total amount owed
            return subTotal - discPrcnt - bulkDisc;
        }



        /// <summary>
        /// For each $100 the customer gets a $5 discount
        /// </summary>
        /// <param name="total">Total amount before discount</param>
        /// <returns>Bulk Discount Ammount</returns>
        private decimal CalculateBulkDiscount(
            decimal total )
        {
            int discTimes = ( int )total / 100;
            return discTimes * 5;
        }
    }
}