Imports ns_enterprise
Imports ns_enterprise.cls_utility
Imports System.Data

Partial Class dir_playground_service_sql
	Inherits System.Web.UI.Page

	Private Sub dir_playground_service_sql_Load(sender As Object, e As EventArgs) Handles Me.Load

		Dim dt_next_question As DataTable = cls_data_access_layer.fnc_get_datatable("select * from udf_question_next()")

		For Each dr As DataRow In dt_next_question.Rows

			Dim str_sms_message As String = fnc_convert_expected_string(dr("question"))
			str_sms_message = Replace(str_sms_message, "[first_name]", fnc_convert_expected_string(dr("first_name")))
			str_sms_message = Replace(str_sms_message, "[pet_name]", fnc_convert_expected_string(dr("pet_name")))

			'cls_sms.sub_send_message(fnc_convert_expected_string(dr("phone_number")), str_sms_message)
			' log question in story

			Dim str_sql As String = ""
			str_sql = "insert into tbl_story (fk_anthology, fk_question_pet, fk_person_user, incoming_flag, from_number, to_number, message) select "
			str_sql = str_sql & fnc_dbwrap(fnc_convert_expected_string(dr("pk_anthology"))) & ", "
			str_sql = str_sql & fnc_dbwrap(fnc_convert_expected_string(dr("pk_question_pet"))) & ", "
			str_sql = str_sql & fnc_dbwrap(fnc_convert_expected_string(dr("pk_person_user"))) & ", "
			str_sql = str_sql & "0, "
			str_sql = str_sql & fnc_dbwrap(cls_constant.str_system_constant("twilio_phone_number")) & ", "
			str_sql = str_sql & fnc_dbwrap(fnc_convert_expected_string(dr("phone_number"))) & ", "
			str_sql = str_sql & fnc_dbwrap(str_sms_message)

			cls_data_access_layer.sub_execute_non_query(str_sql)

			' below not working....argggg
			'Dim inst_sql As New cls_sql
			'inst_sql.str_table_name = "tbl_story"
			'inst_sql.str_operation = cls_sql.en_operation.insert.ToString
			'inst_sql.sub_add_column("fk_anthology", fnc_convert_expected_string(dr("pk_anthology")))
			'inst_sql.sub_add_column("fk_question_pet", fnc_convert_expected_string(dr("pk_question_pet")))
			'inst_sql.sub_add_column("incoming_flag", 0)
			'inst_sql.sub_add_column("from_number", cls_constant.str_system_constant("twilio_phone_number"))
			'inst_sql.sub_add_column("to_number", fnc_convert_expected_string(dr("pk_anthology")))
			'inst_sql.sub_add_column("message", fnc_convert_expected_string(dr("question")))
			'inst_sql.sub_execute()
		Next


	End Sub
End Class
