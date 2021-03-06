// This file was generated by the Gtk# code generator.
// It is now maintained by hand.

using GISharp.GLib;
using GISharp.Runtime;

namespace GISharp.GIRepository
{
    /// <summary>
    /// An error code returned from a Repository method.
    /// </summary>
    [GErrorDomain ("g-irepository-error-quark")]
    public enum RepositoryError
    {
        /// <summary>
        /// The typelib could not be found.
        /// </summary>
        TypelibNotFound,

        /// <summary>
        /// The namespace does not match the requested namespace.
        /// </summary>
        NamespaceMismatch,

        /// <summary>
        /// The version of the typelib does not match the requested version.
        /// </summary>
        NamespaceVersionConflict,

        /// <summary>
        /// The library used by the typelib could not be found.
        /// </summary>
        LibraryNotFound,
    }
}
