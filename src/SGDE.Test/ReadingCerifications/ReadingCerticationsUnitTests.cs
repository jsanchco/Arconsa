using Microsoft.VisualStudio.TestTools.UnitTesting;
using SGDE.ReadingCertifications.Service;

namespace SGDE.Tests.ReadingCerifications
{
    [TestClass]
    public class ReadingCerticationsUnitTests
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
