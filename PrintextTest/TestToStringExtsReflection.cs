using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NoSuchStudio.Common.PrintextTest
{
    [TestClass]
    public sealed class TestToStringExtsReflection
    {
        [TestMethod]
        public void TestInt()
        {
            int i = 1;
            Assert.AreEqual("1", i.ToStringExt());
        }

        [TestMethod]
        public void TestString()
        {
            string str = "one";
            Assert.AreEqual("one", str.ToStringExt());
        }

        [TestMethod]
        public void TestNull()
        {
            string? str = null;
            Assert.AreEqual("null", str.ToStringExt());
        }

        [TestMethod]
        public void TestFlatList()
        {
            var strList = new List<string>() { "one", "two", "three" };
            Assert.AreEqual("{one, two, three}", strList.ToStringExt());
        }

        [TestMethod]
        public void TestListOfLists()
        {
            var listOfLists = new List<List<string>>() {
                new() { "one", "two" },
                new() { "three", "four" }
            };
            Assert.AreEqual("{{one, two}, {three, four}}", listOfLists.ToStringExt());
        }

        [TestMethod]
        public void TestListOfListsOfLists() {
            var listOfListsOfLists = new List<List<List<string>>>() {
                new() {
                    new List<string>() {"one", "two"},
                    new List<string>() {"three", "four"}
                },
                new() {
                    new List<string>() {"five", "six"},
                    new List<string>() {"seven", "eight"}
                }
            };
            Assert.AreEqual("{{{one, two}, {three, four}}, {{five, six}, {seven, eight}}}",
                            listOfListsOfLists.ToStringExt());
        }

        [TestMethod]
        public void TestDictionary()
        {
            var flatDic = new Dictionary<int, string>() {
                [1] = "one",
                [2] = "two"
            };
            Assert.AreEqual("{(1 => one), (2 => two)}", flatDic.ToStringExt());
        }

        [TestMethod]
        public void TestDictionaryOfDictionaries()
        {
            var dic2 = new Dictionary<int, Dictionary<string, string>>() {
                [1] = new Dictionary<string, string>() {
                    ["one"] = "first",
                    ["two"] = "second"
                },
                [2] = new Dictionary<string, string>() {
                    ["three"] = "third",
                    ["four"] = "forth"
                }
            };
            Assert.AreEqual("{(1 => {(one => first), (two => second)}), (2 => {(three => third), (four => forth)})}",
                            dic2.ToStringExt());
        }

        [TestMethod]
        public void TestDictionaryOfLists()
        {
            var dicOfList = new Dictionary<int, List<string>>() {
                [1] = ["one", "two"],
                [2] = ["three", "four"]
            };
            Assert.AreEqual("{(1 => {one, two}), (2 => {three, four})}", dicOfList.ToStringExt());
        }

        [TestMethod]
        public void TestListOfDictionaries()
        {
            var listOfDic = new List<Dictionary<int, string>>() {
                new() {
                    [1] = "one",
                    [2] = "two"
                },
                new() {
                    [3] = "three",
                    [4] = "four"
                }
            };
            Assert.AreEqual("{{(1 => one), (2 => two)}, {(3 => three), (4 => four)}}", listOfDic.ToStringExt());
        }

        [TestMethod]
        public void TestDictionaryOfDictionariesOfLists()
        {
            var dicOfDicOfList = new Dictionary<int, Dictionary<int, List<string>>>() {
                [1] = new Dictionary<int, List<string>>() {
                    [1] = ["one", "two"],
                    [2] = ["three", "four"]
                },
                [2] = new Dictionary<int, List<string>>() {
                    [3] = ["five", "six"],
                    [4] = ["seven", "eight"]
                }
            };
            Assert.AreEqual("{(1 => {(1 => {one, two}), (2 => {three, four})}), (2 => {(3 => {five, six}), (4 => {seven, eight})})}",
                            dicOfDicOfList.ToStringExt());
        }

        [TestMethod]
        public void TestDictionaryOfListsOfDictionaries()
        {
            var dicOfListOfDic = new Dictionary<int, List<Dictionary<int, string>>>() {
                [1] = [
                    new Dictionary<int, string>() { [1] = "one", [2] = "two" },
                    new Dictionary<int, string>() { [3] = "three", [4] = "four" }
                ],
                [2] =
                [
                    new Dictionary<int, string>() { [5] = "five", [6] = "six" },
                    new Dictionary<int, string>() { [7] = "seven", [8] = "eight" }
                ]
            };
            Assert.AreEqual("{(1 => {{(1 => one), (2 => two)}, {(3 => three), (4 => four)}}), (2 => {{(5 => five), (6 => six)}, {(7 => seven), (8 => eight)}})}",
                            dicOfListOfDic.ToStringExt());
        }

        [TestMethod]
        public void TestDictionaryOfListsOfLists()
        {
            var dicOfListOfList = new Dictionary<int, List<List<string>>>() {
                [1] = [
                    ["one", "two"],
                    ["three", "four"]
                ],
                [2] = [
                    ["five", "six"],
                    ["seven", "eight"]
                ]
            };
            Assert.AreEqual("{(1 => {{one, two}, {three, four}}), (2 => {{five, six}, {seven, eight}})}",
                            dicOfListOfList.ToStringExt());
        }

        [TestMethod]
        public void TestListOfDictionariesOfLists()
        {
            var listOfDicOfList = new List<Dictionary<int, List<string>>>() {
                new() {
                    [1] = ["one", "two"],
                    [2] = ["three", "four"]
                },
                new() {
                    [3] = ["five", "six"],
                    [4] = ["seven", "eight"]
                }
            };
            Assert.AreEqual("{{(1 => {one, two}), (2 => {three, four})}, {(3 => {five, six}), (4 => {seven, eight})}}",
                            listOfDicOfList.ToStringExt());
        }

        [TestMethod]
        public void TestListOfListsOfDictionaries()
        {
            var listOfListOfDic = new List<List<Dictionary<int, string>>>()
            {
                new()
                {
                    new Dictionary<int, string>() { [1] = "one", [2] = "two" },
                    new Dictionary<int, string>() { [3] = "three", [4] = "four" }
                },
                new()
                {
                    new Dictionary<int, string>() { [5] = "five", [6] = "six" },
                    new Dictionary<int, string>() { [7] = "seven", [8] = "eight" }
                }
            };
            Assert.AreEqual("{{{(1 => one), (2 => two)}, {(3 => three), (4 => four)}}, {{(5 => five), (6 => six)}, {(7 => seven), (8 => eight)}}}",
                            listOfListOfDic.ToStringExt());
        }

        [TestMethod]
        public void TestHashSet()
        {
            var hashSet = new HashSet<string>() { "one", "two", "three" };
            Assert.AreEqual("{one, two, three}", hashSet.ToStringExt());
        }

        [TestMethod]
        public void TestCastToCollection()
        {
            object l = new List<string>() { "one", "two", "three" };
            Assert.IsInstanceOfType(l.CastTo<ICollection<string>>(),
                                    typeof(ICollection<string>));
            Assert.AreEqual("{one, two, three}", l.ToStringExt());
        }

        [TestMethod]
        public void TestCastToDictionary()
        {
            object l = new Dictionary<int, string>() {
                [1] = "one",
                [2] = "two",
                [3] = "three"
            };
            Assert.IsInstanceOfType(l.CastTo<Dictionary<int, string>>(),
                                    typeof(Dictionary<int, string>));
            Assert.AreEqual("{(1 => one), (2 => two), (3 => three)}", l.ToStringExt());
        }

        [TestMethod]
        public void TestCastToKeyValuePair()
        {
            object l = new KeyValuePair<int, string>(
                1, "one"
            );
            Assert.IsInstanceOfType(l.CastTo<KeyValuePair<int, string>>(),
                                    typeof(KeyValuePair<int, string>));
            Assert.AreEqual("(1 => one)", l.ToStringExt());
        }

        [TestMethod]
        public void TestCastToReflectedCollection()
        {
            object l = new List<string>() { "one", "two", "three" };
            Assert.IsInstanceOfType(l.CastToReflected(typeof(ICollection<string>)),
                                    typeof(ICollection<string>));
            Assert.AreEqual("{one, two, three}", l.ToStringExt());
        }

        [TestMethod]
        public void TestCastToReflectedDictionary()
        {
            object l = new Dictionary<int, string>()
            {
                [1] = "one",
                [2] = "two",
                [3] = "three"
            };
            Assert.IsInstanceOfType(l.CastToReflected(typeof(Dictionary<int, string>)),
                                    typeof(Dictionary<int, string>));
            Assert.AreEqual("{(1 => one), (2 => two), (3 => three)}", l.ToStringExt());
        }

        [TestMethod]
        public void TestCastToReflectedKeyValuePair()
        {
            object l = new KeyValuePair<int, string>(
                1, "one"
            );
            Assert.IsInstanceOfType(l.CastToReflected(typeof(KeyValuePair<int, string>)),
                                    typeof(KeyValuePair<int, string>));
            Assert.AreEqual("(1 => one)", l.ToStringExt());
        }
    }
}
