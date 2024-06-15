using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using Unity.VisualScripting;

public class UIBarInterpolator : MonoBehaviour
{
	[HideInInspector]
	public float _maxValue;
	[HideInInspector]
	public float value;

	[SerializeField] TMP_Text valueText;
	[SerializeField] Image valueFill;
	[SerializeField] Image slowValueFill;
	[SerializeField] Color _maxColor;
	[SerializeField] Color _minColor;
	/*[SerializeField] float _maxTimeToStabilize;
	[SerializeField] float _minTimeToStabilize;*/
	[SerializeField] float _flashExponent;
	[SerializeField] float _flashingThreshold;
	[SerializeField] bool interpolateTextValue;
	[SerializeField] float fraction;
	[SerializeField] float _waitAfterChange;
	[SerializeField] float drainRate;
	[SerializeField] float approximationAbsolutionModifier = 1E-06f;
	[SerializeField] float approximationEpsilonModifier = 8f;
	[HideInInspector]
	public float currentValue01;
	[HideInInspector]
	public float currentSlowValue01;
	private float targetValue01;
	private float waitAfterChange;
	/*
	private float minValue01;
	private float oldValue01;
	private float minTimerStart;
	private float maxTimerStart;
	private float minTimer;
	private float maxTimer;
	private float highestMinTimer;
	private float highestMaxTimer;*/
	private float displayValue;

	private void Awake()
	{
		value = 0;
		if(valueText != null)
			valueText.text = (int)value + "/" + _maxValue;
		valueFill.fillAmount = targetValue01;
		currentSlowValue01 = 0;
		slowValueFill.fillAmount = 0;
	}

	private void Update()
	{
		SlowBarUpdate();
		value = Mathf.Clamp(value, 0, _maxValue);
		currentValue01 = value / _maxValue;
		targetValue01 = Mathf.Lerp(targetValue01, currentValue01, 1 - Mathf.Pow(fraction, Time.deltaTime * 10)); //yes. this is a wrong lerp and no, I don't give a shit anymore

		displayValue = targetValue01 > 0.99f? Mathf.CeilToInt(targetValue01 * _maxValue) : Mathf.FloorToInt(targetValue01 * _maxValue);
		


		valueFill.fillAmount = targetValue01;
		valueFill.color = Color.Lerp(_minColor, _maxColor, (float)value/_maxValue);

		if(valueText != null){
			valueText.text = (interpolateTextValue? (int)displayValue : (int)value) + "/" + _maxValue; 
			valueText.color = Color.Lerp(_minColor, _maxColor, (float)value/_maxValue);
			float flashingPercent = _flashingThreshold / 100f;
			if(value / _maxValue <= flashingPercent && value > 0){ //basically, if the value is below the threshold but higher than 0
				float value10 = 1 - value / _maxValue; //clamp the value and reverse it 
				float timeMultiplier = Mathf.Pow(_flashExponent, value10 / flashingPercent); //exponent to the power of x (x determining how fast it should flash, the lower the faster)
				float flashingTime01 = Mathf.PingPong(Time.time * timeMultiplier, 1); //make the flashing go up and down between 0 and 1 over time, goes up and down faster if the value is lower
				float flashingFrequency = Mathf.Lerp(-0.5f, 0.5f, flashingTime01);
				valueText.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, flashingFrequency);
			}	
			else valueText.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, -0.5f);
		}
		/*if(!Mathf.Approximately(oldValue01, newValue01)){ //if the "absolute" value changes from the last frame to this one
			maxTimerStart = Mathf.Abs(oldValue01 - newValue01) * _minTimeToStabilize; //the higher these stabilizers are, the longer it takes for the values to reach the new value
			minTimerStart = Mathf.Abs(oldValue01 - newValue01) * _maxTimeToStabilize;
			highestMaxTimer += maxTimerStart; please fucking kill me
			highestMinTimer += minTimerStart;
			maxTimer += maxTimerStart;
			minTimer += minTimerStart;
		}
		if(minTimer > 0f){
			minValue01 = Mathf.Lerp(minValue01, newValue01, 1 - minTimer / highestMinTimer);
			minTimer -= Time.deltaTime;
		}
		if(maxTimer > 0f){
			maxValue01 = Mathf.Lerp(maxValue01, newValue01, 1 - maxTimer / highestMaxTimer);
			maxTimer -= Time.deltaTime;
		}*/

	}
	public static bool Approximately(float a, float b, float absolutionModifier = 1E-06f, float epsilonModifier = 8f)
    {
        return Mathf.Abs(b - a) < Mathf.Max(absolutionModifier * Mathf.Max(Mathf.Abs(a), Mathf.Abs(b)), Mathf.Epsilon * epsilonModifier);
    }
	void SlowBarUpdate(){
		if(!Approximately(currentValue01, targetValue01, approximationAbsolutionModifier, approximationEpsilonModifier) && targetValue01 >= currentValue01)
			if(value > 0) waitAfterChange = _waitAfterChange;

		if(waitAfterChange > 0f)
			waitAfterChange -= Time.deltaTime;
		else{
			if(currentSlowValue01 > targetValue01)
				currentSlowValue01 = Mathf.MoveTowards(currentSlowValue01, targetValue01, Time.deltaTime * drainRate);
			else
				currentSlowValue01 = targetValue01;
		}
		slowValueFill.fillAmount = currentSlowValue01;
	}
}
