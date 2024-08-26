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

        private void crawl_page(string url, int depth)
        {
            if (rendermodeoption == true)
            {
                slow_mode_crawl(url, depth); // si esta activada la opcion de renderizar la pagina uso renderizacion de pagina para el crawl
                return;
            }

            string emails = "";
            //HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            if (url == null)
            {
                return;
            }

            Uri urlbase;
            Uri urlbase2;



            bool result = Uri.TryCreate(url, UriKind.Absolute, out urlbase)
                && (urlbase.Scheme == Uri.UriSchemeHttp || urlbase.Scheme == Uri.UriSchemeHttps);
            if (result == false)
            {
                return;
            }
            if ((urlbase.Host != sitehost.Host && getexurlsoption == false))
            {
                return;
            }
            if (listBox1.Items.Contains(url))
            {
                return;
            }
            else
            {

                string urltemp = "";

                var doc = new HtmlAgilityPack.HtmlDocument();
                MyWebClient data = new MyWebClient(); //.DownloadString(url);
                data.Headers["User-Agent"] = "Mozilla/6.0 (Windows NT 10.0; rv:36.0) Gecko/20100101 Firefox/65.0.1";
                string allhtmlstring = ""; //contiene todo el texto de la pagina para luego buscar emails
                try
                {
                    doc.LoadHtml(data.DownloadString(url));
                    allhtmlstring = doc.Text; //tomo el texto de la pagina
                    //compruebo si hay redireccion
                    if ((data.redir != url) && (data.redir != null) && (!listBox1.Items.Contains(url)))
                    {
                        if (!listBox1.Items.Contains(url))
                        {
                            listBox1.Items.Add(data.redir); //agrego la url de redireccion
                        }
                    }
                    else
                    {
                        if (!listBox1.Items.Contains(url))
                        {
                            listBox1.Items.Add(url); //si no hay redireccion agrego la url original a la lista
                        }

                    }

                }
                catch (WebException ex)
                {
                    /*
                    if (!listBox1.Items.Contains(url))
                    {
                        listBox1.Items.Add(url); //si ocurre un error en la redireccion, agrego la url a la lista
                    } */
                    return;
                }
                catch (Exception ex)
                {
                    //return;
                }

                //Uri urlbase = new Uri(url);
                if ((doc.DocumentNode.SelectNodes("//a[@href]") == null) || (rendermodeoption == true))
                {
                    //slow_mode_crawl(url, depth); // si no consigo ningun link pruebo con el modo de renderisado de pagina
                    return; // si no hay links en la pagina termino el proceso
                }


                foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]"))
                {
                    // Get the value of the HREF attribute
                    string hrefValue = link.GetAttributeValue("href", string.Empty);
                    HtmlAttribute Att = link.Attributes["href"];

                    if (Att.Value.Contains("mailto:") && Att.Value.Contains("@"))
                    {

                        urltemp = Att.Value;
                    }
                    else if (Att.Value.Contains("#"))
                    {
                        string[] substring = Att.Value.Split('#');
                        if (substring[0].Length >= 1)
                        {
                            if (!listBox1.Items.Contains(urlbase.Scheme + Uri.SchemeDelimiter + urlbase.Host + substring[0]))
                            {
                                urltemp = urlbase.Scheme + Uri.SchemeDelimiter + urlbase.Host + substring[0];
                            }

                        }
                    }
                    else if ((Att.Value.Length >= 1) && (Att.Value.Substring(0, 1) == "/"))
                    {
                        if (!listBox1.Items.Contains(urlbase.Scheme + Uri.SchemeDelimiter + urlbase.Host + Att.Value))
                        {
                            urltemp = sitehost.Scheme + Uri.SchemeDelimiter + urlbase.Host + Att.Value;

                        }


                    }
                    else
                    {

                        if ((Att.Value.Length >= 3) && (Att.Value.Substring(0, 3) == "www"))
                        {
                            if (!listBox1.Items.Contains(urlbase.Scheme + Uri.SchemeDelimiter + Att.Value))
                            {
                                urltemp = sitehost.Scheme + Uri.SchemeDelimiter + Att.Value;
                            }

                        }
                        if ((Att.Value.Length >= 4) && (Att.Value.Substring(0, 4) == "http"))
                        {
                            if (!listBox1.Items.Contains(Att.Value))
                            {
                                urltemp = Att.Value.Replace("http", sitehost.Scheme);
                                //urltemp = Att.Value;

                            }

                        }
                        if ((Att.Value.Length >= 5) && (Att.Value.Substring(0, 5) == "https"))
                        {
                            if (!listBox1.Items.Contains(Att.Value))
                            {
                                urltemp = Att.Value.Replace("https", sitehost.Scheme);
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
                                            listBox1.Items.Add(urltemp);
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
                                        listBox1.Items.Add(urltemp);
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
                                    listBox1.Items.Add(urltemp);
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
                                            listBox1.Items.Add(urltemp);
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
                                        listBox1.Items.Add(urltemp);
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
                                    listBox1.Items.Add(urltemp);
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

                        string text = @Att.Value;
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
                    /* busco emails en el texto de toda la pagina*/
                    const string MatchEmailPattern2 = @"(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
                    + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
                    + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
                    + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})";
                    Regex rx2 = new Regex(MatchEmailPattern2, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                    //string text = allhtmlstring;
                    MatchCollection matches2 = rx2.Matches(allhtmlstring);
                    // Número de coincidencias
                    int noOfMatches2 = matches2.Count;
                    // Por cada resultado
                    foreach (Match match in matches2)
                    {
                        //Impresión de los correos
                        emails = match.Value.ToString();
                        form2.addinlist(emails, url);

                    }




                }


            }

        }
    }
}
