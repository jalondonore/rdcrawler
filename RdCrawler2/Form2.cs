using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RdCrawler2
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            listView1.View = View.Details;
            listView1.MultiSelect = true;







        }

        private void Form2_Load(object sender, EventArgs e)
        {
            ContextMenuStrip menuStrip;
            menuStrip = new ContextMenuStrip();
            menuStrip.ItemClicked += menuStrip_ItemClicked;
            menuStrip.Items.Add("Cut");
            menuStrip.Items.Add("Copy");
            menuStrip.Items.Add("Paste");
        }





        public void addinlist(String item, String url)
        {
            if ((item.Length >= 4) && (item.Substring(item.Length - 4) == ".jpg")){
                return;
            }
            if (!listView1.Items.ContainsKey(item))
            {
                ListViewItem lista = new ListViewItem(item);
                lista.Name = item;
                lista.Text = item;
                lista.SubItems.Add(url);
                listView1.Items.Add(lista);
            }
        }

        public void clearlist()
        {
            listView1.Items.Clear();
            
        }

        ListViewItem item;
        private void menuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ListView.SelectedListViewItemCollection selectedItems =
            listView1.SelectedItems;
            if (e.ClickedItem.Text == "Copy")
            {
                String text = "";
                foreach (ListViewItem item in selectedItems)
                {
                    text += item.SubItems[1].Text;
                }
                Clipboard.SetText(text);
            }
        }





        private void button1_Click(object sender, EventArgs e)
        {
            /*
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Csv file|*.csv";
            saveFileDialog1.Title = "Save an Csv File";
            saveFileDialog1.ShowDialog();
 
            if (saveFileDialog1.FileName != "")
            {

                using (StreamWriter myOutputStream = new StreamWriter(saveFileDialog1.FileName))
                {
                    foreach (var item in  listView1.Items)
                    {
                        myOutputStream.WriteLine(item.ToString());
                    }
                }


                
            }*/

            {
                SaveFileDialog sfd = new SaveFileDialog
                {
                    Title = "Choose file to save to",
                    FileName = "example.csv",
                    Filter = "CSV (*.csv)|*.csv",
                    FilterIndex = 0,
                    //InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                };

                //show the dialog + display the results in a msgbox unless cancelled


                if (sfd.ShowDialog() == DialogResult.OK)
                {

                    string[] headers = listView1.Columns
                               .OfType<ColumnHeader>()
                               .Select(header => header.Text.Trim())
                               .ToArray();

                    string[][] items = listView1.Items
                                .OfType<ListViewItem>()
                                .Select(lvi => lvi.SubItems
                                    .OfType<ListViewItem.ListViewSubItem>()
                                    .Select(si => si.Text).ToArray()).ToArray();

                    string table = string.Join(";", headers) + Environment.NewLine;
                    foreach (string[] a in items)
                    {
                        //a = a_loopVariable;
                        table += string.Join(";", a) + Environment.NewLine;
                    }
                    table = table.TrimEnd('\r', '\n');
                    System.IO.File.WriteAllText(sfd.FileName, table);

                }
            }

        }

    }
}
