using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(AudioSource))]
	public sealed class UIButton : MonoBehaviour, ISelectHandler, IDeselectHandler
	{
		[SerializeField]
		private Sprite _normalSprite = null;                        // Normal sprite when button is not selected
		[SerializeField]
		private Sprite _selectedSprite = null;                      // Selected sprite when button is selected
		[SerializeField]
		private Sprite _clickSprite = null;                         // Click sprite when button is clicked
		[SerializeField]
		private Sprite _lockedSprite = null;                        // Lock sprite when button is locked
		[SerializeField]
		private AudioClip _selectSound = null;                      // Button select sound
		[SerializeField]
		private AudioClip _clickSound = null;                       // Button click sound

		private Button _button = null;                              // Reference to Button component
		private Image _image = null;                                // Reference to Image component
		private AudioSource _audioSource = null;                    // Reference to AudioSource component

		public bool _firstEnable = true;

		public delegate void UIButtonOnClick(UIButton uiButton);    // Delegate to assign methods to UIButton

		public UIButtonOnClick onClick;                             // To hold UIButton action method

		private void Awake()
		{
			// Get Button component
			_button = GetComponent<Button>();
			// Get Image component
			_image = GetComponent<Image>();
			// Get AudioSource component
			_audioSource = GetComponent<AudioSource>();
		}

		// Use this for initialization
		//private void Start()
		//{
            
		//}

		// Update is called once per frame
		//private void Update()
		//{

		//}

		public void OnSelect(BaseEventData eventData)
		{
            // Change sprite to select sprite
            if (_image != null)
                _image.sprite = _selectedSprite;

			if (_audioSource != null && !_firstEnable)
				_audioSource.PlayOneShot(_selectSound);
		}

		public void OnDeselect(BaseEventData eventData)
		{
            // Change sprite to normal sprite
            if (_image != null)
                _image.sprite = _normalSprite;
		}

		public void OnClick()
		{
            // Change sprite to click sprite
            if (_image != null)
                _image.sprite = _clickSprite;

			if (_audioSource != null)
				_audioSource.PlayOneShot(_clickSound);
		}

        public void SelectButtonOnEnable()
        {
            // Set selected sprite
            if (_image != null)
                _image.sprite = _selectedSprite;
			// Select the button
            if (_button != null)
                _button.Select();
			// Set first enable flag to false
			_firstEnable = false;
		}
    }
}
