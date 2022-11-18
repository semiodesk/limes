﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LatestUpdates.ascx.cs"
Inherits="Profiles.Activity.Modules.LatestUpdates.LatestUpdates" %>
<%@ Register TagName="Activity" TagPrefix="ActivityHistory" Src="~/Activity/Modules/ActivityHistory/ActivityHistory.ascx" %>
<%@ Register TagName="Statistics" TagPrefix="Statistics" Src="~/Activity/Modules/Statistics/Statistics.ascx" %>
<%@ Register TagName="TwitterTimeline" TagPrefix="TwitterTimeline" Src="~/Activity/Modules/TwitterTimeline/TwitterTimeline.ascx" %>
<div class="activeContainer" id="defaultmenu">
  <div class="activeContainerTop"></div>
  <div class="activeContainerCenter">
    <div class="activeSection">
      <div class="activeSectionBody">
        <img src="/Framework/Assets/images/wordcloud.png" class="wordcloud" width="315" height="174" />
        <Statistics:Statistics runat="server" ID="Statistics" Visible="true" />
        <TwitterTimeline:TwitterTimeline runat="server" ID="Activity1" Visible="true" />
      </div>
    </div>
  </div>
  <div class="activeContainerBottom"></div>
</div>
