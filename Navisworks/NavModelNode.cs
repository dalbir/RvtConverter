using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace PluginRvt2Obj.Navisworks
{
    [DataContract]
    public class NavModelNode
    {

        public NavModelNode()
        {
            this.Id = NavModelNode.s_id;
            NavModelNode.s_id++;
            this.GeometryId = "-1";
            this.Children = new List<NavModelNode>();
            this.PropertyCategories = new List<NavPropertyCategory>();
        }
        [DataMember]
        public int Id { get; set; }

 
        [DataMember]
        public string DisplayName { get; set; }

        [DataMember]
        public string GeometryId { get; set; }

        [DataMember]
        public List<NavModelNode> Children { get; set; }


        [DataMember]
        public List<NavPropertyCategory> PropertyCategories { get; set; }

        // Token: 0x0400000E RID: 14
        private static int s_id = 1;
    }
}