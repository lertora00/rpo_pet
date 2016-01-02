Imports ns_enterprise
Imports ns_enterprise.cls_utility

Partial Class security_verification
	Inherits System.Web.UI.Page

	Private Sub btn_setting_Click(sender As Object, e As EventArgs) Handles btn_setting.Click

		Response.Redirect("../user/account.aspx")

	End Sub

	Private Sub btn_signout_Click(sender As Object, e As EventArgs) Handles btn_signout.Click

		FormsAuthentication.SignOut()
		System.Web.HttpContext.Current.Session.Abandon()
		Response.Redirect("../default.aspx")

	End Sub

	Private Sub lbtn_resend_initial_email_Click(sender As Object, e As EventArgs) Handles lbtn_resend_initial_email.Click

		Try
			cls_mailchimp.sub_invite_resend(cls_current_user.str_email_address)
		Catch ex As Exception
			plc_error.Visible = True
			lbl_error.Text = "There was a problem re-sending your initial email."
			Exit Sub
		End Try

		plc_success.Visible = True
		lbl_success.Text = "Initial email re-sent."

	End Sub

	Private Sub lbtn_send_sms_Click(sender As Object, e As EventArgs) Handles lbtn_send_sms.Click

		Dim str_sms_message As String = fnc_convert_expected_string(cls_constant.str_system_constant("text__welcome"))

		str_sms_message = Replace(str_sms_message, "[first_name]", fnc_convert_expected_string(lbl_first_name.Text))
		str_sms_message = Replace(str_sms_message, "[pet_name]", fnc_convert_expected_string(lbl_pet_name.Text))

		cls_sms.sub_send_message(lbl_phone_number.Text, str_sms_message)

		plc_success.Visible = True
		lbl_success.Text = "Initial SMS re-sent.  Please remember to repond with YES."

	End Sub

	Private Sub security_verification_Load(sender As Object, e As EventArgs) Handles Me.Load

		lbl_first_name.Text = cls_current_user.str_first_name
		lbl_phone_number.Text = cls_current_user.str_phone_number
		lbl_pet_name.Text = cls_current_user.str_pet_name
		lbl_email_address.Text = cls_current_user.str_email_address

	End Sub

End Class
