Imports ns_enterprise.cls_utility

Imports System.Data

Namespace ns_enterprise

	Public Class cls_log_security

		Public Enum en_log_security_type
			Registration = 0
			AccountConfirmation = 1
			Login = 2
			Logout = 3
			AccessDenied = 4
		End Enum

		' pk_log_security property
		Private str__prv_pk_log_security As String
		Public Property str_pk_log_security() As String

			Get
				Return str__prv_pk_log_security
			End Get
			Set(ByVal Value As String)
				str__prv_pk_log_security = Value
			End Set

		End Property

		' fk_person_user__apply_to property
		Private str__prv_fk_person_user__apply_to As String
		Public Property str_fk_person_user__apply_to() As String

			Get
				Return str__prv_fk_person_user__apply_to
			End Get
			Set(ByVal Value As String)
				str__prv_fk_person_user__apply_to = Value
			End Set

		End Property

		' fk_person_user__authenticated property
		Private str__prv_fk_person_user__authenticated As String
		Public Property str_fk_person_user__authenticated() As String

			Get
				Return str__prv_fk_person_user__authenticated
			End Get
			Set(ByVal Value As String)
				str__prv_fk_person_user__authenticated = Value
			End Set

		End Property

		' username__if_not_fk property
		Private str__prv_username__if_not_fk As String
		Public Property str_username__if_not_fk() As String

			Get
				Return str__prv_username__if_not_fk
			End Get
			Set(ByVal Value As String)
				str__prv_username__if_not_fk = Value
			End Set

		End Property

		' note property
		Private str__prv_note As String
		Public Property str_note() As String

			Get
				Return str__prv_note
			End Get
			Set(ByVal Value As String)
				str__prv_note = Value
			End Set

		End Property

		' sql_collection for logging 
		Private inst__prv_sql_collection As cls_sql_collection
		Public Property inst_sql_collection() As cls_sql_collection

			Get
				Return inst__prv_sql_collection
			End Get
			Set(ByVal Value As cls_sql_collection)
				inst__prv_sql_collection = Value
			End Set

		End Property

		Public Sub New()

			str__prv_pk_log_security = Guid.NewGuid().ToString

			inst__prv_sql_collection = New cls_sql_collection

		End Sub

		Public Shared Function str_get_pk_lkp_security_type(ByVal int__prm_log_security_type As en_log_security_type) As String

			Dim str_system_name As String = ""

			Select Case int__prm_log_security_type
				Case en_log_security_type.Registration
					str_system_name = "registration"
				Case en_log_security_type.AccountConfirmation
					str_system_name = "account_confirmation"
				Case en_log_security_type.Login
					str_system_name = "login"
				Case en_log_security_type.Logout
					str_system_name = "logout"
				Case en_log_security_type.AccessDenied
					str_system_name = "access_denied"
				Case Else
					Throw New Exception("Invalid security type: " & int__prm_log_security_type)
			End Select

			Return cls_user_interface.fnc_code_lkp("tbl_lkp_log_security_type", str_system_name)

		End Function

		Public Shared Function fnc_select_list() As DataTable

			Dim str_sql As String = "select * from udf_log_security()"

			Return cls_data_access_layer.fnc_get_datatable(str_sql)

		End Function

		Public Shared Function fnc_select_list(ByVal str__prm_filter As String) As DataTable

			If str__prm_filter.Length = 0 Then
				Return fnc_select_list()
				Exit Function
			End If

			Dim str_sql As String = "select * from udf_log_security() where " & str__prm_filter

			Return cls_data_access_layer.fnc_get_datatable(str_sql)

		End Function

		Public Shared Sub sub_delete(ByVal str__prm_pk As String)

			'Dim inst_dynamic_sql As New cls_dynamic_sql
			'inst_dynamic_sql.sub_delete("pk_log_security", str__prm_pk)

		End Sub

		Public Sub sub_log_security(ByVal en__prm_log_security_type As cls_log_security.en_log_security_type, ByVal bln__prm_success_flag As Boolean, ByVal bln__prm_authentication_flag As Boolean, ByVal bln__prm_authorization_flag As Boolean, Optional ByVal str__prm_note As String = "")

			Dim inst_sql As cls_sql = New cls_sql()
			inst_sql.str_operation = cls_sql.en_operation.insert.ToString
			inst_sql.str_table_name = "tbl_log_security"
			inst_sql.sub_add_column("pk_log_security", str__prv_pk_log_security)
			inst_sql.sub_add_column("fk_lkp_log_security_type", cls_log_security.str_get_pk_lkp_security_type(en__prm_log_security_type))
			inst_sql.sub_add_column("fk_person_user__authenticated", str__prv_fk_person_user__authenticated)
			inst_sql.sub_add_column("fk_person_user__apply_to", str__prv_fk_person_user__apply_to)
			inst_sql.sub_add_column("username__if_not_fk", fnc_convert_expected_string(str__prv_username__if_not_fk))
			inst_sql.sub_add_column("success_flag", fnc_convert_expected_bit(bln__prm_success_flag))
			inst_sql.sub_add_column("valid_authentication_flag", fnc_convert_expected_bit(bln__prm_authentication_flag))
			inst_sql.sub_add_column("valid_authorization_flag", fnc_convert_expected_bit(bln__prm_authorization_flag))

			If fnc_convert_expected_string(str__prm_note).Length > 0 Then
				inst_sql.sub_add_column("note", str__prm_note)
			Else
				inst_sql.sub_add_column("note", str__prv_note)
			End If

			inst__prv_sql_collection.Insert(0, inst_sql)

		End Sub

		Public Sub sub_log_security_event(ByVal bln__prm_success_flag As Boolean, ByVal bln__prm_failure_flag As Boolean, ByVal inst__prm_friendly_message As cls_friendly_message, Optional ByVal str__prm_note As String = "")

			sub_log_security_event(bln__prm_success_flag, bln__prm_failure_flag, str__prm_note & " : " & inst__prm_friendly_message.str_message_code_value & " : " & inst__prm_friendly_message.str_friendly_message)

		End Sub

		Public Sub sub_log_security_event(ByVal bln__prm_success_flag As Boolean, ByVal bln__prm_failure_flag As Boolean, Optional ByVal str__prm_note As String = "")

			Dim inst_sql As cls_sql = New cls_sql()
			inst_sql.str_operation = cls_sql.en_operation.insert.ToString
			inst_sql.str_table_name = "tbl_log_security_event"
			inst_sql.sub_add_column("fk_log_security", str__prv_pk_log_security)
			inst_sql.sub_add_column("success_flag", fnc_convert_expected_bit(bln__prm_success_flag))
			inst_sql.sub_add_column("failure_flag", fnc_convert_expected_bit(bln__prm_failure_flag))
			inst_sql.sub_add_column("note", str__prm_note)

			inst__prv_sql_collection.Add(inst_sql)

		End Sub

		Public Sub sub_log_security_parameter(ByVal str__prm_keyword As String, ByVal str__prm_value As String, Optional ByVal str__prm_note As String = "")

			Dim inst_sql As cls_sql = New cls_sql()
			inst_sql.str_operation = cls_sql.en_operation.insert.ToString
			inst_sql.str_table_name = "tbl_log_security_parameter"
			inst_sql.sub_add_column("fk_log_security", str__prv_pk_log_security)
			inst_sql.sub_add_column("keyword", str__prm_keyword)
			inst_sql.sub_add_column("value", str__prm_value)
			inst_sql.sub_add_column("note", str__prm_note)

			inst__prv_sql_collection.Add(inst_sql)

		End Sub

		Public Sub sub_log_execute()

			If cls_constant.bln_disable_log_security = False Then
				inst__prv_sql_collection.sub_execute()
			End If

		End Sub

	End Class

End Namespace
