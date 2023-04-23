using ZSBB.AnimalBT;

namespace ZSBB {
    interface IRelocationMessages {
        void OnHit();

        void OnCageEnter(AnimalCagePreference cage);
        void OnCageExit(AnimalCagePreference cage);
    }
}
