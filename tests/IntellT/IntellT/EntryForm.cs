using Intell.Win32;

namespace IntellT;

public partial class EntryForm: Form {
    public EntryForm() {
        InitializeComponent();

        _ = Intell.Win32.UxTheme.SetWindowTheme(_objectBrowserTreeView.Handle, "Explorer", null);
        User32.SendMessage(_objectBrowserTreeView.Handle, TVM_SETEXTENDEDSTYLE, TVS_EX_DOUBLEBUFFER, TVS_EX_DOUBLEBUFFER);

        this.Load += EntryForm_Load;
    }
    
    private const int TVM_SETEXTENDEDSTYLE = 0x1100 + 44;
    private const int TVS_EX_DOUBLEBUFFER = 0x0004;

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
                    Application.Run((Form)Activator.CreateInstance(tester.Type)!);
                });
                thread.Start();
            }

            if (typeof(ITestClass).IsAssignableFrom(tester.Type) == true) {
                var testclass = (ITestClass)Activator.CreateInstance(tester.Type)!;
                testclass!.Run();
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