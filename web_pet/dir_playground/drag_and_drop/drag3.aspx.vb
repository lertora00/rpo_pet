Imports ns_enterprise
Imports ns_enterprise.cls_utility

Imports System.Data

Partial Class dir_playground_drag3
	Inherits System.Web.UI.Page

	Private Sub dir_playground_drag3_Load(sender As Object, e As EventArgs) Handles Me.Load

		Dim dt As DataTable = cls_data_access_layer.fnc_get_datatable("SELECT 1 as Id, 'maryland' as Location, 'none' as Preference union select 2 as id, 'virginia' as locaion, 'high' as preference union select 3 as id, 'new york' as location, 'medium' as preference")
		gvLocations.DataSource = dt
		gvLocations.DataBind()

	End Sub

	Protected Sub UpdatePreference(sender As Object, e As EventArgs)
		Dim locationIds As Integer() = (From p In Request.Form("LocationId").Split(",")
																		Select Integer.Parse(p)).ToArray()
		Dim preference As Integer = 1
		For Each locationId As Integer In locationIds
			Me.UpdatePreference(locationId, preference)
			preference += 1
		Next

		Response.Redirect(Request.Url.AbsoluteUri)
	End Sub

	Private Sub UpdatePreference(locationId As Integer, preference As Integer)
		'Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
		'Using con As New SqlConnection(constr)
		'	Using cmd As New SqlCommand("UPDATE HolidayLocations SET Preference = @Preference WHERE Id = @Id")
		'		Using sda As New SqlDataAdapter()
		'			cmd.CommandType = CommandType.Text
		'			cmd.Parameters.AddWithValue("@Id", locationId)
		'			cmd.Parameters.AddWithValue("@Preference", preference)
		'			cmd.Connection = con
		'			con.Open()
		'			cmd.ExecuteNonQuery()
		'			con.Close()
		'		End Using
		'	End Using
		'End Using
	End Sub

End Class
