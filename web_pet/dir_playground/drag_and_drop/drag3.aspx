<%@ page language="VB" autoeventwireup="false" codefile="drag3.aspx.vb" inherits="dir_playground_drag3" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>

	<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<link rel="stylesheet" href="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8.24/themes/smoothness/jquery-ui.css" />
<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8.24/jquery-ui.min.js"></script>
<script type="text/javascript">
$(function () {
    $("[id*=gvLocations]").sortable({
        items: 'tr:not(tr:first-child)',
        cursor: 'pointer',
        axis: 'y',
        dropOnEmpty: false,
        start: function (e, ui) {
            ui.item.addClass("selected");
        },
        stop: function (e, ui) {
        	ui.item.removeClass("selected");
        },
        receive: function (e, ui) {
            $(this).find("tbody").append(ui.item);
        }
    });
});
</script>
</head>
<body>
	<form id="form1" runat="server">
		<asp:gridview id="gvLocations" runat="server" autogeneratecolumns="false">
			<columns>
				<asp:templatefield headertext="Id" itemstyle-width="30">
					<itemtemplate>
						<%# Eval("Id") %>
						<input type="hidden" name="LocationId" value='<%# Eval("Id") %>' />
					</itemtemplate>
				</asp:templatefield>
				<asp:boundfield datafield="Location" headertext="Location" itemstyle-width="150" />
				<asp:boundfield datafield="Preference" headertext="Preference" itemstyle-width="100" />
			</columns>
		</asp:gridview>
		<br />
		<asp:button text="Update Preference" runat="server" onclick="UpdatePreference" />

	</form>
</body>
</html>
