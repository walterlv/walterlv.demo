using System;
using System.Runtime.CompilerServices;

namespace Walterlv.Demo
{
    public class Class1
    {
        private bool _done;
        private Action<string> _eee;

        public void Do1()
        {
            if (!_done)
            {
                _done = true;
                DoCore0("");
            }

            _eee -= DoCore0;
            _eee += DoCore0;

            void DoCore0(string text)
            {
                Console.WriteLine(text);
            }
        }

        public void Do2()
        {
            if (!_done)
            {
                _done = true;
                DoCore("");
            }
        }

        private void DoCore(string text)
        {
            Console.WriteLine(text);
        }
    }
}
