using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RdCrawler2
{
    public partial class Form3 : Form
    {
        //private Form1 form1c; //= new Form1();
        //Form1 originalForm;
        //public Form3(Form1 incomingForm)
        public Form3()
        {
            
            //originalForm = incomingForm;
            //Form1 form1 = new Form1();
            InitializeComponent();
            button2.DialogResult = DialogResult.OK;
            button3.DialogResult = DialogResult.Cancel;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Getting Text from Clip board
                string s = Clipboard.GetText();
                //Parsing criteria: New Line 
                string[] lines = s.Split('\n');
                foreach (string ln in lines)
                {
                    listBox1.Items.Add(ln.Trim());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public ListBox getItem()
        {
            return listBox1;
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
