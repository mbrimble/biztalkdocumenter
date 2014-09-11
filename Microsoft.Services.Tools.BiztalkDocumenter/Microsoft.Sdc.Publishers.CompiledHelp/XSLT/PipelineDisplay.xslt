<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:output method="html" indent="no" />
    
    <xsl:template match="/">
			<HEAD>
				<link href="../styles.css" type="text/css" rel="stylesheet"></link>
			</HEAD>
    
        <table class="TableData">
		<tr>
			<td width="10"></td>
			<td class="PageTitle"><nobr>Pipeline Process Flow:</nobr><BR /><BR /></td>
		</tr>
		<tr>
			<td width="10"></td>
			<td>
			    <xsl:apply-templates />
			</td>
		</tr>
		</table>	
            
    </xsl:template>
    
    <xsl:template match="Document/Stages/Stage">
        <UL>
            <xsl:apply-templates select="PolicyFileStage" />
            <OL>
                <xsl:apply-templates select="Components/Component" />
            </OL>
        </UL>
    </xsl:template>
    
    <xsl:template match="PolicyFileStage">
        <NOBR/>
        <xsl:choose>
            <xsl:when test="@Name='Pre-Assemble'"><IMG SRC="../PreAssemble.jpg"/> </xsl:when>
            <xsl:when test="@Name='Assemble'"><IMG SRC="../Assemble.jpg"/> </xsl:when>
            <xsl:when test="@Name='Encode'"><IMG SRC="../Encode.jpg"/> </xsl:when>
            <xsl:when test="@Name='Decode'"><IMG SRC="../Decode.jpg"/> </xsl:when>
            <xsl:when test="@Name='Disassemble'"><IMG SRC="../Disassemble.jpg"/> </xsl:when>
            <xsl:when test="@Name='Validate'"><IMG SRC="../Validate.jpg"/> </xsl:when>
            <xsl:when test="@Name='ResolveParty'"><IMG SRC="../ResolveParty.jpg"/> </xsl:when>
        </xsl:choose>
        <SPAN CLASS="TableTitle"><xsl:value-of select="@Name" /></SPAN><NOBR/><BR /><BR />
    </xsl:template>
    
    <xsl:template match="Component">
        <LI>
            <NOBR/><SPAN CLASS="TableTitle"><xsl:value-of select="ComponentName" /></SPAN><NOBR/>
            <BR />
            <NOBR/><SPAN><xsl:value-of select="Name" />></SPAN><NOBR/>
            <BR />
                
            <!--<xsl:if test="count(./Properties/Property[@Name='DocumentSpecName'])>0 and string-length(./Properties/Property[@Name='DocumentSpecName']/Value)>0">
                <BR /><SPAN CLASS="TableTitle">Document specification(s):</SPAN><NOBR/><BR /><BR />        
                <UL>
                    <xsl:for-each select="./Properties/Property[@Name='DocumentSpecName']">
                        <LI><NOBR/><SPAN><xsl:value-of select="Value" /></SPAN><NOBR/></LI>
                    </xsl:for-each>
                </UL>
            </xsl:if>
                
            <xsl:if test="count(./Properties/Property[@Name='DocumentSpecNames'])>0 and string-length(./Properties/Property[@Name='DocumentSpecNames']/Value)>0">
                <BR /><SPAN CLASS="TableTitle">Document specification(s):</SPAN><NOBR/><BR /><BR />        
                <UL>
                    <xsl:for-each select="./Properties/Property[@Name='DocumentSpecNames']">
                        <LI><NOBR/><SPAN><xsl:value-of select="Value" /></SPAN><NOBR/></LI>
                    </xsl:for-each>
                </UL>
            </xsl:if>
                
            <xsl:if test="count(./Properties/Property[@Name='EnvelopeSpecNames'])>0 and string-length(./Properties/Property[@Name='EnvelopeSpecNames']/Value)>0">
                <BR /><SPAN CLASS="TableTitle">Envelope specification(s):</SPAN><NOBR/><BR /><BR />        
                <UL>
                    <xsl:for-each select="./Properties/Property[@Name='EnvelopeSpecNames']">
                        <LI><NOBR/><SPAN><xsl:value-of select="Value" /></SPAN><NOBR/></LI>
                    </xsl:for-each>
                </UL>
            </xsl:if>-->
          <xsl:if test="count(./Properties/Property)>0 and string-length(./Properties/Property/Value)>0">
            <BR />
            <SPAN CLASS="TableTitle">Default Properties</SPAN>
            <NOBR/>
            <BR />
            <BR />
            <UL>
              <xsl:for-each select="./Properties/Property">
                <LI>
                  <NOBR/>
                  <SPAN>
                    <xsl:value-of select="@Name" /> : <xsl:value-of select="Value" />
                  </SPAN>
                  <NOBR/>
                </LI>
              </xsl:for-each>
            </UL>
          </xsl:if>


          <BR />
        </LI>
    </xsl:template>
    
    <xsl:template match="text()"></xsl:template>
    
</xsl:stylesheet>