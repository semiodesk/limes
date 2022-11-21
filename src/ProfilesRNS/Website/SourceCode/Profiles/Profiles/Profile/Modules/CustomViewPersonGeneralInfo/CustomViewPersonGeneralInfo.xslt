<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"  xmlns:geo="http://aims.fao.org/aos/geopolitical.owl#" xmlns:afn="http://jena.hpl.hp.com/ARQ/function#" xmlns:prns="http://profiles.catalyst.harvard.edu/ontology/prns#" xmlns:obo="http://purl.obolibrary.org/obo/" xmlns:dcelem="http://purl.org/dc/elements/1.1/" xmlns:dcterms="http://purl.org/dc/terms/" xmlns:event="http://purl.org/NET/c4dm/event.owl#" xmlns:bibo="http://purl.org/ontology/bibo/" xmlns:vann="http://purl.org/vocab/vann/" xmlns:vitro07="http://vitro.mannlib.cornell.edu/ns/vitro/0.7#" xmlns:vitro="http://vitro.mannlib.cornell.edu/ns/vitro/public#" xmlns:vivo="http://vivoweb.org/ontology/core#" xmlns:pvs="http://vivoweb.org/ontology/provenance-support#" xmlns:scirr="http://vivoweb.org/ontology/scientific-research-resource#" xmlns:rdf="http://www.w3.org/1999/02/22-rdf-syntax-ns#" xmlns:rdfs="http://www.w3.org/2000/01/rdf-schema#" xmlns:xsd="http://www.w3.org/2001/XMLSchema#" xmlns:owl="http://www.w3.org/2002/07/owl#" xmlns:swvs="http://www.w3.org/2003/06/sw-vocab-status/ns#" xmlns:skco="http://www.w3.org/2004/02/skos/core#" xmlns:owl2="http://www.w3.org/2006/12/owl2-xml#" xmlns:skos="http://www.w3.org/2008/05/skos#" xmlns:foaf="http://xmlns.com/foaf/0.1/">
  <xsl:param name="email"/>
  <xsl:param name="emailAudio"/>
  <xsl:param name="emailAudioImg"/>
  <xsl:param name="root"/>
  <xsl:param name="imgguid"/>
  <xsl:param name="orcid"/>
  <xsl:param name="orcidurl"/>
  <xsl:param name="orcidinfosite"/>
  <xsl:param name="orcidimage"/>
  <xsl:param name="orcidimageguid"/>
  <xsl:param name="nodeid"/>

  <xsl:template match="/">
    <div class="vcard">
      <xsl:call-template name="Name"/>
    </div>
  </xsl:template>

  <!--=============Template for displaying Name table============-->
  <xsl:template name="Name">
    <xsl:if test="rdf:RDF/rdf:Description[@rdf:about= /rdf:RDF[1]/rdf:Description[1]/prns:personInPrimaryPosition/@rdf:resource]/prns:isPrimaryPosition !=''">
      <div class="itemprop">
        <label>Title</label>
        <div itemprop="jobTitle">
          <xsl:value-of select="rdf:RDF/rdf:Description[@rdf:about= /rdf:RDF[1]/rdf:Description[1]/prns:personInPrimaryPosition/@rdf:resource]/vivo:hrJobTitle "/>
        </div>
      </div>
    </xsl:if>
    <xsl:variable name="uriOrganization" select="rdf:RDF/rdf:Description[@rdf:about= /rdf:RDF[1]/rdf:Description[1]/prns:personInPrimaryPosition/@rdf:resource]/vivo:positionInOrganization/@rdf:resource"/>
    <xsl:if test="rdf:RDF/rdf:Description[@rdf:about=$uriOrganization]/rdfs:label !=''">
      <div class="itemprop">
        <label>Institution</label>
        <div itemprop="affiliation">
          <xsl:value-of select="rdf:RDF/rdf:Description[@rdf:about= $uriOrganization]/rdfs:label"/>
        </div>
      </div>
    </xsl:if>
    <xsl:variable name="uriDepartment" select="rdf:RDF/rdf:Description[@rdf:about= /rdf:RDF[1]/rdf:Description[1]/prns:personInPrimaryPosition/@rdf:resource]/prns:positionInDepartment/@rdf:resource"/>
    <xsl:if test="rdf:RDF/rdf:Description[@rdf:about=$uriDepartment]/rdfs:label !=''">
      <div class="itemprop">
        <label>Department</label>
        <div>
          <xsl:value-of select="rdf:RDF/rdf:Description[@rdf:about=$uriDepartment]/rdfs:label "/>
        </div>
      </div>
    </xsl:if>
    <xsl:if test="rdf:RDF[1]/rdf:Description[1]/vivo:mailingAddress/@rdf:resource !=''">
      <div class="itemprop">
        <label>Address</label>
        <div itemprop="address" itemscope="itemscope" itemtype="http://schema.org/PostalAddress">
          <span itemprop="streetAddress">
            <xsl:if test="rdf:RDF/rdf:Description[@rdf:about= /rdf:RDF[1]/rdf:Description[1]/vivo:mailingAddress/@rdf:resource]/vivo:address1 !=''">
              <xsl:value-of select="rdf:RDF/rdf:Description[@rdf:about= /rdf:RDF[1]/rdf:Description[1]/vivo:mailingAddress/@rdf:resource]/vivo:address1 "/>
              <br/>
            </xsl:if>
            <xsl:if test="rdf:RDF/rdf:Description[@rdf:about= /rdf:RDF[1]/rdf:Description[1]/vivo:mailingAddress/@rdf:resource]/vivo:address2 !=''">
              <xsl:value-of select="rdf:RDF/rdf:Description[@rdf:about= /rdf:RDF[1]/rdf:Description[1]/vivo:mailingAddress/@rdf:resource]/vivo:address2 "/>
              <br/>
            </xsl:if>
            <xsl:if test="rdf:RDF/rdf:Description[@rdf:about= /rdf:RDF[1]/rdf:Description[1]/vivo:mailingAddress/@rdf:resource]/vivo:address3 !=''">
              <xsl:value-of select="rdf:RDF/rdf:Description[@rdf:about= /rdf:RDF[1]/rdf:Description[1]/vivo:mailingAddress/@rdf:resource]/vivo:address3 "/>
              <br/>
            </xsl:if>
            <xsl:if test="rdf:RDF/rdf:Description[@rdf:about= /rdf:RDF[1]/rdf:Description[1]/vivo:mailingAddress/@rdf:resource]/vivo:addressCity !=''">
              <xsl:value-of select="rdf:RDF/rdf:Description[@rdf:about= /rdf:RDF[1]/rdf:Description[1]/vivo:mailingAddress/@rdf:resource]/vivo:addressCity"/>
              <xsl:text> </xsl:text>
              <xsl:value-of select="rdf:RDF/rdf:Description[@rdf:about= /rdf:RDF[1]/rdf:Description[1]/vivo:mailingAddress/@rdf:resource]/vivo:addressState"/>
              <xsl:text> </xsl:text>
              <xsl:value-of select="rdf:RDF/rdf:Description[@rdf:about= /rdf:RDF[1]/rdf:Description[1]/vivo:mailingAddress/@rdf:resource]/vivo:addressPostalCode"/>
              <br/>
            </xsl:if>
          </span>
        </div>
      </div>
    </xsl:if>
    <xsl:if test="rdf:RDF[1]/rdf:Description[1]/vivo:phoneNumber !=''">
      <div class="itemprop">
        <label>Phone</label>
        <div itemprop="telephone">
          <xsl:value-of select="rdf:RDF[1]/rdf:Description[1]/vivo:phoneNumber"/>
        </div>
      </div>
    </xsl:if>
    <xsl:choose>
      <xsl:when test="rdf:RDF[1]/rdf:Description[1]/vivo:faxNumber !=''">
        <div class="itemprop">
          <label>Fax</label>
          <div>
            <xsl:value-of select="rdf:RDF[1]/rdf:Description[1]/vivo:faxNumber"/>
          </div>
        </div>
      </xsl:when>
    </xsl:choose>
    <xsl:choose>
      <xsl:when test="$email!=''">
        <div class="itemprop">
          <label>Email</label>
          <div>
            <img id="{$imgguid}" src="{$email}&amp;rnd={$imgguid}" alt=""></img>
            <!--<a href="{$emailAudio}&amp;rnd={$imgguid}">
              <img src="{$emailAudioImg}" alt="Listen to email address" />
            </a>-->
          </div>
        </div>
      </xsl:when>
      <xsl:otherwise>
        <xsl:choose>
          <xsl:when test="rdf:RDF[1]/rdf:Description[1]/vivo:email !=''">
            <div class="itemprop">
              <label>Email</label>
              <div>
                <a href="mailto:{rdf:RDF[1]/rdf:Description[1]/vivo:email}" itemprop="email">
                  <xsl:value-of select="rdf:RDF[1]/rdf:Description[1]/vivo:email"/>
                </a>
              </div>
            </div>
          </xsl:when>
        </xsl:choose>
      </xsl:otherwise>
    </xsl:choose>
    <xsl:choose>
      <xsl:when test="$orcid!=''">
        <div class="itemprop">
          <label>
            ORCID
            <img id="{$orcidimageguid}" src="{$orcidimage}" alt="ORCID Icon" style="vertical-align:text-bottom"/>
          </label>
          <div>
            <a href="{$orcidurl}" target="_blank">
              <xsl:value-of select="$orcid "/>
            </a>
            <xsl:text disable-output-escaping="yes">&#160;</xsl:text><a style="border: none;" href="{$orcidinfosite}" target='_blank'>
              <img style='border-style: none' src="{$root}/Framework/Images/info.png"  border='0' alt='Additional info'/>
            </a>
          </div>
        </div>
      </xsl:when>
    </xsl:choose>

    <a href="{$root}/profile/modules/CustomViewPersonGeneralInfo/vcard.aspx?subject={$nodeid}" class='btn btn-primary mt-2'>
		Download vCard
    </a>
  </xsl:template>
</xsl:stylesheet>
