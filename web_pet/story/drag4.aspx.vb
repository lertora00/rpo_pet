Imports ns_enterprise
Imports ns_enterprise.cls_utility

Imports System.Data

Imports System.Web.Services

Partial Class story_drag4
	Inherits System.Web.UI.Page

	Private Sub BindData()

		'Dim dt As DataTable = cls_data_access_layer.fnc_get_datatable("select 1 as id, 'phil' as name, '14 blue' as address union select 2 as id, 'tom' as name, '15 slice' as address union select 3 as id, 'fred' as name, '222 lark' as address")
		Dim dt As DataTable = cls_data_access_layer.fnc_get_datatable("select top 5 pk_story, message from tbl_story")

		Repeater1.DataSource = dt
		Repeater1.DataBind()

	End Sub

	Private Sub story_drag4_Load(sender As Object, e As EventArgs) Handles Me.Load

		If IsPostBack = False Then BindData()

	End Sub

	<WebMethod>
	Public Shared Function UpdateMethod(personlist As List(Of Object)) As String
		'personlist is the list you post back by ajax, you can operate it ...
		Return personlist.Count.ToString()

	End Function

End Class
