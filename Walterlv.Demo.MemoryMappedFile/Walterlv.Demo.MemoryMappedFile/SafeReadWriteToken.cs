//using System;
//using System.IO;
//using System.IO.MemoryMappedFiles;
//using System.Threading;

//namespace Walterlv.Demo
//{
//    public class SafeReadWrite : IDisposable
//    {
//        public static SafeReadWrite GetReadTokenForFile(string fileName) => GetReadTokenForFile(new FileInfo(fileName));

//        public static SafeReadWrite GetWriteTokenForFile(string fileName) => GetWriteTokenForFile(new FileInfo(fileName));

//        public static SafeReadWrite GetReadTokenForFile(FileInfo file)
//        {
//            if (file is null)
//            {
//                throw new ArgumentNullException(nameof(file));
//            }

//            var path = file.FullName.Replace(Path.DirectorySeparatorChar, '-');
//            var memoryToken = $"-dotnet-campus-configuration-{path}";
//            var fileToken = $"dotnet-campus-configuration-{path}";

//            // 我们会有两个锁，一个是内存锁，一个是文件锁。
//            var memoryState = MemoryMappingState.Read(memoryToken);
//            if (memoryState.CanRead)
//            {

//            }

//            var fileMutex = new Mutex(false, fileToken);
//            fileMutex.WaitOne();

//            return new SafeReadWrite(fileMutex);
//        }

//        public static SafeReadWrite GetWriteTokenForFile(FileInfo file, CriticalDataSafetyMode mode)
//        {
//            if (file is null)
//            {
//                throw new ArgumentNullException(nameof(file));
//            }

//            var path = file.FullName.Replace(Path.DirectorySeparatorChar, '-');
//            var memoryToken = $"-dotnet-campus-configuration-{path}";
//            var fileToken = $"dotnet-campus-configuration-{path}";

//            MemoryMappingState.Write(memoryToken, mode);

//            var fileMutex = new Mutex(false, fileToken);
//            fileMutex.WaitOne();


//            return new SafeReadWrite(fileMutex);
//        }

//        private readonly Mutex _mutex;
//        private bool _disposedValue = false;

//        private SafeReadWrite(Mutex mutex)
//        {
//            _mutex = mutex ?? throw new ArgumentNullException(nameof(mutex));
//        }

//        protected virtual void Dispose(bool disposing)
//        {
//            if (!_disposedValue)
//            {
//                if (disposing)
//                {
//                    _mutex.ReleaseMutex();
//                }
//                _disposedValue = true;
//            }
//        }

//        public void Dispose()
//        {
//            Dispose(true);
//        }

//        private readonly struct MemoryMappingState
//        {
//            private readonly CriticalDataSafetyMode _mode;

//            public MemoryMappingState(CriticalDataSafetyMode mode)
//            {
//                _mode = mode;
//            }

//            internal static MemoryMappingState Read(string token)
//            {
//                using var memoryMutex = new Mutex(false, token);
//                memoryMutex.WaitOne();

//                using var mmf = MemoryMappedFile.CreateOrOpen(token, 32);
//                using var stream = mmf.CreateViewStream(0, 4);

//                var reader = new BinaryReader(stream);
//                var mode = (CriticalDataSafetyMode)reader.ReadInt32();

//                return new MemoryMappingState(mode);
//            }

//            internal static void Write(string token, MemoryMappingState state)
//            {
//                using var memoryMutex = new Mutex(false, token);
//                memoryMutex.WaitOne();

//                using var mmf = MemoryMappedFile.CreateOrOpen(token, 32);
//                using var stream = mmf.CreateViewStream(0, 4);

//                var reader = new BinaryReader(stream);
//                var mode = (CriticalDataSafetyMode)reader.ReadInt32();

//                var newMode = mode.Combine(state._mode);

//                var writer = new BinaryWriter(stream);
//                writer.Write((int)newMode);
//            }
//        }
//    }

public enum CriticalDataSafetyMode
{
    Unsafe,
    UnsafeFirst,
    Safe,
    SafeFirst,
}

public static class CriticalDataSafetyModeExtensions
{
    public static CriticalDataSafetyMode Combine(this CriticalDataSafetyMode existed, CriticalDataSafetyMode mode) => existed switch
    {
        CriticalDataSafetyMode.Unsafe => mode switch
        {
            CriticalDataSafetyMode.Unsafe => CriticalDataSafetyMode.Unsafe,
            CriticalDataSafetyMode.UnsafeFirst => CriticalDataSafetyMode.Unsafe,
            CriticalDataSafetyMode.Safe => throw new InvalidOperationException("Safe 模式无法读写此文件，因为此文件正在被其他进程以 Unsafe 的方式进行读写。"),
            CriticalDataSafetyMode.SafeFirst => CriticalDataSafetyMode.Unsafe,
            _ => existed,
        },
        CriticalDataSafetyMode.UnsafeFirst => mode switch
        {
            CriticalDataSafetyMode.Unsafe => CriticalDataSafetyMode.Unsafe,
            CriticalDataSafetyMode.UnsafeFirst => CriticalDataSafetyMode.UnsafeFirst,
            CriticalDataSafetyMode.Safe => CriticalDataSafetyMode.Safe,
            CriticalDataSafetyMode.SafeFirst => CriticalDataSafetyMode.SafeFirst,
            _ => existed,
        },
        CriticalDataSafetyMode.Safe => mode switch
        {
            CriticalDataSafetyMode.Unsafe => throw new InvalidOperationException("Unsafe 模式无法读写此文件，因为此文件正在被其他进程以 Safe 的方式进行读写。"),
            CriticalDataSafetyMode.UnsafeFirst => CriticalDataSafetyMode.Safe,
            CriticalDataSafetyMode.Safe => CriticalDataSafetyMode.Safe,
            CriticalDataSafetyMode.SafeFirst => CriticalDataSafetyMode.Safe,
            _ => existed,
        },
        CriticalDataSafetyMode.SafeFirst => mode switch
        {
            CriticalDataSafetyMode.Unsafe => CriticalDataSafetyMode.Unsafe,
            CriticalDataSafetyMode.UnsafeFirst => CriticalDataSafetyMode.SafeFirst,
            CriticalDataSafetyMode.Safe => CriticalDataSafetyMode.Safe,
            CriticalDataSafetyMode.SafeFirst => CriticalDataSafetyMode.SafeFirst,
            _ => existed,
        },
        _ => existed,
    };
}
//}


using System;
using System.IO;
using System.Threading;

internal class SafeFileReadWriteToken : IDisposable
{
    public static SafeFileReadWriteToken GetForFile(string fileName) => GetForFile(new FileInfo(fileName));

    public static SafeFileReadWriteToken GetForFile(FileInfo file)
    {
        if (file is null)
        {
            throw new ArgumentNullException(nameof(file));
        }

        var path = file.FullName.Replace(Path.DirectorySeparatorChar, '-');
        var mutex = new Mutex(false, $"dotnet-campus-configuration-{path}");
        mutex.WaitOne();
        return new SafeFileReadWriteToken(mutex);
    }

    private readonly Mutex _mutex;
    private bool _disposedValue = false;

    private SafeFileReadWriteToken(Mutex mutex)
    {
        _mutex = mutex ?? throw new ArgumentNullException(nameof(mutex));
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _mutex.ReleaseMutex();
            }
            _disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
    }
}