<%@ page language="VB" autoeventwireup="false" codefile="tables.aspx.vb" inherits="story_tables" masterpagefile="~/dir_member/Shared/MasterPages/SiteLayout__member.Master" %>

<asp:content id="ctnBodyPage" contentplaceholderid="ctnBody" runat="server">
	<div id="page-wrapper">
		<div class="row">
			<div class="col-lg-12">
				<h1 class="page-header">
					<asp:label id="lbl_pk_anthology" runat="server" visible="false"></asp:label><img src="../dir_image/teddy.png" width="60px" /><asp:label id="lbl_pet_name__navigation" runat="server">Teddy</asp:label>&nbsp;<i><asp:label font-size="16px" id="lbl_pet_tagline" runat="server">Who's a bad dog?</asp:label></i>
				</h1>
			</div>
		</div>
		<div class="row">
			<div class="col-lg-12">
				<div class="panel panel-default content-panel">
					<div class="panel-heading">
						<asp:label id="lbl_pet_name" runat="server">Teddy</asp:label>'s Log
					</div>
					<div class="panel-body">
						<div class="table-responsive">
							<table class="table table-striped table-bordered table-hover" id="dataTables-example">
								<thead>
									<tr>
										<th>Message</th>
										<th>Date</th>
									</tr>
								</thead>
								<tbody>

									<asp:repeater id="rpt_list_00" runat="server">
										<itemtemplate>
											<tr class="odd gradeX">
												<td><%# DataBinder.Eval(Container.DataItem, "message") %>
												</td>
												<td width="16%"><%# ns_enterprise.cls_utility.fnc_format_date__short_date_and_time(DataBinder.Eval(Container.DataItem, "insert_date")) %></td>
											</tr>
										</itemtemplate>
									</asp:repeater>
								</tbody>
							</table>
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>
</asp:content>
<asp:content id="Content1" contentplaceholderid="ctnStylePage" runat="server">
	<link href="../dir_member/Content/StyleSheets/Plugins/dataTables/dataTables.bootstrap.css" rel="stylesheet">
</asp:content>
<asp:content id="Content2" contentplaceholderid="ctnJavaScriptPage" runat="server">
	<script src="../dir_member/Content/Scripts/plugins/dataTables/jquery.dataTables.js"></script>
	<script src="../dir_member/Content/Scripts/plugins/dataTables/dataTables.bootstrap.js"></script>
	<script>
		$(document).ready(function () {
			$('#dataTables-example').dataTable();
		});
	</script>
</asp:content>
