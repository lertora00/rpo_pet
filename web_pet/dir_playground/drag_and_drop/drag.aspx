<%@ page language="VB" autoeventwireup="false" codefile="drag.aspx.vb" inherits="dir_playground_drag" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>

	<script src="http://ajax.aspnetcdn.com/ajax/jquery/jquery-1.10.0.min.js"></script>
	<script src="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.10.3/jquery-ui.min.js" type="text/javascript"></script>
	<link href="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.10.3/themes/blitzer/jquery-ui.css" rel="stylesheet" type="text/css" />
	<script>
		$(function () {
			$("#technology").sortable();
		});
	</script>
</head>
<body>
	<form id="form1" runat="server">
		<div>

			<div>
				<div class="row">
					<ul id="technology">
						<li>1. ASP.NET</li>
						<li>2. MVC</li>
						<li>3. WCF</li>
						<li>4. Web API</li>
						<li>5. SignalR</li>
					</ul>

				</div>
			</div>
		</div>
	</form>
</body>
</html>
