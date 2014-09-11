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

            <table class="TableData">
            <tr>
                <td></td>
                <td class="TableTitle" valign="top">Application:</td>
                <td class="TableData">
                    <xsl:value-of select="BizTalkBaseObject/ApplicationName" />
                </td>
            </tr>
			<tr>
			    <td width="10"></td>
			    <td width="145" class="TableTitle"><nobr>Parent Assembly:</nobr></td>
			    <td class="TableData">
			        <nobr>
                        <xsl:element name="A">
                            <xsl:attribute name="CLASS">TableData</xsl:attribute>
                            <xsl:attribute name="HREF">
                                /Assembly/<xsl:value-of select="BizTalkBaseObject/ParentAssembly/Id" />.htm
                            </xsl:attribute>
                            <xsl:value-of select="BizTalkBaseObject/ParentAssembly/Name" />
                        </xsl:element>
                    </nobr>
                </td>
            </tr>
            <tr>
                <td></td>
			    <td class="TableTitle">Host:</td>
			    <td class="TableData">
			        <xsl:choose>
			            <xsl:when test="string-length(BizTalkBaseObject/Host/Name)>0">       
			                <xsl:element name="A">
					        <xsl:attribute name="CLASS">TableData</xsl:attribute>
					        <xsl:attribute name="HREF">/Host/<xsl:value-of select="BizTalkBaseObject/Host/Id" />.htm</xsl:attribute>
					        <xsl:value-of select="BizTalkBaseObject/Host/Name" />
				            </xsl:element>
				        </xsl:when>
				        <xsl:otherwise><xsl:value-of select="BizTalkBaseObject/Host/Name" /></xsl:otherwise>
				    </xsl:choose>
				</td>
			</tr>			
			<xsl:if test="string-length(BizTalkBaseObject/CustomDescription)>0">
			<tr>
			    <td></td>
			    <td class="TableTitle" valign="top">Description:</td>
			    <td class="TableData"><xsl:value-of select="BizTalkBaseObject/CustomDescription" /></td>
			</tr>
			</xsl:if>			
			</table>
			
			<!-- Variables -->		
			<xsl:if test="count(BizTalkBaseObject/Variables/Variable)>0">	
			<BR/><BR/>
			    <Table class="TableData">
			    <tr>
			        <td width="10"></td>
			        <td class="PageTitle3" colspan="10">Variables defined within this orchestration</td>
			    </tr>
			    <tr>
			        <td></td>
			        <td></td>
			        <td class="TableTitle">Name</td>
			        <td width="10"></td>
			        <td class="TableTitle">Type</td>
			        <td width="10"></td>
			        <td class="TableTitle">Scope</td>
			        <td width="10"></td>
			        <td class="TableTitle">Initial Value</td>
			        <td width="10"></td>
			        <td class="TableTitle">Description</td>
			    </tr>
			    
			    <xsl:for-each select="BizTalkBaseObject/Variables/Variable">
			        <tr>
			            <td></td>
						<td><IMG SRC='../VariableSmall.jpg' VALIGN="center" width="16" height="16"/></td>
			            <td><xsl:value-of select="./Name" /></td>
			            <td></td>
			            <td><xsl:value-of select="./TypeName" /></td>
			            <td></td>
			            <td><nobr><xsl:value-of select="./Scope" /></nobr></td>
			            <td></td>
			            <td><xsl:value-of select="./InitialValue" /></td>
			            <td></td>
			            <td><nobr><xsl:value-of select="./Description" /></nobr></td>
			            <td></td>
			        </tr>
			    </xsl:for-each>
			    
			    </Table>
			</xsl:if>
			
			<!-- Message -->		
			<xsl:if test="count(BizTalkBaseObject/Messages/Message)>0">	
			<BR/><BR/>
			    <Table class="TableData">
			    <tr>
			        <td width="10"></td>
			        <td class="PageTitle3" colspan="9">Messages defined within this orchestration</td>
			    </tr>
			    <tr>
			        <td></td>
			        <td></td>
			        <td class="TableTitle">Name</td>
			        <td width="10"></td>
			        <td class="TableTitle">Type</td>
			        <td width="10"></td>
			        <td class="TableTitle">Scope</td>
			        <td width="10"></td>
			        <td class="TableTitle">Description</td>
			    </tr>
			    
			    <xsl:for-each select="BizTalkBaseObject/Messages/Message">
			        <tr>
			            <td></td>
						<td><IMG SRC='../MessageSmall.jpg' VALIGN="center" width="16" height="16"/></td>
			            <td><xsl:value-of select="./Name" /></td>
			            <td></td>
			            <td><xsl:value-of select="./TypeName" /></td>
			            <td></td>
			            <td><nobr><xsl:value-of select="./Scope" /></nobr></td>
			            <td></td>
			            <td><nobr><xsl:value-of select="./Description" /></nobr></td>
			        </tr>
			    </xsl:for-each>
			    
			    </Table>
			</xsl:if>

                <!-- Multi part Message -->
                <xsl:if test="count(BizTalkBaseObject/MultiPartMessages/MultiPartMessage)>0">
                    <BR/>
                    <BR/>
                    <Table class="TableData" border="0">
                        <tr>
                            <td width="10"></td>
                            <td class="PageTitle3" colspan="9">Multi-Part Messages defined within this orchestration</td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td class="TableTitle">Name</td>
                            <td width="10"></td>
                            <td class="TableTitle">
                                <nobr>Type Modifier</nobr>
                            </td>
                            <td width="10"></td>
                            <td class="TableTitle">
                                <nobr>Part Name</nobr>
                            </td>
                            <td width="10"></td>
                            <td class="TableTitle">
                                <nobr>Is Body Part</nobr>
                            </td>
                            <td width="10"></td>
                            <td class="TableTitle">
                                <nobr>Class Name</nobr>
                            </td>
                        </tr>

                        <xsl:for-each select="BizTalkBaseObject/MultiPartMessages/MultiPartMessage">
                            <tr>
                                <td></td>
                                <td>
                                    <IMG SRC='../MessageSmall.jpg' VALIGN="center" width="16" height="16"/>
                                </td>
                                <td>
                                    <xsl:value-of select="./Name" />
                                </td>
                                <td></td>
                                <td>
                                    <xsl:value-of select="./Modifier" />
                                </td>
                                <td></td>

                                <xsl:for-each select="./Parts/MultiPartMessage">
                                    <xsl:if test="position()=1">
                                        <td class="TableData">
                                            <nobr>
                                                <xsl:value-of select="./Name" />
                                            </nobr>
                                        </td>
                                        <td></td>
                                        <td class="TableData">
                                            <nobr>
                                                <xsl:value-of select="./IsBodyPart" />
                                            </nobr>
                                        </td>
                                        <td></td>
                                        <td class="TableData">
                                            <nobr>
                                                <xsl:value-of select="./ClassName" />
                                            </nobr>
                                        </td>
                                    </xsl:if>
                                    <xsl:if test="position()>1">
                                        <tr>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                            <td class="TableData">
                                                <nobr>
                                                    <xsl:value-of select="./Name" />
                                                </nobr>
                                            </td>
                                            <td></td>
                                            <td class="TableData">
                                                <nobr>
                                                    <xsl:value-of select="./IsBodyPart" />
                                                </nobr>
                                            </td>
                                            <td></td>
                                            <td class="TableData">
                                                <nobr>
                                                    <xsl:value-of select="./ClassName" />
                                                </nobr>
                                            </td>
                                        </tr>
                                    </xsl:if>
                                </xsl:for-each>

                            </tr>
                        </xsl:for-each>

                    </Table>
                </xsl:if>
			
			<!-- Called Rules Policies -->		
			<xsl:if test="count(BizTalkBaseObject/CalledRules/Rule)>0">	
			<BR/><BR/>
			    <Table class="TableData">
			    <tr>
			        <td width="10"></td>
			        <td class="PageTitle3" colspan="9">Rules engine policies called by this orchestration</td>
			    </tr>
			    <tr>
			        <td></td>
			        <td class="TableTitle">Policy Name:</td>
			    </tr>
			    
			    <xsl:for-each select="BizTalkBaseObject/CalledRules/Rule">
			        <tr>
			            <td></td>
			            <td><xsl:value-of select="." /></td>
			        </tr>
			    </xsl:for-each>
			    
			    </Table>
			</xsl:if>
			
			<!-- Transforms -->		
			<xsl:if test="count(BizTalkBaseObject/Transforms/Transform)>0">	
			<BR/><BR/>
			    <Table class="TableData">
			    <tr>
			        <td width="10"></td>
			        <td class="PageTitle3" colspan="2">Maps used by this orchestration</td>
			    </tr>
			    <tr>
			        <td></td>
			        <td></td>
			        <td class="TableTitle">Transform Name:</td>
			    </tr>
			    
			    <xsl:for-each select="BizTalkBaseObject/Transforms/Transform">
			        <tr>
			            <td></td>
			            <td><IMG SRC='../MapSmall.jpg' VALIGN="center" width="16" height="16"/></td>
			            <td>
                            <xsl:element name="A">
                                <xsl:attribute name="CLASS">TableData</xsl:attribute>
                                <xsl:attribute name="HREF">
                                    /Map/<xsl:value-of select="Id" />.html
                                </xsl:attribute>
                                <xsl:value-of select="Name" />
                            </xsl:element>
                        </td>
			        </tr>
			    </xsl:for-each>
			    
			    </Table>
			</xsl:if>
			
			<!-- Correlation Sets -->		
			<xsl:if test="count(BizTalkBaseObject/CorrelationSets/CorrelationSet)>0">	
			<BR/><BR/>
			    <Table class="TableData">
			    <tr>
			        <td width="10"></td>
			        <td class="PageTitle3" colspan="8">Correlation sets defined within this orchestration</td>
			    </tr>
			    <tr>
			        <td></td>
			        <td></td>
			        <td class="TableTitle">Name</td>
			        <td width="10"></td>
			        <td class="TableTitle">Type</td>
			        <td width="10"></td>
			        <td class="TableTitle">Scope</td>
			        <td width="10"></td>
			        <td class="TableTitle">Description</td>
			    </tr>

			    <xsl:for-each select="BizTalkBaseObject/CorrelationSets/CorrelationSet">
			        <tr>
			            <td></td>
						<td><IMG SRC='../CorrelationSetSmall.jpg' VALIGN="center" width="16" height="16"/></td>
			            <td><xsl:value-of select="./Name" /></td>
			            <td></td>
			            <td>							     
			                <xsl:element name="A">
					        <xsl:attribute name="CLASS">TableData</xsl:attribute>
					        <xsl:attribute name="HREF">/Orchestration/CorrelationTypes<xsl:value-of select="../../Id" />.htm</xsl:attribute>
					        <xsl:value-of select="TypeName" />
				            </xsl:element>
				        </td>
			            <td></td>
			            <td><nobr><xsl:value-of select="./Scope" /></nobr></td>
			            <td></td>
			            <td><nobr><xsl:value-of select="./Description" /></nobr></td>
			            <td></td>
			        </tr>
			    </xsl:for-each>
			    
			    </Table>
			</xsl:if>
			
			<!-- Ports -->		
			<xsl:if test="count(BizTalkBaseObject/Ports/OrchestrationPort)>0">	
			<BR/><BR/>
			    <Table class="TableData">
			    <tr>
			        <td width="10"></td>
			        <td class="PageTitle3" colspan="4">Ports contained within this orchestration</td>
			    </tr>
			    <tr>
			        <td></td>
			        <td></td>
			        <td class="TableTitle">Name</td>
			        <td width="10"></td>
			        <td class="TableTitle">Bound To</td>
			    </tr>
    			
			    <xsl:for-each select="BizTalkBaseObject/Ports/OrchestrationPort">
			        <tr>
			            <td></td>
						<td><IMG SRC='../OrchPortSmall.jpg' VALIGN="center" width="16" height="16"/></td>
			            <td><xsl:value-of select="./Name" /></td>
			            <td></td>
			            <td>
			                <xsl:choose>
			                    <xsl:when test="string-length(SendPortName/Name)>0">
			                        <xsl:choose>
			                            <xsl:when test="string-length(./SendPortName/Id)>0"> 
			                                <xsl:element name="A">
					                        <xsl:attribute name="CLASS">TableData</xsl:attribute>
					                        <xsl:attribute name="HREF">/SendPort/<xsl:value-of select="./SendPortName/Id" />.htm</xsl:attribute>
					                        <xsl:value-of select="./SendPortName/Name" />
				                            </xsl:element>
				                        </xsl:when>
				                        <xsl:otherwise><xsl:value-of select="./SendPortName/Name" /></xsl:otherwise>
				                    </xsl:choose>
				                </xsl:when>
			                    <xsl:when test="string-length(SendPortGroupName/Name)>0">
			                        <xsl:choose>
			                            <xsl:when test="string-length(./SendPortGroupName/Id)>0"> 
			                                <xsl:element name="A">
					                        <xsl:attribute name="CLASS">TableData</xsl:attribute>
					                        <xsl:attribute name="HREF">/SendPortGroup/<xsl:value-of select="./SendPortGroupName/Id" />.htm</xsl:attribute>
					                        <xsl:value-of select="./SendPortGroupName/Name" />
				                            </xsl:element>
				                        </xsl:when>
				                        <xsl:otherwise><xsl:value-of select="./SendPortGroupName/Name" /></xsl:otherwise>
				                    </xsl:choose>
				                </xsl:when>
			                    <xsl:when test="string-length(ReceivePortName/Name)>0">
			                        <xsl:choose>
			                            <xsl:when test="string-length(./ReceivePortName/Id)>0"> 
			                                <xsl:element name="A">
					                        <xsl:attribute name="CLASS">TableData</xsl:attribute>
					                        <xsl:attribute name="HREF">/ReceivePort/<xsl:value-of select="./ReceivePortName/Id" />.htm</xsl:attribute>
					                        <xsl:value-of select="./ReceivePortName/Name" />
				                            </xsl:element>
				                        </xsl:when>
				                        <xsl:otherwise><xsl:value-of select="./ReceivePortName/Name" /></xsl:otherwise>
				                    </xsl:choose>
				                </xsl:when>
				                <xsl:otherwise>&lt;Un-Bound&gt;</xsl:otherwise>
				            </xsl:choose>
				        </td>      
			        </tr>
			    </xsl:for-each>
			    </Table>
			</xsl:if>

                <!-- Role Links -->
                <xsl:if test="count(BizTalkBaseObject/RoleLinks/RoleLink)>0">
                    <BR/>
                    <BR/>
                    <Table class="TableData">
                        <tr>
                            <td width="10"></td>
                            <td class="PageTitle3" colspan="8">Role links contained within this orchestration</td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td class="TableTitle">Name</td>
                            <td width="10"></td>
                            <td class="TableTitle">Provider</td>
                            <td width="10"></td>
                            <td class="TableTitle">Consumer</td>
                            <td width="10"></td>
                            <td class="TableTitle">Type</td>
                        </tr>

                        <xsl:for-each select="BizTalkBaseObject/RoleLinks/RoleLink">
                            <tr>
                                <td></td>
                                <td>
                                    <IMG SRC='../Host.gif' VALIGN="center" width="16" height="16"/>
                                </td>
                                <td>
                                    <xsl:value-of select="./Name" />
                                </td>
                                <td></td>
                                <td>
                                    <xsl:element name="A">
                                        <xsl:attribute name="CLASS">TableData</xsl:attribute>
                                        <xsl:attribute name="HREF">
                                            /Role/<xsl:value-of select="ProviderRole/Id" />.htm
                                        </xsl:attribute>
                                        <xsl:value-of select="ProviderRole/Name" />
                                    </xsl:element>
                                </td>
                                <td></td>
                                <td>
                                    <xsl:element name="A">
                                        <xsl:attribute name="CLASS">TableData</xsl:attribute>
                                        <xsl:attribute name="HREF">
                                            /Role/<xsl:value-of select="ConsumerRole/Id" />.htm
                                        </xsl:attribute>
                                        <xsl:value-of select="ConsumerRole/Name" />
                                    </xsl:element>
                                </td>
                                <td></td>
                                <td>
                                    <nobr>
                                        <xsl:value-of select="TypeName" />
                                    </nobr>
                                </td>
                                <td></td>
                            </tr>
                        </xsl:for-each>

                    </Table>
                </xsl:if>
			
			<!-- Invoked Orchestrations -->		
			<xsl:if test="count(BizTalkBaseObject/InvokedOrchestrations/Orchestration)>0">	
			<BR/><BR/>
			    <Table class="TableData">
			    <tr>
			        <td width="10"></td>
			        <td class="PageTitle3" colspan="2">Invoked Orchestrations</td>
			    </tr>
    			
			    <xsl:for-each select="BizTalkBaseObject/InvokedOrchestrations/Orchestration">
			        <tr>
			            <td></td>
			            <td><IMG SRC='../OrchestrationSmall.jpg' VALIGN="center" width="16" height="16"/></td>
			            <td class="TableData"><xsl:value-of select="./Name" /></td>      
			        </tr>
			    </xsl:for-each>
			    </Table>
			</xsl:if>
			
			<!-- Image -->
			<xsl:if test="string-length($ImgFile)>0">
			    <BR/><BR/>
			    <table class="TableData" width="100%">
			    <tr>
			        <td align="center">
			            <xsl:element name="IMG">
				        <xsl:attribute name="SRC"><xsl:value-of select="$ImgFile" /></xsl:attribute>
				        </xsl:element>	
			        </td>
			    </tr>
			    </table>
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
