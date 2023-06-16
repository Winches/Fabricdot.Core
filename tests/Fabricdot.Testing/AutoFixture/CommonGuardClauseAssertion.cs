namespace Fabricdot.Testing.AutoFixture;

public class CommonGuardClauseAssertion : GuardClauseAssertion
{
    public CommonGuardClauseAssertion(ISpecimenBuilder builder)
        : base(
            builder,
            new CompositeBehaviorExpectation(
                new FixedNullReferenceBehaviorExpectation(),
                new EmptyGuidBehaviorExpectation(),
                new EmptyStringBehaviorExpectation(),
                new WhitespaceStringBehaviorExpectation()))
    {
    }
}
