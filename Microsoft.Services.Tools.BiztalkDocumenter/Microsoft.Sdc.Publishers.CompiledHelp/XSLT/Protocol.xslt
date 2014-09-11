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
			    <td width="175" class="TableTitle"><nobr>Application protocol:</nobr></td>
			    <td class="TableData"><xsl:value-of select="BizTalkBaseObject/ApplicationProtocol" /></td>
			</tr>
			<tr>
			    <td></td>
			    <td class="TableTitle"><nobr>Delete protected:</nobr></td>
			    <td class="TableData"><xsl:value-of select="BizTalkBaseObject/DeleteProtected" /></td>
			</tr>
			<tr>
			    <td></td>
			    <td class="TableTitle"><nobr>Uses adapter framework UI for receive handler configuration:</nobr></td>
			    <td class="TableData"><xsl:value-of select="BizTalkBaseObject/InitInboundProtocolContext" /></td>
			</tr>
			<tr>
			    <td></td>
			    <td class="TableTitle"><nobr>Uses adapter framework UI for send handler configuration:</nobr></td>
			    <td class="TableData"><xsl:value-of select="BizTalkBaseObject/InitOutboundProtocolContext" /></td>
			</tr>
			<tr>
			    <td></td>
			    <td class="TableTitle"><nobr>Uses adapter framework UI for receive location configuration:</nobr></td>
			    <td class="TableData"><xsl:value-of select="BizTalkBaseObject/InitReceiveLocationContext" /></td>
			</tr>
			<tr>
			    <td></td>
			    <td class="TableTitle"><nobr>Uses adapter framework UI for send port configuration:</nobr></td>
			    <td class="TableData"><xsl:value-of select="BizTalkBaseObject/InitTransmitLocationContext" /></td>
			</tr>
			<tr>
			    <td></td>
			    <td class="TableTitle"><nobr>Starts when the service starts:</nobr></td>
			    <td class="TableData"><xsl:value-of select="BizTalkBaseObject/InitTransmitterOnServiceStart" /></td>
			</tr>
			<tr>
			    <td></td>
			    <td class="TableTitle"><nobr>Receive handler of adapter is hosted in-process:</nobr></td>
			    <td class="TableData"><xsl:value-of select="BizTalkBaseObject/ReceiveIsCreatable" /></td>
			</tr>
			<tr>
			    <td></td>
			    <td class="TableTitle"><nobr>Requires a single instance per server:</nobr></td>
			    <td class="TableData"><xsl:value-of select="BizTalkBaseObject/RequireSingleInstance" /></td>
			</tr>
			<tr>
			    <td></td>
			    <td class="TableTitle"><nobr>Supports static handlers:</nobr></td>
			    <td class="TableData"><xsl:value-of select="BizTalkBaseObject/StaticHandlers" /></td>
			</tr>			
			<tr>
			    <td></td>
			    <td class="TableTitle"><nobr>Supports ordered delivery:</nobr></td>
			    <td class="TableData"><xsl:value-of select="BizTalkBaseObject/SupportsOrderedDelivery" /></td>
			</tr>
			<tr>
			    <td></td>
			    <td class="TableTitle"><nobr>Supports receive operations:</nobr></td>
			    <td class="TableData"><xsl:value-of select="BizTalkBaseObject/SupportsReceive" /></td>
			</tr>
			<tr>
			    <td></td>
			    <td class="TableTitle"><nobr>Supports request-response operations:</nobr></td>
			    <td class="TableData"><xsl:value-of select="BizTalkBaseObject/SupportsRequestResponse" /></td>
			</tr>
			<tr>
			    <td></td>
			    <td class="TableTitle"><nobr>Supports send operations:</nobr></td>
			    <td class="TableData"><xsl:value-of select="BizTalkBaseObject/SupportsSend" /></td>
			</tr>
			<tr>
			    <td></td>
			    <td class="TableTitle"><nobr>Supports the SOAP protocol:</nobr></td>
			    <td class="TableData"><xsl:value-of select="BizTalkBaseObject/SupportsSoap" /></td>
			</tr>
			<tr>
			    <td></td>
			    <td class="TableTitle"><nobr>Supports solicit-response operations:</nobr></td>
			    <td class="TableData"><xsl:value-of select="BizTalkBaseObject/SupportsSolicitResponse" /></td>
			</tr>			
			</table>
			
			<br/>
			
            <table class="TableData" border="0">
            <xsl:if test="count(BizTalkBaseObject/SendHandlers/SendHandler)>0">
                <tr>
                    <td width="10"></td>
                    <td width="175" valign="top" class="TableTitle">
                        <nobr>Send Handler(s):</nobr>
                    </td>
                    <td class="TableData" valign="top">
                        <xsl:for-each select="BizTalkBaseObject/SendHandlers/SendHandler">
                            <xsl:element name="A">
                                <xsl:attribute name="CLASS">TableData</xsl:attribute>
                                <xsl:attribute name="HREF">
                                    /Host/<xsl:value-of select="Id" />.htm
                                </xsl:attribute>
                                <xsl:value-of select="Name" />
                            </xsl:element>
                            <br/>
                        </xsl:for-each>
                    </td>
                </tr>
            </xsl:if>
			<xsl:if test="count(BizTalkBaseObject/ReceiveHandlers/ReceiveHandler)>0">
			    <tr>
			        <td width="10"></td>
			        <td width="175" valign="top" class="TableTitle"><nobr>Receive Handler(s):</nobr></td>
			        <td class="TableData" valign="top">
			            <xsl:for-each select="BizTalkBaseObject/ReceiveHandlers/ReceiveHandler">
			                <xsl:element name="A">
					            <xsl:attribute name="CLASS">TableData</xsl:attribute>
					            <xsl:attribute name="HREF">/Host/<xsl:value-of select="Id" />.htm</xsl:attribute>
					            <xsl:value-of select="Name" />
				            </xsl:element>
				            <br/>
                        </xsl:for-each>
			        </td>
			    </tr>
			</xsl:if>
            </table>
			
			<!-- Send Ports -->
			<xsl:if test="count(BizTalkBaseObject/SendPorts/SendPort)>0">	
			<BR/>
			    <Table class="TableData" border="0">
			    <tr>
			        <td width="10"></td>
			        <td class="PageTitle3" colspan="2">Send ports using this protocol</td>
			    </tr>
    			
			    <xsl:for-each select="BizTalkBaseObject/SendPorts/SendPort">
			        <tr>
			            <td></td>
			            <td><IMG SRC='../SendPortSmall.jpg' VALIGN="center" width="16" height="16"/></td>
			            <td align="left">
                            <xsl:element name="A">
                                <xsl:attribute name="CLASS">TableData</xsl:attribute>
                                <xsl:attribute name="HREF">
                                    /SendPort/<xsl:value-of select="Id" />.htm
                                </xsl:attribute>
                                <xsl:value-of select="Name" />
                            </xsl:element>
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
                    <td class="PageTitle3" colspan="2">Receive locations using this protocol</td>
                </tr>

                <xsl:for-each select="BizTalkBaseObject/ReceiveLocations/ReceiveLocation">
                    <tr>
                        <td></td>
                        <td>
                            <IMG SRC='../ReceivePortSmall.jpg' VALIGN="center" width="16" height="16"/>
                        </td>
                        <td align="left">
                            <xsl:element name="A">
                                <xsl:attribute name="CLASS">TableData</xsl:attribute>
                                <xsl:attribute name="HREF">
                                    /ReceivePort/<xsl:value-of select="Id" />.htm
                                </xsl:attribute>
                                <xsl:value-of select="Name" />
                            </xsl:element>
                        </td>
                    </tr>
                </xsl:for-each>
            </Table>
        </xsl:if>

        <BR/><BR/>
			
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
                        <img src="../Protocol.jpg" align="middle"/>
                    </nobr>
                </td>
                <td width="100%" background="../topBackground.jpg" valign="middle" CLASS="PageTitle">
                    <nobr>
                        Protocol : <xsl:value-of select="./BizTalkBaseObject/Name"/>
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
