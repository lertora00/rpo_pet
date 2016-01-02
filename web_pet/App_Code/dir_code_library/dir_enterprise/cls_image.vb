Imports System.IO
Imports System.Net
Imports Microsoft.VisualBasic

Namespace ns_enterprise

	Public Class cls_image

		Public Shared Sub sub_save_file_from_url(str__prm_target_file_name As String, str__prm_source_url As String)

			Dim content As Byte()
			Dim request As HttpWebRequest = DirectCast(WebRequest.Create(str__prm_source_url), HttpWebRequest)
			Dim response As WebResponse = request.GetResponse()

			Dim stream As Stream = response.GetResponseStream()

			Using br As New BinaryReader(stream)
				content = br.ReadBytes(2500000)
				br.Close()
			End Using
			response.Close()

			Dim fs As New FileStream(str__prm_target_file_name, FileMode.Create)
			Dim bw As New BinaryWriter(fs)
			Try
				bw.Write(content)
			Finally
				fs.Close()
				bw.Close()
			End Try
		End Sub

	End Class

End Namespace
