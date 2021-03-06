﻿using System;

using NUnit.Framework;
using GISharp.GLib;
using System.Collections.Generic;
using GISharp.GObject;
using GISharp.Runtime;

namespace GISharp.Core.Test.GLib
{
    [TestFixture]
    public class HashTableTests
    {
        void HandleFunc ()
        {

        }

        [Test]
        public void TestConstructor ()
        {
            using (var hashTable = new HashTable<OpaqueInt, OpaqueInt> ()) {
            }

            Utility.AssertNoGLibLog();
        }

        [Test]
        public void TestInsert ()
        {
            using (var hashTable = new HashTable<OpaqueInt, OpaqueInt> ()) {
                Assume.That (hashTable.Size, Is.EqualTo (0));

                // inserting a value returns true
                var ret = hashTable.Insert (new OpaqueInt (0), new OpaqueInt (0));
                Assert.That (ret, Is.True);

                // replacing a value returns false
                ret = hashTable.Insert (new OpaqueInt (0), new OpaqueInt (1));
                Assert.That (ret, Is.False);

                // null key works
                ret = hashTable.Insert (null, new OpaqueInt (0));
                Assert.That (ret, Is.False);

                // null value works
                ret = hashTable.Insert (null, null);
                Assert.That (ret, Is.False);

                hashTable.Dispose ();
                Assert.That (() => hashTable.Insert (null, null),
                             Throws.TypeOf<ObjectDisposedException> ());
            }

            Utility.AssertNoGLibLog();
        }

        [Test]
        public void TestReplace ()
        {
            using (var hashTable = new HashTable<OpaqueInt, OpaqueInt> ()) {
                Assume.That (hashTable.Size, Is.EqualTo (0));

                // adding a new key returns true
                var ret = hashTable.Replace (new OpaqueInt (0), new OpaqueInt (0));
                Assert.That (ret, Is.True);

                // replacing a key returns false
                ret = hashTable.Replace (new OpaqueInt (0), new OpaqueInt (1));
                Assert.That (ret, Is.False);

                // null key works
                ret = hashTable.Replace (null, new OpaqueInt (0));
                Assert.That (ret, Is.False);

                // null value works
                ret = hashTable.Replace (null, null);
                Assert.That (ret, Is.False);

                hashTable.Dispose ();
                Assert.That (() => hashTable.Replace (null, null),
                             Throws.TypeOf<ObjectDisposedException> ());
            }

            Utility.AssertNoGLibLog();
        }

        [Test]
        public void TestAdd ()
        {
            using (var hashTable = new HashTable<OpaqueInt, OpaqueInt> ()) {
                Assume.That (hashTable.Size, Is.EqualTo (0));

                // adding a new key returns true
                var ret = hashTable.TryAdd (new OpaqueInt (0));
                Assert.That (ret, Is.True);

                // replacing a key returns false
                ret = hashTable.TryAdd (new OpaqueInt (0));
                Assert.That (ret, Is.False);

                // null key works
                ret = hashTable.TryAdd (null);
                Assert.That (ret, Is.False);

                hashTable.Dispose ();
                Assert.That (() => hashTable.Add (null),
                             Throws.TypeOf<ObjectDisposedException> ());
            }

            Utility.AssertNoGLibLog();
        }

        [Test]
        public void TestContains ()
        {
            using (var hashTable = new HashTable<OpaqueInt, OpaqueInt> ()) {
                // no match returns false
                var ret = hashTable.Contains (new OpaqueInt (0));
                Assert.That (ret, Is.False);

                // match returns true
                hashTable.Add (new OpaqueInt (0));
                Assume.That (hashTable.Size, Is.EqualTo (1));
                ret = hashTable.Contains (new OpaqueInt (0));
                Assert.That (ret, Is.True);

                // null key works
                ret = hashTable.Contains (null);
                Assert.That (ret, Is.True);

                hashTable.Dispose ();
                Assert.That (() => hashTable.Contains (null),
                             Throws.TypeOf<ObjectDisposedException> ());
            }

            Utility.AssertNoGLibLog();
        }

        [Test]
        public void TestSize ()
        {
            using (var hashTable = new HashTable<OpaqueInt, OpaqueInt> ()) {
                Assume.That (hashTable.Size, Is.EqualTo (0));

                hashTable.Add (new OpaqueInt (0));
                Assert.That (hashTable.Size, Is.EqualTo (1));

                hashTable.Dispose ();
                Assert.That (() => hashTable.Size,
                             Throws.TypeOf<ObjectDisposedException> ());
            }

            Utility.AssertNoGLibLog();
        }

        [Test]
        public void TestLookup ()
        {
            using (var hashTable = new HashTable<OpaqueInt, OpaqueInt> ()) {
                // no match returns false
                var ret = hashTable.Lookup (new OpaqueInt (0));
                // Lookup cannot tell the difference between null and IntPtr.Zero
                Assert.That (ret, Is.Null.Or.EqualTo (new OpaqueInt (0)));

                // match returns true
                hashTable.Add (new OpaqueInt (1));
                Assume.That (hashTable.Size, Is.EqualTo (1));
                ret = hashTable.Lookup (new OpaqueInt (1));
                Assert.That (ret, Is.EqualTo (new OpaqueInt (1)));

                // null key works
                ret = hashTable.Lookup (null);
                // Lookup cannot tell the difference between null and IntPtr.Zero
                Assert.That (ret, Is.Null.Or.EqualTo (new OpaqueInt (0)));

                hashTable.Dispose ();
                Assert.That (() => hashTable.Lookup (null),
                             Throws.TypeOf<ObjectDisposedException> ());
            }

            Utility.AssertNoGLibLog();
        }

        [Test]
        public void TestLookupExtended ()
        {
            using (var hashTable = new HashTable<OpaqueInt, OpaqueInt> ()) {
                OpaqueInt key, value;
                // no match returns false
                var ret = hashTable.Lookup (new OpaqueInt (0), out key, out value);
                Assert.That (ret, Is.False);
                // match returns true
                hashTable.Add (new OpaqueInt (0));
                Assume.That (hashTable.Size, Is.EqualTo (1));
                ret = hashTable.Lookup (new OpaqueInt (0), out key, out value);
                Assert.That (ret, Is.True);
                // null key works
                ret = hashTable.Lookup (null, out key, out value);
                Assert.That (ret, Is.True);

                hashTable.Dispose ();
                Assert.That (() => hashTable.Lookup (null, out key, out value),
                             Throws.TypeOf<ObjectDisposedException> ());
            }

            Utility.AssertNoGLibLog();
        }

        [Test]
        public void TestForeach ()
        {
            var count = 0;
            Action<OpaqueInt, OpaqueInt> foreachFunc = (k, v) => {
                count++;
            };

            using (var hashTable = new HashTable<OpaqueInt, OpaqueInt> ()) {
                hashTable.Add (new OpaqueInt (0));
                Assume.That (hashTable.Size, Is.EqualTo (1));

                // function is called back
                hashTable.Foreach (foreachFunc);
                Assert.That (count, Is.EqualTo (1));

                // null function is not OK
                Assert.That (() => hashTable.Foreach (null),
                    Throws.InstanceOf<ArgumentNullException> ());

                hashTable.Dispose ();
                Assert.That (() => hashTable.Foreach (foreachFunc),
                             Throws.TypeOf<ObjectDisposedException> ());
            }

            Utility.AssertNoGLibLog();
        }

        [Test]
        public void TestFind ()
        {
            Predicate<KeyValuePair<OpaqueInt, OpaqueInt>> findFunc = (p) => {
                return true;
            };

            using (var hashTable = new HashTable<OpaqueInt, OpaqueInt> ()) {
                // no match returns null
                var ret = hashTable.Find (findFunc);
                Assert.That (ret, Is.Null.Or.EqualTo (new OpaqueInt (0)));

                // match returns non-null
                hashTable.Add (new OpaqueInt (1));
                Assume.That (hashTable.Size, Is.EqualTo (1));
                ret = hashTable.Find (findFunc);
                Assert.That (ret, Is.Not.Null);

                // null function is not OK
                Assert.That (() => hashTable.Find (null),
                    Throws.InstanceOf<ArgumentNullException> ());

                hashTable.Dispose ();
                Assert.That (() => hashTable.Find (findFunc),
                             Throws.TypeOf<ObjectDisposedException> ());
            }

            Utility.AssertNoGLibLog();
        }

        [Test]
        public void TestRemove ()
        {
            using (var hashTable = new HashTable<OpaqueInt, OpaqueInt> ()) {
                // no match returns false
                var ret = hashTable.Remove (new OpaqueInt (0));
                Assert.That (ret, Is.False);

                // match returns true
                hashTable.Add (new OpaqueInt (0));
                Assume.That (hashTable.Size, Is.EqualTo (1));
                ret = hashTable.Remove (new OpaqueInt (0));
                Assert.That (ret, Is.True);

                // null key works
                ret = hashTable.Remove (null);
                Assert.That (ret, Is.False);

                hashTable.Dispose ();
                Assert.That (() => hashTable.Remove (null),
                             Throws.TypeOf<ObjectDisposedException> ());
            }

            Utility.AssertNoGLibLog();
        }
#if false
        [Test]
        public void TestSteal ()
        {
            using (var hashTable = new HashTable<OpaqueInt, OpaqueInt> ()) {
                // no match returns false
                var ret = hashTable.Steal (new OpaqueInt (1));
                Assert.That (ret, Is.False);

                // match returns true
                hashTable.Add (new OpaqueInt (1));
                Assume.That (hashTable.Size, Is.EqualTo (1));
                ret = hashTable.Steal (new OpaqueInt (1));
                Assert.That (ret, Is.True);

                // null key works
                ret = hashTable.Steal (null);
                Assert.That (ret, Is.False);

                hashTable.Dispose ();
                Assert.That (() => hashTable.Steal (null),
                             Throws.TypeOf<ObjectDisposedException> ());
            }

            Utility.AssertNoGLibLog();
        }
#endif
        [Test]
        public void TestForeachRemove ()
        {
            var count = 0;
            Predicate<KeyValuePair<OpaqueInt, OpaqueInt>> foreachFunc = (p) => {
                count++;
                return true;
            };

            using (var hashTable = new HashTable<OpaqueInt, OpaqueInt> ()) {
                hashTable.Add (new OpaqueInt (0));
                Assume.That (hashTable.Size, Is.EqualTo (1));

                // function is called back
                var ret = hashTable.ForeachRemove (foreachFunc);
                Assert.That (ret, Is.EqualTo (count));

                // null function is not OK
                Assert.That (() => hashTable.ForeachRemove (null),
                    Throws.InstanceOf<ArgumentNullException> ());

                hashTable.Dispose ();
                Assert.That (() => hashTable.ForeachRemove (foreachFunc),
                             Throws.TypeOf<ObjectDisposedException> ());
            }

            Utility.AssertNoGLibLog();
        }
#if false
        [Test]
        public void TestForeachSteal ()
        {
            var count = 0;
            HRFunc <OpaqueInt,OpaqueInt> foreachFunc = (key, value) => {
                count++;
                return true;
            };

            using (var hashTable = new HashTable<OpaqueInt, OpaqueInt> ()) {
                hashTable.Add (new OpaqueInt (1));
                Assume.That (hashTable.Size, Is.EqualTo (1));
                // function is called back
                var ret = hashTable.ForeachSteal (foreachFunc);
                Assert.That (ret, Is.EqualTo (count));
                // null function is not OK
                Assert.That (() => hashTable.ForeachSteal (null),
                    Throws.InstanceOf<ArgumentNullException> ());

                hashTable.Dispose ();
                Assert.That (() => hashTable.ForeachSteal (foreachFunc),
                             Throws.TypeOf<ObjectDisposedException> ());
            }

            Utility.AssertNoGLibLog();
        }
#endif
        [Test]
        public void TestRemoveAll ()
        {
            using (var hashTable = new HashTable<OpaqueInt, OpaqueInt> ()) {
                hashTable.Add (new OpaqueInt (0));
                Assume.That (hashTable.Size, Is.EqualTo (1));

                hashTable.RemoveAll ();
                Assert.That (hashTable.Size, Is.EqualTo (0));

                hashTable.Dispose ();
                Assert.That (() => hashTable.RemoveAll (),
                             Throws.TypeOf<ObjectDisposedException> ());
            }

            Utility.AssertNoGLibLog();
        }
#if false
        [Test]
        public void TestStealAll ()
        {
            using (var hashTable = new HashTable<OpaqueInt, OpaqueInt> ()) {
                hashTable.Add (new OpaqueInt (1));
                Assume.That (hashTable.Size, Is.EqualTo (1));

                hashTable.StealAll ();
                Assert.That (hashTable.Size, Is.EqualTo (0));

                hashTable.Dispose ();
                Assert.That (() => hashTable.StealAll (),
                             Throws.TypeOf<ObjectDisposedException> ());
            }

            Utility.AssertNoGLibLog();
        }
#endif
        [Test]
        public void TestGetKeys ()
        {
            using (var hashTable = new HashTable<OpaqueInt, OpaqueInt> ()) {
                hashTable.Add (new OpaqueInt (1));
                Assume.That (hashTable.Size, Is.EqualTo (1));

                var ret = hashTable.Keys;
                Assert.That (ret.Length, Is.EqualTo (1));

                hashTable.Dispose ();
                Assert.That (() => hashTable.Keys,
                             Throws.TypeOf<ObjectDisposedException> ());
            }

            Utility.AssertNoGLibLog();
        }

        [Test]
        public void TestGetValues ()
        {
            using (var hashTable = new HashTable<OpaqueInt, OpaqueInt> ()) {
                hashTable.Add (new OpaqueInt (1));
                Assume.That (hashTable.Size, Is.EqualTo (1));

                var ret = hashTable.Values;
                Assert.That (ret.Length, Is.EqualTo (1));

                hashTable.Dispose ();
                Assert.That (() => hashTable.Values,
                             Throws.TypeOf<ObjectDisposedException> ());
            }

            Utility.AssertNoGLibLog();
        }

        [Test]
        public void TestGType ()
        {
            var gtype = typeof (HashTable<OpaqueInt, OpaqueInt>).GetGType ();
            Assert.That (gtype, Is.Not.EqualTo (GType.Invalid));
            Assert.That (gtype.Name, Is.EqualTo ("GHashTable"));

            Utility.AssertNoGLibLog();
        }
    }
}
