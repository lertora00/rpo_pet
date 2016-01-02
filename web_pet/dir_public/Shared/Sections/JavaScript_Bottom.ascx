<%@ Control Language="VB" AutoEventWireup="false" CodeFile="JavaScript_Bottom.ascx.vb" Inherits="dir_public_Shared_Sections_JavaScript_Bottom" %>
<script src="https://code.jquery.com/jquery-1.10.2.min.js" type="text/javascript"></script>
<script src="../../dir_public/Content/Scripts/bootstrap.min.js" type="text/javascript"></script>
<script src="../../dir_public/Content/Scripts/jquery.theme.functions.js" type="text/javascript"></script>
<script src="../../dir_public/Content/Scripts/site.scripts.js" type="text/javascript"></script>

    <script>
        $('.tooltip-demo').tooltip({
            selector: "[data-toggle=tooltip]",
            container: "body"
        });
        $("[data-toggle=popover]").popover();
    </script>
