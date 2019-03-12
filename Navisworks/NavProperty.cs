using System;
using System.Runtime.Serialization;

namespace PluginRvt2Obj.Navisworks
{
    // Token: 0x02000008 RID: 8
    [DataContract]
    public class NavProperty
    {

        [DataMember]
        public string Name { get; set; }


        [DataMember]
        public string Value { get; set; }
    }
}
