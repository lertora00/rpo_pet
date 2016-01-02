Imports System.Data

Namespace ns_enterprise

	Public Class cls_log_message

		Public Shared Function fnc_select_list() As DataTable

			Dim str_sql As String = "select * from udf_log_message()"

			Return cls_data_access_layer.fnc_get_datatable(str_sql)

		End Function

		Public Shared Function fnc_select_list(ByVal str__prm_filter As String) As DataTable

			If str__prm_filter.Length = 0 Then
				Return fnc_select_list()
				Exit Function
			End If

			Dim str_sql As String = "select * from udf_log_message() where " & str__prm_filter

			Return cls_data_access_layer.fnc_get_datatable(str_sql)

		End Function

		Public Shared Sub sub_delete(ByVal str__prm_pk As String)

			'Dim inst_dynamic_sql As New cls_dynamic_sql
			'inst_dynamic_sql.sub_delete("pk_log_message_header", str__prm_pk)

		End Sub

	End Class

End Namespace
