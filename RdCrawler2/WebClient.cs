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
        class MyWebClient : WebClient
        {
            //private Form1 form1 = new Form1();
            public string redir;
            protected override WebRequest GetWebRequest(Uri address)
            {
                HttpWebRequest request = base.GetWebRequest(address) as HttpWebRequest;
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

                if (redirectsoption == true)
                {
                    try
                    {
                        request.MaximumAutomaticRedirections = 10;
                        request.AllowAutoRedirect = true;
                        HttpWebResponse redir2 = (HttpWebResponse)request.GetResponse();
                        redir = redir2.ResponseUri.AbsoluteUri.ToString();
                    }
                    catch (WebException ex)
                    {
                        //return request;
                    }
                    catch (System.StackOverflowException ex2)
                    {
                        //return request;
                    }
                }

                /*
                if(form1.redirectsoption == true && (redir != null))
                {
                     try
                     {
                         request.MaximumAutomaticRedirections = 1;
                         request.AllowAutoRedirect = true;
                         HttpWebResponse redir2 = (HttpWebResponse)request.GetResponse();
                         redir = redir2.ResponseUri.AbsoluteUri;
                     }
                     catch (WebException ex)
                     {
                         return request;
                     }
                     catch (System.StackOverflowException ex2)
                     {
                         return request;
                     }
                 }
                */
                return request;
            }

        }
    }
}
