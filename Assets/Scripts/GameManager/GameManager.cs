using Assets.Scripts.GameController;
using Assets.Scripts.GameController.PlatformControllers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

namespace Assets.Scripts.GameManager
{
	public sealed class GameManager : MonoBehaviour {

		private static GameManager _instance = null;
		private static IGameController _gameControllerInstance = null;

		private readonly string ControllersXMLPath = string.Join (Path.DirectorySeparatorChar.ToString (), new string[] { "Assets", "ConfigFiles", "controllers.xml" });


		#region Properties
		//public static GameManager Instance { get { return _instance; } }
		//public IGameController GameController { get { return _gameControllerInstance; } }
		#endregion

		#region Awake
		void Awake () {

			// Singleton creation
			if (_instance == null) {

				_instance = this;

				DontDestroyOnLoad (gameObject);

				SetUpController ();

			} else {

				DestroyImmediate (gameObject);

			}

		}
		#endregion

		#region Start
		// Use this for initialization
		void Start () {

		}
		#endregion

		#region Update
		// Update is called once per frame
		void Update () {

		}
		#endregion

		#region Private methods
		/// <summary>
		/// 	Adds controller components to GameManager gameObject.
		/// 	One component must implement interface <see cref="IGameController"/> and the other must implement specific methods from controller buttons/axis
		/// </summary>
		private void SetUpController ()
		{
			try
			{
				// Get controller name
				string l_controllerName = Input.GetJoystickNames()[0];
				// Load controllers xml file
				XDocument l_xDoc = XDocument.Load(ControllersXMLPath);
				// Find controller by attribute ("name")
				XElement l_xElem = l_xDoc.Descendants().Where(atr => (string)atr.Attribute("name") == l_controllerName).FirstOrDefault();
				// If null, controller not in the list
				if (l_xElem == null)
				{
					throw new ArgumentNullException ("l_xElem", "Connected controller not in the controllers list");
				}
				// Combine namespace and name to get full name
				string l_fullName = string.Join(".", new string[] { l_xElem.Element("namespace").Value, l_xElem.Element("name").Value });
				// Add IGameController component
				_gameControllerInstance = (IGameController)gameObject.AddComponent (Type.GetType (l_fullName));

				Debug.Log ("Controller: " + l_controllerName);
			}
			catch (IndexOutOfRangeException e)
			{
				Debug.LogException (e);
				Debug.LogWarning ("No controller connected");
			}
			catch (ArgumentNullException e)
			{
				Debug.LogException (e);
			}
			catch (Exception e)
			{
				Debug.LogException (e);
			}
			finally
			{
				if (_gameControllerInstance == null)
				{
					_gameControllerInstance = (IGameController)gameObject.AddComponent<KeyboardGameController>();

					Debug.Log("Default controller: keyboard");
				}

				// Enables controller debug in development mode
				_gameControllerInstance.ControllerDebug (true);
			}
		}
		#endregion
	}	
}

