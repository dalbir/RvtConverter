using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.ComApi;
using Autodesk.Navisworks.Api.DocumentParts;
using Autodesk.Navisworks.Api.Interop.ComApi;
using RvtConverter.Watcher;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComApi = Autodesk.Navisworks.Api.Interop.ComApi;
using ComApiBridge = Autodesk.Navisworks.Api.ComApi.ComApiBridge;
namespace PluginRvt2Obj.Navisworks
{
    public class NavReader
    {
        private Document doc_ = null;

        private ObjWriter writer_ = null;
        private int maxTriangleCount_ = 0;

        private int currentTriangleCount_ = 0;

        private int isDisplayName_ = 0;

        private int maxPropertyCount_ = 1;
        public NavDocument Document { get; set; }


        public List<NavModelGeometry> Geometries { get; set; }


        public NavModelNodePropDocument PropDocument { get; set; }
        public string DocumentName
        {
            get
            {
                return Path.GetFileNameWithoutExtension(this.doc_.FileName);
            }
        }
        public int MaxPropertyCount
        {
            get
            {
                return this.maxPropertyCount_;
            }
        }
        public NavReader(Document doc)
        {
            this.doc_ = doc;
            this.Geometries = new List<NavModelGeometry>();
        }
        private string RefineValue(DataProperty prop)
        {
            string text = prop.Value.ToString();
            bool isNamedConstant = prop.Value.IsNamedConstant;
            if (isNamedConstant)
            {
                NamedConstant namedConstant = prop.Value.ToNamedConstant();
                text = namedConstant.DisplayName;
            }
            else
            {
                int num = text.IndexOf(":");
                text = text.Substring(num + 1);
            }
            return text;
        }
        private void ReadProperties(ModelItem mi, NavModelNode node)
        {
            List<NavPropertyCategory> list = null;
            bool flag = this.maxPropertyCount_ != -1;
            if (flag)
            {
                list = new List<NavPropertyCategory>();
                this.PropDocument.NodeProperties.Add(node.Id, list);
            }
            else
            {
                list = node.PropertyCategories;
            }
            foreach (PropertyCategory propertyCategory in mi.PropertyCategories)
            {
                NavPropertyCategory navPropertyCategory = new NavPropertyCategory
                {
                    Name = ((this.isDisplayName_ == 0) ? propertyCategory.Name : propertyCategory.DisplayName)
                };
                list.Add(navPropertyCategory);
                foreach (DataProperty dataProperty in propertyCategory.Properties)
                {
                    NavProperty item = new NavProperty
                    {
                        Name = ((this.isDisplayName_ == 0) ? dataProperty.Name : dataProperty.DisplayName),
                        Value = this.RefineValue(dataProperty)
                    };
                    navPropertyCategory.Properties.Add(item);
                }
            }
            //this.writer_.WriteDB_Property(this.PropDocument, 1);
        }

        public void Execute()
        {
            //初始化属性
            string targetDirectory = Config.OutPutRoot;
            this.maxTriangleCount_ = int.Parse("100000");
            this.isDisplayName_ = int.Parse("0");
            this.maxPropertyCount_ = int.Parse("100000");
            this.writer_ = new ObjWriter(this, targetDirectory);
            this.Document = new NavDocument();
            this.PropDocument = new NavModelNodePropDocument();
            DocumentModels models = this.doc_.Models;
            //遍历所有Model
            foreach (Model model in models)
            {
                ModelItem rootItem = model.RootItem;
                bool isHidden = rootItem.IsHidden;
                //如果没有隐藏的话
                if (!isHidden)
                {
                    NavModelNode navModelNode = new NavModelNode();
                    this.Document.RootNodes.Add(navModelNode);
                    //访问模型项目
                    this.VisitModelItem(model, rootItem, navModelNode);
                }
            }
            this.WriteModel();
            this.writer_.WriteMtl();
            //this.writer_.WriteDB_Property(this.PropDocument, 2);
            //this.writer_.WriteDb_Tree();
            this.writer_ = null;
        }

        private void VisitModelItem(Model model, ModelItem mi, NavModelNode node)
        {
            node.DisplayName = ((mi.DisplayName.Length == 0) ? mi.ClassDisplayName : mi.DisplayName);
            this.ReadProperties(mi, node);
            //如果该项目有几何图形
            bool hasGeometry = mi.HasGeometry;
            if (hasGeometry)
            {
                //转换为COM选择集
                Autodesk.Navisworks.Api.Interop.ComApi.InwOaPath inwOaPath = ComApiBridge.ToInwOaPath(mi);
                //创建回调对象
                NavGeometryCallback navGeometryCallback = new NavGeometryCallback();
                navGeometryCallback.UnitScaleToMM = (float)UnitConversion.ScaleFactor(model.Units, Units.Millimeters);
                foreach (InwOaFragment3 inwOaFragment in inwOaPath.Fragments())
                {

                    Autodesk.Navisworks.Api.Interop.ComApi.InwOpState10 state = ComApiBridge.State;
                    Autodesk.Navisworks.Api.Interop.ComApi.InwOaPath3 inwOaPath2 = (Autodesk.Navisworks.Api.Interop.ComApi.InwOaPath3)inwOaFragment.path;
                    Autodesk.Navisworks.Api.Interop.ComApi.InwLTransform3f localToWorldMatrix = inwOaFragment.GetLocalToWorldMatrix();
                    Array array3 = (Array)(object)localToWorldMatrix.Matrix;


                    InwOaPath path = inwOaFragment.path;
                    ModelItem obj2 = ComApiBridge.ToModelItem(path);
                    bool flag = !mi.Equals(obj2);
                    if (!flag)
                    {
                        ComApi.InwLTransform3f3 localToWorld = (ComApi.InwLTransform3f3)(object)inwOaFragment.GetLocalToWorldMatrix();
                        //Array array_v1 = (Array)(object)localToWorld.Matrix;

                        InwLTransform3f3 fragmentTransform = (InwLTransform3f3)inwOaFragment.GetLocalToWorldMatrix();
                       // navGeometryCallback.localToWorldMatrix =  inwOaFragment.GetLocalToWorldMatrix();
                        navGeometryCallback.FragmentTransform = fragmentTransform;
                        inwOaFragment.GenerateSimplePrimitives(nwEVertexProperty.eNORMAL, navGeometryCallback);
                    }
                }
                Color originalColor = mi.Geometry.OriginalColor;
                NavColor color = new NavColor((float)originalColor.R, (float)originalColor.G, (float)originalColor.B, 1f - (float)mi.Geometry.ActiveTransparency);
                navGeometryCallback.Geometry.Color = color;
                this.Geometries.Add(navGeometryCallback.Geometry);
                this.currentTriangleCount_ += navGeometryCallback.Geometry.Triangles.Count / 3;
                node.GeometryId = string.Concat(navGeometryCallback.Geometry.Id);
                bool flag2 = this.currentTriangleCount_ >= this.maxTriangleCount_;
                if (flag2)
                {
                    this.WriteModel();
                }
            }
            foreach (ModelItem modelItem in mi.Children)
            {
                bool isHidden = modelItem.IsHidden;
                if (!isHidden)
                {
                    NavModelNode navModelNode = new NavModelNode();
                    node.Children.Add(navModelNode);
                    this.VisitModelItem(model, modelItem, navModelNode);
                }
            }
        }
        private void WriteModel()
        {
            this.writer_.WriteModel();
            this.Geometries.Clear();
            this.currentTriangleCount_ = 0;
        }
    }
}
