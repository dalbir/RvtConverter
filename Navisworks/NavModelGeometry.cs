using System.Collections.Generic;

namespace PluginRvt2Obj.Navisworks
{
    public class NavModelGeometry
    {
        public NavColor Color { get; set; }
        public int Id { get; set; }
        private static int s_id = 1;
        private List<NavVertex> triangles_ = null;
        private List<NavVertex> lines_;
        private List<NavVertex> vertices_;

        public List<NavVertex> Triangles
        {
            get
            {
                return this.triangles_;
            }
        }
        public List<NavVertex> Lines
        {
            get
            {
                return this.lines_;
            }
        }
        public List<NavVertex> Vertices
        {
            get
            {
                return this.vertices_;
            }
        }
        public NavModelGeometry()
        {
            this.vertices_ = new List<NavVertex>();
            this.lines_ = new List<NavVertex>();
            this.triangles_ = new List<NavVertex>();
            this.Id = NavModelGeometry.s_id;
            NavModelGeometry.s_id++;
        }
        public void Add(NavVertex v)
        {
            this.vertices_.Add(v);
        }
        public void Add(NavVertex v0, NavVertex v1)
        {
            this.lines_.Add(v0);
            this.lines_.Add(v1);
        }
        public void Add(NavVertex v0, NavVertex v1, NavVertex v2)
        {
            this.triangles_.Add(v0);
            this.triangles_.Add(v1);
            this.triangles_.Add(v2);
        }
    }
}