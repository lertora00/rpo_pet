Imports System.Data

Namespace ns_enterprise

	Public Class cls_log_sql

		Public Shared Function fnc_select_list() As DataTable

			Dim str_sql As String = "select * from udf_log_sql()"

			Return cls_data_access_layer.fnc_get_datatable(str_sql)

		End Function

		Public Shared Function fnc_select_list(ByVal str__prm_filter As String) As DataTable

			If str__prm_filter.Length = 0 Then
				Return fnc_select_list()
				Exit Function
			End If

			Dim str_sql As String = "select * from udf_log_sql() where " & str__prm_filter

			Return cls_data_access_layer.fnc_get_datatable(str_sql)

		End Function

		Public Shared Sub sub_delete(ByVal str__prm_pk As String)

			'Dim inst_dynamic_sql As New cls_dynamic_sql
			'inst_dynamic_sql.sub_delete("pk_log_sql", str__prm_pk)

		End Sub

		Public Shared Sub sub_log_execute(ByVal str__prm_class_name As String, ByVal str__prm_method_name As String, ByVal str__prm_sql As String)

			Exit Sub

			Dim inst_sql As New cls_sql()

			inst_sql.str_operation = cls_sql.en_operation.insert.ToString
			inst_sql.str_table_name = "tbl_log_sql"

			inst_sql.sub_add_column("class_name", str__prm_class_name)
			inst_sql.sub_add_column("method_name", str__prm_method_name)
			inst_sql.sub_add_column("sql", str__prm_sql)
			inst_sql.sub_add_column("aspnet_sessionid", cls_session.str_aspnet_sessionid)
			inst_sql.sub_add_column("fk_log_session", cls_session.str_pk_log_session)
			inst_sql.sub_add_column("fk_person_user__insert", cls_current_user.str_pk_person_user)
			' cannot log onscreen name as it may not be set.  if not yet set, it will begin an endless loop of looking it up and logging the sql to look it up, etc.
			'inst_sql.sub_add_column("insert_user", cls_current_user.str_onscreen_name)

			cls_asynchronous.sub_execute_non_query(inst_sql.fnc_get_sql(inst_sql))

			inst_sql = Nothing

		End Sub

	End Class

End Namespace
