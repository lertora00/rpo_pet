Imports Microsoft.VisualBasic
Imports ns_enterprise
Imports ns_enterprise.cls_utility

Namespace ns_enterprise
	Public Class cls_person_user_referral

		Public Shared Sub sub_log_referral(str__prm_url As String, str__prm_referral_code As String)

			Dim str_pk_person_user As String = Nothing

			'select substring(CAST(pk_person_user AS char(36)), 1, 3) + cast(id_person_user as char(50)) from tbl_person_user 

		End Sub

	End Class

End Namespace
