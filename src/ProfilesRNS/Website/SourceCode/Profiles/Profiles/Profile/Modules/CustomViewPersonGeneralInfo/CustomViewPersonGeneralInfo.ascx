<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomViewPersonGeneralInfo.ascx.cs"
    Inherits="Profiles.Profile.Modules.CustomViewPersonGeneralInfo.CustomViewPersonGeneralInfo" %>

<div class="prns-subject-container">
    <div class="d-flex flex-column align-items-center flex-sm-row align-items-sm-start text-center text-sm-left mt-3">
        <div class="profile-image">
            <asp:Image itemprop="image" runat="server" ID="imgPhoto" />
        </div>
        <asp:Literal runat="server" ID="litPersonalInfo"></asp:Literal>
    </div>
</div>

<div id="toc"><ul></ul><div style="clear:both;"></div></div>

<!-- for testing ORNG gadgets -->
<asp:Panel runat="server" ID="pnlSandboxGadgets" Visible="false">
    <div class= "PropertyGroup">Newly found "Sandbox" Gadgets</div>
    <div class="SupportText">Note that this section is only visible when you login in through the 
        <asp:HyperLink ID="hlORNG" NavigateUrl="~/ORNG" runat="server">ORNG</asp:HyperLink> interface with new gadgets that you want to test. Unrecognized Gadgets will be rendered in a "sandbox" view</div><p></p>
    <asp:Literal runat="server" ID="litSandboxGadgets" Visible="false"/>
</asp:Panel>
