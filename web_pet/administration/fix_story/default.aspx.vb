Imports ns_enterprise
Imports ns_enterprise.cls_utility

Imports System.Data

Partial Class administration_fix_story_default
	Inherits System.Web.UI.Page

	Private Sub btn_fix_story_Click(sender As Object, e As EventArgs) Handles btn_fix_story.Click

		If txt_password.Text <> "lskdjf" Then
			Response.Write("Sorry, invalid password (usual).")
			Exit Sub
		End If

		Dim dt_story As DataTable = cls_data_access_layer.fnc_get_datatable("select * from tbl_story where fk_anthology is not null")

		For Each dr_story As DataRow In dt_story.Rows
			Dim str_pk_person_user As String = cls_data_access_layer.fnc_get_scaler__string("select top 1 fk_person_user from tbl_anthology where pk_anthology = " & fnc_dbwrap(dr_story("fk_anthology")))
			cls_data_access_layer.sub_execute_non_query("update tbl_story set fk_person_user = " & fnc_dbwrap(str_pk_person_user) & " where pk_story = " & fnc_dbwrap(dr_story("pk_story")))
		Next

		Response.Write("Done")

	End Sub
End Class
