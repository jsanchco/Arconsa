using Microsoft.VisualStudio.TestTools.UnitTesting;
using SGDE.ReadingCertifications;

namespace SGDE.Tests.ReadingCerifications
{
    [TestClass]
    public class ReadingCerticationsTests
    {
        [TestMethod]
        public void ReadFile_ResultOk()
        {
            var serviceReadingCertifications = new ServiceReadingCertifications();
            serviceReadingCertifications.ReadFile();

            //Assert.AreNotEqual(newUserViewModel.roleId, 0);
        }
    }
}
