
namespace Assets.Scripts.Util.Interfaces
{
    public interface IHealth
    {
        void TakeDamage(float damage);
        bool IsAlive();
        float GetTotalHealth();
        float GetCurrentHealth();
        void AddHealth(float healPoints);
        void UpdateHealth();
        void RestoreHealth();
    }
}

