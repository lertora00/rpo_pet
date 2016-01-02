
Imports ns_enterprise
Imports ns_enterprise.cls_utility

Imports System.Data
Imports System.Web.UI
Imports System.Data.SqlClient

Namespace ns_enterprise

  Public Class cls_current_user

    Private Shared ReadOnly Property str__prv_onscreen_name() As String
      Get
        Return str_onscreen_name
      End Get
    End Property

    Public Shared ReadOnly Property str_onscreen_name() As String

      Get
        Return fnc_get_person_user_value("onscreen_name")
      End Get

    End Property

    Private Shared ReadOnly Property str__prv_username() As String
      Get
        Return str_username
      End Get
    End Property

    Public Shared ReadOnly Property str_username() As String

      Get
        Return fnc_get_person_user_value("username")
      End Get

    End Property

    Private Shared ReadOnly Property str__prv_email_address() As String
      Get
        Return str_email_address
      End Get
    End Property

    Public Shared ReadOnly Property str_email_address() As String
      Get
        Return fnc_get_person_user_value("email_address")
      End Get
    End Property

    Private Shared ReadOnly Property str__prv_pk_person() As String
      Get
        Return str_pk_person
      End Get
    End Property

    Public Shared ReadOnly Property str_pk_person() As String
      Get
        Return fnc_get_person_user_value("pk_person")
      End Get
    End Property

    Private Shared ReadOnly Property str__prv_first_name() As String
      Get
        Return str_first_name
      End Get
    End Property

    Public Shared ReadOnly Property str_first_name() As String
      Get
        Return fnc_get_person_user_value("first_name")
      End Get
    End Property
		Public Shared ReadOnly Property str_last_name() As String
			Get
				Return fnc_get_person_user_value("last_name")
			End Get
		End Property

		Public Shared ReadOnly Property str_phone_number() As String
			Get
				Return fnc_get_person_user_value("phone_number")
			End Get
		End Property

		Public Shared ReadOnly Property str_pet_name() As String
			Get
				Return fnc_get_person_user_value("pet_name")
			End Get
		End Property
		Public Shared ReadOnly Property str_pet_tagline() As String
			Get
				Return fnc_get_person_user_value("pet_tagline")
			End Get
		End Property

		Private Shared ReadOnly Property str__prv_name__last_first() As String
			Get
				Return str_name__last_first
			End Get
		End Property

		Public Shared ReadOnly Property str_name__last_first() As String
      Get
        Return fnc_get_person_user_value("name__last_first")
      End Get
    End Property

    Private Shared ReadOnly Property str__prv_name__first_last() As String
      Get
        Return str_name__first_last
      End Get
    End Property

    Public Shared ReadOnly Property str_name__first_last() As String
      Get
        Return fnc_get_person_user_value("name__first_last")
      End Get
    End Property

    Private Shared ReadOnly Property str__prv_date_of_birth() As String
      Get
        Return str_date_of_birth
      End Get
    End Property

    Public Shared ReadOnly Property str_date_of_birth() As String
      Get
        Return fnc_get_person_user_value("date_of_birth")
      End Get
    End Property

    Private Shared ReadOnly Property str__prv_pk_person_user() As String
      Get
        Return str_pk_person_user
      End Get
    End Property

    Public Shared ReadOnly Property str_pk_person_user() As String

      Get
        Return cls_variable.str_pk_person_user()
      End Get

    End Property

    ' to be used inside this method - versus using public property
    Private Shared ReadOnly Property bln__prv_authenticated() As Boolean

      Get
        Return bln_authenticated
      End Get

    End Property

    Public Shared ReadOnly Property bln_authenticated() As Boolean

      Get
        Return cls_variable.bln_authenticated()
      End Get

    End Property

    ' get a value from person user in process store (username, first_name, etc.)
    Public Shared Function fnc_get_person_user_value(ByVal str__prm_key As String) As String

      If bln_authenticated = False Then Return ("")

      Dim hsh As Hashtable = Nothing
      Dim vws As StateBag = Nothing

      hsh = cls_variable.hsh_person_user(str__prv_pk_person_user)

      ' could not retrieve - try to create
      If hsh Is Nothing Then
        sub_persist_current_user()
        hsh = cls_variable.hsh_person_user(str__prv_pk_person_user)
        If hsh Is Nothing Then Throw New Exception("re-persist user data failed")
      End If

      If hsh.ContainsKey(str__prm_key) = True Then
        Return cls_utility.fnc_convert_expected_string(hsh.Item(str__prm_key))
      End If

      Return ""

    End Function

    Public Shared Sub fnc_clear_cache()

      If bln_authenticated = False Then
        Throw New Exception("Must be logged in to clear current user cache")
      End If

      sub_clear_cache__current_user()
      sub_clear_cache__application_role()
      sub_clear_cache__system_role()
      sub_clear_cache__configuration()

    End Sub

    Public Shared Sub sub_clear_cache__current_user()

      If bln_authenticated = False Then
        Throw New Exception("Must be logged in to clear current user person_user cache")
      End If

      cls_variable.sub_remove_hsh_person_user(str_pk_person_user)

    End Sub

    Public Shared Sub sub_clear_cache__configuration()

      If bln_authenticated = False Then
        Throw New Exception("Must be logged in to clear current user configuration cache")
      End If

      cls_variable.sub_remove_hsh_person_user_configuration(str_pk_person_user)

    End Sub

    Public Shared Sub sub_clear_cache__application_role()

      If bln_authenticated = False Then
        Throw New Exception("Must be logged in to clear current user application_role cache")
      End If

      cls_variable.sub_remove_hsh_person_user_application_role(str_pk_person_user)

    End Sub
    Public Shared Sub sub_clear_cache__system_role()

      If bln_authenticated = False Then
        Throw New Exception("Must be logged in to clear current user system_role cache")
      End If

      cls_variable.sub_remove_hsh_person_user_system_role(str_pk_person_user)

    End Sub

    ' sets current users configuration
    Public Shared Sub sub_set_person_user_configuration(ByVal str__prm_keyword As String, ByVal str__prm_value As String)

      If bln_authenticated = False Then
        Throw New Exception("Must be logged in to set person user configuration")
      End If

      ' TODO this should be stored procedure or udf with parms
      Dim dr_count As SqlClient.SqlDataReader = cls_data_access_layer.fnc_get_datareader("select pk_person_user_configuration from tbl_person_user_configuration where fk_person_user = " & cls_utility.fnc_dbwrap(str_pk_person_user) & " and keyword = " & cls_utility.fnc_dbwrap(str__prm_keyword))

      Dim int_count As Int32 = 0
      Dim str_pk_person_user_configuration As String = ""

      If dr_count.HasRows = True Then
        Do While dr_count.Read
          int_count = int_count + 1
          str_pk_person_user_configuration = fnc_convert_expected_string(dr_count("pk_person_user_configuration"))
        Loop
      End If

      ' this should never happen as there should be unique constraint on user/keyword
      If int_count > 1 Then
        Throw New Exception("More than one configuration keyword found for user/keyword: " & str_pk_person_user & " : " & str__prm_keyword)
      End If

      Dim inst_sql As New cls_sql

      If int_count = 1 Then
        inst_sql.str_operation = cls_sql.en_operation.update.ToString
        inst_sql.sub_add_column("pk_person_user_configuration", str_pk_person_user_configuration)
      Else
        ' count is zero, add the new keyword/value for the user
        inst_sql.str_operation = cls_sql.en_operation.insert.ToString
        inst_sql.sub_add_column("fk_person_user", str_pk_person_user)
      End If

      inst_sql.str_table_name = "tbl_person_user_configuration"
      inst_sql.sub_add_column("keyword", str__prm_keyword)
      inst_sql.sub_add_column("value", str__prm_value)
      inst_sql.sub_execute_with_audit(False)

      sub_persist_configuration()

    End Sub

    Public Shared Function fnc_get_person_user_configuration(ByVal str__prm_keyword As String) As String

      If fnc_is_valid_guid(str_pk_person_user) = False Then
        Return Nothing
        Throw New Exception("Must supply person user to get configuration")
      End If

      If fnc_convert_expected_string(str__prm_keyword).Length = 0 Then
        Throw New Exception("Keyword not provided to get configuration")
      End If

      ' check to see if person users config cache exists
      If cls_variable.hsh_person_user_configuration(str_pk_person_user) Is Nothing Then
        sub_persist_configuration()
      End If

      ' if refresh didn't create it, return nothing
      If cls_variable.hsh_person_user_configuration(str_pk_person_user) Is Nothing Then
        Return Nothing
      End If

      Dim hsh As Hashtable = cls_variable.hsh_person_user_configuration(str_pk_person_user)

      If hsh.ContainsKey(str__prm_keyword) = True Then
        Return hsh(str__prm_keyword)
      End If

      Return Nothing

    End Function

    Public Shared Function fnc_get_administrator_level() As Int32

      Dim str As String = fnc_get_person_user_configuration("int_administrator_level")

      If str Is Nothing Then
        Return -1
      End If

      Return fnc_convert_expected_int32(str)

    End Function

    Public Shared Sub sub_set_administrator_level()

      Dim int_administrator_level As Int32 = 0

      If cls_current_user.bln_authenticated = True AndAlso fnc_is_in_system_role("administrator") = True Then
        int_administrator_level = cls_data_access_layer.fnc_get_scaler__number("select system_role_level from tbl_person_user_system_role where fk_person_user = " & cls_utility.fnc_dbwrap(str_pk_person_user) & " and fk_lkp_system_role = (select pk_lkp_system_role from tbl_lkp_system_role where system_name='administrator')")
        sub_set_person_user_configuration("int_administrator_level", int_administrator_level)
      End If

    End Sub

    Public Shared Function fnc_get_system_role_list() As ArrayList

      If bln_authenticated = False Then
        Throw New Exception("Must be logged in to get system_role list")
      End If

      Dim arl_system_role As ArrayList
      arl_system_role = cls_variable.arl_person_user_system_role(str_pk_person_user)

      If arl_system_role Is Nothing Then
        sub_persist_system_role()
        ' should now be available after persist
        arl_system_role = cls_variable.arl_person_user_system_role(str_pk_person_user)
        If arl_system_role Is Nothing Then
          Throw New Exception("Could not persist system_role.  There is always at least one system_role")
        End If
      End If

      Return arl_system_role

    End Function

    Public Shared Function fnc_get_application_role_list() As ArrayList

      If bln_authenticated = False Then
        Throw New Exception("Must be logged in to get application_role list")
      End If

      Dim arl_application_role As ArrayList
      arl_application_role = cls_variable.arl_person_user_application_role(str_pk_person_user)

      If arl_application_role Is Nothing Then
        sub_persist_application_role()
        ' should now be available after persist
        arl_application_role = cls_variable.arl_person_user_application_role(str_pk_person_user)
        If arl_application_role Is Nothing Then
          Throw New Exception("Could not persist application_role.  There is always at least one application_role")
        End If
      End If

      Return arl_application_role

    End Function

    Public Shared Function fnc_get_configuration_list() As Hashtable

      If bln_authenticated = False Then
        Throw New Exception("Must be logged in to get configuration list")
      End If

      Dim hsh As Hashtable
      hsh = cls_variable.hsh_person_user_configuration(str_pk_person_user)

      If hsh Is Nothing Then
        sub_persist_configuration()
        ' should now be available after persist
        hsh = cls_variable.hsh_person_user_configuration(str_pk_person_user)
        If hsh Is Nothing Then
          Throw New Exception("Could not persist configuration.  There is always at least one configuration")
        End If
      End If

      Return hsh

    End Function

    Public Shared Function fnc_is_in_system_role(ByVal str__prm_system_role As String) As Boolean

      If bln_authenticated = False Then
        Throw New Exception("Must be logged in to check system_role")
      End If

      Dim arl_system_role As ArrayList
      arl_system_role = cls_variable.arl_person_user_system_role(str_pk_person_user)

      If arl_system_role Is Nothing Then
        sub_persist_system_role()
        ' should now be available after persist
        arl_system_role = cls_variable.arl_person_user_system_role(str_pk_person_user)
        If arl_system_role Is Nothing Then
          Throw New Exception("Could not persist system_role.  There is always at least one system_role")
        End If
      End If

      If arl_system_role.Contains(str__prm_system_role) = True Then
        Return True
      End If

      Return False

    End Function

    Public Shared Function fnc_is_in_application_role(ByVal str__prm_application_role As String) As Boolean

      If bln_authenticated = False Then
        Throw New Exception("Must be logged in to check application_role")
      End If

      Dim arl_application_role As ArrayList
      arl_application_role = cls_variable.arl_person_user_application_role(str_pk_person_user)

      If arl_application_role Is Nothing Then
        sub_persist_application_role()
        ' should now be available after persist
        arl_application_role = cls_variable.arl_person_user_application_role(str_pk_person_user)
        If arl_application_role Is Nothing Then
          Throw New Exception("Could not persist application_role.  There is always at least one application_role")
        End If
      End If

      If arl_application_role.Contains(str__prm_application_role) = True Then
        Return True
      End If

      Return False

    End Function

    Public Shared Sub sub_persist_current_user__all()

      If bln_authenticated = False Then
        Throw New Exception("Must be logged in to persist user")
      End If

      sub_persist_current_user__all(str_pk_person_user)

    End Sub

    Public Shared Sub sub_persist_current_user__all(ByVal str__prm_pk_person_user As String)

      If fnc_is_valid_guid(str__prm_pk_person_user) = False Then
        Throw New Exception("Invalid user to persist user")
      End If

      sub_persist_current_user(str__prm_pk_person_user)
      sub_persist_application_role(str__prm_pk_person_user)
      sub_persist_system_role(str__prm_pk_person_user)
      sub_persist_configuration(str__prm_pk_person_user)

    End Sub

    Public Shared Sub sub_persist_current_user()

      If bln_authenticated = False Then
        Throw New Exception("Must be logged in to persist current user")
      End If

      sub_persist_current_user(str_pk_person_user)

    End Sub

    Public Shared Sub sub_persist_current_user(ByVal str__prm_pk_person_user As String)

      If fnc_is_valid_guid(str__prm_pk_person_user) = False Then
        Throw New Exception("Invalid user to persist user")
      End If

      ' TODO - convert to generic - 07/2010
      ' this class shouldn't know anything about datareaders.  Should convert to a generic data collection
      '		 (dataset may be ok - but generic object that acts like datareader / dataset would be better
      Dim dr_security As SqlDataReader = cls_data_access_layer.fnc_get_datareader("select * from udf_person_user__persist(" & cls_utility.fnc_dbwrap(str__prm_pk_person_user) & ")")

      If dr_security.HasRows = False Then
        Throw New Exception("Could not retrieve person_user")
      End If

      dr_security.Read()

      Dim hsh As New Hashtable

      hsh.Add("pk_person", fnc_convert_expected_string(dr_security("fk_person")))
      hsh.Add("first_name", fnc_convert_expected_string(dr_security("first_name")))
      hsh.Add("last_name", fnc_convert_expected_string(dr_security("last_name")))
      hsh.Add("name__last_first", fnc_convert_expected_string(dr_security("name__last_first")))
      hsh.Add("name__first_last", fnc_convert_expected_string(dr_security("name__first_last")))
      'hsh.Add("date_of_birth", fnc_convert_expected_datetime__string(dr_security("date_of_birth")))
      hsh.Add("onscreen_name", fnc_convert_expected_string(dr_security("onscreen_name")))
      hsh.Add("username", fnc_convert_expected_string(dr_security("username")))
      hsh.Add("email_address", fnc_convert_expected_string(dr_security("email_address")))
			hsh.Add("email_address__concatinate", fnc_convert_expected_string(dr_security("email_address__concatinate")))
			hsh.Add("phone_number", fnc_convert_expected_string(dr_security("phone_number")))
			hsh.Add("pet_name", fnc_convert_expected_string(dr_security("pet_name")))
			hsh.Add("pet_tagline", fnc_convert_expected_string(dr_security("pet_tagline")))

			cls_variable.hsh_person_user(str_pk_person_user) = hsh

    End Sub

    Public Shared Sub sub_persist_system_role()

      If bln_authenticated = False Then
        Throw New Exception("sub_persist_system_role: not authenticated")
      End If

      sub_persist_system_role(str_pk_person_user)

    End Sub

    Public Shared Sub sub_persist_system_role(ByVal str__prm_pk_person_user As String)

      If fnc_is_valid_guid(str__prm_pk_person_user) = False Then
        Throw New Exception("Invalid user to persist system role")
      End If

      If bln_authenticated = True Then
        sub_clear_cache__system_role()
      End If

      Dim dr As SqlDataReader = cls_data_access_layer.fnc_get_datareader("select system_role_system_name from udf_person_user_system_role() where fk_person_user = " & cls_utility.fnc_dbwrap(str__prm_pk_person_user))
      Dim arl As New ArrayList

      ' everyone that has logged is at least a user.  Also need at least one role so the object is not empty
      arl.Add("system_user")

      Do While dr.Read
        arl.Add(fnc_convert_expected_string(dr("system_role_system_name")))
      Loop

      cls_variable.arl_person_user_system_role(str_pk_person_user) = arl

      dr.Close()
      dr = Nothing

    End Sub

    Public Shared Sub sub_persist_application_role()

      If bln_authenticated = False Then
        Throw New Exception("sub_persist_application_role: not authenticated")
      End If

      sub_persist_application_role(str_pk_person_user)

    End Sub

    Public Shared Sub sub_persist_application_role(ByVal str__prm_pk_person_user As String)

      If fnc_is_valid_guid(str__prm_pk_person_user) = False Then
        Throw New Exception("Invalid user to persist application role")
      End If

      If bln_authenticated = True Then
        sub_clear_cache__application_role()
      End If

      Dim dr As SqlDataReader = cls_data_access_layer.fnc_get_datareader("select application_role_system_name from udf_person_user_application_role() where fk_person_user = " & cls_utility.fnc_dbwrap(str__prm_pk_person_user))
      Dim arl As New ArrayList

      ' everyone that has logged is at least a user.  Also need at least one role so the object is not empty
      arl.Add("application_user")

      Do While dr.Read
        arl.Add(fnc_convert_expected_string(dr("application_role_system_name")))
      Loop

      cls_variable.arl_person_user_application_role(str_pk_person_user) = arl

      dr.Close()
      dr = Nothing

    End Sub

    Public Shared Sub sub_persist_configuration()

      If bln_authenticated = False Then
        Throw New Exception("Refresh person user config cache requires logged in user")
      End If

      sub_persist_configuration(str_pk_person_user)

    End Sub

    Public Shared Sub sub_persist_configuration(ByVal str__prm_pk_person_user As String)

      If fnc_is_valid_guid(str__prm_pk_person_user) = False Then
        Throw New Exception("Invalid user to persist configuration")
      End If

      If bln_authenticated = True Then
        sub_clear_cache__current_user()
      End If

      Dim str_sql As String = ""
      Dim dr As SqlDataReader = Nothing
      Dim hsh As New Hashtable

      hsh.Add("[must_have_at_least_one_key]", "[must_have_at_least_one_value]")

      str_sql = "select keyword, value from tbl_person_user_configuration where fk_person_user = " & cls_utility.fnc_dbwrap(str__prm_pk_person_user) & " and " & cls_constant.str_active_flag_column_name & " = 1"

      dr = cls_data_access_layer.fnc_get_datareader(str_sql)

      If dr.HasRows = False Then
        GoTo no_configuration
      End If

      Do While dr.Read
        hsh.Add(fnc_convert_expected_string(dr("keyword")), fnc_convert_expected_string(dr("value")))
      Loop

no_configuration:

      cls_variable.hsh_person_user_configuration(str_pk_person_user) = hsh

      dr.Close()
      dr = Nothing

    End Sub

    Public Shared Function fnc_get_insert_user_field_list() As String

      If bln_authenticated = True Then
        Return ", " & cls_constant.str_insert_user_column_name_pk & ", " & cls_constant.str_insert_user_column_name
      End If

      Return ""

    End Function

    Public Shared Function fnc_get_insert_user_value_list() As String

      If bln_authenticated = True Then
        Return ", " & cls_utility.fnc_dbwrap(str_pk_person_user) & ", " & cls_utility.fnc_dbwrap(str_onscreen_name)
      End If

      Return ""

    End Function

  End Class

End Namespace