﻿using GISharp.GLib;
using GISharp.GObject;
using GISharp.Runtime;
using NUnit.Framework;

namespace GISharp.Core.Test.GLib
{
    [TestFixture]
    public class ErrorTests
    {
        [Test]
        public void TestGType ()
        {
            var gtype = typeof (Error).GetGType ();
            Assert.That (gtype, Is.Not.EqualTo (GType.Invalid));
            Assert.That (gtype.Name, Is.EqualTo ("GError"));

            Utility.AssertNoGLibLog();
        }

        static Quark ErrorQuark {
            get {
                return TestErrorDomain.Failed.GetGErrorDomain ();
            }
        }
    }

    [GErrorDomain ("gisharp-core-test-error-domain-quark")]
    enum TestErrorDomain
    {
        Failed
    }
}
