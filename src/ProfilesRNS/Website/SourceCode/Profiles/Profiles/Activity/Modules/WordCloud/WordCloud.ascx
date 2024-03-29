﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WordCloud.ascx.cs" Inherits="Profiles.Activity.Modules.WordCloud.WordCloud" %>
 
<style type="text/css">
#wordcloud-canvas {
    overflow: hidden;
    border: 1px solid #dee2e6;
    margin-bottom: 25px;
    width: 100% !important;
    height: 300px !important;
    transition: border-radius .4s ease-out;
    border-radius: 15px;
}

#wordcloud-canvas > span:hover {
    cursor: pointer;
    color: #B51700 !important;
}

@media(min-width:768px) {
    #wordcloud-canvas {
        width: 210px !important;
        height: 210px !important;
    }
}

@media(min-width:992px) {
    #wordcloud-canvas {
        width: 348px !important;
        height: 300px !important;
    }
}
</style>

<div id="wordcloud-canvas" style="width: 348px; height: 348px;"></div>

<script type="text/javascript" src="/Framework/JavaScript/wordcloud2.js"></script>
<asp:Literal Mode="PassThrough" runat="server" ID="script"></asp:Literal>