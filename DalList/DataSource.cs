namespace Dal;

internal static class DataSource
{
    /// <summary>
    /// defining list of objects for each entity
    /// </summary>
    internal static List<DO.Engineer> Engineers { get; } = new();
    internal static List<DO.Task> Tasks { get; } = new();
    internal static List<DO.Dependency> Dependencies { get; } = new();

    /// <summary>
    /// generates automatic running numbers for the fields that are defined as "running ID number".
    /// </summary>
    internal static class Config
    {
        internal const int StartTaskId = 1000;
        private static int nextTaskId = StartTaskId;
        internal static int NextTaskId { get => nextTaskId++; }

        internal const int StartDependencyId = 100;
        private static int nextDependencyId = StartDependencyId;
        internal static int NextDependencyId { get => nextDependencyId++; }
    }
}
