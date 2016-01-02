
Imports ns_enterprise
Imports ns_enterprise.cls_utility

Partial Class dir_playground_log_sms
	Inherits System.Web.UI.Page

	Private Sub btn_log_sms_Click(sender As Object, e As EventArgs) Handles btn_log_sms.Click

		Dim str_sql As String = ""

		str_sql = "insert into tbl_log_sms (phone_number_from, phone_number_to, body) select " & fnc_dbwrap("A") & ", " & fnc_dbwrap("B") & ", " & fnc_dbwrap("C")
		cls_data_access_layer.sub_execute_non_query(str_sql)

		Exit Sub

		' simple send outgoing message.  TODO - add incoming_flag to log table.  For now, the from/to number is the give away.
		Dim inst_sql As New cls_sql
		inst_sql.str_table_name = "tbl_log_sms"
		inst_sql.str_operation = cls_sql.en_operation.insert.ToString
		inst_sql.sub_add_column("phone_number_from", "xx")
		inst_sql.sub_add_column("phone_number_to", "yy")
		inst_sql.sub_add_column("body", "zz")

		inst_sql.sub_execute()

		cls_log_sms.sub_log_message("x", "y", "z", 1)

	End Sub
End Class
