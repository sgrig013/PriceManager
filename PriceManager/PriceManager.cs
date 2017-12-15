using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceManager
{
    public enum Direction { Up, Down };

    public class PriceManagerClass
    {
        double currentValue = 0;
        List<Trigger> triggers = new List<Trigger>();

        /// <summary>
        /// Add trigger from a client to the list.  
        /// </summary>
        /// <param name="trigger"></param>
        public void AddTrigger(Trigger trigger)
        {
            trigger.Id = triggers.Count;
            triggers.Add(trigger);
        }

        /// <summary>
        /// Erase all entries.
        /// </summary>
        public void RemoveAllTriggers()
        {
            triggers.Clear();
        }

        /// <summary>
        /// Check new price value provided by users and notify clients about threshold if applicable.
        /// </summary>
        /// <param name="Value"></param>
        public void SetValue(double value)
        {
            foreach (var trigger in triggers)
            {
                // Ignore insignificant fluctuations, consider two decimal places.
                double diffValueChange = Math.Round(Math.Abs(value - currentValue), 2);
                if (trigger.Dir == Direction.Up 
                    && value >= currentValue
                    && value >= trigger.TriggerValue
                    && diffValueChange > trigger.Sensitivity) 
                {
                    trigger.OnThresholdReached(new ThreshholdReachedEventArgs() { Value = value });
                }
                else if (trigger.Dir == Direction.Down 
                    && value <= currentValue
                    && value <= trigger.TriggerValue
                    && diffValueChange > trigger.Sensitivity)
                {
                    trigger.OnThresholdReached(new ThreshholdReachedEventArgs() { Value = value });
                }
            }
            currentValue = value;
        }
    }

    public class ThreshholdReachedEventArgs : EventArgs
    {
        public double Value;
    }

    /// <summary>
    /// Client specific threshold options.
    /// </summary>
    public class Trigger
    {
        public delegate void ThreshholdReachedEventHandler(object source, ThreshholdReachedEventArgs args);
        public event ThreshholdReachedEventHandler ThresholdReached;
        public virtual void OnThresholdReached(ThreshholdReachedEventArgs args)
        {
            if (ThresholdReached != null)
                ThresholdReached(this, args);
        }

        public double TriggerValue { get; set; }
        public Direction Dir { get; set; }
        public double Sensitivity { get; set; }
        public int Id { get; set; }
   }
}

