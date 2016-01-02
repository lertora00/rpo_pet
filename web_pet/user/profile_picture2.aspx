<%@ page language="VB" autoeventwireup="false" codefile="profile_picture2.aspx.vb" inherits="user_profile_picture" masterpagefile="~/dir_member/Shared/MasterPages/jcrop.Master" title="Pet | Story" %>

<asp:content id="ctnBodyPage" contentplaceholderid="ctnBody" runat="server">
	<script type ="text/javascript" >
        function sub_show_button() {
        	document.getElementById("<%=btnUpload.ClientID%>").style.display = "";
        }
    </script>

	<script type="text/javascript" src="../dir_script/jcrop/jquery.Jcrop.js"></script>
	<script type="text/javascript">
		jQuery(document).ready(function () {
			jQuery('#<%= imgCrop.ClientID %>').Jcrop({
				aspectRatio: 100 / 100,
				onSelect: storeCoords
			});
		});

		function storeCoords(c) {
			jQuery('#<%= X.ClientID %>').val(c.x);
			jQuery('#<%= Y.ClientID %>').val(c.y);
			jQuery('#<%= W.ClientID %>').val(c.w);
			jQuery('#<%= H.ClientID %>').val(c.h);
		};

	</script>

	<asp:label id="lbl_show_row_count" runat="server" visible="false">8</asp:label>
	<div id="page-wrapper">
		<div class="row">
			<div class="col-lg-12">
				<h1 class="page-header">
					<asp:label id="lbl_pk_anthology" runat="server" visible="false"></asp:label>
						<asp:image id="img_anthology" runat="server" imageurl="~/dir_image/pet_stick_figure_with_link.jpg" />&nbsp;<asp:label id="lbl_pet_name__navigation" runat="server">Teddy</asp:label>&nbsp;<i><asp:label font-size="16px" id="lbl_pet_tagline" runat="server">Who's a bad dog?</asp:label></i>
				</h1>
			</div>
		</div>

		<asp:placeholder id="plc_success" runat="server" visible="false">
			<div class="row">
				<div class="col-lg-9">
					<div class="alert alert-success">
						<asp:label id="lbl_success" runat="server"></asp:label>
					</div>
				</div>
			</div>
		</asp:placeholder>

		<div class="row">
			<asp:panel id="pnlUpload" runat="server">
				<asp:fileupload id="Upload" runat="server" onchange="sub_show_button();" />
				<br />
				<input id="xx" type="button" style="display:none;" />
				<asp:button id="btnUpload" runat="server" onclick="btnUpload_Click" text="Upload" style="display:none;" cssclass="btn btn-primary" />
				<asp:label id="lblError" runat="server" visible="false" />
			</asp:panel>
			<asp:panel id="pnlCrop" runat="server" visible="false">
				<asp:image id="imgCrop" runat="server" />
				<br />
				<asp:hiddenfield id="X" runat="server" value="20" />
				<asp:hiddenfield id="Y" runat="server" value="20" />
				<asp:hiddenfield id="W" runat="server" value="200" />
				<asp:hiddenfield id="H" runat="server" value="200" />
				<asp:button id="btnCrop" runat="server" text="Crop" cssclass="btn btn-primary" onclick="btnCrop_Click" />
			</asp:panel>
			<asp:panel id="pnlCropped" runat="server" visible="false">
				<asp:image id="imgCropped" runat="server" />
			</asp:panel>

			<br />
			<br />
		</div>

	</div>
</asp:content>








