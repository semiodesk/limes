<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MainMenu.ascx.cs" Inherits="Profiles.Framework.Modules.MainMenu.MainMenu" %>
<%@ Register TagName="History" TagPrefix="HistoryItem" Src="~/Framework/Modules/MainMenu/History.ascx" %>
<%@ Register TagName="Lists" TagPrefix="MyLists" Src="~/Framework/Modules/MainMenu/MyLists.ascx" %>
<%@ Register TagName="SearchPerson" TagPrefix="prns" Src="~/Search/Modules/SearchPerson/SearchPerson.ascx" %>
<%@ Register TagName="SearchEverything" TagPrefix="prns" Src="~/Search/Modules/SearchEverything/SearchEverything.ascx" %>

<nav id="prns-nav">
    <a href="<%=ResolveUrl("~/search")%>" title="Go to the search homepage." aria-label="Home">
        <i class="fa-solid fa-house"></i><span class="d-none">Search</span>
    </a>

    <form id="prns-minisearch" class="search-container mr-auto" method="get" action="<%=ResolveUrl("~/search/default.aspx")%>">
        <i class="fa fa-search"></i>
        <label class="d-none" for="menu-search">Search</label>
        <input type="text" name="searchfor" placeholder="Find people or research by keyword.." aria-label="Enter your search here." required/>
        <input type="hidden" name="searchtype" value="everything" />
        <HistoryItem:History runat="server" ID="ProfileHistory" Visible="true" />
    </form>

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
            <asp:Literal runat="server" ID="litGroups"></asp:Literal>
            <li id="groupListDivider" visible="false" runat="server">
                <div class="divider"></div>
            </li>
            <asp:Literal runat="server" ID="litLogOut"></asp:Literal>
        </ul>
    </div>

    <div id="prns-help">
        <a class="dropdown-toggle" href="#" tabindex="0" title="Toggle the help menu." data-toggle="dropdown" role="button" aria-label="Activate to access help pages and other documentation." aria-expanded="false">
            Help
        </a>
        <ul id="dropdown-menu-help" class="dropdown-menu dropdown-menu-right">
            <li><a class="dropdown-item" href="<%=ResolveUrl("~/about/default.aspx?tab=faq")%>">Help</a></li>
            <li><a id="data" class="dropdown-item" href="<%=ResolveUrl("~/about/default.aspx?tab=data")%>">Sharing Data</a></li>
            <li><a id="orcid" class="dropdown-item" href="<%=ResolveUrl("~/about/default.aspx?tab=orcid")%>">ORCID</a></li>
            <li><a id="about" class="dropdown-item" href="<%=ResolveUrl("~/about/default.aspx?tab=overview")%>">About</a></li>
        </ul>
    </div>
</nav>

<div id="prns-search-advanced">
    <ul class="nav nav-tabs justify-content-center justify-content-sm-start" id="advanced-search-tabs" role="tablist">
        <li class="nav-link disabled d-none d-sm-block">
            Advanced search:
            </li>
        <li class="nav-item nav-tabs-toggle" role="presentation">
            <button class="nav-link" id="people-tab" data-toggle="tab" data-target="#people-content" type="button" role="tab" aria-controls="people-content" aria-selected="false">
                <i class="fa-solid fa-users"></i> People
            </button>
        </li>
        <li class="nav-item nav-tabs-toggle" role="presentation">
            <button class="nav-link" id="research-tab" data-toggle="tab" data-target="#research-content" type="button" role="tab" aria-controls="research-content" aria-selected="false">
                <i class="fa-solid fa-microscope"></i> Research <span class="d-none d-sm-inline">& Publications</span>
            </button>
        </li>
    </ul>
    <div class="tab-content" id="advanced-search-content">
        <div class="tab-pane" id="people-content" role="tabpanel" aria-labelledby="people-tab">
            <prns:SearchPerson runat="server" />
        </div>
        <div class="tab-pane" id="research-content" role="tabpanel" aria-labelledby="research-tab">
            <prns:SearchEverything runat="server" />
        </div>
    </div>
</div>

<asp:Literal runat="server" ID="litJs"></asp:Literal>

<script type="text/javascript">
$(function () {
    setNavigation();
});

function setNavigation() {
    console.warn("setNavigation");

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
    $('.nav-item.nav-tabs-toggle button').on('click', (e) => {
        e.preventDefault();

        var tab = $(e.target);

        if (tab.hasClass('active')) {
            var content = $(e.target.dataset.target);

            window.setTimeout(() => {
                tab.removeClass('active');
                content.removeClass('active');
            }, 1);
        }
    });
});
</script>
