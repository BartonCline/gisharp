﻿using System;

namespace GISharp.GObject
{
    [AttributeUsage (AttributeTargets.All)]
    public class SinceAttribute : Attribute
    {
        public Version Version { get; private set; }

        public SinceAttribute (string version)
        {
            Version = Version.Parse (version);
        }
    }
}
