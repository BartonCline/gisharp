﻿using System;
using GISharp.Runtime;

namespace GISharp.GObject
{
    /// <summary>
    /// This is a type of <see cref="ParamSpec"/> type that simply redirects operations to
    /// another paramspec.  All operations other than getting or
    /// setting the value are redirected, including accessing the nick and
    /// blurb, validating a value, and so forth. See
    /// <see cref="ParamSpec.RedirectTarget"/> for retrieving the overidden
    /// property. <see cref="ParamSpecOverride"/> is used in implementing
    /// <see cref="ObjectClass.OverrideProperty"/>, and will not be directly useful
    /// unless you are implementing a new base type similar to GObject.
    /// </summary>
    [Since ("2.4")]
    [GType ("GParamOverride", IsWrappedNativeType = true)]
    sealed class ParamSpecOverride : ParamSpec
    {
        struct ParamSpecOverrideStruct
        {
            public ParamSpecStruct ParentInstance;
            public IntPtr Overridden;
        }

        static GType getGType ()
        {
            return paramSpecTypes[20];
        }

        public ParamSpecOverride (IntPtr handle, Transfer ownership)
            : base (handle, ownership)
        {
        }
    }
}