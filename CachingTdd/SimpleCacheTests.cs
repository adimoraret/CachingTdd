using System;
using System.Threading.Tasks;
using NUnit.Framework;
namespace CachingTdd
{
    [TestFixture]
    public class SimpleCacheTests
    {
        private SimpleCache<object> _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new SimpleCache<object>();
        }

        [Test]
        public void ShouldGetNewValueWhenCacheIsEmpty()
        {
            var value = new object();
            Func<string, object> getNewValue = (key) => value;

            var result = _sut.Get("key", getNewValue);

            Assert.That(result, Is.EqualTo(value));
        }

        [Test]
        public void ShouldNotRefreshValueWhenItExistsInCache()
        {
            var myvalue = new object();
            Func<string, object> getNewValue = (key) => myvalue;

            var firstValue = _sut.Get("key", getNewValue);
            var secondValue = _sut.Get("key", getNewValue);

            Assert.That(firstValue, Is.EqualTo(secondValue));
            Assert.That(firstValue, Is.EqualTo(myvalue));
        }

        [Test]
        public void ShouldAddTwoItemsToCache()
        {
            var myvalue1 = new object();
            var myvalue2 = new object();
            Func<string, object> getNewValue1 = (key) => myvalue1;
            Func<string, object> getNewValue2 = (key) => myvalue2;

            var firstValue = _sut.Get("key1", getNewValue1);
            var secondValue = _sut.Get("key2", getNewValue2);

            Assert.That(firstValue, Is.EqualTo(myvalue1));
            Assert.That(secondValue, Is.EqualTo(myvalue2));
        }

        [Test]
        public void ShouldNotCallTheGetNewValueMethodMultipleTimes()
        {
            var numberOfCalls = 0;
            Func<string, object> getNewValue = (key) =>
            {
                numberOfCalls += 1;
                return new object();
            };
            var iterations = new[] { 1, 2, 3, 4 };
            Parallel.ForEach(iterations, iteration =>
            {
                _sut.Get("key", getNewValue);
            });

            Assert.That(numberOfCalls, Is.EqualTo(1));
        }
    }
}
