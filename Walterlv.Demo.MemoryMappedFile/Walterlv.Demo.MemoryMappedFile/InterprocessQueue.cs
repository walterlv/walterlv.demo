using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace Walterlv.Demo
{
    public class InterprocessQueue<T> : IProducerConsumerCollection<T>, IEnumerable<T>, IEnumerable, ICollection, IReadOnlyCollection<T>
        where T : struct
    {
        private readonly string _token;
        private readonly string _mappingToken;
        private readonly int _itemSize;
        private readonly long _memoryMappedFileSize;

        public InterprocessQueue(string token, int capacity)
        {
            _token = token;
            _mappingToken = '-' + _token;
            _itemSize = Unsafe.SizeOf<T>();
            _memoryMappedFileSize = _itemSize * (long)(capacity + 2);
        }

        public void Enqueue(T item)
        {
            using var mutex = new Mutex(false, _token);
            try
            {
                mutex.WaitOne();
                EnqueueCore(item);
            }
            catch(AbandonedMutexException)
            {
                // 其他线程/进程异常退出，可能导致临界区的代码没有执行完成。
                // 这里应该处理其他进程临界区代码没有完成执行时的修复逻辑。
                // 虽然发生了异常，但依然可以获得锁。
                EnqueueCore(item);
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }

        public bool TryDequeue(out T result)
        {
            using var mutex = new Mutex(false, _token);

            try
            {
                mutex.WaitOne();
                return TryDequeueCore(out result);
            }
            catch (AbandonedMutexException)
            {
                // 其他线程/进程异常退出，可能导致临界区的代码没有执行完成。
                // 这里应该处理其他进程临界区代码没有完成执行时的修复逻辑。
                // 虽然发生了异常，但依然可以获得锁。
                return TryDequeueCore(out result);
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }

        private void EnqueueCore(T item)
        {
            var mmf = MemoryMappedFile.CreateOrOpen(_mappingToken, _memoryMappedFileSize, MemoryMappedFileAccess.ReadWrite);
            var stream = mmf.CreateViewStream(0, _memoryMappedFileSize);

            var reader = new BinaryReader(stream);
            var head = reader.ReadInt32();
            var tail = reader.ReadInt32();
            Console.WriteLine($"当前 {tail} 个");

            stream.Seek(-sizeof(int), SeekOrigin.Current);
            var writer = new BinaryWriter(stream);
            writer.Write(tail);

            stream.Seek(tail * _itemSize, SeekOrigin.Current);
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, item);

            stream.Flush();

            stream.Seek(0, SeekOrigin.Begin);
            var ddd = reader.ReadBytes(32);
            stream.Seek(0, SeekOrigin.Begin);
            head = reader.ReadInt32();
            tail = reader.ReadInt32();
            Console.WriteLine($"当前 {tail} 个");

            stream.Seek(0, SeekOrigin.Begin);
        }

        private bool TryDequeueCore(out T result)
        {
            var mmf = MemoryMappedFile.OpenExisting(_mappingToken);
            var stream = mmf.CreateViewStream(0, _memoryMappedFileSize);

            var reader = new BinaryReader(stream);
            var head = reader.ReadInt32();
            var tail = reader.ReadInt32();

            if (tail <= 0)
            {
                result = default;
                return false;
            }

            stream.Seek(-sizeof(int), SeekOrigin.Current);
            var writer = new BinaryWriter(stream);
            writer.Write(tail - 1);

            stream.Seek(_itemSize * (head + 2), SeekOrigin.Current);
            var bytes = reader.ReadBytes(_itemSize * (tail - head));
            stream.Seek(_itemSize + _itemSize, SeekOrigin.Current);
            writer.Write(bytes);
            stream.Seek(_itemSize + _itemSize, SeekOrigin.Current);

            using var itemStream = new MemoryStream(new byte[] { bytes[0], bytes[1], bytes[2], bytes[3] });
            var formatter = new BinaryFormatter();
            result = (T)formatter.Deserialize(stream);

            stream.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            return true;
        }

        public int Count => throw new NotImplementedException();

        public bool IsSynchronized => throw new NotImplementedException();

        public object SyncRoot => throw new NotImplementedException();

        public void CopyTo(T[] array, int index)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public T[] ToArray()
        {
            throw new NotImplementedException();
        }

        public bool TryAdd(T item)
        {
            throw new NotImplementedException();
        }

        public bool TryTake([MaybeNullWhen(false)] out T item)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
