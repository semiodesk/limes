<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Statistics.ascx.cs"
Inherits="Profiles.Activity.Modules.Statistics.Statistics" %>
<section>
  <div class="sidepanel-header">
    <h4 class="act-heading">Profiles Stats</h4>
  </div>
  <table class="metrics">
    <tr class="literals">
      <td><asp:Literal runat="server" ID="publicationsCount" /></td>
      <td><asp:Literal runat="server" ID="totalProfilesCount" /></td>
      <td><asp:Literal runat="server" ID="editedProfilesCount" /></td>
    </tr>
    <tr class="labels">
      <td>Publications</td>
      <td>Total Profiles</td>
      <td>Edited Profiles</td>
    </tr>
  </table>
</section>
