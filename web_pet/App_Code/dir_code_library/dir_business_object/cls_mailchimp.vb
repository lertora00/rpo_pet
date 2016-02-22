Option Infer On
Imports ns_enterprise
Imports ns_enterprise.cls_utility

Imports Microsoft.VisualBasic

Namespace ns_enterprise

	Public Class cls_mailchimp

		Public Shared Sub sub_add_new_user(str__prm_email_address As String)

			Dim mcm As New MailChimp.MailChimpManager(cls_constant.str_system_constant("mailchimp_apikey"))

			Dim ep As New MailChimp.Helper.EmailParameter
			ep.Email = str__prm_email_address

			mcm.Subscribe(cls_constant.str_system_constant("mailchimp_list_id__signup"), ep, Nothing, "html", True, False, False, True)

		End Sub

		Public Shared Sub sub_invite_resend(str__prm_email_address As String)

			Dim mcm As New MailChimp.MailChimpManager(cls_constant.str_system_constant("mailchimp_apikey"))

			' this doesn't work - seems to be tied to an invite rather than subscribe
			'mcm.InviteResend(str__prm_email_address)

			Dim ep As New MailChimp.Helper.EmailParameter
			ep.Email = str__prm_email_address

			mcm.Unsubscribe(cls_constant.str_system_constant("mailchimp_list_id__signup"), ep, True, False, False)
			mcm.Subscribe(cls_constant.str_system_constant("mailchimp_list_id__signup"), ep, Nothing, "html", True, True, False, True)

		End Sub

		Public Shared Sub sub_unsubscribe(str__prm_email_address As String)

			Dim mcm As New MailChimp.MailChimpManager(cls_constant.str_system_constant("mailchimp_apikey"))

			' this doesn't work - seems to be tied to an invite rather than subscribe
			'mcm.InviteResend(str__prm_email_address)

			Dim ep As New MailChimp.Helper.EmailParameter
			ep.Email = str__prm_email_address

			mcm.Unsubscribe(cls_constant.str_system_constant("mailchimp_list_id__signup"), ep, True, True, True)

		End Sub

		Public Shared Function fnc_is_verified(str__prm_email_address As String) As Boolean

			Dim mcm As New MailChimp.MailChimpManager(cls_constant.str_system_constant("mailchimp_apikey"))

			Dim lst_email As New List(Of MailChimp.Helper.EmailParameter)

			Dim ep As New MailChimp.Helper.EmailParameter
			ep.Email = str__prm_email_address

			lst_email.Add(ep)

			Dim info = mcm.GetMemberInfo(cls_constant.str_system_constant("mailchimp_list_id__signup"), lst_email)
			Dim member = info.Data.SingleOrDefault()

			If member Is Nothing Then Return False

			If fnc_convert_expected_string(member.TimestampOptIn).Length > 0 Then
				Return True
			End If

			Return False

		End Function

	End Class

End Namespace
