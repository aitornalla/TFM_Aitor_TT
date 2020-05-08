using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.GameManagerController;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public enum EButtonSpriteType
    {
		Clicked,
		Locked,
		Normal,
        Selected
    }

    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(AudioSource))]
	public sealed class UIButton : MonoBehaviour, ISelectHandler, IDeselectHandler, IUISelectable
	{
		[SerializeField]
		private Sprite _normalSprite = null;                                    // Normal sprite when button is not selected
		[SerializeField]
		private Sprite _selectedSprite = null;                                  // Selected sprite when button is selected
		[SerializeField]
		private Sprite _clickSprite = null;                                     // Click sprite when button is clicked
		[SerializeField]
		private Sprite _lockedSprite = null;                                    // Lock sprite when button is locked
		[SerializeField]
		private AudioClip _selectSound = null;                                  // Button select sound
		[SerializeField]
		private AudioClip _clickSound = null;                                   // Button click sound

		private Button _button = null;                                          // Reference to Button component
		private Image _image = null;                                            // Reference to Image component
		private AudioSource _audioSource = null;                                // Reference to AudioSource component
		private bool _firstEnable = true;
		private Coroutine _changeButtonSpriteToSelectedCoroutine = null;

		#region IUISelectable implementation
		public Selectable Selectable { get { return GetComponent<Button>(); } }
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
			// Get Button component
			_button = GetComponent<Button>();
			// Get Image component
			_image = GetComponent<Image>();
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

        private void OnEnable()
        {
            if (this.gameObject.Equals(EventSystem.current.currentSelectedGameObject))
            {
				ChangeButtonSprite(EButtonSpriteType.Selected);
            }
            else
            {
				ChangeButtonSprite(EButtonSpriteType.Normal);
            }
        }

        private void OnDisable()
        {
			_changeButtonSpriteToSelectedCoroutine = null;
		}

        #region ISelectHandler implementation
        public void OnSelect(BaseEventData eventData)
		{
            // Change sprite to select sprite
            if (_image != null)
                _image.sprite = _selectedSprite;

			if (_audioSource != null && !FirstEnable)
				_audioSource.PlayOneShot(_selectSound);
		}
        #endregion

        #region IDeselectHandler implementation
        public void OnDeselect(BaseEventData eventData)
		{
            // Change sprite to normal sprite
            if (_image != null)
                _image.sprite = _normalSprite;
		}
		#endregion

		#region IUISelectable implementation
		public void OnClick()
		{
			// Change sprite to click sprite
			if (_image != null)
				_image.sprite = _clickSprite;

			if (_audioSource != null)
				_audioSource.PlayOneShot(_clickSound);

			if (_changeButtonSpriteToSelectedCoroutine == null)
				_changeButtonSpriteToSelectedCoroutine = StartCoroutine(ChangeButtonSpriteToSelectedCoroutine());
		}

		public void SelectOnEnable()
        {
            // Set selected sprite
            if (_image != null)
                _image.sprite = _selectedSprite;
			// Select the button
            if (_button != null)
                _button.Select();
			// Set first enable flag to false
			FirstEnable = false;
		}
		#endregion

		public void ChangeButtonSprite(EButtonSpriteType buttonSpriteType)
        {
			Sprite l_sprite = null;

            switch (buttonSpriteType)
            {
				case EButtonSpriteType.Clicked:
					l_sprite = _clickSprite;
					break;
				case EButtonSpriteType.Locked:
					l_sprite = _lockedSprite;
					break;
				case EButtonSpriteType.Normal:
					l_sprite = _normalSprite;
					break;
				case EButtonSpriteType.Selected:
					l_sprite = _selectedSprite;
					break;
				default:
					l_sprite = _lockedSprite;
					break;
            }

			_image.sprite = l_sprite;
        }

        private IEnumerator ChangeButtonSpriteToSelectedCoroutine()
        {
			yield return new WaitForSeconds(0.1f);

			ChangeButtonSprite(EButtonSpriteType.Selected);

			_changeButtonSpriteToSelectedCoroutine = null;
		}
    }
}
