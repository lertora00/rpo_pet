<%@ page language="VB" autoeventwireup="false" codefile="Default.aspx.vb" inherits="administration_email_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
</head>
<body>
	<form id="form1" runat="server">
		<div>
			Email address to verify:
			<asp:textbox id="txt_email_address" runat="server"></asp:textbox>
			<asp:button id="btn_verify" runat="server" text="Verify" />
		</div>
	</form>
</body>
</html>
