<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LatestUpdates.ascx.cs"
Inherits="Profiles.Activity.Modules.LatestUpdates.LatestUpdates" %>
<%@ Register TagName="Activity" TagPrefix="ActivityHistory" Src="~/Activity/Modules/ActivityHistory/ActivityHistory.ascx" %>
<%@ Register TagName="Statistics" TagPrefix="prns" Src="~/Activity/Modules/Statistics/Statistics.ascx" %>
<%@ Register TagName="TwitterTimeline" TagPrefix="prns" Src="~/Activity/Modules/TwitterTimeline/TwitterTimeline.ascx" %>
<%@ Register TagName="WordCloud" TagPrefix="prns" Src="~/Activity/Modules/WordCloud/WordCloud.ascx" %>
<div class="activeContainer" id="defaultmenu">
  <div class="activeContainerTop"></div>
  <div class="activeContainerCenter">
    <div class="activeSection">
      <div class="activeSectionBody">
        <div class="d-flex flex-column my-4 my-lg-0">
          <prns:WordCloud runat="server" ID="WordCloud" Visible="True" />
          <prns:Statistics runat="server" ID="Statistics" Visible="true" />
        </div>
        <prns:TwitterTimeline runat="server" ID="Activity1" Visible="true" />
      </div>
    </div>
  </div>
  <div class="activeContainerBottom"></div>
</div>
