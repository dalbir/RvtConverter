using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PluginRvt2Obj.Navisworks
{
    // Token: 0x02000009 RID: 9
    [DataContract]
    public class NavPropertyCategory
    {
        // Token: 0x06000035 RID: 53 RVA: 0x000027F8 File Offset: 0x000009F8
        public NavPropertyCategory()
        {
            this.Properties = new List<NavProperty>();
        }

        // Token: 0x17000014 RID: 20
        // (get) Token: 0x06000036 RID: 54 RVA: 0x0000280E File Offset: 0x00000A0E
        // (set) Token: 0x06000037 RID: 55 RVA: 0x00002816 File Offset: 0x00000A16
        [DataMember]
        public string Name { get; set; }

        // Token: 0x17000015 RID: 21
        // (get) Token: 0x06000038 RID: 56 RVA: 0x0000281F File Offset: 0x00000A1F
        // (set) Token: 0x06000039 RID: 57 RVA: 0x00002827 File Offset: 0x00000A27
        [DataMember]
        public List<NavProperty> Properties { get; set; }
    }
}
