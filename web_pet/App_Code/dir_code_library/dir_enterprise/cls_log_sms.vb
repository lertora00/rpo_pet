Imports ns_enterprise
Imports ns_enterprise.cls_utility

Imports Microsoft.VisualBasic

Public Class cls_log_sms

	' phone_number_from property
	Private str__prv_phone_number_from As String
	Public Property str_phone_number_from() As String

		Get
			Return str__prv_phone_number_from
		End Get
		Set(ByVal Value As String)
			str__prv_phone_number_from = Value
		End Set

	End Property

	' phone_number_to property
	Private str__prv_phone_number_to As String
	Public Property str_phone_number_to() As String

		Get
			Return fnc_convert_expected_string(str__prv_phone_number_to)
		End Get
		Set(ByVal Value As String)
			str__prv_phone_number_to = Value
		End Set

	End Property

	' message property
	Private str__prv_message As String
	Public Property str_message() As String

		Get
			Return fnc_convert_expected_string(str__prv_message)
		End Get
		Set(ByVal Value As String)
			str__prv_message = Value
		End Set

	End Property

	' nummedia property
	Private int__prv_nummedia As Int32
	Public Property int_nummedia() As String

		Get
			Return fnc_convert_expected_int32(int__prv_nummedia)
		End Get
		Set(ByVal Value As String)
			int__prv_nummedia = Value
		End Set

	End Property

	' from_city property
	Private str__prv_from_city As String
	Public Property str_from_city() As String

		Get
			Return fnc_convert_expected_string(str__prv_from_city)
		End Get
		Set(ByVal Value As String)
			str__prv_from_city = Value
		End Set

	End Property

	' from_state property
	Private str__prv_from_state As String
	Public Property str_from_state() As String

		Get
			Return fnc_convert_expected_string(str__prv_from_state)
		End Get
		Set(ByVal Value As String)
			str__prv_from_state = Value
		End Set

	End Property

	' from_zip property
	Private str__prv_from_zip As String
	Public Property str_from_zip() As String

		Get
			Return fnc_convert_expected_string(str__prv_from_zip)
		End Get
		Set(ByVal Value As String)
			str__prv_from_zip = Value
		End Set

	End Property

	' from_country property
	Private str__prv_from_country As String
	Public Property str_from_country() As String

		Get
			Return fnc_convert_expected_string(str__prv_from_country)
		End Get
		Set(ByVal Value As String)
			str__prv_from_country = Value
		End Set

	End Property

	' media_content_type_0 property
	Private str__prv_media_content_type_0 As String
	Public Property str_media_content_type_0() As String

		Get
			Return fnc_convert_expected_string(str__prv_media_content_type_0)
		End Get
		Set(ByVal Value As String)
			str__prv_media_content_type_0 = Value
		End Set

	End Property

	' media_url_0 property
	Private str__prv_media_url_0 As String
	Public Property str_media_url_0() As String

		Get
			Return fnc_convert_expected_string(str__prv_media_url_0)
		End Get
		Set(ByVal Value As String)
			str__prv_media_url_0 = Value
		End Set

	End Property

	' active_phone_number_flag property
	Private int__prv_active_phone_number_flag As Int32
	Public Property int_active_phone_number_flag() As Int32

		Get
			Return fnc_convert_expected_int32(int__prv_active_phone_number_flag)
		End Get
		Set(ByVal Value As Int32)
			int__prv_active_phone_number_flag = Value
		End Set

	End Property

	Shared Sub sub_log_message(str__prm_phone_number_from As String, str__prm_phone_number_to As String, str__prm_message As String, Optional int__prm_xml_response_flag As Int32 = 0)

		' simple send outgoing message.  TODO - add incoming_flag to log table.  For now, the from/to number is the give away.
		Dim inst_sql As New cls_sql
		inst_sql.str_table_name = "tbl_log_sms"
		inst_sql.str_operation = cls_sql.en_operation.insert.ToString
		inst_sql.sub_add_column("phone_number_from", str__prm_phone_number_from)
		inst_sql.sub_add_column("phone_number_to", str__prm_phone_number_to)
		inst_sql.sub_add_column("body", str__prm_message)

		inst_sql.sub_execute()

	End Sub
	Sub sub_log_message()

		Dim str_pk As String = Guid.NewGuid.ToString

		Dim inst_sql As New cls_sql
		inst_sql.str_table_name = "tbl_log_sms"
		inst_sql.str_operation = cls_sql.en_operation.insert.ToString
		inst_sql.sub_add_column("pk_log_sms", str_pk)
		inst_sql.sub_add_column("phone_number_from", str__prv_phone_number_from)
		inst_sql.sub_add_column("phone_number_to", str__prv_phone_number_to)
		inst_sql.sub_add_column("body", str__prv_message)

		inst_sql.sub_add_column("nummedia", int__prv_nummedia)

		If str__prv_from_city.Length > 0 Then
			inst_sql.sub_add_column("fromcity", str__prv_from_city)
		End If

		If str__prv_from_state.Length > 0 Then
			inst_sql.sub_add_column("fromstate", str__prv_from_state)
		End If

		If str__prv_from_zip.Length > 0 Then
			inst_sql.sub_add_column("fromzip", str__prv_from_zip)
		End If

		If str__prv_from_country.Length > 0 Then
			inst_sql.sub_add_column("fromcountry", str__prv_from_country)
		End If

		inst_sql.sub_add_column("active_phone_number_flag", int__prv_active_phone_number_flag)

		inst_sql.sub_execute()

		If int__prv_nummedia = 0 Then Exit Sub

		inst_sql = New cls_sql
		inst_sql.str_table_name = "tbl_log_sms__media"
		inst_sql.str_operation = cls_sql.en_operation.insert.ToString
		inst_sql.sub_add_column("fk_log_sms", str_pk)
		inst_sql.sub_add_column("mediacontenttype0", str__prv_media_content_type_0)
		inst_sql.sub_add_column("mediaurl0", str__prv_media_url_0)

		inst_sql.sub_execute()

	End Sub

End Class
