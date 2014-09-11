<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:re="http://schemas.microsoft.com/businessruleslanguage/2002"  exclude-result-prefixes="re">	

<xsl:param name="GenDate" />
<xsl:param name="DocVersion" />

<xsl:output method="html" indent="no"/>
		
	<xsl:template match="/">
		<html>
			<head>
				<link href="CommentReport.css" type="text/css" rel="stylesheet"></link>
				<script language="javaScript">

					function ShowItem( itemName )
					{
						eval( 'var itemStyle = ' + itemName + '.style' )

						if( itemStyle.display=="none")
							itemStyle.display="block";
						else
							itemStyle.display="none";
					}

				</script>
			</head>

			<body>			

					<xsl:call-template name="header" /> 

					<table class="TableData">
					<tr>
						<td width="10"></td>
						<td width="145" class="TableTitle">Description:</td>
						<td class="TableData">
							<xsl:choose>
								<xsl:when test="string-length(re:brl/re:ruleset/re:version/@description)>0">
									<xsl:value-of select="re:brl/re:ruleset/re:version/@description"/>
								</xsl:when>
								<xsl:otherwise>N/A</xsl:otherwise>
							</xsl:choose>
						</td>
					</tr>
					<tr>
						<td width="10"></td>
						<td width="145" class="TableTitle">Version:</td>
						<td class="TableData">
							<xsl:value-of select="//re:brl/re:ruleset/re:version[last()]/@major"/>.<xsl:value-of select="//re:brl/re:ruleset/re:version[last()]/@minor"/>
						</td>
					</tr>
					</table>

					<BR/>

					<!-- Rules -->		
					<xsl:if test="count(re:brl/re:ruleset/re:rule)>0">	
						<ul>
							<xsl:apply-templates/>
						</ul>
					</xsl:if>

					<xsl:call-template name="footer" /> 
			</body>
		</html>
		
	</xsl:template>	
	
	<xsl:template match="re:rule">		
		<li type="square"><span class="TableTitle" style="cursor: hand;"><img src="RuleRule.jpg" width="16" height="16"/><xsl:text> </xsl:text><u>
			<xsl:element name="a">
				<xsl:attribute name="onClick">ShowItem( 'Item<xsl:number/>' );</xsl:attribute>
				Rule: - <xsl:value-of select="@name" />
			</xsl:element>

			</u></span>		

			<xsl:element name="div">
				<xsl:attribute name="id">Item<xsl:number/></xsl:attribute>
				<xsl:attribute name="style">display: block;</xsl:attribute>
					<ul><span class="TableData"><xsl:apply-templates/></span></ul>
			</xsl:element>
		</li>
		<br/><br/>
	</xsl:template>

	<!-- =============== -->	
	<!-- IF              -->
	<!-- =============== -->		
	<xsl:template match="re:if">		
		<br/>
		<ul>
			<span class="TableTitle">IF</span>
			<ul>
				<xsl:apply-templates/>
			</ul>
		</ul>
	</xsl:template>

	<!-- =============== -->	
	<!-- THEN            -->
	<!-- =============== -->		
	<xsl:template match="re:then">		
		<br/>
		<ul>
			<span class="TableTitle">THEN</span>
			<ul>
				<xsl:apply-templates/>
			</ul>
		</ul>
	</xsl:template>

	<!-- =============== -->	
	<!-- AND             -->
	<!-- =============== -->		
	<xsl:template match="re:and">
		<xsl:if test="position()>2">			
			<ul>
				<span class="TableTitle">AND</span>
				<ul>
					<xsl:apply-templates/>
				</ul>
			</ul>
		</xsl:if>
		
		<xsl:if test="position()&lt;=2">			
			<span class="TableTitle">AND</span>
			<ul>
				<xsl:apply-templates/>
			</ul>
		</xsl:if>
	</xsl:template>

	<!-- =============== -->	
	<!-- OR              -->
	<!-- =============== -->		
	<xsl:template match="re:or">
		<xsl:if test="position()>2">			
			<ul>
				<span class="TableTitle">OR</span>
				<ul>
					<xsl:apply-templates/>
				</ul>
			</ul>
		</xsl:if>
		
		<xsl:if test="position()&lt;=2">			
			<span class="TableTitle">OR</span>
			<ul>
				<xsl:apply-templates/>
			</ul>
		</xsl:if>
	</xsl:template>

	<!-- =============== -->	
	<!-- NOT             -->
	<!-- =============== -->		
	<xsl:template match="re:not">
		<xsl:if test="position()>2">			
			<ul>
				<span class="TableTitle">NOT</span>
				<ul>
					<xsl:apply-templates/>
				</ul>
			</ul>
		</xsl:if>
		
		<xsl:if test="position()&lt;=2">			
			<span class="TableTitle">NOT</span>
			<ul>
				<xsl:apply-templates/>
			</ul>
		</xsl:if>
	</xsl:template>

	<!-- =============== -->	
	<!-- CONSTANT        -->
	<!-- =============== -->		
	<xsl:template match="re:constant">
		<xsl:for-each select="node()">
			<xsl:call-template name="constantValue" />
		</xsl:for-each>
	</xsl:template>
	
	<xsl:template name="constantValue">
		<xsl:choose>
			<xsl:when test="local-name()='datetime'">
				<xsl:value-of select="substring(.,1,10)" /><xsl:text> </xsl:text><xsl:value-of select="substring(./text(),12,8)" />
			</xsl:when>
			<xsl:otherwise><xsl:value-of select="." /></xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	<!-- =============== -->	
	<!-- AFTER           -->
	<!-- =============== -->	
	<xsl:template match="re:after">		
		<li type="disc">
			<span class="TableData"><xsl:apply-templates select="re:time1" /></span>
			<span class="TableTitle"> is after </span>
			<span class="TableData"><xsl:apply-templates select="re:time2" /></span>
		</li>
	</xsl:template>

	<!-- =============== -->	
	<!-- BEFORE          -->
	<!-- =============== -->	
	<xsl:template match="re:before">	
		<li type="disc">
			<span class="TableData"><xsl:apply-templates select="re:time1" /></span>
			<span class="TableTitle"> is before </span>
			<span class="TableData"><xsl:apply-templates select="re:time2" /></span>
		</li>
	</xsl:template>
	
	<!-- =============== -->	
	<!-- BETWEEN         -->
	<!-- =============== -->	
	<xsl:template match="re:between">
		<li type="disc">
			<span class="TableData"><xsl:apply-templates select="re:time1" /></span>
			<span class="TableTitle"> is between </span>
			<span class="TableData"><xsl:apply-templates select="re:time2" /></span>
			<span class="TableTitle"> and </span>
			<span class="TableData"><xsl:apply-templates select="re:time3" /></span>
		</li>
	</xsl:template>
	
	<!-- =============== -->	
	<!-- COMPARE         -->
	<!-- =============== -->
	<xsl:template match="re:compare">
		<li type="disc">
			<xsl:apply-templates/>
		</li>
	</xsl:template>
	
	<!-- =============== -->	
	<!-- EXISTS          -->
	<!-- =============== -->
	<xsl:template match="re:exists">		
		Exists
		<xsl:apply-templates/>
	</xsl:template>
	
	<!-- =============== -->	
	<!-- MATCH           -->
	<!-- =============== -->	
	<xsl:template match="re:match">		
		<li type="disc">
			<span class="TableData"><xsl:apply-templates select="re:input/." /></span>
			<span class="TableTitle"> contains </span>
			<span class="TableData"><xsl:apply-templates select="re:pattern/." /></span>
		</li>
	</xsl:template>

	<!-- =============== -->	
	<!-- RANGE           -->
	<!-- =============== -->	
	<xsl:template match="re:range">		
		<li type="disc">
			<span class="TableData"><xsl:apply-templates select="re:testvalue/." /></span>
			<span class="TableTitle"> is between </span>
			<span class="TableData"><xsl:apply-templates select="re:rangelow/." /></span>
			<span class="TableTitle"> and </span>
			<span class="TableData"><xsl:apply-templates select="re:rangehigh/." /></span>
		</li>			
	</xsl:template>
	
	<!-- =============== -->	
	<!-- PREDICATE       -->
	<!-- =============== -->
	<xsl:template match="re:predicate">		
		<li type="disc">
			<xsl:apply-templates/>
		</li>
	</xsl:template>
	
	<!-- =============== -->	
	<!-- LHS             -->
	<!-- =============== -->
	<xsl:template match="re:lhs">
		<xsl:apply-templates/>
		<span class="TableTitle"><xsl:text> </xsl:text><xsl:value-of select="../@operator" /><xsl:text> </xsl:text></span>
	</xsl:template>
	
	<!-- =============== -->	
	<!-- RHS             -->
	<!-- =============== -->
	<xsl:template match="re:rhs">
		<xsl:apply-templates/>
	</xsl:template>

	<!-- =============== -->	
	<!-- FUNCTION        -->
	<!-- =============== -->
	<xsl:template match="re:function">
		<xsl:apply-templates/>
	</xsl:template>

	<!-- =============== -->	
	<!-- CLEAR           -->
	<!-- =============== -->
	<xsl:template match="re:clear">		
		<li><span class="TableTitle">Clear</span></li>
	</xsl:template>

	<!-- =============== -->	
	<!-- ASSERT          -->
	<!-- =============== -->
	<xsl:template match="re:assert">		
		<li><span class="TableTitle">Assert</span> <xsl:apply-templates/></li>
	</xsl:template>

	<!-- =============== -->	
	<!-- UPDATE          -->
	<!-- =============== -->
	<xsl:template match="re:update">		
		<li><span class="TableTitle">Update</span> <xsl:apply-templates/></li>
	</xsl:template>

	<!-- =============== -->	
	<!-- RETRACT         -->
	<!-- =============== -->
	<xsl:template match="re:retract">		
		<li><span class="TableTitle">Retract</span> <xsl:apply-templates/></li>
	</xsl:template>

	<!-- =============== -->	
	<!-- HALT            -->
	<!-- =============== -->
	<xsl:template match="re:halt">		
		<li><span class="TableTitle">Halt</span> <xsl:apply-templates/></li>
	</xsl:template>

	<!-- =============== -->	
	<!-- CLASSMEMBER     -->
	<!-- =============== -->	
	<xsl:template match="re:classmember">		
		<xsl:variable name="a"><xsl:value-of select="@classref" /></xsl:variable> 
		<xsl:value-of select="//re:bindings/re:class[@ref=$a]/re:namespace"/>.<xsl:value-of select="//re:bindings/re:class[@ref=$a]/@class"/>.<xsl:value-of select="@member"/>()
		<xsl:apply-templates/>
	</xsl:template>

	<!-- =============== -->	
	<!-- DATAROWMEMBER   -->
	<!-- =============== -->
	<xsl:template match="re:datarowmember">		
		<xsl:variable name="a"><xsl:value-of select="@datarowref" /></xsl:variable> 
		<xsl:value-of select="//re:bindings/re:datarow[@ref=$a]/@dataset"/>.<xsl:value-of select="//re:bindings/re:datarow[@ref=$a]/@table"/>.<xsl:value-of select="@column"/>
	</xsl:template>

	<!-- =============== -->	
	<!-- XMLMEMBER       -->
	<!-- =============== -->
	<xsl:template match="re:xmldocumentmember">		
		<xsl:variable name="a"><xsl:value-of select="@xmldocumentref" /></xsl:variable> 
		<xsl:value-of select="//re:bindings/re:xmldocument[@ref=$a]/@doctype"/>:<xsl:value-of select="//re:bindings/re:xmldocument[@ref=$a]/re:selectoralias"/>/<xsl:value-of select="re:fieldalias"/>
		<xsl:apply-templates/>
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
                        Policy : <xsl:value-of select="re:brl/re:ruleset/@name"/>
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
			
	<xsl:template match="text()">
	</xsl:template>
	
</xsl:stylesheet>
