Imports System.IO
Imports SD = System.Drawing
Imports System.Drawing.Imaging
Imports System.Drawing.Drawing2D
Partial Class dir_playground_jcrop_default
	Inherits System.Web.UI.Page

	Dim path As String = HttpContext.Current.Request.PhysicalApplicationPath + "dir_image\"

	Protected Sub btnUpload_Click(sender As Object, e As EventArgs)
		Dim FileOK As [Boolean] = False
		Dim FileSaved As [Boolean] = False

		If Upload.HasFile Then
			Session("WorkingImage") = Upload.FileName
			Dim FileExtension As [String] = System.IO.Path.GetExtension(Session("WorkingImage").ToString()).ToLower()
			Dim allowedExtensions As [String]() = {".png", ".jpeg", ".jpg", ".gif"}
			For i As Integer = 0 To allowedExtensions.Length - 1
				If FileExtension = allowedExtensions(i) Then
					FileOK = True
				End If
			Next
		End If

		If FileOK Then
			Try
				Upload.PostedFile.SaveAs(path + Session("WorkingImage"))
				FileSaved = True
			Catch ex As Exception
				lblError.Text = "File could not be uploaded." + ex.Message.ToString()
				lblError.Visible = True
				FileSaved = False
			End Try
		Else
			lblError.Text = "Cannot accept files of this type."
			lblError.Visible = True
		End If

		If FileSaved Then
			pnlUpload.Visible = False
			pnlCrop.Visible = True
			imgCrop.ImageUrl = "~/dir_image/" + Session("WorkingImage").ToString()
		End If
	End Sub

	Protected Sub btnCrop_Click(sender As Object, e As EventArgs)
		Dim ImageName As String = Session("WorkingImage").ToString()
		Dim w__1 As Integer = Convert.ToInt32(W.Value)
		Dim h__2 As Integer = Convert.ToInt32(H.Value)
		Dim x__3 As Integer = Convert.ToInt32(X.Value)
		Dim y__4 As Integer = Convert.ToInt32(Y.Value)

		Dim CropImage As Byte() = Crop(path & ImageName, w__1, h__2, x__3, y__4)
		Using ms As New MemoryStream(CropImage, 0, CropImage.Length)
			ms.Write(CropImage, 0, CropImage.Length)
			Using CroppedImage As SD.Image = SD.Image.FromStream(ms, True)
				Dim SaveTo As String = Convert.ToString(path + "crop/") & ImageName
				CroppedImage.Save(SaveTo, CroppedImage.RawFormat)
				pnlCrop.Visible = False
				pnlCropped.Visible = True
				imgCropped.ImageUrl = Convert.ToString("~/dir_image/crop/") & ImageName
			End Using
		End Using

	End Sub

	Private Shared Function Crop(Img As String, Width As Integer, Height As Integer, X As Integer, Y As Integer) As Byte()
		Try
			Using OriginalImage As SD.Image = SD.Image.FromFile(Img)
				Using bmp As New SD.Bitmap(80, 80)
					bmp.SetResolution(OriginalImage.HorizontalResolution, OriginalImage.VerticalResolution)
					Using Graphic As SD.Graphics = SD.Graphics.FromImage(bmp)
						Graphic.SmoothingMode = SmoothingMode.AntiAlias
						Graphic.InterpolationMode = InterpolationMode.HighQualityBicubic
						Graphic.PixelOffsetMode = PixelOffsetMode.HighQuality
						Graphic.DrawImage(OriginalImage, New SD.Rectangle(0, 0, 80, 80), X, Y, Width, Height,
						SD.GraphicsUnit.Pixel)
						Dim ms As New MemoryStream()
						bmp.Save(ms, OriginalImage.RawFormat)
						Return ms.GetBuffer()
					End Using
				End Using
			End Using
		Catch Ex As Exception
			Throw (Ex)
		End Try
	End Function

End Class
