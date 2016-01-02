Imports System.Data
Imports ns_enterprise
Imports ns_enterprise.cls_utility

Imports System.Web.HttpContext

Namespace ns_enterprise

  ' class used to manage values that change during the life of the program (versus constants)
  Public Class cls_variable

    Public Shared ReadOnly Property str_server_variable(ByVal str__prm_name As String) As String

      Get
        If Current Is Nothing Then Return ""
        If Current.Request Is Nothing Then Return ""

        Return fnc_convert_expected_string(Current.Request.ServerVariables(str__prm_name))

      End Get

    End Property

    Public Shared Property hsh_person_user(ByVal str__prm_pk_person_user As String) As Hashtable
      Get

        If Current Is Nothing = False Then
          If Current.Cache Is Nothing = False Then
            Return DirectCast(Current.Cache(str__prm_pk_person_user), Hashtable)
          End If
        End If

        Return Nothing

      End Get

      Set(ByVal value As Hashtable)

        Current.Cache.Insert(str__prm_pk_person_user, value)

      End Set

    End Property

    Public Shared Property hsh_person_user_configuration(ByVal str__prm_pk_person_user As String) As Hashtable
      Get

        If Current Is Nothing = False Then
          If Current.Cache Is Nothing = False Then
            Return DirectCast(Current.Cache(str__prm_pk_person_user & "_configuration"), Hashtable)
          End If
        End If

        Return Nothing

      End Get

      Set(ByVal value As Hashtable)

        Current.Cache.Insert(str__prm_pk_person_user & "_configuration", value, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(30))

      End Set

    End Property

    Public Shared Property arl_person_user_application_role(ByVal str__prm_pk_person_user As String) As ArrayList
      Get

        If Current Is Nothing = False Then
          If Current.Cache Is Nothing = False Then
            Return DirectCast(Current.Cache(str__prm_pk_person_user & "_application_role"), ArrayList)
          End If
        End If

        Return Nothing

      End Get

      Set(ByVal value As ArrayList)

        Current.Cache.Insert(str__prm_pk_person_user & "_application_role", value, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(30))

      End Set

    End Property

    Public Shared Property arl_person_user_system_role(ByVal str__prm_pk_person_user As String) As ArrayList
      Get

        If Current Is Nothing = False Then
          If Current.Cache Is Nothing = False Then
            Return DirectCast(Current.Cache(str__prm_pk_person_user & "_system_role"), ArrayList)
          End If
        End If

        Return Nothing

      End Get

      Set(ByVal value As ArrayList)

        Current.Cache.Insert(str__prm_pk_person_user & "_system_role", value, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(30))

      End Set

    End Property

    Public Shared ReadOnly Property bln_authenticated As Boolean
      Get

        If Current Is Nothing = True Then Return False
        If Current.User Is Nothing = True Then Return False

        If Current.User.Identity.IsAuthenticated = True AndAlso fnc_is_valid_guid(Current.User.Identity.Name) Then
          Return True
        End If
        Return False

      End Get

    End Property

    Public Shared ReadOnly Property str_pk_person_user() As String
      Get

        If Current Is Nothing Then
          Return ""
        End If

        If Current.User Is Nothing Then
          Return ""
        End If

        If Current.User.Identity.IsAuthenticated = True Then
          If fnc_is_valid_guid(Current.User.Identity.Name) = True Then
            Return Current.User.Identity.Name
          End If
        End If

        Return ""

      End Get
    End Property

    'Public Shared ReadOnly Property nvc_querystring As Specialized.NameValueCollection

    '  Get
    '    If str_querystring.Length > 0 Then
    '      Return Current.Request.QueryString
    '    End If

    '    Return Nothing

    '  End Get

    'End Property

    'Public Shared ReadOnly Property str_querystring_value(ByVal str__prm_querystring_key As String) As String

    '  ' return nothing if problematic - else ""
    '  Get

    '    ' if no key passed, return nothing
    '    If fnc_convert_expected_string(str__prm_querystring_key).Length = 0 Then Return Nothing

    '    ' if no querystring available, return nothing
    '    If nvc_querystring Is Nothing Then Return ""

    '    ' if key not found, return nothing
    '    If nvc_querystring.AllKeys.Contains(str__prm_querystring_key) = False Then
    '      Return ""
    '    End If

    '    ' return decoded querystring key
    '    Return fnc_convert_expected_string(Current.Server.UrlDecode(nvc_querystring(str__prm_querystring_key)))

    '  End Get
    'End Property

    'Public Shared ReadOnly Property str_querystring() As String

    '  Get

    '    If Current Is Nothing Then
    '      Return ""
    '    End If

    '    If Current.Request Is Nothing Then
    '      Return ""
    '    End If

    '    If Current.Request.QueryString Is Nothing Then
    '      Return ""
    '    End If

    '    Return Current.Server.UrlDecode(fnc_convert_expected_string(Current.Request.QueryString))

    '  End Get

    'End Property

    'Public Shared ReadOnly Property str_url() As String
    '  Get

    '    If Current Is Nothing Then Return ""
    '    If Current.Request Is Nothing Then Return ""
    '    If Current.Request.Url.ToString.Length = 0 Then Return ""

    '    Return Current.Request.Url.ToString

    '  End Get
    'End Property

    'Public Shared Property str_active_user_list() As String
    '  Get

    '    ' reset every 1 minute - rather than several hundred times in a minute if very active site (admin especially)
    '    If DateAdd(DateInterval.Minute, 1, cls_global.dte__pub_active_user_list_refresh) < Now Then
    '      cls_global.dte__pub_active_user_list_refresh = Now
    '      sub_reset_active_user_list()
    '    End If

    '    Return fnc_convert_expected_string(cls_global.str__pub_active_user_list)
    '  End Get
    '  Set(ByVal value As String)
    '    cls_global.str__pub_active_user_list = fnc_convert_expected_string(value)
    '  End Set
    'End Property

    'Public Shared Property str_to_be_killed() As String
    '  Get

    '    Return fnc_convert_expected_string(cls_global.str__pub_to_be_killed)

    '  End Get
    '  Set(ByVal value As String)

    '    cls_global.str__pub_to_be_killed = fnc_convert_expected_string(value)

    '  End Set
    'End Property

    'Public Shared ReadOnly Property int_active_user_count() As Int32
    '  Get

    '    If fnc_convert_expected_string(cls_variable.str_active_user_list).Length = 0 Then
    '      Return 0
    '    End If

    '    Return cls_variable.str_active_user_list().Split(";").Length - 1
    '  End Get
    'End Property

    Public Shared Property str_connection_string__readwrite() As String
      Get
        Return cls_global.str__pub_connection_string__readwrite
      End Get
      Set(ByVal value As String)
        cls_global.str__pub_connection_string__readwrite = fnc_convert_expected_string(value)
      End Set
    End Property

    Public Shared Property str_application_key__generated() As String
      Get
        Return cls_global.str__pub_application_key__generated
      End Get
      Set(ByVal value As String)
        cls_global.str__pub_application_key__generated = fnc_convert_expected_string(value)
      End Set
    End Property


    'Public Shared Sub sub_reset_active_user_list()

    '  ' tbl_log_session - this call needs to retreive from seperate database if we move logging to seperate database

    '  Dim int_session_timeout_minute As Int32 = fnc_convert_expected_int32(cls_constant.str_system_constant("session_timeout_minute"))
    '  If int_session_timeout_minute = 0 Then int_session_timeout_minute = 20

    '  cls_variable.str_active_user_list() = cls_data_access_layer.fnc_get_scaler__string("select [dbo].[udf_get_active_user_list](" & int_session_timeout_minute & ")")

    'End Sub

    Public Shared Function fnc_get_lkp(ByVal str__prm_table_name As String) As DataTable

			If Current Is Nothing = True Then
				Return Nothing
			End If

			If Current.Cache.Item(str__prm_table_name) Is Nothing Then
				cls_utility.sub_refresh_lkp_cache(str__prm_table_name)
			End If

      ' grab lkp table out of cache
      Dim dt_lkp As DataTable = DirectCast(Current.Cache.Item(str__prm_table_name), DataTable)

			Return dt_lkp

		End Function

		Public Shared Sub sub_remove_hsh_person_user(ByVal str__prm_pk_person_user As String)

      Current.Cache.Remove(str__prm_pk_person_user)

    End Sub

    Shared Sub sub_remove_hsh_person_user_configuration(ByVal str__prm_pk_person_user As String)

      Current.Cache.Remove(str__prm_pk_person_user & "_configuration")

    End Sub

    Shared Sub sub_remove_hsh_person_user_application_role(ByVal str__prm_pk_person_user As String)

      Current.Cache.Remove(str__prm_pk_person_user & "_application_role")

    End Sub

    Shared Sub sub_remove_hsh_person_user_system_role(ByVal str__prm_pk_person_user As String)

      Current.Cache.Remove(str__prm_pk_person_user & "_system_role")

    End Sub

    Public Shared ReadOnly Property idct_context() As IDictionary

      Get
        If Current Is Nothing Then Return Nothing
        If Current.Items.Count = 0 Then Return Nothing

        Return Current.Items

      End Get
    End Property

    Public Shared Property obj_context_value(ByVal str__prm_key As String) As Object
      Get

        If idct_context Is Nothing Then Return Nothing
        If idct_context.Contains(str__prm_key) = False Then Return Nothing

        Return idct_context(str__prm_key)

      End Get
      Set(ByVal value As Object)

        If Current Is Nothing Then Exit Property
        If Current.Items.Count = 0 Then Exit Property

        If Current.Items.Contains(str__prm_key) = True Then
          Current.Items(str__prm_key) = value
        Else
          Current.Items.Add(str__prm_key, value)
        End If

      End Set

    End Property

    Public Shared Property str_context_value(ByVal str__prm_key As String) As String
      Get

        If obj_context_value(str__prm_key) Is Nothing Then Return ""

        Return fnc_convert_expected_string(obj_context_value(str__prm_key))

      End Get
      Set(ByVal value As String)

        obj_context_value(str__prm_key) = value

      End Set
    End Property

    'Public Shared Property mcch_cache_item(ByVal str__prm_key As String) As System.Web.Caching.Cache

    'Get

    '  Dim mcch As MemoryCache = MemoryCache.Default

    '  If mcch.Contains(str__prm_key) Then
    '    Return mcch.Item(str__prm_key)
    '  End If

    '  Return Nothing

    'End Get
    'Set(ByVal value As System.Web.Caching.Cache)

    '  Dim mcch As MemoryCache = MemoryCache.Default
    '  'Dim plc As CacheItemPolicy = New CacheItemPolicy()
    '  'plc.AbsoluteExpiration = DateTimeOffset.Now.AddHours(8)

    '  'mcch.Add(str__prm_key, value, plc)
    '  mcch.Add(str__prm_key, value, New CacheItemPolicy())

    'End Set
    'End Property

    Public Shared Function fnc_get_datatable(ByVal str__prm_table_name As String) As DataTable

      Dim str_sql As String = "select * from " & str__prm_table_name

      Return cls_data_access_layer.fnc_get_datatable(str_sql)

      'If httpruntime  Cache.Item(str__prm_table_name) Is Nothing Then
      'Else
      '  Current.Cache.Remove(str__prm_table_name)
      'End If

      'Current.Cache.Insert(str__prm_table_name, cls_data_access_layer.fnc_get_datatable(str_sql), Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(180))

      '' grab table out of cache
      'Dim dt As DataTable = DirectCast(Current.Cache.Item(str__prm_table_name), DataTable)

      'Return dt

    End Function

		Public Shared ReadOnly Property nvc_querystring As NameValueCollection

			Get
				If str_querystring.Length > 0 Then
					Return Current.Request.QueryString
				End If

				Return Nothing

			End Get

		End Property

		Public Shared ReadOnly Property str_querystring_value(ByVal str__prm_querystring_key As String) As String

			' return nothing if problematic - else ""
			Get

				' if no key passed, return nothing
				If fnc_convert_expected_string(str__prm_querystring_key).Length = 0 Then Return Nothing

				' if no querystring available, return nothing
				If nvc_querystring Is Nothing Then Return ""

				' if key not found, return nothing
				If nvc_querystring.AllKeys.Contains(str__prm_querystring_key) = False Then
					Return ""
				End If

				' return decoded querystring key
				Return fnc_convert_expected_string(Current.Server.UrlDecode(nvc_querystring(str__prm_querystring_key)))

			End Get
		End Property

		Public Shared ReadOnly Property str_querystring() As String

			Get

				If Current Is Nothing Then
					Return ""
				End If

				If Current.Request Is Nothing Then
					Return ""
				End If

				If Current.Request.QueryString Is Nothing Then
					Return ""
				End If

				Return Current.Server.UrlDecode(fnc_convert_expected_string(Current.Request.QueryString))

			End Get

		End Property

		Public Shared ReadOnly Property str_url() As String
			Get

				If Current Is Nothing Then Return ""
				If Current.Request Is Nothing Then Return ""
				If Current.Request.Url.ToString.Length = 0 Then Return ""

				Return Current.Request.Url.ToString

			End Get
		End Property

		Public Shared Sub sub_set_session_value(str__prm_keyword As String, str__prm_value As String)

			Current.Session(str__prm_keyword) = fnc_convert_expected_string(str__prm_value)

		End Sub

		Public Shared Function fnc_get_session_value(str__prm_keyword As String) As String

			If Current Is Nothing Then Return ""
			If Current.Session Is Nothing Then Return ""

			Return fnc_convert_expected_string(Current.Session(str__prm_keyword))

		End Function

	End Class

End Namespace