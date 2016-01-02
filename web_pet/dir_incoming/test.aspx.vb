Imports ns_enterprise
Imports ns_enterprise.cls_utility

Partial Class dir_incoming_test
  Inherits System.Web.UI.Page

  Protected Sub form1_Load(sender As Object, e As EventArgs) Handles form1.Load

    Dim inst_sql As New cls_sql
    inst_sql.str_table_name = "tbl_log_message"
    inst_sql.str_operation = cls_sql.en_operation.insert.ToString
    inst_sql.sub_add_column("application_name", "a")
    inst_sql.sub_add_column("method_name", "b")
    inst_sql.sub_add_column("class_name", "c")
    inst_sql.sub_execute()

  End Sub

End Class
