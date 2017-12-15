using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceManager
{
    class Program
    {
        static void Main(string[] args)
        {
            PriceManagerClass pm = new PriceManagerClass();

            Console.WriteLine("Case 0: no alerts expected for set value = 0.2");
            TestClient client0 = new TestClient(1, Direction.Up, 0.2);

            pm.AddTrigger(client0);
            pm.SetValue(0.2);
            
            Console.WriteLine("\nCase 1: 1 alert expected out of three steps UP 13.7, 14.1, 14.3");
            pm.RemoveAllTriggers();
            // 'Sell trigger' if value >= 14.1, sensitivity=0.2.
            TestClient client1 = new TestClient(14.1, Direction.Up, 0.2); 
            pm.AddTrigger(client1);

            pm.SetValue(13.7);  // no trigger since didn't reach threshold
            pm.SetValue(14.1);  // trigger as reached threshold
            pm.SetValue(14.3);  // no trigger as less than sensitivity

            Console.WriteLine("\nCase 2: 1 alert expected out of two steps DOWN 13.7, 12.7");
            // 'Buy trigger' if value <= 13.0, sensitivity=0.3.
            TestClient client2 = new TestClient(13.0, Direction.Down, 0.3); 
            pm.AddTrigger(client2);
            pm.SetValue(13.7);  // no trigger since didn't reach threshold
            pm.SetValue(12.7);  // trigger as passed threshold down

            Console.WriteLine("\n Please run Price Manager tests to see more scenarios!");

            Console.ReadLine();
        }
    }
}
