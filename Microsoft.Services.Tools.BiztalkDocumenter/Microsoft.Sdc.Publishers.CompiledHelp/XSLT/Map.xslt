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
                <td class="TableData"><xsl:value-of select="BizTalkBaseObject/ApplicationName" /></td>
            </tr>
			<tr>
			    <td width="10"></td>
			    <td width="125" class="TableTitle"><nobr>Parent Assembly:</nobr></td>

                <td>
                    <xsl:element name="A">
                        <xsl:attribute name="CLASS">TableData</xsl:attribute>
                        <xsl:attribute name="HREF">
                            /Assembly/<xsl:value-of select="BizTalkBaseObject/ParentAssembly/Id" />.htm
                        </xsl:attribute>
                        <nobr><xsl:value-of select="BizTalkBaseObject/ParentAssembly/Name" /></nobr>
                    </xsl:element>
                </td>
			</tr>
			<tr>
			    <td width="10"></td>
			    <td width="125" class="TableTitle"><nobr>Source Schema:</nobr></td>
			    <td class="TableData">
                    <xsl:element name="A">
                        <xsl:attribute name="CLASS">TableData</xsl:attribute>
                        <xsl:attribute name="HREF">
                            /Schema/<xsl:value-of select="BizTalkBaseObject/SourceSchema/Id" />.htm
                        </xsl:attribute>
                        <nobr>
                            <xsl:value-of select="BizTalkBaseObject/SourceSchema/Name" />
                        </nobr>
                    </xsl:element>
                </td>
			</tr>
			<tr>
			    <td></td>
			    <td class="TableTitle"><nobr>Target Schema:</nobr></td>
			    <td class="TableData">
                    <xsl:element name="A">
                        <xsl:attribute name="CLASS">TableData</xsl:attribute>
                        <xsl:attribute name="HREF">
                            /Schema/<xsl:value-of select="BizTalkBaseObject/TargetSchema/Id" />.htm
                        </xsl:attribute>
                        <nobr>
                            <xsl:value-of select="BizTalkBaseObject/TargetSchema/Name" />
                        </nobr>
                    </xsl:element>
                </td>
			</tr>		
			<xsl:if test="string-length(BizTalkBaseObject/CustomDescription)>0">
			<tr>
			    <td></td>
			    <td class="TableTitle" valign="top">Description:</td>
			    <td class="TableData"><xsl:value-of select="BizTalkBaseObject/CustomDescription" /></td>
			</tr>
			</xsl:if>
			<tr>
			    <td width="10" colspan="2"></td>
			    <td><xsl:element name="A">
					<xsl:attribute name="CLASS">TableData</xsl:attribute>
					<xsl:attribute name="HREF">/Map/<xsl:value-of select="BizTalkBaseObject/Id" />.xml</xsl:attribute>View Map Source</xsl:element><br/><br/></td>
			</tr>
			</table>
			
			<!-- Send Ports -->
			<xsl:if test="count(BizTalkBaseObject/SendPorts/SendPort)>0">	
			<BR/>
			    <Table class="TableData" border="0">
			    <tr>
			        <td width="10"></td>
			        <td class="PageTitle3" colspan="2">Send ports using this map</td>
			    </tr>
    			
			    <xsl:for-each select="BizTalkBaseObject/SendPorts/SendPort">
			        <tr>
			            <td></td>
			            <td><IMG SRC='../SendPortSmall.jpg' VALIGN="center" width="16" height="16"/></td>
			            <td align="left"><xsl:element name="A">
					        <xsl:attribute name="CLASS">TableData</xsl:attribute>
					        <xsl:attribute name="HREF">/SendPort/<xsl:value-of select="Id" />.htm</xsl:attribute>
					        <xsl:value-of select="text()" />
                            <xsl:value-of select="Name" />
				            </xsl:element></td>
			        </tr>
			    </xsl:for-each>
			    </Table>
			</xsl:if>
			
			<!-- Receive Ports -->
			<xsl:if test="count(BizTalkBaseObject/ReceivePorts/ReceivePort)>0">	
			<BR/>
			    <Table class="TableData" border="0">
			    <tr>
			        <td width="10"></td>
			        <td class="PageTitle3" colspan="2">Receive ports using this map</td>
			    </tr>
    			
			    <xsl:for-each select="BizTalkBaseObject/ReceivePorts/ReceivePort">
			        <tr>
			            <td></td>
			            <td><IMG SRC='../ReceivePortSmall.jpg' VALIGN="center" width="16" height="16"/></td>
			            <td align="left"><xsl:element name="A">
					        <xsl:attribute name="CLASS">TableData</xsl:attribute>
					        <xsl:attribute name="HREF">/ReceivePort/<xsl:value-of select="Id" />.htm</xsl:attribute>
					        <xsl:value-of select="Name" />
				            </xsl:element></td>
			        </tr>
			    </xsl:for-each>
			    </Table>
			</xsl:if>
			
			<!-- Orchestrations -->
			<xsl:if test="count(BizTalkBaseObject/Orchestrations/Orchestration)>0">	
			<BR/>
			    <Table class="TableData" border="0">
			    <tr>
			        <td width="10"></td>
			        <td class="PageTitle3" colspan="2">Orchestrations using this map</td>
			    </tr>
    			
			    <xsl:for-each select="BizTalkBaseObject/Orchestrations/Orchestration">
			        <tr>
			            <td></td>
			            <td><IMG SRC='../OrchestrationSmall.jpg' VALIGN="center" width="16" height="16"/></td>
			            <td align="left"><xsl:element name="A">
					        <xsl:attribute name="CLASS">TableData</xsl:attribute>
					        <xsl:attribute name="HREF">/Orchestration/<xsl:value-of select="Id" />.htm</xsl:attribute>
					        <xsl:value-of select="Name" />
				            </xsl:element></td>
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
                        <img src="../Map.gif" align="middle"/>
                    </nobr>
                </td>
                <td width="100%" background="../topBackground.jpg" valign="middle" CLASS="PageTitle">
                    <nobr>
                        Map : <xsl:value-of select="./BizTalkBaseObject/Name"/>
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
