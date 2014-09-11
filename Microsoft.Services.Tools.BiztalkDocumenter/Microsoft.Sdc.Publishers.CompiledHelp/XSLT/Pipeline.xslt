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

            <table class="TableData">
            <tr>
                <td width="10"></td>
                <td width="125" class="TableTitle"><nobr>Application:</nobr></td>
                <td class="TableData"><xsl:value-of select="BizTalkBaseObject/ApplicationName" /> </td>
            </tr>
			<tr>
			    <td width="10"></td>
			    <td width="125" class="TableTitle"><nobr>Pipeline Type:</nobr></td>
			    <td class="TableData"><xsl:value-of select="BizTalkBaseObject/PipelineType" /></td>
			</tr>
                <tr>
                    <td width="10"></td>
                    <td class="TableTitle">
                        <nobr>Parent Assembly:</nobr>
                    </td>
                    <td class="TableData">
                        <nobr>
                            <xsl:choose>
                                <xsl:when test="string-length(BizTalkBaseObject/ParentAssembly/Id)>0">
                                    <xsl:element name="A">
                                        <xsl:attribute name="CLASS">TableData</xsl:attribute>
                                        <xsl:attribute name="HREF">
                                            /Assembly/<xsl:value-of select="BizTalkBaseObject/ParentAssembly/Id" />.htm
                                        </xsl:attribute>
                                        <xsl:value-of select="BizTalkBaseObject/ParentAssembly/Name" />
                                    </xsl:element>
                                </xsl:when>
                                <xsl:otherwise>
                                    <xsl:value-of select="BizTalkBaseObject/ParentAssembly/Name" />
                                </xsl:otherwise>
                            </xsl:choose>
                        </nobr>
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
			
			${PROCESS_FLOW}
			
			<!-- Send Ports -->
			<xsl:if test="count(BizTalkBaseObject/SendPorts/SendPort)>0">	
			<BR/>
			    <Table class="TableData" border="0">
			    <tr>
			        <td width="10"></td>
			        <td class="PageTitle3" colspan="2">Send ports using this pipeline</td>
			    </tr>
    			
			    <xsl:for-each select="BizTalkBaseObject/SendPorts/SendPort">
			        <tr>
			            <td></td>
			            <td><IMG SRC='../SendPortSmall.jpg' VALIGN="center" width="16" height="16"/></td>
			            <td>
			                <xsl:choose>
			                    <xsl:when test="string-length(@id)>0">
			                        <xsl:element name="A">
					                <xsl:attribute name="CLASS">TableData</xsl:attribute>
					                <xsl:attribute name="HREF">/SendPort/<xsl:value-of select="@id" />.htm</xsl:attribute>
					                <xsl:value-of select="text()" />
				                    </xsl:element>
				                </xsl:when>
				                <xsl:otherwise><xsl:value-of select="text()" /></xsl:otherwise>
				            </xsl:choose>
				        </td>
			        </tr>
			    </xsl:for-each>
			    </Table>
			</xsl:if>
			
			<!-- Receive Ports -->
			<xsl:if test="count(BizTalkBaseObject/ReceiveLocations/ReceiveLocation)>0">	
			<BR/>
			    <Table class="TableData" border="0">
			    <tr>
			        <td width="10"></td>
			        <td class="PageTitle3" colspan="2">Receive locations using this pipeline</td>
			    </tr>
    			
			    <xsl:for-each select="BizTalkBaseObject/ReceiveLocations/ReceiveLocation">
			        <tr>
			            <td></td>
			            <td><IMG SRC='../ReceivePortSmall.jpg' VALIGN="center" width="16" height="16"/></td>
			            <td>
			                <xsl:choose>
			                    <xsl:when test="string-length(@ParentPortId)>0">
			                        <xsl:element name="A">
					                <xsl:attribute name="CLASS">TableData</xsl:attribute>
					                <xsl:attribute name="HREF">/ReceivePort/<xsl:value-of select="@ParentPortId" />.htm</xsl:attribute>
					                <xsl:value-of select="text()" />
				                    </xsl:element>
				                </xsl:when>
				                <xsl:otherwise><xsl:value-of select="text()" /></xsl:otherwise>
				            </xsl:choose>
				        </td>
			        </tr>
			    </xsl:for-each>
			    </Table>
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
                        <img src="../Pipeline.gif" align="middle"/>
                    </nobr>
                </td>
                <td width="100%" background="../topBackground.jpg" valign="middle" CLASS="PageTitle">
                    <nobr>
                        Pipeline : <xsl:value-of select="./BizTalkBaseObject/Name"/>
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
