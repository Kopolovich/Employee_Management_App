using System.Collections;


namespace PL;

internal class EngineerCollectionForFilter : IEnumerable
{
    static readonly IEnumerable<BO.EngineerExperienceForFilter> s_enums =
            (Enum.GetValues(typeof(BO.EngineerExperienceForFilter)) as IEnumerable<BO.EngineerExperienceForFilter>)!;
    
    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}

internal class EngineerCollection : IEnumerable
{
    static readonly IEnumerable<BO.EngineerExperience> s_enums =
            (Enum.GetValues(typeof(BO.EngineerExperience)) as IEnumerable<BO.EngineerExperience>)!;

    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}


