using System;

namespace PluginRvt2Obj.Navisworks
{
    // Token: 0x0200000D RID: 13
    public class NavVector3d
    {
        // Token: 0x06000059 RID: 89 RVA: 0x0000314C File Offset: 0x0000134C
        public NavVector3d()
        {
            this.X = 0f;
            this.Y = 0f;
            this.Z = 0f;
        }

        // Token: 0x0600005A RID: 90 RVA: 0x0000317A File Offset: 0x0000137A
        public NavVector3d(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        // Token: 0x17000020 RID: 32
        // (get) Token: 0x0600005B RID: 91 RVA: 0x0000319C File Offset: 0x0000139C
        // (set) Token: 0x0600005C RID: 92 RVA: 0x000031A4 File Offset: 0x000013A4
        public float X { get; set; }

        // Token: 0x17000021 RID: 33
        // (get) Token: 0x0600005D RID: 93 RVA: 0x000031AD File Offset: 0x000013AD
        // (set) Token: 0x0600005E RID: 94 RVA: 0x000031B5 File Offset: 0x000013B5
        public float Y { get; set; }

        // Token: 0x17000022 RID: 34
        // (get) Token: 0x0600005F RID: 95 RVA: 0x000031BE File Offset: 0x000013BE
        // (set) Token: 0x06000060 RID: 96 RVA: 0x000031C6 File Offset: 0x000013C6
        public float Z { get; set; }
    }
}
