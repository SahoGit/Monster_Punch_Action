using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyUi : MonoBehaviour {

	public Text _coinsText;
	bool _updatedOnce = false;

	// Use this for initialization
	void OnEnable () {
		CurrencyManager.coinsChangedEvent += UpdateCoinsUi;
	}

	void Start()
	{
		if (!_updatedOnce)
		{
			UpdateCoinsUi(CurrencyManager.instance._coinsPref);
		

		}
	}
	// Update is called once per frame
	void OnDisable () {
		CurrencyManager.coinsChangedEvent -= UpdateCoinsUi;
	}

	void UpdateCoinsUi(string _currencyName)
	{
		if (!_coinsText)
			return;
		int _value = PlayerPrefs.GetInt(_currencyName,0);
		_coinsText.text = "" + _value;
		_updatedOnce = true;
		Debug.Log(_value);
	}

	
}
