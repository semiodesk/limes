<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopSearchPhrase.ascx.cs"
Inherits="Profiles.Search.Modules.TopSearchPhrase.TopSearchPhrase" %>

<section>
  <script type="text/javascript">
    function searchThisPhrase(keyword, classuri, searchtype) {
      document.location.href =
        "<%=GetURLDomain()%>/search/default.aspx?searchtype=" +
        searchtype +
        "&searchfor=" +
        keyword +
        "&exactphrase=false&classuri=" +
        classuri;
    }
  </script>

  <h4 class="passiveSectionHead">
    <asp:Literal runat="server" ID="litDescription"></asp:Literal>
  </h4>

  <div class="passiveSectionBody">
    <asp:Literal runat="server" ID="litTopSearchPhrase"></asp:Literal>
  </div>
</section>
