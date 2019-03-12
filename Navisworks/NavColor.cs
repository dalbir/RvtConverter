using System;

namespace PluginRvt2Obj.Navisworks
{
    // Token: 0x02000002 RID: 2
    public class NavColor
    {
        // Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
        public NavColor(float r, float g, float b, float d = 1f)
        {
            this.R = r;
            this.G = g;
            this.B = b;
            this.D = d;
        }

        // Token: 0x17000001 RID: 1
        // (get) Token: 0x06000002 RID: 2 RVA: 0x0000207B File Offset: 0x0000027B
        // (set) Token: 0x06000003 RID: 3 RVA: 0x00002083 File Offset: 0x00000283
        public float R { get; set; }

        // Token: 0x17000002 RID: 2
        // (get) Token: 0x06000004 RID: 4 RVA: 0x0000208C File Offset: 0x0000028C
        // (set) Token: 0x06000005 RID: 5 RVA: 0x00002094 File Offset: 0x00000294
        public float G { get; set; }

        // Token: 0x17000003 RID: 3
        // (get) Token: 0x06000006 RID: 6 RVA: 0x0000209D File Offset: 0x0000029D
        // (set) Token: 0x06000007 RID: 7 RVA: 0x000020A5 File Offset: 0x000002A5
        public float B { get; set; }

        // Token: 0x17000004 RID: 4
        // (get) Token: 0x06000008 RID: 8 RVA: 0x000020AE File Offset: 0x000002AE
        // (set) Token: 0x06000009 RID: 9 RVA: 0x000020B6 File Offset: 0x000002B6
        public float D { get; set; }

        // Token: 0x0600000A RID: 10 RVA: 0x000020C0 File Offset: 0x000002C0
        public bool Equals(NavColor target)
        {
            return this.R == target.R && this.G == target.G && this.B == target.B && this.D == target.D;
        }
    }
}
