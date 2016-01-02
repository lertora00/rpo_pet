<%@ page language="VB" autoeventwireup="false" codefile="shrink_image.aspx.vb" inherits="user_shrink_image" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
</head>
<body>
	<form id="form1" runat="server">
		<div>
			<asp:fileupload id="fileupload" runat="server" />
			<asp:button id="btn_upload" runat="server" />
			<asp:label id="label" runat="server"></asp:label>
			<asp:image id="image1" runat="server" imageurl="~/user/Uploaded-Files/tn_photo.jpg" />
		</div>
	</form>
</body>
</html>
