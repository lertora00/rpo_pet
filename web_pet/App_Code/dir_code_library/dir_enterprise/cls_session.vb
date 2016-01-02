Imports ns_enterprise.cls_utility

Imports System.Data
Imports System.Web.HttpContext

Namespace ns_enterprise

  Public Class cls_session

    Public Shared ReadOnly Property str_aspnet_sessionid() As String
      Get

        If Current Is Nothing = True Then
          Return ""
        End If

        If Current.Session Is Nothing = True Then
          Return ""
        End If

        Return fnc_convert_expected_string(Current.Session.SessionID)

      End Get
    End Property

    Public Shared ReadOnly Property str_pk_log_session() As String
      Get

        If Current Is Nothing = True Then
          Return ""
        End If

        If Current.Session Is Nothing = True Then
          Return ""
        End If

        Return fnc_convert_expected_string(Current.Session("str_pk_log_session"))

      End Get
    End Property

    Public Shared ReadOnly Property int_active_user_count() As Int32
      Get

        'If fnc_convert_expected_string(cls_variable.str_active_user_list).Length = 0 Then
        '  Return 0
        'End If

        'Return cls_variable.str_active_user_list.Split(";").Length - 1
      End Get
    End Property

    Public Shared Function fnc_is_new_session() As Boolean

      Dim bln_new_session As Boolean = Current.Session.IsNewSession

      If bln_new_session = True Then
        If fnc_is_session_from_timeout() = True And Now > fnc_convert_expected_datetime("12/29/2029") Then
          cls_login.fnc_logout(True)
        End If
      End If

      If cls_current_user.str_pk_person_user.Length > 0 Then
        If cls_session.fnc_to_be_killed = True Then
          cls_login.fnc_logout(False, True)
          Return False
        End If
        'If cls_variable.str_active_user_list.Contains(cls_current_user.str_pk_person_user) = False Then
        '  cls_session.sub_add_new_user()
        'End If
      End If

      Return bln_new_session

    End Function

    Public Shared Function fnc_is_session_from_timeout() As Boolean

      If Current.Session Is Nothing = False Then
        If Current.Session.IsNewSession = True Then
          Dim str_cookie_header As String = Current.Request.Headers("Cookie")
          If fnc_convert_expected_string(str_cookie_header).Length > 0 AndAlso str_cookie_header.IndexOf("ASP.NET_SessionId") >= 0 Then
            If Current.Request.IsAuthenticated = True Then
              Return True
            End If
          End If
        End If
      End If

      Return False

    End Function

    Public Shared Function fnc_select_list(ByVal dt__prm As DataTable) As DataTable

      Return dt__prm

    End Function

    Public Shared Function fnc_select_list(ByVal dt__prm As DataTable, ByVal str__prm_filter As String) As DataTable

      If str__prm_filter.Length = 0 Then
        Return fnc_select_list(dt__prm)
        Exit Function
      End If

      Dim dr_select() As DataRow = dt__prm.Select(str__prm_filter)

      ' filter returned no rows
      If dr_select.Length = 0 Then
        Return Nothing
      End If

      Dim dt__return As DataTable = dt__prm.Clone

      ''delete this code if above works
      For Each dr_loop As DataRow In dr_select
        dt__return.ImportRow(dr_loop)
      Next

      Return dt__return

    End Function

    Public Shared Sub sub_mark_for_kill(ByVal str__prm_username As String)

      'cls_variable.str_to_be_killed = cls_variable.str_to_be_killed & cls_person_user.fnc_get_pk_person_user(str__prm_username) & ";"

    End Sub

		'Public Shared Function fnc_release_kill() As Boolean

		'If cls_current_user.str_pk_person_user.Length = 0 Then
		'      Return False
		'    End If

		'    Dim str_to_be_killed As String = cls_variable.str_to_be_killed
		'    If str_to_be_killed.Contains(cls_current_user.str_pk_person_user) = True Then
		'      cls_variable.str_to_be_killed = Replace(cls_variable.str_to_be_killed, cls_current_user.str_pk_person_user & ";", "")
		'    End If

		'    Return False

		'  End Function

		'Public Shared Sub sub_kill()

		'    fnc_kill()

		'  End Sub

		'Public Shared Function fnc_kill() As Boolean

		'  ' user must be logged in with active session to perform kill
		'  If cls_current_user.str_pk_person_user.Length > 0 AndAlso cls_variable.str_active_user_list.Contains(cls_current_user.str_pk_person_user) = True Then
		'    fnc_release_kill()
		'    sub_logout()
		'    Return True
		'  End If

		'  Return False

		'End Function

		'Public Shared Sub sub_logout()

		'    ' remove user
		'    sub_remove_current_user()

		'  End Sub

		'  Public Shared Sub sub_server_session_end()

		'    sub_remove_current_user()

		'  End Sub

		'Public Shared Sub sub_remove_user(ByVal str__prm_active_user As String)

		'  If Current Is Nothing = False AndAlso cls_variable.str_active_user_list.Length > 0 Then
		'    cls_variable.str_active_user_list = Replace(cls_variable.str_active_user_list, str__prm_active_user, "")
		'  End If

		'End Sub

		'Public Shared Sub sub_remove_current_user()

		'  cls_variable.sub_reset_active_user_list()

		'End Sub

		'Public Shared Sub sub_add_current_user()

		'    Dim str_active_user_list As String = cls_variable.str_active_user_list

		'    ' quick check to make sure user is not already out there.
		'    If str_active_user_list.Contains(fnc_assemble_current_user()) = True Then Exit Sub

		'    cls_variable.str_active_user_list = cls_variable.str_active_user_list & fnc_assemble_current_user()

		'  End Sub

		Public Shared Sub sub_add_new_user()

      'sub_add_current_user()

    End Sub

    Public Shared Function fnc_assemble_current_user(ByVal str__prm_pk_person_user As String, ByVal str__prm_username As String) As String

      Return fnc_convert_expected_string(str__prm_pk_person_user) & ":" & fnc_convert_expected_string(str__prm_username) & ";"

    End Function

    Public Shared Function fnc_assemble_current_user() As String

      Return fnc_assemble_current_user(cls_current_user.str_pk_person_user, cls_current_user.str_username)

    End Function

    Public Shared Function fnc_to_be_killed() As Boolean

      If cls_current_user.str_pk_person_user.Length = 0 Then
        Return False
      End If

      If fnc_to_be_killed(cls_current_user.str_pk_person_user) = True Then
        Return True
      End If

      Return False

    End Function

    Public Shared Function fnc_to_be_killed(ByVal str__prm_pk_person_user As String) As Boolean

      Dim str_to_be_killed As String = fnc_convert_expected_string(cls_global.str__pub_to_be_killed)

      If str_to_be_killed.Contains(cls_current_user.str_pk_person_user) = True Then
        Return True
      End If

      Return False

    End Function

    Public Shared Function fnc_get_recent_session_list(ByVal str__prm_username As String) As DataTable

      If fnc_convert_expected_string(str__prm_username).Length = 0 Then Return Nothing

      Dim str_sql As String = "select top 15 pk_log_session, convert(nvarchar(20), insert_date, 101)+ ' - ' + cast(count_event as varchar(15)) + ' events.' as session_information from udf_log_session() where username = " & fnc_dbwrap(str__prm_username) & " order by insert_date desc"

      Return cls_data_access_layer.fnc_get_datatable(str_sql)

    End Function

    Public Shared Sub sub_abandon()

      If Current Is Nothing Then Exit Sub
      If Current.Session Is Nothing Then Exit Sub

      Current.Session.Abandon()

    End Sub

  End Class

End Namespace