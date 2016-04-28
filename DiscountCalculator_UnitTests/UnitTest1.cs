using Calculator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace DiscountCalculator_UnitTests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class UnitTest1
    {
        public UnitTest1( )
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion


        #region Method Tests

        /// <summary>
        /// Test the method that calculates the years a user has been a customer
        /// </summary>
        [TestMethod]
        public void TestYearsOld( )
        {
            var customer = new Customer
            {
                CustomerType = CustomerType.Employee,
                StartDate = new DateTime( 1989, 04, 02 )
            };

            var years = customer.YearsOld( );
            Assert.AreEqual( 27, years );
        }


        /// <summary>
        /// Test the subtotal calculation
        /// </summary>
        public void TestSubTotal( )
        {
            var customer = new Customer
            {
                CustomerType = CustomerType.Employee,
                StartDate = DateTime.Now
            };

            // items Total $1190
            var items = new List<Item>
            {
                new Item { ItemType = ItemType.Other, Value = 900M },
                new Item { ItemType = ItemType.Other, Value = 90M },
                new Item { ItemType = ItemType.Groceries, Value = 200M }
            };

            var shoppingCart = new ShoppingCart
            {
                Customer = customer,
                Items = items
            };

            var subTotal = shoppingCart.GetSubTotal( );
            Assert.AreEqual( 1190M, subTotal );
        }


        /// <summary>
        /// Test the subtotal calculation without groceries
        /// </summary>
        [TestMethod]
        public void TestTotalWithoutGroceries( )
        {
            var customer = new Customer
            {
                CustomerType = CustomerType.Employee,
                StartDate = DateTime.Now
            };

            // non-grocery sub-total - $990
            var items = new List<Item>
            {
                new Item { ItemType = ItemType.Other, Value = 900M },
                new Item { ItemType = ItemType.Other, Value = 90M },
                new Item { ItemType = ItemType.Groceries, Value = 200M }
            };

            var shoppingCart = new ShoppingCart
            {
                Customer = customer,
                Items = items
            };

            var totalNoGroceries = shoppingCart.GetTotalWithoutGroceries( );
            Assert.AreEqual( 990M, totalNoGroceries );
        }



        /// <summary>
        /// Test that the correct discount percentage is returned for an employee
        /// </summary>
        [TestMethod]
        public void TestEmployeeDiscountPercent( )
        {
            var customer = new Customer
            {
                CustomerType = CustomerType.Employee,
                StartDate = DateTime.Now
            };

            var percent = customer.GetDiscountPercent( );
            Assert.AreEqual( 0.3M, percent );
        }



        /// <summary>
        /// Test that the correct discount percentage is returned for an affiliate
        /// </summary>
        [TestMethod]
        public void TestAffiliateDiscountPercent( )
        {
            var customer = new Customer
            {
                CustomerType = CustomerType.Affiliate,
                StartDate = DateTime.Now
            };

            var percent = customer.GetDiscountPercent( );
            Assert.AreEqual( 0.1M, percent );
        }



        /// <summary>
        /// Test that the correct discount percentage is returned for a customer less than 2 years
        /// </summary>
        [TestMethod]
        public void TestNewCustomerDiscountPercent( )
        {
            var customer = new Customer
            {
                CustomerType = CustomerType.Customer,
                StartDate = DateTime.Now
            };

            var percent = customer.GetDiscountPercent( );
            Assert.AreEqual( 0M, percent );
        }



        /// <summary>
        /// Test that the correct discount percentage is returned for a customer more than 2 years
        /// </summary>
        [TestMethod]
        public void TestOldCustomerDiscountPercent( )
        {
            var customer = new Customer
            {
                CustomerType = CustomerType.Customer,
                StartDate = new DateTime( 1989, 2, 4 )
            };

            var percent = customer.GetDiscountPercent( );
            Assert.AreEqual( 0.05M, percent );
        }

        #endregion


        #region Basic Tests for Customer Types    

        /// <summary>    
        /// Calculate Employee Discount
        /// - 30% off given price
        /// </summary>
        [TestMethod]
        public void CalculateEmployeeDiscount( )
        {
            var customer = new Customer
            {
                CustomerType = CustomerType.Employee,
                StartDate = DateTime.Now,
            };

            // items Total $100
            var items = new List<Item>
            {
                new Item { ItemType = ItemType.Other, Value = 50M},
                new Item {ItemType = ItemType.Other, Value = 50M }
            };

            var shoppingCart = new ShoppingCart
            {
                Customer = customer,
                Items = items
            };

            // Input amount is $100, expected output is $65
            // $30 discount from 30% discount + 5$ discount for $100
            var calculator = new DiscountCalculator( );
            var amountOwed = calculator.CalculateDiscount( shoppingCart );
            Assert.AreEqual( 65M, amountOwed );
        }



        /// <summary>     
        /// Calculate Affiliate Discount
        /// - 10% off given price
        /// </summary>
        [TestMethod]
        public void CalculateAffiliateDiscount( )
        {
            var customer = new Customer
            {
                CustomerType = CustomerType.Affiliate,
                StartDate = DateTime.Now
            };

            // items Total $100
            var items = new List<Item>
            {
                new Item { ItemType = ItemType.Other, Value = 50M},
                new Item {ItemType = ItemType.Other, Value = 50M }
            };

            var shoppingCart = new ShoppingCart
            {
                Customer = customer,
                Items = items
            };

            // Input amount is $100, expected output is $85
            // $10 discount from the 10% + 5$ discount for being $100
            var calculator = new DiscountCalculator( );
            var amountOwed = calculator.CalculateDiscount( shoppingCart );
            Assert.AreEqual( 85M, amountOwed );
        }



        /// <summary>     
        /// Calculate Customer Discount for a customer that has been there 0 Years
        /// - 0% discount
        /// </summary>
        [TestMethod]
        public void CalculateCustomerDiscount( )
        {
            var customer = new Customer
            {
                CustomerType = CustomerType.Customer,
                StartDate = DateTime.Now,
            };

            // items Total $50
            var items = new List<Item>
            {
                new Item { ItemType = ItemType.Other, Value = 20M},
                new Item {ItemType = ItemType.Other, Value = 30M }
            };

            var shoppingCart = new ShoppingCart
            {
                Customer = customer,
                Items = items
            };

            //Input amount is $50 (less than $100) so expected output is also $50
            var calculator = new DiscountCalculator( );
            var amountOwed = calculator.CalculateDiscount( shoppingCart );
            Assert.AreEqual( 50M, amountOwed );
        }



        /// <summary>    
        /// Calculate Customer Discount for a customer that has been there more than 2 years
        /// - 5% discount
        /// </summary>
        [TestMethod]
        public void CalculateCustomerDiscount2( )
        {
            var customer = new Customer
            {
                CustomerType = CustomerType.Customer,
                StartDate = new DateTime( 2011, 1, 1 ),
            };

            // items Total $50
            var items = new List<Item>
            {
                new Item { ItemType = ItemType.Other, Value = 20M},
                new Item {ItemType = ItemType.Other, Value = 30M }
            };

            var shoppingCart = new ShoppingCart
            {
                Customer = customer,
                Items = items
            };

            //Input amount is $50 (less than $100) so expected output is also $47.5
            var calculator = new DiscountCalculator( );
            var amountOwed = calculator.CalculateDiscount( shoppingCart );
            Assert.AreEqual( 47.5M, amountOwed );
        }

        #endregion


        #region Tests for discounts on large amounts

        /// <summary>    
        /// Calculate Discount for large amounts
        /// i.e for every $100 on the bill there is a $5 discount
        /// </summary>
        [TestMethod]
        public void CalculateLargeAmountDiscount( )
        {
            var customer = new Customer
            {
                CustomerType = CustomerType.Customer,
                StartDate = DateTime.Now
            };

            // items Total $990
            var items = new List<Item>
            {
                new Item { ItemType = ItemType.Other, Value = 900M},
                new Item {ItemType = ItemType.Other, Value = 90M }
            };

            var shoppingCart = new ShoppingCart
            {
                Customer = customer,
                Items = items
            };

            //Input amount is $990, discount is then $45, output should be $945
            var calculator = new DiscountCalculator( );
            var amountOwed = calculator.CalculateDiscount( shoppingCart );
            Assert.AreEqual( 945M, amountOwed );
        }



        /// <summary>     
        /// Calculate Discount for large amounts
        /// i.e for every $100 on the bill there is a $5 discount
        /// </summary>
        [TestMethod]
        public void CalculateLargeAmountDiscount2( )
        {
            var customer = new Customer
            {
                CustomerType = CustomerType.Customer,
                StartDate = new DateTime( 2011, 1, 1 ),
            };

            // items Total $1000
            var items = new List<Item>
            {
                new Item { ItemType = ItemType.Other, Value = 900M},
                new Item {ItemType = ItemType.Other, Value = 100M }
            };

            var shoppingCart = new ShoppingCart
            {
                Customer = customer,
                Items = items
            };

            //Input amount is $1000, 
            //discount % is then $50,
            //discount amount is $50 
            //output should be $900
            var calculator = new DiscountCalculator( );
            var amountOwed = calculator.CalculateDiscount( shoppingCart );
            Assert.AreEqual( 900M, amountOwed );
        }



        /// <summary>     
        /// Calculate Discount for large amounts for an employee
        /// i.e for every $100 on the bill there is a $5 discount
        /// </summary>
        [TestMethod]
        public void CalculateLargeAmountDiscountForEmployee( )
        {
            var customer = new Customer
            {
                CustomerType = CustomerType.Employee,
                StartDate = new DateTime( 2011, 1, 1 ),
            };

            // items Total $990
            var items = new List<Item>
            {
                new Item { ItemType = ItemType.Other, Value = 900M},
                new Item {ItemType = ItemType.Other, Value = 90M }
            };

            var shoppingCart = new ShoppingCart
            {
                Customer = customer,
                Items = items
            };

            //Input amount is $990,
            //Discount % = $297
            //Bulkd Discount = $45
            //output should be $648
            var calculator = new DiscountCalculator( );
            var amountOwed = calculator.CalculateDiscount( shoppingCart );
            Assert.AreEqual( 648M, amountOwed );
        }



        /// <summary>    
        /// Calculate Discount for large amounts for an affiliate  
        /// </summary>
        [TestMethod]
        public void CalculateLargeAmountDiscountForAffiliate( )
        {
            var customer = new Customer
            {
                CustomerType = CustomerType.Affiliate,
                StartDate = new DateTime( 2011, 1, 1 ),
            };

            // items Total $990
            var items = new List<Item>
            {
                new Item { ItemType = ItemType.Other, Value = 900M},
                new Item {ItemType = ItemType.Other, Value = 90M }
            };

            var shoppingCart = new ShoppingCart
            {
                Customer = customer,
                Items = items
            };

            //Input amount is $990, 
            //discount % =  $99
            //discount amount = $45
            //output should be $846
            var calculator = new DiscountCalculator( );
            var amountOwed = calculator.CalculateDiscount( shoppingCart );
            Assert.AreEqual( 846M, amountOwed );
        }



        /// <summary>     
        /// Calculate Discount for large amounts for an old customer
        /// i.e for every $100 on the bill there is a $5 discount
        /// </summary>
        [TestMethod]
        public void CalculateLargeAmountDiscountForOldCustomer( )
        {
            var customer = new Customer
            {
                CustomerType = CustomerType.Customer,
                StartDate = new DateTime( 2011, 1, 1 ),
            };

            // items Total $990
            var items = new List<Item>
            {
                new Item { ItemType = ItemType.Other, Value = 900M},
                new Item {ItemType = ItemType.Other, Value = 90M }
            };

            var shoppingCart = new ShoppingCart
            {
                Customer = customer,
                Items = items
            };

            //Input amount is $990, 
            //Discount % =  $49.5
            //Bulk discount = $45
            //output should be $895.5
            var calculator = new DiscountCalculator( );
            var amountOwed = calculator.CalculateDiscount( shoppingCart );
            Assert.AreEqual( 895.5M, amountOwed );
        }

        #endregion


        #region Tests for different Item Types in items

        /// <summary>    
        /// Calculate Discount for varying ItemTypes for Employee
        /// </summary>
        [TestMethod]
        public void CalculateVariousItemTypeitemsForEmployee( )
        {
            var customer = new Customer
            {
                CustomerType = CustomerType.Employee,
                StartDate = DateTime.Now
            };

            // items Total $1190
            var items = new List<Item>
            {
                new Item { ItemType = ItemType.Other, Value = 900M },
                new Item { ItemType = ItemType.Other, Value = 90M },
                new Item { ItemType = ItemType.Groceries, Value = 200M }
            };

            var shoppingCart = new ShoppingCart
            {
                Customer = customer,
                Items = items
            };

            //Input amount is $1190,
            //Total without Groceries is $990 
            //discount is then $297, 
            //Bulk Discount = $55
            // output should be $838
            var calculator = new DiscountCalculator( );
            var amountOwed = calculator.CalculateDiscount( shoppingCart );
            Assert.AreEqual( 838M, amountOwed );
        }



        /// <summary>    
        /// Calculate Discount for varying ItemTypes for Affiliate
        /// </summary>
        [TestMethod]
        public void CalculateVariousItemTypeitemsForAffiliate( )
        {
            var customer = new Customer
            {
                CustomerType = CustomerType.Affiliate,
                StartDate = DateTime.Now
            };

            // items Total $1190
            var items = new List<Item>
            {
                new Item { ItemType = ItemType.Other, Value = 900M },
                new Item { ItemType = ItemType.Other, Value = 90M },
                new Item { ItemType = ItemType.Groceries, Value = 200M }
            };

            var shoppingCart = new ShoppingCart
            {
                Customer = customer,
                Items = items
            };

            //Input amount is $1190,
            //Total without Groceries is $990 
            //discount % is then $99,
            //Bulk Discount = $55 
            // output should be $1036
            var calculator = new DiscountCalculator( );
            var amountOwed = calculator.CalculateDiscount( shoppingCart );
            Assert.AreEqual( 1036M, amountOwed );
        }



        /// <summary>    
        /// Calculate Discount for varying ItemTypes for New Customer (<2Years)
        /// </summary>
        [TestMethod]
        public void CalculateVariousItemTypeitemsForNewCustomer( )
        {
            var customer = new Customer
            {
                CustomerType = CustomerType.Customer,
                StartDate = DateTime.Now,
            };

            // items Total $1190
            var items = new List<Item>
            {
                new Item { ItemType = ItemType.Other, Value = 900M },
                new Item { ItemType = ItemType.Other, Value = 90M },
                new Item { ItemType = ItemType.Groceries, Value = 200M }
            };

            var shoppingCart = new ShoppingCart
            {
                Customer = customer,
                Items = items
            };

            //Input amount is $1190,
            //This won't be using a percentage based discount - so we use the grand total 
            //discount is then $55, 
            //output should be $1135
            var calculator = new DiscountCalculator( );
            var amountOwed = calculator.CalculateDiscount( shoppingCart );
            Assert.AreEqual( 1135M, amountOwed );
        }



        /// <summary>    
        /// Calculate Discount for varying ItemTypes for Old Customer (>2Years)
        /// </summary>
        [TestMethod]
        public void CalculateVariousItemTypeitemsForOldCustomer( )
        {
            var customer = new Customer
            {
                CustomerType = CustomerType.Customer,
                StartDate = new DateTime( 2011, 1, 1 ),
            };

            // items Total $1190
            var items = new List<Item>
            {
                new Item { ItemType = ItemType.Other, Value = 900M },
                new Item { ItemType = ItemType.Other, Value = 90M },
                new Item { ItemType = ItemType.Groceries, Value = 200M }
            };

            var shoppingCart = new ShoppingCart
            {
                Customer = customer,
                Items = items
            };

            //Input amount is $1190,
            //Total without Groceries is $990 
            //discount is then $49.5, 
            //Bulk Discount = $55
            //so the output should be $1085.5
            var calculator = new DiscountCalculator( );
            var amountOwed = calculator.CalculateDiscount( shoppingCart );
            Assert.AreEqual( 1085.5M, amountOwed );
        }



        /// <summary>   
        /// Calculate Discount for varying ItemTypes for New Customer (<2Years)
        /// </summary>
        [TestMethod]
        public void CalculateVariousItemTypeitemsForNewCustomerSmallAmount( )
        {
            var customer = new Customer
            {
                CustomerType = CustomerType.Customer,
                StartDate = DateTime.Now
            };

            // items Total $50
            var items = new List<Item>
            {
                new Item { ItemType = ItemType.Other, Value = 20M },
                new Item { ItemType = ItemType.Other, Value = 20M },
                new Item { ItemType = ItemType.Groceries, Value = 10M }
            };

            var shoppingCart = new ShoppingCart
            {
                Customer = customer,
                Items = items
            };

            //Input amount is $50,
            //Total without Groceries is $40 
            //discount is then $0, 
            // output should be $50
            var calculator = new DiscountCalculator( );
            var amountOwed = calculator.CalculateDiscount( shoppingCart );
            Assert.AreEqual( 50M, amountOwed );
        }



        /// <summary>   
        /// Calculate Discount for varying ItemTypes for Old Customer (>2Years)
        /// </summary>
        [TestMethod]
        public void CalculateVariousItemTypeitemsForOldCustomerSmallAmount( )
        {
            var customer = new Customer
            {
                CustomerType = CustomerType.Customer,
                StartDate = new DateTime( 2011, 1, 1 ),
            };

            // items Total $50
            var items = new List<Item>
            {
                new Item { ItemType = ItemType.Other, Value = 20M },
                new Item { ItemType = ItemType.Other, Value = 20M },
                new Item { ItemType = ItemType.Groceries, Value = 10M }
            };

            var shoppingCart = new ShoppingCart
            {
                Customer = customer,
                Items = items
            };

            //Input amount is $50,
            //Total without Groceries is $40 
            //discount is then $2, 
            // output should be $48
            var calculator = new DiscountCalculator( );
            var amountOwed = calculator.CalculateDiscount( shoppingCart );
            Assert.AreEqual( 48M, amountOwed );
        }

        #endregion
    }
}
