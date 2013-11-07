using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JobClient
{
    public partial class FormCreateJob : Form
    {
        private string m_sDescription;
        public string JobDescription
        {
            get { return m_sDescription; }
        }
        
        public FormCreateJob()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            m_sDescription = textBox1.Text;
        }
        private void button2_Click(object sender, System.EventArgs e)
        {
            this.Hide();
        }
    }
}
