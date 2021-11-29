<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="About.ascx.cs" Inherits="Profiles.About.Modules.About.About" %>
<div class="aboutText">
    <asp:Panel runat="server" ID="pnlOverview" Visible="false">
        <table>
            <tr>
                <td>
                    <h2>Overview</h2>
                    <p>
                        Profiles Research Networking Software is a research networking and expertise mining
                        software tool. It not only shows traditional directory information, but also illustrates
                        how each person is connected to others in the broad research community.
                    </p>
                    <p>
                        As you navigate through the website, you will see three types of pages:
                    </p>
                    <p>
                        <asp:Image runat="server" ID="imgProfilesIcon" alt=""></asp:Image>
                        <u>Profile Pages</u>
                        <div style="padding-left: 15px">
                            Each person has a Profile Page that includes his or her name, titles, affiliations,
                            and contact information. Faculty can edit their own profiles, adding publications,
                            awards, narrative, and a photo. Other objects, such as publications, journals, departments,
                            or concepts can have "profiles". This About page is a "profile" of the Profiles
                            Research Networking Software website.
                        </div>
                    </p>
                    <ul>
                        <li style="padding-bottom: 10px"><b>Passive Networks</b> - Passive networks are formed
                            automatically when faculty share common traits such as being in the same department,
                            working in the same building, co-authoring the same paper, or researching the same
                            concepts or topics. A preview of a person's passive networks is shown on the right
                            side of his or her profile.</li>
                        <li><b>Active Networks</b> - Active networks are the ones that you define. When users
                            who login to the website view other people's profiles, they can mark those people
                            as collaborators, advisors, or advisees. In other words, you can build your own
                            network of people that you know. Currently, you can only see the networks that you
                            build. In the future you will be able to share these lists with others. Active networks
                            are shown on your left sidebar.</li>
                    </ul>
                    <br />
                    <p>
                        <asp:Image runat="server" ID="imgNetworkIcon" alt=""/>
                        <u>Network Pages</u><br />
                        <div style="padding-left: 15px">
                            Network Pages show all the people in a particular Passive or Active Network. Networks
                            can also include other types of profiles, not just people. A "concept" network is
                            a list of all the topics a person has written about. There are many ways to display
                            a network other than a simple list, and Profiles offers several types of network
                            visualization tools.</div>
                    </p>
                    <p>
                        <asp:Image runat="server" ID="imgConnectionIcon" alt=""/>
                        <u>Connection Pages</u><br />
                        <div style="padding-left: 15px">
                            Certain Network Pages will include a "Why?" link. These will take you to a Connection
                            Page, which shows why two people or profiles in that network are connected. For
                            example, the Why link in a co-authorship network lists the publications that two
                            people wrote together. The Connection Pages also reveal why certain people appear
                            higher on search results and why particular concepts are highlighted on a person's
                            profile.
                        </div>
                    </p>
                    <h3>
                        Visualizations</h3>
                    <p>
                        Profiles Research Networking Software includes several different ways to view networks,
                        including (from left to right) Concept Clouds, which highlight a person's areas
                        of research; Map Views, which show where a person's co-authors are located; Publication
                        Timelines, which graph the number of publications of different types by year; Radial
                        Network Views, which illustrate clusters of connectivity among related people; and
                        Concept Timelines, which depict how a person's research focus has changed over time.
                    </p>
                    <p>
                        <div align="center">
                            <asp:Image runat="server" ID="imgVis" alt="visualization thumnails"/>
                        </div>
                    </p>
                    <h3>
                        Sharing Data</h3>
                    <p>
                        Profiles Research Networking Software is a Semantic Web application, which means
                        its content can be read and understood by other computer programs. This enables
                        the data in profiles, such as addresses and publications, to be shared with other
                        institutions and appear on other websites. If you click the "Export RDF" link on
                        the left sidebar of a profile page, you can see what computer programs see when
                        visiting a profile. For technical information about how build a computer program
                        that can export data from Profiles Research Networking Software, view the <a href="?tab=data">
                            Sharing Data</a> page.
                    </p>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlFAQ" Visible="false">
        <h2>Help</h2>
        <h4>How do I edit my profile?</h4>
        <p>
            To edit your profile, click the Edit My Profile link on the sidebar. You might be
            prompted to login. The Edit Menu page lists all the types of content that can be
            included on your profile. They are grouped into categories and listed in the same
            order as they appear when viewing your profile. Click any content type to view/edit
            the items or change the privacy settings. Some types of content are imported automatically
            from other systems, and you cannot edit them through Profiles. They appear with
            a "locked" icon and contain more information when you click them. An example of
            this is data that comes from your Human Resources record, such as affiliation, title,
            mailing address, and email address.
        </p>
        <p>
            To view your profile as others see it, click the View My Profile link on the sidebar.
        </p>
        <h4>
            Why are there missing or incorrect publications in my profile?
        </h4>
        <p>
            Publications are added both automatically from PubMed and manually by faculty themselves.
            Unfortunately, there is no easy way to match articles in PubMed to the profiles
            on this website. The algorithm used to find articles from PubMed attempts to minimize
            the number of publications incorrectly added to a profile; however, this method
            results in some missing publications. Faculty with common names or whose articles
            were written at other institutions are most likely to have incomplete publication
            lists. We encourage all faculty to login to the website and add any missing publications
            or remove incorrect ones.
        </p>
        <h4>
            Can I edit my concepts, co-authors, or list of similar people?
        </h4>
        <p>
            These are derived automatically from the PubMed articles listed with your profile.
            You cannot edit these directly, but you can improve these lists by keeping your
            publications up to date. Please note that it takes up to 24 hours for the system
            to update your concepts, co-authors, and similar people after you have modified
            your publications. Concept rankings and similar people lists are based on algorithms
            that weigh multiple factors, such as how many publications you have in a subject
            area compared to the total number of faculty who have published in that area. Your
            feedback is essential to helping us refine these algorithms. A future version of
            this website will allow users to add custom concepts to their profiles, but these
            will be separate from the automatically derived terms.
        </p>
        <h4>
            Who created Profiles Research Networking Software?
        </h4>
        <p>
            This service is made possible by the Profiles Research Networking Software developed
            under the supervision of Griffin M Weber, MD, PhD, with support from Grant Number
            1 UL1 RR025758-01 to Harvard Catalyst: The Harvard Clinical and Translational Science
            Center from the National Center for Research Resources and support from Harvard
            University and its affiliated academic healthcare centers.
        </p>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlData" Visible="false">
        <h2>Sharing Data (Export RDF)</h2>
        <p>
            Profiles Research Networking Software is a Semantic Web application, which means
            its content can be read and understood by other computer programs. This enables
            the data in profiles, such as addresses and publications, to be shared with other
            institutions and appear on other websites. If you click the "Export RDF" link on
            the left sidebar of a profile page, you can see what computer programs see when
            visiting a profile. The section below describes the technical details for building
            a computer program that can export data from Profiles Research Networking Software.
        </p>
        <h3>
            Technical Details</h3>
        <p>
            As a Semantic Web application, Profiles Research Networking Software uses the Resource
            Description Framework (RDF) data model. In RDF, every entity (e.g., person, publication,
            concept) is given a unique URI. (A URI is similar to a URL that you would enter
            into a web browser.) Entities are linked together using "triples" that contain three
            URIs--a subject, predicate, and object. For example, the URI of a Person can be
            connected to the URI of a Concept through a predicate URI of hasResearchArea. Profiles
            Research Networking Software contains millions of URIs and triples. Semantic Web
            applications use an ontology, which describes the classes and properties used to
            define entities and link them together. Profiles Research Networking Software uses
            the VIVO Ontology, which was developed as part of an NIH-funded grant to be a standard
            for academic and research institutions. A growing number of sites around the world
            are adopting research networking platforms that use the VIVO Ontology. Because RDF
            can link different triple-stores that use the same ontology, software developers
            are able to create tools that span multiple institutions and data sources. When
            RDF data is shared with the public, as it is in Profiles Research Networking Software,
            it is called Linked Open Data (LOD).
        </p>
        <p>
            There are four types of application programming interfaces (APIs) in Profiles Research
            Networking Software.
        </p>
        <ul>
            <li>RDF crawl. Because Profiles Research Networking Software is a Semantic Web application,
                every profile has both an HTML page and a corresponding RDF document, which contains
                the data for that page in RDF/XML format. Web crawlers can follow the links embedded
                within the RDF/XML to access additional content.</li>
            <li>SPARQL endpoint. SPARQL is a programming language that enables arbitrary queries
                against RDF data. This provides the most flexibility in accessing data; however,
                the downsides are the complexity in coding SPARQL queries and performance. In general,
                the XML Search API (see below) is better to use than SPARQL.
                <li>XML Search API. This is a web service that provides support for the most common
                    types of queries. It is designed to be easier to use and to offer better performance
                    than SPARQL, but at the expense of fewer options. It enables full-text search across
                    all entity types, faceting, pagination, and sorting options. The request message
                    to the web service is in XML format, but the output is in RDF/XML format.
                    <li>
            Old XML based web services. This provides backwards compatibility for institutions
            that built applications using the older version of Profiles Research Networking
            Software. These web services do not take advantage of many of the new features of
            Profiles Research Networking Software. Users are encouraged to switch to one of
            the new APIs.
        </ul>
        <p>
            For more information about the APIs, please see the <a href="http://profiles.catalyst.harvard.edu/docs/ProfilesRNS_1.0.3_APIGuide.pdf">
                documentation</a> and <a href="http://profiles.catalyst.harvard.edu/docs/ProfilesRNS_1.0.3_API_Examples.zip">
                    example files</a>.
        </p>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlORCID" Visible="false">
        <h2>What is ORCID?</h2>
        <p>
            Researchers and scholars face the ongoing challenge of distinguishing scholarly
            activities from those of others with similar names. They need to be able to easily
            and uniquely attach their identity to scholarly work, such as articles, citations,
            grants, patents and datasets. As individuals collaborate across disciplines and
            institutions, they must interact with an increasing number and diversity of information
            systems. Entering data over and over again can be time-consuming, and often frustrating.
            ORCID is an open, non-profit, community-based effort to reduce that frustration.</p>
        <p>
            ORCID provides a registry of unique researcher identifiers and a transparent method
            of linking research activities and outputs to these identifiers. An ORCID iD is
            a persistent unique identifier that follows an individual throughout their career,
            and looks something like “0000-0003-0423-208X.”</p>
        <p>
            ORCID records hold non-sensitive information such as name, email, organization,
            and activities such as publication, grants, patents and other scholarly works. ORCID
            provides tools for individuals to manage data privacy.</p>
        <p>
            See <a target="_blank" href="http://orcid.org/about/what-is-orcid" title="About ORCID">
                What is ORCID</a> for additional information.</p>
        <h3>
            Why get an ORCID identifier?</h3>
        <p>
            Benefits of getting an ORCID iD include:</p>
        <ul>
            <li>Ensuring researchers get credit for their work</li>
            <li>Reducing time to identify scholarly output (see “Publisher integration,” below)</li>
            <li>Enabling scholars to keep track of and report on their work with funders, publishers
                and institutions</li>
            <li>Repurposing data for use in CV generation, citation repositories, Profiles, annual
                reports, faculty web-sites, and other systems (see “Grant submission integration,”
                below)</li>
            <li>Tying individuals to their scholarly work should make finding academic papers easier
                and more accurate</li>
        </ul>
        <p>
            Publisher integration: Elsevier, Thomson Reuters, Nature and other major publishers
            have begun integrating ORCID iDs into the manuscript submission process, and embedding
            ORCID identifiers across their scientific and scholarly research ecosystem. This
            will save authors time during submission, and enable automatic updating of author
            bibliographies when articles are published.</p>
        <p>
            Grant submission integration: NIH, NSF and other federal agencies are planning to
            integrate ORCID iDs into the ScienCV platform, for linking researchers, their grants,
            and their scientific output. The US federal government has been working to create
            a fed-wide profile system to streamline the grants and contract application process
            and reduce the data entry burden for investigators, and ORCID holds promise to be
            part of the solution.</p>
        <h3>
            What does the process entail?</h3>
        <p>
            There are benefits to having Profiles RNS initiate your ORCID record creation,
            or reporting an existing ORCID, as it paves the way for data exchange between Profiles RNS
            and ORCID on behalf of each scholar.</p>
        <p>
            When Profiles RNS initiates the process, we will upload your name and email address to
            create your ORCID record. Your email will be set as “public” within ORCID, unless
            you are hidden in your instituion directory, in which case you will be set to “limited” (for
            more on ORCID privacy settings, see the <a target="_blank" href="http://support.orcid.org/knowledgebase/articles/124518-orcid-privacy-settings"
                title="ORCID Privacy Settings">ORCID Privacy Settings</a>). You always have
            the ability to change these settings – the ORCID record is controlled by you, not
            your institution. Individuals who are in Profiles RNS will be asked if they want
            to push select information from Profiles RNS to ORCID at the time the ORCID record
            is created.</p>
        <p>
            ORCID will send you an email to claim your record. During the claim process they
            will suggest you set the default privacy setting for new works to be “public.” Data
            labeled as either “public” or “limited” will be accessible by your institution. You may be asked
            to allow your institution to be a Trusted Party (gain access to “limited” data), as a final step
            after claiming your ORCID record. If you do not claim your record within 10 days,
            it will become publicly visible. You can still claim the record at any time, and
            we encourage you to do so.</p>
        <p>
            For those not in Profiles RNS, or with no publication records showing in Profiles RNS,
            you can opt to add works using alternative means, such as the ORCID to Scopus integration.
            This option is available even if you have never used Scopus before. To use this,
            once logged into ORCID, choose &#8220;Update works,&#8221; then &#8220;Also see
            Import Research Activities,&#8221; to get the &#8220;Scopus to ORCID&#8221; option.
            Additional information on the Scopus integration with ORCID can be found <a target="_blank"
                href="http://www.info.sciverse.com/scopus/scopus-in-detail/orcid" title="Scopus ORCID integration">
                here</a>. You can use the same process but choose &#8220;CrossRef Metadata Search&#8221;
            to search CrossRef&#8217;s metadata on journal articles, conference proceedings
            and monographs, and add results to your ORCID profile. Additional information on
            the CrossRef Metadata search can be found <a href="http://labs.crossref.org/crossref-metadata-search/"
                title="CrossRef Metadata search" target="_blank">here</a>. Thomson Reuters will
            be adding integration to Web of Knowledge from within ORCID soon.</p>      
      
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlImprint" Visible="false">
        <h2>Impressum</h2>

        <div class="row">
            <div class="col-lg-6">
                <h4 class="mt-4">Published by:</h4>
                Central Coordination Unit (CCU)<br>
                Life and Medical Sciences (LIMES) Institute<br>
                Carl-Troll-Strasse 31<br>
                53115 Bonn, Germany<br>
                Tel.: +49/ (0)2 28 / 73 – 6 27 22<br>
                contact.ccu@uni-bonn.de
            </div>
            <div class="col-lg-6">
                <h4 class="mt-4">Administrator</h4>
                WGGC Bonn Office<br>
                Dr. Antonella Succurro<br>
                Life and Medical Sciences (LIMES) Institute<br>
                Carl-Troll-Strasse 31<br>
                53115 Bonn, Germany
            </div>
            <div class="col-12">
                <h4 class="mt-4">Webserver provider:</h4>
                Rheinische Friedrich-Wilhelms-Universität Bonn<br>
                Hochschulrechenzentrum HRZ
            </div>
        </div>

        <h4 class="mt-4">Disclaimer:</h4>
        <p>The NGS-CN Profiles webportal is powered by the free, open source Profiles Research Networking Software (RNS). It is used in numerous institutions around the world, including academic research centers, pharmaceutical companies, physician networks, and Federal agencies. If you would like to learn more about Profiles RNS or download the free source code, please visit the Profiles RNS website.</p>
        <p>The NGS-CN Profiles webportal was customized with the support of <a href="https://www.semiodesk.com" target="_blank">Semiodesk GmbH</a>.</p>
        <p>The development of the NGS-CN Profiles webportal is funded by the Deutsche Forschungsgemeinschaft (DFG) in the context of the Next Generation Sequencing initiative and led by the West German Genome Center (WGGC) NGS Competence Center.</p>
        © CCU 2021
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlPrivacy" Visible="false">
        <h2>Privacy Policy</h2>

    <p>This is the Website for the Next Generation Sequencing Network (NGS-CN). The Next Generation Sequencing Competence Network (NGS-CN) is a Deutsche Forschungsge-meinschaft (DFG) funded initiative that is comprised of four NGS Competence Centers (NGS-CCs), including the West German Genome Center(WGGC), a collaboration led by the University of Cologne that also involves the Universities of Bonn and Düsseldorf; the NGS Competence Center Tübingen (NGS-CCT) at the University of Tübingen; the Dresden-Concept Genome Center (DcGC) at the Technical University Dresden; and the Competence Center for Genomic Analysis Kiel (CCGA) at the University of Kiel.<br><br>The Office responsible for this website is the Central Coordination Unit (CCU) based at the University of Bonn.</p>

    <h4 class="mt-4">1. Name and address of the controller</h4>
    The controller within the meaning of the General Data Protection Regulation and other national data protection laws of the Member States as well as other provisions of data protection law is:</p>

    Central Coordinating Unit (CCU) office<br>
    LIMES-Institute<br>
    Carl-Troll-Str. 31<br>
    53115 Bonn<br>

    <p>Tel.: +49/ (0)2 28 / 73 &#8211; 6 27 36</p>

    <p><a href="mailto:contact.ccu@uni-bonn.de">contact.ccu@uni-bonn.de</a></p>

    <h4 class="mt-4">2. Name and address of the data protection officer</h4>
    <br>Data Protection Officer at the University of Bonn:</p>
    Dr. Jörg Hartmann<br>
    E-mail: joerg.hartmann AT uni-bonn.de<br>
    Genscherallee 3, D-53113 Bonn<br>
    Tel: + 49 (0)228 -73 – 6758

    <figure class="wp-block-embed"><div class="wp-block-embed__wrapper">
    https://www.datenschutz.uni-bonn.de
    </div></figure>

    <h4 class="mt-4">3. General information on data processing</h4>
    <h4 class="mt-4">3.1 Scope of processing of personal data</h4>
    We categorically only process personal data of our users if this is necessary to provide a functional website as well as our contents and services. The processing of personal data of our users generally takes place only after consent of the user. An exception applies in those cases where prior consent cannot be obtained for justified reasons and the processing of the data is permitted by law.
    
    <h4 class="mt-4">3.2 Legal basis for the processing of personal data</h4>
    Insofar as we obtain the consent of the data subject for the processing of personal data, Art. 6 (1) lit. a EU General Data Protection Regulation (GDPR) serves as the legal basis.<br>In the processing of personal data required for the performance of a contract to which the data subject is party, Art. 6 (1) lit. b GDPR serves as the legal basis. This also applies to processing operations that are necessary to carry out pre-contractual measures.<br>If processing is necessary for compliance with a legal obligation to which the University of Cologne is subject, Art. 6 (1) lit. c GDPR serves as the legal basis.<br>In the event that processing is necessary in order to protect the vital interests of the data subject or of another natural person, Article 6 (1) lit. d GDPR serves as the legal basis.<br>If processing is necessary for the performance of a task which is in the public interest or to exercise official authority which has been transferred to the university, Art. 6 (1) lit. e GDPR serves as the legal basis for processing.
    
    <h4 class="mt-4">3.3 Data erasure and storage time</h4>
    The personal data of the data subject will be erased or blocked as soon as the purpose of storage ceases to apply. Data may be stored beyond this point in time if this has been provided for by the European or national legislator in EU regulations, laws or other provisions to which the controller is subject. The data will also be blocked or erased if a storage period prescribed by the aforementioned standards expires, unless there is a need for further storage of the data for the conclusion or fulfilment of a contract.</p>

    <h4 class="mt-4">4. Provision of the website and creation of log files</h4>
    <h4 class="mt-4">4.1 Description and scope of data processing</h4>
    Every time you visit our website, our system automatically collects data and information from the computer system of the accessing computer.<br>The following data are collected:</p>

    <ol type="1"><li><em>Information about the browser type and version used</em></li><li><em>The user’s operating system</em></li><li><em>The user’s internet service provider</em></li><li><em>The user’s IP address (pseudonymized, truncated IP address)</em></li><li><em>Date and time of access</em></li><li><em>&nbsp;Websites from which the user’s system gets to our website</em></li><li><em>&nbsp;Websites accessed by the user’s system via our website</em></li></ol>

    <p>The log files contain IP addresses or other data that enable an assignment to a user. This may for instance be the case if the link to the website from which the user accesses the website or the link to the website to which the user switches contains personal data.The data are also stored in the log files of our system. These data are not stored together with other personal data of the user.</p>

    <h4 class="mt-4">4.2 Purpose of data processing</h4>
    The temporary storage of the IP address by the system is necessary to enable the website to be delivered to the user’s computer. For this purpose, the IP address of the user must remain stored for the duration of the session.<br>The data are stored in log files to ensure the functionality of the website. In addition, the data help to optimize the website and to ensure the security of our information technology systems. An evaluation of the data for marketing purposes does not take place in this context.</p>

    <h4 class="mt-4">4.3 Duration of storage</h4>
    The data will be erased as soon as they are no longer necessary to achieve the purpose for which they were collected. In the case of the collection of data for the provision of the website, this is the case when the respective session has ended.<br>If the data are stored in log files, this is the case after seven days at the latest. Storage beyond this point in time is possible. In this case, the IP addresses of the users are deleted or anonymized, so that an assignment of the accessing client is no longer possible.</p>

    <h4 class="mt-4">4.4 Possibility of objection and erasure</h4>
    The collection of the data for the provision of the website and the storage of the data in log files are absolutely necessary for the operation of the website. There is thus no possibility of objection on the part of the user.</p>

    <h4 class="mt-4">5. Use of cookies</h4>
    <h4 class="mt-4">5.1 Description and scope of data processing</h4>
    Our website uses cookies. Cookies are text files that are stored in the internet browser or by the internet browser in the user’s computer system. If a user visits a website, a cookie may be stored in the user’s operating system. This cookie contains a characteristic character string that enables a unique identification of the browser when the website is accessed again.<br>We use cookies to make our website more user-friendly. Some elements of our website require that the accessing browser can be identified even after a page change.<br><br>The website is powered by WordPress, which stores and transmits these cookies:</p>

    <ul><li>If you leave a comment on our site you may opt-in to saving your name, email address and website in cookies. These are for your convenience so that you do not have to fill in your details again when you leave another comment. These cookies will last for one year.</li><li>If you visit our login page, we will set a temporary cookie to determine if your browser accepts cookies. This cookie contains no personal data and is discarded when you close your browser.</li><li>When you log in, we will also set up several cookies to save your login information and your screen display choices. Login cookies last for two days, and screen options cookies last for a year. If you select “Remember Me”, your login will persist for two weeks. If you log out of your account, the login cookies will be removed.</li><li>If you edit or publish an article, an additional cookie will be saved in your browser. This cookie includes no personal data and simply indicates the post ID of the article you just edited. It expires after 1 day.</li></ul>

    <p>These additional cookies can be stored:</p>

    <ul><li>Language settings</li><li>Log-in information</li><li>Entered search terms</li><li>Frequency of page views</li><li>Use of website functions</li></ul>

    <h4 class="mt-4">5.2 Purpose of data processing</h4>
    The purpose of using technically necessary cookies is to simplify the use of websites for users. Some functions of our website cannot be provided without the use of cookies. For these, it is necessary that the browser is recognized even after a page change.<br>We require cookies for the following applications:</p>

    <ul><li>Applying language settings</li></ul>

    <p>The user data collected by technically necessary cookies are not used to create user profiles.<br>The analysis cookies are used to improve the quality of our website and its content. Analysis cookies tell us how the website is used, which means we are able to constantly optimize our offer.</p>

    <h4 class="mt-4">5.3 Duration of storage, possibility of objection and erasure</h4>

    <p>Cookies are stored on the user’s computer and transmitted to our site. Therefore, you as a user also have full control over the use of cookies. You can deactivate or restrict the transmission of cookies by changing the settings in your internet browser. Cookies that have already been saved can be deleted at any time. This can also be done automatically. If cookies are deactivated for our website, it may no longer be possible to use all functions of the website in full.<br><br>You can always select which cookies to allow by opening the Privacy &amp; Cookies Policy label at the bottom of the page.</p>

    <h4 class="mt-4">6. Newsletter</h4>

    <h4 class="mt-4">6.1 Description and scope of data processing</h4>
    Our website gives you the option to subscribe to a free newsletter. When registering for the newsletter, all data from the registration form is transmitted to us.<br>In addition, the following data are collected at registration:</p>

    <ul><li>IP address of the accessing computer</li><li>Date and time of access</li></ul>

    <p>During the registration process, your consent is obtained for the processing of the data and reference is made to the Privacy Policy.<br>No data are passed on to third parties in connection with data processing for the sending of newsletters. The data will be used exclusively for sending the newsletter.</p>

    <h4 class="mt-4">6.2 Legal basis for data processing</h4>
    The legal basis for the processing of the data after registration for the newsletter by the user is Art. 6 (1) lit. a GDPR.

    <h4 class="mt-4">6.3 Purpose of data processing</h4>
    The user’s email address is collected to send the newsletter.<br>The collection of other personal data as part of the registration process serves to prevent misuse of the services or the email address used.</p>

    <h4 class="mt-4">6.4 Duration of storage</h4>
    The data will be erased as soon as they are no longer necessary to achieve the purpose for which they were collected. The user’s email address is stored for as long as the subscription to the newsletter is active.</p>

    <h4 class="mt-4">6.5 Possibility of objection and erasure</h4>
    The subscription to the newsletter can be cancelled by the affected user at any time. There is a corresponding link in every newsletter for this purpose.<br>This also makes it possible to revoke your consent to the storage of personal data collected during the registration process.</p>

    <h4 class="mt-4">7. Special registration forms</h4>
    On our website we use case-specific registration forms to subscribe to specific events (e.g. conferences) or services (e.g. Profiles Web Portal). General rules as listed in the following point (“<strong>8. Contact form and email contact</strong>“) apply, but each form will link to specifications on how the data entered there will be used.</p>

    <h4 class="mt-4">8. Contact form and email contact</h4>
    <h4 class="mt-4">8.1 Description and scope of data processing</h4>
    Our website has a contact form which can be used for electronic contact. If a user takes advantage of this option, the data entered in the form will be transmitted to us and stored. These data are:</p>

    <ul><li>Name, surname and title</li><li>Institution or Company</li><li>Professional role</li></ul>

    <p>At the time of sending the message, the following data are also stored:</p>

    <ul><li>The user’s IP address</li><li>Date and time of access</li></ul>

    <p>During the collection process, your consent is obtained for the processing of the data and reference is made to the Privacy Policy.<br>Alternatively, you can contact us via the email address provided. In this case, the user’s personal data transmitted by email will be stored.<br>The data will not be passed on to third parties in this context. The data will be used exclusively for processing the conversation.</p>

    <h4 class="mt-4">8.2 Legal basis for data processing</h4>
    The legal basis for the processing of the data, if consent by the user has been provided, is Art. 6 (1) lit. a GDPR.<br>If the aim of the email contact is the conclusion of a contract, then additional legal basis for the processing is Art. 6 (1) lit. b GDPR.</p>

    <h4 class="mt-4">8.3 Purpose of data processing</h4>
    We only process the personal data from the form to handle contact being made.<br>The other personal data collected serves to prevent misuse of the contact form and to ensure the security of our information technology systems.</p>

    <h4 class="mt-4">8.4 Duration of storage</h4>
    The data will be erased as soon as they are no longer necessary to achieve the purpose for which they were collected. For the personal data from the input mask of the contact form and those data that were sent by email, this is the case when the respective conversation with the user is finished. The conversation is finished when it can be inferred from the circumstances that the facts in question have been conclusively clarified.</p>

    <h4 class="mt-4">8.5 Possibility of objection and erasure</h4>
    The user has the possibility to revoke his consent to the processing of personal data at any time. If the user only contacts us by email, he can also object to the storage of his personal data here at any time; in such a case, however, the conversation cannot be continued.<br>In order to revoke consent to data processing, &lt;a href=”http://wggc.de/?page_id=53″&gt;this form has to be used&lt;/a&gt;.<br>All personal data stored in the course of contacting us will be erased in the event of revocation.</p>

    <h4 class="mt-4">9. Web analysis with Matomo (formerly PIWIK)</h4>
    <h4 class="mt-4">9.1 Scope of processing of personal data</h4>
    On our website we use the open source software tool Matomo (formerly PIWIK) to analyze the surfing behavior of our users. The software places a cookie on the user’s computer (see above on cookies). If individual pages of our website are accessed, the following data are stored:</p>

    <ul><li>Two bytes of the IP address of the user’s accessing system</li><li>The accessed web page</li><li>The website from which the user has accessed the accessed website (referrer)</li><li>The subpages that are accessed from the accessed website</li><li>Duration of stay on the website</li><li>The frequency of accessing the website</li></ul>

    <p>The software runs exclusively on the servers of our website. The personal data of users are only stored there. The data are not passed on to third parties.<br>The software is set so that the IP addresses are not stored completely and that two bytes of the IP address are masked (e.g.: 192.168.xxx.xxx). In this way it is no longer possible to assign the shortened IP address to the accessing computer.<br><br>You can opt out from automated data collection by unchecking the box here or in the “Privacy &amp; Cookies Policy” tab.<br><br><br></p>

    <h4 class="mt-4">9.2 Purpose of data processing</h4>
    The processing of users’ personal data enables us to analyze the surfing behavior of our users. We are able to compile information about the use of the individual components of our website by evaluating the data obtained. This helps us to continuously improve our website and its user-friendliness. By anonymizing the IP address, users’ interest in protecting their personal data is taken into account.</p>

    <h4 class="mt-4">9.3 Duration of storage</h4>
    The data will be erased as soon as they are no longer needed for our recording purposes.<br>In our case, this is the case after 360 days.</p>

    <h4 class="mt-4">9.4 Possibility of objection and erasure</h4>
    Cookies are stored on the user’s computer and transmitted to our site. Therefore, you as a user also have full control over the use of cookies. You can deactivate or restrict the transmission of cookies by changing the settings in your internet browser. Cookies that have already been saved can be deleted at any time. This can also be done automatically. If cookies are deactivated for our website, it may no longer be possible to use all functions of the website in full.<br>On our website we offer our users the option to opt out from the analysis procedure. To do this, you must follow the corresponding link. Another cookie is then placed on your system, which signals to our system not to store the user’s data. If the user deletes the corresponding cookie from his own system in the meantime, he must set the opt-out cookie again.<br>More information about the privacy settings of the Matomo software can be found at the following link:&nbsp;<a href="https://matomo.org/docs/privacy/">https://matomo.org/docs/privacy/</a>.</p>

    <h4 class="mt-4">10. Use of Google Maps</h4>
    The website of the WGGC of the contains geographical information for contact and direction purposes. The maps are based on the Google Maps API provided by Google Inc, 1700 Amphitheatre Parkway, Mountain View, CA 94043, USA. By accessing the map service, Google can determine your IP address and the language of the system, as well as various browser-specific information.<br>The requested geographical positions are transferred directly to the service. When accessing the page with a GPS-enabled device, the location position can also be transmitted. Other personal data will not be passed on to Google.<br>Google uses cookies. The data processing procedures as well as the purposes of the processing can be requested and viewed directly at Google.<br>If you access the integrated maps, you will be asked for your consent to use the map service under these conditions. You can revoke this consent at any time.<br>The use of Google Maps is in the interest of a fast findability of the institutions represented on the web pages of the WGGC.</p>

    <h4 class="mt-4">11. Integration of YouTube</h4>
    The website of the WGGC uses plugins from the YouTube site, which is operated by Google. The operator of the pages is YouTube, LLC, 901 Cherry Ave., San Bruno, CA 94066, USA.<br>When you visit one of our YouTube plugin-enabled pages, you will be connected to the servers of YouTube. This tells the YouTube server which of our web pages you have visited.<br>If you are logged into your YouTube account, you enable YouTube to associate your surfing behavior directly with your personal profile. You can prevent this by logging out of your YouTube account.<br>The use of YouTube is in the interest of ensuring an attractive presentation of our online offers.<br>For more information on how user data are handled, please see YouTube’s privacy policy:&nbsp;<a href="https://www.google.de/intl/de/policies/privacy">https://www.google.de/intl/de/policies/privacy</a></p>

    <h4 class="mt-4">12. Rights of the data subject</h4>

    <p>If your personal data are processed, you as the data subject have the following rights pursuant to the GDPR towards the controller:
    
    <h4 class="mt-4">12.1 Right to information</h4>
    You can ask the controller to confirm whether personal data concerning you are being processed.<br>If such processing takes place, you can request the following information from the controller:the purposes for which the personal data are processed;</p>

    <ul><li>the categories of personal data being processed;</li><li>the recipients or categories of recipients to whom the personal data concerning you have been or are still being disclosed;</li><li>the planned duration of the storage of the personal data concerning you or, if specific information on this is not possible, criteria for determining the storage period;</li><li>the existence of a right to correction or erasure of the personal data concerning you, a right to restriction of processing by the controller or a right to object to such processing;</li><li>the existence of a right to lodge a complaint with a supervisory authority;</li><li>all available information on the origin of the data, if the personal data are not collected from the data subject;</li><li>the existence of automated decision-making including profiling in accordance with Art. 22 (1) and (4) GDPR and, at least in these cases, meaningful information on the logic involved and the scope and intended effects of such processing on the data subject.</li></ul>

    <p>You have the right to request information as to whether the personal data concerning you are transmitted to a third country or to an international organization. In this context, you may request to be informed of the appropriate guarantees pursuant to Art. 46 GDPR in connection with the transmission.<br>Where data are processed for scientific, historical or statistical research purposes, the right of access may be limited to the extent that it is likely to render impossible or be seriously prejudicial to the realization of the research or statistical purposes and the limitation is necessary for the fulfilment of the research or statistical purposes.</p>

    <h4 class="mt-4">12.2 Right to correction</h4>
    You have the right to correct and/or complete your data towards the controller if the personal data processed concerning you are incorrect or incomplete. The controller must make the correction without delay.<br>Where data are processed for scientific, historical or statistical research purposes, the right of correction may be limited to the extent that it is likely to render impossible or be seriously prejudicial to the realization of the research or statistical purposes and the limitation is necessary for the fulfilment of the research or statistical purposes.</p>

    <h4 class="mt-4">12.3 Right to restriction of processing</h4>
    You may request that the processing of personal data concerning you be restricted under the following conditions:</p>

    <ul><li>if you dispute the accuracy of the personal data concerning you for a period of time that enables the controller to verify the accuracy of the personal data;</li><li>the processing is unlawful, you reject the erasure the personal data and instead demand the restriction of the use of personal data;</li><li>the controller no longer needs the personal data for the purposes of processing, but you require them for the assertion of or to exercise or defend legal claims, or if you have filed an objection to the processing pursuant to Art. 21 (1) GDPR and it has not yet been determined whether the legitimate reasons of the controller outweigh your reasons.</li></ul>

    <p>If the processing of personal data concerning you has been restricted, such data may only be processed – except for being stored – with your consent or for the purpose of asserting, exercising or defending rights or protecting the rights of another natural or legal person or on grounds of an important public interest of the Union or a Member State.<br>If processing has been restricted according to the above conditions, you will be informed by the controller before the restriction is lifted.<br>Where data are processed for scientific, historical or statistical research purposes, the right of limitation of processing may be limited to the extent that it is likely to render impossible or be seriously prejudicial to the realization of the research or statistical purposes and the limitation is necessary for the fulfilment of the research or statistical purposes.</p>

    <h4 class="mt-4">12.4 Right of erasure</h4>

    <h4 class="mt-4"><em>Erasure obligation</em></h4>
    You can demand that the controller erases the personal data concerning you immediately. The controller is obliged to erase these data immediately if one of the following reasons applies:</p>

    <ul><li>The personal data concerning you are no longer necessary for the purposes for which they were collected or otherwise processed.</li><li>You withdraw your consent on which the processing was based in accordance with Art. 6 (1) lit. a GDPR or Art. 9 (2) lit. a GDPR and there is no other legal basis for processing.</li><li>You object to processing in accordance with Art. 21 (1) GDPR and there are no overriding legitimate reasons for processing, or you object to processing in accordance with Art. 21 (2) GDPR.</li><li>Your personal data were processed unlawfully.</li><li>The erasure of your personal data is necessary to fulfil a legal obligation under Union or Member State law which the controller is subject to.</li><li>The personal data concerning you were collected in relation to information society services offered pursuant to Art. 8 (1) GDPR.</li><li>Information to third parties</li></ul>

    <p>If the controller has made the personal data concerning you public and is obliged to erase it pursuant to Art. 17 (1) GDPR, he shall take appropriate measures, including technical measures, taking into account the available technology and the implementation costs, to inform data processors who process the personal data that you as the data subject have requested the erasure of all links to these personal data or of copies or replications of these personal data.</p>

    <h4 class="mt-4"><em>Exceptions</em></h4>
    The right to erasure does not exist insofar as the processing is necessary</p>

    <ul><li>to exercise the right to freedom of expression and information;</li><li>for the performance of a legal obligation required for processing under the law of the Union or of the Member States to which the controller is subject or for the performance of a task in the public interest or in the exercise of official authority conferred on the controller;</li><li>for reasons of public interest in the field of public health pursuant to Art. 9 (2) lit. h and i and Art. 9 (3) GDPR;</li><li>for archiving purposes in the public interest, scientific or historical research purposes or for statistical purposes pursuant to Art. 89 (1) GDPR, insofar as the law referred to under section a) is likely to render impossible or seriously impair the attainment of the objectives of such processing, or</li><li>to assert, exercise or defend legal claims.</li></ul>

    <h4 class="mt-4">12.5 Right to information</h4>
    If you have exercised your right to have the controller correct, erase or limit the processing, the controller is obliged to inform all recipients to whom the personal data concerning you have been disclosed of this correction or erasure of the data or the restriction on processing, unless this proves impossible or involves a disproportionate effort.<br>You have the right to be informed of such recipients by the controller.</p>

    <h4 class="mt-4">12.6 Right to data portability</h4>
    You have the right to receive the personal data concerning you that you have provided to the controller in a structured, common and machine-readable format. In addition, you have the right to pass these data on to another controller without obstruction by the controller to whom the personal data was provided, provided that</p>

    <ul><li>processing is based on consent pursuant to Art. 6 (1) lit. a GDPR or Art. 9 (2) lit. a GDPR or on a contract pursuant to Art. 6 (1) lit. b GDPR and</li><li>processing is carried out using automated methods.</li></ul>

    <p>In exercising this right, you further also have the right to request that the personal data concerning you be transferred directly from one controller to another controller, insofar as this is technically feasible. The freedoms and rights of other persons must not be affected by this.<br>The right to data portability shall not apply to the processing of personal data necessary for the performance of a task in the public interest or in the exercise of official authority conferred on the controller.</p>

    <h4 class="mt-4">12.7 Right of objection</h4>
    You have the right to object at any time, for reasons arising from your particular situation, to the processing of personal data concerning you under Art. 6 (1) lit. e GDPR; this also applies to any profiling based on these provisions.<br>In the event of an objection, the controller will no longer process the personal data concerning you unless the controller can demonstrate compelling legitimate grounds for the processing which override your interests, rights and freedoms or if the processing is required for the establishment, exercise or defense of legal claims.<br>In the case of data processing for scientific, historical or statistical research purposes pursuant to Art. 89 (1) GDPR, you also have the right to object to the processing of personal data concerning you for reasons arising from your particular situation, unless the processing is necessary to fulfil a task in the public interest.</p>

    <h4 class="mt-4">12.8 Right to revoke the data protection consent</h4>
    You have the right to revoke your data protection consent at any time. The revocation of consent shall not affect the legality of the processing carried out on the basis of the consent until revocation.</p>

    <h4 class="mt-4">12.9 Automated decision in individual cases including profiling</h4>
    You have the right to not be subject to a decision based exclusively on automated processing – including profiling – that has legal effect against you or significantly impairs you in a similar manner.<br>This does not apply if the decision</p>

    <ul><li>is necessary for the conclusion or performance of a contract between you and the controller,</li><li>is admissible by law of the Union or of the Member States to which the controller is subject and that law contains appropriate measures to safeguard your rights, freedoms and legitimate interests, or</li><li>is made with your express consent.</li></ul>

    <p>However, these decisions may not be based on special categories of personal data pursuant to Art. 9 (1) GDPR, unless Art. 9 (2) lit. a or g GDPR applies and appropriate measures have been taken to protect your rights and freedoms and your legitimate interests.<br>In the cases referred to in (1) and (3), the controller shall take reasonable measures to safeguard your rights, freedoms and legitimate interests, including at least the right to obtain the intervention of a person by the controller, the right to state one’s own position and to challenge the decision.</p>

    <h4 class="mt-4">12.10 Right to lodge a complaint with a supervisory authority</h4>
    Without prejudice to any other administrative or judicial remedy, you have the right to lodge a complaint with a supervisory authority, in particular in the Member State where you are staying, working or suspected of infringing, if you believe that the processing of personal data concerning you is in breach of the EU General Data Protection Regulation.<br>The supervisory authority with which the complaint has been lodged shall inform the complainant of the status and results of the complaint, including the possibility of a judicial remedy under Art. 78 GDPR.<br>The competent supervisory authority is: Landesbeauftragte für Datenschutz und Informationsfreiheit Nordrhein-Westfalen, PO Box 20 04 44, D-40102 Düsseldorf.
    </asp:Panel>
</div>
