/*
* Copyright (c) 2012-2013 AssimpNet - Nicholas Woodfield
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/

using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using Assimp.Configs;
using Assimp.Unmanaged;
using NUnit.Framework;

namespace Assimp.Test {
    [TestFixture]
    public class AssimpImporterTestFixture {

        [Test]
        public void TestFreeLogStreams() {
            ConsoleLogStream console1 = new ConsoleLogStream();
            ConsoleLogStream console2 = new ConsoleLogStream();
            ConsoleLogStream console3 = new ConsoleLogStream();

            console1.Attach();
            console2.Attach();
            console3.Attach();

            AssimpLibrary.Instance.FreeLibrary();

            IEnumerable<LogStream> logs = LogStream.GetAttachedLogStreams();

            Assert.IsEmpty(logs);
            Assert.IsFalse(console1.IsAttached);
            Assert.IsFalse(console2.IsAttached);
            Assert.IsFalse(console3.IsAttached);
        }

        [Test]
        public void TestImportFromFile() {
            String path = Path.Combine(TestHelper.RootPath, "TestFiles\\sphere.obj");

            AssimpImporter importer = new AssimpImporter();

            importer.SetConfig(new NormalSmoothingAngleConfig(55.0f));
            importer.Scale = .5f;
            importer.XAxisRotation = 25.0f;
            importer.YAxisRotation = 50.0f;
            LogStream.IsVerboseLoggingEnabled = true;

            Assert.IsTrue(importer.ContainsConfig(NormalSmoothingAngleConfig.NormalSmoothingAngleConfigName));

            importer.RemoveConfigs();

            Assert.IsFalse(importer.ContainsConfig(NormalSmoothingAngleConfig.NormalSmoothingAngleConfigName));

            importer.SetConfig(new NormalSmoothingAngleConfig(65.0f));
            importer.SetConfig(new NormalSmoothingAngleConfig(22.5f));
            importer.RemoveConfig(NormalSmoothingAngleConfig.NormalSmoothingAngleConfigName);

            Assert.IsFalse(importer.ContainsConfig(NormalSmoothingAngleConfig.NormalSmoothingAngleConfigName));

            importer.SetConfig(new NormalSmoothingAngleConfig(65.0f));

            Scene scene = importer.ImportFile(path, PostProcessPreset.TargetRealTimeMaximumQuality);

            Assert.IsNotNull(scene);
            Assert.IsTrue((scene.SceneFlags & SceneFlags.Incomplete) != SceneFlags.Incomplete);
        }

        [Test]
        public void TestImportFromStream() {
            String path = Path.Combine(TestHelper.RootPath, "TestFiles\\duck.dae");

            FileStream fs = File.OpenRead(path);

            AssimpImporter importer = new AssimpImporter();
            LogStream.IsVerboseLoggingEnabled = true;

            LogStream logstream = new LogStream(delegate(String msg, String userData) {
                Console.WriteLine(msg);
            });

            logstream.Attach();

            Scene scene = importer.ImportFileFromStream(fs, ".dae");

            fs.Close();

            Assert.IsNotNull(scene);
            Assert.IsTrue((scene.SceneFlags & SceneFlags.Incomplete) != SceneFlags.Incomplete);
        }

        [Test]
        public void TestSupportedFormats() {
            AssimpImporter importer = new AssimpImporter();
            ExportFormatDescription[] exportDescs = importer.GetSupportedExportFormats();

            String[] importFormats = importer.GetSupportedImportFormats();

            Assert.IsNotNull(exportDescs);
            Assert.IsNotNull(importFormats);
            Assert.IsTrue(exportDescs.Length >= 1);
            Assert.IsTrue(importFormats.Length >= 1);

            Assert.IsTrue(importer.IsExportFormatSupported(exportDescs[0].FileExtension));
            Assert.IsTrue(importer.IsImportFormatSupported(importFormats[0]));
        }

        [Test]
        public void TestConvertFromFile() {
            String path = Path.Combine(TestHelper.RootPath, "TestFiles\\Bob.md5mesh");
            String outputPath = Path.Combine(TestHelper.RootPath, "TestFiles\\Bob.dae");

            AssimpImporter importer = new AssimpImporter();
            importer.ConvertFromFileToFile(path, outputPath, "collada");

            ExportDataBlob blob = importer.ConvertFromFileToBlob(path, "collada");
        }

        [Test]
        public void TestConvertFromStream() {
            String path = Path.Combine(TestHelper.RootPath, "TestFiles\\duck.dae");
            String outputPath = Path.Combine(TestHelper.RootPath, "TestFiles\\duck.obj");
            String outputPath2 = Path.Combine(TestHelper.RootPath, "TestFiles\\duck-fromBlob.obj");

            FileStream fs = File.OpenRead(path);

            new ConsoleLogStream().Attach();

            AssimpImporter importer = new AssimpImporter();
            importer.ConvertFromStreamToFile(fs, ".dae", outputPath, "obj");

            fs.Position = 0;

            ExportDataBlob blob = importer.ConvertFromStreamToBlob(fs, ".dae", "collada");

            fs.Close();

            //Take ExportDataBlob's data, write it to a memory stream and export that back to an obj and write it

            MemoryStream memStream = new MemoryStream();
            memStream.Write(blob.Data, 0, blob.Data.Length);

            memStream.Position = 0;

            importer.ConvertFromStreamToFile(memStream, ".dae", outputPath2, "obj");

            memStream.Close();

            LogStream.DetachAllLogstreams();
        }

        [Test]
        public void TestLoadFreeLibrary() {
            if(AssimpLibrary.Instance.IsLibraryLoaded)
                AssimpLibrary.Instance.FreeLibrary();

            AssimpLibrary.Instance.LoadLibrary();
            
            String path = Path.Combine(TestHelper.RootPath, "TestFiles\\duck.dae");
            AssimpImporter importer = new AssimpImporter();
            importer.ImportFile(path);
            importer.Dispose();

            AssimpLibrary.Instance.FreeLibrary();
        }

        [Test]
        public void TestMultipleImportersMultipleThreads() {
            LogStream.IsVerboseLoggingEnabled = true;

            Thread threadA = new Thread(new ThreadStart(LoadSceneB));
            Thread threadB = new Thread(new ThreadStart(LoadSceneB));
            Thread threadC = new Thread(new ThreadStart(ConvertSceneC));

            threadB.Start();
            threadA.Start();
            threadC.Start();

            threadC.Join();
            threadA.Join();
            threadB.Join();

            LogStream.DetachAllLogstreams();
        }

        private void LoadSceneA() {
            Console.WriteLine("Thread A: Starting import.");
            AssimpImporter importer = new AssimpImporter();
            String path = Path.Combine(TestHelper.RootPath, "TestFiles\\Bob.md5mesh");

            new ConsoleLogStream("Thread A:").Attach();
            Console.WriteLine("Thread A: Importing");
            Scene scene = importer.ImportFile(path);
            Console.WriteLine("Thread A: Done importing");
        }

        private void LoadSceneB() {
            Console.WriteLine("Thread B: Starting import.");
            AssimpImporter importer = new AssimpImporter();
            String path = Path.Combine(TestHelper.RootPath, "TestFiles\\duck.dae");

            new ConsoleLogStream("Thread B:").Attach();
            importer.SetConfig(new NormalSmoothingAngleConfig(55.0f));
            Console.WriteLine("Thread B: Importing");
            Scene scene = importer.ImportFile(path);
            Console.WriteLine("Thread B: Done importing");
        }

        private void ConvertSceneC() {
            Console.WriteLine("Thread C: Starting convert.");
            AssimpImporter importer = new AssimpImporter();
            String path = Path.Combine(TestHelper.RootPath, "TestFiles\\duck.dae");
            String outputPath = Path.Combine(TestHelper.RootPath, "TestFiles\\duck2.obj");

            new ConsoleLogStream("Thread C:").Attach();
            importer.SetConfig(new NormalSmoothingAngleConfig(55.0f));
            importer.SetConfig(new FavorSpeedConfig(true));

            Console.WriteLine("Thread C: Converting");
            ExportDataBlob blob = importer.ConvertFromFileToBlob(path, "obj");

            Console.WriteLine("Thread C: Done converting");
        }
    }
}
