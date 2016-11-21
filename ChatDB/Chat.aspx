<%@ Page Language="C#" MasterPageFile="~/Master.master" AutoEventWireup="true" CodeBehind="Chat.aspx.cs" Inherits="ChatDB.Chat" %>

<asp:Content ID="Content" ContentPlaceHolderID="Content" Runat="Server">
    <script src="js/moment.min.js"></script>
    <script src="js/moment-locale.min.js"></script>

    <script>
        $(function () {
            moment.locale("sl-SI");
            var serverTime = moment("<%= DateTime.UtcNow.ToString("o") %>");
            $(".message-date").each((i, e) => { return $(e).html(moment($(e).data("iso")).from(serverTime)); });
            $("#MessageField").focus().addClass("active");
            $(".message-list").scrollTop(999999);
        });
    </script>

    <form runat="server">

    <div class="row">
        <div class="col s12 m9 message-list" id="Messages">
            <ul>
                <% foreach (var m in MessageList) { %>
                    <li class="row">
                        <span class="col s12 m3 message-author"><%= HttpUtility.HtmlEncode(m.Author.DisplayName) %></span>
                        <span class="col s12 m6 message-text"><%= HttpUtility.HtmlEncode(m.Text) %></span>
                        <span class="col s12 m3 message-date" data-iso="<%= m.Time.ToString("o") %>"></span>
                    </li>
                <% } %>
                <% if (MessageList.Count < 1) { %>
                    <li class="row">
                        <span class="col m12">Tukaj ni nobenih sporočil.</span>
                    </li>
                <% } %>
            </ul>
        </div>

        <div class="col s12 m3 users-list" id="Users">
            <ul>
                <% foreach (var account in ChatDB.Account.GetAll()) { %>
                    <li class="<%= (account.Username == Context.User.Identity.Name) ? "chip blue-grey white-text" : "chip" %>"
                    ><%= HttpUtility.HtmlEncode(account.DisplayName) %></li>
                <% } %>
            </ul>
        </div>
    </div>

    <div class="row">
        <div class="col s12 m9">
            <label>Sporočilo</label>
            <asp:TextBox ID="MessageField" runat="server"></asp:TextBox>
        </div>
        <div class="col s4 m1 center-align">
            <asp:Button ID="SendButton" runat="server"
                CssClass="btn-floating btn-large green fontello" 
                OnClick="SendButton_Click"
                Text=""
                type="submit"/>
        </div>
        <div class="col s4 m1 center-align">
            <asp:Button ID="RefreshButton" runat="server"
                CssClass="btn-floating btn-large blue fontello" 
                OnClick="RefreshButton_Click"
                Text=""/>
        </div>
        <div class="col s4 m1 center-align">
            <asp:Button ID="LogoutButton" runat="server"
                CssClass="btn-floating btn-large red fontello" 
                OnClick="LogoutButton_Click"
                Text=""/>
        </div>
    </div>

    </form>

</asp:Content>