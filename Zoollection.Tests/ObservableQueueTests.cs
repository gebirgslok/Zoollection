using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Zoollection.Tests
{
    [TestClass]
    public class ObservableQueueTests
    {
        [TestMethod]
        public void TestDefaultConstructorInitializesCorrectly()
        {
            //Act.
            var queue = new ObservableQueue<string>();

            //Assert.
            Assert.AreEqual(0, queue.Count);
        }

        [TestMethod]
        public void TestConstructorCopiesCollection()
        {
            //Arrange.
            var list = new List<string> { "i", "wish", "i", "could", "talk", "to", "ponys" };

            //Act.
            var queue = new ObservableQueue<string>(list);

            //Assert.
            Assert.AreEqual(7, queue.Count);
        }

        [TestMethod]
        public void TestEnqueueRaisesCollectionChangedCorrectly()
        {
            //Arrange.
            var queue = new ObservableQueue<string>();
            var receivedEvents = new List<NotifyCollectionChangedEventArgs>();
            queue.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs args)
            {
                receivedEvents.Add(args);
            };

            //Act.
            queue.Enqueue("EINTRACHT");
            queue.Enqueue("FRANKFURT");

            //Assert.
            Assert.AreEqual(2, receivedEvents.Count);
            Assert.AreEqual("EINTRACHT", receivedEvents[0].NewItems[0]);
            Assert.AreEqual("FRANKFURT", receivedEvents[1].NewItems[0]);
        }

        [TestMethod]
        public void TestClearDoesNotRaiseCollectionIfAlreadyEmpty()
        {
            //Arrange.
            var queue = new ObservableQueue<string>();
            var receivedEvents = new List<NotifyCollectionChangedEventArgs>();
            queue.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs args)
            {
                receivedEvents.Add(args);
            };

            //Act.
            queue.Clear();

            //Assert.
            Assert.AreEqual(0, receivedEvents.Count);
        }

        [TestMethod]
        public void TestClearRaisesCollectionChangedCorrectly()
        {
            //Arrange.
            var list = new List<string> { "string1", "string2" };
            var queue = new ObservableQueue<string>(list);
            var receivedEvents = new List<NotifyCollectionChangedEventArgs>();
            queue.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs args)
            {
                receivedEvents.Add(args);
            };

            //Act.
            queue.Clear();

            //Assert.
            Assert.AreEqual(1, receivedEvents.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Reset, receivedEvents[0].Action);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestDequeueEmptyQueueRaisesInvalidOperationException()
        {
            //Arrange.
            var queue = new ObservableQueue<string>();
            var receivedEvents = new List<NotifyCollectionChangedEventArgs>();
            queue.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs args)
            {
                receivedEvents.Add(args);
            };

            //Act.
            queue.Dequeue();
        }

        [TestMethod]
        public void TestDequeueRaisesCollectionChangedCorrectly()
        {
            //Arrange.
            var list = new List<string> { "item1", "item2" };
            var queue = new ObservableQueue<string>(list);
            var receivedEvents = new List<NotifyCollectionChangedEventArgs>();
            queue.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs args)
            {
                receivedEvents.Add(args);
            };

            //Act.
            string item1 = queue.Dequeue();
            string item2 = queue.Dequeue();

            //Assert.
            Assert.AreEqual(2, receivedEvents.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Remove, receivedEvents[0].Action);
            Assert.AreEqual(NotifyCollectionChangedAction.Remove, receivedEvents[1].Action);
            Assert.AreEqual(item1, receivedEvents[0].OldItems[0]);
            Assert.AreEqual(item2, receivedEvents[1].OldItems[0]);
        }
    }
}
