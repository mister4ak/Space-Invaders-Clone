using System.Collections.Generic;
using ScriptableObjects.Classes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ShipContainerUI : MonoBehaviour
    {
        [SerializeField] private EnemyData _enemyData;
        [SerializeField] private Image _shipImage;
        [SerializeField] private TMP_Text _pointsText;
        [SerializeField] private TMP_Text _pointsLabel;

        public void Initialize()
        {
            _shipImage.sprite = _enemyData.BaseSprite;
            _pointsText.text = $"= {_enemyData.RewardPoints}";
        }

        public List<TMP_Text> GetAllTexts()
        {
            return new List<TMP_Text>() { _pointsText, _pointsLabel };
        }
    }
}