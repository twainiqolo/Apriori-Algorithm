﻿using AprioriAlgorithm;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace UnitTests
{
    [TestClass()]
    public class AprioriTest
    {
        #region Member Variables

        readonly double minSupport;
        readonly double minConfidence;
        readonly IEnumerable<string> items;
        readonly Dictionary<int, string> transactions;
        readonly Apriori_Accessor target;
        readonly int lastTransId; 

        #endregion

        public AprioriTest()
        {
            minSupport = .5;
            minConfidence = .8;
            items = new string[5] { "a", "b", "c", "d", "e" };
            transactions = new Dictionary<int, string>
            {
                {1,"acd"},
                {2,"bce"},
                {3,"abce"},
                {4,"be"}
            };
            target = new Apriori_Accessor();
            lastTransId = 4;
        }

        #region Test Methods

        [TestMethod()]
        public void SolveTest()
        {
            //Arrange
            IApriori target = new Apriori();


            //Act
            Output actual = target.Solve(minSupport, minConfidence, items, transactions);

            //Assert
            Assert.AreEqual(9, actual.AllFrequentItems.Count);
            Assert.AreEqual(2, actual.AllFrequentItems["a"]);
            Assert.AreEqual(3, actual.AllFrequentItems["b"]);
            Assert.AreEqual(3, actual.AllFrequentItems["c"]);
            Assert.AreEqual(3, actual.AllFrequentItems["e"]);
            Assert.AreEqual(2, actual.AllFrequentItems["ac"]);
            Assert.AreEqual(2, actual.AllFrequentItems["bc"]);
            Assert.AreEqual(3, actual.AllFrequentItems["be"]);
            Assert.AreEqual(2, actual.AllFrequentItems["ce"]);
            Assert.AreEqual(2, actual.AllFrequentItems["bce"]);

            Assert.AreEqual(4, actual.ClosedItemSets.Count);
            Assert.IsTrue(actual.ClosedItemSets.ContainsKey("c"));
            Assert.IsTrue(actual.ClosedItemSets.ContainsKey("be"));
            Assert.IsTrue(actual.ClosedItemSets.ContainsKey("ac"));
            Assert.IsTrue(actual.ClosedItemSets.ContainsKey("bce"));

            Assert.AreEqual(2, actual.MaximalItemSets.Count);
            Assert.AreEqual("ac", actual.MaximalItemSets[0]);
            Assert.AreEqual("bce", actual.MaximalItemSets[1]);

            Assert.AreEqual(5, actual.StrongRules.Count);
        }

        [TestMethod()]
        public void GetL1FrequentItemsTest()
        {
            //Act
            var actual = target.GetL1FrequentItems(minSupport, items, transactions);

            //Assert
            Assert.AreEqual(4, actual.Count);
            Assert.AreEqual(2, actual["a"]);
            Assert.AreEqual(3, actual["b"]);
            Assert.AreEqual(3, actual["c"]);
            Assert.AreEqual(3, actual["e"]);
        }

        [TestMethod()]
        public void GetSupportTest()
        {
            //Arrange
            string candidate = "a";

            //Act
            var actual = target.GetSupport(candidate, transactions);

            //Assert
            Assert.AreEqual(2, actual);
        }

        [TestMethod()]
        public void CheckIsSubsetTest()
        {
            //Arrange
            string child = "abc";
            string parent = "abcde";

            //Act
            var actual = target.CheckIsSubset(child, parent);

            //Assert
            Assert.IsTrue(actual);
        }

        [TestMethod()]
        public void GenerateCandidatesTest()
        {
            //Arrange
            var frequentItems = new Dictionary<string, double> 
            {
                {"a",2},
                {"b",3},
                {"c",3},
                {"e",3}
            };

            //Act
            var actual = target.GenerateCandidates(frequentItems, transactions);

            //Assert
            Assert.AreEqual(actual.Count, 6);

            Assert.AreEqual(actual["ab"], 1);
            Assert.AreEqual(actual["ac"], 2);
            Assert.AreEqual(actual["ae"], 1);
            Assert.AreEqual(actual["bc"], 2);
            Assert.AreEqual(actual["be"], 3);
            Assert.AreEqual(actual["ce"], 2);
        }

        [TestMethod()]
        public void AlphabetizeTest()
        {
            //Arrange
            var token = "cba";

            //Act
            var actual = target.Sort(token);

            //Assert
            Assert.AreEqual(actual, "abc");
        }

        [TestMethod()]
        public void GetCandidateTest()
        {
            //Act
            var actual = target.GetCandidate("be", "bc");

            //Assert
            Assert.AreEqual(actual, "bec");

            //Act
            actual = target.GetCandidate("a", "b");

            //Assert
            Assert.AreEqual(actual, "ab");

            //Act
            actual = target.GetCandidate("ce", "be");

            //Assert
            Assert.AreEqual(actual, string.Empty);
        }

        [TestMethod()]
        public void GetFrequentItemsTest()
        {
            //Arrange
            var candidates = new Dictionary<string, double>
            {
                {"ab", 1},
                {"ac", 2},
                {"ae", 1},
                {"bc", 2},
                {"be", 3},
                {"ce", 2} 
            };

            //Act
            var actual = target.GetFrequentItems(candidates, minSupport, lastTransId);

            //Assert
            Assert.AreEqual(actual.Count, 4);

            Assert.AreEqual(actual["ce"], 2);
            Assert.AreEqual(actual["be"], 3);
            Assert.AreEqual(actual["bc"], 2);
            Assert.AreEqual(actual["ac"], 2);
        }

        [TestMethod()]
        public void GetClosedItemSetsTest()
        {
            //Arrange
            var allFrequentItems = new Dictionary<string, double>
            {
                {"a", 2},
                {"b", 3},
                {"c", 3},
                {"e", 3},
                {"ce", 2},
                {"be", 3},
                {"bc", 2},
                {"ac", 2},
                {"bce", 2}
            };

            //Act
            var actual = target.GetClosedItemSets(allFrequentItems);

            //Assert
            Assert.AreEqual(4, actual.Count);

            Assert.IsTrue(actual.ContainsKey("c"));
            Assert.IsTrue(actual.ContainsKey("be"));
            Assert.IsTrue(actual.ContainsKey("ac"));
            Assert.IsTrue(actual.ContainsKey("bce"));
        } 

        #endregion
    }
}