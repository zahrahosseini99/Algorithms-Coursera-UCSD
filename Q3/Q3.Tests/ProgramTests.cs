using Microsoft.VisualStudio.TestTools.UnitTesting;
using ta_class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace program.Tests
{
    [TestClass()]
    public class ProgramTests
    {
        [TestMethod()]
        public void SearchTest()
        {
            string text = "aabxaabxcaabxaabxay";
            string patern = "aabx";
            int[] resualts = { 0, 4, 9,13};

            List<int> res = Program.search(text, patern).ToList();
            CollectionAssert.AreEqual(resualts, res);

  
        }
    }
}