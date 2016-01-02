<%@ page language="VB" autoeventwireup="false" codefile="contenteditible.aspx.vb" inherits="dir_playground_contenteditible" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
	<style>
		body {
			background-color: lightgrey;
		}

		h1 {
			color: blue;
		}

		p {
			color: green;
		}
		.asd {
    background:rgba(0,0,0,0);
		border: 1px solid rgba(1,1,1,1);
}
		.hover {
  -moz-box-shadow: 0px 0px 4px #ffffff;
  -webkit-box-shadow: 0px 0px 4px #ffffff;
  box-shadow: 0px 0px 4px #ffffff;
}
	</style>

</head>
<body>
	<form id="form1" runat="server">
		<div contenteditable="true">test</div>
		<div>

			<asp:label id="lbl_name" runat="server" contenteditable="true" text="initial text"></asp:label>
		</div>
		<input type="text" class="asd" value="Test" onmouseover="this.addClass=('hover');" onmouseout="this.removeClass=('hover');" />
	</form>
</body>
</html>
