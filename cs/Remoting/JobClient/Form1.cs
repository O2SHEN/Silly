using Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Services;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobClient
{
    public partial class Form1 : Form
    {
        // This member holds a reference to the IJobServer interface
        // on the remote object.
        private IJobServer m_IJobServer;
        // Need a delegate to the ListView.Items.Add method.
        public delegate ListViewItem dlgtListViewItemsAdd(ListViewItem lvItem);
        public delegate IEnumerator dlgtItemsGetEnumerator();

        public Form1()
        {
            InitializeComponent();
            // Get a reference to the remote object.
            m_IJobServer = GetIJobServer();

            EventProxy eventProxy = new EventProxy();
            eventProxy.MyJobEventHandler += this.MyJobEventHandler;
            // Subscribe to the JobEvent.
            m_IJobServer.JobEvent +=
            new JobEventHandler(eventProxy.OnCalculateStateChanged);
        }

        private IJobServer GetIJobServer()
        {
            //
            // Register a channel.
            #region programmatically configured
            #region using tcp channel
            //BinaryClientFormatterSinkProvider clientProv = new BinaryClientFormatterSinkProvider();
            //BinaryServerFormatterSinkProvider serverProv = new BinaryServerFormatterSinkProvider();
            //serverProv.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;

            //Hashtable props = new Hashtable();
            //props["port"] = 0;      //First available port

            //TcpChannel tcpChan = new TcpChannel(props, clientProv, serverProv);
            //ChannelServices.RegisterChannel(tcpChan, false);
            #endregion

            #region using http channel
            SoapClientFormatterSinkProvider clientProv = new SoapClientFormatterSinkProvider();
            SoapServerFormatterSinkProvider serverProv = new SoapServerFormatterSinkProvider();
            serverProv.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;

            Hashtable props = new Hashtable();
            props["port"] = 0;      //First available port

            HttpChannel tcpChan = new HttpChannel(props, clientProv, serverProv);
            ChannelServices.RegisterChannel(tcpChan, false);
            #endregion
            #endregion

            return (IJobServer)Activator.GetObject(typeof(IJobServer), "http://127.0.0.1:1234/JobURI");

        }

        public void MyJobEventHandler(object sender, JobEventArgs args)
        {
            switch (args.Reason)
            {
                case JobEventArgs.ReasonCode.NEW:
                    AddJobToListView(args.Job);
                    break;
                case JobEventArgs.ReasonCode.CHANGE:
                    UpdateJobInListView(args.Job);
                    break;
            }
        }

        // Add job to the list view.
        void AddJobToListView(JobInfo ji)
        {
            // Create a delegate targeting the listView1.Items.Add method.
            dlgtListViewItemsAdd lvadd =
            new dlgtListViewItemsAdd(listView1.Items.Add);
            // Package the JobInfo data in a ListViewItem instance.
            ListViewItem lvItem =
            new ListViewItem(new string[] { ji.m_nID.ToString(),
                ji.m_sDescription,
                ji.m_sAssignedUser,
                ji.m_sStatus });
            // Use Invoke to add the ListViewItem to the list view.
            listView1.Invoke(lvadd, new object[] { lvItem });
        }

        // Update job in list view.
        void UpdateJobInListView(JobInfo ji)
        {
            IEnumerator ie = (IEnumerator)listView1.Invoke(new dlgtItemsGetEnumerator(listView1.Items.GetEnumerator));
            while (ie.MoveNext())
            {
                // Find the job in the list view matching this JobInfo.
                ListViewItem lvItem = (ListViewItem)ie.Current;
                if (!lvItem.Text.Equals(ji.m_nID.ToString()))
                {
                    continue;
                }
                // Found it. Now go through the ListViewItem's subitems
                // and update accordingly.
                IEnumerator ieSub = lvItem.SubItems.GetEnumerator();
                ieSub.MoveNext(); // Skip JobID.
                // Update the description.
                ieSub.MoveNext();
                if (((ListViewItem.ListViewSubItem)ieSub.Current).Text !=
                ji.m_sDescription)
                {
                    ((ListViewItem.ListViewSubItem)ieSub.Current).Text =
                    ji.m_sDescription;
                }
                // Update the assigned user.
                ieSub.MoveNext();
                if (((ListViewItem.ListViewSubItem)ieSub.Current).Text !=
                ji.m_sAssignedUser)
                {
                    ((ListViewItem.ListViewSubItem)ieSub.Current).Text =
                    ji.m_sAssignedUser;
                }
                // Update the status.
                ieSub.MoveNext();
                if (((ListViewItem.ListViewSubItem)ieSub.Current).Text !=
                ji.m_sStatus)
                {
                    ((ListViewItem.ListViewSubItem)ieSub.Current).Text =
                    ji.m_sStatus;
                }
            } // End while
        }

        private JobInfo GetSelectedJobInfo()
        {
            JobInfo ji = new JobInfo();
            // Which job is selected?
            IEnumerator ie = listView1.SelectedItems.GetEnumerator();
            while (ie.MoveNext())
            {
                // Our list view does not allow multiple selections, so we
                // should have no more than one job selected.
                ji =
                ConvertListViewItemToJobInfo((ListViewItem)ie.Current);
            }
            return ji;
        }

        private JobInfo ConvertListViewItemToJobInfo(ListViewItem lvItem)
        {
            JobInfo ji = new JobInfo();
            IEnumerator ieSub = lvItem.SubItems.GetEnumerator();
            ieSub.MoveNext();
            ji.m_nID =
            Convert.ToInt32(
            ((ListViewItem.ListViewSubItem)ieSub.Current).Text);
            ieSub.MoveNext();
            ji.m_sDescription =
            ((ListViewItem.ListViewSubItem)ieSub.Current).Text;
            ieSub.MoveNext();
            ji.m_sAssignedUser =
            ((ListViewItem.ListViewSubItem)ieSub.Current).Text;
            ieSub.MoveNext();
            ji.m_sStatus =
            ((ListViewItem.ListViewSubItem)ieSub.Current).Text;
            return ji;
        }

        private void buttonCreate_Click(object sender, System.EventArgs e)
        {
            // Show Create New Job form.
            FormCreateJob frm = new FormCreateJob();
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                // Create the job on the server.
                string s = frm.JobDescription;
                if (s.Length > 0)
                {
                    m_IJobServer.CreateJob(frm.JobDescription);
                }
            }
        }

        private void buttonAssign_Click(object sender, System.EventArgs e)
        {
            // Which job is selected?
            JobInfo ji = GetSelectedJobInfo();
            m_IJobServer.UpdateJobState(ji.m_nID,
            System.Environment.MachineName,
            "Assigned");
        }

        private void buttonComplete_Click(object sender, System.EventArgs e)
        {
            // Which job is selected?
            JobInfo ji = GetSelectedJobInfo();
            m_IJobServer.UpdateJobState(ji.m_nID,
            System.Environment.MachineName,
            "Completed");
        }

        private void OnClosed(object sender, System.EventArgs e)
        {
            // Make sure we unsubscribe from the JobEvent.
            m_IJobServer.JobEvent -= new JobEventHandler(this.MyJobEventHandler);
        }

    }
}
