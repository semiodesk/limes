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

            var stats = data.GetConceptStats("PROC", 10, 5, 20).Select(x => new object[] { x.Label, x.FontSize }).ToList();
            stats.AddRange(data.GetConceptStats("DEVI", 8, 4, 20).Select(x => new object[] { x.Label, x.FontSize }));
            stats.AddRange(data.GetConceptStats("CONC", 6, 4, 20).Select(x => new object[] { x.Label, x.FontSize }));

            var script = (Literal)FindControl("script");
            script.Text = $@"
            <script type='text/javascript'>
            var canvas = $('#wordcloud-canvas');
            var options = {{
              list: {JsonConvert.SerializeObject(stats)},
              fontFamily: 'Mina, Roboto, Arial, Helvetica, sans-serif',
              color: function (word, weight) {{
                console.warn(word, weight);
                if(weight >= 6) return '#36A4DE';
                if(weight >= 5) return '#2f528f';
                return '#70767D';
              }},
              weightFactor: (size) => 1.9 * size,
              gridSize: Math.round(16 * canvas.width() / 1024),
              rotateRatio: 0.5,
              rotationSteps: 2,
              shrinkToFit: true,
              drawOutOfBound: false
            }};

            WordCloud($('#wordcloud-canvas')[0] , options);
            </script>";
        }
    }
}