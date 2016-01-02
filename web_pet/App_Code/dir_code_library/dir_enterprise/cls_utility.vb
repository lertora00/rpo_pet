Imports System.Data
Imports ns_enterprise

Imports System.Web.UI.WebControls
Imports System.Configuration
Imports System.Web.HttpContext
Imports System.Globalization
Imports System.Web
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Web.UI
Imports System.Reflection
Imports System.Collections.Specialized

Namespace ns_enterprise

	Public Class cls_utility

		Public Shared Function fnc_dbwrap(ByVal obj__prm As Object) As String

			Return cls_utility.fnc_dbwrap(fnc_convert_expected_string(obj__prm))

		End Function

		Public Shared Function fnc_dbwrap(ByVal str__prm_database_value As String) As String

			Dim str_value As String = fnc_convert_expected_string(str__prm_database_value)

      ' if already wrapped with single quotes, remove them to avoid double wrapping.
      If str_value.StartsWith("'") = True And str_value.EndsWith("'") = True Then
				str_value = str_value.Remove(0, 1)
				str_value = str_value.Remove(str_value.Length - 1, 1)
			End If

			str_value = fnc_clean_for_database((str_value))

      ' empty strings should yield null
      If str_value.Length = 0 Then
				Return "null"
			End If

      ' don't wrap null if passed in
      If str_value = "null" Then
				Return "null"
			End If

			Return cls_constant.str_db_wrap & str_value & cls_constant.str_db_wrap

		End Function

		Public Shared Function fnc_clean_for_database(ByRef str__prm As String) As String

      ' here is where you can replace characters the database may not like

      ' escape single quotes
      Return fnc_escape_for_database(str__prm)

		End Function

		Private Shared Function fnc_escape_for_database(ByVal str__prm As String) As String

			Dim str_return = fnc_convert_expected_string(str__prm)

			If str_return.Length = 0 Then Return ""

			str_return = fnc_convert_expected_string(Replace(str_return, "'", "''"))

      ' TODO: this will not work if user enters more than 2 single quotes
      str_return = fnc_convert_expected_string(Replace(str_return, "''''", "'''"))
			str_return = fnc_convert_expected_string(Replace(str_return, "'''", "''"))

			Return str_return

		End Function

    ' function to check to see if an object is empty
    Public Shared Function fnc_is_empty(ByVal obj__prm As Object) As Boolean

      ' if converted to string returns zero length, then empty it is
      If fnc_convert_expected_string(obj__prm).Length = 0 Then
				Return True
			End If

			Return False

		End Function


		Public Shared Function fnc_get_user_input(obj__prm As Object) As String

			Dim str As String = fnc_convert_expected_string(obj__prm)

			' strip out a bunch of characters
			Return fnc_return_regex_match(str, cls_constant.str_system_constant("regex_user_input__simple"))

		End Function
    ' function to handle nothing, null, or empty strings
    Public Shared Function fnc_convert_expected_string(ByVal obj__prm As Object) As String

			If obj__prm Is Nothing = True Then
				Return ""
			End If

			If IsDBNull(obj__prm) = True Then
				Return ""
			End If

			Return obj__prm.ToString

		End Function

    ' function to handle nothing, null, true, false, yes, no, 1, 0 and return boolean
    Public Shared Function fnc_convert_expected_guid(ByVal obj__prm As Object) As Guid

			If fnc_is_valid_guid(obj__prm) = True Then
				Return DirectCast(obj__prm, Guid)
			End If

			Return Nothing

		End Function

    ' function to handle nothing, null, true, false, yes, no, 1, 0 and return boolean
    Public Shared Function fnc_convert_expected_bit(ByVal obj__prm As Object) As Int32

			If fnc_convert_expected_boolean(obj__prm) = True Then
				Return 1
			Else
				Return 0
			End If

		End Function

    ' function to handle nothing, null, true, false, yes, no, 1, 0 and return boolean
    Public Shared Function fnc_convert_expected_boolean(ByVal obj__prm As Object) As Boolean

      ' if null, nothing, empty, etc. return false
      If fnc_convert_expected_string(obj__prm).Length = 0 Then
				Return False
			End If

			Dim str_boolean As String = fnc_convert_expected_string(obj__prm).ToLower

			Select Case str_boolean
				Case "1", "true", "on", "yes"
					Return True
				Case "0", "false", "off", "no"
					Return False
				Case Else
					Return True
			End Select

		End Function

		Public Shared Function fnc_format_date__short_date_and_time(ByVal obj__prm As Object) As String

			Return fnc_convert_expected_datetime(obj__prm).ToString("MM/dd/yy hh:mmt")

		End Function

		Public Shared Function fnc_format_date__short_date(ByVal obj__prm As Object) As String

			Return fnc_convert_expected_datetime(obj__prm).ToString("MM/dd/yy")

		End Function

		Public Shared Function fnc_format_date__short_date__ccyy(ByVal obj__prm As Object) As String

			Return fnc_convert_expected_datetime(obj__prm).ToString("MM/dd/yyyy")

		End Function

    ' function to handle nothing, null, or empty strings and attempt to convert to valid date
    Public Shared Function fnc_convert_expected_datetime(ByVal obj__prm As Object) As DateTime

			If fnc_convert_expected_string(obj__prm).Length = 0 Then
				Return Nothing
			End If

			If cls_validation.fnc_is_date(obj__prm) = True Then
				Return Convert.ToDateTime(obj__prm)
			Else
				Return Nothing
			End If

		End Function

    ' function to handle nothing, null, or empty strings and attempt to convert to valid date
    Public Shared Function fnc_convert_expected_datetime__string(ByVal obj__prm As Object, ByVal df__prm_date_format As DateFormat) As String

			Dim dt_date As Date = fnc_convert_expected_datetime(obj__prm)

			If dt_date = Nothing Then
				Return ""
			End If

			Dim str_date As String = fnc_convert_expected_string(obj__prm)

			If InStr(str_date, Year(dt_date).ToString) = 0 Then
        ' if it put in 19 as century for two digit year - make it 20
      End If

      ' if nothing passed for date format, return general 
      If df__prm_date_format = Nothing Then
				Return Convert.ToDateTime(obj__prm).ToShortDateString & " " & Convert.ToDateTime(obj__prm).ToShortTimeString
			End If

			If cls_validation.fnc_is_date(obj__prm) = True Then
				Select Case df__prm_date_format
					Case DateFormat.ShortDate
						Return Format(Convert.ToDateTime(obj__prm), cls_constant.str_date_format)
					Case DateFormat.LongDate
						Return Convert.ToDateTime(obj__prm).ToLongDateString
					Case DateFormat.GeneralDate
						Return Convert.ToDateTime(obj__prm).ToShortDateString & " " & Convert.ToDateTime(obj__prm).ToShortTimeString
					Case DateFormat.LongTime
						Return Convert.ToDateTime(obj__prm).ToLongTimeString
					Case DateFormat.ShortTime
						Return Convert.ToDateTime(obj__prm).ToShortTimeString
				End Select
			End If

			Return ""

		End Function

    ' function to handle nothing, null, or empty strings and attempt to convert to valid date
    Public Shared Function fnc_convert_expected_datetime__string(ByVal obj__prm As Object, ByVal str__prm_date_format As String) As String

			str__prm_date_format = fnc_convert_expected_string(str__prm_date_format)

			Select Case str__prm_date_format
				Case "short_date", "date"
					Return fnc_convert_expected_datetime__string(obj__prm, DateFormat.ShortDate)
				Case "long_date"
					Return fnc_convert_expected_datetime__string(obj__prm, DateFormat.LongDate)
				Case "general_date"
					Return fnc_convert_expected_datetime__string(obj__prm, DateFormat.GeneralDate)
				Case "long_time"
					Return fnc_convert_expected_datetime__string(obj__prm, DateFormat.LongTime)
				Case "short_time"
					Return fnc_convert_expected_datetime__string(obj__prm, DateFormat.ShortTime)
				Case Else
          ' all other formats (including none) return general date
          Return fnc_convert_expected_datetime__string(obj__prm, DateFormat.GeneralDate)
			End Select

		End Function

    ' function to handle nothing, null, or empty strings and attempt to convert to valid date
    Public Shared Function fnc_convert_expected_datetime__string(ByVal obj__prm As Object) As String

			Return fnc_convert_expected_datetime__string(obj__prm, DateFormat.ShortDate)

		End Function

		Public Shared Function fnc_convert_expected_time__string(ByVal obj__prm As Object) As String

			Dim dt_local As DateTime

			dt_local = fnc_convert_expected_datetime(obj__prm)

			If dt_local = Nothing Then
				Return ""
			End If

			Return dt_local.ToShortTimeString

		End Function

    ' function to check to see if valid int32's exist in an object
    Public Shared Function fnc_contains_int32(ByVal obj__prm As Object) As Boolean

      ' check for null, nothing, empty
      Dim str_passed As String = fnc_convert_expected_string(obj__prm)

      ' is null, nothing, or empty
      If str_passed.Length = 0 Then
				Return False
			End If

			Dim str_int32 As String = fnc_match_regular_expression(str_passed, cls_constant.str_system_constant("regex_integer"))

			If str_int32.Length > 0 Then
				Return True
			End If

			Return False

		End Function

    ' function to handle nothing, null, or empty strings
    Public Shared Function fnc_convert_expected_int32(ByVal obj__prm As Object) As Int32

      ' check for null, nothing, empty
      Dim str_passed As String = fnc_convert_expected_string(obj__prm)

      ' is null, nothing, or empty
      If str_passed.Length = 0 Then
				Return 0
			End If

			Dim str_int32 As String = fnc_match_regular_expression(str_passed, cls_constant.str_system_constant("regex_numeric"))

			If str_int32.Length = 0 Then
				Return 0
			End If

			Return Convert.ToInt32(Convert.ToDouble(str_int32))

      ' TODO - following was part of old regex to get any valid number - above may be cleaner for int32

      ' if numeric, then convert floor in case decimals exist
      If IsNumeric(str_int32) = True Then
				Return Convert.ToInt32(Math.Floor(Convert.ToDouble(str_int32, CultureInfo.InvariantCulture)))
			End If

			Return 0

		End Function

    ' function to handle nothing, null, or empty strings
    Public Shared Function fnc_convert_expected_double(ByVal obj__prm As Object) As Double

      ' if null, nothing, empty, etc return 0
      If fnc_convert_expected_string(obj__prm).Length = 0 Then
				Return 0
			End If

			If IsNumeric(obj__prm) = False Then
				Return 0
			End If

			Return Convert.ToDouble(obj__prm)

      'Dim str_number As String

      '' return only numbers from string (strip everything else)
      'Dim reg_number As New RegularExpressions.Regex("\d{0,6}")
      'str_number = reg_number.Match(obj__prm.ToString).ToString

      'If str_number.Length = 0 Then
      'Return 0
      'End If

      ' if numeric, then convert floor in case decimals exist
      ''If IsNumeric(str_number) = True Then
      ''Return Convert.ToDouble(str_number)
      ''End If

      ''Return 0

    End Function

    'Formats a collection of querystring key/value pairs with ? and & and urlencoding
    Public Shared Function fnc_format_querystring_collection(ByVal nvc__prm_querystring As Specialized.NameValueCollection) As String

			If nvc__prm_querystring Is Nothing Then
				Return ""
			End If

			If nvc__prm_querystring.Count = 0 Then
				Return ""
			End If

			Dim str_querystring As New StringBuilder
			str_querystring.Append("?")

      ' build single string with & delimiter encoding key and value
      Dim str_continue As String = ""
			For Each str_querystring_key As String In nvc__prm_querystring.AllKeys
				str_querystring.Append(str_continue)
				str_querystring.Append(Current.Server.UrlEncode(str_querystring_key))
				str_querystring.Append("=")
				str_querystring.Append(Current.Server.UrlEncode(nvc__prm_querystring(str_querystring_key)))
				str_continue = "&"
			Next

			Return str_querystring.ToString

		End Function

    'Receives a datatable and attempts to sort given criteria
    Public Shared Function fnc_sort_data_table(ByVal dt__prm As DataTable, ByVal str_sort_criteria As String) As DataTable

      ' create a dataview for possible sort expression 
      Dim dv_list As New DataView
			dv_list = dt__prm.DefaultView

      ' .sort method doesn't work with full word ascending/descending
      str_sort_criteria = Replace(str_sort_criteria, " ascending", " asc")
			str_sort_criteria = Replace(str_sort_criteria, " descending", " desc")

      ' attempt to sort view based on querystring supplied sort criteria (user can alter)
      Try
				dv_list.Sort = str_sort_criteria
			Catch ex As System.Data.DataException
        ' log "error" but do not redirect to error page as user could be tampering with sort criteria (URL)
        '''ExceptionManager.Publish(ex)
        Throw New Exception("Invalid sort attempted: " & str_sort_criteria)
        'cls_controller.sub_redirect(Current.Request.Url.AbsolutePath)
        ' don't think this return would ever get called
        Return dt__prm
			Catch ex As Exception
        'Dim x As New cls_exception_publisher
        'x.Publish(ex, Nothing, ConfigurationManager.AppSettings)

        ' log "error" but do not redirect to error page as user could be tampering with sort criteria (URL)
        '''ExceptionManager.Publish(ex)
        ''cls_controller.sub_redirect(Current.Request.Url.AbsolutePath)
        Throw New Exception("Invalid sort or querystring tamper error:")
        ' don't think this return would ever get called
        Return dt__prm
			End Try

      ' must clone rows in the dataview table into a new datatable as there is no way to get
      '   the sorted results out
      Dim dt_clone As DataTable = dv_list.Table.Clone
			dt_clone.TableName = dv_list.Table.TableName

      ' for each row in sorted view, add to clone table
      For Each drv_list As DataRowView In dv_list
				dt_clone.ImportRow(drv_list.Row)
			Next

			Return dt_clone

		End Function

    ' Will reduce the size of a datatable to the page requested
    Public Shared Function fnc_paged_sorted_data_table(ByVal dt__prm As DataTable, ByVal int_page_number As Integer, ByVal pageSize As Integer, ByRef actualint_page_number As Integer) As DataTable

			Return fnc__prv_paged_sorted_data_table(dt__prm, int_page_number, pageSize, actualint_page_number)

		End Function

    ' Will reduce the size of a datatable to the page requested; can be used as a viable alternative to performing paging in the database.
    Public Shared Function fnc_paged_sorted_data_table(ByVal dt__prm As DataTable, ByVal int_page_number As Integer, ByVal int__prm_page_size As Integer) As DataTable

			Return fnc__prv_paged_sorted_data_table(dt__prm, int_page_number, int__prm_page_size, 0)

		End Function

    'Will reduce the size of a datatable to the page requested; can be used as a viable alternative to performing paging in the database.
    Private Shared Function fnc__prv_paged_sorted_data_table(ByVal dt__prm As DataTable, ByVal int__prm_page_number As Integer, ByVal int__prm_page_size As Integer, ByRef int_actual_page_number As Integer) As DataTable

			Dim int_page_number As Integer = int__prm_page_number

      ' default actual page number to page requested
      int_actual_page_number = int_page_number

			Dim int_page_size As Integer = int__prm_page_size

      ' nothing to do - return original DataTable
      If dt__prm Is Nothing OrElse dt__prm.Rows.Count = 0 Then
				Return dt__prm
			End If

      ' fewer rows than int__prm_page_size, nothing to do, return original DataTable
      If int_page_size >= dt__prm.Rows.Count Then
				Return dt__prm
			End If

      'If the product of int_page_number and int_page_size is greater than the number of rows in dt__prm, set int_page_number to the highest possible value (the last page of data)
      If (int_page_size * int_page_number) > dt__prm.Rows.Count Then
				int_page_number = CInt(Math.Ceiling(dt__prm.Rows.Count / int_page_size))
			End If

      ' set Actual Page number now that it's been determined
      int_actual_page_number = int_page_number

      ' setup table clone to populate in for next loop
      Dim dt_clone As DataTable = dt__prm.Clone
			dt_clone.TableName = dt__prm.TableName

      ' figure out starting and ending row index (paged)
      Dim int_row_start As Integer = (int_page_number * int_page_size) - int_page_size + 1
			Dim int_row_end As Integer = (int_page_number * int_page_size)

      ' loop through rows, marking those withing page requested as edited so getchanges can return them in new DataTable
      For int_row As Integer = 0 To dt__prm.Rows.Count - 1
				If int_row + 1 >= int_row_start And int_row + 1 <= int_row_end Then
					dt_clone.ImportRow(dt__prm.Rows(int_row))
				End If
        ' completed page size, no need to continue looping, exit
        If int_row + 1 > int_row_end Then Exit For
			Next

      ' return rows marked as edited (by begin/end edit above)
      Return dt_clone

		End Function

    ' method to see if a value is one of many legal boolean choices
    Public Shared Function fnc_is_boolean_data(ByVal obj__prm As Object) As Boolean

			Dim str As String = fnc_convert_expected_string(obj__prm).ToLower

			If str.Length = 0 Then Return False

			Select Case str
				Case "true", "false", "yes", "no"
					Return True
				Case "1", "0"
					Return True
			End Select

			Return False

		End Function

		Public Shared Function fnc_is_valid_guid(ByVal obj__prm As Object) As Boolean

			Dim str As String = fnc_convert_expected_string(obj__prm)

			If str.Length = 0 Then
				Return False
			End If

      ' strip delimiter if found
      str = Replace(str, "'", "")

			Return fnc_is_valid_guid(str)

		End Function

		Public Shared Function fnc_is_valid_guid(ByVal guid__prm As String) As Boolean

			Dim str_guid As String = fnc_convert_expected_string(guid__prm)

			If str_guid.Length = 0 Then Return False

			Dim reg_is_guid As New Regex(cls_constant.str_system_constant("regex_guid"), RegexOptions.IgnoreCase)

			Return reg_is_guid.IsMatch(str_guid)

		End Function

    ' will return the absolute path without the page name
    Public Shared Function fnc_get_path_without_page(ByVal req__prm As HttpRequest, Optional ByVal bln__prm_remove_subweb As Boolean = False) As String

			Dim str_absolute_path As String = req__prm.Url.AbsolutePath
			Dim str_page As String = req__prm.Url.Segments(req__prm.Url.Segments.Length - 1)
			Dim str_return As String = Replace(str_absolute_path, str_page, "")

			If bln__prm_remove_subweb = True Then
				str_return = Replace(str_return, cls_constant.str_subweb(True), "")
			End If

			Return str_return

		End Function

		Public Shared Sub sub_set_cookie(ByVal str__prm_name As String, ByVal str__prm_value As String)

			Dim ck_local As New HttpCookie(str__prm_name)
			ck_local.Value = str__prm_value
			ck_local.Expires = Date.Now.AddYears(30)

			Current.Response.Cookies.Add(ck_local)

		End Sub

		Public Shared Function fnc_get_cookie(ByVal str__prm_name As String) As String

			If Current.Request.Cookies.Count = 0 Then
				Return ""
			End If

			If Current.Request.Cookies.Get(str__prm_name) Is Nothing Then
				Return ""
			End If

			Return fnc_convert_expected_string(Current.Request.Cookies(str__prm_name).Value)

		End Function

		Public Shared Function fnc_get_name__base(ByVal str__prm_object_name As String) As String

			Dim arr_object_name As String() = Split(str__prm_object_name, "_")
			Dim str_base_name As String = ""
			Dim str_underscore As String = ""

			Dim int_index As Int32 = 1

      ' if a role is found (__prm) then skip it
      If arr_object_name(int_index).Length = 0 Then
				int_index = 3
			End If

			For int_loop_ctr As Int32 = int_index To arr_object_name.Length - 1
        ' if a qualification is found (__manager) then name complete
        If arr_object_name(int_loop_ctr).Length = 0 Then
					Exit For
				End If
				str_base_name = str_base_name & str_underscore & arr_object_name(int_loop_ctr)
				str_underscore = "_"
			Next

			Return str_base_name

		End Function

		Public Shared Sub sub_create_system_name(ByVal str__prm_lkp_table_name As String)

      ' generate a system_name for a lookup table row where blank
      '		use name if supplied, else code value, else throw exception

      Dim ds_system_name As DataSet
			Dim ds_system_name_check As DataSet
			Dim str_lkp_table_name As String = str__prm_lkp_table_name
			Dim str_sql As String = "select * from " & str_lkp_table_name & " where system_name is null or system_name = ''"
			Dim str_pk_name As String
			Dim str_pk_value As String
			Dim str_name As String
			Dim str_system_name As String
			Dim str_code_value As String

			ds_system_name = cls_data_access_layer.fnc_get_dataset(str_sql)

			If ds_system_name.Tables(0).Rows.Count > 0 Then
				For Each dr_system_name As DataRow In ds_system_name.Tables(0).Rows
          ' for each row (no system_name), generate a system_name
          str_pk_name = fnc_convert_expected_string(dr_system_name.Table.Columns(0).ColumnName)
					str_pk_value = fnc_convert_expected_string(dr_system_name(0))
					str_name = fnc_convert_expected_string(dr_system_name("name"))
					If str_name.Length > 0 Then
            ' system name uses underscores and is all lowercase
            str_system_name = Replace(str_name, " ", "_").ToLower
					Else
						str_code_value = fnc_convert_expected_string(dr_system_name("code_value"))
						If str_code_value.Length > 0 Then
              ' system name uses underscores and is all lowercase
              str_system_name = Replace(str_code_value, " ", "_").ToLower
						Else
              ' no name or code value found, throw new exception showing table name
              Throw New Exception("While attempting to generate a system_name, no name or code_value found for lkp table: " & str__prm_lkp_table_name)
						End If
					End If
          ' check to see if system_name generated exists in database, if not insert
          '		if so, check should return ordered by system_name descending.  grab highest 
          '		number and add 1

          ' should probably be a datareader for performance reasons
          str_sql = "select system_name from " & str_lkp_table_name & " where system_name = " & cls_utility.fnc_dbwrap(str_system_name) & ";"
          ' return second result set where system_name like generated ordered descending to take top 1
          str_sql = str_sql & "select top 1 system_name from " & str_lkp_table_name & " where system_name like '" & str_system_name & "%' order by system_name desc"
					ds_system_name_check = cls_data_access_layer.fnc_get_dataset(str_sql)

          ' prepare to update system_name
          str_sql = "update " & str__prm_lkp_table_name

					If ds_system_name_check.Tables(0).Rows.Count > 0 Then
            ' take top 1 system_name, add 1 then update
            Dim arr_system_name() As String = Split(ds_system_name_check.Tables(1).Rows(0)("system_name").ToString, "_")
						str_system_name = str_system_name & "_" & fnc_convert_expected_int32(arr_system_name(arr_system_name.Length - 1)) + 1
					End If

          ' set system_name
          str_sql = str_sql & " set system_name = " & cls_utility.fnc_dbwrap(str_system_name)
          ' add where clause
          str_sql = str_sql & " where " & str_pk_name & " = " & cls_utility.fnc_dbwrap(str_pk_value)
          ' execute (update system_name)
          ' NOTE: Would prefer to batch these sql statements and run as one but in case a very large lookup table is "loaded" and then updated, it might be an enormous batched SQL statement
          cls_data_access_layer.sub_execute_non_query(str_sql)
				Next
			End If

		End Sub

    ' webconfig cannot hold values with < and >.  Use [ and ] and this will replace them
    Public Shared Function fnc_swap_config_tag(ByVal str__prm As String) As String

			If fnc_convert_expected_string(str__prm).Length = 0 Then
				Return ""
			End If

			Dim str_replace As String
      ' replace invalid webconfig html tags
      str_replace = Replace(str__prm, "[", "<")
			str_replace = Replace(str_replace, "]", ">")

			Return str_replace

		End Function

		Public Shared Function fnc_get_datatype(ByVal str__prm_table_name As String, ByVal str__prm_column_name As String) As String

			If str__prm_table_name.Length = 0 Then
				Throw New Exception("fnc_get_datatype failed - table name missing")
			End If

			If str__prm_column_name.Length = 0 Then
				Throw New Exception("fnc_get_datatype failed - column name missing")
			End If

			Dim dt_database_column As DataTable = cls_constant.dt_database_column

			Dim str_filter As String = "table_name = " & cls_utility.fnc_dbwrap(str__prm_table_name) & " and column_name = " & cls_utility.fnc_dbwrap(str__prm_column_name)
			Dim dr_table_column() As DataRow = dt_database_column.Select(str_filter)

			If dr_table_column.Length = 0 Then
				Throw New Exception("Databased column (" & str__prm_column_name & ") not found in udf_system_database_column(02) - table: " & str__prm_table_name)
			End If

			Return fnc_convert_expected_string(dr_table_column(0)("data_type")).ToLower

		End Function

		Public Shared Function fnc_get_column_attribute(ByVal ctl__prm As Control, ByVal str__prm_attribute As String) As String

			Dim str_table_name As String = ""
			Dim str_column_name As String = ""

      'str_table_name = fnc_get_control_attribute_value(ctl__prm, "table_name")
      'str_column_name = fnc_get_control_attribute_value(ctl__prm, "column_name")

      If str_table_name.Length > 0 And str_column_name.Length > 0 Then
				Return fnc_get_column_attribute(str_table_name, str_column_name, str__prm_attribute)
			End If

			Return Nothing

		End Function

		Public Shared Function fnc_get_column_attribute(ByVal str__prm_table_name As String, ByVal str__prm_column_name As String, ByVal str__prm_attribute As String) As String

			Dim dt_database_column As DataTable = cls_constant.dt_database_column

			Dim str_filter As String = "table_name = " & cls_utility.fnc_dbwrap(str__prm_table_name) & " and column_name = " & cls_utility.fnc_dbwrap(str__prm_column_name)
			Dim dr_table_column() As DataRow = dt_database_column.Select(str_filter)

			If dr_table_column.Length = 0 Then
				Throw New Exception("Databased column (" & str__prm_column_name & ") not found in udf_system_database_column(03) - table: " & str__prm_table_name)
			End If

			Dim str_attribute As String = fnc_convert_expected_string(dr_table_column(0)(str__prm_attribute))

      ' TODO if asking for an attribute and it doesn't exist - why return default hover text??
      If str_attribute.Length = 0 Then
        'str_attribute = fnc_convert_expected_string(ConfigurationManager.AppSettings("default_hover_text"))
      End If

			Return str_attribute

		End Function

    ' returns the display name (stored in tbl_table) for a passed in table
    Public Shared Function fnc_get_display_name(ByVal str__prm_table_name As String) As String

			Return ""

		End Function

    ' return a collection of column names for a table
    Public Shared Function fnc_get_column_list(ByVal str__prm_table_name As String) As StringCollection

			Dim stc_column As New StringCollection

			Dim dt_database_column As DataTable

			If cls_global.dt__pub_database_column Is Nothing Then
				Throw New Exception("dt database column missing")
			End If

			dt_database_column = cls_global.dt__pub_database_column

			Dim dr_select() As DataRow

			dr_select = dt_database_column.Select("table_name = " & cls_utility.fnc_dbwrap(str__prm_table_name))

			If dr_select.Length = 0 Then
				Throw New Exception("Table not found in dt db col - table: " & str__prm_table_name)
			End If

			For Each dr As DataRow In dr_select
				stc_column.Add(dr("column_name"))
			Next

			Return stc_column

		End Function
    ' returns the display name (stored in tbl_columm) for a passed in column
    Public Shared Function fnc_get_display_name(ByVal str__prm_table_name As String, ByVal str__prm_column_name As String) As String

			Dim dt_database_column As DataTable = cls_constant.dt_database_column

			Dim str_filter As String = "table_name = " & cls_utility.fnc_dbwrap(str__prm_table_name) & " and column_name = " & cls_utility.fnc_dbwrap(str__prm_column_name)
			Dim dr_table_column() As DataRow = dt_database_column.Select(str_filter)

			If dr_table_column.Length = 0 Then
				Throw New Exception("Databased column (" & str__prm_column_name & ") not found in udf_system_database_column(04) - table: " & str__prm_table_name)
			End If

			Return fnc_convert_expected_string(dr_table_column(0)("display_name__column"))

		End Function

    ' will convert a primary key name to a table name
    Public Shared Function fnc_table_from_primary_key(ByVal str__prm_primary_key_name As String) As String

      ' check for lookup table prefix first
      If str__prm_primary_key_name.StartsWith(cls_constant.str_primary_key_prefix__lookup) = True Then
				Return Replace(str__prm_primary_key_name, cls_constant.str_primary_key_prefix__lookup, cls_constant.str_table_prefix__lookup)
			End If

      ' check for standard table prefix
      If str__prm_primary_key_name.StartsWith(cls_constant.str_primary_key_prefix) = True Then
				Return Replace(str__prm_primary_key_name, cls_constant.str_primary_key_prefix, cls_constant.str_table_prefix)
			End If

      ' if here - this is really an error
      Throw New Exception("Could not convert primary key name passed to table name: " & str__prm_primary_key_name)

		End Function

    ' will convert a foreign key name to a table name
    Public Shared Function fnc_table_from_foreign_key(ByVal str__prm_foreign_key_name As String) As String

			Dim str_table_name__parent As String = ""

      ' check for lookup table prefix first
      If str__prm_foreign_key_name.StartsWith(cls_constant.str_foreign_key_prefix__lookup) = True Then
				str_table_name__parent = Replace(str__prm_foreign_key_name, cls_constant.str_foreign_key_prefix__lookup, cls_constant.str_table_prefix__lookup)
			End If

      ' check for standard table prefix
      If str__prm_foreign_key_name.StartsWith(cls_constant.str_foreign_key_prefix) = True Then
				str_table_name__parent = Replace(str__prm_foreign_key_name, cls_constant.str_foreign_key_prefix, cls_constant.str_table_prefix)
			End If

			If str_table_name__parent.Length = 0 Then
        ' Foreign key name doesn't start with correct prefix (fk_...)
        Throw New Exception("Could not convert foreign key name passed to table name: " & str__prm_foreign_key_name)
			End If

			Dim int_qualifier_position As Int32 = str_table_name__parent.IndexOf("__")

      ' if a foreign key has a qualifier name, remove it (i.e. fk_person__manager)
      If int_qualifier_position > 0 Then
				str_table_name__parent = str_table_name__parent.Substring(0, int_qualifier_position)
			End If

			Return str_table_name__parent

		End Function

    ' will convert a table name to a primary key name
    Public Shared Function fnc_primary_key_from_table(ByVal str__prm_table_name As String) As String

      ' check for lookup table prefix first
      If str__prm_table_name.IndexOf(cls_constant.str_table_prefix__lookup) = 0 Then
				Return Replace(str__prm_table_name, cls_constant.str_table_prefix__lookup, cls_constant.str_primary_key_prefix__lookup)
			End If

      ' check for standard table prefix
      If str__prm_table_name.IndexOf(cls_constant.str_table_prefix) = 0 Then
				Return Replace(str__prm_table_name, cls_constant.str_table_prefix, cls_constant.str_primary_key_prefix)
			End If

      ' if here - this is really an error
      Throw New Exception("Could not convert table_name passed to primary_key: " & str__prm_table_name)

		End Function

    ' will convert a table name to a primary key name
    Public Shared Function fnc_foreign_key_from_table(ByVal str__prm_table_name As String) As String

      ' check for lookup table prefix first
      If str__prm_table_name.IndexOf(cls_constant.str_table_prefix__lookup) = 0 Then
				Return Replace(str__prm_table_name, cls_constant.str_table_prefix__lookup, cls_constant.str_foreign_key_prefix__lookup)
			End If

      ' check for standard table prefix
      If str__prm_table_name.IndexOf(cls_constant.str_table_prefix) = 0 Then
				Return Replace(str__prm_table_name, cls_constant.str_table_prefix, cls_constant.str_foreign_key_prefix)
			End If

      ' if here - this is really an error
      Throw New Exception("Could not convert table_name passed to foreign_key: " & str__prm_table_name)

		End Function

    ' will convert a table name to a id name
    Public Shared Function fnc_id_from_table(ByVal str__prm_table_name As String) As String

      ' check for lookup table prefix first
      If str__prm_table_name.IndexOf(cls_constant.str_table_prefix__lookup) = 0 Then
				Return Replace(str__prm_table_name, cls_constant.str_table_prefix__lookup, cls_constant.str_id_prefix__lookup)
			End If

      ' check for standard table prefix
      If str__prm_table_name.IndexOf(cls_constant.str_table_prefix) = 0 Then
				Return Replace(str__prm_table_name, cls_constant.str_table_prefix, cls_constant.str_id_prefix)
			End If

      ' if here - this is really an error
      Throw New Exception("Could not convert id name passed to table name: " & str__prm_table_name)

		End Function

    ' returns url decoded value for querystring and key passed
    Public Shared Function fnc_get_querystring_value(ByVal nvc__prm_querystring As Specialized.NameValueCollection, ByVal str__prm_key As String) As String

			If nvc__prm_querystring Is Nothing Then
				Return ""
			End If

			If fnc_convert_expected_string(str__prm_key).Length = 0 Then
				Return ""
			End If

			Return System.Web.HttpUtility.UrlDecode(fnc_convert_expected_string(nvc__prm_querystring.Item(str__prm_key)))

		End Function

		Public Shared Function fnc_format_percentage(ByVal obj__prm As Object) As String

			Return fnc_format_percentage(obj__prm, 2)

		End Function

		Public Shared Function fnc_format_percentage(ByVal obj__prm As Object, ByVal int__prm_decimal_position As Int32) As String

			Dim str_value As String = ""
			Dim str_period As String = "."
			Dim str_zero As String = ""

			For int_ctr As Int32 = 1 To int__prm_decimal_position
				str_zero = str_period & str_zero & "0"
				str_period = ""
			Next

			str_value = fnc_convert_expected_string(obj__prm)

			If str_value.Length = 0 Then
				Return "0" & str_zero & "%"
			End If

			Return FormatPercent(str_value, int__prm_decimal_position)

		End Function

		Public Shared Function fnc_format_number(ByVal obj__prm As Object, ByVal int__prm_scale As Int32) As String

			Dim int_scale As Int32 = fnc_convert_expected_int32(int__prm_scale)
			Dim str_decimal_point As String = ""

			If int_scale > 0 Then
				str_decimal_point = "."
			End If

			If fnc_convert_expected_string(obj__prm).Length = 0 Then
				If int_scale = 0 Then Return 0
				Return "0" & str_decimal_point.PadRight(int_scale + 1, Convert.ToChar("0"))
			End If

			If fnc_convert_expected_double(obj__prm) = 0 Then
				If int_scale = 0 Then Return 0
				Return "0" & str_decimal_point.PadRight(int_scale + 1, Convert.ToChar("0"))
			End If

			Return FormatNumber(obj__prm, int__prm_scale).ToString

		End Function

		Public Shared Sub sub_load_meta_data()

      ' grab all database tables and columns including datatypes, etc for use with dynamic validation
      '' TODO should probably change this to a datatable or something smaller

      cls_data_access_layer.sub_reconcile_meta()
			cls_global.dt__pub_database_column = cls_data_access_layer.fnc_get_datatable("select * from udf_system_database_column()")

			cls_data_access_layer.fnc_validate_meta_data()

      ' grab all database column validations - i.e. account number must begin with letter, etc.
      cls_global.dt__pub_system_column_validation = cls_data_access_layer.fnc_get_datatable("select * from udf_system_column_validation()")

      ' grab all database help content - a site with many pages and/or lots of help content should store this data elsewhere.  xml on webserver, etc.
      ''      ds_meta = cls_data_access_layer.fnc_get_dataset("select * from udf_system_help()")
      ''      app__prm("dt_system_help") = ds_meta.Tables(0)

      ' load audit tables/columns to exclude in application variables
      Dim arrl As New ArrayList
			arrl.AddRange(Split(fnc_convert_expected_string(ConfigurationManager.AppSettings("audit_exclude_table_list")), ","))
			arrl.TrimToSize()
			cls_global.arrl__pub_audit_exclude_table_list = arrl.Clone
			arrl.Clear()

      ' load audit columns to exlude
      ' TODO - globally move all appsettings code to cls_constant 
      arrl.AddRange(Split(fnc_convert_expected_string(ConfigurationManager.AppSettings("audit_exclude_column_list")), ","))
			arrl.TrimToSize()
			cls_global.arrl__pub_audit_exclude_column_list = arrl.Clone
			arrl.Clear()

      ' load tbl_system_constants
      cls_global.dct__pub_system_constant = cls_data_access_layer.fnc_get_dictionary("select constant_keyword, constant_value from udf_system_constant()")


			cls_global.str__pub_database_version = cls_utility.fnc_get_database_version

      ' set default user
      cls_global.str__pub_pk_person_user__default = cls_data_access_layer.fnc_get_scaler__string("select pk_person_user from udf_default_person_user()")

      ' cache all lkp tables
      sub_refresh_lkp_cache()

		End Sub

		Public Shared Sub sub_refresh_lkp_cache()

      ' refresh all lkp table cache
      Dim dt_lkp As DataTable
			Dim str_table_name As String

      ' retrieve lkp table names
      dt_lkp = cls_data_access_layer.fnc_get_dataset("select name from sysobjects where name like '" & cls_constant.str_table_prefix__lookup & "%'").Tables(0)

      ' add each lkp table to cache
      For Each dr_lkp As DataRow In dt_lkp.Rows
				str_table_name = fnc_convert_expected_string(dr_lkp(0).ToString)
				sub_refresh_lkp_cache(str_table_name)
			Next

		End Sub

		Public Shared Sub sub_refresh_lkp_cache(ByVal str__prm_table_name As String)

			Dim str_sql As String = ""
      ''Dim str_udf As String = ""
      Dim str_pk_name As String = cls_utility.fnc_primary_key_from_table(str__prm_table_name)
			Dim str_id_name As String = Replace(str_pk_name, "pk_", "id_")

			Dim str_extra_column_list As String = ""

			If str__prm_table_name.Contains("lkp_friendly_message") Then
				str_extra_column_list = ", message_context, friendly_message, friendly_suggestion, css_class, control_id"
			End If

      ' no longer user udf with count.  initial idea was to allow delete on lookups if a value was never used.  requires all 
      '   lookup tables to have corresponding with_count udf that correctly tallies from all uses, total times it's been used
      '   when 0, you could safely allow them to delete it.
      ''str_udf = Replace(str__prm_table_name, "tbl_", "udf_")
      ''str_udf = str_udf & "_with_count()"

      If Current Is Nothing Then
				Exit Sub
			End If

			Current.Cache.Remove(str__prm_table_name)
			str_sql = "select " & str_pk_name & ", " & str_pk_name & " as pk_lkp, " & str_id_name & " as id_lkp, name, system_name, code_value, explanation, sort_order " & str_extra_column_list & " from " & str__prm_table_name & " where " & cls_constant.str_active_flag_column_name & "=1 order by sort_order, name, code_value"
			Current.Cache.Insert(str__prm_table_name, cls_data_access_layer.fnc_get_datatable(str_sql), Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(30))

		End Sub

		Public Shared Sub sub_refresh_person_user_application_role()

			Dim str_sql As String = ""

			Current.Cache.Remove("person_user_application_role")
			str_sql = "select * from udf_person_user_application_role() where fk_lkp_application_role is not null and fk_person_user is not null"
			Current.Cache.Insert("person_user_application_role", cls_data_access_layer.fnc_get_datatable(str_sql), Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(30))

		End Sub

		Public Shared Function fnc_get_base_name(ByVal str__prm_object_name As String) As String

      ' TODO this needs to break apart an object by the 4 pieces and return the base name
      ' str__prm_first_name__manager should return first_name

      Return "method neeeds to be fixed"

      ' remove any qualification names from object name (example: fk_person__manager)
      Dim arr_fk() As String = Split(str__prm_object_name, "_")
			Dim int_arr_count As Int32 = arr_fk.Length

      ' TODO sending in str__prm_first_name will not work but should.  
      '		-- need to ignore double underscores folling object type prefix
      ' qualifications are always the last 'name' after the last set of double underscores

      If int_arr_count <= 2 Then
				Return arr_fk(0)
			End If

			Return arr_fk(arr_fk.Length - 1)

		End Function

		Public Function fnc_get_base_column_name(ByVal str__prm_column_name As String) As String

      ' TODO this needs to break apart an object by the 4 pieces and return the base name
      ' str__prm_first_name__manager should return first_name

      Dim str_base_name As String = str__prm_column_name

      ' remove any key prefix names
      str_base_name = Replace(str_base_name, cls_constant.str_primary_key_prefix, "")
			str_base_name = Replace(str_base_name, cls_constant.str_primary_key_prefix__lookup, "")
			str_base_name = Replace(str_base_name, cls_constant.str_foreign_key_prefix, "")
			str_base_name = Replace(str_base_name, cls_constant.str_foreign_key_prefix__lookup, "")

      ' remove any qualification names from object name (example: fk_person__manager)
      Dim arr_fk() As String = Split(str_base_name, "__")

			Return arr_fk(0)

		End Function

		Public Shared Function fnc_findcast__literal(ByVal ctl__prm As Control, ByVal str__prm_id_to_find As String) As Literal

			Dim ctl As Control

			ctl = fnc_find_nested_control(ctl__prm, str__prm_id_to_find)

			If ctl Is Nothing Then
				Return Nothing
			End If

			If TypeName(ctl).ToLower <> "literal" Then
				Throw New Exception("Control found but was not of type literal" & ctl.ID)
			End If

			Return DirectCast(ctl, Literal)

		End Function

		Public Shared Function fnc_findcast__button(ByVal ctl__prm As Control, ByVal str__prm_id_to_find As String) As Button

			Dim ctl As Control

			ctl = fnc_find_nested_control(ctl__prm, str__prm_id_to_find)

			If ctl Is Nothing Then
				Return Nothing
			End If

			If TypeName(ctl).ToLower <> "button" Then
				Throw New Exception("Control found but was not of type button" & ctl.ID)
			End If

			Return DirectCast(ctl, Button)

		End Function

		Public Shared Function fnc_findcast__label(ByVal ctl__prm As Control, ByVal str__prm_id_to_find As String) As Label

			Dim ctl As Control

			ctl = fnc_find_nested_control(ctl__prm, str__prm_id_to_find)

			If ctl Is Nothing Then
				Return Nothing
			End If

			If TypeName(ctl).ToLower <> "label" Then
				Throw New Exception("Control found but was not of type label" & ctl.ID)
			End If

			Return DirectCast(ctl, Label)

		End Function


		Public Shared Function fnc_findcast__usercontrol(ByVal ctl__prm As Control, ByVal str__prm_id_to_find As String) As UserControl

			Dim ctl As Control

			ctl = fnc_find_nested_control(ctl__prm, str__prm_id_to_find)

			If ctl Is Nothing Then
				Return Nothing
			End If

			If TypeName(ctl).ToLower <> "usercontrol" Then
				Throw New Exception("Control found but was not of type textbox" & ctl.ID)
			End If

			Return DirectCast(ctl, UserControl)

		End Function

		Public Shared Function fnc_findcast__textbox(ByVal ctl__prm As Control, ByVal str__prm_id_to_find As String) As TextBox

			Dim ctl As Control

			ctl = fnc_find_nested_control(ctl__prm, str__prm_id_to_find)

			If ctl Is Nothing Then
				Return Nothing
			End If

			If TypeName(ctl).ToLower <> "textbox" Then
				Throw New Exception("Control found but was not of type textbox" & ctl.ID)
			End If

			Return DirectCast(ctl, TextBox)

		End Function

		Public Shared Function fnc_findcast__linkbutton(ByVal ctl__prm As Control, ByVal str__prm_id_to_find As String) As LinkButton

			Dim ctl As Control

			ctl = fnc_find_nested_control(ctl__prm, str__prm_id_to_find)

			If ctl Is Nothing Then
				Return Nothing
			End If

			If TypeName(ctl).ToLower <> "linkbutton" Then
				Throw New Exception("Control found but was not of type linkbutton" & ctl.ID)
			End If

			Return DirectCast(ctl, LinkButton)

		End Function

		Public Shared Function fnc_findcast__dropdownlist(ByVal ctl__prm As Control, ByVal str__prm_id_to_find As String) As DropDownList

			Dim ctl As Control

			ctl = fnc_find_nested_control(ctl__prm, str__prm_id_to_find)

			If ctl Is Nothing Then
				Return Nothing
			End If

			If TypeName(ctl).ToLower <> "dropdownlist" Then
				Throw New Exception("Control found but was not of type dropdownlist" & ctl.ID)
			End If

			Return DirectCast(ctl, DropDownList)

		End Function

		Public Shared Function fnc_findcast__radiobutton(ByVal ctl__prm As Control, ByVal str__prm_id_to_find As String) As RadioButton

			Dim ctl As Control

			ctl = fnc_find_nested_control(ctl__prm, str__prm_id_to_find)

			If ctl Is Nothing Then
				Return Nothing
			End If

			If TypeName(ctl).ToLower <> "radiobutton" Then
				Throw New Exception("Control found but was not of type radiobutton" & ctl.ID)
			End If

			Return DirectCast(ctl, RadioButton)

		End Function

		Public Shared Function fnc_findcast__radiobuttonlist(ByVal ctl__prm As Control, ByVal str__prm_id_to_find As String) As RadioButtonList

			Dim ctl As Control

			ctl = fnc_find_nested_control(ctl__prm, str__prm_id_to_find)

			If ctl Is Nothing Then
				Return Nothing
			End If

			If TypeName(ctl).ToLower <> "radiobuttonlist" Then
				Throw New Exception("Control found but was not of type radiobuttonlist" & ctl.ID)
			End If

			Return DirectCast(ctl, RadioButtonList)

		End Function

		Public Shared Function fnc_findcast__panel(ByVal ctl__prm As Control, ByVal str__prm_id_to_find As String) As Panel

			Dim ctl As Control

			ctl = fnc_find_nested_control(ctl__prm, str__prm_id_to_find)

			If ctl Is Nothing Then
				Return Nothing
			End If

			If TypeName(ctl).ToLower <> "panel" Then
				Throw New Exception("Control found but was not of type panel" & ctl.ID)
			End If

			Return DirectCast(ctl, Panel)

		End Function

		Public Shared Function fnc_findcast__repeater(ByVal ctl__prm As Control, ByVal str__prm_id_to_find As String) As Repeater

			Dim ctl As Control

			ctl = fnc_find_nested_control(ctl__prm, str__prm_id_to_find)

			If ctl Is Nothing Then
				Return Nothing
			End If

			If TypeName(ctl).ToLower <> "repeater" Then
				Throw New Exception("Control found but was not of type panel" & ctl.ID)
			End If

			Return DirectCast(ctl, Repeater)

		End Function

		Public Shared Function fnc_findcast__placeholder(ByVal ctl__prm As Control, ByVal str__prm_id_to_find As String) As PlaceHolder

			Dim ctl As Control

			ctl = fnc_find_nested_control(ctl__prm, str__prm_id_to_find)

			If ctl Is Nothing Then
				Return Nothing
			End If

			If TypeName(ctl).ToLower <> "placeholder" Then
				Throw New Exception("Control found but was not of type placeholder" & ctl.ID)
			End If

			Return DirectCast(ctl, PlaceHolder)

		End Function

		Public Shared Function fnc_findcast__contentplaceholder(ByVal ctl__prm As Control, ByVal str__prm_id_to_find As String) As ContentPlaceHolder

			Dim ctl As Control

			ctl = fnc_find_nested_control(ctl__prm, str__prm_id_to_find)

			If ctl Is Nothing Then
				Return Nothing
			End If

			If TypeName(ctl).ToLower <> "contentplaceholder" Then
				Throw New Exception("Control found but was not of type contentplaceholder" & ctl.ID)
			End If

			Return DirectCast(ctl, ContentPlaceHolder)

		End Function

    ''Private Shared str__prv_button_list As String = ""
    ''Private Shared str__prv_delimiter As String = ";"
    ''Public Shared Function fnc_get_button_list_by_command_name(ByVal ctl__prm As Control, ByVal str__prm_command_name As String, Optional ByVal bln__prm_contains As Boolean = False) As String

    ''  If ctl__prm Is Nothing Then Return Nothing
    ''  If fnc_convert_expected_string(str__prm_command_name).Length = 0 Then Return Nothing

    ''  str__prv_button_list = ""
    ''  str__prv_delimiter = ""

    ''  ' first check starts with
    ''  If bln__prm_contains = True Then
    ''    ' check to see if current control is "the one"
    ''    If fnc_get_control_command_name(ctl__prm).ToLower.Contains(str__prm_command_name.ToLower) Then
    ''      str__prv_button_list = str__prv_button_list & str__prv_delimiter & ctl__prm.ClientID
    ''      str__prv_delimiter = ";"
    ''    End If
    ''  End If

    ''  ' check to see if current control is "the one"
    ''  If fnc_get_control_command_name(ctl__prm).ToLower = str__prm_command_name.ToLower Then
    ''    str__prv_button_list = str__prv_button_list & str__prv_delimiter & ctl__prm.ClientID
    ''    str__prv_delimiter = ";"
    ''  End If

    ''  Dim inst_utility As New cls_utility
    ''  inst_utility.sub_find_nested_control_list__by_command_name(ctl__prm, str__prm_command_name, bln__prm_contains)

    ''  Return str__prv_button_list

    ''End Function

    '' changed from a function to a sub because I couldn't get the recursion to stop once the control was found
    'Public Sub sub_find_nested_control_list__by_command_name(ByVal ctl__prm As Control, ByVal str__prm_command_name As String, Optional ByVal bln__prm_contains As Boolean = False)

    '  For Each ctl As Control In ctl__prm.Controls

    '    If bln__prm_contains = True Then
    '      If fnc_get_control_command_name(ctl).ToLower.Contains(str__prm_command_name.ToLower) Then
    '        str__prv_button_list = str__prv_button_list & str__prv_delimiter & ctl.ClientID
    '        str__prv_delimiter = ";"
    '      End If
    '    End If

    '    If fnc_get_control_command_name(ctl).ToLower = str__prm_command_name.ToLower Then
    '      str__prv_button_list = str__prv_button_list & str__prv_delimiter & ctl.ClientID
    '      str__prv_delimiter = ";"
    '    End If

    '    ' Recurse function
    '    If ctl.Controls.Count > 0 Then
    '      sub_find_nested_control_list__by_command_name(ctl, str__prm_command_name, bln__prm_contains)
    '    End If
    '  Next

    'End Sub

    Protected ctl__prt_found As Control
		Public Shared Function fnc_find_nested_control(ByVal ctl__prm As Control, ByVal str__prm_id_to_find As String) As Control

			' make sure control and id to find are useable
			If ctl__prm Is Nothing Then Return Nothing
			If fnc_convert_expected_string(str__prm_id_to_find).Length = 0 Then Return Nothing

			' check to see if current control is "the one"
			If ctl__prm.ID = str__prm_id_to_find Or ctl__prm.UniqueID = str__prm_id_to_find Then
				Return ctl__prm
			End If

			Dim inst_utility As New cls_utility
			inst_utility.ctl__prt_found = Nothing

			inst_utility.sub__prv_find_nested_control(ctl__prm, str__prm_id_to_find)

			Return inst_utility.ctl__prt_found

		End Function

		'' changed from a function to a sub because I couldn't get the recursion to stop once the control was found
		Private Sub sub__prv_find_nested_control(ByVal ctl__prm As Control, ByVal str__prm_id_to_find As String)

			' found already, in recursive loop, exit sub
			If ctl__prt_found Is Nothing = False Then Exit Sub

			For Each ctl As Control In ctl__prm.Controls
				If ctl.ID = str__prm_id_to_find Or ctl.UniqueID = str__prm_id_to_find Then
					ctl__prt_found = ctl
					Exit For
				End If

				' Recurse function
				If ctl.Controls.Count > 0 Then
					sub__prv_find_nested_control(ctl, str__prm_id_to_find)
				End If
			Next

		End Sub

		'Protected ctl__prt_nested__by_attribute_name As Control
		'Public Shared Function fnc_find_nested_control__by_attribute_name(ByVal ctl__prm As Control, ByVal str__prm_attribute_name As String) As Control

		'  If ctl__prm Is Nothing Then Return Nothing
		'  If str__prm_attribute_name Is Nothing Then Return Nothing

		'  ' check to see if current control is "the one"
		'  If fnc_verify_control_has_attribute(ctl__prm, str__prm_attribute_name) Then
		'    Return ctl__prm
		'  End If

		'  Dim inst_utility As New cls_utility
		'  inst_utility.ctl__prt_nested__by_attribute_name = Nothing

		'  inst_utility.sub__prv_find_nested__control__by_attribute_name(ctl__prm, str__prm_attribute_name)

		'  Return inst_utility.ctl__prt_nested__by_attribute_name

		'End Function

		'' changed from a function to a sub because I couldn't get the recursion to stop once the control was found
		'Private Sub sub__prv_find_nested__control__by_attribute_name(ByVal ctl__prm As Control, ByVal str__prm_attribute_name As String)

		'  ' found already, in recursive loop, exit sub
		'  If ctl__prt_nested__by_attribute_name Is Nothing = False Then Exit Sub

		'  For Each ctl As Control In ctl__prm.Controls
		'    If fnc_verify_control_has_attribute(ctl, str__prm_attribute_name) Then
		'      ctl__prt_nested__by_attribute_name = ctl
		'      Exit For
		'    End If

		'    ' Recurse function
		'    If ctl.Controls.Count > 0 Then
		'      sub__prv_find_nested__control__by_attribute_name(ctl, str__prm_attribute_name)
		'    End If
		'  Next

		'End Sub

		'Protected ctl__prt_nested__command_name As Control
		'Public Shared Function fnc_find_nested_control__by_command_name(ByVal ctl__prm As Control, ByVal str__prm_command_name As String) As Control

		'  If ctl__prm Is Nothing Then Return Nothing
		'  If str__prm_command_name Is Nothing Then Return Nothing

		'  ' check to see if current control is "the one"
		'  If fnc_get_control_command_name(ctl__prm).ToLower = str__prm_command_name.ToLower Then
		'    Return ctl__prm
		'  End If

		'  Dim inst_utility As New cls_utility
		'  inst_utility.ctl__prt_nested__command_name = Nothing

		'  inst_utility.sub__prv_find_nested__control__by_command_name(ctl__prm, str__prm_command_name)

		'  Return inst_utility.ctl__prt_nested__command_name

		'End Function

		'' changed from a function to a sub because I couldn't get the recursion to stop once the control was found
		'Private Sub sub__prv_find_nested__control__by_command_name(ByVal ctl__prm As Control, ByVal str__prm_command_name As String)

		'  ' found already, in recursive loop, exit sub
		'  If ctl__prt_nested__command_name Is Nothing = False Then Exit Sub

		'  For Each ctl As Control In ctl__prm.Controls

		'    If fnc_get_control_command_name(ctl).ToLower = str__prm_command_name.ToLower Then
		'      ctl__prt_nested__command_name = ctl
		'      Exit For
		'    End If

		'    ' Recurse function
		'    If ctl.Controls.Count > 0 Then
		'      sub__prv_find_nested__control__by_command_name(ctl, str__prm_command_name)
		'    End If
		'  Next

		'End Sub

		Protected ctl__prt_parent As Control
		Public Shared Function fnc_find_parent_control(ByVal ctl__prm As Control, ByVal str__prm_typename As String) As Control

			If ctl__prm Is Nothing Then Return Nothing
			If fnc_convert_expected_string(str__prm_typename).Length = 0 Then Return Nothing

			' check to see if current control is "the one"
			If TypeName(ctl__prm).ToLower = str__prm_typename.ToLower Then
				Return ctl__prm
			End If

			Dim inst_utility As New cls_utility
			inst_utility.ctl__prt_parent = Nothing

			inst_utility.sub__prv_find_parent_control(ctl__prm, str__prm_typename)

			Return inst_utility.ctl__prt_parent

		End Function

		' look through parental chain for control of requested typename
		Private Sub sub__prv_find_parent_control(ByVal ctl__prm As Control, ByVal str__prm_typename As String)

			' found already, in recursive loop, exit sub
			If ctl__prt_parent Is Nothing = False Then Exit Sub

			' if there are no more parents, exit sub
			If ctl__prm.Parent Is Nothing = True Then Exit Sub

			Dim ctl As Control
			ctl = ctl__prm.Parent

			' check to see if current control is "the one"
			If TypeName(ctl).ToLower = str__prm_typename.ToLower Then
				ctl__prt_parent = ctl
				Exit Sub
			End If

			' call again for next parent
			sub__prv_find_parent_control(ctl, str__prm_typename)

		End Sub

		'' finds parent control by id with starts with option
		'' TODO create method to find parent by object type (textbox, ns_databsaed.row, etc.)
		'' TODO delete fnc_find_parent_control as it uses private variable - code is in use though.
		'Public Shared Function fnc_find_parental_control(ByVal ctl__prm As Control, ByVal str__prm_id_to_find As String, Optional ByVal bln__prm_id_starts_with As Boolean = False) As Control

		'  Dim ctl As Control = ctl__prm

		'  For int_loop_ctr As Int32 = 1 To 40

		'    ' check to see if current control is "the one"
		'    If ctl.ID = str__prm_id_to_find Then
		'      Return ctl
		'    End If

		'    ' check to see if current control starts with and therefore is the one
		'    If bln__prm_id_starts_with = True AndAlso ctl.ID.StartsWith(str__prm_id_to_find) = True Then
		'      Return ctl
		'    End If

		'    If ctl.Parent Is Nothing = True Then Return Nothing

		'    ctl = ctl.Parent

		'    int_loop_ctr = int_loop_ctr + 1
		'  Next

		'  Return Nothing

		'End Function

		Public Shared Function fnc_convert_datatype(ByVal str__prm_datatype As String) As String

			Select Case str__prm_datatype
				Case "nvarchar", "varchar"
					Return "string"
				Case "date", "time", "datetime"
					Return "date"
				Case "numeric", "decimal", "float", "money", "smallmoney", "real", "bigint", "int", "smallint", "tinyint"
					Return "number"
				Case "bit"
					Return "boolean"
			End Select

			Return str__prm_datatype

		End Function
		Private Shared Function fnc_get_InternetExplorerVersion() As Integer

      'Returns the version of Internet Explorer or a -1 (indicating the use of another browser).

      Dim rv As Integer = -1
			Dim bc As HttpBrowserCapabilities = Current.Request.Browser

			If bc.Browser = "IE" Then
				rv = bc.MajorVersion + bc.MinorVersion
			End If

			Return rv
		End Function

		Public Shared Function fnc_compatible_browser() As Boolean

			Dim ver As Integer = fnc_get_InternetExplorerVersion()

			If (ver > 0) Then '' IE
        If (ver >= 7) Then
					Return True
				Else
					Return False
				End If
			Else
        ' not using Internet Explorer. any version is compatible
        Return True
			End If

		End Function

    'Public Shared Sub sub_log_browser_capability(ByRef inst__prm_cls_log_security As cls_log_security)

    '  Dim bc As HttpBrowserCapabilities
    '  bc = Current.Request.Browser

    '  inst__prm_cls_log_security.sub_log_security_parameter("Browser Type", bc.Type)
    '  inst__prm_cls_log_security.sub_log_security_parameter("Browser Name", bc.Browser)
    '  inst__prm_cls_log_security.sub_log_security_parameter("Browser Version", bc.Version)
    '  inst__prm_cls_log_security.sub_log_security_parameter("Browser Major Version", bc.MajorVersion)
    '  inst__prm_cls_log_security.sub_log_security_parameter("Browser Minor Version", bc.MinorVersion)
    '  inst__prm_cls_log_security.sub_log_security_parameter("Browser Platform", bc.Platform)
    '  inst__prm_cls_log_security.sub_log_security_parameter("Browser Is Beta", bc.Beta)
    '  inst__prm_cls_log_security.sub_log_security_parameter("Browser Supports Cookies", bc.Cookies)
    '  inst__prm_cls_log_security.sub_log_security_parameter("Browser Supports Javascript", bc.JavaScript)
    '  inst__prm_cls_log_security.sub_log_security_parameter("Browser Supports ActiveXControls", bc.ActiveXControls)


    'End Sub

    Public Shared Function fnc_match_regular_expression(ByVal str__prm As String, ByVal str__prm_regex As String) As String

			Dim str_value As String = fnc_convert_expected_string(str__prm)
			Dim str_regex As String = fnc_convert_expected_string(str__prm_regex)

			If str_value.Length = 0 Then Return True
			If str_regex.Length = 0 Then Return True

			Dim str As String = ""

			Dim reg_rule As New System.Text.RegularExpressions.Regex(str_regex)
			For Each reg_match As System.Text.RegularExpressions.Match In reg_rule.Matches(str_value)
				str = str & reg_match.ToString
			Next

			Return str

		End Function

		Public Shared Function fnc_get_regular_expression_invalid_data(ByVal str__prm As String, ByVal str__prm_regex As String) As String

			If fnc_validate_regular_expression(str__prm, str__prm_regex) = True Then
				Return ""
			End If

			Dim str_input As String = fnc_convert_expected_string(str__prm)
			Dim str_output As String = str_input

			Dim str_regex As String = fnc_convert_expected_string(str__prm_regex)

			If str_input.Length = 0 Then Return ""
			If str_regex.Length = 0 Then Return ""

			Dim str As String = ""

			Dim reg_rule As New System.Text.RegularExpressions.Regex(str_regex)

			For Each str_character As Char In str_input.ToCharArray
				For Each reg_match As System.Text.RegularExpressions.Match In reg_rule.Matches(str_character)
					str_output = Replace(str_output, reg_match.ToString, "")
				Next
			Next

			Return str_output

		End Function

		Public Shared Function fnc_validate_regular_expression(ByVal str__prm As String, ByVal str__prm_regex As String) As String

			Dim str_value As String = fnc_convert_expected_string(str__prm)
			Dim str_regex As String = fnc_convert_expected_string(str__prm_regex)

			If str_value.Length = 0 Then Return True
			If str_regex.Length = 0 Then Return True

			Dim str As String = ""

			Dim reg_number As New System.Text.RegularExpressions.Regex(str_regex)
			For Each reg_match As System.Text.RegularExpressions.Match In reg_number.Matches(str_value)
				str = str & reg_match.ToString
			Next

			If str = str_value Then
				Return True
			End If

			Return False

		End Function

		Public Shared Function fnc_return_regex_match(obj__prm As Object, str__prm_regex As String) As String

			Dim str_value As String = fnc_convert_expected_string(obj__prm)
			Dim str_regex As String = fnc_convert_expected_string(str__prm_regex)

			If str_value.Length = 0 Then Return ""
			If str_regex.Length = 0 Then Return ""

			Dim str As String = ""

			Dim reg_number As New System.Text.RegularExpressions.Regex(str_regex)
			For Each reg_match As System.Text.RegularExpressions.Match In reg_number.Matches(str_value)
				str = str & reg_match.ToString
			Next

			Return str

		End Function

		Public Shared Function fnc_get_application_version() As String

			Dim assembly As System.Reflection.Assembly
			Dim sb As New StringBuilder

      ' Load the assembly info   
      assembly = System.Reflection.Assembly.Load("App_Code")

      ' Iterate through all the attributes for the assembly.   
      For Each attr As Attribute In Attribute.GetCustomAttributes(assembly)

        ' Append each value to the StringBuilder   
        Select Case attr.GetType.ToString

					Case "System.Reflection.AssemblyTitleAttribute"
						sb.Append("Title: " & CType(attr, AssemblyTitleAttribute).Title & "<br>" & vbCrLf)

					Case "System.Reflection.AssemblyDescriptionAttribute"
						sb.Append("Description: " & CType(attr, AssemblyDescriptionAttribute).Description & "<br>" & vbCrLf)

					Case "System.Reflection.AssemblyCompanyAttribute"
						sb.Append("Company: " & CType(attr, AssemblyCompanyAttribute).Company & "<br>" & vbCrLf)

					Case "System.Reflection.AssemblyProductAttribute"
						sb.Append("Product: " & CType(attr, AssemblyProductAttribute).Product & "<br>" & vbCrLf)

					Case "System.CLSCompliantAttribute"
						sb.Append("CLS Compliant: " & CType(attr, CLSCompliantAttribute).IsCompliant & "<br>" & vbCrLf)

					Case "System.Runtime.InteropServices.GuidAttribute"
						sb.Append("Guid: " & CType(attr, System.Runtime.InteropServices.GuidAttribute).Value & "<br>" & vbCrLf)

				End Select

			Next

      ' Add the version info   
      sb.Append(assembly.GetName().Version.ToString())

			Return sb.ToString

		End Function

		Public Shared Function fnc_get_database_version() As String

			Return cls_data_access_layer.fnc_get_scaler__string("select top 1 release_number from tbl_system_release order by id_system_release desc")

		End Function

		Public Shared Function fnc_get_control_id_sender(ByVal pg__prm As Page) As String

			Dim ctl As Control = fnc_get_control_sender(pg__prm)

			If ctl Is Nothing Then
				Return ""
			End If

			Dim str_command_name As String = ""
			Dim str_command_argument As String = ""

			Select Case TypeName(ctl).ToLower
				Case "linkbutton"
					Dim ctl_linkbutton As LinkButton = DirectCast(ctl, LinkButton)
					str_command_name = ctl_linkbutton.CommandName
					str_command_argument = ctl_linkbutton.CommandArgument
				Case "button"
					Dim ctl_button As Button = DirectCast(ctl, Button)
					str_command_name = ctl_button.CommandName
					str_command_argument = ctl_button.CommandArgument
				Case "imagebutton"
					Dim ctl_imagebutton As ImageButton = DirectCast(ctl, ImageButton)
					str_command_name = ctl_imagebutton.CommandName
					str_command_argument = ctl_imagebutton.CommandArgument
				Case Else
					Return ""
			End Select

			Return fnc_convert_expected_string(ctl.ID) & IIf(str_command_name.Length > 0, ":" & str_command_name, "") & IIf(str_command_argument.Length > 0, ":" & str_command_argument, "")

		End Function

		Public Shared Function fnc_get_control_sender(ByVal pg__prm As Page) As Control

			Dim str_event_target As String = fnc_convert_expected_string(pg__prm.Request.Params.Get("__EVENTTARGET"))
      'Dim str_event_target As String = ""

      Dim ctl As Control = Nothing

			If str_event_target.Length = 0 Then
				For Each str_form As String In pg__prm.Request.Form
					ctl = pg__prm.FindControl(str_form)
					If TypeOf (ctl) Is Button Then
						Exit For
					End If
					If TypeOf (ctl) Is LinkButton Then
						Exit For
					End If
				Next
			Else
				ctl = pg__prm.FindControl(str_event_target)
			End If

			Return ctl

		End Function

    'Public Shared Function fnc_count_control_with_attribute(ByVal ctl__prm_container As Control, ByVal str__prm_attribuate_name As String, ByVal str__prm_attribute_value As String) As Int32

    '  Dim inst_utility As New cls_utility
    '  inst_utility.int__prv_count = 0

    '  inst_utility.sub__prv_count_control_with_attribute(ctl__prm_container, str__prm_attribuate_name, str__prm_attribute_value)

    '  Return inst_utility.int__prv_count

    'End Function

    'Protected int__prv_count As Int32 = 0
    'Private Sub sub__prv_count_control_with_attribute(ByVal ctl__prm_container As Control, ByVal str__prm_attribuate_name As String, ByVal str__prm_attribute_value As String)

    '  For Each ctl In ctl__prm_container.Controls

    '    If fnc_get_control_attribute_value(ctl, str__prm_attribuate_name) = str__prm_attribute_value Then
    '      int__prv_count = int__prv_count + 1
    '    End If

    '    ' Recurse function
    '    If ctl.Controls.Count > 0 Then
    '      sub__prv_count_control_with_attribute(ctl, str__prm_attribuate_name, str__prm_attribute_value)
    '    End If

    '  Next

    'End Sub

    'Public Shared Function fnc_get_image_path(ByVal str__prm_page_path As String) As String

    '  Dim str As String = Nothing
    '  Dim str_split() As String = Nothing

    '  ' nothing(blank) passed in
    '  str = fnc_convert_expected_string(str__prm_page_path)
    '  If str.Length = 0 Then Return ""

    '  ' replace all slashes with the same type of slash
    '  str = fnc_clean_path(str__prm_page_path)

    '  ' split on slash
    '  str_split = str__prm_page_path.Split("/")

    '  ' there should be at least two items in array or else we cannot use it
    '  If str_split.Length < 2 Then Return ""

    '  ' pages named with periods, images with underscores
    '  str = cls_controller.fnc_get_root() & "dir_image/dir_mod/dir_" & Replace(str_split(2), ".", "_")

    '  Return str

    'End Function

    '' set visibility to true if control is entry type control and false if other
    'Public Shared Sub sub_show_entry_webcontrol(ByVal ctl__prm As Control)

    '  For Each ctl As Control In ctl__prm.Controls

    'Dim str_typename As String = TypeName(ctl)
    '    Select Case str_typename.ToLower
    '      Case "label"
    '        ctl.Visible = False
    '      Case "textbox", "checkboxlist", "radiobuttonlist", "dropdownlist"
    '        ctl.Visible = True
    '      Case "checkbox"
    '        ctl.Visible = True
    '' this is a bit of a hack.  can certainly take this out.
    ''DirectCast(ctl, CheckBox).Text = DirectCast(ctl, CheckBox).Text & "<br />"
    '    End Select

    '' Recurse function
    '    If ctl.Controls.Count > 0 Then
    '      sub_show_entry_webcontrol(ctl)
    '    End If
    '  Next

    'End Sub

    '' set visibility to false if control value is empty
    'Public Shared Sub sub_hide_empty_webcontrol(ByVal ctl__prm As Control)

    '  For Each ctl As Control In ctl__prm.Controls

    '    Dim str_typename As String = TypeName(ctl)
    '    Select Case str_typename.ToLower
    '      Case "textbox", "label", "checkbox", "checkboxlist", "radiobuttonlist", "dropdownlist"
    '        If fnc_get_control_value(ctl).Length = 0 Then
    '          ctl.Visible = False
    '        End If
    '    End Select

    '    ' Recurse function
    '    If ctl.Controls.Count > 0 Then
    '      sub_hide_empty_webcontrol(ctl)
    '    End If
    '  Next

    'End Sub

    '' set visibility to false if control value is empty
    'Public Shared Sub sub_hide_empty_control(ByVal ctl__prm As Control)

    '  For Each ctl As Control In ctl__prm.Controls

    '    If fnc_get_control_value(ctl).Length = 0 Then
    '      ctl.Visible = False
    '    End If

    '    ' Recurse function
    '    If ctl.Controls.Count > 0 Then
    '      sub_hide_empty_control(ctl)
    '    End If
    '  Next

    'End Sub

    '' will add any value to a control (usually a <br />
    'Public Shared Sub sub_add_extend_control_value(ByVal ctl__prm As Control, ByVal bln__prm_extend_only_empty As Boolean, ByVal str__prm_value_to_extend As String)

    '  For Each ctl As Control In ctl__prm.Controls

    '    If (bln__prm_extend_only_empty = False Or fnc_get_control_value(ctl).Length > 0) And ctl.Visible = True Then
    '      sub_set_control_value(ctl, fnc_get_control_value(ctl) & str__prm_value_to_extend)
    '    End If

    '    ' Recurse function
    '    If ctl.Controls.Count > 0 Then
    '      sub_add_extend_control_value(ctl, bln__prm_extend_only_empty, str__prm_value_to_extend)
    '    End If
    '  Next

    'End Sub

    Public Shared Function fnc_pad_zero(ByVal int__prm_input As Int32, ByVal int__prm_pad_length As Int32) As String

      ' if less than 1 return empty string
      If int__prm_pad_length < 1 Then Return ""
      ' max out at 50.
      If int__prm_pad_length > 50 Then int__prm_pad_length = 50

			Dim str_input As String = fnc_convert_expected_string(int__prm_input)

			Dim str_pad As String = ""
			For int_ctr As Int32 = 1 To int__prm_pad_length
				str_pad = str_pad & "0"
			Next

			Return str_pad.Substring(0, int__prm_pad_length - str_input.Length) & str_input

		End Function

		Public Shared Function fnc_truncate_long_text(ByVal str__prm_value As String, ByVal int__prm_truncate_length As Int32) As String

			Dim str_value As String = fnc_convert_expected_string(str__prm_value)

			If str_value.Length > int__prm_truncate_length Then
				Return str_value.Substring(0, int__prm_truncate_length) & cls_constant.str_truncate_long_text
			End If

			Return str_value

		End Function

		Public Shared Sub sub_recurse_for_testing(ByVal ctl__prm As Control)

			For Each ctl As Control In ctl__prm.Controls

				Diagnostics.Debug.WriteLine("Check what you'd like here")

				If ctl.Controls.Count > 0 Then
					sub_recurse_for_testing(ctl)
				End If

			Next

		End Sub

		Public Shared Function fnc_append_if_empty(ByVal str__prm As String, ByVal str__prm_append As String) As String

			If fnc_convert_expected_string(str__prm).Length = 0 Then
				Return str__prm & str__prm_append
			End If

			Return str__prm

		End Function

		Public Shared Function fnc_append_if_not_empty(ByVal str__prm As String, ByVal str__prm_append As String) As String

			If fnc_convert_expected_string(str__prm).Length > 0 Then
				Return str__prm & str__prm_append
			End If

			Return str__prm

		End Function

		Public Shared Function fnc_check_equality(ByVal str__prm_1 As String, ByVal str__prm_2 As String, ByVal str__prm_datatype As String) As Boolean

      ' if values are the same, return true - done
      If fnc_convert_expected_string(str__prm_1) = fnc_convert_expected_string(str__prm_2) Then
				Return True
			End If

      ' hackish dealing with numbers with decimals (1.00 vs 1)
      Select Case str__prm_datatype
				Case "numeric", "decimal", "float", "money", "smallmoney", "real", "bigint", "int", "smallint", "tinyint"
					If fnc_convert_expected_double(str__prm_1) = fnc_convert_expected_double(str__prm_2) Then
						Return True
					End If
			End Select

      ' otherwise, hack time to check datetime datatypes which are sent to the database
      '		in one format and returned in another
      If str__prm_datatype.IndexOf("date") >= 0 Then
        ' Major hack as cannot figure out how to compare pre-update (already formatted for DB storage)
        '		with just selected which comes back as raw date.  UI element stores date format but don't
        '		know exactly which UI element it belongs to since page object to find control is
        '		not currently available.  Also, there could be many onscreen but assumption (bad) is
        '		they are all the same date format.
        If fnc_convert_expected_datetime__string(str__prm_1, DateFormat.GeneralDate) = fnc_convert_expected_string(str__prm_2) _
				Or fnc_convert_expected_datetime__string(str__prm_1, DateFormat.LongDate) = fnc_convert_expected_string(str__prm_2) _
				Or fnc_convert_expected_datetime__string(str__prm_1, DateFormat.LongTime) = fnc_convert_expected_string(str__prm_2) _
				Or fnc_convert_expected_datetime__string(str__prm_1, DateFormat.ShortDate) = fnc_convert_expected_string(str__prm_2) _
				Or fnc_convert_expected_datetime__string(str__prm_1, DateFormat.ShortTime) = fnc_convert_expected_string(str__prm_2) _
				Then
          ' after hack format check - they are the same datetime
          Return True
				Else
					Return False
				End If
			End If

      ' more hacking as bit datatypes are returned as true or false from database
      '		and 0 or 1 onscreen
      If str__prm_datatype = "bit" Then
				If fnc_convert_expected_boolean(str__prm_1) = fnc_convert_expected_boolean(str__prm_2) Then
          ' if they both convert to same boolean value - return true
          Return True
				Else
					Return False
				End If
			End If

      ' can you say a little more hacking?  
      If str__prm_datatype = "uniqueidentifier" Then
				If fnc_convert_expected_string(str__prm_1).ToUpper = fnc_convert_expected_string(str__prm_2).ToUpper Then
          ' if they both convert to same value - return true
          Return True
				Else
					Return False
				End If
			End If

			Return False

		End Function

    '' checks to see if during a list child add, you have specified the same individual value twice
    'Public Shared Sub sub_check_duplicate_recurse(ByVal ctl__prm_container As Control, ByVal str__prm_control_to_code As String, ByRef dct_initially_nothing As IDictionary, ByRef bln__prm_failed As Boolean)

    '  ' look for controls identified by passed in id as participating in a code action
    '  For Each ctl As Control In ctl__prm_container.Controls
    '    If fnc_convert_expected_string(ctl.ID) = str__prm_control_to_code Then
    '      sub_check_duplicate(ctl, dct_initially_nothing, bln__prm_failed)
    '    End If

    '    If ctl.Controls.Count > 0 Then
    '      sub_check_duplicate_recurse(ctl, str__prm_control_to_code, dct_initially_nothing, bln__prm_failed)
    '    End If

    '  Next

    'End Sub

    'Public Shared Sub sub_check_duplicate(ByVal ctl__prm As Control, ByRef dct__prm As IDictionary, ByRef bln__prm_failed As Boolean)

    '  Dim str_value As String = fnc_get_control_value(ctl__prm)
    '  If str_value.Length = 0 Then Exit Sub

    '  If dct__prm Is Nothing Then dct__prm = New Dictionary(Of String, String)

    '  If dct__prm.Contains(str_value) Then
    '    bln__prm_failed = True
    '    Dim str_to_code_validator_duplicate As String = fnc_get_control_attribute_value(ctl__prm, "to_code_validator_duplicate")
    '    If str_to_code_validator_duplicate.Length > 0 Then
    '      Dim cusr As CustomValidator = fnc_find_nested_control(ctl__prm.Parent, str_to_code_validator_duplicate)
    '      If cusr Is Nothing Then Throw New Exception("Could not find ctl_to_validator")
    '      cusr.ErrorMessage = Replace(cusr.ErrorMessage, "[invalid_value_to_code]", str_value)
    '      cusr.IsValid = False
    '    End If
    '  Else
    '    dct__prm.Add(str_value, str_value)
    '  End If

    'End Sub

    ' find all controls on a page that are to be coded (initial use is user enters username, function finds and set pk_person_user)
    'Public Shared Sub sub_code_value_recurse(ByVal ctl__prm_container As Control, ByVal str__prm_control_to_code As String, ByRef bln__prm_failed As Boolean)

    '  ' look for controls identified by passed in id as participating in a code action
    '  For Each ctl As Control In ctl__prm_container.Controls
    '    If fnc_convert_expected_string(ctl.ID) = str__prm_control_to_code Then
    '      sub_code_value(ctl, bln__prm_failed)
    '    End If

    '    If ctl.Controls.Count > 0 Then
    '      sub_code_value_recurse(ctl, str__prm_control_to_code, bln__prm_failed)
    '    End If

    '  Next

    'End Sub

    '' attempt to code (convert column value to primary key value (i.e. username to pk_person_user))
    '' control to convert needs target (for coding) control_id, table_name and column_name to code with and optional custom validator
    'Public Shared Sub sub_code_value(ByVal ctl__prm As Control, ByRef bln__prm_failed As Boolean)

    '  Dim str_value As String = fnc_get_control_value(ctl__prm)
    '  ' cannot code if control has no value
    '  If str_value.Length = 0 Then Exit Sub

    '  Dim str_to_code_table_name As String = fnc_get_control_attribute_value(ctl__prm, "to_code_table_name")
    '  Dim str_to_code_column_name As String = fnc_get_control_attribute_value(ctl__prm, "to_code_column_name")
    '  Dim str_to_code_control_id As String = fnc_get_control_attribute_value(ctl__prm, "to_code_control_id")
    '  Dim str_to_code_validator As String = fnc_get_control_attribute_value(ctl__prm, "to_code_validator")

    '  If str_to_code_table_name.Length = 0 Then Throw New Exception("Missing to_code_table_name")
    '  If str_to_code_column_name.Length = 0 Then Throw New Exception("Missing to_code_column_name")
    '  If str_to_code_control_id.Length = 0 Then Throw New Exception("Missing to_code_control_id")

    '  If cls_system_table.fnc_has_column(str_to_code_table_name, str_to_code_column_name) = False Then
    '    Throw New Exception("Column (" & str_to_code_column_name & " ) not found in table (" & str_to_code_table_name & ").")
    '  End If

    '  Dim str_primary_key_name = fnc_primary_key_from_table(str_to_code_table_name)
    '  ' attempt to code
    '  Dim str_value__coded As String = cls_data_access_layer.fnc_get_scaler__string("select " & str_primary_key_name & " from " & str_to_code_table_name & " where " & str_to_code_column_name & " = " & cls_utility.fnc_dbwrap(str_value))

    '  ' successful code (TODO should this only code to primary key - if yes - should be valid guid instead of not empty)
    '  If str_value__coded.Length > 0 Then
    '    ' find control to set coded value to
    '    Dim ctl_to_code As Control = fnc_find_nested_control(ctl__prm.Parent, str_to_code_control_id)
    '    If ctl_to_code Is Nothing Then Throw New Exception("Could not find ctl_to_code")
    '    sub_set_control_value(ctl_to_code, str_value__coded)
    '    ' if child add and you just set value, switch over set table name
    '    fnc_rename_attribute(ctl_to_code, "set_table_name", "table_name")
    '  Else
    '    ' could not code.  bad input.  if validator specified, find it and invalidate it.
    '    bln__prm_failed = True
    '    If str_to_code_validator.Length > 0 Then
    '      Dim cusr As CustomValidator = fnc_find_nested_control(ctl__prm.Parent, str_to_code_validator)
    '      If cusr Is Nothing Then Throw New Exception("Could not find ctl_to_validator")
    '      cusr.ErrorMessage = Replace(cusr.ErrorMessage, "[invalid_value_to_code]", str_value)
    '      cusr.IsValid = False
    '    End If
    '  End If

    'End Sub

    'Public Shared Function fnc_get_container(ByVal str__prm_container_name As String) As Control

    '  ' already found it, return it
    '  If Current.Items(str__prm_container_name) Is Nothing = False Then
    '    Return Current.Items(str__prm_container_name)
    '  End If

    '  Dim ctl_start As Control = Nothing
    '  Dim ctl_page As Control = Current.Handler

    '  ' start is base for all data
    '  ctl_start = fnc_findcast__placeholder(ctl_page, str__prm_container_name)
    '  If ctl_start Is Nothing = False Then
    '    Current.Items.Add(str__prm_container_name, ctl_start)
    '    Return ctl_start
    '  End If

    '  ' start is base for detail page (needs to be changed to above - hence message)
    '  Current.Response.Write("Warning: " & str__prm_container_name & " not found, trying plc_data__base, then page.  You should wrap all your database data in a placeholder (" & cls_constant.str_data_container_name & ")" & "<br/>")
    '  ctl_start = fnc_findcast__placeholder(ctl_page, "plc_data__base")

    '  If ctl_start Is Nothing = False Then
    '    Current.Items.Add(str__prm_container_name, ctl_start)
    '    Return ctl_start
    '  End If

    '  'ctl_start = fnc_findcast__contentplaceholder(ctl_page, "cph_site_00")

    '  'If ctl_start Is Nothing = False Then
    '  '	Current.Items.Add(str__prm_container_name, ctl_start)
    '  '	Return ctl_start
    '  'End If

    '  ' TODO - above block can't find contentplaceholder using master id or cont_main
    '  ' Report control doesn't like using page when setting attributes - must have plc_data__all or base
    '  Current.Items.Add(str__prm_container_name, ctl_page)
    '  Return ctl_page

    'End Function

    Public Shared ReadOnly Property str_physical_path As String
			Get

				Try
					If Current Is Nothing = True Then Return "could not obtain physical_path"
					If Current.Request Is Nothing = True Then Return "could not obtain physical_path 2"
				Catch
					Return "could not obtain physical_path 3"
				End Try

				Dim str_request_physical_path As String = fnc_clean_path(Current.Request.PhysicalPath)
				Dim str_page As String = Current.Request.Url.Segments(Current.Request.Url.Segments.Length - 1)

				Return Replace(str_request_physical_path, str_page, "")

			End Get
		End Property
		Public Shared ReadOnly Property str_physical_root() As String

			Get

				Dim str As String = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath

				str = fnc_clean_path(str, True)

				If str.Contains(cls_constant.str_subweb) = True Then
					str = Replace(str, "/" & cls_constant.str_subweb, "")
				End If

				Return str

			End Get

		End Property

    ' return webcontrol if it is one
    Public Shared Function fnc_cast_if__webcontrol(ByVal obj__prm As Object) As WebControl

			If obj__prm.GetType.IsSubclassOf(GetType(WebControl)) Then
				Return DirectCast(obj__prm, WebControl)
			End If

			Return Nothing

		End Function

    '' return htmlcontrol if it is one
    'Public Shared Function fnc_cast_if__htmlcontrol(ByVal obj__prm As Object) As HtmlControl

    '  If obj__prm.GetType.IsSubclassOf(GetType(HtmlControl)) Then
    '    Return DirectCast(obj__prm, HtmlControl)
    '  End If

    '  Return Nothing

    'End Function

    '' return usercontrol if it is one
    'Public Shared Function fnc_cast_if__usercontrol(ByVal obj__prm As Object) As UserControl

    '  If obj__prm.GetType.IsSubclassOf(GetType(UserControl)) Then
    '    Return DirectCast(obj__prm, UserControl)
    '  End If

    '  Return Nothing

    'End Function

    Public Shared Function fnc_clean_path(ByVal str__prm_path As String, Optional ByVal bln__prm_terminate_with_slash As Boolean = False) As String

			Dim str As String = fnc_convert_expected_string(str__prm_path)

			If str.Length = 0 Then Return ""

      ' re-orient slashes
      str = Replace(str, "\", "/")
      ' remove duplicate slashes
      str = Replace(str, "//", "/")

			If bln__prm_terminate_with_slash = True Then
				If str.EndsWith("/") = False Then
					str = str & "/"
				End If
			End If

			Return str

		End Function

	End Class

End Namespace
