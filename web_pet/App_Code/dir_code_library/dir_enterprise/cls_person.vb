
Imports ns_enterprise
Imports ns_enterprise.cls_utility

Imports System.Data
Imports System.Web.HttpContext

Namespace ns_enterprise

  Public Class cls_person

    Private str__prv_first_name As String
    Public Property str_first_name() As String

      Get
        Return str__prv_first_name
      End Get
      Set(ByVal Value As String)
        str__prv_first_name = Value
      End Set

    End Property

		Private str__prv_last_name As String
    Public Property str_last_name() As String

      Get
        Return str__prv_last_name
      End Get
      Set(ByVal Value As String)
        str__prv_last_name = Value
      End Set

    End Property

    Private str__prv_phone_number As String
    Public Property str_phone_number() As String

      Get
        Return str__prv_phone_number
      End Get
      Set(ByVal Value As String)
        str__prv_phone_number = Value
      End Set

    End Property

    Private dt__prv_date_of_birth As Date
    Public Property dt_date_of_birth() As Date

      Get
        Return dt__prv_date_of_birth
      End Get
      Set(ByVal Value As Date)
        dt__prv_date_of_birth = Value
      End Set

    End Property

    Private str__prv_email_address As String
    Public Property str_email_address() As String

      Get
        Return str__prv_email_address
      End Get
      Set(ByVal Value As String)
        str__prv_email_address = Value
      End Set

    End Property

		Private str__prv_pk_person As String
		Public Property str_pk_person() As String

      Get
        Return str__prv_pk_person
      End Get
      Set(ByVal Value As String)
        str__prv_pk_person = Value
      End Set

    End Property

    Public Function fnc_insert() As String

			' without date of birth - this function isn't really helpful

			' validate required fields
			If str__prv_first_name.Length = 0 Then
        Throw New Exception("First name is required for person.insert")
      End If

      If str__prv_last_name.Length = 0 Then
        Throw New Exception("Last name is required for person.insert")
      End If

      ''' check to see if the person is already out there
      ''str__prv_pk_person = cls_data_access_layer.fnc_get_scaler__string("select pk_person from udf_person() where first_name = " & fnc_dbwrap(str__prv_first_name) & " and last_name = " & fnc_dbwrap(str__prv_last_name) & " and date_of_birth = " & fnc_dbwrap(dt__prv_date_of_birth.ToShortDateString))

      ''' if found, return
      ''If fnc_convert_expected_string(str__prv_pk_person).Length > 0 Then
      ''  Return str__prv_pk_person
      ''End If

      ' not found, create new GUID to return
      str__prv_pk_person = Guid.NewGuid().ToString

      ' insert new user
			Dim inst_sql As New cls_sql()

      inst_sql.str_operation = cls_sql.en_operation.insert.ToString
      inst_sql.str_table_name = "tbl_person"
      inst_sql.sub_add_column("pk_person", str__prv_pk_person)
      inst_sql.sub_add_column("first_name", str__prv_first_name)
			inst_sql.sub_add_column("last_name", str__prv_last_name)
			'   inst_sql.sub_add_column("date_of_birth", dt__prv_date_of_birth.ToShortDateString)
			inst_sql.sub_execute()

      Return str__prv_pk_person

    End Function

    Public Function fnc_insert__email() As Boolean

      ' validate required fields
      If fnc_convert_expected_string(str__prv_pk_person).Length = 0 Then
        Throw New Exception("Person key required for person.insert__email")
      End If

      ' validate required fields
      If fnc_convert_expected_string(str__prv_email_address).Length = 0 Then
        Throw New Exception("Email address required for person.insert__email")
      End If

      ' check to see if the person email is already out there
      Dim bln_found As Boolean = False
      bln_found = cls_data_access_layer.fnc_get_scaler__boolean("select count(*) from udf_person_email() where fk_person = " & fnc_dbwrap(str__prv_pk_person) & " and email_address = " & fnc_dbwrap(str__prv_email_address))

      ' if found, return
      If bln_found = True Then
        Return True
      End If

      ' insert new email
      Dim inst_sql As cls_sql = New cls_sql()
      inst_sql.str_operation = cls_sql.en_operation.insert.ToString
      inst_sql.str_table_name = "tbl_person_email"
      inst_sql.sub_add_column("fk_person", str__prv_pk_person)
      inst_sql.sub_add_column("email_address", str__prv_email_address)
      inst_sql.sub_execute()

      Return True

    End Function
		Public Function fnc_insert__phone_number() As Boolean

      ' validate required fields
      If fnc_convert_expected_string(str__prv_pk_person).Length = 0 Then
				Throw New Exception("Person key required for person.insert__phone_number")
			End If

      ' validate required fields
      If fnc_convert_expected_string(str__prv_phone_number).Length = 0 Then
				Throw New Exception("phone_number address required for person.insert__phone_number")
			End If

      ' check to see if the person phone_number is already out there
      Dim bln_found As Boolean = False
			bln_found = cls_data_access_layer.fnc_get_scaler__boolean("select count(*) from udf_person_phone() where fk_person = " & fnc_dbwrap(str__prv_pk_person) & " and phone_number = " & fnc_dbwrap(str__prv_phone_number))

      ' if found, return
      If bln_found = True Then
				Return True
			End If

      ' insert new phone_number
      Dim inst_sql As cls_sql = New cls_sql()
			inst_sql.str_operation = cls_sql.en_operation.insert.ToString
			inst_sql.str_table_name = "tbl_person_phone"
			inst_sql.sub_add_column("fk_person", str__prv_pk_person)
			inst_sql.sub_add_column("phone_number", str__prv_phone_number)
			inst_sql.sub_execute()

			Return True

		End Function

		Public Shared Function fnc_select_list() As DataTable

      Dim str_sql As String = "select * from udf_person()"

      Return cls_data_access_layer.fnc_get_datatable(str_sql)

    End Function

    Public Shared Function fnc_select_list(ByVal str__prm_filter As String) As DataTable

      If str__prm_filter.Length = 0 Then
        Return fnc_select_list()
        Exit Function
      End If

      Dim str_sql As String = "select * from udf_person() where " & str__prm_filter

      Return cls_data_access_layer.fnc_get_datatable(str_sql)

    End Function

    Public Sub sub_select(ByVal str__prm_pk As String)

      Dim str_sql As String = "select * from udf_person() where pk_person = " & fnc_dbwrap(str__prm_pk)
      Dim dr_local As SqlClient.SqlDataReader

      dr_local = cls_data_access_layer.fnc_get_sqldatareader(str_sql)

      If dr_local.HasRows = False Then
        Throw New Exception("Could not find person: " & str__prm_pk)
      End If

      dr_local.Read()

      str__prv_first_name = dr_local("first_name").ToString
      str__prv_last_name = dr_local("last_name").ToString

    End Sub

    Public Sub sub_select(ByVal guid__prm_pk As Guid)

      sub_select(guid__prm_pk.ToString)

    End Sub

		Public Shared Sub sub_delete(ByVal str__prm_pk As String)

      'Dim inst_dynamic_sql As New cls_dynamic_sql
      'inst_dynamic_sql.sub_delete("pk_person", str__prm_pk)

    End Sub

		Public Sub sub_update()

			If fnc_is_valid_guid(str__prv_pk_person) = False Then Exit Sub

			' insert new phone_number
			Dim inst_sql As cls_sql = New cls_sql()
			inst_sql.str_table_name = "tbl_person"
			inst_sql.str_operation = cls_sql.en_operation.update.ToString
			inst_sql.sub_add_column("pk_person", str__prv_pk_person)
			If fnc_convert_expected_string(str__prv_first_name).Length > 0 Then inst_sql.sub_add_column("first_name", str__prv_first_name)
			If fnc_convert_expected_string(str__prv_last_name).Length > 0 Then inst_sql.sub_add_column("last_name", str__prv_last_name)
			inst_sql.sub_execute_with_audit(False)

		End Sub

	End Class

End Namespace
