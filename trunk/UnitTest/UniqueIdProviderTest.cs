using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stump.Core.Pool;

namespace UnitTest
{
    /// <summary>
    ///This is a test class for UniqueIdProviderTest and is intended
    ///to contain all UniqueIdProviderTest Unit Tests
    ///</summary>
    [TestClass]
    public class UniqueIdProviderTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get;
            set;
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
        [TestMethod]
        public void InterCrossIdProvider()
        {
            var provider = new UniqueIdProvider();
            var providedIds = new List<int>();

            ParameterizedThreadStart removeId = id =>
                                                    {
                                                        providedIds.Remove((int) id);

                                                        provider.Push((int) id);
                                                        Debug.WriteLine("Pushed : " + id);
                                                    };

            ThreadStart getId = () =>
                                    {
                                        var thread2 = new Thread(removeId);

                                        for (int i = 0; i < 100; i++)
                                        {
                                            int id = provider.Pop();
                                            Debug.WriteLine("Popped : " + id);

                                            providedIds.Add(id);

                                            if (i > 1)
                                                thread2.Start(providedIds.Last());
                                        }
                                    };

            var thread = new Thread(getId);
            thread.Start();
            thread.Join();

            Assert.IsTrue(providedIds.Distinct().Count() == providedIds.Count());
        }
    }
}