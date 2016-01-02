Imports ns_enterprise.cls_utility

Imports System.Data.SqlClient

Namespace ns_enterprise

	Public Class cls_security

		' pk_security property
		Private str__prv_pk_security As String
		Public Property str_pk_security() As String

			Get
				Return str__prv_pk_security
			End Get
			Set(ByVal Value As String)
				str__prv_pk_security = Value
			End Set

		End Property

		Public Shared Sub sub_quick_login()

			sub_quick_login("x", "x", cls_variable.str_url)

		End Sub

		Public Shared Sub sub_quick_login(ByVal str__prm_username As String, ByVal str__prm_password As String)

			sub_quick_login(str__prm_username, str__prm_password, cls_variable.str_url)

		End Sub

		Public Shared Sub sub_quick_login(ByVal str__prm_username As String, ByVal str__prm_password As String, ByVal str__prm_url As String)

			Dim dr_security As SqlDataReader = cls_data_access_layer.fnc_get_datareader("select * from udf_person_user_login() where username = " & fnc_dbwrap(str__prm_username) & " and password = " & fnc_dbwrap(str__prm_password))
			Dim str_pk_person_user As String = ""

			If dr_security.HasRows Then
				dr_security.Read()
				str_pk_person_user = fnc_convert_expected_string(dr_security("pk_person_user"))
				cls_current_user.sub_persist_current_user__all(str_pk_person_user)
				FormsAuthentication.SetAuthCookie(str_pk_person_user, False)
				cls_controller.sub_redirect(str__prm_url)
			Else
				Throw New Exception("Could not quick login user requested: " & str__prm_username)
			End If

		End Sub



	End Class

End Namespace
