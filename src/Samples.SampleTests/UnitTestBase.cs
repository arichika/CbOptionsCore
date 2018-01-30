using System;
using System.Diagnostics;
using CbOptionsCore;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Binder;
using Microsoft.Extensions.Options;
using Xunit;
using Xunit.Abstractions;

namespace Samples.SampleBizLogic.Tests
{ 
    public class UnitTestBase
    {
        public readonly ITestOutputHelper _output;
        private readonly UnitTestTraceWriter _traceWriter;

        public UnitTestBase(ITestOutputHelper output)
        {
            _output = output;

            _traceWriter = new UnitTestTraceWriter(
                TraceLevel.Verbose,
                te => output.WriteLine($"{te}"));
        }
    }
}
