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

                <!-- Aliases -->
                <xsl:if test="count(BizTalkBaseObject/Aliases/Alias)>0">
                    <BR/>

                    <Table class="TableData" border="0">
                        <tr>
                            <td width="10"></td>
                            <td class="PageTitle3" colspan="4">Aliases</td>
                        </tr>
                        <!--
                        <tr>
                            <td width="10" colspan="2"></td>
                            <td class="TableTitle">
                                <nobr>Name</nobr>
                            </td>
                            <td class="TableTitle">
                                <nobr>AutoCreated</nobr>
                            </td>
                            <td class="TableTitle">
                                <nobr>Qualifier</nobr>
                            </td>
                        </tr>
                        -->

                        <xsl:for-each select="BizTalkBaseObject/Aliases/Alias">
                            <tr>
                                <td></td>
                                <td>
                                    <IMG SRC='../Host.gif' VALIGN="center" width="16" height="16"/>
                                </td>
                                <td>
                                    <xsl:value-of select="Value" />
                                </td>
                                <!--
                                <td>
                                    <xsl:value-of select="IsAutoCreated" />
                                </td>
                                <td>
                                    <xsl:value-of select="Qualifier" />
                                </td>
                                -->
                            </tr>
                        </xsl:for-each>
                    </Table>

                </xsl:if>



                <!-- Send Ports -->
                <xsl:if test="count(BizTalkBaseObject/SendPorts/SendPort)>0">
                    <BR/>

                    <Table class="TableData" border="0">
                        <tr>
                            <td width="10"></td>
                            <td class="PageTitle3" colspan="2">Send Ports</td>
                        </tr>

                        <xsl:for-each select="BizTalkBaseObject/SendPorts/SendPort">
                            <tr>
                                <td></td>
                                <td>
                                    <IMG SRC='../SendPortSmall.jpg' VALIGN="center" width="16" height="16"/>
                                </td>
                                <td>
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
                        Party : <xsl:value-of select="./BizTalkBaseObject/Name"/>
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
