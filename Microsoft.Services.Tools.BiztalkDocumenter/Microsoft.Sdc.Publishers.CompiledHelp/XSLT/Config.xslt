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
            <SPAN CLASS="StartOfFile">Installation Configuration</SPAN>

            <xsl:call-template name="header" /> 
            
            <!-- Questions -->   
            <xsl:if test="count(Configuration/Question)>0">
            <Table class="TableData" border="0">
            <tr>
                <td width="10"></td>
                <td class="PageTitle3" colspan="4">Installation Questions and Answers</td>
            </tr>
            <tr>
                <td width="10"></td>
                <td class="TableTitle" width="500"><nobr>Question</nobr></td>
                <td width="10"></td>
                <td class="TableTitle">Answer</td>
                <td></td>
            </tr>

            <xsl:for-each select="Configuration/Question">
                <tr>
                    <td width="10"></td>
                    <td><xsl:value-of select="@Text" /></td>
                    <td width="10"></td>
                    <td>

                        <xsl:choose>
                            <xsl:when test="string-length(.//Answer[@Selected='Yes']/@Value)>0">
                                <xsl:value-of select=".//Answer[@Selected='Yes']/@Value" />
                            </xsl:when>
                            <xsl:otherwise><xsl:value-of select="@Default" /></xsl:otherwise>
                        </xsl:choose>

                    </td>
                </tr>
            </xsl:for-each>
            </Table>   
            </xsl:if>
            
            <!-- BTS Hosts-->    
            <xsl:if test="count(Configuration/Name)>0">        
            <BR/>
            <Table class="TableData" border="0">
            <tr>
                <td width="10"></td>
                <td class="PageTitle3" colspan="2">BizTalk Hosts Configuration</td>
            </tr>
            <tr>
                <td width="10"></td>
                <td class="TableTitle" width="300">Name</td>
                <td width="10"></td>
                <td class="TableTitle">Value</td>
                <td width="10"></td>
                <td class="TableTitle" width="500">Description</td>
            </tr>

            <xsl:for-each select="Configuration/Name">
                <xsl:sort select="@DisplayName" />
                <tr>
                    <td width="10"></td>
                    <td valign="top"><nobr><xsl:value-of select="@DisplayName" /></nobr></td>
                    <td width="10"></td>
                    <td valign="top" width="170"><nobr><xsl:value-of select="Value" /></nobr></td>                        
                    <td width="10"></td>
                    <td valign="top"><xsl:value-of select="@Description" /></td>                        
                    <td width="10"></td>
                </tr>
            </xsl:for-each>
            </Table> 
            </xsl:if>
            
            <!-- Services -->            
            <xsl:if test="count(Configuration/NTService)>0">
            <BR/>
            <Table class="TableData" border="0" width="1500">
            <tr>
                <td width="10"></td>
                <td class="PageTitle3" colspan="2">Windows Services Configuration</td>
            </tr>
            <tr>
                <td width="10"></td>
                <td class="TableTitle" width="300">Service Name</td>
                <td width="10"></td>
                <td class="TableTitle">Identity</td>
                <td width="10"></td>
                <td class="TableTitle" width="500">Description</td>
            </tr>

            <xsl:for-each select="Configuration/NTService">
                <xsl:sort select="@DisplayName" />
                <tr>
                    <td width="10"></td>
                    <td valign="top"><xsl:value-of select="@DisplayName" /></td>
                    <td width="10"></td>
                    <td valign="top" width="170">

                        <xsl:choose>
                            <xsl:when test="string-length(Domain)>0">
                                <xsl:value-of select="Domain" />
                            </xsl:when>
                            <xsl:otherwise>.</xsl:otherwise>
                        </xsl:choose>                    
                        \
                        <xsl:value-of select="UserName" /></td>
                        
                    
                    <td width="10"></td>
                    <td valign="top"><xsl:value-of select="@Description" /></td>
                </tr>
            </xsl:for-each>
            </Table> 
            </xsl:if>   
            
            <!-- Service Account-->     
            <xsl:if test="count(Configuration/NTCredential)>0">       
            <BR/>
            <Table class="TableData" border="0" width="1500">
            <tr>
                <td width="10"></td>
                <td class="PageTitle3" colspan="5">Service Account Configuration</td>
            </tr>
            <tr>
                <td width="10"></td>
                <td class="TableTitle" width="300">BizTalk Group Name</td>
                <td width="10"></td>
                <td class="TableTitle">NT Group Name</td>
                <td width="10"></td>
                <td class="TableTitle" width="500">Description</td>
            </tr>

            <xsl:for-each select="Configuration/NTCredential">
                <xsl:sort select="@DisplayName" />
                <tr>
                    <td width="10"></td>
                    <td valign="top"><nobr><xsl:value-of select="@DisplayName" /></nobr></td>
                    <td width="10"></td>
                    <td width="170" valign="top"><nobr><xsl:value-of select="NTAccount" /></nobr></td>                        
                    <td width="10"></td>
                    <td valign="top"><xsl:value-of select="@Description" /></td>                        
                    <td width="10"></td>
                </tr>
            </xsl:for-each>
            </Table> 
            </xsl:if>
            
            <!-- SQL -->   
            <xsl:if test="count(Configuration/SQL)>0">         
            <BR/>
            <Table class="TableData" border="0" width="1500">
            <tr>
                <td width="10"></td>
                <td class="PageTitle3" colspan="2">Database Configuration</td>
            </tr>
            <tr>
                <td width="10"></td>
                <td class="TableTitle">Database</td>
                <td width="10"></td>
                <td class="TableTitle">Servver Name</td>
                <td width="10"></td>
                <td class="TableTitle">Database Name</td>
            </tr>

            <xsl:for-each select="Configuration/SQL">
                <xsl:sort select="@DisplayName" />
                <tr>
                    <td width="10"></td>
                    <td width="300"><xsl:value-of select="@DisplayName" /></td>
                    <td width="10"></td>
                    <td width="170"><xsl:value-of select="Server" /></td>
                    <td width="10"></td>
                    <td><xsl:value-of select="Database" /></td>
                    <td width="10"></td>
                </tr>
            </xsl:for-each>
            </Table> 
            </xsl:if>      
            
            
            <!-- MSMQT -->                       
            <xsl:if test="count(Configuration/MSMQT)>0">   
            <br/>
            <table class="TableData" border="0">
            <tr>
                <td width="10"></td>
                <td class="PageTitle3" colspan="2">MSMQT Configuration</td>
            </tr>
            <tr>
                <td width="10"></td>
                <td class="TableData" colspan="2"><xsl:value-of select="Configuration/MSMQT/@Description" /><br/><br/></td>
            </tr>
            <tr>
                <td width="10"></td>
                <td width="175" class="TableTitle"><nobr>IP Address:</nobr></td>
                <td class="TableData"><xsl:value-of select="Configuration/MSMQT/IPAddress" /></td>
            </tr>
            <tr>
                <td></td>
                <td class="TableTitle"><nobr>Server:</nobr></td>
                <td class="TableData"><xsl:value-of select="Configuration/MSMQT/Server" /></td>
            </tr>
            <tr>
                <td></td>
                <td class="TableTitle"><nobr>RegisterInDNS:</nobr></td>
                <td class="TableData"><xsl:value-of select="Configuration/MSMQT/RegisterInDNS" /></td>
            </tr>
            <tr>
                <td></td>
                <td class="TableTitle"><nobr>Integrate MQ With AD:</nobr></td>
                <td class="TableData"><xsl:value-of select="Configuration/MSMQT/IntegrateMQWithAD" /></td>
            </tr>
            <tr>
                <td></td>
                <td class="TableTitle"><nobr>Domain:</nobr></td>
                <td class="TableData"><xsl:value-of select="Configuration/MSMQT/Domain" /></td>
            </tr>
            <tr>
                <td></td>
                <td class="TableTitle"><nobr>MQ Router:</nobr></td>
                <td class="TableData"><xsl:value-of select="Configuration/MSMQT/MQRouter" /></td>
            </tr>
            </table>
            </xsl:if>
            
            <!-- Features -->            
            <xsl:if test="count(Configuration/InstalledFeature)>0">
            <BR/>
            <Table class="TableData" border="0">
            <tr>
                <td width="10"></td>
                <td class="PageTitle3" colspan="2">Installed Features</td>
            </tr>
            <tr>
                <td width="10"></td>
                <td class="TableTitle">   
                    <ul>
                    <xsl:for-each select="Configuration/InstalledFeature">
                        <xsl:sort select="text()" />
                            <li class="TableData"><xsl:value-of select="text()" /></li>
                    </xsl:for-each>
                    </ul>
                </td>
            </tr>
            </Table> 
            </xsl:if>  
            
                                    
            <xsl:call-template name="footer" /> 
            
            </body>
        </html>
    </xsl:template>
    
    <xsl:template name="header">
        <BR/><BR/>
        <table>
        <tr>
            <td width="10"></td>
            <td valign="center" CLASS="PageTitle">Installation Configuration</td>
        </tr>
        </table>
        <BR/>
    </xsl:template>
    
    <xsl:template name="footer">
        <BR/><BR/><BR/><BR/><HR CLASS="Rule"/><P CLASS="Copyright">Generated on: <xsl:value-of select="$GenDate"/><BR/>Microsoft.Services.Tools.BiztalkDocumenter version: <xsl:value-of select="$DocVersion"/></P><BR/>       
    </xsl:template> 


</xsl:stylesheet>
