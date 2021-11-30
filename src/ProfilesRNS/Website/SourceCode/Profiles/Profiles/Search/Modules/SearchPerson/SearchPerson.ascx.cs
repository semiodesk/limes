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
using System.Xml;
using System.Configuration;
using Profiles.Framework.Utilities;
using Profiles.Search.Utilities;


namespace Profiles.Search.Modules.SearchPerson
{
    public partial class SearchPerson : BaseModule
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public SearchPerson() { }

        public SearchPerson(XmlDocument pagedata, List<ModuleParams> moduleparams, XmlNamespaceManager pagenamespaces)
        {
            if (Convert.ToBoolean(ConfigurationSettings.AppSettings["ShowInstitutions"]) == true)
            {
                litInstitution.Text = SearchDropDowns.BuildDropdown("institution", "249", "");
            }
            else
            {
                trInstitution.Visible = false;
            }

            if (Convert.ToBoolean(ConfigurationSettings.AppSettings["ShowDivisions"]) == true)
            {
                litDivision.Text = SearchDropDowns.BuildDropdown("division", "249", "");
            }
            else
            {
                trDivision.Visible = false;
            }
        }
    }
}
