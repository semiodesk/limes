<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchPerson.ascx.cs"
Inherits="Profiles.Search.Modules.SearchPerson.SearchPerson" EnableViewState="true" %> <%@ Register
Src="ComboTreeCheck.ascx" TagName="ComboTreeCheck" TagPrefix="uc1" %>

<form id="people-search-form" class="searchForm" method="get" action="<%=ResolveUrl("~/search/default.aspx")%>">
  <h3>Find people</h3>

  <asp:HiddenField ID="hdnSearch" runat="server" Value="hdnSearch"></asp:HiddenField>

  <input type="hidden" name="searchtype" value="people" />
  <input type="hidden" name="classuri" value="http://xmlns.com/foaf/0.1/Person" />
  
  <section class="searchSection pt-3" id="div1">
    <div class="row">
      <div class="col-sm-6 form-group">
        <label for="fname">First Name</label>
        <input type="text" id="fname" name="fname" aria-label="Enter the first name." maxlength="50" />
      </div>
      <div class="col-sm-6 form-group">
        <label for="lname">Last Name</label>
        <input type="text" id="lname" name="lname" aria-label="Enter the last name." maxlength="50" />
      </div>
    </div>
    <div runat="server" id="trInstitution" class="form-group">
      <label for="institution">Institution</label>
      <asp:Literal runat="server" ID="litInstitution"></asp:Literal>

      <p class="form-control-subline">
        <input type="checkbox" name="institutionallexcept" id="institutionallexcept" value="true" />
        <label for="institutionallexcept">All <i>except</i> the one selected</label>
      </p>
    </div>
    <div runat="server" id="trDivision" class="form-group">
      <label for="division">Competence Center</label>
      <asp:Literal runat="server" ID="litDivision"></asp:Literal>

      <p class="form-control-subline">
        <input type="checkbox" name="divisionallexcept" id="divisionallexcept" value="true" />
        <label for="divisionallexcept">All <i>except</i> the one selected</label>
      </p>
    </div>
    <div class="form-group text-right">
      <button type="submit" class="search-button" disabled>
        <i class="fa fa-search"></i> Search
      </button>
    </div>
  </section>
  <script type="text/javascript">
  $(document).ready(() => {
    const form = $('#people-search-form');
    const submit = $('#people-search-form .search-button');

    const controls = {
      fname: $('#fname'),
      lname: $('#lname'),
      institution: $('#institution'),
      division: $('#division')
    };

    const validate = () => {
      var invalid = true;

      Object.keys(controls)
        .map(c => ({ name: c, value: controls[c].val() }))
        .map(c => ({ ...c, empty: !c.value || c.value.trim().length == 0 }))
        .forEach(c => invalid &= c.empty);

      submit.prop('disabled', invalid);
    };

    form.on('input', (e) => validate(e));

    validate();
  });
  </script>
</form>