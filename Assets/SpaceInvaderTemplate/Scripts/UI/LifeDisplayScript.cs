using SpaceInvaderTemplate.Utils;
using TMPro;
using UnityEngine;

namespace SpaceInvaderTemplate.Scripts.UI
{
    public class LifeDisplayScript : MonoBehaviour
    {
        [SerializeField] private TMP_Text _lifeDisplay;
        [SerializeField] private Player _player;

        private void Awake()
        {
            
            _lifeDisplay.text = PrimeSumHealthDisplayUtil.GetHealthDisplay(_player.LifeAmount).ToString();
        }

        private void OnEnable()
        {
            _player.DamageTaken += PlayerOnDamageTaken;
        }
        
        private void OnDisable()
        {
            _player.DamageTaken += PlayerOnDamageTaken;
        }

        private void PlayerOnDamageTaken()
        {
            _lifeDisplay.text = PrimeSumHealthDisplayUtil.GetHealthDisplay(_player.LifeAmount).ToString();
        }
    }
}