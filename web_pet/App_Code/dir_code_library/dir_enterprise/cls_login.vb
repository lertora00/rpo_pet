Imports ns_enterprise.cls_utility
Imports ns_enterprise.cls_friendly_message

Imports System.Data
Imports System.Data.SqlClient

Namespace ns_enterprise

	Public Class cls_login

#Region "Properties"

		' fk_person_user property
		Private str__prv_fk_person_user As String
		Public Property str_fk_person_user() As String

			Get
				Return str__prv_fk_person_user
			End Get
			Set(ByVal Value As String)
				str__prv_fk_person_user = Value
			End Set

		End Property

		' first_login_flag property
		Private bln__prv_first_login_flag As Boolean
		Public Property bln_first_login_flag() As Boolean

			Get
				Return bln__prv_first_login_flag
			End Get
			Set(ByVal Value As Boolean)
				bln__prv_first_login_flag = Value
			End Set
		End Property

		' proxy_flag property
		Private bln__prv_proxy_flag As Boolean
		Public Property bln_proxy_flag() As Boolean

			Get
				Return bln__prv_proxy_flag
			End Get
			Set(ByVal Value As Boolean)
				bln__prv_proxy_flag = Value
			End Set

		End Property

		' proxy_prior_user property
		Private str__prv_proxy_prior_user As String
		Public Property str_proxy_prior_user() As String

			Get
				Return str__prv_proxy_prior_user
			End Get
			Set(ByVal Value As String)
				str__prv_proxy_prior_user = Value
			End Set

		End Property

		' username property
		Private str__prv_username As String
		Public Property str_username() As String

			Get
				Return str__prv_username
			End Get
			Set(ByVal Value As String)
				str__prv_username = Value
			End Set

		End Property

		' password property
		Private str__prv_password As String
		Public Property str_password() As String

			Get
				Return str__prv_password
			End Get
			Set(ByVal Value As String)
				str__prv_password = Value
			End Set

		End Property

		' url property
		Private str__prv_url As String
		Public Property str_url() As String

			Get
				Return str__prv_url
			End Get
			Set(ByVal Value As String)
				str__prv_url = Value
			End Set

		End Property

		' ip_address property
		Private str__prv_ip_address As String
		Public Property str_ip_address() As String

			Get
				Return str__prv_ip_address
			End Get
			Set(ByVal Value As String)
				str__prv_ip_address = Value
			End Set

		End Property

		' login_limit_time_window_minute__valid property
		Private ReadOnly int__prv_login_limit_time_window_minute__valid As String
		Public ReadOnly Property int_login_limit_time_window_minute__valid() As Int32

			Get
				Return fnc_convert_expected_int32(cls_constant.str_system_constant("login_limit_time_window_minute__valid"))
			End Get

		End Property

		' login_limit_time_window_minute__invalid property
		Private ReadOnly int__prv_login_limit_time_window_minute__invalid As String
		Public ReadOnly Property int_login_limit_time_window_minute__invalid() As Int32

			Get
				Return fnc_convert_expected_int32(cls_constant.str_system_constant("login_limit_time_window_minute__invalid"))
			End Get

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

		Private inst__prv_log_security As cls_log_security

#End Region

#Region "Methods"

		Public Sub New()

			str__prv_url = cls_variable.str_url
			' TODO - need to lookup ServerVariable for IP ADDRESS and test it
			str__prv_ip_address = cls_variable.str_server_variable("REMOTE_ADDR")
			inst__prv_log_security = New cls_log_security

		End Sub

		Public Function fnc_login() As Boolean

			If fnc_convert_expected_string(cls_constant.str_system_constant("str_system_locked_message_code")).Length > 0 Then
				inst__prv_friendly_message_collection.Add(New cls_friendly_message(cls_constant.str_system_constant("str_system_locked_message_code"), cls_friendly_message.en_message_category.system_message, Me))
				inst__prv_log_security.sub_log_security_event(False, False, inst__prv_friendly_message_collection.Item(inst__prv_friendly_message_collection.Count - 1), "cls_login.fnc_login login: system locked")
				Return False
			End If

			Dim bln_logged_in As Boolean = True
			Dim bln_authenticated As Boolean = Nothing
			Dim bln_authorizad As Boolean = Nothing

			inst__prv_log_security.sub_log_security_event(False, False, "Initial login attempt")

			inst__prv_log_security.sub_log_security_parameter("username", str__prv_username)
			' TODO password may be encrypted but if so, likely only on invalid attempt.  Admin will need to see what they are trying to use if not working.
			inst__prv_log_security.sub_log_security_parameter("password", str__prv_password)
			inst__prv_log_security.sub_log_security_parameter("url", str__prv_url)
			inst__prv_log_security.sub_log_security_parameter("ip_address", str__prv_ip_address)
			inst__prv_log_security.sub_log_security_parameter("proxy_flag", bln__prv_proxy_flag)
			inst__prv_log_security.sub_log_security_parameter("proxy_prior_user", str__prv_proxy_prior_user)

			'sub_log_browser_capability(inst__prv_log_security)

			' TODO log more parameters
			''inst__prv_log_security.sub_log_security_parameter(" our cookie ", 1)

			Dim bln_is_valid As Boolean = False

			' check data input
			bln_is_valid = fnc_validate_input()
			If bln_is_valid = False Then
				bln_logged_in = False
				GoTo end_function
			End If
			inst__prv_log_security.sub_log_security_event(bln_is_valid, False, "validate_input complete")

			If bln_is_valid = True Then
				' authenticate user
				bln_is_valid = fnc_validate_user()
				If bln_is_valid = False Then
					bln_logged_in = False
					bln_authenticated = False
					GoTo end_function
				Else
					bln_authenticated = True
				End If
				inst__prv_log_security.sub_log_security_event(bln_is_valid, False, "validate_user complete")
			End If

			If fnc_system_role("administrator") = True Then
				bln_authorizad = True
				GoTo end_function
			End If

			' TODO - do we want to do an eligibility check if they had any issues prior - especially invalid login??  We could expose eligibility data on any user with/without password
			' authorize (eligible?)
			If bln_is_valid = True Then
				bln_is_valid = fnc_validate_eligibility()
				If bln_is_valid = False Then
					bln_logged_in = False
					bln_authorizad = False
					GoTo end_function
				Else
					bln_authorizad = True
				End If
				inst__prv_log_security.sub_log_security_event(bln_is_valid, False, "eligibility_check complete")
			End If

end_function:

			If bln_logged_in = True Then
				inst__prv_log_security.sub_log_security_event(bln_is_valid, False, "Successful Login complete")
				inst__prv_log_security.str_fk_person_user__authenticated = str__prv_fk_person_user

				' updating first login date to simplify reporting
				If bln__prv_first_login_flag = True Then
					cls_data_access_layer.sub_execute_non_query("update tbl_person_user set first_login_date = getdate() where pk_person_user = " & fnc_dbwrap(str__prv_fk_person_user))
				End If

			Else
				inst__prv_log_security.sub_log_security_event(bln_is_valid, False, "Failed Login attempt")
			End If

			' Log results of login attempt
			inst__prv_log_security.str_fk_person_user__apply_to = str__prv_fk_person_user
			inst__prv_log_security.str_username__if_not_fk = str__prv_username
			inst__prv_log_security.sub_log_security(cls_log_security.en_log_security_type.Login, bln_logged_in, bln_authenticated, bln_authorizad)
			inst__prv_log_security.sub_log_execute()

			Return bln_logged_in

		End Function

		Private inst__prv_friendly_message_collection As New cls_friendly_message_collection()
		Public ReadOnly Property inst_friendly_message_collection() As cls_friendly_message_collection
			Get
				Return inst__prv_friendly_message_collection
			End Get
		End Property

		Public Function fnc_validate_input() As Boolean

			Dim is_valid As Boolean = True

			' this check has been updated to use new cls_friendly_message
			If fnc_validate_username(str__prv_username) = False Then
				inst__prv_friendly_message_collection.Add(New cls_friendly_message("lgn_001", en_message_category.validation_condition, Me))
				inst__prv_log_security.sub_log_security_event(False, True, inst__prv_friendly_message_collection.Item(inst__prv_friendly_message_collection.Count - 1), "cls_login.validate_input login: invalid username format")
				is_valid = False
			End If

			If fnc_validate_password(str__prv_password) = False Then
				inst__prv_friendly_message_collection.Add(New cls_friendly_message("lgn_002", en_message_category.validation_condition, Me))
				inst__prv_log_security.sub_log_security_event(False, True, inst__prv_friendly_message_collection.Item(inst__prv_friendly_message_collection.Count - 1), "cls_login.validate_input login: invalid password format")
				is_valid = False
			End If

			' other validation checks

			' could return a collection of error messages which if empty would mean validation passed, etc.
			Return is_valid

		End Function

		Public Function fnc_validate_user() As Boolean

			Dim is_valid As Boolean = True

			Dim dr As SqlDataReader = cls_data_access_layer.fnc_get_datareader("select *, dbo.udf_login_count(" & fnc_dbwrap(str__prv_username) & ", " & int_login_limit_time_window_minute__valid & ", 1) as int_login_success, dbo.udf_login_count(" & fnc_dbwrap(str__prv_username) & ", " & int_login_limit_time_window_minute__valid & ", 0) as int_login_failure from udf_person_user_login() where username = " & fnc_dbwrap(str__prv_username))

			' did not find user by username/password (after regex cleaned input)
			If dr.HasRows = False Then
				inst__prv_friendly_message_collection.Add(New cls_friendly_message("lgn_003", en_message_category.validation_condition, Me))
				inst__prv_log_security.sub_log_security_event(False, True, inst__prv_friendly_message_collection.Item(inst__prv_friendly_message_collection.Count - 1), "cls_login.fnc_validate_user: username not found")
				is_valid = False
				GoTo check_complete
			End If

			dr.Read()

			If fnc_convert_expected_int32(dr("int_login_failure")) >= fnc_convert_expected_int32(cls_constant.str_system_constant("login_limit_count__invalid")) Then
				inst__prv_friendly_message_collection.Add(New cls_friendly_message("lgn_004", en_message_category.validation_condition, Me))
				inst__prv_log_security.sub_log_security_event(False, True, inst__prv_friendly_message_collection.Item(inst__prv_friendly_message_collection.Count - 1), "cls_login.fnc_validate_user: invalid login lock")
				is_valid = False
				GoTo check_complete
			End If

			If fnc_convert_expected_bit(dr("account_confirmation_flag")) = False AndAlso fnc_convert_expected_bit(dr("int_administrator")) = False Then
				' administrators cannot login in without confirm flag.  need to skip this check if admin
				' added administrator flag to udf login so this code can work again.  pl.
				inst__prv_friendly_message_collection.Add(New cls_friendly_message("lgn_005", en_message_category.validation_condition, Me))
				inst__prv_log_security.sub_log_security_event(False, True, inst__prv_friendly_message_collection.Item(inst__prv_friendly_message_collection.Count - 1), "cls_login.fnc_validate_user: not yet activated")
				is_valid = False
				GoTo check_complete
			End If

			' valid user but invalid password
			If fnc_convert_expected_string(dr("password")) <> str__prv_password Then
				inst__prv_friendly_message_collection.Add(New cls_friendly_message("lgn_006", en_message_category.validation_condition, Me))
				inst__prv_log_security.sub_log_security_event(False, True, inst__prv_friendly_message_collection.Item(inst__prv_friendly_message_collection.Count - 1), "cls_login.fnc_validate_user: incorrect password")
				is_valid = False
				GoTo check_complete
			End If

			If fnc_convert_expected_int32(dr("int_login_success")) >= fnc_convert_expected_int32(cls_constant.str_system_constant("login_limit_count__valid")) Then
				inst__prv_friendly_message_collection.Add(New cls_friendly_message("lgn_007", en_message_category.validation_condition, Me))
				inst__prv_log_security.sub_log_security_event(False, True, inst__prv_friendly_message_collection.Item(inst__prv_friendly_message_collection.Count - 1), "cls_login.fnc_validate_user: valid login lock")
				is_valid = False
			End If

			bln__prv_first_login_flag = False
			If fnc_convert_expected_string(dr("first_login_date")).Length = 0 Then
				bln__prv_first_login_flag = True
			End If

			' valid user/password
			str_fk_person_user = fnc_convert_expected_string(dr("pk_person_user"))

check_complete:
			dr.Close()

			Return is_valid

		End Function

		Public Function fnc_validate_eligibility() As Boolean

			Dim is_valid As Boolean = True

			' todo - this is probably all one SQL above but need to move all this code into a new project
			Dim dr As SqlClient.SqlDataReader = cls_data_access_layer.fnc_get_datareader("select eligibility_start, eligibility_end from tbl_eligibility where fk_person_user = " & fnc_dbwrap(str_fk_person_user))

			' did not find eligiblity data
			If dr.HasRows = False Then
				inst__prv_friendly_message_collection.Add(New cls_friendly_message("lgn_008", en_message_category.validation_condition, Me))
				inst__prv_log_security.sub_log_security_event(False, True, inst__prv_friendly_message_collection.Item(inst__prv_friendly_message_collection.Count - 1), "cls_login.fnc_validate_eligibility: eligibility_check failed")
				is_valid = False
				GoTo check_complete
			End If

			' force return so eligibility date checks are not performed. This might be added later but for now bypass.
			Return True

			dr.Read()

			' not yet eligible
			If fnc_convert_expected_datetime(dr("eligibility_start")) > Now Then
				inst__prv_friendly_message_collection.Add(New cls_friendly_message("lgn_009", en_message_category.validation_condition, Me))
				inst__prv_log_security.sub_log_security_event(False, True, inst__prv_friendly_message_collection.Item(inst__prv_friendly_message_collection.Count - 1), "cls_login.fnc_validate_eligibility: before start date")
				is_valid = False
			End If

			' eligibility past
			If fnc_convert_expected_datetime(dr("eligibility_end")) < Now Then
				inst__prv_friendly_message_collection.Add(New cls_friendly_message("lgn_010", en_message_category.validation_condition, Me))
				inst__prv_log_security.sub_log_security_event(False, True, inst__prv_friendly_message_collection.Item(inst__prv_friendly_message_collection.Count - 1), "cls_login.fnc_validate_eligibility: after end date")
				is_valid = False
			End If

check_complete:
			dr.Close()

			Return is_valid

		End Function

		Private Function fnc_validate_username(ByVal str__prm As String) As Boolean

			Return cls_utility.fnc_validate_regular_expression(str__prm, cls_constant.str_system_constant("regex_email"))

		End Function

		Private Function fnc_validate_password(ByVal str__prm As String) As Boolean

			Return cls_utility.fnc_validate_regular_expression(str__prm, cls_constant.str_system_constant("regex_password"))

		End Function

		Public Function fnc_get_password(ByVal str__prm_email_address As String) As String

			Return cls_data_access_layer.fnc_get_scaler__string("select password from tbl_person_user where username = " & fnc_dbwrap(str__prm_email_address))

		End Function

		Private Function fnc_system_role(ByVal str__prm_system_role As String) As Boolean

			Dim dr As SqlDataReader = cls_data_access_layer.fnc_get_datareader("select system_role_system_name from udf_person_user_system_role() where fk_person_user = " & fnc_dbwrap(str__prv_fk_person_user))
			Dim arl_system_role As New ArrayList

			' everyone that has logged is at least a user
			arl_system_role.Add("authenticated_user")

			Do While dr.Read
				arl_system_role.Add(fnc_convert_expected_string(dr("system_role_system_name")))
			Loop

			If arl_system_role.Contains(str__prm_system_role) = True Then
				Return True
			End If

			Return False

		End Function

		Public Shared Function fnc_logout(ByVal bln__prm_timeout As Boolean, ByVal bln__prm_killed As Boolean) As Boolean

			Dim inst_login As New cls_login()
			Dim str_logout_note As String = ""

			inst_login.inst__prv_log_security.str_fk_person_user__apply_to = cls_current_user.str_pk_person_user
			inst_login.inst__prv_log_security.str_username__if_not_fk = cls_current_user.str_username
			inst_login.inst__prv_log_security.str_fk_person_user__authenticated = cls_current_user.str_pk_person_user
			inst_login.inst__prv_log_security.sub_log_security(cls_log_security.en_log_security_type.Logout, True, False, False)

			str_logout_note = "Logout attempt"

			If bln__prm_timeout = True Then
				str_logout_note = "Forced logout via Timeout"
				If cls_constant.str_system_constant("int_show_logout__timeout") = 1 Then
					cls_utility.sub_set_cookie("str_logout_message", cls_friendly_message.fnc_get_friendly_message("LOG_101", "Session logged out due to Inactivity Timeout", True))
				End If
			End If

			If bln__prm_killed = True Then
				str_logout_note = "Forced logout via Kill"
				'cls_session.sub_kill()
			End If

			inst_login.inst__prv_log_security.sub_log_security_event(False, False, str_logout_note)

			inst_login.inst__prv_log_security.sub_log_security_parameter("onscreen_name", cls_current_user.str_onscreen_name)
			inst_login.inst__prv_log_security.sub_log_security_parameter("str_pk_person_user", cls_current_user.str_pk_person_user)
			inst_login.inst__prv_log_security.sub_log_security_parameter("username", cls_current_user.str_username)

			If cls_current_user.str_pk_person_user.Length > 0 Then
				'cls_session.sub_logout()
			End If

			inst_login.inst__prv_log_security.sub_log_security_event(True, False, "Logout complete")
			inst_login.inst__prv_log_security.sub_log_execute()

			If cls_current_user.str_pk_person_user.Length > 0 Then
				cls_current_user.fnc_clear_cache()
			End If

			cls_session.sub_abandon()

			System.Web.Security.FormsAuthentication.SignOut()

			If bln__prm_killed = True And cls_constant.str_system_constant("int_show_logout__kill") = 1 Then
				cls_utility.sub_set_cookie("str_logout_message", cls_friendly_message.fnc_get_friendly_message("LOG_101", "Session logged out due to Administrator Kill", True))
			End If

			Return True

		End Function

		Public Shared Function fnc_logout(ByVal bln__prm_timeout As Boolean) As Boolean

			Return fnc_logout(bln__prm_timeout, False)

		End Function

		Public Shared Function fnc_logout() As Boolean

			Return fnc_logout(False, False)

		End Function

#End Region

	End Class

End Namespace
