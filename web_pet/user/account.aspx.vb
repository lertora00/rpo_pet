Imports ns_enterprise
Imports ns_enterprise.cls_utility

Imports System.Data


Partial Class user_account
	Inherits System.Web.UI.Page

	Private Sub btn_save_Click(sender As Object, e As EventArgs) Handles btn_save.Click

		sub_validate_input()

		If IsValid = False Then
			plc_error.Visible = True
			Exit Sub
		End If

		Dim bln_updated As Boolean = False

		If txt_first_name.Text <> cls_current_user.str_first_name Or txt_last_name.Text <> cls_current_user.str_last_name Then
			bln_updated = True
			Dim inst_person As New cls_person
			inst_person.str_pk_person = cls_current_user.str_pk_person
			inst_person.str_first_name = cls_utility.fnc_get_user_input(txt_first_name.Text)
			inst_person.str_last_name = cls_utility.fnc_get_user_input(txt_last_name.Text)
			inst_person.sub_update
		End If

		If fnc_convert_expected_string(txt_password__current.Text).Length > 0 Then
			bln_updated = True
			Dim inst_person_user As New cls_person_user
			inst_person_user.str_pk_person_user = cls_current_user.str_pk_person_user
			inst_person_user.str_password = fnc_convert_expected_string(txt_password.Text)
			inst_person_user.sub_update()
		End If

		If txt_email_address.Text <> cls_current_user.str_email_address Then
			bln_updated = True
			Dim inst_person_user As New cls_person_user
			inst_person_user.str_pk_person_user = cls_current_user.str_pk_person_user
			inst_person_user.str_username = fnc_convert_expected_string(txt_email_address.Text)
			inst_person_user.sub_update()

			Dim inst_person_email As New cls_person_email
			inst_person_email.str_pk_person_email = cls_data_access_layer.fnc_get_scaler__string("select pk_person_email from tbl_person_email where fk_person = " & fnc_dbwrap(cls_current_user.str_pk_person) & " and email_address = " & fnc_dbwrap(cls_current_user.str_email_address))
			inst_person_email.str_email_address = fnc_convert_expected_string(txt_email_address.Text)
			inst_person_email.sub_update()

			cls_mailchimp.sub_unsubscribe(cls_current_user.str_email_address)
			cls_mailchimp.sub_unsubscribe(txt_email_address.Text)
			cls_data_access_layer.sub_execute_non_query("update tbl_person_user set email_validated = 0 where pk_person_user = " & fnc_dbwrap(cls_current_user.str_pk_person_user))
		End If

		If fnc_convert_expected_string(txt_phone_number.Text) <> cls_current_user.str_phone_number Then
			bln_updated = True
			Dim inst_person_phone As New cls_person_phone
			inst_person_phone.str_pk_person_phone = cls_data_access_layer.fnc_get_scaler__string("select pk_person_phone from tbl_person_phone where fk_person = " & fnc_dbwrap(cls_current_user.str_pk_person) & " and phone_number = " & fnc_dbwrap(cls_current_user.str_phone_number))
			inst_person_phone.str_phone_number = fnc_get_user_input(txt_phone_number.Text)
			inst_person_phone.sub_update()

			cls_data_access_layer.sub_execute_non_query("update tbl_person_user set phone_number_validated = 0 where pk_person_user = " & fnc_dbwrap(cls_current_user.str_pk_person_user))
			' let them know they are good to go; and ask first question; inform scheduled messages to follow
			Dim str_sms_message As String = fnc_convert_expected_string(cls_constant.str_system_constant("text__phone_number_changed"))
			str_sms_message = Replace(str_sms_message, "[first_name]", txt_first_name.Text)
			str_sms_message = Replace(str_sms_message, "[pet_name]", cls_current_user.str_pet_name)

			cls_sms.sub_send_message(txt_phone_number.Text, str_sms_message)
		End If

		If chk_disable_sms.Checked <> cls_data_access_layer.fnc_get_scaler__boolean("select disable_sms from tbl_person_user where pk_person_user = " & fnc_dbwrap(cls_current_user.str_pk_person_user)) Then
			bln_updated = True

			Dim inst_person_user As New cls_person_user
			inst_person_user.str_pk_person_user = cls_current_user.str_pk_person_user
			inst_person_user.bln_disable_sms = fnc_convert_expected_boolean(chk_disable_sms.Checked)
			inst_person_user.sub_update()
		End If

		If bln_updated = True Then
			cls_current_user.sub_persist_current_user__all(cls_current_user.str_pk_person_user)
			plc_success.Visible = True
			lbl_success.Text = "Success!  Account information updated."
		End If

	End Sub

	Private Sub user_account_Load(sender As Object, e As EventArgs) Handles Me.Load

		If fnc_convert_expected_string(Request.UrlReferrer).Contains("verification") Then
			Dim a As HtmlAnchor = DirectCast(fnc_find_nested_control(Page, "a_pet"), HtmlAnchor)
			a.Visible = False
			Dim plc As PlaceHolder = DirectCast(fnc_find_nested_control(Page, "plc_setting"), PlaceHolder)
			plc.Visible = False
		End If

		lbl_phone_number_changed.Text = "0"

		plc_error.Visible = False
		plc_success.Visible = False

		If IsPostBack = False Then
			lbl_first_name.Text = cls_current_user.str_first_name
			txt_first_name.Text = cls_current_user.str_first_name
			txt_last_name.Text = cls_current_user.str_last_name
			txt_email_address.Text = cls_current_user.str_email_address
			txt_phone_number.Text = cls_current_user.str_phone_number
			chk_disable_sms.Checked = cls_data_access_layer.fnc_get_scaler__boolean("select disable_sms from tbl_person_user where pk_person_user = " & fnc_dbwrap(cls_current_user.str_pk_person_user))
		End If

	End Sub

	Sub sub_validate_input()

		' check email if changed
		If txt_email_address.Text <> cls_current_user.str_email_address Then
			If cls_validation.fnc_validate_email(fnc_convert_expected_string(txt_email_address.Text)) = False Then
				cval_email_address.IsValid = False
			End If

			Dim int_email_address_count As Int32 = 0
			int_email_address_count = cls_data_access_layer.fnc_get_scaler__number("Select count(*) from tbl_person_user where username = " & fnc_dbwrap(txt_email_address.Text))
			If int_email_address_count > 0 Then
				cval_email_address__member.IsValid = False
			End If
		End If

		' check phone if changed
		If txt_phone_number.Text <> cls_current_user.str_phone_number Then
			If cls_validation.fnc_validate_phone_number_10_digit_only(fnc_convert_expected_string(txt_phone_number.Text)) = False Then
				cval_phone_number__format.IsValid = False
			End If

			Dim int_phone_number_count As Int32 = 0
			int_phone_number_count = cls_data_access_layer.fnc_get_scaler__number("Select count(*) from tbl_person_phone where phone_number = " & fnc_dbwrap(txt_phone_number.Text))
			If int_phone_number_count > 0 Then
				cval_phone_number.IsValid = False
			End If

			lbl_phone_number_changed.Text = "1"
		End If

		If fnc_convert_expected_string(txt_password.Text) <> fnc_convert_expected_string(txt_password_confirm.Text) Then
			cval_password_confirm.IsValid = False
		End If

		If fnc_convert_expected_string(txt_password__current.Text).Length > 0 Then
			If fnc_convert_expected_string(txt_password__current.Text) <> cls_data_access_layer.fnc_get_scaler__string("Select password from tbl_person_user where pk_person_user = " & fnc_dbwrap(cls_current_user.str_pk_person_user)) Then
				cval_password_current.IsValid = False
			End If
		End If

		If fnc_convert_expected_string(txt_password__current.Text).Length = 0 And fnc_convert_expected_string(txt_password.Text).Length > 0 Then
			cval_password_current.IsValid = False
		End If

	End Sub

End Class
