
Imports ns_enterprise
Imports ns_enterprise.cls_utility

Imports System.Data
Partial Class administration_user_default
	Inherits System.Web.UI.Page

	Private Sub dg_user_PageIndexChanged(source As Object, e As DataGridPageChangedEventArgs) Handles dg_user.PageIndexChanged

		dg_user.CurrentPageIndex = e.NewPageIndex

		' Rebind the data to refresh the DataGrid control. 
		dg_user.DataSource = cls_data_access_layer.fnc_get_dataset("select pk_person_user, username, phone_number_validated, email_validated from udf_person_user()")
		dg_user.DataBind()

	End Sub

	Private Sub form1_Load(sender As Object, e As EventArgs) Handles form1.Load

		If IsPostBack = False Then
			dg_user.DataSource = cls_data_access_layer.fnc_get_dataset("select pk_person_user, username, phone_number_validated, email_validated from udf_person_user()")
			dg_user.DataBind()
		End If

	End Sub

End Class
