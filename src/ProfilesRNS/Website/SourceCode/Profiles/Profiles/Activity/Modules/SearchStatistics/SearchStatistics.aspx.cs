using System;
using System.Xml;

namespace Profiles.Activity
{
    public partial class SearchStatistics : System.Web.UI.Page
    {
        private Profiles.Framework.Template masterpage;

        public SearchStatistics() { }

        protected void Page_Load(object sender, EventArgs e)
        {
            masterpage = (Framework.Template)base.Master;

            this.LoadAssets();
            this.LoadPresentationXML();

            masterpage.Tab = Request.QueryString["tab"];
            masterpage.PresentationXML = this.PresentationXML;
        }

        private void LoadAssets()
        {
        }

        public void LoadPresentationXML()
        {
            string presentationxml = string.Empty;

            presentationxml = System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "/Activity/PresentationXML/SearchStatistics.xml");

            this.PresentationXML = new XmlDocument();
            this.PresentationXML.LoadXml(presentationxml);

            Framework.Utilities.DebugLogging.Log(presentationxml);
        }

        public XmlDocument PresentationXML { get; set; }
    }
}