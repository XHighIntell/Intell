using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntellT {
    public class Tester {

        public Tester(string fullname, Type type) {
            FullName = fullname;
            Type = type;
        }

        ///<summary>Gets or sets full name of </summary>
        ///<remarks>Ex: "Intell.Net.Sockets.Tcp.Socket"</remarks>
        public string FullName { get; set; } 

        public Type Type { get; set; }


        public void Run() { 
            
        }

        public static Tester[] GetAll() {
            var list = new List<Tester>();
            var all_types = typeof(TestClassAttribute).Assembly.GetTypes();

            for (var i = 0; i < all_types.Length; i++) {
                var type = all_types[i];
                var o = type.GetCustomAttributes(typeof(TestClassAttribute), false);

                if (o.Length != 0) {
                    var data = (TestClassAttribute?)o.FirstOrDefault();
                    if (data != null) list.Add(new Tester(data.FullName, type));
                }
            }

            return list.ToArray();
        }
    }
}
