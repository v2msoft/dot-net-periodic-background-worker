## PeriodicBackgroundWorker

Abstract .NET class that implements a periodical background worker that has graceful shutdown.

## Usage Example

```
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;

namespace Test {

    class MyWorker : PeriodicBackgroundWorker {

        public MyWorker() : base(1000) {}

        protected override void DoWork(object sender, ElapsedEventArgs e) {
            Console.WriteLine("Hello World!");
            Thread.Sleep(10000);
            Console.WriteLine("Bye World!");
        }

    }

    class Program {


        static void Main(string[] args) {

            MyWorker worker = new MyWorker();
            worker.Start();

            Thread.Sleep(2000);

            new Thread(() => {
                worker.Join();
                Console.WriteLine("Exiting Thread 1");
            }).Start();

            new Thread(() => {
                worker.Join();
                Console.WriteLine("Exiting Thread 2");
            }).Start();


            new Thread(() => {
                worker.Join();
                Console.WriteLine("Exiting Thread 3");
            }).Start();

            worker.Stop();
            Console.WriteLine("DONE!");
            Console.Read();

        }
       
    }
}
```

### License
MIT License

Copyright (c) 2016 V2MSoftware Consultoria Informatica

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
