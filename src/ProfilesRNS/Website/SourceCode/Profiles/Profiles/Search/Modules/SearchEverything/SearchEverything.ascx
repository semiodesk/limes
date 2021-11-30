<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchEverything.ascx.cs"
Inherits="Profiles.Search.Modules.SearchEverything.SearchEverything" %>

<form id="everything-search-form" class="searchForm" method="get" action="<%=ResolveUrl("~/search/default.aspx")%>">
  <h3>Find publications, research and more</h3>

  <input type="hidden" name="searchtype" value="everything" />
  <input type="hidden" name="classgroupuri" value="" />
  <input type="hidden" name="classuri" value="" />

  <section class="searchSection">
    <div class="form-group pt-3">
      <label for="searchfor">Keywords</label>
      <div colspan="2" class="fieldOptions">
        <input type="text" name="searchfor" id="searchfor" aria-label="Enter your search keywords here." maxlength="50" />
        <p class="form-control-subline">
          <input type="checkbox" name="exactphrase" id="exactphrase" aria-label="Enforce exact matching of the keywords." value="true"/>
          <label for="exactphrase">Search for <i>exact</i> phrase</label>
        </p>
      </div>
    </div>
    <div class="form-group text-right">
      <button type="submit" class="search-button" disabled>
        <i class="fa fa-search"></i> Search
      </button>
    </div>
  </section>
  <script type="text/javascript">
  $(document).ready(() => {
    const form = $('#everything-search-form');
    const submit = $('#everything-search-form .search-button');

    const controls = {
      searchfor: $('#searchfor'),
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
