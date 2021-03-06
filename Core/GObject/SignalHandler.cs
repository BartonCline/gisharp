using System;
using System.Runtime.InteropServices;

using GISharp.GLib;

using nulong = GISharp.Runtime.NativeULong;

namespace GISharp.GObject
{
    public class SignalHandler
    {
        readonly Object instance;
        readonly nulong handlerId;

        internal SignalHandler (Object instance, nulong handlerId)
        {
            if (instance == null) {
                throw new ArgumentNullException (nameof(instance));
            }

            this.instance = instance;
            this.handlerId = handlerId;
        }

        /// <summary>
        /// Disconnects a handler from an instance so it will not be called during
        /// any future or currently ongoing emissions of the signal it has been
        /// connected to. The @handler_id becomes invalid and may be reused.
        /// </summary>
        /// <remarks>
        /// The @handler_id has to be a valid signal handler id, connected to a
        /// signal of @instance.
        /// </remarks>
        /// <param name="instance">
        /// The instance to remove the signal handler from.
        /// </param>
        /// <param name="handlerId">
        /// Handler id of the handler to be disconnected.
        /// </param>
        [DllImport ("gobject-2.0", CallingConvention = CallingConvention.Cdecl)]
        /* <type name="none" type="void" managed-name="None" /> */
        /* transfer-ownership:none */
        static extern void g_signal_handler_disconnect (
            /* <type name="Object" type="gpointer" managed-name="Object" /> */
            /* transfer-ownership:none */
            IntPtr instance,
            /* <type name="gulong" type="gulong" managed-name="Gulong" /> */
            /* transfer-ownership:none */
            nulong handlerId);

        /// <summary>
        /// Disconnects a signal handler.
        /// </summary>
        public void Disconnect ()
        {
            g_signal_handler_disconnect (instance.Handle, handlerId);
        }

        /// <summary>
        /// Finds the first signal handler that matches certain selection criteria.
        /// The criteria mask is passed as an OR-ed combination of #GSignalMatchType
        /// flags, and the criteria values are passed as arguments.
        /// The match @mask has to be non-0 for successful matches.
        /// If no handler was found, 0 is returned.
        /// </summary>
        /// <param name="instance">
        /// The instance owning the signal handler to be found.
        /// </param>
        /// <param name="mask">
        /// Mask indicating which of @signal_id, @detail, @closure, @func
        ///  and/or @data the handler has to match.
        /// </param>
        /// <param name="signalId">
        /// Signal the handler has to be connected to.
        /// </param>
        /// <param name="detail">
        /// Signal detail the handler has to be connected to.
        /// </param>
        /// <param name="closure">
        /// The closure the handler will invoke.
        /// </param>
        /// <param name="func">
        /// The C closure callback of the handler (useless for non-C closures).
        /// </param>
        /// <param name="data">
        /// The closure data of the handler's closure.
        /// </param>
        /// <returns>
        /// A valid non-0 signal handler id for a successful match.
        /// </returns>
        [DllImport ("gobject-2.0", CallingConvention = CallingConvention.Cdecl)]
        /* <type name="gulong" type="gulong" managed-name="Gulong" /> */
        /* transfer-ownership:none */
        static extern nulong g_signal_handler_find (
            /* <type name="Object" type="gpointer" managed-name="Object" /> */
            /* transfer-ownership:none */
            IntPtr instance,
            /* <type name="SignalMatchType" type="GSignalMatchType" managed-name="SignalMatchType" /> */
            /* transfer-ownership:none */
            SignalMatchType mask,
            /* <type name="guint" type="guint" managed-name="Guint" /> */
            /* transfer-ownership:none */
            uint signalId,
            /* <type name="GLib.Quark" type="GQuark" managed-name="GLib.Quark" /> */
            /* transfer-ownership:none */
            Quark detail,
            /* <type name="Closure" type="GClosure*" managed-name="Closure" /> */
            /* transfer-ownership:none nullable:1 allow-none:1 */
            IntPtr closure,
            /* <type name="gpointer" type="gpointer" managed-name="Gpointer" /> */
            /* transfer-ownership:none */
            IntPtr func,
            /* <type name="gpointer" type="gpointer" managed-name="Gpointer" /> */
            /* transfer-ownership:none */
            IntPtr data);

        /// <summary>
        /// Finds the first signal handler that matches certain selection criteria.
        /// The criteria mask is passed as an OR-ed combination of #GSignalMatchType
        /// flags, and the criteria values are passed as arguments.
        /// The match @mask has to be non-0 for successful matches.
        /// If no handler was found, 0 is returned.
        /// </summary>
        /// <param name="instance">
        /// The instance owning the signal handler to be found.
        /// </param>
        /// <param name="mask">
        /// Mask indicating which of @signal_id, @detail, @closure, @func
        ///  and/or @data the handler has to match.
        /// </param>
        /// <param name="signalId">
        /// Signal the handler has to be connected to.
        /// </param>
        /// <param name="detail">
        /// Signal detail the handler has to be connected to.
        /// </param>
        /// <param name="closure">
        /// The closure the handler will invoke.
        /// </param>
        /// <param name="func">
        /// The C closure callback of the handler (useless for non-C closures).
        /// </param>
        /// <param name="data">
        /// The closure data of the handler's closure.
        /// </param>
        /// <returns>
        /// A valid non-0 signal handler id for a successful match.
        /// </returns>
//        public static SignalHandler Find (Object instance, SignalMatchType mask, uint signalId, Quark detail, GISharp.GObject.Closure closure, IntPtr func, IntPtr data)
//        {
//            if (instance == null) {
//                throw new System.ArgumentNullException ("instance");
//            }
//            var instance_ = instance == null ? IntPtr.Zero : instance.Handle;
//            var closure_ = closure == null ? IntPtr.Zero : closure.Handle;
//            var ret = g_signal_handler_find (instance_, mask, signalId, detail, closure_, func, data);
//            return ret;
//        }

        /// <summary>
        /// Returns whether @handler_id is the id of a handler connected to @instance.
        /// </summary>
        /// <param name="instance">
        /// The instance where a signal handler is sought.
        /// </param>
        /// <param name="handlerId">
        /// the handler id.
        /// </param>
        /// <returns>
        /// whether @handler_id identifies a handler connected to @instance.
        /// </returns>
        [DllImport ("gobject-2.0", CallingConvention = CallingConvention.Cdecl)]
        /* <type name="gboolean" type="gboolean" managed-name="Gboolean" /> */
        /* transfer-ownership:none */
        static extern bool g_signal_handler_is_connected (
            /* <type name="Object" type="gpointer" managed-name="Object" /> */
            /* transfer-ownership:none */
            IntPtr instance,
            /* <type name="gulong" type="gulong" managed-name="Gulong" /> */
            /* transfer-ownership:none */
            nulong handlerId);

        /// <summary>
        /// Returns whether @handler_id is the id of a handler connected to @instance.
        /// </summary>
        /// <returns>
        /// whether @handler_id identifies a handler connected to @instance.
        /// </returns>
        public bool IsConnected {
            get {
                return g_signal_handler_is_connected (instance.Handle, handlerId);
            }
        }

        /// <summary>
        /// Undoes the effect of a previous g_signal_handler_block() call.  A
        /// blocked handler is skipped during signal emissions and will not be
        /// invoked, unblocking it (for exactly the amount of times it has been
        /// blocked before) reverts its "blocked" state, so the handler will be
        /// recognized by the signal system and is called upon future or
        /// currently ongoing signal emissions (since the order in which
        /// handlers are called during signal emissions is deterministic,
        /// whether the unblocked handler in question is called as part of a
        /// currently ongoing emission depends on how far that emission has
        /// proceeded yet).
        /// </summary>
        /// <remarks>
        /// The @handler_id has to be a valid id of a signal handler that is
        /// connected to a signal of @instance and is currently blocked.
        /// </remarks>
        /// <param name="instance">
        /// The instance to unblock the signal handler of.
        /// </param>
        /// <param name="handlerId">
        /// Handler id of the handler to be unblocked.
        /// </param>
        [DllImport ("gobject-2.0", CallingConvention = CallingConvention.Cdecl)]
        /* <type name="none" type="void" managed-name="None" /> */
        /* transfer-ownership:none */
        static extern void g_signal_handler_unblock (
            /* <type name="Object" type="gpointer" managed-name="Object" /> */
            /* transfer-ownership:none */
            IntPtr instance,
            /* <type name="gulong" type="gulong" managed-name="Gulong" /> */
            /* transfer-ownership:none */
            nulong handlerId);

        /// <summary>
        /// Undoes the effect of a previous g_signal_handler_block() call.  A
        /// blocked handler is skipped during signal emissions and will not be
        /// invoked, unblocking it (for exactly the amount of times it has been
        /// blocked before) reverts its "blocked" state, so the handler will be
        /// recognized by the signal system and is called upon future or
        /// currently ongoing signal emissions (since the order in which
        /// handlers are called during signal emissions is deterministic,
        /// whether the unblocked handler in question is called as part of a
        /// currently ongoing emission depends on how far that emission has
        /// proceeded yet).
        /// </summary>
        /// <remarks>
        /// The @handler_id has to be a valid id of a signal handler that is
        /// connected to a signal of @instance and is currently blocked.
        /// </remarks>
        public void Unblock ()
        {
            g_signal_handler_unblock (instance.Handle, handlerId);
        }
    }
}
