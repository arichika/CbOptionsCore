# CbOptionsCore
The **CbOptionsCore** This is a [`ConfigurationBuilder`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.configurationbuilder) wrapper for Azure Functions 2.0 enviroments with .NET Core / Unit Testing by xUnit.

| This is the **beta** release |
|--------------|
| This library depends on the Azure Functions 2.0 runtime beta / azure-webjobs-sdk 3.0.0-beta4 release. |

## What is pleasing about CbOptionsCore?
---

* **Consistent usage**. Provide a consistent ConfigurationBuilder usage environment for both "asp net core" and "azure functions 2.0".
  * Configuration Binding by Azure portal, setting.json and enviroment value.
  * Options provided by means like (ASP. NET Core's) DI,
* **Easy and Safty Configuration Binding**. The `ConfigurationBuilder` is the best Configuration Binding approach for .NET Core Web technology.
  * For Strongly typed configuration settings, ConfigurationBuilder is very usefull.
  * In development, local debugging and unit test, settings provided by files, and in operation environment, setting via Azure portal (= environment variables). This design divides management and responsibility in separation of development and operation.
* **Easy Unit Testing** by xUnit with setting.json on Visual Studio 2017. When developing "Azure Functions 2.0" with Visual Studio, make it easy to use local.setting.json in unit test.
  * Read parameters from *some.* setting.json for Unit Test

## How to Use / Install
---

The library provides in [NuGet](https://www.nuget.org/packages/CbOptionsCore/) .NET Standard 2.0 targeted for .NET Core 2.0.

```
Install-Package CbOptionsCore
```
Or, Please cloning this repository and refer to this project from Azure Functions' projects in Visual Studio 2017 with .NET Core 2.0 + Azure Functions SDK 2.0.

## Quick Start
---

You can specify the following as arguments of the trigger methods of Azure Functions.
Please refer to the included sample project.

You can receive strong typed custom options with arguments of methods called from Functions.

```csharp
    public static class SampleFunction2WithOptions
    {
        [FunctionName("SampleFunction2WithOptions")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req,
            TraceWriter log,
            [CbOptions(sectionKey: "SampleBizLogic:MyBizOptions", reloadOnChange: true)]MyBizOptions options )
        {
            //...

```

`MyBizOptions` is the strongly typed configuration settings class defined by yourself. 

So, It is easy to reuse business logic modules already developed for the asp net core with options.  

The following parameters can be specified by the `CbOptions` attribute.

```csharp
[CbOptions(sectionKey: {sectionKey}, settingJsonPath: {settingJsonPath}, optional: {optional}, reloadOnChange: {reloadOnChange})] SomeType options, ...
```

**string {sectionKey}**: The key of the configuration section.   
**string {settingJsonPath}**: Path relative to the base path stored in.  
**bool {optional}**: Whether the file is optional. Default value is `true`.  
**bool {reloadOnChange}**: Whether the configuration should be reloaded if the file changes.   Default value is `false`.  

Please check this official [document](https://docs.microsoft.com/ja-jp/dotnet/api/microsoft.extensions.configuration.jsonconfigurationextensions.addjsonfile?view=aspnetcore-2.0#Microsoft_Extensions_Configuration_JsonConfigurationExtensions_AddJsonFile_Microsoft_Extensions_Configuration_IConfigurationBuilder_Microsoft_Extensions_FileProviders_IFileProvider_System_String_System_Boolean_System_Boolean_) for ConfigurationBuilder's extention method.


For `ConfigurationBuilder` details, check here.
[Configure an ASP.NET Core App](
https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/).


For the following explanation, an example option class is shown.

```csharp
    public class MyBizOptions
    {
        public string MyBizOptions10 { get; set; } = string.Empty;

        public int MyBizOptions20 { get; set; } = -1;

        public MyBizOptions30 MyBizOptions30 { get; set; } = new MyBizOptions30();
    }
    //...
    public class MyBizOptions30
    {
        public DateTimeOffset MyBizOptions31 { get; set; } = new DateTimeOffset().UtcDateTime;

        public bool MyBizOptions32 { get; set; } = false;
    }    
```

### Development in Visual Studio / local debugging
In development or local debugging. this params provided from `local.setting.json`.
```csharp
{
    //...
    "SampleBizLogic": {
      "MyBizOptions": {
        "MyBizOptions10": "this is in local.setting.json",
        "MyBizOptions20": 1,
        "MyBizOptions30": {
          "MyBizOptions31": "2018-01-01T00:00:00.0000000+09:00",
          "MyBizOptions32": true
        }
      }
    }
}
```

### Unit testing by xUnit
In the same way, you can specify as follows with xUnit's Fact. 
The setting file `tests.settings.json` is placed in the Unit test project in this sample and set the param "Copy on build.".

```csharp
    public class SampleFact : UnitTestBase
    {

        private readonly MyBizOptions _options;

        public SampleFact(ITestOutputHelper output) : base(output)
        {
            _options = CbOptions.Create<MyBizOptions>(
                new CbOptionsAttribute(sectionKey: "SampleBizLogic:MyBizOptions", settingJsonPath: "tests.settings.json"));
        }

        [Fact]
        public void Test1()
        {
            _output.WriteLine($"_options={_options}");

            var (result, methodName) = new SomeFunction(_options).SomeMethod();

            _output.WriteLine($"result={result}, methodName={methodName}");

            Assert.Equal(_options.MyBizOptions10, result);
            Assert.Equal(@"SomeMethod", methodName);
        }
    }
```

### Azure Portal

This article helpfull for you.
Working with Azure App Services Application Settings and Connection Strings in ASP.NET Core  https://blogs.msdn.microsoft.com/cjaliaga/2016/08/10/working-with-azure-app-services-application-settings-and-connection-strings-in-asp-net-core/

```csharp
    "SampleBizLogic": {
      "MyBizOptions": {
        "MyBizOptions10": "this is in local.setting.json",
        "MyBizOptions20": 1,
        "MyBizOptions30": {
          "MyBizOptions31": "2018-01-01T00:00:00.0000000+09:00",
          "MyBizOptions32": true
        }
      }
    }
```

This setting is the same as specifying the following key/values in the application setting of Azure Portal.

```
Key: SampleBizLogic:MyBizOptions:MyBizOptions10
Value: this is in local.setting.json

Key: SampleBizLogic:MyBizOptions:MyBizOptions20
Value: 1

Key: SampleBizLogic:MyBizOptions:MyBizOptions30:MyBizOptions31
Value: 2018-01-01T00:00:00.0000000+09:00

Key: SampleBizLogic:MyBizOptions:MyBizOptions30:MyBizOptions32
Value: true

```


## See Also 
---
Authoring a Custom Binding for Azure Functions  
https://mikhail.io/2017/07/authoring-custom-binding-azure-functions/

Azure WebJobs SDK Extensions  
https://github.com/Azure/azure-webjobs-sdk-extensions


## License
---
This library is under the MIT License.