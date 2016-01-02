<%@ page language="C#" autoeventwireup="true" codefile="Working.aspx.cs" inherits="dir_playground_drag_and_drop_Default2" %>

<html>
<head>
	<title>jQuery UI Droppable</title>
	<link type="text/css" href="jquery-ui-1.7.2.custom.css" rel="stylesheet" />
	<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.3.2/jquery.min.js"></script>
	<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.7.2/jquery-ui.js"></script>
	<style type="text/css">
		.active {
			border: 5px solid #655e4e;
			background: blue;
			font-weight: bold;
			outline-width: thick;
		}

		.hover {
			background: yellow;
			text-decoration: none;
			outline: none;
		}
	</style>

	<script type="text/javascript">

		$(function () {
			//$("#draggable, #draggable1 ").draggable();//Shortcut to specify dragging for multiple items in single statement.
			//Use accept property to specify target for draggable.
			$("#draggable").draggable({ revert: 'invalid' });  //Reverts to previous location,when dragged without dropping
			$("#draggable1").draggable({ revert: 'valid' }); //Reverts to previous location,when dropped
			$('#Subdroppable').droppable({ greedy: true, cursor: 'move', activeClass: 'active', hoverClass: 'hover', drop: function (event, ui) { $('#Subdroppable').html('Element Dropped...'); } });
			$('#droppable').droppable({
				cursor: 'move', activeClass: 'active', hoverClass: 'hover',
				drop: function (event, ui) {
					$('#droppable').append('Element Dropped...');
					var divx = $(this).children('div').attr("ab");
					var lid = $(ui.draggable).attr("ab");
					alert(divx);
					alert(lid);
				}
			});
		});
	</script>
</head>
<body>
	<div id="dragLimit" style="width: 400px; height: 400px">
		<div id="draggable" ab="1" class="ui-widget-content" style="width: 100px; height: 100px">
			JQuery Enabled Dragging Area....
		</div>
		<div id="draggable1" ab="2" class="ui-widget-content" style="width: 100px; height: 100px">
			JQuery Enabled Dragging Area[Invalid Drop] ....
		</div>
		<div id="droppable" ab="3" class="ui-widget-content" style="width: 500px; height: 500px">
			DIV ID Droppable Non Droppable Area....
            <div id="Subdroppable" ab="4" class="ui-widget-content" style="width: 300px; height: 300px">
							Sub Droppable Area....
						</div>
		</div>
	</div>
</body>
</html>
