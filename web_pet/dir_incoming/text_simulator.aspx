<%@ page language="VB" autoeventwireup="false" codefile="text_simulator.aspx.vb" inherits="dir_incoming_text_simulator" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
</head>
<body>
	<form id="form1" runat="server">
		<div>
			From:
			<input type="text" name="From" />
			Body:
			<input type="text" name="Body" />
			nummedia
			<input type="text" name="nummedia" />
			media content type 0
			<input type="text" name="mediacontenttype0" />
			media url 0
			<input type="text" name="mediaurl0" />
			<asp:button id="txt_post" runat="server" postbackurl="~/dir_incoming/receiver.aspx" text="Post" />
		</div>
	</form>
</body>
</html>
