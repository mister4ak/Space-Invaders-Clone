using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Extensions;
using TMPro;
using UnityEngine;

namespace UI
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _headerLabel;
        [SerializeField] private TMP_Text _scoreTableLabel;
        [SerializeField] private List<ShipContainerUI> _shipContainers;
        [SerializeField] private float _textSpeed;

        private void Start()
        {
            foreach (var shipContainer in _shipContainers) 
                shipContainer.Initialize();

            StartCoroutine(ShowStartText());
        }

        private IEnumerator ShowStartText()
        {
            _headerLabel.Deactivate();
            _scoreTableLabel.Deactivate();
            foreach (var shipContainer in _shipContainers) shipContainer.Deactivate();

            yield return new WaitForSeconds(1f);

            _headerLabel.Activate();
            yield return new DOTweenCYInstruction.WaitForCompletion(TweenText(_headerLabel));
            
            yield return new WaitForSeconds(1f);
            
            _scoreTableLabel.Activate();
            foreach (var shipContainer in _shipContainers)
            {
                shipContainer.Activate();
                foreach (var text in shipContainer.GetAllTexts()) 
                    text.Deactivate();
            }
            
            yield return new WaitForSeconds(1f);

            foreach (var shipContainer in _shipContainers)
            {
                foreach (var text in shipContainer.GetAllTexts())
                {
                    text.Activate();
                    yield return new DOTweenCYInstruction.WaitForCompletion(TweenText(text));
                }
            }
        }

        private Tweener TweenText(TMP_Text text)
        {
            var completedText = text.text;
            text.text = string.Empty;
            return DOTween.To(() => text.text, x => text.text = x, completedText, _textSpeed)
                .SetSpeedBased();
        }
    }
}