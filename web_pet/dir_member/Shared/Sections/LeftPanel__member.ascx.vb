Imports ns_enterprise
Imports ns_enterprise.cls_utility
Imports System.Data

Partial Class dir_member_Shared_Sections_LeftPanel__member
	Inherits System.Web.UI.UserControl

	Private Sub dir_member_Shared_Sections_LeftPanel__member_Load(sender As Object, e As EventArgs) Handles Me.Load

		If IsPostBack = False Then
			Dim dt As DataTable = cls_data_access_layer.fnc_get_datatable("select pk_anthology, name, tagline from tbl_anthology where active_flag = 1 and fk_person_user = " & fnc_dbwrap(cls_current_user.str_pk_person_user) & " and active_flag = 1 order by default_flag desc, sort_order")
			rpt_pet.DataSource = dt
			rpt_pet.DataBind()
		End If

	End Sub

End Class
