using System;
using Autodesk.Navisworks.Api.Interop.ComApi;
namespace PluginRvt2Obj.Navisworks
{
    internal class NavGeometryCallback : InwSimplePrimitivesCB
    {
        public NavGeometryCallback()
        {
            this.geometry_ = new NavModelGeometry();
        }
        public NavModelGeometry Geometry
        {
            get
            {
                return this.geometry_;
            }
        }
        public InwLTransform3f3 FragmentTransform { get; set; }
       // public InwLTransform3f2 FragmentTransform2 { get; set; }
   //     public InwLTransform3f localToWorldMatrix { get; set; }
        public float UnitScaleToMM { get; set; }
        public void Line(InwSimpleVertex v0, InwSimpleVertex v1)
        {
            this.geometry_.Add(this.ConvertFrom(v0), this.ConvertFrom(v1));
        }
        public void Point(InwSimpleVertex v)
        {
            this.geometry_.Add(this.ConvertFrom(v));
        }
        public void SnapPoint(InwSimpleVertex v)
        {
            this.geometry_.Add(this.ConvertFrom(v));
        }
        public void Triangle(InwSimpleVertex v0, InwSimpleVertex v1, InwSimpleVertex v2)
        {
            this.geometry_.Add(this.ConvertFrom(v0), this.ConvertFrom(v1), this.ConvertFrom(v2));
        }

        private NavVertex ConvertFrom(InwSimpleVertex v)
        {
           
            Array array = (Array)(object)this.FragmentTransform.Matrix;
           
            NavVector3d navVector3d = new NavVector3d(0,2,1);
            NavVector3d normal = new NavVector3d((float)1, (float)2, (float)3);
            return new NavVertex(navVector3d, normal);
        }

        private NavModelGeometry geometry_ = null;
    }
}