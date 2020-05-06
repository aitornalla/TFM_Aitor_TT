using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.GameController
{
	/// <summary>
	/// 	Implements generic game actions across all the game so the controller implementation can be abstracted.
	/// </summary>
    public interface IGameController
    {
		/// <summary>
		/// 	Enables or disable the controller debug implementation
		/// </summary>
		/// <param name="enable">If set to <c>true</c> debug is enabled.</param>
		void ControllerDebug(bool enable);

		#region Player
		bool PlayerAttack();
		bool PlayerThrow();
		bool PlayerJump();
		bool PlayerLeft();
		bool PlayerRight();
		bool PlayerUp();
		bool PlayerDown();
		bool PlayerSliding();
		bool PlayerQuitSliding();
		bool PlayerGliding();
		bool PlayerQuitGliding();
		#endregion

		#region Options
		bool Pause();
		bool Accept();
		bool Cancel();
		bool Option();
		#endregion

		#region Menu
		bool MenuLeft();
		bool MenuRight();
		bool MenuUp();
		bool MenuDown();
		#endregion
    }
}
