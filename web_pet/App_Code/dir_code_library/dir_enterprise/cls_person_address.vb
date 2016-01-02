Imports ns_enterprise.cls_utility

Imports System.Data

Imports System.Web.HttpContext

Namespace ns_enterprise

  Public Class cls_person_address

    Private str__prv_address_line_01 As String

    Public Property str_address_line_01() As String

      Get
        Return str__prv_address_line_01
      End Get
      Set(ByVal Value As String)
        str__prv_address_line_01 = Value
      End Set

    End Property

    Private str__prv_address_line_02 As String

    Public Property str_address_line_02() As String

      Get
        Return str__prv_address_line_02
      End Get
      Set(ByVal Value As String)
        str__prv_address_line_02 = Value
      End Set

    End Property

    Private str__prv_city As String

    Public Property str_city() As String

      Get
        Return str__prv_city
      End Get
      Set(ByVal Value As String)
        str__prv_city = Value
      End Set

    End Property

    Private str__prv_fk_lkp_address_type As String

    Public Property str_fk_lkp_address_type() As String

      Get
        Return str__prv_fk_lkp_address_type
      End Get
      Set(ByVal Value As String)
        str__prv_fk_lkp_address_type = Value
      End Set

    End Property

    Private str__prv_fk_lkp_state As String

    Public Property str_fk_lkp_state() As String

      Get
        Return str__prv_fk_lkp_state
      End Get
      Set(ByVal Value As String)
        str__prv_fk_lkp_state = Value
      End Set

    End Property

    Private str__prv_zip As String

    Public Property str_zip() As String

      Get
        Return str__prv_zip
      End Get
      Set(ByVal Value As String)
        str__prv_zip = Value
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

      Dim str_sql As String = "select * from udf_person_address() "

      Return cls_data_access_layer.fnc_get_dataset(str_sql)

    End Function

    Public Shared Function fnc_select_list(ByVal str__prm_filter As String) As DataSet

      If str__prm_filter.Length = 0 Then
        Return fnc_select_list()
        Exit Function
      End If

      Dim str_sql As String = "select * from udf_person_address() where " & str__prm_filter

      Return cls_data_access_layer.fnc_get_dataset(str_sql)

    End Function

    Public Sub sub_select(ByVal str__prm_pk As String)

      Dim str_sql As String = "select * from udf_person_address() where pk_person_address = " & fnc_dbwrap(str__prm_pk)
      Dim dr_local As SqlClient.SqlDataReader

      dr_local = cls_data_access_layer.fnc_get_sqldatareader(str_sql)

      If dr_local.HasRows = False Then
        Throw New Exception("Could not find person: " & str__prm_pk)
      End If

      dr_local.Read()

      str__prv_fk_lkp_address_type = dr_local("fk_lkp_address_type").ToString
      str__prv_address_line_01 = dr_local("address_line_01").ToString
      str__prv_address_line_02 = dr_local("address_line_02").ToString
      str__prv_city = dr_local("city").ToString
      str__prv_fk_lkp_state = dr_local("fk_lkp_state").ToString
      str__prv_zip = dr_local("zip").ToString
      str__prv_note = dr_local("note").ToString

    End Sub

    Public Sub sub_select(ByVal guid__prm_pk As Guid)

      sub_select(guid__prm_pk.ToString)

    End Sub

    Public Shared Sub sub_delete(ByVal str__prm_pk As String)

      'Dim inst_dynamic_sql As New cls_dynamic_sql
      'inst_dynamic_sql.sub_delete("pk_person_address", str__prm_pk)

    End Sub

  End Class

End Namespace
