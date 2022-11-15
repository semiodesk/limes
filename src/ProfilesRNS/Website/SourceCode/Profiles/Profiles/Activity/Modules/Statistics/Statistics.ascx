<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Statistics.ascx.cs"
Inherits="Profiles.Activity.Modules.Statistics.Statistics" %>
<section>
  <div class="metrics flex-row p-0">
    <div class="text-center w-50">
      <div class="literal">
          <div class="circle m-auto">
             <asp:Literal runat="server" ID="publicationsCount" />
          </div>
      </div>
      <div class="label">Publications</div>
    </div>
    <div class="text-center w-50">
      <div class="literal">
          <div class="circle m-auto">
             <asp:Literal runat="server" ID="totalProfilesCount" />
          </div>
      </div>
      <div class="label">People</div>
    </div>
    <!--
    <div>
      <div class="literal"><asp:Literal runat="server" ID="editedProfilesCount" /></div>
      <div class="label">Edited Profiles</div>
    </div>
    -->
  </div>
</section>
