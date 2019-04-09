namespace Walterlv.FastKeyValue
{
    public sealed partial class FastKeyValue
    {
        /// <summary>
        /// 表示当前解析配置文件的阅读区状态。
        /// </summary>
        private enum ReadingState
        {
            /// <summary>
            /// 表示当前状态开始。
            /// </summary>
            Start = 0,

            /// <summary>
            /// 表示当前处于分隔区。
            /// </summary>
            Splitter,

            /// <summary>
            /// 表示 Splitter 和 Key 的临界区，可能回到 Splitter，可能进入 Key。
            /// </summary>
            CriticalKey,

            /// <summary>
            /// 表示当前处于 Key 阅读区。
            /// </summary>
            Key,

            /// <summary>
            /// 表示 Key 和 Value 的临界区，可能进入 Value，可能发生错误然后进入 Splitter。
            /// </summary>
            CriticalValue,

            /// <summary>
            /// 表示当前处于 Value 阅读区。
            /// </summary>
            Value,

            /// <summary>
            /// 表示当前处于 Value 和 Splitter 的临界区，可能回到 Value，可能进入 Splitter。
            /// </summary>
            CriticalSplitter,
        }
    }
}
