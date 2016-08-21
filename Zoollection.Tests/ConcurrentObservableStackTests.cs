using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;

namespace Zoollection.Tests
{
    [TestClass]
    public class ConcurrentObservableStackTests
    {
        [TestMethod]
        public void TestDefaultConstuctorInitializesCorrectly()
        {
            //Act.
            var stack = new ConcurrentObservableStack<string>();

            //Assert.
            Assert.AreEqual(0, stack.Count);
        }

        [TestMethod]
        public void TestConstructorCopiesCollection()
        {
            //Arrange.
            var list = new List<string> { "i1", "i2" };

            //Act.
            var stack = new ConcurrentObservableStack<string>(list);

            //Assert.
            Assert.AreEqual(2, stack.Count);
        }

        [TestMethod]
        public void TestClearRaisesCollectionChangedCorrectly()
        {
            //Arrange.
            var list = new List<string> { "i1", "i2" };
            var stack = new ConcurrentObservableStack<string>(list);
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
        public void TestClearClearsStackCorrectly()
        {
            //Arrange.
            var list = new List<string> { "i1", "i2" };
            var stack = new ConcurrentObservableStack<string>(list);

            //Act.
            stack.Clear();

            //Assert.
            Assert.AreEqual(0, stack.Count);
        }

        [TestMethod]
        public void TestClearOnEmptyStackDoesNotRaiseCollectionChanged()
        {
            //Arrange.
            var stack = new ConcurrentObservableStack<string>();
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
        public void TestPushRaisesCollectionChangedCorrectly()
        {
            //Arrange.
            var stack = new ConcurrentObservableStack<string>();
            var receivedEvents = new List<NotifyCollectionChangedEventArgs>();
            stack.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs args)
            {
                receivedEvents.Add(args);
            };

            //Act.
            stack.Push("Foo");
            stack.Push("Bar");

            //Assert.
            Assert.AreEqual(2, receivedEvents.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Add, receivedEvents[0].Action);
            Assert.AreEqual("Foo", receivedEvents[0].NewItems[0]);
            Assert.AreEqual(NotifyCollectionChangedAction.Add, receivedEvents[1].Action);
            Assert.AreEqual("Bar", receivedEvents[1].NewItems[0]);
        }

        [TestMethod]
        public void TestPushFromMultiThreadsRaisesCollectionChanged()
        {
            //Arrange.
            var stack = new ConcurrentObservableStack<string>();
            var receivedEvents = new List<NotifyCollectionChangedEventArgs>();
            stack.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs args)
            {
                receivedEvents.Add(args);
            };

            //Act.
            var random = new Random();
            var numOfPushes = random.Next(25, 35);
            var threads = new Thread[numOfPushes];
            for (int i = 0; i < numOfPushes; i++)
            {
                var str = "foo" + i;
                var thread = new Thread(() => stack.Push(str));
                thread.Start();
                threads[i] = thread;
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            //Assert.
            Assert.AreEqual(numOfPushes, receivedEvents.Count);
        }

        [TestMethod]
        public void TestPushIncreasesQueueCount()
        {
            //Arrange.
            var stack = new ConcurrentObservableStack<string>();

            //Act.
            stack.Push("Foo");
            int count1 = stack.Count;
            stack.Push("Bar");
            int count2 = stack.Count;

            //Assert.
            Assert.AreEqual(1, count1);
            Assert.AreEqual(2, count2);
        }

        [TestMethod]
        public void TestPushRangeRaisesCollectionChangedCorrectly()
        {
            //Arrange.
            var stack = new ConcurrentObservableStack<string>();
            var receivedEvents = new List<NotifyCollectionChangedEventArgs>();
            stack.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs args)
            {
                receivedEvents.Add(args);
            };

            //Act.
            stack.PushRange(new[] { "Foo1", "Foo2" });
            stack.PushRange(new[] { "Bar1", "Bar2" });

            //Assert.
            Assert.AreEqual(2, receivedEvents.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Add, receivedEvents[0].Action);
            Assert.AreEqual("Foo1", receivedEvents[0].NewItems[0]);
            Assert.AreEqual("Foo2", receivedEvents[0].NewItems[1]);
            Assert.AreEqual(NotifyCollectionChangedAction.Add, receivedEvents[1].Action);
            Assert.AreEqual("Bar1", receivedEvents[1].NewItems[0]);
            Assert.AreEqual("Bar2", receivedEvents[1].NewItems[1]);
        }

        [TestMethod]
        public void TestPushRangeFromMultiThreadsRaisesCollectionChanged()
        {
            //Arrange.
            var stack = new ConcurrentObservableStack<string>();
            var receivedEvents = new List<NotifyCollectionChangedEventArgs>();
            stack.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs args)
            {
                receivedEvents.Add(args);
            };

            //Act.
            var random = new Random();
            var numOfPushes = random.Next(25, 35);
            var threads = new Thread[numOfPushes];
            for (int i = 0; i < numOfPushes; i++)
            {
                var str1 = "foo" + i;
                var str2 = "bar" + i;
                var thread = new Thread(() => stack.PushRange(new[] { str1, str2 }));
                thread.Start();
                threads[i] = thread;
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            //Assert.
            Assert.AreEqual(numOfPushes, receivedEvents.Count);
        }

        [TestMethod]
        public void TestPushRangeIncreasesQueueCount()
        {
            //Arrange.
            var stack = new ConcurrentObservableStack<string>();

            //Act.
            stack.PushRange(new[] { "Foo1", "Foo2" });
            int count1 = stack.Count;

            stack.PushRange(new[] { "Bar1", "Bar2" });
            int count2 = stack.Count;

            //Assert.
            Assert.AreEqual(2, count1);
            Assert.AreEqual(4, count2);
        }

        [TestMethod]
        public void TestPushRangeSubArrayRaisesCollectionChangedCorrectly()
        {
            //Arrange.
            var stack = new ConcurrentObservableStack<string>();
            var receivedEvents = new List<NotifyCollectionChangedEventArgs>();
            stack.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs args)
            {
                receivedEvents.Add(args);
            };

            //Act.
            stack.PushRange(new[] { "Foo1", "Foo2", "Foo3", "Foo4" }, 2, 2);
            stack.PushRange(new[] { "Bar1", "Bar2", "Bar3", "Bar4" }, 2, 2);

            //Assert.
            Assert.AreEqual(2, receivedEvents.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Add, receivedEvents[0].Action);
            Assert.AreEqual("Foo3", receivedEvents[0].NewItems[0]);
            Assert.AreEqual("Foo4", receivedEvents[0].NewItems[1]);
            Assert.AreEqual(NotifyCollectionChangedAction.Add, receivedEvents[1].Action);
            Assert.AreEqual("Bar3", receivedEvents[1].NewItems[0]);
            Assert.AreEqual("Bar4", receivedEvents[1].NewItems[1]);
        }

        [TestMethod]
        public void TestPushRangeSubArrayFromMultiThreadsRaisesCollectionChanged()
        {
            //Arrange.
            var stack = new ConcurrentObservableStack<string>();
            var receivedEvents = new List<NotifyCollectionChangedEventArgs>();
            stack.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs args)
            {
                receivedEvents.Add(args);
            };

            //Act.
            var random = new Random();
            var numOfPushes = random.Next(25, 35);
            var threads = new Thread[numOfPushes];
            for (int i = 0; i < numOfPushes; i++)
            {
                var str1 = "foo" + i;
                var str2 = "bar" + i;
                var str3 = "uni" + i;
                var str4 = "corn" + i;
                var thread = new Thread(() => stack.PushRange(new[] { str1, str2, str3, str4 }, 2, 2));
                thread.Start();
                threads[i] = thread;
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            //Assert.
            Assert.AreEqual(numOfPushes, receivedEvents.Count);
        }

        [TestMethod]
        public void TestPushRangeSubArrayIncreasesQueueCount()
        {
            //Arrange.
            var stack = new ConcurrentObservableStack<string>();

            //Act.
            stack.PushRange(new[] { "Foo1", "Foo2", "Foo3", "Foo4" }, 2, 2);
            int count1 = stack.Count;
            stack.PushRange(new[] { "Bar1", "Bar2", "Bar3", "Bar4" }, 2, 2);
            int count2 = stack.Count;

            //Assert.
            Assert.AreEqual(2, count1);
            Assert.AreEqual(4, count2);
        }

        [TestMethod]
        public void TestTryPopRaisesCollectionChangedCorrectly()
        {
            //Arrange.
            var list = new List<string> { "hello", "world" };
            var stack = new ConcurrentObservableStack<string>(list);
            var receivedEvents = new List<NotifyCollectionChangedEventArgs>();
            stack.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs args)
            {
                receivedEvents.Add(args);
            };

            //Act.
            string item1, item2;
            stack.TryPop(out item1);
            stack.TryPop(out item2);

            Assert.AreEqual(2, receivedEvents.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Remove, receivedEvents[0].Action);
            Assert.AreEqual(NotifyCollectionChangedAction.Remove, receivedEvents[1].Action);
            Assert.AreEqual(item1, receivedEvents[0].OldItems[0]);
            Assert.AreEqual(item2, receivedEvents[1].OldItems[0]);
        }

        [TestMethod]
        public void TestTryPopFromMultiThreadsRaisesCollectionChanged()
        {
            //Arrange.
            var stack = new ConcurrentObservableStack<string>();
            var random = new Random();
            var numOfPushesPops = random.Next(25, 35);
            for (int i = 0; i < numOfPushesPops; i++)
            {
                stack.Push("foo" + i);
            }

            var receivedEvents = new List<NotifyCollectionChangedEventArgs>();
            stack.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs args)
            {
                receivedEvents.Add(args);
            };

            //Act.
            var threads = new Thread[numOfPushesPops];
            for (int i = 0; i < numOfPushesPops; i++)
            {
                string item;
                var thread = new Thread(() => stack.TryPop(out item));
                thread.Start();
                threads[i] = thread;
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            //Assert.
            Assert.AreEqual(numOfPushesPops, receivedEvents.Count);
        }

        [TestMethod]
        public void TestTryPopRangeDoesNotRaiseCollectionChangedOnFailure()
        {
            //Arrange.
            var stack = new ConcurrentObservableStack<string>();
            var receivedEvents = new List<NotifyCollectionChangedEventArgs>();
            stack.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs args)
            {
                receivedEvents.Add(args);
            };

            //Act.
            string[] range = new string[3];
            int numOfPoppedItems = stack.TryPopRange(range);

            //Assert.
            Assert.AreEqual(0, numOfPoppedItems);
            Assert.AreEqual(0, receivedEvents.Count);
        }

        [TestMethod]
        public void TestTryPopRangeReturnsRemainingItemsIfRangeIsGreaterThanCount()
        {
            //Arrange.
            var list = new List<string> { "i1", "i2" };
            var stack = new ConcurrentObservableStack<string>(list);

            //Act.
            string[] range = new string[3];
            int count = stack.TryPopRange(range);

            //Assert.
            Assert.AreEqual(2, count);
        }

        [TestMethod]
        public void TestTryPopRangeRaisesCollectionChangedCorrectly()
        {
            //Arrange.
            var list = new List<string> { "hello", "world" };
            var stack = new ConcurrentObservableStack<string>(list);
            var receivedEvents = new List<NotifyCollectionChangedEventArgs>();
            stack.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs args)
            {
                receivedEvents.Add(args);
            };

            //Act.
            string[] items = new string[2];
            stack.TryPopRange(items);

            Assert.AreEqual(1, receivedEvents.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Remove, receivedEvents[0].Action);
            Assert.AreEqual("world", receivedEvents[0].OldItems[0]);
            Assert.AreEqual("hello", receivedEvents[0].OldItems[1]);
        }

        [TestMethod]
        public void TestTryPopRangeFromMultiThreadsRaisesCollectionChanged()
        {
            //Arrange.
            var stack = new ConcurrentObservableStack<string>();
            var numOfPushesPops = 30;
            for (int i = 0; i < numOfPushesPops; i++)
            {
                stack.Push("foo" + i);
            }

            var receivedEvents = new List<NotifyCollectionChangedEventArgs>();
            stack.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs args)
            {
                receivedEvents.Add(args);
            };

            //Act.
            var numOfPushesPops2 = numOfPushesPops/2;
            var threads = new Thread[numOfPushesPops2];
            for (int i = 0; i < numOfPushesPops2; i++)
            {
                string[] items = new string[2];
                var thread = new Thread(() => stack.TryPopRange(items));
                thread.Start();
                threads[i] = thread;
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            //Assert.
            Assert.AreEqual(numOfPushesPops2, receivedEvents.Count);
        }

        [TestMethod]
        public void TestTryPopRangeSubArrayDoesNotRaiseCollectionChangedOnFailure()
        {
            //Arrange.
            var stack = new ConcurrentObservableStack<string>();
            var receivedEvents = new List<NotifyCollectionChangedEventArgs>();
            stack.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs args)
            {
                receivedEvents.Add(args);
            };

            //Act.
            string[] range = new string[3];
            int numOfPoppedItems = stack.TryPopRange(range, 1, 2);

            //Assert.
            Assert.AreEqual(0, numOfPoppedItems);
            Assert.AreEqual(0, receivedEvents.Count);
        }

        [TestMethod]
        public void TestTryPopRangeSubArrayReturnsRemainingItemsIfRangeIsGreaterThanCount()
        {
            //Arrange.
            var list = new List<string> { "i1", "i2" };
            var stack = new ConcurrentObservableStack<string>(list);

            //Act.
            string[] range = new string[4];
            int count = stack.TryPopRange(range, 1, 3);

            //Assert.
            Assert.AreEqual(2, count);
        }

        [TestMethod]
        public void TestTryPopRangeSubArrayRaisesCollectionChangedCorrectly()
        {
            //Arrange.
            var list = new List<string> { "hello", "brave", "new", "world" };
            var stack = new ConcurrentObservableStack<string>(list);
            var receivedEvents = new List<NotifyCollectionChangedEventArgs>();
            stack.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs args)
            {
                receivedEvents.Add(args);
            };

            //Act.
            string[] items = new string[4];
            stack.TryPopRange(items, 2, 2);

            Assert.AreEqual(1, receivedEvents.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Remove, receivedEvents[0].Action);
            Assert.AreEqual("world", receivedEvents[0].OldItems[0]);
            Assert.AreEqual("new", receivedEvents[0].OldItems[1]);
        }

        [TestMethod]
        public void TestTryPopRangeSubArrayFromMultiThreadsRaisesCollectionChanged()
        {
            //Arrange.
            var stack = new ConcurrentObservableStack<string>();
            var numOfPushesPops = 30;
            for (int i = 0; i < numOfPushesPops; i++)
            {
                stack.Push("foo" + i);
            }

            var receivedEvents = new List<NotifyCollectionChangedEventArgs>();
            stack.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs args)
            {
                receivedEvents.Add(args);
            };

            //Act.
            var numOfPushesPops2 = numOfPushesPops / 2;
            var threads = new Thread[numOfPushesPops2];
            for (int i = 0; i < numOfPushesPops2; i++)
            {
                string[] items = new string[4];
                var thread = new Thread(() => stack.TryPopRange(items, 2, 2));
                thread.Start();
                threads[i] = thread;
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            //Assert.
            Assert.AreEqual(numOfPushesPops2, receivedEvents.Count);
        }
    }
}
