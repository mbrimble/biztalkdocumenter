<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">	

<xsl:param name="ImgFile" />
<xsl:param name="CodeFile" />
 	      
<xsl:output method="html" indent="no"/>

	<xsl:template match="/">
		<html>
			<HEAD>
				<link href="../CommentReport.css" type="text/css" rel="stylesheet"></link>
			</HEAD>
			
			<body>
                
                <xsl:call-template name="header" />

                <!-- Image -->
                <xsl:if test="string-length($ImgFile)>0">
                    <table class="TableData" width="100%">
                        <tr>
                            <td align="center">
                                <span class="Copyright">
                                    <br/>Click through and hover available on send, receive, expression and message assignment shapes<br/>
                                </span>

                                <xsl:if test="count(BizTalkBaseObject/ShapeMap/Shape)>0">
                                    <map name="orchMap">
                                        <xsl:apply-templates />
                                    </map>
                                </xsl:if>

                                <xsl:element name="IMG">
                                    <xsl:attribute name="SRC">
                                        <xsl:value-of select="$ImgFile" />
                                    </xsl:attribute>

                                    <xsl:if test="count(BizTalkBaseObject/ShapeMap/Shape)>0">
                                        <xsl:attribute name="BORDER">0</xsl:attribute>
                                        <xsl:attribute name="usemap">#orchMap</xsl:attribute>
                                    </xsl:if>

                                </xsl:element>

                            </td>
                        </tr>
                    </table>
                </xsl:if>

            </body>
        </html>
    </xsl:template>
	
	<xsl:template match="BizTalkBaseObject/ShapeMap/Shape[ShapeType/text()!='SendShape']">
		<xsl:element name="area">	
			<xsl:attribute name="shape">rect</xsl:attribute>
			<xsl:attribute name="coords"><xsl:value-of select="SelectionArea/Coordinates" /></xsl:attribute>
			<xsl:attribute name="href"><xsl:value-of select="$CodeFile" />#<xsl:value-of select="Id" /></xsl:attribute>
			<xsl:attribute name="alt"><xsl:value-of select="Text" /></xsl:attribute>
		</xsl:element>
	</xsl:template>
	
	<xsl:template match="BizTalkBaseObject/ShapeMap/Shape[ShapeType/text()='SendShape']">
		<xsl:element name="area">	
			<xsl:attribute name="shape">rect</xsl:attribute>
			<xsl:attribute name="coords"><xsl:value-of select="SelectionArea/Coordinates" /></xsl:attribute>
			<xsl:attribute name="href"><xsl:value-of select="$CodeFile" />#<xsl:value-of select="Id" /></xsl:attribute>
			<xsl:attribute name="alt">Sends message '<xsl:value-of select="MessageName" />' to port '<xsl:value-of select="PortName" />'</xsl:attribute>
		</xsl:element>
	</xsl:template>
	
	<xsl:template match="BizTalkBaseObject/ShapeMap/Shape[ShapeType/text()='ReceiveShape']">
		<xsl:element name="area">	
			<xsl:attribute name="shape">rect</xsl:attribute>
			<xsl:attribute name="coords"><xsl:value-of select="SelectionArea/Coordinates" /></xsl:attribute>
			<xsl:attribute name="href"><xsl:value-of select="$CodeFile" />#<xsl:value-of select="Id" /></xsl:attribute>
			<xsl:attribute name="alt">Receives message '<xsl:value-of select="MessageName" />' from port '<xsl:value-of select="PortName" />'</xsl:attribute>
		</xsl:element>
	</xsl:template>
 
  <!--Added by Colin D>-->
  <xsl:template match="ArrayOfAnyType/anyType[ShapeType/text()='TransformShape']">
              <xsl:element name="area">  
                     <xsl:attribute name="shape">rect</xsl:attribute>
                     <xsl:attribute name="coords"><xsl:value-of select="SelectionArea/Coordinates" /></xsl:attribute>
                     <xsl:attribute name="href"><xsl:value-of select="$CodeFile" />#<xsl:value-of select="Id" /></xsl:attribute>
                     <xsl:attribute name="alt">Map '<xsl:value-of select="TransformName" />' transforms '<xsl:value-of select="InputMessage" />' to '<xsl:value-of select="OutputMessage" />'</xsl:attribute>
              </xsl:element>
       </xsl:template>

	<xsl:template match="text()">
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
                        <img src="../Orchestration.gif" align="middle"/>
                    </nobr>
                </td>
                <td width="100%" background="../topBackground.jpg" valign="middle" CLASS="PageTitle">
                    <nobr>
                        Orchestration : <xsl:value-of select="./BizTalkBaseObject/Name"/>
                    </nobr>
                </td>
                <td background="../topBackground.jpg" valign="middle" align="right">
                    <IMG SRC="../topRight.jpg" ALIGN="middle"/>
                </td>
            </tr>
        </table>
        <BR/>

    </xsl:template>

</xsl:stylesheet>
