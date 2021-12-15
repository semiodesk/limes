<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomViewPersonSameDepartment.ascx.cs"
    Inherits="Profiles.Profile.Modules.CustomViewPersonSameDepartment.CustomViewPersonSameDepartment" %>
<asp:Repeater ID='rptSameDepartment' runat="server" OnItemDataBound="SameDepartmentItemBound">
    <HeaderTemplate>
        <div class="sidepanel-header">
            <h4>         
                Same Department
            </h4>
            <asp:HyperLink runat="server" ID="moreurl" Text="Explore" Title="People who are also in this person's primary department." CssClass="prns-explore-btn"></asp:HyperLink>
        </div>
        <div class="passiveSectionBody">
            <ul>
    </HeaderTemplate>
    <ItemTemplate>
        <asp:Literal runat="server" ID="litListItem"></asp:Literal>
    </ItemTemplate>
    <FooterTemplate>
         </ul>   
        </div>
        
    </FooterTemplate>
</asp:Repeater>
