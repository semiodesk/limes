<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MainMenu.ascx.cs" Inherits="Profiles.Framework.Modules.MainMenu.MainMenu" %>
<%@ Register TagName="History" TagPrefix="HistoryItem" Src="~/Framework/Modules/MainMenu/History.ascx" %>
<%@ Register TagName="Lists" TagPrefix="MyLists" Src="~/Framework/Modules/MainMenu/MyLists.ascx" %>
<div id="prns-nav">
    <!-- MAIN NAVIGATION MENU -->
    <nav>
        <ul class="prns-main primary">
            <li class="main-nav">
                <a href="<%=ResolveUrl("~/search")%>"><i class="bi bi-search"></i>></a>
            </li>
            <li class="main-nav item-search">
                <div class="search-container">
                    <input name="search" id="menu-search" placeholder="Find people by name.." type="text"/>
                </div>
            </li>
        </ul>
        <ul class="prns-main secondary">
            <HistoryItem:History runat="server" ID="ProfileHistory" Visible="true" />
            <li class="main-nav" style="margin-left: auto;">
                <a href='#'>About</a>
                <ul class="drop">
                    <li>
                        <a id="about" href="<%=ResolveUrl("~/about/default.aspx?tab=overview")%>">Overview</a>
                    </li>
                    <li>
                        <a id="data" href="<%=ResolveUrl("~/about/default.aspx?tab=data")%>">Sharing Data</a>
                    </li>
                    <li>
                        <a id="orcid" href="<%=ResolveUrl("~/about/default.aspx?tab=orcid")%>">ORCID</a>
                    </li>
                </ul>

            </li>
            <li class="main-nav">
                <a href="<%=ResolveUrl("~/about/default.aspx?tab=faq")%>">Help</a>
            </li>
            <%-- <li class="main-nav">
                <a href="<%=ResolveUrl("~/about/default.aspx?type=UseOurData")%>">Use Our Data</a>
                <ul class="drop">
                    <li>
                        <a id="useourdata" href="<%=ResolveUrl("~/about/default.aspx?type=UseOurData")%>">Overview</a>
                    </li>
                    <asp:Literal runat="server" ID="litExportRDF"></asp:Literal>
                </ul>
            </li>--%>
        </ul>
        <!-- USER LOGIN MSG / USER FUNCTION MENU -->
        <div id="prns-usrnav" class="pub" class-help="class should be [pub|user]">
            <div class="loginbar">
                <asp:Literal runat="server" ID="litLogin"></asp:Literal>
            </div>
            <!-- SUB NAVIGATION MENU (logged on) -->
            <ul class="usermenu">
                <asp:Literal runat="server" ID="litViewMyProfile"></asp:Literal>
                <li style="margin-top: 0px !important;">
                    <div class="divider"></div>
                </li>
                <asp:Literal runat="server" ID="litEditThisProfile"></asp:Literal>
                <li>
                    <div class="divider"></div>
                </li>
                <asp:Literal runat="server" ID="litProxy"></asp:Literal>               
                <li id="ListDivider">
                    <div class="divider"></div>
                </li>
                <li id="navMyLists">
                   <a href="#">My Person List (<span id="list-count">0</span>)</a>
                    <MyLists:Lists runat="server" ID="MyLists" Visible="false" />
                </li>
                 <li>
                    <div class="divider"></div>
                </li>
              <%--  <li>
                    <asp:Literal ID="litDashboard" runat="server" /></li>
                <li>
                    <div class="divider"></div>
                </li>--%>
                <asp:Literal runat="server" ID="litGroups"></asp:Literal>
                <li id="groupListDivider" visible="false" runat="server">
                    <div class="divider"></div>
                </li>
                <asp:Literal runat="server" ID="litLogOut"></asp:Literal>
            </ul>
        </div>
    </nav>
</div>

<asp:Literal runat="server" ID="litJs"></asp:Literal>
<script type="text/javascript">
    $(function () {
        setNavigation();
    });

    function setNavigation() {
        var path = $(location).attr('href');
        path = path.replace(/\/$/, "");
        path = decodeURIComponent(path);

        $(".prns-main li").each(function () {

            var href = $(this).find("a").attr('href');
            var urlParams = window.location.search;

            if ((path + urlParams).indexOf(href) >= 0) {
                $(this).addClass('landed');
            }
        });


        return true;
    }

    $(document).ready(function () {
        $("#menu-search").on("keypress", function (e) {
            if (e.which == 13) {
                minisearch();
                return false;
            }
            return true;
        });

        $("#img-mag-glass").on("click", function () {
            minisearch();
            return true;
        });
    });

    function minisearch() {
        var keyword = $("#menu-search").val();
        var classuri = 'http://xmlns.com/foaf/0.1/Person';
        document.location.href = '<%=ResolveUrl("~/search/default.aspx")%>?searchtype=people&searchfor=' + keyword + '&classuri=' + classuri;
        return true;
    }
</script>


