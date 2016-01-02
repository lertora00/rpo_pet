Imports ns_enterprise
Imports ns_enterprise.cls_utility

Imports System.Data

Partial Class story_tables
	Inherits System.Web.UI.Page

	Private Sub story_tables_Load(sender As Object, e As EventArgs) Handles Me.Load

		Dim dr_anthology As DataRow

		If IsPostBack = False Then
			dr_anthology = cls_data_access_layer.fnc_get_datarow("select top 1 * from tbl_anthology where fk_person_user = " & fnc_dbwrap(cls_current_user.str_pk_person_user) & " order by default_flag desc, sort_order")
			lbl_pk_anthology.Text = fnc_convert_expected_string(dr_anthology("pk_anthology"))
			lbl_pet_name.Text = fnc_convert_expected_string(dr_anthology("name"))
			lbl_pet_name__navigation.Text = fnc_convert_expected_string(dr_anthology("name"))
			lbl_pet_tagline.Text = fnc_convert_expected_string(dr_anthology("tagline"))
		End If


		Dim dt As DataTable = cls_data_access_layer.fnc_get_datatable("select * from tbl_story where fk_anthology = " & fnc_dbwrap(lbl_pk_anthology.Text))
		rpt_list_00.DataSource = dt
		rpt_list_00.DataBind()

	End Sub

End Class
