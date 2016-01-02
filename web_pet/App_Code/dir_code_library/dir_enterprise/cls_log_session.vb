Imports System.Data

Namespace ns_enterprise

	Public Class cls_log_session

		Public Enum en_log_session_event_type
			page_access = 1
			redirect = 2
			prerender = 3
		End Enum

		' pk_log_session property
		Private str__prv_pk_log_session As String
		Public Property str_pk_log_session() As String

			Get
				Return str__prv_pk_log_session
			End Get
			Set(ByVal Value As String)
				str__prv_pk_log_session = Value
			End Set

		End Property

		' note property
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

		Public Sub New(ByVal str__prm_pk_log_session As String)

			str__prv_pk_log_session = str__prm_pk_log_session
			inst__prv_sql_collection = New cls_sql_collection()

		End Sub

		Public Sub New()

			str__prv_pk_log_session = Guid.NewGuid.ToString

			inst__prv_sql_collection = New cls_sql_collection()

		End Sub

		Public Sub sub_log_session(Optional ByVal str__prm_note As String = "")

			Dim inst_sql As cls_sql = New cls_sql()
			inst_sql.str_operation = cls_sql.en_operation.insert.ToString
			inst_sql.str_table_name = "tbl_log_session"
			inst_sql.sub_add_column("pk_log_session", str__prv_pk_log_session)
			inst_sql.sub_add_column("aspnet_sessionid", cls_session.str_aspnet_sessionid)
			inst_sql.sub_add_column("note", str__prm_note)

			inst__prv_sql_collection.Insert(0, inst_sql)

		End Sub

		Public Sub sub_log_session_event(ByVal str__prm_sender As String, ByVal str__prm_event_argument As String, ByVal str__prm_session_event_type As String, Optional ByVal str__prm_note As String = "")

			Dim inst_sql As cls_sql = New cls_sql()
			inst_sql.str_operation = cls_sql.en_operation.insert.ToString
			inst_sql.str_table_name = "tbl_log_session_event"
			inst_sql.sub_add_column("fk_log_session", str__prv_pk_log_session)
			inst_sql.sub_add_column("fk_person_user__authenticated", cls_current_user.str_pk_person_user)
			inst_sql.sub_add_column("fk_lkp_session_event_type", cls_user_interface.fnc_code_lkp("tbl_lkp_session_event_type", str__prm_session_event_type))
			inst_sql.sub_add_column("fk_system_page", cls_page.fnc_get_pk_system_page())
			inst_sql.sub_add_column("querystring", cls_variable.str_querystring)

			' quick hack to blank out sender as master page
			If str__prm_sender.ToLower.StartsWith("asp") Then
				str__prm_sender = ""
			End If

			inst_sql.sub_add_column("sender", str__prm_sender)
			inst_sql.sub_add_column("event_argument", str__prm_event_argument)
			inst_sql.sub_add_column("note", str__prm_note)

			inst__prv_sql_collection.Add(inst_sql)

		End Sub

		Public Sub sub_log_session_parameter(ByVal str__prm_keyword As String, ByVal str__prm_value As String, Optional ByVal str__prm_note As String = "")

			Dim inst_sql As cls_sql = New cls_sql()
			inst_sql.str_operation = cls_sql.en_operation.insert.ToString
			inst_sql.str_table_name = "tbl_log_session_parameter"
			inst_sql.sub_add_column("fk_log_session", str__prv_pk_log_session)
			inst_sql.sub_add_column("keyword", str__prm_keyword)
			inst_sql.sub_add_column("value", str__prm_value)
			inst_sql.sub_add_column("note", str__prm_note)

			inst__prv_sql_collection.Add(inst_sql)

		End Sub

		Public Sub sub_log_execute()

			If cls_constant.bln_disable_log_session = False Then
				Dim str_sql As String = inst__prv_sql_collection.fnc_get_sql(False, False)

				Try
					cls_asynchronous.sub_execute_non_query(str_sql)
				Catch ex As Exception
				End Try

				' inst__prv_sql_collection.sub_execute(False, False)
			End If

		End Sub

		Public Shared Function fnc_select_list() As DataTable

			Dim str_sql As String = "select * from udf_log_session_event()"

			Return cls_data_access_layer.fnc_get_datatable(str_sql)

		End Function

		Public Shared Function fnc_select_list(ByVal str__prm_filter As String) As DataTable

			If str__prm_filter.Length = 0 Then
				Return fnc_select_list()
				Exit Function
			End If

			Dim str_sql As String = "select * from udf_log_session_event() where " & str__prm_filter

			Return cls_data_access_layer.fnc_get_datatable(str_sql)

		End Function

		Public Shared Sub sub_delete(ByVal str__prm_pk As String)

			'Dim inst_dynamic_sql As New cls_dynamic_sql
			'inst_dynamic_sql.sub_delete("pk_log_session", str__prm_pk)

		End Sub

	End Class

End Namespace