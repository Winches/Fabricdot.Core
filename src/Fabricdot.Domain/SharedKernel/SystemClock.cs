namespace Fabricdot.Domain.SharedKernel;

public static class SystemClock
{
    public static DateTimeKind Kind { get; private set; }

    public static DateTime Now => Kind == DateTimeKind.Utc ? DateTime.UtcNow : DateTime.Now;

    public static void Configure(DateTimeKind kind)
    {
        Kind = kind;
    }

    public static DateTime Normalize(DateTime dateTime)
    {
        return Kind == DateTimeKind.Unspecified || Kind == dateTime.Kind
            ? dateTime
            : Kind switch
            {
                DateTimeKind.Local when dateTime.Kind == DateTimeKind.Utc => dateTime.ToLocalTime(),
                DateTimeKind.Utc when dateTime.Kind == DateTimeKind.Local => dateTime.ToUniversalTime(),
                _ => DateTime.SpecifyKind(dateTime, Kind)
            };
    }
}