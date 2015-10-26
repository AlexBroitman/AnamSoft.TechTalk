using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            const string str = "All,Active,Inactice,Cancelled,Deferred,InProgress";
            var state = WorkItemState.All;
            var arr = str.Split(',').Where(s => Enum.TryParse(s, true, out state)).Select(s => state).ToArray();
            Assert.AreEqual(3, arr.Length);
        }



        public enum WorkItemState
        {

            /// <remarks/>
            All,

            /// <remarks/>
            Active,

            /// <remarks/>
            Cancelled,

            /// <remarks/>
            Draft,

            /// <remarks/>
            Completed,

            /// <remarks/>
            OnHold,

            /// <remarks/>
            Current,

            /// <remarks/>
            Upcoming,

            /// <remarks/>
            InTheWorks,

            /// <remarks/>
            Executable,
        }
    }
}
