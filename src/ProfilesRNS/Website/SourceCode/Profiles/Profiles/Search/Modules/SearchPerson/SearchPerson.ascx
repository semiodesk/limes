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

<div class="searchForm" onkeypress="JavaScript:runScript(event);">
  <h3>Find people</h3>
  <section class="searchSection pt-3" id="div1">
    <div class="row">
      <div class="col-sm-6 form-group">
        <label for="ctl00_ContentMain_rptMain_ctl00_ctl00_txtFname">First Name</label>
        <asp:TextBox runat="server" ID="txtFname" CssClass="inputText" title="First Name"></asp:TextBox>
      </div>
      <div class="col-sm-6 form-group">
        <label for="ctl00_ContentMain_rptMain_ctl00_ctl00_txtLname">Last Name</label>
        <asp:TextBox runat="server" ID="txtLname" CssClass="inputText" title="Last Name"></asp:TextBox>
      </div>
    </div>
    <div runat="server" id="trInstitution" class="form-group">
      <label for="institution">Institution</label>
      <asp:Literal runat="server" ID="litInstitution"></asp:Literal>
      <p class="form-control-subline">
        <asp:CheckBox runat="server" ID="institutionallexcept" Text="All <i>except</i> the one selected" />
      </p>
    </div>
    <!-- <div runat="server" id="trDepartment" class="form-group">
        <label for="litDepartment">Department</label>
        <asp:Literal runat="server" ID="litDepartment"></asp:Literal>
        <div class="fw-normal">
          <asp:CheckBox
            runat="server"
            ID="departmentallexcept"
            label="except department"
            Text="&nbsp;All <b>except</b> the one selected"
          />
        </div>
      </div> -->
    <div runat="server" id="trDivision" class="form-group">
      <label for="division">Competence Center</label>
      <asp:Literal runat="server" ID="litDivision"></asp:Literal>
      <p class="form-control-subline">
        <asp:CheckBox runat="server" ID="divisionallexcept" Text="All <i>except</i> the one selected" />
      </p>
    </div>
    <!-- <div runat="server" id="trFacultyType" class="form-group">
        <label>Faculty Type</label>
        <div>
          <asp:PlaceHolder ID="phDDLCHK" runat="server"></asp:PlaceHolder>
        </div>
        <div>
          <asp:PlaceHolder ID="phDDLList" runat="server"></asp:PlaceHolder>
        </div>
        <asp:Label ID="lblSelectedItem" runat="server"></asp:Label>
        <asp:HiddenField ID="hidList" runat="server" />
        <asp:HiddenField ID="hidURIs" runat="server" />
      </div> -->
    <!-- <div runat="server" id="trOtherOptions" class="form-group">
        <label style="vertical-align: top">Other Options</label>
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
      </div> -->
    <div class="form-group text-right">
      <button href="JavaScript:search();" class="search-button">
        <i class="fa fa-search"></i> Search
      </button>
    </div>
    <asp:Literal runat="server" ID="litFacRankScript"></asp:Literal>
  </section>
</div>

<script>
  $(document).ready(function () {
    $("[id*=ddlChkList]").css("width", "249px");
    $("select").css("height", "25px");
  });
</script>
