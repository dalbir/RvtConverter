using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PluginRvt2Obj.Navisworks
{
 
    [DataContract]
    public class NavModelNodePropDocument
    {
       
        public NavModelNodePropDocument()
        {
            this.NodeProperties = new Dictionary<int, List<NavPropertyCategory>>();
        }


        [DataMember]
        public Dictionary<int, List<NavPropertyCategory>> NodeProperties { get; set; }
    }
}
