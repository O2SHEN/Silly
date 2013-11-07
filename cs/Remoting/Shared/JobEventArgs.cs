using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public delegate void JobEventHandler(object sender, JobEventArgs args);
    [Serializable]
    public class JobEventArgs : System.EventArgs
    {
        public enum ReasonCode { NEW, CHANGE };
        private ReasonCode m_Reason;
        private JobInfo m_JobInfo;
        public JobEventArgs(JobInfo NewJob, ReasonCode Reason)
        {
            m_JobInfo = NewJob;
            m_Reason = Reason;
        }
        public JobInfo Job
        {
            get
            { return m_JobInfo; }
            set
            { m_JobInfo = value; }
        }
        public ReasonCode Reason
        {
            get
            { return m_Reason; }
        }
    }
}
