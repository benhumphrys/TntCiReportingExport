using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Kofax.ReleaseLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tnt.KofaxCapture.A6.TntExportPacsRel2;

namespace Tnt.KofaxCapture.A6.TntExportPacsRel2UnitTests
{
    [TestClass]
    public class KfxReleaseScriptUnitTest
    {
        private readonly List<string> _outputDirectories = new List<string>();
        private readonly List<string> _filesToDelete = new List<string>();

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        /// Pass the TestContext to the utility class so that we can wire-up the fake Kofax API object to the
        /// output logging.
        /// </summary>
        /// <param name="testContext"></param>
        [ClassInitialize]
        public static void UnitTestClassInitialize(TestContext testContext)
        {
            UnitTestUtility.TestContext = testContext;
        }

        /// <summary>
        /// Clear the database tables, ensure we have empty directories to use, and set %TEMP% to our test TEMP
        /// directory.
        /// </summary>
        [TestInitialize]
        public void ActionsBeforeEachTest()
        {
            Directory.CreateDirectory("A6Output");
            _outputDirectories.Add("A6Output");
        }

        /// <summary>
        /// Delete files that may be created by the test, and reset the %TEMP% variable.
        /// </summary>
        [TestCleanup]
        public void ActionsAfterEachTest()
        {
            foreach (var filePath in _filesToDelete.Where(File.Exists))
            {
                File.Delete(filePath);
            }

            _filesToDelete.Clear();

            foreach (var directoryPath in _outputDirectories)
            {
                if (Directory.Exists(directoryPath))
                {
                    Directory.Delete(directoryPath, true);
                }

                //Directory.CreateDirectory(directoryPath);
            }

            _outputDirectories.Clear();
        }

        /// <summary>
        /// Test valid Consignment release.
        /// </summary>
        [TestMethod]
        public void TestValidConsignmentRelease()
        {
            // Simulate the release of a two-document batch.
            var releaseResult = SimulateScansRelease(release =>
            {
                //release.SkipCustomLog = true;
                release.DocumentData = UnitTestUtility.GetValidReleaseData();
                Assert.AreEqual(KfxReturnValue.KFX_REL_SUCCESS, release.ReleaseDoc());

                //release.DocumentData = UnitTestUtility.GetValidReleaseData(2);
                //Assert.AreEqual(KfxReturnValue.KFX_REL_SUCCESS, release.ReleaseDoc());
            });

            var bytes = File.ReadAllBytes(@"KfxReleaseScriptUnitTestResources\AuditReport_A15012016101444_BUD_09.xml");
            var auditHash = UnitTestUtility.GetMd5Hash(bytes);

            bytes = File.ReadAllBytes(@"KfxReleaseScriptUnitTestResources\BatchControl_B15012016101444_BUD_09_001.xml");
            var batchHash = UnitTestUtility.GetMd5Hash(bytes);

            // Check that the output is expected.  We can check MD5 hashes for the files (except audit files because
            // the content is dynamic).
            var pathsAndHashes = new[]
            {
                new
                {
                    FilePath = @"A6Output\AuditReport_A15012016101444_BUD_09.xml",
                    Hash = auditHash
                },
                new
                {
                    FilePath = @"A6Output\BatchControl_B15012016101444_BUD_09_001.xml",
                    Hash = batchHash
                },
                new
                {
                    FilePath = @"A6Output\AuditMarker_A15012016101444_BUD_09.mkr",
                    Hash = "D41D8CD98F00B204E9800998ECF8427E"
                },
                new
                {
                    FilePath = @"A6Output\BatchMarker_B15012016101444_BUD_09_001.mkr",
                    Hash = "D41D8CD98F00B204E9800998ECF8427E"
                },
                new
                {
                    FilePath = @"A6Output\BatchImage_B15012016101444_BUD_09_001.tif",
                    Hash = "5A59C8DB07D5843BE2CAAEE942F08BF3"
                }
            };

            foreach (var pathAndHash in pathsAndHashes)
            {
                Assert.IsTrue(File.Exists(pathAndHash.FilePath));

                bytes = File.ReadAllBytes(pathAndHash.FilePath);
                var hash = UnitTestUtility.GetMd5Hash(bytes);
                Assert.AreEqual(pathAndHash.Hash, hash, true);
            }

            Assert.IsTrue(File.Exists(GetMisAuditData(releaseResult).AuditFilePath));
        }

        /// <summary>
        /// Test valid two-document consignment release.
        /// </summary>
        [TestMethod]
        public void TestValid2DocConsignmentRelease()
        {
            // Simulate the release of a 2-document batch.
            var releaseResult = SimulateScansRelease(release =>
            {
                //release.SkipCustomLog = true;
                release.DocumentData = UnitTestUtility.GetValidReleaseData();
                Assert.AreEqual(KfxReturnValue.KFX_REL_SUCCESS, release.ReleaseDoc());

                release.DocumentData = UnitTestUtility.GetValidReleaseData(2);
                Assert.AreEqual(KfxReturnValue.KFX_REL_SUCCESS, release.ReleaseDoc());
            });

            var bytes = File.ReadAllBytes(@"KfxReleaseScriptUnitTestResources\AuditReport_A15012016101444_BUD_09.xml");
            var auditHash = UnitTestUtility.GetMd5Hash(bytes);

            // Check that the output is expected.  We can check MD5 hashes for the files (except audit files because
            // the content is dynamic).
            var pathsAndHashes = new[]
            {
                new
                {
                    FilePath = @"A6Output\AuditReport_A15012016101444_BUD_09.xml",
                    Hash = auditHash
                },
                new
                {
                    FilePath = @"A6Output\BatchControl_B15012016101444_BUD_09_001.xml",
                    Hash =
                        UnitTestUtility.GetMd5Hash(
                            File.ReadAllBytes(
                                @"KfxReleaseScriptUnitTestResources\BatchControl_B15012016101444_BUD_09_001.xml"))
                },
                new
                {
                    FilePath = @"A6Output\AuditMarker_A15012016101444_BUD_09.mkr",
                    Hash = "D41D8CD98F00B204E9800998ECF8427E"
                },
                new
                {
                    FilePath = @"A6Output\BatchMarker_B15012016101444_BUD_09_001.mkr",
                    Hash = "D41D8CD98F00B204E9800998ECF8427E"
                },
                new
                {
                    FilePath = @"A6Output\BatchImage_B15012016101444_BUD_09_001.tif",
                    Hash = "5A59C8DB07D5843BE2CAAEE942F08BF3"
                },
                new
                {
                    FilePath = @"A6Output\BatchControl_B15012016101444_BUD_09_002.xml",
                    Hash =
                        UnitTestUtility.GetMd5Hash(
                            File.ReadAllBytes(
                                @"KfxReleaseScriptUnitTestResources\BatchControl_B15012016101444_BUD_09_002.xml"))
                },
                new
                {
                    FilePath = @"A6Output\BatchMarker_B15012016101444_BUD_09_002.mkr",
                    Hash = "D41D8CD98F00B204E9800998ECF8427E"
                },
                new
                {
                    FilePath = @"A6Output\BatchImage_B15012016101444_BUD_09_002.tif",
                    Hash = "5A59C8DB07D5843BE2CAAEE942F08BF3"
                }
            };

            foreach (var pathAndHash in pathsAndHashes)
            {
                Assert.IsTrue(File.Exists(pathAndHash.FilePath));

                bytes = File.ReadAllBytes(pathAndHash.FilePath);
                var hash = UnitTestUtility.GetMd5Hash(bytes);
                Assert.AreEqual(pathAndHash.Hash, hash, true);
            }

            Assert.IsTrue(File.Exists(GetMisAuditData(releaseResult).AuditFilePath));
        }

        /// <summary>
        /// Test no document output
        /// </summary>
        [TestMethod]
        public void TestValidNoDocOutput()
        {
            // Simulate the release of a 2-document batch.
            var releaseResult = SimulateScansRelease(release =>
            {
                //release.SkipCustomLog = true;
                release.DocumentData = UnitTestUtility.GetValidReleaseData(pacsDocumentId: "");
                Assert.AreEqual(KfxReturnValue.KFX_REL_SUCCESS, release.ReleaseDoc());

                release.DocumentData = UnitTestUtility.GetValidReleaseData(2, pacsDocumentId: "");
                Assert.AreEqual(KfxReturnValue.KFX_REL_SUCCESS, release.ReleaseDoc());
            });
            
            Assert.IsTrue(File.Exists(GetMisAuditData(releaseResult).AuditFilePath));
        }

        private KfxReleaseScript SimulateScansRelease(Action<KfxReleaseScript> scriptLogic,
            KfxReturnValue expectedOpenScriptValue = KfxReturnValue.KFX_REL_SUCCESS,
            KfxReturnValue expectedCloseScriptValue = KfxReturnValue.KFX_REL_SUCCESS)
        {
            var release = new KfxReleaseScript { DocumentData = UnitTestUtility.GetValidReleaseData() };
            Assert.AreEqual(expectedOpenScriptValue, release.OpenScript());

            scriptLogic(release);

            var auditDetails = GetMisAuditData(release);
            _outputDirectories.Add(Path.GetDirectoryName(auditDetails.AuditFilePath));

            Assert.AreEqual(expectedCloseScriptValue, release.CloseScript());
            return release;
        }

        private MisAuditData GetMisAuditData(KfxReleaseScript release)
        {
            var misAuditData = (MisAuditData)new PrivateObject(release, "_misAuditData").Target;
            return misAuditData;
        }
    }
}
