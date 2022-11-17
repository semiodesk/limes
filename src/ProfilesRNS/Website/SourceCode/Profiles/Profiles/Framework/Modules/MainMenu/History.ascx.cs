
using System;
using System.Collections.Generic;
using System.Xml;
using Profiles.Framework.Utilities;


namespace Profiles.Framework.Modules.MainMenu
{
    public partial class History : System.Web.UI.UserControl
    {
        UserHistory uh;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.RDFData != null)
            {
                uh = new UserHistory();

                if (this.RDFData.InnerXml != "")
                {
                    RecordHistory();
                }

                if (uh.GetItems() != null)
                {
                    DrawProfilesModule();
                }
                else
                {
                    lblHistoryItems.Text = @"
<a href='#' class='dropdown-toggle dropdown-no-indicator' data-toggle='dropdown' role='button' aria-expanded='false' title='Navigation history is empty.'>
    <i class='fa-solid fa-clock-rotate-left'></i> <span class='badge d-none'>0</span>
</a>
<ul id='dropdown-menu-history' class='dropdown-menu dropdown-menu-right'>
    <li><a class='dropdown-item'>No items in history</a></li>
</ul>";
                }
            }
        }
        
        private void DrawProfilesModule()
        {
            int count = 0;
            int total = uh.GetItems().Count;

            lblHistoryItems.Text = $@"
<a href='#' class='dropdown-toggle dropdown-no-indicator' data-toggle='dropdown' role='button' aria-expanded='false' title='Navigation history contains {total} page(s).'>
    <i class='fa-solid fa-clock-rotate-left'></i> <span class='badge'>{total}</span>
</a>
<ul id='dropdown-menu-history' class='dropdown-menu dropdown-menu-right'>";

            foreach (HistoryItem h in uh.GetItems(5))
            {                
                lblHistoryItems.Text += "<li><a class='dropdown-item' href='" + h.URI + "'>" + h.ItemLabel + "</a></li>";                
                count++;
            }

            if (total > 1)
            {
                lblHistoryItems.Text += "<li><a class='dropdown-item' href='" + Root.Domain + "/history'>See All " + total.ToString() + " Pages</a></li>";
            }
            else if (total == 1)
            {
                lblHistoryItems.Text += "<li><a class='dropdown-item' href='" + Root.Domain + "/history'>See All Pages</a></li>";
            }

            lblHistoryItems.Text += "</ul></li>";
        }

        private void RecordHistory()
        {
            try
            {
                if (this.PresentationXML != null)
                {
                    if (this.PresentationXML.SelectSingleNode("Presentation/@PresentationClass").Value.ToLower() == "profile" && !Request.RawUrl.ToLower().Contains("/search"))
                    {
                        UserHistory uh = new UserHistory();
                        HistoryItem hi;
                        List<string> types = new List<string>();

                        foreach (XmlNode x in this.RDFData.SelectNodes("rdf:RDF/rdf:Description[1]/rdf:type/@rdf:resource", this.Namespaces))
                        {
                            if (this.RDFData.SelectSingleNode("rdf:RDF/rdf:Description[@rdf:about='" + x.Value + "']/rdfs:label", this.Namespaces) != null)
                            {
                                types.Add(this.RDFData.SelectSingleNode("rdf:RDF/rdf:Description[@rdf:about='" + x.Value + "']/rdfs:label", this.Namespaces).InnerText);
                            }
                            else
                            {
                                string[] s = x.Value.Split('/');
                                types.Add(s[s.Length - 1]);
                            }
                        }


                        if (this.RDFData.SelectSingleNode("rdf:RDF/rdf:Description/rdfs:label", this.Namespaces) != null)
                        {
                            hi = new HistoryItem(this.RDFData.SelectSingleNode("rdf:RDF/rdf:Description/rdfs:label", this.Namespaces).InnerText,
                                RDFData.SelectSingleNode("rdf:RDF/rdf:Description/@rdf:about", this.Namespaces).Value
                                , types);

                            uh.LoadItem(hi);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Framework.Utilities.DebugLogging.Log(ex.Message + " " + ex.InnerException.Message);
            }
        }
        public XmlNamespaceManager Namespaces { get; set; }
        public XmlDocument PresentationXML { get; set; }
        public XmlDocument RDFData { get; set; }


    }
}