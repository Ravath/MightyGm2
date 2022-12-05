using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MightyGm2.Engine.Control
{
	/// <summary>
	/// Main controller of the application.
	/// </summary>
	public class ApplicationControl
	{
		private static ApplicationControl _control;

		/// <summary>
		/// Static singleton access to the application controller.
		/// </summary>
		public static ApplicationControl Control
		{
			get
			{
				if(_control == null)
				{
					_control = new ApplicationControl();
                    _control.PostInit();
                }
				return _control;
			}
		}

		/// <summary>
		/// Database Controller access.
		/// </summary>
		public DatabaseCtrl Database { get; }

		/// <summary>
		/// Files Controller access.
		/// </summary>
		public FileCtrl File { get; }

        /// <summary>
        /// Audio Controller access.
        /// </summary>
        public AudioCtrl Audio { get; }

        /// <summary>
        /// Rpg Context Controller access.
        /// </summary>
        public RpgControl Rpg { get; }

        /// <summary>
        /// Private constructor. Use static singleton.
        /// </summary>
        private ApplicationControl()
		{
			Database = new DatabaseCtrl();
			File = new FileCtrl();
            Audio = new AudioCtrl();
            Rpg = new RpgControl();
		}

        private void PostInit()
        {
            Rpg.LoadRpgData();
        }
	}
}
