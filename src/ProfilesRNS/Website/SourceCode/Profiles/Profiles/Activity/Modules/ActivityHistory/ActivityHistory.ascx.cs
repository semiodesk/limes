/*  
 
    Copyright (c) 2008-2012 by the President and Fellows of Harvard College. All rights reserved.  
    Profiles Research Networking Software was developed under the supervision of Griffin M Weber, MD, PhD.,
    and Harvard Catalyst: The Harvard Clinical and Translational Science Center, with support from the 
    National Center for Research Resources and Harvard University.


    Code licensed under a BSD License. 
    For details, see: LICENSE.txt 
  
*/
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Profiles.Framework.Utilities;
using System.Web.UI.HtmlControls;
using System.Text;
using Profiles.Activity.Utilities;

namespace Profiles.Activity.Modules.ActivityHistory
{
    public partial class ActivityHistory : BaseModule
    {
        public ActivityHistory() { }

        public ActivityHistory(XmlDocument pagedata, List<ModuleParams> moduleparams, XmlNamespaceManager pagenamespaces)
            : base(pagedata, moduleparams, pagenamespaces)
        {
            DrawProfilesModule(); 
        }

        public void setModuleParams(List<ModuleParams> moduleparams)
        {
            base.ModuleParams = moduleparams;
        }

        public void DrawProfilesModule()
        {
            LoadAssets();

            //int count = Convert.ToInt32(base.GetModuleParamString("Show"));
            //linkSeeMore.Visible = "True".Equals(base.GetModuleParamString("SeeMore"));

            // grab a bunch of activities from the Database
            Utilities.DataIO data = new Utilities.DataIO();

            var activities = data.RenderRecentActivities(20);
            rptActivityHistory.DataSource = activities;
            rptActivityHistory.DataBind();
        }

        public void rptActivityHistory_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var html = (string)e.Item.DataItem;

            if (html != null)
            {
                var body = (Literal)e.Item.FindControl("body");
                body.Text = html;
            }
        }

        // return an empty string for false so that Javscript will interpret it correctly
        public string FixedSize()
        {
            return "True".Equals(base.GetModuleParamString("Scrolling")) ? "" : "True";
        }

        public string GetURLDomain()
        {
            return Root.Domain;
        }

        private void LoadAssets()
        {
            HtmlLink Searchcss = new HtmlLink();
            Searchcss.Href = Root.Domain + "/Activity/CSS/activity.css";
            Searchcss.Attributes["rel"] = "stylesheet";
            Searchcss.Attributes["type"] = "text/css";
            Searchcss.Attributes["media"] = "all";
            Page.Header.Controls.Add(Searchcss);

            // Inject script into HEADER
            Literal script = new Literal();
            script.Text = "<script>var _path = \"" + Root.Domain + "\";</script>";
            Page.Header.Controls.Add(script);
        }
    }
}