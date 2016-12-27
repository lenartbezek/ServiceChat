<%@ Page Language="C#" MasterPageFile="~/Master.master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ServiceChat.Login" ValidateRequest="false"%>

<asp:Content ID="Content" ContentPlaceHolderID="Content" Runat="Server">

    <form runat="server" class="row">

        <div class="col s12 m6 l4 offset-l2">
            <div class="center-align">
                <h4>Registracija</h4>
                <h6>Novi uporabniki</h6>
            </div>
              
            <label>Ime in priimek</label>
            <asp:TextBox ID="RegisterNameField" runat="server" CssClass="validate"></asp:TextBox>
            <label class="error-label"><%= RegisterNameError %></label>
              
            <label>Uporabniško ime</label>
            <asp:TextBox ID="RegisterUsernameField" runat="server" CssClass="validate"></asp:TextBox>
            <label class="error-label"><%= RegisterUsernameError %></label>
                    
            <label>Geslo</label>
            <asp:TextBox ID="RegisterPasswordField" runat="server" CssClass="validate" type="password" ></asp:TextBox>
            <label class="error-label"><%= RegisterPasswordError %></label>

            <label>Ponovite geslo</label>
            <asp:TextBox ID="RegisterPasswordRepeatField" runat="server" CssClass="validate" type="password" ></asp:TextBox>
            <label class="error-label"><%= RegisterPasswordRepeatError %></label>

            <div class="center-align">
                <asp:Button ID="RegisterButton" runat="server" Text="Registracija" 
                    CssClass="waves-effect waves-light btn" 
                    OnClick="RegisterButton_Click"
                    type="button"/>

                <p><%= RegisterSuccessMessage %></p>
            </div>
        </div>

        <div class="col s12 m6 l4">
            <div class="center-align">
                <h4>Prijava</h4>
                <h6>Obstoječi uporabniki</h6>
            </div>
                    
            <label>Uporabniško ime</label>
            <asp:TextBox ID="UsernameField" runat="server" CssClass="validate"></asp:TextBox>
            <label class="error-label"><%= UsernameError %></label>
                    
            <label>Geslo</label>
            <asp:TextBox ID="PasswordField" runat="server" CssClass="validate" type="password" ></asp:TextBox>
            <label class="error-label"><%= PasswordError %></label>

            <div class="center-align">
                <asp:Button ID="LoginButton" runat="server" Text="Prijava" 
                    CssClass="waves-effect waves-light btn" 
                    OnClick="LoginButton_Click"
                    type="submit"/>
            </div>
            
            <div class="center-align" style="margin-top: 1rem">
                <asp:Button ID="AdminLoginButton" runat="server" Text="Prijava kot administrator" 
                    CssClass="waves-effect waves-teal btn-flat grey-text" 
                    OnClick="AdminLoginButton_Click"
                    type="button"/>
                <br/>
                <p><%= MissingRoleError %></p>
            </div>
            
        </div>

    </form>

</asp:Content>