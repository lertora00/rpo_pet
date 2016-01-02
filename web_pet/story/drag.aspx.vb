Imports ns_enterprise
Imports ns_enterprise.cls_utility

Imports System.Data

Partial Class story_drag
	Inherits System.Web.UI.Page

	Private Sub story_drag_Load(sender As Object, e As EventArgs) Handles Me.Load

		If IsPostBack = False Then
			Dim dt As DataTable = cls_data_access_layer.fnc_get_datatable("select top 2 * from tbl_person")
			rpt_test.DataSource = dt
			rpt_test.DataBind()
		End If

	End Sub
End Class
