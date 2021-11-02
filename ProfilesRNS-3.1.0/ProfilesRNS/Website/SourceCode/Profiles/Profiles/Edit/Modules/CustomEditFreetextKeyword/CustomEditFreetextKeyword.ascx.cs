﻿/*  
 
    Copyright (c) 2008-2012 by the President and Fellows of Harvard College. All rights reserved.  
    Profiles Research Networking Software was developed under the supervision of Griffin M Weber, MD, PhD.,
    and Harvard Catalyst: The Harvard Clinical and Translational Science Center, with support from the 
    National Center for Research Resources and Harvard University.


    Code licensed under a BSD License. 
    For details, see: LICENSE.txt 
  
*/
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Xml;
using Profiles.Framework.Utilities;
using Profiles.Edit.Utilities;

namespace Profiles.Edit.Modules.CustomEditFreetextKeyword
{
    public partial class CustomEditFreetextKeyword : BaseModule
    {
        Edit.Utilities.DataIO data;
        Profiles.Profile.Utilities.DataIO propdata;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.FillPropertyGrid(false);
            if (!IsPostBack)
            {
                Session["pnlInsertProperty.Visible"] = null;
            }

        }

        public CustomEditFreetextKeyword() { }
        public CustomEditFreetextKeyword(XmlDocument pagedata, List<ModuleParams> moduleparams, XmlNamespaceManager pagenamespaces)
            : base(pagedata, moduleparams, pagenamespaces)
        {
            SessionManagement sm = new SessionManagement();
            propdata = new Profiles.Profile.Utilities.DataIO();
            data = new Profiles.Edit.Utilities.DataIO();
            string predicateuri = Request.QueryString["predicateuri"].Replace("!", "#");
            this.PropertyListXML = propdata.GetPropertyList(this.BaseData, base.PresentationXML, predicateuri, false, true, false);
            PropertyLabel = this.PropertyListXML.SelectSingleNode("PropertyList/PropertyGroup/Property/@Label").Value;

            if (Request.QueryString["subject"] != null)
                this.SubjectID = Convert.ToInt64(Request.QueryString["subject"]);
            else if (base.GetRawQueryStringItem("subject") != null)
                this.SubjectID = Convert.ToInt64(base.GetRawQueryStringItem("subject"));
            else
                Response.Redirect("~/search");

            litBackLink.Text = "<a href='" + Root.Domain + "/edit/" + this.SubjectID + "'>Edit Menu</a> &gt; <b>" + PropertyListXML.SelectSingleNode("PropertyList/PropertyGroup/Property/@Label").Value + "</b>";

            //btnEditProperty.Text = "Add " + PropertyLabel;
            imbAddArrow.Visible = true;

            this.PropertyListXML = propdata.GetPropertyList(this.BaseData, base.PresentationXML, predicateuri, false, true, false);
            this.MaxCardinality = this.PropertyListXML.SelectSingleNode("PropertyList/PropertyGroup/Property/@MaxCardinality").Value;
            this.MinCardinality = this.PropertyListXML.SelectSingleNode("PropertyList/PropertyGroup/Property/@MinCardinality").Value;

            securityOptions.Subject = this.SubjectID;
            securityOptions.PredicateURI = predicateuri;
            securityOptions.PrivacyCode = Convert.ToInt32(this.PropertyListXML.SelectSingleNode("PropertyList/PropertyGroup/Property/@ViewSecurityGroup").Value);
            securityOptions.SecurityGroups = new XmlDataDocument();
            securityOptions.SecurityGroups.LoadXml(base.PresentationXML.DocumentElement.LastChild.OuterXml);

            txtLabel.Attributes.Add("data-autocomplete-url", Root.Domain + "/edit/Modules/CustomEditFreetextKeyword/keywordAutocomplete.aspx?keys=");
        }

        #region Property

        protected void btnEditProperty_OnClick(object sender, EventArgs e)
        {
            if (Session["pnlInsertProperty.Visible"] != null)
            {
                phSecurityOptions.Visible = true;
                phEditProperty.Visible = true;
                //phDelAll.Visible = true;
                btnInsertCancel_OnClick(sender, e);
                imbAddArrow.ImageUrl = "~/Framework/Images/icon_squareArrow.gif";
                Session["pnlInsertProperty.Visible"] = null;
            }
            else
            {
                phSecurityOptions.Visible = false;
                phEditProperty.Visible = true;
                //phDelAll.Visible = false;
                pnlInsertProperty.Visible = true;
                imbAddArrow.ImageUrl = "~/Framework/Images/icon_squareDownArrow.gif";
                Session["pnlInsertProperty.Visible"] = true;

            }
            upnlEditSection.Update();
        }

        protected void btnDelAll_OnClick(object sender, EventArgs e)
        {

            if (Session["pnlDeleteAll.Visible"] != null)
            {

                phSecurityOptions.Visible = true;
                phEditProperty.Visible = true;
                //phDelAll.Visible = true;
                pnlDeleteAll.Visible = false;
                imbDelArrow.ImageUrl = "~/Framework/Images/icon_squareArrow.gif";
                Session["pnlDeleteAll.Visible"] = null;
            }
            else
            {
                phSecurityOptions.Visible = false;
                phEditProperty.Visible = false;
                //phDelAll.Visible = true;
                pnlDeleteAll.Visible = true;
                imbDelArrow.ImageUrl = "~/Framework/Images/icon_squareDownArrow.gif";
                Session["pnlDeleteAll.Visible"] = true;

            }
            upnlEditSection.Update();
        }

        protected void btnBulkInsert_OnClick(object sender, EventArgs e)
        {
            txtLabel.Text = "";
            pnlInsertProperty.Visible = false;
            pnlInsertPropertyBulk.Visible = true;
            imbAddArrow.ImageUrl = "~/Framework/Images/icon_squareDownArrow.gif";
            upnlEditSection.Update();
        }

        protected void btnSingleInsert_OnClick(object sender, EventArgs e)
        {
            txtLabelBulk.Text = "";
            pnlInsertProperty.Visible = true;
            pnlInsertPropertyBulk.Visible = false;
            imbAddArrow.ImageUrl = "~/Framework/Images/icon_squareDownArrow.gif";
            upnlEditSection.Update();
        }

        protected void GridViewProperty_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            TextBox txtLabelGrid = null;

            ImageButton lnkEdit = null;
            ImageButton lnkDelete = null;
            
            //  System.Web.UI.WebControls.Panel pnlMovePanel = null;
            LiteralState ls = (LiteralState)e.Row.DataItem;
            Label lblLabel = (Label)e.Row.FindControl("lblLabel");

            LiteralState literalstate = null;
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Text = "Keyword";
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                lnkEdit = (ImageButton)e.Row.Cells[1].FindControl("lnkEdit");
                lnkDelete = (ImageButton)e.Row.Cells[1].FindControl("lnkDelete");
                // pnlMovePanel = (System.Web.UI.WebControls.Panel)e.Row.Cells[2].FindControl("pnlMovePanel");


                literalstate = (LiteralState)e.Row.DataItem;

                if (literalstate.EditDelete == false)
                    lnkDelete.Visible = false;
                if (literalstate.EditExisting == false)
                {
                    lnkEdit.Visible = false;
                    //     pnlMovePanel.Visible = false;
                }

                if (lnkDelete != null)
                    lnkDelete.OnClientClick = "Javascript:return confirm('Are you sure you want to delete these " + PropertyLabel + "?');";

                if (lblLabel != null)
                    lblLabel.Text = "" + literalstate.Literal.Replace("\n", "<br/>");               
            }

            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
            {
                txtLabelGrid = (TextBox)e.Row.Cells[0].FindControl("txtLabelGrid");
                txtLabelGrid.Text = "" + literalstate.Literal.Trim();
                txtLabelGrid.Attributes.Add("data-autocomplete-url", Root.Domain + "/edit/Modules/CustomEditFreetextKeyword/keywordAutocomplete.aspx?keys=");
            }
        }

        protected void GridViewProperty_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewProperty.EditIndex = e.NewEditIndex;
            this.FillPropertyGrid(false);
            upnlEditSection.Update();
        }

        protected void GridViewProperty_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string _object = GridViewProperty.DataKeys[e.RowIndex].Values[2].ToString();

            TextBox txtLabelGrid = (TextBox)GridViewProperty.Rows[e.RowIndex].FindControl("txtLabelGrid");

            data.UpdateLiteral(this.SubjectID, this.PredicateID, Convert.ToInt64(_object), data.GetStoreNode(txtLabelGrid.Text.Trim()), this.PropertyListXML);

            GridViewProperty.EditIndex = -1;
            this.FillPropertyGrid(true);
            upnlEditSection.Update();
        }

        protected void GridViewProperty_RowUpdated(object sender, GridViewUpdatedEventArgs e)
        {
            this.FillPropertyGrid(false);
            upnlEditSection.Update();
        }

        protected void GridViewProperty_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewProperty.EditIndex = -1;

            this.FillPropertyGrid(false);
            upnlEditSection.Update();
        }

        protected void GridViewProperty_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string _object = GridViewProperty.DataKeys[e.RowIndex].Values[2].ToString();

            data.DeleteTriple(this.SubjectID, this.PredicateID, Convert.ToInt64(_object));

            this.FillPropertyGrid(true);
            upnlEditSection.Update();
        }

        protected void btnInsertCancel_OnClick(object sender, EventArgs e)
        {
            txtLabel.Text = "";
            pnlInsertProperty.Visible = false;
            upnlEditSection.Update();
        }

        protected void btnInsertClose_OnClick(object sender, EventArgs e)
        {
            if (Session["pnlInsertProperty.Visible"] != null)
            {
                if (txtLabel.Text.Trim().Length > 0)
                {
                    data.AddLiteral(this.SubjectID, this.PredicateID, data.GetStoreNode(txtLabel.Text.Trim()), this.PropertyListXML);
                }
                this.FillPropertyGrid(true);
                Session["pnlInsertProperty.Visible"] = null;
                btnInsertCancel_OnClick(sender, e);
            }
        }

        protected void btnInsert_OnClick(object sender, EventArgs e)
        {
            if (Session["pnlInsertProperty.Visible"] != null)
            {
                if (txtLabel.Text.Trim().Length > 0)
                {
                    data.AddLiteral(this.SubjectID, this.PredicateID, data.GetStoreNode(txtLabel.Text.Trim()), this.PropertyListXML);
                }
                this.FillPropertyGrid(true);
                pnlInsertProperty.Visible = true;
                txtLabel.Text = "";
                imbAddArrow.ImageUrl = "~/Framework/Images/icon_squareDownArrow.gif";
                Session["pnlInsertProperty.Visible"] = true;
                upnlEditSection.Update();
            }
        }

        protected void btnInsertBulkClose_OnClick(object sender, EventArgs e)
        {
            if (Session["pnlInsertProperty.Visible"] != null)
            {
                // grab them all, separated by carriage return or commas or tab
                foreach (string item in txtLabelBulk.Text.Trim().Split(new Char[] { '\n', ',', '\t' }))
                {
                    if (item.Trim().Length > 0)
                    {
                        data.AddLiteral(this.SubjectID, this.PredicateID, data.GetStoreNode(item.Trim()), this.PropertyListXML);
                    }
                }
                this.FillPropertyGrid(true);
                Session["pnlInsertProperty.Visible"] = null;
                btnInsertCancel_OnClick(sender, e);
            }
        }

        protected void btnDeleteAll_OnClick(object sender, EventArgs e)
        {
            for(int i = 0; i < GridViewProperty.DataKeys.Count; i++)
            {
                string _object = GridViewProperty.DataKeys[i].Values[2].ToString();

                data.DeleteTriple(this.SubjectID, this.PredicateID, Convert.ToInt64(_object));
            }
            this.FillPropertyGrid(true);
            upnlEditSection.Update();
            phSecurityOptions.Visible = true;
            phEditProperty.Visible = true;
            //phDelAll.Visible = true;
            pnlDeleteAll.Visible = false;
            imbDelArrow.ImageUrl = "~/Framework/Images/icon_squareArrow.gif";
            Session["pnlDeleteAll.Visible"] = null;
        }


        protected void ibUp_Click(object sender, EventArgs e)
        {

            GridViewRow row = ((ImageButton)sender).Parent.Parent as GridViewRow;
            GridViewProperty.EditIndex = -1;

            if (GridViewProperty.Rows.Count > 1)
            {
                Int64 subject = Convert.ToInt64(GridViewProperty.DataKeys[row.RowIndex].Values[0].ToString());
                Int64 predicate = Convert.ToInt64(GridViewProperty.DataKeys[row.RowIndex].Values[1].ToString());
                Int64 _object = Convert.ToInt64(GridViewProperty.DataKeys[row.RowIndex].Values[2].ToString());


                data.MoveTripleDown(subject, predicate, _object);

                this.FillPropertyGrid(true);
            }
            upnlEditSection.Update();


        }

        protected void ibDown_Click(object sender, EventArgs e)
        {
            GridViewRow row = ((ImageButton)sender).Parent.Parent as GridViewRow;
            GridViewProperty.EditIndex = -1;
            if (GridViewProperty.Rows.Count > 1)
            {
                Int64 subject = Convert.ToInt64(GridViewProperty.DataKeys[row.RowIndex].Values[0].ToString());
                Int64 predicate = Convert.ToInt64(GridViewProperty.DataKeys[row.RowIndex].Values[1].ToString());
                Int64 _object = Convert.ToInt64(GridViewProperty.DataKeys[row.RowIndex].Values[2].ToString());

                data.MoveTripleUp(subject, predicate, _object);
            }
            this.FillPropertyGrid(true);
            upnlEditSection.Update();

        }

        protected void FillPropertyGrid(bool refresh)
        {
            if (refresh)
            {
                base.GetSubjectProfile();
                this.PropertyListXML = propdata.GetPropertyList(this.BaseData, base.PresentationXML, this.PredicateURI, false, true, false);
            }

            List<LiteralState> literalstate = new List<LiteralState>();

            bool editexisting = false;
            bool editaddnew = false;
            bool editdelete = false;

            if (this.PropertyListXML.SelectSingleNode("PropertyList/PropertyGroup/Property/@EditExisting").Value.ToLower() == "true" ||
             this.PropertyListXML.SelectSingleNode("PropertyList/PropertyGroup/Property/@CustomEdit").Value.ToLower() == "true")
                editexisting = true;

            if (this.PropertyListXML.SelectSingleNode("PropertyList/PropertyGroup/Property/@EditAddNew").Value.ToLower() == "true" ||
                this.PropertyListXML.SelectSingleNode("PropertyList/PropertyGroup/Property/@CustomEdit").Value.ToLower() == "true")
                editaddnew = true;

            if (this.PropertyListXML.SelectSingleNode("PropertyList/PropertyGroup/Property/@EditDelete").Value.ToLower() == "true" ||
                this.PropertyListXML.SelectSingleNode("PropertyList/PropertyGroup/Property/@CustomEdit").Value.ToLower() == "true")
                editdelete = true;

            if (!editaddnew)
            {
                btnEditProperty.Visible = false;
                imbAddArrow.Visible = false;
            }

            this.SubjectID = Convert.ToInt64(base.GetRawQueryStringItem("subject"));


            this.PredicateURI = Server.UrlDecode(base.GetRawQueryStringItem("predicateuri").Replace("!", "#"));
            this.PredicateID = data.GetStoreNode(this.PredicateURI);

            foreach (XmlNode property in this.PropertyListXML.SelectNodes("PropertyList/PropertyGroup/Property/Network/Connection"))
            {
                literalstate.Add(new LiteralState(this.SubjectID, this.PredicateID, data.GetStoreNode(property.InnerText.Trim()), property.InnerText.Trim(), editexisting, editdelete));
            }

            btnEditProperty.Visible = true;
            imbAddArrow.Visible = true;       
            if (literalstate.Count > 0)
            {

                GridViewProperty.DataSource = literalstate;
                GridViewProperty.DataBind();
                lblNoItems.Visible = false;
                GridViewProperty.Visible = true;

                // not sure of this
                if (MaxCardinality == literalstate.Count.ToString())
                {
                    btnEditProperty.Visible = false;
                    imbAddArrow.Visible = false;
                    btnInsertProperty.Visible = false;
                }
            }
            else
            {
                lblNoItems.Visible = true;
                GridViewProperty.Visible = false;        
                if (MaxCardinality == "1") // not sure about this
                { 
                    btnInsertProperty.Visible = false;
                }
                upnlEditSection.Update();
            }


        }
        #endregion

        private Int64 SubjectID { get; set; }
        private Int64 PredicateID { get; set; }
        private string PredicateURI { get; set; }

        private XmlDocument XMLData { get; set; }

        private XmlDocument PropertyListXML { get; set; }
        private string PropertyLabel { get; set; }
        private string MaxCardinality { get; set; }
        private string MinCardinality { get; set; }


    }
}