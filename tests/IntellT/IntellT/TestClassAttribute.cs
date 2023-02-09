using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IntellT {
    internal class TestClassAttribute: Attribute {

        public string FullName { get; set; }

        public TestClassAttribute(string fullName) {
            FullName = fullName;
        }
    }
}
