Imports ns_enterprise.cls_utility

Imports System.Web.Mail
Imports System.Configuration
Imports System.Data.SqlClient

Namespace ns_enterprise

  Public Class cls_email

    Public Shared Sub sub_send(ByVal str__prm_subject As String, ByVal str__prm_body As String, ByVal bln__prm_log_email As Boolean)

      sub_send(ConfigurationManager.AppSettings("email_update_from"), ConfigurationManager.AppSettings("email_update_to"), str__prm_subject, str__prm_body, bln__prm_log_email)

    End Sub

    Public Shared Sub sub_send(ByVal str__prm_from As String, ByVal str__prm_to As String, ByVal str__prm_subject As String, ByVal str__prm_body As String, ByVal bln__prm_log_email As Boolean)

      sub_send(str__prm_from, str__prm_to, "", "", str__prm_subject, str__prm_body, cls_constant.str_smtp_server, bln__prm_log_email)

    End Sub

    Public Shared Sub sub_send(ByVal str__prm_from As String, ByVal str__prm_to As String, ByVal str__prm_cc As String, ByVal str__prm_bcc As String, ByVal str__prm_subject As String, ByVal str__prm_body As String, ByVal bln__prm_log_email As Boolean)

      fnc_send(str__prm_from, str__prm_to, str__prm_cc, str__prm_bcc, str__prm_subject, str__prm_body, cls_constant.str_smtp_server, bln__prm_log_email, "", "")

    End Sub

    Public Shared Sub sub_send(ByVal str__prm_from As String, ByVal str__prm_to As String, ByVal str__prm_cc As String, ByVal str__prm_bcc As String, ByVal str__prm_subject As String, ByVal str__prm_body As String, ByVal bln__prm_log_email As Boolean, ByVal str__prm_source_table_name As String, ByVal str__prm_source_row_primary_key As String)

      fnc_send(str__prm_from, str__prm_to, str__prm_cc, str__prm_bcc, str__prm_subject, str__prm_body, cls_constant.str_smtp_server, bln__prm_log_email, str__prm_source_table_name, str__prm_source_row_primary_key)

    End Sub

    Public Shared Sub sub_send(ByVal str__prm_from As String, ByVal str__prm_to As String, ByVal str__prm_cc As String, ByVal str__prm_bcc As String, ByVal str__prm_subject As String, ByVal str__prm_body As String, ByVal str__prm_smtp_server As String, ByVal bln__prm_log_email As Boolean)

      fnc_send(str__prm_from, str__prm_to, str__prm_cc, str__prm_bcc, str__prm_subject, str__prm_body, str__prm_smtp_server, bln__prm_log_email, "", "")

    End Sub

    Public Shared Function fnc_send(ByVal str__prm_from As String, ByVal str__prm_to As String, ByVal str__prm_cc As String, ByVal str__prm_bcc As String, ByVal str__prm_subject As String, ByVal str__prm_body As String, ByVal str__prm_smtp_server As String, ByVal bln__prm_log_email As Boolean, ByVal str__prm_source_table_name As String, ByVal str__prm_source_row_primary_key As String) As Boolean

      Dim str_pk_log_email As String = ""
      Dim bln_send As Boolean = True

      Dim str_to As String = str__prm_to & fnc_email_append(fnc_convert_expected_string(str__prm_to), "to")
      Dim str_cc As String = str__prm_cc & fnc_email_append(fnc_convert_expected_string(str__prm_cc), "cc")
      Dim str_bcc As String = str__prm_bcc & fnc_email_append(fnc_convert_expected_string(str__prm_bcc), "bcc")

      If cls_constant.bln_swap_email = True Then
        Dim strb_swap As New StringBuilder()
        strb_swap.Append("Swapped To: " & str__prm_to & vbCrLf)
        strb_swap.Append("Swapped Cc: " & str__prm_cc & vbCrLf)
        strb_swap.Append("Swapped Bcc: " & str__prm_bcc & vbCrLf)
        str__prm_to = cls_constant.str_email_swap_to
        str__prm_cc = cls_constant.str_email_swap_to
        str__prm_bcc = cls_constant.str_email_swap_to
        str__prm_body = strb_swap.ToString & vbCrLf & str__prm_body
      End If

      If bln__prm_log_email = True Then
        str_pk_log_email = fnc_log_email(str__prm_from, str__prm_to, str__prm_cc, str__prm_bcc, str__prm_subject, str__prm_body, str__prm_smtp_server, str__prm_source_table_name, str__prm_source_row_primary_key)

        If fnc_convert_expected_string(str_pk_log_email).Length = 0 Then
          Throw New Exception("Could not log email to be sent : ")
        End If
      End If

      ' sending the email (versus logging) can be turned off in webconfig
      If fnc_convert_expected_int32(ConfigurationManager.AppSettings("int_enable_immediate_email")) = 1 Then

        Try

          Dim mm_local As New System.Net.Mail.MailMessage(str__prm_from, str__prm_to)
          Dim mm_smtp As New System.Net.Mail.SmtpClient(str__prm_smtp_server)

          If str__prm_cc.Length > 0 Then
            mm_local.CC.Add(str__prm_cc)
          End If
          If str__prm_bcc.Length > 0 Then
            mm_local.Bcc.Add(str__prm_bcc)
          End If
          mm_local.Subject = str__prm_subject
          mm_local.Body = str__prm_body

          mm_smtp.Send(mm_local)

        Catch e As Exception

          ' log exception but don't notify user
          ''ExceptionManager.Publish(e)
          bln_send = False

        End Try

      End If

      If bln_send = True And bln__prm_log_email = True Then
        ' update the email log to stamp when the attempted send took place
        Dim str_sql As String = ""
        str_sql = "update tbl_log_email set smtp_send_date = getdate() where pk_log_email = " & fnc_dbwrap(str_pk_log_email)
        cls_data_access_layer.fnc_execute_non_query(str_sql)
      End If

      Return bln_send

    End Function

		Private Shared Function fnc_email_append(ByVal str__prm_email_address As String, ByVal str__prm_address_line As String) As String

			Dim str_email_append As String

			Select Case str__prm_address_line
				Case "to"
					str_email_append = cls_constant.str_email_append_to
				Case "cc"
					str_email_append = cls_constant.str_email_append_cc
				Case "bcc"
					str_email_append = cls_constant.str_email_append_bcc
				Case Else
					' not a valid address line type (to, cc, bcc), return original email address
					Return str__prm_email_address
			End Select

			' nothing to append - return original email address
			If str_email_append.Length = 0 Then
				Return str__prm_email_address
			End If

			If str__prm_email_address.Length > 0 Then
				' return original with appended list
				Return str__prm_email_address & cls_constant.str_email_delimiter & str_email_append
			Else
				' return only appended list as original is blank
				Return str_email_append
			End If

		End Function

		Public Shared Function fnc_log_email(ByVal str__prm_from As String, ByVal str__prm_to As String, ByVal str__prm_cc As String, ByVal str__prm_bcc As String, ByVal str__prm_subject As String, ByVal str__prm_body As String, ByVal str_smtp_server As String, ByVal str__prm_source_table_name As String, ByVal str__prm_source_row_primary_key As String) As String

			Dim strb_sql As New System.Text.StringBuilder
			Dim str_pk_log_email As String = Guid.NewGuid().ToString

			strb_sql.Append("insert into tbl_log_email ")
			strb_sql.Append("(")
			' insert keys
			strb_sql.Append("pk_log_email, fk_lkp_email_priority, ")
			' base fields
			strb_sql.Append("[from], [to], cc, bcc, subject, body, smtp_server, source_table_name, source_row_primary_key ")
			' accounting field
			strb_sql.Append(cls_data_access_layer.fnc_add_accounting_insert_columm_list)

			strb_sql.Append(") ")
			' insert key values
			strb_sql.Append("select " & fnc_dbwrap(str_pk_log_email) & ", (select pk_lkp_email_priority from tbl_lkp_email_priority where system_name = 'normal'), ")
			' insert base values

			If str__prm_source_row_primary_key.Length = 0 Then
				str__prm_source_row_primary_key = "null"
			Else
				' TODO violation to update a parameter value  :)
				str__prm_source_row_primary_key = fnc_dbwrap(str__prm_source_row_primary_key)
			End If

			strb_sql.Append(fnc_dbwrap(str__prm_from) & ", " & fnc_dbwrap(str__prm_to) & ", " & fnc_dbwrap(str__prm_cc) & ", " & fnc_dbwrap(str__prm_bcc) & ", " & fnc_dbwrap(str__prm_subject) & ", " & fnc_dbwrap(str__prm_body) & ", " & fnc_dbwrap(str_smtp_server) & ", " & fnc_dbwrap(str__prm_source_table_name) & ", " & str__prm_source_row_primary_key & " ")
			' insert accounting values
			strb_sql.Append(cls_data_access_layer.fnc_add_accounting_insert_value_list)

			' excecute sql (errors handled inside)
			cls_data_access_layer.fnc_execute_non_query(strb_sql.ToString)

			' if here, insert succeeded - return primary key inserted
			Return str_pk_log_email

		End Function

    Public Shared Function fnc_get_email_address_by_person_user(ByVal str__prm_pk_person_user As String) As String

      Dim str_sql As String = ""
      Dim str_email_address As String = ""

      If cls_utility.fnc_is_valid_guid(str__prm_pk_person_user) = False Then
        ' TODO should log error if it is 1. not valid guid; 2. not null; 3. not nothing; 4. not empty string
        Return ""
      End If

      str_sql = "select top 1 email_address from tbl_person_email, tbl_person_user where tbl_person_email.fk_person = tbl_person_user.fk_person and pk_person_user = " & fnc_dbwrap(str__prm_pk_person_user)

      str_email_address = fnc_convert_expected_string(cls_data_access_layer.fnc_get_scaler__string(str_sql))

      Return str_email_address

    End Function

    Public Shared Function fnc_get_email_append(ByVal str__prm_type As String) As String

      Dim dr As SqlDataReader
      Dim str_email_address As New System.Text.StringBuilder
      Dim str_delimiter As String = ""

      Select Case str__prm_type
        Case "to", "cc", "bcc"
        Case Else
          Throw New Exception("invalid email type")
      End Select

			dr = cls_data_access_layer.fnc_get_datareader("select email_address from tbl_system_email_append_" & str__prm_type)

      Do While dr.Read
        str_email_address.Append(str_delimiter & dr("email_address").ToString)
        str_delimiter = "; "
      Loop

      Return str_email_address.ToString

    End Function

  End Class

End Namespace
