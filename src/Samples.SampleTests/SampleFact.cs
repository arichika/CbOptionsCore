using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;
using CbOptionsCore;

namespace Samples.SampleBizLogic.Tests
{
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
}
