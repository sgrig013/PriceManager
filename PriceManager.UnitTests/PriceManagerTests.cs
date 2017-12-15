using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PriceManager.UnitTests
{
    [TestClass]
    public class PriceManagerTests
    {
        public static PriceManagerClass PriceManager;
        [ClassInitialize()]
        static public void Init(TestContext context)
        {
            // By default initial price value is 0.
            PriceManager = new PriceManagerClass();
        }

        [TestMethod]
        public void SetValuesNoTrigger()
        {
            // Set threshold =1, direction = UP, sensitivity = 0.2.
            TestClient client = new TestClient(1, Direction.Up, 0.2);
            PriceManager.AddTrigger(client);
            PriceManager.SetValue(0.1);
            PriceManager.SetValue(0.2);
            
            Assert.AreEqual(0, client.AlertsCount);
        }

        [TestMethod]
        public void SetValuesToTriggerOnceOnUp()
        {
            PriceManager.RemoveAllTriggers();
            TestClient client = new TestClient(14, Direction.Up, 0.2);
            PriceManager.AddTrigger(client);
            PriceManager.SetValue(14.30);
            PriceManager.SetValue(14.32);

            Assert.AreEqual(1, client.AlertsCount);
        }

        [TestMethod]
        public void SetValuesToTriggerOnceOnDown()
        {
            PriceManager.RemoveAllTriggers();
            TestClient client = new TestClient(14, Direction.Down, 0.2);
            PriceManager.AddTrigger(client);
            PriceManager.SetValue(14.5);
            PriceManager.SetValue(13.7);

            Assert.AreEqual(1, client.AlertsCount);
        }

        [TestMethod]
        public void SetValuesToTriggerTwiceOnUp()
        {
            PriceManager.RemoveAllTriggers();
            TestClient client = new TestClient(14, Direction.Up, 0.2);
            PriceManager.AddTrigger(client);
            PriceManager.SetValue(14.5);
            PriceManager.SetValue(13.7);
            PriceManager.SetValue(14.0);

            Assert.AreEqual(2, client.AlertsCount);
        }

        [TestMethod]
        public void SetValuesToTriggerTwiceOnDown()
        {
            PriceManager.RemoveAllTriggers();
            TestClient client = new TestClient(14, Direction.Down, 0.2);
            PriceManager.AddTrigger(client);
            PriceManager.SetValue(14.5);
            PriceManager.SetValue(13.7);
            PriceManager.SetValue(14.7);
            PriceManager.SetValue(14.0);

            Assert.AreEqual(2, client.AlertsCount);
        }

        [TestMethod]
        public void SetValuesToCheckSensitivity()
        {
            const string valuesInput = "14.30, 14.27, 14.24, 14.25, 14.26, 14.25, 14.26, 14.24, 14.25, 14.35, 14.40";
            string[] values = valuesInput.Split(',');
            
            PriceManager.RemoveAllTriggers();
            TestClient client = new TestClient(14.25, Direction.Down, 0.02);
            PriceManager.AddTrigger(client);
            foreach (string val in values)
            {
                double price = Convert.ToDouble(val);
                PriceManager.SetValue(price);
            }
            Assert.AreEqual(1, client.AlertsCount);
        }

        [TestMethod]
        public void SetValuesTwoClients()
        {
            PriceManager.RemoveAllTriggers();
            // One alert expected out of three steps UP.
            TestClient client1 = new TestClient(14.1, Direction.Up, 0.2);
            PriceManager.AddTrigger(client1);
            // One alert expected out of two steps DOWN.
            TestClient client2 = new TestClient(13.0, Direction.Down, 0.3);
            PriceManager.AddTrigger(client2);

            PriceManager.SetValue(13.7);  // Client1: no trigger since didn't reach threshold
            PriceManager.SetValue(14.1);  // Client1: trigger as reached threshold
            PriceManager.SetValue(14.3);  // Client1: no trigger as less than sensitivity
            PriceManager.SetValue(13.7);  // Client2: no trigger since didn't reach threshold
            PriceManager.SetValue(12.7);  // Client2: trigger as passed threshold down
 
            Assert.AreEqual(1, client1.AlertsCount);
            Assert.AreEqual(1, client2.AlertsCount);
        }
    }
}
