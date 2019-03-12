using Autodesk.Navisworks.Api;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace PluginRvt2Obj.Navisworks
{
    public class NavDocument
    {
        public NavDocument()
        {
            this.GeometryFiles = new List<string>();
            this.RootNodes = new List<NavModelNode>();
        }
        [DataMember]
        public List<string> GeometryFiles { get; set; }
        [DataMember]
        public List<NavModelNode> RootNodes { get; set; }
    }
}