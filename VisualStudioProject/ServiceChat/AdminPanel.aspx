<%@ Page Title="" Language="C#" MasterPageFile="~/Master.master" AutoEventWireup="true" CodeBehind="AdminPanel.aspx.cs" Inherits="ServiceChat.AdminPanel" %>
<%@ Import Namespace="ServiceChat" %>

<asp:Content ID="Content" ContentPlaceHolderID="Content" Runat="Server">
    <form runat="server">
        
        <div class="row">
            
            <div class="col s12 push-m8 m4 push-l10 l2">
                <asp:Button ID="LogoutButton" runat="server"
                    CssClass="btn-floating btn-large red fontello" 
                    OnClick="LogoutButton_Click"
                    Text=""/>

            </div>

            <div class="col s12 pull-m4 m8 pull-l2 l10">
                <ul class="collection">

                    <% foreach (var account in AccountList) { %>
                    <li class="collection-item">
                        <div class="row">
                            <strong class="col m2"><%= HttpUtility.HtmlEncode(account.Username) %></strong>
                            <span class="col m3"><%= HttpUtility.HtmlEncode(account.DisplayName) %></span>
                            <span class="col m3">Število sporočil: <%= MessageList.Count(m => m.Username == account.Username) %></span>
                            <div class="col-m4">
                                <input type="checkbox" class="filled-in" <% if (account.Admin) { %>checked="checked" <% } %> />
                                <label>Administrator</label>
                                <a class="waves-effect waves-red btn-flat">Izbriši</a>
                            </div>
                        </div>
                        
                        <div class="secondary-content">
                            
                        </div>
                    </li>
                    <% } %>

                </ul>
            </div>

        </div>

    </form>
</asp:Content>
