﻿using System.Collections.Generic;

using Profiles.Framework.Utilities;

namespace Profiles.Search.Utilities
{
    public static class SearchDropDowns
    {


        public static string BuildDropdown(string type, string width, string defaultitem, string placeholder = null)
        {
            Utilities.DataIO data = new Profiles.Search.Utilities.DataIO();
            string output = string.Empty;
            
            List<GenericListItem> list = new List<GenericListItem>();

            switch (type)
            {
                case "institution":
                    list = data.GetInstitutions();            
                    break;

                case "department":
                    list = data.GetDepartments();
                    break;
                case "division":
                    list = data.GetDivisions();
                    break;
            }

            if (!string.IsNullOrEmpty(placeholder))
            {
                output += $"<option value=\"\" disabled selected>{placeholder}</option>";
            }
            else if(string.IsNullOrEmpty(defaultitem))
            {
                output += "<option value=\"\"></option>";
            }

            foreach (GenericListItem item in list)
            {
                if (!defaultitem.IsNullOrEmpty() && defaultitem == item.Value)
                    output += "<option selected=\"true\" value=\"" + item.Value + "\">" + item.Text + "</option>";
                else
                    output += "<option value=\"" + item.Value + "\">" + item.Text + "</option>";
            }

            return $"<select title=\"{type}\" name=\"{type}\" id=\"{type}\" class=\"form-control\">{output}</select>";
        }
    }
}
