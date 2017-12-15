using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceManager
{
    public class TestClient : Trigger
    {
        public int AlertsCount = 0;

        public TestClient(double thresholdValue, Direction direction, double sensitivity)
        {
            TriggerValue = thresholdValue;
            Dir = direction;
            Sensitivity = sensitivity;
            ThresholdReached += OnThresholdReached;
        }

        // This should listen for the alert events and do something.
        public void OnThresholdReached(Object source, ThreshholdReachedEventArgs args)
        {
            double value = args.Value;
            AlertsCount++;
            Console.WriteLine("onTrigger: ID={0}, value={1}, AlertCount={2}",
                Id, value, AlertsCount);
        }
    }
}
