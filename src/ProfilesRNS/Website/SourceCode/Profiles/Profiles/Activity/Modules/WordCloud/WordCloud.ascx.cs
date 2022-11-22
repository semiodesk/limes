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
using System.Linq;
using System.Web.UI.WebControls;
using System.Xml;
using Newtonsoft.Json;
using Profiles.Framework.Utilities;

namespace Profiles.Activity.Modules.WordCloud
{
    /// <summary>
    /// This module is used to test a presentation XML for proper module placement.
    /// </summary>
    public partial class WordCloud : BaseModule
    {
        public WordCloud() { }

        public WordCloud(XmlDocument pagedata, List<ModuleParams> moduleparams, XmlNamespaceManager pagenamespaces)
            : base(pagedata, moduleparams, pagenamespaces) { }

        protected void Page_Load(object sender, EventArgs e)
        {
            Utilities.DataIO data = new Utilities.DataIO();

            var random = new Random();

            var stats = data.GetConceptStats("PROC", 9, 5, 20).Select(x => new object[] { x.Label, random.Next(5, 10) }).ToList();
            stats.AddRange(data.GetConceptStats("DEVI", 9, 5, 20).Select(x => new object[] { x.Label, random.Next(5, 10) }));
            stats.AddRange(data.GetConceptStats("CONC", 8, 5, 20).Select(x => new object[] { x.Label, random.Next(5, 10) }));

            var script = (Literal)FindControl("script");
            script.Text = $@"
            <script type='text/javascript'>
            var canvas = $('#wordcloud-canvas');
            var options = {{
              list: {JsonConvert.SerializeObject(stats)},
              fontFamily: 'Mina, Roboto, Arial, Helvetica, sans-serif',
              color: function (word, weight) {{
                if(weight >= 9) return '#9A5AA2';
                if(weight >= 7) return '#9ACD67';
                if(weight >= 6) return '#36A4DE';
                return '#53555A';
              }},
              weightFactor: (size) => Math.pow(size, 2.1) * canvas.width() / 1024,
              gridSize: Math.round(16 * canvas.width() / 1024),
              rotateRatio: 0.5,
              rotationSteps: 2,
              shrinkToFit: true,
              drawOutOfBound: true
            }};

            WordCloud($('#wordcloud-canvas')[0] , options);
            </script>";
        }
    }
}