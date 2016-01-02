Imports ns_enterprise
Imports ns_enterprise.cls_utility

Partial Class user_option
	Inherits System.Web.UI.Page

	Private Sub user_option_Load(sender As Object, e As EventArgs) Handles Me.Load

		If IsPostBack = False Then
			lbl_first_name.Text = cls_current_user.str_first_name
		End If

	End Sub
End Class
