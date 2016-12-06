<%@ Page Language="C#" %>

<%-- TRIDION: <%@ Page Language="C#" Inherits="Tridion.Web.UI.Editors.CME.Views.Page" %> --%>
<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Usage Report</title>
    <%--
    <cc:tridionmanager runat="server" editor="CME">
            <dependencies runat="server">
                <dependency runat="server">Tridion.Web.UI.Editors.CME</dependency>
                <dependency runat="server">Tridion.Web.UI.Editors.CME.commands</dependency>
                <dependency runat="server">Alchemy.Resources.Libs.Jquery</dependency>
                <dependency runat="server">Alchemy.Plugins.${PluginName}.Resources.UsageReportPopupResourceGroup</dependency>
            </dependencies>
        </cc:tridionmanager> --%>
    <!-- remove this line and uncomment above for use with Alchemy -->
    <link type="text/css" rel="stylesheet" href="tridion.css" />
</head>
<body>
    <div class="tabs">
        <div class="active">test</div>
    </div>
    <div class="tab-body active" data-tcm="301-46173-64">
        <h2>Zoekresultaat is used by:</h2>
        Zoekresultaat is not being used by any items
        <h2>Zoekresultaat uses:</h2>
        <div class="usedItems results">
            <div class="headings">
                <div class="icon">&nbsp;</div>
                <div class="name">Name</div>
                <div class="path">Path</div>
                <div class="id">ID</div>
            </div>
            <div class="item">
                <div class="icon"></div>
                <div class="name">Meta_Page</div>
                <div class="path">\000 System Parent\Building Blocks\System\Schema\Metadata</div>
                <div class="id">tcm:301-2593-8</div>
            </div>
            <div class="item">
                <div class="icon" style="background-image: url(/WebUI/Editors/CME/Themes/Carbon2/icon_v7.1.0.66.627_.png?name=T32L0P0S4&amp;size=16)"></div>
                <div class="name">SearchResult Google</div>
                <div class="path">\001 Internet Parent\Building Blocks\System\Templates\Dynamic Visual Blocks</div>
                <div class="id">tcm:301-60279-32</div>
            </div>
            <div class="item">
                <div class="icon" style="background-image: url(/WebUI/Editors/CME/Themes/Carbon2/icon_v7.1.0.66.627_.png?name=T16L0P1&amp;size=16)"></div>
                <div class="name">GoogleSearch - Particulier</div>
                <div class="path">\100 Essent NL\Building Blocks\System\GoogleSearch</div>
                <div class="id">tcm:301-86717</div>
            </div>
            <div class="item">
                <div class="icon" style="background-image: url(/WebUI/Editors/CME/Themes/Carbon2/icon_v7.1.0.66.627_.png?name=T1024L0P0&amp;size=16)"></div>
                <div class="name">article</div>
                <div class="path">\000 System Parent\Open Graph object types</div>
                <div class="id">tcm:301-113074-1024</div>
            </div>
        </div>
    </div>
    <div class="controls">
        <div class="button disabled" id="open_item"><span class="text">Open</span></div>
        <div class="button disabled" id="where_used_using"><span class="text">Where Used</span></div>
        <div class="button disabled" id="pages_where_used"><span class="text">Pages Where Used</span></div>

        <div class="button disabled" id="go_to_item_location"><span class="text">Go To Location</span></div>
    </div>
</body>
</html>