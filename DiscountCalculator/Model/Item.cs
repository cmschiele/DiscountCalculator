namespace Calculator
{
    /// <summary>
    /// Item in a shopping cart
    /// </summary>
    public class Item
    {
        /// <summary>
        /// Type of Item (Default / Groceries)
        /// </summary>
        public ItemType ItemType { get; set; }

        /// <summary>
        /// Value of the Item
        /// </summary>
        public decimal Value { get; set; }
    }


    /// <summary>
    /// Possible Item types that a Item in the Cart can have
    /// </summary>
    public enum ItemType
    {
        Other,
        Groceries
    }
}
