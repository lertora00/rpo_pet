Imports ns_enterprise.cls_utility

Namespace ns_enterprise

	Public Class cls_javascript

		Public Shared Function fnc_wrap_try_catch(ByVal str__prm_javascript As String) As String

			Dim str_javascript As String = fnc_convert_expected_string(str__prm_javascript)
			If str_javascript.Length = 0 Then Return ""

			' already starts with try catch
			If str_javascript.ToLower.Substring(0, 3) = "try" Then Return str__prm_javascript

			str_javascript = "try {" & str_javascript & "} catch (err) {};"

			Return str_javascript

		End Function

	End Class

End Namespace
