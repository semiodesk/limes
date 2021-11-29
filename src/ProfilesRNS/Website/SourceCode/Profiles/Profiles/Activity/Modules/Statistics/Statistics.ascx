<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Statistics.ascx.cs"
Inherits="Profiles.Activity.Modules.Statistics.Statistics" %>
<section>
  <div class="sidepanel-header">
    <h4 class="act-heading">Profiles Stats</h4>
  </div>
  <div class="metrics flex-md-column flex-lg-row">
    <div>
      <div class="literal"><asp:Literal runat="server" ID="publicationsCount" /></div>
      <div class="label">Publications</div>
    </div>
    <div>
      <div class="literal"><asp:Literal runat="server" ID="totalProfilesCount" /></div>
      <div class="label">Total Profiles</div>
    </div>
    <div>
      <div class="literal"><asp:Literal runat="server" ID="editedProfilesCount" /></div>
      <div class="label">Edited Profiles</div>
    </div>
  </div>
</section>
