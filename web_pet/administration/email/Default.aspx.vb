Imports ns_enterprise
Imports ns_enterprise.cls_utility

Partial Class administration_email_Default
	Inherits System.Web.UI.Page

	Private Sub btn_verify_Click(sender As Object, e As EventArgs) Handles btn_verify.Click

		Dim int_count As Int32 = cls_data_access_layer.fnc_get_scaler__number("select count(*) from tbl_person_user where username = " & fnc_dbwrap(txt_email_address.Text))

		If int_count = 1 Then
		Else
			Response.Write("Error retrieving user by email")
			Exit Sub
		End If

		cls_data_access_layer.sub_execute_non_query("update tbl_person_user set email_validated = 1 where username = " & fnc_dbwrap(txt_email_address.Text))

		Response.Write("Email validated.")

	End Sub

End Class
