<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MainMenu.ascx.cs" Inherits="Profiles.Framework.Modules.MainMenu.MainMenu" %>
<%@ Register TagName="History" TagPrefix="HistoryItem" Src="~/Framework/Modules/MainMenu/History.ascx" %>
<%@ Register TagName="Lists" TagPrefix="MyLists" Src="~/Framework/Modules/MainMenu/MyLists.ascx" %>

<div id="prns-nav">
    <!-- MAIN NAVIGATION MENU -->
    <nav>
        <ul class="prns-main primary">
            <li class="main-nav item-search">
                <form class="search-container" id="minisearch" method="get" action="<%=ResolveUrl("~/search/default.aspx")%>">
                    <i class="fa fa-search"></i>
                    <label class="d-none" for="menu-search">Search</label>
                    <input type="text" name="searchfor" placeholder="Find people or research by keyword.." aria-label="Enter your search here." required/>
                    <input type="hidden" name="searchtype" value="everything" />
                    <input type="hidden" name="classuri" value="http://xmlns.com/foaf/0.1/Person" />
                    <HistoryItem:History runat="server" ID="ProfileHistory" Visible="true" />
                </form>
            </li>
        </ul>
        <ul class="prns-main secondary">
            <li class="main-nav">
                <a href="<%=ResolveUrl("~/search")%>" title="Go to the search homepage." aria-label="Home">
                    <i class="fa-solid fa-house"></i><span class="d-none">Search</span>
                </a>
            </li>
            <li class="main-nav" style="margin-left: auto;">
                <a class="menu-toggle" data-drop="help-menu-drop" tabindex="0" title="Toggle the help menu." aria-label="Activate to access help pages and other documentation.">
                    Help
                </a>
                <ul id="help-menu-drop" class="menu-drop align-right">
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
                <ul class="menu-drop">
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
    hideMenu = (e) => {
        $('.menu-toggle').removeClass('toggled');
        $('.menu-drop').hide();
    }

    toggleMenu = (e) => {
        var toggle = $(e.currentTarget);
        var drop = $('#' + toggle.attr('data-drop'));

        if(drop) {
            toggle.toggleClass('toggled');
            drop.toggle();

            e.stopPropagation()
        }
    }

    $('.menu-drop').hide();

    $('.menu-toggle').on('click', (e) => toggleMenu(e));
    $('.menu-toggle').on('keypress', (e) => {
        var keycode = (event.keyCode ? event.keyCode : event.which);
            
        if(keycode == '13') {
            toggleMenu(e);
        }
    })

    $(document).on('click', (e) => hideMenu(e))
    $(document).on('keydown', (e) => {
        var keycode = (event.keyCode ? event.keyCode : event.which);
            
        if(keycode == '27') {
            hideMenu(e);
        }
    })
});
</script>
