<%@ page language="VB" autoeventwireup="false" codefile="validatortest.aspx.vb" inherits="security_validatortest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
</head>
<body>
	<form id="form1" runat="server">
		<div>
			<asp:textbox id="txt_first_name" runat="server"></asp:textbox>
			<asp:requiredfieldvalidator id="x" runat="server" errormessage="Required" controltovalidate="txt_first_name"></asp:requiredfieldvalidator>
			<asp:button id="btn_validate" text="Validate" runat="server" />
		</div>
	</form>
</body>
</html>
