<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:user="http://www.sunaptic.com/xslt"
	exclude-result-prefixes="xsl msxsl user" >

	<!-- **********************************************************************************************
	Title:			BizTalk Map Documenter Stylesheet (HTML Output)
	Source:			http://biztalkmapdoc.codeplex.com/
	Description:	This stylesheet is designed to generate an XHTML file that documents a BizTalk map (.btm file).  
	
	History:
	Steve Hart		20 Feb 2006  Original.  See http://www.hartsteve.com/2006/02/20/biztalk-map-documenter/
	Charlie Mott	30 Sep 2008	 Output label data.  Also updated table layout and css.
	Mark Abrams		15 Oct 2008  Output constants.  Also added map paging seperation.
  Mark Brimble  14 Mar 2013 1)Fixed Extension function parameters or return values which have CLR type ‘ConcatString’ are not supported.” error.
                            2) Enabled content for Constant functoids or links into functoids
                            3) Added change to get functoid names from BizTalk 2010 maps. I have not added all of them yet.
  Mark Brimble   9 Apr 2014 Added all functoids and DATACOM custom functoids
	*********************************************************************************************** -->

	<!-- Want XHTML output. -->
	<xsl:output
		method="xml"
		indent="yes"
		doctype-system="http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"
		doctype-public="-//W3C//DTD XHTML 1.0 Transitional//EN"
		encoding="iso-8859-1" />

	<!--============================================================
	Main Template Entry Point
	=============================================================-->
	<xsl:template match="/">

		<xsl:variable name="destSchema" select="/mapsource/TrgTree/Reference/@Location" />

		<html>
			<head>
				<title>
					Map To <xsl:value-of select="$destSchema"/>
				</title>
				<style type="text/css">
					/*
					Branding Colour palette.
					Blue light				R:070 G:155 B:200	#469bc8
					Blue dark				R:050 G:075 B:118	#324b76
					Shade 1	(dark mauve)	R:129 G:158 B:187	#819ebb
					Shade 2	(mauve)			R:174 G:200 B:216	#aec8d8
					Shade 3	(light mauve)	R:215 G:231 B:239	#d7e7ef
					Grey dark				R:070 G:070 B:070	#464646
					Grey medium				R:165 G:165 B:165	#a5a5a5
					Grey light				R:190 G:190 B:190	#bcbfbe
					*/

					body	{ font-family: Arial }
					hr		{ border: 0; height: 2px; width: 100%; }
					table	{ width: 100%; font-size: small; }
					th		{ color: white;	background-color: #324b76; text-align: left;}
					h1		{ color: #469bc8; font-family: Arial; }
					h2		{ color: #324b76; font-family: Arial; }
					h3		{ color: #464646; font-family: Arial; }

					#pageTitle { color: #469bc8; font-size: 20px; font-weight: bold }

					.titleArea { border-left: 5px solid #333366; padding-left: 5px }
					.titleSchema { color: #324b76; font-size: 14px; font-weight: bold }
					.linkTableAltRow { 	background-color: #ededed; }
					.linkTableDiffNodeRow { background-color: #C8E2FF }
					.linkTableCol { border-bottom: 1px solid black; background-color: #CCCC99 }
					.functoidArea { font-family: Lucida Console, Courier New; font-size: 11px }
					.functoidName { color: blue }
					.functoidBracket { color: green; font-weight: bold }
					.functoidScript { color: #003300 }
					.paramComma { color: blue; font-weight: bold }
					.internalLink { font-size: 10px }
					.collapseref { font-size: 10px; cursor:pointer }
				</style>
			</head>
			<body>
				<a name="top"></a>

				<div class="titleArea">
					<h1>BizTalk Map Documentation</h1>
					<table border="0">
						<tr class="titleSchema">
							<td>From Schema(s):</td>
							<td>
								<xsl:choose>
									<xsl:when test="/mapsource/SrcTree/Reference">
										<xsl:value-of select="/mapsource/SrcTree/Reference/@Location"/>
									</xsl:when>
									<xsl:when test="/mapsource/SrcTree/*[local-name()='schema']">
										<xsl:for-each select="/mapsource/SrcTree/*[local-name()='schema']/*[local-name()='import']">
											<xsl:if test="position() &gt; 1">
												<xsl:text>, </xsl:text>
											</xsl:if>
											<xsl:value-of select="@schemaLocation"/>
										</xsl:for-each>
									</xsl:when>
									<xsl:otherwise>
										Unable to determine source schema(s). Update stylesheet to correct this issue.
									</xsl:otherwise>
								</xsl:choose>
							</td>
						</tr>
						<tr class="titleSchema">
							<td>To Schema:</td>
							<td>
								<xsl:value-of select="$destSchema"/>
							</td>
						</tr>
					</table>
				</div>
				<a href="#functoids" class="internalLink">Functoids</a>
				<a href="#constantvalues" class="internalLink">Constant Values</a>
				<hr/>

				<!-- Process straight links (i.e. no functoid calls) -->
				<h2>Direct Node-to-Node Links (No Functoids)</h2>

				<!-- Apply template for each page -->
				<xsl:apply-templates select="/mapsource/Pages/Page" mode="direct" />

				<hr/>
				<a name="functoids"></a>
				<span style="font-size: 12px">^&#160;</span>
				<a href="#top" class="internalLink">Top</a>
				<br/>
				<br/>

				<!-- Process the functoid links -->
				<h2>Functoid Mappings</h2>

				<!-- Apply template for each page -->
				<xsl:apply-templates select="/mapsource/Pages/Page" mode="functoids" />

				<hr/>
				<a name="constantvalues"></a>
				<span style="font-size: 12px">^&#160;</span>
				<a href="#top" class="internalLink">Top</a>
				<br/>
				<br/>

				<!-- Process the constant values -->
				<h2>Constant Values</h2>

				<!-- Apply template (constant values are not assigned to a page, just to the map itself) -->
				<xsl:apply-templates select="/mapsource/TreeValues/ConstantValues" />

			</body>

			<!-- Client-side Javascript -->
			<script type='text/javascript'>
				<![CDATA[
				
	function toggleCollapse(obj, objAnchor) {
	var el = document.getElementById(obj);
	var elAnchor = document.getElementById(objAnchor);
	if ( el.style.display != "none" ) {
	  el.style.display = 'none';
	  elAnchor.innerHTML = 'Expand';
	}
	else {
	  el.style.display = '';
	  elAnchor.innerHTML = 'Collapse';
	}
	}
	
				]]>
			</script>

		</html>
	</xsl:template>

	<!--======================================================================
	Template: Mapping Page (Direct Links)
	=======================================================================-->
	<xsl:template match="Page" mode="direct">

		<!-- Create ids for a div element and anchor that will be unique in the document -->
		<xsl:variable name="divId" select="concat('pageDirect', position())" />
		<xsl:variable name="divIdAnchor" select="concat('pageDirect', position(), 'Anchor')" />
		<h3>
			<xsl:value-of select="concat('Page ', position(), ': ', @Name)"/>
		</h3>
		<p>
			<a id="{$divIdAnchor}" onclick="toggleCollapse('{$divId}', '{$divIdAnchor}');" title="Collapse" class="collapseref">Collapse</a>
		</p>
		<div id="{$divId}">
			<table>
				<tr>
					<th width="40%">From</th>
					<th width="40%">To</th>
					<th width="20%">Label</th>
				</tr>

				<!-- Apply template for each direct link in the page -->
				<xsl:apply-templates select="./Links/Link[not( number(@LinkFrom) or number(@LinkTo) )]" mode="direct" />

			</table>
		</div>

	</xsl:template>

	<!--======================================================================
	Template: Mapping Page (Functoid Links)
	=======================================================================-->
	<xsl:template match="Page" mode="functoids">

		<!-- Create ids for a div element and anchor that will be unique in the document -->
		<xsl:variable name="divId" select="concat('pageFunctoids', position())" />
		<xsl:variable name="divIdAnchor" select="concat('pageFunctoids', position(), 'Anchor')" />

		<h3>
			<xsl:value-of select="concat('Page ', position(), ': ', @Name)"/>
		</h3>
		<p>
			<a id="{$divIdAnchor}" onclick="toggleCollapse('{$divId}', '{$divIdAnchor}');" title="Collapse" class="collapseref">Collapse</a>
		</p>
		<div id="{$divId}">
			<table>
				<tr>
					<th width="40%">Functoid Sequence</th>
					<th width="40%">Destination Node</th>
					<th width="20%">Labels</th>
				</tr>

				<!-- Apply template for each functoid in the page -->
				<xsl:apply-templates select="./Links/Link[number(@LinkFrom) &gt; 0 and not( number(@LinkTo) )]" mode="functoids" />

			</table>
		</div>

	</xsl:template>

	<!--======================================================================
	Template: Constant Values
	=======================================================================-->
	<xsl:template match="ConstantValues" >

		<!-- Create ids for a div element and anchor that will be unique in the document -->
		<xsl:variable name="divId" select="'constant1Values'" />
		<xsl:variable name="divIdAnchor" select="concat('constant1Values', 'Anchor')" />

		<p>
			<a id="{$divIdAnchor}" onclick="toggleCollapse('{$divId}', '{$divIdAnchor}');" title="Collapse" class="collapseref">Collapse</a>
		</p>
		<div id="{$divId}">
			<table>
				<tr>
					<th width="40%">Value</th>
					<th width="60%">Destination Node</th>
				</tr>

				<!-- Apply template for each constant value -->
				<xsl:apply-templates select="./Value" />

			</table>
		</div>

	</xsl:template>

	<!--======================================================================
	Template: Mapping Link (Direct Links)
	=======================================================================-->
	<xsl:template match="Link" mode="direct">

		<xsl:variable name="linkFrom" select="user:parseLinkPath( string(@LinkFrom) )" />
		<xsl:variable name="linkTo" select="user:parseLinkPath( string(@LinkTo) )" />
		<xsl:variable name="linkToLabel" select="@Label" />

		<xsl:variable name="isHL7NodeDiffLink" select="user:isHL7NodeDiffLink( $linkFrom, $linkTo )" />

		<xsl:choose>
			<xsl:when test="$isHL7NodeDiffLink">
				<tr class="linkTableDiffNodeRow">
					<td>
						<xsl:value-of select="$linkFrom" />
					</td>
					<td>
						<xsl:value-of select="$linkTo" />
					</td>
					<td>
						<xsl:value-of select="$linkToLabel" />
					</td>
				</tr>
			</xsl:when>

			<xsl:when test="position() mod 2 = 0">
				<tr class="linkTableAltRow">
					<td>
						<xsl:value-of select="$linkFrom" />
					</td>
					<td>
						<xsl:value-of select="$linkTo" />
					</td>
					<td>
						<xsl:value-of select="$linkToLabel" />
					</td>
				</tr>
			</xsl:when>

			<xsl:otherwise>
				<tr>
					<td>
						<xsl:value-of select="$linkFrom" />
					</td>
					<td>
						<xsl:value-of select="$linkTo" />
					</td>
					<td>
						<xsl:value-of select="$linkToLabel" />
					</td>
				</tr>
			</xsl:otherwise>
		</xsl:choose>

	</xsl:template>

	<!--======================================================================
	Template: Mapping Link (Functoid Links)
	=======================================================================-->
	<xsl:template match="Link" mode="functoids">

		<xsl:variable name="linkFrom" select="@LinkFrom" />
		<xsl:variable name="linkFromFunctoidLabel" select="@Label" />
		<xsl:variable name="functoidNode" select="//Functoids/Functoid[@FunctoidID = $linkFrom]" />

		<xsl:if test="position() mod 2 = 0">
			<tr class="linkTableAltRow">
				<td>
					<div class="functoidArea">
						<xsl:call-template name="ProcessFunctoid">
							<xsl:with-param name="functoidNode" select="$functoidNode" />
							<xsl:with-param name="callLevel" select="number(1)" />
						</xsl:call-template>
					</div>
				</td>
				<td>
					<xsl:value-of select="user:parseLinkPath( string(@LinkTo) )" />
				</td>
				<td>
					<xsl:call-template name="ProcessLinksToFunctoidLabels">
						<xsl:with-param name="functoidNode" select="$functoidNode" />
					</xsl:call-template>
					<xsl:if test="string-length($functoidNode/@Label)!=0">
						<xsl:value-of select="$functoidNode/@Label"/>
						<br />
					</xsl:if>
					<xsl:value-of select="$linkFromFunctoidLabel" />
				</td>
			</tr>
		</xsl:if>
		<xsl:if test="not( position() mod 2 = 0 )">
			<tr>
				<td>
					<div class="functoidArea">
						<xsl:call-template name="ProcessFunctoid">
							<xsl:with-param name="functoidNode" select="$functoidNode" />
							<xsl:with-param name="callLevel" select="number(1)" />
						</xsl:call-template>
					</div>
				</td>
				<td>
					<xsl:value-of select="user:parseLinkPath( string(@LinkTo) )" />
				</td>
				<td>
					<xsl:call-template name="ProcessLinksToFunctoidLabels">
						<xsl:with-param name="functoidNode" select="$functoidNode" />
					</xsl:call-template>
					<xsl:if test="string-length($functoidNode/@Label)!=0">
						<xsl:value-of select="$functoidNode/@Label"/>
						<br />
					</xsl:if>
					<xsl:value-of select="$linkFromFunctoidLabel" />
				</td>
			</tr>
		</xsl:if>

	</xsl:template>

	<!--======================================================================
	Template: Constant Value
	=======================================================================-->
	<xsl:template match="Value">

		<xsl:variable name="value" select="@value" />
		<xsl:variable name="linkTo" select="user:parseLinkPath( string(@Query) )" />

		<xsl:choose>
			<xsl:when test="position() mod 2 = 0">
				<tr class="linkTableAltRow">
					<td>
						<xsl:value-of select="$value" />
					</td>
					<td>
						<xsl:value-of select="$linkTo" />
					</td>
				</tr>
			</xsl:when>

			<xsl:otherwise>
				<tr>
					<td>
						<xsl:value-of select="$value" />
					</td>
					<td>
						<xsl:value-of select="$linkTo" />
					</td>
				</tr>
			</xsl:otherwise>
		</xsl:choose>

	</xsl:template>

	<!--======================================================================
	Template: ProcessFunctoid
	Description:
	Works from destination node back through all connected functoids
	making recursive calls to this template when another functoid is 
	encountered.
	=======================================================================-->
	<xsl:template name="ProcessFunctoid">
		<xsl:param name="functoidNode" />
		<xsl:param name="callLevel" />

		<xsl:variable name="numberOfSpaces" select="number( ($callLevel - 1) * 3 )" />

		<xsl:value-of select="user:getHtmlSpaces( $numberOfSpaces )" disable-output-escaping="yes" />

		<span class="functoidName">
			<!--<xsl:value-of select="$functoidNode/@Functoid-Name" />-->
      <!--Added by Mahindra 2011/08/31-->
      <xsl:choose>
        <xsl:when test="$functoidNode/@Functoid-FID = 101">String Find</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 102">String Left</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 103">Lowercase</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 104">String Right</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 105">Size</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 106">String Extract</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 107">String Concatenate</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 108">String Left Trim</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 109">String Right Trim</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 110">Uppercase</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 111">Absolute Value</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 112">Integer</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 113">Maximum Value</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 114">Minimum Value</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 115">Modulo</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 116">Round</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 117">Square Root</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 118">Addition</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 119">Subtraction</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 120">Multiplication</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 121">Division</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 122">Add Days</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 123">Date</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 124">Time</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 125">Date and Time</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 126">Character to ASCII</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 127">ASCII to Character</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 128">HexaDecimal</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 129">Octal</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 130">Arc Tangent</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 131">Cosine</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 132">X^Y</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 133">Sine</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 134">Tangent</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 135">Natural Exponential Function</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 136">Natural Logarithm</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 137">10^n</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 138">Common Logarithm</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 139">Base-Specified Logarithm</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 260">Scripting</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 311">Greater Than</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 312">Greater Than or Equal To</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 313">Less Than</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 314">Less Than or Equal To</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 315">Equal</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 316">Not Equal</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 317">Logical String</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 318">Logical Date</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 319">Logical Numeric</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 320">Logical OR</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 321">Logical And</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 322">Record Count</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 323">Index</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 324">Cumulative Sum</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 325">Cumulative Average</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 326">Cumulative Minimum</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 327">Cumulative Maximum</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 328">Cumulative Concatenate</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 374">Value Mapping(Flattening)</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 375">Value Mapping</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 376">Nil value</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 424">Looping</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 474">Iteration</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 574">Database Lookup</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 575">Error Return</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 701">Logical Existence</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 703">Table Looping</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 704">Table Extractor</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 705">Logical Not</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 707">Assert</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 802">Mass Copy</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 5000">Value Extractor</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 5001">Format Message</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 5002">Get Application ID</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 5003">Get Common ID</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 5004">Get Common Value</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 5005">Set Common ID</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 5006">Remove Application ID</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 5006">Remove Application ID</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 54009">RcvPort Context Accesor</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 54011">String DateTime</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 54012">HumanReadableDateToEpoch</xsl:when>
        <xsl:when test="$functoidNode/@Functoid-FID = 54013">EpochToHumanReadableDate</xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$functoidNode/@Functoid-FID" />
        </xsl:otherwise>
      </xsl:choose>
      
    </span>

		<!-- Spit out all script code. -->
		<xsl:for-each select="$functoidNode/ScripterCode/Script">
			<span class="functoidScript">
				<xsl:value-of select="user:getHtmlSpaces( $numberOfSpaces )" disable-output-escaping="yes" />
				<xsl:choose>
					<xsl:when test="@Language = 'ExternalAssembly'">
						<br/>
						<xsl:value-of select="concat( @Class, '.', @Function )" />
					</xsl:when>
					<xsl:otherwise>
						<br/>
						<xsl:value-of select="." />
					</xsl:otherwise>
				</xsl:choose>
			</span>
		</xsl:for-each>

		<xsl:variable name="paramCount" select="count($functoidNode/Input-Parameters/Parameter)" />

		<!-- Starting functoid bracket. -->
		<xsl:choose>
			<xsl:when test="$paramCount = 1">
				<span class="functoidBracket">
					<xsl:value-of select="'( '" />
				</span>
			</xsl:when>
			<xsl:when test="$paramCount &gt; 1">
				<br/>
				<xsl:value-of select="user:getHtmlSpaces( $numberOfSpaces )" disable-output-escaping="yes" />
				<span class="functoidBracket">
					<xsl:value-of select="'('" />
				</span>
				<br/>
			</xsl:when>
		</xsl:choose>

		<!-- List the functoid parameters. -->
		<xsl:for-each select="$functoidNode/Input-Parameters/Parameter">
			<xsl:choose>
				<xsl:when test="@Type = 'link'">
					<xsl:variable name="linkId" select="@Value" />
					<xsl:variable name="linkFrom" select="//Links/Link[@LinkID = $linkId]/@LinkFrom" />
					<xsl:variable name="linkTo" select="//Links/Link[@LinkID = $linkId]/@LinkTo" />
					<xsl:choose>
						<xsl:when test="number($linkFrom)">
							<!-- Parameter is a link from another functoid; apply functoid template recursively. -->
							<xsl:call-template name="ProcessFunctoid">
								<xsl:with-param name="functoidNode" select="//Functoids/Functoid[@FunctoidID = $linkFrom]" />
								<xsl:with-param name="callLevel" select="number( $callLevel + 1 )" />
							</xsl:call-template>
						</xsl:when>
						<xsl:otherwise>
							<!-- Parameter is a path; list it. -->
							<xsl:if test="$paramCount &gt; 1">
								<!-- When more than 1 parameter, parameters are indented. -->
								<xsl:value-of select="user:getHtmlSpaces( $numberOfSpaces + 3 )" disable-output-escaping="yes" />
							</xsl:if>
							<xsl:value-of select="user:parseLinkPath( string($linkFrom) )" />
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
				<xsl:when test="@Type = 'constant'">
					<xsl:value-of select="concat( '&quot;', @Value, '&quot;' )" />
				</xsl:when>
			</xsl:choose>
			<xsl:if test="position() &lt; $paramCount">
				<span class="paramComma">,</span>
				<br/>
			</xsl:if>
		</xsl:for-each>

		<!-- Closing functoid bracket. -->
		<xsl:choose>
			<xsl:when test="$paramCount = 1">
				<span class="functoidBracket">
					<xsl:value-of select="' )'"/>
				</span>
			</xsl:when>
			<xsl:when test="$paramCount &gt; 1">
				<br/>
				<xsl:value-of select="user:getHtmlSpaces( $numberOfSpaces )" disable-output-escaping="yes" />
				<span class="functoidBracket">
					<xsl:value-of select="')'"/>
				</span>
			</xsl:when>
		</xsl:choose>

	</xsl:template>

	<!--======================================================================
	Template: ProcessLinksToFunctoidLabels
	Description:
	Works from destination node back through all connected functoids
	making recursive calls to this template when another functoid is 
	encountered.
	=======================================================================-->
	<xsl:template name="ProcessLinksToFunctoidLabels">
		<xsl:param name="functoidNode" />

		<!--<xsl:variable name="linksToFunctoidLabel" select="@Label" />
		<xsl:variable name="functoidLabel" select="$functoidNode/@Label" />	
		<xsl:variable name="linkFromFunctoidLabel" select="@Label" />-->

		<xsl:for-each select="$functoidNode/Input-Parameters/Parameter">
			<xsl:choose>
				<xsl:when test="@Type = 'link'">
					<xsl:variable name="linkId" select="@Value" />
					<xsl:if test="string-length(//Links/Link[@LinkID = $linkId]/@Label)!=0">
						<xsl:value-of select="//Links/Link[@LinkID = $linkId]/@Label" />
						<br />
					</xsl:if>
				</xsl:when>
			</xsl:choose>
		</xsl:for-each>

	</xsl:template>

	<!--======================================================================
	Custom script functions used to provide functionality that cannot be 
	achieved (or not easily achieved) using XPath 1.0 functions.
	=======================================================================-->
	<msxsl:script language="JScript" implements-prefix="user">
		<![CDATA[
		
	// Parses a supplied link path returning a the path without the clutter of the
	// "local-name()" XPath statements.
	//
	function parseLinkPath( linkPath ) {
		// We want to parse out all the local-name() nodes, excluding what appears to 
		// be the standard first node ('Schema') for BizTalk maps.
		//
		var parsedLink = "";
		var nodeName = "";
		var startPos = -1;
		var endPos = -1;
		var nodeCount = 0;
		
		startPos = linkPath.indexOf( "'", 0 );
		
		// Find all single quoted nodes, extract them and put them back together in 
		// a path which excludes all the "local-name()=" xpath stuff.  There's probably
		// an easier way of doing this (like, perhaps, converting the string to an XPath
		// statement and using XPath-related methods to "clean" the path)?
		//
		while ( startPos > -1 ) {
			nodeCount++;
			endPos = linkPath.indexOf( "'", startPos+1 );
			if ( endPos > -1 ) {
				nodeName = linkPath.substring( startPos+1, endPos );

				// Ignore first node named 'Schema' which seems to be the standard (in BizTalk) 
				// first node in the path for all maps?
				//
				if ( (nodeCount > 1) || (nodeName != "<Schema>") ) {
					if ( parsedLink.length > 0 ) {
						parsedLink += "/";
					}
					parsedLink += nodeName;
				}
			}
			startPos = linkPath.indexOf( "'", endPos+1 );
		}
		if ( parsedLink.length == 0 ) {
			// Just in case nothing was parsed from the supplied link path.
			//
			parsedLink = linkPath;
		}
    //return "" + parsedLink; MTB 2011/08/30
		return "" + parsedLink;
	}

	// Determines if the source and destination links are HL7 links and if the
	// linked nodes are different.
	//
	function isHL7NodeDiffLink( fromLink, toLink ) {
		var isDiffNodeLink = false;
		var fromLinkIsHL7 = false;
		var toLinkIsHL7 = false;
		var fromLinkNodes = fromLink.split( "/" );
		var toLinkNodes = toLink.split( "/" );
		if ( (fromLinkNodes[0].search(/GLO_DEF$/) > -1) && (toLinkNodes[0].search(/GLO_DEF$/) > -1) ) {
			var fromNode = fromLinkNodes[ fromLinkNodes.length - 1 ];
			var toNode = toLinkNodes[ toLinkNodes.length - 1 ];
			// From observations so far, it appears that all the schema node names generated/created 
			// via the HL7 accelerator have an underscore following the field/segment identifier.
			var fromNodeId = fromNode.substring( 0, fromNode.indexOf("_") );
			var toNodeId = toNode.substring( 0, toNode.indexOf("_") );
			if ( fromNodeId != toNodeId ) {
				isDiffNodeLink = true;
			}
		}
		return isDiffNodeLink;
	}
	
	// Builds a string of HTML non-breaking spaces.
	//	
	function getHtmlSpaces( spacesCount ) {
		var spaces = "";
		for ( var i = 0; i < spacesCount; i++ ) {
			spaces += "&nbsp;";
		}
    //return spaces; MTB 2010/08/31
		return "" + spaces;
	}

	]]>
	</msxsl:script>

</xsl:stylesheet>