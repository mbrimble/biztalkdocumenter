<?xml version="1.0" encoding="iso-8859-1"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:param name="GenDate" />
  <xsl:param name="DocVersion" />

  <xsl:output method="html" indent="no" />

  <xsl:template match="/">
    <html>
      <HEAD>
        <title>
          SSO Configuraion:
          <xsl:value-of select="./sso/application/description" />
          </title>
        <link href="../CommentReport.css" type="text/css" rel="stylesheet"></link>
      </HEAD>

      <body>

        <xsl:call-template name="header" />
        
        <table class="TableData" >
          <tr>
            
            <td width="200" class="TableTitle">Application:</td>
            <td class="TableData">
              <nobr>
                <xsl:value-of select="sso/application/description" />
              </nobr>
            </td>
          </tr>

          <tr>
            <td class="TableTitle">User Acount:</td>
            <td class="TableData">
              <xsl:value-of select="sso/application/appUserAccount" />
            </td>
          </tr>
          <tr>
            <td class="TableTitle">Admin Account:</td>
            <td class="TableData">
              <xsl:value-of select="sso/application/appAdminAccount" />
            </td>
          </tr>

          <tr>
            <td colspan="3">
              <BR />
            </td>
          </tr>

          <xsl:for-each select="sso/application/field">
            
            <xsl:variable name="css-class">
              <xsl:choose>
                <xsl:when test="position() mod 2 = 0">RowNorm</xsl:when>
                <xsl:otherwise>RowAlt</xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <tr class="{$css-class}">

             
              <td class="TableTitle">
                <xsl:value-of select="@label" />
              </td>
             
              <td class="TableData">
                <xsl:value-of select="text()" />
              </td>
            </tr>

          </xsl:for-each>


        </table>

        <xsl:call-template name="footer" />

      </body>
    </html>
  </xsl:template>

  <xsl:template name="header">

    <table width="100%" border="0" cellpadding="0" cellspacing="0">
      <tr>
        <td width="10" background="../topBackground.jpg">
          <nobr>
            <img src="../topSpacer.gif" />
          </nobr>
        </td>
        <td width="10" background="../topBackground.jpg" valign="middle">
          <nobr>
            <img src="../Assembly.gif" align="middle" />
          </nobr>
        </td>
        <td width="100%" background="../topBackground.jpg" valign="middle" CLASS="PageTitle">
          <nobr>
            SSO Configuration
            <xsl:value-of select="./BizTalkBaseObject/DisplayName" />
          </nobr>
        </td>
        <td background="../topBackground.jpg" valign="middle" align="right">
          <IMG SRC="../topRight.jpg" ALIGN="middle" />
        </td>
      </tr>
    </table>
    <BR />

  </xsl:template>

  <xsl:template name="footer">
    <BR />
    <BR />
    <BR />
    <BR />
    <HR CLASS="Rule" />
    <P CLASS="Copyright">
      Generated on:
      <xsl:value-of select="$GenDate" />
      <BR />
      Microsoft.Services.Tools.BiztalkDocumenter version:
      <xsl:value-of select="$DocVersion" />
    </P>
    <BR />
  </xsl:template>


</xsl:stylesheet>