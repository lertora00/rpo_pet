Imports ns_enterprise
Imports ns_enterprise.cls_utility
Imports System.Data

Imports System.Web.HttpContext

Imports System.IO
Imports SD = System.Drawing
Imports System.Drawing.Imaging
Imports System.Drawing.Drawing2D

Partial Class user_profile_picture
	Inherits System.Web.UI.Page


	Protected Sub btnUpload_Click(sender As Object, e As EventArgs)

		System.IO.Directory.CreateDirectory(Current.Request.PhysicalApplicationPath & "dir_image\dir_anthology\" & lbl_pk_anthology.Text & "\")

		Dim path As String = HttpContext.Current.Request.PhysicalApplicationPath & "dir_image\dir_anthology\" & lbl_pk_anthology.Text & "\"

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
				Upload.PostedFile.SaveAs(Path + "profile.jpg")
				FileSaved = True
			Catch ex As Exception
				lblError.Text = "File could Not be uploaded." + ex.Message.ToString()
				lblError.Visible = True
				FileSaved = False
			End Try
		Else
			lblError.Text = "Cannot accept files Of this type."
			lblError.Visible = True
		End If

		If FileSaved Then
			pnlUpload.Visible = False
			pnlCrop.Visible = True
			imgCrop.ImageUrl = "~/dir_image/dir_anthology/" & lbl_pk_anthology.Text & "/profile.jpg"
		End If
	End Sub
	Protected Sub btnCrop_Click(sender As Object, e As EventArgs)

		System.IO.Directory.CreateDirectory(Current.Request.PhysicalApplicationPath & "dir_image\dir_anthology\" & lbl_pk_anthology.Text & "\dir_crop\")

		Dim path As String = HttpContext.Current.Request.PhysicalApplicationPath & "dir_image\dir_anthology\" & lbl_pk_anthology.Text & "\"
		Dim path_crop As String = HttpContext.Current.Request.PhysicalApplicationPath & "dir_image\dir_anthology\" & lbl_pk_anthology.Text & "\dir_crop\"

		Dim ImageName As String = "profile.jpg"
		Dim w__1 As Integer = Convert.ToInt32(W.Value)
		Dim h__2 As Integer = Convert.ToInt32(H.Value)
		Dim x__3 As Integer = Convert.ToInt32(X.Value)
		Dim y__4 As Integer = Convert.ToInt32(Y.Value)

		Dim CropImage As Byte() = Crop(path & ImageName, w__1, h__2, x__3, y__4)
		Using ms As New MemoryStream(CropImage, 0, CropImage.Length)
			ms.Write(CropImage, 0, CropImage.Length)
			Using CroppedImage As SD.Image = SD.Image.FromStream(ms, True)
				Dim SaveTo As String = path_crop & ImageName
				CroppedImage.Save(SaveTo, CroppedImage.RawFormat)
				pnlUpload.Visible = True
				pnlCrop.Visible = False
				img_anthology.ImageUrl = "~/dir_image/dir_anthology/" & lbl_pk_anthology.Text & "/dir_crop/profile.jpg?" & DateTime.Now.ToString
				lbl_success.Text = "Profile picture updated."
				plc_success.Visible = True
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
	Private Sub user_profile_picture_Load(sender As Object, e As EventArgs) Handles Me.Load

		img_anthology.ImageUrl = img_anthology.ImageUrl & "?" & Now.ToString

		Dim str_pka As String = fnc_convert_expected_string(Request("pka"))
		Dim dr_anthology As DataRow

		If IsPostBack = False Then
			If fnc_is_valid_guid(str_pka) Then
				dr_anthology = cls_data_access_layer.fnc_get_datarow("Select * from tbl_anthology where pk_anthology = " & fnc_dbwrap(str_pka) & " And active_flag = 1")
			Else
				dr_anthology = cls_data_access_layer.fnc_get_datarow("Select top 1 * from tbl_anthology where fk_person_user = " & fnc_dbwrap(cls_current_user.str_pk_person_user) & " And active_flag = 1 order by default_flag desc, sort_order")
			End If

			If dr_anthology Is Nothing Then
				Throw New System.Exception("Error retrieving pet-fkpu:  " & cls_current_user.str_pk_person_user)
			End If

			lbl_pk_anthology.Text = fnc_convert_expected_string(dr_anthology("pk_anthology"))
			lbl_pet_name__navigation.Text = fnc_convert_expected_string(dr_anthology("name"))
			lbl_pet_tagline.Text = fnc_convert_expected_string(dr_anthology("tagline"))

			Dim str_crop_file As String = HttpContext.Current.Request.PhysicalApplicationPath & "dir_image\dir_anthology\" & lbl_pk_anthology.Text & "\dir_crop\profile.jpg"

			If System.IO.File.Exists(str_crop_file) Then
				img_anthology.ImageUrl = "~/dir_image/dir_anthology/" & lbl_pk_anthology.Text & "/dir_crop/profile.jpg"
			End If
			plc_success.Visible = False

		End If


	End Sub
End Class
