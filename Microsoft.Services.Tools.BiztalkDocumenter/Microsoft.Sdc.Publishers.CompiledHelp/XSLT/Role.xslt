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
                        <td width="125" class="TableTitle">
                            <nobr>Name:</nobr>
                        </td>
                        <td class="TableData">
                            <xsl:value-of select="BizTalkBaseObject/Name" />
                        </td>
                    </tr>
                    <tr>
                        <td width="10"></td>
                        <td width="125" class="TableTitle">
                            <nobr>Application:</nobr>
                        </td>
                        <td class="TableData">
                            <xsl:value-of select="BizTalkBaseObject/Application/Name" />
                        </td>
                    </tr>
                    <tr>
                        <td width="10"></td>
                        <td width="125" class="TableTitle">
                            <nobr>Parent Assembly:</nobr>
                        </td>
                        <td class="TableData">
                            <xsl:element name="A">
                                <xsl:attribute name="CLASS">TableData</xsl:attribute>
                                <xsl:attribute name="HREF">
                                    /Assembly/<xsl:value-of select="BizTalkBaseObject/ParentAssembly/Id" />.htm
                                </xsl:attribute>
                                <xsl:value-of select="BizTalkBaseObject/ParentAssembly/Name" />
                            </xsl:element>
                        </td>
                    </tr>
                    <tr>
                        <td width="10"></td>
                        <td width="125" class="TableTitle">
                            <nobr>Service Link Type:</nobr>
                        </td>
                        <td class="TableData">
                            <xsl:value-of select="BizTalkBaseObject/ServiceLinkType" />
                        </td>
                    </tr>
                </table>

                <!-- Enlisted Parties -->
                <xsl:if test="count(BizTalkBaseObject/EnlistedParties/Party)>0">
                    <BR/>

                    <Table class="TableData" border="0">
                        <tr>
                            <td width="10"></td>
                            <td class="PageTitle3" colspan="2">Enlisted Parties</td>
                        </tr>

                        <xsl:for-each select="BizTalkBaseObject/EnlistedParties/Party">
                            <tr>
                                <td></td>
                                <td>
                                    <IMG SRC='../Host.gif' VALIGN="center" width="16" height="16"/>
                                </td>
                                <td>
                                    <xsl:element name="A">
                                        <xsl:attribute name="CLASS">TableData</xsl:attribute>
                                        <xsl:attribute name="HREF">
                                            /Party/<xsl:value-of select="Id" />.htm
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
                        Role : <xsl:value-of select="./BizTalkBaseObject/Name"/>
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
