Imports ns_enterprise
Imports ns_enterprise.cls_utility

Imports System.Data

Imports Microsoft.VisualBasic

Namespace ns_enterprise

	Public Class cls_text_response

		Public Shared Function fnc_get_random_response() As String

			Return cls_data_access_layer.fnc_get_scaler__string("select top 1 response from tbl_text_response where active_flag = 1 order by newid()")

		End Function

	End Class
End Namespace
