using System;
using System.Runtime.InteropServices;
using GISharp.GObject;
using GISharp.Runtime;
using System.Threading.Tasks;

// using GNetworkMonitor for testing since it is in gio, which is likely to
// be installed and it has methods, properties and a signal as well as having
// GInitable as a prerequisite.
using GISharp.GLib;

namespace GISharp.Core.Test
{
    [GType ("GInitable", IsWrappedUnmanagedType = true)]
    [GTypeStruct (typeof(InitableIface))]
    [GTypePrerequisite (typeof(GISharp.GObject.Object))]
    public interface IInitable
    {
        bool Init (IntPtr cancellable);
    }

    sealed class InitableIface : TypeInterface
    {
        static readonly IntPtr initOffset = Marshal.OffsetOf<Struct> (nameof (Struct.Init));
        static readonly Struct.UnmanagedInit initDelegate = UnmanagedInit;
        static readonly IntPtr initPtr = Marshal.GetFunctionPointerForDelegate (initDelegate);
        
        new struct Struct
        {
            #pragma warning disable CS0649
            public TypeInterface.Struct GIface;
            public UnmanagedInit Init;
            #pragma warning restore CS0649

            public delegate bool UnmanagedInit (IntPtr initablePtr, IntPtr cancellablePtr, ref IntPtr errorPtr);
        }

        public override Type StructType => typeof(Struct);

        static void InterfaceInit (IntPtr gIface, IntPtr userData)
        {
            try {
                Marshal.WriteIntPtr (gIface, (int)initOffset, initPtr);
            }
            catch (Exception ex) {
                ex.DumpUnhandledException ();
            }
        }

        static void InterfaceFinalize (IntPtr gIface, IntPtr userData)
        {
        }

        public override InterfaceInfo CreateInterfaceInfo (Type instanceType)
        {
            var ret = new InterfaceInfo {
                InterfaceInit = InterfaceInit,
                InterfaceFinalize = InterfaceFinalize,
            };

            return ret;
        }

        static bool UnmanagedInit (IntPtr initablePtr, IntPtr cancellablePtr, ref IntPtr errorPtr)
        {
            try {
                var initable = (IInitable)GetInstance<GISharp.GObject.Object> (initablePtr, Transfer.None);
                var ret = initable.Init (cancellablePtr);
                return ret;
            }
            catch (GErrorException ex) {
                GMarshal.PropagateError (errorPtr, ex.Error);
            }
            catch (Exception ex) {
                // FIXME: we should convert managed exception to GError
                ex.DumpUnhandledException ();
            }
            return false;
        }

        public InitableIface (IntPtr handle, Transfer ownership) : base (handle, ownership)
        {
        }
    }

    public static class Initable
    {
        [DllImport ("gio-2.0", CallingConvention = CallingConvention.Cdecl)]
        static extern GType g_initable_get_type ();

        static GType getGType ()
        {
            return g_initable_get_type ();
        }

        [DllImport ("gio-2.0", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr g_initable_newv (GType objectType, uint nParameters, IntPtr parameters, IntPtr cancellable, out IntPtr errorPtr);

        public static GISharp.GObject.Object New (GType objectType, params object[] parameters)
        {
            IntPtr errorPtr;
            var ret_ = g_initable_newv (objectType, 0, IntPtr.Zero, IntPtr.Zero, out errorPtr);
            if (errorPtr != IntPtr.Zero) {
                var error = Opaque.GetInstance<Error> (errorPtr, Transfer.Full);
                throw new GErrorException (error);
            }
            var ret = Opaque.GetInstance<GISharp.GObject.Object> (ret_, Transfer.Full);

            return ret;
        }

        [DllImport ("gio-2.0", CallingConvention = CallingConvention.Cdecl)]
        static extern bool g_initable_init (IntPtr initable, IntPtr cancellable, out IntPtr errorPtr);

        public static bool Init (this IInitable initable)
        {
            if (initable == null) {
                throw new ArgumentNullException (nameof(initable));
            }
            var instance = (GISharp.GObject.Object)initable;
            var ret = g_initable_init (instance.Handle, IntPtr.Zero, out var errorPtr);
            GC.KeepAlive (instance);
            if (errorPtr != IntPtr.Zero) {
                var error = Opaque.GetInstance<Error> (errorPtr, Transfer.Full);
                throw new GErrorException (error);
            }
            return ret;
        }
    }

    [GType ("GNetworkMonitor", IsWrappedUnmanagedType = true)]
    [GTypeStruct (typeof(NetworkMonitorInterface))]
    public interface INetworkMonitor : IInitable
    {
        bool CanReach (IntPtr connectable, IntPtr cancellable);
        void CanReachAsync (IntPtr connectable, IntPtr cancellable, Action<IntPtr> callback);
        bool CanReachFinish (IntPtr result);
        void OnNetworkChanged (bool available);

        [GTypeProperty ("connectivity")]
        int Connectivity { get; }
        [GTypeProperty ("network-available")]
        bool NetworkAvailable { get; }
        [GTypeProperty ("network-metered")]
        bool NetworkMetered { get; }

        [GTypeSignal ("network-changed")]
        event Action<bool> NetworkChanged;
    }

    sealed class NetworkMonitorInterface : TypeInterface
    {
        static readonly IntPtr networkChangedOffset = Marshal.OffsetOf<Struct> (nameof (Struct.NetworkChanged));
        static readonly Struct.UnmanagedNetworkChanged networkChangedDelegate = UnmanagedNetworkChanged;
        static readonly IntPtr networkChangedPtr = Marshal.GetFunctionPointerForDelegate (networkChangedDelegate);
        static readonly IntPtr canReachOffset = Marshal.OffsetOf<Struct> (nameof (Struct.CanReach));
        static readonly Struct.UnmanagedCanReach canReachDelegate = UnmanagedCanReach;
        static readonly IntPtr canReachPtr = Marshal.GetFunctionPointerForDelegate (canReachDelegate);
        static readonly IntPtr canReachAsyncOffset = Marshal.OffsetOf<Struct> (nameof (Struct.CanReachAsync));
        static readonly Struct.UnmanagedCanReachAsync canReachAsyncDelegate = UnmanagedCanReachAsync;
        static readonly IntPtr canReachAsyncPtr = Marshal.GetFunctionPointerForDelegate (canReachAsyncDelegate);
        static readonly IntPtr canReachAsyncFinishOffset = Marshal.OffsetOf<Struct> (nameof (Struct.CanReachAsyncFinish));
        static readonly Struct.UnmanagedCanReachAsyncFinish canReachAsyncFinishDelegate = UnmanagedCanReachAsyncFinish;
        static readonly IntPtr canReachAsyncFinishPtr = Marshal.GetFunctionPointerForDelegate (canReachAsyncFinishDelegate);

        new struct Struct
        {
#pragma warning disable CS0649
            public TypeInterface.Struct GIface;
            public UnmanagedNetworkChanged NetworkChanged;
            public UnmanagedCanReach CanReach;
            public UnmanagedCanReachAsync CanReachAsync;
            public UnmanagedCanReachAsyncFinish CanReachAsyncFinish;
#pragma warning restore CS0649

            [UnmanagedFunctionPointer (CallingConvention.Cdecl)]
            public delegate void UnmanagedNetworkChanged (IntPtr monitorPtr, bool available);
            [UnmanagedFunctionPointer (CallingConvention.Cdecl)]
            public delegate bool UnmanagedCanReach (IntPtr monitorPtr, IntPtr connectablePtr, IntPtr cancellablePtr, ref IntPtr errorPtr);
            [UnmanagedFunctionPointer (CallingConvention.Cdecl)]
            public delegate void UnmanagedCanReachAsync (IntPtr monitorPtr, IntPtr connectablePtr, IntPtr cancellablePtr, Action<IntPtr, IntPtr, IntPtr> callback, IntPtr userData);
            [UnmanagedFunctionPointer (CallingConvention.Cdecl)]
            public delegate void UnmanagedCanReachAsyncFinish (IntPtr monitorPtr, IntPtr result, ref IntPtr errorPtr);
        }

        public override Type StructType => typeof(Struct);

        static void InterfaceInit (IntPtr gIface, IntPtr userData)
        {
            try {
                Marshal.WriteIntPtr (gIface, (int)networkChangedOffset, networkChangedPtr);
                Marshal.WriteIntPtr (gIface, (int)canReachOffset, canReachPtr);
                Marshal.WriteIntPtr (gIface, (int)canReachAsyncOffset, canReachAsyncPtr);
                Marshal.WriteIntPtr (gIface, (int)canReachAsyncFinishOffset, canReachAsyncFinishPtr);
            }
            catch (Exception ex) {
                ex.DumpUnhandledException ();
            }
        }

        static void InterfaceFinalize (IntPtr gIface, IntPtr userData)
        {
        }

        public override InterfaceInfo CreateInterfaceInfo (Type instanceType)
        {
            var ret = new InterfaceInfo {
                InterfaceInit = InterfaceInit,
                InterfaceFinalize = InterfaceFinalize,
            };

            return ret;
        }

        static void UnmanagedNetworkChanged (IntPtr monitorPtr, bool available)
        {
            try {
                var monitor = (INetworkMonitor)Opaque.GetInstance<GISharp.GObject.Object> (monitorPtr, Transfer.None);
                monitor.OnNetworkChanged (available);
            }
            catch (Exception ex) {
                ex.DumpUnhandledException ();
            }
        }

        static bool UnmanagedCanReach (IntPtr monitorPtr, IntPtr connectablePtr, IntPtr cancellablePtr, ref IntPtr errorPtr)
        {
            try {
                var monitor = (INetworkMonitor)Opaque.GetInstance<GISharp.GObject.Object> (monitorPtr, Transfer.None);
                var ret = monitor.CanReach (connectablePtr, cancellablePtr);
                return ret;
            }
            catch (GErrorException ex) {
                GMarshal.PropagateError (errorPtr, ex.Error);
            }
            catch (Exception ex) {
                // FIXME: convert managed exception to GError
                ex.DumpUnhandledException ();
            }
            return false;
        }

        static void UnmanagedCanReachAsync (IntPtr monitorPtr, IntPtr connectablePtr, IntPtr cancellablePtr, Action<IntPtr, IntPtr, IntPtr> callback, IntPtr userData)
        {
            try {
                var monitor = (INetworkMonitor)Opaque.GetInstance<GISharp.GObject.Object> (monitorPtr, Transfer.None);
                Action<IntPtr> managedCallback = (result) => {
                    callback (monitorPtr, result, userData);
                };
                monitor.CanReachAsync (connectablePtr, cancellablePtr, managedCallback);
            }
            catch (Exception ex) {
                ex.DumpUnhandledException ();
            }
        }

        static void UnmanagedCanReachAsyncFinish (IntPtr monitorPtr, IntPtr result, ref IntPtr errorPtr)
        {
            try {
                var monitor = (INetworkMonitor)Opaque.GetInstance<GISharp.GObject.Object> (monitorPtr, Transfer.None);
                monitor.CanReachFinish (result);
            } catch (GErrorException ex) {
                GMarshal.PropagateError (errorPtr, ex.Error);
            }
            catch (Exception ex) {
                // FIXME: convert managed exception to GError
                ex.DumpUnhandledException ();
            }
        }

        public NetworkMonitorInterface (IntPtr handle, Transfer ownership) : base (handle, ownership)
        {
        }
    }

    public static class NetworkMonitor
    {
        [DllImport ("gio-2.0", CallingConvention = CallingConvention.Cdecl)]
        static extern GType g_network_monitor_get_type ();

        static GType getGType ()
        {
            return g_network_monitor_get_type ();
        }

        [DllImport ("gio-2.0", CallingConvention = CallingConvention.Cdecl)]
        static extern bool g_network_monitor_get_network_available (IntPtr monitor);

        public static bool GetNetworkAvailable (this INetworkMonitor monitor)
        {
            if (monitor == null) {
                throw new ArgumentNullException (nameof(monitor));
            }
            var instance = (GISharp.GObject.Object)monitor;
            var ret = g_network_monitor_get_network_available (instance.Handle);
            GC.KeepAlive (instance);

            return ret;
        }

        [DllImport ("gio-2.0", CallingConvention = CallingConvention.Cdecl)]
        static extern bool g_network_monitor_get_network_metered (IntPtr monitor);

        public static bool GetNetworkMetered (this INetworkMonitor monitor)
        {
            if (monitor == null) {
                throw new ArgumentNullException (nameof(monitor));
            }
            var instance = (GISharp.GObject.Object)monitor;
            var ret = g_network_monitor_get_network_metered (instance.Handle);
            GC.KeepAlive (instance);

            return ret;
        }

        [DllImport ("gio-2.0", CallingConvention = CallingConvention.Cdecl)]
        static extern bool g_network_monitor_can_reach (IntPtr monitor, IntPtr connectable, IntPtr cancellable, out IntPtr errorPtr);

        public static bool CanReach (this INetworkMonitor monitor, IntPtr connectable, IntPtr cancellable = default(IntPtr))
        {
            if (monitor == null) {
                throw new ArgumentNullException (nameof(monitor));
            }
            var instance = (GISharp.GObject.Object)monitor;
            var ret = g_network_monitor_can_reach (instance.Handle, connectable, cancellable, out var errorPtr);
            GC.KeepAlive (instance);
            if (errorPtr != IntPtr.Zero) {
                var error = Opaque.GetInstance<Error> (errorPtr, Transfer.Full);
                throw new GErrorException (error);
            }

            return ret;
        }

        [DllImport ("gio-2.0", CallingConvention = CallingConvention.Cdecl)]
        static extern void g_network_monitor_can_reach_async (IntPtr monitor, IntPtr connectable, IntPtr cancellable, Action<IntPtr, IntPtr, IntPtr> callback, IntPtr userData);

        [DllImport ("gio-2.0", CallingConvention = CallingConvention.Cdecl)]
        static extern bool g_network_monitor_can_reach_async_finish (IntPtr monitor, IntPtr result, out IntPtr errorPtr);

        public static Task<bool> CanReachAsync (this INetworkMonitor monitor, IntPtr connectable, IntPtr cancellable = default(IntPtr))
        {
            if (monitor == null) {
                throw new ArgumentNullException (nameof(monitor));
            }
            var instance = (GISharp.GObject.Object)monitor;
            var completion = new TaskCompletionSource<bool> ();
            Action<IntPtr, IntPtr, IntPtr> nativeCallback = (sourceObjectPtr, resultPtr, userDataPtr) => {
                IntPtr errorPtr;
                var ret = g_network_monitor_can_reach_async_finish (sourceObjectPtr, resultPtr, out errorPtr);
                if (errorPtr != IntPtr.Zero) {
                    var error = Opaque.GetInstance<Error> (errorPtr, Transfer.Full);
                    completion.SetException (new GErrorException (error));
                } else {
                    completion.SetResult (ret);
                }
                GCHandle.FromIntPtr (userDataPtr).Free ();
            };
            var callbackGCHandle = GCHandle.Alloc (nativeCallback);
            g_network_monitor_can_reach_async (instance.Handle, connectable, cancellable, nativeCallback, (IntPtr)callbackGCHandle);
            GC.KeepAlive (instance);

            return completion.Task;
        }
    }
}
