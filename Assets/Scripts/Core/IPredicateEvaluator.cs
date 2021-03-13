namespace RPG.Core
{
    public interface IPredicateEvaluator
    {
        bool? Evaluate(PredicateType predicate, string[] parameters);
    }
}