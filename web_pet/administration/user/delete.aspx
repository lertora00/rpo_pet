<%@ Page Language="VB" AutoEventWireup="false" CodeFile="delete.aspx.vb" Inherits="administration_user_delete" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>

			Enter the username to delete...
			<br />
			Username: <asp:textbox id="txt_username" runat="server"></asp:textbox>
			<asp:button id="btn_delete"	runat="server" text="Delete Person, User, Email, Phone, etc." />

		<asp:placeholder id="plc_user" runat="server" visible="false">
			<br />
			<br />
			User found - firstname is: <asp:label id="lbl_first_name" runat="server"></asp:label>
			<br />
			Person: <asp:label id="lbl_pk_person" runat="server"></asp:label>
			<br />
			Enter delete password: <asp:textbox id="txt_delete_password" runat="server" textmode="Password"></asp:textbox>
			<asp:button id="btn_delete_for_real" runat="server" text="Delete For Real?" />

		</asp:placeholder>
    
    </div>
    </form>
</body>
</html>
