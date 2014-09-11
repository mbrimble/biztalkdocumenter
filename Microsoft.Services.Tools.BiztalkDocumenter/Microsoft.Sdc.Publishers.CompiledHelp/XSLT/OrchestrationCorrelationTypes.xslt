<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">	

<xsl:param name="GenDate" />
<xsl:param name="ImgFile" />
<xsl:param name="DocVersion" />
 	      
<xsl:output method="html" indent="no"/>

	<xsl:template match="/">
		<html>
			<HEAD>
				<link href="../CommentReport.css" type="text/css" rel="stylesheet"></link>
			</HEAD>
			
			<body>

            <xsl:call-template name="header" /> 
			
			<!-- Correlation Set Types -->		
			<xsl:if test="count(BizTalkBaseObject/CorrelationSetTypes/CorrelationSetType)>0">	

			    <Table class="TableData">
			    <tr>
			        <td width="10"></td>
			        <td class="PageTitle3" colspan="2">Correlation set types defined within this orchestration</td>
			    </tr>
			    
			    <xsl:for-each select="BizTalkBaseObject/CorrelationSetTypes/CorrelationSetType">
			        <tr>
			            <td width="10"></td>
						<td><IMG SRC='../CorrelationSetSmall.jpg' VALIGN="center" width="16" height="16"/></td>
			            <td><span class="TableTitle"><xsl:value-of select="Name" /></span></td>
			        </tr>
			        <tr>
			            <td width="10"></td>
						<td></td>
			            <td><u>Correlated Properties</u></td>
			        </tr>
			        
						<xsl:for-each select="CorrelatedProperties/Property">
							<tr>
								<td width="10"></td>
								<td></td>
								<td><xsl:value-of select="text()" /></td>
							</tr>
						</xsl:for-each>
						
			        <tr>
			            <td width="10"></td>
						<td></td>
			            <td><br/><br/></td>
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
    
	<xsl:template name="footer">
		<BR/><BR/><BR/><BR/><HR CLASS="Rule"/><P CLASS="Copyright">Generated on: <xsl:value-of select="$GenDate"/><BR/>Microsoft.Services.Tools.BiztalkDocumenter version: <xsl:value-of select="$DocVersion"/></P><BR/>		
	</xsl:template>	


</xsl:stylesheet>
