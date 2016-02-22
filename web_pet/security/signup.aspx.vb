Imports ns_enterprise
Imports ns_enterprise.cls_utility

Partial Class security_signup
	Inherits System.Web.UI.Page

	Private Sub btn_signup_Click(sender As Object, e As EventArgs) Handles btn_signup.Click

		sub_validate_input()

		If Page.IsValid = False Then
			plc_error.Visible = True
			Exit Sub
		End If

		Dim str_pk_person_user As String = fnc_create_user()

		Dim str_pet_name As String = fnc_convert_expected_string(txt_pet_name.Text)
		cls_anthology.sub_insert(str_pk_person_user, str_pet_name)

		Dim str_phone_number As String = fnc_convert_expected_string(txt_phone_number.Text)
		Dim str_sms_message As String = fnc_convert_expected_string(cls_constant.str_system_constant("text__welcome"))

		str_sms_message = Replace(str_sms_message, "[first_name]", fnc_convert_expected_string(txt_first_name.Text))
		str_sms_message = Replace(str_sms_message, "[pet_name]", fnc_convert_expected_string(txt_pet_name.Text))

		If Request.Url.ToString.ToLower.Contains("localhost") = False Then
			cls_sms.sub_send_message(str_phone_number, str_sms_message)
			cls_mailchimp.sub_add_new_user(txt_email_address.Text)
		End If

		If lbl_referral_code.Text.Length > 0 Then
			cls_person_user_referral.sub_log_referral(Request.Url.ToString, lbl_referral_code.Text, True)
		End If

		cls_current_user.sub_persist_current_user__all(str_pk_person_user)
		FormsAuthentication.SetAuthCookie(str_pk_person_user, True)
		Response.Redirect("verification.aspx")

	End Sub

	Sub sub_validate_input()

		Me.Validate()

		If cls_validation.fnc_validate_email(fnc_convert_expected_string(txt_email_address.Text)) = False Then
			cval_email_address.IsValid = False
		End If

		Dim int_email_address_count As Int32 = 0
		int_email_address_count = cls_data_access_layer.fnc_get_scaler__number("select count(*) from tbl_person_user where username = " & fnc_dbwrap(txt_email_address.Text))
		If int_email_address_count > 0 Then
			cval_email_address__member.IsValid = False
		End If

		If cls_validation.fnc_validate_phone_number_10_digit_only(fnc_convert_expected_string(txt_phone_number.Text)) = False Then
			cval_phone_number__format.IsValid = False
		End If

		Dim int_phone_number_count As Int32 = 0
		int_phone_number_count = cls_data_access_layer.fnc_get_scaler__number("select count(*) from tbl_person_phone where phone_number = " & fnc_dbwrap(txt_phone_number.Text))
		If int_phone_number_count > 0 Then
			cval_phone_number.IsValid = False
		End If

		If fnc_convert_expected_string(txt_password.Text) <> fnc_convert_expected_string(txt_password_confirm.Text) Then
			cval_password_confirm.IsValid = False
		End If

	End Sub

	Function fnc_create_user() As String

		' grab form fields
		Dim str_first_name As String = fnc_convert_expected_string(txt_first_name.Text)
		Dim str_last_name As String = fnc_convert_expected_string(txt_last_name.Text)
		Dim str_phone_number As String = fnc_convert_expected_string(txt_phone_number.Text)

		Dim str_email_address As String = fnc_convert_expected_string(txt_email_address.Text)
		Dim str_password As String = fnc_convert_expected_string(txt_password.Text)

		' create person 
		Dim inst_person As New cls_person()
		inst_person.str_first_name = str_first_name
		inst_person.str_last_name = str_last_name

		inst_person.fnc_insert()

		' create person phone
		inst_person.str_phone_number = str_phone_number
		inst_person.fnc_insert__phone_number()

		' create person email address
		inst_person.str_email_address = str_email_address
		inst_person.fnc_insert__email()

		' create person user
		Dim inst_person_user As New cls_person_user()
		inst_person_user.str_fk_person = inst_person.str_pk_person

		inst_person_user.str_onscreen_name = str_first_name & " " & str_last_name
		inst_person_user.str_password = str_password
		inst_person_user.str_username = str_email_address

		Dim str_pk_person_user As String = inst_person_user.fnc_insert()

		Return str_pk_person_user

	End Function

	Private Sub security_signup_Load(sender As Object, e As EventArgs) Handles Me.Load

		plc_error.Visible = False
		plc_success.Visible = False

		If IsPostBack = False Then
			If fnc_convert_expected_string(Session("ref")).Length > 0 Then
				lbl_referral_code.Text = fnc_convert_expected_string(Session("ref"))
			End If
			FormsAuthentication.SignOut()
			System.Web.HttpContext.Current.Session.Abandon()
		End If

	End Sub

End Class
