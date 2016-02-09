<%@ page language="VB" autoeventwireup="false" codefile="default.aspx.vb" inherits="photography_default" masterpagefile="~/dir_public/Shared/MasterPages/Site.Master" title="Photography | Jeannie Bartow Hartman" %>

<asp:content id="headPage" contentplaceholderid="head" runat="server">
</asp:content>
<asp:content id="bodyPage" contentplaceholderid="ContentBody" runat="server">
	<section class="section-divider">
		<div class="container w well2">
			<div class="row well">
				<h4 class="color2">Photography</h4>
				<p class="color2">
					Jeannie Bartow Hartman is a leading Photographer based in Atlanta specializing in the photography of domestic animals.  Her education in the field of Photography and Fine Art includes New York University, Parsons School of Design, the International Center for Photography, and the Atlanta College of Art.
Her stock photography work has been licensed throughout the world by publishers on a variety of commercial products. 
				</p>

				<br />
				<div class="contact">
					For more information on the services Jeannie offers, you can reach her at:
				<br />
					Jeannie Bartow Hartman  |  5805 Winterthur Lane  |  Atlanta, GA 30328  USA
				<br />
					<br />
					JeannieHartman7@gmail.com  |  GeoDecadence.com
				</div>

			</div>
			<div class="row">
				<div class="col-lg-4 center">
					<asp:image id="img_02" runat="server" imageurl="~/dir_image/dir_petfolio/mist.JPG" cssclass="img-responsive dropshadow center" />
				</div>
				<div class="col-lg-4 center">
					<asp:image id="img_01" runat="server" imageurl="~/dir_image/dir_petfolio/JBH_Horse_5.JPG" cssclass="img-responsive dropshadow center" />
					<h4>Jeannie Bartow Hartman Photograpy</h4>
				</div>
				<div class="col-lg-4 center">
					<asp:image id="img_03" runat="server" imageurl="~/dir_image/dir_petfolio/riderless.JPG" cssclass="img-responsive dropshadow center" />
				</div>
			</div>
		</div>
	</section>
</asp:content>
