﻿namespace Fabricdot.WebApi.Core.Tracing
{
    public class CorrelationIdOptions
    {
        public string HeaderKey { get; set; } = "X-CorrelationId";

        public bool IncludeResponse { get; set; } = true;
    }
}