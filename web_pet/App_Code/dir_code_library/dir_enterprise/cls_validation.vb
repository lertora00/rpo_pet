Imports ns_enterprise
Imports ns_enterprise.cls_utility

Namespace ns_enterprise

  Public Class cls_validation

    ' escape single quotes; balance parenthesis
    Public Function fnc_escape_sql(ByVal str__prm_value As String) As String
      Dim str_value As String = str__prm_value
      ' escape single quotes
      str_value = Replace(str__prm_value, "'", "''")
      ' if there are more open parens then closed, add additional closed parens to end of string
      '   only comes into play in where clause - i.e. last_name = 'smith(jr' would bomb stp but is valid sql
      ' as it causes problem in stp_paging__fast - to be addressed later
      If Replace(str_value, "(", "").Length < Replace(str_value, ")", "").Length Then
        For int_parenthesis As Integer = 1 To Replace(str_value, ")", "").Length - Replace(str_value, "(", "").Length
          str_value = str_value & ")"
        Next
      End If

      Return str_value
    End Function

    ' validate URL to ensure it begins with HTTP or HTTPS
    Public Function fnc_validate_url(ByVal str__prm_url As String) As Boolean

      ' if string is empty, passes validation
      If str__prm_url.Length = 0 Then
        Return True
      End If

      ' if string begins with HTTP or HTTPS, passes validation
      If (Left(UCase(str__prm_url), 7) = "HTTP://" Or Left(UCase(str__prm_url), 8) = "HTTPS://") Then
        Return True
      End If

      ' checks failed, return false
      Return False

    End Function

    Public Shared Function fnc_is_date(ByVal obj__prm_value As Object) As Boolean

      ' it's not an invalid date if it's empty
      If fnc_convert_expected_string(obj__prm_value).Length = 0 Then
        Return True
      End If

      If IsDate(obj__prm_value) = True Then
        Return True
      End If

      Return False

    End Function

    Public Shared Function fnc_is_bit(ByVal obj__prm_value As Object) As Boolean

      Dim str_bit As String = fnc_convert_expected_string(obj__prm_value)

      ' it's not an invalid value if it's empty
      If str_bit.Length = 0 Then
        Return True
      End If

      Select Case str_bit.ToLower
        Case "true", "false", "yes", "no", "1", "0"
          Return True
      End Select

      Return False

    End Function

    Public Shared Function fnc_validate_email(ByVal str__prm As String) As Boolean

      Return cls_utility.fnc_validate_regular_expression(str__prm, cls_constant.str_system_constant("regex_email"))

    End Function

    Public Shared Function fnc_validate_password(ByVal str__prm As String) As Boolean

      Return cls_utility.fnc_validate_regular_expression(str__prm, cls_constant.str_system_constant("regex_password"))

    End Function

    Public Shared Function fnc_get_invalid_data(ByVal str__prm_data_value As String, ByVal str__prm_datatype As String, ByVal str__prm_regular_expression As String) As String

      If str__prm_datatype.ToLower.Contains("date") Then
        Return fnc_get_invalid__date(str__prm_data_value)
      End If

      If str__prm_datatype.ToLower = "bit" Then
        ' Return str__prm_data_value
        Return fnc_get_invalid__bit(str__prm_data_value)
      End If

      Select Case str__prm_datatype.ToLower
        Case "numeric", "decimal", "float", "money", "smallmoney", "real"
          Return fnc_get_invalid__numeric(str__prm_data_value)
        Case "smallint", "tinyint", "int", "bigint"
          Return fnc_get_invalid__integer(str__prm_data_value)
      End Select

      Return str__prm_data_value

    End Function

    Public Shared Function fnc_get_invalid__date(ByVal str__prm_data_value As String) As String

      Return str__prm_data_value

    End Function

    Public Shared Function fnc_get_invalid__bit(ByVal str__prm_data_value As String) As String

      Dim int_index As Int32 = 0
      Dim str_before As String = ""
      Dim str_after As String = ""
      Dim str_truncate As String = ""
      Dim str_data_value = str__prm_data_value

      If str_data_value.EndsWith(cls_constant.str_truncate_long_text) Then
        str_truncate = cls_constant.str_truncate_long_text
        str_data_value = str_data_value.Substring(0, str_data_value.Length - cls_constant.str_truncate_long_text.Length)
      End If

      Dim str_return As String = ""

      str_return = fnc_highlight_matched(str_data_value, "1")
      If fnc_convert_expected_string(str_return).Length > 0 Then Return str_return & str_truncate

      str_return = fnc_highlight_matched(str_data_value, "0")
      If fnc_convert_expected_string(str_return).Length > 0 Then Return str_return & str_truncate

      str_return = fnc_highlight_matched(str_data_value, "true")
      If fnc_convert_expected_string(str_return).Length > 0 Then Return str_return & str_truncate

      str_return = fnc_highlight_matched(str_data_value, "false")
      If fnc_convert_expected_string(str_return).Length > 0 Then Return str_return & str_truncate

      Return str__prm_data_value

    End Function

    Public Shared Function fnc_get_invalid__numeric(ByVal str__prm_data_value As String) As String

      Dim str_data_value = fnc_convert_expected_string(str__prm_data_value)
      ' if empty, return empty string (could have been passed in nothing)
      If str_data_value.Length = 0 Then Return str_data_value

      Dim str_truncate As String = ""

      If str_data_value.EndsWith(cls_constant.str_truncate_long_text) Then
        str_truncate = cls_constant.str_truncate_long_text
        str_data_value = str_data_value.Substring(0, str_data_value.Length - cls_constant.str_truncate_long_text.Length)
      End If

      Dim strb_return As New System.Text.StringBuilder

      Dim bln_period As Boolean = False

      For Each chr As Char In str_data_value
        If bln_period = True And chr = "." Then
          strb_return.Append("<span class='error--invalid-data'>" & chr & "</span>")
        Else
          If fnc_validate_regular_expression(chr, cls_constant.str_system_constant("regex_numeric")) = True Then
            strb_return.Append(chr)
          Else
            strb_return.Append("<span class='error--invalid-data'>" & chr & "</span>")
          End If
        End If
        If chr = "." Then bln_period = True
      Next

      Return strb_return.ToString & str_truncate

    End Function

    Public Shared Function fnc_get_invalid__integer(ByVal str__prm_data_value As String) As String

      Dim str_data_value = fnc_convert_expected_string(str__prm_data_value)
      ' if empty, return empty string (could have been passed in nothing)
      If str_data_value.Length = 0 Then Return str_data_value

      Dim str_truncate As String = ""

      If str_data_value.EndsWith(cls_constant.str_truncate_long_text) Then
        str_truncate = cls_constant.str_truncate_long_text
        str_data_value = str_data_value.Substring(0, str_data_value.Length - cls_constant.str_truncate_long_text.Length)
      End If

      Dim strb_return As New System.Text.StringBuilder

      For Each chr As Char In str_data_value
        If fnc_validate_regular_expression(chr, cls_constant.str_system_constant("regex_integer")) = True Then
          strb_return.Append(chr)
        Else
          strb_return.Append("<span class='error--invalid-data'>" & chr & "</span>")
        End If
      Next

      Return strb_return.ToString & str_truncate

    End Function

    Public Shared Function fnc_highlight_matched(ByVal str__prm_data_value As String, ByVal str__prm_match As String) As String

      Dim str_before As String = ""
      Dim str_after As String = ""
      Dim int_index As Int32 = 0

      int_index = str__prm_data_value.ToLower.IndexOf(str__prm_match)

      If int_index >= 0 Then
        str_before = "<span class='error--invalid-data'>" & str__prm_data_value.Substring(0, int_index) & "</span>"
        str_after = "<span class='error--invalid-data'>" & str__prm_data_value.Substring(int_index + Len(str__prm_match)) & "</span>"
        Return str_before & str__prm_data_value.Substring(int_index, Len(str__prm_match)) & str_after
      End If

      Return Nothing

    End Function

		Public Shared Function fnc_validate_phone_number_10_digit_only(str__prm_phone_number As String) As Boolean

			Return cls_utility.fnc_validate_regular_expression(str__prm_phone_number, cls_constant.str_system_constant("regex_phone_number_10_digit_only"))

		End Function

	End Class

End Namespace

