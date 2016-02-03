<%@ Control Language="c#" AutoEventWireup="false" Codebehind="DumpSQL.ascx.cs" Inherits="Taoqi._controls.DumpSQL" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%
// 10/08/2008   If we restrict the SQL code to admins, then we can deploy a debug build to a production machine. 
// 04/22/2009   We need to be able to show SQL for a non-admin so that we can debug the queries. 
if ( (bDebug && Taoqi.Security.isAdmin) || Sql.ToBoolean(Application["CONFIG.show_sql"]) )
{
	%>
	<script type="text/javascript">
		if ( sDebugSQL != 'undefined' )
		{
			document.write('<pre>');
			document.write(sDebugSQL);
			document.write('</pre>');
		}
	</script>
	<%
}
// 11/26/2010   When used inside an InlineScript, we need to have at least one script block. 
else
{
	%>
	<script type="text/javascript">
	</script>
	<%
}
%>

