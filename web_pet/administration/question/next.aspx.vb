Imports ns_enterprise
Imports ns_enterprise.cls_utility

Imports System.Data

Partial Class administration_question_next
	Inherits System.Web.UI.Page

	Private Sub btn_text_all_Click(sender As Object, e As EventArgs) Handles btn_text_all.Click

		Dim dt As DataTable = cls_data_access_layer.fnc_get_datatable("select * from udf_question_next()")

		If dt.Rows.Count = 0 Then
			lbl_error.Text = "Sorry, there are not users ready (active, pet, etc.)"
			Exit Sub
		End If

		For Each dr As DataRow In dt.Rows
			Dim str_sms_message As String = fnc_convert_expected_string(dr("question"))
			str_sms_message = Replace(str_sms_message, "[first_name]", fnc_convert_expected_string(dr("first_name")))
			str_sms_message = Replace(str_sms_message, "[pet_name]", fnc_convert_expected_string(dr("pet_name")))

			cls_sms.sub_send_message(fnc_convert_expected_string(dr("phone_number")), str_sms_message)

			Dim dt_additional_user As DataTable = cls_data_access_layer.fnc_get_datatable("select * from udf_anthology_additional_user() where fk_anthology = " & fnc_dbwrap(fnc_convert_expected_string(dr("pk_anthology"))))

			' if someone is setup with additional users on a pet, send them the same question...but don't log the question in the story.
			For Each dr_additional_user As DataRow In dt_additional_user.Rows
				Dim str_sms_message_au As String = fnc_convert_expected_string(dr("question"))
				str_sms_message_au = Replace(str_sms_message_au, "[first_name]", fnc_convert_expected_string(dr_additional_user("first_name")))
				str_sms_message_au = Replace(str_sms_message_au, "[pet_name]", fnc_convert_expected_string(dr_additional_user("pet_name")))

				cls_sms.sub_send_message(fnc_convert_expected_string(dr_additional_user("phone_number")), str_sms_message_au)
			Next

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
		Next

	End Sub

	Private Sub btn_text_specific_user_Click(sender As Object, e As EventArgs) Handles btn_text_specific_user.Click

		Dim str_phone_number As String = fnc_convert_expected_string(txt_phone_number.Text)

		If str_phone_number.Length = 0 Then
			lbl_error.Text = "Sorry, must specify a phone number"
			Exit Sub
		End If

		If str_phone_number.Length <> 10 Then
			lbl_error.Text = "Sorry, phone number must be 10 digits only"
			Exit Sub
		End If

		Dim dt As DataTable = cls_data_access_layer.fnc_get_datatable("select * from udf_question_next() where phone_number = " & fnc_dbwrap(str_phone_number))

		If dt.Rows.Count = 0 Then
			lbl_error.Text = "Sorry, that phone number isn't valid (active, pet, etc.)"
			Exit Sub
		End If

		For Each dr As DataRow In dt.Rows
			Dim str_sms_message As String = fnc_convert_expected_string(dr("question"))
			str_sms_message = Replace(str_sms_message, "[first_name]", fnc_convert_expected_string(dr("first_name")))
			str_sms_message = Replace(str_sms_message, "[pet_name]", fnc_convert_expected_string(dr("pet_name")))

			cls_sms.sub_send_message(fnc_convert_expected_string(dr("phone_number")), str_sms_message)

			Dim dt_additional_user As DataTable = cls_data_access_layer.fnc_get_datatable("select * from udf_anthology_additional_user() where fk_anthology = " & fnc_dbwrap(fnc_convert_expected_string(dr("pk_anthology"))))

			' if someone is setup with additional users on a pet, send them the same question...but don't log the question in the story.
			For Each dr_additional_user As DataRow In dt_additional_user.Rows
				Dim str_sms_message_au As String = fnc_convert_expected_string(dr("question"))
				str_sms_message_au = Replace(str_sms_message_au, "[first_name]", fnc_convert_expected_string(dr_additional_user("first_name")))
				str_sms_message_au = Replace(str_sms_message_au, "[pet_name]", fnc_convert_expected_string(dr_additional_user("pet_name")))

				cls_sms.sub_send_message(fnc_convert_expected_string(dr_additional_user("phone_number")), str_sms_message_au)
			Next

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
		Next

	End Sub

End Class
