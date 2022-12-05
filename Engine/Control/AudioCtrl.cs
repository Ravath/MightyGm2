using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MightyGm2.Engine.Control
{
    public class AudioCtrl
    {
        private List<TrackControl> _tracks = new List<TrackControl>();

        /// <summary>
        /// Collection of the Tracks currently loaded.
        /// </summary>
        public IEnumerable<TrackControl> Tracks { get { return _tracks; } }

        public AudioCtrl()
        {

        }

        /// <summary>
        /// Load the given file as a track.
        /// </summary>
        /// <param name="track"></param>
        /// <returns>Null if can't read the soundtrack.</returns>
        public TrackControl GetTrack(FileInfo track)
        {
            AudioFileReader audio = new AudioFileReader(track.FullName);
            TrackControl newTrack = null;
            if (audio.CanRead)
            {
                newTrack = new TrackControl(this, audio);
                _tracks.Add(newTrack);
            }
            return newTrack;
        }

        /// <summary>
        /// Remove the given track from the loaded files. Called at TrackControl Disposal.
        /// </summary>
        /// <param name="track">The track to remove.</param>
        internal void RemoveTrack(TrackControl track)
        {
            _tracks.Remove(track);
        }
    }
}
