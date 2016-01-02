Imports ns_enterprise
Imports ns_enterprise.cls_utility
Imports System.Data

Partial Class story_default2
	Inherits System.Web.UI.Page

	Private Sub rpt_list_00_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rpt_list_00.ItemDataBound

		Dim int_message_type As Int32 = fnc_convert_expected_int32(e.Item.DataItem("message_type"))

		Dim lbl_question As Label = DirectCast(e.Item.FindControl("lbl_question"), Label)
		Dim lbl_post As Label = DirectCast(e.Item.FindControl("lbl_post"), Label)
		Dim lbl_answer As Label = DirectCast(e.Item.FindControl("lbl_answer"), Label)
		Dim txt_answer As TextBox = DirectCast(e.Item.FindControl("txt_answer"), TextBox)
		Dim div_answer As HtmlGenericControl = DirectCast(e.Item.FindControl("div_answer"), HtmlGenericControl)
		Dim img_answer As Image = DirectCast(e.Item.FindControl("img_answer"), Image)

		Dim lbtn_delete As LinkButton = DirectCast(e.Item.FindControl("lbtn_delete"), LinkButton)
		Dim lbtn_delete__post As LinkButton = DirectCast(e.Item.FindControl("lbtn_delete__post"), LinkButton)
		Dim lbtn_plus As LinkButton = DirectCast(e.Item.FindControl("lbtn_plus"), LinkButton)
		Dim plc_post As PlaceHolder = DirectCast(e.Item.FindControl("plc_post"), PlaceHolder)
		Dim plc_user_time As PlaceHolder = DirectCast(e.Item.FindControl("plc_user_time"), PlaceHolder)
		Dim div_row As HtmlGenericControl = DirectCast(e.Item.FindControl("div_row"), HtmlGenericControl)
		Dim lbl_first_name As Label = DirectCast(e.Item.FindControl("lbl_first_name"), Label)
		Dim lbl_first_name__post As Label = DirectCast(e.Item.FindControl("lbl_first_name__post"), Label)

		If lbl_first_name.Text = cls_current_user.str_first_name Then
			lbl_first_name.Text = "You"
			lbl_first_name__post.Text = "You"
		End If

		'div_answer.Attributes.Add("onblur", "setValue('" & lbl_answer.ClientID & "', this.innerHTML;);setValue('" & txt_answer.ClientID & "', '" & div_answer.InnerHtml & "');")

		'div_row.Attributes.Add("onmouseover", "document.getElementById('" & lbl_question.ClientID & "')style.visibility = 'hidden';")

		'div_row.Attributes.Add("onmouseover", "alert(" & lbl_question.ClientID & " );")
		'div_row.Attributes.Add("onmouseover", "alert(1);")

		'Dim int_incoming_flag As Int32 = fnc_convert_expected_int32(e.Item.DataItem("incoming_flag"))

		lbl_question.Visible = False
		lbl_post.Visible = True
		lbl_answer.Visible = False
		txt_answer.Visible = False
		lbtn_delete.Visible = False
		plc_user_time.Visible = False
		div_answer.Visible = False
		plc_post.Visible = False
		img_answer.Visible = False

		If fnc_convert_expected_string(e.Item.DataItem("media_url")).Length > 0 Then
			img_answer.Visible = True
		End If

		Select Case int_message_type
			' question we sent
			Case 0
				lbl_question.Visible = True
				lbtn_delete.Visible = True
				' post by user (either on website or via extra text answer) - not associated to question
				div_row.Attributes.Add("onmouseout", "hideById('" & lbtn_delete.ClientID & "')")
				div_row.Attributes.Add("onmouseover", "showById('" & lbtn_delete.ClientID & "')")
			Case 1
				plc_post.Visible = True
				lbtn_delete__post.Visible = True
				div_row.Attributes.Add("onmouseout", "hideById('" & lbtn_delete__post.ClientID & "')")
				div_row.Attributes.Add("onmouseover", "showById('" & lbtn_delete__post.ClientID & "')")
			' answer mapped to question
			Case 2
				'lbl_answer.Visible = True
				plc_user_time.Visible = True
				txt_answer.Visible = True
			' post by user on website associated to question
			Case 3
				If fnc_is_valid_guid(e.Item.DataItem("pk_story")) = True Then
					'lbl_answer.Visible = True
					'txt_answer.Visible = True
					plc_user_time.Visible = True
				Else
					'lbtn_plus.Visible = True
					'plc_user_time.Visible = True
				End If
				txt_answer.Visible = True
		End Select

	End Sub

	Protected Sub sub_plus(sender As Object, e As EventArgs)

		Dim lbtn As LinkButton = DirectCast(sender, LinkButton)
		Dim txt_answer As TextBox = DirectCast(lbtn.NamingContainer.FindControl("txt_answer"), TextBox)
		txt_answer.Visible = True
		lbtn.Visible = False

	End Sub
	Protected Sub sub_delete(sender As Object, e As EventArgs)

		Dim lbtn As LinkButton = DirectCast(sender, LinkButton)
		Dim lbl_pk_story As Label = DirectCast(lbtn.NamingContainer.FindControl("lbl_pk_story"), Label)

		cls_story.sub_delete(lbl_pk_story.Text)

		sub_bind_list()

	End Sub

	Protected Sub sub_text_changed(sender As Object, e As EventArgs)

		' grab updated textbox 
		Dim txt_message As TextBox = DirectCast(sender, TextBox)
		' find pk
		Dim str_pk As String = fnc_findcast__label(txt_message.Parent, "lbl_pk_story").Text
		' find pk
		Dim str_fk_story As String = fnc_findcast__label(txt_message.Parent, "lbl_fk_story").Text
		' find fk_anthology
		Dim str_fk_anthology As String = fnc_findcast__label(txt_message.Parent, "lbl_fk_anthology").Text
		' grab updated value
		Dim str_new_value As String = txt_message.Text

		Dim inst_sql As New cls_sql
		inst_sql.str_table_name = "tbl_story"
		inst_sql.sub_add_column("message", str_new_value)
		inst_sql.sub_add_column("fk_anthology", str_fk_anthology)

		If fnc_is_valid_guid(str_pk) = True Then
			' update in database
			inst_sql.str_operation = cls_sql.en_operation.update.ToString
			inst_sql.sub_add_column("pk_story", str_pk)
		Else
			' insert new answer in database
			inst_sql.str_operation = cls_sql.en_operation.insert.ToString
			inst_sql.sub_add_column("fk_story", str_fk_story)
			inst_sql.sub_add_column("fk_person_user", cls_current_user.str_pk_person_user)
		End If

		inst_sql.sub_execute_with_audit(False)

		sub_bind_list()

	End Sub

	Private Sub story_default_Load(sender As Object, e As EventArgs) Handles Me.Load

		img_anthology.ImageUrl = img_anthology.ImageUrl & "?" & Now.ToString

		If IsPostBack = False Then

			sub_load_anthology()
			sub_bind_list()

			Dim str_crop_file As String = HttpContext.Current.Request.PhysicalApplicationPath & "dir_image\dir_anthology\" & lbl_pk_anthology.Text & "\dir_crop\profile.jpg"

			If System.IO.File.Exists(str_crop_file) Then
				img_anthology.ImageUrl = "~/dir_image/dir_anthology/" & lbl_pk_anthology.Text & "/dir_crop/profile.jpg?" & Now.ToString
			End If

		End If

	End Sub

	Private Sub btn_load_more_Click(sender As Object, e As EventArgs) Handles btn_load_more.Click

		lbl_show_row_count.Text = fnc_convert_expected_int32(lbl_show_row_count.Text + 8).ToString

		sub_bind_list()

	End Sub

	Private Sub lbtn_change_picture_Click(sender As Object, e As EventArgs) Handles lbtn_change_picture.Click

		Response.Redirect(cls_controller.fnc_get_root() & "user/profile_picture.aspx")

	End Sub

	Protected Sub sub_save(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs)

		Response.Write(e.CommandArgument.ToString)

		Dim lbtn As LinkButton = DirectCast(sender, LinkButton)
		Dim lbl As Label = fnc_findcast__label(lbtn.Parent, "lbl_answer")

		Response.Write(lbl.Text)

	End Sub

	Sub sub_load_anthology()

		Dim str_pka As String = fnc_convert_expected_string(Request("pka"))

		Dim dr_anthology As DataRow

		If fnc_is_valid_guid(str_pka) Then
			dr_anthology = cls_data_access_layer.fnc_get_datarow("select * from tbl_anthology where pk_anthology = " & fnc_dbwrap(str_pka) & " And active_flag = 1")
		Else
			dr_anthology = cls_data_access_layer.fnc_get_datarow("select top 1 * from tbl_anthology where fk_person_user = " & fnc_dbwrap(cls_current_user.str_pk_person_user) & " And active_flag = 1 order by default_flag desc, sort_order")
		End If

		If dr_anthology Is Nothing Then
			Throw New System.Exception("Error retrieving pet-fkpu: " & cls_current_user.str_pk_person_user)
		End If

		lbl_pk_anthology.Text = fnc_convert_expected_string(dr_anthology("pk_anthology"))
		lbl_pet_name__navigation.Text = fnc_convert_expected_string(dr_anthology("name"))
		lbl_pet_tagline.Text = fnc_convert_expected_string(dr_anthology("tagline"))

	End Sub

	Sub sub_bind_list()

		Dim str_top As String = "top " & lbl_show_row_count.Text
		Dim int_top = fnc_convert_expected_int32(str_top)

		Dim str_udf As String = "udf_story_management()"
		If rdo_with.Checked Then str_udf = "udf_story_management__q_with_a()"
		If rdo_wout.Checked Then str_udf = "udf_story_management__q_wout_a()"

		'Dim dt As DataTable = cls_data_access_layer.fnc_get_datatable("select " & str_top & " * from " & str_udf & " where fk_anthology = " & fnc_dbwrap(lbl_pk_anthology.Text) & " order by sort_date desc, message_type asc, insert_date desc")
		Dim dt As DataTable = cls_data_access_layer.fnc_get_datatable("select * from " & str_udf & " story, (select " & str_top & " pk_story from " & str_udf & " where message_type in (0, 1) and fk_anthology = " & fnc_dbwrap(lbl_pk_anthology.Text) & " order by sort_date desc, message_type asc, insert_date desc) as qry_top where story.pk_story = qry_top.pk_story or qry_top.pk_story = story.fk_story order by sort_date desc, message_type asc, insert_date desc")

		Dim dr_next As DataRow = Nothing
		Dim dr_total As Int32 = dt.Rows.Count
		Dim dct As New Dictionary(Of Int32, DataRow)

		For Each dr As DataRow In dt.Rows
			Select Case fnc_convert_expected_int32(dr("message_type"))
				Case 0
					'If rdo_with
					' is this question the last row
					If dt.Rows.IndexOf(dr) + 1 = dr_total Then
						' add a blank answer row
						Dim dr_new As DataRow = dt.NewRow()
						dr_new("message_type") = 3
						dr_new("fk_story") = fnc_convert_expected_string(dr("pk_story"))
						dr_new("fk_anthology") = fnc_convert_expected_string(dr("fk_anthology"))
						dct.Add(dt.Rows.IndexOf(dr) + 1, dr_new)
						GoTo next_row
					End If
					dr_next = dt.Rows.Item(dt.Rows.IndexOf(dr) + 1)
					If fnc_convert_expected_int32(dr_next("message_type")) = 0 Or fnc_convert_expected_int32(dr_next("message_type")) = 1 Then
						' add a blank answer row
						Dim dr_new As DataRow = dt.NewRow()
						dr_new("message_type") = 3
						dr_new("fk_story") = fnc_convert_expected_string(dr("pk_story"))
						dr_new("fk_anthology") = fnc_convert_expected_string(dr("fk_anthology"))
						dct.Add(dt.Rows.IndexOf(dr) + 1, dr_new)
					End If
				Case 1, 2
					GoTo next_row
			End Select
next_row:
		Next

		Dim int_offset As Int32 = 0
		For Each int_loop As Int32 In dct.Keys
			dt.Rows.InsertAt(DirectCast(dct(int_loop), DataRow), int_loop + int_offset)
			int_offset = int_offset + 1
		Next

		rpt_list_00.DataSource = dt
		rpt_list_00.DataBind()

		Dim int_result_count As Int32 = cls_data_access_layer.fnc_get_scaler__number("select count (pk_story) from " & str_udf & " where fk_anthology = " & fnc_dbwrap(lbl_pk_anthology.Text))
		If int_result_count > int_top Then
			btn_load_more.Enabled = True
		Else
			btn_load_more.Enabled = False
		End If

	End Sub

	Private Sub rdo_all_CheckedChanged(sender As Object, e As EventArgs) Handles rdo_all.CheckedChanged, rdo_with.CheckedChanged, rdo_wout.CheckedChanged

		sub_bind_list()

	End Sub

	Private Sub btn_save_Click(sender As Object, e As EventArgs) Handles btn_save.Click

		If fnc_convert_expected_string(txt_post.Text).Length > 0 Then
			Dim inst_sql As New cls_sql
			inst_sql.str_table_name = "tbl_story"
			inst_sql.str_operation = cls_sql.en_operation.insert.ToString
			inst_sql.sub_add_column("fk_anthology", lbl_pk_anthology.Text)
			inst_sql.sub_add_column("fk_person_user", cls_current_user.str_pk_person_user)
			inst_sql.sub_add_column("message", txt_post.Text)
			inst_sql.sub_execute_with_audit(False)
			sub_bind_list()
			txt_post.Text = ""
		End If

	End Sub
End Class
