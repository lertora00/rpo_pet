Imports System.Data
Imports ns_enterprise

Namespace ns_enterprise
	Public Class cls_system_column

		Private str__prv_name As String
		Public Property str_name() As String

			Get
				Return str__prv_name
			End Get
			Set(ByVal Value As String)
				str__prv_name = Value
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

		Public Shared Function fnc_select_list() As DataTable

			Dim str_sql As String = "select * from udf_system_column() "

			Return cls_data_access_layer.fnc_get_datatable(str_sql)

		End Function

		Public Shared Function fnc_select_list(ByVal str__prm_filter As String) As DataTable

			If str__prm_filter.Length = 0 Then
				Return fnc_select_list()
				Exit Function
			End If

			Dim str_sql As String = "select * from udf_system_column() where " & str__prm_filter

			Return cls_data_access_layer.fnc_get_datatable(str_sql)

		End Function

		Public Shared Sub sub_delete(ByVal str__prm_pk As String)

			'Dim inst_dynamic_sql As New cls_dynamic_sql
			'inst_dynamic_sql.sub_delete("pk_system_column", str__prm_pk, cls_constant.en_delete_type_override.permanent, cls_constant.en_delete_audit_override.do_not_audit_delete)

		End Sub

		Public Sub sub_select(ByVal str__prm_pk As String)

			Dim str_sql As String = "select * from udf_system_column() where pk_system_column = " & cls_utility.fnc_dbwrap(str__prm_pk)
			Dim dr_local As SqlClient.SqlDataReader

			dr_local = cls_data_access_layer.fnc_get_sqldatareader(str_sql)

			If dr_local.HasRows = False Then
				Throw New Exception("Could not find system_column: " & str__prm_pk)
			End If

			dr_local.Read()

			str__prv_name = dr_local("name").ToString
			str__prv_note = dr_local("note").ToString

		End Sub

	End Class
End Namespace