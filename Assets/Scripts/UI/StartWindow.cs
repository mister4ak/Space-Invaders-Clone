using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Extensions;
using TMPro;
using UnityEngine;

namespace UI
{
    public class StartWindow : BaseWindow
    {
        public event Action ShowFinished;
    
        [Space(20)]
        [SerializeField] private List<ShipContainerUI> _shipContainers;
        [SerializeField] private TMP_Text _headerLabel;
        [SerializeField] private TMP_Text _scoreTableLabel;
        [SerializeField] private float _textSpeed;
        
        public void Initialize()
        {
            foreach (var shipContainer in _shipContainers) 
                shipContainer.Initialize();
        }

        public override void Show(Action finished = null)
        {
            _canvas.Activate();
            _canvasGroup.Show();
            StartCoroutine(ShowStartText());
        }

        public override void Hide(Action finished = null)
        {
            _canvasGroup.Hide(_fadeDuration, 
                callback: () => _canvas.Deactivate());
        }
    
        private IEnumerator ShowStartText()
        {
            DisableAllTexts();

            yield return new WaitForSeconds(0.5f);

            _headerLabel.Activate();
        
            yield return new DOTweenCYInstruction.WaitForCompletion(TweenTextSymbols(_headerLabel));
            
            yield return new WaitForSeconds(0.5f);
            
            _scoreTableLabel.Activate();
            ShowShipContainerImagesExceptText();
            
            yield return new WaitForSeconds(1f);

            foreach (var text in _shipContainers.SelectMany(shipContainer => shipContainer.GetAllTexts()))
            {
                text.Activate();
                yield return new DOTweenCYInstruction.WaitForCompletion(TweenTextSymbols(text));
            }
        
            yield return new WaitForSeconds(1f);
        
            ShowFinished?.Invoke();
        }

        private void DisableAllTexts()
        {
            _headerLabel.Deactivate();
            _scoreTableLabel.Deactivate();
            foreach (var shipContainer in _shipContainers) 
                shipContainer.Deactivate();
        }

        private void ShowShipContainerImagesExceptText()
        {
            foreach (var shipContainer in _shipContainers)
            {
                shipContainer.Activate();
                foreach (var text in shipContainer.GetAllTexts())
                    text.Deactivate();
            }
        }

        private Tweener TweenTextSymbols(TMP_Text text)
        {
            var completedText = text.text;
            text.text = string.Empty;
            return DOTween.To(() => text.text,
                    x => text.text = x,
                    completedText,
                    _textSpeed)
                .SetSpeedBased();
        }
    }
}