<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchEverything.ascx.cs"
Inherits="Profiles.Search.Modules.SearchEverything.SearchEverything" %>

<script type="text/javascript">
  function submitEverythingSearch() {
    if (document.getElementById("<%=searchfor.ClientID%>").value.length > 1) {
      document.location =
        "default.aspx?searchtype=everything&searchfor=" +
        document.getElementById("<%=searchfor.ClientID%>").value +
        "&exactphrase=" +
        document.getElementById("<%=chkExactPhrase.ClientID%>").checked;
    } else {
      alert("Search is too broad");
    }
  }
  function runScript(e) {
    $(document).keypress(function (e) {
      if (e.keyCode == 13) {
        submitEverythingSearch();
        return false;
      }
      return;
    });
  }
</script>

<input type="hidden" id="classgroupuri" name="classgroupuri" value="" />
<input type="hidden" id="classuri" name="classuri" value="" />
<input type="hidden" id="searchtype" name="searchtype" value="everything" />
<input type="hidden" id="txtSearchFor" name="txtSearchFor" value="" />

<div class="searchForm">
  <h3>Find publications, research and more</h3>

  <div class="searchSection" onkeypress="JavaScript:runScript(event);">
    <div class="form-group pt-3">
      <label for="searchfor">Keywords</label>
      <div colspan="2" class="fieldOptions">
        <asp:TextBox EnableViewState="false" runat="server" ID="searchfor" CssClass="inputText" title="Keywords" />
        <p class="form-control-subline">
          <asp:CheckBox runat="server" ID="chkExactPhrase" text="Search for exact phrase" />
        </p>
      </div>
    </div>
    <div class="form-group text-sm-right">
      <a href="JavaScript:submitEverythingSearch();" class="search-button"> <i class="bi bi-search"></i> Search </a>
    </div>
  </div>
</div>
