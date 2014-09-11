<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

    <xsl:param name="GenDate" />
    <xsl:param name="DocVersion" />

    <xsl:output method="html" indent="no"/>

    <xsl:template match="/">
        <html>

            <HEAD>
                <title>Application:<xsl:value-of select ="./BizTalkBaseObject/Name"/></title>
                <link href="../CommentReport.css" type="text/css" rel="stylesheet"></link>
            </HEAD>

            <body>

                <xsl:call-template name="header" />

                <xsl:if test="count(BizTalkBaseObject/BackReferencedApplications/Application)>0">
                    <Table class="TableData" border="0">
                        <tr>
                            <td width="50"></td>
                            <td class="TableTitle" valign="top" colspan="2">Back Referenced Applications</td>
                        </tr>

                        <xsl:for-each select="BizTalkBaseObject/BackReferencedApplications/Application">
                            <tr>
                                <td></td>
                                <td>
                                    <xsl:element name="A">
                                        <xsl:attribute name="CLASS">TableData</xsl:attribute>
                                        <xsl:attribute name="HREF">
                                            /Application/<xsl:value-of select="Id" />.html
                                        </xsl:attribute>
                                        <xsl:value-of select="Name" />
                                    </xsl:element>
                                </td>
                            </tr>
                        </xsl:for-each>
                    </Table>
                    <br/>
                    <br/>
                </xsl:if>

                <xsl:if test="count(BizTalkBaseObject/ReferencedApplications/Application)>0">
                    <Table class="TableData" border="0">
                        <tr>
                            <td width="50"></td>
                            <td class="TableTitle" valign="top" colspan="2">Referenced Applications</td>
                        </tr>

                        <xsl:for-each select="BizTalkBaseObject/ReferencedApplications/Application">
                            <tr>
                                <td></td>
                                <td>
                                    <xsl:element name="A">
                                        <xsl:attribute name="CLASS">TableData</xsl:attribute>
                                        <xsl:attribute name="HREF">
                                            /Application/<xsl:value-of select="Id" />.html
                                        </xsl:attribute>
                                        <xsl:value-of select="Name" />
                                    </xsl:element>
                                </td>
                            </tr>
                        </xsl:for-each>
                    </Table>
                    <br/>
                    <br/>
                </xsl:if>

                <Table class="TableData" border="0">
                    <tr>
                        <td width="50"></td>
                        <td class="TableTitle" valign="top" colspan="2">Number of Deployed Assemblies:</td>
                        <td class="TableData" align="right" width="20">
                            <xsl:value-of select="count(BizTalkBaseObject/Assemblies/Assembly)" />
                        </td>
                    </tr>
                    <tr>
                        <td class="TableTitle" valign="top" colspan="4"></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td class="TableTitle" valign="top" colspan="2">Number of Maps:</td>
                        <td class="TableData" align="right" width="20">
                            <xsl:value-of select="count(BizTalkBaseObject/Maps/Map)" />
                        </td>
                    </tr>
                    <tr>
                        <td class="TableTitle" valign="top" colspan="4"></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td class="TableTitle" valign="top" colspan="2">Number of Orchestrations:</td>
                        <td class="TableData" align="right" width="20">
                            <xsl:value-of select="count(BizTalkBaseObject/Orchestrations/Orchestration)" />
                        </td>
                    </tr>
                    <tr>
                        <td class="TableTitle" valign="top" colspan="4"></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td class="TableTitle" valign="top" colspan="2">Number of Pipelines:</td>
                        <td class="TableData" align="right" width="20"></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td width="20"></td>
                        <td class="TableTitle" valign="top">Receive Pipelines:</td>
                        <td class="TableData" align="right" width="20">
                            <xsl:value-of select="count(BizTalkBaseObject/Pipelines/Pipeline[PipelineType='Receive'])" />
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td width="20"></td>
                        <td class="TableTitle" valign="top">Send Pipelines:</td>
                        <td class="TableData" align="right" width="20">
                            <xsl:value-of select="count(BizTalkBaseObject/Pipelines/Pipeline[PipelineType='Send'])" />
                        </td>
                    </tr>
                    <tr>
                        <td class="TableTitle" valign="top" colspan="4"></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td class="TableTitle" valign="top" colspan="2">Number of Receive Ports:</td>
                        <td class="TableData" align="right" width="20">
                            <xsl:value-of select="count(BizTalkBaseObject/ReceivePorts/ReceivePort)" />
                        </td>
                    </tr>
                    <tr>
                        <td class="TableTitle" valign="top" colspan="4"></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td class="TableTitle" valign="top" colspan="2">Number of Schema:</td>
                        <td class="TableData" align="right" width="20"></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td width="20"></td>
                        <td class="TableTitle" valign="top">Document Schema:</td>
                        <td class="TableData" align="right" width="20">
                            <xsl:value-of select="count(BizTalkBaseObject/Schemas/Schema[SchemaType='Document'])" />
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td width="20"></td>
                        <td class="TableTitle" valign="top">Envelope Schema:</td>
                        <td class="TableData" align="right" width="20">
                            <xsl:value-of select="count(BizTalkBaseObject/Schemas/Schema[SchemaType='Envelope'])" />
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td width="20"></td>
                        <td class="TableTitle" valign="top">Property Schema:</td>
                        <td class="TableData" align="right" width="20">
                            <xsl:value-of select="count(BizTalkBaseObject/Schemas/Schema[SchemaType='Property'])" />
                        </td>
                    </tr>
                    <tr>
                        <td class="TableTitle" valign="top" colspan="4"></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td class="TableTitle" valign="top" colspan="2">Number of Send Ports:</td>
                        <td class="TableData" align="right" width="20">
                            <xsl:value-of select="count(BizTalkBaseObject/SendPorts/SendPort)" />
                        </td>
                    </tr>
                    <tr>
                        <td class="TableTitle" valign="top" colspan="4"></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td class="TableTitle" valign="top" colspan="2">Number of Send Port Groups:</td>
                        <td class="TableData" align="right" width="20">
                            <xsl:value-of select="count(BizTalkBaseObject/SendPortGroups/SendPortGroup)" />
                        </td>
                    </tr>
                </Table>


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
                        <img src="../Assembly.gif" align="middle"/>
                    </nobr>
                </td>
                <td width="100%" background="../topBackground.jpg" valign="middle" CLASS="PageTitle">
                    <nobr>
                        Application : <xsl:value-of select="./BizTalkBaseObject/Name"/>
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
