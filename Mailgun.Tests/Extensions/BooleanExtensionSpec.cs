using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mailgun.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;

namespace Mailgun.Tests.Extensions
{
    [TestClass]
    public class BooleanExtensionSpec
    {
        public void TestBooleanNo()
        {
            const bool yesNo = false;
            yesNo.AsYesNo().ShouldEqual("no");
        }

        public void TestBooleanYes()
        {
            const bool yesNo = true;
            yesNo.AsYesNo().ShouldEqual("yes");
        }
    }
}