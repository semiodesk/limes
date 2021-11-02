﻿/*  
 
    Copyright (c) 2008-2011 by the President and Fellows of Harvard College. All rights reserved.  
    Profiles Research Networking Software was developed under the supervision of Griffin M Weber, MD, PhD.,
    and Harvard Catalyst: The Harvard Clinical and Translational Science Center, with support from the 
    National Center for Research Resources and Harvard University.


    Code licensed under a BSD License. 
    For details, see: LICENSE.txt 
  
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace Connects.Profiles.Service.ServiceImplementation
{
    public class DataIO
    {

        #region "DB SQL.NET Methods"

        /// <summary>
        /// returns sqlconnection object
        /// </summary>
        /// <param name="Connectionstring"></param>
        /// <returns></returns>
        public SqlConnection GetDBConnection(string Connectionstring)
        {
            if (Connectionstring.CompareTo("") == 0)
                Connectionstring = ConfigurationManager.ConnectionStrings["ProfilesDB"].ConnectionString;
            else
            {
                if (Connectionstring.Length < 25)
                    Connectionstring = ConfigurationManager.ConnectionStrings[Connectionstring].ConnectionString;
            }
            SqlConnection dbsqlconnection = new SqlConnection(Connectionstring);
            try
            {
                dbsqlconnection.Open();
            }
            catch (Exception ex)
            {
                DebugLogging.Log(ex.Message);
                DebugLogging.Log(ex.StackTrace);
            }
            return dbsqlconnection;
        }

        public SqlCommand GetDBCommand(SqlConnection sqlcn, String CmdText, CommandType CmdType, CommandBehavior CmdBehavior, SqlParameter[] sqlParam)
        {
            SqlCommand sqlcmd = null;

            try
            {
                sqlcmd = new SqlCommand(CmdText, sqlcn);
                sqlcmd.CommandType = CmdType;


                DebugLogging.Log("CONNECTION STRING " + sqlcn.ConnectionString);
                DebugLogging.Log("COMMAND TEXT " + CmdText);
                DebugLogging.Log("COMMAND TYPE " + CmdType.ToString());
                if (sqlParam != null)
                    DebugLogging.Log("NUMBER OF PARAMS " + sqlParam.Length);

                AddSQLParameters(sqlcmd, sqlParam);

            }
            catch (Exception ex)
            {
                DebugLogging.Log(ex.Message);
                DebugLogging.Log(ex.StackTrace);
            }
            return sqlcmd;
        }

        public SqlCommand GetDBCommand(string SqlConnectionString, String CmdText, CommandType CmdType, CommandBehavior CmdBehavior, SqlParameter[] sqlParam)
        {

            SqlCommand sqlcmd = null;

            try
            {
                sqlcmd = new SqlCommand(CmdText, GetDBConnection(SqlConnectionString));
                sqlcmd.CommandType = CmdType;

                DebugLogging.Log("CONNECTION STRING " + SqlConnectionString);
                DebugLogging.Log("COMMAND TEXT " + CmdText);
                DebugLogging.Log("COMMAND TYPE " + CmdType.ToString());
                if (sqlParam != null)
                    DebugLogging.Log("NUMBER OF PARAMS " + sqlParam.Length);


                if (sqlParam != null)
                    AddSQLParameters(sqlcmd, sqlParam);


            }
            catch (Exception ex)
            {
                DebugLogging.Log(ex.Message);
                DebugLogging.Log(ex.StackTrace);
            }
            return sqlcmd;
        }

        public SqlCommand GetDBCommand(ref SqlConnection cn, String CmdText, CommandType CmdType, CommandBehavior CmdBehavior, SqlParameter[] sqlParam)
        {

            cn = GetDBConnection("");
            SqlCommand sqlcmd = null;

            try
            {
                sqlcmd = new SqlCommand(CmdText, cn);
                sqlcmd.CommandType = CmdType;

                if (sqlParam != null)
                    AddSQLParameters(sqlcmd, sqlParam);

            }
            catch (Exception ex)
            {
                DebugLogging.Log(ex.Message);
                DebugLogging.Log(ex.StackTrace);
            }

            return sqlcmd;
        }

        public void AddSQLParameters(SqlCommand sqlcmd, SqlParameter[] sqlParam)
        {
            for (int i = 0; i < sqlParam.GetLength(0); i++)
            {
                sqlcmd.Parameters.Add(sqlParam[i]);
                sqlcmd.Parameters[i].Direction = sqlParam[i].Direction;


            }
        }

        public SqlDataReader GetSQLDataReader(SqlCommand sqlcmd)
        {
            SqlDataReader sqldr = null;
            try
            {

                sqldr = sqlcmd.ExecuteReader(CommandBehavior.CloseConnection);

            }
            catch (Exception ex)
            {
                DebugLogging.Log("ERROR" + ex.Message);
                DebugLogging.Log("ERROR" + ex.StackTrace);
            }
            return sqldr;

        }

        public SqlDataReader GetSQLDataReader(string ConnectionString, String CmdText, CommandType CmdType, CommandBehavior CmdBehavior, SqlParameter[] sqlParam)
        {

            SqlDataReader sqldr = null;
            try
            {

                sqldr = this.GetSQLDataReader(this.GetDBCommand(ConnectionString, CmdText, CmdType, CmdBehavior, sqlParam));


            }
            catch (Exception ex)
            {
                DebugLogging.Log("ERROR" + ex.Message);
                DebugLogging.Log("ERROR" + ex.StackTrace);
            }
            return sqldr;
        }

        public void ExecuteSQLDataCommand(SqlCommand sqlcmd)
        {
            try
            {
                sqlcmd.ExecuteNonQuery();
                sqlcmd.Dispose();
            }
            catch (Exception ex)
            {
                DebugLogging.Log("ERROR" + ex.Message);
                DebugLogging.Log("ERROR" + ex.StackTrace);

            }
        }

        public void ExecuteSQLDataCommand(SqlCommand sqlcmd, object o)
        {

            try
            {
                o = sqlcmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                DebugLogging.Log("ERROR" + ex.Message);
                DebugLogging.Log("ERROR" + ex.StackTrace);
            }

            sqlcmd.Dispose();


        }

        #endregion
        public XmlDocument GetProfileNetworkForBrowser(Int32 personid)
        {
            string xmlstr = string.Empty;
            XmlDocument xmlrtn = new XmlDocument();

            if (Cache.Fetch(personid.ToString() + "GetProfileNetworkForBrowser") == null)
            {
                try
                {
                    string connstr = ConfigurationManager.ConnectionStrings["ProfilesDB"].ConnectionString;
                    SqlConnection dbconnection = new SqlConnection(connstr);
                    SqlCommand dbcommand = new SqlCommand("[Profile.Data].[usp_GetFlashNetwork_XML]");

                    SqlDataReader dbreader;
                    dbconnection.Open();
                    dbcommand.CommandType = CommandType.StoredProcedure;

                    dbcommand.Parameters.Add(new SqlParameter("@nodeid", personid));
                    dbcommand.Parameters.Add(new SqlParameter("@sessionid", HttpContext.Current.Session.SessionID.ToString()));
                    dbcommand.Connection = dbconnection;
                    dbreader = dbcommand.ExecuteReader(CommandBehavior.CloseConnection);

                    while (dbreader.Read())
                        xmlstr += dbreader[0].ToString();

                    DebugLogging.Log(xmlstr);

                    if (!dbreader.IsClosed)
                        dbreader.Close();

                    xmlstr = xmlstr.Replace(" id=", " lid=");
                    xmlstr = xmlstr.Replace(" nodeid=", " id=");

                    xmlstr = xmlstr.Replace(" id1=", " lid1=");
                    xmlstr = xmlstr.Replace(" id2=", " lid2=");
                    xmlstr = xmlstr.Replace(" nodeid1=", " id1=");
                    xmlstr = xmlstr.Replace(" nodeid2=", " id2=");


                    xmlrtn.LoadXml(xmlstr);

                }
                catch (Exception ex)
                {

                    DebugLogging.Log(ex.Message + " ++ " + ex.StackTrace);
                }
            }
            else
            {
                xmlrtn = Cache.Fetch(personid.ToString() + "GetProfileNetworkForBrowser");
            }

            return xmlrtn;
        }


        public XmlDocument SearchRequest(string searchstring, string exactphrase, string fname, string lname,
            string institution, string institutionallexcept, string department, string departmentallexcept,
            string division, string divisionallexcept,
            string classuri, string limit, string offset,
            string sortby, string sortdirection,
            string otherfilters, string personid, string ecomid, string harvardid)
        {

            System.Text.StringBuilder search = new System.Text.StringBuilder();
            XmlDocument xmlrequest = new XmlDocument();
            XmlDocument searchxml = new XmlDocument();

            string isexclude = "0";

            if (searchstring == null)
                searchstring = string.Empty;

            if (fname == null)
                fname = string.Empty;

            if (lname == null)
                lname = string.Empty;

            if (classuri == null)
                classuri = string.Empty;

            if (limit == null)
                limit = string.Empty;

            if (offset == null)
                offset = string.Empty;


            search.Append("<SearchOptions>");
            search.Append("<MatchOptions>");

            if (searchstring != string.Empty)
            {
                search.Append("<SearchString ExactMatch=\"" + exactphrase.ToLower() + "\">");

                search.Append(searchstring);
                search.Append("</SearchString>");
            }
            else if (xmlrequest.SelectSingleNode("//SearchString") != null)
            {
                search.Append(xmlrequest.SelectSingleNode("//SearchString").OuterXml);
            }

            search.Append("<SearchFiltersList>");



            if (fname != string.Empty)
            {
                search.Append(" <SearchFilter Property=\"http://xmlns.com/foaf/0.1/firstName\" MatchType=\"Left\">" + fname + "</SearchFilter>");
            }

            if (lname != string.Empty)
            {
                search.Append("<SearchFilter Property=\"http://xmlns.com/foaf/0.1/lastName\" MatchType=\"Left\">" + lname + "</SearchFilter>");
            }


            if (institution != string.Empty)
            {

                if (institutionallexcept == "true")
                    isexclude = "1";

                search.Append("<SearchFilter IsExclude=\"" + isexclude + "\" Property=\"http://profiles.catalyst.harvard.edu/ontology/prns#personInPrimaryPosition\"  Property2=\"http://vivoweb.org/ontology/core#positionInOrganization\"  MatchType=\"Left\">" + this.ConvertInstitution(institution) + "</SearchFilter>");
                isexclude = "0";
            }

            if (department != string.Empty)
            {
                if (departmentallexcept == "true")
                    isexclude = "1";

                search.Append("<SearchFilter IsExclude=\"" + isexclude + "\"  Property=\"http://profiles.catalyst.harvard.edu/ontology/prns#personInPrimaryPosition\"  Property2=\"http://profiles.catalyst.harvard.edu/ontology/prns#positionInDepartment\"   MatchType=\"Left\">" + this.ConvertDepartment(department) + "</SearchFilter>");
                isexclude = "0";
            }

            if (division != string.Empty)
            {
                if (divisionallexcept == "true")
                    isexclude = "1";

                search.Append("<SearchFilter IsExclude=\"" + isexclude + "\"  Property=\"http://profiles.catalyst.harvard.edu/ontology/prns#personInPrimaryPosition\"  Property2=\"http://profiles.catalyst.harvard.edu/ontology/prns#positionInDivision\"   MatchType=\"Left\">" + this.ConvertDivision(division) + "</SearchFilter>");
                isexclude = "0";
            }

            if (personid != string.Empty)
            {
                search.Append("<SearchFilter IsExclude=\"0\"  Property=\"http://profiles.catalyst.harvard.edu/ontology/prns#personId\" MatchType=\"Exact\">" + personid + "</SearchFilter>");
            }



            if (ecomid != string.Empty)
            {
                search.Append("<SearchFilter IsExclude=\"0\"  Property=\"http://profiles.catalyst.harvard.edu/ontology/catalyst#eCommonsLogin\"  MatchType=\"Exact\">" + ecomid + "</SearchFilter>");
            }



            if (harvardid != string.Empty)
            {
                search.Append("<SearchFilter IsExclude=\"0\"  Property=\"http://profiles.catalyst.harvard.edu/ontology/catalyst#harvardId\"  MatchType=\"Exact\">" + harvardid + "</SearchFilter>");
            }




            List<Utility.GenericListItem> filters = new List<Utility.GenericListItem>();

            filters = GetOtherOptions(otherfilters);
            if (filters.Count > 0)
            {
                foreach (Utility.GenericListItem item in filters)
                {
                    search.Append("<SearchFilter Property=\"http://profiles.catalyst.harvard.edu/ontology/prns#hasPersonFilter\" MatchType=\"Left\">" + item.Value + "</SearchFilter>");
                }
            }
            else
            {
                foreach (XmlNode node in xmlrequest.SelectNodes("//SearchFiltersList/SearchFilter[@Property='http://profiles.catalyst.harvard.edu/ontology/prns#hasPersonFilter']"))
                {
                    search.Append(node.OuterXml);
                }
            }

            //List<GenericListItem> facranks = GetFacultyRanks(facrank);

            //if (facranks.Count == 1)
            //{
            //    foreach (GenericListItem item in facranks)
            //    {
            //        search.Append("<SearchFilter Property=\"http://profiles.catalyst.harvard.edu/ontology/prns#hasFacultyRank\" MatchType=\"Exact\">" + item.Value + "</SearchFilter>");
            //    }
            //}
            //else if (facranks.Count > 1)
            //{
            //    search.Append("<SearchFilter Property=\"http://profiles.catalyst.harvard.edu/ontology/prns#hasFacultyRank\" MatchType=\"In\">");

            //    foreach (GenericListItem item in facranks)
            //    {
            //        search.Append("<Item>" + item.Value + "</Item>");
            //    }
            //    search.Append("</SearchFilter>");
            //}

            search.Append("</SearchFiltersList>");


            if (classuri != string.Empty)
            {
                search.Append("<ClassURI>");
                search.Append(classuri);
                search.Append("</ClassURI>");
            }
            else
            {
                search.Append("<ClassURI>");
                search.Append(xmlrequest.SelectSingleNode("//SearchOptions/MatchOptions/ClassURI").InnerText);
                search.Append("</ClassURI>");
            }


            search.Append("</MatchOptions>");
            search.Append("<OutputOptions><Offset>");
            search.Append(offset.ToString());
            search.Append("</Offset><Limit>");
            search.Append(limit.ToString());
            search.Append("</Limit>");


            search.Append("<SortByList>");

            if (sortby == string.Empty)
            {
                search.Append(this.NameSort("desc"));
            }
            else
            {

                switch (sortby.ToLower())
                {
                    case "name":
                        sortby = this.NameSort(sortdirection);
                        break;
                    case "title":
                        sortby = this.TitleSort(sortdirection);
                        break;
                    case "institution":
                        sortby = this.InstitutionSort(sortdirection);
                        break;
                    case "department":
                        sortby = this.DepartmentSort(sortdirection);
                        break;

                }

                search.Append(sortby);
            }

            search.Append("</SortByList>");


            search.Append("</OutputOptions>");
            search.Append("</SearchOptions>");


            searchxml.LoadXml(search.ToString());

            return searchxml;

        }

        #region "SESSION"
        /// <summary>
        ///     Used to create a custom Profiles Session instance.  This instance is used to track and store user activity as a form of Profiles Network.
        /// </summary>
        /// <param name="session">ref of Framework.Session object that stores the state of a Profiles user session</param>
        public void SessionCreate(ref Session session)
        {
            SqlDataReader dbreader;
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@RequestIP", session.RequestIP);
            param[1] = new SqlParameter("@UserAgent", session.UserAgent);

            dbreader = GetSQLDataReader(GetDBCommand("", "[User.Session].[CreateSession]", CommandType.StoredProcedure, CommandBehavior.CloseConnection, param));
            if (dbreader != null)
            {
                if (dbreader.Read()) //Returns a data ready with one row of user Session Info.  {Profiles Session Info, not IIS}
                {
                    session.SessionID = dbreader["SessionID"].ToString();
                    session.CreateDate = dbreader["CreateDate"].ToString();
                    session.LastUsedDate = Convert.ToDateTime(dbreader["LastUsedDate"].ToString());

                    DebugLogging.Log("Session object created:" + session.SessionID + " On " + session.CreateDate);
                }
                //Always close your readers
                if (!dbreader.IsClosed)
                    dbreader.Close();

            }
            else
            {
                session = null;
            }

        }



        /// <summary>
        ///     Used to create a custom Profiles Session instance.  This instance is used to track and store user activity as a form of Profiles Network.
        /// </summary>
        /// <param name="session">ref of Framework.Session object that stores the state of a Profiles user session</param>
        public void SessionUpdate(ref Session session)
        {

            string connstr = ConfigurationManager.ConnectionStrings["ProfilesDB"].ConnectionString;
            SessionManagement sm = new SessionManagement();

            SqlConnection dbconnection = new SqlConnection(connstr);

            SqlParameter[] param;

            param = new SqlParameter[6];

            SqlCommand dbcommand = new SqlCommand();

            dbconnection.Open();

            dbcommand.CommandTimeout = this.GetCommandTimeout();

            param[0] = new SqlParameter("@SessionID", session.SessionID);
            param[1] = new SqlParameter("@UserID", session.UserID);

            param[2] = new SqlParameter("@LastUsedDate", DateTime.Now);

            param[3] = new SqlParameter("@SessionPersonNodeID", 0);
            param[3].Direction = ParameterDirection.Output;

            param[4] = new SqlParameter("@SessionPersonURI", SqlDbType.VarChar, 400);
            param[4].Direction = ParameterDirection.Output;

            if (session.LogoutDate > DateTime.Now.AddDays(-5))
            {
                param[5] = new SqlParameter("@LogoutDate", session.LogoutDate.ToString());
            }

            dbcommand.Connection = dbconnection;

            try
            {
                //For Output Parameters you need to pass a connection object to the framework so you can close it before reading the output params value.
                ExecuteSQLDataCommand(GetDBCommand(ref dbconnection, "[User.Session].[UpdateSession]", CommandType.StoredProcedure, CommandBehavior.CloseConnection, param));

            }
            catch (Exception ex) { }

            try
            {
                dbcommand.Connection.Close();
                session.NodeID = Convert.ToInt64(param[3].Value);
                session.PersonURI = param[4].Value.ToString();
            }
            catch (Exception ex)
            {


            }


        }

        public int GetCommandTimeout()
        {
            return Convert.ToInt32(ConfigurationSettings.AppSettings["COMMANDTIMEOUT"]);

        }

        #endregion


        public XmlDocument Search(XmlDocument searchoptions, bool secure)
        {
            string xmlstr = string.Empty;
            XmlDocument xmlrtn = new XmlDocument();

            string cachekey = searchoptions.OuterXml + secure.ToString();
            SessionManagement sm = new SessionManagement();
            string sessionid = string.Empty;


            if (Utility.CacheUtil.Fetch(cachekey) == null)
            {
                try
                {
                    string connstr = ConfigurationManager.ConnectionStrings["ProfilesDB"].ConnectionString;

                    sessionid = sm.SessionCreate();
                    SqlConnection dbconnection = new SqlConnection(connstr);
                    SqlCommand dbcommand = new SqlCommand();

                    SqlDataReader dbreader;
                    dbconnection.Open();
                    dbcommand.CommandType = CommandType.StoredProcedure;

                    dbcommand.CommandText = "[Search.].[GetNodes]";

                    dbcommand.CommandTimeout = this.GetCommandTimeout();

                    Session session = new Session();
                    session.SessionID = sessionid;

                    if (secure)
                    {
                        dbcommand.Parameters.Add(new SqlParameter("@UseCache", "Private"));
                        User user = new User();
                        user.UserName = ConfigurationSettings.AppSettings["SecureGenericUserName"];
                        user.UserID = Convert.ToInt32(ConfigurationSettings.AppSettings["SecureGenericUserID"]);

                    }

                    this.SessionUpdate(ref session);


                    dbcommand.Parameters.Add(new SqlParameter("@sessionid", sessionid));

                    dbcommand.Parameters.Add(new SqlParameter("@SearchOptions", searchoptions.OuterXml));


                    dbcommand.Connection = dbconnection;

                    dbreader = dbcommand.ExecuteReader(CommandBehavior.CloseConnection);

                    while (dbreader.Read())
                    {
                        xmlstr += dbreader[0].ToString();
                    }

                    if (!dbreader.IsClosed)
                        dbreader.Close();


                    xmlrtn.LoadXml(xmlstr);


                    Utility.CacheUtil.Set(cachekey, xmlrtn);
                    xmlstr = string.Empty;

                }
                catch (Exception e)
                {

                    throw new Exception(e.Message);
                }
            }
            else
            {
                xmlrtn = Utility.CacheUtil.Fetch(cachekey);
            }

            return xmlrtn;

        }

        public XmlDocument Search(string personid, bool secure)
        {
            System.Text.StringBuilder sql = new System.Text.StringBuilder();
            string xmlstr = string.Empty;
            XmlDocument xmlrtn = new XmlDocument();
            SessionManagement sm = new SessionManagement();
            Int64 nodeid = 0;

            string sessionid = string.Empty;

            try { Convert.ToInt16(personid); }
            catch (Exception ex)
            {
                personid = "'" + personid + "'";
            }

            string connstr = ConfigurationManager.ConnectionStrings["ProfilesDB"].ConnectionString;

            sql.AppendLine("select i.nodeid from  [RDF.Stage].internalnodemap i with(nolock) join [Profile.Data].Person p with(nolock) on i.InternalID = p.PersonID  where  i.class = 'http://xmlns.com/foaf/0.1/Person' and i.internalid = " + personid + " and p.IsActive = 1 ");

            SqlConnection dbconnection = new SqlConnection(connstr);
            SqlCommand dbcommand = new SqlCommand();

            SqlDataReader dbreader;
            dbconnection.Open();
            dbcommand.CommandType = CommandType.Text;

            dbcommand.CommandText = sql.ToString();
            dbcommand.CommandTimeout = 5000;

            dbcommand.Connection = dbconnection;

            dbreader = dbcommand.ExecuteReader(CommandBehavior.CloseConnection);

            while (dbreader.Read())
            {
                nodeid = Convert.ToInt32(dbreader[0].ToString());
            }

            if (!dbreader.IsClosed)
                dbreader.Close();


            dbcommand = new SqlCommand();
            dbcommand.CommandType = CommandType.StoredProcedure;
            dbcommand.CommandTimeout = 5000;
            dbconnection.Open();
            dbcommand.Connection = dbconnection;

            dbcommand.CommandText = "[RDF.].[GetDataRDF]";
            dbcommand.Parameters.Add(new SqlParameter("@subject", nodeid));
            dbcommand.Parameters.Add(new SqlParameter("@predicate", 0));
            dbcommand.Parameters.Add(new SqlParameter("@object", 0));

            dbcommand.Parameters.Add(new SqlParameter("@showDetails", "true"));
            dbcommand.Parameters.Add(new SqlParameter("@expand", "true"));

            sessionid = sm.SessionCreate();
            Session session = new Session();
            if (secure)
            {

                User user = new User();
                user.UserName = ConfigurationSettings.AppSettings["SecureGenericUserName"];
                user.UserID = Convert.ToInt32(ConfigurationSettings.AppSettings["SecureGenericUserID"]);


                session.UserID = user.UserID;
                session.SessionID = sessionid;


            }
            this.SessionUpdate(ref session);
            dbcommand.Parameters.Add(new SqlParameter("@sessionid", sessionid));

            dbcommand.Connection = dbconnection;

            dbreader = dbcommand.ExecuteReader(CommandBehavior.CloseConnection);

            while (dbreader.Read())
            {
                xmlstr += dbreader[0].ToString();
            }

            dbconnection.Dispose();


            if (!dbreader.IsClosed)
                dbreader.Close();

            dbreader.Dispose();
            try
            {
                xmlrtn.LoadXml(xmlstr);
            }
            catch (Exception ex) { }

            return xmlrtn;

        }




        public string ConvertV2ToBetaSearch(string RDFResults, string queryid, string version, bool individual)
        {
            System.Text.StringBuilder returnxml = new System.Text.StringBuilder();

            XmlDocument RDF = new XmlDocument();
            Utility.Namespace namespacemgr = new Connects.Profiles.Utility.Namespace();
            XmlNamespaceManager namespaces;
            string uri = string.Empty;

            string affiliationuri = string.Empty;
            string xpathbufferDepartment = string.Empty;

            string topnode = string.Empty;
            RDF.LoadXml(RDFResults);
            namespaces = namespacemgr.LoadNamespaces(RDF);
            int count = 0;
            XmlNode pub = null;
            List<CustomPub> custompubs;
            XmlNodeList persons;
            Int64 currentnode = 0;
            PersonData persondata;

            bool bynodeid = false;


            if (RDF.SelectNodes("//prns:hasConnection[@rdf:nodeID!='']", namespaces) != null && RDF.SelectSingleNode("rdf:RDF/rdf:Description[1]/@rdf:about", namespaces) == null)
                bynodeid = true;

            if (individual && !bynodeid)
            {
                persons = RDF.SelectNodes("//rdf:RDF", namespaces);
            }
            else
            {
                persons = RDF.SelectNodes("//prns:hasConnection[@rdf:nodeID!='']", namespaces);
            }



            foreach (XmlNode person in persons)
            {
                if (individual && !bynodeid)
                {
                    if (RDF.SelectSingleNode("rdf:RDF/rdf:Description[1]/@rdf:about", namespaces) != null)
                    {
                        uri = RDF.SelectSingleNode("rdf:RDF[1]/rdf:Description[1]/@rdf:about", namespaces).Value;
                        returnxml.Append("<Person QueryRelevance=\"1\" Visible=\"true\">");
                    }
                }
                else
                {
                    if (RDF.SelectSingleNode("rdf:RDF[1]/rdf:Description[@rdf:nodeID='" + person.SelectSingleNode("@rdf:nodeID", namespaces).Value + "']/rdf:object/@rdf:resource", namespaces) != null)
                    {
                        uri = RDF.SelectSingleNode("rdf:RDF[1]/rdf:Description[@rdf:nodeID='" + person.SelectSingleNode("@rdf:nodeID", namespaces).Value + "']/rdf:object/@rdf:resource", namespaces).Value;
                        returnxml.Append("<Person QueryRelevance=\"" + RDF.SelectSingleNode("rdf:RDF[1]/rdf:Description[@rdf:nodeID='" + person.SelectSingleNode("@rdf:nodeID", namespaces).Value + "']/prns:connectionWeight", namespaces).InnerText + "\" Visible=\"true\">");
                    }
                    else
                    {
                        returnxml.Append("<Person QueryRelevance=\"" + RDF.SelectSingleNode("rdf:RDF[1]/rdf:Description[@rdf:nodeID='" + person.SelectSingleNode("@rdf:nodeID", namespaces).Value + "']/prns:connectionWeight", namespaces).InnerText + "\" Visible=\"true\">");

                    }
                }

                if (uri != string.Empty)
                {

                    currentnode = Convert.ToInt64(uri.Split('/')[uri.Split('/').Length - 1]);


                    if (version == "2")
                    {
                        returnxml.Append("<PersonID>" + this.GetPersonID(currentnode).ToString() + "</PersonID>");
                    }
                    else
                    {
                        returnxml.Append("<PersonID>" + RDF.SelectSingleNode("rdf:RDF/rdf:Description[@rdf:about='" + uri + "']", namespaces).InnerText + "</PersonID>");
                    }


                    persondata = this.GetPersonData(this.GetPersonID(currentnode));

                    //NAME
                    returnxml.Append("<Name>");
                    returnxml.Append("<FullName>");
                    if (RDF.SelectSingleNode("rdf:RDF[1]/rdf:Description[@rdf:about='" + uri + "']/prns:fullName", namespaces) != null)
                        returnxml.Append(RDF.SelectSingleNode("rdf:RDF[1]/rdf:Description[@rdf:about='" + uri + "']/prns:fullName", namespaces).InnerText);
                    returnxml.Append("</FullName>");

                    returnxml.Append("<FirstName>");
                    if (RDF.SelectSingleNode("rdf:RDF[1]/rdf:Description[@rdf:about='" + uri + "']/foaf:firstName", namespaces) != null)
                        returnxml.Append(RDF.SelectSingleNode("rdf:RDF[1]/rdf:Description[@rdf:about='" + uri + "']/foaf:firstName", namespaces).InnerText);
                    returnxml.Append("</FirstName>");

                    returnxml.Append("<LastName>");
                    if (RDF.SelectSingleNode("rdf:RDF[1]/rdf:Description[@rdf:about='" + uri + "']/foaf:lastName", namespaces) != null)
                        returnxml.Append(RDF.SelectSingleNode("rdf:RDF[1]/rdf:Description[@rdf:about='" + uri + "']/foaf:lastName", namespaces).InnerText);
                    returnxml.Append("</LastName>");
                    returnxml.Append("</Name>");
                    //END NAME                


/*

                    returnxml.Append("<InternalIDList>");
                    if (RDF.SelectSingleNode("rdf:RDF[1]/rdf:Description[@rdf:about='" + uri + "']/catalyst:eCommonsLogin", namespaces) != null)
                    {
                        returnxml.Append("<InternalID Name=\"EcommonsUsername\">");
                        returnxml.Append(RDF.SelectSingleNode("rdf:RDF[1]/rdf:Description[@rdf:about='" + uri + "']/catalyst:eCommonsLogin", namespaces).InnerText);
                        returnxml.Append("</InternalID>");
                    }

                    if (RDF.SelectSingleNode("rdf:RDF[1]/rdf:Description[@rdf:about='" + uri + "']/catalyst:harvardId", namespaces) != null)
                    {
                        returnxml.Append("<InternalID Name=\"HarvardID\">");
                        returnxml.Append(RDF.SelectSingleNode("rdf:RDF[1]/rdf:Description[@rdf:about='" + uri + "']/catalyst:harvardId", namespaces).InnerText);
                        returnxml.Append("</InternalID>");
                    }

                    returnxml.Append("</InternalIDList>");
*/
                    if (version == "2")
                    {
                        ////ADDRESS
                        returnxml.Append("<Address>");
                        returnxml.Append("<Address1>");
                        returnxml.Append(persondata.AddressLine1);
                        returnxml.Append("</Address1>");

                        returnxml.Append("<Address2>");
                        returnxml.Append(persondata.AddressLine2);
                        returnxml.Append("</Address2>");

                        returnxml.Append("<Address3>");
                        returnxml.Append(persondata.AddressLine3);
                        returnxml.Append("</Address3>");

                        returnxml.Append("<Address4>");
                        returnxml.Append(persondata.AddressLine4);
                        returnxml.Append("</Address4>");

                        returnxml.Append("<Telephone>");
                        returnxml.Append(persondata.Phone);
                        returnxml.Append("</Telephone>");

                        returnxml.Append("<Fax>");
                        returnxml.Append(persondata.Fax);
                        returnxml.Append("</Fax>");

                        returnxml.Append("<Latitude>");
                        if (RDF.SelectSingleNode("rdf:RDF[1]/rdf:Description[@rdf:about='" + uri + "']/prns:latitude", namespaces) != null)
                            returnxml.Append(Convert.ToDecimal(RDF.SelectSingleNode("rdf:RDF[1]/rdf:Description[@rdf:about='" + uri + "']/prns:latitude", namespaces).InnerText));
                        returnxml.Append("</Latitude>");

                        returnxml.Append("<Longitude>");
                        if (RDF.SelectSingleNode("rdf:RDF[1]/rdf:Description[@rdf:about='" + uri + "']/prns:longitude", namespaces) != null)
                            returnxml.Append(Convert.ToDecimal(RDF.SelectSingleNode("rdf:RDF[1]/rdf:Description[@rdf:about='" + uri + "']/prns:longitude", namespaces).InnerText));
                        returnxml.Append("</Longitude>");

                        returnxml.Append("</Address>");
                        //END ADDRESS
                    }

                    returnxml.Append("<AffiliationList Visible=\"true\">");
                    string organization = string.Empty;
                    string department = string.Empty;
                    string primary = string.Empty;

                    if (RDF.SelectNodes("rdf:RDF[1]/rdf:Description[@rdf:about='" + uri + "']/vivo:personInPosition/@rdf:resource", namespaces) != null)
                    {
                        try
                        {
                            affiliationuri = RDF.SelectSingleNode("rdf:RDF[1]/rdf:Description[@rdf:about='" + uri + "']/prns:personInPrimaryPosition/@rdf:resource", namespaces).Value;
                        }
                        catch (Exception ex) { }

                        XmlNodeList afflist = null;

                        if (individual)
                        {
                            afflist = RDF.SelectNodes("rdf:RDF[1]/rdf:Description[@rdf:about='" + uri + "']/vivo:personInPosition/@rdf:resource", namespaces);
                        }
                        else
                        {
                            afflist = RDF.SelectNodes("rdf:RDF[1]/rdf:Description[@rdf:about='" + affiliationuri + "']", namespaces);
                        }

                        foreach (XmlNode affiliation in afflist)
                        {

                            if (affiliationuri != string.Empty)
                            {

                                if (RDF.SelectSingleNode("rdf:RDF[1]/rdf:Description[@rdf:about='" + affiliation.Value + "']/prns:isPrimaryPosition", namespaces) != null && individual)
                                {
                                    primary = "true";
                                }
                                else if (RDF.SelectSingleNode("rdf:RDF[1]/rdf:Description[@rdf:about='" + affiliationuri + "']/prns:isPrimaryPosition", namespaces) != null && !individual)
                                {
                                    primary = "true";
                                }
                                else
                                {

                                    primary = "false";
                                }


                                returnxml.Append("<Affiliation Primary=\"" + primary + "\">");

                                returnxml.Append("<JobTitle>");
                                if (individual)
                                    returnxml.Append(RDF.SelectSingleNode("rdf:RDF[1]/rdf:Description[@rdf:about='" + affiliation.Value + "']/vivo:hrJobTitle", namespaces).InnerText);
                                else
                                    returnxml.Append(RDF.SelectSingleNode("rdf:RDF[1]/rdf:Description[@rdf:about='" + affiliationuri + "']/vivo:hrJobTitle", namespaces).InnerText);
                                returnxml.Append("</JobTitle>");

                                returnxml.Append("<InstitutionAbbreviation>");
                                try
                                {
                                    if (individual)
                                        organization = RDF.SelectSingleNode("rdf:RDF[1]/rdf:Description[@rdf:about='" + affiliation.Value + "']/vivo:positionInOrganization/@rdf:resource", namespaces).InnerText;
                                    else
                                        organization = RDF.SelectSingleNode("rdf:RDF[1]/rdf:Description[@rdf:about='" + affiliationuri + "']/vivo:positionInOrganization/@rdf:resource", namespaces).InnerText;
                                    returnxml.Append(GetInstitutionAbbreviation(RDF.SelectSingleNode("rdf:RDF/rdf:Description[@rdf:about='" + organization + "']/rdfs:label", namespaces).InnerText));
                                }
                                catch (Exception e) { }
                                returnxml.Append("</InstitutionAbbreviation>");

                                returnxml.Append("<InstitutionName>");
                                try
                                {
                                    returnxml.Append(RDF.SelectSingleNode("rdf:RDF[1]/rdf:Description[@rdf:about='" + organization + "']/rdfs:label", namespaces).InnerText);
                                }
                                catch (Exception e) { }
                                returnxml.Append("</InstitutionName>");

                                returnxml.Append("<DepartmentName>");
                                try
                                {
                                    if (individual)
                                        department = RDF.SelectSingleNode("rdf:RDF[1]/rdf:Description[@rdf:about='" + affiliation.Value + "']/prns:positionInDepartment/@rdf:resource", namespaces).InnerText;
                                    else
                                        department = RDF.SelectSingleNode("rdf:RDF[1]/rdf:Description[@rdf:about='" + affiliationuri + "']/prns:positionInDepartment/@rdf:resource", namespaces).InnerText;

                                    returnxml.Append(RDF.SelectSingleNode("rdf:RDF[1]/rdf:Description[@rdf:about='" + department + "']/rdfs:label", namespaces).InnerText);
                                }
                                catch (Exception e) { }
                                returnxml.Append("</DepartmentName>");
                            }


                            if (RDF.SelectSingleNode("rdf:RDF[1]/rdf:Description[@rdf:about=/rdf:RDF/rdf:Description[@rdf:about='" + uri + "']/prns:hasFacultyRank/@rdf:resource]/rdfs:label", namespaces) != null)
                            {
                                returnxml.Append("<FacultyType>");
                                returnxml.Append(RDF.SelectSingleNode("rdf:RDF/rdf:Description[@rdf:about=/rdf:RDF/rdf:Description[@rdf:about='" + uri + "']/prns:hasFacultyRank/@rdf:resource]/rdfs:label", namespaces).InnerText);
                                returnxml.Append("</FacultyType>");
                            }

                            returnxml.Append("</Affiliation>");

                        }

                    }

                    returnxml.Append("</AffiliationList>");

                    returnxml.Append("<ProfileURL Visible=\"true\">" + uri + "</ProfileURL>");

                    if (RDF.SelectSingleNode("rdf:RDF[1]/rdf:Description[@rdf:about='" + uri + "']/vivo:email", namespaces) != null)
                    {

                        returnxml.Append("<EmailImageUrl Visible=\"true\">");                     
                        returnxml.Append(RDF.SelectSingleNode("rdf:RDF[1]/rdf:Description[@rdf:about='" + uri + "']/vivo:email", namespaces).InnerText);
                        returnxml.Append("</EmailImageUrl>");
                    }
                    else
                    {
                        returnxml.Append("<EmailImageUrl/>");
                    }

                    if (RDF.SelectSingleNode("rdf:RDF[1]/rdf:Description[@rdf:about='" + uri + "']/prns:mainImage/@rdf:resource", namespaces) != null)
                    {
                        returnxml.Append("<PhotoUrl Visible=\"true\">");
                        returnxml.Append(RDF.SelectSingleNode("rdf:RDF[1]/rdf:Description[@rdf:about='" + uri + "']/prns:mainImage/@rdf:resource", namespaces).InnerText);
                        returnxml.Append("</PhotoUrl>");
                    }
                    else
                    {
                        returnxml.Append("<PhotoUrl/>");
                    }


                    returnxml.Append("<BasicStatistics Visible=\"true\">");
                    returnxml.Append("<PublicationCount>");
                    returnxml.Append(this.GetPubCount(Convert.ToInt64(uri.Split('/')[uri.Split('/').Length - 1])));
                    returnxml.Append("</PublicationCount>");
                    returnxml.Append("<MatchingPublicationCount>0</MatchingPublicationCount>");  //No equlivant to new system
                    try
                    {
                        returnxml.Append("<MatchScore>" + RDF.SelectSingleNode("rdf:RDF[1]/rdf:Description[@rdf:nodeID='" + person.SelectSingleNode("@rdf:nodeID", namespaces).Value + "']/prns:connectionWeight", namespaces).InnerText + "</MatchScore>");
                    }
                    catch (Exception ex) { }
                    returnxml.Append("</BasicStatistics>");


                    if (individual)
                    {

                        if (version != "2")
                            returnxml.Append("<Publications>");

                        returnxml.Append("<PublicationList>");

                        custompubs = this.GetCustomPubs(currentnode);

                        foreach (XmlNode publication in RDF.SelectNodes("//vivo:authorInAuthorship", namespaces))
                        {
                            try
                            {

                                pub = RDF.SelectSingleNode("rdf:RDF[1]/rdf:Description[@rdf:about='" + RDF.SelectSingleNode("rdf:RDF[1]/rdf:Description[@rdf:about='" + publication.SelectSingleNode("@rdf:resource", namespaces).Value + "']/vivo:linkedInformationResource/@rdf:resource", namespaces).Value + "']", namespaces);
                            }
                            catch (Exception ex) { pub = null; }

                            if (pub != null)
                            {

                                if (pub.SelectSingleNode("bibo:pmid", namespaces) != null)
                                    returnxml.Append("<Publication CustomCategory='' Type='PubMed' Visible='true'>");
                                else
                                    returnxml.Append("<Publication CustomCategory='' Type='Custom' Visible='true'>");


                                returnxml.Append("<PublicationID>");
                                returnxml.Append(publication.SelectSingleNode("@rdf:resource", namespaces).Value);
                                returnxml.Append("</PublicationID>");

                                returnxml.Append("<PublicationReference>");
                                returnxml.Append(((pub.SelectSingleNode("prns:informationResourceReference", namespaces).InnerText).Replace("<", "&lt").Replace(">", "&gt")));
                                returnxml.Append("</PublicationReference>");

                                returnxml.Append("<PublicationMatchDetailList>");

                                returnxml.Append("</PublicationMatchDetailList>");

                                if (version == "2")
                                {
                                    returnxml.Append("<PublicationSourceList>");

                                    if (pub.SelectSingleNode("bibo:pmid", namespaces) != null)
                                    {

                                        returnxml.Append("<PublicationSource ID='" + pub.SelectSingleNode("bibo:pmid", namespaces).InnerText + "' URL='" + "http://www.ncbi.nlm.nih.gov/pubmed/" + pub.SelectSingleNode("bibo:pmid", namespaces).InnerText + "' Primary='true' Name='PubMed'/>");
                                    }
                                    returnxml.Append("</PublicationSourceList>");

                                }
                                else
                                {
                                    returnxml.Append("<PublicationURL>");
                                    try
                                    {
                                        returnxml.Append("http://www.ncbi.nlm.nih.gov/pubmed/" + pub.SelectSingleNode("bibo:pmid", namespaces).InnerText);
                                    }
                                    catch (Exception ex) { }
                                    returnxml.Append("</PublicationURL>");
                                    returnxml.Append("<PubMedID>");
                                    try
                                    {
                                        returnxml.Append(pub.SelectSingleNode("bibo:pmid", namespaces).InnerText);
                                    }
                                    catch (Exception ex) { }
                                    returnxml.Append("</PubMedID>");

                                    returnxml.Append("<PublicationSourceList/>");
                                }
                                returnxml.Append("</Publication>");

                                pub = null;
                            }
                        }

                        returnxml.Append("</PublicationList>");


                        if (version != "2")
                            returnxml.Append("</Publications>");


                    }



                    if (individual)
                    {
                        returnxml.Append("<PassiveNetworks>");

                        returnxml.Append("<SimilarPersonList TotalSimilarPeopleCount=\"" + RDF.SelectNodes("rdf:RDF[1]/rdf:Description/prns:similarTo", namespaces).Count + "\">");
                        foreach (XmlNode similarto in RDF.SelectNodes("rdf:RDF[1]/rdf:Description/prns:similarTo", namespaces))
                        {
                            if (similarto.SelectSingleNode("@rdf:resource", namespaces).Value != uri)
                            {
                                currentnode = Convert.ToInt64(similarto.SelectSingleNode("@rdf:resource", namespaces).Value.Split('/')[uri.Split('/').Length - 1]);

                                returnxml.Append("<SimilarPerson PersonID=\"" + this.GetPersonID(currentnode) + "\">");
                                returnxml.Append(RDF.SelectSingleNode("rdf:RDF[1]/rdf:Description[@rdf:about='" + similarto.SelectSingleNode("@rdf:resource", namespaces).Value + "']/prns:fullName", namespaces).InnerText);
                                returnxml.Append("</SimilarPerson>");
                            }
                        }
                        returnxml.Append("</SimilarPersonList>");

                        returnxml.Append("<CoAuthorList TotalCoAuthorCount=\"" + RDF.SelectNodes("rdf:RDF/rdf:Description[1]/vivo:authorInAuthorship", namespaces).Count + "\">");


                        foreach (XmlNode coauthorpub in RDF.SelectNodes("rdf:RDF/rdf:Description[@rdf:about= /rdf:RDF[1]/rdf:Description[1]/prns:coAuthorOf/@rdf:resource][position() < 5]", namespaces))
                        {
                            if (coauthorpub.SelectSingleNode("@rdf:about", namespaces).Value != uri)
                            {
                                currentnode = Convert.ToInt64(coauthorpub.SelectSingleNode("@rdf:about", namespaces).Value.Split('/')[uri.Split('/').Length - 1]);

                                returnxml.Append("<CoAuthor PersonID=\"" + this.GetPersonID(currentnode) + "\" Institution=\"" + (RDF.SelectSingleNode("rdf:RDF/rdf:Description[@rdf:about='" + RDF.SelectSingleNode("rdf:RDF/rdf:Description[@rdf:about='" + coauthorpub.SelectSingleNode("@rdf:about", namespaces).Value + "']/prns:personInPrimaryPosition/@rdf:resource", namespaces).Value + "']/rdfs:label", namespaces)).InnerText + "\">");
                                returnxml.Append(coauthorpub.SelectSingleNode("rdfs:label", namespaces).InnerText);
                                returnxml.Append("</CoAuthor>");
                            }
                        }
                        returnxml.Append("</CoAuthorList>");

                        returnxml.Append("<NeighborList>");
                        foreach (XmlNode neighbor in RDF.SelectNodes("rdf:RDF/rdf:Description[1]/prns:physicalNeighborOf", namespaces))
                        {

                            if (neighbor.SelectSingleNode("@rdf:resource", namespaces).Value != uri)
                            {
                                currentnode = Convert.ToInt64(neighbor.SelectSingleNode("@rdf:resource", namespaces).Value.Split('/')[uri.Split('/').Length - 1]);
                                returnxml.Append("<Neighbor PersonID=\"" + this.GetPersonID(currentnode) + "\">");
                                returnxml.Append(RDF.SelectSingleNode("rdf:RDF[1]/rdf:Description[@rdf:about='" + neighbor.SelectSingleNode("@rdf:resource", namespaces).Value + "']/prns:fullName", namespaces).InnerText);
                                returnxml.Append("</Neighbor>");
                            }
                        }
                        returnxml.Append("</NeighborList>");

                        returnxml.Append("<KeywordList TotalKeywordCount=\"" + RDF.SelectNodes("rdf:RDF/rdf:Description[1]/vivo:hasResearchArea", namespaces).Count + "\">");
                        foreach (XmlNode keyword in RDF.SelectNodes("rdf:RDF/rdf:Description[1]/vivo:hasResearchArea", namespaces))
                        {
                            returnxml.Append("<Keyword>");
                            returnxml.Append(RDF.SelectSingleNode("rdf:RDF[1]/rdf:Description[@rdf:about='" + keyword.SelectSingleNode("@rdf:resource", namespaces).Value + "']/rdfs:label", namespaces).InnerText);
                            returnxml.Append("</Keyword>");
                        }
                        returnxml.Append("</KeywordList>");
                        returnxml.Append("</PassiveNetworks>");
                    }

                    returnxml.Append("</Person>");

                    count++;
                }
                else
                {
                    if (person.SelectSingleNode("@rdf:nodeID", namespaces) != null)
                        returnxml.Append("<PersonID>" + RDF.SelectSingleNode("rdf:RDF[1]/rdf:Description[@rdf:nodeID='" + RDF.SelectSingleNode("rdf:RDF[1]/rdf:Description[@rdf:nodeID='" + person.SelectSingleNode("@rdf:nodeID", namespaces).Value + "']/rdf:object/@rdf:nodeID", namespaces).Value + "']/prns:personId", namespaces).InnerText + "</PersonID></Person>");
                }
            }

            string complete = "false";
            string totalcount = string.Empty;

            if (!individual)
            {
                if (count.ToString() == RDF.SelectSingleNode("rdf:RDF[1]/rdf:Description/prns:numberOfConnections", namespaces).InnerText)
                    complete = "true";

                totalcount = RDF.SelectSingleNode("rdf:RDF[1]/rdf:Description/prns:numberOfConnections", namespaces).InnerText;

            }
            else
            {

                count = 1;
                totalcount = count.ToString();
                complete = "true";

            }

            topnode = "<?xml version=\"1.0\"?><PersonList Complete=\"" + complete + "\"  ThisCount=\"" + count.ToString() + "\"  TotalCount=\"" +
                      totalcount + "\"  QueryID=\"" +
                        queryid + "\"  xmlns=\"http://connects.profiles.schema/profiles/personlist\"  xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"" +
                        " xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">";


            returnxml.Append("</PersonList>");

            string returnstring = (topnode + returnxml.ToString()).Replace("&", "&amp;");

            return returnstring;
        }

        public string GetRESTBasePath()
        {
            string rtn = string.Empty;

            if (ServiceImplementation.Cache.Fetch("GetRESTBasePath") == null)
            {

                string sql = "select [value] from [Framework.].[parameter] with(nolock) where parameterid = 'basepath'";

                SqlDataReader sqldr = this.GetSQLDataReader("", sql, CommandType.Text, CommandBehavior.CloseConnection, null);

                while (sqldr.Read())
                {
                    rtn = sqldr[0].ToString();
                }


                if (!sqldr.IsClosed)
                    sqldr.Close();

                ServiceImplementation.Cache.Set("GetRESTBasePath", rtn);
            }
            else
            {
                rtn = (string)ServiceImplementation.Cache.FetchObject("GetRESTBasePath");
            }

            return rtn;
        }



        public List<GenericListItem> GetFacultyRanks()
        {
            List<GenericListItem> ranks = new List<GenericListItem>();
            try
            {
                string sql = "EXEC [Profile.Data].[Person.GetFacultyRanks]";

                SqlDataReader sqldr = this.GetSQLDataReader("", sql, CommandType.Text, CommandBehavior.CloseConnection, null);

                while (sqldr.Read())
                    ranks.Add(new GenericListItem(sqldr["FacultyRank"].ToString(), sqldr["URI"].ToString()));

                //Always close your readers
                if (!sqldr.IsClosed)
                    sqldr.Close();


            }
            catch (Exception e)
            {

            }


            return ranks;
        }

        public List<GenericListItem> GetFacultyRanks(string rankstrings)
        {
            List<GenericListItem> ranks = new List<GenericListItem>();


            rankstrings = rankstrings.Replace(" ,", ",").Replace(", ", ",");
            string[] items = rankstrings.Split(',');


            foreach (GenericListItem gli in this.GetFacultyRanks())
            {
                if (items.Contains(gli.Text))
                {
                    ranks.Add(new GenericListItem(gli.Text, gli.Value));

                }

            }

            return ranks;

        }



        public Int64 GetTotalSearchConnections(XmlDocument searchdata, XmlNamespaceManager namespaces)
        {
            Int64 totalcount = 0;

            foreach (XmlNode x in searchdata.SelectNodes("//prns:matchesClassGroup/prns:numberOfConnections", namespaces))
            {
                totalcount = totalcount + Convert.ToInt64(x.InnerText);
            }

            return totalcount;

        }


        private PersonData GetPersonData(Int32 PersonId)
        {
            System.Text.StringBuilder sql = new System.Text.StringBuilder();
            PersonData person;

            string connstr = ConfigurationManager.ConnectionStrings["ProfilesDB"].ConnectionString;

            sql.AppendLine("select * from [Profile.Data].[person] where personid = " + PersonId.ToString());

            SqlConnection dbconnection = new SqlConnection(connstr);
            SqlCommand dbcommand = new SqlCommand();

            SqlDataReader dbreader;
            dbconnection.Open();
            dbcommand.CommandType = CommandType.Text;

            dbcommand.CommandText = sql.ToString();
            dbcommand.CommandTimeout = 5000;

            dbcommand.Connection = dbconnection;

            dbreader = dbcommand.ExecuteReader(CommandBehavior.CloseConnection);

            dbreader.Read();

            person = new PersonData(dbreader["EmailAddr"].ToString(), dbreader["Phone"].ToString(), dbreader["Fax"].ToString(),
                dbreader["AddressLine1"].ToString(), dbreader["AddressLine2"].ToString(), dbreader["AddressLine3"].ToString(),
                dbreader["AddressLine4"].ToString(), dbreader["City"].ToString(), dbreader["State"].ToString(),
                dbreader["Zip"].ToString(), dbreader["Building"].ToString(), dbreader["Floor"].ToString(),
                dbreader["Room"].ToString(), dbreader["Latitude"].ToString(), dbreader["Longitude"].ToString(),
                dbreader["FacultyRankID"].ToString(), dbreader["AddressString"].ToString());

            if (dbcommand.Connection.State == ConnectionState.Open)
                dbreader.Close();

            return person;

        }

        private List<CustomPub> GetCustomPubs(Int64 nodeid)
        {
            System.Text.StringBuilder sql = new System.Text.StringBuilder();
            List<CustomPub> pubs = new List<CustomPub>();

            string connstr = ConfigurationManager.ConnectionStrings["ProfilesDB"].ConnectionString;

            sql.AppendLine("Declare @person Bigint");

            sql.AppendLine("select @person = i.internalid from  [RDF.Stage].internalnodemap i with(nolock) where i.nodeid = " + nodeid.ToString());

            sql.AppendLine("select * FROM [Profile.Data].[Publication.MyPub.General] where mpid in(select mpid from [Profile.Data].[Publication.Person.Include] where personid = @person)");


            SqlConnection dbconnection = new SqlConnection(connstr);
            SqlCommand dbcommand = new SqlCommand();

            SqlDataReader dbreader;
            dbconnection.Open();
            dbcommand.CommandType = CommandType.Text;

            dbcommand.CommandText = sql.ToString();
            dbcommand.CommandTimeout = 5000;

            dbcommand.Connection = dbconnection;

            dbreader = dbcommand.ExecuteReader(CommandBehavior.CloseConnection);

            while (dbreader.Read())
            {
                pubs.Add(new CustomPub(dbreader["MPID"].ToString(), dbreader["HmsPubCategory"].ToString(), dbreader["PubTitle"].ToString(),
                    dbreader["ArticleTitle"].ToString(), dbreader["EDITION"].ToString(), dbreader["Publisher"].ToString()));
            }

            if (dbcommand.Connection.State == ConnectionState.Open)
                dbreader.Close();

            return pubs;

        }

        private int GetPersonID(Int64 nodeid)
        {
            System.Text.StringBuilder sql = new System.Text.StringBuilder();
            int personid = 0;

            string connstr = ConfigurationManager.ConnectionStrings["ProfilesDB"].ConnectionString;


            sql.AppendLine("select i.internalid from  [RDF.Stage].internalnodemap i with(nolock) where  i.class = 'http://xmlns.com/foaf/0.1/Person' and i.nodeid = " + nodeid);

            SqlConnection dbconnection = new SqlConnection(connstr);
            SqlCommand dbcommand = new SqlCommand();

            SqlDataReader dbreader;
            dbconnection.Open();
            dbcommand.CommandType = CommandType.Text;

            dbcommand.CommandText = sql.ToString();
            dbcommand.CommandTimeout = 5000;

            dbcommand.Connection = dbconnection;

            dbreader = dbcommand.ExecuteReader(CommandBehavior.CloseConnection);

            while (dbreader.Read())
            {
                personid = Convert.ToInt32(dbreader[0].ToString());
            }

            if (!dbreader.IsClosed)
                dbreader.Close();


            return personid;


        }


        public int GetPersonID(string ecomid)
        {
            System.Text.StringBuilder sql = new System.Text.StringBuilder();
            int personid = 0;

            string connstr = ConfigurationManager.ConnectionStrings["ProfilesDB"].ConnectionString;

            sql.AppendLine("select ProfilesUserID FROM [resnav_home].[dbo].[MPI] where eCommonsLogin ='" + ecomid + "'");

            SqlConnection dbconnection = new SqlConnection(connstr);
            SqlCommand dbcommand = new SqlCommand();

            SqlDataReader dbreader;
            dbconnection.Open();
            dbcommand.CommandType = CommandType.Text;

            dbcommand.CommandText = sql.ToString();
            dbcommand.CommandTimeout = 5000;

            dbcommand.Connection = dbconnection;

            dbreader = dbcommand.ExecuteReader(CommandBehavior.CloseConnection);

            while (dbreader.Read())
            {
                personid = Convert.ToInt32(dbreader[0].ToString());
            }

            if (!dbreader.IsClosed)
                dbreader.Close();


            return personid;


        }


        private int GetPubCount(Int64 nodeid)
        {
            System.Text.StringBuilder sql = new System.Text.StringBuilder();

            int count = 0;

            string connstr = ConfigurationManager.ConnectionStrings["ProfilesDB"].ConnectionString;

            sql.AppendLine("Declare @person Bigint");

            sql.AppendLine("select @person = i.internalid from  [RDF.Stage].internalnodemap i with(nolock) where i.nodeid = " + nodeid.ToString());

            sql.AppendLine("select count(*) from [Profile.Data].[Publication.Person.Include] where personid = @person ");


            SqlConnection dbconnection = new SqlConnection(connstr);
            SqlCommand dbcommand = new SqlCommand();

            SqlDataReader dbreader;
            dbconnection.Open();
            dbcommand.CommandType = CommandType.Text;

            dbcommand.CommandText = sql.ToString();
            dbcommand.CommandTimeout = 5000;

            dbcommand.Connection = dbconnection;

            dbreader = dbcommand.ExecuteReader(CommandBehavior.CloseConnection);

            while (dbreader.Read())
            {
                count = Convert.ToInt32(dbreader[0].ToString());
            }

            if (!dbreader.IsClosed)
                dbreader.Close();

            return count;

        }

        private string NameSort(string direction)
        {
            string sort = "<SortBy IsDesc=\"" + (direction == "desc" ? "1" : "0") + "\" Property=\"http://xmlns.com/foaf/0.1/lastName\" />";
            sort += "<SortBy IsDesc=\"0\" Property=\"http://xmlns.com/foaf/0.1/firstName\" />";


            return sort;
        }

        private string TitleSort(string direction)
        {
            string sort = "<SortBy IsDesc=\"" + (direction == "desc" ? "1" : "0") + "\" Property=\"http://profiles.catalyst.harvard.edu/ontology/prns#personInPrimaryPosition\" Property2=\"http://www.w3.org/2000/01/rdf-schema#label\"/>";
            sort += "<SortBy IsDesc=\"0\" Property=\"http://xmlns.com/foaf/0.1/lastName\" />";

            return sort;
        }
        private string InstitutionSort(string direction)
        {
            string sort = "<SortBy IsDesc=\"" + (direction == "desc" ? "1" : "0") + "\" Property=\"http://profiles.catalyst.harvard.edu/ontology/prns#personInPrimaryPosition\" Property2=\"http://vivoweb.org/ontology/core#positionInOrganization\"  Property3=\"http://www.w3.org/2000/01/rdf-schema#label\"/>";
            sort += "<SortBy IsDesc=\"0\" Property=\"http://xmlns.com/foaf/0.1/lastName\" />";

            return sort;
        }
        private string DepartmentSort(string direction)
        {
            string sort = "<SortBy IsDesc=\"" + (direction == "desc" ? "1" : "0") + "\" Property=\"http://profiles.catalyst.harvard.edu/ontology/prns#personInPrimaryPosition\" Property2=\"http://vivoweb.org/ontology/core#positionInDepartment\"  Property3=\"http://www.w3.org/2000/01/rdf-schema#label\"/>";
            sort += "<SortBy IsDesc=\"0\" Property=\"http://xmlns.com/foaf/0.1/lastName\" />";

            return sort;
        }
        private string DivisionSort(string direction)
        {
            string sort = "<SortBy IsDesc=\"" + (direction == "desc" ? "1" : "0") + "\" Property=\"http://profiles.catalyst.harvard.edu/ontology/prns#personInPrimaryPosition\" Property2=\"http://vivoweb.org/ontology/core#positionInDepartment\"  Property3=\"http://www.w3.org/2000/01/rdf-schema#label\"/>";
            sort += "<SortBy IsDesc=\"0\" Property=\"http://xmlns.com/foaf/0.1/lastName\" />";

            return sort;
        }
        private string FilterSort(string direction)
        {
            string sort = "<SortBy IsDesc=\"" + (direction == "desc" ? "1" : "0") + "\" Property=\"http://profiles.catalyst.harvard.edu/ontology/prns#personInPrimaryPosition\" Property2=\"http://vivoweb.org/ontology/core#positionInDepartment\"  Property3=\"http://www.w3.org/2000/01/rdf-schema#label\"/>";
            sort += "<SortBy IsDesc=\"0\" Property=\"http://xmlns.com/foaf/0.1/lastName\" />";

            return sort;
        }


        /// <summary>
        /// To return the list of all the Divisions
        /// </summary>
        /// <returns></returns>
        public List<Utility.GenericListItem> GetDivisions()
        {

            List<Utility.GenericListItem> divisions = new List<Utility.GenericListItem>();


            if (Utility.CacheUtil.FetchObject("GetDivisions") == null)
            {
                try
                {

                    string sql = "EXEC [Profile.Data].[Organization.GetDivisions]";

                    SqlDataReader sqldr = this.GetSQLDataReader("", sql, CommandType.Text, CommandBehavior.CloseConnection, null);

                    while (sqldr.Read())
                        divisions.Add(new Utility.GenericListItem(sqldr["DivisionName"].ToString(), sqldr["URI"].ToString()));
                    //Always close your readers
                    if (!sqldr.IsClosed)
                        sqldr.Close();


                    //Defaulted this to be one hour
                    Utility.CacheUtil.Set("GetDivisions", divisions, 3600);


                }
                catch (Exception e)
                {

                    throw new Exception(e.Message);
                }
            }
            else
            {
                divisions = (List<Utility.GenericListItem>)Utility.CacheUtil.FetchObject("GetDivisions");

            }




            return divisions;
        }
        public string ConvertDivision(string division)
        {

            List<Utility.GenericListItem> divisions = GetDivisions();
            return this.GetConvertedListItem(divisions, division);
        }
        public string GetInstitutionAbbreviation(string institutionname)
        {
            string abb = string.Empty;

            try
            {
                string sql = "SELECT [InstitutionAbbreviation] FROM [Profile.Data].[Organization.Institution] where [InstitutionName] = '" + institutionname.Replace("'", "''") + "'";

                SqlDataReader sqldr = this.GetSQLDataReader("", sql, CommandType.Text, CommandBehavior.CloseConnection, null);

                sqldr.Read();

                abb = sqldr["InstitutionAbbreviation"].ToString();

                //Always close your readers
                if (!sqldr.IsClosed)
                    sqldr.Close();

            }
            catch (Exception e)
            {
                //do nothing.   
            }

            return abb;

        }

        /// <summary>
        /// To return the list of all the Institutions
        /// </summary>
        /// <returns></returns>
        public List<Utility.GenericListItem> GetInstitutions()
        {
            List<Utility.GenericListItem> institutions = new List<Utility.GenericListItem>();

            if (Utility.CacheUtil.FetchObject("GetInstitutions") == null)
            {
                try
                {

                    string sql = "EXEC [Profile.Data].[Organization.GetInstitutions]";

                    SqlDataReader sqldr = this.GetSQLDataReader("", sql, CommandType.Text, CommandBehavior.CloseConnection, null);

                    while (sqldr.Read())
                        institutions.Add(new Utility.GenericListItem(sqldr["InstitutionName"].ToString(), sqldr["URI"].ToString()));

                    //Always close your readers
                    if (!sqldr.IsClosed)
                        sqldr.Close();

                    //Defaulted this to be one hour
                    Utility.CacheUtil.Set("GetInstitutions", institutions, 3600);


                }
                catch (Exception e)
                {

                    throw new Exception(e.Message);
                }
            }
            else
            {
                institutions = (List<Utility.GenericListItem>)Utility.CacheUtil.FetchObject("GetInstitutions");

            }

            return institutions;
        }
        public string ConvertInstitution(string institution)
        {

            List<Utility.GenericListItem> institutions = GetInstitutions();
            return this.GetConvertedListItem(institutions, institution);
        }

        /// <summary>
        /// To return the list of all the Departments
        /// </summary>
        /// <returns></returns>
        public List<Utility.GenericListItem> GetDepartments()
        {
            List<Utility.GenericListItem> departments = new List<Utility.GenericListItem>();

            if (Utility.CacheUtil.FetchObject("GetDepartments") == null)
            {
                try
                {


                    string sql = "EXEC [Profile.Data].[Organization.GetDepartments]";


                    SqlDataReader sqldr = this.GetSQLDataReader("", sql, CommandType.Text, CommandBehavior.CloseConnection, null);

                    while (sqldr.Read())
                        departments.Add(new Utility.GenericListItem(sqldr["Department"].ToString(), sqldr["URI"].ToString()));

                    //Always close your readers
                    if (!sqldr.IsClosed)
                        sqldr.Close();

                    //Defaulted this to be one hour
                    Utility.CacheUtil.Set("GetDepartments", departments, 3600);

                }
                catch (Exception e)
                {

                    throw new Exception(e.Message);
                }



            }
            else
            {
                departments = (List<Utility.GenericListItem>)Utility.CacheUtil.FetchObject("GetDepartments");

            }
            return departments;
        }
        public string ConvertDepartment(string department)
        {
            List<Utility.GenericListItem> departments = GetDepartments();
            return this.GetConvertedListItem(departments, department);
        }

        public DataSet GetFilters()
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da;


            if (Utility.CacheUtil.FetchObject("GetFilters") == null)
            {
                try
                {

                    string sql = "EXEC [Profile.Data].[Person.GetFilters]";
                    SqlConnection conn = this.GetDBConnection("");
                    da = new SqlDataAdapter(sql, conn);

                    da.Fill(ds, "Table");
                    //Defaulted this to be one hour
                    Utility.CacheUtil.Set("GetFilters", ds, 3600);

                    conn.Close();

                }
                catch (Exception e)
                {

                    throw new Exception(e.Message);
                }
            }
            else
            {
                ds = (DataSet)Utility.CacheUtil.FetchObject("GetFilters");

            }
            return ds;

        }

        /// <summary>
        /// Get a list of person types from the database.
        /// Logically reorganized to be in the correct format for the ComboTreeCheck control
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet GetPersonTypes()
        {
            DataSet personTypesReturn;

            DataSet personTypes = this.GetFilters();
            personTypesReturn = new DataSet();

            // New table for the master records
            personTypesReturn.Tables.Add("DataMasterName");
            personTypesReturn.Tables[0].Columns.Add("personTypeGroupId", Type.GetType("System.Int32"));
            personTypesReturn.Tables[0].Columns.Add("personTypeGroup", Type.GetType("System.String"));
            personTypesReturn.Tables[0].Columns.Add("Expanded", Type.GetType("System.Boolean"));

            // New table for the detail records
            personTypesReturn.Tables.Add("DataDetailName");
            personTypesReturn.Tables[1].Columns.Add("personTypeGroupId", Type.GetType("System.Int32"));
            personTypesReturn.Tables[1].Columns.Add("personTypeFlagId", Type.GetType("System.Int32"));
            personTypesReturn.Tables[1].Columns.Add("personTypeFlag", Type.GetType("System.String"));
            personTypesReturn.Tables[1].Columns.Add("Checked", Type.GetType("System.Boolean"));

            string lastParentTag = "";
            string currentParentTag = "";
            string currentChildTag = "";
            int parentRowId = 0;
            int childRowId = 1;
            bool getChild = false;

            foreach (DataRow pRow in personTypes.Tables[0].Rows)
            {
                currentParentTag = Convert.ToString(pRow["PersonFilterCategory"]);
                currentChildTag = Convert.ToString(pRow["personfilter"]);
                getChild = false;

                if (currentParentTag != lastParentTag)
                {
                    // We have a new parent
                    parentRowId++;
                    personTypesReturn.Tables[0].Rows.Add(parentRowId, currentParentTag);
                    lastParentTag = currentParentTag;

                    // Also get child
                    getChild = true;
                }
                else
                {
                    // We only have child
                    getChild = true;
                }

                if (getChild)
                {
                    personTypesReturn.Tables[1].Rows.Add(parentRowId, childRowId, currentChildTag);
                    childRowId++;
                }
            }


            return personTypesReturn;
        }

        private List<Utility.GenericListItem> GetOtherOptions(string otheroptions)
        {

            List<Utility.GenericListItem> filters = new List<Utility.GenericListItem>();
            DataSet ds = this.GetFilters();
            otheroptions = otheroptions.Replace(" ,", ",").Replace(", ", ",");
            string[] items = otheroptions.Split(',');


            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (items.Contains(dr[2].ToString()))
                    filters.Add(new Utility.GenericListItem(dr[2].ToString(), dr[5].ToString()));

            }

            return filters;


        }

        public string GetConvertedListItem(List<Utility.GenericListItem> listitems, string itemtoconvert)
        {
            Utility.GenericListItem rtnlistitem = null;
            rtnlistitem = listitems.Find(delegate(Utility.GenericListItem module) { return module.Text == itemtoconvert; });

            return rtnlistitem.Value;
        }

        public CustomPub GetCustomPubItem(List<CustomPub> pubs, string mpid)
        {
            CustomPub rtnitem = null;
            rtnitem = pubs.Find(delegate(CustomPub item) { return item.MPID == mpid; });

            return rtnitem;
        }

        /// <summary>
        /// Wraper xmlreader so that database connection and xmlreader will be closed 
        /// after XmlReaderScope is disposed
        /// </summary>
        public class XmlReaderScope : IDisposable
        {
            private SqlConnection _dbConnection = null;
            private XmlReader _reader = null;

            public XmlReader Reader
            {
                get { return _reader; }
            }

            /// <summary>
            /// constructor, this class is only for internal use
            /// </summary>
            /// <param name="conn"></param>
            /// <param name="reader"></param>
            internal XmlReaderScope(SqlConnection conn, XmlReader reader)
            {
                _dbConnection = conn;
                _reader = reader;

            }

            private void Close()
            {

                if (_dbConnection != null && _dbConnection.State != ConnectionState.Closed)
                    _dbConnection.Close();

                if (_reader != null)
                    _reader.Close();
            }

            #region IDisposable Members

            protected virtual void Dispose(bool disposing)
            {
                if (disposing)
                    Close();
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }





            #endregion
        }


    }

    public class PersonData
    {
        public PersonData(string emailaddr,
        string phone,
        string fax,
        string addressline1,
        string addressline2,
        string addressline3,
        string addressline4,
        string city,
        string state,
        string zip,
        string building,
        string floor,
        string room,
        string addressstring,
        string latitude,
        string longitude,
        string facultyrankid)
        {
            EmailAddr = emailaddr;
            Phone = phone;
            Fax = fax;
            AddressLine1 = addressline1;
            AddressLine2 = addressline2;
            AddressLine3 = addressline3;
            AddressLine4 = addressline4;
            City = city;
            State = state;
            Zip = zip;
            Building = building;
            Floor = floor;
            Room = room;
            Latitude = latitude;
            Longitude = longitude;
            FacultyRankID = facultyrankid;
            AddressString = addressstring;
        }

        public string EmailAddr { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Building { get; set; }
        public string Floor { get; set; }
        public string Room { get; set; }
        public string AddressString { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string FacultyRankID { get; set; }

    }

    public class CustomPub
    {

        public CustomPub(string mpid, string hmspubcategory, string pubtitle,
            string articletitle, string edition, string publisher)
        {

            this.MPID = mpid;
            this.HmsPubCategory = hmspubcategory;
            this.PubTitle = pubtitle;
            this.ArticleTitle = articletitle;
            this.EDITION = edition;
            this.Publisher = publisher;
        }

        public string MPID { get; set; }
        public string HmsPubCategory { get; set; }
        public string PubTitle { get; set; }
        public string ArticleTitle { get; set; }
        public string EDITION { get; set; }
        public string Publisher { get; set; }

    }

    /// <summary>
    /// Wraper xmlreader so that database connection and xmlreader will be closed 
    /// after XmlReaderScope is disposed
    /// </summary>
    public class XmlReaderScope : IDisposable
    {
        private SqlConnection _dbConnection = null;
        private XmlReader _reader = null;

        public XmlReader Reader
        {
            get { return _reader; }
        }

        /// <summary>
        /// constructor, this class is only for internal use
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="reader"></param>
        internal XmlReaderScope(SqlConnection conn, XmlReader reader)
        {
            _dbConnection = conn;
            _reader = reader;

        }

        private void Close()
        {

            if (_dbConnection != null && _dbConnection.State != ConnectionState.Closed)
                _dbConnection.Close();

            if (_reader != null)
                _reader.Close();
        }

        #region IDisposable Members

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                Close();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }





        #endregion

    }

    public class User
    {
        public string UserName { get; set; }
        public int UserID { get; set; }
        public string UsernameType { get; set; }
    }



}
