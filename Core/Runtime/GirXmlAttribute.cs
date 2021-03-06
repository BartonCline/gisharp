﻿using System;

namespace GISharp.Runtime
{
    [AttributeUsage (AttributeTargets.Class | AttributeTargets.Delegate | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.Struct)]
    public sealed class GirXmlAttribute : Attribute
    {
        public string Xml { get; private set; }

        public GirXmlAttribute (string xml)
        {
            if (xml == null) {
                throw new ArgumentNullException (nameof (xml));
            }
            Xml = xml;
        }
    }
}
