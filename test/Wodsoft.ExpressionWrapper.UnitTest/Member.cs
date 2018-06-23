using System;
using System.Collections.Generic;
using System.Text;

namespace Wodsoft.ExpressionWrapper.UnitTest
{
    public class Member : IMember
    {
        public string Username { get; set; }

        public string Username2 { get; set; }

        public string Password { get; set; }

        public string GetName()
        {
            return Username;
        }

        public string GetName2()
        {
            return Username2;
        }

        string IMember.GetName()
        {
            return Username2;
        }
    }
}
