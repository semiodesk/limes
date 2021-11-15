<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchOptions.ascx.cs"
    Inherits="Profiles.Search.Modules.SearchOptions.SearchOptions" %>
    <script type="text/javascript" language="javascript">

    function modify(root,tab,searchrequest) {       
                    
          document.location =root + "/search/default.aspx?tab=" + tab + "&action=modify&searchrequest=" + searchrequest;
          
    }
</script>
<div id="divSearchCriteria">
    <div class="sidepanel-header">
        <h4>Search Options</h4>
    </div>    
    <div class="passiveSectionBody">
        <ul>
            <li><asp:Literal runat="server" ID="litModifySearch"></asp:Literal></li>
            <li><asp:Literal runat="server" ID="litSearchOtherInstitutions"></asp:Literal></li>
        </ul> 
    </div>
</div>
