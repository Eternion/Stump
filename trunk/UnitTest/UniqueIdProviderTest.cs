using System.Threading;
using Stump.Core.Pool;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTest
{
    
    
    /// <summary>
    ///This is a test class for UniqueIdProviderTest and is intended
    ///to contain all UniqueIdProviderTest Unit Tests
    ///</summary>
    [TestClass()]
    public class UniqueIdProviderTest
    {


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
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for UniqueIdProvider Constructor
        ///</summary>
        [TestMethod()]
        public void InterCrossIdProvider()
        {
            var provider = new UniqueIdProvider();
            var providedIds = new List<int>();

            ThreadStart getId = () => providedIds.Add(provider.Pop());
            for (int m = 0; m < 100; m++)
            {

                for (int i = 0; i < 1000; i++)
                {
                    var thread = new Thread(getId);
                    thread.Start();
                }

                Thread.Sleep(100);

                Assert.IsTrue(providedIds.Distinct().Count() == providedIds.Count());

                providedIds.Clear();
            }

        }

    }
}
