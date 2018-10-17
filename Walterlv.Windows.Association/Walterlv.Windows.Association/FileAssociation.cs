using System;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Win32;

namespace Walterlv.Windows.Association
{
    public class FileAssociation
    {
        private readonly string _progId;

        public string FileTypeDescription { get; set; }

        public FileAssociation(string progId)
        {
            _progId = progId ?? throw new ArgumentNullException(nameof(progId));
        }

        public void Create(bool forAllUsers = false)
        {
            // 准备文件关联所需要的键值。
            var description = FileTypeDescription ?? _progId;
            var openCommand = $@"""{GetCurrentExecutablePath()}"" ""%1""";

            // 找到注册表关联的根键。
            var root = forAllUsers ? Registry.LocalMachine : Registry.CurrentUser;
            var classesKey = root.OpenSubKey(@"Software\Classes", true);

            // 找到应用程序 ProgID 键。
            var progIdKey = classesKey.CreateSubKey(_progId);
            progIdKey.SetValue(null, description);

            // 找到打开命令键。
            var openCommandKey = progIdKey.CreateSubKey(@"Shell\Open\Command");
            openCommandKey.SetValue(null, openCommand);
        }

        private static string GetCurrentExecutablePath()
        {
            var processPath = Process.GetCurrentProcess().MainModule.FileName;
            return processPath;
        }
    }
}