
Partial Class cls_error
	Inherits System.Web.UI.Page

	Private Sub cls_error_Load(sender As Object, e As EventArgs) Handles Me.Load

		Try
			Try
				Dim exc As Exception = DirectCast(Cache("exc_last_error"), Exception)

				If exc Is Nothing Then
					lbl_error.Text = "General error retrieving last exception."
					Exit Sub
				End If

				Dim strb_output_stack As New System.Text.StringBuilder
				'strb_output_stack.Append(exc.ToString())
				Dim byte_innerReferences As Byte = 0 'to not fetch more than certain inner exceptions.

				Dim innerException As Exception = exc.InnerException
				While Not innerException Is Nothing _
					AndAlso byte_innerReferences < 10
					strb_output_stack.Insert(0, vbCrLf)
					strb_output_stack.Insert(0, vbCrLf)
					strb_output_stack.Insert(0, "<br />")
					strb_output_stack.Insert(0, "<br />")
					strb_output_stack.Insert(0, "<hr />")
					strb_output_stack.Insert(0, Replace(innerException.ToString(), "at ns_enterprise.cls_data_access_layer", "<br /><br /><strong>" & "at ns_enterprise.cls_data_access_layer"))
					strb_output_stack.AppendLine()
					innerException = innerException.InnerException
					byte_innerReferences += CByte(1)
				End While

				lbl_error.Text = strb_output_stack.ToString

				HttpContext.Current.Cache.Remove("exc_last_error")

			Catch

			End Try

		Catch
		End Try

	End Sub

End Class
