<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">	

<xsl:param name="GenDate" />
<xsl:param name="DocVersion" />
 	      
<xsl:output method="html" indent="no"/>

	<xsl:template match="/">
		<html>
			<HEAD>
                <title>Assembly:<xsl:value-of select ="./BizTalkBaseObject/Name"/> </title>               
				<link href="../CommentReport.css" type="text/css" rel="stylesheet"></link>
			</HEAD>
			
			<body>

            <xsl:call-template name="header" /> 

            <table class="TableData">
			<tr>
			    <td width="10"></td>
			    <td width="145" class="TableTitle">Name:</td>
			    <td class="TableTitle"></td>
			    <td class="TableData"><nobr><xsl:value-of select="BizTalkBaseObject/DisplayName" /></nobr></td>
			</tr>
			<tr>
			    <td></td>
			    <td class="TableTitle">Version:</td>
			    <td class="TableTitle"></td>
			    <td class="TableData"><xsl:value-of select="BizTalkBaseObject/Version" /></td>
			</tr>
			<tr>
			    <td></td>
			    <td class="TableTitle">Culture:</td>
			    <td class="TableTitle"></td>
			    <td class="TableData"><xsl:value-of select="BizTalkBaseObject/Culture" /></td>
			</tr>
			<tr>
			    <td></td>
			    <td class="TableTitle"><nobr>Public Key Token:</nobr></td>
			    <td class="TableTitle"></td>
			    <td class="TableData"><xsl:value-of select="BizTalkBaseObject/PublicKeyToken" /></td>
			</tr>
			<tr>
			    <td colspan="3"><BR/></td>
			</tr>
			<tr>
			    <td></td>
			    <td class="TableTitle" valign="top"><nobr>Referenced Assemblies:</nobr></td>
			    <td class="TableTitle"></td>
			    <td class="TableData" valign="top">
					<xsl:for-each select="BizTalkBaseObject/References/ReferencedAssembly">
						<xsl:sort select="text()" />
						<nobr><xsl:value-of select="text()" /></nobr><BR/>
					</xsl:for-each>			    
			    </td>
			</tr>
			</table>
			
			<!-- Orchestrations -->
			<xsl:if test="count(BizTalkBaseObject/Orchestrations/Orchestration)>0">	
			<BR/><BR/>
			    <Table class="TableData" border="0">
			    <tr>
			        <td width="10"></td>
			        <td class="PageTitle3" colspan="2">Orchestrations contained in this assembly</td>
			    </tr>
    			
			    <xsl:for-each select="BizTalkBaseObject/Orchestrations/Orchestration">
			        <tr>
			            <td></td>
			            <td><IMG SRC='../OrchestrationSmall.jpg' VALIGN="center" width="16" height="16"/></td>
			            <td>
			                <xsl:choose>
			                    <xsl:when test="string-length(./Id)>0">
			                        <xsl:element name="A">
					                <xsl:attribute name="CLASS">TableData</xsl:attribute>
					                <xsl:attribute name="HREF">/Orchestration/<xsl:value-of select="./Id" />.htm</xsl:attribute>
					                <xsl:value-of select="./Name" />
				                    </xsl:element>
				                </xsl:when>
				                <xsl:otherwise><xsl:value-of select="Name" /></xsl:otherwise>
				            </xsl:choose>
				        </td>
			        </tr>
			    </xsl:for-each>
			    </Table>
			</xsl:if>
			
			<!-- Pipelines -->
			<xsl:if test="count(BizTalkBaseObject/Pipelines/Pipeline)>0">	
			<BR/><BR/>
			    <Table class="TableData" border="0">
			    <tr>
			        <td width="10"></td>
			        <td class="PageTitle3" colspan="2">Pipelines contained in this assembly</td>
			    </tr>
    			
			    <xsl:for-each select="BizTalkBaseObject/Pipelines/Pipeline">
			        <tr>
			            <td></td>
			            <td><IMG SRC='../Pipeline.gif' VALIGN="center" width="16" height="16"/></td>
			            <td>
			                <xsl:choose>
			                    <xsl:when test="string-length(./Id)>0">
			                        <xsl:element name="A">
					                <xsl:attribute name="CLASS">TableData</xsl:attribute>
					                <xsl:attribute name="HREF">/Pipeline/<xsl:value-of select="./Id" />.htm</xsl:attribute>
					                <xsl:value-of select="./Name" />
				                    </xsl:element>
				                </xsl:when>
				                <xsl:otherwise><xsl:value-of select="Name" /></xsl:otherwise>
				            </xsl:choose>
				        </td>
			        </tr>
			    </xsl:for-each>
			    </Table>
			</xsl:if>
			
			<!-- Maps -->
			<xsl:if test="count(BizTalkBaseObject/Maps/Transform)>0">		
			<BR/><BR/>	
			    <Table class="TableData" border="0">
			    <tr>
			        <td width="10"></td>
			        <td class="PageTitle3" colspan="5">Maps contained in this assembly</td>
			    </tr>
			    
			    <xsl:for-each select="BizTalkBaseObject/Maps/Transform">
			        <tr>
			            <td></td>
			            <td><IMG SRC='../MapSmall.jpg' VALIGN="center" width="16" height="16"/></td>
			            <td>
			                <xsl:choose>
			                    <xsl:when test="string-length(./Id)>0">
									<xsl:element name="A">
									<xsl:attribute name="CLASS">TableData</xsl:attribute>
									<xsl:attribute name="HREF">/Map/<xsl:value-of select="./Id" />.html</xsl:attribute><xsl:value-of select="./Name" /></xsl:element>
				                </xsl:when>
				                <xsl:otherwise><xsl:value-of select="Name" /></xsl:otherwise>
				            </xsl:choose>
				        </td>
			        </tr>		
			    </xsl:for-each>
			    
			    </Table>
			</xsl:if>
			
			<!-- Schemas -->		
			<xsl:if test="count(BizTalkBaseObject/Schemas/Schema)>0">	
			<BR/><BR/>
			    <Table class="TableData">
			    <tr>
			        <td width="10"></td>
			        <td class="PageTitle3" colspan="2">Schema contained in this assembly</td>
			    </tr>
    			
			    <xsl:for-each select="BizTalkBaseObject/Schemas/Schema">
			        <tr>
			            <td width="10"></td>
			            <td><IMG SRC='../SchemaSmall.jpg' VALIGN="center" width="16" height="16"/></td>
			            <td>
			                <xsl:choose>
			                    <xsl:when test="string-length(./Id)>0">
									<xsl:element name="A">
									<xsl:attribute name="CLASS">TableData</xsl:attribute>
									<xsl:attribute name="HREF">/Schema/<xsl:value-of select="./Id" />.htm</xsl:attribute>
									<xsl:value-of select="./Name" />
									</xsl:element>
				                </xsl:when>
				                <xsl:otherwise><xsl:value-of select="Name" /></xsl:otherwise>
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
                        <img src="../Assembly.gif" align="middle"/>
                    </nobr>
                </td>
                <td width="100%" background="../topBackground.jpg" valign="middle" CLASS="PageTitle">
                    <nobr>Assembly : <xsl:value-of select="./BizTalkBaseObject/DisplayName"/></nobr>
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
