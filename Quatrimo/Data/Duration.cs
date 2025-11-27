using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quatrimo.Data
{
    public class Duration
    {
        DurationType type;

        public int counter = 0;


        public bool completed = false;

        public void Tick(DurationType tickType)
        {
            
            if(tickType == type)
            {
                counter--;
                if(counter <= 0)
                {
                    completed = true;
                }
            }

        }

        public enum DurationType
        {
            Turn = 0,
            Battle = 1,
        }
    }
}
