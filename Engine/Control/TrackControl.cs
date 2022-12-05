using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MightyGm2.Engine.Control
{

    public class TrackControl : IDisposable
    {
        private AudioCtrl _audioCtrl;

        private WaveOutEvent _outputDevice;
        private AudioFileReader _audioFile;
        private bool _closing = false;

        public bool Loop { get; set; } = false;
        public PlaybackState PlaybackState { get => _outputDevice.PlaybackState; }
        public string File { get => _audioFile.FileName; }
        public TimeSpan CurrentTime { get => _audioFile.CurrentTime; }
        public TimeSpan TotalTime { get => _audioFile.TotalTime; }
        public double Position {
            get => _outputDevice.GetPosition();
            set => SetPosition(value);
        }

        public event EventHandler<StoppedEventArgs> PlaybackStopped
        {
            add { _outputDevice.PlaybackStopped += value; }
            remove { _outputDevice.PlaybackStopped -= value; }
        }

        internal TrackControl(AudioCtrl audioCtrl, AudioFileReader track)
        {
            _audioCtrl = audioCtrl;
            _audioFile = track;
            _outputDevice = new WaveOutEvent();
            _outputDevice.PlaybackStopped += OnPlaybackStopped;
            _outputDevice.Init(_audioFile);
        }

        public void Play()
        {
            _outputDevice.Play();
        }

        public void Stop()
        {
            _outputDevice.Stop();
        }

        public void Pause()
        {
            _outputDevice.Pause();
        }

        private void OnPlaybackStopped(object sender, StoppedEventArgs args)
        {
            if (_closing)
            {
                _outputDevice.Dispose();
                _outputDevice = null;
                _audioFile.Dispose();
                _audioFile = null;
            }
            else if (Loop)
                Play();
        }

        /// <summary>
        /// Set the position of the reader on the track.
        /// </summary>
        /// <param name="position">In percent, from 0.0 to 1.0</param>
        public void SetPosition(double position)
        {
            long newPosition = (long)(_audioFile.Length * position) % _audioFile.Length;
            _audioFile.Position = newPosition;
        }

        /// <summary>
        /// Set the volume of the reader.
        /// </summary>
        /// <param name="position">In percent, from 0.0 to 1.0</param>
        public void SetColume(double volume)
        {
            _audioFile.Volume = (float)volume;
        }

        public void Dispose()
        {
            _closing = true;
            _outputDevice.Stop();
            _audioCtrl.RemoveTrack(this);
        }
    }
}
