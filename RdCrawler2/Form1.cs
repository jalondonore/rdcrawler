using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using System.Xml;
using System.Net;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Net.Sockets;
using Gecko;

//using SharpUpdate;
//using System.Reflection;


namespace RdCrawler2
{
    public partial class Form1 : Form
    {
        private Thread hilo;
        private Uri sitehost;
        private Uri siteredirected;
        private Form2 form2 = new Form2();
        private OptionsForm optionsform = new OptionsForm();
        private List<String> templist = new List<String>();
        public GeckoElementCollection links;
        /*updater*/
        //private SharpUpdater updater;
        /* setup variables */
        public static bool htmlpagesoption;
        public static bool fullsitecrawloption;
        public static bool redirectsoption;
        public static bool getexurlsoption;
        public static bool rendermodeoption;
        public static bool ignoreresourcesoption;
        //private List<String[][]> tempdeeplist = new List<String[][]>();
        //private Int32 selecteddepth;
        //private Form3 form3; // form3 = new Form3();

        public Form1()
        {
            InitializeComponent();
            //emailchecker();
            /*
             * UPDATER
             */
            //updater = new SharpUpdater(Assembly.GetExecutingAssembly(), this, new Uri("https://raw.githubusercontent.com/henryxrl/SharpUpdate/master/project.xml"));
            //updater = new SharpUpdater(Assembly.GetExecutingAssembly(), this, new Uri(new System.IO.FileInfo(@"..\..\..\project.xml").FullName));       // for local testing
            /*-UPDATER- Code end*/

            //add geckobrowser control to form1

            //Xpcom.Initialize("Firefox");
            //GeckoWebBrowser geckoWebBrowser1 = new GeckoWebBrowser();
            //var geckoWebBrowser1 = new GeckoWebBrowser { Dock = DockStyle.Fill };
            //geckoWebBrowser1.Visible = false;
            //Controls.Add(geckoWebBrowser1);


            CheckForIllegalCrossThreadCalls = false; //avoit to error acces to listbox from thread
            numericUpDown1.Minimum = 1;
            numericUpDown1.Maximum = 5000;
            button1.Text = "GO!";
            getsitemap_checkBox.Enabled = true;
            button2.Visible = false;
            updateAhrefsDataToolStripMenuItem.Enabled = false;
            openToolStripMenuItem.Enabled = false;
            saveToolStripMenuItem.Enabled = false;
            saveAsToolStripMenuItem.Enabled = false;
            closeToolStripMenuItem.Enabled = false;
            aboutToolStripMenuItem.Enabled = false;

            loadsettings();
            if (htmlpagesoption)
            {
                getsitemap_checkBox.Checked = true;
            }

            if (fullsitecrawloption)
            {
                checkBox3.Checked = true;
            }
            /*
            if (redirectsoption)
            {
                checkBox2.Checked = true;
            }
            */
            //Gecko.Xpcom.Initialize(Environment.CurrentDirectory + "\\xulrunner");
            Xpcom.Initialize("Firefox");
            string sUserAgent = "Mozilla/6.0 (Windows NT 10.0; rv:36.0) Gecko/20100101 Firefox/65.0.1";
            Gecko.GeckoPreferences.User["general.useragent.override"] = sUserAgent;

            nsIPrefBranch pref = Xpcom.GetService<nsIPrefBranch>("@mozilla.org/preferences-service;1");

            // Show same page as firefox does for unsecure SSL/ TLS connections...
            pref.SetIntPref("browser.ssl_override_behavior", 1);
            pref.SetIntPref("security.OCSP.enabled", 0);
            pref.SetBoolPref("security.OCSP.require", false);
            pref.SetBoolPref("extensions.hotfix.cert.checkAttributes", true);
            pref.SetBoolPref("security.remember_cert_checkbox_default_setting", true);
            pref.SetBoolPref("services.sync.prefs.sync.security.default_personal_cert", true);
            pref.SetBoolPref("browser.xul.error_pages.enabled", true);
            pref.SetBoolPref("browser.xul.error_pages.expert_bad_cert", false);

            // disable caching of http documents
            pref.SetBoolPref("network.http.use-cache", false);

            // disalbe memory caching
            pref.SetBoolPref("browser.cache.memory.enable", false);

            // Desktop Notification
            pref.SetBoolPref("notification.feature.enabled", true);

            // WebSMS
            pref.SetBoolPref("dom.sms.enabled", true);
           // pref.SetCharPref("dom.sms.whitelist", "");

            // WebContacts
            pref.SetBoolPref("dom.mozContacts.enabled", true);
            //pref.SetCharPref("dom.mozContacts.whitelist", "");

            pref.SetBoolPref("social.enabled", false);

            // WebAlarms
            pref.SetBoolPref("dom.mozAlarms.enabled", true);

            // WebSettings
            pref.SetBoolPref("dom.mozSettings.enabled", true);

            pref.SetBoolPref("network.jar.open-unsafe-types", true);
            pref.SetBoolPref("security.warn_entering_secure", false);
            pref.SetBoolPref("security.warn_entering_weak", false);
            pref.SetBoolPref("security.warn_leaving_secure", false);
            pref.SetBoolPref("security.warn_viewing_mixed", false);
            pref.SetBoolPref("security.warn_submit_insecure", false);
            pref.SetIntPref("security.ssl.warn_missing_rfc5746", 1);
            pref.SetBoolPref("security.ssl.enable_false_start", false);
            pref.SetBoolPref("security.enable_ssl3", true);
            pref.SetBoolPref("security.enable_tls", true);
            pref.SetBoolPref("security.enable_tls_session_tickets", true);
            pref.SetIntPref("privacy.popups.disable_from_plugins", 2);

            // don't store passwords
            pref.SetIntPref("security.ask_for_password", 1);
            pref.SetIntPref("security.password_lifetime", 0);
            pref.SetBoolPref("signon.prefillForms", false);
            pref.SetBoolPref("signon.rememberSignons", false);
            pref.SetBoolPref("browser.fixup.hide_user_pass", false);
            pref.SetBoolPref("privacy.item.passwords", true);
            /*
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Xpcom.GetService<nsIMemory>("@mozilla.org/xpcom/memory-service;1").HeapMinimize(true);
            */

        }


        private void Form1_Load(object sender, EventArgs e)
        {


        }

        private void CargarDatosHilo()
        {

                //UseWaitCursor = true; //ICONO DE ESPERA EN EL PUNTERO
                listBox1.Items.Clear();
                /*panelPrincipal.Enabled = false;*/
                /*Podemos sustituir con un panel para mas facilidad y menos lineas*/
                var selectdepth = 0;

                if (getsitemap_checkBox.Checked == true)
                {
                    readsitemaxml(textBox1.Text); //leo el sitemap.xml
                }

                if (textBox1.Text.Substring(0, 3) == "www")
                {
                    this.sitehost = new Uri("http://" + textBox1.Text);
                }
                else if (textBox1.Text.Substring(0, 4) == "http")
                {
                    this.sitehost = new Uri(textBox1.Text);
                }
                else
                {
                    this.sitehost = new Uri("http://" + textBox1.Text + "/");
                }

                /**************************************************/
                /* COMPRUEBO SI LA URL PRINCIPAL TIENE REDIRECCION*/
                if (redirectsoption == true)
                {
                    var doc = new HtmlAgilityPack.HtmlDocument();
                    MyWebClient data = new MyWebClient(); //.DownloadString(url);
                    data.Headers["User-Agent"] = "Mozilla/6.0 (Windows NT 10.0; rv:36.0) Gecko/20100101 Firefox/65.0.1";
                    try
                    {
                        doc.LoadHtml(data.DownloadString(this.sitehost.ToString()));
                    }
                    catch (WebException ex)
                    {
                        //return;
                    }
                    catch (Exception ex)
                    {
                        //return;
                    }
                    if (data.redir != null && data.redir != sitehost.AbsoluteUri.ToString())
                    {
                        this.sitehost = new Uri(data.redir);
                    }
                }
                /****************************************************/
                /*desactivo opciones del panel*/
                checkBox2.Enabled = false;
                checkBox3.Enabled = false;
                getsitemap_checkBox.Enabled = false;
                textBox1.Enabled = false;
                numericUpDown1.Enabled = false;


                selectdepth = Convert.ToInt32(numericUpDown1.Value);
                if (!this.checkBox3.Checked)
                {

                    templist.Clear();
                    int deepcount = 1;
                    if (selectdepth == 1)
                    {
                        this.crawl_page(sitehost.ToString(), 1);

                    }
                    else
                    {
                        deepcount = 2;
                        this.crawl_page(sitehost.ToString(), 0);
                    }

                    int deepindex = templist.Count;
                    int inicio = 0;
                    for (int index2 = deepcount; index2 <= selectdepth; index2++)
                    {
                        for (int index = inicio; index < deepindex; index++)
                        {
                            if (index > 0 && ((templist.Count * index) / templist.Count) > 0)
                            {
                                this.progressBar1.Maximum = templist.Count;
                                this.progressBar1.Value = (templist.Count * index) / templist.Count;
                            }
                            if (index2 == selectdepth)
                            {
                                this.crawl_page(templist[index], 1);
                            }

                            else
                            {
                                this.crawl_page(templist[index], 0);
                            }


                        }
                        inicio = deepindex;
                        deepindex = templist.Count;
                    }

                    //button2.Visible = false;

                }
                else
                {
                    templist.Clear();


                        this.crawl_page(sitehost.ToString(), 0);
                    //this.slow_mode_crawl(sitehost.ToString(), 0);

                    for (int index = 0; index < templist.Count; index++)
                    {
                        if (index > 0 && ((templist.Count * index) / templist.Count) > 0)
                        {
                            this.progressBar1.Maximum = templist.Count;
                            this.progressBar1.Value = (templist.Count * index) / templist.Count;
                        }

                        this.crawl_page(templist[index], 0);
                        //this.slow_mode_crawl(templist[index], 0);
                    }
                    button2.Visible = false;
                }
                UseWaitCursor = false;
                this.Cursor = Cursors.Default;
                /*panelPrincipal.Enabled = true;*/
                button1.Text = "GO!";
                progressBar1.Value = progressBar1.Maximum;

                /*activo todo el panel de nuevo; sustituir con un panel para mas facilidad*/
                checkBox2.Enabled = true;
                checkBox3.Enabled = true;
                getsitemap_checkBox.Enabled = true;
                textBox1.Enabled = true;
                if (!checkBox3.Checked)
                {
                    numericUpDown1.Enabled = true;
                }

                this.Refresh();
                finishcrawlprosses();
       
        }




        
        /*************************************************************/
        /*              Carga el hilo de el email explorer           */
        /************************************************************/
        private void CargarEmailHilo()
        {
            //UseWaitCursor = true; //ICONO DE ESPERA EN EL PUNTERO

            /****************************************************/
            /*desactivo opciones del panel*/
            checkBox2.Enabled = false;
            checkBox3.Enabled = false;
            getsitemap_checkBox.Enabled = false;
            textBox1.Enabled = false;
            numericUpDown1.Enabled = false;
            var selectdepth = 0;
            selectdepth = Convert.ToInt32(numericUpDown1.Value);
            foreach (Uri names in listBox1.Items)
            {
                //templist.Add(names.ToString());

                /*panelPrincipal.Enabled = false;*/
                /*Podemos sustituir con un panel para mas facilidad y menos lineas*/

                /*
                if (getsitemap_checkBox.Checked == true)
                {
                    readsitemaxml(templist[0]); //leo el sitemap.xml
                }
                */
                if (names.ToString().Substring(0, 3) == "www")
                {
                    this.sitehost = new Uri("http://" + names.ToString());
                }
                else if (names.ToString().Substring(0, 4) == "http")
                {
                    this.sitehost = new Uri(names.ToString());
                }
                else
                {
                    this.sitehost = new Uri("http://" + names.ToString() + "/");
                }

                /**************************************************/
                /* COMPRUEBO SI LA URL PRINCIPAL TIENE REDIRECCION*/
                if (redirectsoption == true)
                {
                    var doc = new HtmlAgilityPack.HtmlDocument();
                    MyWebClient data = new MyWebClient(); //.DownloadString(url);
                    data.Headers["User-Agent"] = "Mozilla/6.0 (Windows NT 10.0; rv:36.0) Gecko/20100101 Firefox/65.0.1";
                    try
                    {
                        doc.LoadHtml(data.DownloadString(this.sitehost.ToString()));
                    }
                    catch (WebException ex)
                    {
                        //return;
                    }
                    catch (Exception ex)
                    {
                        //return;
                    }
                    if (data.redir != null && data.redir != sitehost.AbsoluteUri.ToString())
                    {
                        this.sitehost = new Uri(data.redir);
                    }
                }

                if (!this.checkBox3.Checked)
                {

                    templist.Clear();
                    int deepcount = 1;
                    if (selectdepth == 1)
                    {
                        this.emailexplorer(sitehost.ToString(), 1);

                    }
                    else
                    {
                        deepcount = 2;
                        this.emailexplorer(sitehost.ToString(), 0);
                    }

                    int deepindex = templist.Count;
                    int inicio = 0;
                    for (int index2 = deepcount; index2 <= selectdepth; index2++)
                    {
                        for (int index = inicio; index < deepindex; index++)
                        {
                            if (index > 0 && ((templist.Count * index) / templist.Count) > 0)
                            {
                                this.progressBar1.Maximum = templist.Count;
                                this.progressBar1.Value = (templist.Count * index) / templist.Count;
                            }
                            if (index2 == selectdepth)
                            {
                                this.emailexplorer(templist[index], 1);
                            }

                            else
                            {
                                this.emailexplorer(templist[index], 0);
                            }


                        }
                        inicio = deepindex;
                        deepindex = templist.Count;
                    }

                    //button2.Visible = false;

                }
                else
                {
                    templist.Clear();
                    this.emailexplorer(sitehost.ToString(), 0);
                    //this.slow_mode_crawl(sitehost.ToString(), 0);

                    for (int index = 0; index < templist.Count; index++)
                    {
                        if (index > 0 && ((templist.Count * index) / templist.Count) > 0)
                        {
                            this.progressBar1.Maximum = templist.Count;
                            this.progressBar1.Value = (templist.Count * index) / templist.Count;
                        }

                        this.emailexplorer(templist[index], 0);
                        //this.slow_mode_crawl(templist[index], 0);
                    }
                    //button2.Visible = false;
                }
            }
                UseWaitCursor = false;
                this.Cursor = Cursors.Default;
                progressBar1.Value = progressBar1.Maximum;

                /*activo todo el panel de nuevo; sustituir con un panel para mas facilidad*/
                checkBox2.Enabled = true;
                checkBox3.Enabled = true;
                getsitemap_checkBox.Enabled = true;
                textBox1.Enabled = false;
                if (!checkBox3.Checked)
                {
                    numericUpDown1.Enabled = true;
                }
            
                this.Refresh();
                finishcrawlprosses();
            
        }





        private void finishcrawlprosses()
        {
            //MessageBox.Show("Crawl Job Finish" + " Skiped: " + errcount, "RDSCrawler",
            MessageBox.Show("Crawl Job Finish", "RDSCrawler",
            MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            
            if (button1.Text == "GO!")
            {
                this.hilo = new Thread(CargarDatosHilo);
                this.hilo.Start();
                if (checkBox2.Checked)
                {
                    button2.Visible = true;
                }
                else
                {
                    button2.Visible = false;
                }
                
                button1.Text = "STOP";

            }
            else if(button1.Text == "STOP")
            {
                this.hilo.Abort();
                UseWaitCursor = false;
                this.Cursor = Cursors.Default;
                /*panelPrincipal.Enabled = true;*/
                textBox1.Enabled = true;


                checkBox2.Enabled = true;
                getsitemap_checkBox.Enabled = true;
                checkBox3.Enabled = true;
                textBox1.Enabled = true;
                
                getsitemap_checkBox.Enabled = true;
                if (!checkBox3.Checked)
                {
                    numericUpDown1.Enabled = true;
                }





                this.progressBar1.Value = 0;
                this.Refresh();
                button1.Text = "GO!";

            } else if(button1.Text == "Explore @")
            {
                //this.emailexplorer();
                
                numericUpDown1.Enabled = false;
                form2.clearlist();
                listBoxHidden.Items.Clear(); //limpia el listbox oculto este listbox solo es para testing
                this.hilo = new Thread(CargarEmailHilo);
                this.hilo.Start();
            }


        }


        private void exportToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            /*OpenFileDialog open = new OpenFileDialog();
            open.Filter = "CSV Files(*.csv)|*.csv";
            open.Title = "csv File";
            if (open.ShowDialog() == DialogResult.OK)
            {
                var ruta = open.FileName;
            }
            open.Dispose();*/
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Csv file|*.csv";
            saveFileDialog1.Title = "Save an Csv File";
            saveFileDialog1.ShowDialog();
            // If the file name is not an empty string open it for saving.  
            if (saveFileDialog1.FileName != "")
            {
                // Saves the Image via a FileStream created by the OpenFile method.  
                /*System.IO.FileStream fs =
                   (System.IO.FileStream)saveFileDialog1.OpenFile();*/
                // Saves the Image in the appropriate ImageFormat based upon the  
                // File type selected in the dialog box.  
                // NOTE that the FilterIndex property is one-based.  
                /*switch (saveFileDialog1.FilterIndex)
                {
                    case 1:
                        this.button2.Image.Save(fs,
                           System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;

                    case 2:
                        this.button2.Image.Save(fs,
                           System.Drawing.Imaging.ImageFormat.Bmp);
                        break;

                    case 3:
                        this.button2.Image.Save(fs,
                           System.Drawing.Imaging.ImageFormat.Gif);
                        break;
                }*/
                using (StreamWriter myOutputStream = new StreamWriter(saveFileDialog1.FileName))
                {
                    foreach (var item in listBox1.Items)
                    {
                        myOutputStream.WriteLine(item.ToString());
                    }
                }


                /*fs.Close();*/
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            form2.ShowDialog();
            //form2.Dispose();
        }

        private void getEmailFromUrlListToolStripMenuItem_Click(object sender, EventArgs e)
        {
          
            Form3 form3 = new Form3();
            //Form2 form3 = new Form2();
            if (form3.ShowDialog(this) == DialogResult.OK)
            {
                //listBox1.Items.Add(form3.getItem());
                ListBox temporallist = new ListBox();
                temporallist = form3.getItem();
                //Uri urlbase;
                foreach (var item in temporallist.Items)
                {
                    try
                    {
                        if (item.ToString().Substring(0, 3) == "www")
                        {
                        listBox1.Items.Add (sitehost = new Uri("http://" + item.ToString()));
                        }
                        else if (item.ToString().Substring(0, 4) == "http")
                        {
                        listBox1.Items.Add(sitehost = new Uri(item.ToString()));
                        }
                        else
                        {
                        listBox1.Items.Add(sitehost = new Uri("http://" + item.ToString() ));
                        }

                        //sitehost = new Uri(item.ToString());
                        //listBox1.Items.Add(sitehost = new Uri(item.ToString()));
                        
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("There are an incorrect Url Format", "RDSCrawler",
                            MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            return;
                        }
  

                }
                form3.Close();
                //disable crawler controls
                checkBox3.Checked = false;
                checkBox3.Enabled = true;
                checkBox2.Enabled = false;
                getsitemap_checkBox.Enabled = false;
                textBox1.Enabled = false;
                
                numericUpDown1.Enabled = true;
                button1.Text = "Explore @";
                button2.Visible = true;
            }
            form3.Close();
            form3.Dispose();
            }

        private void newCrawlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            form2.clearlist();
            textBox1.Clear();
            numericUpDown1.Value = 1;

            /*

            if (button1.Text == "GO!")
            {
                this.hilo = new Thread(CargarDatosHilo);
                this.hilo.Start();
                if (checkBox2.Checked)
                {
                    button2.Visible = true;
                }
                else
                {
                    button2.Visible = false;
                }

                button1.Text = "STOP";

            }*/
            if (button1.Text == "STOP")
            {
                

                button1.Text = "GO!";

            }
            if (button1.Text == "Explore @")
            {
                
                button1.Text = "GO!";
                
            }
            if (this.hilo != null)
            {
                this.hilo.Abort();
            }
            /*if (checkBox3.Checked)
            {
                checkBox3.Checked = false;
            }*/

            UseWaitCursor = false;
            this.Cursor = Cursors.Default;
            /*panelPrincipal.Enabled = true;*/
            checkBox2.Enabled = true;
            checkBox2.Checked = false;
            checkBox3.Enabled = true;
            checkBox3.Checked = false;
            getsitemap_checkBox.Enabled = true;
            getsitemap_checkBox.Checked = false;
            textBox1.Enabled = true;
            numericUpDown1.Enabled = true;
            button2.Visible = false;
            this.progressBar1.Value = 0;
            this.Refresh();
      
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                numericUpDown1.Enabled = false;
            }
            else
            {
                numericUpDown1.Enabled = true;
            }
            
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }


        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            optionsform.ShowDialog();
        }

        public void loadsettings()
        {
                AppSettingsReader crawlersetings = new AppSettingsReader();
                htmlpagesoption = (bool)crawlersetings.GetValue("htmlpages", typeof(bool));
                fullsitecrawloption = (bool)crawlersetings.GetValue("fullsitecrawl", typeof(bool));
                redirectsoption = (bool)crawlersetings.GetValue("redirects", typeof(bool));
                getexurlsoption = (bool)crawlersetings.GetValue("getexurls", typeof(bool));
                rendermodeoption = (bool)crawlersetings.GetValue("rendermode", typeof(bool));
                ignoreresourcesoption = (bool)crawlersetings.GetValue("ignoreresources", typeof(bool));
            
        }

        public static void savesettings()
        {
            rdsettings.setvalue("htmlpages", htmlpagesoption);
            rdsettings.setvalue("fullsitecrawl", fullsitecrawloption);
            rdsettings.setvalue("redirects", redirectsoption);
            rdsettings.setvalue("getexurls", getexurlsoption);
            rdsettings.setvalue("rendermode", rendermodeoption);
            rdsettings.setvalue("ignoreresources", ignoreresourcesoption);
        }

        /*Leo el archivo sitemap.xml si esxiste y pongo todas las urls en cola*/
        private void readsitemaxml(string url)
        {
            if (url == null)
            {
                return;
            }

            string urltemp = url;

            if (textBox1.Text.Substring(0, 3) == "www")
            {
                if (urltemp.Substring(urltemp.Length - 1) == "/")
                {
                    urltemp = "http://" + url;
                }
                else
                {
                    urltemp = "http://" + url + "/";
                }
            }
            else if (textBox1.Text.Substring(0, 4) == "http")
            {
                if (urltemp.Substring(urltemp.Length - 1) == "/")
                {
                    urltemp = url;
                }
                else
                {
                    urltemp = url + "/";
                }
            }
            else
            {
                if (urltemp.Substring(urltemp.Length - 1) == "/"){
                    urltemp = "http://" + url;
                }
                else {
                    urltemp = "http://" + url + "/";
                }
                
            }

 
            var doc = new HtmlAgilityPack.HtmlDocument();
            MyWebClient data = new MyWebClient(); //.DownloadString(url);
            data.Headers["User-Agent"] = "Mozilla/6.0 (Windows NT 10.0; rv:36.0) Gecko/20100101 Firefox/65.0.1";
            try
            {
                doc.LoadHtml(data.DownloadString(urltemp + "sitemap.xml"));
                foreach (HtmlNode nodeRss in doc.DocumentNode.SelectNodes("urlset"))
                {
                    foreach (HtmlNode aNode2 in nodeRss.SelectNodes("url"))
                    {
                      templist.Add(aNode2.SelectSingleNode("loc").InnerText);
                    }
                }
            }
            catch (WebException ex)
            {
               return;
            }
            catch (Exception ex)
            {
               return;
            }
                        
        }

            private void updatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void checkUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //updater.DoUpdate();
        }

        private void ignorefiles_checkBox_CheckedChanged(object sender, EventArgs e)
        {

        }
        /******************************************/



        /******************************************/
    }
    }

