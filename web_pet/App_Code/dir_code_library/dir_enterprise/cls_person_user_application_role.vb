Imports ns_enterprise.cls_utility

Imports System.Data
Imports System.Web.HttpContext

Namespace ns_enterprise

	Public Class cls_person_user_application_role

		Private str__prv_pk_person_user_application_role As String
		Public Property str_pk_person_user_application_role() As String

			Get
				Return str__prv_pk_person_user_application_role
			End Get
			Set(ByVal Value As String)
				str__prv_pk_person_user_application_role = Value
			End Set

		End Property

		Private str__prv_fk_person_user As String
		Public Property str_fk_person_user() As String

			Get
				Return str__prv_fk_person_user
			End Get
			Set(ByVal Value As String)
				str__prv_fk_person_user = Value
			End Set

		End Property

		Private str__prv_fk_lkp_application_role As String
		Public Property str_fk_lkp_application_role() As String

			Get
				Return str__prv_fk_lkp_application_role
			End Get
			Set(ByVal Value As String)
				str__prv_fk_lkp_application_role = Value
			End Set

		End Property

		Public Shared Function fnc_get_person_user_in_application_role(ByVal str__prm_lkp_application_role As String) As DataTable

			If Current.Cache.Item("person_user_application_role") Is Nothing Then
				sub_refresh_person_user_application_role()
			End If

      ' grab lkp table out of cache
      Dim dt As DataTable = DirectCast(Current.Cache.Item("person_user_application_role"), DataTable)

			Dim dr_select() As DataRow = dt.Select("fk_lkp_application_role = " & fnc_dbwrap(str__prm_lkp_application_role), "onscreen_name asc")

			If dr_select.Length = 0 Then
				Return Nothing
			End If

			Return dt.Copy

      ' exit sub???

      Dim dt_return As DataTable = dr_select(0).Table.Clone
			dt_return.TableName = dr_select(0).Table.TableName

			For Each dr As DataRow In dr_select
				dt_return.ImportRow(dr)
			Next

			Return dt_return

		End Function

		Public Sub sub_select_most_recent_by_person_user(ByVal str__prm_fk_person_user As String)

			Dim str_sql As String = "select top 1 * from udf_person_user_application_role() where fk_person_user = " & fnc_dbwrap(str__prm_fk_person_user) & " order by " & cls_constant.str_insert_date_column_name & " desc"
			Dim dr_local As SqlClient.SqlDataReader

			dr_local = cls_data_access_layer.fnc_get_sqldatareader(str_sql)

			If dr_local.HasRows = False Then
				Throw New Exception("Could not find person_user_application_role by person_user: " & str__prm_fk_person_user)
			End If

			dr_local.Read()

			str__prv_pk_person_user_application_role = fnc_convert_expected_string(dr_local("pk_person_user_application_role"))
			str__prv_fk_person_user = fnc_convert_expected_string(dr_local("fk_person_user"))
			str__prv_fk_lkp_application_role = fnc_convert_expected_string(dr_local("fk_lkp_application_role"))

			dr_local.Close()

		End Sub

		Public Shared Sub sub_delete(ByVal str__prm_pk As String)

			'Dim inst_dynamic_sql As New cls_dynamic_sql
			'inst_dynamic_sql.sub_delete("pk_person_user_application_role", str__prm_pk)

		End Sub

		Public Shared Function fnc_select_list() As DataSet

			Dim str_sql As String = "select * from udf_person_user_application_role() "

			Return cls_data_access_layer.fnc_get_dataset(str_sql)

		End Function

		Public Shared Function fnc_select_list(ByVal str__prm_filter As String) As DataSet

			If str__prm_filter.Length = 0 Then
				Return fnc_select_list()
				Exit Function
			End If

			Dim str_sql As String = "select * from udf_person_user_application_role() where " & str__prm_filter

			Return cls_data_access_layer.fnc_get_dataset(str_sql)

		End Function

	End Class

End Namespace
