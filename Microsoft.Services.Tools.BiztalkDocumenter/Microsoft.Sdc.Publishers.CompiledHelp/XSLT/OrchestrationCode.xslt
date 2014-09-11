<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:om="http://schemas.microsoft.com/BizTalk/2003/DesignerData">	

<xsl:param name="OrchName" />
 	      
<xsl:output method="html" indent="no"/>

	<xsl:template match="/">
		<html>
			<HEAD>
				<link href="../CommentReport.css" type="text/css" rel="stylesheet"></link>
			</HEAD>
			
			<body>

                <xsl:call-template name="header" />
			
			<!-- Code -->
			<xsl:apply-templates />
						
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
                        <img src="../Orchestration.gif" width="24" height="24"  align="middle"/>
                    </nobr>
                </td>
                <td width="100%" background="../topBackground.jpg" valign="middle" CLASS="PageTitle">
                    <nobr>Orchestration : <xsl:value-of select="$OrchName"/></nobr>
                </td>
                <td background="../topBackground.jpg" valign="middle" align="right">
                    <IMG SRC="../topRight.jpg" ALIGN="middle"/>
                </td>
            </tr>
        </table>
        <BR/>

    </xsl:template>
	
	<xsl:template match="//om:Element[@Type='VariableAssignment']">
        <table width="98%">
            <tr>
                <td>
                    <xsl:call-template name="CodeElement">
                        <xsl:with-param name="Caption">Expression Shape</xsl:with-param>
                        <xsl:with-param name="Code">
                            <xsl:value-of select="om:Property[@Name='Expression']/@Value" />
                        </xsl:with-param>
                    </xsl:call-template>
                </td>
            </tr>
        </table>
        <br/>
        <br/>
	</xsl:template>
	
	<xsl:template match="//om:Element[@Type='MessageAssignment']">
        <table width="98%">
                <tr>
                    <td>
                        <xsl:call-template name="CodeElement">
                            <xsl:with-param name="Caption">Message Assignment Shape</xsl:with-param>
                            <xsl:with-param name="Code">
                                <xsl:value-of select="om:Property[@Name='Expression']/@Value" />
                            </xsl:with-param>
                        </xsl:call-template>
                    </td>
                </tr>
            </table>
            <br/>
            <br/>
        </xsl:template>

  <xsl:template match="//om:Element[@Type='DecisionBranch']">
    <table width="98%">
      <tr>
        <td>
          <xsl:call-template name="CodeElement">
            <xsl:with-param name="Caption">
              <xsl:value-of select="concat('Decision Branch : ',../om:Property[@Name='Name']/@Value)" />
            </xsl:with-param>
            <xsl:with-param name="Desc">
              <xsl:value-of select="om:Property[@Name='AnalystComments']/@Value" />
            </xsl:with-param>
            <xsl:with-param name="Code">
              <xsl:value-of select="om:Property[@Name='Expression']/@Value" />
              <xsl:for-each select="om:Element">
                Sub shape: <xsl:value-of select="om:Property[@Name='Name']/@Value"/>
              </xsl:for-each>
            </xsl:with-param>
          </xsl:call-template>
        </td>
      </tr>
    </table>
    <br/>
    <br/>
    <xsl:apply-templates select="om:Element" />
  </xsl:template>


  <xsl:template match="//om:Element[@Type='Send']">
    <table width="98%">
      <tr>
        <td>
          <xsl:call-template name="CodeElement">
            <xsl:with-param name="Caption">Send Shape</xsl:with-param>
            <xsl:with-param name="Desc">
              <xsl:value-of select="om:Property[@Name='AnalystComments']/@Value" />
            </xsl:with-param>
            <xsl:with-param name="Code">
              Sends message '<xsl:value-of select="om:Property[@Name='MessageName']/@Value" />' to port '<xsl:value-of select="om:Property[@Name='PortName']/@Value" />'
              <xsl:variable name="PortOID">
                <xsl:value-of select="@OID"/>
              </xsl:variable>
              <xsl:for-each select="//om:Element[@Type='ServiceDeclaration']
                                               /om:Element[@Type='CorrelationDeclaration']
                                      
                                               /om:Element[@Type='StatementRef']
                                                     /om:Property[@Name='Ref' and @Value=$PortOID]">
                <xsl:choose>
                  <xsl:when test="../om:Property[@Name='Initializes']/@Value = 'False'">
                    Following
                  </xsl:when>
                  <xsl:otherwise>
                    Initializing
                  </xsl:otherwise>
                </xsl:choose> Correlation: <xsl:value-of select ="../../om:Property[@Name='Type']/@Value"/>
              </xsl:for-each>
            </xsl:with-param>
          </xsl:call-template>
        </td>
      </tr>
    </table>
    <br/>
    <br/>
  </xsl:template>

  <xsl:template match="//om:Element[@Type='Receive']">
    <table width="98%">
      <tr>
        <td>
          <xsl:call-template name="CodeElement">
            <xsl:with-param name="Caption">Receive Shape</xsl:with-param>
            <xsl:with-param name="Desc">
              <xsl:value-of select="om:Property[@Name='AnalystComments']/@Value" />
            </xsl:with-param>
            <xsl:with-param name="Code">
              Receives message '<xsl:value-of select="om:Property[@Name='MessageName']/@Value" />' from port '<xsl:value-of select="om:Property[@Name='PortName']/@Value" />'
              <xsl:for-each select="om:Element[@Type='DNFPredicate']">
                Filter: <xsl:value-of select="concat(om:Property[@Name='LHS']/@Value,' ')"/>
                <xsl:value-of select="concat(om:Property[@Name='Operator']/@Value,' ')"/>
                <xsl:value-of select="concat(om:Property[@Name='RHS']/@Value,' ')"/>
                <xsl:value-of select="om:Property[@Name='Grouping']/@Value"/>
              </xsl:for-each>
              <xsl:variable name="PortOID">
                <xsl:value-of select="@OID"/>
              </xsl:variable>

              <xsl:for-each select="//om:Element[@Type='ServiceDeclaration']
                                               /om:Element[@Type='CorrelationDeclaration']
                                      
                                               /om:Element[@Type='StatementRef']
                                                     /om:Property[@Name='Ref' and @Value=$PortOID]">
                <xsl:choose>
                  <xsl:when test="../om:Property[@Name='Initializes']/@Value = 'False'">
                    Following
                  </xsl:when>
                  <xsl:otherwise>
                    Initializing
                  </xsl:otherwise>
                </xsl:choose> Correlation: <xsl:value-of select ="../../om:Property[@Name='Type']/@Value"/>
              </xsl:for-each>

            </xsl:with-param>
          </xsl:call-template>
        </td>
      </tr>
    </table>
    <br/>
    <br/>
  </xsl:template>

  <xsl:template name="CodeElement">

        <xsl:param name="Caption" />
        <xsl:param name="Desc" />
        <xsl:param name="Code" />

        <table border="0" cellspacing="0" cellpadding="0" width="98%" align="left">
            <tr>
                <td width="1">
                    <xsl:element name="A">
                        <xsl:attribute name="ID">
                            <xsl:value-of select="@OID" />
                        </xsl:attribute>
                    </xsl:element>
                    <img src="../topSpacer.gif"/>
                </td>
                <td>

                    <table border="1" cellspacing="0" cellpadding="0" width="100%" style="border: 1px;" align="left">
                        <tr>
                            <td>
                                <table border="0" cellspacing="0" cellpadding="2" width="100%">
                                    <tr>
                                        <td class="tableTitle" bgcolor="#c0c0c0" color="#DADBFC">
                                            <xsl:value-of select="$Caption" />: <xsl:value-of select="om:Property[@Name='Name']/@Value" />
                                        </td>
                                    </tr>
                                    <xsl:if test="string-length($Desc)>0">
                                        <tr>
                                            <td class="tableData"><xsl:value-of select="$Desc" /></td>
                                        </tr>
                                    </xsl:if>
                                    <xsl:if test="string-length($Code)>0">
                                        <tr>
                                            <td class="tableData" style="font-size=8pt; font-family: courier new;" bgcolor="#ECEAEC"><pre style="font-size=8pt;"><xsl:value-of select="$Code" /></pre></td>
                                        </tr>
                                    </xsl:if>
                                </table>
                            </td>
                        </tr>
                    </table>

                </td>
            </tr>
        </table>
        
    </xsl:template>

  <xsl:template match="//om:Element[@Type='Delay']">
    <table width="98%">
      <tr>
        <td>
          <xsl:call-template name="CodeElement">
            <xsl:with-param name="Caption">Delay</xsl:with-param>
            <xsl:with-param name="Desc">
              <xsl:value-of select="om:Property[@Name='AnalystComments']/@Value" />
            </xsl:with-param>
            <xsl:with-param name="Code">
              <xsl:value-of select="om:Property[@Name='Timeout']/@Value" />
            </xsl:with-param>
          </xsl:call-template>
        </td>
      </tr>
    </table>
    <br/>
    <br/>
  </xsl:template>

  <xsl:template match="//om:Element[@Type='Throw']">
    <table width="98%">
      <tr>
        <td>
          <xsl:call-template name="CodeElement">
            <xsl:with-param name="Caption">Throw Exception</xsl:with-param>
            <xsl:with-param name="Desc">
              <xsl:value-of select="om:Property[@Name='AnalystComments']/@Value" />
            </xsl:with-param>
            <xsl:with-param name="Code">
              <xsl:value-of select="om:Property[@Name='ThrownReference']/@Value" />
            </xsl:with-param>
          </xsl:call-template>
        </td>
      </tr>
    </table>
    <br/>
    <br/>
  </xsl:template>


  <xsl:template match="//om:Element[@Type='CallRules']">
    <table width="98%">
      <tr>
        <td>
          <xsl:call-template name="CodeElement">
            <xsl:with-param name="Caption">Call Rules</xsl:with-param>
            <xsl:with-param name="Desc">
              <xsl:value-of select="om:Property[@Name='PolicyName']/@Value" />
            </xsl:with-param>
            <xsl:with-param name="Code">
              <xsl:for-each select="om:Element[@Type='RulesParameterRef']">
                Input: <xsl:value-of select="om:Property[@Name='Reference']/@Value" />
              </xsl:for-each>
            </xsl:with-param>
          </xsl:call-template>
        </td>
      </tr>
    </table>
    <br/>
    <br/>
  </xsl:template>

  <xsl:template match="//om:Element[@Type='Task']">
    <table width="98%">
      <tr>
        <td>
          <xsl:call-template name="CodeElement">
            <xsl:with-param name="Caption">
              Group
              <xsl:value-of select="/om:Property[@Name='Name']/@Value" />
            </xsl:with-param>
            <xsl:with-param name="Desc">
              <xsl:value-of select="om:Property[@Name='AnalystComments']/@Value" />
            </xsl:with-param>
            <xsl:with-param name="Code">
              <xsl:for-each select="om:Element">
                Sub shape: <xsl:value-of select="om:Property[@Name='Name']/@Value"/>
              </xsl:for-each>
            </xsl:with-param>
          </xsl:call-template>
        </td>
      </tr>
    </table>
    <br/>
    <br/>
    <xsl:apply-templates select="om:Element" />
  </xsl:template>

  <xsl:template match="//om:Element[@Type='Suspend']">
    <table width="98%">
      <tr>
        <td>
          <xsl:call-template name="CodeElement">
            <xsl:with-param name="Caption">Suspend</xsl:with-param>
            <xsl:with-param name="Desc">
              <xsl:value-of select="om:Property[@Name='AnalystComments']/@Value" />
            </xsl:with-param>
            <xsl:with-param name="Code">
              <xsl:value-of select="om:Property[@Name='ErrorMessage']/@Value" />
            </xsl:with-param>
          </xsl:call-template>
        </td>
      </tr>
    </table>
    <br/>
    <br/>
  </xsl:template>

  <xsl:template match="//om:Element[@Type='Terminate']">
    <table width="98%">
      <tr>
        <td>
          <xsl:call-template name="CodeElement">
            <xsl:with-param name="Caption">Terminate</xsl:with-param>
            <xsl:with-param name="Desc">
              <xsl:value-of select="om:Property[@Name='AnalystComments']/@Value" />
            </xsl:with-param>
            <xsl:with-param name="Code">
              <xsl:value-of select="om:Property[@Name='ErrorMessage']/@Value" />
            </xsl:with-param>
          </xsl:call-template>
        </td>
      </tr>
    </table>
    <br/>
    <br/>
  </xsl:template>

  <xsl:template match="//om:Element[@Type='Compensate']">
    <table width="98%">
      <tr>
        <td>
          <xsl:call-template name="CodeElement">
            <xsl:with-param name="Caption">Compensate</xsl:with-param>
            <xsl:with-param name="Desc">
              <xsl:value-of select="om:Property[@Name='AnalystComments']/@Value" />
            </xsl:with-param>
            <xsl:with-param name="Code">
              Compensation: <xsl:value-of select="om:Property[@Name='Invokee']/@Value" />
            </xsl:with-param>
          </xsl:call-template>
        </td>
      </tr>
    </table>
    <br/>
    <br/>
  </xsl:template>


  <xsl:template match="//om:Element[@Type='Call']">
    <table width="98%">
      <tr>
        <td>
          <xsl:call-template name="CodeElement">
            <xsl:with-param name="Caption">Call Orchestration</xsl:with-param>
            <xsl:with-param name="Desc">
              <xsl:value-of select="om:Property[@Name='AnalystComments']/@Value" />
            </xsl:with-param>
            <xsl:with-param name="Code">
              Called Orchestration: <xsl:value-of select="om:Property[@Name='Invokee']/@Value" />
            </xsl:with-param>
          </xsl:call-template>
        </td>
      </tr>
    </table>
    <br/>
    <br/>
  </xsl:template>

  <xsl:template match="text()">
	</xsl:template>

</xsl:stylesheet>
