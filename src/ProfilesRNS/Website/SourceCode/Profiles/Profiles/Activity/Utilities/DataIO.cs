/*  
 
    Copyright (c) 2008-2012 by the President and Fellows of Harvard College. All rights reserved.  
    Profiles Research Networking Software was developed under the supervision of Griffin M Weber, MD, PhD.,
    and Harvard Catalyst: The Harvard Clinical and Translational Science Center, with support from the 
    National Center for Research Resources and Harvard University.


    Code licensed under a BSD License. 
    For details, see: LICENSE.txt 
  
*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Profiles.Framework.Utilities;
using System.Linq;

namespace Profiles.Activity.Utilities
{

    public class DataIO : Framework.Utilities.DataIO
    {

        private static readonly int activityCacheSize = 1000;
        private static readonly int cacheExpirationSeconds = 36000; // 10 hours
        private static readonly int chechForNewActivitiesSeconds = 60; // once a minute

        private readonly object syncLock = new object();
        private Random random = new Random();

        public List<Activity> GetActivity(Int64 lastActivityLogID, int count, bool declump)
        {
            List<Activity> activities = new List<Activity>();
            SortedList<Int64, Activity> cache = GetFreshCache();
            // grab as many as you can from the cache
            if (lastActivityLogID == -1)
            {
                activities.AddRange(cache.Values);
            }
            else if (cache.IndexOfKey(lastActivityLogID) != -1)
            {
                activities.AddRange(cache.Values);
                activities.RemoveRange(0, cache.IndexOfKey(lastActivityLogID) + 1);
            }

            List<Activity> retval = activities;

            if (declump)
            {
                retval = GetUnclumpedSubset(activities, count);
            }
            else if (count < retval.Count)
            {
                retval.RemoveRange(count, activities.Count - count);
            }

            if (count > retval.Count && retval.Count > 0)
            {
                // we need to go to the DB to get more. If we are declumping, we don't know exacly how many more we need but we make a good guess
                // and loop as needed
                if (declump)
                {
                    while (count > retval.Count)
                    {
                        SortedList<Int64, Activity> newActivities = GetRecentActivity(activities[activities.Count - 1].Id, 10 * (count - retval.Count), true);
                        if (newActivities.Count == 0)
                        {
                            // nothing more to load, time to bail
                            break;
                        }
                        else
                        {
                            activities.AddRange(newActivities.Values);
                            retval = GetUnclumpedSubset(activities, count);
                        }
                    }
                }
                else
                {
                    retval.AddRange(GetRecentActivity(retval[retval.Count - 1].Id, count - retval.Count, true).Values);
                }
            }
            return retval;
        }

        // makes sure you do not get consecutive activites for the same person. Instead, just randomly pick one of the activities in the consecutive 'clump'
        private List<Activity> GetUnclumpedSubset(List<Activity> activities, int count)
        {
            int id = -1;
            List<Activity> clumpedList = new List<Activity>();
            List<Activity> subset = new List<Activity>();

            foreach (Activity activity in activities)
            {
                if (id != activity.Profile.PersonId)
                {
                    //grab a random one from the old clumpedList
                    if (clumpedList.Count > 0)
                    {
                        subset.Add(clumpedList[random.Next(0, clumpedList.Count)]);
                        if (subset.Count == count)
                        {
                            clumpedList.Clear();
                            break;
                        }
                    }
                    // start a new clump for the new person
                    clumpedList.Clear();
                    id = activity.Profile.PersonId;
                }
                clumpedList.Add(activity);
            }
            // add the last one if needed
            if (clumpedList.Count > 0)
            {
                subset.Add(clumpedList[random.Next(0, clumpedList.Count)]);
            }

            return subset;        
        }

        private SortedList<Int64, Activity> GetFreshCache()
        {
            SortedList<Int64, Activity> cache = (SortedList<Int64, Activity>)Framework.Utilities.Cache.FetchObject("ActivityHistory");
            object isFresh = Framework.Utilities.Cache.FetchObject("ActivityHistoryIsFresh");
            if (cache == null)
            {
                // Grab a whole new one. This is expensive and should be unnecessary if we manage getting new ones well, so we don't do this often
                cache = GetRecentActivity(-1, activityCacheSize, true);
                Framework.Utilities.Cache.SetWithTimeout("ActivityHistory", cache, cacheExpirationSeconds);
            }
            else if (isFresh == null)
            {
                lock (syncLock)
                {
                    // get new ones from the DB

                    Int64 lastActivityLogID = cache.Count == 0 ? -1 : cache.Values[0].Id;
                    SortedList<Int64, Activity> newActivities = GetRecentActivity(lastActivityLogID, activityCacheSize, false);

                    // in with the new
                    foreach (Activity activity in newActivities.Values)
                        cache.Add(activity.Id, activity);
                    // out with the old
                    while (cache.Count > activityCacheSize)
                        cache.RemoveAt(cache.Count - 1);
                }
                // look for new activities once every minute
                Framework.Utilities.Cache.SetWithTimeout("ActivityHistoryIsFresh", new object(), chechForNewActivitiesSeconds);
            }
            return cache;
        }

        private SortedList<Int64, Activity> GetRecentActivity(Int64 lastActivityLogID, int count, bool older)
        {
            SortedList<Int64, Activity> activities = new SortedList<Int64, Activity>(new ReverseComparer());

            string sql = "SELECT top " + count + "  i.activityLogID," +
                            "p.personid,n.nodeid,p.firstname,p.lastname," +
                            "i.methodName,i.property,cp._PropertyLabel as propertyLabel,i.param1,i.param2,i.createdDT " +
                            "FROM [Framework.].[Log.Activity] i " +
                            "LEFT OUTER JOIN [Profile.Data].[Person] p ON i.personId = p.personID " +
                            "LEFT OUTER JOIN [RDF.Stage].internalnodemap n on n.internalid = p.personId and n.[class] = 'http://xmlns.com/foaf/0.1/Person' " +
                            "LEFT OUTER JOIN [Ontology.].[ClassProperty] cp ON cp.Property = i.property  and cp.Class = 'http://xmlns.com/foaf/0.1/Person' " +
                            "LEFT OUTER JOIN [RDF.].[Node] rn on [RDF.].fnValueHash(null, null, i.property) = rn.ValueHash " +
                            "LEFT OUTER JOIN [RDF.Security].[NodeProperty] np on n.NodeID = np.NodeID and rn.NodeID = np.Property " +
                            "where p.IsActive=1 and (np.ViewSecurityGroup = -1 or (i.privacyCode = -1 and np.ViewSecurityGroup is null) or (i.privacyCode is null and np.ViewSecurityGroup is null))" +
                            (lastActivityLogID != -1 ? (" and i.activityLogID " + (older ? "< " : "> ") + lastActivityLogID) : "") +
                            "order by i.activityLogID desc";

            using (SqlDataReader reader = GetQueryOutputReader(sql))
            {
                while (reader.Read())
                {
                    string param1 = reader["param1"].ToString();
                    string param2 = reader["param2"].ToString();
                    string activityLogId = reader["activityLogId"].ToString();
                    string property = reader["property"].ToString();
                    string propertyLabel = reader["propertyLabel"].ToString();
                    string personid = reader["personid"].ToString();
                    string nodeid = reader["nodeid"].ToString();
                    string firstname = reader["firstname"].ToString();
                    string lastname = reader["lastname"].ToString();
                    string methodName = reader["methodName"].ToString();
                    string journalTitle = "";
                    string url = "";
                    string queryTitle = "";
                    string title = "";
                    string body = "";

                    if (param1 == "PMID" || param1 == "Add PMID")
                    {
                        url = "http://www.ncbi.nlm.nih.gov/pubmed/" + param2;
                        queryTitle = "SELECT JournalTitle FROM [Profile.Data].[Publication.PubMed.General] with(nolock) " +
                                        "WHERE PMID = cast(" + param2 + " as int)";

                        journalTitle = GetStringValue(queryTitle, "JournalTitle");
                    }

                    if (property == "http://vivoweb.org/ontology/core#ResearcherRole")
                    {
                        queryTitle = "select AgreementLabel from [Profile.Data].[Funding.Role] r " +
                                        "join [Profile.Data].[Funding.Agreement] a " +
                                        "on r.FundingAgreementID = a.FundingAgreementID " +
                                        " and r.FundingRoleID = '" + param1 + "'";

                        journalTitle = GetStringValue(queryTitle, "AgreementLabel");
                    }

                    if (methodName.CompareTo("Profiles.Edit.Utilities.DataIO.AddPublication") == 0)
                    {
                        title = "added a PubMed publication";
                        body = "added a publication from: " + journalTitle;
                    }
                    else if (methodName.CompareTo("Profiles.Edit.Utilities.DataIO.AddCustomPublication") == 0)
                    {
                        title = "added a custom publication";
                        if (param2.Length > 100) param2 = param2.Substring(0, 100) + "...";
                        body = "added \"" + param1 + "\" into " + propertyLabel +
                            " section : " + param2;
                    }
                    else if (methodName.CompareTo("Profiles.Edit.Utilities.DataIO.UpdateSecuritySetting") == 0)
                    {
                        title = "made a section visible";
                        body = "made \"" + propertyLabel + "\" public";
                    }
                    else if (methodName.CompareTo("Profiles.Edit.Utilities.DataIO.AddUpdateFunding") == 0)
                    {
                        title = "added a research activity or funding";
                        body = "added a research activity or funding: " + journalTitle;
                    }
                    else if (methodName.CompareTo("[Profile.Data].[Funding.LoadDisambiguationResults]") == 0)
                    {
                        title = "has a new research activity or funding";
                        body = "has a new research activity or funding: " + journalTitle;
                    }
                    else if (property == "http://vivoweb.org/ontology/core#hasMemberRole")
                    {
                        queryTitle = "select GroupName from [Profile.Data].[vwGroup.General] where GroupNodeID = " + param1;
                        string groupName = GetStringValue(queryTitle, "GroupName");
                        title = "joined group: " + groupName;
                        body = "joined group: " + groupName;
                    }
                    else if (methodName.IndexOf("Profiles.Edit.Utilities.DataIO.Add") == 0)
                    {
                        title = "added an item";

                        if (param1.Length != 0)
                        {
                            body = body = "added \"" + param1 + "\" into " + propertyLabel + " section";
                        }
                        else
                        {
                            body = "added \"" + propertyLabel + "\" section";
                        }
                    }
                    else if (methodName.IndexOf("Profiles.Edit.Utilities.DataIO.Update") == 0)
                    {
                        title = "updated an item";

                        if (param1.Length != 0)
                        {
                            body = "updated \"" + param1 + "\" in " + propertyLabel + " section";
                        }
                        else
                        {
                            body = "updated \"" + propertyLabel + "\" section";
                        }
                    }
                    else if (methodName.CompareTo("[Profile.Data].[Publication.Pubmed.LoadDisambiguationResults]") == 0 && param1.CompareTo("Add PMID") == 0)
                    {
                        title = "has a new PubMed publication";
                        body = "has a new publication listed from: " + journalTitle;
                    }
                    else if (methodName.CompareTo("[Profile.Import].[LoadProfilesData]") == 0 && param1.CompareTo("Person Insert") == 0)
                    {
                        title = "added to Profiles";
                        body = "now has a Profile page";
                    }

                    if (!String.IsNullOrEmpty(title))
                    {
                        try
                        {
                            Activity act = new Activity
                            {
                                Id = Convert.ToInt64(activityLogId),
                                Message = body.Trim(),
                                LinkUrl = url.Trim(),
                                Title = title.Trim(),
                                CreatedDT = Convert.ToDateTime(reader["CreatedDT"]),
                                CreatedById = activityLogId,
                                Profile = new Profile
                                {
                                    Name = firstname.Trim() + " " + lastname.Trim(),
                                    PersonId = Convert.ToInt32(personid),
                                    NodeID = Convert.ToInt64(nodeid),
                                    URL = Root.Domain + "/profile/" + nodeid,
                                    Thumbnail = Root.Domain + "/profile/Modules/CustomViewPersonGeneralInfo/PhotoHandler.ashx?NodeID=" + nodeid + "&Thumbnail=True&Width=45"
                                }
                            };
                            activities.Add(act.Id, act);
                        }
                        catch (Exception e) { }
                    }
                }
            }
            return activities;
        }

        public List<ActivityLogItem> GetRecentActivities(int pageSize, int offset = 0)
        {
            if(pageSize <= 0)
            {
                return new List<ActivityLogItem>();
            }

            string sql = $@"
            SELECT DISTINCT
                i.activityLogID,
                i.createdDT as activityDate,
	            CASE i.methodName
		            WHEN '[Profile.Data].[Publication.Pubmed.LoadDisambiguationResults]' THEN
			            CASE
			            WHEN (pm.ArticleYear IS NOT NULL) THEN
				            PARSE(TRIM(CONCAT(pm.ArticleYear, ' ', COALESCE(pm.ArticleMonth, '01'), ' ', COALESCE(pm.ArticleDay, '01'))) AS DATETIME)
			            WHEN (pm.JournalYear IS NOT NULL) THEN
				            PARSE(TRIM(CONCAT(pm.JournalYear, ' ', COALESCE(pm.JournalMonth, '01'), ' ', COALESCE(pm.JournalDay, '01'))) AS DATETIME)
			            WHEN (fa.StartDate IS NOT NULL) THEN
				            fa.[StartDate]
			            END
	            ELSE
		            i.[createdDT]
	            END AS [contentDate],
	            i.methodName,
                i.param1,
                i.param2,
                n.nodeid as personNodeID,
                p.firstname as firstName,
                p.lastname as lastName,
                i.property,
                cp._PropertyLabel as propertyLabel,
	            pm.ArticleYear,
	            pm.ArticleMonth,
	            pm.JournalYear,
	            pm.JournalMonth,
	            pm.journalTitle,
	            pm.articleTitle,
	            pmt.[Subject] AS articleNodeID,
	            pm.PMID AS pubMedID,
	            fa.agreementLabel
            FROM [Framework.].[Log.Activity] i
            LEFT JOIN [Profile.Data].[Person] p ON i.personId = p.personID
            LEFT JOIN [RDF.Stage].[internalnodemap] n on n.internalid = p.personId and n.[class] = 'http://xmlns.com/foaf/0.1/Person'
            LEFT JOIN [Ontology.].[ClassProperty] cp ON cp.Property = i.property  and cp.[Class] = 'http://xmlns.com/foaf/0.1/Person'
            LEFT JOIN [RDF.].[Node] rn ON [RDF.].fnValueHash(null, null, i.property) = rn.ValueHash
            LEFT JOIN [RDF.Security].[NodeProperty] np ON n.NodeID = np.NodeID and rn.NodeID = np.Property
            LEFT JOIN [Profile.Data].[Publication.PubMed.General] pm ON (i.param1 = 'PMID' OR i.param1 = 'Add PMID') AND pm.PMID = i.param2
            LEFT JOIN [RDF.].[Node] pmo ON (i.param1 = 'PMID' OR i.param1 = 'Add PMID') AND pmo.[Value] = i.param2
            LEFT JOIN [RDF.].[Triple] pmt ON pmt.[Object] = pmo.NodeID
            LEFT JOIN [Profile.Data].[Funding.Role] fr ON (i.property = 'http://vivoweb.org/ontology/core#ResearcherRole' AND fr.FundingRoleID = i.param1)
            LEFT JOIN [Profile.Data].[Funding.Agreement] fa ON fr.FundingAgreementID = fa.FundingAgreementID
            WHERE
                p.IsActive=1
                AND (
                    np.ViewSecurityGroup = -1
                    OR (i.privacyCode = -1 AND np.ViewSecurityGroup IS NULL)
                    OR (i.privacyCode IS NULL AND np.ViewSecurityGroup IS NULL)
                )
	            AND i.param1 != 'Person Update'
            ORDER BY contentDate DESC
                OFFSET {offset} ROWS
                FETCH NEXT {pageSize} ROWS ONLY
                ";

            using (SqlDataReader reader = GetQueryOutputReader(sql))
            {
                var result = new List<ActivityLogItem>();

                while (reader.Read())
                {
                    try
                    {
                        var profile = new Profile
                        {
                            NodeID = Convert.ToInt64(reader["personNodeID"]),
                            Name = $"{reader["firstName"]} {reader["lastName"]}",
                            URL = $"{Root.Domain}/profile/{reader["personNodeID"]}",
                            Thumbnail = $"{Root.Domain}/profile/Modules/CustomViewPersonGeneralInfo/PhotoHandler.ashx?NodeID={reader["personNodeID"]}&Thumbnail=True&Width=45"
                        };

                        var source = new ActivityLogItemSource()
                        {
                            Property = reader["property"].ToString(),
                            PropertyLabel = reader["propertyLabel"].ToString(),
                            Method = reader["methodName"].ToString(),
                            Param1 = reader["param1"].ToString(),
                            Param2 = reader["param2"].ToString()
                        };

                        var content = new Asset();
                        content.DatePublished = Convert.ToDateTime(reader["contentDate"]);

                        var activity = new ActivityLogItem();
                        activity.Id = Convert.ToInt64(reader["activityLogID"]);
                        activity.DateCreated = Convert.ToDateTime(reader["activityDate"]);
                        activity.Source = source;
                        activity.Content = content;
                        activity.Profile = profile;

                        var journalTitle = reader["journalTitle"];
                        var articleTitle = reader["articleTitle"];
                        var agreementTitle = reader["agreementLabel"];

                        switch (source.Method)
                        {
                            case "[Profile.Import].[LoadProfilesData]":
                                {
                                    if (source.Param1 == "Person Insert")
                                    {
                                        activity.Type = ActivityTypes.CreateProfileActivity;
                                    }
                                    else if (source.Param1 == "Person Update")
                                    {
                                        activity.Type = ActivityTypes.UpdateProfileActivity;
                                    }
                                    else
                                    {
                                        activity.Type = ActivityTypes.DeleteProfileActivity;
                                    }

                                    break;
                                }
                            case "Profiles.Edit.Utilities.DataIO.AddPublication":
                            case "Profiles.Edit.Utilities.DataIO.AddCustomPublication":
                            case "[Profile.Data].[Publication.Pubmed.LoadDisambiguationResults]":
                                {
                                    activity.Type = ActivityTypes.AddContentActivity;

                                    content.Type = AssetTypes.Publication;
                                    content.Title = articleTitle?.ToString() ?? source.Param1;
                                    content.Channel = journalTitle?.ToString() ?? source.PropertyLabel;
                                    content.URL = $"{Root.Domain}/profile/{reader["articleNodeID"]}";

                                    break;
                                }
                            case "Profiles.Edit.Utilities.DataIO.AddUpdateFunding":
                            case "[Profile.Data].[Funding.LoadDisambiguationResults]":
                                {
                                    activity.Type = ActivityTypes.AddContentActivity;

                                    content.Type = AssetTypes.Funding;
                                    content.Title = articleTitle?.ToString() ?? agreementTitle?.ToString();
                                    content.Channel = journalTitle?.ToString() ?? source.PropertyLabel;

                                    break;
                                }
                            case "Profiles.Edit.Utilities.DataIO.Add":
                                {
                                    activity.Type = ActivityTypes.AddContentActivity;

                                    content.Type = AssetTypes.Generic;
                                    content.Title = source.Param1;
                                    content.Channel = source.PropertyLabel;

                                    break;
                                }
                            case "Profiles.Edit.Utilities.DataIO.Update":
                                {
                                    activity.Type = ActivityTypes.UpdateContentActivity;

                                    content.Type = AssetTypes.Generic;
                                    content.Title = source.Param1;
                                    content.Channel = source.PropertyLabel;

                                    break;
                                }
                            default:
                                {
                                    continue;
                                }
                        }

                        result.Add(activity);
                    }
                    catch { }
                }

                return result;
            }
        }

        public List<string> RenderRecentActivities(int limit, int offset = 0)
        {
            return GetRecentActivities(limit, offset).Select(a => RenderActivity(a)).ToList();
        }

        private string RenderActivity(Utilities.ActivityLogItem item)
        {
            switch (item.Type)
            {
                case ActivityTypes.AddContentActivity:
                    {
                        return $@"
<div class='act-list-item'>
    <div class='act-list-item-header'>
        <div class='dot'><i class='fa-solid fa-file'></i></div>
    </div>
    <div class='act-list-item-body'>
        <div class='act-title'>
            <b><a href='{item.Content.URL}'>{item.Content.Title}</a></b>
        </div>
        <div class='act-body'>
            {item.Content.DatePublished:d MMMM yyyy} by <a href='{item.Profile.URL}'><i class='fa fa-user'></i> {item.Profile.Name}</a> in <span>'{item.Content.Channel}'</span>
        </div>
    </div>
</div>
";
                    }
                case ActivityTypes.CreateProfileActivity:
                    {
                        return $@"
<div class='act-list-item'>
    <div class='act-list-item-header'>
        <div class='dot color-blue'><i class='fa-solid fa-user-plus'></i></div>
    </div>
    <div class='act-list-item-body'>
        <div class='act-body'>
            {item.DateCreated:d MMMM yyyy} <a href='{item.Profile.URL}'><i class='fa fa-user'></i> {item.Profile.Name}</a> was added.
        </div>
    </div>
</div>
";
                    }
            }

            return "";
        }

        public int GetEditedCount()
        {
            string sql = "select count(*) from [Profile.Data].Person p " +
                            "join (select InternalID as PersonID from [RDF.Stage].InternalNodeMap i " +
                            "join (select distinct subject from [RDF.].Triple t " +
                            "join [RDF.].Node n on t.Predicate = n.NodeID and n.value in " +
                            "('http://profiles.catalyst.harvard.edu/ontology/prns#mainImage', 'http://vivoweb.org/ontology/core#awardOrHonor', " +
                            "'http://vivoweb.org/ontology/core#educationalTraining', 'http://vivoweb.org/ontology/core#freetextKeyword', 'http://vivoweb.org/ontology/core#overview')) t " +
                            "on i.NodeID = t.Subject and i.Class = 'http://xmlns.com/foaf/0.1/Person' union " +
                            "select distinct personid from [Profile.Data].[Publication.Person.Add] union " +
                            "select distinct personid from [Profile.Data].[Publication.Person.Exclude] as u) t " +
                            "on t.PersonID = p.PersonID " +
                            "and p.IsActive = 1";
                
            return GetCount(sql);
        }

        public int GetGrantCount()
        {
            return GetCount("SELECT [_NumberOfNodes] FROM [Ontology.].[ClassGroupClass] with (nolock) where ClassGroupURI='http://profiles.catalyst.harvard.edu/ontology/prns#ClassGroupResearch' and ClassURI='http://vivoweb.org/ontology/core#Grant'");
        }

        public int GetProfilesCount()
        {
            return GetCount("SELECT [_NumberOfNodes] FROM [Ontology.].[ClassGroupClass] with (nolock) where ClassGroupURI='http://profiles.catalyst.harvard.edu/ontology/prns#ClassGroupPeople' and ClassURI='http://xmlns.com/foaf/0.1/Person'");
        }

        public int GetPublicationsCount()
        {
            string sql = "SELECT [_NumberOfNodes] FROM [Ontology.].[ClassGroupClass] with (nolock) where classuri = 'http://purl.org/ontology/bibo/AcademicArticle' and classgroupuri = 'http://profiles.catalyst.harvard.edu/ontology/prns#ClassGroupResearch'";
            return GetCount(sql);
        }

        private int GetCount(string sql)
        {
            string key = "Statistics: " + sql;
            // store this in the cache. Use the sql as part of the key
            string cnt = (string)Framework.Utilities.Cache.FetchObject(key);

            if (String.IsNullOrEmpty(cnt))
            {
                using (SqlDataReader sqldr = GetSQLDataReader(sql, CommandType.Text, CommandBehavior.CloseConnection, null))
                {
                    if (sqldr.Read())
                    {
                        cnt = sqldr[0].ToString();
                        Framework.Utilities.Cache.Set(key, cnt);
                    }
                }
            }
            return String.IsNullOrEmpty(cnt) ? 0 : Convert.ToInt32(cnt);
        }

        private SqlDataReader GetQueryOutputReader(string sql)
        {

            string connstr = ConfigurationManager.ConnectionStrings["ProfilesDB"].ConnectionString;
            SqlConnection dbconnection = new SqlConnection(connstr);
            SqlCommand dbcommand = new SqlCommand(sql, dbconnection);
            SqlDataReader dbreader = null;
            dbconnection.Open();
            dbcommand.CommandTimeout = 5000;
            try
            {
                dbreader = dbcommand.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            { string dd = ex.Message; }
            return dbreader;
        }

        private string GetStringValue(string sql, string columnName)
        {
            string value = "";
            using (SqlDataReader reader = GetQueryOutputReader(sql))
            {
                if (reader.Read())
                {
                    value = reader[columnName].ToString();
                }
            }
            return value;
        }

    }
}
