Imports System.Collections
Imports System.Configuration
Imports System.Data
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.IO
' this is for the file upload
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging

Partial Class user_shrink_image
	Inherits System.Web.UI.Page

	Protected Sub UploadFile(s As [Object], e As EventArgs)
	End Sub

	Private Sub btn_upload_Click(sender As Object, e As EventArgs) Handles btn_upload.Click

		' First we check to see if the user has selected a file
		If fileupload.HasFile Then
			' Find the fileUpload control
			Dim filename As String = fileupload.FileName

			' Check if the directory we want the image uploaded to actually exists or not
			If Not Directory.Exists(MapPath("Uploaded-Files")) Then
				' If it doesn't then we just create it before going any further
				Directory.CreateDirectory(MapPath("Uploaded-Files"))
			End If

			' Specify the upload directory
			Dim directory__1 As String = Server.MapPath("Uploaded-Files\")

			' Create a bitmap of the content of the fileUpload control in memory
			Dim originalBMP As New Bitmap(fileupload.FileContent)


			' Calculate the new image dimensions 
			Dim thumbnailSize As Integer = 250
			Dim newWidth As Integer, newHeight As Integer
			If originalBMP.Width > originalBMP.Height Then
				newWidth = thumbnailSize
				newHeight = originalBMP.Height * thumbnailSize / originalBMP.Width
			Else
				newWidth = originalBMP.Width * thumbnailSize / originalBMP.Height
				newHeight = thumbnailSize
			End If

			'=======================================================
			'Service provided by Telerik (www.telerik.com)
			'Conversion powered by NRefactory.
			'Twitter: @telerik
			'Facebook: facebook.com/telerik
			'=======================================================

			' Create a new bitmap which will hold the previous resized bitmap
			Dim newBMP As New Bitmap(originalBMP, newWidth, newHeight)
			' Create a graphic based on the new bitmap
			Dim oGraphics As Graphics = Graphics.FromImage(newBMP)

			' Set the properties for the new graphic file
			oGraphics.SmoothingMode = SmoothingMode.AntiAlias
			oGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic
			' Draw the new graphic based on the resized bitmap
			oGraphics.DrawImage(originalBMP, 0, 0, newWidth, newHeight)

			' Save the new graphic file to the server
			newBMP.Save(Convert.ToString(directory__1 & Convert.ToString("tn_")) & filename)

			' Once finished with the bitmap objects, we deallocate them.
			originalBMP.Dispose()
			newBMP.Dispose()
			oGraphics.Dispose()

			' Write a message to inform the user all is OK
			label.Text = (Convert.ToString("File Name: <b style='color: red;'>") & filename) + "</b><br>"
			label.Text += "Content Type: <b style='color: red;'>" + fileupload.PostedFile.ContentType + "</b><br>"
			label.Text += "File Size: <b style='color: red;'>" + fileupload.PostedFile.ContentLength.ToString() + "</b>"
			' Display the image to the user
			image1.Visible = True
			image1.ImageUrl = Convert.ToString("/Uploaded-Files/tn_") & filename
		Else
			label.Text = "No file uploaded!"
		End If

	End Sub
End Class

