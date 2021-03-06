
using System;
using GISharp.GLib;
using GISharp.GObject;
using NUnit.Framework;

using Object = GISharp.GObject.Object;

namespace GISharp.Core.Test.GObject
{
    [TestFixture]
    public class SignalTests
    {
        [Test]
        public void TestValidateName()
        {
            Assert.That(() => Signal.ValidateName("4"), Throws.ArgumentException,
                "Name must start with a letter");
            Assert.That(() => Signal.ValidateName("s$"), Throws.ArgumentException,
                "$ is not allowed");
            Assert.That(() => Signal.ValidateName("s-s_s"), Throws.ArgumentException,
                "can't have both - and _");

            Assert.That(() => Signal.ValidateName("s"), Throws.Nothing);
            Assert.That(() => Signal.ValidateName("S"), Throws.Nothing);
            Assert.That(() => Signal.ValidateName("s1"), Throws.Nothing);
            Assert.That(() => Signal.ValidateName("s-s"), Throws.Nothing);
            Assert.That(() => Signal.ValidateName("s_s"), Throws.Nothing);

            Utility.AssertNoGLibLog();
        }

        [Test]
        public void TestParseName()
        {
            var id = Signal.TryLookup("notify", GType.Object);
            Assume.That(id, Is.Not.Zero);

            uint signalId;
            Quark detail;

            // try a real signal name
            if (Signal.TryParseName("notify", GType.Object, out signalId, out detail)) {
                Assert.That(signalId, Is.EqualTo(id));
                Assert.That(detail, Is.EqualTo(Quark.Zero));
            }
            else {
                Assert.Fail("Should have returned true");
            }

            // again with the exception throwing version
            (signalId, detail) = Signal.ParseName("notify", GType.Object);
            Assert.That(signalId, Is.EqualTo(id));
            Assert.That(detail, Is.EqualTo(Quark.Zero));

            // A bad signal name returns false
            if (Signal.TryParseName("does-not-exist", GType.Object, out signalId, out detail)) {
                Assert.Fail("Should have returned false");
            }

            // again with the exception throwing version
            Assert.That(() => Signal.ParseName("does-not-exist", GType.Object), Throws.ArgumentException);

            Utility.AssertNoGLibLog();
        }

        [Test]
        public void TestStopEmission()
        {
            bool stopEmission = false;
            int handler1Count = 0;
            int handler2Count = 0;

            using (var pspec = new ParamSpecBoolean("test-param", "test-param", "test-param",
                false, ParamFlags.Readwrite | ParamFlags.StaticStrings))
            using (var obj = new Object()) {
                var id = Signal.TryLookup("notify", GType.Object);
                Assume.That(id, Is.Not.EqualTo(0));

                Object.NotifyEventHandler handler1 = (s, e) => {
                    handler1Count++;
                    if (stopEmission) {
                        obj.StopEmission(id);
                    }
                };

                Object.NotifyEventHandler handler2 = (s, e) => handler2Count++;

                obj.Notify += handler1;
                obj.Notify += handler2;

                // make sure our callbacks are working
                obj.Emit(id, 0, pspec);
                Assume.That(handler1Count, Is.EqualTo(1));
                Assume.That(handler2Count, Is.EqualTo(1));

                // now try to stop the emission
                stopEmission = true;
                obj.Emit(id, 0, pspec);

                Assert.That(handler1Count, Is.EqualTo(2));
                Assert.That(handler2Count, Is.EqualTo(1));
            }

            Utility.AssertNoGLibLog();
        }

        [Test]
        public void TestStopEmissionByName()
        {
            bool stopEmission = false;
            int handler1Count = 0;
            int handler2Count = 0;

            using (var pspec = new ParamSpecBoolean("test-param", "test-param", "test-param",
                false, ParamFlags.Readwrite | ParamFlags.StaticStrings))
            using (var obj = new Object()) {
                Object.NotifyEventHandler handler1 = (s, e) => {
                    handler1Count++;
                    if (stopEmission) {
                        obj.StopEmissionByName("notify::test-param");
                    }
                };

                Object.NotifyEventHandler handler2 = (s, e) => handler2Count++;

                obj.Notify += handler1;
                obj.Notify += handler2;

                // make sure our callbacks are working
                obj.EmitNotify(pspec);
                Assume.That(handler1Count, Is.EqualTo(1));
                Assume.That(handler2Count, Is.EqualTo(1));

                // now try to stop the emission
                stopEmission = true;
                obj.EmitNotify(pspec);

                Assert.That(handler1Count, Is.EqualTo(2));
                Assert.That(handler2Count, Is.EqualTo(1));
            }

            Utility.AssertNoGLibLog();
        }
    }
}
