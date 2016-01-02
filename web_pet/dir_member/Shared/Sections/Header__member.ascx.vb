Imports ns_enterprise
Imports ns_enterprise.cls_utility

Partial Class dir_member_Shared_Sections_Header__member
	Inherits System.Web.UI.UserControl

	Private Sub dir_member_Shared_Sections_Header__member_Load(sender As Object, e As EventArgs) Handles Me.Load

		If IsPostBack = False Then
			lbl_username.Text = cls_current_user.str_first_name
			a_pet.InnerText = cls_current_user.str_pet_name
		End If

	End Sub

	Private Sub lbtn_logout_Click(sender As Object, e As EventArgs) Handles lbtn_logout.Click

		FormsAuthentication.SignOut()
		Session.Abandon()
		Response.Redirect("~/default.aspx")

	End Sub
End Class
