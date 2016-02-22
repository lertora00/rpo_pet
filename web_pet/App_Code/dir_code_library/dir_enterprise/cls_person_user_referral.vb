Imports Microsoft.VisualBasic
Imports ns_enterprise
Imports ns_enterprise.cls_utility

Namespace ns_enterprise
	Public Class cls_person_user_referral

		Public Shared Sub sub_log_referral(str__prm_url As String, str__prm_referral_code As String, Optional bln__prm_sign_up As Boolean = False)

			Dim str_pk_person_user As String = cls_data_access_layer.fnc_get_scaler__string("select pk_person_user from tbl_person_user where substring(CAST(pk_person_user AS char(36)), 1, 3) + cast(id_person_user as char(50)) = " & fnc_dbwrap(str__prm_referral_code))

			If fnc_is_valid_guid(str_pk_person_user) = False Then Exit Sub

			Dim inst_sql As New cls_sql()
			inst_sql.str_table_name = "tbl_person_user_referral"
			inst_sql.str_operation = cls_sql.en_operation.insert.ToString
			If bln__prm_sign_up = True Then
				inst_sql.sub_add_column("referral_type", "sign_up")
			Else
				inst_sql.sub_add_column("referral_type", "page_access")
			End If
			inst_sql.sub_add_column("referral_url", str__prm_url)
			inst_sql.sub_add_column("referral_code", str__prm_referral_code)
			inst_sql.sub_add_column("fk_person_user", str_pk_person_user)
			inst_sql.sub_execute()

		End Sub

	End Class

End Namespace
