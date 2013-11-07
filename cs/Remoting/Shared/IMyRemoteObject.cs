using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public interface IMyRemoteObject
    {
        int GetValue();
        void SetValue(int nValue);
    }
}
