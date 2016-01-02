<%@ page language="VB" autoeventwireup="false" codefile="signup.aspx.vb" inherits="security_signup" masterpagefile="~/dir_public/Shared/MasterPages/Site.Master" title="Security | SignUp" %>

<asp:content id="headPage" contentplaceholderid="head" runat="server">
</asp:content>
<asp:content id="bodyPage" contentplaceholderid="ContentBody" runat="server">
	<form runat="server">
		<div class="container desc">
			<div class="col-lg-10">
				<div class="row ">
					<div class="well">
						<h4>Welcome
						</h4>
							PetFolio is a service designed to help develop a story about the relationship between you and your pet. To do so, we need a text-capable phone number to regularly send you questions via text. By responding to those questions, we help you document lasting memories.
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
				<asp:placeholder id="plc_error" runat="server" visible="false">
					<div class="row">
						<div class="col-lg-9">
							<div class="alert alert-danger">
								<asp:validationsummary id="vsum_error" runat="server" headertext="Oops..take a look, something went amiss." />
							</div>
						</div>
					</div>
				</asp:placeholder>
				<div class="row">
					<div class="col-sm-2">First Name: </div>
					<div class="col-sm-3">
						<asp:textbox id="txt_first_name" runat="server" cssclass="form-control"></asp:textbox>
						<asp:requiredfieldvalidator id="rval_first_name" controltovalidate="txt_first_name" runat="server" display="none" errormessage="First name is required.  Anything will do."></asp:requiredfieldvalidator>
					</div>
				</div>


				<div class="row">
					<div class="col-sm-2">Last Name: </div>
					<div class="col-sm-3">
						<asp:textbox cssclass="form-control" id="txt_last_name" runat="server"></asp:textbox>
						<asp:requiredfieldvalidator id="rval_last_name" runat="server" controltovalidate="txt_last_name" errormessage="Last name is required.  Be creative." display="None"></asp:requiredfieldvalidator>
					</div>
				</div>


				<div class="row">
					<div class="col-sm-2">Phone Number: </div>
					<div class="col-sm-4">
						<div class="tooltip-demo">
							<asp:textbox cssclass="form-control" id="txt_phone_number" runat="server" width="150px" data-toggle="tooltip" data-placement="right" title="Please enter a text capable, 10 digit (digits only) phone number."></asp:textbox>
						</div>
						<asp:requiredfieldvalidator id="rval_phone_number" runat="server" controltovalidate="txt_phone_number" errormessage="We need a phone number to send you text messages." display="None"></asp:requiredfieldvalidator>
						<asp:customvalidator id="cval_phone_number" runat="server" errormessage="Hmm...this number is already a member?  Trying to develop stories on multiple pets?  If so, email us at support@petnarrative.com." controltovalidate="txt_phone_number" display="none"></asp:customvalidator>
						<asp:customvalidator id="cval_phone_number__format" runat="server" errormessage="Please enter only digits...and 10 of them. &nbsp;&nbsp; :)" controltovalidate="txt_phone_number" display="none"></asp:customvalidator>

					</div>
				</div>

				<div class="row">
					<div class="col-sm-2">Pet Name: </div>
					<div class="col-sm-3">
						<asp:textbox cssclass="form-control" id="txt_pet_name" runat="server"></asp:textbox>
						<asp:requiredfieldvalidator id="rval_pet_name" runat="server" controltovalidate="txt_pet_name" errormessage="Pet name is required." display="None"></asp:requiredfieldvalidator>
					</div>
				</div>

				<div class="row">
					User/account information:	
				</div>

				<div class="row">
					<div class="col-sm-2">Email: </div>
					<div class="col-sm-4">
						<div class="tooltip-demo">

							<asp:textbox cssclass="form-control" id="txt_email_address" runat="server" width="300px" data-toggle="tooltip" data-placement="right" title="Note: This is also your username."></asp:textbox>
						</div>
						<asp:requiredfieldvalidator id="rval_email_address" runat="server" controltovalidate="txt_email_address" errormessage="Please provide an email so we can keep you informed." display="None"></asp:requiredfieldvalidator>
						<asp:customvalidator id="cval_email_address__member" runat="server" errormessage="Hmm...this email is already a member?  Trying to develop stories on multiple pets?  If so, email us at support@petnarrative.com." controltovalidate="txt_email_address" display="none"></asp:customvalidator>
						<asp:customvalidator id="cval_email_address" runat="server" errormessage="Sorry...your email address doesn't appear to be valid.  Try again?" controltovalidate="txt_email_address" display="none"></asp:customvalidator>
					</div>
				</div>

				<div class="row">
					<div class="col-sm-2">Password: </div>
					<div class="col-sm-3">
						<asp:textbox cssclass="form-control" id="txt_password" textmode="Password" runat="server"></asp:textbox>
						<asp:requiredfieldvalidator id="rval_password" runat="server" errormessage="You gotta have a password!" controltovalidate="txt_password" display="None"></asp:requiredfieldvalidator>
					</div>
				</div>

				<div class="row">
					<div class="col-sm-2">Confirm password: </div>
					<div class="col-sm-3">
						<asp:textbox cssclass="form-control" id="txt_password_confirm" textmode="Password" runat="server"></asp:textbox>
						<asp:customvalidator id="cval_password_confirm" runat="server" errormessage="Your password and confirmation password need to match." controltovalidate="txt_password_confirm" display="none"></asp:customvalidator>
					</div>
				</div>

				<div class="row">
					<div class="col-sm-2">&nbsp;</div>
					<div class="col-sm-3">
						<br />
						<asp:button id="btn_signup" runat="server" text="Sign Up!" cssclass="btn btn-primary" causesvalidation="false" />
					</div>
				</div>

				<div class="row">
					<div class="col-sm-2">&nbsp;</div>
					<div class="col-sm-8">
						<br />
						Already a member?  Sign on <a href="login.aspx">here</a>
					</div>
				</div>

				<br />
				<br />
			</div>
		</div>
	</form>
</asp:content>

