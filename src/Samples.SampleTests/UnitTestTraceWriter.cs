using System;
using System.Diagnostics;
using Microsoft.Azure.WebJobs.Host;

namespace Samples.SampleBizLogic.Tests
{
    public class UnitTestTraceWriter : TraceWriter
    {
        private readonly Action<TraceEvent> _traceAction;

        public UnitTestTraceWriter(TraceLevel level, Action<TraceEvent> traceAction) : base(level)
        {
            _traceAction = traceAction;
        }

        public override void Trace(TraceEvent traceEvent)
        {
            _traceAction?.Invoke(traceEvent);
        }
    }
}
