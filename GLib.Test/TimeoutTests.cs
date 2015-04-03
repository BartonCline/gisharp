﻿using NUnit.Framework;
using System;
using System.Threading.Tasks;

using GISharp.GLib;

namespace GISharp.GLib.Test
{
    [TestFixture ()]
    public class TimeoutTests
    {
        [Test ()]
        public void TestTimeoutAdd ()
        {
            var timeoutInvoked = false;

            Task.Run (() => {
                var mainLoop = new MainLoop ();
                Timeout.Add (0, () => {
                    mainLoop.Quit ();
                    timeoutInvoked = true;
                    return Source.Remove;
                });
                mainLoop.Run ();
            }).Wait (100);

            Assert.That (timeoutInvoked, Is.True);
        }

        [Test ()]
        public void TestTimeoutAddSeconds ()
        {
            var timeoutInvoked = false;

            Task.Run (() => {
                var mainLoop = new MainLoop ();
                Timeout.AddSeconds (0, () => {
                    mainLoop.Quit ();
                    timeoutInvoked = true;
                    return Source.Remove;
                });
                mainLoop.Run ();
            }).Wait (2000);

            Assert.That (timeoutInvoked, Is.True);
        }

        [Test ()]
        public void TestTimeoutAddNew ()
        {
            var source = Timeout.SourceNew (0);
            Assert.That (source, Is.Not.Null);
        }

        [Test ()]
        public void TestTimeoutAddNewSeconds ()
        {
            var source = Timeout.SourceNewSeconds (0);
            Assert.That (source, Is.Not.Null);
        }
    }
}