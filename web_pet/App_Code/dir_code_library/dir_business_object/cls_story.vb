Imports ns_enterprise.cls_utility

Imports Microsoft.VisualBasic

Namespace ns_enterprise
	Public Class cls_story

		Private str__prv_pk_story As String
		Public Property str_pk_story() As String

			Get
				Return str__prv_pk_story
			End Get
			Set(ByVal Value As String)
				str__prv_pk_story = fnc_convert_expected_string(Value)
			End Set

		End Property

		Private str__prv_from_number As String
		Public Property str_from_number() As String

			Get
				Return str__prv_from_number
			End Get
			Set(ByVal Value As String)
				str__prv_from_number = fnc_convert_expected_string(Value)
			End Set

		End Property

		Private str__prv_to_number As String
		Public Property str_to_number() As String

			Get
				Return str__prv_to_number
			End Get
			Set(ByVal Value As String)
				str__prv_to_number = fnc_convert_expected_string(Value)
			End Set

		End Property

		Private str__prv_message As String
		Public Property str_message() As String

			Get
				Return str__prv_message
			End Get
			Set(ByVal Value As String)
				str__prv_message = fnc_convert_expected_string(Value)
			End Set

		End Property

		Private str__prv_fk_anthology As String
		Public Property str_fk_anthology() As String

			Get
				Return str__prv_fk_anthology
			End Get
			Set(ByVal Value As String)
				str__prv_fk_anthology = fnc_convert_expected_string(Value)
			End Set

		End Property

		Private str__prv_fk_story As String
		Public Property str_fk_story() As String

			Get
				Return str__prv_fk_story
			End Get
			Set(ByVal Value As String)
				str__prv_fk_story = fnc_convert_expected_string(Value)
			End Set

		End Property

		Private str__prv_fk_person_user As String
		Public Property str_fk_person_user() As String

			Get
				Return str__prv_fk_person_user
			End Get
			Set(ByVal Value As String)
				str__prv_fk_person_user = fnc_convert_expected_string(Value)
			End Set

		End Property

		Private str__prv_fk_question_pet As String
		Public Property str_fk_question_pet() As String

			Get
				Return str__prv_fk_question_pet
			End Get
			Set(ByVal Value As String)
				str__prv_fk_question_pet = fnc_convert_expected_string(Value)
			End Set

		End Property

		Private str__prv_bln_incoming As Boolean
		Public Property str_bln_incoming() As Boolean

			Get
				Return str__prv_bln_incoming
			End Get
			Set(ByVal Value As Boolean)
				str__prv_bln_incoming = fnc_convert_expected_boolean(Value)
			End Set

		End Property

		Private str__prv_fk_person_user__insert As String
		Public Property str_fk_person_user__insert() As String

			Get
				Return str__prv_fk_person_user__insert
			End Get
			Set(ByVal Value As String)
				str__prv_fk_person_user__insert = fnc_convert_expected_string(Value)
			End Set

		End Property

		Private str__prv_media_content_type As String
		Public Property str_media_content_type() As String

			Get
				Return str__prv_media_content_type
			End Get
			Set(ByVal Value As String)
				str__prv_media_content_type = fnc_convert_expected_string(Value)
			End Set

		End Property

		Private str__prv_media_url As String

		Public Sub New()

			str__prv_pk_story = Guid.NewGuid.ToString

		End Sub

		Public Property str_media_url() As String

			Get
				Return str__prv_media_url
			End Get
			Set(ByVal Value As String)
				str__prv_media_url = fnc_convert_expected_string(Value)
			End Set

		End Property

		Public Sub sub_insert()

			Try

				Dim inst_sql As New cls_sql

				inst_sql.str_operation = cls_sql.en_operation.insert.ToString
				inst_sql.str_table_name = "tbl_story"
				inst_sql.sub_add_column("fk_anthology", str__prv_fk_anthology)
				inst_sql.sub_add_column("fk_question_pet", str__prv_fk_question_pet)
				inst_sql.sub_add_column("fk_story", str__prv_fk_story)
				inst_sql.sub_add_column("fk_person_user", str__prv_fk_person_user)
				inst_sql.sub_add_column("from_number", str__prv_from_number)
				inst_sql.sub_add_column("to_number", str__prv_to_number)
				inst_sql.sub_add_column("message", str__prv_message)
				inst_sql.sub_add_column("incoming_flag", str__prv_bln_incoming)
				If fnc_convert_expected_string(str__prv_media_content_type).Length > 0 Then
					inst_sql.sub_add_column("media_content_type", str__prv_media_content_type)
					inst_sql.sub_add_column("media_url", str__prv_media_url)
				End If
				inst_sql.sub_execute_with_audit(False)

			Catch ex As Exception
				HttpContext.Current.Response.Write(ex.Message)
				HttpContext.Current.Response.End()
			End Try

		End Sub

		Public Shared Sub sub_delete(str__prm As String)

			Dim inst_sql As New cls_sql

			inst_sql.str_operation = cls_sql.en_operation.update.ToString
			inst_sql.str_table_name = "tbl_story"
			inst_sql.sub_add_column("pk_story", str__prm)
			inst_sql.sub_add_column("active_flag", 0)

			inst_sql.sub_execute_with_audit(False)

		End Sub

	End Class

End Namespace
