namespace BO;

/// <summary>
/// enum for levels of engineer experience
/// </summary>
public enum EngineerExperience
{
    Beginner, AdvancedBeginner, Intermediate, Advanced, Expert
}

public enum EngineerExperienceForFilter
{
    Beginner, AdvancedBeginner, Intermediate, Advanced, Expert, All
}

/// <summary>
/// enum for status of task completion
/// </summary>
public enum Status
{
    Unscheduled, Scheduled, OnTrack, Late, Done
}

/// <summary>
/// enum for status of project
/// </summary>
public enum ProjectStatus
{
   InPlanning, InExecution
}

/// <summary>
/// enum for role of user
/// </summary>
public enum UserRole
{
    Admin, Engineer
}