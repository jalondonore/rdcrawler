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
    public partial class OptionsForm : Form
    {
        //private Form1 form1 = new Form1();

        public OptionsForm()
        {
            InitializeComponent();
            rendermodeoption.Enabled = false; //desactivo opcion hasta que sea bien provada
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        public void settings( bool fullsitecrawloption, bool htmlpagesoption)
        {
            this.fullsitecraeloptioncheck.Checked = fullsitecrawloption;
            this.onlyhtmloptioncheck.Checked = htmlpagesoption;
        }


        //Tomo parametros cargados es la pantalla principal 
        private void OptionsForm_Load(object sender, EventArgs e)
        {
            
            if (Form1.htmlpagesoption == true)
            {
                onlyhtmloptioncheck.Checked = true;
            }
            else
            {
                onlyhtmloptioncheck.Checked = false;
            }

            if (Form1.fullsitecrawloption == true)
            {
                fullsitecraeloptioncheck.Checked = true;
            }
            else
            {
                fullsitecraeloptioncheck.Checked = false;
            }
            if (Form1.redirectsoption == true)
            {
                redirectsoption.Checked = true;
            }
            else
            {
                redirectsoption.Checked = false;
            }
            if (Form1.getexurlsoption == true)
            {
                getexternalurloption.Checked = true;
            }
            else
            {
                getexternalurloption.Checked = false;
            }

            if (Form1.rendermodeoption == true)
            {
                rendermodeoption.Checked = true;
            }
            else
            {
                rendermodeoption.Checked = false;
            }

            if (Form1.rendermodeoption == true)
            {
                rendermodeoption.Checked = true;
            }
            else
            {
                rendermodeoption.Checked = false;
            }

            if (Form1.ignoreresourcesoption == true)
            {
                ignoreresourcesoption.Checked = true;
            }
            else
            {
                ignoreresourcesoption.Checked = false;
            }

        }

        //Al aplicar cambios pongo las nuevas configuraciones en las variables del programa
        private void apply_button_Click(object sender, EventArgs e)
        {
            if (onlyhtmloptioncheck.Checked)
            {
                Form1.htmlpagesoption = true;
            }
            else
            {
                Form1.htmlpagesoption = false;
            }
            if (fullsitecraeloptioncheck.Checked)
            {
                Form1.fullsitecrawloption = true;
            }
            else
            {
                Form1.fullsitecrawloption = false;
            }
            if (redirectsoption.Checked)
            {
                Form1.redirectsoption = true;
            }
            else
            {
                Form1.redirectsoption = false;
            }
            if (getexternalurloption.Checked)
            {
                Form1.getexurlsoption = true;
            }
            else
            {
                Form1.getexurlsoption = false;
            }

            if (rendermodeoption.Checked)
            {
                Form1.rendermodeoption = true;
            }
            else
            {
                Form1.rendermodeoption = false;
            }

            if (ignoreresourcesoption.Checked)
            {
                Form1.ignoreresourcesoption = true;
            }
            else
            {
                Form1.ignoreresourcesoption = false;
            }

            Form1.savesettings();
        }

        private void cancel_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //si se preciona ok pongo los valores y cierro la ventana
        private void ok_button_Click(object sender, EventArgs e)
        {
            if (onlyhtmloptioncheck.Checked == true)
            {
                Form1.htmlpagesoption = true;
            }
            else
            {
                Form1.htmlpagesoption = false;
            }

            if (fullsitecraeloptioncheck.Checked == true)
            {
                Form1.fullsitecrawloption = true;
            }
            else
            {
                Form1.fullsitecrawloption = false;
            }

            if (redirectsoption.Checked == true)
            {
                Form1.redirectsoption = true;
            }
            else
            {
                Form1.redirectsoption = false;
            }

            if (getexternalurloption.Checked == true)
            {
                Form1.getexurlsoption = true;
            }
            else
            {
                Form1.getexurlsoption = false;
            }

            if (rendermodeoption.Checked == true)
            {
                Form1.rendermodeoption = true;
            }
            else
            {
                Form1.rendermodeoption = false;
            }

            if (ignoreresourcesoption.Checked == true)
            {
                Form1.ignoreresourcesoption = true;
            }
            else
            {
                Form1.ignoreresourcesoption = false;
            }

            
            Form1.savesettings();

            this.Close();
        }
    }
}
