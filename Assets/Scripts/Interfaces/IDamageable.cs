using Enums;

namespace Interfaces
{
    public interface IDamageable
    {
        ShooterType ShooterType { get; }
        void TakeDamage();
    }
}