using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Runtime.InteropServices;
using GISharp.Runtime;
using System.Reflection;

using GISharp.GLib;

namespace GISharp.GObject
{
    /// <summary>
    /// All the fields in the GObject structure are private
    /// to the #GObject implementation and should never be accessed directly.
    /// </summary>
    [GType ("GObject", IsProxyForUnmanagedType = true)]
    [GTypeStruct (typeof (ObjectClass))]
    public class Object : TypeInstance, INotifyPropertyChanged
    {
        static readonly Quark toggleRefGCHandleQuark = Quark.FromString("gisharp-gobject-toggle-ref-gc-handle-quark");
        static readonly IntPtr refCountOffset = Marshal.OffsetOf<Struct> (nameof(Struct.RefCount));

        UnmanagedToggleNotify toggleNotifyDelegate;

        protected new struct Struct
        {
            #pragma warning disable CS0649
            public TypeInstance.Struct GTypeInstance;
            public uint RefCount;
            public IntPtr Qdata;
            #pragma warning restore CS0649
        }

        uint RefCount {
            get {
                AssertNotDisposed ();
                return (uint)Marshal.ReadInt32 (handle + (int)refCountOffset);
            }
        }

        public Object (IntPtr handle, Transfer ownership) : base (handle)
        {
            if (ownership == Transfer.None) {
                this.handle = g_object_ref_sink (handle);
            }

            // by creating a new delegate for each instance, we are in effect
            // creating a unique identifier for this instance that will be used
            // when removing the toggle reference in Dispose().
            toggleNotifyDelegate = toggleNotifyCallback;

            // always start with a strong reference to the managed object
            var gcHandle = GCHandle.Alloc (this);
            g_object_set_qdata(handle, toggleRefGCHandleQuark, (IntPtr)gcHandle);
            g_object_add_toggle_ref (handle, toggleNotifyDelegate, IntPtr.Zero);

            // IntPtr always owns a reference so release it now that we have a toggle reference instead.
            // If this is the last normal reference, toggleNotifyCallback will be called immediately
            // to convert the strong reference to a weak reference
            g_object_unref (handle);
        }

        protected override void Dispose (bool disposing)
        {
            if (handle != IntPtr.Zero) {
                g_object_ref (handle);
                g_object_remove_toggle_ref (handle, toggleNotifyDelegate, IntPtr.Zero);
                var gcHandle = (GCHandle)g_object_get_qdata(handle, toggleRefGCHandleQuark);
                g_object_set_qdata(handle, toggleRefGCHandleQuark, IntPtr.Zero);
                gcHandle.Free ();
                g_object_unref (handle);
            }
            base.Dispose (disposing);
        }

        [DllImport ("gobject-2.0", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr g_object_ref (IntPtr @object);

        [DllImport ("gobject-2.0", CallingConvention = CallingConvention.Cdecl)]
        static extern void g_object_unref(IntPtr @object);

        [DllImport ("gobject-2.0", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr g_object_ref_sink (IntPtr @object);

        [DllImport ("gobject-2.0", CallingConvention = CallingConvention.Cdecl)]
        static extern void g_object_add_toggle_ref (IntPtr @object, UnmanagedToggleNotify notify, IntPtr data);

        [DllImport ("gobject-2.0", CallingConvention = CallingConvention.Cdecl)]
        static extern void g_object_remove_toggle_ref (IntPtr @object, UnmanagedToggleNotify notify, IntPtr data);

        static void toggleNotifyCallback (IntPtr data, IntPtr @object, bool isLastRef)
        {
            try {
                // free the existing GCHandle
                var gcHandle = (GCHandle)g_object_get_qdata(@object, toggleRefGCHandleQuark);
                var obj = (Object)gcHandle.Target;
                gcHandle.Free ();

                // alloc a new GCHandle with weak/strong reference depending on isLastRef
                gcHandle = GCHandle.Alloc (obj, isLastRef ? GCHandleType.Weak : GCHandleType.Normal);
                g_object_set_qdata(@object, toggleRefGCHandleQuark, (IntPtr)gcHandle);
            }
            catch (Exception ex) {
                ex.LogUnhandledException ();
            }
        }

        [DllImport ("gobject-2.0", CallingConvention = CallingConvention.Cdecl)]
        static extern GType g_object_get_type ();

        static GType getType ()
        {
            return g_object_get_type ();
        }

        public class NotifyEventArgs : EventArgs
        {
            public ParamSpec Pspec { get; private set; }

            public NotifyEventArgs(ParamSpec pspec)
            {
                Pspec = pspec;
            }
        }

        public delegate void NotifyEventHandler(Object sender, NotifyEventArgs pspec);
        
        delegate void UnmangedNotify(IntPtr gobject, IntPtr pspec, IntPtr userData);

        ConcurrentDictionary<NotifyEventHandler, SignalHandler> notifiedHandlers =
            new ConcurrentDictionary<NotifyEventHandler, SignalHandler>();

        [GSignal("notify", When = EmissionStage.First, NoRecurse = true, Detailed = true, Action = true, NoHooks = true)]
        public event NotifyEventHandler Notify {
            add {
                notifiedHandlers.AddOrUpdate(value,
                    v => this.Connect("notify", UnmanagedNotifyCallbackFactory.CreateNotifyCallback, v),
                    (v, h) => { throw new NotSupportedException(); });
            }
            remove {
                if (notifiedHandlers.TryRemove(value, out var handler)) {
                    handler.Disconnect();
                }
            }
        }

        static class UnmanagedNotifyCallbackFactory
        {
            class UnmanagedNotifyData
            {
                public NotifyEventHandler Handler;
                public UnmangedNotify UnmanagedHandler;
                public UnmanagedClosureNotify UnmangedNotify;
            }

            public static ValueTuple<Delegate, UnmanagedClosureNotify, IntPtr> CreateNotifyCallback(NotifyEventHandler handler)
            {
                var data = new UnmanagedNotifyData {
                    Handler = handler,
                    UnmanagedHandler = UnmanagedHandler,
                    UnmangedNotify = UnmanagedNotify,
                };
                var gcHandle = GCHandle.Alloc(data);

                return (data.UnmanagedHandler, data.UnmangedNotify, (IntPtr)gcHandle);
            }

            static void UnmanagedHandler(IntPtr gobject_, IntPtr pspec_, IntPtr userData_)
            {
                try {
                    var gobject = Object.GetInstance(gobject_, Transfer.None);
                    var pspec = ParamSpec.GetInstance(pspec_, Transfer.None);
                    var gcHandle = (GCHandle)userData_;
                    var data = (UnmanagedNotifyData)gcHandle.Target;

                    var args = new NotifyEventArgs(pspec);
                    data.Handler(gobject, args);
                }
                catch (Exception ex) {
                    ex.LogUnhandledException();
                }
            }

            static void UnmanagedNotify(IntPtr data_, IntPtr closure_)
            {
                try {
                    var gcHandle = (GCHandle)data_;
                    gcHandle.Free();
                }
                catch (Exception ex) {
                    ex.LogUnhandledException();
                }
            }
        }

        [UnmanagedFunctionPointer (CallingConvention.Cdecl)]
        delegate void UnmanagedNotify (IntPtr gobjectPtr, IntPtr pspecPtr, IntPtr userDataPtr);

        static readonly UnmanagedNotify nativeNotifyDelegate = UnmanagedOnNotify;
        static readonly IntPtr nativeNotifyPtr = Marshal.GetFunctionPointerForDelegate (nativeNotifyDelegate);

        static void UnmanagedOnNotify (IntPtr gobjectPtr, IntPtr pspecPtr, IntPtr userDataPtr)
        {
            try {
                var obj = GetInstance(gobjectPtr, Transfer.None);
                var pspec = ParamSpec.GetInstance(pspecPtr, Transfer.None);
                var propInfo = (PropertyInfo)pspec[ObjectClass.managedClassPropertyInfoQuark];
                obj.propertyChangedHandler?.Invoke(obj, new PropertyChangedEventArgs(propInfo.Name));
            }
            catch (Exception ex) {
                ex.LogUnhandledException ();
            }
        }

        PropertyChangedEventHandler propertyChangedHandler;
        object propertyChangedHandlerLock = new object ();
        SignalHandler notifySignalHandler;

        SignalHandler ConnectNotifySignal ()
        {
            var detailedSignalPtr = GMarshal.StringToUtf8Ptr ("notify");
            var id = Signal.g_signal_connect_data (handle, detailedSignalPtr,
                nativeNotifyPtr, IntPtr.Zero, null, default (ConnectFlags));
            GMarshal.Free (detailedSignalPtr);

            return new SignalHandler (this, id);
        }

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged {
            add {
                lock (propertyChangedHandlerLock) {
                    if (propertyChangedHandler == null) {
                        notifySignalHandler = ConnectNotifySignal ();
                    }
                    propertyChangedHandler += value;
                }
            }
            remove {
                lock (propertyChangedHandlerLock) {
                    propertyChangedHandler -= value;
                    if (propertyChangedHandler == null) {
                        notifySignalHandler.Disconnect ();
                        notifySignalHandler = null;
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// Creates a new instance of a #GObject subtype and sets its properties.
        /// </summary>
        /// <remarks>
        /// Construction parameters (see #G_PARAM_CONSTRUCT, #G_PARAM_CONSTRUCT_ONLY)
        /// which are not explicitly specified are set to their default values.
        /// </remarks>
        /// <param name="objectType">
        /// the type id of the #GObject subtype to instantiate
        /// </param>
        /// <param name="nParameters">
        /// the length of the @parameters array
        /// </param>
        /// <param name="parameters">
        /// an array of #GParameter
        /// </param>
        /// <returns>
        /// a new instance of
        /// @object_type
        /// </returns>
        [DllImport ("gobject-2.0", CallingConvention = CallingConvention.Cdecl)]
        /* <type name="Object" type="gpointer" managed-name="Object" /> */
        /* transfer-ownership:full */
        internal static extern IntPtr g_object_newv (
            /* <type name="GType" type="GType" managed-name="GType" /> */
            /* transfer-ownership:none */
            GType objectType,
            /* <type name="guint" type="guint" managed-name="Guint" /> */
            /* transfer-ownership:none */
            uint nParameters,
            /* <array length="1" zero-terminated="0" type="GParameter*">
               <type name="Parameter" type="GParameter" managed-name="Parameter" />
               </array> */
            /* transfer-ownership:none */
            IntPtr parameters);

        protected static IntPtr New<T> (params object[] parameters) where T : Object
        {
            var gtype = GType.TypeOf<T> ();
            var paramArray = new Parameter[parameters.Length / 2];
            for (int i = 0; i < parameters.Length; i += 2) {
                var name = parameters[i] as string;
                if (name == null) {
                    var message = string.Format ("Expecting string at index {0}", i);
                    throw new ArgumentException (message, nameof (parameters));
                }
                var objClass = TypeClass.Get<ObjectClass> (gtype);
                var paramSpec = objClass.FindProperty (name);
                if (paramSpec == null) {
                    var message = string.Format ("Could not find property '{0}'", name);
                    throw new ArgumentException (message, nameof (parameters));
                }
                var value = new Value (paramSpec.ValueType, parameters[i + 1]);
                paramArray[i / 2] = new Parameter {
                    Name = GMarshal.StringToUtf8Ptr (name),
                };
                Marshal.StructureToPtr<Value> (value, paramArray[i / 2].Value, false);
            }
            var paramArrayPtr = GMarshal.CArrayToPtr<Parameter> (paramArray, false);
            try {
                var ret = g_object_newv (gtype, (uint)paramArray.Length, paramArrayPtr);
                return ret;
            }
            finally {
                GMarshal.Free (paramArrayPtr);
                foreach (var p in paramArray) {
                    GMarshal.Free (p.Name);
                }
            }
        }

        /// <summary>
        /// Creates a new instance of type <typeparamref name="T"/> using the specified
        /// property names and values.
        /// </summary>
        /// <returns>The instance.</returns>
        /// <param name="parameters">Property name and value pairs.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T CreateInstance<T> (params object[] parameters) where T : Object
        {
            var handle = New<T> (parameters);
            var instance = (T)Activator.CreateInstance (typeof (T), handle);

            return instance;
        }

        public Object () : this (New<Object> (), Transfer.Full)
        {
        }

        [Since ("2.10")]
        [DllImport ("gobject-2.0", CallingConvention = CallingConvention.Cdecl)]
        static extern bool g_object_is_floating (IntPtr @object);

        bool IsFloating {
            get {
                AssertNotDisposed ();
                return g_object_is_floating (handle);
            }
        }

        /// <summary>
        /// Find the #GParamSpec with the given name for an
        /// interface. Generally, the interface vtable passed in as @g_iface
        /// will be the default vtable from g_type_default_interface_ref(), or,
        /// if you know the interface has already been loaded,
        /// g_type_default_interface_peek().
        /// </summary>
        /// <param name="gIface">
        /// any interface vtable for the interface, or the default
        ///  vtable for the interface
        /// </param>
        /// <param name="propertyName">
        /// name of a property to lookup.
        /// </param>
        /// <returns>
        /// the #GParamSpec for the property of the
        ///          interface with the name @property_name, or %NULL if no
        ///          such property exists.
        /// </returns>
        [Since ("2.4")]
        [DllImport ("gobject-2.0", CallingConvention = CallingConvention.Cdecl)]
        /* <type name="ParamSpec" type="GParamSpec*" managed-name="ParamSpec" /> */
        /* transfer-ownership:none */
        static extern IntPtr g_object_interface_find_property (
            /* <type name="gpointer" type="gpointer" managed-name="Gpointer" /> */
            /* transfer-ownership:none */
            IntPtr gIface,
            /* <type name="utf8" type="const gchar*" managed-name="Utf8" /> */
            /* transfer-ownership:none */
            IntPtr propertyName);

        /// <summary>
        /// Find the #GParamSpec with the given name for an
        /// interface. Generally, the interface vtable passed in as @g_iface
        /// will be the default vtable from g_type_default_interface_ref(), or,
        /// if you know the interface has already been loaded,
        /// g_type_default_interface_peek().
        /// </summary>
        /// <param name="gIface">
        /// any interface vtable for the interface, or the default
        ///  vtable for the interface
        /// </param>
        /// <param name="propertyName">
        /// name of a property to lookup.
        /// </param>
        /// <returns>
        /// the #GParamSpec for the property of the
        ///          interface with the name @property_name, or %NULL if no
        ///          such property exists.
        /// </returns>
        [Since ("2.4")]
        static ParamSpec InterfaceFindProperty (IntPtr gIface, string propertyName)
        {
            if (propertyName == null) {
                throw new ArgumentNullException (nameof (propertyName));
            }
            var propertyName_ = GMarshal.StringToUtf8Ptr (propertyName);
            var ret_ = g_object_interface_find_property (gIface, propertyName_);
            var ret = ParamSpec.GetInstance(ret_, Transfer.None);
            GMarshal.Free (propertyName_);
            return ret;
        }

        /// <summary>
        /// Add a property to an interface; this is only useful for interfaces
        /// that are added to GObject-derived types. Adding a property to an
        /// interface forces all objects classes with that interface to have a
        /// compatible property. The compatible property could be a newly
        /// created #GParamSpec, but normally
        /// g_object_class_override_property() will be used so that the object
        /// class only needs to provide an implementation and inherits the
        /// property description, default value, bounds, and so forth from the
        /// interface property.
        /// </summary>
        /// <remarks>
        /// This function is meant to be called from the interface's default
        /// vtable initialization function (the @class_init member of
        /// #GTypeInfo.) It must not be called after after @class_init has
        /// been called for any object types implementing this interface.
        /// </remarks>
        /// <param name="gIface">
        /// any interface vtable for the interface, or the default
        ///  vtable for the interface.
        /// </param>
        /// <param name="pspec">
        /// the #GParamSpec for the new property
        /// </param>
        [Since ("2.4")]
        [DllImport ("gobject-2.0", CallingConvention = CallingConvention.Cdecl)]
        /* <type name="none" type="void" managed-name="None" /> */
        /* transfer-ownership:none */
        static extern void g_object_interface_install_property (
            /* <type name="gpointer" type="gpointer" managed-name="Gpointer" /> */
            /* transfer-ownership:none */
            IntPtr gIface,
            /* <type name="ParamSpec" type="GParamSpec*" managed-name="ParamSpec" /> */
            /* transfer-ownership:none */
            IntPtr pspec);

        /// <summary>
        /// Add a property to an interface; this is only useful for interfaces
        /// that are added to GObject-derived types. Adding a property to an
        /// interface forces all objects classes with that interface to have a
        /// compatible property. The compatible property could be a newly
        /// created #GParamSpec, but normally
        /// g_object_class_override_property() will be used so that the object
        /// class only needs to provide an implementation and inherits the
        /// property description, default value, bounds, and so forth from the
        /// interface property.
        /// </summary>
        /// <remarks>
        /// This function is meant to be called from the interface's default
        /// vtable initialization function (the @class_init member of
        /// #GTypeInfo.) It must not be called after after @class_init has
        /// been called for any object types implementing this interface.
        /// </remarks>
        /// <param name="gIface">
        /// any interface vtable for the interface, or the default
        ///  vtable for the interface.
        /// </param>
        /// <param name="pspec">
        /// the #GParamSpec for the new property
        /// </param>
        [Since ("2.4")]
        static void InterfaceInstallProperty (IntPtr gIface, ParamSpec pspec)
        {
            if (pspec == null) {
                throw new ArgumentNullException (nameof (pspec));
            }
            g_object_interface_install_property (gIface, pspec.Handle);
            GC.KeepAlive (pspec);
        }

        /// <summary>
        /// Lists the properties of an interface.Generally, the interface
        /// vtable passed in as @g_iface will be the default vtable from
        /// g_type_default_interface_ref(), or, if you know the interface has
        /// already been loaded, g_type_default_interface_peek().
        /// </summary>
        /// <param name="gIface">
        /// any interface vtable for the interface, or the default
        ///  vtable for the interface
        /// </param>
        /// <param name="nPropertiesP">
        /// location to store number of properties returned.
        /// </param>
        /// <returns>
        /// a
        ///          pointer to an array of pointers to #GParamSpec
        ///          structures. The paramspecs are owned by GLib, but the
        ///          array should be freed with g_free() when you are done with
        ///          it.
        /// </returns>
        [Since ("2.4")]
        [DllImport ("gobject-2.0", CallingConvention = CallingConvention.Cdecl)]
        /* <array length="1" zero-terminated="0" type="GParamSpec**">
          <type name="ParamSpec" type="GParamSpec*" managed-name="ParamSpec" />
          </array> */
        /* transfer-ownership:container */
        static extern IntPtr g_object_interface_list_properties (
            /* <type name="gpointer" type="gpointer" managed-name="Gpointer" /> */
            /* transfer-ownership:none */
            IntPtr gIface,
            /* <type name="guint" type="guint*" managed-name="Guint" /> */
            /* direction:out caller-allocates:0 transfer-ownership:full */
            out uint nPropertiesP);

        /// <summary>
        /// Lists the properties of an interface.Generally, the interface
        /// vtable passed in as @g_iface will be the default vtable from
        /// g_type_default_interface_ref(), or, if you know the interface has
        /// already been loaded, g_type_default_interface_peek().
        /// </summary>
        /// <param name="gIface">
        /// any interface vtable for the interface, or the default
        ///  vtable for the interface
        /// </param>
        /// <returns>
        /// a
        ///          pointer to an array of pointers to #GParamSpec
        ///          structures. The paramspecs are owned by GLib, but the
        ///          array should be freed with g_free() when you are done with
        ///          it.
        /// </returns>
        [Since ("2.4")]
        static ParamSpec[] InterfaceListProperties (IntPtr gIface)
        {
            uint nPropertiesP_;
            var ret_ = g_object_interface_list_properties (gIface, out nPropertiesP_);
            var ret = GMarshal.PtrToOpaqueCArray<ParamSpec> (ret_, (int)nPropertiesP_, true);
            return ret;
        }

        /// <summary>
        /// Creates a binding between @source_property on @source and @target_property
        /// on @target. Whenever the @source_property is changed the @target_property is
        /// updated using the same value. For instance:
        /// </summary>
        /// <remarks>
        /// |[
        ///   g_object_bind_property (action, "active", widget, "sensitive", 0);
        /// ]|
        ///
        /// Will result in the "sensitive" property of the widget #GObject instance to be
        /// updated with the same value of the "active" property of the action #GObject
        /// instance.
        ///
        /// If @flags contains %G_BINDING_BIDIRECTIONAL then the binding will be mutual:
        /// if @target_property on @target changes then the @source_property on @source
        /// will be updated as well.
        ///
        /// The binding will automatically be removed when either the @source or the
        /// @target instances are finalized. To remove the binding without affecting the
        /// @source and the @target you can just call g_object_unref() on the returned
        /// #GBinding instance.
        ///
        /// A #GObject can have multiple bindings.
        /// </remarks>
        /// <param name="source">
        /// the source #GObject
        /// </param>
        /// <param name="sourceProperty">
        /// the property on @source to bind
        /// </param>
        /// <param name="target">
        /// the target #GObject
        /// </param>
        /// <param name="targetProperty">
        /// the property on @target to bind
        /// </param>
        /// <param name="flags">
        /// flags to pass to #GBinding
        /// </param>
        /// <returns>
        /// the #GBinding instance representing the
        ///     binding between the two #GObject instances. The binding is released
        ///     whenever the #GBinding reference count reaches zero.
        /// </returns>
        [Since ("2.26")]
        [DllImport ("gobject-2.0", CallingConvention = CallingConvention.Cdecl)]
        /* <type name="Binding" type="GBinding*" managed-name="Binding" /> */
        /* transfer-ownership:none */
        static extern IntPtr g_object_bind_property (
            /* <type name="Object" type="gpointer" managed-name="Object" /> */
            /* transfer-ownership:none */
            IntPtr source,
            /* <type name="utf8" type="const gchar*" managed-name="Utf8" /> */
            /* transfer-ownership:none */
            IntPtr sourceProperty,
            /* <type name="Object" type="gpointer" managed-name="Object" /> */
            /* transfer-ownership:none */
            IntPtr target,
            /* <type name="utf8" type="const gchar*" managed-name="Utf8" /> */
            /* transfer-ownership:none */
            IntPtr targetProperty,
            /* <type name="BindingFlags" type="GBindingFlags" managed-name="BindingFlags" /> */
            /* transfer-ownership:none */
            BindingFlags flags);

        /// <summary>
        /// Creates a binding between <paramref name="sourceProperty"/> on
        /// <paramref name="target"/> and <paramref name="targetProperty"/>
        /// on <paramref name="target"/>.
        /// </summary>
        /// <remarks>
        /// Whenever the <paramref name="sourceProperty"/>
        /// is changed the <paramref name="targetProperty"/> is
        /// updated using the same value. For instance:
        /// 
        /// |[
        ///   g_object_bind_property (action, "active", widget, "sensitive", 0);
        /// ]|
        ///
        /// Will result in the "sensitive" property of the widget #GObject instance to be
        /// updated with the same value of the "active" property of the action #GObject
        /// instance.
        ///
        /// If @flags contains %G_BINDING_BIDIRECTIONAL then the binding will be mutual:
        /// if @target_property on @target changes then the @source_property on @source
        /// will be updated as well.
        ///
        /// The binding will automatically be removed when either the @source or the
        /// @target instances are finalized. To remove the binding without affecting the
        /// @source and the @target you can just call g_object_unref() on the returned
        /// #GBinding instance.
        ///
        /// A #GObject can have multiple bindings.
        /// </remarks>
        /// <param name="sourceProperty">
        /// the property on this instance to bind
        /// </param>
        /// <param name="target">
        /// the target <see cref="Object"/>
        /// </param>
        /// <param name="targetProperty">
        /// the property on <paramref name="target"/> to bind
        /// </param>
        /// <param name="flags">
        /// flags to pass to <see cref="Binding"/>
        /// </param>
        /// <returns>
        /// the <see cref="Binding"/> instance representing the binding between
        /// the two <see cref="Object"/> instances. The binding is released
        /// whenever the <see cref="Binding"/> reference count reaches zero.
        /// </returns>
        [Since ("2.26")]
        public Binding BindProperty (string sourceProperty, Object target, string targetProperty, BindingFlags flags = BindingFlags.Default)
        {
            AssertNotDisposed ();
            if (sourceProperty == null) {
                throw new ArgumentNullException (nameof (sourceProperty));
            }
            if (target == null) {
                throw new ArgumentNullException (nameof (target));
            }
            if (targetProperty == null) {
                throw new ArgumentNullException (nameof (targetProperty));
            }

            var sourcePropertyInfo = GetType ().GetProperty (sourceProperty);
            if (sourcePropertyInfo == null) {
                throw new ArgumentException ("No matching property", nameof (sourceProperty));
            }
            sourceProperty = sourcePropertyInfo.TryGetGPropertyName();
            if (sourceProperty == null) {
                var message = $"{sourcePropertyInfo.Name} is not a registered GType property";
                throw new ArgumentException (message, nameof(sourceProperty));
            }

            var targetPropertyInfo = target.GetType ().GetProperty (targetProperty);
            if (targetPropertyInfo == null) {
                throw new ArgumentException ("No matching property", nameof (targetProperty));
            }
            targetProperty = targetPropertyInfo.TryGetGPropertyName();
            if (targetProperty == null) {
                var message = $"{targetPropertyInfo.Name} is not a registered GType property";
                throw new ArgumentException (message, nameof(targetProperty));
            }

            var sourceProperty_ = GMarshal.StringToUtf8Ptr (sourceProperty);
            var targetProperty_ = GMarshal.StringToUtf8Ptr (targetProperty);
            var ret_ = g_object_bind_property (handle, sourceProperty_, target.handle, targetProperty_, flags);
            var ret = GetInstance<Binding> (ret_, Transfer.None);
            GMarshal.Free (sourceProperty_);
            GMarshal.Free (targetProperty_);
            return ret;
        }

        [Since ("2.26")]
        [DllImport ("gobject-2.0", CallingConvention = CallingConvention.Cdecl)]
        /* <type name="Binding" type="GBinding*" managed-name="Binding" /> */
        /* transfer-ownership:none */
        static extern IntPtr g_object_bind_property_full (
            /* <type name="Object" type="gpointer" managed-name="Object" /> */
            /* transfer-ownership:none */
            IntPtr source,
            /* <type name="utf8" type="const gchar*" managed-name="Utf8" /> */
            /* transfer-ownership:none */
            IntPtr sourceProperty,
            /* <type name="Object" type="gpointer" managed-name="Object" /> */
            /* transfer-ownership:none */
            IntPtr target,
            /* <type name="utf8" type="const gchar*" managed-name="Utf8" /> */
            /* transfer-ownership:none */
            IntPtr targetProperty,
            /* <type name="BindingFlags" type="GBindingFlags" managed-name="BindingFlags" /> */
            /* transfer-ownership:none */
            BindingFlags flags,
            UnmanagedBindingTransformFunc transformTo,
            UnmanagedBindingTransformFunc transformFrom,
            IntPtr userData,
            UnmanagedDestroyNotify notify);

        /// <summary>
        /// Creates a binding between <paramref name="sourceProperty"/> on 
        /// this instance and <paramref name="targetProperty"/> on <paramref name="target"/>,
        /// allowing you to set the transformation functions to be used by
        /// the binding.
        /// </summary>
        /// <remarks>
        /// If flags contains <see cref="BindingFlags.Bidirectional"/> then the
        /// binding will be mutual: if <paramref name="targetProperty"/> on
        /// <paramref name="target"/> changes then the <paramref name="sourceProperty"/>
        /// on this instance will be updated as well. The <paramref name="transformFrom"/>
        /// function is only used in case of bidirectional bindings, otherwise it will be ignored.
        ///
        /// The binding will automatically be removed when either the this instance
        /// or the <paramref name="target"/> instances are finalized. To remove the binding
        /// without affecting this instance and the <paramref name="target"/> you can
        /// just call <see cref="Binding.Unbind"/> on the returned <see cref="Binding"/> instance.
        ///
        /// An <see cref="Object"/> can have multiple bindings.
        /// </remarks>
        /// <param name="sourceProperty">
        /// the property on this instance to bind
        /// </param>
        /// <param name="target">
        /// the target <see cref="Object"/>
        /// </param>
        /// <param name="targetProperty">
        /// the property on <paramref name="target"/> to bind
        /// </param>
        /// <param name="flags">
        /// flags to pass to <see cref="Binding"/>
        /// </param>
        /// <param name="transformTo">
        /// the transformation function from this instance to the <paramref name="target"/>,
        /// or <c>null</c> to use the default
        /// </param>
        /// <param name="transformFrom">
        /// the transformation function from the <paramref name="target"/> to this
        /// instance, or <c>null</c> to use the default
        /// </param>
        /// <returns>
        /// the <see cref="Binding"/> instance representing the binding between
        /// the two <see cref="Object"/> instances. The binding is released
        /// whenever the <see cref="Binding"/> reference count reaches zero.
        /// </returns>
        [Since ("2.26")]
        public Binding BindProperty (string sourceProperty, Object target, string targetProperty, BindingFlags flags, BindingTransformFunc transformTo, BindingTransformFunc transformFrom)
        {
            AssertNotDisposed ();
            if (sourceProperty == null) {
                throw new ArgumentNullException (nameof (sourceProperty));
            }
            if (target == null) {
                throw new ArgumentNullException (nameof (target));
            }
            if (targetProperty == null) {
                throw new ArgumentNullException (nameof (targetProperty));
            }
            var sourceProperty_ = GMarshal.StringToUtf8Ptr (sourceProperty);
            var targetProperty_ = GMarshal.StringToUtf8Ptr (targetProperty);
            var (transformTo_, transformFrom_, notify_, userData_) = UnmangedBindingTransformFuncFactory.CreateNotifyDelegate (transformTo, transformFrom);
            var ret_ = g_object_bind_property_full (handle, sourceProperty_, target.handle, targetProperty_, flags,
                                                    transformTo_, transformFrom_, userData_, notify_);
            var ret = GetInstance<Binding> (ret_, Transfer.None);
            GMarshal.Free (sourceProperty_);
            GMarshal.Free (targetProperty_);
            return ret;
        }

        static class UnmangedBindingTransformFuncFactory
        {
            class BindingTransformFuncData
            {
                public BindingTransformFunc TransformTo;
                public UnmanagedBindingTransformFunc UnmangedTransformTo;
                public BindingTransformFunc TransformFrom;
                public UnmanagedBindingTransformFunc UnmanagedTransformFrom;
                public UnmanagedDestroyNotify UnmanagedNotify;
            }

            public static ValueTuple<UnmanagedBindingTransformFunc, UnmanagedBindingTransformFunc, UnmanagedDestroyNotify, IntPtr>
                CreateNotifyDelegate (BindingTransformFunc transformTo, BindingTransformFunc transformFrom) {
                    var userData = new BindingTransformFuncData();

                    if (transformTo != null) {
                        userData.TransformTo = transformTo;
                        userData.UnmangedTransformTo = TransformToFunc;
                    }

                    if (transformFrom != null) {
                        userData.TransformFrom = transformFrom;
                        userData.UnmanagedTransformFrom = TransformFromFunc;
                    }

                    userData.UnmanagedNotify = UnmanagedNotify;

                    var userData_ = GCHandle.Alloc (userData);

                    return (userData.UnmangedTransformTo, userData.UnmanagedTransformFrom, userData.UnmanagedNotify, (IntPtr)userData_);
                }

            static bool TransformToFunc (IntPtr bindingPtr, ref Value toValue, ref Value fromValue, IntPtr userDataPtr)
            {
                try {
                    var binding = GetInstance<Binding> (bindingPtr, Transfer.None);
                    var gcHandle = (GCHandle)userDataPtr;
                    var userData = (BindingTransformFuncData)gcHandle.Target;
                    var ret = userData.TransformTo (binding, ref toValue, ref fromValue);
                    return ret;
                }
                catch (Exception ex) {
                    ex.LogUnhandledException ();
                    return default(bool);
                }
            }

            static bool TransformFromFunc (IntPtr bindingPtr, ref Value toValue, ref Value fromValue, IntPtr userDataPtr)
            {
                try {
                    var binding = GetInstance<Binding> (bindingPtr, Transfer.None);
                    var gcHandle = (GCHandle)userDataPtr;
                    var userData = (BindingTransformFuncData)gcHandle.Target;
                    var ret = userData.TransformFrom (binding, ref toValue, ref fromValue);
                    return ret;
                }
                catch (Exception ex) {
                    ex.LogUnhandledException ();
                    return default(bool);
                }
            }

            static void UnmanagedNotify(IntPtr userData_)
            {
                try {
                    var gcHandle = (GCHandle)userData_;
                    gcHandle.Free();
                }
                catch (Exception ex) {
                    ex.LogUnhandledException();
                }
            }
        }

        /// <summary>
        /// Increases the freeze count on @object. If the freeze count is
        /// non-zero, the emission of "notify" signals on @object is
        /// stopped. The signals are queued until the freeze count is decreased
        /// to zero. Duplicate notifications are squashed so that at most one
        /// #GObject::notify signal is emitted for each property modified while the
        /// object is frozen.
        /// </summary>
        /// <remarks>
        /// This is necessary for accessors that modify multiple properties to prevent
        /// premature notification while the object is still being modified.
        /// </remarks>
        /// <param name="object">
        /// a #GObject
        /// </param>
        [DllImport ("gobject-2.0", CallingConvention = CallingConvention.Cdecl)]
        /* <type name="none" type="void" managed-name="None" /> */
        /* transfer-ownership:none */
        static extern void g_object_freeze_notify (
            /* <type name="Object" type="GObject*" managed-name="Object" /> */
            /* transfer-ownership:none */
            IntPtr @object);

        /// <summary>
        /// Increases the freeze count on @object. If the freeze count is
        /// non-zero, the emission of "notify" signals on @object is
        /// stopped. The signals are queued until the freeze count is decreased
        /// to zero. Duplicate notifications are squashed so that at most one
        /// #GObject::notify signal is emitted for each property modified while the
        /// object is frozen.
        /// </summary>
        /// <remarks>
        /// This is necessary for accessors that modify multiple properties to prevent
        /// premature notification while the object is still being modified.
        /// </remarks>
        public void FreezeNotify ()
        {
            AssertNotDisposed ();
            g_object_freeze_notify (handle);
        }

        /// <summary>
        /// Gets a property of an object. @value must have been initialized to the
        /// expected type of the property (or a type to which the expected type can be
        /// transformed) using g_value_init().
        /// </summary>
        /// <remarks>
        /// In general, a copy is made of the property contents and the caller is
        /// responsible for freeing the memory by calling g_value_unset().
        ///
        /// Note that g_object_get_property() is really intended for language
        /// bindings, g_object_get() is much more convenient for C programming.
        /// </remarks>
        /// <param name="object">
        /// a #GObject
        /// </param>
        /// <param name="propertyName">
        /// the name of the property to get
        /// </param>
        /// <param name="value">
        /// return location for the property value
        /// </param>
        [DllImport ("gobject-2.0", CallingConvention = CallingConvention.Cdecl)]
        /* <type name="none" type="void" managed-name="None" /> */
        /* transfer-ownership:none */
        static extern void g_object_get_property (
            /* <type name="Object" type="GObject*" managed-name="Object" /> */
            /* transfer-ownership:none */
            IntPtr @object,
            /* <type name="utf8" type="const gchar*" managed-name="Utf8" /> */
            /* transfer-ownership:none */
            IntPtr propertyName,
            /* <type name="Value" type="GValue*" managed-name="Value" /> */
            /* transfer-ownership:none */
            ref Value value);

        /// <summary>
        /// Gets a property of an object.
        /// </summary>
        /// <param name="propertyName">
        /// the GType system name of the property to get
        /// </param>
        /// <returns>
        /// the property value
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Throw when <paramref name="propertyName"/> is <c>null</c>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Throw when <paramref name="propertyName"/> is not a valid property name
        /// </exception>
        public object GetProperty(string propertyName)
        {
            AssertNotDisposed ();
            var pspec = GClass.FindProperty(propertyName);
            if (pspec == null) {
                var message = $"No such property \"{propertyName}\"";
                throw new ArgumentException(message, nameof(propertyName));
            }
            var value = new Value(pspec.ValueType);
            g_object_get_property(handle, pspec.Name.Handle, ref value);
            var ret = value.Get();
            value.Unset();

            return ret;
        }

        /// <summary>
        /// Emits a "notify" signal for the property @property_name on @object.
        /// </summary>
        /// <remarks>
        /// When possible, eg. when signaling a property change from within the class
        /// that registered the property, you should use g_object_notify_by_pspec()
        /// instead.
        ///
        /// Note that emission of the notify signal may be blocked with
        /// g_object_freeze_notify(). In this case, the signal emissions are queued
        /// and will be emitted (in reverse order) when g_object_thaw_notify() is
        /// called.
        /// </remarks>
        /// <param name="object">
        /// a #GObject
        /// </param>
        /// <param name="propertyName">
        /// the name of a property installed on the class of @object.
        /// </param>
        [DllImport ("gobject-2.0", CallingConvention = CallingConvention.Cdecl)]
        /* <type name="none" type="void" managed-name="None" /> */
        /* transfer-ownership:none */
        static extern void g_object_notify (
            /* <type name="Object" type="GObject*" managed-name="Object" /> */
            /* transfer-ownership:none */
            IntPtr @object,
            /* <type name="utf8" type="const gchar*" managed-name="Utf8" /> */
            /* transfer-ownership:none */
            IntPtr propertyName);

        /// <summary>
        /// Emits a "notify" signal for the property @property_name on @object.
        /// </summary>
        /// <remarks>
        /// When possible, eg. when signaling a property change from within the class
        /// that registered the property, you should use g_object_notify_by_pspec()
        /// instead.
        ///
        /// Note that emission of the notify signal may be blocked with
        /// g_object_freeze_notify(). In this case, the signal emissions are queued
        /// and will be emitted (in reverse order) when g_object_thaw_notify() is
        /// called.
        /// </remarks>
        /// <param name="propertyName">
        /// the name of a property installed on the class of @object.
        /// </param>
        public void EmitNotify(string propertyName)
        {
            AssertNotDisposed ();
            if (propertyName == null) {
                throw new ArgumentNullException (nameof (propertyName));
            }
            var propertyName_ = GMarshal.StringToUtf8Ptr (propertyName);
            g_object_notify (handle, propertyName_);
            GMarshal.Free (propertyName_);
        }

        /// <summary>
        /// Emits a "notify" signal for the property specified by @pspec on @object.
        /// </summary>
        /// <remarks>
        /// This function omits the property name lookup, hence it is faster than
        /// g_object_notify().
        ///
        /// One way to avoid using g_object_notify() from within the
        /// class that registered the properties, and using g_object_notify_by_pspec()
        /// instead, is to store the GParamSpec used with
        /// g_object_class_install_property() inside a static array, e.g.:
        ///
        /// |[&lt;!-- language="C" --&gt;
        ///   enum
        ///   {
        ///     PROP_0,
        ///     PROP_FOO,
        ///     PROP_LAST
        ///   };
        ///
        ///   static GParamSpec *properties[PROP_LAST];
        ///
        ///   static void
        ///   my_object_class_init (MyObjectClass *klass)
        ///   {
        ///     properties[PROP_FOO] = g_param_spec_int ("foo", "Foo", "The foo",
        ///                                              0, 100,
        ///                                              50,
        ///                                              G_PARAM_READWRITE);
        ///     g_object_class_install_property (gobject_class,
        ///                                      PROP_FOO,
        ///                                      properties[PROP_FOO]);
        ///   }
        /// ]|
        ///
        /// and then notify a change on the "foo" property with:
        ///
        /// |[&lt;!-- language="C" --&gt;
        ///   g_object_notify_by_pspec (self, properties[PROP_FOO]);
        /// ]|
        /// </remarks>
        /// <param name="object">
        /// a #GObject
        /// </param>
        /// <param name="pspec">
        /// the #GParamSpec of a property installed on the class of @object.
        /// </param>
        [Since ("2.26")]
        [DllImport ("gobject-2.0", CallingConvention = CallingConvention.Cdecl)]
        /* <type name="none" type="void" managed-name="None" /> */
        /* transfer-ownership:none */
        static extern void g_object_notify_by_pspec (
            /* <type name="Object" type="GObject*" managed-name="Object" /> */
            /* transfer-ownership:none */
            IntPtr @object,
            /* <type name="ParamSpec" type="GParamSpec*" managed-name="ParamSpec" /> */
            /* transfer-ownership:none */
            IntPtr pspec);

        /// <summary>
        /// Emits a "notify" signal for the property specified by @pspec on @object.
        /// </summary>
        /// <remarks>
        /// This function omits the property name lookup, hence it is faster than
        /// g_object_notify().
        ///
        /// One way to avoid using g_object_notify() from within the
        /// class that registered the properties, and using g_object_notify_by_pspec()
        /// instead, is to store the GParamSpec used with
        /// g_object_class_install_property() inside a static array, e.g.:
        ///
        /// |[&lt;!-- language="C" --&gt;
        ///   enum
        ///   {
        ///     PROP_0,
        ///     PROP_FOO,
        ///     PROP_LAST
        ///   };
        ///
        ///   static GParamSpec *properties[PROP_LAST];
        ///
        ///   static void
        ///   my_object_class_init (MyObjectClass *klass)
        ///   {
        ///     properties[PROP_FOO] = g_param_spec_int ("foo", "Foo", "The foo",
        ///                                              0, 100,
        ///                                              50,
        ///                                              G_PARAM_READWRITE);
        ///     g_object_class_install_property (gobject_class,
        ///                                      PROP_FOO,
        ///                                      properties[PROP_FOO]);
        ///   }
        /// ]|
        ///
        /// and then notify a change on the "foo" property with:
        ///
        /// |[&lt;!-- language="C" --&gt;
        ///   g_object_notify_by_pspec (self, properties[PROP_FOO]);
        /// ]|
        /// </remarks>
        /// <param name="pspec">
        /// the #GParamSpec of a property installed on the class of @object.
        /// </param>
        [Since ("2.26")]
        public void EmitNotify(ParamSpec pspec)
        {
            AssertNotDisposed ();
            if (pspec == null) {
                throw new ArgumentNullException (nameof (pspec));
            }
            g_object_notify_by_pspec (handle, pspec.Handle);
            GC.KeepAlive (pspec);
        }

        /// <summary>
        /// Sets a property on an object.
        /// </summary>
        /// <param name="object">
        /// a #GObject
        /// </param>
        /// <param name="propertyName">
        /// the name of the property to set
        /// </param>
        /// <param name="value">
        /// the value
        /// </param>
        [DllImport ("gobject-2.0", CallingConvention = CallingConvention.Cdecl)]
        /* <type name="none" type="void" managed-name="None" /> */
        /* transfer-ownership:none */
        static extern void g_object_set_property (
            /* <type name="Object" type="GObject*" managed-name="Object" /> */
            /* transfer-ownership:none */
            IntPtr @object,
            /* <type name="utf8" type="const gchar*" managed-name="Utf8" /> */
            /* transfer-ownership:none */
            IntPtr propertyName,
            /* <type name="Value" type="const GValue*" managed-name="Value" /> */
            /* transfer-ownership:none */
            ref Value value);

        /// <summary>
        /// Sets a property on an object.
        /// </summary>
        /// <param name="propertyName">
        /// the name of the property to set
        /// </param>
        /// <param name="value">
        /// the value
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Throw when <paramref name="propertyName"/> is <c>null</c>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Throw when <paramref name="propertyName"/> is not a valid property name
        /// </exception>
        public void SetProperty(string propertyName, object value)
        {
            AssertNotDisposed();
            var pspec = GClass.FindProperty(propertyName);
            if (pspec == null) {
                var message = $"No such property \"{propertyName}\"";
                throw new ArgumentException(message, nameof(propertyName));
            }
            var value_ = new Value(pspec.ValueType);
            value_.Set(value);
            g_object_set_property(handle, pspec.Name.Handle, ref value_);
            value_.Unset();
        }

        /// <summary>
        /// Reverts the effect of a previous call to
        /// g_object_freeze_notify(). The freeze count is decreased on @object
        /// and when it reaches zero, queued "notify" signals are emitted.
        /// </summary>
        /// <remarks>
        /// Duplicate notifications for each property are squashed so that at most one
        /// #GObject::notify signal is emitted for each property, in the reverse order
        /// in which they have been queued.
        ///
        /// It is an error to call this function when the freeze count is zero.
        /// </remarks>
        /// <param name="object">
        /// a #GObject
        /// </param>
        [DllImport ("gobject-2.0", CallingConvention = CallingConvention.Cdecl)]
        /* <type name="none" type="void" managed-name="None" /> */
        /* transfer-ownership:none */
        static extern void g_object_thaw_notify (
            /* <type name="Object" type="GObject*" managed-name="Object" /> */
            /* transfer-ownership:none */
            IntPtr @object);

        /// <summary>
        /// Reverts the effect of a previous call to
        /// g_object_freeze_notify(). The freeze count is decreased on @object
        /// and when it reaches zero, queued "notify" signals are emitted.
        /// </summary>
        /// <remarks>
        /// Duplicate notifications for each property are squashed so that at most one
        /// #GObject::notify signal is emitted for each property, in the reverse order
        /// in which they have been queued.
        ///
        /// It is an error to call this function when the freeze count is zero.
        /// </remarks>
        public void ThawNotify ()
        {
            AssertNotDisposed ();
            g_object_thaw_notify (handle);
        }

        [DllImport ("gobject-2.0", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr g_object_get_data (
            IntPtr @object,
            IntPtr key);

        [DllImport ("gobject-2.0", CallingConvention = CallingConvention.Cdecl)]
        static extern void g_object_set_data (
            IntPtr @object,
            IntPtr key,
            IntPtr data);

        [DllImport ("gobject-2.0", CallingConvention = CallingConvention.Cdecl)]
        static extern void g_object_set_data_full (
            IntPtr @object,
            IntPtr key,
            IntPtr data,
            UnmanagedDestroyNotify destroy);

        public object this[string key] {
            get {
                AssertNotDisposed ();
                var key_ = GMarshal.StringToUtf8Ptr (key);
                var data_ = g_object_get_data (handle, key_);
                GMarshal.Free (key_);
                if (data_ == IntPtr.Zero) {
                    return null;
                }
                var data = GCHandle.FromIntPtr (data_).Target;
                return data;
            }
            set {
                AssertNotDisposed ();
                var key_ = GMarshal.StringToUtf8Ptr (key);
                if (value == null) {
                    g_object_set_data (handle, key_, IntPtr.Zero);
                }
                else {
                    var data_ = GCHandle.ToIntPtr (GCHandle.Alloc (value));
                    g_object_set_data_full (handle, key_, data_, freeDataDelegate);
                }
                GMarshal.Free (key_);
            }
        }

        static UnmanagedDestroyNotify freeDataDelegate = FreeData;

        static void FreeData (IntPtr dataPtr)
        {
            try {
                var data = GCHandle.FromIntPtr (dataPtr);
                data.Free ();
            }
            catch (Exception ex) {
                ex.LogUnhandledException ();
            }
        }

        [DllImport ("gobject-2.0", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr g_object_get_qdata (
            IntPtr @object,
            Quark quark);

        [DllImport ("gobject-2.0", CallingConvention = CallingConvention.Cdecl)]
        static extern void g_object_set_qdata (
            IntPtr @object,
            Quark quark,
            IntPtr data);

        [DllImport ("gobject-2.0", CallingConvention = CallingConvention.Cdecl)]
        static extern void g_object_set_qdata_full (
            IntPtr @object,
            Quark quark,
            IntPtr data,
            UnmanagedDestroyNotify destroy);

        /// <summary>
        /// Gets a managed proxy for a an unmanged GObject.
        /// </summary>
        /// <param name="handle">
        /// The pointer to the unmanaged instance
        /// </param>
        /// <param name="ownership">
        /// Indicates if we already have a reference to the unmanged instance
        /// or not.
        /// </param>
        /// <returns>
        /// A managed proxy instance
        /// </returns>
        /// <remarks>
        /// This method tries to get an existing managed proxy instance by
        /// looking for a GC handle attached to the unmanaged instance (using
        /// QData). If one is found, it returns the existing managed instance,
        /// otherwise a new instance is created.
        /// </remarks>
        public static new T GetInstance<T>(IntPtr handle, Transfer ownership) where T : Object
        {
            if (handle == IntPtr.Zero) {
                return null;
            }

            // see if the unmanaged object has a managed GC handle
            var ptr = g_object_get_qdata(handle, toggleRefGCHandleQuark);
            if (ptr != IntPtr.Zero) {
                var gcHandle = (GCHandle)ptr;
                if (gcHandle.IsAllocated) {
                    // the GC handle looks good, so we should have the managed
                    // proxy for the unmanged object here
                    var target = (Object)gcHandle.Target;
                    // make sure the managed object has not been disposed
                    if (target.handle == handle) {
                        // release the extra reference, if there is one
                        if (ownership != Transfer.None) {
                            g_object_unref(handle);
                        }
                        // return the existing managed proxy
                        return (T)(object)target;
                    }
                }
            }

            // if we get here, that means that there wasn't a viable existing
            // proxy, so we need to create a new managed instance

            // get the exact type of the object
            ptr = Marshal.ReadIntPtr(handle);
            var gtype = Marshal.PtrToStructure<GType>(ptr);
            var type = GType.TypeOf(gtype);

            return (T)Activator.CreateInstance(type, handle, ownership);
        }

        /// <summary>
        /// Gets a managed proxy for a an unmanged GObject.
        /// </summary>
        /// <seealso cref="GetInstance{T}"/>
        public static Object GetInstance(IntPtr handle, Transfer ownership)
        {
            return GetInstance<Object>(handle, ownership);
        }
    }

    /// <summary>
    /// A callback function used for notification when the state
    /// of a toggle reference changes. See g_object_add_toggle_ref().
    /// </summary>
    [UnmanagedFunctionPointer (CallingConvention.Cdecl)]
    delegate void UnmanagedToggleNotify (
        /* <type name="gpointer" type="gpointer" managed-name="Gpointer" /> */
        /* transfer-ownership:none */
        IntPtr data,
        /* <type name="Object" type="GObject*" managed-name="Object" /> */
        /* transfer-ownership:none */
        IntPtr @object,
        /* <type name="gboolean" type="gboolean" managed-name="Gboolean" /> */
        /* transfer-ownership:none */
        bool isLastRef);

    /// <summary>
    /// The GParameter struct is an auxiliary structure used
    /// to hand parameter name/value pairs to g_object_newv().
    /// </summary>
    struct Parameter
    {
        #pragma warning disable CS0649
        /// <summary>
        /// the parameter name
        /// </summary>
        public IntPtr Name;

        /// <summary>
        /// the parameter value
        /// </summary>
        public IntPtr Value;
        #pragma warning restore CS0649
    }
}
