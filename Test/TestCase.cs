using java.lang;
using JavaPort;
using JavaPort.Collections;
using com.google.i18n.phonenumbers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Test
{
    public class TestCase
    {
        public void assertEquals(object test1, object test2)
        {
            if (test1 is ICollection && test2 is ICollection)
            {
                CollectionAssert.AreEqual((ICollection)test1, (ICollection)test2);
            }
            else Assert.AreEqual(test1, test2);
        }

        public void assertEquals(string test1, object test2)
        {
            Assert.AreEqual((String)test1, test2);
        }

        public void assertEquals(string message, object test1, object test2)
        {
            Assert.AreEqual(test1, test2, message);
        }

        public void assertTrue(bool test)
        {
            Assert.IsTrue(test);
        }

        public void assertTrue(string message, bool test)
        {
            Assert.IsTrue(test, message);
        }

        public void assertFalse(bool test)
        {
            Assert.IsFalse(test);
        }

        public void assertFalse(string message, bool test)
        {
            Assert.IsFalse(test, message);
        }

        public void assertNotNull(object test)
        {
            Assert.IsNotNull(test);
        }

        public void assertNull(object test)
        {
            Assert.IsNull(test);
        }

        public void fail()
        {
            Assert.Fail();
        }

        public void fail(string message)
        {
            Assert.Fail(message);
        }

        public void assertEqualRange(String text, int i, int p1, int p2)
        {
            throw new System.NotImplementedException();
        }

        public void assertNotNull(string p, object match)
        {
            Assert.IsNotNull(match, p);
        }

        public void assertSame(String p1, String p2)
        {
            Assert.AreSame(p1, p2);
        }

        internal void setUp()
        {
            throw new System.NotImplementedException();
        }

        internal void tearDown()
        {
            throw new System.NotImplementedException();
        }
    }
}
