<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Statistics.ascx.cs"
Inherits="Profiles.Activity.Modules.Statistics.Statistics" %>
<section>
  <h4 class="act-heading">Profiles Stats</h4>
  <table class="metrics">
    <tr>
      <td class="literal">
        <asp:Literal runat="server" ID="publicationsCount" />
      </td>
      <td>Publications</td>
    </tr>
    <tr>
      <td class="literal">
        <asp:Literal runat="server" ID="totalProfilesCount" />
      </td>
      <td>Total Profiles</td>
    </tr>
    <tr>
      <td class="literal">
        <asp:Literal runat="server" ID="editedProfilesCount" />
      </td>
      <td>Edited Profiles</td>
    </tr>
  </table>
</section>
