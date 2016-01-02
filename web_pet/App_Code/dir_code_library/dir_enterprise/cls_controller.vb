Imports ns_enterprise.cls_utility

Imports System.Web
Imports System.Web.HttpContext
Imports System.Collections.Specialized
Imports System.Configuration

Namespace ns_enterprise

	' controls user actions (redirecting, etc)
	Public Class cls_controller

		Private str__prv_url As String
		Public Property str_url() As String
			Get
				Return str__prv_url
			End Get
			Set(ByVal Value As String)
				str__prv_url = Value
			End Set
		End Property

    Public Shared ReadOnly Property str_path() As String
      Get
				Return Replace(Replace(Current.Request.Path, cls_constant.str_subweb(True), ""), "code/", "")
			End Get
    End Property

		' returns path (folder) by path depth - 0 returns root
		Public Shared ReadOnly Property str_path(ByVal int__prm_depth As Int32) As String

			Get
				If int__prm_depth = 0 Then Return fnc_get_root()

				Dim str_path_without_page As String = fnc_get_path_without_page(HttpContext.Current.Request, True)

				If str_path_without_page.EndsWith("/") Then str_path_without_page = str_path_without_page.Remove(str_path_without_page.Length - 1, 1)

				If int__prm_depth < str_path_without_page.Split("/").Length Then
					Return str_path_without_page.Split("/")(int__prm_depth)
				End If

				Return ""

			End Get
		End Property

		Private nvc__prv_querystring As Specialized.NameValueCollection

		Public Property nvc_querystring() As Specialized.NameValueCollection

			Get
				Return nvc__prv_querystring
			End Get
			Set(ByVal Value As Specialized.NameValueCollection)
				nvc__prv_querystring = Value
			End Set

		End Property

		Public Sub New()

			MyBase.new()

		End Sub

		Public Sub New(ByVal str__prm_url As String)

			MyBase.new()
			str__prv_url = str__prm_url

		End Sub

		Public Sub New(ByVal str__prm_url As String, ByVal str__prm_key As String, ByVal str__prm_value As String)

			MyBase.new()
			str__prv_url = str__prm_url

			sub_add_querystring(str__prm_key, str__prm_value)

		End Sub

		Public Sub sub_add_querystring(ByVal str__prm_key As String, ByVal str__prm_value As String)

			' cannot add a querystring without a key.  without a value seems worthless
			If str__prm_key.Length = 0 Then Exit Sub
			If str__prm_value.Length = 0 Then Exit Sub

			' if not yet created, create it
			If nvc__prv_querystring Is Nothing Then
				nvc__prv_querystring = New Specialized.NameValueCollection
			End If

			' add encoded querystring key and value
			' TODO using prv when this is called by shared method - could be a problem.
			nvc__prv_querystring.Add(Current.Server.UrlEncode(str__prm_key), Current.Server.UrlEncode(str__prm_value))

		End Sub

		Public Sub sub_redirect()

			' redirect based on collection and url
			Current.Response.Redirect(str__prv_url & fnc_format_querystring_collection(nvc__prv_querystring))

		End Sub

		Public Shared Sub sub_redirect(ByVal str__prm_url_complete As String)

			Current.Response.Write("This method needs to breakdown querystring, if available, rebuild it and then redirect")

			'' TODO Needs to breakdown url and then call redirect
			''If str__prm_url_complete.IndexOf("?") = 0 Then
			Current.Response.Redirect(str__prm_url_complete)
			''End If

		End Sub

		Public Sub sub_redirect(ByVal str__prm_url As String, ByVal str__prm_key As String, ByVal str__prm_value As String)

			' this needs to build the querstrying, etc and call sub_redirect
			Current.Response.End()

		End Sub

		Public Sub sub_set_back_url()

      sub_set_cookie(str__prv_url, Current.Request.Url.PathAndQuery)

		End Sub

		Public Sub sub_set_back_url(ByVal str__prv_back_url As String)

      sub_set_cookie(str__prv_url, str__prv_back_url)

    End Sub

    Public Shared Sub sub_set_back_url(ByVal req__prm As System.Web.HttpRequest, ByVal str__prm_value As String)

      ' request object is nothing, exit
      If req__prm Is Nothing Then
        Exit Sub
      End If

      ' referrer is nothing, exit
      If req__prm.UrlReferrer Is Nothing Then
        Exit Sub
      End If

      ' make sure referring page is different than current page, if same - exit
      If req__prm.UrlReferrer.AbsolutePath = req__prm.Url.AbsolutePath Then
        Exit Sub
      End If

      ' write back cookie for specific page
      sub_set_cookie(req__prm.Url.AbsolutePath, req__prm.UrlReferrer.PathAndQuery)

    End Sub

		Public Shared Function fnc_get_back_url(ByVal req__prm As System.Web.HttpRequest, ByVal str__prm_url_if_no_cookie As String) As String

			Dim str_url As String

			' request object is nothing, exit
			If req__prm Is Nothing Then
				Return str__prm_url_if_no_cookie
			End If

			' if url is nothing, exit
			If req__prm.Url Is Nothing Then
				Return str__prm_url_if_no_cookie
			End If

			' attempt to get cookie
			If req__prm.Cookies(req__prm.Url.AbsolutePath) Is Nothing = False Then
				str_url = fnc_convert_expected_string(req__prm.Cookies(req__prm.Url.AbsolutePath).Value)
			Else
				str_url = ""
			End If

			' if cookie url retrieved, return it, else return no cookie value
			If str_url.Length > 0 Then
				Return str_url
			Else
				Return str__prm_url_if_no_cookie
			End If

		End Function

		Public Shared Sub sub_redirect(ByVal str__prm_url As String, ByVal str__prm_querystring As String)

			' be sure to htmlencode querystring
			Current.Response.Redirect(str__prm_url & "?" & Current.Server.HtmlEncode(str__prm_querystring))

		End Sub

		'Copy a querystring key (and value) to a new namevaluecollection
		Public Sub sub_copy_querystring(ByVal nvc__prm_querystring As System.Collections.Specialized.NameValueCollection)

			For Each str_querystring As String In nvc__prm_querystring.AllKeys
				sub_add_querystring(str_querystring, nvc__prm_querystring(str_querystring))
			Next

		End Sub

		Public Shared Function fnc_convert_string_to_querystring(ByVal str__prm_querystring As String) As NameValueCollection

			Return Nothing

			' A querystring is a URL plus 0 or many key/value combinations
			' TODO - need a set of methods (cls_web) to handle
			'		may want to create user defined datatype of string plus NVC
			'			could also have a class devoted entirely to querystring
			' Make sure to review existing classes for querystring - at least use as a guide
			'		problem with existing is they are readonly.

      Dim str_querystring As String = ""
			Dim arr_querystring As String()
			Dim nvc_querystring As NameValueCollection = New NameValueCollection

			If fnc_convert_expected_string(str__prm_querystring).Length = 0 Then
				Return Nothing
			End If

			' establish single delimiter
			str_querystring = Replace(str__prm_querystring, "?", "&")

			' break up by deimiter
			' MAJOR TODO - how to handle encoded ampersands or will delimiter (versus real data)
			'		always be un-encoded?
			arr_querystring = Split(str__prm_querystring, "&")

			For int_index As Int32 = 0 To arr_querystring.Length - 1
				nvc_querystring.Add("", "")
			Next

		End Function

		Public Sub sub_copy_querystring(ByVal req__prm As HttpRequest, ByVal str__prm_key As String)

			' no request, do nothing
			If req__prm Is Nothing Then
				Exit Sub
			End If

			' if querystring empty, do nothing
			If req__prm.QueryString Is Nothing OrElse _
			 req__prm.QueryString.ToString.Length = 0 OrElse _
			 req__prm.QueryString.Item(str__prm_key) Is Nothing OrElse _
			 req__prm.QueryString.Item(str__prm_key).Length = 0 Then
				Exit Sub
			End If

			' add prior value to new querystring
			sub_add_querystring(str__prm_key, req__prm.QueryString.Item(str__prm_key))

		End Sub

    Public Shared Function fnc_add_to_querystring(ByVal str__prm_querystring As String, ByVal str__prm_key As String, ByVal str__prm_value As String) As String

      Dim str_delimiter As String = "?"

      ' No querystring key to add - return original querystring
      If fnc_convert_expected_string(str__prm_key).Length = 0 Then
        Return str__prm_querystring
      End If

      If str__prm_querystring.IndexOf("?") >= 0 Then
        str_delimiter = "&"
      End If

      Return str__prm_querystring & str_delimiter & Current.Server.UrlEncode(str__prm_key) & "=" & Current.Server.UrlEncode(str__prm_value)

    End Function

    Public Shared Function fnc_add_to_querystring(ByVal str__prm_querystring As String, ByVal str__prm_key As String, ByVal str__prm_value As String, ByVal bln__prm_replace_existing_key As Boolean) As String

      ' needs to take the string querystring, convert to nvs, remove, return to string
      Return Nothing

      Dim str_delimiter As String = "?"

      ' No querystring key to add - return original querystring
      If fnc_convert_expected_string(str__prm_key).Length = 0 Then
        Return str__prm_querystring
      End If

      If str__prm_querystring.IndexOf("?") >= 0 Then
        str_delimiter = "&"
      End If

      Return str__prm_querystring & str_delimiter & Current.Server.UrlEncode(str__prm_key) & "=" & Current.Server.UrlEncode(str__prm_value)

    End Function

		' gets the root URL of the site
		Public Shared Function fnc_get_root() As String

			Dim str_url As String = Current.Request.Url.AbsoluteUri
			Dim str_path_and_query As String = Current.Request.Url.PathAndQuery

			Dim str_return As String = Replace(str_url, str_path_and_query, "") & "/"

			' if you are running as a subweb versus primary web (localhost verus localhost/web_service_lifecycle) then add subweb to root url
			If str_url.Contains(cls_constant.str_subweb) = True And str_return.Contains(cls_constant.str_subweb) = False Then
				str_return = str_return & cls_constant.str_subweb(True)
			End If

			Return str_return

    End Function

    Public Shared Function fnc_get_querystring_value(ByVal str__prm_querystring_key As String) As String

      Return fnc_get_querystring_value(Current.Request.QueryString, str__prm_querystring_key)

    End Function

    Public Shared Function fnc_get_querystring_value(ByVal nvc__prm_querystring As Specialized.NameValueCollection, ByVal str__prm_querystring_key As String) As String

      Return cls_utility.fnc_get_querystring_value(nvc__prm_querystring, str__prm_querystring_key)

    End Function

		Public Shared Function fnc_remove_querystring_key_set_length(ByVal str__prm_querystring As String, ByVal str__prm_key As String, ByVal int__prm_key_value_set_length As Int32) As String

			If str__prm_querystring.IndexOf(str__prm_key) < 0 Then
				Return str__prm_querystring
			End If

			Return str__prm_querystring.Remove(str__prm_querystring.IndexOf(str__prm_key), int__prm_key_value_set_length)

		End Function

		Public Shared Function fnc_remove_querystring_key(ByVal nvc__prm_querystring As Specialized.NameValueCollection, ByVal str__prm_key As String) As Specialized.NameValueCollection

			Dim nvc As New NameValueCollection(nvc__prm_querystring)

			Try
				nvc.Remove(str__prm_key)
			Catch e As Exception
			End Try

			Return nvc

		End Function

    Public Shared Sub sub_go_root()

      Current.Response.Redirect(fnc_get_root)

    End Sub

		Public Shared Sub sub_write(ByVal str__prm As String)

			If Current Is Nothing Then Exit Sub

			If Current.Response Is Nothing Then Exit Sub

			Current.Response.Write(str__prm)

		End Sub

		Public Shared Sub sub_end()

			If Current Is Nothing Then Exit Sub

			If Current.Response Is Nothing Then Exit Sub

			Current.Response.End()

		End Sub

	End Class

End Namespace
