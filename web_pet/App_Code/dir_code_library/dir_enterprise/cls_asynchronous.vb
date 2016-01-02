Imports ns_enterprise

Public Delegate Sub sub_thread_write(ByVal str__prm_sql As String)

Public NotInheritable Class cls_asynchronous

  Public Sub sub_execute_non_query__delegate(ByVal str__prm_sql As String)

    'For int_loop As Int32 = 1 To 350
    'System.Diagnostics.Debug.WriteLine("int_loop: " & int_loop)
    'Next

    ns_enterprise.cls_data_access_layer.sub_execute_non_query(str__prm_sql)

  End Sub

  Public Shared Sub sub_execute_non_query(ByVal str__prm_sql As String)

    ' This site is low volume and would rather have immediate access to data (data audit especially) than performance boost
    ' REMOVE next two lines to utilize async writing
    cls_data_access_layer.sub_execute_non_query(str__prm_sql)
    Exit Sub

    Dim inst_asynchronous As New cls_asynchronous
    Dim dlgt As New sub_thread_write(AddressOf inst_asynchronous.sub_execute_non_query__delegate)    ' Initiate the asynchronous call.    
    Dim ar As IAsyncResult = dlgt.BeginInvoke(str__prm_sql, Nothing, Nothing)

  End Sub

End Class
