Imports ns_enterprise
Imports ns_enterprise.cls_utility

Imports System.Net
Imports System.Net.Mail

Partial Class security_forgot_password
	Inherits System.Web.UI.Page

	Private Sub btn_forgot_password_Click(sender As Object, e As EventArgs) Handles btn_send.Click

		If fnc_convert_expected_string(txt_email_address.Text).Length = 0 Then
			plc_error.Visible = True
			lbl_error.Text = "Please enter a valid email address."
			Exit Sub
		End If

		Dim str_pk_person_user As String = cls_data_access_layer.fnc_get_scaler__string("select pk_person_user from tbl_person_user where username = " & fnc_dbwrap(txt_email_address.Text) & " and active_flag = 1")

		If fnc_is_valid_guid(str_pk_person_user) = False Then
			plc_error.Visible = True
			lbl_error.Text = "Email address not valid."
		End If

		Dim str_new_password As String = fnc_generate_password(6)

		Dim mm As New MailMessage("Support@PetFolio.com", txt_email_address.Text)
		mm.Subject = "Password Recovery"
		mm.Body = String.Format("Hi {0},<br /><br />Your password is {1}.<br /><br />Thank You.", "Phil@Lertora.com", str_new_password)
		mm.IsBodyHtml = True
		Dim smtp As New SmtpClient()
		smtp.Host = "smtp.gmail.com"
		smtp.EnableSsl = True
		Dim NetworkCred As New NetworkCredential()
		NetworkCred.UserName = "phil.lertora@gmail.com"
		NetworkCred.Password = "lskdjf00"
		smtp.UseDefaultCredentials = True
		smtp.Credentials = NetworkCred
		smtp.Port = 587
		smtp.Send(mm)

		Dim inst_sql As New cls_sql
		inst_sql.str_table_name = "tbl_person_user"
		inst_sql.str_operation = cls_sql.en_operation.update.ToString
		inst_sql.sub_add_column("pk_person_user", str_pk_person_user)
		inst_sql.sub_add_column("password", str_new_password)
		inst_sql.sub_execute_with_audit(False)

		plc_success.Visible = True
		lbl_success.Text = "New password sent to email address specified."

	End Sub

	Private Sub security_forgot_password_Load(sender As Object, e As EventArgs) Handles Me.Load

		If IsPostBack = True Then
			plc_error.Visible = False
			plc_success.Visible = False
		End If

	End Sub

	Public Function fnc_generate_password(length As Integer) As String

		Const valid As String = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890"
		Dim strb_result As New StringBuilder()
		Dim rnd As New Random()
		While 0 < System.Math.Max(System.Threading.Interlocked.Decrement(length), length + 1)
			strb_result.Append(valid(rnd.[Next](valid.Length)))
		End While

		Return strb_result.ToString()

	End Function

End Class
