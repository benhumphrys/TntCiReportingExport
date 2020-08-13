using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using FakeItEasy;
using Kofax.ReleaseLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tnt.KofaxCapture.A6.TntExportPacsRel2UnitTests
{
    internal static class UnitTestUtility
    {
        private const KfxLinkSourceType KofaxVar = KfxLinkSourceType.KFX_REL_VARIABLE;
        private const KfxLinkSourceType BatchVar = KfxLinkSourceType.KFX_REL_BATCHFIELD;
        private const KfxLinkSourceType IndexVar = KfxLinkSourceType.KFX_REL_INDEXFIELD;

        private const KfxIndexFieldType Varchar = KfxIndexFieldType.SQL_VARCHAR;

        public static TestContext TestContext { get; set; }

        public static ReleaseData GetValidReleaseData(int docId = 1, string batchClassName = "A7 Test Batch Class", string docClassName = "A7 Test Document Class", string scanDepot = "BUD", string scanDateTime = "15/01/2016 10:14:44", string docStatus = "SUCCESS", string conNumber = "966210891", string documentType = "1", string pageCount = "1", string reject = "NO", string pacsDocumentId = "1")
        {
            var batchName = "B15012016101444_BUD_09"; 
            var releaseData = A.Fake<ReleaseData>();
            A.CallTo(() => releaseData.Name).Returns("A6 Test Export");
            A.CallTo(() => releaseData.BatchClassName).Returns(batchClassName);
            A.CallTo(() => releaseData.BatchName).Returns(batchName);
            A.CallTo(() => releaseData.UniqueDocumentID).Returns(docId);
            A.CallTo(() => releaseData.DocClassName).Returns(docClassName);

            var values = new MockValues();
            A.CallTo(() => releaseData.Values).Returns(values);

            var mockValues = new List<MockValue>
            {
                new MockValue {SourceName = "B_MISA6AuditPath", SourceType = BatchVar, Value = "AuditA6"},
                new MockValue {SourceName = "B_DocumentType", SourceType = BatchVar, Value = documentType},
                new MockValue {SourceName = "B_ScanDepot", SourceType = BatchVar, Value = scanDepot},
                new MockValue {SourceName = "B_ScanDateTime", SourceType = BatchVar, Value = scanDateTime},
                new MockValue {SourceName = "B_ReScanFlag", SourceType = BatchVar, Value = "0"},
                new MockValue {SourceName = "B_WorkstationName", SourceType = BatchVar, Value = "VMXPBHTNT"},
                new MockValue {SourceName = "B_BlankSheetsScanned", SourceType = BatchVar, Value = "0"},
                new MockValue {SourceName = "B_ParentBatchName", SourceType = BatchVar, Value = "PARENTBATCHNAMETESET"},
                new MockValue {SourceName = "Send to 3rd Party", SourceType = BatchVar, Value = ""},
                new MockValue {SourceName = "B_StoreinDMS", SourceType = BatchVar, Value = "1"},
                new MockValue {SourceName = "Action", SourceType = BatchVar, Value = ""},
                new MockValue {SourceName = "Batch Type", SourceType = BatchVar, Value = ""},
                new MockValue {SourceName = "B_DomainAndUserName", SourceType = BatchVar, Value = "VMXPBHTNT\\Administrator"},
                new MockValue {SourceName = "B_RoundID", SourceType = BatchVar, Value = "123"},
                new MockValue {SourceName = "DocStatus", SourceType = IndexVar, Value = docStatus},
                new MockValue {SourceName = "Reject", SourceType = IndexVar, Value = reject},
                new MockValue {SourceName = "PACSDocumentID", SourceType = IndexVar, Value = pacsDocumentId},
                new MockValue {SourceName = "ConNumber", SourceType = IndexVar, Value = conNumber},
                new MockValue {SourceName = "TotalNumberOfPages", SourceType = IndexVar, Value = pageCount},
                new MockValue {SourceName = "AutoIndexedFlag", SourceType = IndexVar, Value = "1"},
                new MockValue {SourceName = "ExternalBatchName", SourceType = IndexVar, Value = batchName},
                new MockValue {SourceName = "Batch Class Name", SourceType = KofaxVar, Value = batchClassName},
                new MockValue {SourceName = "Batch Creation Date", SourceType = KofaxVar, Value = "02/05/2014"},
                new MockValue {SourceName = "Document Class Name", SourceType = KofaxVar, Value = docClassName},
                new MockValue {SourceName = "Document Count", SourceType = KofaxVar, Value = "2"},
                new MockValue {SourceName = "Document Form Name", SourceType = KofaxVar, Value = "A5A6 Test Form Type"},
                new MockValue {SourceName = "Document GUID", SourceType = KofaxVar, Value = Guid.NewGuid().ToString()},
                new MockValue {SourceName = "Document ID", SourceType = KofaxVar, Value = docId.ToString(CultureInfo.InvariantCulture)},
                new MockValue {SourceName = "First Page Endorsing String", SourceType = KofaxVar, Value = ""},
                new MockValue {SourceName = "First Page Image Address", SourceType = KofaxVar, Value = ""},
                new MockValue {SourceName = "First Page Original File Name", SourceType = KofaxVar, Value = ""},
                new MockValue {SourceName = "First Page Roll Number", SourceType = KofaxVar, Value = ""},
                new MockValue {SourceName = "Index Operator's Station ID", SourceType = KofaxVar, Value = "TEST1"},
                new MockValue {SourceName = "Batch Creation Time", SourceType = KofaxVar, Value = "13:37:22"},
                new MockValue {SourceName = "Operator Description", SourceType = KofaxVar, Value = ""},
                new MockValue {SourceName = "Operator Name", SourceType = KofaxVar, Value = "Scansation"},
                new MockValue {SourceName = "Operator User ID", SourceType = KofaxVar, Value = "Scansation"},
                new MockValue {SourceName = "Queue", SourceType = KofaxVar, Value = ""},
                new MockValue {SourceName = "Remote Validation User ID", SourceType = KofaxVar, Value = "Scansation"},
                new MockValue {SourceName = "Scan Operator's Station ID", SourceType = KofaxVar, Value = "TEST1"},
                new MockValue {SourceName = "Scan Operator's User ID", SourceType = KofaxVar, Value = "Scansation"},
                new MockValue {SourceName = "Sequence Number", SourceType = KofaxVar, Value = ""},
                new MockValue {SourceName = "Site ID", SourceType = KofaxVar, Value = "555"},
                new MockValue {SourceName = "Batch Creator's Station ID", SourceType = KofaxVar, Value = "TEST1"},
                new MockValue {SourceName = "Station ID", SourceType = KofaxVar, Value = "TEST1"},
                new MockValue {SourceName = "User Name", SourceType = KofaxVar, Value = "Scansation"},
                new MockValue {SourceName = "UTC Offset", SourceType = KofaxVar, Value = ""},
                new MockValue {SourceName = "Validation Operator's Station ID", SourceType = KofaxVar, Value = "TEST1"},
                new MockValue {SourceName = "Batch Description", SourceType = KofaxVar, Value = ""},
                new MockValue {SourceName = "Batch ID", SourceType = KofaxVar, Value = "1"},
                new MockValue {SourceName = "Batch Name", SourceType = KofaxVar, Value = batchName},
                new MockValue {SourceName = "Batch Reference ID", SourceType = KofaxVar, Value = ""},
                new MockValue {SourceName = "Current Date", SourceType = KofaxVar, Value = DateTime.Now.ToShortDateString()},
                new MockValue {SourceName = "Current Time", SourceType = KofaxVar, Value = DateTime.Now.ToShortTimeString()}
            };
            
            foreach (var field in mockValues)
            {
                values.Add(field, field.SourceName + field.SourceType);
            }

            var imageFile = new MockImageFile(docId) {FileName = @"KfxReleaseScriptUnitTestResources\00000008.tif"};
            var imageFiles = new MockImageFiles {{imageFile, imageFile.FileName}};
            A.CallTo(() => releaseData.ImageFiles).Returns(imageFiles);

            A.CallTo(() => releaseData.KofaxPDFReleaseScriptEnabled).Returns(true);
            A.CallTo(() => releaseData.KofaxPDFPath).Returns("A6Output");
            A.CallTo(() => releaseData.CopyKofaxPDFFileToPath(A<string>.Ignored)).Invokes(c =>
            {
                var docIdHex = docId.ToString("X8");
                var fileName = docIdHex + "." + "pdf";
                var filePath = Path.Combine(c.GetArgument<string>(0), fileName);
                File.Copy(@"KfxReleaseScriptUnitTestResources\00000008.pdf", filePath, true);
            });

            A.CallTo(() => releaseData.ImageFilePath).Returns("A6Output");

            ApplyWorkingCustomPropertiesCollectionToReleaseData(releaseData);
            AddRequiredCustomPropertyElementsToReleaseData(releaseData);

            A.CallTo(() => releaseData.SendMessage(A<string>.Ignored, A<int>.Ignored, A<KfxInfoReturnValue>.Ignored))
                .Invokes(c => TestContext.WriteLine(c.GetArgument<string>(0)));
            A.CallTo(
                () =>
                    releaseData.LogError(A<int>.Ignored, A<int>.Ignored, A<int>.Ignored, A<string>.Ignored,
                        A<string>.Ignored, A<int>.Ignored))
                .Invokes(c => TestContext.WriteLine(c.GetArgument<string>(3)));

            return releaseData;
        }

        private static void AddRequiredCustomPropertyElementsToReleaseData(ReleaseData releaseData)
        {
            //TODO: Anything here?
        }

        private static void ApplyWorkingCustomPropertiesCollectionToReleaseData(ReleaseData releaseData)
        {
            var customProperties = new MockCustomProperties();
            A.CallTo(() => releaseData.CustomProperties).Returns(customProperties);
        }

        public static ReleaseSetupData GetValidSetupData()
        {
            var setupData = GetBasicReleaseSetupData();

            ApplyValidIndexFieldsToSetupData(setupData);
            ApplyValidBatchFieldsToSetupData(setupData);
            ApplyValidKofaxValuesToSetupData(setupData);
            ApplyWorkingCustomPropertiesCollectionToSetupData(setupData);
            AddRequiredCustomPropertyElementsToSetupData(setupData);

            return setupData;
        }

        public static ReleaseSetupData GetDefaultSetupData()
        {
            var setupData = GetBasicReleaseSetupData(true);

            ApplyNewFlagToSetupData(setupData);
            ApplyValidIndexFieldsToSetupData(setupData);
            ApplyValidBatchFieldsToSetupData(setupData);
            ApplyValidKofaxValuesToSetupData(setupData);
            ApplyWorkingCustomPropertiesCollectionToSetupData(setupData);

            return setupData;
        }

        public static ReleaseSetupData GetDefaultSetupDataMissingFields()
        {
            var setupData = GetBasicReleaseSetupData(true);

            ApplyNewFlagToSetupData(setupData);
            A.CallTo(()=> setupData.BatchVariableNames ).Returns(new List<object>());
            ApplyWorkingCustomPropertiesCollectionToSetupData(setupData);

            return setupData;
        }

        private static void AddRequiredCustomPropertyElementsToSetupData(ReleaseSetupData setupData)
        {
            //TODO: Anything here?
        }

        private static void ApplyWorkingCustomPropertiesCollectionToSetupData(ReleaseSetupData setupData)
        {
            var customProperties = new MockCustomProperties();
            A.CallTo(() => setupData.CustomProperties).Returns(customProperties);
        }

        private static void ApplyValidKofaxValuesToSetupData(ReleaseSetupData setupData)
        {
            var batchVariableNames = new[]
            {
                "Batch Class Name",
                "Batch Creation Date",
                "Document Class Name",
                "Document Count",
                "Document Form Name",
                "Document GUID",
                "Document ID",
                "First Page Endorsing String",
                "First Page Image Address",
                "First Page Original File Name",
                "First Page Roll Number",
                "Index Operator's Station ID",
                "Batch Creation Time",
                "Operator Description",
                "Operator Name",
                "Operator User ID",
                "Queue",
                "Remote Validation User ID",
                "Scan Operator's Station ID",
                "Scan Operator's User ID",
                "Sequence Number",
                "Site ID",
                "Batch Creator's Station ID",
                "Station ID",
                "User Name",
                "UTC Offset",
                "Validation Operator's Station ID",
                "Batch Description",
                "Batch ID",
                "Batch Name",
                "Batch Reference ID",
                "Current Date",
                "Current Time"
            };
            A.CallTo(() => setupData.BatchVariableNames).Returns(batchVariableNames);
        }

        private static void ApplyValidBatchFieldsToSetupData(ReleaseSetupData setupData)
        {
            var batchFields = SetupFakeBatchFields();
            A.CallTo(() => setupData.BatchFields).Returns(batchFields);
        }

        private static void ApplyValidIndexFieldsToSetupData(ReleaseSetupData setupData)
        {
            var indexFields = SetupFakeIndexFields();
            A.CallTo(() => setupData.IndexFields).Returns(indexFields);
        }

        private static void ApplyNewFlagToSetupData(ReleaseSetupData setupData)
        {
            A.CallTo(() => setupData.New).Returns(1);
        }

        private static ReleaseSetupData GetBasicReleaseSetupData(bool defaultSetup = false)
        {
            Links links = new MockLinks();
            var setupData = A.Fake<ReleaseSetupData>();

            A.CallTo(() => setupData.BatchClassName).Returns("A7 Test Batch Class");
            A.CallTo(() => setupData.DocClassName).Returns("A7 Test Document Class");
            A.CallTo(() => setupData.Name).Returns("A7 Test Document Class");
            A.CallTo(() => setupData.Links).Returns(links);

            A.CallTo(() => setupData.ImageTypes)
                .Returns(
                    new MockImageTypesList(new[]
                    {
                        new MockImageType
                        {
                            Description = "Multipage TIFF - CCITT Group 4",
                            FilterName = "TIFF",
                            Method = 8,
                            MethodName = "CCITT Group 4",
                            MultiplePage = 1,
                            Type = 0
                        },
                        new MockImageType
                        {
                            Description = "TIFF - CCITT Group 4",
                            FilterName = "TIFF",
                            Method = 8,
                            MethodName = "CCITT Group 4",
                            MultiplePage = 0,
                            Type = 6
                        }
                    }));

            A.CallTo(() => setupData.KofaxPDFDocClassEnabled).Returns(1);
            A.CallTo(() => setupData.KofaxPDFReleaseScriptEnabled).Returns(true);

            if (defaultSetup)
            {
                A.CallTo(() => setupData.KofaxPDFPath).Returns("");
                A.CallTo(() => setupData.ImageFilePath).Returns("");
            }
            else
            {
                A.CallTo(() => setupData.KofaxPDFPath).Returns("A6Output");
                A.CallTo(() => setupData.ImageFilePath).Returns("A6Output");
            }

            A.CallTo(() => setupData.Apply()).DoesNothing();

            return setupData;
        }

        private static IndexFields SetupFakeIndexFields()
        {
            var indexFields = A.Fake<IndexFields>();
            var indexFieldsList = GetFakeIndexFields();

            object[] refVal =
            {
                1, 2, 
            };
            // ReSharper disable UseIndexedProperty
            A.CallTo(() => indexFields.get_Item(ref refVal[0])).ReturnsLazily(() => indexFieldsList[0]);
            A.CallTo(() => indexFields.get_Item(ref refVal[1])).ReturnsLazily(() => indexFieldsList[1]);
            // ReSharper restore UseIndexedProperty

            A.CallTo(() => indexFields.GetEnumerator())
                .Returns(GetFakedEnumerator(indexFieldsList[0], indexFieldsList[1]));

            A.CallTo(() => indexFields.Count).ReturnsLazily(() => indexFieldsList.Count);
            return indexFields;
        }

        private static BatchFields SetupFakeBatchFields()
        {
            var batchFields = A.Fake<BatchFields>();
            var batchFieldsList = GetFakeBatchFields();

            object[] refVal =
            {
                1, 2, 3, 4
            };
            // ReSharper disable UseIndexedProperty
            A.CallTo(() => batchFields.get_Item(ref refVal[0])).ReturnsLazily(() => batchFieldsList[0]);
            A.CallTo(() => batchFields.get_Item(ref refVal[1])).ReturnsLazily(() => batchFieldsList[1]);
            A.CallTo(() => batchFields.get_Item(ref refVal[2])).ReturnsLazily(() => batchFieldsList[2]);
            A.CallTo(() => batchFields.get_Item(ref refVal[3])).ReturnsLazily(() => batchFieldsList[3]);
            // ReSharper restore UseIndexedProperty

            A.CallTo(() => batchFields.GetEnumerator()).Returns(GetFakedEnumerator(batchFieldsList[0],
                batchFieldsList[1], batchFieldsList[2], batchFieldsList[3]));

            A.CallTo(() => batchFields.Count).ReturnsLazily(() => batchFieldsList.Count);
            return batchFields;
        }

        private static List<BatchField> GetFakeBatchFields()
        {
            var batchFields = new List<BatchField>();

            var fields = new[]
            {
                "B_ScanDepot",
                "B_ScanDateTime",
                "B_ScanCountryCode",
                "B_ArchDocType"
            };

            foreach (var field in fields)
            {
                var batchField = A.Fake<BatchField>();
                A.CallTo(() => batchField.Name).Returns(field);
                A.CallTo(() => batchField.Type).Returns(Varchar);
                batchFields.Add(batchField);
            }

            return batchFields;
        }

        private static List<IndexField> GetFakeIndexFields()
        {
            var indexFields = new List<IndexField>();

            var fields = new[]
            {
                "DocStatus",
                "ConNumber"
            };

            foreach (var field in fields)
            {
                var indexField = A.Fake<IndexField>();
                A.CallTo(() => indexField.Name).Returns(field);
                A.CallTo(() => indexField.Type).Returns(Varchar);
                indexFields.Add(indexField);
            }

            return indexFields;
        }

        private static IEnumerator<T> GetFakedEnumerator<T>(params T[] values)
        {
            foreach (var value in values)
            {
                yield return value;
            }
        }

        private static IEnumerator<T> GetFakedEnumerator<T>(IEnumerable<T> values)
        {
            foreach (var value in values)
            {
                yield return value;
            }
        }

        private static Value GetFakeValue(string destination, string sourceName,
            KfxLinkSourceType sourceType, string stringValue)
        {
            var value = A.Fake<Value>();
            A.CallTo(() => value.Destination).Returns(destination);
            A.CallTo(() => value.SourceName).Returns(sourceName);
            A.CallTo(() => value.SourceType).Returns(sourceType);
            A.CallTo(() => value.Value).Returns(stringValue);
            return value;
        }

        private static IEnumerator<Value> GetFakedValuesEnumerator(IEnumerable<Value> values)
        {
            foreach (var value in values)
            {
                yield return value;
            }
        }

        /// <summary>
        /// Copy the file.
        /// </summary>
        /// <param name="sourceFilePath">Source file to copy.</param>
        /// <param name="docId">The Doc ID</param>
        /// <param name="directoryPath">Output directory.</param>
        /// <param name="fileExt">File extension.</param>
        /// <returns>Path to copied file</returns>
        public static string CopyFile(string sourceFilePath, int docId, string directoryPath, string fileExt)
        {
            var docIdHex = docId.ToString("X8");
            var filePath = Path.Combine(directoryPath, docIdHex + fileExt);
            
            File.Copy(sourceFilePath, filePath, true);
            return filePath;
        }


        /// <summary>
        /// Generate an MD5 hash for the supplied string and return it.
        /// </summary>
        /// <param name="content">String content to generate an MD5 hash for.</param>
        /// <returns>MD5 hash.</returns>
        public static string GetMd5Hash(string content)
        {
            if (content == null) throw new ArgumentNullException("content");

            var rawBytes = new UnicodeEncoding().GetBytes(content);
            var md5 = GetMd5Hash(rawBytes);
            return md5;
        }

        /// <summary>
        /// Calculate an MD5 of the input bytes and return as a hexadecimal string.
        /// </summary>
        /// <param name="input">Bytes to hash.</param>
        /// <returns>Hexadecimal representation of the MD5 hash.</returns>
        public static string GetMd5Hash(byte[] input)
        {
            if (input == null) throw new ArgumentNullException("input");

            // Create a new instance of the MD5CryptoServiceProvider object.
            using (var md5Hasher = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                var data = md5Hasher.ComputeHash(input);

                // Create a new Stringbuilder to collect the bytes and create a string.
                var hashHex = new StringBuilder();

                // Loop through each byte of the hashed data and format each one as a hexadecimal string.
                foreach (var b in data)
                {
                    hashHex.Append(b.ToString("x2", CultureInfo.InvariantCulture));
                }

                // Return the hexadecimal string.
                return hashHex.ToString();
            }
        }
    }
}