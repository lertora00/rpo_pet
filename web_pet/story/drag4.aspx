<%@ Page Language="VB" AutoEventWireup="false" CodeFile="drag4.aspx.vb" Inherits="story_drag4" %>

<head runat="server">
    <title></title>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.4/themes/smoothness/jquery-ui.css"/>
    <script src="http://code.jquery.com/jquery-1.9.1.js"></script>
    <script src="http://code.jquery.com/ui/1.10.4/jquery-ui.js"></script>
    <script>
        $(function () {
           

            $("#sortable").sortable({
                stop: function (event, ui) {
                    var data = {
                        personlist: []
                    };
                    $("#sortable li").each(function () {
                        
                        data.personlist.push({
                            "Id": $(this).find(".spanId").html(),
                            "Name": $(this).find(".spanName").html(),
                            "Address": $(this).find(".spanAddress").html()
                        });

                    });
                    var finalval = JSON.stringify(data);
                    alert(finalval);
                    $.ajax({
                        type: "POST",
                        url: "drag4.aspx/UpdateMethod",
                        data: JSON.stringify(data),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            alert(msg.d);
                        }
                    });
                   
                }
            });
            $("#sortable").disableSelection();

        });
</script>

</head>
<body>
    <form id="form1" runat="server">
     
    <asp:Repeater ID="Repeater1" runat="server" >
        <HeaderTemplate>
            <ul id="sortable">
        </HeaderTemplate>
        <ItemTemplate>
            <li>
                <div >
                    The Person <span class="spanId"> <%#Container.DataItem("id")%></span>
                               <span class="spanName"><%#Container.DataItem("name")%></span>
                               <span class="spanAddress"><%#Container.DataItem("address")%></span>
                </div>
            </li>
        </ItemTemplate>
        <FooterTemplate>
            </ul>
        </FooterTemplate>
    </asp:Repeater>
     
    </form>
</body>