Imports ns_enterprise.cls_utility

Imports System.Data

Namespace ns_enterprise

	Public Class cls_page

		' pk_system_page property
		Private str__prv_pk_system_page As String
		Public Property str_pk_system_page() As String

			Get
				Return str__prv_pk_system_page
			End Get
			Set(ByVal Value As String)
				str__prv_pk_system_page = Value
			End Set

		End Property

		' path property
		Private str__prv_path As String
		Public Property str_path() As String

			Get
				Return str__prv_path
			End Get
			Set(ByVal Value As String)
				str__prv_path = Value
			End Set

		End Property

		Public Shared Function fnc_get_pk_system_page() As String

			Dim str_path As String = cls_controller.str_path

			Return fnc_convert_expected_string(fnc_get_page(str_path)("pk_system_page"))

		End Function

		Public Shared Function fnc_get_page(ByVal str__prm_request_path As String) As DataRow

			Dim str_sql As String

			If HttpRuntime.Cache("tbl_system_page") Is Nothing Then
				str_sql = "select pk_system_page, request_path from tbl_system_page"
				HttpRuntime.Cache.Insert("tbl_system_page", cls_data_access_layer.fnc_get_datatable(str_sql), Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(30))
			End If

			If HttpRuntime.Cache("tbl_system_page") Is Nothing Then
				Throw New Exception("Error retrieving/setting tbl_system_page cache")
			End If

			Dim dr_select() As DataRow
			dr_select = DirectCast(HttpRuntime.Cache("tbl_system_page"), DataTable).Select("request_path=" & fnc_dbwrap(str__prm_request_path))

			If dr_select.Length = 0 Then
				cls_data_access_layer.sub_execute_non_query("insert into tbl_system_page (request_path) select " & fnc_dbwrap(str__prm_request_path))
				HttpRuntime.Cache.Remove("tbl_system_page")
				str_sql = "select pk_system_page, request_path from tbl_system_page"
				HttpRuntime.Cache.Insert("tbl_system_page", cls_data_access_layer.fnc_get_datatable(str_sql), Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(30))
				dr_select = DirectCast(HttpRuntime.Cache("tbl_system_page"), DataTable).Select("request_path=" & fnc_dbwrap(str__prm_request_path))
			End If

			If dr_select.Length = 0 Then
				Throw New Exception("Could not add new page to cache: " & str__prm_request_path)
			End If

			If dr_select.Length > 1 Then
				Throw New Exception("Request path in tbl_system_page more than once: " & str__prm_request_path)
			End If

			Return dr_select(0)

		End Function

		Public Shared Function fnc_select_list() As DataTable

			Dim str_sql As String = "select * from udf_page()"

			Return cls_data_access_layer.fnc_get_datatable(str_sql)

		End Function

		Public Shared Function fnc_select_list(ByVal str__prm_filter As String) As DataTable

			If str__prm_filter.Length = 0 Then
				Return fnc_select_list()
				Exit Function
			End If

			Dim str_sql As String = "select * from udf_page() where " & str__prm_filter

			Return cls_data_access_layer.fnc_get_datatable(str_sql)

		End Function

		Public Shared Sub sub_delete(ByVal str__prm_pk As String)

			'Dim inst_dynamic_sql As New cls_dynamic_sql
			'inst_dynamic_sql.sub_delete("pk_system_page", str__prm_pk)

		End Sub

	End Class

End Namespace
