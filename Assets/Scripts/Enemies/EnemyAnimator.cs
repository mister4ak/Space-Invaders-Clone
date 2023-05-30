using System.Collections.Generic;
using ScriptableObjects.Classes;
using UnityEngine;

namespace Enemies
{
    public class EnemyAnimator : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private readonly List<Sprite> _animationSprites = new();
        private EnemyData _enemyData;
        private int _currentSpriteIndex;

        public void Initialize(EnemyData enemyData)
        {
            _enemyData = enemyData;
            
            _animationSprites.AddRange(new []
            {
                _enemyData.BaseSprite,
                _enemyData.SecondSprite
            });
            
            ResetData();
        }

        public void Animate()
        {
            IncreaseIndex();
            SetSprite();
        }

        private void ResetData()
        {
            _currentSpriteIndex = 0;
            SetSprite();
        }

        private void SetSprite()
        {
            _spriteRenderer.sprite = _animationSprites[_currentSpriteIndex];
        }

        private void IncreaseIndex()
        {
            _currentSpriteIndex++;
            if (_currentSpriteIndex >= _animationSprites.Count)
                _currentSpriteIndex = 0;
        }
    }
}