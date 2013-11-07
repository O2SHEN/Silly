using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class EventProxy : MarshalByRefObject
    {
        public event JobEventHandler MyJobEventHandler;

        public EventProxy()
        {

        }

        public void OnCalculateStateChanged(object sender, JobEventArgs arg)
        {

            if (MyJobEventHandler != null)
            {

                MyJobEventHandler(sender, arg);

            }
        }
    }
}
