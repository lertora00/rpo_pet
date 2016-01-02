Imports ns_enterprise
Imports ns_enterprise.cls_utility

Imports System.Collections.Specialized
Imports System.Text
Imports System.Data

Namespace ns_enterprise

	Public Class cls_sql

		Protected Friend sdt__prf_declare As StringDictionary = New StringDictionary

    ' ord__prv_column is ordereddictionary versus stringdictionary because you can't iterate through 
    '   stringdictionary and update it.  it seems column could handle values but then columns are created before values so
    '   adding a value would usually be updating a column entry.  control added to keep track of controls as part of sql
    Private ord__prv_column As OrderedDictionary = New OrderedDictionary
		Private ord__prv_value As OrderedDictionary = New OrderedDictionary
		Private ord__prv_control As OrderedDictionary = New OrderedDictionary

		Private str__prv_pk_log_system_audit_table As String = ""
		Private str__prv_pk_log_system_audit_column As String = ""

		Public Enum en_operation
			insert
			update
			delete
			[select]
		End Enum

    ' operation property
    Private str__prv_operation As String = ""
		Public Property str_operation() As String

			Get
				Return str__prv_operation
			End Get
			Set(ByVal Value As String)
				str__prv_operation = Value
			End Set

		End Property

    ' sql property
    Private str__prv_sql As String = ""
		Public ReadOnly Property str_sql() As String

			Get
				Return str__prv_sql
			End Get

		End Property

    ' table_name property
    Private str__prv_table_name As String = ""
		Public Property str_table_name() As String

			Get
				Return str__prv_table_name
			End Get
			Set(ByVal Value As String)
				str__prv_table_name = Value
			End Set

		End Property

    ' execute_sql_order property
    Private int__prv_execute_sql_order As Int32 = 0
		Public Property int_execute_sql_order() As Int32

			Get
				Return int__prv_execute_sql_order
			End Get
			Set(ByVal Value As Int32)
				int__prv_execute_sql_order = Value
			End Set

		End Property

    ' sql_collection for auditing
    Private inst__prv_sql_collection__audit As cls_sql_collection = New cls_sql_collection
		Public Property inst_sql_collection__audit() As cls_sql_collection

			Get
				Return inst__prv_sql_collection__audit
			End Get
			Set(ByVal Value As cls_sql_collection)
				inst__prv_sql_collection__audit = Value
			End Set

		End Property

    ' sql object for prior values (pre-select update, empty for inserts, pre-select for delete)
    Private inst__prv_sql__prior As cls_sql
		Public ReadOnly Property inst_sql__prior() As cls_sql

			Get
				Return inst__prv_sql__prior
			End Get

		End Property

    ' sql object presented to database - includes pk, fk, formatting (datetime), processing fields, etc.
    Private inst__prv_sql__presented As cls_sql
		Public ReadOnly Property inst_sql__presented() As cls_sql

			Get
				Return inst__prv_sql__presented
			End Get

		End Property

    ' sql object delta - difference between prior and presented (basis for audit)
    Private inst__prv_sql__delta As cls_sql
		Public ReadOnly Property inst_sql__delta() As cls_sql

			Get
				Return inst__prv_sql__delta
			End Get

		End Property

		Public Sub sub_add_column(ByVal str__prm_column_name As String, ByVal str__prm_value As String, Optional ByVal ctl__prm_control As Object = Nothing)

			sub_add_column(str__prm_column_name)
			sub_add_value(str__prm_column_name, str__prm_value)

			If ctl__prm_control Is Nothing = False Then
				sub_add_control(str__prm_column_name, ctl__prm_control)
			End If

		End Sub

		Public Sub sub_add_or_update_column(ByVal str__prm_column_name As String, ByVal str__prm_value As String, Optional ByVal ctl__prm_control As Object = Nothing)

			sub_remove_column(str__prm_column_name)
			sub_remove_value(str__prm_column_name)

			sub_add_column(str__prm_column_name)
			sub_add_value(str__prm_column_name, str__prm_value)

			If ctl__prm_control Is Nothing = False Then
				sub_add_control(str__prm_column_name, ctl__prm_control)
			End If

		End Sub

		Public Sub sub_remove_column(ByVal str__prm_column_name As String)

			If ord__prv_column.Contains(str__prm_column_name) = True Then
				ord__prv_column.Remove(str__prm_column_name)
				sub_remove_value(str__prm_column_name)
				sub_remove_control(str__prm_column_name)
			End If

		End Sub

		Private Sub sub_remove_value(ByVal str__prm_value_name As String)

			If ord__prv_value.Contains(str__prm_value_name) = True Then
				ord__prv_value.Remove(str__prm_value_name)
			End If

		End Sub

		Private Sub sub_remove_control(ByVal str__prm_value_name As String)

			If ord__prv_control.Contains(str__prm_value_name) = True Then
				ord__prv_control.Remove(str__prm_value_name)
			End If
		End Sub

		Public Sub sub_add_column(ByVal str__prm_column_name As String)

			If fnc_convert_expected_string(str__prm_column_name).Length = 0 Then
				Throw New Exception("Column Name required: " & str__prm_column_name)
			End If

			If ord__prv_column.Contains(str__prm_column_name) = True Then
				Throw New Exception("Column name already exists: " & str__prm_column_name)
			End If

			ord__prv_column.Add(str__prm_column_name, str__prm_column_name)

		End Sub

		Private Sub sub_add_control(ByVal str__prm_column_name As String, ByVal ctl__prm_control As Control)

			If fnc_convert_expected_string(str__prm_column_name).Length = 0 Then
				Throw New Exception("Control name required: " & str__prm_column_name)
			End If

			If ord__prv_control.Contains(str__prm_column_name) = True Then
				Throw New Exception("Control name already exists: " & str__prm_column_name)
			End If

			ord__prv_control.Add(str__prm_column_name, ctl__prm_control)

		End Sub

		Public Sub sub_update_value(ByVal str__prm_column_name As String, ByVal str__prm_value As String)

			Dim str_value As String = fnc_convert_expected_string(str__prm_value)

			If str_value.Length = 0 Then
				str_value = "null"
			End If

			ord__prv_value(str__prm_column_name) = str__prm_value

		End Sub

		Private Sub sub_add_value(ByVal str__prm_column_name As String, ByVal str__prm_value As String)

			Dim str_value As String = fnc_convert_expected_string(str__prm_value)

			If str_value.Length = 0 Then
				str_value = "null"
			End If

			ord__prv_value.Add(str__prm_column_name, str__prm_value)

		End Sub

		Public Sub sub_execute()

      ' send in proposed
      Dim str_sql As String = fnc_get_sql(Me)

      ' this will create prior, presented, delta
      sub_generate_ppd(Me)

			cls_data_access_layer.fnc_execute_non_query(str_sql)

		End Sub

		Public Sub sub_execute_with_audit(ByVal bln__prm_async As Boolean)

			sub_execute_with_audit(Me, bln__prm_async)

		End Sub

		Public Sub sub_execute_with_audit(ByVal inst__prm_sql As cls_sql, ByVal bln__prm_async As Boolean)

			Dim str_sql As String = ""
			Dim str_sql_audit As String = ""

      ' this will create prior, presented, delta
      sub_generate_ppd(inst__prm_sql)

      ' get base table sql without declares
      str_sql = fnc_get_sql(inst__prm_sql, False)

      ' get accompanying audit sql
      str_sql_audit = inst__prm_sql.fnc_get_sql_audit()

			If bln__prm_async = True Then
				cls_data_access_layer.fnc_execute_non_query(fnc_get_sql_declare(inst__prm_sql) & " " & str_sql)
				cls_asynchronous.sub_execute_non_query(fnc_get_sql_declare_unified(inst__prm_sql, False) & " " & str_sql_audit)
			Else
				cls_data_access_layer.fnc_execute_non_query(fnc_get_sql_declare_unified(inst__prm_sql) & " " & str_sql & str_sql_audit)
			End If

		End Sub

		Public Sub sub_execute_async()

      'cls_async.fnc_execute_non_query(fnc_get_sql)

    End Sub

		Public Function fnc_get_sql(ByVal inst__prm_sql As cls_sql, Optional ByVal bln__prm_generate_declare As Boolean = True) As String

			Dim str_sql As String = fnc_generate_sql(inst__prm_sql)

			If fnc_convert_expected_string(str_sql).Length = 0 Then
				Throw New Exception("SQL could not be generated")
			End If

			Dim strb_sql As New StringBuilder()

			If bln__prm_generate_declare = True Then
				strb_sql.Append(" " & fnc_get_sql_declare(inst__prm_sql))
			End If
			strb_sql.Append(" " & fnc_get_sql_set(inst__prm_sql))
			strb_sql.Append(" " & str_sql)
			strb_sql.Append(" ;")

			Return strb_sql.ToString

		End Function

		Public Function fnc_get_sql_audit() As String

			Return fnc_get_sql_audit(Guid.NewGuid.ToString)

		End Function

		Public Function fnc_get_sql_audit(ByVal str__prm_pk_log_system_audit_header As String) As String

			Return fnc_get_sql_audit(Me, str__prm_pk_log_system_audit_header)

		End Function

		Public Function fnc_get_sql_audit(ByVal inst__prm_sql As cls_sql, ByVal str__prm_pk_log_system_audit_header As String) As String

			Dim strb_sql As New StringBuilder()

      ' generate audit
      inst__prm_sql.sub_generate_audit(str__prm_pk_log_system_audit_header, True)

			For Each inst_sql__audit As cls_sql In inst__prm_sql.inst_sql_collection__audit
				sub_generate_ppd(inst_sql__audit)
        ' generate set and select for each audit insert.  declares cannot be part of this as they'd dupe with each row
        strb_sql.Append(" " & fnc_get_sql_set(inst_sql__audit))
				strb_sql.Append(" " & fnc_generate_sql(inst_sql__audit))
				strb_sql.Append(" ;")
			Next

			Return strb_sql.ToString

		End Function

		Public Sub sub_generate_sql()

			fnc_generate_sql(Me)

		End Sub

		Public Function fnc_generate_sql() As String

			Return fnc_generate_sql(Me)

		End Function

		Public Sub sub_generate_ppd(ByVal inst__prm_sql As cls_sql)

			Select Case inst__prm_sql.str_operation
				Case "insert"
					sub_generate_ppd_insert(inst__prm_sql)
				Case "update"
					sub_generate_ppd_update(inst__prm_sql)
				Case "delete"
					sub_generate_ppd_delete(inst__prm_sql)
				Case "select"
					sub_generate_ppd_select(inst__prm_sql)
					Exit Sub
				Case Else
					Throw New Exception("Invalid operation specified for ppd generation")
			End Select

			sub_remove_system_information(inst__prm_sql.inst__prv_sql__delta)

		End Sub

		Public Sub sub_remove_system_information(ByVal inst__prm_sql As cls_sql)

			Dim arl As New ArrayList

			Dim str_primary_key_name As String = fnc_primary_key_from_table(inst__prm_sql.str_table_name)

			For Each str As String In inst__prm_sql.ord__prv_column.Keys
				Select Case str
          ' remove primary key from delta
          Case str_primary_key_name
						arl.Add(str)
					Case cls_constant.str_insert_date_column_name, cls_constant.str_insert_user_column_name, cls_constant.str_insert_user_column_name_pk
						arl.Add(str)
					Case cls_constant.str_update_date_column_name, cls_constant.str_update_user_column_name, cls_constant.str_update_user_column_name_pk
						arl.Add(str)
					Case cls_constant.str_active_flag_column_name
						arl.Add(str)
				End Select
			Next

			For Each Str As String In arl
				inst__prm_sql.sub_remove_column(Str)
			Next

		End Sub

		Public Function fnc_generate_sql(ByVal inst__prm_sql As cls_sql) As String

			If inst__prm_sql.inst__prv_sql__presented Is Nothing Then
				sub_generate_ppd(inst__prm_sql)
			End If

			Select Case inst__prm_sql.str_operation
				Case "insert"
					str__prv_sql = fnc_generate_insert(inst__prm_sql.inst__prv_sql__presented)
					Return str__prv_sql
				Case "update"
					str__prv_sql = fnc_generate_update(inst__prm_sql.inst__prv_sql__presented)
					Return str__prv_sql
				Case "delete"
					str__prv_sql = fnc_generate_delete(inst__prm_sql.inst__prv_sql__presented)
					Return str__prv_sql
				Case "select"
					str__prv_sql = fnc_generate_select(inst__prm_sql.inst__prv_sql__presented)
					Return str__prv_sql
				Case Else
					Throw New Exception("Invalid operation specified for sql generation")
			End Select

			Return Nothing

		End Function

		Private Function fnc_generate_insert() As String

			Return fnc_generate_insert(Me)

		End Function

    ' generate prior, presented and delta
    Private Sub sub_generate_ppd_insert(ByVal inst__prm_sql As cls_sql)

			sub_validate(inst__prm_sql)

			inst__prm_sql.inst__prv_sql__presented = fnc_clone_sql(inst__prm_sql)

			If inst__prm_sql.inst__prv_sql__presented.ord__prv_column.Contains(fnc_primary_key_from_table(inst__prm_sql.inst__prv_sql__presented.str__prv_table_name)) = False Then
				inst__prm_sql.inst__prv_sql__presented.sub_add_column(fnc_primary_key_from_table(inst__prm_sql.inst__prv_sql__presented.str__prv_table_name), Guid.NewGuid.ToString)
			End If

      ' stamp insert date and user
      inst__prm_sql.inst__prv_sql__presented.sub_add_or_update_column(cls_constant.str_insert_date_column_name, Now.ToString)

      ' stamp insert user 
      If cls_current_user.bln_authenticated = True Then
        ' if insert user exists, stamp it.
        If cls_system_table.fnc_has_column(inst__prv_sql__presented.str_table_name, cls_constant.str_insert_user_column_name) = True Then
					inst__prv_sql__presented.sub_add_or_update_column(cls_constant.str_insert_user_column_name, cls_current_user.str_onscreen_name)
				End If

        ' if insert user (pk) exists, stamp it.
        If cls_system_table.fnc_has_column(inst__prv_sql__presented.str_table_name, cls_constant.str_insert_user_column_name_pk) = True Then
					inst__prv_sql__presented.sub_add_or_update_column(cls_constant.str_insert_user_column_name_pk, cls_current_user.str_pk_person_user)
				End If
			End If

      ' create prior (all empty fields based on presented)
      inst__prm_sql.inst__prv_sql__prior = fnc_clone_sql(inst__prm_sql.inst__prv_sql__presented)
			sub_empty_sql(inst__prm_sql.inst__prv_sql__prior)

      ' create delta - based on pre-clean presented having removed empty fields
      inst__prm_sql.inst__prv_sql__delta = fnc_clone_sql(inst__prm_sql.inst__prv_sql__presented)
			sub_remove_empty_sql(inst__prm_sql.inst__prv_sql__delta)

		End Sub

		Private Function fnc_generate_insert(ByVal inst__prm_sql As cls_sql) As String

			sub_validate(inst__prm_sql)

			Dim str_sql As New StringBuilder()
			Dim str_comma As String = ""

      ' make sure data is wrapped and cleaned
      ' removed - we only want to do this in sql
      'sub_wrap_and_clean_sql(inst__prm_sql)

      str_sql.Append("insert into " & inst__prm_sql.str__prv_table_name & " ")
			str_sql.Append("(")
			str_comma = ""
			For Each str As String In inst__prm_sql.ord__prv_column.Keys
				str_sql.Append(str_comma & inst__prm_sql.ord__prv_column(str))
				str_comma = ", "
			Next
			str_sql.Append(")")
			str_sql.Append(" values ")
			str_sql.Append("(")
			str_comma = ""
			For Each str As String In inst__prm_sql.ord__prv_column.Keys
				str_sql.Append(str_comma & "@" & inst__prm_sql.ord__prv_column(str))
				str_comma = ", "
			Next
			str_sql.Append(")")

			Return str_sql.ToString

		End Function

		Private Function fnc_generate_update() As String

			Return fnc_generate_update(Me)

		End Function

		Private Sub sub_generate_ppd_update(ByVal inst__prm_sql As cls_sql)

			sub_validate(inst__prm_sql)

			inst__prm_sql.inst__prv_sql__presented = fnc_clone_sql(inst__prm_sql)

      ' stamp update date and user
      inst__prm_sql.inst__prv_sql__presented.sub_add_or_update_column(cls_constant.str_update_date_column_name, Now.ToString)
			If cls_current_user.bln_authenticated = True Then
				inst__prm_sql.inst__prv_sql__presented.sub_add_or_update_column(cls_constant.str_update_user_column_name, cls_current_user.str_onscreen_name)
				inst__prm_sql.inst__prv_sql__presented.sub_add_or_update_column(cls_constant.str_update_user_column_name_pk, cls_current_user.str_pk_person_user)
			End If

      ' create prior (pre-update select)
      inst__prm_sql.inst__prv_sql__prior = fnc_pre_action_select_sql(inst__prm_sql.inst__prv_sql__presented)

      ' create delta - based on pre-clean presented having removed empty fields
      inst__prm_sql.inst__prv_sql__delta = fnc_get_delta_sql(inst__prm_sql.inst__prv_sql__prior, inst__prm_sql.inst__prv_sql__presented)

		End Sub

		Private Function fnc_generate_update(ByVal inst__prm_sql As cls_sql) As String

			sub_validate(inst__prm_sql)

			Dim str_primary_key_name As String = fnc_primary_key_from_table(inst__prm_sql.str_table_name)
			Dim str_primary_key_value As String = inst__prm_sql.ord__prv_value(str_primary_key_name).ToString

			If str_primary_key_value.Length = 0 Then
        ' you really don't need a primary key to run an update.  just usually a mistake when omitted.  
        ' TODO need override to allow without primary key (conscience call)
        Throw New Exception("Primary key value required to generate update")
			End If

      ' make sure data is wrapped and cleaned
      ' removed - we only want to do this in sql
      'sub_wrap_and_clean_sql(inst__prm_sql)

      Dim str_sql As New StringBuilder()
			Dim str_comma As String = ""

			str_sql.Append("update " & inst__prm_sql.str__prv_table_name & " ")
			str_comma = "set "
			For Each str As String In inst__prm_sql.ord__prv_column.Keys
				str_sql.Append(str_comma & inst__prm_sql.ord__prv_column(str) & " = @" & inst__prm_sql.ord__prv_column(str))
				str_comma = ", "
			Next
			str_sql.Append(" where ")
			str_sql.Append(str_primary_key_name)
			str_sql.Append(" = ")
			str_sql.Append(fnc_dbwrap(str_primary_key_value))

			Return str_sql.ToString

		End Function

		Public Function fnc_generate_select(ByVal str__prm_table_name As String, ByVal str__prm_primary_key_value As String) As String

			Dim stc_column As StringCollection = cls_utility.fnc_get_column_list(str__prm_table_name)

			Dim inst_sql As New cls_sql()
			inst_sql.str_table_name = str__prm_table_name

			Dim str_data_value As String = ""

			For Each Str As String In stc_column
				str_data_value = ""
				If Str.StartsWith(cls_constant.str_primary_key_prefix) Then
					str_data_value = str__prm_primary_key_value
				End If
				inst_sql.sub_add_column(Str, str_data_value)
			Next

			Return inst_sql.fnc_generate_select()

		End Function

		Private Sub sub_generate_ppd_select(ByVal inst__prm_sql As cls_sql)

			inst__prm_sql.inst__prv_sql__presented = fnc_clone_sql(inst__prm_sql)

		End Sub

		Private Function fnc_generate_delete(Optional ByVal bln__prm_logical_delete As Boolean = True) As String

			Return fnc_generate_delete(Me, bln__prm_logical_delete)

		End Function

		Private Sub sub_generate_ppd_delete(ByVal inst__prm_sql As cls_sql, Optional ByVal bln__prm_logical_delete As Boolean = True)

			sub_validate(inst__prm_sql)

			inst__prm_sql.inst__prv_sql__presented = fnc_clone_sql(inst__prm_sql)

      ' stamp update date and user
      inst__prm_sql.inst__prv_sql__presented.sub_add_or_update_column(cls_constant.str_update_date_column_name, Now.ToString)
			If cls_current_user.bln_authenticated = True Then
				inst__prm_sql.inst__prv_sql__presented.sub_add_or_update_column(cls_constant.str_update_user_column_name, cls_current_user.str_onscreen_name)
				inst__prm_sql.inst__prv_sql__presented.sub_add_or_update_column(cls_constant.str_update_user_column_name_pk, cls_current_user.str_pk_person_user)
			End If

      ' add active flag = false for logical delete
      inst__prm_sql.inst__prv_sql__presented.sub_add_or_update_column(cls_constant.str_active_flag_column_name, 0)

      ' create prior (pre-update select)
      inst__prm_sql.inst__prv_sql__prior = fnc_pre_action_select_sql(inst__prm_sql.inst__prv_sql__presented)

      ' create delta - equal to select * (prior)
      inst__prm_sql.inst__prv_sql__delta = fnc_clone_sql(inst__prm_sql.inst__prv_sql__prior)

		End Sub

		Private Function fnc_generate_delete(ByVal inst__prm_sql As cls_sql, Optional ByVal bln__prm_logical_delete As Boolean = True) As String

			sub_validate(inst__prm_sql)

			Dim str_primary_key_name As String = fnc_primary_key_from_table(inst__prm_sql.str_table_name)
			Dim str_primary_key_value As String = inst__prm_sql.ord__prv_value(str_primary_key_name).ToString

			If str_primary_key_value.Length = 0 Then
				Throw New Exception("Primary key value required to generate delete")
			End If

			Dim str_sql As New StringBuilder()
			Dim str_comma As String = ""

			If bln__prm_logical_delete = True Then
        ' TODO - if logical delete, this should create an update cls_sql and call that (sql only) - no audit on update as delete is already being audited
        str_sql.Append("update ")
				str_sql.Append(inst__prm_sql.str_table_name)
				str_sql.Append(" set  ")
				For Each str As String In inst__prm_sql.ord__prv_column.Keys
					str_sql.Append(str_comma & inst__prm_sql.ord__prv_column(str) & " = " & cls_utility.fnc_dbwrap(inst__prm_sql.ord__prv_value(str)))
					str_comma = ", "
				Next
			End If

			If bln__prm_logical_delete = False Then
				str_sql.Append("delete from ")
				str_sql.Append(inst__prm_sql.str_table_name)
			End If

			str_sql.Append(" where ")
			str_sql.Append(str_primary_key_name)
			str_sql.Append(" = ")
			str_sql.Append(fnc_dbwrap(str_primary_key_value))
      'str_sql.Append(" and ")
      'str_sql.Append(cls_constant.str_active_flag_column_name & " = 1 ")

      Return str_sql.ToString

		End Function

		Private Function fnc_generate_select() As String

			Return fnc_generate_select(Me)

		End Function

		Private Function fnc_generate_select(ByVal inst__prm_sql As cls_sql) As String

			sub_validate(inst__prm_sql)

			Dim str_primary_key_name As String = fnc_primary_key_from_table(inst__prm_sql.str_table_name)
			Dim str_primary_key_value As String = inst__prm_sql.ord__prv_value(str_primary_key_name).ToString

			If str_primary_key_value.Length = 0 Then
				Throw New Exception("Primary key value required to generate select")
			End If

			Dim str_sql As New StringBuilder()
			Dim str_comma As String = ""

			str_sql.Append("select ")
			str_comma = ""
			For Each str As String In inst__prm_sql.ord__prv_column.Keys
				str_sql.Append(str_comma & inst__prm_sql.ord__prv_column(str))
				str_comma = ", "
			Next
			str_sql.Append(" from " & inst__prm_sql.str_table_name)
			str_sql.Append(" where ")
			str_sql.Append(str_primary_key_name)
			str_sql.Append(" = ")
			str_sql.Append(fnc_dbwrap(str_primary_key_value))
			str_sql.Append(" and ")
			str_sql.Append(cls_constant.str_active_flag_column_name & " = 1 ")

			Return str_sql.ToString

		End Function

		Public Function fnc_get_sql_declare() As String

			Return fnc_get_sql_declare(Me)

		End Function

		Public Function fnc_get_sql_declare(ByVal inst__prm_sql As cls_sql) As String

			Dim str_declare As New System.Text.StringBuilder

			For Each Str As String In inst__prm_sql.inst__prv_sql__presented.ord__prv_value.Keys
				str_declare.Append("declare @" & Str & " as nvarchar(max) ")
			Next

			Return str_declare.ToString

		End Function

		Public Function fnc_get_sql_declare_unified(ByVal inst__prm_sql As cls_sql, Optional ByVal bln__prm_include_base_sql As Boolean = True) As String

      ' add all columns to declare collection
      If bln__prm_include_base_sql = True Then
				For Each str_column As String In inst__prm_sql.inst__prv_sql__presented.ord__prv_column.Keys
					If inst__prm_sql.sdt__prf_declare.ContainsKey(str_column) = False Then
						inst__prm_sql.sdt__prf_declare.Add(str_column, str_column)
					End If
				Next
			End If

      ' add all columns for each audit sql to declare collection
      For Each inst_sql__audit As cls_sql In inst__prm_sql.inst_sql_collection__audit
				For Each str_column__audit As String In inst_sql__audit.inst__prv_sql__presented.fnc_get_column_collection.Keys
					If inst__prm_sql.sdt__prf_declare.ContainsKey(str_column__audit) = False Then
						inst__prm_sql.sdt__prf_declare.Add(str_column__audit, str_column__audit)
					End If
				Next
			Next

			Dim str_sql As New System.Text.StringBuilder()

      ' add declare for batched sql (no duplicates)
      str_sql.Append(fnc_generate_sql_declare_unified(inst__prm_sql))

			Return str_sql.ToString

		End Function

		Public Function fnc_get_sql_set() As String

			Return fnc_get_sql_set(Me)

		End Function

		Public Function fnc_get_sql_set(ByVal inst__prm_sql As cls_sql) As String

			Dim str_set As New System.Text.StringBuilder

			For Each str As String In inst__prm_sql.inst__prv_sql__presented.ord__prv_value.Keys
				str_set.Append("set @" & str & " = " & cls_utility.fnc_dbwrap(inst__prm_sql.inst__prv_sql__presented.ord__prv_value(str)) & " ")
			Next

			Return str_set.ToString

		End Function

		Public Function fnc_get_column_collection() As OrderedDictionary

			Return ord__prv_column

		End Function

		Public Function fnc_get_column_value(ByVal str__prm_column_name As String) As String

			If ord__prv_column.Contains(str__prm_column_name) = False Then
				Return Nothing
			End If

			Return fnc_convert_expected_string(ord__prv_value(str__prm_column_name))

		End Function

		Public Function fnc_get_control(ByVal str__prm_column_name As String) As Control

			If ord__prv_column.Contains(str__prm_column_name) = False Then
				Return Nothing
			End If

			Return ord__prv_control(str__prm_column_name)

		End Function

		Public Sub sub_generate_audit(ByVal str__prm_pk_log_system_audit_header As String, Optional ByVal bln__prm_generate_audit_header As Boolean = False)

			If fnc_validate_audit() = False Then
				Exit Sub
			End If

      ' initialize audit sql collection
      inst__prv_sql_collection__audit = New cls_sql_collection()

      ' individual call to cls_sql with audit needs to generate an audit header.  collection of sql (dynamic update)
      '   would have header created outside collection loop
      If bln__prm_generate_audit_header = True Then
				Dim inst_sql_header As cls_sql = cls_data_audit.fnc_create_header(str__prm_pk_log_system_audit_header)
				inst__prv_sql_collection__audit.Add(inst_sql_header)
			End If

      ' individual call to cls_sql with audit needs to generate an audit header.  collection of sql (dynamic update)
      '   would have header created outside collection loop
      If bln__prm_generate_audit_header = True Then
				sub_insert_audit__table(str__prm_pk_log_system_audit_header)
			Else
        ' pk_log_system_audit_header will be replaced during collection execute
        sub_insert_audit__table("[replace_header_at_collection]")
			End If

			Dim str_data_value__old As String = ""
			Dim str_data_value__old__string As String = ""      'used only for uniqueidentifiers
      Dim str_data_value__new As String = ""
			Dim str_data_value__new__string As String = ""      'used only for uniqueidentifiers

      Dim str_table_suffix As String
			Dim bln_audit_found As Boolean = False

			For Each str_column_name As String In inst__prv_sql__delta.ord__prv_column.Keys

        ' ignore columns added to the collection for processing purposes and null values
        ' also ignore tables or columns marked for exclusion from auditing
        If fnc_is_auditable_column(inst__prv_sql__delta.str_table_name, str_column_name, inst__prv_sql__delta.str_operation) = False Then
					GoTo next_column
				End If

				str_data_value__old__string = "null"
				str_data_value__new__string = "null"

				str_data_value__new = inst__prv_sql__presented.fnc_get_column_value(str_column_name)
				str_data_value__old = inst__prv_sql__prior.fnc_get_column_value(str_column_name)

				sub_insert_audit__column(str_column_name)

        ' based on datatype, insert into appropriate log table
        str_table_suffix = fnc_audit_get_table_suffix(str__prv_table_name, str_column_name)

				If str_table_suffix = "uniqueidentifier" Then
					str_data_value__old__string = fnc_audit_decode(str_column_name, str_data_value__old)
					str_data_value__new__string = fnc_audit_decode(str_column_name, str_data_value__new)
				End If

        ' force all new values to be empty when deleting.  overwriting pk, accounting, etc.
        If inst__prv_sql__presented.str_operation = cls_sql.en_operation.delete.ToString Then
					str_data_value__new = ""
					str_data_value__new__string = ""
				End If

				Dim inst_sql As New cls_sql()
				inst_sql.str_operation = cls_sql.en_operation.insert.ToString
				inst_sql.str_table_name = "tbl_log_system_audit_value_" & str_table_suffix
				inst_sql.sub_add_column("fk_log_system_audit_column", str__prv_pk_log_system_audit_column)
				inst_sql.sub_add_column("old_value", str_data_value__old)
				inst_sql.sub_add_column("new_value", str_data_value__new)
				If str_table_suffix = "uniqueidentifier" Then
					inst_sql.sub_add_column("old_value__string", str_data_value__old__string)
					inst_sql.sub_add_column("new_value__string", str_data_value__new__string)
				End If
				inst_sql.sub_add_column(cls_constant.str_insert_user_column_name_pk, cls_current_user.str_pk_person_user)
				inst_sql.sub_add_column(cls_constant.str_insert_user_column_name, cls_current_user.str_onscreen_name)

				inst__prv_sql_collection__audit.Add(inst_sql)

				bln_audit_found = True
next_column:

			Next

      ' no audit inserts generated (valid insert/updates only include non (or disabled) auditable fields, etc)
      If bln_audit_found = False Then
				inst__prv_sql_collection__audit = New cls_sql_collection()
			End If

		End Sub

		Private Function fnc_validate_audit() As Boolean

			sub_generate_ppd(Me)

      ' prior sql is nothing.  did not call generate ppd (insert, update, delete all have prior sql
      If inst__prv_sql__prior Is Nothing Then
				Throw New Exception("Must generate PPD (prior) before calling audit generation")
			End If

      ' if delta has no columns or values (or is nothing?), no auditing to do
      If inst__prv_sql__delta.ord__prv_value Is Nothing OrElse inst__prv_sql__delta.ord__prv_value.Count < 1 Then
				Return False
			End If

			If inst__prv_sql__delta.str_operation = cls_sql.en_operation.update.ToString Then
				Dim bln_was_updated As Boolean = False
				For Each str_column_name As String In inst__prv_sql__delta.ord__prv_column.Keys
					Select Case str_column_name
						Case fnc_primary_key_from_table(inst__prv_sql__delta.str_table_name)
						Case cls_constant.str_update_date_column_name
						Case cls_constant.str_update_user_column_name
						Case cls_constant.str_update_user_column_name_pk
						Case Else
							bln_was_updated = True
							Exit For
					End Select
				Next
        ' only pk and accounting (or less) were updated.  nothing to audit
        If bln_was_updated = False Then Return False
			End If

      ' global auditing turned off?
      If cls_constant.int_disable_sql_audit = 1 Then
				Return False
			End If

      ' operation specific auditing turned off?
      If cls_constant.int_disable_sql_audit(inst__prv_sql__delta.str__prv_operation) = 1 Then
				Return False
			End If

			Return True

		End Function

		Private Sub sub_insert_audit__table(ByVal str__prm_pk_log_system_audit_header As String)

      ' TODO if it doesn't get the context (calling inst_sql) correct, pass in table name and operation

      str__prv_pk_log_system_audit_table = Guid.NewGuid.ToString
			Dim str_primary_key_name As String = cls_utility.fnc_primary_key_from_table(inst_sql__presented.str__prv_table_name)

			Dim inst_sql As New cls_sql()
			inst_sql.str_operation = cls_sql.en_operation.insert.ToString
			inst_sql.str_table_name = "tbl_log_system_audit_table"
			inst_sql.sub_add_column("pk_log_system_audit_table", str__prv_pk_log_system_audit_table)
			inst_sql.sub_add_column("fk_log_system_audit_header", str__prm_pk_log_system_audit_header)
			inst_sql.sub_add_column("table_name", inst_sql__presented.str__prv_table_name)
			inst_sql.sub_add_column("row_primary_key", inst_sql__presented.ord__prv_value(str_primary_key_name))
			inst_sql.sub_add_column("action", inst_sql__presented.str__prv_operation)

			inst_sql.sub_add_column(cls_constant.str_insert_user_column_name_pk, cls_current_user.str_pk_person_user)
			inst_sql.sub_add_column(cls_constant.str_insert_user_column_name, cls_current_user.str_onscreen_name)

			inst__prv_sql_collection__audit.Add(inst_sql)

		End Sub

		Private Sub sub_insert_audit__column(ByVal str__prm_column_name As String)

			str__prv_pk_log_system_audit_column = Guid.NewGuid.ToString

			Dim inst_sql As New cls_sql()
			inst_sql.str_operation = cls_sql.en_operation.insert.ToString
			inst_sql.str_table_name = "tbl_log_system_audit_column"
			inst_sql.sub_add_column("pk_log_system_audit_column", str__prv_pk_log_system_audit_column)
			inst_sql.sub_add_column("fk_log_system_audit_table", str__prv_pk_log_system_audit_table)
			inst_sql.sub_add_column("column_name", str__prm_column_name)

			inst_sql.sub_add_column(cls_constant.str_insert_user_column_name_pk, cls_current_user.str_pk_person_user)
			inst_sql.sub_add_column(cls_constant.str_insert_user_column_name, cls_current_user.str_onscreen_name)

			inst__prv_sql_collection__audit.Add(inst_sql)

		End Sub

		Private Shared Function fnc_is_auditable_column(ByVal str__prm_table_name As String, ByVal str__prm_column_name As String, ByVal str__prm_operation_name As String) As Boolean

			Return True


      'Dim arrl_audit As ArrayList = Nothing

      'If cls_global.arrl__pub_audit_exclude_table_list Is Nothing Then
      '  sub_load_meta_data()
      'End If

      'arrl_audit = cls_global.arrl__pub_audit_exclude_table_list

      'If arrl_audit Is Nothing Then
      '  Throw New Exception("audit_exclude_table_list is missing")
      'End If

      ''is table (entire) excluded from auditing
      'If arrl_audit.IndexOf(str__prm_table_name) > -1 Then
      '  Return False
      'End If

      'arrl_audit = cls_global.arrl__pub_audit_exclude_column_list

      'If arrl_audit Is Nothing Then
      '  Throw New Exception("audit_exclude_column_list is missing")
      'End If

      ''is column (in any table) name excluded from auditing
      'If arrl_audit.IndexOf(str__prm_column_name) > -1 Then
      '  Return False
      'End If

      ''todo: provide support to exclude table/column combinations

      'If str__prm_operation_name = "delete" Then Return True

      '' do not want to audit insert date/user when auditing inserts as same is captured 
      ''		in each audit row
      '' do not audit audit insert fields on insert
      'If str__prm_operation_name = "insert" Then
      '  If str__prm_column_name.StartsWith(cls_constant.str_insert_user_column_name) Then Return False
      '  If str__prm_column_name.StartsWith(cls_constant.str_insert_user_column_name_pk) Then Return False
      '  If str__prm_column_name = cls_constant.str_active_flag_column_name Then Return False
      '  If str__prm_column_name = cls_constant.str_insert_date_column_name Then Return False
      'End If

      '' do not audit audit update fields on update as insert date in audit matches update date/user in base table
      'If str__prm_operation_name = "update" Then
      '  If str__prm_column_name.StartsWith(cls_constant.str_update_user_column_name) Then Return False
      '  If str__prm_column_name.StartsWith(cls_constant.str_update_user_column_name_pk) Then Return False
      '  If str__prm_column_name.StartsWith(cls_constant.str_update_date_column_name) Then Return False
      'End If

      ' '' '' ignore processing fields added (if they exist) 
      '' ''If fnc_convert_expected_string(dc__prm.ExtendedProperties.Item("database_column")) = "false" Then
      '' ''  Return False
      '' ''End If

      '' not auditing primary keys
      'If str__prm_column_name.StartsWith(cls_constant.str_primary_key_prefix) = True Then
      '  Return False
      'End If

      '' not auditing table id's
      'If str__prm_column_name.StartsWith(cls_constant.str_id_prefix) = True Then
      '  Return False
      'End If

      'Return True

    End Function

		Private Shared Function fnc_audit_get_table_suffix(ByVal str__prm_table_name As String, ByVal str__prm_column_name As String) As String

			Select Case cls_utility.fnc_get_datatype(str__prm_table_name, str__prm_column_name)
				Case "varchar", "nvarchar", "char", "bit"            'bit is returned as True by dataset
          Return "character"
				Case "datetime", "smalldatetime", "date"
					Return "datetime"
				Case "numeric", "decimal", "float", "money", "smallmoney", "real", "bigint", "int", "smallint", "tinyint"
					Return "numeric"
				Case "image"
					Return "image"
				Case "text", "ntext"
					Return "text"
				Case "uniqueidentifier"
					Return "uniqueidentifier"
			End Select

			Return "character"

		End Function

		Private Shared Function fnc_audit_decode(ByVal str__prm_column_name As String, ByVal str__prm_column_value As String) As String

			If fnc_convert_expected_string(str__prm_column_value).Length = 0 Then
				Return ""
			End If

			If str__prm_column_value = "null" Then
				Return ""
			End If

      ' when storing uniqueidentifiers, need to store user meaningful text
      '   TODO: This handles lookups if they follow naming convention
      '			need to handle all uniqueidentifiers (join on the way back out?) and 
      '			non standard lookup tables

      If str__prm_column_name.StartsWith(cls_constant.str_foreign_key_prefix__lookup) = True Then
				Return cls_data_access_layer.fnc_decode_lookup_for_db(str__prm_column_name, str__prm_column_value)
        ''This used .NET CLR but removed it because hosted SQL Server did not have CLR enabled
        ''Return "dbo.udf_decode_lookup(" & cls_utility.fnc_dbwrap(str__prm_column_name) & ", " & cls_utility.fnc_dbwrap(str__prm_column_value) & ")"
      End If

			If str__prm_column_name.StartsWith("fk_person_user__") Or str__prm_column_name = "fk_person_user" Or str__prm_column_name = "pk_person_user" Then
        ' if it is the current user, return onscreen name
        If str__prm_column_value = cls_current_user.str_pk_person_user Then
					Return cls_current_user.str_onscreen_name
				End If
				Return cls_utility.fnc_dbwrap(cls_current_user.fnc_get_person_user_value("onscreen_name"))
        ''This used .NET CLR but removed it because hosted SQL Server did not have CLR enabled
        ''Return "dbo.udf_get_person_user_name(" & cls_utility.fnc_dbwrap (str__prm_column_value) & ")"
      End If

			If str__prm_column_name.StartsWith("fk_person__") Or str__prm_column_name = "fk_person" Or str__prm_column_name = "pk_person" Then
        ' if it is the current user, return onscreen name
        Return cls_utility.fnc_dbwrap(cls_current_user.fnc_get_person_user_value("name__first_last"))
        ''This used .NET CLR but removed it because hosted SQL Server did not have CLR enabled
        ''Return "dbo.udf_get_person_user_name(" & cls_utility.fnc_dbwrap (str__prm_column_value) & ")"
      End If

      ' ''If str__prm_column_name.StartsWith("fk_organization_internal__") Or str__prm_column_name = "fk_organization_internal" Or str__prm_column_name = "pk_organization_internal" ThenThen
      ' ''  Return str__prm_column_value
      ' ''  ''Return "dbo.udf_audit_current_value('tbl_organization_internal', 'short_name', " & cls_utility.fnc_dbwrap (str__prm_column_value) & ")"
      ' ''End If

      If str__prm_column_name.StartsWith(cls_constant.str_foreign_key_prefix) Then
				Return "null"
			End If

      ' here is where the SQL server .NET assemblies are called.  problem in shared hosting
      ' commented out for the moment
      Return str__prm_column_value

		End Function

    ' all sql generation needs the following validation
    Private Sub sub_validate()

			sub_validate(Me)

		End Sub

		Private Sub sub_validate(ByVal inst__prm_sql As cls_sql)

			If fnc_convert_expected_string(inst__prm_sql.str__prv_table_name).Length = 0 Then
				Throw New Exception("Every cls_sql instance requires a table name")
			End If

			If inst__prm_sql.ord__prv_column.Count = 0 Then
				Throw New Exception("Every cls_sql instance requires at least one column: " & inst__prm_sql.str__prv_table_name)
			End If

		End Sub

		Private Function fnc_generate_sql_declare_unified(ByVal inst__prm_sql As cls_sql) As String

			Dim str_declare As New System.Text.StringBuilder

			For Each Str As String In inst__prm_sql.sdt__prf_declare.Keys
				str_declare.Append("declare @" & Str & " as nvarchar(max) ")
			Next

			Return str_declare.ToString

		End Function

		Public Function fnc_clone_sql(ByVal inst__prm_sql_source As cls_sql) As cls_sql

			Dim inst_sql_destination As cls_sql = New cls_sql

			inst_sql_destination.str__prv_operation = inst__prm_sql_source.str__prv_operation
			inst_sql_destination.str__prv_pk_log_system_audit_column = inst__prm_sql_source.str__prv_pk_log_system_audit_column
			inst_sql_destination.str__prv_pk_log_system_audit_table = inst__prm_sql_source.str__prv_pk_log_system_audit_table
			inst_sql_destination.str__prv_table_name = inst__prm_sql_source.str__prv_table_name
			inst_sql_destination.int_execute_sql_order = inst__prm_sql_source.int_execute_sql_order

			For Each str As String In inst__prm_sql_source.ord__prv_column.Keys
				inst_sql_destination.sub_add_column(str, inst__prm_sql_source.ord__prv_value(str), inst__prm_sql_source.ord__prv_control(str))
			Next

			Return inst_sql_destination

		End Function

		Public Sub sub_wrap_and_clean_sql(ByRef inst__prm_sql As cls_sql)

      ' dont think we ever want to wrap cls_sql values.  only do it when pulling them out (generate sql, etc.)
      Exit Sub

			For Each str As String In inst__prm_sql.ord__prv_column.Keys
				inst__prm_sql.sub_update_value(str, cls_utility.fnc_dbwrap(inst__prm_sql.ord__prv_value(str)))
			Next

		End Sub

		Public Sub sub_empty_sql(ByRef inst__prm_sql As cls_sql)

			For Each str As String In inst__prm_sql.ord__prv_column.Keys
				inst__prm_sql.sub_update_value(str, "")
			Next

		End Sub

		Public Sub sub_remove_empty_sql(ByRef inst__prm_sql As cls_sql)

			Dim arl As New ArrayList

			For Each str As String In inst__prm_sql.ord__prv_column.Keys
				If fnc_convert_expected_string(inst__prm_sql.ord__prv_value(str)).Length = 0 Then
					arl.Add(str)
				End If
			Next

			For Each Str As String In arl
				inst__prm_sql.sub_remove_column(Str)
			Next

		End Sub

		Private Function fnc_pre_action_select_sql(ByVal inst__prm_sql As cls_sql) As cls_sql

			Dim inst_sql_select As New cls_sql

      ' generate select sql, execute, reset operation to targetted, populate fields, return

      inst_sql_select.str__prv_operation = cls_sql.en_operation.select.ToString
			inst_sql_select.str__prv_table_name = inst__prm_sql.str__prv_table_name

			For Each str As String In inst__prm_sql.ord__prv_column.Keys
				inst_sql_select.sub_add_column(str, inst__prm_sql.ord__prv_value(str))
			Next

			Dim str_pk_name As String = fnc_primary_key_from_table(inst_sql_select.str_table_name)
			Dim str_pk_value As String = inst_sql_select.fnc_get_column_value(str_pk_name)

			Dim dr As SqlClient.SqlDataReader

			If inst__prm_sql.str_operation = cls_sql.en_operation.delete.ToString Then
        ' select * as pre-operation query when deleting
        dr = cls_data_access_layer.fnc_get_datareader("select * from " & inst_sql_select.str_table_name & " where " & str_pk_name & " = " & cls_utility.fnc_dbwrap(str_pk_value))
			Else
				dr = cls_data_access_layer.fnc_get_datareader(inst_sql_select.fnc_generate_select(inst_sql_select))
			End If

			sub_empty_sql(inst_sql_select)
			inst_sql_select.str__prv_operation = inst__prm_sql.str__prv_operation

			If dr.HasRows = False Then
				Return inst_sql_select
			End If

			dr.Read()

			For int_ctr As Int32 = 0 To dr.FieldCount - 1
				inst_sql_select.sub_add_or_update_column(dr.GetName(int_ctr), dr(int_ctr).ToString)
			Next

			dr.Close()

			Return inst_sql_select

		End Function

		Private Function fnc_get_delta_sql(ByRef inst__prm_sql_prior As cls_sql, ByRef inst__prm_sql_presented As cls_sql) As cls_sql

			Dim inst_sql_delta As New cls_sql

			inst_sql_delta.str__prv_operation = inst__prm_sql_prior.str_operation
			inst_sql_delta.str__prv_table_name = inst__prm_sql_prior.str__prv_table_name

			For Each str As String In inst__prm_sql_prior.ord__prv_column.Keys
				If fnc_check_equality(inst__prm_sql_prior.fnc_get_column_value(str), inst__prm_sql_presented.fnc_get_column_value(str), fnc_get_datatype(inst__prm_sql_prior.str_table_name, str)) = False Then
					inst_sql_delta.sub_add_column(str, inst__prm_sql_presented.fnc_get_column_value(str), inst__prm_sql_presented.fnc_get_control(str))
				Else
          ' remove unchanged data between presented and prior
          ''If fnc_convert_expected_int32(ConfigurationManager.AppSettings("int_enable_dynamic_remove_unedited")) = 1 _
          ''And str.StartsWith(cls_constant.str_primary_key_prefix) = False Then
          ''  ' if dynamic remove is enabled and not primary key, remove value
          ''  inst__prm_sql_presented.sub_remove_column(str)
          ''End If
        End If
			Next

			Return inst_sql_delta

		End Function

		Public Shared Function fnc_get_changed_data(ByVal inst__prm_sql_collection As cls_sql_collection) As DataTable

			Dim dt As DataTable
			Dim dr As DataRow

			Dim int_row_count As Int32 = 0

      'create a DataTable        
      dt = New DataTable
			dt.Columns.Add(New DataColumn("row_number", GetType(Integer)))
			dt.Columns.Add(New DataColumn("operation", GetType(String)))
			dt.Columns.Add(New DataColumn("table_name", GetType(String)))
			dt.Columns.Add(New DataColumn("primary_key_value", GetType(String)))
			dt.Columns.Add(New DataColumn("column_name", GetType(String)))
			dt.Columns.Add(New DataColumn("old_value", GetType(String)))
			dt.Columns.Add(New DataColumn("new_value", GetType(String)))
			dt.Columns.Add(New DataColumn("insert_date", GetType(DateTime)))

      ' For each sql in the returned dynamic update collection, show old/new values
      '   cannot use audit sql objects because auditing may not have been called (or is disabled)
      For Each inst_sql As cls_sql In inst__prm_sql_collection

        ' audit header is added to top of list of classes to process whereas all other audit lives under each sql class
        If inst_sql.str_table_name = "tbl_log_system_audit_header" Then GoTo skip_sql

				For Each str_column_name As String In inst_sql.inst_sql__delta.ord__prv_column.Keys

					If str_column_name.StartsWith(cls_constant.str_foreign_key_prefix) Then
						If str_column_name.StartsWith(cls_constant.str_foreign_key_prefix__lookup) = False Then
              ' we don't want to show changes to foreign keys the system "stamps" to support onscreen joins
              If fnc_exists_as_pk(inst__prm_sql_collection, str_column_name) = True Then
								GoTo skip_column
							End If
						End If
					End If

					dr = dt.NewRow()
					dr("row_number") = int_row_count
					dr("operation") = inst_sql.inst_sql__presented.str_operation
					dr("table_name") = inst_sql.inst_sql__delta.str_table_name
					dr("primary_key_value") = inst_sql.inst_sql__presented.fnc_get_column_value(fnc_primary_key_from_table(inst_sql.inst_sql__presented.str_table_name))
					dr("column_name") = cls_utility.fnc_get_display_name(inst_sql.inst_sql__presented.str_table_name, str_column_name)
					dr("old_value") = inst_sql.inst_sql__prior.fnc_get_column_value(str_column_name)
					dr("new_value") = inst_sql.inst_sql__delta.fnc_get_column_value(str_column_name)
					dr("insert_date") = DateTime.Now.ToShortTimeString

					int_row_count = int_row_count + 1

					dt.Rows.Add(dr)

skip_column:
				Next

skip_sql:
			Next

      'return the DataTable     
      Return dt

		End Function

		Public Shared Function fnc_get_changed_data(ByVal inst__prm_sql_collection As cls_sql_collection, ByVal str__prm_column_name As String) As String

      ' loop through all returned sql and see if column requested is in presented (typical request is newly created pk)
      For Each inst_sql As cls_sql In inst__prm_sql_collection
				For Each str_column_name As String In inst_sql.inst_sql__presented.ord__prv_column.Keys
          ' this will return the first occurance of the requested column
          ' when updating multiple rows with the same column name, new method is needed to return collection
          If str_column_name = str__prm_column_name Then
						Return inst_sql.inst_sql__presented.fnc_get_column_value(str_column_name)
					End If
				Next
			Next

      ' could not find the requested column
      Return Nothing

		End Function

		Public Shared Function fnc_exists_as_pk(ByVal inst__prm_sql_collection As cls_sql_collection, ByVal str__prm_column_name As String) As Boolean

			Dim str_primary_key_name = Replace(str__prm_column_name, cls_constant.str_foreign_key_prefix, cls_constant.str_primary_key_prefix)

			For Each inst_sql As cls_sql In inst__prm_sql_collection
				For Each str_column_name As String In inst_sql.inst__prv_sql__presented.ord__prv_column.Keys
					If str_column_name = str_primary_key_name Then
						Return True
					End If
				Next

			Next

			Return False

		End Function

	End Class

	Public Class cls_sql_collection
		Inherits List(Of cls_sql)

		Protected Friend sdt__prf_declare As New StringDictionary

    ' str__prv_pk_log_system_audit_header property
    Private str__prv_pk_log_system_audit_header As String = ""
		Public Property str_str__prv_pk_log_system_audit_header() As String

			Get
				Return str__prv_pk_log_system_audit_header
			End Get
			Set(ByVal Value As String)
				str__prv_pk_log_system_audit_header = Value
			End Set

		End Property

		Public Sub sub_execute()

			Dim str_sql As String = fnc_get_sql(True, True)
			Dim str_sql_audit As String = fnc_get_sql_audit(Nothing, True, True)

			If str_sql.Length > 0 Then
				cls_data_access_layer.sub_execute_non_query(str_sql)
				cls_asynchronous.sub_execute_non_query(str_sql_audit)
			End If

		End Sub

    ' sql added with a execute_sql_order will execute in order.  all sql added without an order will be numbered as they were added (audit tables, etc.)
    Public Sub sub_sequence_unordered_sql()

			Dim int_execute_sql_as_added As Int32 = 1000

      ' loop through all sql and assign a sequence if not already assigned.
      For Each inst_sql As cls_sql In Me
				If inst_sql.int_execute_sql_order = 0 Then
					inst_sql.int_execute_sql_order = int_execute_sql_as_added
					int_execute_sql_as_added = int_execute_sql_as_added + 1
				End If

        ' loop through all audit sql for each sql and assign a sequence if not already assigned.
        For Each inst_sql__audit As cls_sql In inst_sql.inst_sql_collection__audit
					If inst_sql__audit.int_execute_sql_order = 0 Then
						inst_sql__audit.int_execute_sql_order = int_execute_sql_as_added
						int_execute_sql_as_added = int_execute_sql_as_added + 1
					End If
				Next
			Next

		End Sub

		Public Sub sub_execute(ByVal bln__prm_use_transaction As Boolean, ByVal bln__prm_add_audit As Boolean)

			Dim str_sql As String = fnc_get_sql(bln__prm_use_transaction, bln__prm_add_audit)
			Dim str_sql_audit As String = fnc_get_sql_audit(Nothing, bln__prm_use_transaction, bln__prm_add_audit)

			If str_sql.Length > 0 Then
				cls_data_access_layer.sub_execute_non_query(str_sql)
				cls_asynchronous.sub_execute_non_query(str_sql_audit)
			End If

		End Sub

		Public Function fnc_get_sql(ByVal bln__prm_use_transaction As Boolean, ByVal bln__prm_add_audit As Boolean) As String

			Dim bln_sql_to_run As Boolean = False

      ' no sql to run, bagout
      If Me.Count = 0 Then
				Return ""
			End If

      ' make sure all ppd exists as we need declares for presented sql
      For Each inst_sql As cls_sql In Me
				inst_sql.sub_generate_ppd(inst_sql)
			Next

      ' order all unordered sql
      Me.sub_sequence_unordered_sql()

      ' this only sorts the primary sql statements.  audit sql is a child class to each sql and is not sorted with this
      '   will either make audit sql on the same level as standard sql or handle below with new collection of sql
      Me.Sort(New GenericComparer(Of cls_sql)("int_execute_sql_order", GenericComparer(Of ns_enterprise.cls_sql).en_sort_order.ascending))

      'TODO verify we need this sorteddictionary.  Believe it was only used to get the audit in order.  sort above
      '  should put sql in order
      Dim ord_sql As New SortedDictionary(Of Int32, String)

      ' pass though to get all declares needed at top of batched sql statement
      For Each inst_sql As cls_sql In Me
        ' only address cls_sql with delta (something to do)
        If inst_sql.inst_sql__delta.fnc_get_column_collection.Count > 0 Then
					bln_sql_to_run = True
					For Each str_column As String In inst_sql.inst_sql__presented.fnc_get_column_collection.Keys
						If sdt__prf_declare.ContainsKey(str_column) = False Then
							sdt__prf_declare.Add(str_column, str_column)
						End If
					Next
          ' TODO - this if is the only reason the sorteddictionary is still being used.  once two calls to update are clean, 
          '   this can go.
          If ord_sql.ContainsKey(inst_sql.int_execute_sql_order) Then
            ' TODO - rather than throw this error, clear everything at initial call or end of call so each call will be fresh
            Throw New Exception("Ordered sql already exists, make sure each call to dynamic_sql update is done with a new instance")
					End If
					ord_sql.Add(inst_sql.int_execute_sql_order, "  " & inst_sql.fnc_get_sql(inst_sql, False))
				End If
			Next

      ' all deltas are empty, no sql to run
      If bln_sql_to_run = False Then
				Return ""
			End If

			Dim str_sql As New System.Text.StringBuilder()

      ' add declare for batched sql (no duplicates)
      str_sql.Append(fnc_get_sql_declare())

			If bln__prm_use_transaction = True Then
				str_sql.Append("begin tran;")
			End If

			For Each str_sql__ordered As String In ord_sql.Values
				str_sql.Append(str_sql__ordered)
			Next

			If bln__prm_use_transaction = True Then
				str_sql.Append("commit;")
			End If

			Return str_sql.ToString

		End Function

		Public Function fnc_get_sql_audit(ByRef obj__prm_pk_log_system_audit_header As Object, ByVal bln__prm_use_transaction As Boolean, ByVal bln__prm_add_audit As Boolean) As String

			sdt__prf_declare = New StringDictionary
			Dim ord_sql As New SortedDictionary(Of Int32, String)

      ' If add header was requested, it's added as top cls sql in collection.  need declares for it.
      If bln__prm_add_audit = True Then
				sub_insert_audit_header()

				Dim inst_sql__audit_header As cls_sql = Me(0)
				inst_sql__audit_header.sub_generate_ppd(inst_sql__audit_header)
				For Each str_column__audit As String In inst_sql__audit_header.inst_sql__presented.fnc_get_column_collection.Keys
					If sdt__prf_declare.ContainsKey(str_column__audit) = False Then
						sdt__prf_declare.Add(str_column__audit, str_column__audit)
					End If
				Next
				ord_sql.Add(0, "  " & inst_sql__audit_header.fnc_get_sql(inst_sql__audit_header, False))
			End If

			Dim int_loop As Int32 = 1

      ' this block will make sure the fk audit header is correct and add to declare collection
      For Each inst_sql As cls_sql In Me

				inst_sql.sub_generate_audit(str__prv_pk_log_system_audit_header, False)

				For Each inst_sql__audit As cls_sql In inst_sql.inst_sql_collection__audit
          ' make sure all audit in collection is associated with single audit header.
          ' NOTE: this only involves sql in collection that has audit.  some may not.
          If inst_sql__audit.str_table_name = "tbl_log_system_audit_table" Then
						inst_sql__audit.sub_update_value("fk_log_system_audit_header", str__prv_pk_log_system_audit_header)
					End If
					inst_sql__audit.sub_generate_ppd(inst_sql__audit)
					For Each str_column__audit As String In inst_sql__audit.inst_sql__presented.fnc_get_column_collection.Keys
						If sdt__prf_declare.ContainsKey(str_column__audit) = False Then
							sdt__prf_declare.Add(str_column__audit, str_column__audit)
						End If
					Next
					ord_sql.Add(int_loop, "  " & inst_sql__audit.fnc_get_sql(inst_sql__audit, False))
					int_loop = int_loop + 1
				Next
			Next

			Dim str_sql As New System.Text.StringBuilder()

      ' add declare for batched sql (no duplicates)
      str_sql.Append(fnc_get_sql_declare())

			If bln__prm_use_transaction = True Then
				str_sql.Append("begin tran;")
			End If

			For Each str_sql__ordered As String In ord_sql.Values
				str_sql.Append(str_sql__ordered)
			Next

			If bln__prm_use_transaction = True Then
				str_sql.Append("commit;")
			End If

			obj__prm_pk_log_system_audit_header = str__prv_pk_log_system_audit_header

			Return str_sql.ToString

		End Function

		Private Sub sub_insert_audit_header()

			str__prv_pk_log_system_audit_header = Guid.NewGuid.ToString
			sub_insert_audit_header(str__prv_pk_log_system_audit_header)

		End Sub

		Private Sub sub_insert_audit_header(ByVal str__prm_pk_log_system_audit_header As String)

			Me.Insert(0, cls_data_audit.fnc_create_header(str__prm_pk_log_system_audit_header))

		End Sub

		Private Function fnc_get_sql_declare() As String

			Dim str_declare As New System.Text.StringBuilder

			For Each str As String In sdt__prf_declare.Keys
				str_declare.Append("declare @" & str & " as nvarchar(max) ")
			Next

			Return str_declare.ToString

		End Function

	End Class

End Namespace
