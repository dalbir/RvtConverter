using System.IO;
using System.Text;

namespace RvtConverter.Watcher
{
    public class MyFileInfo
    {
        public MyFileInfo(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                return;
            }
            this.FullPath = filename;
            this.OutDir = GetOutName();
        }

        public string Dir
        {
            get
            {
                if (string.IsNullOrEmpty(FullPath))
                {
                    return null;
                }
                return Path.GetDirectoryName(FullPath);
            }
        }
        public string name { get=>Path.GetFileName(FullPath); }
        public string FullPath { get; private set; }
        public string MD5 { get => GetMD5HashFromFile(); }
        public string OutDir { get; private set; }
        private string GetMD5HashFromFile()
        {

            try
            {
                FileStream file = new FileStream(FullPath, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));


                }
                return sb.ToString();

            }
            catch (System.Exception ex)
            {

                Log.logger.Error("错误信息：" + ex);
                return null;

            }

        }

        private string GetOutName()
        {
            var result = "";
            var filename = Path.GetFileName(FullPath);
            result = Path.Combine(Config.OutPutRoot, MD5, filename);
            var dir = Path.GetDirectoryName(result);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            return result;
        }
    }
}