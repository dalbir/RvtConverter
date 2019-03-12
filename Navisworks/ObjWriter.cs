using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PluginRvt2Obj.Navisworks
{
    public class ObjWriter
    {
        public ObjWriter(NavReader reader, string targetDirectory)
        {
            this.reader_ = reader;
            this.targetDirectory_ = targetDirectory;
            this.colors_ = new List<NavColor>();
        }
        private NavReader reader_ = null;

        private string targetDirectory_ = null;

        private int docId_ = 0;

        private int propDocId_ = 0;

        // Token: 0x04000031 RID: 49
        private List<NavColor> colors_ = null;
        public void WriteModel()
        {
            bool flag = this.reader_.Geometries.Count == 0;
            if (!flag)
            {
                this.docId_++;
                string str = this.reader_.DocumentName + "-" + this.docId_;
                string text = str + ".obj";
                string str2 = this.reader_.DocumentName + ".mtl";
                string path = Path.Combine(this.targetDirectory_, text);
                FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);
                StreamWriter streamWriter = new StreamWriter(fileStream);
                streamWriter.WriteLine("mtllib " + str2);
                streamWriter.WriteLine("");
                int num = 0;
                foreach (NavModelGeometry geometry in this.reader_.Geometries)
                {
                    streamWriter.WriteLine("g " + geometry.Id);
                    int num2 = this.colors_.FindIndex((NavColor item) => item.Equals(geometry.Color));
                    bool flag2 = num2 == -1;
                    if (flag2)
                    {
                        this.colors_.Add(geometry.Color);
                    }
                    streamWriter.WriteLine("usemtl " + this.colors_.FindIndex((NavColor item) => item.Equals(geometry.Color)));
                    streamWriter.Write(this.GetVerticesString(geometry.Triangles));
                    streamWriter.Write(this.GetFaceString(num, geometry.Triangles.Count / 3));
                    num += geometry.Triangles.Count;
                    streamWriter.WriteLine("");
                }
            }
        }
        private string GetVerticesString(List<NavVertex> vertices)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (NavVertex navVertex in vertices)
            {
                stringBuilder.AppendLine(string.Concat(new object[]
                {
                    "v ",
                    navVertex.Coord.X,
                    " ",
                    navVertex.Coord.Y,
                    " ",
                    navVertex.Coord.Z
                }));
            }
            return stringBuilder.ToString();
        }
        private string GetNormalsString(List<NavVertex> vertices)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (NavVertex navVertex in vertices)
            {
                stringBuilder.AppendLine(string.Concat(new object[]
                {
                    "vn ",
                    navVertex.Normal.X,
                    " ",
                    navVertex.Normal.Y,
                    " ",
                    navVertex.Normal.Z
                }));
            }
            return stringBuilder.ToString();
        }
        private string GetFaceString(int startIndex, int triangleCount)
        {
            bool flag = triangleCount <= 0;
            string result;
            if (flag)
            {
                result = "";
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < triangleCount; i++)
                {
                    stringBuilder.AppendLine(string.Concat(new object[]
                    {
                        "f ",
                        3 * i + 1 + startIndex,
                        " ",
                        3 * i + 2 + startIndex,
                        " ",
                        3 * i + 3 + startIndex
                    }));
                }
                result = stringBuilder.ToString();
            }
            return result;
        }
        private string GetColorString(NavColor color)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("newmtl " + this.colors_.IndexOf(color));
            stringBuilder.AppendLine(string.Concat(new object[]
            {
                "Kd ",
                color.R,
                " ",
                color.G,
                " ",
                color.B
            }));
            stringBuilder.AppendLine("d " + color.D);
            return stringBuilder.ToString();
        }
        public void WriteMtl()
        {
            string path = this.reader_.DocumentName + ".mtl";
            string path2 = Path.Combine(this.targetDirectory_, path);
            FileStream fileStream = new FileStream(path2, FileMode.Create, FileAccess.ReadWrite);
            StreamWriter streamWriter = new StreamWriter(fileStream);
            foreach (NavColor color in this.colors_)
            {
                streamWriter.Write(this.GetColorString(color));
                streamWriter.WriteLine("");
            }
            streamWriter.Flush();
            streamWriter.Close();
            fileStream.Close();
        }
    }
}