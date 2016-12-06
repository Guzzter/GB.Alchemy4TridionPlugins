<%@ Page Language="C#" Inherits="Tridion.Web.UI.Editors.CME.Views.Page" %>

<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Usage Report</title>
    <cc:tridionmanager runat="server" editor="CME">
            <dependencies runat="server">
                <dependency runat="server">Tridion.Web.UI.Editors.CME</dependency>
                <dependency runat="server">Tridion.Web.UI.Editors.CME.commands</dependency>
                <dependency runat="server">Alchemy.Resources.Libs.Jquery</dependency>
                <dependency runat="server">Alchemy.Plugins.${PluginName}.Resources.UsageReportPopupResourceGroup</dependency>
            </dependencies>
        </cc:tridionmanager>
    <link rel='shortcut icon' type='image/x-icon' href='${ImgUrl}favicon.png' />
</head>
<body>
    <div class="tabs">
        <div class="active"></div>
    </div>
    <div class="tab-body active">
    </div>
    <div class="controls">
        <div class="button disabled" id="open_item"><span class="text">Open</span></div>
        <div class="button disabled" id="where_used_using"><span class="text">Where Used</span></div>
        <div class="button disabled" id="pages_where_used"><span class="text">Pages Where Used</span></div>

        <div class="button disabled" id="go_to_item_location"><span class="text">Go To Location</span></div>
    </div>
</body>
</html>