using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public interface IJobServer
    {
        event JobEventHandler JobEvent;
        void CreateJob(string sDescription);
        void UpdateJobState(int nJobID,
        string sUser,
        string sStatus);
        ArrayList GetJobs();
    }
}
