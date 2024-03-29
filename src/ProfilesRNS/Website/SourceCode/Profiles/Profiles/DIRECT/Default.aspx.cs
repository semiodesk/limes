﻿/*  
 
    Copyright (c) 2008-2012 by the President and Fellows of Harvard College. All rights reserved.  
    Profiles Research Networking Software was developed under the supervision of Griffin M Weber, MD, PhD.,
    and Harvard Catalyst: The Harvard Clinical and Translational Science Center, with support from the 
    National Center for Research Resources and Harvard University.


    Code licensed under a BSD License. 
    For details, see: LICENSE.txt 
  
*/
using System;
using System.Xml;


namespace Profiles.DIRECT
{
    public partial class Default : System.Web.UI.Page
    {     

        protected void Page_Load(object sender, EventArgs e)
        {

            Profiles.Framework.Template masterpage;
            masterpage = (Framework.Template)base.Master;
            LoadPresentationXML();
            masterpage.PresentationXML = this.PresentationXML;
            masterpage.RDFData = null;
            
            if(Request.QueryString["searchrequest"]!=null)
                masterpage.SearchRequest = Request.QueryString["searchrequest"];

            masterpage.RDFNamespaces = null;

        }

        public void LoadPresentationXML()
        {
            string presentationxml = string.Empty;

            presentationxml = System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "/Direct/PresentationXML/DirectPresentation.xml");


            this.PresentationXML = new XmlDocument();
            this.PresentationXML.LoadXml(presentationxml);
            Framework.Utilities.DebugLogging.Log(presentationxml);
        }
        public XmlDocument PresentationXML { get; set; }



    }
}
