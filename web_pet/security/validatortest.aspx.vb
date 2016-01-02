
Partial Class security_validatortest
	Inherits System.Web.UI.Page

	Private Sub btn_validate_Click(sender As Object, e As EventArgs) Handles btn_validate.Click

		If Page.IsValid = False Then
			Response.Write("Invalid")
		Else
			Response.Write("Valid")
		End If

	End Sub
End Class
