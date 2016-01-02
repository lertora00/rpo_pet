Imports ns_enterprise
Imports ns_enterprise.cls_utility

Imports Microsoft.ApplicationBlocks.Data
Imports System.Data

Imports System.Web.HttpContext
Imports System.Text
Imports System.Collections.Generic
Imports System.Collections
Imports System.Collections.Specialized
Imports System.Data.SqlClient

Namespace ns_enterprise

  Public Class cls_data_access_layer


		Public Shared Function fnc_get_dataset(ByVal str__prm_sql As String, ByVal int__prm_timeout_second As Int32) As DataSet

			Dim cnn As New SqlClient.SqlConnection(cls_constant.fnc_get_connection_string__readonly)
			cnn.Open()
			Dim cmd As New SqlClient.SqlCommand(str__prm_sql, cnn)
			Dim da As New SqlClient.SqlDataAdapter(cmd)

			cmd.CommandType = CommandType.Text
			cmd.CommandTimeout = int__prm_timeout_second

			Dim ds As New DataSet("ds")

			da.Fill(ds, "dt")

			cnn.Close()
			cmd.Dispose()

			Return ds

		End Function

		Public Shared Function fnc_get_dataset(ByVal str__prm_sql As String) As DataSet

      'Try

      'cls_utility.fnc_dbwrapsub_log_execute("cls_data_access_layer", "fnc_get_dataset", str__prm_sql)

      Return SqlHelper.ExecuteDataset(cls_constant.fnc_get_connection_string__readonly, CommandType.Text, str__prm_sql)

      'Catch e As Exception
      'Throw New Exception(e.Message & " sql: " & fnc_render_clean_sql(str__prm_sql))
      'End Try

    End Function

		Public Shared Function fnc_get_datareader(ByVal str__prm_sql As String) As SqlClient.SqlDataReader

      Try

        'cls_utility.fnc_dbwrapsub_log_execute("cls_data_access_layer", "fnc_get_datareader", str__prm_sql)

        Return SqlHelper.ExecuteReader(cls_constant.fnc_get_connection_string__readonly, CommandType.Text, str__prm_sql)

      Catch e As Exception
        Throw New Exception(e.Message & " sql: " & fnc_render_clean_sql(str__prm_sql))
      End Try

    End Function

    ' quick (and dirty) get datatable based on connection string
    Public Shared Function fnc_get_datatable(ByVal str__prm_sql As String, ByVal str__prm_connection_string As String) As DataTable

      'cls_utility.fnc_dbwrapsub_log_execute("cls_data_access_layer", "fnc_get_datatable", str__prm_sql)

      Return SqlHelper.ExecuteDataset(str__prm_connection_string, CommandType.Text, str__prm_sql).Tables(0)

    End Function

    ' pass in a collection of data rows and return a new datatable
    Public Shared Function fnc_get_datatable(ByVal dr__prm() As DataRow) As DataTable

      Dim dt_return As DataTable = dr__prm(0).Table.Clone
      dt_return.TableName = dr__prm(0).Table.TableName

      For Each dr As DataRow In dr__prm
        dt_return.ImportRow(dr)
      Next

      Return dt_return

    End Function

    Public Shared Function fnc_get_datatable(ByVal str__prm_sql As String) As DataTable

      Return fnc_get_dataset(str__prm_sql).Tables(0)

    End Function

    Public Shared Function fnc_get_datarow(ByVal str__prm_sql As String) As DataRow

      Dim dt As DataTable = fnc_get_datatable(str__prm_sql)

      If dt.Rows.Count = 0 Then
        Return Nothing
      End If

      Return dt.Rows(0)

    End Function

    Public Shared Function fnc_get_dictionary(ByVal str__prm_sql As String) As Dictionary(Of String, String)

      Dim dct As IDictionary(Of String, String) = New Dictionary(Of String, String)

      Dim dt As DataTable = fnc_get_datatable(str__prm_sql)

      ' no rows, return nothing
      If dt.Rows.Count = 0 Then Return Nothing

      ' must have at least 2 columns for arraylist (keyword, value), if more than 2, taking first 2
      If dt.Columns.Count < 2 Then
        Throw New Exception("fnc_get_arraylist requires at least two columns: " & fnc_render_clean_sql(str__prm_sql))
        Return Nothing
      End If

      For Each dr As DataRow In dt.Rows
        If dct.ContainsKey(fnc_convert_expected_string(dr(0))) Then
          Throw New Exception("dictionary keys must be unique: " & fnc_render_clean_sql(str__prm_sql))
        End If

        ' taking first two items in requested sql
        dct.Add(fnc_convert_expected_string(dr(0)), fnc_convert_expected_string(dr(1)))
      Next

      Return dct

    End Function

    Public Shared Function fnc_get_sqldatareader(ByVal str__prm_sql As String) As SqlClient.SqlDataReader

      Try

        'cls_utility.fnc_dbwrapsub_log_execute("cls_data_access_layer", "fnc_get_sqldatareader", str__prm_sql)

        Return SqlHelper.ExecuteReader(cls_constant.fnc_get_connection_string__readonly, CommandType.Text, str__prm_sql)

      Catch e As Exception
        Throw New Exception(e.Message & " sql: " & fnc_render_clean_sql(str__prm_sql))
      End Try

    End Function

    Public Shared Function fnc_get_scaler__string(ByVal str__prm_sql As String) As String

      Try

        'cls_utility.fnc_dbwrapsub_log_execute("cls_data_access_layer", "fnc_get_scaler__string", str__prm_sql)

        Return fnc_convert_expected_string(SqlHelper.ExecuteScalar(cls_constant.fnc_get_connection_string__readonly, CommandType.Text, str__prm_sql))

      Catch e As Exception
        Throw New Exception(e.Message & " sql: " & fnc_render_clean_sql(str__prm_sql))
      End Try

    End Function

    Public Shared Function fnc_get_scaler__guid(ByVal str__prm_sql As String) As Guid

      Try

        'cls_utility.fnc_dbwrapsub_log_execute("cls_data_access_layer", "fnc_get_scaler__guid", str__prm_sql)

        Return fnc_convert_expected_guid(SqlHelper.ExecuteScalar(cls_constant.fnc_get_connection_string__readonly, CommandType.Text, str__prm_sql))

      Catch e As Exception
        Throw New Exception(e.Message & " sql: " & fnc_render_clean_sql(str__prm_sql))
      End Try

    End Function

    Public Shared Function fnc_get_scaler__guid__string(ByVal str__prm_sql As String) As String

      Dim str_guid As String = ""

      Try

        'cls_utility.fnc_dbwrapsub_log_execute("cls_data_access_layer", "fnc_get_scaler__guid__string", str__prm_sql)

        str_guid = fnc_convert_expected_string(SqlHelper.ExecuteScalar(cls_constant.fnc_get_connection_string__readonly, CommandType.Text, str__prm_sql))

        If fnc_is_valid_guid(str_guid) = True Then
          Return str_guid
        End If

        Return ""

      Catch e As Exception
        Throw New Exception(e.Message & " sql: " & fnc_render_clean_sql(str__prm_sql))
      End Try

    End Function

    Public Shared Function fnc_get_scaler__datetime(ByVal str__prm_sql As String) As DateTime

      Try

        'cls_utility.fnc_dbwrapsub_log_execute("cls_data_access_layer", "fnc_get_scaler__datetime", str__prm_sql)

        Return fnc_convert_expected_datetime(SqlHelper.ExecuteScalar(cls_constant.fnc_get_connection_string__readonly, CommandType.Text, str__prm_sql))

      Catch e As Exception
        Throw New Exception(e.Message & " sql: " & fnc_render_clean_sql(str__prm_sql))
      End Try

    End Function

    Public Shared Function fnc_get_scaler__boolean(ByVal str__prm_sql As String) As Boolean

      Try

        'cls_utility.fnc_dbwrapsub_log_execute("cls_data_access_layer", "fnc_get_scaler__boolean", str__prm_sql)

        Return fnc_convert_expected_boolean(SqlHelper.ExecuteScalar(cls_constant.fnc_get_connection_string__readonly, CommandType.Text, str__prm_sql))

      Catch e As Exception
        Throw New Exception(e.Message & " sql: " & fnc_render_clean_sql(str__prm_sql))
      End Try

    End Function

    Public Shared Function fnc_get_scaler__number(ByVal str__prm_sql As String) As Double

      Try

        'cls_utility.fnc_dbwrapsub_log_execute("cls_data_access_layer", "fnc_get_scaler__number", str__prm_sql)

        Return fnc_convert_expected_double(SqlHelper.ExecuteScalar(cls_constant.fnc_get_connection_string__readonly, CommandType.Text, str__prm_sql))

      Catch e As Exception
        Throw New Exception(e.Message & " sql: " & fnc_render_clean_sql(str__prm_sql))
      End Try

    End Function

    Public Shared Function fnc_get_scaler__object(ByVal str__prm_sql As String) As Object

      Try
        'cls_utility.fnc_dbwrapsub_log_execute("cls_data_access_layer", "fnc_get_scaler__object", str__prm_sql)

        Return SqlHelper.ExecuteScalar(cls_constant.fnc_get_connection_string__readonly, CommandType.Text, str__prm_sql)

      Catch e As Exception
        Throw New Exception(e.Message & " sql: " & fnc_render_clean_sql(str__prm_sql))
      End Try

    End Function

    'Public Shared Function fnc_execute_non_queryOLD(ByVal str__prm_sql As String, ByVal sdct__prm_parameter As OrderedDictionary) As Integer


    '  Dim cnn As SqlConnection = Nothing
    '  Dim str_parameter As New System.Text.StringBuilder()

    '  Try

    '    ' returns rows effected (yet to confirm)

    '    cnn = New SqlConnection(cls_constant.fnc_get_connection_string__readwrite)
    '    cnn.Open()

    '    Dim str_sql As String = ""
    '    ''        str_sql = cls_sql.fnc_get_sql_declare(sdct__prm_parameter) & fnc_get_sql_set(sdct__prm_parameter) & str__prm_sql

    '    Dim cmd As SqlCommand = New SqlCommand(str_sql)
    '    cmd.Connection = cnn

    '    ' This was problematic with delimeters.  Uniqueidentifer didn't want any.  Dates had their own issue.  
    '    '' ''Dim str_delimiter As String = ""

    '    '' ''Dim sql_parameter As New SqlParameter()
    '    '' ''For Each Str As String In sdct__prm_parameter.Keys
    '    '' ''  sql_parameter = New SqlParameter(Str, sdct__prm_parameter(Str))
    '    '' ''  cmd.Parameters.Add(sql_parameter)
    '    '' ''  str_parameter.Append(str_delimiter & sdct__prm_parameter(Str))
    '    '' ''  str_delimiter = "^"
    '    '' ''Next

    '    cmd.ExecuteNonQuery()

    '  Catch e As Exception
    '    Throw New Exception(e.Message & " sql: " & fnc_render_clean_sql(str__prm_sql) & " data: " & str_parameter.ToString)
    '  Finally
    '    cnn.Close()
    '  End Try

    'End Function

    Public Shared Function fnc_execute_non_query(ByVal str__prm_sql As String) As Integer

      Try

        ' returns rows effected (yet to confirm)
        Return SqlHelper.ExecuteNonQuery(cls_constant.fnc_get_connection_string__readwrite, CommandType.Text, str__prm_sql)

      Catch e As Exception
        Throw New Exception(e.Message & " sql: " & fnc_render_clean_sql(str__prm_sql))
      End Try

    End Function

    Public Shared Sub sub_execute_non_query(ByVal str__prm_sql As String)

      Try

        SqlHelper.ExecuteNonQuery(cls_constant.fnc_get_connection_string__readwrite, CommandType.Text, str__prm_sql)

      Catch e As Exception
        Throw New Exception(e.Message & " sql: " & fnc_render_clean_sql(str__prm_sql))
      End Try

    End Sub

    Public Shared Function fnc_get_lkp(ByVal str__prm_table_name As String) As DataTable

      If cls_variable.fnc_get_lkp(str__prm_table_name) Is Nothing Then
        cls_utility.sub_refresh_lkp_cache(str__prm_table_name)
      End If

      Return cls_variable.fnc_get_lkp(str__prm_table_name)

    End Function

    Public Shared Function fnc_get_lkp__filter_down(ByVal str__prm_table_name As String, ByVal str__prm_primary_key_condition As String) As DataTable

      Dim dt2 As DataTable = cls_data_access_layer.fnc_get_datatable("select * from " & str__prm_table_name & " where " & cls_utility.fnc_primary_key_from_table(str__prm_table_name) & " not in (select target_primary_key_filter_down from tbl_filter_down_condition, tbl_filter_down where pk_filter_down = fk_filter_down and target_table_name = " & cls_utility.fnc_dbwrap(str__prm_table_name) & " and primary_key_condition in (" & cls_utility.fnc_dbwrap(str__prm_primary_key_condition) & ")) order by sort_order, name, code_value")

      Return dt2

      ' below code does not work because the .select on a table doesn't support the not in aspect of the where clause

      Dim dt As DataTable = fnc_get_lkp(str__prm_table_name)

      If dt Is Nothing Then
        Return Nothing
      End If

      Dim dr() As DataRow = dt.Select(cls_utility.fnc_primary_key_from_table(str__prm_table_name) & " not in (select target_primary_key_filter_down from tbl_filter_down_condition, tbl_filter_down where pk_filter_down = fk_filter_down and target_table_name = " & cls_utility.fnc_dbwrap(str__prm_table_name) & " and primary_key_condition in (" & cls_utility.fnc_dbwrap(str__prm_primary_key_condition) & "))")

      If dr.Length = 0 Then
        Return Nothing
      End If

      Return fnc_get_datatable(dr)

    End Function

    Public Shared Sub sub_test_connection()

      Dim str_connection_name As String = ""

      Dim cn As SqlClient.SqlConnection

      Try

        str_connection_name = "readonly"
        cn = New SqlClient.SqlConnection(cls_constant.fnc_get_connection_string__readonly)

        cn.Open()
        If cn.State = ConnectionState.Open Then
          cn.Close()
        Else
          Throw New Exception("Connection State not open:" & str_connection_name)
        End If

        str_connection_name = "readwrite"
        cn = New SqlClient.SqlConnection(cls_constant.fnc_get_connection_string__readwrite)

        cn.Open()
        If cn.State = ConnectionState.Open Then
          cn.Close()
        Else
          Throw New Exception("Connection State not open:" & str_connection_name)
        End If

        str_connection_name = "administrator"
        cn = New SqlClient.SqlConnection(cls_constant.fnc_get_connection_string__administrator)

        cn.Open()
        If cn.State = ConnectionState.Open Then
          cn.Close()
        Else
          Throw New Exception("Connection State not open:" & str_connection_name)
        End If

      Catch ex As Exception
        Throw New Exception("Could not connect via: " & str_connection_name, ex.InnerException)
      End Try

    End Sub

    Public Shared Function fnc_add_accounting_update() As String

      If cls_current_user.str_pk_person_user.Length = 0 Then Return ""

      Dim str As String = ""

      str = str & ", "
      str = str & cls_constant.str_update_user_column_name_pk & " = " & cls_utility.fnc_dbwrap(cls_current_user.str_pk_person_user)
      str = str & ", "
      str = str & cls_constant.str_update_user_column_name & " = " & cls_utility.fnc_dbwrap(cls_current_user.str_onscreen_name)
      str = str & ", "
      str = str & cls_constant.str_update_date_column_name & " = " & cls_utility.fnc_dbwrap(Now.ToString)

      Return str

    End Function

    Public Shared Function fnc_add_accounting_insert_columm_list() As String

      If cls_current_user.str_pk_person_user.Length = 0 Then Return ""

      Dim str As String = ""

      str = str & ", "
      str = str & cls_constant.str_insert_user_column_name_pk
      str = str & ", "
      str = str & cls_constant.str_insert_user_column_name
      str = str & ", "
      str = str & cls_constant.str_insert_date_column_name

      Return str

    End Function

    Public Shared Function fnc_add_accounting_insert_value_list() As String

      If cls_current_user.str_pk_person_user.Length = 0 Then Return ""

      Dim str As String = ""

      str = str & ", "
      str = str & cls_utility.fnc_dbwrap(cls_current_user.str_pk_person_user)
      str = str & ", "
      str = str & cls_utility.fnc_dbwrap(cls_current_user.str_onscreen_name)
      str = str & ", "
      str = str & cls_utility.fnc_dbwrap(Now.ToString)

      Return str

    End Function

    Public Shared Function fnc_get_erd__down(ByVal str__prm_table_name As String) As DataTable

      Dim dt_erd_parent As DataTable = fnc_create_erd_parent(str__prm_table_name)
      Dim dt_erd_relation As DataTable = fnc_create_erd_relation()
      Dim dr_parent_set() As DataRow = Nothing
      Dim dr_erd As DataRow = Nothing

      Dim dt_meta As DataTable = cls_constant.dt_database_column
      Dim dr_meta_set() As DataRow = Nothing
      Dim str_filter As String = ""

      Dim str_table_name_initial As String = str__prm_table_name

      ' create topmost relation with table requested and no children
      sub_add_erd_relation(dt_erd_relation, str__prm_table_name, cls_utility.fnc_primary_key_from_table(str__prm_table_name), "", "", 0)

      For int_depth As Int16 = 1 To 20

        dr_parent_set = dt_erd_parent.Select("child_check_flag = 0")

        ' no remaining parents to check for children
        If dr_parent_set.Length = 0 Then Exit For

        For Each dr_parent As DataRow In dr_parent_set

          ' find child tables based on foreign key name - starts with fk name
          str_filter = "(column_name = " & cls_utility.fnc_dbwrap(cls_utility.fnc_foreign_key_from_table(dr_parent("table_name"))) & " or column_name like '" & cls_utility.fnc_foreign_key_from_table(dr_parent("table_name")) & "__%') and column_name not like '%__update' and column_name not like '%__insert' and table_name <> " & cls_utility.fnc_dbwrap(dr_parent("table_name"))
          dr_meta_set = dt_meta.Select(str_filter)

          If dr_meta_set.Length > 0 Then
            For Each dr As DataRow In dr_meta_set
              sub_add_erd_parent(str_table_name_initial, dt_erd_parent, dt_erd_relation, dr_parent("table_name"), fnc_convert_expected_string(dr("table_name")), int_depth)
              sub_add_erd_relation(dt_erd_relation, dr_parent("table_name"), cls_utility.fnc_primary_key_from_table(dr_parent("table_name")), fnc_convert_expected_string(dr("table_name")), fnc_convert_expected_string(dr("column_name")), int_depth)
            Next
            ' update parent with check and count
            sub_update_erd_parent(dr_parent, dr_meta_set.Length)
          Else
            sub_update_erd_parent(dr_parent, 0)
          End If

        Next

      Next

      Return dt_erd_relation

    End Function

    Public Shared Function fnc_get_erd__up(ByVal str__prm_table_name As String) As DataTable

      Return Nothing

    End Function

    Private Shared Function fnc_create_erd_parent(ByVal str__prm_table_name As String) As DataTable

      Dim dt_erd As DataTable
      Dim dr_erd As DataRow

      dt_erd = New DataTable("tbl_erd_parent")

      dt_erd.Columns.Add("id_erd", GetType(Integer))
      dt_erd.Columns("id_erd").AutoIncrement = True
      dt_erd.Columns.Add("table_name", GetType(String))
      dt_erd.Columns.Add("primary_key_name", GetType(String))
      dt_erd.Columns.Add("depth", GetType(Integer))
      dt_erd.Columns.Add("child_check_flag", GetType(Boolean))
      dt_erd.Columns.Add("offspring_count", GetType(Integer))
      dt_erd.Columns.Add("update_date", GetType(Date))
      dt_erd.Columns.Add("insert_date", GetType(Date))

      dr_erd = dt_erd.NewRow
      dr_erd("table_name") = str__prm_table_name
      dr_erd("primary_key_name") = cls_utility.fnc_primary_key_from_table(str__prm_table_name)
      dr_erd("depth") = 0
      dr_erd("child_check_flag") = 0
      dr_erd("offspring_count") = 0
      dr_erd("insert_date") = Now
      dt_erd.Rows.Add(dr_erd)

      Return dt_erd

    End Function

    Private Shared Function fnc_create_erd_relation() As DataTable

      Dim dt_erd As DataTable = Nothing
      Dim dr_erd As DataRow = Nothing

      dt_erd = New DataTable("tbl_erd_relation")

      dt_erd.Columns.Add("id_erd", GetType(Integer))
      dt_erd.Columns("id_erd").AutoIncrement = True
      dt_erd.Columns.Add("parent_table_name", GetType(String))
      dt_erd.Columns.Add("parent_column_name", GetType(String))
      dt_erd.Columns.Add("child_table_name", GetType(String))
      dt_erd.Columns.Add("child_column_name", GetType(String))
      dt_erd.Columns.Add("depth", GetType(Integer))
      dt_erd.Columns.Add("insert_date", GetType(Date))

      Return dt_erd

    End Function

    Private Shared Sub sub_add_erd_relation(ByRef dt__prm As DataTable, ByVal str__prm_parent_table_name As String, ByVal str__prm_parent_column_name As String, ByVal str__prm_child_table_name As String, ByVal str__prm_child_column_name As String, ByVal int__prm_depth As Int32)

      ' check to see if table is already in erd relation via parent table/child table route - if so, do not re-add to parent check list
      'If dt__prm.Select("parent_table_name = " & cls_utility.fnc_dbwrap (str__prm_parent_table_name) & " and child_table_name = " & cls_utility.fnc_dbwrap (str__prm_child_table_name)).Length > 0 Then
      'Exit Sub
      'End If

      Dim dr_erd As DataRow = Nothing

      dr_erd = dt__prm.NewRow
      dr_erd("parent_table_name") = str__prm_parent_table_name
      dr_erd("parent_column_name") = str__prm_parent_column_name
      dr_erd("child_table_name") = str__prm_child_table_name
      dr_erd("child_column_name") = str__prm_child_column_name
      dr_erd("depth") = int__prm_depth
      dr_erd("insert_date") = Now
      dt__prm.Rows.Add(dr_erd)

    End Sub

    Private Shared Sub sub_add_erd_parent(ByVal str__prm_table_name_initial As String, ByRef dt__prm_parent As DataTable, ByVal dt__prm_relation As DataTable, ByVal str__prm_parent_table_name As String, ByVal str__prm_child_table_name As String, ByVal int__prm_depth As Int32)

      ' if parent is already in parent check table at same depth, do not re-add
      If dt__prm_parent.Select("table_name = " & cls_utility.fnc_dbwrap(str__prm_child_table_name) & " and depth = " & int__prm_depth).Length > 0 Then
        Exit Sub
      End If

      ' do not create new parent check for initial table (could happen if you wind up back at initial table)
      If str__prm_table_name_initial = str__prm_child_table_name Then Exit Sub

      Dim dr_erd As DataRow = Nothing

      dr_erd = dt__prm_parent.NewRow
      dr_erd("table_name") = str__prm_child_table_name
      dr_erd("primary_key_name") = cls_utility.fnc_primary_key_from_table(str__prm_child_table_name)
      dr_erd("child_check_flag") = 0
      dr_erd("depth") = int__prm_depth
      dr_erd("insert_date") = Now
      dt__prm_parent.Rows.Add(dr_erd)

      System.Diagnostics.Debug.WriteLine(str__prm_child_table_name)

    End Sub

    Private Shared Sub sub_update_erd_parent(ByRef dr__prm As DataRow, ByVal int__prm_offspring_count As Int32)

      dr__prm("offspring_count") = int__prm_offspring_count
      dr__prm("child_check_flag") = 1
      dr__prm("update_date") = Now

    End Sub

    Public Shared Function fnc_get_last_update_audit__string(ByVal str__prm_table_name As String, ByVal str__prm_row_primary_key As String, Optional ByVal bln__prm_include_header_key As Boolean = False) As String

      ' likely calling last audit on insert page (no primary key) - return nothing
      If fnc_convert_expected_string(str__prm_row_primary_key).Length = 0 Or fnc_convert_expected_string(str__prm_table_name).Length = 0 Then
        Return ""
      End If

      Dim dr_audit As DataRow = fnc_get_last_update_audit(str__prm_table_name, str__prm_row_primary_key)

      If dr_audit Is Nothing Then
        ' this could happen if auditing is disabled on a table - otherwise, there should always be at least an insert
        Return cls_constant.str_audit_not_available
      End If

      Dim str_return As New StringBuilder()

      Dim str_old_value As String = fnc_convert_expected_string(dr_audit("old_value"))
      If str_old_value.Length = 0 Then str_old_value = "(empty)"
      Dim str_new_value As String = fnc_convert_expected_string(dr_audit("new_value"))
      If str_new_value.Length = 0 Then str_new_value = "(empty)"

      Dim str_action As String = fnc_convert_expected_string(dr_audit("action"))

      ' this is R&D type code.  No UI would normally be here.  TODO remove UI
      str_return.Append("<span class='audit-label'>Last Action:</span><span class='audit-data'>" & str_action & "</span>")
      str_return.Append("<span class='audit-label'>By:</span><span class='audit-data'>" & fnc_convert_expected_string(dr_audit("insert_user")) & "</span>")
      str_return.Append("<span class='audit-label'>On:</span><span class='audit-data'>" & fnc_convert_expected_datetime__string(dr_audit("insert_date"), DateFormat.ShortDate) & "</span>")
      str_return.Append("<span class='audit-label'>Column:</span><span class='audit-data'>" & fnc_convert_expected_string(dr_audit("column_name__display")) & "</span>")
      str_return.Append("<span class='audit-label'>New Value:</span><span class='audit-data'>" & str_new_value & "</span>")
      If str_action.ToLower = "update" Then
        str_return.Append("<span class='audit-label'>Old Value:</span><span class='audit-data'>" & str_old_value & "</span>")
      End If

      If bln__prm_include_header_key = True Then
        str_return.Append("^")
        str_return.Append(fnc_convert_expected_string(dr_audit(0)))
      End If

      Return str_return.ToString

    End Function

    Public Shared Function fnc_get_last_update_audit(ByVal str__prm_table_name As String, ByVal str__prm_row_primary_key As String) As DataRow

      Dim dt As DataTable = cls_data_access_layer.fnc_get_erd__down(str__prm_table_name)

      If dt Is Nothing Then Throw New Exception("invalid erd down: " & str__prm_table_name)

      ' TODO - hack to pass primary key value in - using output which is confusing
      '   didn't want to create a new field to use as it is a recursive function
      Dim str_return As New System.Text.StringBuilder
      str_return.Append(str__prm_row_primary_key)

      ' TODO - bit of a back to store row primary key in output string for first pass
      sub_build_sql_pk(dt.Rows(0), Nothing, Nothing, str_return)
      str_return.Append("; select top 1 	* from  udf_log_system_audit(), #tbl_primary_key where primary_key_value = row_primary_key order by insert_date desc")

      Return cls_data_access_layer.fnc_get_datarow(str_return.ToString)

    End Function

    Private Shared Sub sub_build_sql_pk(ByVal dr__prm_relation As DataRow, ByVal srtl__prm_from As SortedList, ByVal srtl__prm_where As SortedList, ByRef str__prm_return As System.Text.StringBuilder)

      Dim str_select As String = ""
      Dim str_from As String = ""
      Dim str_where As String = ""

      Dim str_child_table_name As String = fnc_convert_expected_string(dr__prm_relation("child_table_name"))

      If str_child_table_name.Length = 0 Then
        str_select = fnc_build_select_pk(fnc_convert_expected_string(dr__prm_relation("parent_table_name")), 0)
        str_from = fnc_build_from_pk(fnc_convert_expected_string(dr__prm_relation("parent_table_name")), srtl__prm_from, 0)
        str_where = fnc_build_where_pk(fnc_convert_expected_string(dr__prm_relation("parent_column_name")), Nothing, cls_utility.fnc_dbwrap(str__prm_return.ToString), srtl__prm_where, 0)
        str__prm_return = New System.Text.StringBuilder()
        sub_set_clause(str_from, srtl__prm_from, 0)
        sub_set_clause(str_where, srtl__prm_where, 0)
        str__prm_return.Append(vbCrLf & str_select & fnc_get_from_pk(srtl__prm_from) & fnc_get_where_pk(srtl__prm_where))
        str_child_table_name = fnc_convert_expected_string(dr__prm_relation("parent_table_name"))
      End If

      Dim int_depth As Int32 = fnc_convert_expected_int32(dr__prm_relation("depth"))
      Dim int_depth_child As Int32 = int_depth + 1

      Dim dr_relation_set() As DataRow = Nothing
      Dim dr_child_set() As DataRow = Nothing

      dr_relation_set = dr__prm_relation.Table.Select("parent_table_name = " & cls_utility.fnc_dbwrap(str_child_table_name) & " and depth = " & int_depth_child)

      For Each dr As DataRow In dr_relation_set

        str_child_table_name = fnc_convert_expected_string(dr("child_table_name"))

        Dim str_child_column_name As String = fnc_convert_expected_string(dr("child_column_name"))
        Dim str_parent_column_name As String = fnc_convert_expected_string(dr("parent_column_name"))

        sub_remove_at_and_above(srtl__prm_from, int_depth_child)
        sub_remove_at_and_above(srtl__prm_where, int_depth_child)

        str_select = fnc_build_select_pk(str_child_table_name, int_depth_child)
        str_from = fnc_build_from_pk(str_child_table_name, srtl__prm_from, int_depth_child)
        str_where = fnc_build_where_pk(str_parent_column_name, str_child_table_name, str_child_column_name, srtl__prm_where, int_depth_child)

        sub_set_clause(str_from, srtl__prm_from, int_depth_child)
        sub_set_clause(str_where, srtl__prm_where, int_depth_child)

        str__prm_return.Append(vbCrLf & " union " & str_select & fnc_get_from_pk(srtl__prm_from) & fnc_get_where_pk(srtl__prm_where))

        dr_child_set = dr__prm_relation.Table.Select("parent_table_name = " & cls_utility.fnc_dbwrap(str_child_table_name) & " and depth = " & fnc_convert_expected_int32(dr("depth")) + 1)

        If dr_child_set.Length > 0 Then
          sub_build_sql_pk(dr, srtl__prm_from, srtl__prm_where, str__prm_return)
        End If

      Next

    End Sub

    Private Shared Function fnc_get_from_pk(ByVal srtl__prm As SortedList) As String

      Dim str_clause As New System.Text.StringBuilder(" from ")
      Dim str_comma As String = ""

      For Each str As String In srtl__prm.Values
        str_clause.Append(str_comma & str)
        str_comma = ", "
      Next

      Return str_clause.ToString

    End Function

    Private Shared Function fnc_get_where_pk(ByVal srtl__prm As SortedList) As String

      Dim str_clause As New System.Text.StringBuilder(" where ")
      Dim str_and As String = ""

      'For Each kvp As KeyValuePair(Of Int32, String) In srtl
      For Each str As String In srtl__prm.Values
        str_clause.Append(str_and & str.Trim)
        str_and = " and "
      Next

      Return str_clause.ToString

    End Function

    Private Shared Sub sub_remove_at_and_above(ByRef srtl__prm As SortedList, ByVal int__prm_depth As Int32)

      If srtl__prm Is Nothing Then Exit Sub

      ' remove list items at or above the current index
      For int_index As Int32 = 0 To srtl__prm.Count - 1
        If int_index >= int__prm_depth Then
          srtl__prm.Remove(int_index)
        End If
      Next

    End Sub

    Private Shared Sub sub_set_clause(ByVal str__prm_clause As String, ByRef srtl__prm As SortedList, ByVal int__prm_depth As Int32)

      If srtl__prm Is Nothing Then
        srtl__prm = New SortedList
      End If

      srtl__prm.Add(int__prm_depth, str__prm_clause)

    End Sub

    Private Shared Function fnc_build_select_pk(ByVal str__prm_table_name As String, ByVal int__prm_depth As Int32) As String

      Dim str_clause As New System.Text.StringBuilder

      str_clause.Append("select ")
      str_clause.Append("a" & int__prm_depth.ToString & "." & cls_utility.fnc_primary_key_from_table(str__prm_table_name) & " as primary_key_value")

      If int__prm_depth = 0 Then
        ' TODO: Not really a hack as without inserting into a temp table, you may get a too many tables error
        str_clause.Append(" into #tbl_primary_key ")
      End If

      Return str_clause.ToString

    End Function

    Private Shared Function fnc_build_from_pk(ByVal str__prm_table_name As String, ByVal srtl__from As SortedList, ByVal int__prm_depth As Int32) As String

      Dim str_clause As New System.Text.StringBuilder

      str_clause.Append(str__prm_table_name & " a" & int__prm_depth.ToString & " ")

      Return str_clause.ToString

    End Function

    Private Shared Function fnc_build_where_pk(ByVal str__prm_where_left As String, ByVal str__prm_where_right_table_name As String, ByVal str__prm_where_right As String, ByVal srtl__where As SortedList, ByVal int__prm_depth As Int32) As String

      If fnc_convert_expected_string(str__prm_where_right_table_name).Length = 0 Then
        Return "a0." & str__prm_where_left & " = " & str__prm_where_right & " "
      Else
        Return "a" & (int__prm_depth - 1).ToString & "." & str__prm_where_left & " = a" & int__prm_depth & "." & str__prm_where_right & " "
      End If

    End Function

    Public Shared Function fnc_create_datatable(ByVal int__prm_row_count As Int32, ByVal int__prm_list_index As Int32, Optional ByVal int__prm_row_index__parent As Int32 = 0) As DataTable

      Dim dt As DataTable
      Dim dr As DataRow

      Dim int_row_count As Int32

      'create a DataTable        
      dt = New DataTable
      dt.Columns.Add(New DataColumn("int_list_index", GetType(Integer)))
      dt.Columns.Add(New DataColumn("int_row_index", GetType(Integer)))
      dt.Columns.Add(New DataColumn("int_row_index__parent", GetType(Integer)))
      dt.Columns.Add(New DataColumn("int_row_number", GetType(Integer)))
      dt.Columns.Add(New DataColumn("str_value", GetType(String)))
      dt.Columns.Add(New DataColumn("insert_date", GetType(DateTime)))
      dt.Columns.Add(New DataColumn("bln_value", GetType(Boolean)))
      dt.Columns.Add(New DataColumn("dbl_value", GetType(Double)))
      dt.Columns.Add(New DataColumn("parent_row_index", GetType(Integer)))

      'Make some rows and put some sample data in        
      For int_row_count = 1 To int__prm_row_count

        dr = dt.NewRow()
        dr("int_list_index") = int__prm_list_index
        dr("int_row_index") = int_row_count - 1
        dr("int_row_index__parent") = int__prm_row_index__parent
        dr("int_row_number") = int_row_count
        dr("str_value") = "Item " + int_row_count.ToString()
        dr("insert_date") = DateTime.Now.ToShortTimeString

        If (int_row_count Mod 2 <> 0) Then
          dr("bln_value") = True
        Else
          dr("bln_value") = False
        End If
        dr("dbl_value") = 1.23 * (int_row_count + 1)

        'add the row to the datatable      
        dt.Rows.Add(dr)
      Next

      'return  the DataTable     
      Return dt

    End Function

    Public Shared Function fnc_render_clean_sql(ByVal str__prm_sql As String) As String

      ' TODO - removed as it's harder to debug 
      Return str__prm_sql

      Dim str_sql As String = str__prm_sql

      str_sql = Replace(str_sql, "tbl_", "")
      str_sql = Replace(str_sql, "pk_", "")
      str_sql = Replace(str_sql, "fk_", "")

      str_sql = Replace(str_sql, "_", "-")

      str_sql = Replace(str_sql, "  ", " ")

      Return str_sql

    End Function

    ' this new method is similar to user interface method of similar name.  need to reconcile them
    Public Shared Function fnc_decode_lookup_for_db _
    ( _
    ByVal str__prm_pk_or_fk_name As String _
    , ByVal str__prm_pk_or_fk_value As String _
    ) _
    As String

      Dim str_sql As String = ""
      Dim str_table_name As String = ""
      Dim str_primary_key_name As String = ""
      Dim str_value As String = ""

      ' determine table name for lookup tables
      str_table_name = Replace(str__prm_pk_or_fk_name, cls_constant.str_primary_key_prefix__lookup, cls_constant.str_table_prefix__lookup)
      str_table_name = Replace(str_table_name, cls_constant.str_foreign_key_prefix__lookup, cls_constant.str_table_prefix__lookup)

      ' determine primary key name for lookup tables
      str_primary_key_name = Replace(str_table_name, cls_constant.str_table_prefix__lookup, cls_constant.str_primary_key_prefix__lookup)

      ' if you cannot determine the primary key name, stop and throw exception
      If str_primary_key_name.Length = 0 Then
        Throw New Exception("Error converting table name to primary key name: " & str_table_name)
      End If

      Dim dt_lkp As DataTable = fnc_get_lkp(str_table_name)
      Dim dr_lkp() As DataRow

      dr_lkp = dt_lkp.Select(str_primary_key_name & "=" & cls_utility.fnc_dbwrap(str__prm_pk_or_fk_value))

      If dr_lkp.Length = 0 Then
        Throw New Exception("Error decoding lookup: " & str_primary_key_name & ":" & str__prm_pk_or_fk_value)
      End If

      str_value = fnc_convert_expected_string(dr_lkp(0)("name"))

      If fnc_convert_expected_string(str_value).Length = 0 Then
        Return "null"
      End If

      Return cls_utility.fnc_dbwrap(str_value)

    End Function

    Public Shared Function fnc_build_dt_sql(ByVal str__prm_sql As String) As DataTable

      Dim dt As New DataTable("tbl_sql")
      dt.Columns.Add("sql", GetType(String))

      Dim dr As DataRow = dt.NewRow()
      dr("sql") = str__prm_sql

      dt.Rows.Add(dr)

      Return dt

    End Function

    Public Shared Function fnc_build_dt_where(ByVal str__prm_where As String) As DataTable

      Dim dt As New DataTable("tbl_where")
      dt.Columns.Add("where", GetType(String))

      Dim dr As DataRow = dt.NewRow()
      dr("where") = str__prm_where

      dt.Rows.Add(dr)

      Return dt

    End Function

    Public Shared Function fnc_create_standard_datatable(ByVal str__prm_table_name As String) As DataTable

      Dim dt As New DataTable
      If fnc_convert_expected_string(str__prm_table_name).Length > 0 Then dt.TableName = str__prm_table_name

      Dim str_primary_key_name As String = ""
      If str__prm_table_name.StartsWith(cls_constant.str_table_prefix) Then
        str_primary_key_name = cls_utility.fnc_primary_key_from_table(str__prm_table_name)
      Else
        str_primary_key_name = cls_constant.str_primary_key_prefix & str__prm_table_name
      End If

      Dim str_id_name As String = Replace(str_primary_key_name, cls_constant.str_primary_key_prefix, cls_constant.str_id_prefix)

      '		consider adding uniqueidentifier so this "table" can be saved to the database for debugging purposes
      dt.Columns.Add(str_primary_key_name, GetType(Guid))
      dt.Columns(str_primary_key_name).DefaultValue = Guid.NewGuid()
      dt.Constraints.Add("cnst_" & str_primary_key_name, dt.Columns(str_primary_key_name), True)
      dt.Columns.Add(str_id_name, GetType(Integer))
      dt.Columns(str_id_name).AutoIncrement = True
      dt.Columns(str_id_name).AutoIncrementSeed = 1
      dt.Columns.Add("insert_date", GetType(Date))

      Return dt

    End Function

    Public Shared Function fnc_is_valid_table(ByVal str__prm_table_name As String) As Boolean

      If str__prm_table_name.Length = 0 Then
        Throw New Exception("fnc_is_valid_table failed - table name missing")
      End If

      Dim dt_database_column As DataTable = cls_constant.dt_database_column

      Dim str_filter As String = "table_name = " & cls_utility.fnc_dbwrap(str__prm_table_name)

      Dim dr_result() As DataRow = dt_database_column.Select(str_filter)

      If dr_result.Length = 0 Then
        'Throw New Exception("Table name (" & str__prm_table_name & ") not found in udf_system_database_column(02)")
        Return False
      End If

      Return True

    End Function

    Public Shared Function fnc_is_valid_column(ByVal str__prm_table_name As String, ByVal str__prm_column_name As String) As Boolean

      If str__prm_table_name.Length = 0 Then
        Throw New Exception("fnc_is_valid_column failed - table name missing")
      End If

      If str__prm_column_name.Length = 0 Then
        Throw New Exception("fnc_is_valid_column failed - column name missing")
      End If

      Dim dt_database_column As DataTable = cls_constant.dt_database_column

      Dim str_filter As String = "table_name = " & cls_utility.fnc_dbwrap(str__prm_table_name) & " and column_name = " & cls_utility.fnc_dbwrap(str__prm_column_name)

      Dim dr_result() As DataRow = dt_database_column.Select(str_filter)

      If dr_result.Length = 0 Then
        'Throw New Exception("Table name (" & str__prm_table_name & ") not found in udf_system_database_column(02)")
        Return False
      End If

      Return True

    End Function

    Public Shared Function fnc_validate_meta_data() As Boolean

      Dim dt_meta As DataTable = cls_global.dt__pub_database_column
      Dim str_table_name As String = ""
      Dim str_column_name As String = ""
      Dim str_table_name__derived As String = ""

      For Each dr_meta As DataRow In dt_meta.Rows
        str_table_name = fnc_convert_expected_string(dr_meta("table_name"))
        str_column_name = fnc_convert_expected_string(dr_meta("column_name"))

        ' check to make sure a primary key has a corresponding (correct) table name
        If str_column_name.StartsWith(cls_constant.str_primary_key_prefix) Then
          str_table_name__derived = cls_utility.fnc_table_from_primary_key(str_column_name)
          If fnc_is_valid_column(str_table_name__derived, str_column_name) = False Then
            Throw New Exception("Invalid table or column name (: derived table name): " & str_table_name & ", " & str_column_name & ": " & str_table_name__derived)
          End If
        End If

        ' make sure foreign key has corresponding parent table/pk
        If str_column_name.StartsWith(cls_constant.str_foreign_key_prefix) Then
          str_table_name__derived = cls_utility.fnc_table_from_foreign_key(str_column_name)
          If fnc_is_valid_table(str_table_name__derived) = False Then
            Throw New Exception("Invalid parent table for foreign key: " & str_column_name & " in " & str_table_name)
          End If
        End If

      Next

      Return True

    End Function

    Public Shared Sub sub_reconcile_meta()

      sub_execute_non_query("insert into tbl_system_table (table_name) select distinct table_name from information_schema.columns where table_name not in (select table_name from tbl_system_table) and table_name like 'tbl_%'; insert into tbl_system_column (fk_system_table, column_name) select pk_system_table, column_name from information_schema.columns a, tbl_system_table b where a.table_name = b.table_name and a.table_name <> 'tbl_information_schema_column' and column_name not in (select column_name from udf_system_column() where b.pk_system_table = fk_system_table) order by a.table_name, a.column_name; delete from tbl_system_column where pk_system_column in ( select pk_system_column from tbl_system_column a , tbl_system_table b where column_name not in (select column_name from information_schema.columns where table_name = b.table_name) and pk_system_table = fk_system_table ); ")

    End Sub

  End Class

End Namespace