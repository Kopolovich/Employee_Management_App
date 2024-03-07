namespace DO;
/// <summary>
/// Engineer entity represents an engineer with all its props
/// </summary>
/// <param name="Id">Main personal unique ID of the engineer</param>
/// <param name="Email">engineer's email address</param>
/// <param name="Cost">Hourly salary for the engineer</param>
/// <param name="Name">The engineer's full name</param>
/// <param name="Level">The level of experience of the engineer</param>
public record Engineer
(
    int Id,
    EngineerExperience Level,
    bool IsActive = true,
    string? Email = null,
    double? Cost = null,
    string? Name = null    
)
{
    public Engineer() : this(0, EngineerExperience.Beginner) { } //empty ctor
}

