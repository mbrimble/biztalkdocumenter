<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:re="http://schemas.microsoft.com/businessruleslanguage/2002"  exclude-result-prefixes="re">

    <xsl:param name="GenDate" />
    <xsl:param name="DocVersion" />
    <xsl:param name="SourceFileName" />

    <xsl:output method="html" indent="no"/>

    <xsl:template match="/">

        <xsl:variable name="version">
            <xsl:value-of select="//re:brl/re:vocabulary/re:version[last()]/@major"/>.<xsl:value-of select="//re:brl/re:vocabulary/re:version[last()]/@minor"/>
        </xsl:variable>

        <html>
            <HEAD>
                <link href="../CommentReport.css" type="text/css" rel="stylesheet"></link>
            </HEAD>

            <body>

                <xsl:call-template name="header" />

                <table class="TableData">
                    <tr>
                        <td width="10"></td>
                        <td width="145" class="TableTitle">Description:</td>
                        <td class="TableData">
                            <xsl:choose>
                                <xsl:when test="string-length(re:brl/re:vocabulary/@description)>0">
                                    <xsl:value-of select="re:brl/re:vocabulary/@description"/>
                                </xsl:when>
                                <xsl:otherwise>N/A</xsl:otherwise>
                            </xsl:choose>
                        </td>
                    </tr>
                    <tr>
                        <td width="10"></td>
                        <td width="145" class="TableTitle">Version:</td>
                        <td class="TableData">
                            <xsl:value-of select="$version"/>
                        </td>
                    </tr>
                    <tr>
                        <td width="10"></td>
                        <td width="145" class="TableTitle">

                            <xsl:element name="A">
                                <xsl:attribute name="CLASS">TableData</xsl:attribute>
                                <xsl:attribute name="HREF">
                                    /Vocabulary/<xsl:value-of select="re:brl/re:vocabulary/@name"/><xsl:text> </xsl:text><xsl:value-of select="$version"/>.xml
                                </xsl:attribute>
                                Vocabulary Source File
                            </xsl:element>

                        </td>
                        <td class="TableData"></td>
                    </tr>
                </table>

                <BR/>

                <!-- Definitions -->
                <xsl:if test="count(re:brl/re:vocabulary/re:vocabularydefinition)>0">
                    <BR/>
                    <Table class="TableData" width="80%" cellspacing="2" border="0">
                        <tr>
                            <td width="10"></td>
                            <td class="PageTitle3" colspan="3">
                                Vocabulary Definitions<BR/><BR/>
                            </td>
                        </tr>
                        <!--
			    <tr>
			        <td></td>
			        <td class="TableTitle">Name</td>
			        <td class="TableTitle">Description</td>
			        <td class="TableTitle">Type</td>
			    </tr>
			    -->

                        <xsl:apply-templates select="re:brl/re:vocabulary/re:vocabularydefinition">
                            <xsl:sort select="@name" />
                        </xsl:apply-templates>

                    </Table>
                </xsl:if>

                <xsl:call-template name="footer" />

            </body>
        </html>
    </xsl:template>

    <xsl:template match="re:vocabularydefinition">
        <tr>
            <td width="10"></td>
            <td valign="top">
                <span class="TableTitle">Name: </span>
                <nobr>
                    <xsl:value-of select="@name" />
                </nobr>
            </td>
        </tr>
        <tr>
            <td width="10"></td>
            <td valign="top">
                <span class="TableTitle">Description: </span>
                <xsl:choose>
                    <xsl:when test="string-length(@description)>0">
                        <xsl:value-of select="@description"/>
                    </xsl:when>
                    <xsl:otherwise>N/A</xsl:otherwise>
                </xsl:choose>
            </td>
        </tr>
        <tr>
            <td width="10"></td>
            <td valign="top">
                <span class="TableTitle">Format String: </span>
                <nobr>
                    <xsl:value-of select="re:formatstring/@string" />
                </nobr>
            </td>
        </tr>
        <tr>
            <td width="10"></td>
            <td valign="top">
                <span class="TableTitle">Type: </span>
                <nobr>
                    <xsl:choose>
                        <xsl:when test="string-length(re:engineoperationdefinition/@type)>0">
                            <xsl:value-of select="re:engineoperationdefinition/@type" />
                        </xsl:when>
                        <xsl:when test="string-length(re:literaldefinition/@type)>0">
                            <xsl:value-of select="re:literaldefinition/@type" />
                        </xsl:when>
                        <xsl:when test="string-length(re:bindingdefinition/re:databasecolumnbindingdefinition/@type)>0">
                            <xsl:value-of select="re:bindingdefinition/re:databasecolumnbindingdefinition/@type" />
                        </xsl:when>
                        <xsl:when test="string-length(re:bindingdefinition/re:documentelementbindingdefinition/@type)>0">
                            <xsl:value-of select="re:bindingdefinition/re:documentelementbindingdefinition/@type" />
                        </xsl:when>
                        <xsl:when test="string-length(re:bindingdefinition/re:classmemberbindingdefinition/@type)>0">
                            <xsl:value-of select="re:bindingdefinition/re:classmemberbindingdefinition/@type" />
                        </xsl:when>
                        <xsl:when test="string-length(re:setdefinition/@type)>0">
                            <xsl:value-of select="re:setdefinition/@type" />
                        </xsl:when>
                        <xsl:otherwise></xsl:otherwise>
                    </xsl:choose>
                </nobr>
            </td>
        </tr>
        <tr>
            <td width="10"></td>
            <td valign="top">
                <nobr>
                    <xsl:choose>
                        <xsl:when test="string-length(re:engineoperationdefinition/@type)>0">
                            <span class="TableTitle">Operation: </span>
                            <xsl:value-of select="re:engineoperationdefinition/@type"/>
                        </xsl:when>
                        <xsl:when test="string-length(re:literaldefinition/@type)>0">
                            <span class="TableTitle">Constant value: </span> '<xsl:value-of select="re:literaldefinition/child::*/text()"/>'
                        </xsl:when>
                        <xsl:when test="string-length(re:bindingdefinition/re:databasecolumnbindingdefinition/@column)>0">
                            <span class="TableTitle">Database binding: </span> <xsl:value-of select="re:bindingdefinition/re:databasecolumnbindingdefinition/re:databaseinfo/@database"/>..<xsl:value-of select="re:bindingdefinition/re:databasecolumnbindingdefinition/re:databaseinfo/@table"/>.<xsl:value-of select="re:bindingdefinition/re:databasecolumnbindingdefinition/@column"/>
                        </xsl:when>
                        <xsl:when test="string-length(re:bindingdefinition/re:documentelementbindingdefinition/@field)>0">
                            <span class="TableTitle">Document element binding: </span>
                            <xsl:value-of select="re:bindingdefinition/re:documentelementbindingdefinition/re:documentinfo/@selector"/>
                            <!--:Root/<xsl:value-of select="re:bindingdefinition/re:documentelementbindingdefinition/@field"/>-->
                        </xsl:when>
                        <xsl:when test="string-length(re:bindingdefinition/re:classmemberbindingdefinition/@member)>0">
                            <span class="TableTitle">Class member binding: </span> <xsl:value-of select="re:bindingdefinition/re:classmemberbindingdefinition/re:classinfo/@class"/>.<xsl:value-of select="re:bindingdefinition/re:classmemberbindingdefinition/@member"/>
                        </xsl:when>
                        <xsl:otherwise></xsl:otherwise>
                    </xsl:choose>
                </nobr>
                <br/>
                <br/>
            </td>
        </tr>
    </xsl:template>

    <xsl:template name="header">

        <table width="100%" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td width="10" background="../topBackground.jpg">
                    <nobr>
                        <img src="../topSpacer.gif"/>
                    </nobr>
                </td>
                <td width="100%" background="../topBackground.jpg" valign="middle" CLASS="PageTitle">
                    <nobr>
                        Vocabulary : <xsl:value-of select="re:brl/re:vocabulary/@name"/>
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
