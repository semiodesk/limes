<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PassiveList.ascx.cs" Inherits="Profiles.Profile.Modules.PassiveList.PassiveList" %>
<asp:Repeater ID="passiveList" runat="server" OnItemDataBound="passiveList_OnItemDataBound">
    <HeaderTemplate>
        <div class="sidepanel-header">
            <h4>         
                <asp:Literal runat="server" ID="InfoCaption"></asp:Literal>
                <asp:Literal runat="server" ID="TotalCount"></asp:Literal>
            </h4>
            <asp:HyperLink runat="server" ID="moreurl">Explore</asp:HyperLink>
        </div>
        <div class="passiveSectionBody">
            <ul>
    </HeaderTemplate>
    <ItemTemplate>
        <li>
            <asp:HyperLink runat="server" ID="itemUrl"></asp:HyperLink>
        </li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
        </div>
    </FooterTemplate>
</asp:Repeater>
