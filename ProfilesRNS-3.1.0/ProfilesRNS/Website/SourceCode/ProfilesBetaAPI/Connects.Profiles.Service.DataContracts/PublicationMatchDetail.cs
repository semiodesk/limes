﻿/*  
 
    Copyright (c) 2008-2010 by the President and Fellows of Harvard College. All rights reserved.  
    Profiles Research Networking Software was developed under the supervision of Griffin M Weber, MD, PhD.,
    and Harvard Catalyst: The Harvard Clinical and Translational Science Center, with support from the 
    National Center for Research Resources and Harvard University.


    Code licensed under a BSD License. 
    For details, see: LICENSE.txt 
  
*/
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Connects.Profiles.Service.DataContracts
{
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute()]
    [DataContract(Name = "PublicationMatchDetail")]
    public partial class PublicationMatchDetail
    {
        private PublicationPhraseDetailList phraseMeasurementsListField;
        private string searchPhraseField;

        [DataMember(IsRequired = false, Name = "PublicationPhraseDetailList", Order = 1)]
        public PublicationPhraseDetailList PublicationPhraseDetailList
        {
            get
            {
                return this.phraseMeasurementsListField;
            }
            set
            {
                this.phraseMeasurementsListField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [DataMember(IsRequired = false, Name = "SearchPhrase", Order = 2, EmitDefaultValue = false)]
        public string SearchPhrase
        {
            get
            {
                return this.searchPhraseField;
            }
            set
            {
                this.searchPhraseField = value;
            }
        }

    }


    [System.Xml.Serialization.XmlTypeAttribute()]
    [CollectionDataContract(Name = "PublicationMatchDetailList")]
    public class PublicationMatchDetailList : List<PublicationMatchDetail>
    {
    }

}
