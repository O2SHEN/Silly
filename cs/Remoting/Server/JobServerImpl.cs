using Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class JobServerImpl : MarshalByRefObject, IJobServer
    {
        private int m_nNextJobNumber;
        private ArrayList m_JobArray;
        public JobServerImpl()
        {
            m_nNextJobNumber = 0;
            m_JobArray = new ArrayList();
        }

        // Implement the IJobServer interface.
        public event JobEventHandler JobEvent;
        public ArrayList GetJobs()
        {
            return m_JobArray;
        }

        public void CreateJob(string sDescription)
        {
            // Create a new JobInfo instance.
            JobInfo oJobInfo = new JobInfo(m_nNextJobNumber,
            sDescription,
            "",
            "");
            // Increment the next job number.
            m_nNextJobNumber++;
            // Add the JobInfo instance to our JobArray.
            m_JobArray.Add(oJobInfo);
            // Notify any attached clients of the new job.
            NotifyClients(new JobEventArgs(oJobInfo,
            JobEventArgs.ReasonCode.NEW));
        }

        public void UpdateJobState(int nJobID,
        string sUser,
        string sStatus)
        {
            // Get the specified job from the array.
            JobInfo oJobInfo = (JobInfo)m_JobArray[nJobID];
            // Update the user and status fields.
            oJobInfo.m_sAssignedUser = sUser;
            oJobInfo.m_sStatus = sStatus;
            // Update the array element because JobInfo is a value type.
            m_JobArray[nJobID] = oJobInfo;
            // Notify any attached clients of the new job.
            NotifyClients(new JobEventArgs(oJobInfo,
            JobEventArgs.ReasonCode.CHANGE));
        }

        // Helper function to raise IJobServer.JobEvent
        private void NotifyClients(JobEventArgs args)
        {
            //
            // Manually invoke each event handler to
            // catch disconnected clients.
            System.Delegate[] invkList = JobEvent.GetInvocationList();
            IEnumerator ie = invkList.GetEnumerator();
            while (ie.MoveNext())
            {
                JobEventHandler handler = (JobEventHandler)ie.Current;
                try
                {
                     //handler.Invoke(this, args); //help to debug
                   handler.BeginInvoke(this, args, null, null);
                }
                catch (System.Exception e)
                {
                    JobEvent -= handler;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e.Message);
                    Console.ResetColor();
                }
            }
        }
    }
}
