<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PassiveList.ascx.cs" Inherits="Profiles.Profile.Modules.PassiveList.PassiveList" %>
<asp:Repeater ID="passiveList" runat="server" OnItemDataBound="passiveList_OnItemDataBound">
    <HeaderTemplate>
        <div class="passiveSectionHead">
            <h4>         
                <asp:HyperLink runat="server" ID="moreurl">
                    <i class="fa fa-chevron-right"></i>
                    <asp:Literal runat="server" ID="InfoCaption"></asp:Literal>
                    <asp:Literal runat="server" ID="TotalCount"></asp:Literal>
                </asp:HyperLink>
            </h4>
            <asp:Literal runat="server" ID="divStart"></asp:Literal>
            <asp:Literal runat="server" ID="divEnd"></asp:Literal>
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
