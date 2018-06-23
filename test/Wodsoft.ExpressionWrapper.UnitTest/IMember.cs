using System;
using System.Collections.Generic;
using System.Text;

namespace Wodsoft.ExpressionWrapper.UnitTest
{
    public interface IMember
    {
        string Username { get; set; }

        string Password { get; set; }

        string GetName();
    }
}
