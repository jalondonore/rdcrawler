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

        private void crawl_page_email_wDeep(string url, int depth)
        {
            if (url == null)
            {
                return;
            }
            String emails = "";
            Uri urlbase;
            if (!Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out urlbase)) //debo comprobar por que hay url malformed
            {
                return;
            }
            //= new Uri(url);
            //Uri sitehost = new Uri(textBox1.Text); /*tratar de optimizar*/
            if (listBoxHidden.Items.Contains(url) || depth < 0 || urlbase.Host != sitehost.Host)
            {
                return;
            }
            else
            {
                listBoxHidden.Items.Add(url);
                string urltemp = "";

                var doc = new HtmlAgilityPack.HtmlDocument();
                MyWebClient data = new MyWebClient(); //.DownloadString(url);
                data.Headers["User-Agent"] = "Mozilla/6.0 (Windows NT 10.0; rv:36.0) Gecko/20100101 Firefox/65.0.1";
                try
                {
                    //data.DownloadString(url);// DownloadString(url);
                    doc.LoadHtml(data.DownloadString(url));

                    if ((data.redir != url) && (!listBoxHidden.Items.Contains(url)))
                    {
                        listBoxHidden.Items.Add(data.redir);
                    }

                }
                catch (WebException ex)
                {
                    return;
                }


                //Uri urlbase = new Uri(url);
                if (doc.DocumentNode.SelectNodes("//a[@href]") == null)
                {
                    return;
                }
                foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]"))
                {
                    // Get the value of the HREF attribute
                    string hrefValue = link.GetAttributeValue("href", string.Empty);
                    HtmlAttribute Att = link.Attributes["href"];


                    if (Att.Value.Contains("mailto:") && Att.Value.Contains("@"))
                    {
                        //form2.addinlist(Att.Value);
                        urltemp = Att.Value;
                    }
                    else if (Att.Value.Contains("#"))
                    {
                        string[] substring = Att.Value.Split('#');
                        if (substring[0].Length >= 1)
                        {
                            if (!listBoxHidden.Items.Contains(urlbase.Scheme + Uri.SchemeDelimiter + urlbase.Host + substring[0]))
                            {
                                //crawl_page(urlbase.Scheme + Uri.SchemeDelimiter + urlbase.Host + substring[0], depth - 1);
                                //listBox1.Items.Add(urlbase.Scheme + Uri.SchemeDelimiter + urlbase.Host + substring[0]);
                                urltemp = urlbase.Scheme + Uri.SchemeDelimiter + urlbase.Host + substring[0];
                            }

                        }
                    }
                    else if ((Att.Value.Length >= 1) && (Att.Value.Substring(0, 1) == "/"))
                    {
                        if (!listBoxHidden.Items.Contains(urlbase.Scheme + Uri.SchemeDelimiter + urlbase.Host + Att.Value))
                        {
                            //crawl_page(urlbase.Scheme + Uri.SchemeDelimiter + urlbase.Host + Att.Value, depth - 1);
                            //listBox1.Items.Add(urlbase.Scheme + Uri.SchemeDelimiter + urlbase.Host + Att.Value);
                            urltemp = urlbase.Scheme + Uri.SchemeDelimiter + urlbase.Host + Att.Value;
                        }


                    }
                    else
                    {

                        if ((Att.Value.Length >= 3) && (Att.Value.Substring(0, 3) == "www"))
                        {
                            if (!listBoxHidden.Items.Contains(urlbase.Scheme + Uri.SchemeDelimiter + Att.Value))
                            {
                                urltemp = sitehost.Scheme + Uri.SchemeDelimiter + Att.Value;
                            }

                        }
                        if ((Att.Value.Length >= 4) && (Att.Value.Substring(0, 4) == "http"))
                        {
                            if (!listBoxHidden.Items.Contains(Att.Value))
                            {
                                urltemp = Att.Value.Replace("http", sitehost.Scheme);
                                //urltemp = Att.Value;

                            }

                        }
                        if ((Att.Value.Length >= 5) && (Att.Value.Substring(0, 5) == "https"))
                        {
                            if (!listBoxHidden.Items.Contains(Att.Value))
                            {
                                urltemp = Att.Value.Replace("https", sitehost.Scheme);
                                //urltemp = Att.Value;

                            }

                        }
                    }
                    if (!listBoxHidden.Items.Contains(urltemp) && (urltemp != "") && (urltemp != null) && (!urltemp.Contains("mailto:")))
                    {

                        if ((urltemp.Substring(urltemp.Length - 1) == "/") || (urltemp.Substring(urltemp.Length - 3) == "php") || (urltemp.Substring(urltemp.Length - 4) == "html") || (urltemp.Substring(urltemp.Length - 3) == "htm") || (urltemp.Substring(urltemp.Length - 4) == "aspx") || (urltemp.Substring(urltemp.Length - 3) == "jsp") || (urltemp.Substring(urltemp.Length - 3) == "asp"))
                        {
                            crawl_page_email_wDeep(urltemp, depth - 1);
                        }


                    }
                    else if (urltemp.Contains("mailto:") && (urltemp.Contains("@")))
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

                }

            }

        }

    }
}
