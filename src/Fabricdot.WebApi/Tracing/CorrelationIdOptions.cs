namespace Fabricdot.WebApi.Tracing;

public class CorrelationIdOptions
{
    public string HeaderKey { get; set; } = "X-CorrelationId";

    public bool IncludeResponse { get; set; } = true;
}