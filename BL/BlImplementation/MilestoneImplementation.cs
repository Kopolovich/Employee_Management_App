namespace BlImplementation;
using BlApi;

internal class MilestoneImplementation : IMilestone
{
    private DalApi.IDal _dal = Factory.Get;
}
