Imports ns_enterprise
Imports ns_enterprise.cls_utility
Imports System.Data

Imports Microsoft.VisualBasic

Public Class cls_global
  Inherits System.Web.HttpApplication

  Private Shared dt__prv_database_column As DataTable
  Private Shared dt__prv_system_column_validation As DataTable
  Private Shared arrl__prv_audit_exclude_table_list As ArrayList
  Private Shared arrl__prv_audit_exclude_column_list As ArrayList
  Private Shared dct__prv_system_constant As IDictionary
  Private Shared str__prv_email_append_to As String
  Private Shared str__prv_email_append_cc As String
  Private Shared str__prv_email_append_bcc As String
  Private Shared str__prv_database_version As String
  Private Shared str__prv_pk_person_user__default As String
  Private Shared str__prv_application_key__generated As String
  Private Shared str__prv_active_user_list As String
  Private Shared dte__prv_active_user_list_refresh As DateTime
  Private Shared str__prv_to_be_killed As String
  Private Shared int__prv_active_user_count As Int32
  Private Shared str__prv_connection_string__readwrite As String
  Private Shared str__prv_connection_string__log As String

  Public Shared Property dt__pub_database_column() As DataTable
    Get
      Return dt__prv_database_column
    End Get
    Set(ByVal value As DataTable)
      dt__prv_database_column = value
    End Set
  End Property

  Public Shared Property dt__pub_system_column_validation() As DataTable
    Get
      Return dt__prv_system_column_validation
    End Get
    Set(ByVal value As DataTable)
      dt__prv_system_column_validation = value
    End Set
  End Property

  Public Shared Property arrl__pub_audit_exclude_table_list() As ArrayList
    Get
      Return arrl__prv_audit_exclude_table_list
    End Get
    Set(ByVal value As ArrayList)
      arrl__prv_audit_exclude_table_list = value
    End Set
  End Property

  Public Shared Property arrl__pub_audit_exclude_column_list() As ArrayList
    Get
      Return arrl__prv_audit_exclude_column_list
    End Get
    Set(ByVal value As ArrayList)
      arrl__prv_audit_exclude_column_list = value
    End Set
  End Property

  Public Shared Property dct__pub_system_constant() As IDictionary
    Get
      Return dct__prv_system_constant
    End Get
    Set(ByVal value As IDictionary)
      dct__prv_system_constant = value
    End Set
  End Property

  Public Shared Property str__pub_email_append_to() As String
    Get
      Return str__prv_email_append_to
    End Get
    Set(ByVal value As String)
      str__prv_email_append_to = value
    End Set
  End Property

  Public Shared Property str__pub_email_append_cc() As String
    Get
      Return str__prv_email_append_cc
    End Get
    Set(ByVal value As String)
      str__prv_email_append_cc = value
    End Set
  End Property

  Public Shared Property str__pub_email_append_bcc() As String
    Get
      Return str__prv_email_append_bcc
    End Get
    Set(ByVal value As String)
      str__prv_email_append_bcc = value
    End Set
  End Property

  Public Shared Property str__pub_database_version() As String
    Get
      Return str__prv_database_version
    End Get
    Set(ByVal value As String)
      str__prv_database_version = value
    End Set
  End Property

  Public Shared Property str__pub_pk_person_user__default() As String
    Get
      Return str__prv_pk_person_user__default
    End Get
    Set(ByVal value As String)
      str__prv_pk_person_user__default = value
    End Set
  End Property

  Public Shared Property str__pub_application_key__generated() As String
    Get
      Return str__prv_application_key__generated
    End Get
    Set(ByVal value As String)
      str__prv_application_key__generated = value
    End Set
  End Property

  Public Shared Property str__pub_active_user_list() As String
    Get
      Return str__prv_active_user_list
    End Get
    Set(ByVal value As String)
      str__prv_active_user_list = value
    End Set
  End Property

  Public Shared Property dte__pub_active_user_list_refresh() As DateTime
    Get
      Return dte__prv_active_user_list_refresh
    End Get
    Set(ByVal value As DateTime)
      dte__prv_active_user_list_refresh = value
    End Set
  End Property

  Public Shared Property str__pub_to_be_killed() As String
    Get
      Return str__prv_to_be_killed
    End Get
    Set(ByVal value As String)
      str__prv_to_be_killed = value
    End Set
  End Property

  Public Shared Property int__pub_active_user_count() As Int32
    Get
      Return int__prv_active_user_count
    End Get
    Set(ByVal value As Int32)
      int__prv_active_user_count = fnc_convert_expected_int32(value)
    End Set
  End Property

  Public Shared Property str__pub_connection_string__readwrite() As String
    Get
      Return str__prv_connection_string__readwrite
    End Get
    Set(ByVal value As String)
      str__prv_connection_string__readwrite = value
    End Set
  End Property


  Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)

    cls_utility.sub_load_meta_data()

    ' unique application wide key used to uniquely identify variables created by this application (former application("x") variables)
    '  variables are now stored in database by application 
    cls_variable.str_application_key__generated = Guid.NewGuid.ToString

    Exit Sub

  End Sub




End Class
