<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchEverything.ascx.cs" Inherits="Profiles.Search.Modules.SearchEverything.SearchEverything" %>

<form id="everything-search-form" class="searchForm" method="get" action="<%=ResolveUrl("~/search/default.aspx")%>">
    <input type="hidden" name="searchtype" value="everything" />
    <input type="hidden" name="classuri" value="" />
    <input type="hidden" name="new" value="true" />

    
    <label for="classgroupuri">Type</label>
    <div class="form-group"'>        
        <div class="form-check form-check-inline">
            <input class="form-check-input" type="radio" name="classgroupuri" id="type-research" value="http://profiles.catalyst.harvard.edu/ontology/prns#ClassGroupResearch" checked>
            <label class="form-check-label" for="type-research">Research</label>
        </div>

        <div class="form-check form-check-inline">
            <input class="form-check-input" type="radio" name="classgroupuri" id="type-concepts" value="http://profiles.catalyst.harvard.edu/ontology/prns#ClassGroupConcepts">
            <label class="form-check-label" for="type-concepts">Concepts</label>
        </div>
    </div>

    <div class="form-group">
        <label for="searchfor">Keywords</label>
        <input class="form-control" type="text" name="searchfor" id="searchfor" aria-label="Enter your search keywords here." maxlength="50" />

        <div class="form-control-subline">
            <input type="checkbox" name="exactphrase" id="exactphrase" aria-label="Enforce exact matching of the keywords." value="true"/>
            <label for="exactphrase">Search for <i>exact</i> phrase</label>
        </div>
    </div>

    <div class="text-right">
        <button type="submit" class="search-button" disabled>
        <i class="fa fa-search"></i> Search
        </button>
    </div>

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
