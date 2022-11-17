<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ActivityHistory.ascx.cs" Inherits="Profiles.Activity.Modules.ActivityHistory.ActivityHistory" %>

<section class="activities">
  <div class="sidepanel-header">
    <h3 class="act-heading-live-updates">Recent Activity</h3>
  </div>
    
  <asp:Panel runat="server" ID="pnlActivities" CssClass="act-list">
    <asp:Repeater runat="server" ID="rptActivityHistory" OnItemDataBound="rptActivityHistory_OnItemDataBound">
      <ItemTemplate>
        <asp:Literal Mode="PassThrough" runat="server" ID="body"></asp:Literal>
      </ItemTemplate>
    </asp:Repeater>
  </asp:Panel>

  <div class="act-list-footer">
    <div id="progress-spinner" class="btn btn-primary mt-2 px-5" style="display: none">
        <div class="d-flex flex-row align-items-center">
            <div class="spinner spinner-s mr-2"></div><span>Loading..</span>
        </div>
    </div>

    <button id="btn-load-next" class="btn btn-primary mt-2 px-5" onclick="loadNext()">
        View more
    </button>
  </div>
</section>

<script type="text/javascript">
    var pageSize = 20;

    function loadNext() {
        var itemsCount = $('.act-list-item').length;

        $('#btn-load-next').hide();
        $("#progress-spinner").show();

        $.ajax({
            url: "<%=GetURLDomain()%>/Activity/Modules/ActivityHistory/ActivityDetails.aspx/GetRecentActivities",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({
                offset: itemsCount,
                limit: pageSize
            }),
            success: (response) => {
                $("#progress-spinner").hide();

                if (response.d.length > 0) {
                    $('.act-list').append(response.d);
                    $('#btn-load-next').show();
                }
            },
            failure: (response) => {
                $("#progress-spinner").hide();
                $('#btn-load-next').show();

                console.error(response.d);
            },
            error: (response) => {
                $("#progress-spinner").hide();
                $('#btn-load-next').show();

                console.error(response.d);
            },
        });
    }
</script>