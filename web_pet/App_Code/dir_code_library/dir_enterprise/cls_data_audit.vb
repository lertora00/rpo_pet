Imports ns_enterprise

Namespace ns_enterprise

	Public Class cls_data_audit

		Public Shared Function fnc_create_header(ByVal str__prm_pk_log_system_audit_header As String) As cls_sql

			Dim inst_sql As New cls_sql()
			inst_sql.str_operation = cls_sql.en_operation.insert.ToString
			inst_sql.str_table_name = "tbl_log_system_audit_header"
			inst_sql.sub_add_column("pk_log_system_audit_header", str__prm_pk_log_system_audit_header)
			' TODO - create new column in audit to store session identifier (may want aspnet.session id and pk type too)
			'inst_sql.sub_add_column("fk_log_session", cls_session.str_pk_log_session)
			'inst_sql.sub_add_column("aspnet_sessionid", cls_session.str_aspnet_sessionid)
			If cls_variable.str_context_value("str_ctl_sender_id").Length > 0 Then
				inst_sql.sub_add_column("sender_id", cls_variable.str_context_value("str_ctl_sender_id"))
			End If
			inst_sql.sub_add_column(cls_constant.str_insert_user_column_name_pk, cls_utility.fnc_dbwrap(cls_current_user.str_pk_person_user))
			inst_sql.sub_add_column(cls_constant.str_insert_user_column_name, cls_utility.fnc_dbwrap(cls_current_user.str_onscreen_name))

			Return inst_sql

		End Function

	End Class

End Namespace
