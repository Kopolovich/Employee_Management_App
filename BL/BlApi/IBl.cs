using BO;

namespace BlApi;

public interface IBl
{
    public ITask Task { get; }
    public IEngineer Engineer { get; }
    public IMilestone Milestone { get; }

    //public ProjectStatus GetProjectStatus();

    public void CreateProjectSchedule(List<BO.Task> tasks, DateTime projectStartDate);

    //DateTime GetEarliestDate(BO.Task task, DateTime projectStartDate);
}
