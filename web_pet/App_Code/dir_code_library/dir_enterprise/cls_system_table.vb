Imports System.Data
Imports ns_enterprise

Namespace ns_enterprise
	Public Class cls_system_table

		Private str__prv_table_name As String
		Public Property str_table_name() As String

			Get
				Return str__prv_table_name
			End Get
			Set(ByVal Value As String)
				str__prv_table_name = Value
			End Set

		End Property

		Public Shared Function fnc_has_column(ByVal str__prm_table_name As String, ByVal str__prm_column_name As String) As Boolean

			Dim dt As DataTable = cls_constant.dt_database_column

			If dt.Select("table_name = " & cls_utility.fnc_dbwrap(str__prm_table_name) & " and column_name = " & cls_utility.fnc_dbwrap(str__prm_column_name)).Length = 1 Then
				Return True
			End If

			Return False

		End Function

	End Class

End Namespace