using Walterlv.Framework;

namespace Walterlv
{
    internal class Option
    {
        /// <summary>
        /// 表示通过打开的文件路径。
        /// </summary>
        [Value(0), Option('f', "file")]
        public string FilePath { get; }

        /// <summary>
        /// 当此参数值为 true 时，表示此进程是从 Cloud 端启动的 Shell 进程。
        /// </summary>
        [Option("cloud")]
        public bool IsFromCloud { get; }

        /// <summary>
        /// 表示 Shell 端启动的模式。
        /// </summary>
        [Option('m', "mode")]
        public StartupMode StartupMode { get; }

        /// <summary>
        /// 表示当前是否是静默方式启动，通常由 Shell 启动 Cloud 时使用。
        /// </summary>
        [Option('s', "silence")]
        public bool IsSilence { get; }

        /// <summary>
        /// 表示当前启动时需要针对 IWB 进行处理。
        /// </summary>
        [Option("iwb")]
        public bool IsIwb { get; }

        public Option(string filePath, bool isFromCloud, StartupMode startupMode, bool isSilence, bool isIwb)
        {
            FilePath = filePath;
            IsFromCloud = isFromCloud;
            StartupMode = startupMode;
            IsSilence = isSilence;
            IsIwb = isIwb;
        }
    }
}