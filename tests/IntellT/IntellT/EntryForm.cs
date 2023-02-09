using System.Diagnostics.Metrics;
using System.Reflection;
using System.Windows.Forms;

namespace IntellT {
    public partial class EntryForm: Form {
        public EntryForm() {
            InitializeComponent();

            _ = Intell.Win32.UxTheme.SetWindowTheme(_objectBrowserTreeView.Handle, "Explorer", null);

            this.Load += EntryForm_Load;
        }

        void EntryForm_Load(object? sender, EventArgs e) {
            _imageList.Images.Add("namespace", IntellT.Resource.namespace_24);
            _imageList.Images.Add("class", IntellT.Resource.class_24);

            var testers = Tester.GetAll();

            foreach (var tester in testers) Add(tester);
            

            _objectBrowserTreeView.Sort();
            _objectBrowserTreeView.ExpandAll();

            _objectBrowserTreeView.NodeMouseDoubleClick += (sender, e) => {
                var tester = e.Node.Tag as Tester;
                if (tester == null) return;

                if (tester.Type.IsSubclassOf(typeof(Form)) == true) {
                    var thread = new Thread(() => {
                        Application.Run((Form)Activator.CreateInstance(tester.Type));
                    });
                    thread.Start();
                }

                if (typeof(ITestClass).IsAssignableFrom(tester.Type) == true) {
                    var testclass = (ITestClass)Activator.CreateInstance(tester.Type);
                    testclass.Run();
                }
                if (tester.Type.IsSubclassOf(typeof(ITestClass)) == true) {
                    
                }
            };
        }

        void Add(Tester tester) {
            var splits = tester.FullName.Split('.');
            var _namespace = string.Join('.', splits, 0, splits.Length - 1);
            var _classname = splits.Last();

            var parentNode = _objectBrowserTreeView.Nodes.Find(_namespace, true).FirstOrDefault();
            if (parentNode == null) {
                parentNode = _objectBrowserTreeView.Nodes.Add(_namespace, _namespace);
                parentNode.ImageKey= parentNode.SelectedImageKey = "namespace";
            }
            
            var childNode = parentNode.Nodes.Add(_classname);
            childNode.ImageKey= childNode.SelectedImageKey = "class";
            childNode.Tag = tester;
        }

    }
}