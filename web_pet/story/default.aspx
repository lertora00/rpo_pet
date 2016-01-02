<%@ page language="VB" autoeventwireup="false" codefile="default.aspx.vb" inherits="story_default2" masterpagefile="~/dir_member/Shared/MasterPages/SiteLayout__member.Master" title="Pet | Story" %>

<asp:content id="ctnBodyPage" contentplaceholderid="ctnBody" runat="server">

	<script type="text/javascript">
		function hideById(ctl) {
			var element = document.getElementById(ctl);
			if (element) {
				//				element.style.display = 'none';
				//				element.style.visibility = 'hidden';
				element.classList.remove('color3');
			}
		}
		function showById2(ctl) {
			var element = document.getElementById(ctl);
			if (element) {
				element.style.display = '';
				element.style.visibility = 'visible';

			}
		}
		function showById(ctl) {
			var element = document.getElementById(ctl);
			if (element) {
				//				element.style.display = '';
				//				element.style.visibility = 'visible';
				element.classList.add('color3');
				element.classList.add('hidingDelete');
			}
		}
		function setValue(ctl, theValue) {
			alert(theValue);
			document.getElementById(ctl).value = theValue;
		}
	</script>

	<script>
		function grow(textField) {
			if (textField.clientHeight < textField.scrollHeight) {
				textField.style.height = textField.scrollHeight + "px";
				if (textField.clientHeight < textField.scrollHeight) {
					textField.style.height =
						(textField.scrollHeight * 2 - textField.clientHeight) + "px";
				}
			}
		}
	</script>

	<asp:label id="lbl_show_row_count" runat="server" visible="false">8</asp:label>
	<div>
		<div class="row">
			<div class="col-lg-2">
				<div class="panel-body">
					<h4>
						<strong>
							<asp:label id="lbl_pet_name__navigation" runat="server">Teddy</asp:label></strong></h4>
					<p>
						<small>
							<asp:label id="lbl_pet_tagline" runat="server">Who's a bad dog?</asp:label></small>
					</p>
					<asp:label id="lbl_pk_anthology" runat="server" visible="false"></asp:label><asp:linkbutton id="lbtn_change_picture" runat="server">
						<div class="tooltip-demo">
							<asp:image id="img_anthology" imageurl="~/dir_image/pet_stick_figure_with_link.jpg" runat="server" cssclass="img-responsive" data-toggle="tooltip" data-placement="right" title="Click the picture to add your own photo!" /></div>
					</asp:linkbutton>&nbsp;&nbsp;
				</div>
			</div>
			<div class="col-lg-10">
				<div class="panel-body">
					<div align="right">
						<asp:textbox id="txt_post" style="display: none;" runat="server" width="60%" cssclass="form-control-inline"></asp:textbox>
						<asp:button id="btn_save" style="display: none;" runat="server" cssclass="btn btn-primary" text="Save" />
						<a href="#" onclick="showById2('<%= txt_post.ClientID %>');showById2('<%= btn_save.ClientID %>');" class="btn btn-primary">Post A Message</a>
					</div>
					<div class="table-responsive">
						<table width="100%" id="story">
							<tbody>
								<asp:repeater id="rpt_list_00" runat="server">
									<itemtemplate>
										<tr>
											<td>
												<div id="div_row" runat="server">
													<asp:placeholder id="plc_question" runat="server">
														<asp:linkbutton id="lbtn_delete" runat="server" oncommand="sub_delete" causesvalidation="false" cssclass="fa fa-times color0 inline">&nbsp;</asp:linkbutton><asp:label id="lbl_pk_story" runat="server" visible="false" text='<%# DataBinder.Eval(Container.DataItem, "pk_story") %>'></asp:label>
														<asp:label id="lbl_question" cssclass="question" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "message") %>'></asp:label>
														<asp:linkbutton id="Linkbutton1" runat="server" visible="false" oncommand="sub_plus" causesvalidation="false"><i class="fa fa-plus color3"></i></asp:linkbutton>
													</asp:placeholder>
													<asp:linkbutton id="lbtn_plus" runat="server" visible="false" oncommand="sub_plus" causesvalidation="false"><i class="fa fa-plus color3"></i></asp:linkbutton>
													<asp:placeholder id="plc_post" runat="server">
														<div class="indent-block">&nbsp;</div>
														<div class="indent-inline"></div>
														<asp:linkbutton id="lbtn_delete__post" runat="server" oncommand="sub_delete" causesvalidation="false" cssclass="fa fa-times color0 inline">&nbsp;</asp:linkbutton><asp:label id="Label1" runat="server" visible="false" text='<%# DataBinder.Eval(Container.DataItem, "pk_story") %>'></asp:label>
														<div class="post">
															<asp:label id="lbl_post" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "message") %>'></asp:label>
															<div class="small text-primary usertime indent-inline">
																<i>-<asp:label id="lbl_first_name__post" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "person__first_name") %>'></asp:label>,
															<asp:label id="Label2" cssclass="small" runat="server" text='<%# ns_enterprise.cls_utility.fnc_format_date__short_date(DataBinder.Eval(Container.DataItem, "insert_date")) %>'></asp:label></i>
															</div>
														</div>
													</asp:placeholder>
													<asp:linkbutton visible="false" id="lbtn_save" runat="server" text="save" oncommand="sub_save" commandargument='<%# DataBinder.Eval(Container.DataItem, "pk_story") %>'></asp:linkbutton>
													<div id="div_answer" contenteditable="true" onblur="alert(this.innerHTML);" runat="server"><%# DataBinder.Eval(Container.DataItem, "message") %></div>
													<asp:label id="lbl_answer" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "message") %>' cssclass="answer"></asp:label>
													<asp:textbox id="txt_answer" onmouseover="grow(this);" textmode="MultiLine" rows="1" cssclass="textbox-label" runat="server" ontextchanged="sub_text_changed" autopostback="true" text='<%# DataBinder.Eval(Container.DataItem, "message") %>'></asp:textbox>
													<asp:image id="img_answer" runat="server" imageurl='<%# DataBinder.Eval(Container.DataItem, "media_url") %>' cssclass="img-responsive dropshadow" />
													<asp:placeholder id="plc_user_time" runat="server" visible="false">
														<div class="small text-primary usertime">
															<i>-<asp:label id="lbl_first_name" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "person__first_name") %>'></asp:label>,
															<asp:label id="lbl_insert_date" cssclass="small" runat="server" text='<%# ns_enterprise.cls_utility.fnc_format_date__short_date(DataBinder.Eval(Container.DataItem, "insert_date")) %>'></asp:label></i>
														</div>
													</asp:placeholder>

													<asp:label id="lbl_fk_story" runat="server" visible="false" text='<%# DataBinder.Eval(Container.DataItem, "fk_story") %>'></asp:label>
													<asp:label id="lbl_fk_anthology" runat="server" visible="false" text='<%# DataBinder.Eval(Container.DataItem, "fk_anthology") %>'></asp:label>
													<asp:label id="lbl_question_type" runat="server" visible="false" text='<%# DataBinder.Eval(Container.DataItem, "message_type") %>'></asp:label>
												</div>
											</td>
										</tr>
									</itemtemplate>
								</asp:repeater>
							</tbody>
						</table>
					</div>
					<div class="topmargin"></div>
					<asp:button id="btn_load_more" runat="server" cssclass="btn btn-primary" text="Load More Messages" />
					&nbsp;&nbsp;Show messages:
					<asp:radiobutton id="rdo_all" cssclass="form-input" runat="server" checked="true" autopostback="true" causesvalidation="false" groupname="answer" />
					All &nbsp;<asp:radiobutton id="rdo_with" runat="server" autopostback="true" causesvalidation="false" groupname="answer" />
					w/Answers &nbsp;
					<asp:radiobutton id="rdo_wout" runat="server" autopostback="true" causesvalidation="false" groupname="answer" />
					wout/Answers &nbsp;
				</div>
			</div>
		</div>
	</div>

</asp:content>
<asp:content id="ctnJavaScript" contentplaceholderid="ctnJavaScriptPage" runat="server">
	<script>
		$('.tooltip-demo').tooltip({
			selector: "[data-toggle=tooltip]",
			container: "body"
		});
		$("[data-toggle=popover]").popover();
	</script>
</asp:content>







