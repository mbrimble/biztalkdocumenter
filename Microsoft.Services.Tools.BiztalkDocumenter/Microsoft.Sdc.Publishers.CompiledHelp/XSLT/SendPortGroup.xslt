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
      
			<!-- Send ports -->			
			<Table class="TableData">
                <tr>
                    <td></td>
                    <td class="TableTitle" valign="top">Application:</td>
                    <td class="TableData">
                        <xsl:value-of select="BizTalkBaseObject/ApplicationName" />
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
			    <td width="10"></td>
			    <td class="PageTitle3" colspan="3">Send ports contained in this group</td>
			</tr>
			
			<xsl:for-each select="BizTalkBaseObject/SendPorts/SendPort">
			    <tr>
			        <td></td>
			        <td>
			            <xsl:choose>
			                <xsl:when test="string-length(./Id)>0">
			                    <xsl:element name="A">
					            <xsl:attribute name="CLASS">TableData</xsl:attribute>
					            <xsl:attribute name="HREF">/SendPort/<xsl:value-of select="./Id" />.htm</xsl:attribute>
					            <xsl:value-of select="./Name" />
				                </xsl:element>
				            </xsl:when>
				            <xsl:otherwise><xsl:value-of select="./Name" /></xsl:otherwise>
				        </xsl:choose>
				    </td> 
			    </tr>
			</xsl:for-each>
			</Table>
			
			<!-- Filter groups -->	
			<xsl:if test="count(BizTalkBaseObject/FilterGroups/FilterGroup)>0">			
			<BR/>
			    <Table class="TableData" cellpadding="0" cellspacing="0">
			    <tr>
			        <td width="10"></td>
			        <td class="PageTitle3" colspan="6">Filters</td>
			    </tr>
			    <tr>
			        <td></td>
			        <td></td>
			        <td class="TableTitle">Filter Property</td>
			        <td width="10"></td>
			        <td class="TableTitle">Operator</td>
			        <td width="10"></td>
			        <td class="TableTitle">Value</td>
			    </tr>
    			
			    <xsl:for-each select="BizTalkBaseObject/FilterGroups/FilterGroup">
			        <xsl:variable name="parentPos"><xsl:value-of select="position()" /></xsl:variable>
			        <xsl:variable name="numItems"><xsl:value-of select="count(Filter/Filter)" /></xsl:variable>
			        <xsl:for-each select="Filter/Filter">
			            <tr>
			                <td></td>
			                <td>
			                    <xsl:choose>
			                        <xsl:when test="position() = 1 and position() != $numItems">
			                            <IMG SRC='../divTop.jpg' VALIGN="center" width="16" height="16"/>
			                        </xsl:when>
			                    </xsl:choose>
			                    
			                    <xsl:choose>
			                        <xsl:when test="position() = $numItems and position() != 1">
			                            <IMG SRC='../divBot.jpg' VALIGN="center" width="16" height="16"/>
			                        </xsl:when>
			                    </xsl:choose>
			                    
			                    <xsl:choose>
			                        <xsl:when test="position() != $numItems and position() != 1">
			                            <IMG SRC='../divMid.jpg' VALIGN="center" width="16" height="16"/>
			                        </xsl:when>
			                        
			                        <xsl:otherwise><IMG SRC='../divBlank.jpg' VALIGN="center" width="16" height="16"/></xsl:otherwise>
			                    </xsl:choose>			                    
			                </td>
			                <td><xsl:value-of select="./Property" /></td>
			                <td width="10"></td>
			                <td><xsl:value-of select="./FilterOperator" /></td>
			                <td width="10"></td>
			                <td><xsl:value-of select="./Value" /></td>        
			            </tr>
			        </xsl:for-each>
			    </xsl:for-each>
			    </Table>
			</xsl:if>
			
			<!-- Bound Orchestrations -->
			<xsl:if test="count(BizTalkBaseObject/BoundOrchestrations/Orchestration)>0">	
			<BR/>
			    <Table class="TableData" border="0">
			    <tr>
			        <td width="10"></td>
			        <td class="PageTitle3" colspan="2">Orchestrations bound to this group</td>
			    </tr>
    			
			    <xsl:for-each select="BizTalkBaseObject/BoundOrchestrations/Orchestration">
			        <tr>
			            <td></td>
			            <td><IMG SRC='../OrchestrationSmall.jpg' VALIGN="center" width="16" height="16"/></td>
			            <td>
			                <xsl:choose>
			                    <xsl:when test="string-length(Id)>0">
			                        <xsl:element name="A">
					                <xsl:attribute name="CLASS">TableData</xsl:attribute>
					                <xsl:attribute name="HREF">/Orchestration/<xsl:value-of select="Id" />.htm</xsl:attribute>
					                <xsl:value-of select="Name" />
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
                        <img src="../SendPort1.gif" align="middle"/>
                    </nobr>
                </td>
                <td width="100%" background="../topBackground.jpg" valign="middle" CLASS="PageTitle">
                    <nobr>
                        Send Port Group : <xsl:value-of select="./BizTalkBaseObject/Name"/>
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
