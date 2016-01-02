Imports ns_enterprise
Imports ns_enterprise.cls_utility

Partial Class dir_incoming_response
	Inherits System.Web.UI.Page

	Private Sub dir_incoming_response_Load(sender As Object, e As EventArgs) Handles Me.Load

		Dim str_first_name As String = fnc_convert_expected_string(HttpContext.Current.Items("str_first_name"))
		Dim str_pet_name As String = fnc_convert_expected_string(HttpContext.Current.Items("str_pet_name"))
		Dim str_from As String = fnc_convert_expected_string(HttpContext.Current.Items("str_from"))

		Dim str_response As String = cls_text_response.fnc_get_random_response()
		str_response = Replace(str_response, "[first_name]", str_first_name)
		str_response = Replace(str_response, "[pet_name]", str_pet_name)

		' log outgoing text (response)
		cls_log_sms.sub_log_message("n/a", str_from, str_response, 1)

		cls_sms.sub_xml_response(str_response)

	End Sub

End Class
