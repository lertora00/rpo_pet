<%@ page language="VB" autoeventwireup="false" codefile="drag.aspx.vb" inherits="story_drag" masterpagefile="~/dir_member/Shared/MasterPages/SiteLayout__member.Master" title="Pet | Story" %>

<asp:content id="ctnBodyPage" contentplaceholderid="ctnBody" runat="server">

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
	<asp:repeater id="rpt_test" runat="server">
		<itemtemplate></itemtemplate>
	</asp:repeater>

	<div id="dragLimit" style="width: 400px; height: 400px">
		<div id="draggable" ab="1" class="ui-widget-content" style="width: 100px; height: 100px">
			A - JQuery Enabled Dragging Area....
		</div>



		<div id="droppable" ab="3" class="ui-widget-content" style="width: 300px; height: 200px">
			Droppable Area....<div ab="hello">test</div>
		</div>
		<div id="droppable2" ab="3b" class="ui-widget-content" style="width: 300px; height: 200px">
			Droppable Area2....<div ab="hello2">test2</div>
		</div>
	</div>

</asp:content>

<asp:content id="ctnJavaScript" contentplaceholderid="ctnJavaScriptPage" runat="server">
	<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.3.2/jquery.min.js"></script>
	<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.7.2/jquery-ui.js"></script>

	<script>

		$(function () {
			//Use accept property to specify target for draggable.
			$("#draggable").draggable({ revert: 'invalid' });  //Reverts to previous location,when dragged without dropping
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
			$('#droppable2').droppable({
				cursor: 'move', activeClass: 'active', hoverClass: 'hover',
				drop: function (event, ui) {
					$('#droppable').append('Element Dropped...');
					var divx = $(this).attr("ab");
					var lid = $(ui.draggable).attr("ab");
					alert(divx);
					alert(lid);
				}
			});
		});
	</script>

</asp:content>



