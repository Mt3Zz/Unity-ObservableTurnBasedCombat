

namespace ObservableTurnBasedCombat.Application
{
    public interface IUnitRepository
    {
        ObservableUnitComponent FetchComponentById(UnitComponentId id);
    }
}
