<%@ page language="VB" autoeventwireup="false" codefile="timeline.aspx.vb" inherits="story_timeline" masterpagefile="~/dir_member/Shared/MasterPages/SiteLayout__member.Master" title="Pet | Story" %>



<asp:content id="ctnBodyPage" contentplaceholderid="ctnBody" runat="server">

	<head>
		<meta charset="UTF-8">
		<meta name="viewport" content="width=device-width, initial-scale=1">

		<link rel="stylesheet" href="css/reset.css">
		<!-- CSS reset -->
		<link rel="stylesheet" href="css/style.css">
		<!-- Resource style -->
		<script src="js/modernizr.js"></script>
		<!-- Modernizr -->

		<title>Responsive Vertical Timeline</title>
	</head>

	<body>
		<asp:label id="lbl_pk_anthology" runat="server" visible="false"></asp:label>
		<asp:label id="lbl_pet_name" runat="server" visible="false"></asp:label>
		<asp:textbox id="txt_tagline" runat="server" visible="false"></asp:textbox>

		<div class="row">
			<div class="col-lg-2">
				<div class="panel-body pet-profile">
					<asp:label id="lbl_pet_name__navigation" runat="server" cssclass="pet-profile-name">Pet Name</asp:label>
					<asp:image id="img_anthology" imageurl="~/dir_image/pet_stick_figure_with_link.jpg" runat="server" cssclass="img-responsive dropshadow pet-profile-image" data-toggle="tooltip" data-placement="right" />
				</div>
			</div>
			<div class="col-lg-10 zone-body">
				<section id="cd-timeline" class="cd-container">
					<asp:repeater id="rpt_timeline" runat="server">
						<itemtemplate>
							<div class="cd-timeline-block">
								<div class="cd-timeline-img">
									<img src="../dir_image/logo-only.gif" class="img-responsive cd-timeline-img" alt="Post">
								</div>
								<!-- cd-timeline-img -->

								<div class="cd-timeline-content">
									<h2>
										<asp:label id="lbl_question" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "message") %>'></asp:label></h2>
									<br />
									<asp:label id="lbl_answer" runat="server"></asp:label>
									<asp:placeholder id="plc_user_time" runat="server" visible="true">
										<div class="usertime2 inline">
											<i>-<asp:label id="lbl_first_name" runat="server"></asp:label>,
															<asp:label id="lbl_insert_date" runat="server"></asp:label></i>
										</div>
									</asp:placeholder>

									<asp:placeholder id="plc_image" runat="server">
										<br />
										<br />
										<asp:image id="img_answer" runat="server" cssclass="img-responsive dropshadow" />
									</asp:placeholder>

									<span class="cd-date">
										<asp:label id="lbl_sort_date" runat="server" text='<%# ns_enterprise.cls_utility.fnc_format_date__short_date(DataBinder.Eval(Container.DataItem, "sort_date")) %>'></asp:label></span>
								</div>
								<!-- cd-timeline-content -->
							</div>
						</itemtemplate>
					</asp:repeater>
				</section>
			</div>
		</div>
		<!-- cd-timeline -->
		<script src="http://ajax.googleapis.com/ajax/libs/jquery/1.11.0/jquery.min.js"></script>
		<script src="js/main.js"></script>
		<!-- Resource jQuery -->
	</body>
	</html>
</asp:content>


