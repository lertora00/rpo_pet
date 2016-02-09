Imports System.Data

Imports ns_enterprise
Imports ns_enterprise.cls_utility

Imports System.Configuration.ConfigurationSettings
Imports System.Configuration
Imports System.Web.Configuration
Imports System.Web
Imports System.Web.HttpContext

Namespace ns_enterprise

  Public Class cls_constant

		' Globally turns off sql auditing
    Public Shared ReadOnly Property int_disable_sql_audit() As Int32
      Get
				Return fnc_convert_expected_int32(ConfigurationManager.AppSettings("int_disable_sql_audit"))
      End Get
    End Property

    ' auditing turned off for type of audit (insert, update, delete)
    Public Shared ReadOnly Property int_disable_sql_audit(ByVal str__prm_operation As String) As Int32
      Get
				Return fnc_convert_expected_int32(ConfigurationManager.AppSettings("int_disable_sql_audit__" & str__prm_operation))
      End Get
    End Property


    ' will add the message time (including milliseconds to messages rendered to ui)
    Public Shared ReadOnly Property bln_show_message_time() As Boolean
      Get
        Return fnc_convert_expected_boolean(ConfigurationManager.AppSettings("bln_show_message_time"))
      End Get
    End Property

    Public Enum en_delete_type_override
      use_global_setting = 0
      logical = 1
      permanent = 2
    End Enum

    Public Enum en_delete_audit_override
      use_global_setting = 0
      audit_delete = 1
      do_not_audit_delete = 2
    End Enum

    Public Shared ReadOnly Property str_db_wrap() As String
      Get
        Return "'"
      End Get
    End Property

    Public Shared ReadOnly Property str_application_name() As String
      Get
        Return "win_1095"
      End Get
    End Property

		Public Shared ReadOnly Property str_subweb(Optional ByVal bln__prm_include_slash As Boolean = False) As String
			Get
				If bln__prm_include_slash = True Then
					Return fnc_convert_expected_string(ConfigurationManager.AppSettings("subweb")) & "/"
				Else
					Return fnc_convert_expected_string(ConfigurationManager.AppSettings("subweb"))
				End If
			End Get
		End Property

		Public Shared ReadOnly Property bln_disable_log_security() As Boolean
			Get
				Return fnc_convert_expected_boolean(ConfigurationManager.AppSettings("bln_disable_log_security"))
			End Get
		End Property

    Public Shared ReadOnly Property bln_swap_email() As Boolean
      Get
        Return fnc_convert_expected_boolean(ConfigurationManager.AppSettings("bln_swap_email"))
      End Get
    End Property

    Public Shared ReadOnly Property str_email_swap_to() As String
      Get
        Return fnc_convert_expected_string(ConfigurationManager.AppSettings("str_email_swap_to"))
      End Get
    End Property

    Public Shared ReadOnly Property str_email_swap_cc() As String
      Get
        Return fnc_convert_expected_string(ConfigurationManager.AppSettings("str_email_swap_cc"))
      End Get
    End Property

    Public Shared ReadOnly Property str_email_swap_bcc() As String
      Get
        Return fnc_convert_expected_string(ConfigurationManager.AppSettings("str_email_swap_bcc"))
      End Get
    End Property

    Public Shared ReadOnly Property bln_disable_log_session() As Boolean
      Get
        Return fnc_convert_expected_boolean(ConfigurationManager.AppSettings("bln_disable_log_session"))
      End Get
    End Property

    Public Shared ReadOnly Property str_message__forgot_password__found() As String
      Get
        Return fnc_convert_expected_string(ConfigurationManager.AppSettings("str_message__forgot_password__found"))
      End Get
    End Property

    Public Shared ReadOnly Property int_email_error_log_severity() As Int16
      Get
        Return ConfigurationManager.AppSettings("int_email_error_log_severity")
      End Get
    End Property

    Public Shared ReadOnly Property str_error_email_to_list() As String
      Get
        Return ConfigurationManager.AppSettings("str_error_email_to_list")
      End Get
    End Property

    Public Shared ReadOnly Property bln_dynamic_sql_debug() As Boolean
      Get
        Return fnc_convert_expected_boolean(ConfigurationManager.AppSettings("cls_dynamic_sql_debug"))
      End Get
    End Property
    Public Shared ReadOnly Property bln_disable_logical_delete() As Boolean
      Get
        Return fnc_convert_expected_boolean(ConfigurationManager.AppSettings("bln_disable_logical_delete"))
      End Get
    End Property

    Public Shared ReadOnly Property bln_disable_dynamic_audit__delete() As Boolean
      Get
        Return fnc_convert_expected_boolean(ConfigurationManager.AppSettings("int_disable_sql_audit__delete"))
      End Get
    End Property

    Public Shared ReadOnly Property str_email_from() As String
      Get
        Return fnc_convert_expected_string(ConfigurationManager.AppSettings("email_from"))
      End Get
    End Property

    Public Shared ReadOnly Property str_email_delimiter() As String
      Get
        Return fnc_convert_expected_string(ConfigurationManager.AppSettings("email_delimiter"))
      End Get
    End Property

    Public Shared ReadOnly Property str_smtp_server() As String
      Get
        Return fnc_convert_expected_string(ConfigurationManager.AppSettings("email_smtp_server"))
      End Get
    End Property

    Public Shared ReadOnly Property str_update_date_column_name() As String
      Get
        Return fnc_convert_expected_string(ConfigurationManager.AppSettings("str_update_date_column_name"))
      End Get
    End Property

    Public Shared ReadOnly Property str_update_user_column_name_pk() As String
      Get
        Return fnc_convert_expected_string(ConfigurationManager.AppSettings("str_update_user_column_name_pk"))
      End Get
    End Property

    Public Shared ReadOnly Property str_update_user_column_name() As String
      Get
        Return fnc_convert_expected_string(ConfigurationManager.AppSettings("str_update_user_column_name"))
      End Get
    End Property

    Public Shared ReadOnly Property str_insert_date_column_name() As String
      Get
        Return fnc_convert_expected_string(ConfigurationManager.AppSettings("str_insert_date_column_name"))
      End Get
    End Property

    Public Shared ReadOnly Property str_insert_user_column_name_pk() As String
      Get
        Return fnc_convert_expected_string(ConfigurationManager.AppSettings("str_insert_user_column_name_pk"))
      End Get
    End Property

    Public Shared ReadOnly Property str_insert_user_column_name() As String
      Get
        Return fnc_convert_expected_string(ConfigurationManager.AppSettings("str_insert_user_column_name"))
      End Get
    End Property

    Public Shared ReadOnly Property int_audit_top_limit__table() As Int32
      Get
        Return fnc_convert_expected_string(ConfigurationManager.AppSettings("int_audit_top_limit__table"))
      End Get
    End Property

    Public Shared ReadOnly Property int_audit_top_limit__column() As Int32
      Get
        Return fnc_convert_expected_string(ConfigurationManager.AppSettings("int_audit_top_limit__column"))
      End Get
    End Property

    Public Shared ReadOnly Property bln_show_audit_override_role() As Boolean
      Get
        Return fnc_convert_expected_boolean(ConfigurationManager.AppSettings("int_show_audit_override_role"))
      End Get
    End Property

    Public Shared ReadOnly Property str_active_flag_column_name() As String
      Get
        Return "active_flag"
      End Get
    End Property

    Public Shared ReadOnly Property str_user_defined_function_prefix() As String
      Get
        Return "udf_"
      End Get
    End Property

    Public Shared ReadOnly Property str_table_prefix() As String
      Get
        Return "tbl_"
      End Get
    End Property

    Public Shared ReadOnly Property str_table_prefix__lookup() As String
      Get
        Return "tbl_lkp_"
      End Get
    End Property

    Public Shared ReadOnly Property str_foreign_key_prefix() As String
      Get
        Return "fk_"
      End Get
    End Property

    Public Shared ReadOnly Property str_foreign_key_prefix__lookup() As String
      Get
        Return "fk_lkp_"
      End Get
    End Property

    Public Shared ReadOnly Property str_primary_key_prefix() As String
      Get
        Return "pk_"
      End Get
    End Property

    Public Shared ReadOnly Property str_id_prefix() As String
      Get
        Return "id_"
      End Get
    End Property

    Public Shared ReadOnly Property str_primary_key_prefix__lookup() As String
      Get
        Return "pk_lkp_"
      End Get
    End Property

    Public Shared ReadOnly Property str_id_prefix__lookup() As String
      Get
        Return "id_lkp_"
      End Get
    End Property

    Public Shared ReadOnly Property str_primary_key_querystring_key() As String
      Get
        Return "pk"
      End Get
    End Property

    Public Shared ReadOnly Property str_foreign_key_querystring_key() As String
      Get
        Return "fk"
      End Get
    End Property

    Public Shared ReadOnly Property str_ascending() As String
      Get
        Return "ascending"
      End Get
    End Property

    Public Shared ReadOnly Property str_descending() As String
      Get
        Return "descending"
      End Get
    End Property

    Public Shared ReadOnly Property str_toggle() As String
      Get
        Return "toggle"
      End Get
    End Property

    Public Shared ReadOnly Property str_command_delimiter() As String
      Get
        Return ":"
      End Get
    End Property

    Public Shared ReadOnly Property str_truncate_long_text() As String
      Get
        Return "..."
      End Get
    End Property

    Public Shared ReadOnly Property str_list_index_lowerbound() As String
      Get
        Return "00"
      End Get
    End Property

    Public Shared ReadOnly Property str_date_format() As String
      Get
        Return fnc_convert_expected_string(ConfigurationManager.AppSettings("str_date_format"))
      End Get
    End Property

    Public Shared ReadOnly Property str_audit_old_value_display__insert() As String
      Get
        Return fnc_convert_expected_string(ConfigurationManager.AppSettings("str_audit_old_value_display__insert"))
      End Get
    End Property

    Public Shared ReadOnly Property str_audit_old_value_display__update__empty() As String
      Get
        Return fnc_convert_expected_string(ConfigurationManager.AppSettings("str_audit_old_value_display__update__empty"))
      End Get
    End Property

		Public Shared ReadOnly Property str_data_container_name() As String
			Get
				Return fnc_convert_expected_string(ConfigurationManager.AppSettings("str_data_container_name"))
			End Get
		End Property

		Public Shared Function fnc_get_connection_string__readonly() As String

			Return fnc_convert_expected_string(System.Configuration.ConfigurationManager.ConnectionStrings("db_petfolio"))

			'Return "Server=(local);Database=db_service_lifecycle;Trusted_Connection=True;"

			' future use - can use different configurations (readonly, administrator, read-write, etc.

			Return fnc_get_connection_string__readwrite()

		End Function

		Public Shared Function fnc_get_connection_string__readwrite() As String

			Return fnc_convert_expected_string(System.Configuration.ConfigurationManager.ConnectionStrings("db_petfolio"))

      'Return "Server=(local);Database=db_service_lifecycle;Trusted_Connection=True;"

      ' storing in cls_variable so we don't have to hit the file system over and over
      If fnc_convert_expected_string(cls_variable.str_connection_string__readwrite).Length > 0 Then
				Return cls_variable.str_connection_string__readwrite
			End If

      Dim str_path__configuration As String = cls_utility.str_physical_root & "/" & cls_constant.str_application_name & "__configuration/dir_environment/dir_database/"

			str_path__configuration = cls_utility.fnc_clean_path(str_path__configuration, True)

			' check external web.config file exists - there must be a file named web.config (contents don't matter)
			Dim fi_web_config As New IO.FileInfo(str_path__configuration & "web.config")
			If fi_web_config.Exists = False Then
				Throw New Exception("External db config (00) not found: " & str_path__configuration & "web.config")
			End If

			' check to be sure the web.config.config external file exists
			Dim fi_web_config_config = New IO.FileInfo(str_path__configuration & "web.config.config")
			If fi_web_config_config.Exists = False Then
				Throw New Exception("External db config.config (01) not found: " & str_path__configuration & "web.config.config")
			End If

			Dim cnf_database As Configuration = ConfigurationManager.OpenExeConfiguration(str_path__configuration & "web.config")

			If cnf_database.AppSettings.SectionInformation.IsProtected = False Then
				cnf_database.AppSettings.SectionInformation.ProtectSection("DataProtectionConfigurationProvider")
				cnf_database.Save()
			End If

			Dim str_connection_string As String
			Dim str_connection_name As String = fnc_convert_expected_string(cnf_database.AppSettings.Settings("connection_string_name").Value)

			str_connection_string = cnf_database.AppSettings.Settings("connection_string_" & str_connection_name).Value

			' if connection_string not found, throw meaningful exception
			If str_connection_string Is Nothing OrElse str_connection_string.Length = 0 Then
				Throw New Exception("Connection string name and string not found in webconfig.")
			End If

			cls_variable.str_connection_string__readwrite = str_connection_string

			Return str_connection_string

		End Function

		Public Shared Function fnc_get_connection_string__administrator() As String

			' future use - can use different configurations (readonly, administrator, read-write, etc.

			Return fnc_get_connection_string__readwrite()

		End Function

		Public Shared ReadOnly Property str_operation_insert() As String
			Get
				Return "insert"
			End Get
		End Property

    Public Shared ReadOnly Property str_operation_update() As String
      Get
        Return "update"
      End Get
    End Property

    Public Shared ReadOnly Property str_operation_delete() As String
      Get
        Return "delete"
      End Get
    End Property

    Public Shared ReadOnly Property str_validation_date() As String
      Get
        Return "Invalid date/time format."
      End Get
    End Property

    Public Shared ReadOnly Property str_validation_length() As String
      Get
        Return "[label_name] Input ([input size]) exceeds defined size ([defined size])"
      End Get
    End Property

    Public Shared ReadOnly Property str_validation_datatype() As String
      Get
        Return "[label_name] Invalid input ([input_value]) for type: [datatype] [valid_data]"
      End Get
    End Property

    Public Shared ReadOnly Property str_validation_backcolor() As String
      Get
        Return fnc_convert_expected_string(ConfigurationManager.AppSettings("validation_backcolor"))
      End Get
    End Property

    Public Shared ReadOnly Property str_validation_bordercolor() As String
      Get
        Return fnc_convert_expected_string(ConfigurationManager.AppSettings("validation_bordercolor"))
      End Get
    End Property

    Public Shared ReadOnly Property int_validation_borderwidth() As Int32
      Get
        Return fnc_convert_expected_int32(ConfigurationManager.AppSettings("validation_borderwidth"))
      End Get
    End Property

    Public Shared ReadOnly Property str_admininstrator_list() As String
      Get
        Return "2A89659E-32DD-41C7-A450-2F5BE3EF365D, D7EF7FD0-E800-49C4-909C-5E68BDEEA8B0, 4CB56A22-8614-43CA-93D6-CFB47749D0C5"
      End Get
    End Property

		Public Shared ReadOnly Property str_email_append_to() As String
			Get
				Return cls_global.str__pub_email_append_to
			End Get
		End Property

		Public Shared ReadOnly Property str_email_append_cc() As String
			Get
				Return cls_global.str__pub_email_append_cc
			End Get
		End Property

		Public Shared ReadOnly Property str_email_append_bcc() As String
			Get
				Return cls_global.str__pub_email_append_bcc
			End Get
		End Property

		Public Shared ReadOnly Property str_error_class__ctl() As String
			Get
				Return fnc_convert_expected_string(ConfigurationManager.AppSettings("error_class__ctl"))
			End Get
		End Property

    Public Shared ReadOnly Property str_error_class__ctl__watermark() As String
      Get
        Return fnc_convert_expected_string(ConfigurationManager.AppSettings("error_class__ctl__watermark"))
      End Get
    End Property

		Public Shared ReadOnly Property str_system_constant(ByVal str__prm_keyword As String) As String

			Get
				Dim dct As IDictionary = cls_global.dct__pub_system_constant

				If dct Is Nothing Then
					cls_utility.sub_load_meta_data()
					dct = cls_global.dct__pub_system_constant
				End If

				If dct Is Nothing Then
					Throw New Exception("Could not load system constants - table may be empty")
				End If

				If dct.Contains(str__prm_keyword) = True Then
					Return dct.Item(str__prm_keyword).ToString
				End If

				Return Nothing

			End Get

		End Property

		Public Shared ReadOnly Property dt_database_column() As DataTable
			Get

				If cls_global.dt__pub_database_column Is Nothing = True Then
					cls_utility.sub_load_meta_data()
				End If

				Return cls_global.dt__pub_database_column

			End Get
		End Property

		Public Shared ReadOnly Property dt_system_column_validation() As DataTable
			Get

				If cls_global.dt__pub_system_column_validation Is Nothing = True Then
					cls_utility.sub_load_meta_data()
				End If

				Return cls_global.dt__pub_system_column_validation
			End Get
		End Property

		Public Shared ReadOnly Property arrl_audit_exclude_table_list() As ArrayList
			Get

				If cls_global.arrl__pub_audit_exclude_table_list Is Nothing Then
					cls_utility.sub_load_meta_data()
				End If
				Return cls_global.arrl__pub_audit_exclude_table_list

			End Get
		End Property

		Public Shared ReadOnly Property arrl_audit_exclude_column_list() As ArrayList
			Get

				If cls_global.arrl__pub_audit_exclude_column_list Is Nothing Then
					cls_utility.sub_load_meta_data()
				End If
				Return cls_global.arrl__pub_audit_exclude_column_list

			End Get
		End Property

		Public Shared ReadOnly Property str_database_version() As String
			Get

				If fnc_convert_expected_string(cls_global.str__pub_database_version).Length = 0 Then
					cls_global.str__pub_database_version = cls_utility.fnc_get_database_version
				End If

				Return Replace(fnc_convert_expected_string(cls_global.str__pub_database_version), " ", "")

			End Get
		End Property

		Public Shared ReadOnly Property str_lkp_admin_exclude_list() As String
			Get
				Return Replace(fnc_convert_expected_string(ConfigurationManager.AppSettings("str_lkp_admin_exclude_list")), ", ", "', '")
			End Get
		End Property

    Public Shared ReadOnly Property str_css_saved() As String
      Get
        Return "control-saved"
      End Get
    End Property

		Public Shared ReadOnly Property str_audit_not_available() As String
			Get
				Return "Audit data is not available"
			End Get
		End Property

		Public Shared ReadOnly Property str_administration_database_password() As String
			Get
				Dim cnf_database As Configuration = WebConfigurationManager.OpenWebConfiguration("~/dir_configuration/dir_environment/dir_database")

				Return fnc_convert_expected_string(cnf_database.AppSettings.Settings("administration_database_password").Value)

			End Get

		End Property

	End Class

End Namespace

