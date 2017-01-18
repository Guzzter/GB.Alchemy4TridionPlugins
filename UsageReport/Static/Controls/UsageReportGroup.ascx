<%@ Control Language="C#" %>
<%@ Import Namespace="Tridion.Web.UI" %>

<!-- This is an example of a drop down ribbon toolbar button. You assign the filename of this control in the RibbonToolbar's
     Group property. -->
<c:ribbonbutton runat="server" commandname="UsageReport" isdropdownbutton="false" title="Usage Report" label="Usage Report" id="UsageReportBtn" />