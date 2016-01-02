Imports System.Data
Imports System.Net
Imports System.Net.Mail
Imports ns_enterprise
Imports ns_enterprise.cls_utility


Partial Class security_login
	Inherits System.Web.UI.Page

	Protected Sub btn_login_Click(sender As Object, e As EventArgs) Handles btn_login.Click

		Dim str_username As String = fnc_convert_expected_string(txt_username.Text)
		If str_username.Length = 0 Then str_username = "(left blank)"

		Dim str_password As String = fnc_convert_expected_string(txt_password.Text)
		If str_password.Length = 0 Then str_password = "(left blank)"

		Dim str_pk_person_user As String = cls_data_access_layer.fnc_get_scaler__string("select pk_person_user from tbl_person_user where active_flag = 1 and username = " & fnc_dbwrap(txt_username.Text))

		Dim inst_log_security As New cls_log_security

		If fnc_is_valid_guid(str_pk_person_user) = False Then
			inst_log_security = New cls_log_security
			inst_log_security.str_username__if_not_fk = str_username
			inst_log_security.str_note = "Invalid username...pwd: " & str_password
			inst_log_security.sub_log_security(cls_log_security.en_log_security_type.Login, False, False, False)
			inst_log_security.sub_log_execute()
			plc_error.Visible = True
			Exit Sub
		End If

		Dim str_pk_person_user__password As String = cls_data_access_layer.fnc_get_scaler__string("select pk_person_user from tbl_person_user where active_flag = 1 and pk_person_user = " & fnc_dbwrap(str_pk_person_user) & " and password = " & fnc_dbwrap(txt_password.Text))

		If fnc_is_valid_guid(str_pk_person_user__password) = False Then
			inst_log_security = New cls_log_security
			inst_log_security.str_fk_person_user__apply_to = str_pk_person_user
			inst_log_security.str_username__if_not_fk = str_username
			inst_log_security.str_note = "Invalid password...pwd: " & str_password
			inst_log_security.sub_log_security(cls_log_security.en_log_security_type.Login, False, False, False)
			inst_log_security.sub_log_execute()
			plc_error.Visible = True
			Exit Sub
		End If

		inst_log_security = New cls_log_security
		inst_log_security.str_fk_person_user__authenticated = str_pk_person_user
		inst_log_security.str_username__if_not_fk = str_username
		inst_log_security.sub_log_security(cls_log_security.en_log_security_type.Login, True, True, True)
		inst_log_security.sub_log_execute()

		Dim int_phone_number_validated As Int32 = cls_data_access_layer.fnc_get_scaler__number("select phone_number_validated from tbl_person_user where pk_person_user = " & fnc_dbwrap(str_pk_person_user))
		Dim int_email_validated As Int32 = cls_data_access_layer.fnc_get_scaler__number("select email_validated from tbl_person_user where pk_person_user = " & fnc_dbwrap(str_pk_person_user))

		cls_current_user.sub_persist_current_user__all(str_pk_person_user)

		If int_phone_number_validated = 1 And int_email_validated = 1 Then
			FormsAuthentication.RedirectFromLoginPage(str_pk_person_user, True)
		Else
			FormsAuthentication.SetAuthCookie(str_pk_person_user, True)
			Response.Redirect("verification.aspx")
		End If

	End Sub

	Private Sub btn_forgot_password_Click(sender As Object, e As EventArgs) Handles btn_forgot_password.Click

		Response.Redirect("forgot_password.aspx")

	End Sub

	Private Sub security_login_Load(sender As Object, e As EventArgs) Handles Me.Load

		plc_error.Visible = False

	End Sub

End Class
