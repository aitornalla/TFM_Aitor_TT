using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.GameManagerController;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(Toggle))]
    [RequireComponent(typeof(AudioSource))]
	public sealed class UIToggle : MonoBehaviour, ISelectHandler, IDeselectHandler, IUISelectable
	{
		[SerializeField]
		private Sprite _selectedSprite = null;                                  // Selected sprite when toggle is selected
		[SerializeField]
		private Image _checkmarkImage = null;                                   // Checkmark Image inside the toggle
		[SerializeField]
		private AudioClip _selectSound = null;                                  // Button select sound
		[SerializeField]
		private AudioClip _clickSound = null;                                   // Button click sound

		private Toggle _toggle = null;                                          // Reference to Button component
		private AudioSource _audioSource = null;                                // Reference to AudioSource component
		private bool _firstEnable = true;

		#region IUISelectable implementation
		public Selectable Selectable { get { return GetComponent<Toggle>(); } }
		public UISelectableOnClick OnClickCallback { get; set; }
		public bool FirstEnable
		{
			get
			{ return _firstEnable; }

			set
			{ _firstEnable = value; }
		}
		#endregion

		private void Awake()
		{
			// Get Toggle component
			_toggle = GetComponent<Toggle>();
			// Get AudioSource component
			_audioSource = GetComponent<AudioSource>();
			// Assign AudioMixerGroup for AudioSource component
			_audioSource.outputAudioMixerGroup =
				GameManager.Instance.AudioMixerController.MainAudioMixerGroups[0];
		}

        // Use this for initialization
        //private void Start()
        //{
        //    
        //}

        // Update is called once per frame
        //private void Update()
        //{
        //
        //}

        /// <summary>
        ///     Set toogle on/off
        /// </summary>
        /// <param name="isOn">Is toggle on or off?</param>
        public void SetToggle(bool isOn)
        {
			// Set toggle isOn flag
			_toggle.isOn = isOn;
			// Change checkmark whether is on or not
			_checkmarkImage.sprite = isOn ? _selectedSprite : null;
		}

        #region ISelectHandler implementation
        public void OnSelect(BaseEventData eventData)
		{
			// Change sprite to select sprite
			//if (_image != null)
			//    _image.sprite = _selectedSprite;

			if (_audioSource != null && !FirstEnable)
				_audioSource.PlayOneShot(_selectSound);
		}
        #endregion

        #region IDeselectHandler implementation
        public void OnDeselect(BaseEventData eventData)
		{
            // Change sprite to normal sprite
            //if (_image != null)
            //    _image.sprite = _normalSprite;
		}
		#endregion

		#region IUISelectable implementation
		public void OnClick()
		{
            // Change checkmark whether is on or not
			_checkmarkImage.sprite = _toggle.isOn ? _selectedSprite : null;

			if (_audioSource != null)
				_audioSource.PlayOneShot(_clickSound);
		}

		public void SelectOnEnable()
        {
			// Change checkmark whether is on or not
			_checkmarkImage.sprite = _toggle.isOn ? _selectedSprite : null;
			// Set first enable flag to false
			FirstEnable = false;
		}
        #endregion
    }
}
