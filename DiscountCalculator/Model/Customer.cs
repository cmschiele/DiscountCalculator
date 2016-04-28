using System;

namespace Calculator
{
    public class Customer
    {
        /// <summary>
        /// Type of Customer 
        /// i.e. Employee / Affiliate / Customer
        /// </summary>
        public CustomerType CustomerType { get; set; }

        /// <summary>
        /// Date User became a Customer
        /// </summary>
        public DateTime StartDate { get; set; }


        /// <summary>
        /// Get the Discount Percentage based on the given Customer Type
        /// </summary>
        /// <param name="customerType">Customer Type</param>
        /// <returns>Discount Percentage</returns>
        public decimal GetDiscountPercent( )
        {
            switch ( CustomerType )
            {
                case CustomerType.Employee:
                    return 0.3M;

                case CustomerType.Affiliate:
                    return 0.1M;

                case CustomerType.Customer:
                    return ( YearsOld( ) >= 2 ) ? 0.05M : 0.0M;
            }

            return 0M;
        }



        /// <summary>
        /// Calculate the number of years a customer has been there
        /// </summary>
        /// <param name="startDate">Year customer started</param>
        /// <returns>Number of years rounded down</returns>
        public int YearsOld( )
        {
            var startDate = StartDate;
            var now = DateTime.Now;

            return ( now.Year - startDate.Year - 1 ) + ( ( ( now.Month > startDate.Month ) ||
                ( ( now.Month == startDate.Month ) && ( now.Day >= startDate.Day ) ) ) ? 1 : 0 );
        }
    }


    /// <summary>
    /// Possible Customer Types a Customer can have
    /// </summary>
    public enum CustomerType
    {
        Employee,
        Affiliate,
        Customer
    }
}
