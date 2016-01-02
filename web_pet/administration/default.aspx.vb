
Partial Class administration_default
	Inherits System.Web.UI.Page

	Private Sub administration_default_Load(sender As Object, e As EventArgs) Handles Me.Load

		Response.Write(HttpContext.Current.User.Identity.Name.ToString)
		Response.Write(ns_enterprise.cls_current_user.str_username)

	End Sub
End Class
