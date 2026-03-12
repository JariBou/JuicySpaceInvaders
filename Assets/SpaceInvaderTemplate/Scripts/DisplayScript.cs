using UnityEngine;
using UnityEngine.UI;

namespace SpaceInvaderTemplate.Scripts
{
    public class DisplayScript : MonoBehaviour
    {
        [SerializeField] private RenderTexture _renderTexture;
        [SerializeField] private Image _renderImage;

        private void Update()
        {
            // _renderTexture.
            // _renderImage.sprite = _renderTexture;
        }
    }
}