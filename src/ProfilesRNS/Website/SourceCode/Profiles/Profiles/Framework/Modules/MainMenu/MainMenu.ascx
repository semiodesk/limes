<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MainMenu.ascx.cs" Inherits="Profiles.Framework.Modules.MainMenu.MainMenu" %>
<%@ Register TagName="History" TagPrefix="HistoryItem" Src="~/Framework/Modules/MainMenu/History.ascx" %>
<%@ Register TagName="Lists" TagPrefix="MyLists" Src="~/Framework/Modules/MainMenu/MyLists.ascx" %>
<div id="prns-nav">
    <!-- MAIN NAVIGATION MENU -->
    <nav>
        <ul class="prns-main primary">
            <HistoryItem:History runat="server" ID="ProfileHistory" Visible="true" />
            <li class="main-nav item-search">
                <div class="search-container">
                    <label class="d-none" for="menu-search">Search</label>
                    <input name="search" id="menu-search" placeholder="Find people by name.." type="text" aria-label="Enter your search here."/>
                </div>
            </li>
        </ul>
        <ul class="prns-main secondary">
            <li class="main-nav">
                <a href="<%=ResolveUrl("~/search")%>" title="Go to the search homepage." aria-label="Home">
                    <i class="fa fa-home"></i><span class="d-none">Search</span>
                </a>
            </li>
            <li class="main-nav" style="margin-left: auto;">
                <a id="menu-help-toggle" class="menu-toggle" tabindex="0" title="Toggle the help menu." aria-label="Help">
                    <i class="fa fa-question-circle"></i><span class="d-none">About</span>
                </a>
                <ul id="menu-help-drop" class="menu-drop align-right">
                    <li>
                        <a href="<%=ResolveUrl("~/about/default.aspx?tab=faq")%>">Help</a>
                    </li>
                    <li>
                        <a id="data" href="<%=ResolveUrl("~/about/default.aspx?tab=data")%>">Sharing Data</a>
                    </li>
                    <li>
                        <a id="orcid" href="<%=ResolveUrl("~/about/default.aspx?tab=orcid")%>">ORCID</a>
                    </li>
                    <li>
                        <a id="about" href="<%=ResolveUrl("~/about/default.aspx?tab=overview")%>">About</a>
                    </li>
                </ul>
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

        $('#menu-help-drop').hide();

        $('#menu-help-toggle').on('click', (e) => {
            $('#menu-help-toggle').toggleClass('toggled');
            $('#menu-help-drop').toggle();

            e.stopPropagation()
        })

        $('#menu-help-toggle').on('keypress', (e) => {
            var keycode = (event.keyCode ? event.keyCode : event.which);
            
            if(keycode == '13') {
                $('#menu-help-toggle').toggleClass('toggled');
                $('#menu-help-drop').toggle();

                e.stopPropagation()
            }
        })

        $("body").on('click', (e) => {
            $('.menu-toggle').removeClass('toggled');
            $('.menu-drop').hide();
        })

        $(document).on('keydown', (e) => {
            var keycode = (event.keyCode ? event.keyCode : event.which);
            
            if(keycode == '27') {
                $('.menu-toggle').removeClass('toggled');
                $('.menu-drop').hide();
            }
        })
    });

    function minisearch() {
        var keyword = $("#menu-search").val();
        var classuri = 'http://xmlns.com/foaf/0.1/Person';
        document.location.href = '<%=ResolveUrl("~/search/default.aspx")%>?searchtype=people&searchfor=' + keyword + '&classuri=' + classuri;
        return true;
    }
</script>


