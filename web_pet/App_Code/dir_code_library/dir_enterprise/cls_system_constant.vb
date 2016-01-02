Imports System.Data
Imports ns_enterprise

Namespace ns_enterprise
	Public Class cls_system_constant

		Public Shared Function fnc_select_list() As DataTable

			Dim str_sql As String = "select * from udf_system_constant()"

			Return cls_data_access_layer.fnc_get_datatable(str_sql)

		End Function

		Public Shared Function fnc_select_list(ByVal str__prm_filter As String) As DataTable

			If str__prm_filter.Length = 0 Then
				Return fnc_select_list()
				Exit Function
			End If

			Dim str_sql As String = "select * from udf_system_constant() where " & str__prm_filter

			Return cls_data_access_layer.fnc_get_datatable(str_sql)

		End Function

		Public Shared Sub sub_delete(ByVal str__prm_pk As String)

			'Dim inst_dynamic_sql As New cls_dynamic_sql
			'inst_dynamic_sql.sub_delete("pk_system_constant", str__prm_pk)

		End Sub

	End Class

End Namespace
