using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wodsoft.ExpressionWrapper.UnitTest
{
    [TestClass]
    public class WrapperTest
    {
        [TestMethod]
        public void ClassAndInterfaceWrap()
        {
            List<Member> items = new List<Member>()
            {
                new Member{ Username = "A" },
                new Member{ Username = "A" },
                new Member{ Username = "A" },
                new Member{ Username = "B" },
                new Member{ Username = "B" },
                new Member{ Username = "C" },
                new Member{ Username = "D" },
            };

            WrapperContext context = new WrapperContext();
            context.Set<IMember, Member>();

            var queryable = items.AsQueryable();
            var wrappedQueryable = queryable.Wrap<IMember, Member>(context);
            wrappedQueryable = wrappedQueryable.Where(t => t.Username == "A");
            var result1 = wrappedQueryable.ToArray();
            Assert.AreEqual(3, result1.Length);

            queryable = wrappedQueryable.Unwrap<IMember, Member>();
            var result2 = queryable.ToArray();
            Assert.AreEqual(3, result2.Length);
        }

        [TestMethod]
        public void PropertyToPropertyWrap()
        {
            List<Member> items = new List<Member>()
            {
                new Member{ Username2 = "A" },
                new Member{ Username2 = "A" },
                new Member{ Username2 = "A" },
                new Member{ Username2 = "B" },
                new Member{ Username2 = "B" },
                new Member{ Username2 = "C" },
                new Member{ Username2 = "D" },
            };

            WrapperContext context = new WrapperContext();
            context.Set<Member, string>(t => t.Username, t => t.Username2);

            var queryable = items.AsQueryable();
            queryable = queryable.Where(t => t.Username == "A").Wrap(context);
            var result = queryable.ToArray();
            Assert.AreEqual(3, result.Length);
        }

        [TestMethod]
        public void MethodToMethodWrap()
        {
            List<Member> items = new List<Member>()
            {
                new Member{ Username = "A", Username2 = "AA" },
                new Member{ Username = "A", Username2 = "AA" },
                new Member{ Username = "A", Username2 = "AA" },
                new Member{ Username = "B", Username2 = "BB" },
                new Member{ Username = "B", Username2 = "BB" },
                new Member{ Username = "C", Username2 = "CC" },
                new Member{ Username = "D", Username2 = "DD" },
            };

            WrapperContext context = new WrapperContext();
            context.Set<Member>("GetName", "GetName2");

            var queryable = items.AsQueryable();
            queryable = queryable.Where(t => t.GetName() == "AA").Wrap(context);
            var result = queryable.ToArray();
            Assert.AreEqual(3, result.Length);
        }


        [TestMethod]
        public void InterfaceMethodToClassMethodWrap()
        {
            List<Member> items = new List<Member>()
            {
                new Member{ Username = "A", Username2 = "AA" },
                new Member{ Username = "A", Username2 = "AA" },
                new Member{ Username = "A", Username2 = "AA" },
                new Member{ Username = "B", Username2 = "BB" },
                new Member{ Username = "B", Username2 = "BB" },
                new Member{ Username = "C", Username2 = "CC" },
                new Member{ Username = "D", Username2 = "DD" },
            };

            WrapperContext context = new WrapperContext();
            context.Set<IMember, Member>();
            context.Set<IMember, Member>("GetName", "GetName");

            Assert.AreEqual(0, items.AsQueryable().Wrap<IMember, Member>().Count(t => t.GetName() == "A"));

            var queryable = items.AsQueryable();
            var wrappedQueryable = queryable.Wrap<IMember, Member>(context);
            wrappedQueryable = wrappedQueryable.Where(t => t.GetName() == "A");
            var result1 = wrappedQueryable.ToArray();
            Assert.AreEqual(3, result1.Length);

            queryable = wrappedQueryable.Unwrap<IMember, Member>();
            var result2 = queryable.ToArray();
            Assert.AreEqual(3, result2.Length);
        }
    }
}
