using Controllers.ShipComponents;

namespace Controllers.UI
{
    public interface IComponentEntryController
    {
        void Initialise(IComponentController i);

        void ActivatePoweredControls();
    }
}