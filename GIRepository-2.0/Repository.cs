// This file was originally generated by the Gtk# (gapi3) code generator.
// It is now maintained by hand.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using GISharp.Runtime;
using GISharp.GLib;
using GISharp.GObject;

namespace GISharp.GIRepository
{
    public static class Repository
    {
        static NamespaceCollection namespaces;

        internal static InfoDictionary<BaseInfo> GetInfos (string @namespace)
        {
            return new InfoDictionary<BaseInfo> (GetNInfos (@namespace), (i) => GetInfo (@namespace, i));
        }

        public static NamespaceCollection Namespaces {
            get {
                if (namespaces == null)
                    namespaces = new NamespaceCollection ();
                return namespaces;
            }
        }

        [DllImport ("libgirepository-1.0", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr g_irepository_get_default ();

        [DllImport ("libgirepository-1.0", CallingConvention = CallingConvention.Cdecl)]
        static extern bool g_irepository_dump (IntPtr arg, out IntPtr error);

        public static void Dump (string arg)
        {
            IntPtr native_arg = GMarshal.StringToUtf8Ptr (arg);
            IntPtr error_ = IntPtr.Zero;
            g_irepository_dump (native_arg, out error_);
            GMarshal.Free (native_arg);
            if (error_ != IntPtr.Zero) {
                var error = new Error.SafeHandle (error_, Runtime.Transfer.Full);
                throw new GErrorException (error);
            }
        }

        [DllImport ("libgirepository-1.0", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr g_irepository_enumerate_versions (IntPtr raw, IntPtr @namespace);

        internal static string[] GetVersions (string @namespace)
        {
            IntPtr native_namespace = GMarshal.StringToUtf8Ptr (@namespace);
            IntPtr raw_ret = g_irepository_enumerate_versions (IntPtr.Zero, native_namespace);
            var ret = GMarshal.GListToStringArray (raw_ret, freePtr: true);
            GMarshal.Free (native_namespace);
            return ret;
        }

        [DllImport ("libgirepository-1.0", CallingConvention = CallingConvention.Cdecl)]
        static extern Quark g_irepository_error_quark ();

        /// <summary>
        /// Error domain for Repository.
        /// </summary>
        /// <value>The error domain.</value>
        /// <remarks>
        /// Errors in this domain will be from the <see cref="RepositoryError"/> enumeration.
        /// </remarks>
        public static Quark ErrorQuark {
            get {
                return g_irepository_error_quark ();
            }
        }

        [DllImport ("libgirepository-1.0", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr g_irepository_find_by_error_domain (IntPtr raw, Quark domain);

        /// <summary>
        /// Searches for the enum type corresponding to the given GError domain.
        /// </summary>
        /// <returns>EnumInfo representing metadata about domain's enum type, or <c>null</c>.</returns>
        /// <param name="domain">A GError domain.</param>
        public static EnumInfo FindByErrorDomain (Quark domain)
        {
            IntPtr raw_ret = g_irepository_find_by_error_domain (IntPtr.Zero, domain);
            EnumInfo ret = BaseInfo.MarshalPtr<EnumInfo> (raw_ret);
            return ret;
        }

        [DllImport ("libgirepository-1.0", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr g_irepository_find_by_name (IntPtr raw, IntPtr @namespace, IntPtr name);

        internal static GISharp.GIRepository.BaseInfo FindByName (string @namespace, string name)
        {
            IntPtr native_namespace = GMarshal.StringToUtf8Ptr (@namespace);
            IntPtr native_name = GMarshal.StringToUtf8Ptr (name);
            IntPtr raw_ret = g_irepository_find_by_name (IntPtr.Zero, native_namespace, native_name);
            GISharp.GIRepository.BaseInfo ret = BaseInfo.MarshalPtr<BaseInfo> (raw_ret);
            GMarshal.Free (native_namespace);
            GMarshal.Free (native_name);
            return ret;
        }

        [DllImport ("libgirepository-1.0", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr g_irepository_find_by_gtype (IntPtr raw, GType gtype);

        public static BaseInfo FindByGType (GType gtype)
        {
            var raw_ret = g_irepository_find_by_gtype (IntPtr.Zero, gtype);
            var ret = BaseInfo.MarshalPtr<BaseInfo> (raw_ret);

            return ret;
        }

        [DllImport ("libgirepository-1.0", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr g_irepository_get_c_prefix (IntPtr raw, IntPtr @namespace);

        internal static string GetCPrefix (string @namespace)
        {
            IntPtr native_namespace = GMarshal.StringToUtf8Ptr (@namespace);
            IntPtr raw_ret = g_irepository_get_c_prefix (IntPtr.Zero, native_namespace);
            string ret = GMarshal.Utf8PtrToString (raw_ret);
            GMarshal.Free (native_namespace);
            return ret;
        }

        [DllImport ("libgirepository-1.0", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr g_irepository_get_dependencies (IntPtr raw, IntPtr @namespace);

        internal static string[] GetDependencies (string @namespace)
        {
            var native_namespace = GMarshal.StringToUtf8Ptr (@namespace);
            var raw_ret = g_irepository_get_dependencies (IntPtr.Zero, native_namespace);
            var ret = GMarshal.GStrvPtrToStringArray (raw_ret, freePtr: true, freeElements: true);
            GMarshal.Free (native_namespace);
            return ret;
        }

        [DllImport ("libgirepository-1.0", CallingConvention = CallingConvention.Cdecl)]
        [Since ("1.44")]
        static extern IntPtr g_irepository_get_immediate_dependencies (IntPtr raw, IntPtr @namespace);

        [Since ("1.44")]
        internal static string[] GetImmediateDependencies (string @namespace)
        {
            var native_namespace = GMarshal.StringToUtf8Ptr (@namespace);
            var raw_ret = g_irepository_get_immediate_dependencies (IntPtr.Zero, native_namespace);
            var ret = GMarshal.GStrvPtrToStringArray (raw_ret, freePtr: true, freeElements: true);
            GMarshal.Free (native_namespace);
            return ret;
        }

        [DllImport ("libgirepository-1.0", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr g_irepository_get_info (IntPtr raw, IntPtr @namespace, int index);

        internal static GISharp.GIRepository.BaseInfo GetInfo (string @namespace, int index)
        {
            IntPtr native_namespace = GMarshal.StringToUtf8Ptr (@namespace);
            IntPtr raw_ret = g_irepository_get_info (IntPtr.Zero, native_namespace, index);
            GISharp.GIRepository.BaseInfo ret = BaseInfo.MarshalPtr<BaseInfo> (raw_ret);
            GMarshal.Free (native_namespace);
            return ret;
        }

        [DllImport ("libgirepository-1.0", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr g_irepository_get_loaded_namespaces (IntPtr raw);

        /// <summary>
        /// Return the list of currently loaded namespaces.
        /// </summary>
        /// <value>List of namespaces.</value>
        public static string[] LoadedNamespaces {
            get {
                IntPtr raw_ret = g_irepository_get_loaded_namespaces (IntPtr.Zero);
                return GMarshal.GStrvPtrToStringArray (raw_ret, freePtr: true, freeElements: true);
            }
        }

        [DllImport ("libgirepository-1.0", CallingConvention = CallingConvention.Cdecl)]
        static extern int g_irepository_get_n_infos (IntPtr raw, IntPtr @namespace);

        static int GetNInfos (string @namespace)
        {
            var native_namespace = GMarshal.StringToUtf8Ptr (@namespace);
            var ret = g_irepository_get_n_infos (IntPtr.Zero, native_namespace);
            GMarshal.Free (native_namespace);
            return ret;
        }

        [DllImport ("libgirepository-1.0", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr g_irepository_get_search_path ();

        /// <summary>
        /// Returns the current search path GIRepository will use when loading typelib files.
        /// </summary>
        /// <value>The search path.</value>
        public static string[] SearchPaths {
            get {
                IntPtr raw_ret = g_irepository_get_search_path ();
                if (raw_ret == IntPtr.Zero) {
                    // if no method has been called yet that uses the native
                    // GIRepository object, g_irepository_get_search_path will
                    // return null. If that is the case, we call g_irepository_get_default
                    // to create the instance and try again.
                    g_irepository_get_default ();
                    raw_ret = g_irepository_get_search_path ();
                }
                var ret = GMarshal.GSListToStringArray (raw_ret);
                return ret;
            }
        }

        [DllImport ("libgirepository-1.0", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr g_irepository_get_shared_library (IntPtr raw, IntPtr @namespace);

        internal static string GetSharedLibrary (string @namespace)
        {
            IntPtr native_namespace = GMarshal.StringToUtf8Ptr (@namespace);
            IntPtr raw_ret = g_irepository_get_shared_library (IntPtr.Zero, native_namespace);
            string ret = GMarshal.Utf8PtrToString (raw_ret);
            GMarshal.Free (native_namespace);
            return ret;
        }

        [DllImport ("libgirepository-1.0", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr g_irepository_get_typelib_path (IntPtr raw, IntPtr @namespace);

        internal static string GetTypelibPath (string @namespace)
        {
            IntPtr native_namespace = GMarshal.StringToUtf8Ptr (@namespace);
            IntPtr raw_ret = g_irepository_get_typelib_path (IntPtr.Zero, native_namespace);
            string ret = GMarshal.Utf8PtrToString (raw_ret);
            GMarshal.Free (native_namespace);
            return ret;
        }

        [DllImport ("libgirepository-1.0", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr g_irepository_get_version (IntPtr raw, IntPtr @namespace);

        internal static string GetVersion (string @namespace)
        {
            IntPtr native_namespace = GMarshal.StringToUtf8Ptr (@namespace);
            IntPtr raw_ret = g_irepository_get_version (IntPtr.Zero, native_namespace);
            string ret = GMarshal.Utf8PtrToString (raw_ret);
            GMarshal.Free (native_namespace);
            return ret;
        }

        [DllImport ("libgirepository-1.0", CallingConvention = CallingConvention.Cdecl)]
        static extern bool g_irepository_is_registered (IntPtr raw, IntPtr @namespace, IntPtr version);

        /// <summary>
        /// Check whether a particular namespace (and optionally, a specific
        /// version thereof) is currently loaded.
        /// </summary>
        /// <returns><c>true</c> if is registered the specified <c>namespace-version</c>
        /// was loaded; otherwise, <c>false</c>.</returns>
        /// <param name="namespace">Namespace of interest.</param>
        /// <param name="version">Requred version or <c>null</c> for latest.</param>
        public static bool IsRegistered (string @namespace, string version = null)
        {
            if (@namespace == null) {
                throw new ArgumentNullException ("namespace");
            }
            IntPtr native_namespace = GMarshal.StringToUtf8Ptr (@namespace);
            IntPtr native_version = GMarshal.StringToUtf8Ptr (version);
            bool raw_ret = g_irepository_is_registered (IntPtr.Zero, native_namespace, native_version);
            bool ret = raw_ret;
            GMarshal.Free (native_namespace);
            GMarshal.Free (native_version);
            return ret;
        }

        [DllImport ("libgirepository-1.0", CallingConvention = CallingConvention.Cdecl)]
        static extern void g_irepository_prepend_library_path (IntPtr directory);

        /// <summary>
        /// Prepends <paramref name="directory"/> to the search path that is used
        /// to search shared libraries referenced by imported namespaces.
        /// </summary>
        /// <param name="directory">A single directory to scan for shared libraries.</param>
        /// <remarks>
        /// Multiple calls to this function all contribute to the final list of
        /// paths. The list of paths is unique and shared for all GIRepository
        /// instances across the process, but it doesn't affect namespaces imported
        /// before the call.
        ///
        /// If the library is not found in the directories configured in this way,
        /// loading will fall back to the system library path (ie. LD_LIBRARY_PATH
        /// and DT_RPATH in ELF systems). See the documentation of your dynamic
        /// linker for full details.
        /// </remarks>
        public static void PrependLibraryPath (string directory)
        {
            if (directory == null) {
                throw new ArgumentNullException ("directory");
            }
            // TODO: Marshal as filename, not UTF8
            IntPtr native_directory = GMarshal.StringToUtf8Ptr (directory);
            g_irepository_prepend_library_path (native_directory);
            GMarshal.Free (native_directory);
        }

        [DllImport ("libgirepository-1.0", CallingConvention = CallingConvention.Cdecl)]
        static extern void g_irepository_prepend_search_path (IntPtr directory);

        /// <summary>
        /// Prepends directory to the typelib search path.
        /// </summary>
        /// <param name="directory">Directory name to prepend to the typelib search path.</param>
        /// <seealso cref="PrependLibraryPath"/>
        public static void PrependSearchPath (string directory)
        {
            if (directory == null) {
                throw new ArgumentNullException ("directory");
            }
            IntPtr native_directory = GMarshal.StringToUtf8Ptr (directory);
            g_irepository_prepend_search_path (native_directory);
            GMarshal.Free (native_directory);
        }

        [DllImport ("libgirepository-1.0", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr g_irepository_require (IntPtr raw, IntPtr @namespace, IntPtr version, int flags, out IntPtr error);

        /// <summary>
        /// Force the namespace <paramref name="namespace"/> to be loaded if it
        /// isn't already.
        /// </summary>
        /// <param name="namespace">Namespace.</param>
        /// <param name="version">Version.</param>
        /// <param name="flags">Flags.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="namespace"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="GErrorException">On failure.</exception>
        /// <remarks>
        /// If <paramref name="namespace"/> is not loaded, this function will
        /// search for a ".typelib" file using the repository search path. In
        /// addition, a version version of namespace may be specified. If version
        /// is not specified, the latest will be used.
        /// </remarks>
        public static void Require (string @namespace, string version = null,
            RepositoryLoadFlags flags = default(RepositoryLoadFlags))
        {
            if (@namespace == null) {
                throw new ArgumentNullException (nameof(@namespace));
            }
            IntPtr native_namespace = GMarshal.StringToUtf8Ptr (@namespace);
            IntPtr native_version = GMarshal.StringToUtf8Ptr (version);
            IntPtr error_;
            g_irepository_require (IntPtr.Zero, native_namespace, native_version, (int)flags, out error_);
            GMarshal.Free (native_namespace);
            GMarshal.Free (native_version);
            if (error_ != IntPtr.Zero) {
                var error = new Error.SafeHandle (error_, Runtime.Transfer.Full);
                throw new GErrorException (error);
            }
        }

        [DllImport ("libgirepository-1.0", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr g_irepository_require_private (IntPtr raw, IntPtr typelibDir, IntPtr @namespace, IntPtr version, int flags, out IntPtr error);

        /// <summary>
        /// Force the namespace namespace_ to be loaded if it isn't already.
        /// </summary>
        /// <param name="typelibDir">Private directory where to find the requested typelib.</param>
        /// <param name="namespace">Namespace.</param>
        /// <param name="version">Version of namespace, may be <c>null</c> for latest.</param>
        /// <param name="flags">Flags.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="typelibDir"/>
        /// or <paramref name="namespace"/> is <c>null<c/>.</exception>
        /// <exception cref="GErrorException">On failure.</exception>
        /// <remarks>
        /// If <paramref name="namespace"/> is not loaded, this function will
        /// search for a ".typelib" file within the private directory only. In
        /// addition, a version <paramref name="version"/> of namespace may be
        /// specified. If <paramref name="version"/> is not specified, the latest
        /// will be used.
        /// </remarks>
        public static void RequirePrivate (string typelibDir, string @namespace,
            string version = null, RepositoryLoadFlags flags = default(RepositoryLoadFlags))
        {
            if (typelibDir == null) {
                throw new ArgumentNullException ("typelibDir");
            }
            if (@namespace == null) {
                throw new ArgumentNullException ("namespace");
            }
            IntPtr native_typelib_dir = GMarshal.StringToUtf8Ptr (typelibDir);
            IntPtr native_namespace = GMarshal.StringToUtf8Ptr (@namespace);
            IntPtr native_version = GMarshal.StringToUtf8Ptr (version);
            IntPtr error_;
            g_irepository_require_private (IntPtr.Zero, native_typelib_dir, native_namespace, native_version, (int)flags, out error_);
            GMarshal.Free (native_typelib_dir);
            GMarshal.Free (native_namespace);
            GMarshal.Free (native_version);
            if (error_ != IntPtr.Zero) {
                var error = new Error.SafeHandle (error_, Runtime.Transfer.Full);
                throw new GErrorException (error);
            }
        }
    }
}
