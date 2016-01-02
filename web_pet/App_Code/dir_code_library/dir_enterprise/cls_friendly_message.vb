Imports ns_enterprise.cls_utility

Imports System.Xml
Imports System.IO
Imports System.Diagnostics
Imports System.Reflection
Imports System.Data

Namespace ns_enterprise

  Public Class cls_friendly_message

    'property pk_person_user
    Private str__prv_pk_person_user As String = ""
    Public Property str_pk_person_user() As String
      Get
        Return str__prv_pk_person_user
      End Get
      Set(ByVal value As String)
        str__prv_pk_person_user = value
      End Set
    End Property

    'public enum for message category
    Public Enum en_message_category
      system_message = 1
      validation_condition = 2
      exception = 3
    End Enum

    'property str_message_category
    Private int__prv_message_category As en_message_category = en_message_category.system_message
    Public Property str_message_category() As en_message_category
      Get
        Return int__prv_message_category
      End Get
      Set(ByVal value As en_message_category)
        int__prv_message_category = value
      End Set
    End Property

    ' public enum for message severity
    Public Enum en_message_severity
      critical = 1
      high = 2
      medium = 3
      low = 4
    End Enum

    'property int_message_severity
    Private int__message_severity As en_message_severity = en_message_severity.low
    Public Property int_message_severity() As en_message_severity
      Get
        Return int__message_severity
      End Get
      Set(ByVal value As en_message_severity)
        int__message_severity = value
      End Set
    End Property

    'property message_code_value
    Private str__prv_message_code_value As String = ""
    Public Property str_message_code_value() As String

      Get
        Return str__prv_message_code_value
      End Get
      Set(ByVal Value As String)
        str__prv_message_code_value = Value
      End Set
    End Property

    'property message_context
    Private str__prv_message_context As String = ""
    Public Property str_message_context() As String

      Get
        Return str__prv_message_context
      End Get
      Set(ByVal Value As String)
        str__prv_message_context = Value
      End Set

    End Property

    Private str__prv_error_message As String = ""
    Public Property str_error_message() As String
      Get
        Return str__prv_error_message
      End Get
      Set(ByVal value As String)
        str__prv_error_message = value
      End Set
    End Property

    'property bln_log_to_database
    Private bln__prv_log_to_database As Boolean = True
    Public Property bln_log_to_database() As Boolean

      Get
        Return bln__prv_log_to_database
      End Get
      Set(ByVal Value As Boolean)
        bln__prv_log_to_database = Value
      End Set

    End Property

    ' bln_log_to_file property
    Private bln__prv_log_to_file As Boolean = True
    Public Property bln_log_to_file() As Boolean

      Get
        Return bln__prv_log_to_file
      End Get
      Set(ByVal Value As Boolean)
        bln__prv_log_to_file = Value
      End Set

    End Property

    ' bln_send_email property
    Private bln__prv_send_email As Boolean = True
    Public Property bln_send_email() As Boolean

      Get
        Return bln__prv_send_email
      End Get
      Set(ByVal Value As Boolean)
        bln__prv_send_email = Value
      End Set

    End Property

    'property bln_render_to_ui
    Private bln__prv_render_to_ui As Boolean = True
    Public Property bln_render_to_ui() As Boolean

      Get
        Return bln__prv_render_to_ui
      End Get
      Set(ByVal Value As Boolean)
        bln__prv_render_to_ui = Value
      End Set

    End Property

    'property exc_exception
    Private exc__prv_exception As Exception = Nothing
    Public Property exc_exception() As Exception

      Get
        Return exc__prv_exception
      End Get
      Set(ByVal Value As Exception)
        exc__prv_exception = Value
      End Set

    End Property

    'property application_name 
    Private str__prv_application_name As String = ""
    Public Property str_application_name() As String

      Get
        Return str__prv_application_name
      End Get
      Set(ByVal Value As String)
        str__prv_application_name = Value
      End Set

    End Property

    'property class_name 
    Private str__prv_class_name As String = ""
    Public Property str_class_name() As String

      Get
        Return str__prv_class_name
      End Get
      Set(ByVal Value As String)
        str__prv_class_name = Value
      End Set

    End Property

    'property method_name 
    Private str__prv_method_name As String = ""
    Public Property str_method_name() As String

      Get
        Return str__prv_method_name
      End Get
      Set(ByVal Value As String)
        str__prv_method_name = Value
      End Set

    End Property

    'property control 
    Private ctl__prv_control As Control = Nothing
    Public Property ctl_control() As Control

      Get
        Return ctl__prv_control
      End Get
      Set(ByVal Value As Control)
        ctl__prv_control = Value
      End Set

    End Property

    'property friendly_message 
    Private str__prv_friendly_message As String = ""
    Public Property str_friendly_message() As String

      Get
        Return str__prv_friendly_message
      End Get
      Set(ByVal Value As String)
        str__prv_friendly_message = Value
      End Set

    End Property

    'property friendly_suggestion 
    Private str__prv_friendly_suggestion As String = ""
    Public Property str_friendly_suggestion() As String

      Get
        Return str__prv_friendly_suggestion
      End Get
      Set(ByVal Value As String)
        str__prv_friendly_suggestion = Value
      End Set

    End Property

    'property css_class 
    Private str__prv_css_class As String = ""
    Public Property str_css_class() As String

      Get
        Return str__prv_css_class
      End Get
      Set(ByVal Value As String)
        str__prv_css_class = Value
      End Set

    End Property

    'property control_id 
    Private str__prv_control_id As String = ""
    Public Property str_control_id() As String

      Get
        Return str__prv_control_id
      End Get
      Set(ByVal Value As String)
        str__prv_control_id = Value
      End Set

    End Property

    'property pk_lkp_friendly_message 
    Private str__prv_pk_lkp_friendly_message As String = ""
    Public ReadOnly Property str_pk_lkp_friendly_message() As String

      Get
        Return str__prv_pk_lkp_friendly_message
      End Get

    End Property

    'property id_lkp_friendly_message 
    Private str__prv_id_lkp_friendly_message As String = ""
    Public ReadOnly Property str_id_lkp_friendly_message() As String

      Get
        Return str__prv_id_lkp_friendly_message
      End Get

    End Property

    Public Sub New(ByVal bln_for_administration_only As Boolean)

    End Sub

    Public Sub New(ByVal str__prm_message_code_value As String, ByVal en__prm_message_category As en_message_category, ByVal obj__prm_me As Object, Optional ByVal str__prm_message_context As String = "")

      Me.str__prv_message_code_value = str__prm_message_code_value
      Me.int__prv_message_category = en__prm_message_category
      Me.str__prv_class_name = obj__prm_me.ToString
      Me.str__prv_message_context = str__prm_message_context

      sub_new()

    End Sub

    Public Sub New(ByVal str__prm_message_code_value As String, ByVal en__prm_message_category As en_message_category, Optional ByVal str__prm_message_context As String = "")

      Me.str__prv_message_code_value = str__prm_message_code_value
      Me.int__prv_message_category = en__prm_message_category
      Me.str__prv_message_context = str__prm_message_context

      sub_new()

    End Sub

    Public Sub New(ByVal str__prm_message_code_value As String, ByVal en__prm_message_category As en_message_category, ByVal obj__prm_me As Object, ByVal ctl__prm As Control, Optional ByVal str__prm_message_context As String = "")

      Me.str__prv_message_code_value = str__prm_message_code_value
      Me.int__prv_message_category = en__prm_message_category
      Me.str__prv_class_name = obj__prm_me.ToString
      Me.ctl__prv_control = ctl__prm
      Me.str__prv_message_context = str__prm_message_context

      sub_new()

    End Sub

    Public Sub New(ByVal str__prm_message_code_value As String, ByVal en__prm_message_category As en_message_category, ByVal exc__prm_exception As Exception)

      Me.str__prv_message_code_value = str__prm_message_code_value
      Me.int__prv_message_category = en__prm_message_category
      Me.exc__prv_exception = exc__prm_exception

      sub_new()

    End Sub

    Public Sub New(ByVal str__prm_message_code_value As String, ByVal en__prm_message_category As en_message_category, ByVal exc__prm_exception As Exception, ByVal obj__prm_me As Object, Optional ByVal str__prm_message_context As String = "")

      Me.str__prv_message_code_value = str__prm_message_code_value
      Me.int__prv_message_category = en__prm_message_category
      Me.exc__prv_exception = exc__prm_exception
      Me.str__prv_class_name = obj__prm_me.ToString
      Me.str__prv_message_context = str__prm_message_context

      sub_new()

    End Sub

    Private Sub sub_new()

			Me.str__prv_application_name = fnc_convert_expected_string(System.Web.HttpContext.Current.Application)

      sub_get_friendly_data()

      Dim stk As New StackTrace
      Dim stkf As StackFrame = stk.GetFrame(2)
      ''stkf.GetFileLineNumber()
      Dim mthb As MethodBase = stkf.GetMethod

      Me.str__prv_method_name = mthb.Name

      '' set the pk_person_user if it is available.
      Me.str__prv_pk_person_user = cls_current_user.str_pk_person_user

    End Sub

    Public Sub sub_get_friendly_data()

			Dim dt As DataTable = cls_data_access_layer.fnc_get_lkp("tbl_lkp_friendly_message")

      Dim dr() As DataRow

      Dim str_message_context As String = ""
      If Me.str__prv_message_context.Length > 0 Then
        str_message_context = " and message_context = " & fnc_dbwrap(Me.str__prv_message_context)
      End If
      dr = dt.Select("code_value=" & fnc_dbwrap(Me.str__prv_message_code_value) & str_message_context)

      If dr.Length = 0 Then
				'dr = dt.Select("code_value='MSG_000'")
				'If dr.Length = 0 Then Throw New Exception("Cannot retreive request or default message" & Me.str__prv_message_code_value)
				Exit Sub
      End If

      Me.str__prv_friendly_message = fnc_convert_expected_string(dr(0)("friendly_message"))
      Me.str__prv_friendly_suggestion = fnc_convert_expected_string(dr(0)("friendly_suggestion"))
      Me.str__prv_css_class = fnc_convert_expected_string(dr(0)("css_class"))
      Me.str__prv_control_id = fnc_convert_expected_string(dr(0)("control_id"))

    End Sub

    Protected Friend Function fnc_get_formatted_message() As String

      Dim strb_message_object_string As New StringBuilder()

      Dim str_tab_filler As String = vbTab & ":" & vbTab

      strb_message_object_string.Append("fk_person_user" & vbTab & str_tab_filler & str_pk_person_user & vbCrLf)
      strb_message_object_string.Append("application_name" & str_tab_filler & str__prv_application_name & vbCrLf)
      strb_message_object_string.Append("class_name" & vbTab & str_tab_filler & str__prv_class_name & vbCrLf)
      strb_message_object_string.Append("method_name" & vbTab & str_tab_filler & str__prv_method_name & vbCrLf)
      strb_message_object_string.Append("message_code" & vbTab & str_tab_filler & str__prv_message_code_value & vbCrLf)
      strb_message_object_string.Append("message_context" & vbTab & str_tab_filler & str__prv_message_context & vbCrLf)
      strb_message_object_string.Append("friendly_message" & str_tab_filler & str__prv_friendly_message & vbCrLf)
      strb_message_object_string.Append("friendly_suggestion" & str_tab_filler & str__prv_friendly_suggestion & vbCrLf)
      strb_message_object_string.Append("message_category" & str_tab_filler & int__prv_message_category.ToString() & vbCrLf)
			strb_message_object_string.Append("aspnet_sessionid" & IIf(cls_session.str_aspnet_sessionid.Length > 0, _
						 str_tab_filler & cls_session.str_aspnet_sessionid & vbCrLf _
						 , "No Session"))
			strb_message_object_string.Append("error_message" & vbTab & str_tab_filler & str__prv_error_message & vbCrLf)

      Return strb_message_object_string.ToString()

    End Function

    Protected Friend Sub sub_log_to_file()

      Dim str_current_file_name As String
      Dim fs As FileStream
			Dim sw As StreamWriter = Nothing
			Dim strb_message As StringBuilder = New StringBuilder()

      Dim str_root_path As String = System.AppDomain.CurrentDomain.BaseDirectory

      Try

        str_current_file_name = str_root_path & "dir_storage\dir_log\dir_message\log_message_" & Format(Today, "yyyy_MM_dd") & ".txt"
        fs = New FileStream(str_current_file_name, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite)
        sw = New StreamWriter(fs)

        ' Set the file pointer to the end of the file
        sw.BaseStream.Seek(0, SeekOrigin.Begin)

        ' Create the message
        strb_message.Append("--------------------------------------------------")
        strb_message.Append(System.DateTime.Now.ToString())
        strb_message.Append("--------------------------------------------------")
        strb_message.AppendLine()
        strb_message.Append(fnc_get_formatted_message())
        strb_message.AppendLine()

        ' Force the write to the underlying file
        sw.WriteLine(strb_message.ToString())
        sw.Flush()
        sw.Close()

      Catch ex As Exception
        Throw ex
      Finally

        If Not sw Is Nothing Then
          sw.Close()
        End If
      End Try


    End Sub

    Protected Friend Sub sub_log_to_database()

      Dim str_commant_text As String

      ' build command text
      str_commant_text = "exec dbo.stp_log_message "
      str_commant_text = str_commant_text & fnc_dbwrap(str__prv_pk_person_user) & ","
      str_commant_text = str_commant_text & fnc_dbwrap(str__prv_application_name) & ","
      str_commant_text = str_commant_text & fnc_dbwrap(int__prv_message_category.ToString()) & ","
			If cls_session.str_aspnet_sessionid.Length > 0 Then
				str_commant_text = str_commant_text & fnc_dbwrap(cls_session.str_aspnet_sessionid) & ","
			Else
				str_commant_text = str_commant_text & fnc_dbwrap("no session") & ","
			End If
      str_commant_text = str_commant_text & fnc_dbwrap(str__prv_message_context) & ","
      str_commant_text = str_commant_text & fnc_dbwrap(str__prv_class_name) & ","
      str_commant_text = str_commant_text & fnc_dbwrap(str__prv_method_name) & ","
      str_commant_text = str_commant_text & fnc_dbwrap(str__prv_message_code_value) & ","
      str_commant_text = str_commant_text & fnc_dbwrap(str__prv_error_message) & ","
      If ctl__prv_control Is Nothing = False Then
        str_commant_text = str_commant_text & fnc_dbwrap(ctl__prv_control.ToString()) & ","
      Else
        str_commant_text = str_commant_text & fnc_dbwrap("''") & ","
      End If
      str_commant_text = str_commant_text & fnc_dbwrap(str__prv_friendly_message) & ","
      str_commant_text = str_commant_text & fnc_dbwrap(str__prv_friendly_suggestion) & ","
      'str_commant_text = str_commant_text & fnc_dbwrap(int__message_severity)
      str_commant_text = str_commant_text & fnc_dbwrap(0)

      ' execute stored procedure
      cls_data_access_layer.fnc_execute_non_query(str_commant_text)

    End Sub

    Public Sub sub_send_error_email()

      ' send email
      Dim inst_email As New cls_email
      Dim str_prv_email_address As String = cls_constant.str_error_email_to_list
      Dim strb_email_body As New System.Text.StringBuilder

      strb_email_body.Append(Me.fnc_get_formatted_message)

      Try
        cls_email.sub_send("error@[site].com", str_prv_email_address, "", "", "[site] Exception Occured", strb_email_body.ToString, cls_constant.str_smtp_server, False)
      Catch ex As Exception
        Throw ex
      End Try

    End Sub

    Public Shared Function fnc_get_friendly_message(ByVal str__prm_message_code_value As String, ByVal str__prm_default_message_if_not_found As String, ByVal bln_for_overload_only As Boolean, Optional ByVal str__prm_message_context As String = "") As String

			Dim dt As DataTable = cls_data_access_layer.fnc_get_lkp("tbl_lkp_friendly_message")

      Dim dr() As DataRow
      Dim str_return As String = ""

      Dim str_message_context As String = ""
      If str__prm_message_context.Length > 0 Then
        str_message_context = " and message_context = " & fnc_dbwrap(str__prm_message_context)
      End If
      dr = dt.Select("code_value=" & fnc_dbwrap(str__prm_message_code_value) & str_message_context)

      ' check requested error message
      If dr.Length = 0 Then

        If str__prm_default_message_if_not_found.Length = 0 Then
          dr = dt.Select("code_value='MSG_000'")
        Else
          Return str__prm_default_message_if_not_found
        End If

      End If

      ' could not find requested and no default message passed, return msg_000
      If dr.Length = 0 Then
        Throw New Exception("Could not return requested message, no default message provided and msg_000 not found for cd: " & str__prm_message_code_value)
      End If

      Return fnc_convert_expected_string(dr(0)("friendly_message"))

    End Function

    Public Shared Function fnc_get_friendly_message(ByVal str__prm_message_code_value As String, Optional ByVal str__prm_message_context As String = "") As String

      Return fnc_get_friendly_message(str__prm_message_code_value, "", False, str__prm_message_context)

    End Function

    Public Shared Function fnc_select_list() As DataTable
      Dim str_sql As String = "select * from udf_lkp_friendly_message()"

      Return cls_data_access_layer.fnc_get_datatable(str_sql)

    End Function

    Public Shared Function fnc_select_list(ByVal str__prm_filter As String) As DataTable
      If str__prm_filter.Length = 0 Then
        Return fnc_select_list()
        Exit Function
      End If

      Dim str_sql As String = "select * from udf_lkp_friendly_message() where " & str__prm_filter

      Return cls_data_access_layer.fnc_get_datatable(str_sql)

    End Function

    Public Shared Sub sub_delete(ByVal str__prm_pk As String)
      'Dim inst_dynamic_sql As New cls_dynamic_sql
      'inst_dynamic_sql.sub_delete("pk_lkp_friendly_message", str__prm_pk)

    End Sub

  End Class

  Public Class cls_friendly_message_collection
    Inherits List(Of cls_friendly_message)

    Public Sub sub_log()

      For Each inst_friendly_message As cls_friendly_message In Me

        ' when the message is not exception, create a dummy exception object to make use of exception policy infrastructure
        If Not inst_friendly_message.exc_exception Is Nothing Then
          inst_friendly_message.str_error_message = fnc_get_stacktrace(inst_friendly_message.exc_exception)
        End If

        '******************************* logging to database ************************************
        If inst_friendly_message.bln_log_to_database Then
          Try
						'inst_friendly_message.sub_log_to_database()
          Catch ex As Exception
            Throw ex
          End Try
        End If

        '******************************* logging to Falt File ************************************
        ' temporarily removed
        If 1 = 2 Then
          If inst_friendly_message.bln_log_to_file Then
            Try

              inst_friendly_message.sub_log_to_file()

            Catch ex As Exception
              Throw ex
            End Try
          End If
        End If

        ''******************************* logging to email ************************************
        If inst_friendly_message.bln_send_email _
                And cls_constant.int_email_error_log_severity >= inst_friendly_message.int_message_severity Then
          Try
            inst_friendly_message.sub_send_error_email()
          Catch ex As Exception
            Throw ex
          End Try
        End If

      Next

    End Sub

    Private Function fnc_get_stacktrace(ByVal ex As Exception) As String

      If ex Is Nothing Then
        Return ""
      End If

      Dim strb_output_Stack As New System.Text.StringBuilder
      strb_output_Stack.Append(ex.ToString())
      Dim byte_innerReferences As Byte = 0 'to not fetch more than certain inner exceptions.

      Dim innerException As Exception = ex.InnerException
      While Not innerException Is Nothing _
          AndAlso byte_innerReferences < 10
        strb_output_Stack.Insert(0, innerException.ToString())
        strb_output_Stack.AppendLine()
        innerException = innerException.InnerException
        byte_innerReferences += CByte(1)
      End While
      Return strb_output_Stack.ToString
    End Function

  End Class
End Namespace




