using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    [Serializable]
    public struct JobInfo
    {
        public JobInfo(int nID, string sDescription,
        string sAssignedUser, string sStatus)
        {
            m_nID = nID;
            m_sDescription = sDescription;
            m_sAssignedUser = sAssignedUser;
            m_sStatus = sStatus;
        }
        public int m_nID;
        public string m_sDescription;
        public string m_sAssignedUser;
        public string m_sStatus;
    }
}
