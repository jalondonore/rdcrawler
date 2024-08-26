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

namespace RdCrawler2
{
    public partial class Form1
    {

        //se ejecuta cuando la pagina ha cargado completa
        private void slow_mode_crawl(string url, int depth)
        {

            this.Invoke(new Action(() => {
                this.geckoWebBrowser1.Navigate("about:blank");
                //this.geckoWebBrowser1.Navigate("about:memory");
                this.geckoWebBrowser1.Document.Cookie = "";
                this.geckoWebBrowser1.Stop();
                GC.Collect();
                GC.WaitForPendingFinalizers();

                var _memoryService = Xpcom.GetService<nsIMemory>("@mozilla.org/xpcom/memory-service;1");
                _memoryService.HeapMinimize(true);
                nsICookieManager CookieMan;
                CookieMan = Xpcom.GetService<nsICookieManager>("@mozilla.org/cookiemanager;1");
                CookieMan = Xpcom.QueryInterface<nsICookieManager>(CookieMan);
                CookieMan.RemoveAll();


            }));

            geckoWebBrowser1.NavigateFinishedNotifier.BlockUntilNavigationFinished();




            this.Invoke(new Action(() => {
                this.geckoWebBrowser1.Navigate(url);

            }));
            /*geckoWebBrowser1.Navigate(url);*/
            /*
                Action<string> action = url2 => geckoWebBrowser1.Navigate(url2);
                this.geckoWebBrowser1.Invoke(action, new object[] { url });*/
            geckoWebBrowser1.NavigateFinishedNotifier.BlockUntilNavigationFinished();
            string geckourl = "";
            this.Invoke(new Action(() => {
                geckourl = this.geckoWebBrowser1.Url.ToString();
            }));


            //geckoWebBrowser1.Navigate(url);
            //geckoWebBrowser1.NavigateFinishedNotifier.BlockUntilNavigationFinished();
            //geckoWebBrowser1.Stop();
            string urltemp = "";
            string emails = "";
            Uri urlbase;
            Uri urlbase2;

            urlbase = new Uri(textBox1.Text);

            if (url != geckourl)
            {
                if (!listBox1.Items.Contains(url))
                {
                    listBox1.Items.Add(geckourl);
                }
            }
            else
            {
                listBox1.Items.Add(geckourl);
            }

            //GeckoElementCollection links;

            this.Invoke(new Action(() => {
                GeckoElementCollection links = this.geckoWebBrowser1.Document.GetElementsByTagName("a");
                this.geckoWebBrowser1.Navigate("about:blank");
                this.geckoWebBrowser1.Document.Cookie = "";
                this.geckoWebBrowser1.Stop();
                //geckoWebBrowser1.Dispose();
                //GeckoElementCollection links = this.geckoWebBrowser1.Document.GetElementsByTagName("a");
                foreach (GeckoHtmlElement itemlink in links)
                {
                    // Get the value of the HREF attribute
                    /*string hrefValue = link.GetAttributeValue("href", string.Empty);
                    HtmlAttribute Att = link.Attributes["href"];*/

                    string Att = itemlink.GetAttribute("href");
                    if (Att == null)
                    {
                        continue;
                    }
                    if (Att.Contains("mailto:") && Att.Contains("@"))
                    {

                        urltemp = Att;
                    }
                    else if (Att.Contains("#"))
                    {
                        string[] substring = Att.Split('#');
                        if (substring[0].Length >= 1)
                        {
                            if (!listBox1.Items.Contains(urlbase.Scheme + Uri.SchemeDelimiter + urlbase.Host + substring[0]))
                            {
                                urltemp = urlbase.Scheme + Uri.SchemeDelimiter + urlbase.Host + substring[0];
                            }

                        }
                    }
                    else if ((Att.Length >= 1) && (Att.Substring(0, 1) == "/"))
                    {
                        if (!listBox1.Items.Contains(urlbase.Scheme + Uri.SchemeDelimiter + urlbase.Host + Att))
                        {
                            urltemp = sitehost.Scheme + Uri.SchemeDelimiter + urlbase.Host + Att;

                        }


                    }
                    else
                    {

                        if ((Att.Length >= 3) && (Att.Substring(0, 3) == "www"))
                        {
                            if (!listBox1.Items.Contains(urlbase.Scheme + Uri.SchemeDelimiter + Att))
                            {
                                urltemp = sitehost.Scheme + Uri.SchemeDelimiter + Att;
                            }

                        }
                        if ((Att.Length >= 4) && (Att.Substring(0, 4) == "http"))
                        {
                            if (!listBox1.Items.Contains(Att))
                            {
                                urltemp = Att.Replace("http", sitehost.Scheme);
                                //urltemp = Att.Value;

                            }

                        }
                        if ((Att.Length >= 5) && (Att.Substring(0, 5) == "https"))
                        {
                            if (!listBox1.Items.Contains(Att))
                            {
                                urltemp = Att.Replace("https", sitehost.Scheme);
                                //urltemp = Att.Value;

                            }

                        }

                    }
                    if (urltemp.Contains("?"))
                    {
                        String[] arreglo = urltemp.Split('?');
                        urltemp = arreglo[0];
                    }
                    if (!listBox1.Items.Contains(urltemp) && (urltemp != "") && (urltemp != null) && (!urltemp.Contains("mailto:")))
                    {
                        bool result2 = Uri.TryCreate(urltemp, UriKind.Absolute, out urlbase2)
                            && (urlbase2.Scheme == Uri.UriSchemeHttp || urlbase2.Scheme == Uri.UriSchemeHttps);
                        if ((result2 == true) && (urlbase2.Host == sitehost.Host) && (getexurlsoption == false)) //compruebo si la url es valida y si pertenece al mismo dominio
                        {

                            if (ignoreresourcesoption == true) //si la opcion de ignorar archivos que no sean paginas esta activa
                            {
                                string contiene = urltemp.Substring(urltemp.Length - 4);
                                if (contiene.Contains("."))
                                {
                                    if (contiene == ".php" || contiene == ".html" || contiene == ".htm" || contiene == ".asp" || contiene == ".aspx" || contiene == ".jsp")
                                    {
                                        if (!templist.Contains(urltemp))
                                        {
                                            templist.Add(urltemp);
                                        }
                                        if (depth == 1 && (!listBox1.Items.Contains(urltemp)))
                                        {
                                            if (!listBox1.Items.Contains(urltemp))
                                            {
                                                listBox1.Items.Add(urltemp);
                                            }
                                        }
                                    }

                                }
                                else
                                {
                                    if (!templist.Contains(urltemp))
                                    {
                                        templist.Add(urltemp);
                                    }
                                    if (depth == 1 && (!listBox1.Items.Contains(urltemp)))
                                    {
                                        if (!listBox1.Items.Contains(urltemp))
                                        {
                                            listBox1.Items.Add(urltemp);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (!templist.Contains(urltemp))
                                {
                                    templist.Add(urltemp);
                                }
                                if (depth == 1 && (!listBox1.Items.Contains(urltemp)))
                                {
                                    if (!listBox1.Items.Contains(urltemp))
                                    {
                                        listBox1.Items.Add(urltemp);
                                    }
                                }
                            }
                        }
                        else if ((result2 == true) && (urlbase2.Host != sitehost.Host) && (getexurlsoption == true)) //si la opcion de aceptar todos los dominios esta activa
                        {
                            if (ignoreresourcesoption == true) //si la opcion de ignorar archivos que no sean paginas esta activa
                            {
                                string contiene = urltemp.Substring(urltemp.Length - 4);
                                if (contiene.Contains("."))
                                {
                                    if (contiene == ".php" || contiene == ".html" || contiene == ".htm" || contiene == ".asp" || contiene == ".aspx" || contiene == ".jsp")
                                    {
                                        if (!templist.Contains(urltemp))
                                        {
                                            templist.Add(urltemp);
                                        }
                                        if (depth == 1 && (!listBox1.Items.Contains(urltemp)))
                                        {
                                            if (!listBox1.Items.Contains(urltemp))
                                            {
                                                listBox1.Items.Add(urltemp);
                                            }
                                        }
                                    }

                                }
                                else
                                {
                                    if (!templist.Contains(urltemp))
                                    {
                                        templist.Add(urltemp);
                                    }
                                    if (depth == 1 && (!listBox1.Items.Contains(urltemp)))
                                    {
                                        if (!listBox1.Items.Contains(urltemp))
                                        {
                                            listBox1.Items.Add(urltemp);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (!templist.Contains(urltemp))
                                {
                                    templist.Add(urltemp);
                                }
                                if (depth == 1 && (!listBox1.Items.Contains(urltemp)))
                                {
                                    if (!listBox1.Items.Contains(urltemp))
                                    {
                                        listBox1.Items.Add(urltemp);
                                    }
                                }
                            }
                        }
                        /*
                        if ((urltemp.Substring(urltemp.Length - 1) == "/") || (urltemp.Substring(urltemp.Length - 3) == "php") || (urltemp.Substring(urltemp.Length - 4) == "html") || (urltemp.Substring(urltemp.Length - 3) == "htm") || (urltemp.Substring(urltemp.Length - 4) == "aspx") || (urltemp.Substring(urltemp.Length - 3) == "jsp") || (urltemp.Substring(urltemp.Length - 3) == "asp")){
                            //crawl_page(urltemp, depth - 1);
                            if (!templist.Contains(urltemp))
                            {
                                templist.Add(urltemp);
                            }
                            if (depth == 1 && (!listBox1.Items.Contains(urltemp)))
                            {
                                listBox1.Items.Add(urltemp);
                            }

                        }  *//*temporal*/

                        /************************************************************/
                        /******************OJO TEMPORAL *//////////////////////////
                                                          //crawl_page(urltemp, depth - 1);     

                    }
                    if (checkBox2.Checked == true && urltemp.Contains("mailto:") && (urltemp.Contains("@")))
                    {

                        //Att.Value = System.Uri.EscapeDataString(Att.Value);

                        const string MatchEmailPattern = @"(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
                          + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
                            + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
                          + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})";
                        Regex rx = new Regex(MatchEmailPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                        string text = @Att;
                        MatchCollection matches = rx.Matches(text);
                        // Número de coincidencias
                        int noOfMatches = matches.Count;
                        // Por cada resultado
                        foreach (Match match in matches)
                        {
                            //Impresión de los correos
                            emails = match.Value.ToString();
                        }

                        form2.addinlist(emails, url);


                    }

                }
                return;
            }));


            /*
            this.Invoke(new Action(() => {
                links.dispose
                }))
            geckoWebBrowser1.NavigateFinishedNotifier.BlockUntilNavigationFinished();*/



        }
    }
}
