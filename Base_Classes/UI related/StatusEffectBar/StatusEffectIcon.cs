using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusEffectIcon : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI durationText;
    [SerializeField] private TextMeshProUGUI stacksText;

    private StatusEffect statusEffect;

    public StatusEffect StatusEffect { get { return statusEffect; } }

    public void Initialize(StatusEffect statusEffect)
    {
        this.statusEffect = statusEffect;
        iconImage.sprite = statusEffect.EffectData.Icon;
        GetComponent<Image>().sprite = statusEffect.EffectData.Icon;
    }

    private void UpdateDurationText()
    {
        durationText.text = statusEffect.Duration.ToString("F1");
    }

    private void UpdateStacksText()
    {
        if (statusEffect.EffectData.Stacks)
        {
            stacksText.text = statusEffect.StackSize.ToString();
        }
    }

    // Add an update method that checks if the duration has changed
    private void Update()
    {
        if (statusEffect.Duration != float.PositiveInfinity && statusEffect.Duration > 0)
        {
            UpdateDurationText();
            UpdateStacksText();
        }
    }
}
