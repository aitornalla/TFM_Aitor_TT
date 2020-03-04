using Assets.Scripts.GameController;
using Assets.Scripts.GameController.PlatformControllers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

namespace Assets.Scripts.GameManager
{
	public class GameManager : MonoBehaviour {

		private static GameManager _instance = null;
		private static IGameController _gameControllerInstance = null;

		private const string ControllersXMLPath = @"Assets/ConfigFiles/controllers.xml";

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
					throw new ArgumentNullException ("l_xElem", "Connected controller not in the controllers list! Default: keyboard");
				}
				// Combine namespace and name to get full name
				string l_fullName = string.Join(".", new string[] { l_xElem.Element("namespace").Value, l_xElem.Element("name").Value });
				// Add IGameController component
				_gameControllerInstance = (IGameController)gameObject.AddComponent (Type.GetType (l_fullName));

				Debug.Log ("Controller: " + l_controllerName);
			}
			catch (IndexOutOfRangeException e)
			{
				Debug.Log("No controller connected! Default: keyboard");
			}
			catch (ArgumentNullException e)
			{
				Debug.Log(e.Message);
			}
			catch (Exception e)
			{
				Debug.Log (e.InnerException.Message);
				Debug.Log("Switching to default: keyboard");
			}
			finally
			{
				if (_gameControllerInstance == null)
				{
					_gameControllerInstance = (IGameController)gameObject.AddComponent<KeyboardGameController>();
				}
			}
		}
		#endregion
	}	
}

