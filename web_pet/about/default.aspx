<%@ page language="VB" autoeventwireup="false" codefile="default.aspx.vb" inherits="about_default" masterpagefile="~/dir_public/Shared/MasterPages/Site.Master" title="About | Our Team" %>
<asp:content id="headPage" contentplaceholderid="head" runat="server">
</asp:content>
<asp:content id="bodyPage" contentplaceholderid="ContentBody" runat="server">
	<section class="section-divider divider4">
		<div class="container w well2">
			<div class="row well">
				<h4>About</h4>
				PetFolio’s mission is to help extract and capture your pet’s amazing story. &nbsp;We do this by regularly sending (text or email) you thought-provoking questions that along with your answers are stored permanently online where they can be viewed and edited. In the process, you'll be creating an amazing memory "book" (PetFolio!) that tells and re-tells your pet’s story in your own voice.
			</div>
			<div class="row">
				<div class="col-lg-4">
					<div class="well">
						<h4>Brian c
						</h4>
						Brian (aka <b>"The Business"</b>). &nbsp;Brian's focus is on the business-side of the house. &nbsp;The proud owner of Sadie, a 13-year old Boxer/Pit mix adopted through his wife Kim. &nbsp;Sadie and Brian’s young son are best friends and Brian wanted to help create a way to record memories of Sadie he could share with his son when he gets older.
						<asp:image id="img_about__brian" runat="server" imageurl="~/dir_image/about/brian.jpg" cssclass="img-responsive dropshadow" />
					</div>

				</div>
				<div class="col-lg-4">
					<div class="well">
						<h4>Phil
						</h4>
						Phil (aka <b>"The Geek"</b>). &nbsp;Phil's job is to figure out all the technical detail. &nbsp;Phil, Rebecca, Kailin, and Jacob have two labs - one black (Cooper) and the other, a troublesome chocolate named Teddy (or otherwise knows as The Gremlin). &nbsp;Without PetFolio, how could we ever remember (forget?) all the fun, labrador puppy years!
						<asp:image id="img_about__phil" runat="server" imageurl="~/dir_image/about/phil.jpg" cssclass="img-responsive dropshadow" />
					</div>
				</div>
				<div class="col-lg-4">
					<div class="well">
						<h4>Andrew
						</h4>
						Andrew (aka <b>"The Taskmaster"</b>). &nbsp;Without Andrew keeping everyone on task, we'd be lucky to have a home page! &nbsp;Andrew is in the market for a new companion, so says his ony real boss - his wife! &nbsp;If it's true your pet says a great deal about you and your personality, look for a constrictor or even rat coming soon! 
						<asp:image id="img_about__andrew" runat="server" imageurl="~/dir_image/about/andrew.jpg" cssclass="img-responsive dropshadow" />
					</div>
				</div>
			</div>
			<div class="row well">
				<h4>Testimonials</h4>
				“I had never thought to start a memory book for Fido, but since using PetFolio, I really wish I had started one sooner!” – Stephanie from San Diego, CA
				<hr />
				“I now look forward to receiving each new prompt about Blue. &nbsp;It’s been great to reminisce through the great memories I have of my dog.” – Jonas, Tampa, FL
				<hr />
				“Being able to store texts and pictures about Fifi in one place has been great. &nbsp;Before, I posted everything to Facebook and Instagram, but everything was lost amongst all other posts. &nbsp;Now, not only is everything in one place, I'm prompted to develop my story so I no longer have to remember.” – Ali, Reston, VA
			</div>
		</div>
	</section>
</asp:content>
