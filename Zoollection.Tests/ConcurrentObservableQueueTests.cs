using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Zoollection.Tests
{
    [TestClass]
    public class ConcurrentObservableQueueTests
    {
        [TestMethod]
        public void TestConstructorCopiesCollection()
        {
            //Arrange.
            var list = new List<string> {"Glory", "leader", "riding", "topless", "on", "bear"};

            //Act.
            var queue = new ConcurrentObservableQueue<string>(list);

            //Assert.
            Assert.AreEqual(6, queue.Count);
        }

        [TestMethod]
        public void TestConstructorInitializesCorrectly()
        {
            //Arrange.
            var queue = new ConcurrentObservableQueue<string>();

            //Assert.
            Assert.AreEqual(0, queue.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestCopyToArrayIsNullThrowsArgumentNullException()
        {
            //Arrange.
            var queue = new ConcurrentObservableQueue<string>();

            //Act.
            queue.CopyTo(null, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestCopyToIndexIsNegativeThrowsArgumentOutOfRangeException()
        {
            //Arrange.
            var queue = new ConcurrentObservableQueue<string>();
            var arr = new[] { "string1" };

            //Act.
            queue.CopyTo(arr, -1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestCopyToIndexIsGreaterArrayLengthThrowsArgumentException()
        {
            //Arrange.
            var queue = new ConcurrentObservableQueue<string>();
            var arr = new[] { "kim", "jong", "un", "glory", "leader", "riding", "rainbow", "spitting", "unicorn" };

            //Act.
            queue.CopyTo(arr, 10);
        }

        [TestMethod]
        public void TestEnqueueIncreasesQueueCount()
        {
            //Arrange.
            var queue = new ConcurrentObservableQueue<string>();

            //Act.
            queue.Enqueue("Foo");
            int count1 = queue.Count;
            queue.Enqueue("Bar");
            int count2 = queue.Count;

            //Assert.
            Assert.AreEqual(1, count1);
            Assert.AreEqual(2, count2);
        }

        [TestMethod]
        public void TestEnqueueRaisesCollectionChangedCorrectly()
        {
            //Arrange.
            var queue = new ConcurrentObservableQueue<string>();
            var receivedEvents = new List<NotifyCollectionChangedEventArgs>();
            queue.CollectionChanged += delegate(object sender, NotifyCollectionChangedEventArgs args)
            {
                receivedEvents.Add(args);
            };   

            //Act.
            queue.Enqueue("Foo");
            queue.Enqueue("Bar");

            //Assert.
            Assert.AreEqual(2, receivedEvents.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Add, receivedEvents[0].Action);
            Assert.AreEqual("Foo", receivedEvents[0].NewItems[0]);
            Assert.AreEqual(NotifyCollectionChangedAction.Add, receivedEvents[1].Action);
            Assert.AreEqual("Bar", receivedEvents[1].NewItems[0]);
        }

        [TestMethod]
        public void TestEnqueueFromMultiThreadsRaisesCollectionChanged()
        {
            //Arrange.
            var queue = new ConcurrentObservableQueue<string>();
            var receivedEvents = new List<NotifyCollectionChangedEventArgs>();
            queue.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs args)
            {
                receivedEvents.Add(args);
            };

            //Act.
            var random = new Random();
            var numOfEnqueues = random.Next(25, 35);
            var threads = new Thread[numOfEnqueues];
            for (int i = 0; i < numOfEnqueues; i++)
            {
                var str = "foo" + i;
                var thread = new Thread(() => queue.Enqueue(str));
                thread.Start();
                threads[i] = thread;
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            //Assert.
            Assert.AreEqual(numOfEnqueues, receivedEvents.Count);
        }

        [TestMethod]
        public void TestTryPeekReturnsTrue()
        {
            //Arrange.
            var queue = new ConcurrentObservableQueue<string>();
            queue.Enqueue("Foo");
            queue.Enqueue("Bar");

            //Act.
            string item;
            var success = queue.TryPeek(out item);

            Assert.AreEqual("Foo", item);
            Assert.IsTrue(success, "Expected 'success' to be true but received false.");
        }

        [TestMethod]
        public void TestTryPeekReturnsFalse()
        {
            //Arrange.
            var queue = new ConcurrentObservableQueue<string>();

            //Act.
            string item;
            var success = queue.TryPeek(out item);

            Assert.IsNull(item, "Expected 'item to be null but received a value");
            Assert.IsFalse(success, "Expected 'success' to be false but received true.");
        }

        [TestMethod]
        public void TestTryDequeueDecreasesCountAndReturnsTrue()
        {
            //Arrange.
            var queue = new ConcurrentObservableQueue<string>();
            queue.Enqueue("Foo1");
            queue.Enqueue("Foo2");

            //Act.
            string item;
            var success = queue.TryDequeue(out item);

            Assert.AreEqual(1, queue.Count);
            Assert.IsTrue(success, "Expected 'success' to be true but received false.");
        }

        [TestMethod]
        public void TestTryDequeueRaisesCollectionChangedCorrectly()
        {
            //Arrange.
            var queue = new ConcurrentObservableQueue<string>();
            queue.Enqueue("Foo");
            queue.Enqueue("Bar");
            var receivedEvents = new List<NotifyCollectionChangedEventArgs>();
            queue.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs args)
            {
                receivedEvents.Add(args);
            };

            //Act.
            string item1, item2;
            bool success1 = queue.TryDequeue(out item1);
            bool success2 = queue.TryDequeue(out item2);

            //Assert.
            Assert.AreEqual(2, receivedEvents.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Remove, receivedEvents[0].Action);
            Assert.AreEqual(item1, receivedEvents[0].OldItems[0]);
            Assert.AreEqual(NotifyCollectionChangedAction.Remove, receivedEvents[1].Action);
            Assert.AreEqual(item2, receivedEvents[1].OldItems[0]);
            Assert.IsTrue(success1);
            Assert.IsTrue(success2);
        }

        [TestMethod]
        public void TestTryDequeueFromMultiThreadsRaisesCollectionChanged()
        {
            //Arrange.
            var queue = new ConcurrentObservableQueue<string>();
            var random = new Random();
            var numOfDequeuesEnqueues = random.Next(25, 35);
            for (int i = 0; i < numOfDequeuesEnqueues; i++)
            {
                queue.Enqueue("foo" + i);
            }

            var receivedEvents = new List<NotifyCollectionChangedEventArgs>();
            queue.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs args)
            {
                receivedEvents.Add(args);
            };

            //Act.
            var threads = new Thread[numOfDequeuesEnqueues];
            for (int i = 0; i < numOfDequeuesEnqueues; i++)
            {
                string item;
                var thread = new Thread(() => queue.TryDequeue(out item));
                thread.Start();
                threads[i] = thread;
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            //Assert.
            Assert.AreEqual(numOfDequeuesEnqueues, receivedEvents.Count);
        }
    }
}
