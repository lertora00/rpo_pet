
Partial Class _default
	Inherits System.Web.UI.Page

	Private Sub _default_Load(sender As Object, e As EventArgs) Handles Me.Load

		If IsPostBack = False Then
			If ns_enterprise.cls_controller.fnc_get_querystring_value("ref").Length > 0 Then
				lbl_referral_code.Text = ns_enterprise.cls_controller.fnc_get_querystring_value("ref")
				Session("ref") = lbl_referral_code.Text
			End If
		End If

	End Sub

End Class
