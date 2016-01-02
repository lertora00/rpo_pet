Imports Microsoft.VisualBasic

Namespace ns_enterprise
	Public Class cls_anthology

		Public Shared Sub sub_insert(ByVal str__prm_fk_person_user As String, ByVal str__prm_pet_name As String)

			Dim inst_sql As New cls_sql
			inst_sql.str_table_name = "tbl_anthology"
			inst_sql.str_operation = cls_sql.en_operation.insert.ToString
			inst_sql.sub_add_column("fk_person_user", str__prm_fk_person_user)
			inst_sql.sub_add_column("name", str__prm_pet_name)

			inst_sql.sub_execute_with_audit(False)

		End Sub

	End Class

End Namespace