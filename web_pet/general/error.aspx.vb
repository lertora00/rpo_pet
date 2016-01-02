
Partial Class general_error
	Inherits System.Web.UI.Page

	Private Sub btn_signout_Click(sender As Object, e As EventArgs) Handles btn_signout.Click

		FormsAuthentication.SignOut()
		Session.Abandon()

		Response.Redirect("~")

	End Sub
End Class
