<%@ page language="VB" autoeventwireup="false" codefile="next.aspx.vb" inherits="administration_question_next" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
</head>
<body>
	<form id="form1" runat="server">
		<div>
			<br />
			<asp:label id="lbl_error" runat="server" forecolor="Red"></asp:label>
			<br />
			<br />
			Use this to ask user's questions.  Either by phone number (must be valid user) or all.
			<br />
			<br />
			Specific valid user's phone number to text:
			<asp:textbox id="txt_phone_number" runat="server"></asp:textbox>
			<br />
			<asp:button id="btn_text_specific_user" runat="server" text="Text Specific User their next Question." />
			<br />
			<br />
			<asp:button id="btn_text_all" runat="server" text="Text all valid users their next Question." />
		</div>
	</form>
</body>
</html>
