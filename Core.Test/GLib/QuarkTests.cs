﻿using NUnit.Framework;
using System;

using GISharp.GLib;

namespace GISharp.Core.Test.GLib
{
    [TestFixture]
    public class QuarkTests
    {
        const string testQuarkPrefix = "gisharp-glib-test-quark-";

        [Test]
        public void TestImplicitOperator ()
        {
            uint expected = 1;
            Quark quark = expected;
            uint actual = quark;
            Assert.That (actual, Is.EqualTo (expected));

            Utility.AssertNoGLibLog();
        }

        [Test]
        public void TestFromString ()
        {
            Quark actual;

            // null always returns 0
            actual = Quark.FromString (null);
            Assert.That (actual, Is.EqualTo (default(Quark)));

            // this creates a new quark if it does not exist
            actual = Quark.FromString (testQuarkPrefix + "test-from-string");
            Assert.That (actual, Is.Not.EqualTo (default(Quark)));

            Utility.AssertNoGLibLog();
        }

        [Test]
        public void TestToString ()
        {
            string actual;

            // null always returns 0
            actual = default(Quark).ToString ();
            Assert.That (actual, Is.Null);

            // undefined quark creates new
            var quarkString = testQuarkPrefix + "test-to-string";
            Assume.That (Quark.TryString (quarkString), Is.EqualTo (default(Quark)));
            var quark = Quark.FromString (quarkString);
            Assume.That (quark, Is.Not.EqualTo (default(Quark)));
            actual = quark.ToString ();
            Assert.That (actual, Is.EqualTo (quarkString));

            Utility.AssertNoGLibLog();
        }


        [Test]
        public void TestTryString ()
        {
            Quark actual;
            var quarkString = testQuarkPrefix + "test-try-string";

            // null always returns 0
            actual = Quark.TryString (null);
            Assert.That (actual, Is.EqualTo (default(Quark)));

            // undefined quark returns 0
            actual = Quark.TryString (quarkString);
            Assert.That (actual, Is.EqualTo (default(Quark)));

            // defined quark returns value
            var quark = Quark.FromString (quarkString);
            actual = Quark.TryString (quarkString);
            Assert.That (actual, Is.EqualTo (quark));

            Utility.AssertNoGLibLog();
        }
    }
}
