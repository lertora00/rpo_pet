Imports ns_enterprise.cls_utility
Imports System.Data

Imports System.Web.HttpContext

Namespace ns_enterprise

  Public Class cls_person_note

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

      Dim str_sql As String = "select * from udf_person_note() "

      Return cls_data_access_layer.fnc_get_dataset(str_sql)

    End Function

    Public Shared Function fnc_select_list(ByVal str__prm_filter As String) As DataSet

      If str__prm_filter.Length = 0 Then
        Return fnc_select_list()
        Exit Function
      End If

      Dim str_sql As String = "select * from udf_person_note() where " & str__prm_filter

      Return cls_data_access_layer.fnc_get_dataset(str_sql)

    End Function

    Public Sub sub_select(ByVal str__prm_pk As String)

      Dim str_sql As String = "select * from udf_person_note() where pk_person_note = " & fnc_dbwrap(str__prm_pk)
      Dim dr_local As SqlClient.SqlDataReader

      dr_local = cls_data_access_layer.fnc_get_sqldatareader(str_sql)

      If dr_local.HasRows = False Then
        Throw New Exception("Could not find person: " & str__prm_pk)
      End If

      dr_local.Read()

      str__prv_note = dr_local("note").ToString

    End Sub

    Public Sub sub_select(ByVal guid__prm_pk As Guid)

      sub_select(guid__prm_pk.ToString)

    End Sub

    Public Shared Sub sub_delete(ByVal str__prm_pk As String)

      'Dim inst_dynamic_sql As New cls_dynamic_sql
      'inst_dynamic_sql.sub_delete("pk_person_note", str__prm_pk)

    End Sub

  End Class

End Namespace
