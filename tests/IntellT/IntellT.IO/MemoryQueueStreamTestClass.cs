using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Intell.IO;

namespace IntellT.IO {

    [TestClassAttribute("Intell.IO.MemoryQueueStream - Auto")]
    public class MemoryQueueStreamTestClass: ITestClass {
        void ITestClass.Run() {
            var stream = new MemoryQueueStream();

            var buffer1 = Encoding.ASCII.GetBytes("12345");
            var buffer2 = Encoding.ASCII.GetBytes("123456789");

            stream.Write(buffer1);
            stream.Write(buffer2, 5, 4);

            var correct_text = "123456789";
            var correct_count = correct_text.Length;

            var buffer3 = new byte[100] ;
            var count = stream.Read(buffer3, 0, 100);
            var text = Encoding.ASCII.GetString(buffer3, 0, count);

            if (count != correct_count) throw new Exception("Fail test");
            if (text != correct_text) throw new Exception("Fail test");


        }
    }
}
