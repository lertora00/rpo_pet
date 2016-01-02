Imports ns_enterprise.cls_utility

Imports System.Data

Imports System.Web
Imports System.Configuration

Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.UI
Imports System.Web.HttpContext
Imports System.ComponentModel.Design.StandardToolWindows
Imports System.Reflection

Namespace ns_enterprise

	Public Class cls_user_interface

    ' this class contains methods related to user interface development

    ' this routine will bind a dropdownlist to a specified sql string
    ' it will make the first field the datavalue and the second the datatext
    Public Shared Sub sub_drpdwn_databind _
		(
		 ByRef drpdwn__prm As UI.WebControls.DropDownList,
		 ByVal obj__prm_datasource As Object,
		 ByVal bln_insert_none As Boolean
		)

			If drpdwn__prm Is Nothing Then
				Throw New Exception("drp passed has no value")
			End If

      ' bind and insert standard none option
      drpdwn__prm.DataSource = obj__prm_datasource
			drpdwn__prm.DataBind()

			If bln_insert_none = True Then
				sub_drpdwn_insert_none(drpdwn__prm)
			End If

		End Sub

    ' this routine will bind a dropdownlist to a specified sql string
    ' it will make the first field the datavalue and the second the datatext
    Public Shared Sub sub_drpdwn_databind _
		(
		 ByRef drpdwn__prm As UI.WebControls.DropDownList,
		 ByVal str__prm_sql As String
		)

      ' call private method passing public properties
      sub_drpdwn_databind__private(drpdwn__prm, str__prm_sql, True, "", "")

		End Sub

    ' this routine will bind a dropdownlist to a specified sql string
    ' it will make the first field the datavalue and the second the datatext
    ' it will also insert the standard none text
    Public Shared Sub sub_drpdwn_databind _
		(
		 ByRef drpdwn__prm As UI.WebControls.DropDownList,
		 ByVal str__prm_sql As String,
		 ByVal bln__prm_insert_none As Boolean
		)

      ' call private method passing public properties
      sub_drpdwn_databind__private(drpdwn__prm, str__prm_sql, True, "", "")

			If bln__prm_insert_none = True Then
				sub_drpdwn_insert_none(drpdwn__prm)
			End If

		End Sub

    ' this routine will bind a dropdownlist to a specified sql string
    ' it will make the first field the datavalue and the second the datatext
    ' it also includes a boolean to ignore to make the overloaded signature different
    Public Shared Sub sub_drpdwn_databind _
		(
		ByRef drpdwn__prm As UI.WebControls.DropDownList,
		ByVal str__prm_sql As String,
		ByVal str__prm_data_value_field__top As String,
		ByVal str__prm_data_text_field__top As String,
		ByVal bln__prm_ignore_this_parameter As Boolean
		)

      ' call private method passing public properties
      sub_drpdwn_databind__private(drpdwn__prm, str__prm_sql, True, "", "")

		End Sub

    ' this routine will bind a dropdownlist to a specified sql string
    Public Shared Sub sub_drpdwn_databind _
		(
		ByRef drpdwn__prm As UI.WebControls.DropDownList,
		ByVal str__prm_sql As String,
		ByVal str__prm_data_value_field As String,
		ByVal str__prm_data_text_field As String
		)

      ' call private method passing public properties
      sub_drpdwn_databind__private(drpdwn__prm, str__prm_sql, False, str__prm_data_value_field, str__prm_data_text_field)

		End Sub

    ' this routine will bind a dropdownlist to a specified sql string
    Public Shared Sub sub_drpdwn_databind _
		(
		ByVal drpdwn__prm As UI.WebControls.DropDownList,
		ByVal str__prm_sql As String,
		ByVal str__prm_data_value_field As String,
		ByVal str__prm_data_text_field As String,
		ByVal str__prm_data_value_field__top As String,
		ByVal str__prm_data_text_field__top As String
		)

      ' call private method passing public properties
      sub_drpdwn_databind__private(drpdwn__prm, str__prm_sql, False, str__prm_data_value_field, str__prm_data_text_field)

		End Sub

    ' takes a lookup table name and filter down condition and returns abbreviated list of lookup value (if filtered down in db)
    Public Shared Sub sub_drpdwn_databind _
		 (
		 ByRef drpdwn__prm As UI.WebControls.DropDownList,
		 ByVal str__prm_lkp_table_name As String,
		 ByVal str__prm_data_text_field As String,
		 ByVal str__prm_filter_down_condition As String,
		 ByVal int__prm_filter_down_signature_overload As Int32
		 )

			Dim dt As DataTable = cls_data_access_layer.fnc_get_lkp__filter_down(str__prm_lkp_table_name, str__prm_filter_down_condition)

			If drpdwn__prm.DataTextField.Length = 0 Then
				drpdwn__prm.DataTextField = "name"
			Else
				drpdwn__prm.DataTextField = str__prm_data_text_field
			End If

			drpdwn__prm.DataValueField = cls_utility.fnc_primary_key_from_table(str__prm_lkp_table_name)

			drpdwn__prm.DataSource = dt
			drpdwn__prm.DataBind()

		End Sub

    ' private method with full parameter list
    ' this method assists in binding a dropdownlist to a sql string
    ' table must have name and sort_order columns for this to work
    Private Shared Sub sub_drpdwn_databind__private _
		 (
		 ByRef drpdwn__prm As UI.WebControls.DropDownList,
		 ByVal str__prm_sql As String,
		 ByVal bln__determine_datavalue_datatext As Boolean,
		 ByVal str__prm_data_value_field As String,
		 ByVal str__prm_data_text_field As String
		 )

			Dim dt_local As DataTable = Nothing
			Dim str_table_name As String = ""
			Dim bln_lookup_table As Boolean = False

      ' convert table name to sql statement
      Dim str_sql As String = str__prm_sql
			If str_sql.StartsWith(cls_constant.str_table_prefix__lookup) Then
				bln_lookup_table = True
				str_table_name = str_sql
        ' next statement doesn't appear to do anything.  was likely never removed after cached get lkp was added
        str_sql = "select " & cls_utility.fnc_primary_key_from_table(str_table_name) & ", name from " & str_table_name & " where " & cls_constant.str_active_flag_column_name & " = 1 order by sort_order, name"
				dt_local = cls_data_access_layer.fnc_get_lkp(str_table_name)
			End If

			If dt_local Is Nothing Then
				dt_local = cls_data_access_layer.fnc_get_dataset(str_sql).Tables(0)
			End If

      ' identify datavalue and datatext field names if not provided in ASPX
      If drpdwn__prm.DataTextField.Length = 0 Then
				If bln__determine_datavalue_datatext = True Then
					drpdwn__prm.DataValueField = dt_local.Columns(0).ColumnName
					If dt_local.Columns.Count = 1 Then
						drpdwn__prm.DataTextField = dt_local.Columns(0).ColumnName
					Else
						If bln_lookup_table = True Then
							drpdwn__prm.DataTextField = "name"
						Else
							drpdwn__prm.DataTextField = dt_local.Columns(1).ColumnName
						End If
					End If
				Else
					drpdwn__prm.DataValueField = str__prm_data_value_field
					drpdwn__prm.DataTextField = str__prm_data_text_field
				End If
			End If

      ' bind datareader to dropdownlist
      drpdwn__prm.DataSource = dt_local
			drpdwn__prm.DataBind()

		End Sub

    ' will insert a value (at the top) for a dropdown by passed in text and value
    Public Shared Sub sub_drpdwn_insert_item _
		(
		ByRef drpdwn__prm As UI.WebControls.DropDownList,
		ByVal str__prm_data_value_field__top As String,
		ByVal str__prm_data_text_field__top As String
		)

			Dim listitem_local As ListItem = New ListItem(str__prm_data_text_field__top, str__prm_data_value_field__top)
			drpdwn__prm.Items.Insert(0, listitem_local)

		End Sub

    ' will insert standard none (at the top) for a dropdown
    Public Shared Sub sub_drpdwn_insert_none _
		(
		 ByRef drpdwn__prm As UI.WebControls.DropDownList
		)

			Dim listitem_local As ListItem = New ListItem(ConfigurationManager.AppSettings("default_dropdown_none_text"), ConfigurationManager.AppSettings("default_dropdown_none_value"))
			drpdwn__prm.Items.Insert(0, listitem_local)

		End Sub

    ' will select a value for a dropdown by passed in value
    Public Shared Sub sub_drpdwn_select_by_value _
		(
		ByRef drpdwn__prm As UI.WebControls.DropDownList,
		ByVal str__prm_value As String
		)

			If drpdwn__prm Is Nothing Then
				Throw New Exception("sub_drpdwn_select_by_value:drpdwn__prm passed in is nothing.")
			End If

			Dim str_value As String = fnc_convert_expected_string(str__prm_value)

      ' TODO rather than try catch, consider trying to find the value a different way
      '		try catch is expensive
      drpdwn__prm.ClearSelection()

			If drpdwn__prm.Items.FindByValue(str_value) Is Nothing Then
			Else
				drpdwn__prm.Items.FindByValue(str_value).Selected = True
			End If

		End Sub

    ' will select a value for a dropdown by passed in text value
    Public Shared Sub sub_drpdwn_select_by_text _
		(
		ByRef drpdwn__prm As UI.WebControls.DropDownList,
		ByVal str__prm_text As String
		)

			If drpdwn__prm Is Nothing Then
				Throw New Exception("sub_drpdwn_select_by_text:drpdwn__prm passed in is nothing.")
			End If

      ' TODO rather than try catch, consider trying to find the value a different way
      '		try catch is expensive
      Try
				drpdwn__prm.Items.FindByText(str__prm_text).Selected = True
			Catch
			End Try

		End Sub

    ' wrap paging stored procedure and find to repeater
    Public Function fnc_paging_repeater_databind _
		 (
		 ByRef rpt__prm As Repeater,
		 ByVal str__prm_sql As String,
		 ByVal int__prm_page_number As Int32,
		 ByVal int__prm_page_size As Int32
		 ) _
		 As DataSet

      ' using dataset as multiple results sets are returned
      Dim ds_paging As DataSet
			Dim str_sql As String = ""

      ' escape single quotes and wrap paging stored procedure
      str_sql = "exec stp_paging__fast " & fnc_dbwrap(str__prm_sql) & ", " & int__prm_page_number & ", " & int__prm_page_size
			Try
				ds_paging = cls_data_access_layer.fnc_get_dataset(str_sql)
			Catch ex As Exception
				Throw (ex)
			End Try

			If ds_paging.Tables(0).Rows.Count = 0 And ds_paging.Tables(1).Rows.Count > 0 Then
        ' rerun query as user requested page number / row count combination beyond last record
        ' TODO update stored procedure rather than rerunning query
        ' escape single quotes and wrap paging stored procedure
        str_sql = "exec stp_paging__fast " & fnc_dbwrap(str__prm_sql) & ", " & ds_paging.Tables(1).Rows(0)("total_page_count").ToString & ", " & int__prm_page_size
				Try
					ds_paging = cls_data_access_layer.fnc_get_dataset(str_sql)
				Catch ex As Exception
					Throw (ex)
				End Try
			End If

			rpt__prm.DataSource = ds_paging.Tables(0)
			rpt__prm.DataBind()

			Return ds_paging

		End Function

		Public Function fnc_writetxt(ByVal obj__prm_text As Object) As String

			If IsDBNull(obj__prm_text) OrElse obj__prm_text.ToString = "" Then
				Return "writetxt("""")"
			End If

      ' replace single and double quotes as they cause javascript errors
      Dim str_text As String = obj__prm_text.ToString
			str_text = Replace(str_text, """", "``")
			str_text = Replace(str_text, "'", "`")
			str_text = Replace(str_text, vbCrLf, "")

			Return "writetxt(""" & str_text & """)"

		End Function

    ' return a controls command name if it exists
    Public Shared Function fnc_get_control_command_name(ByVal ctl__prm As Control) As String

			If ctl__prm Is Nothing = True Then Return ""

			Select Case TypeName(ctl__prm).ToLower
				Case "button"
					Return fnc_convert_expected_string(DirectCast(ctl__prm, Button).CommandName)
				Case "linkbutton"
					Return fnc_convert_expected_string(DirectCast(ctl__prm, LinkButton).CommandName)
				Case "imagebutton"
					Return fnc_convert_expected_string(DirectCast(ctl__prm, ImageButton).CommandName)
			End Select

			Return ""

		End Function

    ' todo - this method needs to return a collection of controls that have the attribute name requested
    ' a seperate method can determine if the attribute value is duplicated
    Public Shared Function fnc_get_control_attribute_value_collection(ByVal ctl__prm_container As Control, ByVal str__prm_attribute_name As String, Optional ByVal bln__prm_only_with_value As Boolean = True) As IDictionary

			Dim idic_filter As IDictionary = New Dictionary(Of String, String)
			Dim col_control As Collection
			Dim str_control_value As String = ""

      ' get all controls that have a "key" attribute (key used for list filters)
      col_control = fnc_get_control_with_attribute(ctl__prm_container, str__prm_attribute_name)

      ' for each key control, add it to a collection to return.
      For Each ctl As Control In col_control
				str_control_value = fnc_get_control_value(ctl)
				If str_control_value.Length > 0 Or bln__prm_only_with_value = False Then
					If idic_filter.Contains(fnc_get_control_attribute_value(ctl, str__prm_attribute_name)) Then Throw New Exception(fnc_get_control_attribute_value(ctl, str__prm_attribute_name) & " already exists in collection")
					idic_filter.Add(fnc_get_control_attribute_value(ctl, str__prm_attribute_name), str_control_value)
				End If
			Next

			Return idic_filter

		End Function

    ' return a named attribute from a control
    Public Shared Function fnc_get_control_attribute_value(ByRef ctl__prm As Control, ByVal str__prm_attribute_name As String) As String

			If fnc_verify_control_has_attribute(ctl__prm, str__prm_attribute_name) = False Then
				Return ""
			End If

      ' return requested attribute value if WebControl
      If ctl__prm.GetType.IsSubclassOf(GetType(WebControl)) Then
				If DirectCast(ctl__prm, WebControl).Attributes(str__prm_attribute_name) Is Nothing Then
					Return ""
				Else
					Return fnc_convert_expected_string(DirectCast(ctl__prm, WebControl).Attributes(str__prm_attribute_name))
				End If
			End If

      ' return requested attribute value if HtmlControl
      If ctl__prm.GetType.IsSubclassOf(GetType(HtmlControl)) Then
				If DirectCast(ctl__prm, HtmlControl).Attributes(str__prm_attribute_name) Is Nothing Then
					Return ""
				Else
					Return fnc_convert_expected_string(DirectCast(ctl__prm, HtmlControl).Attributes(str__prm_attribute_name))
				End If
			End If

      ' return requested attribute value if UserControl
      If ctl__prm.GetType.IsSubclassOf(GetType(UserControl)) Then
				If DirectCast(ctl__prm, UserControl).Attributes(str__prm_attribute_name) Is Nothing Then
					Return ""
				Else
					Return fnc_convert_expected_string(DirectCast(ctl__prm, UserControl).Attributes(str__prm_attribute_name))
				End If
			End If

      'If ctl__prm.GetType.IsSubclassOf(GetType(ns_databased.cls_db_control)) Then
      '  Dim prix As Reflection.PropertyInfo() = ctl__prm.GetType.GetProperties()
      '  For Each pri As Reflection.PropertyInfo In prix
      '    If pri.Name.ToLower = str__prm_attribute_name.ToLower Then
      '      Return fnc_convert_expected_string(pri.GetValue(ctl__prm, Nothing))
      '    End If
      '  Next
      'End If

      Return ""

		End Function

    ' return a named attribute from a control
    Public Shared Function fnc_remove_control_attribute_value(ByRef ctl__prm As Control, ByVal str__prm_attribute_name As String) As Boolean

			If ctl__prm Is Nothing = True Then
				Return False
			End If

      ' a control without an id is not of interest
      If ctl__prm.ID Is Nothing OrElse ctl__prm.ID.Length = 0 Then
				Return False
			End If

      ' a repeater is in the webcontrols namespace but cannot be coerced into a webcontrol
      ' a placeholder does not support custom attributes (i.e. repeater custom attribute is not allowed)
      If TypeOf (ctl__prm) Is Repeater Or TypeOf (ctl__prm) Is PlaceHolder Then
				Return False
			End If

			Try
        ' return requested attribute value if WebControl
        If ctl__prm.GetType.IsSubclassOf(GetType(WebControl)) Then
          ' if attribute found, attempt to remove it
          If DirectCast(ctl__prm, WebControl).Attributes(str__prm_attribute_name) Is Nothing = False Then
						DirectCast(ctl__prm, WebControl).Attributes.Remove(str__prm_attribute_name)
						Return True
					End If
				End If

        ' return requested attribute value if HtmlControl
        If ctl__prm.GetType.IsSubclassOf(GetType(HtmlControl)) Then
          ' if attribute found, attempt to remove it
          If DirectCast(ctl__prm, HtmlControl).Attributes(str__prm_attribute_name) Is Nothing = False Then
						DirectCast(ctl__prm, HtmlControl).Attributes.Remove(str__prm_attribute_name)
						Return True
					End If
				End If

        ' return requested attribute value if UserControl
        If ctl__prm.GetType.IsSubclassOf(GetType(UserControl)) Then
          ' if attribute found, attempt to remove it
          If DirectCast(ctl__prm, UserControl).Attributes(str__prm_attribute_name) Is Nothing = False Then
						DirectCast(ctl__prm, UserControl).Attributes.Remove(str__prm_attribute_name)
						Return True
					End If
				End If
			Catch
        ' could not be cast to control type even though it is in type namespace (i.e. repeater cannot be cast to webcontrol
        '   repeater is handled expicitly above but serves as good example of cast failure)
        Return False
			End Try

      ' if here, assumed to fail
      Return False

		End Function

    ' insert if not exists or update an existing control attribute value
    Public Shared Function fnc_insert_or_update_control_attribute_value(ByRef ctl__prm As Control, ByVal str__prm_attribute_name As String, ByVal str__prm_attribute_value As String) As Boolean

			If ctl__prm Is Nothing = True Then
				Return False
			End If

      ' a control without an id is not of interest
      If ctl__prm.ID Is Nothing OrElse ctl__prm.ID.Length = 0 Then
				Return False
			End If

      ' a repeater is in the webcontrols namespace but cannot be coerced into a webcontrol
      ' a placeholder does not support custom attributes (i.e. repeater custom attribute is not allowed)
      If TypeOf (ctl__prm) Is Repeater Or TypeOf (ctl__prm) Is PlaceHolder Then
				Return False
			End If

			Try
        ' return requested attribute value if WebControl
        If ctl__prm.GetType.IsSubclassOf(GetType(WebControl)) Then
          ' if attribute not found, attempt to add it
          If DirectCast(ctl__prm, WebControl).Attributes(str__prm_attribute_name) Is Nothing Then
						DirectCast(ctl__prm, WebControl).Attributes.Add(str__prm_attribute_name, str__prm_attribute_value)
						Return True
					Else
						DirectCast(ctl__prm, WebControl).Attributes.Item(str__prm_attribute_name) = str__prm_attribute_value
						Return True
					End If
				End If

        ' return requested attribute value if HtmlControl
        If ctl__prm.GetType.IsSubclassOf(GetType(HtmlControl)) Then
					If DirectCast(ctl__prm, HtmlControl).Attributes(str__prm_attribute_name) Is Nothing Then
						DirectCast(ctl__prm, HtmlControl).Attributes.Add(str__prm_attribute_name, str__prm_attribute_value)
						Return True
					Else
						DirectCast(ctl__prm, HtmlControl).Attributes.Item(str__prm_attribute_name) = str__prm_attribute_value
						Return True
					End If
				End If

        ' return requested attribute value if UserControl
        If ctl__prm.GetType.IsSubclassOf(GetType(UserControl)) Then
					If DirectCast(ctl__prm, UserControl).Attributes(str__prm_attribute_name) Is Nothing Then
						DirectCast(ctl__prm, UserControl).Attributes.Add(str__prm_attribute_name, str__prm_attribute_value)
						Return True
					Else
						DirectCast(ctl__prm, UserControl).Attributes.Item(str__prm_attribute_name) = str__prm_attribute_value
						Return True
					End If
				End If
			Catch
        ' could not be cast to control type even though it is in type namespace (i.e. repeater cannot be cast to webcontrol
        '   repeater is handled expicitly above but serves as good example of cast failure)
        Return False
			End Try

      ' if here, assumed to fail
      Return False

		End Function

    ' return a named attribute from a control
    Public Shared Function fnc_insert_control_attribute_value(ByRef ctl__prm As Control, ByVal str__prm_attribute_name As String, ByVal str__prm_attribute_value As String) As Boolean

			If ctl__prm Is Nothing = True Then
				Return False
			End If

      ' a control without an id is not of interest
      If ctl__prm.ID Is Nothing OrElse ctl__prm.ID.Length = 0 Then
				Return False
			End If

      ' a repeater is in the webcontrols namespace but cannot be coerced into a webcontrol
      ' a placeholder does not support custom attributes (i.e. repeater custom attribute is not allowed)
      If TypeOf (ctl__prm) Is Repeater Or TypeOf (ctl__prm) Is PlaceHolder Then
				Return False
			End If

			Try
        ' return requested attribute value if WebControl
        If ctl__prm.GetType.IsSubclassOf(GetType(WebControl)) Then
          ' if attribute not found, attempt to add it
          If DirectCast(ctl__prm, WebControl).Attributes(str__prm_attribute_name) Is Nothing Then
						DirectCast(ctl__prm, WebControl).Attributes.Add(str__prm_attribute_name, str__prm_attribute_value)
						Return True
					End If
				End If

        ' return requested attribute value if HtmlControl
        If ctl__prm.GetType.IsSubclassOf(GetType(HtmlControl)) Then
					If DirectCast(ctl__prm, HtmlControl).Attributes(str__prm_attribute_name) Is Nothing Then
						DirectCast(ctl__prm, HtmlControl).Attributes.Add(str__prm_attribute_name, str__prm_attribute_value)
						Return True
					End If
				End If

        ' return requested attribute value if UserControl
        If ctl__prm.GetType.IsSubclassOf(GetType(UserControl)) Then
					If DirectCast(ctl__prm, UserControl).Attributes(str__prm_attribute_name) Is Nothing Then
						DirectCast(ctl__prm, UserControl).Attributes.Add(str__prm_attribute_name, str__prm_attribute_value)
						Return True
					End If
				End If
			Catch
        ' could not be cast to control type even though it is in type namespace (i.e. repeater cannot be cast to webcontrol
        '   repeater is handled expicitly above but serves as good example of cast failure)
        Return False
			End Try

      ' if here, assumed to fail
      Return False

		End Function

    ' return a named attribute from a control
    Public Shared Function fnc_update_control_attribute_value(ByRef ctl__prm As Control, ByVal str__prm_attribute_name As String, ByVal str__prm_attribute_value As String) As Boolean

			If ctl__prm Is Nothing = True Then
				Return False
			End If

      ' a control without an id is not of interest
      If ctl__prm.ID Is Nothing OrElse ctl__prm.ID.Length = 0 Then
				Return False
			End If

      ' a repeater is in the webcontrols namespace but cannot be coerced into a webcontrol
      ' a placeholder does not support custom attributes (i.e. repeater custom attribute is not allowed)
      If TypeOf (ctl__prm) Is Repeater Or TypeOf (ctl__prm) Is PlaceHolder Then
				Return False
			End If

			Try
        ' return requested attribute value if WebControl
        If ctl__prm.GetType.IsSubclassOf(GetType(WebControl)) Then
          ' if attribute not found, attempt to add it
          If DirectCast(ctl__prm, WebControl).Attributes(str__prm_attribute_name) Is Nothing = False Then
						Return False
					Else
						DirectCast(ctl__prm, WebControl).Attributes.Item(str__prm_attribute_name) = str__prm_attribute_value
						Return True
					End If
				End If

        ' return requested attribute value if HtmlControl
        If ctl__prm.GetType.IsSubclassOf(GetType(HtmlControl)) Then
					If DirectCast(ctl__prm, HtmlControl).Attributes(str__prm_attribute_name) Is Nothing = False Then
						Return False
					Else
						DirectCast(ctl__prm, HtmlControl).Attributes.Item(str__prm_attribute_name) = str__prm_attribute_value
						Return True
					End If
				End If

        ' return requested attribute value if UserControl
        If ctl__prm.GetType.IsSubclassOf(GetType(UserControl)) Then
					If DirectCast(ctl__prm, UserControl).Attributes(str__prm_attribute_name) Is Nothing = False Then
						Return False
					Else
						DirectCast(ctl__prm, UserControl).Attributes.Item(str__prm_attribute_name) = str__prm_attribute_value
						Return True
					End If
				End If
			Catch
        ' could not be cast to control type even though it is in type namespace (i.e. repeater cannot be cast to webcontrol
        '   repeater is handled expicitly above but serves as good example of cast failure)
        Return False
			End Try

      'If ctl__prm.GetType.IsSubclassOf(GetType(ns_databased.cls_db_control)) Then
      '  Dim prix As Reflection.PropertyInfo() = ctl__prm.GetType.GetProperties()
      '  For Each pri As Reflection.PropertyInfo In prix
      '    If pri.Name.ToLower = str__prm_attribute_name.ToLower Then
      '      pri.SetValue(ctl__prm, str__prm_attribute_value, Nothing)
      '    End If
      '  Next
      'End If

      ' if here, assumed to fail
      Return False

		End Function

    ' format phone numbers in following format: aaa-bbb-cccc xdddd where space 'x' applies only to extensions
    '   if a phone number has less than 10 numeric digits, then nothing will be displayed
    Public Function fnc_format_phone(ByVal obj__prm_phone_number As Object) As String

			Dim str_phone_number As String
			Dim str_phone_number_rest As String

      ' if nothing or empty string, exit
      If obj__prm_phone_number Is Nothing OrElse IsDBNull(obj__prm_phone_number) = True Then
				Return ""
			End If

      ' if overall length < 10, exit
      If obj__prm_phone_number.ToString.Length < 10 Then
				Return ""
			End If

      ' assign local string variable
      str_phone_number = obj__prm_phone_number.ToString()

      ' declare variable to hold first 10 letters or numbers
      Dim strb_phone_number_first_10 As New System.Text.StringBuilder
			strb_phone_number_first_10.Append("")
      ' declare variable to hold remaining characters (after first 10 letters and numbers)
      Dim strb_phone_number_rest As New System.Text.StringBuilder
			strb_phone_number_rest.Append("")

      ' extract any letters or numbers
      For Each chr_phone_number As Char In str_phone_number
        ' if first 10 characters have been found, start populating remaining characters
        If strb_phone_number_first_10.Length = 10 Then
					strb_phone_number_rest.Append(chr_phone_number)
				Else
          ' first 10 not yet populated, if letter or digit, add to first 10
          If Char.IsLetterOrDigit(chr_phone_number) = True Then
						strb_phone_number_first_10.Append(chr_phone_number)
					End If
				End If
			Next

      ' there are not at least 10 letters or numbers
      If strb_phone_number_first_10.Length < 10 Then
				Return ""
			End If

      ' remove leading spaces from "rest" string
      str_phone_number_rest = Trim(strb_phone_number_rest.ToString)

      ' remove leading comma from "rest" string
      str_phone_number_rest = Replace(Left(str_phone_number_rest, 1), ",", "") & Mid(str_phone_number_rest, 2)

      ' remove leading spaces again in case comma was previously first character in "rest" field
      str_phone_number_rest = Trim(str_phone_number_rest)

      ' return formatted phone with or without extension
      If str_phone_number_rest.Length > 0 Then
				Return Left(strb_phone_number_first_10.ToString, 3) & "-" & Mid(strb_phone_number_first_10.ToString, 4, 3) & "-" & Mid(strb_phone_number_first_10.ToString, 7, 4) & ", " & str_phone_number_rest
			Else
				Return Left(strb_phone_number_first_10.ToString, 3) & "-" & Mid(strb_phone_number_first_10.ToString, 4, 3) & "-" & Mid(strb_phone_number_first_10.ToString, 7, 4)
			End If

		End Function

		Public Function fnc_get_content(ByVal str__prm_system_name As String) As String

			Dim ds_content As DataSet = DirectCast(Current.Application("ds_content"), DataSet)
			Dim dr_content() As DataRow
			dr_content = ds_content.Tables(0).Select("system_name = '" + str__prm_system_name + "'")
			If dr_content.Length > 0 Then
				Return Current.Server.UrlDecode(dr_content(0)("content").ToString)
			End If

			Return ""

		End Function

    ' return the value for any type of control (textbox, dropdown, checkbox, etc.)
    Public Shared Function fnc_get_control_value(ByVal ctl__prm As Control, Optional ByVal bln__prm_return_text As Boolean = False) As String

      ' a control without an id is not of interest
      If ctl__prm.ID Is Nothing OrElse ctl__prm.ID.Length = 0 Then
				Return ""
			End If

      ' a repeater is in the webcontrols namespace but cannot be coerced into a webcontrol
      ' a placeholder does not support custom attributes (i.e. repeater custom attribute is not allowed)
      If TypeOf (ctl__prm) Is Repeater Or TypeOf (ctl__prm) Is PlaceHolder Then
				Return ""
			End If

      ' check to see if control has custom attribute holding data_value; if so return that
      Dim str_control_value As String
			str_control_value = fnc_get_control_attribute_value(ctl__prm, "data_value")

			If str_control_value.Length > 0 Then
				Return str_control_value
			End If

      ' no custom attribute or empty, return standard control property
      ' TODO need to handle other types (HTMLControls, other WebControls, etc.)

      Select Case TypeName(ctl__prm)
        'Case "InlineEditBox"
        '  Return DirectCast(ctl__prm, Sys.Web.Ajax.InlineEditBox).Text
        Case "TextBox"
					Return DirectCast(ctl__prm, TextBox).Text
				Case "DropDownList"
					If DirectCast(ctl__prm, DropDownList).SelectedIndex <> -1 Then
						If bln__prm_return_text = True Then
							Return DirectCast(ctl__prm, DropDownList).SelectedItem.Text
						Else
							Return DirectCast(ctl__prm, DropDownList).SelectedValue
						End If
					End If
				Case "Label"
					Return DirectCast(ctl__prm, Label).Text
				Case "CheckBox"
					Dim chk As CheckBox = DirectCast(ctl__prm, CheckBox)
					If fnc_convert_expected_string(chk.Text).Length > 0 Then
						If chk.Checked Then
							Return fnc_convert_expected_string(chk.Text)
						Else
							Return ""
						End If
					Else
						If chk.Checked = True Then
							Return "1"
						Else
							Return "0"
						End If
					End If
				Case "RadioButtonList"
					Return DirectCast(ctl__prm, RadioButtonList).SelectedValue
				Case "RadioButton"
					Return DirectCast(ctl__prm, RadioButton).Text
				Case Else
					Dim x As String = "this line is to set debugging statements"
          ''Return DirectCast(ctl__prm, CheckBox).Checked.ToString
          'Case "ComboBox"
          ' Return directcast(ctl__prm, ProgStudios.WebControls.ComboBox).Value
      End Select

			Return ""

		End Function

    ' apply data format (short date, percentage, etc.) if supplied
    Public Shared Sub sub_format_and_set_control_value(ByVal ctl__prm As Control, ByVal str__prm_value As String)

			Dim str_format As String

      ' a control without an id is not of interest
      If ctl__prm.ID Is Nothing OrElse ctl__prm.ID.Length = 0 Then
				Exit Sub
			End If

      ' if no value to set, exit
      '   REMOVE 1/4/07 because not setting a value when not posting back leaves the old value there (new batch edit)
      'If str__prm_value.Length = 0 Then
      'Exit Sub
      'End If

      str_format = fnc_get_control_attribute_value(ctl__prm, "format")

      ' no format specified, set value as is
      If str_format.Length = 0 Then
				sub_set_control_value(ctl__prm, str__prm_value)
				Exit Sub
			End If

      ' datetime format specified
      ' this might be an issue when not using date picker because a date with format of date and value of '' might get 1/1/001
      If str_format.IndexOf("date") = 0 Or str_format.IndexOf("time") = 0 Then
				sub_set_control_value(ctl__prm, fnc_convert_expected_datetime__string(str__prm_value, str_format))
				Exit Sub
			End If

      ' apply whatever format exists
      sub_set_control_value(ctl__prm, Format(str__prm_value, str_format))

		End Sub
    ' return the value for any type of control (textbox, dropdown, checkbox, etc.)
    Public Shared Sub sub_set_control_value(ByVal ctl__prm As Control, ByVal str__prm_value As String)

      ' a control without an id is not of interest
      If ctl__prm.ID Is Nothing OrElse ctl__prm.ID.Length = 0 Then
				Exit Sub
			End If

      ' if no value to set, exit
      ''If str__prm_value.Length = 0 Then
      ''Exit Sub
      ''End If

      ' if a user has indicated the contol should not be populated, exit
      Dim bln_do_not_populate As Boolean = fnc_convert_expected_boolean(fnc_get_control_attribute_value(ctl__prm, "do_not_populate"))
			If bln_do_not_populate = True Then Exit Sub

      ' TODO check to see if a control has a data_value attribute and if so, set it.

      ' TODO need to handle other types (HTMLControls, other WebControls, etc.)
      Select Case TypeName(ctl__prm)
        'Case "InlineEditBox"
        '  DirectCast(ctl__prm, Sys.Web.Ajax.InlineEditBox).Text = str__prm_value
        Case "TextBox"
					DirectCast(ctl__prm, TextBox).Text = str__prm_value
				Case "DropDownList"
					If DirectCast(ctl__prm, DropDownList).Items.FindByValue(str__prm_value) Is Nothing Then
            ' throw new warning ("Could not find selected item")
          Else
						cls_user_interface.sub_drpdwn_select_by_value(DirectCast(ctl__prm, DropDownList), str__prm_value)
					End If
				Case "Label"
					DirectCast(ctl__prm, Label).Text = str__prm_value
				''Case "CheckBox"
				''Return directcast(ctl__prm, CheckBox).Checked.ToString
				Case "CheckBox"
					Dim chk As CheckBox = DirectCast(ctl__prm, CheckBox)
					If fnc_is_boolean_data(str__prm_value) Or str__prm_value.Length = 0 Then
						chk.Checked = fnc_convert_expected_boolean(str__prm_value)
					Else
						If chk.Text = str__prm_value Then
							chk.Checked = True
						Else
							chk.Checked = False
						End If
					End If
				Case "RadioButtonList"
					If str__prm_value.Length = 0 Then
						DirectCast(ctl__prm, System.Web.UI.WebControls.RadioButtonList).ClearSelection()
					Else
						If DirectCast(ctl__prm, System.Web.UI.WebControls.RadioButtonList).Items.FindByValue(str__prm_value) Is Nothing Then
              ' throw new warning ("Could not find value in list")
            Else
							DirectCast(ctl__prm, System.Web.UI.WebControls.RadioButtonList).SelectedValue = str__prm_value
						End If
					End If
				Case "RadioButton"
					DirectCast(ctl__prm, RadioButton).Text = str__prm_value
				Case "Panel"
					Try
						DirectCast(ctl__prm.Controls(0), LiteralControl).Text = str__prm_value
					Catch
					End Try
			End Select

		End Sub

    ' clear custom attributes from view source (they are still in the control's viewstate)
    Public Shared Sub sub_clear_control_custom_attribute(ByVal ctl__prm As Control)

      ' loop through controls in control container
      For Each ctl As Control In ctl__prm.Controls

        ' if custom attributes were found, get the value from the server control
        If fnc_has_custom_attribute(ctl) = True Then
					If ctl.GetType.IsSubclassOf(GetType(WebControl)) Then
						Dim arl_key As New ArrayList
						For Each str As String In DirectCast(ctl, WebControl).Attributes.Keys
							Select Case str
								Case "add_attribute", "str_watermark_text"
                  'arl_key.Add(str)
                Case "child_add_row_count"
                  'arl_key.Add(str)
                Case "column_name", "table_name", "datatype", "format"
									arl_key.Add(str)
								Case "css_original"
                  'arl_key.Add(str)
                Case "onchange", "onkeydown", "onkeyup"
                  'arl_key.Add(str)
                Case "set_table_name"
                  'arl_key.Add(str)
              End Select
						Next
						For Each str As String In arl_key
							DirectCast(ctl, WebControl).Attributes.Remove(str)
						Next
					End If
          ' trying to clear attributes.  Would rather not use a try catch but works for now
          ' TODO rework without TRY CATCH
          'DirectCast(ctl, WebControl).Attributes.Clear()
        End If

        ' Recurse function
        If ctl.Controls.Count > 0 Then
					sub_clear_control_custom_attribute(ctl)
				End If
			Next

		End Sub

    ' clear custom attributes from view source (they are still in the control's viewstate)
    Public Shared Sub sub_clear_control_databased_attribute(ByVal ctl__prm As Control)

      ' loop through controls in control container
      For Each ctl As Control In ctl__prm.Controls

        ' if custom attributes were found, get the value from the server control
        If fnc_has_databased_attribute(ctl) = True Then
					Try
            ' trying to clear attributes.  Would rather not use a try catch but works for now
            ' TODO rework without TRY CATCH
            DirectCast(ctl, WebControl).Attributes.Clear()
					Catch
					End Try
				End If

        ' Recurse function
        If ctl.Controls.Count > 0 Then
					sub_clear_control_databased_attribute(ctl)
				End If
			Next

		End Sub

    ' disable buttons (insert, save, delete) when not authorized
    Public Shared Sub sub_disable_control__readonly(ByVal ctl__prm_container As Control, ByVal str__prm_command_name_starts_with As String)

			Dim str_command_name As String = ""

      ' loop through controls in control container
      For Each ctl_current As Control In ctl__prm_container.Controls

				str_command_name = fnc_get_control_command_name(ctl_current)

				If str_command_name.StartsWith(str__prm_command_name_starts_with) Then
					DirectCast(ctl_current, WebControl).Enabled = False
				End If

        ' Recurse function
        If ctl_current.Controls.Count > 0 Then
					sub_disable_control__readonly(ctl_current)
				End If
			Next

		End Sub

    ' disable buttons (insert, save, delete) when not authorized
    Public Shared Sub sub_disable_control__readonly(ByVal ctl__prm_container As Control)

			Dim str_command_name As String = ""

      ' loop through controls in control container
      For Each ctl_current As Control In ctl__prm_container.Controls

				str_command_name = fnc_get_control_command_name(ctl_current)

				If str_command_name.StartsWith("delete") Then
					DirectCast(ctl_current, WebControl).Enabled = False
				End If

				If str_command_name.StartsWith("insert") Then
					DirectCast(ctl_current, WebControl).Enabled = False
				End If

        ' updating doesn't commit - save does.  therefore, update can be thought of as bringing up detail page
        'If str_command_name.StartsWith("update") Then
        'DirectCast(ctl_current, WebControl).Enabled = False
        'End If

        If str_command_name.StartsWith("save") Then
					DirectCast(ctl_current, WebControl).Enabled = False
				End If

				If str_command_name.StartsWith("duplicate") Then
					DirectCast(ctl_current, WebControl).Enabled = False
				End If

				If str_command_name.StartsWith("batch") Then
					DirectCast(ctl_current, WebControl).Enabled = False
				End If

				If str_command_name.StartsWith("activate") Then
					DirectCast(ctl_current, WebControl).Enabled = False
				End If

				If str_command_name.StartsWith("rebase") Then
					DirectCast(ctl_current, WebControl).Enabled = False
				End If

				If str_command_name = "track" Or str_command_name = "stop_tracking" Then
					DirectCast(ctl_current, WebControl).Enabled = False
				End If

        ' Recurse function
        If ctl_current.Controls.Count > 0 Then
					sub_disable_control__readonly(ctl_current)
				End If
			Next

		End Sub

    ' visible specific control
    Public Shared Sub sub_visible_control_by_id(ByVal ctl__prm_container As Control, ByVal str__prm_control_id As String, Optional ByVal bln__prm_visible As Boolean = False)

			For Each ctl As Control In ctl__prm_container.Controls

				If fnc_convert_expected_string(ctl.ID).ToLower = str__prm_control_id.ToLower Then
					sub_visible_control__individual(ctl, bln__prm_visible)
				End If

				If ctl.Controls.Count > 0 Then
					sub_visible_control_by_id(ctl, str__prm_control_id, bln__prm_visible)
				End If
			Next

		End Sub

    ' disable specific control
    Public Shared Sub sub_disable_control_by_id(ByVal ctl__prm_container As Control, ByVal str__prm_control_id As String, Optional ByVal bln__prm_disable As Boolean = False)

			For Each ctl As Control In ctl__prm_container.Controls

				If fnc_convert_expected_string(ctl.ID).ToLower = str__prm_control_id.ToLower Then
					sub_disable_control__individual(ctl, bln__prm_disable)
				End If

				If ctl.Controls.Count > 0 Then
					sub_disable_control_by_id(ctl, str__prm_control_id, bln__prm_disable)
				End If
			Next

		End Sub
    ' disable buttons (insert, save, delete) when not authorized
    Public Shared Sub sub_disable_control(ByVal ctl__prm_container As Control, ByVal str__prm_operation As String)

			Dim str_operation As String() = Split(str__prm_operation, ",")

			For Each ctl_current As Control In ctl__prm_container.Controls
				Dim str_command_name As String = fnc_get_control_command_name(ctl_current)
				If str_command_name.Length > 0 Then
					For int_loop As Int32 = 0 To str_operation.Length - 1
						If str_command_name = str_operation(int_loop).Trim Then
							sub_disable_control(ctl_current, False)
						End If
					Next
				End If

        ' recurse
        If ctl_current.Controls.Count > 0 Then
					sub_disable_control(ctl_current, str__prm_operation)
				End If

			Next

		End Sub

    ' change visibility a single control
    Public Shared Sub sub_visible_control__individual(ByVal ctl__prm As Control, ByVal bln__prm_visible As Boolean)

			ctl__prm.Visible = bln__prm_visible

		End Sub

    ' disable (or enable) a single control
    Public Shared Sub sub_disable_control__individual(ByVal ctl__prm As Control, ByVal bln__prm_enable As Boolean)

			If ctl__prm.GetType.IsSubclassOf(GetType(WebControl)) Then
				DirectCast(ctl__prm, WebControl).Enabled = bln__prm_enable
			End If

		End Sub

    ' disable all controls (list results not found, etc.)
    Public Shared Sub sub_disable_control(ByVal ctl__prm_container As Control, Optional ByVal bln__prm_enable As Boolean = False)

			If ctl__prm_container Is Nothing Then Exit Sub

      ' loop through controls in control container
      For Each ctl_current As Control In ctl__prm_container.Controls

				If ctl_current.GetType.IsSubclassOf(GetType(WebControl)) = True Then
          'If TypeOf (ctl_current) Is Repeater Or TypeOf (ctl_current) Is RepeaterItem Or TypeOf (ctl_current) Is PlaceHolder Or TypeOf (ctl_current) Is LiteralControl Then
          'Else
          DirectCast(ctl_current, WebControl).Enabled = bln__prm_enable
          'End If
        End If

        ' Recurse function
        If ctl_current.Controls.Count > 0 Then
					sub_disable_control(ctl_current, bln__prm_enable)
				End If
			Next

		End Sub

    ' disable all linkbuttons (sort row - list results not found, etc.)
    Public Shared Sub sub_disable_linkbutton(ByVal ctl__prm_container As Control)

			If ctl__prm_container Is Nothing Then Exit Sub

      ' loop through controls in control container
      For Each ctl_current As Control In ctl__prm_container.Controls

				If TypeName(ctl_current) = "LinkButton" Then
					DirectCast(ctl_current, LinkButton).CommandName = ""
				End If

        ' Recurse function
        If ctl_current.Controls.Count > 0 Then
					sub_disable_linkbutton(ctl_current)
				End If
			Next

		End Sub

    ' decode a lookup from primary key to name (future to allow them to decode into other fields (abbreviation, etc.)
    Public Shared Function fnc_decode_lkp(ByVal str__prm_column_name As String, ByVal obj__prm_data_value As Object) As String

      ' name is the default field to return when decoding a lkp
      Return fnc_decode_lkp(str__prm_column_name, obj__prm_data_value, "name")

		End Function

    ' decode a lookup from primary key to name (future to allow them to decode into other fields (abbreviation, etc.)
    Public Shared Function fnc_decode_lkp(ByVal str__prm_column_name As String, ByVal obj__prm_data_value As Object, ByVal str__prm_decode_column As String) As String

			Dim str_data_value As String = fnc_convert_expected_string(obj__prm_data_value)

      ' if value sent in is empty, log issue, return empty string
      If str_data_value.Length = 0 Then
        ''ExceptionManager.Publish(New Exception("Could not decode lookup as value sent in was empty"))
        Return ""
			End If

			Dim str_table_name As String
			Dim str_primary_key_name As String

      ' if replace yields the same value then you are not working with a primary key lookup
      If str__prm_column_name = Replace(str__prm_column_name, cls_constant.str_primary_key_prefix__lookup, "") Then
				str_table_name = fnc_table_from_foreign_key(str__prm_column_name)
			Else
				str_table_name = fnc_table_from_primary_key(str__prm_column_name)
			End If

      ' if primary key name sent in, this does nothing
      '		otherwise it will convert foreign key name to primary key name
      str_primary_key_name = Replace(str__prm_column_name, cls_constant.str_foreign_key_prefix__lookup, cls_constant.str_primary_key_prefix__lookup)

			If cls_system_table.fnc_has_column(str_table_name, str__prm_decode_column) = False Then
				Throw New Exception("Error decoding lookup - column name to return invalid (" & str__prm_decode_column & ") for table (" & str_table_name & ")")
			End If

      ' grab lkp table out of cache
      Dim dt_lkp As DataTable = cls_data_access_layer.fnc_get_lkp(str_table_name)

			Try
				Return fnc_convert_expected_string(dt_lkp.Select(str_primary_key_name & "= " & fnc_dbwrap(str_data_value))(0)(str__prm_decode_column))
			Catch ex As Exception
        ' since this is usually decoding by guid - it would rarely fail (unless lookup row was deleted)
        Throw New Exception("Error decoding lookup (may have been deleted): " & str__prm_column_name & " value: " & str_data_value)
			End Try

		End Function

		Public Shared Function fnc_clone_control(obj__prm As Object) As Object

			Dim type As Type = obj__prm.[GetType]()
			Dim properties As PropertyInfo() = type.GetProperties()
			Dim retObject As [Object] = type.InvokeMember("", System.Reflection.BindingFlags.CreateInstance, Nothing, obj__prm, Nothing)
			For Each propertyInfo As PropertyInfo In properties
				If propertyInfo.CanWrite Then
					propertyInfo.SetValue(retObject, propertyInfo.GetValue(obj__prm, Nothing), Nothing)
				End If
			Next

			Return retObject

		End Function

		Public Shared Function fnc_code_lkp(ByVal str__prm_table_name As String, ByVal str__prm_system_name As String) As String

      ' grab lkp table out of cache
      Dim dt_lkp As DataTable = cls_data_access_layer.fnc_get_lkp(str__prm_table_name)

			Try
				Return fnc_convert_expected_string(dt_lkp.Select("system_name = " & fnc_dbwrap(str__prm_system_name))(0)(0))
			Catch ex As Exception
				Throw New Exception("Error coding lookup: " & str__prm_table_name & " system_name: " & str__prm_system_name)
			End Try

		End Function

		Public Shared Function fnc_get_control_with_attribute(ByVal ctl__prm_container As Control, ByVal str__prm_attribute_name As String) As Collection

			Dim cll_result As New Collection

			sub_get_control_with_attribute(ctl__prm_container, str__prm_attribute_name, cll_result)

			Return cll_result

		End Function

		Private Shared Sub sub_get_control_with_attribute(ByVal ctl__prm_container As Control, ByVal str__prm_attribute_name As String, ByRef cll__prm_result As Collection)

			For Each ctl As Control In ctl__prm_container.Controls

				If fnc_verify_control_has_attribute(ctl, str__prm_attribute_name) = True Then
					cll__prm_result.Add(ctl)
				End If

				If ctl.Controls.Count > 0 Then
					sub_get_control_with_attribute(ctl, str__prm_attribute_name, cll__prm_result)
				End If
next_iteration:
			Next

		End Sub

		Public Shared Function fnc_verify_control_has_attribute(ByVal ctl__prm As Control, ByVal str__prm_attribute_name As String) As Boolean

      ' if no control, return false
      If ctl__prm Is Nothing Then
				Return False
			End If

      ' a control without an id is not of interest
      If ctl__prm.ID Is Nothing OrElse ctl__prm.ID.Length = 0 Then
				Return False
			End If

      ' a repeater is in the webcontrols namespace but cannot be coerced into a webcontrol
      ' a placeholder does not support custom attributes (i.e. repeater custom attribute is not allowed)
      If TypeOf (ctl__prm) Is Repeater Or TypeOf (ctl__prm) Is PlaceHolder Then
				Return False
			End If

			Dim icol_keys As ICollection = Nothing

			If ctl__prm.GetType.IsSubclassOf(GetType(WebControl)) = True Then
				Dim ctl As WebControl = DirectCast(ctl__prm, WebControl)
				icol_keys = ctl.Attributes.Keys
			End If

			If ctl__prm.GetType.IsSubclassOf(GetType(HtmlControl)) = True Then
				Dim ctl As HtmlControl = DirectCast(ctl__prm, HtmlControl)
				icol_keys = ctl.Attributes.Keys
			End If

			If ctl__prm.GetType.IsSubclassOf(GetType(UserControl)) Then
				Dim ctl As UserControl = DirectCast(ctl__prm, UserControl)
				icol_keys = ctl.Attributes.Keys
			End If

      'If ctl__prm.GetType.IsSubclassOf(GetType(ns_databased.cls_db_control)) Then
      '  Dim prix As Reflection.PropertyInfo() = ctl__prm.GetType.GetProperties()
      '  For Each pri As Reflection.PropertyInfo In prix
      '    If pri.Name.ToLower = str__prm_attribute_name.ToLower Then
      '      Return True
      '    End If
      '  Next
      'End If

      If icol_keys Is Nothing Then Return False

			For Each Str As String In icol_keys
				If Str.ToLower = str__prm_attribute_name.ToLower Then Return True
			Next

			Return False

		End Function

    ' this routine will clone a dropdownlist.  the from will be a bound ddl
    '   the method will set the same datatext and value fields and bind
    Public Shared Sub sub_drpdwn_clone _
		(ByVal drp__prm_clone_from As DropDownList, ByVal drp__prm_clone_to As DropDownList)

			drp__prm_clone_to.DataTextField = drp__prm_clone_from.DataTextField
			drp__prm_clone_to.DataValueField = drp__prm_clone_from.DataValueField

			For Each li As ListItem In drp__prm_clone_from.Items
				drp__prm_clone_to.Items.Add(li)
			Next

		End Sub

		Public Shared Sub sub_zero_out_null_recurse(ByVal ctl__prm_container As Control)

			For Each ctl As Control In ctl__prm_container.Controls

				If ctl.ID Is Nothing = False AndAlso ctl.ID.Length > 0 Then
					If Trim(fnc_get_control_value(ctl)).Length = 0 Then
						sub_zero_out_null(ctl)
					End If

          ' Recurse function
          If ctl.Controls.Count > 0 Then
						sub_zero_out_null_recurse(ctl)
					End If
				End If

			Next

		End Sub

		Public Shared Sub sub_zero_out_null(ByVal ctl__prm As Control)

			cls_user_interface.sub_set_control_value(ctl__prm, "0")

		End Sub

		Public Shared Sub sub_clear_control_value_recurse(ByVal ctl__prm_container As Control)

			For Each ctl As Control In ctl__prm_container.Controls

				sub_clear_control(ctl)

        ' Recurse function
        If ctl.Controls.Count > 0 Then
					sub_clear_control_value_recurse(ctl)
				End If

			Next

		End Sub

		Public Shared Sub sub_clear_control(ByVal ctl__prm As Control)

			cls_user_interface.sub_set_control_value(ctl__prm, "")

		End Sub

		Public Shared Sub sub_set_use_if_empty(ByVal ctl__prm_container As Control)

			Dim str_use_if_empty As String = ""

			For Each ctl As Control In ctl__prm_container.Controls

				If cls_user_interface.fnc_get_control_value(ctl).Length = 0 Then
					str_use_if_empty = fnc_get_control_attribute_value(ctl, "use_if_empty")
					If str_use_if_empty.Length > 0 Then
						sub_set_control_value(ctl, str_use_if_empty)
					End If
				End If

        ' Recurse function
        If ctl.Controls.Count > 0 Then
					sub_set_use_if_empty(ctl)
				End If

			Next

		End Sub

		Public Shared Sub sub_populate_help_recurse(ByVal ctl__prm_container As Control)

			For Each ctl As Control In ctl__prm_container.Controls

				sub_populate_help(ctl)

        ' Recurse function
        If ctl.Controls.Count > 0 Then
					sub_populate_help_recurse(ctl)
				End If

			Next

		End Sub

		Public Shared Sub sub_populate_help_by_id(ByVal ctl__prm As Control, ByVal str__prm_id As String, ByVal str__prm_key As String)

			Dim ctl As Control = cls_utility.fnc_find_nested_control(ctl__prm, str__prm_id)

			If ctl Is Nothing Then
				Exit Sub
			End If

			Dim dt As DataTable = DirectCast(Current.Application("dt_system_help"), DataTable)
			Dim dr() As DataRow

			dr = dt.Select("system_name = " & fnc_dbwrap(str__prm_key))

			If dr.Length <> 1 Then
				Throw New Exception("Error retrieving help - h_s_n=" & str__prm_key)
			End If

			cls_user_interface.sub_set_control_value(ctl, fnc_convert_expected_string(dr(0)("content")))

		End Sub

		Public Shared Sub sub_populate_help(ByVal ctl__prm As Control)

			Dim str_help_system_name As String = ""

			If fnc_convert_expected_boolean(fnc_get_control_attribute_value(ctl__prm, "bln_help_content")) = True Then
				str_help_system_name = fnc_get_control_attribute_value(ctl__prm, "help_system_name")
				If str_help_system_name.Length = 0 Then
					Exit Sub
				End If

				Dim dt As DataTable = DirectCast(Current.Application("dt_system_help"), DataTable)
				Dim dr() As DataRow

				dr = dt.Select("system_name = " & fnc_dbwrap(str_help_system_name))

				If dr.Length <> 1 Then
					Throw New Exception("Error retrieving help - h_s_n=" & str_help_system_name)
				End If

				cls_user_interface.sub_set_control_value(ctl__prm, fnc_convert_expected_string(dr(0)("content")))
			End If

		End Sub

		Public Shared Sub sub_set_default_property(ByVal ctl__prm_container As Control)

      ' this currently only works with default_visibility
      Dim ctl_web As WebControl = Nothing

			For Each ctl As Control In ctl__prm_container.Controls

				If ctl.GetType.IsSubclassOf(GetType(WebControl)) = True Then
					ctl_web = DirectCast(ctl, WebControl)
				End If

				If fnc_get_control_attribute_value(ctl, "default_visibility").Length > 0 Then
					ctl_web.Visible = fnc_convert_expected_boolean(fnc_get_control_attribute_value(ctl, "default_visibility"))
				End If

        ' Recurse function
        If ctl.Controls.Count > 0 Then
					sub_set_default_property(ctl)
				End If

			Next

		End Sub

		Public Shared Sub sub_pad_list(ByVal plc__prm As PlaceHolder, ByVal str__prm_list_index As String)

			Dim int_page_number As Int32 = fnc_convert_expected_int32(fnc_findcast__textbox(plc__prm.Page, "txt_page_number" & str__prm_list_index).Text)
			Dim int_page_count As Int32 = fnc_convert_expected_int32(fnc_findcast__label(plc__prm.Page, "lbl_page_count" & str__prm_list_index).Text)

      ' pad only if viewing last page
      If int_page_number <> int_page_count Then
				Exit Sub
			End If

			Dim lit_list_pad__base As Literal = DirectCast(fnc_find_nested_control(plc__prm.Page, "lit_list_pad" & str__prm_list_index), Literal)

      ' no padding to be done (padding ui in aspx, if empty padding is 'turned off')
      If lit_list_pad__base.Text.Length = 0 Then
				Exit Sub
			End If

			Dim plc_list_pad__base As PlaceHolder = fnc_findcast__placeholder(plc__prm.Page, "plc_list_pad" & str__prm_list_index)
			plc_list_pad__base.Visible = True

			Dim int_page_size As Int32 = fnc_convert_expected_int32(fnc_findcast__textbox(plc__prm.Page, "txt_page_size" & str__prm_list_index).Text)
			Dim int_record_number_lower_bound As Int32 = fnc_convert_expected_int32(fnc_findcast__label(plc__prm.Page, "lbl_record_number_lower_bound" & str__prm_list_index).Text)
			Dim int_record_number_upper_bound As Int32 = fnc_convert_expected_int32(fnc_findcast__label(plc__prm.Page, "lbl_record_number_upper_bound" & str__prm_list_index).Text)

			lit_list_pad__base.Visible = True
			Dim str_list_pad As String = lit_list_pad__base.Text
			lit_list_pad__base.Text = ""

			For int_loop As Int32 = 1 To (int_page_size - (int_record_number_upper_bound - int_record_number_lower_bound) - 1)
				lit_list_pad__base.Text = lit_list_pad__base.Text & str_list_pad
			Next

		End Sub

    'Public Shared Sub sub_process_paging(ByVal plc__prm_list As PlaceHolder, ByVal str__prm_list_index As String, ByVal int__prm_row_count As Int32)

    '  ' calculate paging metrics
    '  Dim inst_paged_sorted_data As cls_paged_sorted_data
    '  inst_paged_sorted_data = cls_paged_sorted_data.fnc_calculate_paging_metric(int__prm_row_count, fnc_convert_expected_int32(DirectCast(plc__prm_list.FindControl("txt_page_size" & str__prm_list_index), TextBox).Text), fnc_convert_expected_int32(DirectCast(plc__prm_list.FindControl("txt_page_number" & str__prm_list_index), TextBox).Text))

    '  ' assign paging metrics
    '  DirectCast(plc__prm_list.FindControl("lbl_record_number_lower_bound" & str__prm_list_index), Label).Text = inst_paged_sorted_data.int_record_number_lower_bound.ToString
    '  DirectCast(plc__prm_list.FindControl("lbl_record_number_upper_bound" & str__prm_list_index), Label).Text = inst_paged_sorted_data.int_record_number_upper_bound.ToString
    '  DirectCast(plc__prm_list.FindControl("lbl_page_count" & str__prm_list_index), Label).Text = inst_paged_sorted_data.int_page_count.ToString
    '  DirectCast(plc__prm_list.FindControl("lbl_record_count" & str__prm_list_index), Label).Text = inst_paged_sorted_data.int_record_count.ToString

    '  ' reset page size if requested is greater than available for single page result
    '  '  NOTE: This should only be done when padding is not used (child lists)
    '  ''If inst_paged_sorted_data.int_page_count = 1 Then
    '  ''DirectCast(plc__prm_list.FindControl("txt_page_size" & str__prm_list_index), TextBox).Text = inst_paged_sorted_data.int_record_count.ToString
    '  ''End If

    '  ' enable/disable paging links
    '  If DirectCast(plc__prm_list.FindControl("lbl_page_number" & str__prm_list_index), Label).Text = "1" Then
    '    DirectCast(plc__prm_list.FindControl("lbtn_goto_page_first" & str__prm_list_index), LinkButton).Enabled = False
    '    DirectCast(plc__prm_list.FindControl("lbtn_goto_page_previous" & str__prm_list_index), LinkButton).Enabled = False
    '  End If
    '  If DirectCast(plc__prm_list.FindControl("lbl_page_number" & str__prm_list_index), Label).Text = DirectCast(plc__prm_list.FindControl("lbl_page_count" & str__prm_list_index), Label).Text Then
    '    DirectCast(plc__prm_list.FindControl("lbtn_goto_page_next" & str__prm_list_index), LinkButton).Enabled = False
    '    DirectCast(plc__prm_list.FindControl("lbtn_goto_page_last" & str__prm_list_index), LinkButton).Enabled = False
    '  End If

    'End Sub

    Public Shared Sub sub_bind_child_add(ByVal rpt__prm As Repeater, ByVal int__prm_list_index As Int32, Optional ByVal int__prm_row_index__parent As Int32 = 0)

      ' int_row_count used to control how many child rows can be added before new rows stop appearing
      ' try global setting, try to override, finally set to 5 if necesary
      Dim int_row_count As Int32 = 0

			int_row_count = fnc_convert_expected_int32(ConfigurationManager.AppSettings("child_add_row_count"))

      ' now try to see if its been overridden in page
      Dim lbl As Label
      ' this may need to have the list index at the end on a detail screen with multiple lists...
      lbl = fnc_findcast__label(rpt__prm.Parent, "lbl_child_add_row_count")

			Dim int_child_add_row_count As Int32 = 0

			If lbl Is Nothing = False Then
				int_child_add_row_count = fnc_convert_expected_int32(fnc_get_control_attribute_value(lbl, "child_add_row_count"))
				If int_child_add_row_count > 0 Then
					int_row_count = int_child_add_row_count
				End If
			End If

			If int_row_count = 0 Then int_row_count = 5

			rpt__prm.DataSource = cls_data_access_layer.fnc_create_datatable(int_row_count, int__prm_list_index, int__prm_row_index__parent)
			rpt__prm.DataBind()

		End Sub

    ' add table_name for set_table_name on passed control down
    Public Shared Sub sub_set_table_name__down(ByVal ctl__prm As Control)

			For Each ctl As Control In ctl__prm.Controls
				If fnc_get_control_attribute_value(ctl, "column_name").Length > 0 Then
					If fnc_get_control_attribute_value(ctl, "table_name").Length = 0 Then
						If fnc_get_control_attribute_value(ctl, "set_table_name").Length > 0 Then
							fnc_insert_or_update_control_attribute_value(ctl, "table_name", fnc_get_control_attribute_value(ctl, "set_table_name"))
						End If
					End If
				End If

				If ctl.Controls.Count > 0 Then
					sub_set_table_name__down(ctl)
				End If

			Next

		End Sub

		Private Shared Sub sub_set_table_name(ByVal ctl__prm As Control)

			fnc_insert_or_update_control_attribute_value(ctl__prm, "table_name", fnc_get_control_attribute_value(ctl__prm, "set_table_name"))

		End Sub
		Public Shared Sub sub_set_table_name(ByVal ctl__prm As Control, ByVal bln__prm_include_children As Boolean)

      ' not sure how but this does happen.  pl 5/2009.  didn't investigate the how.
      If ctl__prm Is Nothing Then Exit Sub

			If fnc_get_control_attribute_value(ctl__prm, "set_table_name").Length > 0 Then
				sub_set_table_name(ctl__prm)
			End If

			If bln__prm_include_children = True Then
				For Each ctl As Control In ctl__prm.Controls

					If fnc_get_control_attribute_value(ctl, "set_table_name").Length > 0 Then
						sub_set_table_name(ctl)
					End If

					If ctl.Controls.Count > 0 Then
						sub_set_table_name(ctl, bln__prm_include_children)
					End If
				Next
			End If

		End Sub

		Public Shared Sub sub_set_table_name__revert(ByVal ctl__prm As Control)

      ' process standard fields (non db_textboxes, etc.)
      For Each ctl As Control In ctl__prm.Parent.Controls
				If fnc_get_control_attribute_value(ctl, "column_name").Length > 0 Then
					If fnc_get_control_attribute_value(ctl, "set_table_name").Length > 0 Then
						fnc_remove_control_attribute_value(ctl, "table_name")
					End If
				End If
			Next

		End Sub
    ' replace watermark default text where not overwritten by user
    'Public Shared Sub sub_remove_watermark_text(ByVal ctl__prm As Control)

    '  ' this method is apparantly no longer needed.  Textbox no longer seems to receive watermark value when posting.
    '  Exit Sub

    '  Dim wm As AjaxControlToolkit.TextBoxWatermarkExtender = Nothing
    '  Dim txt As TextBox = Nothing

    '  ' loop through controls in control container
    '  For Each ctl_current As Control In ctl__prm.Controls

    '    If TypeName(ctl_current) = "TextBoxWatermarkExtender" Then
    '      wm = DirectCast(ctl_current, AjaxControlToolkit.TextBoxWatermarkExtender)
    '      txt = DirectCast(wm.Parent.FindControl(wm.TargetControlID), TextBox)
    '      txt.Text = Replace(txt.Text, wm.WatermarkText, "")
    '    End If

    '    ' Recurse function
    '    If ctl_current.Controls.Count > 0 Then
    '      sub_remove_watermark_text(ctl_current)
    '    End If
    '  Next

    'End Sub

    'Public Shared Sub sub_set_saved_ui(ByVal inst__prm_sql_collection As cls_sql_collection)

    '  For Each inst_sql As cls_sql In inst__prm_sql_collection
    '    For Each str_column_name As String In inst_sql.inst_sql__delta.fnc_get_column_collection.Keys
    '      If inst_sql.inst_sql__delta.fnc_get_control(str_column_name) Is Nothing = False Then
    '        sub_set_saved_ui(inst_sql.inst_sql__delta.fnc_get_control(str_column_name))
    '      End If
    '    Next
    '  Next

    'End Sub

    'Public Shared Sub sub_set_saved_ui(ByVal ctl__prm As Control)

    '  Dim wctl As WebControl = Nothing

    '  If ctl__prm.GetType.IsSubclassOf(GetType(WebControl)) Then
    '    wctl = DirectCast(ctl__prm, WebControl)
    '    Dim ane_saved As AnimationExtender = DirectCast(ctl__prm.Parent.FindControl("ane_saved__" & wctl.ID), AnimationExtender)
    '    If ane_saved Is Nothing = False Then ane_saved.Enabled = True
    '  End If

    '  'If ctl__prm.GetType.IsSubclassOf(GetType(cls_db_control__date_picker_old)) Then
    '  ' txt_0 = DirectCast(gmd_0.FindControl("ctl00"), TextBox)
    '  ' ctl_0 = gmd_0
    '  ' End If

    '  If wctl Is Nothing = False Then
    '    wctl.CssClass = cls_constant.str_css_saved
    '    If wctl.Parent.ID.Contains("td_") Then
    '      'wctl.Parent.cssclass = cls_constant.str_css_saved
    '    End If
    '  End If

    'End Sub

    Public Shared Sub sub_set_validation_ui(ByVal ctl__prm As Control)

      ' reset validation UI appearnace/text
      'sub_reset_validation_ui(ctl__prm)

      Dim ctl As WebControl
			Dim lit_error_message As Literal
			Dim strb_error_summary__body As New StringBuilder

			Dim str_error_message As String = ""
			Dim int_error_index As Int32 = 0

      'css-oroginal

      For Each vld As BaseValidator In ctl__prm.Page.Validators
				If vld.Enabled = True And vld.IsValid = False Then
					ctl = fnc_find_nested_control(vld.Parent, vld.ControlToValidate)
					If ctl Is Nothing Then ctl = fnc_find_nested_control(vld.Page, fnc_get_control_attribute_value(vld, "str_custom_control_to_validate"))

					Dim htc As HtmlTableCell = fnc_find_parent_control(vld, "htmltablecell")
					If htc Is Nothing = False Then
						fnc_insert_or_update_control_attribute_value(htc, "class", "error--cell")
						htc = fnc_find_nested_control(htc.Parent, htc.ID + "__invalid")
						If htc Is Nothing = False Then
							fnc_insert_or_update_control_attribute_value(htc, "class", "invalid")
						End If
					End If

					lit_error_message = fnc_findcast__literal(ctl.Page, ctl.UniqueID & "__error_message")
					If lit_error_message Is Nothing Then
						lit_error_message = fnc_findcast__literal(ctl.Page, "lit__error_message")
					End If

          ' TODO - what's up with error-summary--body then replacing it with error-summary--body--control??  UI also in code behind
          int_error_index = int_error_index + 1
					str_error_message = "<span class='error-summary--body'>" & int_error_index.ToString & ". &nbsp;" & vld.ErrorMessage & "</span>"
					If lit_error_message Is Nothing = False Then
						lit_error_message.Text = lit_error_message.Text & str_error_message
						lit_error_message.Text = Replace(lit_error_message.Text, "error-summary--body'", "error-summary--body--control'")
            'lit_error_message.Text = "<div class='error-class--ctl--watermark'>" & lit_error_message.Text & "</div>"
          End If
					strb_error_summary__body.Append(str_error_message)
					ctl.CssClass = cls_constant.str_error_class__ctl
				End If
			Next

      ' show hide based on errors were found
      If int_error_index > 0 Then
				Dim lbl_error_summary__title As Label = fnc_findcast__label(ctl__prm.Page, "lbl_error_summary__title")
				If lbl_error_summary__title Is Nothing = False Then
					lbl_error_summary__title.Visible = True
				End If

        'if there is a placeholder for the message area, make that visible too
        Dim plc As PlaceHolder = fnc_findcast__placeholder(ctl__prm.Page, "plc_error")
				If plc Is Nothing = False Then
					plc.Visible = True
				End If

        'if there is a panel for the message area, make that visible too
        Dim pnl As Panel = fnc_findcast__panel(ctl__prm.Page, "pnl_error")
				If pnl Is Nothing = False Then
					pnl.Visible = True
				End If

			End If

      ' find page level error detail block
      Dim lbl_error_summary__body As Label = fnc_findcast__label(ctl__prm.Page, "lbl_error_summary__body")
			If lbl_error_summary__body Is Nothing = False Then
        ' no real point in doing any of this without error detail section
        lbl_error_summary__body.Text = strb_error_summary__body.ToString
			End If

		End Sub

		Public Shared Sub sub_clear_validation_ui(ByVal pg__prm As Page)

			Dim ctl As WebControl

			For Each vld As BaseValidator In pg__prm.Validators
				ctl = fnc_find_nested_control(vld.Parent, vld.ControlToValidate)
				ctl.CssClass = fnc_get_control_attribute_value(ctl, "css_original")
        ' remove after conversion to css
        ''Select Case TypeName(ctl)
        ''  Case "TextBox"
        ''    DirectCast(ctl, TextBox).BackColor = Nothing
        ''    DirectCast(ctl, TextBox).BorderColor = Nothing
        ''    DirectCast(ctl, TextBox).BorderWidth = Nothing
        ''  Case "DropDownList"
        ''    DirectCast(ctl, DropDownList).BackColor = Nothing
        ''    DirectCast(ctl, DropDownList).BorderColor = Nothing
        ''    DirectCast(ctl, DropDownList).BorderWidth = Nothing
        ''End Select
      Next

		End Sub

    'Public Shared Sub sub_save_initialization(ByVal ctl__prm As Control)

    '  ' strip invalid characters from numbers(%, comma, etc.), etc.
    '  sub_format_value_recursive(ctl__prm)
    '  ' if there is a watermark, remove the text
    '  sub_remove_watermark_text(ctl__prm)

    'End Sub

    ' perform validation of databased fields
    Public Shared Sub sub_format_value_recursive(ByVal ctl__prm As Control)

			For Each ctl As Control In ctl__prm.Controls

				If fnc_get_control_attribute_value(ctl, "column_name").Length > 0 Then
					sub_format_value(ctl)
				End If

				If ctl.Controls.Count > 0 Then
					sub_format_value_recursive(ctl)
				End If
			Next

		End Sub

		Private Shared Sub sub_format_value(ByVal ctl__prm As Control)

			Dim str_data_value As String = fnc_get_control_value(ctl__prm)
			Dim str_column_name As String = fnc_get_control_attribute_value(ctl__prm, "column_name")
			Dim str_table_name As String = fnc_get_control_attribute_value(ctl__prm, "table_name")

      ' might have set_table_name - not yet set
      If str_table_name.Length = 0 Then str_table_name = fnc_get_control_attribute_value(ctl__prm, "set_table_name")

      ' not a databased control
      If str_table_name.Length = 0 Or str_column_name.Length = 0 Then Exit Sub

			Dim str_datatype As String = fnc_get_datatype(str_table_name, str_column_name)

      ' strip extra characters out of numbers ($ comma percent, etc)
      ' this was removed because it is more valuable to force them to use numbers and 0 or 1 decimal
      '   and tell them in error message correct format versus removing bad characters.  mask can help too.
      'Select Case str_datatype
      'Case "numeric", "decimal", "float", "money", "smallmoney", "real"
      'txt.Text = fnc_match_regular_expression(str_column_value, cls_constant.str_system_constant("regex_numeric"))
      'Case "int", "smallint", "tinyint", "bigint"
      'txt.Text = fnc_match_regular_expression(str_column_value, cls_constant.str_system_constant("regex_integer"))
      'End Select

      Select Case str_datatype
				Case "numeric", "decimal", "float", "money", "smallmoney", "real", "bigint", "int", "smallint", "tinyint"
					If str_data_value.IndexOf(" ") > 0 Then
						sub_format_and_set_control_value(ctl__prm, Replace(str_data_value, " ", ""))
					End If
			End Select

      ' put date in standard date format
      If str_datatype.IndexOf("date") >= 0 Then
				If cls_validation.fnc_is_date(str_data_value) = False Then
				Else
          ' do not format and set control if invalid date as that will blank it out
          sub_format_and_set_control_value(ctl__prm, str_data_value)
				End If
			End If

		End Sub

		Public Shared Function fnc_is_valid_entry_webcontrol(ByVal wctl__prm As WebControl) As Boolean

			Select Case TypeName(wctl__prm).ToLower
				Case "checkbox", "textbox", "checkboxlist", "radiobuttonlist", "dropdownlist"
					Return True
				Case Else
					Return False
			End Select

		End Function

		'Public Shared Sub sub_show_saved_recurse(ByVal ctl__prm As Control)

		'  For Each ctl As Control In ctl__prm.Controls
		'    If ctl.ID Is Nothing = False Then
		'      If ctl.ID = "ane_saved" Then
		'        sub_show_saved(ctl)
		'      End If
		'    End If
		'    If ctl.Controls.Count > 0 Then
		'      sub_show_saved_recurse(ctl)
		'    End If
		'  Next

		'End Sub

		'Public Shared Sub sub_show_saved(ByVal ane__prm As AnimationExtender)

		'  If ane__prm.Enabled = True Then
		'    ane__prm.Enabled = False
		'    ane__prm.Enabled = True
		'  End If

		'End Sub

		' would like this to indicate the last databased columns that were saved (green square in status grid).  
		'Public Shared Sub sub_set_ui__last_saved()

		'  Dim inst_dynamic_sql As New cls_dynamic_sql

		'  Dim pg_current As Page = HttpContext.Current.Handler

		'  If cls_variable.obj_context_value("ds_page_audit") Is Nothing Then
		'    inst_dynamic_sql.sub_persist_page_audit_header(pg_current)
		'    inst_dynamic_sql.sub_persist_page_audit(pg_current)
		'  End If

		'  Dim ds_page_audit_header As DataSet = DirectCast(cls_variable.obj_context_value("ds_page_audit_header"), DataSet)
		'  Dim ds_page_audit As DataSet = DirectCast(cls_variable.obj_context_value("ds_page_audit"), DataSet)

		'  ' insufficient audit data? - exit
		'  If ds_page_audit_header.Tables.Count = 0 _
		'   OrElse ds_page_audit.Tables.Count = 0 _
		'   OrElse ds_page_audit_header.Tables(0).Rows.Count = 0 _
		'   OrElse ds_page_audit.Tables(0).Rows.Count = 0 Then
		'    Exit Sub
		'  End If

		'  Dim dr_arr As DataRow() = ds_page_audit.Tables(0).Select("pk_log_system_audit_header = " & fnc_dbwrap(ds_page_audit_header.Tables(0).Rows(0)(0)))

		'  inst_dynamic_sql = New cls_dynamic_sql
		'  Dim ds As DataSet = inst_dynamic_sql.fnc_get_databased_control(pg_current, False, True)

		'  For Each dr As DataRow In dr_arr
		'    Dim ctl As Control = cls_dynamic_sql.fnc_get_control(ds, fnc_convert_expected_string(dr("row_primary_key")), fnc_convert_expected_string(dr("column_name")))

		'    If ctl Is Nothing = True Then
		'      'Throw New Exception("Could not get control for column:" & fnc_convert_expected_string(dr("column_name")) & " : Row PK " & fnc_convert_expected_string(dr("row_primary_key")))
		'    End If

		'    Dim td As HtmlTableCell = fnc_find_parent_control(ctl, "htmltablecell")
		'    If td Is Nothing = False Then
		'      Dim td__saved As HtmlTableCell = fnc_find_nested_control(td.Parent, td.ID & "__saved")
		'      If td__saved Is Nothing = False Then
		'        fnc_insert_control_attribute_value(td__saved, "class", "saved")
		'      End If
		'    End If

		'  Next

		'End Sub

		' set class of controls (and TD's) when control was part of the last database save
		'Public Shared Sub sub_highlight(ByVal str__prm_pk_log_system_audit_header As String, Optional ByVal bln__prm_show_current_data As Boolean = False)

		'  Dim inst_dynamic_sql As New cls_dynamic_sql

		'  Dim pg_current As Page = HttpContext.Current.Handler

		'  If cls_variable.obj_context_value("ds_page_audit") Is Nothing Then
		'    inst_dynamic_sql.sub_persist_page_audit_header(pg_current)
		'    inst_dynamic_sql.sub_persist_page_audit(pg_current)
		'  End If

		'  Dim ds_page_audit As DataSet = DirectCast(cls_variable.obj_context_value("ds_page_audit"), DataSet)

		'  Dim dr_arr As DataRow() = ds_page_audit.Tables(0).Select("pk_log_system_audit_header = " & fnc_dbwrap(str__prm_pk_log_system_audit_header))

		'  Dim ds As DataSet
		'  inst_dynamic_sql = New cls_dynamic_sql
		'  ds = inst_dynamic_sql.fnc_get_databased_control(pg_current, False, True)

		'  For Each dr As DataRow In dr_arr
		'    Dim ctl As Control = cls_dynamic_sql.fnc_get_control(ds, fnc_convert_expected_string(dr("row_primary_key")), fnc_convert_expected_string(dr("column_name")))

		'    If ctl Is Nothing = True Then Exit Sub

		'    Dim str_control_value As String = fnc_convert_expected_string(dr("old_value"))
		'    Dim str_action As String = fnc_convert_expected_string(dr("action"))

		'    ' set value to show if inserted or updated from blank.  
		'    ' TODO - these values should probably be used in repeater audit next to controls
		'    If str_action.ToLower = "insert" Then str_control_value = cls_constant.str_audit_old_value_display__insert
		'    If str_action.ToLower = "update" And str_control_value.Length = 0 Then str_control_value = cls_constant.str_audit_old_value_display__update__empty

		'    If bln__prm_show_current_data = False Then
		'      sub_set_control_value(ctl, str_control_value)
		'    End If

		'    Dim ane_saved As New AnimationExtender
		'    'Dim ane_saved__master As AnimationExtender = ane_master__saved  (GET ANE_MASTER__SAVED FROM TOP MASTER)

		'    'If ane_saved__master Is Nothing = False Then
		'    'ane_saved.Animations = Replace(ane_saved__master.Animations, "FFFFFF", "eeeeee")
		'    ane_saved.TargetControlID = ctl.ID
		'    ane_saved.Enabled = True
		'    ctl.Parent.Controls.Add(ane_saved)

		'    Dim td As HtmlTableCell = fnc_find_parent_control(ctl, "htmltablecell")
		'    If td Is Nothing = False Then
		'      DirectCast(td, HtmlTableCell).Attributes("class") = "saved--cell"
		'    End If

		'    If ctl.GetType.IsSubclassOf(GetType(WebControl)) Then
		'      DirectCast(ctl, WebControl).CssClass = "control-saved"
		'    End If

		'  Next

		'  ' try to set disable container to base data placeholder.
		'  'sub_disable_control(fnc_get_container("plc_form"))
		'  sub_visible_control_by_id(fnc_get_container(cls_constant.str_data_container_name), "img_audit")

		'  ' find page data placeholder and attempt to copy data values to labels (audit sets text values)
		'  Dim plc As PlaceHolder = fnc_find_nested_control(pg_current, "plc_data__all")

		'  If plc Is Nothing = False Then
		'    cls_user_interface.sub_ctl_2cpy(plc)
		'  End If

		'End Sub

		'    Public Shared Sub sub_set_show_next_row(ByVal ctl__container As Control)

		'      For Each ctl As Control In ctl__container.Controls
		'        ' only address databased fields
		'        If TypeName(ctl) <> "column" Then GoTo next_control
		'        Dim dbc As ns_databased.column = DirectCast(ctl, ns_databased.column)

		'        ' if it's a databased field and show next row is set
		'        If dbc.bln_show_next_row = True Then

		'          ' find DB Row for column
		'          Dim dbr As ns_databased.row = fnc_find_parent_control(ctl, "row")
		'          If dbr Is Nothing Then Throw New Exception("Could not find row for " & ctl.ID)
		'          If dbr.int_row_number = 0 Then GoTo next_control
		'          Dim int_row_number As Int32 = dbr.int_row_number


		'          ' find DB Table for row
		'          Dim dbt As ns_databased.table = fnc_find_parent_control(dbr, "table")
		'          If dbt Is Nothing Then Throw New Exception("Could not find table for " & dbt.ID)

		'          ' grab Table's list index
		'          Dim str_list_index As String = dbt.str_list_index

		'          ' grab high_row point - high_row is set when show next row fires so we know the highest row a user reached
		'          Dim txt_highest_revealed_row As TextBox = dbt.FindControl("txt_highest_revealed_row")

		'          ' prepare to set javascript on control to show next row when they type in a textbox or pick a dropdown - also set highrow too
		'          Dim str_script As String = "fnc_snr('" & str_list_index & "','" & int_row_number & "','" & txt_highest_revealed_row.ClientID & "')"
		'          str_script = cls_javascript.fnc_wrap_try_catch(str_script)

		'          Dim ctl__target As Control = dbc.Parent.FindControl(dbc.str_target_control_id)
		'          Dim str_on_event As String = "onchange"

		'          fnc_insert_or_update_control_attribute_value(ctl__target, str_on_event, str_script)

		'          If TypeName(ctl__target).ToLower <> "dropdownlist" Then
		'            str_on_event = "onkeydown"
		'          End If

		'          ' abcd this for a dropdown, looks like it will add it twice.  even for regular, call prior method??
		'          fnc_insert_or_update_control_attribute_value(ctl__target, str_on_event, str_script)
		'        End If
		'next_control:

		'        If ctl.Controls.Count > 0 Then
		'          sub_set_show_next_row(ctl)
		'        End If

		'      Next

		'    End Sub

		'' loop through all rows to high row point and set visibility to show
		'Public Shared Function fnc_restore_style_hidden(ByVal ctl__prm As Control) As String

		'    ' find every txt_highest_revealed_row
		'    ' grab list_index

		'    Dim strb_javascript As New StringBuilder
		'    sub_restore_style_hidden__recurse(ctl__prm, strb_javascript)

		'    Return strb_javascript.ToString

		'  End Function

		'Public Shared Sub sub_restore_style_hidden__recurse(ByVal ctl__prm As Control, ByRef str__prm_javascript As StringBuilder)

		'  Dim str_list_index As String = ""

		'  If ctl__prm.ID = Nothing = False Then
		'    If ctl__prm.ID.StartsWith("txt_highest_revealed_row") = True Then
		'      Dim tbl As ns_databased.table = fnc_find_parent_control(ctl__prm, "table")
		'      str_list_index = tbl.str_list_index
		'      For int_loop As Int32 = 1 To fnc_convert_expected_int32(fnc_get_control_value(ctl__prm))
		'        str__prm_javascript.Append("document.getElementById('tr_" & str_list_index & "_" & int_loop & "').style.display = 'block';")
		'      Next
		'    End If
		'  End If

		'  ' loop through controls in control container
		'  For Each ctl_current As Control In ctl__prm.Controls

		'    If ctl_current.ID = Nothing = False Then
		'      If ctl_current.ID.StartsWith("txt_highest_revealed_row") = True Then
		'        Dim tbl As ns_databased.table = fnc_find_parent_control(ctl_current, "table")
		'        str_list_index = tbl.str_list_index
		'        For int_loop As Int32 = 1 To fnc_convert_expected_int32(fnc_get_control_value(ctl_current))
		'          str__prm_javascript.Append("document.getElementById('tr_" & str_list_index & "_" & int_loop & "').style.display = 'block';")
		'        Next
		'      End If
		'    End If

		'    ' Recurse function
		'    If ctl_current.Controls.Count > 0 Then
		'      sub_restore_style_hidden__recurse(ctl_current, str__prm_javascript)
		'    End If
		'  Next

		'End Sub

		' check to see if a control has an attribute with an underscore (those should all be custom)
		Public Shared Function fnc_has_custom_attribute(ByVal ctl__prm As Control) As Boolean

			Dim wctl As WebControl = Nothing
			Dim hctl As HtmlControl = Nothing

			If ctl__prm.GetType.IsSubclassOf(GetType(WebControl)) Then
				wctl = DirectCast(ctl__prm, WebControl)
				For Each str__attribute_name As String In wctl.Attributes.Keys
					If str__attribute_name.Contains("_") Then Return True
				Next
			End If

			If ctl__prm.GetType.IsSubclassOf(GetType(HtmlControl)) Then
				hctl = DirectCast(ctl__prm, HtmlControl)
				For Each str__attribute_name As String In hctl.Attributes.Keys
					If str__attribute_name.Contains("_") Then Return True
				Next
			End If

			Return False

		End Function

    ' check to see if a control has a specific attribute that indicates it's a databased control
    Public Shared Function fnc_has_databased_attribute(ByVal ctl__prm As Control) As Boolean

			Dim wctl As WebControl = Nothing
			Dim hctl As HtmlControl = Nothing

			If ctl__prm.GetType.IsSubclassOf(GetType(WebControl)) Then
				wctl = DirectCast(ctl__prm, WebControl)
				For Each str__attribute_name As String In wctl.Attributes.Keys
					If str__attribute_name.Contains("table_name") Then Return True
					If str__attribute_name.Contains("column_name") Then Return True
				Next
			End If

			Return False

		End Function

    ' rename attribute (insert/delete) if old attribute exists
    'Public Shared Function fnc_rename_attribute(ByRef ctl__prm As Control, ByVal str__prm_attribute_name__old As String, ByVal str__prm_attribute_name__new As String) As Boolean

    '  If ctl__prm Is Nothing = True Then
    '    Return False
    '  End If

    '  ' a control without an id is not of interest
    '  If ctl__prm.ID Is Nothing OrElse ctl__prm.ID.Length = 0 Then
    '    Return False
    '  End If

    '  ' a repeater is in the webcontrols namespace but cannot be coerced into a webcontrol
    '  ' a placeholder does not support custom attributes (i.e. repeater custom attribute is not allowed)
    '  If TypeOf (ctl__prm) Is Repeater Or TypeOf (ctl__prm) Is PlaceHolder Then
    '    Return False
    '  End If

    '  Try
    '    ' return requested attribute value if WebControl
    '    If ctl__prm.GetType.IsSubclassOf(GetType(WebControl)) Then
    '      Dim wctl As WebControl = fnc_cast_if__webcontrol(ctl__prm)

    '      ' cannot rename if old doesn't exist
    '      If wctl.Attributes(str__prm_attribute_name__old) Is Nothing Then
    '        Return False
    '      End If

    '      ' insert or update the new attribute name
    '      fnc_insert_or_update_control_attribute_value(wctl, str__prm_attribute_name__new, fnc_get_control_attribute_value(wctl, str__prm_attribute_name__old))

    '      ' remove the old attribute name
    '      wctl.Attributes.Remove(str__prm_attribute_name__old)

    '      Return True

    '    End If

    '    ' return requested attribute value if HtmlControl
    '    If ctl__prm.GetType.IsSubclassOf(GetType(HtmlControl)) Then
    '      Dim wctl As HtmlControl = fnc_cast_if__htmlcontrol(ctl__prm)

    '      ' cannot rename if old doesn't exist
    '      If wctl.Attributes(str__prm_attribute_name__old) Is Nothing Then
    '        Return False
    '      End If

    '      ' insert or update the new attribute name
    '      fnc_insert_or_update_control_attribute_value(wctl, str__prm_attribute_name__new, fnc_get_control_attribute_value(wctl, str__prm_attribute_name__old))

    '      ' remove the old attribute name
    '      wctl.Attributes.Remove(str__prm_attribute_name__old)

    '      Return True

    '    End If

    '    ' return requested attribute value if UserControl
    '    If ctl__prm.GetType.IsSubclassOf(GetType(UserControl)) Then
    '      Dim wctl As UserControl = fnc_cast_if__usercontrol(ctl__prm)

    '      ' cannot rename if old doesn't exist
    '      If wctl.Attributes(str__prm_attribute_name__old) Is Nothing Then
    '        Return False
    '      End If

    '      ' insert or update the new attribute name
    '      fnc_insert_or_update_control_attribute_value(wctl, str__prm_attribute_name__new, fnc_get_control_attribute_value(wctl, str__prm_attribute_name__old))

    '      ' remove the old attribute name
    '      wctl.Attributes.Remove(str__prm_attribute_name__old)

    '      Return True

    '    End If
    '  Catch
    '    ' could not be cast to control type even though it is in type namespace (i.e. repeater cannot be cast to webcontrol
    '    '   repeater is handled expicitly above but serves as good example of cast failure)
    '    Return False
    '  End Try

    '  ' if here, assumed to fail
    '  Return False

    'End Function

    Public Shared Sub sub_ctl_2cpy(ByVal ctl__prm As Control)
      ' This method is used, for example, to have a label (not bound to DB) and textbox on the same screen with the same values
      '   Developed to support having javascript switch between view and edit mode without postback

      For Each ctl As Control In ctl__prm.Controls

				If fnc_get_control_attribute_value(ctl, "ctl_2cpy").Length > 0 Then
					Dim ctl_source As Control = ctl.Parent.FindControl(fnc_get_control_attribute_value(ctl, "ctl_2cpy"))
					Dim str_value As String = fnc_get_control_value(ctl_source, True)
					sub_set_control_value(ctl, str_value)
				End If

				If ctl.Controls.Count > 0 Then
					sub_ctl_2cpy(ctl)
				End If
			Next

		End Sub

		Public Shared Sub sub_set_ui__edit_mode(ByVal ctl__prm As Control)
      ' this is typically used on viewmode (labels, readonly textboxes) to allow clicking and switching to edit mode

      For Each ctl As Control In ctl__prm.Controls
				If fnc_verify_control_has_attribute(ctl, "ve") = True Then
					Select Case fnc_get_control_attribute_value(ctl, "ve")
						Case "e"
              'fnc_insert_or_update_control_attribute_value(ctl, "onclick", "sm('e');")
            Case "v"
							fnc_insert_or_update_control_attribute_value(ctl, "onclick", "sm('e');")
						Case Else
							Throw New Exception("Invalid ve value")
					End Select
				End If
				If ctl.Controls.Count > 0 Then
					sub_set_ui__edit_mode(ctl)
				End If
			Next

		End Sub

	End Class

End Namespace
