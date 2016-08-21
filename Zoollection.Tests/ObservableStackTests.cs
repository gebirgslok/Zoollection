using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Zoollection.Tests
{
    [TestClass]
    public class ObservableStackTests
    {
        [TestMethod]
        public void TestDefaultConstructorInitializesCorrectly()
        {
            //Act.
            var stack = new ObservableStack<string>();

            //Assert.
            Assert.AreEqual(0, stack.Count);
        }

        [TestMethod]
        public void TestConstructorCopiesCollection()
        {
            //Arrange.
            var list = new List<string> { "jet", "fuel", "cannot", "melt", "steel", "beams" };

            //Act.
            var stack = new ObservableStack<string>(list);

            //Assert.
            Assert.AreEqual(6, stack.Count);
        }

        [TestMethod]
        public void TestClearOnEmptyStackDoesNotRaiseCollectionChanged()
        {
            //Arrange.
            var stack = new ObservableStack<string>();
            var receivedEvents = new List<NotifyCollectionChangedEventArgs>();
            stack.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs args)
            {
                receivedEvents.Add(args);
            };

            //Act.
            stack.Clear();

            //Assert.
            Assert.AreEqual(0, receivedEvents.Count);
        }

        [TestMethod]
        public void TestClearRaisesCollectionChangedCorrectly()
        {
            //Arrange.
            var list = new List<string> { "item1", "item2", "item3" };
            var stack = new ObservableStack<string>(list);
            var receivedEvents = new List<NotifyCollectionChangedEventArgs>();
            stack.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs args)
            {
                receivedEvents.Add(args);
            };

            //Act.
            stack.Clear();

            //Assert.
            Assert.AreEqual(1, receivedEvents.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Reset, receivedEvents[0].Action);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestPopThrowsInvalidOperationExceptionIfStackIsEmpty()
        {
            //Arrange.
            var stack = new ObservableStack<string>();

            //Act.
            stack.Pop();
        }

        [TestMethod]
        public void TestPopRaisesCollectionChangedCorrectly()
        {
            //Arrange.
            var list = new List<string> { "item1", "item2" };
            var stack = new ObservableStack<string>(list);
            var receivedEvents = new List<NotifyCollectionChangedEventArgs>();
            stack.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs args)
            {
                receivedEvents.Add(args);
            };

            //Act.
            var item1 = stack.Pop();
            var item2 = stack.Pop();

            //Assert.
            Assert.AreEqual(2, receivedEvents.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Remove, receivedEvents[0].Action);
            Assert.AreEqual(NotifyCollectionChangedAction.Remove, receivedEvents[1].Action);
            Assert.AreEqual(item1, receivedEvents[0].OldItems[0]);
            Assert.AreEqual(item2, receivedEvents[1].OldItems[0]);
        }

        [TestMethod]
        public void TestPushRaisesCollectionChangedCorrectly()
        {
            //Arrange.
            var stack = new ObservableStack<string>();
            var receivedEvents = new List<NotifyCollectionChangedEventArgs>();
            stack.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs args)
            {
                receivedEvents.Add(args);
            };

            //Act.
            stack.Push("item1");
            stack.Push("item2");

            //Assert.
            Assert.AreEqual(2, receivedEvents.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Add, receivedEvents[0].Action);
            Assert.AreEqual(NotifyCollectionChangedAction.Add, receivedEvents[1].Action);
            Assert.AreEqual("item1", receivedEvents[0].NewItems[0]);
            Assert.AreEqual("item2", receivedEvents[1].NewItems[0]);
        }
    }
}
