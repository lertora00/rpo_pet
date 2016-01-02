Imports ns_enterprise
Imports ns_enterprise.cls_utility

Imports System.Data

Partial Class administration_user_delete
	Inherits System.Web.UI.Page

	Private Sub btn_delete_Click(sender As Object, e As EventArgs) Handles btn_delete.Click

		Dim dt As DataTable = cls_data_access_layer.fnc_get_datatable("select * from udf_person_user() where username = " & fnc_dbwrap(txt_username.Text))

		If dt.Rows.Count = 0 Then
			Response.Write("Username not found.")
			Exit Sub
		End If

		Dim str_username As String = fnc_convert_expected_string(dt.Rows(0)("username"))
		lbl_first_name.Text = fnc_convert_expected_string(dt.Rows(0)("person__first_name"))
		lbl_pk_person.Text = fnc_convert_expected_string(dt.Rows(0)("fk_person"))

		plc_user.Visible = True

	End Sub

	Private Sub btn_delete_for_real_Click(sender As Object, e As EventArgs) Handles btn_delete_for_real.Click

		If txt_delete_password.Text <> "x" Then
			Response.Write("Invalid password")
			Exit Sub
		End If

		cls_data_access_layer.sub_execute_non_query("delete from tbl_person where pk_person = " & fnc_dbwrap(lbl_pk_person.Text))
		Response.Clear()
		Response.Write("User, Person, etc. deleted")
		Response.End()

	End Sub

End Class
