using UnityEngine;

namespace ScriptableObjects.Classes
{
    [CreateAssetMenu(menuName = "Enemy Data", fileName = "New EnemyData")]
    public class EnemyData : ScriptableObject
    {
        [SerializeField] private EnemyType _type;
        [SerializeField] private Sprite _baseSprite;
        [SerializeField] private Sprite _secondSprite;
        [SerializeField] private int _rewardScore;

        public EnemyType Type => _type;

        public Sprite BaseSprite => _baseSprite;

        public Sprite SecondSprite => _secondSprite;

        public int RewardScore => _rewardScore;
    }
}