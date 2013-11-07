using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    /* This class implements IMyRemoteObject interface in a MarshalByRefObject class. It simply sets and
      * gets an integer value */
    public class MyRemoteObject : MarshalByRefObject, IMyRemoteObject
    {
        // Data members
        private int nData;

        // Constructors
        public MyRemoteObject()
        {
            Console.WriteLine("MyRemoteObject.Constructor: New Object Created!");
        }
        public MyRemoteObject(int n)
        {
            Console.WriteLine("MyRemoteObject.Constructor({0}): New Object with an initial value", n);
            nData = n;
        }
        // IMyRemoteObject Interface implementation
        public void SetValue(int n)
        {
            Console.WriteLine("SetValue(): Old Value is {0}. New Value is {1}", nData, n);
            nData = n;
        }
        public int GetValue()
        {
            Console.WriteLine("GetValue(): Current Value is {0}", nData);
            return nData;
        }

        public override System.Runtime.Remoting.ObjRef CreateObjRef(Type requestedType)
        {
             ObjRef objRef= base.CreateObjRef(requestedType);
             return objRef;
        }
    }
}
