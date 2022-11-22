<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchPerson.ascx.cs" Inherits="Profiles.Search.Modules.SearchPerson.SearchPerson" %>

<form id="people-search-form" class="searchForm" method="get" action="<%=ResolveUrl("~/search/default.aspx")%>">
    <input type="hidden" name="searchtype" value="people" />
    <input type="hidden" name="classuri" value="http://xmlns.com/foaf/0.1/Person" />
    <input type="hidden" name="new" value="true" />
  
    <div class="row">
        <div class="col-sm-6 form-group">
            <label for="fname">First Name</label>
            <input class="form-control" type="text" id="fname" name="fname" aria-label="Enter the first name." maxlength="50" />
        </div>
        <div class="col-sm-6 form-group">
            <label for="lname">Last Name</label>
            <input class="form-control" type="text" id="lname" name="lname" aria-label="Enter the last name." maxlength="50" />
        </div>
    </div>

    <div runat="server" id="trInstitution" class="form-group">
        <label for="institution">University or Institute</label>
        <asp:Literal runat="server" ID="litInstitution"></asp:Literal>

        <div class="form-control-subline">
            <input type="checkbox" class="form-check-input" name="institutionallexcept" id="institutionallexcept" value="true" />
            <label for="institutionallexcept" class="form-check-label">All <i>except</i> the one selected</label>
        </div>
    </div>

    <div runat="server" id="trDivision" class="form-group">
        <label for="division">Competence Center</label><small class="text-muted ml-2">e.g. CCGA</small>
        <asp:Literal runat="server" ID="litDivision"></asp:Literal>

        <div class="form-control-subline">
            <input type="checkbox" class="form-check-input" name="divisionallexcept" id="divisionallexcept" value="true" />
            <label for="divisionallexcept" class="form-check-label">All <i>except</i> the one selected</label>
        </div>
    </div>

    <div class="text-right">
        <button type="submit" class="search-button" disabled>
        <i class="fa fa-search"></i> Search
        </button>
    </div>

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