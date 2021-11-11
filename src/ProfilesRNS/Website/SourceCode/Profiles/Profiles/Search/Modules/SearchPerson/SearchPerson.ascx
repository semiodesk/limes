<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchPerson.ascx.cs"
Inherits="Profiles.Search.Modules.SearchPerson.SearchPerson" EnableViewState="true" %> <%@ Register
Src="ComboTreeCheck.ascx" TagName="ComboTreeCheck" TagPrefix="uc1" %>

<script type="text/javascript">
  function runScript(e) {
    if (e.keyCode == 13) {
      search();
    }
    return false;
  }

  function search() {
    document.getElementById("<%=hdnSearch.ClientID%>").value = "true";
    document.forms[0].submit();
  }

  function showdiv() {
    var divChkList = $("[id$=divChkList]").attr("id");
    var chkListItem = $("[id$=chkLstItem_0]").attr("id");
    document.getElementById(divChkList).style.display = "block";

    document.getElementById(chkListItem).focus();
  }

  function showdivonClick() {
    var objDLL = $("[id$=divChkList]").attr("id"); // document.getElementById("divChkList");

    if (document.getElementById(objDLL).style.display == "block")
      document.getElementById(objDLL).style.display = "none";
    else document.getElementById(objDLL).style.display = "block";
  }

  function getSelectedItem(lstValue, lstNo, lstID, ctrlType) {
    var noItemChecked = 0;
    var ddlChkList = document.getElementById($("[id$=ddlChkList]").attr("id"));
    var selectedItems = "";
    var selectedValues = "";
    var arr = document.getElementById($("[id$=chkLstItem]").attr("id")).getElementsByTagName("input");
    var arrlbl = document.getElementById($("[id$=chkLstItem]").attr("id")).getElementsByTagName("label");
    var objLstId = document.getElementById($("[id$=hidList]").attr("id")); //document.getElementById('hidList');

    for (i = 0; i < arr.length; i++) {
      checkbox = arr[i];
      if (i == lstNo) {
        if (ctrlType == "anchor") {
          if (!checkbox.checked) {
            checkbox.checked = true;
          } else {
            checkbox.checked = false;
          }
        }
      }

      if (checkbox.checked) {
        var buffer;
        if (arrlbl[i].innerText == undefined) buffer = arrlbl[i].textContent;
        else buffer = arrlbl[i].innerText;

        if (selectedItems == "") {
          selectedItems = buffer;
        } else {
          selectedItems = selectedItems + "," + buffer;
        }
        noItemChecked = noItemChecked + 1;
      }
    }

    ddlChkList.title = selectedItems;

    if (noItemChecked != "0") ddlChkList.options[ddlChkList.selectedIndex].text = selectedItems;
    else ddlChkList.options[ddlChkList.selectedIndex].text = "";

    var hidList = document.getElementById($("[id$=hidList]").attr("id"));
    hidList.value = ddlChkList.options[ddlChkList.selectedIndex].text;
  }

  document.onclick = check;
  function check(e) {
    var target = (e && e.target) || (event && event.srcElement);
    var obj = document.getElementById($("[id$=divChkList]").attr("id"));
    var obj1 = document.getElementById($("[id$=ddlChkList]").attr("id"));
    if (target.id != "alst" && !target.id.match($("[id$=chkLstItem]").attr("id"))) {
      if (!(target == obj || target == obj1)) {
        //obj.style.display = 'none'
      } else if (target == obj || target == obj1) {
        if (obj.style.display == "block") {
          obj.style.display = "block";
        } else {
          obj.style.display = "none";
          document.getElementById($("[id$=ddlChkList]").attr("id")).blur();
        }
      }
    }
  }
</script>

<asp:HiddenField ID="hdnSearch" runat="server" Value="hdnSearch"></asp:HiddenField>
<div class="content_container">
  <div class="tabContainer">
    <div class="searchForm" onkeypress="JavaScript:runScript(event);">
      <div>Find people by keyword</div>
      <section class="searchSection">
        <div class="row">
          <div class="col-md-3 text-right">
            <label>Keywords</label>
          </div>
          <div class="col-md-9">
            <asp:TextBox runat="server" ID="txtSearchFor" CssClass="inputText" title="Keywords"></asp:TextBox>
            <div class="fw-normal">
              <asp:CheckBox runat="server" ID="chkExactphrase" Text="&nbsp;Search for exact phrase" />
            </div>
          </div>
        </div>
        <div class="row">
          <div class="col-md-9 col-md-offset-3">
            <div class="search-button-container" style="margin-top: 1em">
              <a href="JavaScript:search();" class="search-button"> <i class="fa fa-search"></i> Search </a>
            </div>
          </div>
        </div>
      </section>

      <div class="headings">Find people by name/organization</div>
      <section class="searchSection" id="div1">
        <table class="searchForm">
          <tr>
            <th>Last Name</th>
            <td colspan="2">
              <asp:TextBox runat="server" ID="txtLname" CssClass="inputText" title="Last Name"></asp:TextBox>
            </td>
          </tr>
          <tr>
            <th>First Name</th>
            <td colspan="2">
              <asp:TextBox runat="server" ID="txtFname" CssClass="inputText" title="First Name"></asp:TextBox>
            </td>
          </tr>
          <tr runat="server" id="trInstitution">
            <th>Institution</th>
            <td colspan="2">
              <asp:Literal runat="server" ID="litInstitution"></asp:Literal>
              <div class="fw-normal">
                <asp:CheckBox
                  runat="server"
                  ID="institutionallexcept"
                  Text="&nbsp;All <b>except</b> the one selected"
                />
              </div>
            </td>
          </tr>
          <tr runat="server" id="trDepartment">
            <th>Department</th>
            <td colspan="2">
              <asp:Literal runat="server" ID="litDepartment"></asp:Literal>
              <div class="fw-normal">
                <asp:CheckBox
                  runat="server"
                  ID="departmentallexcept"
                  label="except department"
                  Text="&nbsp;All <b>except</b> the one selected"
                />
              </div>
            </td>
          </tr>
          <tr runat="server" id="trDivision">
            <th>Division</th>
            <td colspan="2">
              <asp:Literal runat="server" ID="litDivision"></asp:Literal>
              <div class="fw-normal">
                <asp:CheckBox runat="server" ID="divisionallexcept" Text="&nbsp;All <b>except</b> the one selected" />
              </div>
            </td>
          </tr>
          <tr runat="server" id="trFacultyType">
            <th>Faculty Type</th>
            <td colspan="2">
              <div>
                <asp:PlaceHolder ID="phDDLCHK" runat="server"></asp:PlaceHolder>
              </div>
              <div>
                <asp:PlaceHolder ID="phDDLList" runat="server"></asp:PlaceHolder>
              </div>
              <asp:Label ID="lblSelectedItem" runat="server"></asp:Label>
              <asp:HiddenField ID="hidList" runat="server" />
              <asp:HiddenField ID="hidURIs" runat="server" />
            </td>
          </tr>
          <tr runat="server" id="trOtherOptions">
            <th style="vertical-align: top">Other Options</th>
            <td colspan="2">
              <select
                onmousedown="(function(e){ e.preventDefault(); })(event, this)"
                id="selOtherOptions"
                style="width: 249px; height: 20px"
                title="other options"
              >
                <option value=""></option>
              </select>
              <table>
                <tr>
                  <td>
                    <div id="divOtherOptions">
                      <uc1:ComboTreeCheck ID="ctcFirst" runat="server" Width="255px" />
                    </div>
                  </td>
                </tr>
              </table>
            </td>
          </tr>
          <tr>
            <td></td>
            <td colspan="2">
              <div class="search-button-container">
                <a href="JavaScript:search();" class="search-button"><i class="fa fa-search"></i> Search</a>
              </div>
            </td>
          </tr>
        </table>
        <asp:Literal runat="server" ID="litFacRankScript"></asp:Literal>
      </section>
    </div>
  </div>
</div>
<script>
  $(document).ready(function () {
    $("[id*=ddlChkList]").css("width", "249px");
    $("select").css("height", "25px");
  });
</script>
