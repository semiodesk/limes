<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomViewConceptTopJournal.ascx.cs" Inherits="Profiles.Profile.Modules.CustomViewConceptTopJournal" %>

<div class="sidepanel-header">
  <h4 id="sectionTitle" runat="server"><%= this.GetModuleParamString("InfoCaption") %></h4>
</div>
<div class="passiveSectionBody">
	<ul>
		<asp:Literal runat="server" ID="lineItemLiteral"></asp:Literal>
	</ul>
</div>

       