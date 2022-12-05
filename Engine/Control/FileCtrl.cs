using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MightyGm2.Engine.Control
{
	public class FileCtrl
	{
		#region File Compatibility
		private string[] _imageExtensions = { ".jpg", ".jpeg", ".png" };
		public bool IsCompatibleImage(string extension)
		{
			return _imageExtensions.Contains(extension);
		}
		private string[] _soundtrackExtensions = { ".mp3", ".wma", ".ogg" };
		public bool IsCompatibleSoundtrack(string extension)
		{
			return _soundtrackExtensions.Contains(extension);
		}
		private string[] _textExtensions = { ".txt" };
		public bool IsCompatibleText(string extension)
		{
			return _textExtensions.Contains(extension);
		}
		private string[] _archiveExtensions = { ".zip", ".7z" };
		internal bool IsCompatibleArchive(string extension)
		{
			return _archiveExtensions.Contains(extension);
		}
		private string[] _pdfExtensions = { ".pdf" };
		internal bool IsCompatiblePdf(string extension)
		{
			return _pdfExtensions.Contains(extension);
		}
		private string[] _videoExtensions = { ".mkv", ".mp4", ".avi" };
		internal bool IsCompatibleVideo(string extension)
		{
			return _videoExtensions.Contains(extension);
		}
		#endregion

	}
}
