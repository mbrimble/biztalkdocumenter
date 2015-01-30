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
                        <td width="175" class="TableTitle">
                            <nobr>Application:</nobr>
                        </td>
                        <td class="TableData">
                            <xsl:value-of select="BizTalkBaseObject/ApplicationName" />
                        </td>
                    </tr>
                    <tr>
                        <td width="10"></td>
                        <td width="175" class="TableTitle">
                            <nobr>Authentication Type:</nobr>
                        </td>
                        <td class="TableData">
                            <xsl:value-of select="BizTalkBaseObject/AuthenticationType" />
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td class="TableTitle">
                            <nobr>Tracking Type:</nobr>
                        </td>
                        <td class="TableData">
                            <xsl:choose>
                                <xsl:when test="string-length(BizTalkBaseObject/TrackingType)>0">
                                    <xsl:value-of select="BizTalkBaseObject/TrackingType" />
                                </xsl:when>
                                <xsl:otherwise>
                                    <xsl:text>&lt;None&gt;</xsl:text>
                                </xsl:otherwise>
                            </xsl:choose>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td class="TableTitle">
                            <nobr>Two-Way:</nobr>
                        </td>
                        <td class="TableData">
                            <xsl:value-of select="BizTalkBaseObject/TwoWay" />
                        </td>
                    </tr>
                    <xsl:if test="string-length(BizTalkBaseObject/CustomDescription)>0">
                        <tr>
                            <td></td>
                            <td class="TableTitle" valign="top">Description:</td>
                            <td class="TableData">
                                <xsl:value-of select="BizTalkBaseObject/CustomDescription" />
                            </td>
                        </tr>
                    </xsl:if>
                </table>

                <!-- Locations -->
                <xsl:if test="count(BizTalkBaseObject/ReceiveLocations/ReceiveLocation)>0">
                    <BR/>
                    <Table class="TableData">
                        <tr>
                            <td width="10"></td>
                            <td class="PageTitle3" colspan="4">Receive Locations</td>
                        </tr>

                        <xsl:for-each select="BizTalkBaseObject/ReceiveLocations/ReceiveLocation">
                            <tr>
                                <td></td>
                                <td>
                                    <IMG SRC='../ReceivePortSmall.jpg' VALIGN="center" width="16" height="16"/>
                                </td>
                                <td colspan="2" width="149" class="TableTitle">
                                    <xsl:value-of select="./Name" />
                                </td>
                            </tr>
                          <!-- Added by PCA 2015-01-03 -->
                          <xsl:if test="string-length(./CustomDescription)>0">
                            <tr>
                              <td></td>
                              <td></td>
                              <td width="149">Description:</td>
                              <td>
                                <xsl:value-of select="./CustomDescription" />
                              </td>
                            </tr>
                          </xsl:if>
                          <!-- End Added by PCA 2015-01-03 -->
                          <tr>
                                <td></td>
                                <td></td>
                                <td width="149">Address:</td>
                                <td>
                                    <xsl:value-of select="Address" />
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td>Transport Protocol:</td>
                                <td>
                                    <nobr>
                                        <xsl:choose>
                                            <xsl:when test="string-length(TransportProtocol/@id)>0">
                                                <xsl:element name="A">
                                                    <xsl:attribute name="CLASS">TableData</xsl:attribute>
                                                    <xsl:attribute name="HREF">
                                                        /Protocol/<xsl:value-of select="TransportProtocol/@id" />.htm
                                                    </xsl:attribute>
                                                    <xsl:value-of select="TransportProtocol" />
                                                </xsl:element>
                                            </xsl:when>
                                            <xsl:otherwise>
                                                <xsl:value-of select="TransportProtocol" />
                                            </xsl:otherwise>
                                        </xsl:choose>
                                    </nobr>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <!--<td>Receive Pipeline:</td>
                                <td>
                                    <xsl:element name="A">
                                        <xsl:attribute name="CLASS">TableData</xsl:attribute>
                                        <xsl:attribute name="HREF">
                                            /Pipeline/<xsl:value-of select="ReceivePipeline/Id" />.htm
                                        </xsl:attribute>
                                        <xsl:value-of select="ReceivePipeline/Name" />
                                    </xsl:element>
                                </td> Changed by Colin Dijkgraaf 20140210-->
                              <td>Receive Pipeline:</td>
                              <td>
                                <xsl:choose>
                                  <xsl:when test="string-length(ReceivePipeline/Id)>0">
                                    <xsl:element name="A">
                                      <xsl:attribute name="CLASS">TableData</xsl:attribute>
                                      <xsl:attribute name="HREF">
                                        /Pipeline/<xsl:value-of select="ReceivePipeline/Id" />.htm
                                      </xsl:attribute>
                                      <xsl:value-of select="ReceivePipeline/Name" />
                                    </xsl:element>
                                  </xsl:when>
                                  <xsl:otherwise>
                                    <xsl:value-of select="ReceivePipeline/Name" />
                                  </xsl:otherwise>
                                </xsl:choose>
                              </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td>Receive Handler:</td>
                                <td>
                                    <xsl:element name="A">
                                        <xsl:attribute name="CLASS">TableData</xsl:attribute>
                                        <xsl:attribute name="HREF">
                                            /Host/<xsl:value-of select="ReceiveHandler/Id" />.htm
                                        </xsl:attribute>
                                        <xsl:value-of select="ReceiveHandler/Name" />
                                    </xsl:element>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td>Send Pipeline:</td>
                                <td>
                                    <xsl:choose>
                                        <xsl:when test="string-length(SendPipeline/Id)>0">
                                            <xsl:element name="A">
                                                <xsl:attribute name="CLASS">TableData</xsl:attribute>
                                                <xsl:attribute name="HREF">
                                                    /Pipeline/<xsl:value-of select="SendPipeline/Id" />.htm
                                                </xsl:attribute>
                                                <xsl:value-of select="SendPipeline/Name" />
                                            </xsl:element>
                                        </xsl:when>
                                        <xsl:otherwise>
                                            <xsl:value-of select="SendPipeline/Name" />
                                        </xsl:otherwise>
                                    </xsl:choose>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td class="TableData">
                                    <nobr>
                                        <xsl:choose>
                                            <xsl:when test="DataSpecified='true'">
                                                <xsl:element name="A">
                                                    <xsl:attribute name="CLASS">TableData</xsl:attribute>
                                                    <xsl:attribute name="HREF">
                                                        /ReceivePort/<xsl:value-of select="Id" />Data.xml
                                                    </xsl:attribute>
                                                    Custom Data
                                                </xsl:element>
                                            </xsl:when>
                                            <xsl:otherwise>No Custom Data Specified</xsl:otherwise>
                                        </xsl:choose>
                                    </nobr>
                                </td>
                                <td class="TableData"></td>
                            </tr>
                            <tr>
                                <td>
                                    <br/>
                                </td>
                            </tr>
                        </xsl:for-each>
                    </Table>
                </xsl:if>

                <!-- Outbound Maps -->
                <xsl:if test="count(BizTalkBaseObject/OutboundMaps/Transform)>0">
                    <BR/>
                    <Table class="TableData">
                        <tr>
                            <td width="10"></td>
                            <td class="PageTitle3" colspan="5">Outbound Maps</td>
                        </tr>

                        <xsl:for-each select="BizTalkBaseObject/OutboundMaps/Transform">
                            <tr>
                                <td width="10"></td>
                                <td>
                                    <IMG SRC='../MapSmall.jpg' VALIGN="center" width="16" height="16"/>
                                </td>
                                <td>
                                    <xsl:element name="A">
                                        <xsl:attribute name="CLASS">TableData</xsl:attribute>
                                        <xsl:attribute name="HREF">
                                            /Map/<xsl:value-of select="Id" />.html
                                        </xsl:attribute>
                                        <xsl:value-of select="Name" />
                                    </xsl:element>
                                    <br/>
                                    <br/>
                                </td>
                            </tr>
                        </xsl:for-each>

                    </Table>
                </xsl:if>

                <!-- Inbound Maps -->
                <xsl:if test="count(BizTalkBaseObject/InboundMaps/Transform)>0">
                    <BR/>
                    <Table class="TableData">
                        <tr>
                            <td width="10"></td>
                            <td class="PageTitle3" colspan="5">Inbound Maps</td>
                        </tr>

                        <xsl:for-each select="BizTalkBaseObject/InboundMaps/Transform">
                            <tr>
                                <td width="10"></td>
                                <td>
                                    <IMG SRC='../MapSmall.jpg' VALIGN="center" width="16" height="16"/>
                                </td>
                                <td>
                                    <xsl:element name="A">
                                        <xsl:attribute name="CLASS">TableData</xsl:attribute>
                                        <xsl:attribute name="HREF">
                                            /Map/<xsl:value-of select="Id" />.html
                                        </xsl:attribute>
                                        <xsl:value-of select="Name" />
                                    </xsl:element>
                                    <br/>
                                    <br/>
                                </td>
                            </tr>
                        </xsl:for-each>

                    </Table>
                </xsl:if>

                <!-- Bound Orchestrations -->
                <xsl:if test="count(BizTalkBaseObject/BoundOrchestrations/Orchestration)>0">
                    <BR/>
                    <Table class="TableData" border="0">
                        <tr>
                            <td width="10"></td>
                            <td class="PageTitle3" colspan="2">Orchestrations bound to this receive port</td>
                        </tr>

                        <xsl:for-each select="BizTalkBaseObject/BoundOrchestrations/Orchestration">
                            <tr>
                                <td></td>
                                <td>
                                    <IMG SRC='../OrchestrationSmall.jpg' VALIGN="center" width="16" height="16"/>
                                </td>
                                <td>
                                    <xsl:choose>
                                        <xsl:when test="string-length(Id)>0">
                                            <xsl:element name="A">
                                                <xsl:attribute name="CLASS">TableData</xsl:attribute>
                                                <xsl:attribute name="HREF">
                                                    /Orchestration/<xsl:value-of select="Id" />.htm
                                                </xsl:attribute>
                                                <xsl:value-of select="Name" />
                                            </xsl:element>
                                        </xsl:when>
                                        <xsl:otherwise>
                                            <xsl:value-of select="Name" />
                                        </xsl:otherwise>
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
                        <img src="../ReceivePort.gif" align="middle"/>
                    </nobr>
                </td>
                <td width="100%" background="../topBackground.jpg" valign="middle" CLASS="PageTitle">
                    <nobr>
                        Receive Port : <xsl:value-of select="./BizTalkBaseObject/Name"/>
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
        <BR/>
        <BR/>
        <BR/>
        <BR/>
        <HR CLASS="Rule"/>
        <P CLASS="Copyright">
            Generated on: <xsl:value-of select="$GenDate"/><BR/>Microsoft.Services.Tools.BiztalkDocumenter version: <xsl:value-of select="$DocVersion"/>
        </P>
        <BR/>
    </xsl:template>


</xsl:stylesheet>
