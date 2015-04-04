# Overview #

Using Assimp .NET is really easy, you can download the source and build it yourself, or download the latest binaries (Downloads page). AssimpNet uses P/Invoke to communicate with the native Assimp library.

The API is divided into two categories:

  1. A "low-level" set of functions that exposes the Assimp C-API. This is the P/Invoke layer and will require you to marshal the model data from unmanageable memory. These exist in the **Assimp.Unmanaged** namespace.
  1. A "high-level" more .NET API that loads a model, marshals all the data to and from managed memory, and provides access to that data. This data structure closely resembles the Assimp data structure in naming conventions and organization, but provides convenient access to the data that .NET developers will appreciate.

# Importing a model #

The main object you'll be using in AssimpNet is the **AssimpContext** object. Each context instance can be considered as separate local state - all the property configurations you attach to each one will remain true only for that specific instance. The only global state are the logging streams which are handled independently of the context instances. So the library fully supports multithreaded model loading.

When a model is imported, the returned data structure is similar to the Assimp data structure. It's organization and documentation closely resembles the native Assimp data structure. Unlike in past versions of the library, the data structure is fully writable, allowing for exporting of models. You can either import a model, change data, then export or create a scene entirely from scratch and populate it with your data before exporting.

Some example code is provided below. This is all that is required to use the API to load up a model. Of course, the hard part now would be to translate the Assimp model data structure into your own model structure (or you could use it directly if you wish).

```
using System;
using System.IO;
using System.Reflection;
using Assimp;
using Assimp.Configs;

namespace Example {
    class Program {
        static void Main(string[] args) {
            //Filepath to our model
            String fileName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Seymour.dae");

            //Create a new importer
            AssimpContext importer = new AssimpContext();

            //This is how we add a configuration (each config is its own class)
            NormalSmoothingAngleConfig config = new NormalSmoothingAngleConfig(66.0f);
            importer.SetConfig(config);

            //This is how we add a logging callback 
            LogStream logstream = new LogStream(delegate(String msg, String userData) {
                Console.WriteLine(msg);
            });
            logstream.Attach();

            //Import the model. All configs are set. The model
            //is imported, loaded into managed memory. Then the unmanaged memory is released, and everything is reset.
            Scene model = importer.ImportFile(fileName, PostProcessPreset.TargetRealTimeMaximumQuality);

            //TODO: Load the model data into your own structures

            //End of example
            importer.Dispose();
        }
    }
}
```