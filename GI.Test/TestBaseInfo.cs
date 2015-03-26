﻿using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;

namespace GI
{
    [TestFixture ()]
    public class TestBaseInfo
    {
        InfoCollection<BaseInfo> infos;

        [TestFixtureSetUp ()]
        public void TestGetDefault ()
        {
            // The default repository is process global, so we must initalize it only once.
            // It will be used all of the tests
            Repository.Require ("GLib", "2.0", (RepositoryLoadFlags)0);
            infos = Repository.Namespaces ["GLib"].Infos;
        }

        [Test ()]
        public void TestEqual ()
        {
            var info1 = infos [0];
            var info2 = infos [0];
            // want to make sure that we compare by value and not by reference
            Assume.That (ReferenceEquals (info1, info2), Is.False);
            Assert.That (info1, Is.EqualTo (info2));
            Assert.That (info1 == info2, Is.True);
            Assert.That (info1 != info2, Is.False);

            info2 = infos [1];
            Assert.That (info1, Is.Not.EqualTo (info2));
            Assert.That (info1 == info2, Is.False);
            Assert.That (info1 != info2, Is.True);

            Assert.That (info1 == null, Is.False);
            Assert.That (info1 != null, Is.True);
            Assert.That (null == info1, Is.False);
            Assert.That (null != info1, Is.True);

            info1 = null;
            info2 = null;
            Assert.That (info1 == info2, Is.True);
            Assert.That (info1 != info2, Is.False);
        }

        [Test ()]
        public void TestGetType ()
        {
            Assert.That (infos.First ().InfoType, Is.EqualTo (InfoType.Constant));
        }

        [Test ()]
        public void TestGetTypelib ()
        {
            Assert.That (infos.First ().Typelib, Is.Not.Null);
        }

        [Test ()]
        public void TestGetNameSpace ()
        {
            Assert.That (infos.First ().Namespace, Is.EqualTo ("GLib"));
        }

        [Test ()]
        public void TestGetName ()
        {
            Assert.That (infos.First ().Name, Is.EqualTo ("ANALYZER_ANALYZING"));
        }

        [Test ()]
        public void TestGetAttribute ()
        {
            // TODO: Need to figure out how to add attributes.
            //Assert.That (infos.First ().GetAttribute("type"), Is.EqualTo (""));
        }

        [Test ()]
        public void TestIterateAttributes ()
        {
            // TODO: Need to figure out how to add attributes.
            foreach (var info in infos) {
                foreach (var pair in info.Attributes) {
                    Console.WriteLine (pair.Key, pair.Value);
                }
            }
        }

        [Test (), Ignore]
        public void TestGetContainer ()
        {
            // TODO: it seems nothing in GLib has a Container
            var container = infos.First (i => i.Container != null).Container;
            // making sure that it is marshalled as an acutal type and not base
            Assert.That (container, Is.TypeOf<BaseInfo> ());
            Assert.That (container.GetType (), Is.Not.EqualTo (typeof(BaseInfo)));
        }

        [Test ()]
        public void TestIsDeprecated ()
        {
            var count = infos.Count (i => i.IsDeprecated == true);
            Assert.That (count, Is.GreaterThan (0));
        }
    }
}

