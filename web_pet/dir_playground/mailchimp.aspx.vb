Imports ns_enterprise
Imports ns_enterprise.cls_utility

Partial Class dir_playground_mailchimp
	Inherits System.Web.UI.Page

	Private Sub btn_call_Click(sender As Object, e As EventArgs) Handles btn_call.Click

		Dim x As New MailChimp.MailChimpManager("765924cbd9858a1cc64c60017c3c11b2-us12")

		Dim y As New MailChimp.Helper.EmailParameter
		y.Email = "PhilTest@Lertora.com"

		'		x.Subscribe(cls_constant.str_system_constant("mailchimp_list_id__signup"), y, Nothing, "html", True, False, False, True)
		x.Subscribe(cls_constant.str_system_constant("mailchimp_list_id__signup"), y, Nothing, "html", True, False, False, True)

	End Sub

End Class
