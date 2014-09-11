<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">	

<xsl:param name="GenDate" />
<xsl:param name="DocVersion" />
 	      
<xsl:output method="html" indent="no"/>

	<xsl:template match="/">
		<html>
			<HEAD>
				<link href="../CommentReport.css" type="text/css" rel="stylesheet"></link>
			</HEAD>
			
			<body>

            <xsl:call-template name="header" /> 

            <table class="TableData" border="0">
			<tr>
			    <td width="10"></td>
			    <td width="175" class="TableTitle"><nobr>Group Name:</nobr></td>
			    <td class="TableData"><xsl:value-of select="BizTalkBaseObject/GroupName" /></td>
			</tr>
			<tr>
			    <td></td>
			    <td class="TableTitle"><nobr>Host Tracking Enabled:</nobr></td>
			    <td class="TableData"><xsl:value-of select="BizTalkBaseObject/HostTrackingEnabled" /></td>
			</tr>
			<tr>
			    <td></td>
			    <td class="TableTitle"><nobr>Authentication Trusted:</nobr></td>
			    <td class="TableData"><xsl:value-of select="BizTalkBaseObject/AuthTrusted" /></td>
			</tr>
			<tr>
			    <td></td>
			    <td class="TableTitle"><nobr>Default Host:</nobr></td>
			    <td class="TableData"><xsl:value-of select="BizTalkBaseObject/DefaultHost" /></td>
			</tr>
			<tr>
			    <td></td>
			    <td class="TableTitle"><nobr>Host Type:</nobr></td>
			    <td class="TableData">
			        <xsl:choose>
			            <xsl:when test="BizTalkBaseObject/Isolated='true'">Isolated</xsl:when>
			            <xsl:otherwise>In-Process</xsl:otherwise>
			        </xsl:choose>
			    </td>
			</tr>		
			<xsl:if test="string-length(BizTalkBaseObject/CustomDescription)>0">
			<tr>
			    <td></td>
			    <td class="TableTitle" valign="top">Description:</td>
			    <td class="TableData"><xsl:value-of select="BizTalkBaseObject/CustomDescription" /></td>
			</tr>
			</xsl:if>
			</table>
			
			<!-- Server Instancess -->
			<xsl:if test="count(BizTalkBaseObject/HostInstances/HostInstance)>0">	
			<BR/>
			    <Table class="TableData" border="0">
			    <tr>
			        <td width="10"></td>
			        <td class="PageTitle3" colspan="2">Server instances configured in this host</td>
			    </tr>
    			
			    <xsl:for-each select="BizTalkBaseObject/HostInstances/HostInstance">
			        <tr>
			            <td></td>
			            <td>
			                <xsl:value-of select="Name" /> <xsl:text> </xsl:text>
			                
			                <xsl:choose>
			                    <xsl:when test="Disabled='true'">(Disabled)</xsl:when>
				                <xsl:otherwise>(Enabled)</xsl:otherwise>
				            </xsl:choose>
				            
				            <xsl:if test="string-length(Logon)>0"><xsl:text> </xsl:text> - running as <xsl:value-of select="Logon" /></xsl:if>
				            
				        </td>
			            <td></td>
			        </tr>
			    </xsl:for-each>
			    </Table>
			</xsl:if>
			
			<!-- Bound Orchestrations -->
			<xsl:if test="count(BizTalkBaseObject/HostedOrchestrations/Orchestration)>0">	
			<BR/>
			    <Table class="TableData" border="0">
			    <tr>
			        <td width="10"></td>
			        <td class="PageTitle3" colspan="2">Orchestrations running in this host</td>
			    </tr>
    			
			    <xsl:for-each select="BizTalkBaseObject/HostedOrchestrations/Orchestration">
			        <tr>
			            <td></td>
			            <td><IMG SRC='../OrchestrationSmall.jpg' VALIGN="center" width="16" height="16"/></td>
			            <td>
                            <xsl:element name="A">
                                <xsl:attribute name="CLASS">TableData</xsl:attribute>
                                <xsl:attribute name="HREF">
                                    /Orchestration/<xsl:value-of select="Id" />.htm
                                </xsl:attribute>
                                <xsl:value-of select="Name" />
                            </xsl:element>  
                        </td>
                    </tr>
                </xsl:for-each>
            </Table>
        </xsl:if>

        <!-- Hosted Receive Locations -->
			<xsl:if test="count(BizTalkBaseObject/HostedReceiveLocations/ReceiveLocation)>0">	
			<BR/>
			    <Table class="TableData" border="0">
			    <tr>
			        <td width="10"></td>
			        <td class="PageTitle3" colspan="2">Receive locations running in this host</td>
			    </tr>
    			
			    <xsl:for-each select="BizTalkBaseObject/HostedReceiveLocations/ReceiveLocation">
			        <tr>
			            <td></td>
			            <td><IMG SRC='../ReceivePortSmall.jpg' VALIGN="center" width="16" height="16"/></td>
			            <td>
			                <xsl:element name="A">
					        <xsl:attribute name="CLASS">TableData</xsl:attribute>
					        <xsl:attribute name="HREF">/ReceivePort/<xsl:value-of select="Id" />.htm</xsl:attribute>
					        <xsl:value-of select="Name" />
				            </xsl:element>
				        </td>
			        </tr>
			    </xsl:for-each>
			    </Table>    
			</xsl:if>
			
			
			<!-- Hosted Send Ports -->
			<xsl:if test="count(BizTalkBaseObject/HostedSendPorts/SendPort)>0">	
			<BR/>
			    <Table class="TableData" border="0">
			    <tr>
			        <td width="10"></td>
			        <td class="PageTitle3" colspan="2">Send ports running in this host</td>
			    </tr>
    			
			    <xsl:for-each select="BizTalkBaseObject/HostedSendPorts/SendPort">
			        <tr>
			            <td></td>
			            <td><IMG SRC='../SendPortSmall.jpg' VALIGN="center" width="16" height="16"/></td>
			            <td>
			                <xsl:element name="A">
					        <xsl:attribute name="CLASS">TableData</xsl:attribute>
					        <xsl:attribute name="HREF">/SendPort/<xsl:value-of select="Id" />.htm</xsl:attribute>
					        <xsl:value-of select="Name" />
				            </xsl:element>
				        </td>
			        </tr>
			    </xsl:for-each>
			    </Table>
			    <BR/><BR/>
			</xsl:if>
						
			<xsl:call-template name="footer" /> 
			
			</body>
		</html>
	</xsl:template>

    <xsl:template name="header">

        <table width="100%" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td width="10" background="../topBackground.jpg">
                    <nobr>
                        <img src="../topSpacer.gif"/>
                    </nobr>
                </td>
                <td width="10" background="../topBackground.jpg" valign="middle">
                    <nobr>
                        <img src="../Host.gif" align="middle"/>
                    </nobr>
                </td>
                <td width="100%" background="../topBackground.jpg" valign="middle" CLASS="PageTitle">
                    <nobr>
                        Host : <xsl:value-of select="./BizTalkBaseObject/Name"/>
                    </nobr>
                </td>
                <td background="../topBackground.jpg" valign="middle" align="right">
                    <IMG SRC="../topRight.jpg" ALIGN="middle"/>
                </td>
            </tr>
        </table>
        <BR/>

    </xsl:template>
    
	<xsl:template name="footer">
		<BR/><BR/><BR/><BR/><HR CLASS="Rule"/><P CLASS="Copyright">Generated on: <xsl:value-of select="$GenDate"/><BR/>Microsoft.Services.Tools.BiztalkDocumenter version: <xsl:value-of select="$DocVersion"/></P><BR/>		
	</xsl:template>	


</xsl:stylesheet>
