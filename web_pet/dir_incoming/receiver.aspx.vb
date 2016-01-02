Imports ns_enterprise
Imports ns_enterprise.cls_utility
Imports System.Data

Partial Class dir_incoming_receiver
	Inherits System.Web.UI.Page

	Private Sub dir_incoming_receiver_Load(sender As Object, e As EventArgs) Handles Me.Load

		Try

			If Request Is Nothing = True Then Exit Sub

			' log all incoming SMS texts
			' TODO log MMS messages and texts with pictures

			Dim str_from As String = fnc_convert_expected_string(Request("from"))
			Dim str_to As String = fnc_convert_expected_string(Request("to"))
			Dim str_body As String = fnc_convert_expected_string(Request("body"))
			Dim int_nummedia As Int32 = fnc_convert_expected_int32(Request("nummedia"))
			Dim str_fromcity As String = fnc_convert_expected_string(Request("fromcity"))
			Dim str_fromstate As String = fnc_convert_expected_string(Request("fromstate"))
			Dim str_fromzip As String = fnc_convert_expected_string(Request("fromzip"))
			Dim str_fromcountry As String = fnc_convert_expected_string(Request("fromcountry"))
			Dim str_media_content_type_0 As String = fnc_convert_expected_string(Request("mediacontenttype0"))
			Dim str_media_url_0 As String = fnc_convert_expected_string(Request("mediaurl0"))
			Dim str_media_content_type_1 As String = fnc_convert_expected_string(Request("mediacontenttype1"))
			Dim str_media_url_1 As String = fnc_convert_expected_string(Request("mediaurl1"))
			Dim str_media_content_type_2 As String = fnc_convert_expected_string(Request("mediacontenttype2"))
			Dim str_media_url_2 As String = fnc_convert_expected_string(Request("mediaurl2"))
			Dim str_media_content_type_3 As String = fnc_convert_expected_string(Request("mediacontenttype3"))
			Dim str_media_url_3 As String = fnc_convert_expected_string(Request("mediaurl3"))
			Dim str_media_content_type_4 As String = fnc_convert_expected_string(Request("mediacontenttype4"))
			Dim str_media_url_4 As String = fnc_convert_expected_string(Request("mediaurl4"))
			Dim str_media_content_type_5 As String = fnc_convert_expected_string(Request("mediacontenttype5"))
			Dim str_media_url_5 As String = fnc_convert_expected_string(Request("mediaurl5"))
			Dim str_media_content_type_6 As String = fnc_convert_expected_string(Request("mediacontenttype6"))
			Dim str_media_url_6 As String = fnc_convert_expected_string(Request("mediaurl6"))
			Dim str_media_content_type_7 As String = fnc_convert_expected_string(Request("mediacontenttype7"))
			Dim str_media_url_7 As String = fnc_convert_expected_string(Request("mediaurl7"))
			Dim str_media_content_type_8 As String = fnc_convert_expected_string(Request("mediacontenttype8"))
			Dim str_media_url_8 As String = fnc_convert_expected_string(Request("mediaurl8"))
			Dim str_media_content_type_9 As String = fnc_convert_expected_string(Request("mediacontenttype9"))
			Dim str_media_url_9 As String = fnc_convert_expected_string(Request("mediaurl9"))

			Dim str_phone_number As String = Replace(str_from, "+1", "")
			Dim str_pk_person_user As String = cls_data_access_layer.fnc_get_scaler__string("select top 1 pk_person_user from tbl_person_phone, tbl_person, tbl_person_user where phone_number = " & cls_utility.fnc_dbwrap(str_phone_number) & " and tbl_person_phone.fk_person = pk_person and pk_person = tbl_person_user.fk_person and tbl_person_phone.active_flag = 1")

			Dim int_active_phone_number As Int32 = IIf(fnc_is_valid_guid(str_pk_person_user) = True, 1, 0)

			Dim inst_log_sms As New cls_log_sms
			inst_log_sms.str_phone_number_from = str_from
			inst_log_sms.str_phone_number_to = str_to
			inst_log_sms.str_message = str_body
			inst_log_sms.int_nummedia = int_nummedia
			inst_log_sms.str_from_city = str_fromcity
			inst_log_sms.str_from_state = str_fromstate
			inst_log_sms.str_from_zip = str_fromzip
			inst_log_sms.str_from_country = str_fromcountry
			inst_log_sms.int_active_phone_number_flag = int_active_phone_number

			If int_nummedia > 0 Then
				inst_log_sms.str_media_content_type_0 = str_media_content_type_0
				inst_log_sms.str_media_url_0 = str_media_url_0
			End If

			inst_log_sms.sub_log_message()

			' not an active or existing customer
			If int_active_phone_number = 0 Then Exit Sub

			Dim dr_user_profile As DataRow = cls_data_access_layer.fnc_get_datarow("select top 1 * from udf_select_pet(" & fnc_dbwrap(str_pk_person_user) & ")")

			If dr_user_profile Is Nothing Then
				sub_no_pet_found(str_pk_person_user, str_phone_number)
				Exit Sub
			End If

			Dim str_pk_anthology As String = fnc_convert_expected_string(dr_user_profile("pk_anthology"))
			Dim str_first_name As String = fnc_convert_expected_string(dr_user_profile("first_name"))
			Dim str_pet_name As String = fnc_convert_expected_string(dr_user_profile("pet_name"))
			Dim int_primary_user As Int32 = fnc_convert_expected_int32(dr_user_profile("primary_user_flag"))

			Dim inst_story As New cls_story

			Dim int_verified_phone_number As Int32 = fnc_convert_expected_int32(dr_user_profile("phone_number_validated"))

			' if unvalidated number and they text YES, validate number so they can access their account.
			If int_verified_phone_number = 0 And str_body.ToLower = "yes" Then
				cls_data_access_layer.sub_execute_non_query("update tbl_person_user set phone_number_validated = 1 where pk_person_user = " & fnc_dbwrap(str_pk_person_user))

				' todo - check first to ensure email is verified.  If yes, do below.  If not, do nothing and email verify will check phone and send account ready message

				' let them know they are good to go; and ask first question; inform scheduled messages to follow
				Dim str_sms_message As String = fnc_convert_expected_string(cls_constant.str_system_constant("text__account_ready"))
				str_sms_message = Replace(str_sms_message, "[first_name]", str_first_name)
				str_sms_message = Replace(str_sms_message, "[pet_name]", str_pet_name)

				cls_sms.sub_send_message(str_phone_number, str_sms_message)

				System.Threading.Thread.Sleep(5000)

				' ask first question
				Dim dr_question As DataRow = cls_question_pet.fnc_get_question__initial()

				str_sms_message = fnc_convert_expected_string(dr_question("question"))
				str_sms_message = Replace(str_sms_message, "[first_name]", str_first_name)
				str_sms_message = Replace(str_sms_message, "[pet_name]", str_pet_name)
				cls_sms.sub_send_message(str_phone_number, str_sms_message)

				' log first question in story

				inst_story = New cls_story
				inst_story.str_fk_anthology = fnc_convert_expected_string(dr_user_profile("pk_anthology"))
				inst_story.str_fk_question_pet = fnc_convert_expected_string(dr_question("pk_question_pet"))
				inst_story.str_fk_person_user = str_pk_person_user
				inst_story.str_from_number = cls_constant.str_system_constant("twilio_phone_number")
				inst_story.str_to_number = str_phone_number
				inst_story.str_message = str_sms_message
				inst_story.str_bln_incoming = False
				inst_story.str_fk_person_user__insert = str_pk_person_user
				inst_story.sub_insert()

				Exit Sub
			End If

			' associate incoming text with a question from the pets story - the most recent question
			Dim str_pk_story__answer As String = cls_data_access_layer.fnc_get_scaler__string("select dbo.udf_get_question_for_answer(" & fnc_dbwrap(str_pk_anthology) & ")")
			If fnc_is_valid_guid(str_pk_story__answer) Then
				Dim int_count_answer As Int32 = cls_data_access_layer.fnc_get_scaler__number("select count(*) from tbl_story where fk_story = " & fnc_dbwrap(str_pk_story__answer) & " and active_flag = 1")
				If int_count_answer > 0 Then str_pk_story__answer = ""
			End If

			inst_story = New cls_story
			inst_story.str_fk_anthology = str_pk_anthology
			If fnc_is_valid_guid(str_pk_story__answer) Then
				inst_story.str_fk_story = str_pk_story__answer
			End If
			inst_story.str_fk_person_user = str_pk_person_user
			inst_story.str_from_number = str_from
			inst_story.str_to_number = str_to
			inst_story.str_message = str_body
			inst_story.str_bln_incoming = True
			inst_story.str_fk_person_user__insert = str_pk_person_user
			If int_nummedia > 0 Then
				Dim str_image_filename As String = ""
				Dim str_image_path__physical As String = HttpContext.Current.Request.PhysicalApplicationPath & "dir_image\dir_anthology\" & str_pk_anthology & "\dir_story\" & inst_story.str_pk_story & "\"
				Select Case str_media_content_type_0
					Case "image/jpeg", "image/jpg"
						str_image_filename = Guid.NewGuid.ToString & ".jpg"
					Case "image/png"
						str_image_filename = Guid.NewGuid.ToString & ".png"
				End Select
				Dim str_image_path__relative = "~\dir_image\dir_anthology\" & str_pk_anthology & "\dir_story\" & inst_story.str_pk_story & "\" & str_image_filename
				System.IO.Directory.CreateDirectory(str_image_path__physical)
				cls_image.sub_save_file_from_url(str_image_path__physical & str_image_filename, str_media_url_0)
				inst_story.str_media_content_type = str_media_content_type_0
				inst_story.str_media_url = str_image_path__relative
			End If
			inst_story.sub_insert()

			HttpContext.Current.Items.Add("str_first_name", str_first_name)
			HttpContext.Current.Items.Add("str_pet_name", str_pet_name)
			HttpContext.Current.Items.Add("str_from", str_from)

			Server.Transfer("response.aspx")

		Catch ex As Exception
			If ex.Message <> "Thread was being aborted." Then
				'Response.Write(ex.Message)
			End If
		End Try

	End Sub

	Sub sub_no_pet_found(str__prm_pk_person_user As String, str__prm_phone_number_from As String)

		Dim inst_sql As New cls_sql
		inst_sql.str_table_name = "tbl_log_message"
		inst_sql.str_operation = cls_sql.en_operation.insert.ToString
		inst_sql.sub_add_column("fk_person_user", str__prm_pk_person_user)
		inst_sql.sub_add_column("message_context", str__prm_phone_number_from)
		inst_sql.sub_add_column("error_message", "Could Not find anthology (pet) record")
		inst_sql.sub_execute()

	End Sub

End Class
