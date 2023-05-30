using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerAnimator : MonoBehaviour
    {
        private readonly int _isTakeDamageTriggerHash = Animator.StringToHash("IsTakeDamage");
        
        [SerializeField] private Animator _animator;

        public void PlayTakeDamage()
        {
            _animator.SetTrigger(_isTakeDamageTriggerHash);
        }
    }
}