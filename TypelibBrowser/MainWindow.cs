﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

using Gtk;

namespace GISharp.TypelibBrowser
{
    public partial class MainWindow : Window
    {
        static readonly string[] ignoredProperties = {
            "Attributes",
            "Container",
            "Handle",
            "Parent",
        };

        static readonly string[] childProperties = {
            "Args",
            "Constants",
            "Discriminators",
            "DiscriminatorType",
            "Fields",
            "Interfaces",
            "Methods",
            "Prerequisites",
            "Properties",
            "Property",
            "ReturnTypeInfo",
            "Signals",
            "TypeInfo",
            "Values",
            "VFuncs",
        };

        static readonly string[] linkProperties = {
            "ClassStruct",
            "IfaceStruct",
            "Interface",
            "Invoker",
            "VFunc",
        };

        Stack<string> backStack = new Stack<string> ();
        Stack<string> forwardStack = new Stack<string> ();

        public event EventHandler<SelectedNamespaceChangedEventArgs> SelectedNamespaceChanged;

        public event EventHandler<SelectedInfoChangedEventArgs> SelectedInfoChanged;

        public MainWindow () : base (Gtk.WindowType.Toplevel)
        {
            Build ();

            backButton.Label = null;
            backButton.Image = new Image (Stock.GoBack, IconSize.Button);
            backButton.Clicked += backButton_Clicked;
            forwardButton.Label = null;
            forwardButton.Image = new Image (Stock.GoForward, IconSize.Button);
            forwardButton.ImagePosition = PositionType.Right;
            forwardButton.Clicked += forwardButton_Clicked;

            namespaceNodeview.AppendColumn ("Namespace", new CellRendererText (), "text", 0);
            namespaceNodeview.AppendColumn ("Version", new CellRendererText (), "text", 1);
            namespaceNodeview.NodeStore = new NodeStore (typeof(NamespaceTreeNode));
            namespaceNodeview.NodeSelection.Changed += (sender, e) => {
                if (SelectedNamespaceChanged != null) {
                    var selectedNode = namespaceNodeview.NodeSelection.SelectedNode as NamespaceTreeNode;
                    var @namespace = selectedNode == null ? null : selectedNode.Namespace;
                    SelectedNamespaceChanged (this, new SelectedNamespaceChangedEventArgs (@namespace));
                }
            };

            infoTreeview.AppendColumn ("Name", new CellRendererText(), "text", 0, "strikethrough", 1, "foreground", 2);
            infoTreeview.Selection.Changed += (sender, e) => {
                if (SelectedInfoChanged != null) {
                    object selected = null;
                    TreeIter iter;
                    if (infoTreeview.Selection.GetSelected(out iter) && iter.UserData != IntPtr.Zero) {
                        selected = GCHandle.FromIntPtr (iter.UserData).Target;
                    }
                    SelectedInfoChanged (this, new SelectedInfoChangedEventArgs (selected));
                }
            };

            ClearTypelibInfo();
            ClearTypeInfo ();
        }

        public void AddNamespace (string @namespace, string version)
        {
            if (@namespace == null) {
                throw new ArgumentNullException ("namespace");
            }
            if (version == null) {
                throw new ArgumentNullException ("version");
            }
            namespaceNodeview.NodeStore.AddNode (new NamespaceTreeNode (@namespace, version));
        }

        public void ClearTypelibInfo ()
        {
            pathLabel.LabelProp = null;
            versionsLabel.LabelProp = null;
            dependsLabel.LabelProp = null;
            libraryLabel.LabelProp = null;
        }

        public void SetTypelibInfo (string path, string versions, string depends, string library)
        {
            pathLabel.LabelProp = path;
            versionsLabel.LabelProp = versions;
            dependsLabel.LabelProp = depends;
            libraryLabel.LabelProp = library;
        }

        public void ClearInfos()
        {
            infoTreeview.Model = null;
        }

        public void AddInfo (TreeModelImplementor implementor)
        {
            if (implementor== null) {
                throw new ArgumentNullException ("implementor");
            }
            infoTreeview.Model = new TreeModelAdapter (implementor);
        }

        public void ClearTypeInfo ()
        {
            typeInfoLabel.LabelProp = "<b>Type Info</b>";
            foreach (var child in typeInfoVbox.Children.ToArray ()) {
                typeInfoVbox.Remove (child);
            }
        }

        public void SetTypeInfo (object obj)
        {
            var objValueLabel = new Label () {
                LabelProp = string.Format ("<b>{0}</b>",
                    System.Security.SecurityElement.Escape (obj.ToString ())),
                UseMarkup = true,
                Xalign = 0,
            };
            objValueLabel.SizeAllocated += (o, args) => {
                if (args.Allocation.Width < objValueLabel.ChildRequisition.Width) {
                    objValueLabel.TooltipText = obj.ToString ();
                } else {
                    objValueLabel.TooltipText = null;
                }
            };
            objValueLabel.Show ();
            typeInfoVbox.PackStart (objValueLabel, false, false, 12);

            var properties = obj.GetType ().GetProperties ();
            var propertyTable = new Table ((uint)properties.Length, 2, false) {
                RowSpacing = 6,
                ColumnSpacing = 6
            };

            uint row = 0;
            foreach (var property in properties) {
                
                if (ignoredProperties.Contains(property.Name) || childProperties.Contains(property.Name)) {
                    // skip these properties
                    continue;
                }
                var descLabel = new Label (property.Name + ":") {
                    Xalign = 1
                };
                propertyTable.Attach (descLabel, 0, 1, row, row + 1);

                var value = property.GetValue (obj);
                if (linkProperties.Contains (property.Name) && value != null) {
                    var linkLabel = new LinkButton (string.Empty, value.ToString ()) {
                        Xalign = 0,
                        HasTooltip = false,
                    };
                    linkLabel.Clicked += (sender, e) => {
                        navigateTo (linkLabel.Label);
                    };
                    propertyTable.Attach (linkLabel, 1, 2, row, row + 1);
                } else {
                    var valueLabel = new Label () {
                        LabelProp = (value ?? "<null>").ToString (),
                        Xalign = 0,
                    };
                    propertyTable.Attach (valueLabel, 1, 2, row, row + 1);
                }
                row++;
            }
            propertyTable.ShowAll ();
            typeInfoVbox.PackStart (propertyTable, false, false, 12);
        }

        void navigateTo (string infoPath, bool push = true)
        {
            if (push) {
                InfoTreeModelImpl.Info selectedInfo = null;
                TreeIter infoIter;
                if (infoTreeview.Selection.GetSelected(out infoIter) && infoIter.UserData != IntPtr.Zero) {
                    selectedInfo = GCHandle.FromIntPtr (infoIter.UserData).Target as InfoTreeModelImpl.Info;
                }
                if (selectedInfo == null) {
                    Debug.Fail ("Could not get selected info.");
                    return;
                }

                backStack.Push (selectedInfo.Namespace + "." + selectedInfo.Path);
                backButton.Sensitive = true;
                forwardStack.Clear ();
                forwardButton.Sensitive = false;
            }

            var pathElements = infoPath.Split ('.');
            TreeIter iter;

            foreach (NamespaceTreeNode node in namespaceNodeview.NodeStore) {
                if (node.Namespace == pathElements[0]) {
                    namespaceNodeview.NodeSelection.SelectedNode = node;
                    namespaceNodeview.Selection.GetSelected (out iter);
                    var path = namespaceNodeview.Model.GetPath (iter);
                    namespaceNodeview.ScrollToCell (path, namespaceNodeview.Columns[0], false, 0, 0);
                    break;
                }
            }

            infoTreeview.Model.IterChildren (out iter);
            foreach (var element in pathElements.Skip (1)) {
                do {
                    if (iter.UserData == IntPtr.Zero) {
                        continue;
                    }
                    var node = GCHandle.FromIntPtr (iter.UserData).Target as InfoTreeModelImpl.INode;
                    if (node.Name == element) {
                        if (!infoTreeview.Model.IterHasChild (iter)) {
                            break;
                        }
                        if (infoTreeview.Model.IterChildren (out iter, iter)) {
                            break;
                        }
                        throw new Exception ();
                    }
                } while (infoTreeview.Model.IterNext (ref iter));
            }
            var treePath = infoTreeview.Model.GetPath (iter);
            if (treePath.Depth >= pathElements.Length) {
                // if the target node had children, iter will be the first child
                // so we have to go up a level to get the desired node.
                treePath.Up ();
                infoTreeview.Model.IterParent (out iter, iter);
            }
            infoTreeview.ExpandToPath (treePath);
            infoTreeview.ScrollToCell (treePath, infoTreeview.Columns [0], false, 0, 0);
            infoTreeview.Selection.SelectIter (iter);
        }

        void backButton_Clicked (object sender, EventArgs e)
        {
            if (backStack.Count == 0) {
                Debug.Fail ("Empty back stack.");
                return;
            }
            var destination = backStack.Pop ();
            if (backStack.Count == 0) {
                backButton.Sensitive = false;
            }
            navigateTo (destination, push: false);
            // TODO: this should actuall be the current selection, not `destination`.
            forwardStack.Push (destination);
            forwardButton.Sensitive = true;
        }

        void forwardButton_Clicked (object sender, EventArgs e)
        {
            if (forwardStack.Count == 0) {
                Debug.Fail ("Empty forward stack.");
                return;
            }
            var destination = forwardStack.Pop ();
            if (forwardStack.Count == 0) {
                forwardButton.Sensitive = false;
            }
            navigateTo (destination, push: false);
            backStack.Push (destination);
            backButton.Sensitive = true;
        }
    }

    [TreeNode (ListOnly=true)]
    public class NamespaceTreeNode : TreeNode
    {
        public NamespaceTreeNode (string @namespace, string version)
        {
            Namespace = @namespace;
            Version = version;
        }

        [TreeNodeValue (Column=0)]
        public string Namespace { get; private set; }

        [TreeNodeValue (Column=1)]
        public string Version { get; private set; }
    }

    public class SelectedNamespaceChangedEventArgs : EventArgs
    {
        public string Namespace { get; private set; }

        public SelectedNamespaceChangedEventArgs (string @namespace)
        {
            Namespace = @namespace;
        }
    }

    public class SelectedInfoChangedEventArgs : EventArgs
    {
        public object UserData { get; private set; }

        public SelectedInfoChangedEventArgs (object userData)
        {
            UserData = userData;
        }
    }
}