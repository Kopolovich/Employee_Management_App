using BO;
namespace BlApi;

public interface IBl
{
    public ITask Task { get; }
    public IEngineer Engineer { get; }
    public ProjectStatus GetProjectStatus();
    public void CreateProjectSchedule(DateTime projectStartDate);
    public void Reset();
    public void Initialize();
}
