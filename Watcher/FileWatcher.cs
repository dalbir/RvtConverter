using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RvtConverter.Watcher
{
    public class FileWatcher
    {
        // 为保证线程安全，使用一个锁来保护_task的访问
        private static readonly object _locker = new object();

        // 任务队列
        private static Queue<MyFileInfo> _tasks = new Queue<MyFileInfo>();

        // 通过 _wh 给工作线程发信号
        private static EventWaitHandle _wh = new AutoResetEvent(false);

        static FileWatcher()
        {
            //是否要在载入时就遍历文件
            if (Config.ReLoad)
            {
                string[] array = Directory.GetFiles(Config.TempDir, "*.rvt");
                for (int i = 0; i < array.Length; i++)
                {
                    string file = array[i];
                    FileWatcher.EnqueueTask(file);
                }
            }
        }
        /// <summary>插入任务</summary>
        public static void EnqueueTask(string filename)
        {
            lock (_locker)
            {
                MyFileInfo fileInfo = new MyFileInfo(filename);
                _tasks.Enqueue(fileInfo);  // 向队列中插入任务
            }

            _wh.Set();  // 给工作线程发信号
        }

        /// <summary>
        /// 开始监听指定文件夹中的事件
        /// </summary>
        public static void Strat()
        {
            FileSystemWatcher watcher = new FileSystemWatcher() { Path = Config.TempDir, Filter = "*.rvt" };
            //watcher.Changed += new FileSystemEventHandler(OnProcess);
            watcher.Created += new FileSystemEventHandler(OnProcess);
            //  watcher.Deleted += new FileSystemEventHandler(OnProcess);
            watcher.EnableRaisingEvents = true;
            watcher.NotifyFilter = NotifyFilters.Attributes | NotifyFilters.CreationTime | NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastAccess
                                       | NotifyFilters.LastWrite | NotifyFilters.Security | NotifyFilters.Size;
            watcher.IncludeSubdirectories = true;
        }
        /// <summary>执行工作</summary>
        public static void Work()
        {
            while (true)
            {
                MyFileInfo fileinfo = null;
                lock (_locker)
                {
                    if (_tasks.Count > 0)
                    {
                        fileinfo = _tasks.Dequeue(); // 有任务时，出列任务

                        if (fileinfo == null)  // 退出机制：当遇见一个null任务时，代表任务结束
                            return;
                    }
                }

                if (fileinfo != null)
                {
                    Log.logger.Info("Loading " + fileinfo.FullPath);
                    Convert.Invoke(fileinfo);
                }
                else
                {
                    _wh.WaitOne();   // 没有任务了，等待信号
                    Log.logger.Info("Waiting .....");
                }
            }
        }

        /// <summary>结束释放</summary>
        public static void Dispose()
        {
            EnqueueTask(null);      // 插入一个Null任务，通知工作线程退出
            //_worker.Join();         // 等待工作线程完成
            _wh.Close();            // 释放资源

        }

        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            Console.WriteLine("文件改变事件处理逻辑{0}  {1}  {2}", e.ChangeType, e.FullPath, e.Name);
            Log.logger.InfoFormat("Add {0}", e.Name);
            if (Path.GetExtension(e.FullPath) == "rvt")
                EnqueueTask(e.FullPath);
        }

        private static void OnCreated(object source, FileSystemEventArgs e)
        {
            Console.WriteLine("文件新建事件处理逻辑 {0}  {1}  {2}", e.ChangeType, e.FullPath, e.Name);
            // 生产者将数据插入队里中，并给工作线程发信号
            if (Path.GetExtension(e.FullPath) == "rvt")
                EnqueueTask(e.FullPath);
        }

        private static void OnDeleted(object source, FileSystemEventArgs e)
        {
            Console.WriteLine("文件删除事件处理逻辑{0}  {1}   {2}", e.ChangeType, e.FullPath, e.Name);
        }

        private static void OnProcess(object source, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Created)
            {
                OnCreated(source, e);
            }
            else if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                OnChanged(source, e);
            }
            else if (e.ChangeType == WatcherChangeTypes.Deleted)
            {
                OnDeleted(source, e);
            }
        }
    }
}
