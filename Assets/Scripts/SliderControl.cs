using UnityEngine;
using UnityEngine.UI;

public class SliderControl : MonoBehaviour
{
   [Header("UI Elements")] [SerializeField]
   private Slider slider;

   public GameObject sound;
   private AudioSource audioSource;
   void Start()
   {
      slider.value = slider.maxValue;
      audioSource = sound.GetComponent<AudioSource>();
   }

   public void OnSliderValueChanged(float value)
   {
      audioSource.volume = slider.value;
   }

   private void OnDestroy()
   {
      if (audioSource != null && slider != null)
      {
         slider.onValueChanged.RemoveAllListeners();
      }
   }
}
