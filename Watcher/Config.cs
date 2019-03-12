using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;

namespace RvtConverter.Watcher
{
    public class Config
    {
        private static string _outPutDir;
        private static string _tempDir;

        public static string OutPutRoot
        {
            get
            {
                if (_outPutDir == null)
                {
                    _outPutDir = @"D:\test\20190311\out";
                    _outPutDir = Verify(_outPutDir);

                    Log.logger.InfoFormat("Current OutPutRoot is :{0}", _outPutDir);
                }
                return _outPutDir;
            }
        }

        public static bool ReLoad { get => true; }

        public static string TempDir
        {
            get
            {
                if (_tempDir == null)
                {
                    _tempDir = @"D:\test\20190311\in";
                    _tempDir = Verify(_tempDir);

                    Log.logger.InfoFormat("Current TempDir is :{0}", _tempDir);
                }
                return _tempDir;
            }
        }
        private static string Verify(string path)
        {
            if (path.StartsWith("/"))
            {
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path.Substring(1));
            }
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }
    }
}
