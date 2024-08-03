

namespace ObservableTurnBasedCombat.ObservableUnit
{
    public interface IUnitRepository
    {
        ObservableUnitComponent FetchComponentById(UnitComponentId id);
    }
}
