Imports ns_enterprise.cls_utility

Imports System.Web.HttpContext
Imports Microsoft.VisualBasic
Imports Twilio

Namespace ns_enterprise

	Public Class cls_sms

		Public Shared Sub sub_send_message(str__prm_phone_number As String, str__prm_message As String)

			If str__prm_phone_number.Length = 0 Then
				Throw New Exception("cls_sms.sub_send_phone_number: empty phone_number")
			End If

			If str__prm_message.Length = 0 Then
				Throw New Exception("cls_sms.sub_send_message: empty message")
			End If

			Dim str_phone_number_from As String = cls_constant.str_system_constant("twilio_phone_number")

			Dim trc_twilio_rest_client As New TwilioRestClient(cls_constant.str_system_constant("twilio_accountsid"), cls_constant.str_system_constant("twilio_authtoken"))

			'If System.Web.HttpContext.Current.Request.Url.ToString.ToLower.Contains("localhost") = False Then
			trc_twilio_rest_client.SendMessage(str_phone_number_from, str__prm_phone_number, str__prm_message)
			'End If

			cls_log_sms.sub_log_message(str_phone_number_from, str__prm_phone_number, str__prm_message)

		End Sub

		Public Shared Sub sub_xml_response(str__prm_response As String)

			Dim str_xml As String = ""
			str_xml = "<?xml version=""1.0"" encoding=""UTF-8""?><Response><Message>" & str__prm_response & "</Message></Response>"
			Dim xmlDocument As System.Xml.XmlDocument = New System.Xml.XmlDocument
			xmlDocument.LoadXml(str_xml)

			Current.Response.Clear()
			Current.Response.ContentType = "text/xml"
			Current.Response.ContentEncoding = Encoding.UTF8
			Current.Response.Write(xmlDocument.InnerXml)
			Current.Response.End()

		End Sub

	End Class

End Namespace
