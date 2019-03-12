using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Navisworks.Api;
using Application = Autodesk.Navisworks.Api.Application;
using ComApi = Autodesk.Navisworks.Api.Interop.ComApi;
using Autodesk.Navisworks.Api.ComApi;
using Autodesk.Navisworks.Api.Interop;
using Autodesk.Navisworks.Api.Interop.ComApi;
using System.Windows.Media.Media3D.Converters;
using System.Windows.Media.Media3D;
using PluginRvt2Obj.Navisworks;
namespace RvtConverter.Watcher
{
    public class Convert
    {
        public static bool Check()
        {
            return Application.ActiveDocument.Models.Count != 0;
        }
        public static void Invoke(MyFileInfo myFileInfo)
        {

            Autodesk.Navisworks.Api.View activeView = Autodesk.Navisworks.Api.Application.ActiveDocument.ActiveView;
            var doc = Autodesk.Navisworks.Api.Application.ActiveDocument;

           
            if (doc.TryOpenFile(myFileInfo.FullPath))
            {
                NavReader navReader = new NavReader(doc);
                navReader.Execute();

            }
        }
        public static void Invoke()
        {
            if (!Check())
                return;
            var modelItemEnumerableCollection = Application.ActiveDocument.Models.RootItemDescendantsAndSelf;
            foreach (ModelItem item in modelItemEnumerableCollection)
            {
                if (item.HasGeometry)
                {
                    //转换为COM对象
                    Autodesk.Navisworks.Api.Interop.ComApi.InwOaPath3 inwOaPath = (InwOaPath3)ComApiBridge.ToInwOaPath(item);
                    foreach (Autodesk.Navisworks.Api.Interop.ComApi.InwOaFragment3 inwOaFragment in inwOaPath.Fragments())
                    {
                        Autodesk.Navisworks.Api.Interop.ComApi.InwOpState10 state = ComApiBridge.State;
                        Autodesk.Navisworks.Api.Interop.ComApi.InwOaPath3 inwOaPath2 = (Autodesk.Navisworks.Api.Interop.ComApi.InwOaPath3)inwOaFragment.path;
                        Autodesk.Navisworks.Api.Interop.ComApi.InwLTransform3f localToWorldMatrix = inwOaFragment.GetLocalToWorldMatrix();
                        //矩阵
                        Array array3 = (Array)(object)localToWorldMatrix.Matrix;
                        //列向量
                        Matrix3D transformation = new Matrix3D((double)array3.GetValue(1), (double)array3.GetValue(2), (double)array3.GetValue(3), (double)array3.GetValue(4), (double)array3.GetValue(5), (double)array3.GetValue(6), (double)array3.GetValue(7), (double)array3.GetValue(8), (double)array3.GetValue(9), (double)array3.GetValue(10), (double)array3.GetValue(11), (double)array3.GetValue(12), (double)array3.GetValue(13), (double)array3.GetValue(14), (double)array3.GetValue(15), (double)array3.GetValue(16));
       
                    }
                }
            }

        }
    }
}
