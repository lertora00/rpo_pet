Imports ns_enterprise.cls_utility

Imports System.Data

Imports System.Web.HttpContext

Namespace ns_enterprise

  Public Class cls_person_email

    Private str__prv_pk_person_email As String
    Public Property str_pk_person_email() As String

      Get
        Return str__prv_pk_person_email
      End Get
      Set(ByVal Value As String)
        str__prv_pk_person_email = Value
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

    Private str__prv_email_address As String
    Public Property str_email_address() As String

      Get
        Return str__prv_email_address
      End Get
      Set(ByVal Value As String)
        str__prv_email_address = Value
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

    Public Shared Function fnc_select_list() As DataSet

      Dim str_sql As String = "select * from udf_person_email() "

      Return cls_data_access_layer.fnc_get_dataset(str_sql)

    End Function

    Public Shared Function fnc_select_list(ByVal str__prm_filter As String) As DataSet

      If str__prm_filter.Length = 0 Then
        Return fnc_select_list()
        Exit Function
      End If

      Dim str_sql As String = "select * from udf_person_email() where " & str__prm_filter

      Return cls_data_access_layer.fnc_get_dataset(str_sql)

    End Function

    Public Sub sub_select(ByVal str__prm_pk As String)

      Dim str_sql As String = "select * from udf_person_email() where pk_person_email = " & fnc_dbwrap(str__prm_pk)
      Dim dr_local As SqlClient.SqlDataReader

      dr_local = cls_data_access_layer.fnc_get_sqldatareader(str_sql)

      If dr_local.HasRows = False Then
        Throw New Exception("Could not find person: " & str__prm_pk)
      End If

      dr_local.Read()

      str__prv_email_address = dr_local("email_address").ToString
      str__prv_note = dr_local("note").ToString

    End Sub

    Public Sub sub_select_most_recent_by_person(ByVal str__prm_fk_person As String)

      Dim str_sql As String = "select top 1 * from udf_person_email() where fk_person = " & fnc_dbwrap(str__prm_fk_person) & " order by " & cls_constant.str_insert_date_column_name & " desc"
      Dim dr_local As SqlClient.SqlDataReader

      dr_local = cls_data_access_layer.fnc_get_sqldatareader(str_sql)

      If dr_local.HasRows = False Then
        Throw New Exception("Could not find email for person: " & str__prm_fk_person)
      End If

      dr_local.Read()

      str__prv_pk_person_email = fnc_convert_expected_string(dr_local("pk_person_email"))
      str__prv_fk_person = fnc_convert_expected_string(dr_local("fk_person"))
      str__prv_email_address = fnc_convert_expected_string(dr_local("email_address"))
      str__prv_note = fnc_convert_expected_string(dr_local("note"))

      dr_local.Close()

    End Sub

    Public Sub sub_select(ByVal guid__prm_pk As Guid)

      sub_select(guid__prm_pk.ToString)

    End Sub

    Public Shared Sub sub_delete(ByVal str__prm_pk As String)

      'Dim inst_dynamic_sql As New cls_dynamic_sql
      'inst_dynamic_sql.sub_delete("pk_person_email", str__prm_pk)

    End Sub

		Public Sub sub_update()

			If fnc_is_valid_guid(str__prv_pk_person_email) = False Then Exit Sub

			' insert new phone_number
			Dim inst_sql As cls_sql = New cls_sql()
			inst_sql.str_table_name = "tbl_person_email"
			inst_sql.str_operation = cls_sql.en_operation.update.ToString
			inst_sql.sub_add_column("pk_person_email", str__prv_pk_person_email)
			If fnc_convert_expected_string(str__prv_email_address).Length > 0 Then inst_sql.sub_add_column("email_address", str__prv_email_address)
			inst_sql.sub_execute_with_audit(False)

		End Sub

	End Class

End Namespace
