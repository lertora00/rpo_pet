<%@ page language="VB" autoeventwireup="false" codefile="default.aspx.vb" inherits="administration_user_default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
</head>
<body>
	<form id="form1" runat="server">
		<div>

			<asp:datagrid id="dg_user" runat="server" pagesize="50" allowpaging="true" allowsorting="true"></asp:datagrid>

		</div>
	</form>
</body>
</html>
