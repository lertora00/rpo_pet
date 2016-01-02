
Imports ns_enterprise
Imports ns_enterprise.cls_utility

Imports System.Data
Imports System.Web.HttpContext

Namespace ns_enterprise

  Public Class cls_person_user

		Public Shared ReadOnly Property str_pk_person_user__default() As String
			Get
				'Return fnc_convert_expected_string(cls_global.str__pub_pk_person_user__default)
			End Get
		End Property

		' return current user but if no current user, return default user
		Public Shared ReadOnly Property str_pk_person_user_current_or_default() As String
			Get
				If cls_current_user.str_pk_person_user.Length > 0 Then Return cls_current_user.str_pk_person_user

				Return str_pk_person_user__default
			End Get
		End Property

		Private Shared str__prv_pk_person_user As String
    Public Property str_pk_person_user() As String

      Get
        Return str__prv_pk_person_user
      End Get
      Set(ByVal Value As String)
        str__prv_pk_person_user = Value
      End Set

    End Property

    Private Shared str__prv_account_confirmation_key As String
    Public Property str_account_confirmation_key() As String

      Get
        Return str__prv_account_confirmation_key
      End Get
      Set(ByVal Value As String)
        str__prv_account_confirmation_key = Value
      End Set

    End Property

    Private str__prv_fk_person As String
    Public Property str_fk_person() As String

      Get
        Return str__prv_fk_person
      End Get
      Set(ByVal Value As String)
        str__prv_fk_person = Value
      End Set

    End Property

    Private str__prv_username As String
    Public Property str_username() As String

      Get
        Return str__prv_username
      End Get
      Set(ByVal Value As String)
        str__prv_username = Value
      End Set

    End Property

		Private bln__prv_disable_sms As Boolean
		Public Property bln_disable_sms() As Boolean

			Get
				Return bln__prv_disable_sms
			End Get
			Set(ByVal Value As Boolean)
				bln__prv_disable_sms = Value
			End Set

		End Property

		Private str__prv_onscreen_name As String
		Public Property str_onscreen_name() As String

      Get
        Return str__prv_onscreen_name
      End Get
      Set(ByVal Value As String)
        str__prv_onscreen_name = Value
      End Set

    End Property

    Private str__prv_password As String
    Public Property str_password() As String

      Get
        Return str__prv_password
      End Get
      Set(ByVal Value As String)
        str__prv_password = Value
      End Set

    End Property

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

      'initialize collections
      inst_sql_collection = New cls_sql_collection()

    End Sub

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

		Public Function fnc_insert(str__prm_first_name As String, str__prm_last_name As String, str__prm_onscreen_name As String, str__prm_password As String, str__prm_email_address As String, str__prm_phone_number As String) As String

			Return Nothing

		End Function

		Public Function fnc_insert() As String

      ' validate required fields
      If str__prv_onscreen_name.Length = 0 Then
				Throw New Exception("Onscreen name is required for person_user.insert")
			End If

			If str__prv_fk_person.Length = 0 Then
				Throw New Exception("fk_person is required for person_user.insert")
			End If

			If str__prv_username.Length = 0 Then
				Throw New Exception("Username is required for person_user.insert")
			End If

			If str__prv_password.Length = 0 Then
				Throw New Exception("Password is required for person_user.insert")
			End If

      ' check to see if the user is already out there
      str__prv_pk_person_user = cls_data_access_layer.fnc_get_scaler__string("select pk_person_user from udf_person_user() where username = " & fnc_dbwrap(str__prv_username))

      ' if found, return
      If fnc_convert_expected_string(str__prv_pk_person_user).Length > 0 Then
				Throw New Exception("reg_021")
        ' cannot register with username that has been used
        ''Return str__prv_pk_person_user
        Return ""
			End If

      ' not found, create new GUID to return
      str__prv_pk_person_user = Guid.NewGuid().ToString

      ' insert new user
      Dim inst_sql As cls_sql = New cls_sql()
			inst_sql.str_operation = cls_sql.en_operation.insert.ToString
			inst_sql.str_table_name = "tbl_person_user"
			inst_sql.sub_add_column("pk_person_user", str__prv_pk_person_user)
			inst_sql.sub_add_column("fk_person", str__prv_fk_person)
			inst_sql.sub_add_column("username", str__prv_username)
			inst_sql.sub_add_column("password", str__prv_password)
			inst_sql.sub_add_column("onscreen_name", str__prv_onscreen_name)
			inst_sql.sub_add_column("account_confirmation_key", str__prv_account_confirmation_key)
			inst_sql.sub_execute()

			Return str__prv_pk_person_user

		End Function

		Public Shared Function fnc_select_list() As DataSet

      Dim str_sql As String = "select * from udf_person_user() order by onscreen_name "

      Return cls_data_access_layer.fnc_get_dataset(str_sql)

    End Function

    Public Shared Function fnc_select_list(ByVal str__prm_filter As String) As DataSet

      If str__prm_filter.Length = 0 Then
        Return fnc_select_list()
        Exit Function
      End If

      Dim str_sql As String = "select * from udf_person_user() where " & str__prm_filter

      Return cls_data_access_layer.fnc_get_dataset(str_sql)

    End Function

    Public Sub sub_select(ByVal str__prm_pk As String)

      Dim str_sql As String = "select * from udf_person_user() where pk_person_user = " & fnc_dbwrap(str__prm_pk)
      Dim dr_local As SqlClient.SqlDataReader

      dr_local = cls_data_access_layer.fnc_get_sqldatareader(str_sql)

      If dr_local.HasRows = False Then
        Throw New Exception("Could not find person: " & str__prm_pk)
      End If

      dr_local.Read()

      str__prv_username = dr_local("username").ToString
      str__prv_password = dr_local("password").ToString
      str__prv_onscreen_name = dr_local("onscreen_name").ToString
      str__prv_note = dr_local("note").ToString

    End Sub

    Public Sub sub_select_most_recent_by_person(ByVal str__prm_fk_person As String)

      Dim str_sql As String = "select top 1 * from udf_person_user() where fk_person = " & fnc_dbwrap(str__prm_fk_person) & " order by " & cls_constant.str_insert_date_column_name & " desc"
      Dim dr_local As SqlClient.SqlDataReader

      dr_local = cls_data_access_layer.fnc_get_sqldatareader(str_sql)

      If dr_local.HasRows = False Then
        Throw New Exception("Could not find person_user by person: " & str__prm_fk_person)
      End If

      dr_local.Read()

      str__prv_pk_person_user = fnc_convert_expected_string(dr_local("pk_person_user"))
      str__prv_username = fnc_convert_expected_string(dr_local("username"))
      str__prv_password = fnc_convert_expected_string(dr_local("password"))
      str__prv_onscreen_name = fnc_convert_expected_string(dr_local("onscreen_name"))
      str__prv_note = fnc_convert_expected_string(dr_local("note"))

      dr_local.Close()

    End Sub

    Public Sub sub_select(ByVal guid__prm_pk As Guid)

      sub_select(guid__prm_pk.ToString)

    End Sub

    Public Shared Sub sub_delete(ByVal str__prm_pk As String)

      'Dim inst_dynamic_sql As New cls_dynamic_sql
      'inst_dynamic_sql.sub_delete("pk_person_user", str__prm_pk)

    End Sub

    Public Shared Function fnc_get_email_address() As String

      Return fnc_convert_expected_string(cls_data_access_layer.fnc_get_scaler__string("select top 1 email_address from tbl_person_email where fk_person in (select fk_person from tbl_person_user where pk_person_user = " & fnc_dbwrap(cls_current_user.str_pk_person_user) & ")"))

    End Function

    Public Shared Function fnc_get_email_address(ByVal str__prm_pk_person_user As String) As String

      Return fnc_convert_expected_string(cls_data_access_layer.fnc_get_scaler__string("select top 1 email_address from tbl_person_email where fk_person in (select fk_person from tbl_person_user where pk_person_user = " & fnc_dbwrap(str__prm_pk_person_user) & ")"))

    End Function

    Public Shared Function fnc_select_organization_internal_hierarchy() As SqlClient.SqlDataReader

      Dim str_sql As String = "select * from select * from udf_organization_internal_hierarchy_down(" & fnc_dbwrap(cls_current_user.str_pk_person_user) & ") "

      Return cls_data_access_layer.fnc_get_datareader(str_sql)

    End Function

    Public Function fnc_get_password(ByVal str__prm_id As String) As Boolean

      str__prv_password = cls_data_access_layer.fnc_get_scaler__string("select password from udf_person_user() where username = " & fnc_dbwrap(str__prm_id))

      If str__prv_password.Length = 0 Then
        Return False
      End If

      Return True

    End Function

    Public Shared Sub sub_unlock(ByVal str__prm_pk_person As String)

      cls_data_access_layer.fnc_execute_non_query("update tbl_person_user set last_unlock_date = getdate() where pk_person_user = " & fnc_dbwrap(str__prm_pk_person))

    End Sub

    Public Shared Function fnc_get_pk_person_user(ByVal str__prm_username As String) As String

      If fnc_convert_expected_string(str__prm_username).Length > 0 Then
        Return cls_data_access_layer.fnc_get_scaler__string("select pk_person_user from tbl_person_user where username = " & fnc_dbwrap(str__prm_username))
      End If

      Return Nothing

    End Function

    Public Shared Sub sub_flush(ByVal str__prm_pk_person_user As String)

      Dim str_table_name As String = ""
      Dim str_udf_name As String = ""
      Dim str_pk_name As String = ""

      ' delete person_user_configuration (logical
      str_table_name = "tbl_person_user_configuration"
      'str_pk_name = fnc_primary_key_from_table(str_table_name)

      'inst_sql = New cls_sql
      'inst_sql.str_table_name = str_table_name
      'inst_sql.str_operation = cls_sql.en_operation.delete.ToString
      'inst_sql.sub_add_column(str_pk_name, fnc_convert_expected_string(dr(str_pk_name)))
      'inst_sql.sub_execute()

      cls_data_access_layer.sub_execute_non_query("delete from " & str_table_name & " where fk_person_user = " & fnc_dbwrap(str__prm_pk_person_user))

    End Sub

		Public Sub sub_update()

			If fnc_is_valid_guid(str__prv_pk_person_user) = False Then Exit Sub

			Dim inst_sql As cls_sql = New cls_sql()
			inst_sql.str_table_name = "tbl_person_user"
			inst_sql.str_operation = cls_sql.en_operation.update.ToString
			inst_sql.sub_add_column("pk_person_user", str__prv_pk_person_user)
			If fnc_convert_expected_string(str__prv_password).Length > 0 Then inst_sql.sub_add_column("password", str__prv_password)
			If fnc_convert_expected_string(str__prv_onscreen_name).Length > 0 Then inst_sql.sub_add_column("onscree_name", str__prv_onscreen_name)
			If fnc_convert_expected_string(str__prv_username).Length > 0 Then inst_sql.sub_add_column("username", str__prv_username)
			If IsNothing(bln_disable_sms) = False Then inst_sql.sub_add_column("disable_sms", bln__prv_disable_sms)
			inst_sql.sub_execute_with_audit(False)

		End Sub

	End Class

End Namespace
