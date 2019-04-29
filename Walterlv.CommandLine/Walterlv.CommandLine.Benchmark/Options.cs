using System.ComponentModel;
using CommandLine;

namespace Walterlv.CommandLine.Benchmark
{
    /// <summary>
    /// 表示此程序在被启动的时候使用的参数信息。此类型是不可变类型，所有实例都是线程安全的。
    /// </summary>
    public class ComparedOptions
    {
        /// <summary>
        /// 表示通过打开的文件路径。此属性可能为 null，但绝不会是空字符串或空白字符串。
        /// </summary>
        [Value(0), Option('f', "file")]
        public string FilePath { get; set; }

        /// <summary>
        /// 当此参数值为 true 时，表示此进程是从 Cloud 端启动的 Shell 进程。此属性默认值是 false。
        /// </summary>
        [Option("cloud"), DefaultValue(false)]
        public bool IsFromCloud { get; set; }

        /// <summary>
        /// 表示 Shell 端启动的模式。此属性可能为 null，但绝不会是空字符串或空白字符串。
        /// </summary>
        [Option('m', "mode")]
        public string StartupMode { get; set; }

        /// <summary>
        /// 表示当前是否是静默方式启动，通常由 Shell 启动 Cloud 时使用。此属性默认值是 false。
        /// </summary>
        [Option('s', "silence"), DefaultValue(false)]
        public bool IsSilence { get; set; }

        /// <summary>
        /// 表示当前启动时需要针对 IWB 进行处理。此属性默认值是 false。
        /// </summary>
        [Option("iwb"), DefaultValue(false)]
        public bool IsIwb { get; set; }

        /// <summary>
        /// 表示当前窗口启动时应该安放的位置。此属性可能为 null，但绝不会是空字符串或空白字符串。
        /// </summary>
        [Option('p', "placement")]
        public string Placement { get; set; }

        /// <summary>
        /// 表示一个启动会话 Id，用于在多个进程间同步一些信息。此属性可能为 null，但绝不会是空字符串或空白字符串。
        /// </summary>
        [Option("startup-session")]
        public string StartupSession { get; set; }
    }
}
