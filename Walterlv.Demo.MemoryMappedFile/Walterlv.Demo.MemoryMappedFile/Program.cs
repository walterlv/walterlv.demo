using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;

namespace Walterlv.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;

            if (args.Length > 0)
            {
                Debugger.Launch();
                const int MMF_MAX_SIZE = 1024;  // allocated memory for this memory mapped file (bytes)
                const int MMF_VIEW_SIZE = 1024; // how many bytes of the allocated memory can this process access

                // creates the memory mapped file
                MemoryMappedFile mmf = MemoryMappedFile.OpenExisting("mmf1");
                MemoryMappedViewStream mmvStream = mmf.CreateViewStream(0, MMF_VIEW_SIZE); // stream used to read data

                BinaryFormatter formatter = new BinaryFormatter();

                // needed for deserialization
                byte[] buffer = new byte[MMF_VIEW_SIZE];

                Message message1;

                // reads every second what's in the shared memory
                while (mmvStream.CanRead)
                {
                    // stores everything into this buffer
                    mmvStream.Read(buffer, 0, MMF_VIEW_SIZE);

                    // deserializes the buffer & prints the message
                    message1 = (Message)formatter.Deserialize(new MemoryStream(buffer));
                    Console.WriteLine(message1.title + "\n" + message1.content + "\n");

                    System.Threading.Thread.Sleep(1000);
                }
                Console.ReadLine();
            }
            else
            {
                const int MMF_MAX_SIZE = 1024;  // allocated memory for this memory mapped file (bytes)
                const int MMF_VIEW_SIZE = 1024; // how many bytes of the allocated memory can this process access

                // creates the memory mapped file which allows 'Reading' and 'Writing'
                MemoryMappedFile mmf = MemoryMappedFile.CreateOrOpen("mmf1", MMF_MAX_SIZE, MemoryMappedFileAccess.ReadWrite);

                // creates a stream for this process, which allows it to write data from offset 0 to 1024 (whole memory)
                MemoryMappedViewStream mmvStream = mmf.CreateViewStream(0, MMF_VIEW_SIZE);

                // this is what we want to write to the memory mapped file
                Message message1 = new Message();
                message1.title = "test";
                message1.content = "hello world";

                // serialize the variable 'message1' and write it to the memory mapped file
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(mmvStream, message1);
                mmvStream.Seek(0, SeekOrigin.Begin); // sets the current position back to the beginning of the stream

                // the memory mapped file lives as long as this process is running
                Console.ReadLine();
            }

            return;

            Read1Async();
            Read1Async();

            for (int i = 0; i < 100; i++)
            {
                Task.WaitAll(
                    Task.Run(Read1Async),
                    Task.Run(Write1Async)
                    );
                Console.WriteLine();
            }
        }

        private static async Task Read1Async()
        {
            var path = new FileInfo("a.md").FullName.Replace(Path.DirectorySeparatorChar, '-');
            var token = $"dotnet-campus-configuration-{path}";

            var queue = new InterprocessQueue<int>(token, 32);
            var item = DateTime.Now.Second;
            queue.Enqueue(item);
            Console.WriteLine($"入队：{item}");
        }

        private static async Task Write1Async()
        {
            var path = new FileInfo("a.md").FullName.Replace(Path.DirectorySeparatorChar, '-');
            var token = $"dotnet-campus-configuration-{path}";

            var queue = new InterprocessQueue<int>(token, 32);
            if (queue.TryDequeue(out var result))
            {
                Console.WriteLine($"出队：{result}");
            }
            else
            {
                Console.WriteLine($"无事可做");
            }
        }
        [Serializable]  // mandatory
        class Message
        {
            public string title;
            public string content;
        }
    }
}
