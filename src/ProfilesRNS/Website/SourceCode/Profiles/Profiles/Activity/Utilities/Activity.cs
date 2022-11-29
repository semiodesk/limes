using System;
using System.Collections.Generic;

namespace Profiles.Activity.Utilities
{
    public class Activity : IComparable<Activity>
    {
        public Int64 Id { get; set; }

        /// <summary>
        /// Who created the activity
        /// </summary>
        public string CreatedById { get; set; }

        public Profile Profile { get; set; }

        /// <summary>
        /// Simple TimeStamp 
        /// </summary>
        public DateTime CreatedDT { get; set; }

        public string Date
        {
            get { return String.Format("{0:MMMM d, yyyy}", CreatedDT); }
            set { }
        }

        public string Message { get; set; }

        public string LinkUrl { get; set; }

        public string Title { get; set; }

        public int CompareTo(Activity o)
        {
            return o.Id.CompareTo(this.Id);
        }

    }

    public class Profile
    {
        public string Name { get; set; }

        public long NodeID { get; set; }

        public int PersonId { get; set; }

        public string URL { get; set; }

        public string Thumbnail { get; set; }
    }

    public class ReverseComparer : IComparer<Int64>
    {
        public int Compare(Int64 x, Int64 y)
        {
            return y.CompareTo(x);
        }
    }

    public static class prns
    {
        public const string NamespaceUri = "http://profiles.catalyst.harvard.edu/ontology/prns#";
    }

    public static class ActivityTypes
    {
        public const string CreateProfileActivity = prns.NamespaceUri + "CreateProfileActivity";

        public const string UpdateProfileActivity = prns.NamespaceUri + "UpdateProfileActivity";

        public const string DeleteProfileActivity = prns.NamespaceUri + "DeleteProfileActivity";

        public const string AddContentActivity = prns.NamespaceUri + "AddContentActivity";

        public const string UpdateContentActivity = prns.NamespaceUri + "UpdateContentActivity";

        public const string RemoveContentActivity = prns.NamespaceUri + "RemoveContentActivity";
    }
    public static class AssetTypes
    {
        public const string Generic = prns.NamespaceUri + "Generic";

        public const string Funding = prns.NamespaceUri + "Funding";

        public const string Publication = prns.NamespaceUri + "Publication";
    }

    public class ActivityLogItem
    {
        public long Id { get; set; }

        public string Type { get; set; }

        public DateTime DateCreated { get; set; }

        public ActivityLogItemSource Source { get; set; }

        public Profile Profile { get; set; }

        public Asset Content { get; set; }

    }

    public class ActivityLogItemSource
    {
        public string Method { get; set; }

        public string Param1 { get; set; }

        public string Param2 { get; set; }

        public string Property { get; set; }

        public string PropertyLabel { get; set; }
    }

    public class Asset
    {
        /// <summary>
        /// Gets or sets the content type (i.e. publication, funding or generic)
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the date the content was published.
        /// </summary>
        public DateTime DatePublished { get; set; }

        /// <summary>
        /// Gets or sets the title of the content.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the publication journal or Profiles RNS content section.
        /// </summary>
        public string Channel { get; set; }

        /// <summary>
        /// Gets or sets the publication journal or Profiles RNS content section.
        /// </summary>
        public string URL { get; set; }
    }

    public class ConceptStats
    {
        public string Id { get; set; }

        public string Label { get; set; }

        public string GroupLabel { get; set; }

        public int PublicationsCount { get; set; }

        public int FontSize { get; set; }
    }
}
