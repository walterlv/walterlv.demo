using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Walterlv.FastKeyValue
{
    public sealed partial class FastKeyValue
    {
        private readonly Dictionary<string, string> _dictionary = new Dictionary<string, string>();

        public async Task LoadAsync(string fileName)
        {
            using (var stream = File.OpenRead(fileName))
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                var fast = new FastKeyValue();
                var result = fast.LoadAsync(reader);
            }
        }

        public async Task<Dictionary<string, string>> LoadAsync(StreamReader reader)
        {
            const int bufferSize = 1024;
            var buffer = new char[bufferSize];
            var totalReadCount = 0;
            int readCount;

            do
            {
                readCount = reader.Read(buffer, totalReadCount, bufferSize);
                totalReadCount += readCount;

                for (var i = 0; i < readCount; i++)
                {
                    // 外层 while 和 内层 for 合并起来，就是在依次阅读文件的每一个字符。
                    var current = buffer[i];
                    
                }
            } while (readCount == bufferSize);
        }
    }
}
