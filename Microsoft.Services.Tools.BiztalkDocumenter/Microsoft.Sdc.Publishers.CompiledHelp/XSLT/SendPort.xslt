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
                    <td width="145" class="TableTitle">Application:</td>
                    <td class="TableData">
                        <xsl:value-of select="BizTalkBaseObject/ApplicationName" />
                    </td>
                </tr>
                <tr>
                    <td width="10"></td>
                    <td width="145" class="TableTitle">Priority:</td>
                    <td class="TableData">
                        <xsl:value-of select="BizTalkBaseObject/Priority" />
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td class="TableTitle">
                        <nobr>Tracking Type:</nobr>
                    </td>
                    <td class="TableData">
                        <xsl:value-of select="BizTalkBaseObject/TrackingType" />
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td class="TableTitle">
                        <nobr>Route Failed Message:</nobr>
                    </td>
                    <td class="TableData">
                        <xsl:value-of select="BizTalkBaseObject/RouteFailedMessage" />
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td class="TableTitle">
                        <nobr>Stop Sending On Failure:</nobr>
                    </td>
                    <td class="TableData">
                        <xsl:value-of select="BizTalkBaseObject/StopSendingOnFailure" />
                    </td>
                </tr>
                
			<tr>
			    <td></td>
        <td class="TableTitle">
          <nobr>Send Pipeline:</nobr>
        </td>
        <td class="TableData">
          <xsl:choose>
            <xsl:when test="string-length(BizTalkBaseObject/SendPipeline/Id)>0">
              <xsl:element name="A">
                <xsl:attribute name="CLASS">TableData</xsl:attribute>
                <xsl:attribute name="HREF">
                  /Pipeline/<xsl:value-of select="BizTalkBaseObject/SendPipeline/Id" />.htm
                </xsl:attribute>
                <xsl:value-of select="BizTalkBaseObject/SendPipeline/Name" />
              </xsl:element>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="BizTalkBaseObject/SendPipeline/Name" />
            </xsl:otherwise>
          </xsl:choose>
				</td>
			</tr>

              <!--<tr>
                <td></td>
                <td class="TableTitle">
                  <nobr>Send Handler:</nobr>
                </td>
                <td class="TableData">
                  <xsl:element name="A">
                    <xsl:attribute name="CLASS">TableData</xsl:attribute>
                    <xsl:attribute name="HREF">
                      /Host/<xsl:value-of select="BizTalkBaseObject/SendHandler/Id" />.htm
                    </xsl:attribute>
                    <xsl:value-of select="BizTalkBaseObject/SendHandler/Name" />
                  </xsl:element>
                </td>
              </tr>-->
              <!--<xsl:if test="BizTalkBaseObject/TwoWay='true'">
			    <tr>
			        <td></td>
			        <td class="TableTitle"><nobr>Receive Pipeline:</nobr></td>
			        <td class="TableData">
			            <xsl:choose>
			                <xsl:when test="string-length(BizTalkBaseObject/ReceivePipeline/@id)>0">
			                    <xsl:element name="A">
					            <xsl:attribute name="CLASS">TableData</xsl:attribute>
					            <xsl:attribute name="HREF">/Pipeline/<xsl:value-of select="BizTalkBaseObject/ReceivePipeline/@id" />.htm</xsl:attribute>
					            <xsl:value-of select="BizTalkBaseObject/ReceivePipeline" />
				                </xsl:element>
				        </xsl:when>
				        <xsl:otherwise><xsl:value-of select="BizTalkBaseObject/ReceivePipeline" /></xsl:otherwise>
				    </xsl:choose>
				    </td>
			    </tr>
			</xsl:if>-->
              <xsl:if test="BizTalkBaseObject/TwoWay='true'">
                <tr>
                  <td></td>
                  <td class="TableTitle">
                    <nobr>Receive Pipeline:</nobr>
                  </td>
                  <td class="TableData">
                    <xsl:choose>
                      <xsl:when test="string-length(BizTalkBaseObject/ReceivePipeline/Id)>0">
                        <xsl:element name="A">
                          <xsl:attribute name="CLASS">TableData</xsl:attribute>
                          <xsl:attribute name="HREF">
                            /Pipeline/<xsl:value-of select="BizTalkBaseObject/ReceivePipeline/Id" />.htm
                          </xsl:attribute>
                          <xsl:value-of select="BizTalkBaseObject/ReceivePipeline/Name" />
                        </xsl:element>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="BizTalkBaseObject/ReceivePipeline/Name" />
                      </xsl:otherwise>
                    </xsl:choose>
                  </td>
                </tr>
              </xsl:if>
              <tr>
			    <td></td>
			    <td class="TableTitle">Dynamic:</td>
			    <td class="TableData"><xsl:value-of select="BizTalkBaseObject/Dynamic" /></td>
			</tr>
			<tr>
			    <td></td>
			    <td class="TableTitle"><nobr>Two-Way:</nobr></td>
			    <td class="TableData"><xsl:value-of select="BizTalkBaseObject/TwoWay" /></td>
			</tr>			
			<xsl:if test="string-length(BizTalkBaseObject/CustomDescription)>0">
			<tr>
			    <td></td>
			    <td class="TableTitle" valign="top">Description:</td>
			    <td class="TableData"><xsl:value-of select="BizTalkBaseObject/CustomDescription" /></td>
			</tr>
			</xsl:if>

                <!-- Certificate -->
                <xsl:if test="BizTalkBaseObject/EncryptionCert">
                    <tr>
                        <td></td>
                        <td class="TableTitle">
                            <br/>
                            <nobr>
                                <u>Certificate Information</u>
                            </nobr>
                        </td>
                        <td class="TableData"></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td class="TableTitle">
                            <nobr>Certificate Short Name:</nobr>
                        </td>
                        <td class="TableData">
                            <xsl:value-of select="BizTalkBaseObject/EncryptionCert/ShortName" />
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td class="TableTitle">
                            <nobr>Certificate Long Name:</nobr>
                        </td>
                        <td class="TableData">
                            <xsl:value-of select="BizTalkBaseObject/EncryptionCert/LongName" />
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td class="TableTitle">
                            <nobr>Certificate Thumprint:</nobr>
                        </td>
                        <td class="TableData">
                            <xsl:value-of select="BizTalkBaseObject/EncryptionCert/Thumprint" />
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td class="TableTitle">
                            <nobr>Certificate Usage</nobr>
                        </td>
                        <td class="TableData">
                            <xsl:value-of select="BizTalkBaseObject/EncryptionCert/Usage" />
                        </td>
                    </tr>
                </xsl:if>
                
			</table>
			
			<BR/><BR/>
			
			<!-- Primary Transport -->
			<table class="TableData">
			<tr>
			    <td width="10"></td>
			    <td><IMG SRC='../TransportSmall.jpg' VALIGN="center" width="16" height="16"/></td>
			    <td class="PageTitle3" colspan="2"><nobr>Primary Transport</nobr><BR/></td>
			</tr>
			</table>
			<xsl:choose>
			    <xsl:when test="string-length(BizTalkBaseObject/PrimaryTransport/Type)>0">
			        <table>
			        <tr>
			            <td width="10"></td>
			            <td width="145" class="TableTitle">Address:</td>
			            <td class="TableData"><xsl:value-of select="BizTalkBaseObject/PrimaryTransport/Address" /></td>
			        </tr>
			        <tr>
			            <td></td>
			            <td class="TableTitle">Type:</td>
			            <td class="TableData">
			                <xsl:choose>
			                    <xsl:when test="string-length(BizTalkBaseObject/PrimaryTransport/Type/@id)>0">
			                        <xsl:element name="A">
					                <xsl:attribute name="CLASS">TableData</xsl:attribute>
					                <xsl:attribute name="HREF">/Protocol/<xsl:value-of select="BizTalkBaseObject/PrimaryTransport/Type/@id" />.htm</xsl:attribute>
					                <xsl:value-of select="BizTalkBaseObject/PrimaryTransport/Type" />
				                    </xsl:element>
				                </xsl:when>
				                <xsl:otherwise><xsl:value-of select="BizTalkBaseObject/PrimaryTransport/Type" /></xsl:otherwise>
				            </xsl:choose>
				        </td>
			        </tr>
                <tr>
                  <td></td>
                  <td class="TableTitle">
                    <nobr>Send Handler:</nobr>
                  </td>
                  <td class="TableData">
                    <xsl:element name="A">
                      <xsl:attribute name="CLASS">TableData</xsl:attribute>
                      <xsl:attribute name="HREF">
                        /Host/<xsl:value-of select="BizTalkBaseObject/PrimaryTransport/SendHandler/Id" />.htm
                      </xsl:attribute>
                      <xsl:value-of select="BizTalkBaseObject/PrimaryTransport/SendHandler/Name" />
                    </xsl:element>
                  </td>
                </tr>
			        <tr>
			            <td></td>
			            <td class="TableTitle"><nobr>Ordered Delivery:</nobr></td>
			            <td class="TableData"><xsl:value-of select="BizTalkBaseObject/PrimaryTransport/OrderedDelivery" /></td>
			        </tr>
			        <tr>
			            <td></td>
			            <td class="TableTitle"><nobr>Retry Count:</nobr></td>
			            <td class="TableData"><xsl:value-of select="BizTalkBaseObject/PrimaryTransport/RetryCount" /></td>
			        </tr>
			        <tr>
			            <td></td>
			            <td class="TableTitle"><nobr>Retry Interval:</nobr></td>
			            <td class="TableData"><xsl:value-of select="BizTalkBaseObject/PrimaryTransport/RetryInterval" /></td>
			        </tr>
			        <tr>
			            <td></td>
			            <td class="TableTitle">
			                <nobr>
			                <xsl:choose>
			                    <xsl:when test="BizTalkBaseObject/PrimaryTransport/DataSpecified='true'">
			                        <xsl:element name="A">
					                <xsl:attribute name="CLASS">TableTitle</xsl:attribute>
					                <xsl:attribute name="HREF">/SendPort/<xsl:value-of select="BizTalkBaseObject/Id" />PTData.xml</xsl:attribute>
					                Custom Data
				                    </xsl:element>
				                </xsl:when>
				                <xsl:otherwise>No Custom Data Specified</xsl:otherwise>
				            </xsl:choose>
				            </nobr>
				        </td>
			            <td class="TableData"></td>
			        </tr>
			        </table>
        			<BR/><BR/>
			        <table class="TableData">
			        <tr>
			            <td width="10"></td>
			            <td width="145" class="TableTitle">Service Window<BR/><BR/></td>
			            <td width="10"></td>
			        </tr>
			        <tr>
			            <td></td>
			            <td class="TableTitle">Enabled:</td>
			            <td class="TableData"><xsl:value-of select="BizTalkBaseObject/PrimaryTransport/ServiceWindow/Enabled" /></td>
			        </tr>
			        <tr>
			            <td></td>
			            <td class="TableTitle">Start Date:</td>
			            <td class="TableData"><xsl:value-of select="substring(BizTalkBaseObject/PrimaryTransport/ServiceWindow/StartTime,1,10)" /><xsl:text> </xsl:text><xsl:value-of select="substring(BizTalkBaseObject/PrimaryTransport/ServiceWindow/StartTime,12,8)" /></td>
			        </tr>
			        <tr>
			            <td></td>
			            <td class="TableTitle">End Date:</td>
			            <td class="TableData"><xsl:value-of select="substring(BizTalkBaseObject/PrimaryTransport/ServiceWindow/EndTime,1,10)" /><xsl:text> </xsl:text><xsl:value-of select="substring(BizTalkBaseObject/PrimaryTransport/ServiceWindow/EndTime,12,8)" /></td>
			        </tr>
			    </table>
			    </xsl:when>
			    <xsl:otherwise>
			        <table>
			        <tr>
			            <td width="10"></td>
			            <td class="TableTitle">Not Yet Specified</td>
			            <td class="TableTitle"></td>
			            <td class="TableData"></td>
			        </tr>
			    </table>
			    </xsl:otherwise>
			</xsl:choose> 
			
			<BR/><BR/>
			
			<!-- =================== -->
			<!-- Secondary Transport -->
			<!-- =================== -->
			<table class="TableData">
			<tr>
			    <td width="10"></td>
			    <td><IMG SRC='../TransportSmall.jpg' VALIGN="center" width="16" height="16"/></td>
			    <td class="PageTitle3" colspan="2"><nobr>Secondary Transport</nobr><BR/></td>
			</tr>
			</table>
			<xsl:choose>
			    <xsl:when test="string-length(BizTalkBaseObject/SecondaryTransport/Type)>0">
			        <table>
			        <tr>
			            <td width="10"></td>
			            <td width="145" class="TableTitle">Address:</td>
			            <td class="TableData"><xsl:value-of select="BizTalkBaseObject/SecondaryTransport/Address" /></td>
			        </tr>
			        <tr>
			            <td></td>
			            <td class="TableTitle">Type:</td>
			            <td class="TableData">
			                <xsl:choose>
			                    <xsl:when test="string-length(BizTalkBaseObject/SecondaryTransport/Type/@id)>0">
			                        <xsl:element name="A">
					                <xsl:attribute name="CLASS">TableData</xsl:attribute>
					                <xsl:attribute name="HREF">/Protocol/<xsl:value-of select="BizTalkBaseObject/SecondaryTransport/Type/@id" />.htm</xsl:attribute>
					                <xsl:value-of select="BizTalkBaseObject/SecondaryTransport/Type" />
				                    </xsl:element>
				                </xsl:when>
				                <xsl:otherwise><xsl:value-of select="BizTalkBaseObject/SecondaryTransport/Type" /></xsl:otherwise>
				            </xsl:choose>
				        </td>
			        </tr>
              <tr>
                  <td></td>
                  <td class="TableTitle">
                    <nobr>Send Handler:</nobr>
                  </td>
                  <td class="TableData">
                    <xsl:element name="A">
                      <xsl:attribute name="CLASS">TableData</xsl:attribute>
                      <xsl:attribute name="HREF">
                        /Host/<xsl:value-of select="BizTalkBaseObject/SecondaryTransport/SendHandler/Id" />.htm
                      </xsl:attribute>
                      <xsl:value-of select="BizTalkBaseObject/SecondaryTransport/SendHandler/Name" />
                    </xsl:element>
                  </td>
                </tr>                 
			        <tr>
			            <td></td>
			            <td class="TableTitle"><nobr>Ordered Delivery:</nobr></td>
			            <td class="TableData"><xsl:value-of select="BizTalkBaseObject/SecondaryTransport/OrderedDelivery" /></td>
			        </tr>
			        <tr>
			            <td></td>
			            <td class="TableTitle"><nobr>Retry Count:</nobr></td>
			            <td class="TableData"><xsl:value-of select="BizTalkBaseObject/SecondaryTransport/RetryCount" /></td>
			        </tr>
			        <tr>
			            <td></td>
			            <td class="TableTitle"><nobr>Retry Interval:</nobr></td>
			            <td class="TableData"><xsl:value-of select="BizTalkBaseObject/SecondaryTransport/RetryInterval" /></td>
			        </tr>
			        <tr>
			            <td></td>
			            <td class="TableTitle">
			                <nobr>
			                <xsl:choose>
			                    <xsl:when test="BizTalkBaseObject/SecondaryTransport/DataSpecified='true'">
			                        <xsl:element name="A">
					                <xsl:attribute name="CLASS">TableTitle</xsl:attribute>
					                <xsl:attribute name="HREF">/SendPort/<xsl:value-of select="BizTalkBaseObject/Id" />PTData.xml</xsl:attribute>
					                Custom Data
				                    </xsl:element>
				                </xsl:when>
				                <xsl:otherwise>No Custom Data Specified</xsl:otherwise>
				            </xsl:choose>
				            </nobr>
				        </td>
			            <td class="TableData"></td>
			        </tr>
			        </table>
        			<BR/><BR/>
			        <table class="TableData">
			        <tr>
			            <td width="10"></td>
			            <td width="145" class="TableTitle">Service Window<BR/><BR/></td>
			            <td width="10"></td>
			        </tr>
			        <tr>
			            <td></td>
			            <td class="TableTitle">Enabled:</td>
			            <td class="TableData"><xsl:value-of select="BizTalkBaseObject/SecondaryTransport/ServiceWindow/Enabled" /></td>
			        </tr>
			        <tr>
			            <td></td>
			            <td class="TableTitle">Start Date:</td>
			            <td class="TableData"><xsl:value-of select="substring(BizTalkBaseObject/SecondaryTransport/ServiceWindow/StartTime,1,10)" /><xsl:text> </xsl:text><xsl:value-of select="substring(BizTalkBaseObject/SecondaryTransport/ServiceWindow/StartTime,12,8)" /></td>
			        </tr>
			        <tr>
			            <td></td>
			            <td class="TableTitle">End Date:</td>
			            <td class="TableData"><xsl:value-of select="substring(BizTalkBaseObject/SecondaryTransport/ServiceWindow/EndTime,1,10)" /><xsl:text> </xsl:text><xsl:value-of select="substring(BizTalkBaseObject/SecondaryTransport/ServiceWindow/EndTime,12,8)" /></td>
			        </tr>
			    </table>
			    </xsl:when>
			    <xsl:otherwise>
			        <table>
			        <tr>
			            <td width="10"></td>
			            <td class="TableTitle">Not Yet Specified</td>
			            <td class="TableTitle"></td>
			            <td class="TableData"></td>
			        </tr>
			        </table>
			    </xsl:otherwise>
			</xsl:choose> 
			
			<!-- Filter groups -->	
			<xsl:if test="count(BizTalkBaseObject/FilterGroups/FilterGroup)>0">			
			<BR/><BR/>
			    <Table class="TableData" cellpadding="0" cellspacing="0">
			    <tr>
			        <td width="10"></td>
			        <td width="16"><IMG SRC='../FilterSmall.jpg' VALIGN="center" width="16" height="16"/></td>
			        <td class="PageTitle3" colspan="5">Filters</td>
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
			                <td><nobr><xsl:value-of select="./Value" /></nobr></td>        
			            </tr>
			        </xsl:for-each>
			    </xsl:for-each>
			    </Table>
			</xsl:if>
			
			<!-- Outbound Maps -->
			<xsl:if test="count(BizTalkBaseObject/OutboundMaps/Transform)>0">		
			<BR/><BR/>	
			    <Table class="TableData">
			    <tr>
			        <td width="10"></td>
			        <td class="PageTitle3" colspan="5">Outbound Maps</td>
			    </tr>
			    
			    <xsl:for-each select="BizTalkBaseObject/OutboundMaps/Transform">
			        <tr>
			            <td width="10"></td>
			            <td><IMG SRC='../MapSmall.jpg' VALIGN="center" width="16" height="16"/></td>
			            <td><xsl:element name="A">
					        <xsl:attribute name="CLASS">TableData</xsl:attribute>
					        <xsl:attribute name="HREF">/Map/<xsl:value-of select="Id" />.html</xsl:attribute><xsl:value-of select="Name" /></xsl:element><br/><br/></td>
			        </tr>		
			    </xsl:for-each>
			    
			    </Table>
			</xsl:if>
			<BR/><BR/>
			
			<!-- Inbound Maps -->
			<xsl:if test="count(BizTalkBaseObject/InboundMaps/Transform)>0">		
			<BR/><BR/>	
			    <Table class="TableData">
			    <tr>
			        <td width="10"></td>
			        <td class="PageTitle3" colspan="5">Inbound Maps</td>
			    </tr>
			    
			    <xsl:for-each select="BizTalkBaseObject/InboundMaps/Transform">
			        <tr>
			            <td width="10"></td>
			            <td><IMG SRC='../MapSmall.jpg' VALIGN="center" width="16" height="16"/></td>
			            <td><xsl:element name="A">
					        <xsl:attribute name="CLASS">TableData</xsl:attribute>
					        <xsl:attribute name="HREF">/Map/<xsl:value-of select="Id" />.html</xsl:attribute><xsl:value-of select="Name" /></xsl:element><br/><br/></td>
			        </tr>
			    </xsl:for-each>
			    
			    </Table>
			</xsl:if>
			<BR/><BR/>
			
			<!-- Bound Orchestrations -->
			<xsl:if test="count(BizTalkBaseObject/BoundOrchestrations/Orchestration)>0">	
			<BR/>
			    <Table class="TableData" border="0">
			    <tr>
			        <td width="10"></td>
			        <td class="PageTitle3" colspan="2">Orchestrations bound to this send port</td>
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
                        Send Port : <xsl:value-of select="./BizTalkBaseObject/Name"/>
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
