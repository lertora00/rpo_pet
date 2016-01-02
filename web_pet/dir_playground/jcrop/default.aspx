<%@ page language="VB" autoeventwireup="false" codefile="default.aspx.vb" inherits="dir_playground_jcrop_default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>

	<link rel="stylesheet" href="../../dir_member/Content/StyleSheets/jcrop/jcrop.css" type="text/css" />
	<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.3/jquery.min.js"></script>
	<script type="text/javascript" src="../../dir_script/jcrop/jquery.Jcrop.js"></script>
	<script type="text/javascript">
		jQuery(document).ready(function () {
			jQuery('#imgCrop').Jcrop({
				aspectRatio: 100 / 100,
				onSelect: storeCoords
			});
		});

		function storeCoords(c) {
			jQuery('#X').val(c.x);
			jQuery('#Y').val(c.y);
			jQuery('#W').val(c.w);
			jQuery('#H').val(c.h);
		};

	</script>
</head>
<body>
	<form id="form1" runat="server">
		<div>
			<asp:panel id="pnlUpload" runat="server">
				<asp:fileupload id="Upload" runat="server" />
				<br />
				<asp:button id="btnUpload" runat="server" onclick="btnUpload_Click" text="Upload" />
				<asp:label id="lblError" runat="server" visible="false" />
			</asp:panel>
			<asp:panel id="pnlCrop" runat="server" visible="false">
				<asp:image id="imgCrop" runat="server" />
				<br />
				<asp:hiddenfield id="X" runat="server" value="20" />
				<asp:hiddenfield id="Y" runat="server" value="20" />
				<asp:hiddenfield id="W" runat="server" value="200" />
				<asp:hiddenfield id="H" runat="server" value="200" />
				<asp:button id="btnCrop" runat="server" text="Crop" onclick="btnCrop_Click" />
			</asp:panel>
			<asp:panel id="pnlCropped" runat="server" visible="false">
				<asp:image id="imgCropped" runat="server" />
			</asp:panel>
		</div>
	</form>
</body>
</html>
