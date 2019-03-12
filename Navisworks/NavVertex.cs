using System;

namespace PluginRvt2Obj.Navisworks
{
    // Token: 0x02000010 RID: 16
    public class NavVertex
    {
        // Token: 0x06000066 RID: 102 RVA: 0x0000328D File Offset: 0x0000148D
        public NavVertex(NavVector3d coord, NavVector3d normal = null)
        {
            this.Coord = coord;
            this.Normal = normal;
        }

        // Token: 0x17000023 RID: 35
        // (get) Token: 0x06000067 RID: 103 RVA: 0x000032A7 File Offset: 0x000014A7
        // (set) Token: 0x06000068 RID: 104 RVA: 0x000032AF File Offset: 0x000014AF
        public NavVector3d Coord { get; set; }

        // Token: 0x17000024 RID: 36
        // (get) Token: 0x06000069 RID: 105 RVA: 0x000032B8 File Offset: 0x000014B8
        // (set) Token: 0x0600006A RID: 106 RVA: 0x000032C0 File Offset: 0x000014C0
        public NavVector3d Normal { get; set; }
    }
}
