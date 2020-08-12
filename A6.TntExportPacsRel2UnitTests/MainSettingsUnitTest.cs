using System.IO;
using System.Linq;
using FakeItEasy;
using Kofax.ReleaseLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tnt.KofaxCapture.A6.TntExportPacsRel;

namespace Tnt.KofaxCapture.A6.TntExportPacsRelUnitTests
{
    [TestClass]
    public class MainSettingsUnitTest
    {
        private TestContext _testContext;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get { return _testContext; }
            set
            {
                _testContext = value;
                UnitTestUtility.TestContext = value;
            }
        }

        /// <summary>
        /// Clear the database tables, ensure we have empty directories to use, and set %TEMP% to our test TEMP
        /// directory.
        /// </summary>
        [TestInitialize]
        public void ActionsBeforeEachTest()
        {
            Directory.CreateDirectory("A6Output");
        }

        [TestMethod]
        public void TestValidSetupSettings()
        {
            var setupData = UnitTestUtility.GetValidSetupData();
            var settings = new MainSettings(setupData);

            Assert.IsTrue(settings.IsSetupDataValid());
        }

        [TestMethod]
        public void TestDefaultNewSetupSettings()
        {
            var setupData = UnitTestUtility.GetDefaultSetupData();
            var settings = new MainSettings(setupData);

            Assert.IsFalse(settings.IsSetupDataValid());
            OutputSettingErrorMessages(settings);
        }

        private void OutputSettingErrorMessages(MainSettings settings)
        {
            var allErrors = new[]
            {
                settings.SetupDataErrors,
            };

            var messages = allErrors.SelectMany(e => e);

            foreach (var message in messages)
            {
                TestContext.WriteLine(message);
            }
        }

        [TestMethod]
        public void TestInvalidDefaultNewSetupSettings()
        {
            var setupData = UnitTestUtility.GetDefaultSetupDataMissingFields();
            var settings = new MainSettings(setupData);

            Assert.IsFalse(settings.IsSetupDataValid());
            OutputSettingErrorMessages(settings);
        }

        [TestMethod]
        public void TestValidReleaseSettings()
        {
            var releaseData = UnitTestUtility.GetValidReleaseData();
            A.CallTo(() => releaseData.SendMessage(A<string>.Ignored, A<int>.Ignored, A<KfxInfoReturnValue>.Ignored)).
                Invokes(c => TestContext.WriteLine(c.GetArgument<string>(0)));

            var settings = new MainSettings(releaseData);
            Assert.IsNotNull(settings);
        }
    }
}
