Imports ns_enterprise.cls_utility
Imports System.Data

Imports System.Web.HttpContext


Namespace ns_enterprise

  Public Class cls_person_user_system_role

    Public Shared Function fnc_get_person_user_in_application_role(ByVal str__prm_lkp_system_role As String) As DataTable

      Dim str_sql As String = "select * from udf_person_user_system_role() where fk_lkp_system_role = " & fnc_dbwrap(str__prm_lkp_system_role)

      Return cls_data_access_layer.fnc_get_dataset(str_sql).Tables(0)

    End Function

    Public Shared Sub sub_delete(ByVal str__prm_pk As String)

      'Dim inst_dynamic_sql As New cls_dynamic_sql
      'inst_dynamic_sql.sub_delete("pk_person_user_system_role", str__prm_pk)

    End Sub

    Public Shared Function fnc_select_list() As DataSet

      Dim str_sql As String = "select * from udf_person_user_system_role() "

      Return cls_data_access_layer.fnc_get_dataset(str_sql)

    End Function

    Public Shared Function fnc_select_list(ByVal str__prm_filter As String) As DataSet

      If str__prm_filter.Length = 0 Then
        Return fnc_select_list()
        Exit Function
      End If

      Dim str_sql As String = "select * from udf_person_user_system_role() where " & str__prm_filter

      Return cls_data_access_layer.fnc_get_dataset(str_sql)

    End Function

  End Class

End Namespace
