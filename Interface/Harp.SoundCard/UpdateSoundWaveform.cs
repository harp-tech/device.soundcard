using System;
using System.ComponentModel;
using System.Reactive.Linq;
using Bonsai;
using OpenCV.Net;

namespace Harp.SoundCard
{
    /// <summary>
    /// Represents an operator that replaces the specified sound waveform in the
    /// SoundCard device with each of the sample buffers in the sequence.
    /// </summary>
    [Description("Replaces the specified sound waveform in the SoundCard device with each of the sample buffers in the sequence.")]
    public class UpdateSoundWaveform : Sink<byte[]>
    {
        /// <summary>
        /// Gets or sets the index of the SoundCard device to update. If no index
        /// is specified, the first SoundCard will be used.
        /// </summary>
        [Description("The index of the SoundCard device to update. If no index is specified, the first SoundCard will be used.")]
        public int? DeviceIndex { get; set; }

        /// <summary>
        /// Gets or sets the index of the sound to update.
        /// </summary>
        [Range(2, 31)]
        [Editor(DesignTypes.NumericUpDownEditor, DesignTypes.UITypeEditor)]
        [Description("The index of the sound to update.")]
        public int SoundIndex { get; set; } = 2;

        /// <summary>
        /// Gets or sets the name of the sound to be stored in the device.
        /// This field is optional.
        /// </summary>
        [Description("The name of the sound to be stored in the device. This field is optional.")]
        public string SoundName { get; set; }

        /// <summary>
        /// Gets or sets a value specifying the sample rate used to playback the
        /// sound waveform.
        /// </summary>
        [Description("Specifies the sample rate used to playback the sound waveform.")]
        public SampleRate SampleRate { get; set; }

        /// <summary>
        /// Replaces the specified sound waveform in the SoundCard device with each
        /// of the sample buffers in an observable sequence.
        /// </summary>
        /// <param name="source">
        /// A sequence of binary array objects representing all the raw samples of
        /// the sound waveform. Continuous streaming is not supported.
        /// </param>
        /// <returns>
        /// An observable sequence that is identical to the <paramref name="source"/> sequence
        /// but where there is an additional side effect of replacing the sound waveform
        /// of the specified sound with each of the sample buffers in the sequence.
        /// </returns>
        public override IObservable<byte[]> Process(IObservable<byte[]> source)
        {
            return source.Do(value =>
            {
                UpdateWaveform(DeviceIndex, SoundIndex, SampleRate, SampleType.Int32, value, SoundName);
            });
        }

        /// <summary>
        /// Replaces the specified sound waveform in the SoundCard device with each
        /// of the sample buffers in an observable sequence.
        /// </summary>
        /// <param name="source">
        /// A sequence of <see cref="Mat"/> objects representing all the raw samples of
        /// the sound waveform. Continuous streaming is not supported.
        /// </param>
        /// <returns>
        /// An observable sequence that is identical to the <paramref name="source"/> sequence
        /// but where there is an additional side effect of replacing the sound waveform
        /// of the specified sound with each of the sample buffers in the sequence.
        /// </returns>
        public IObservable<Mat> Process(IObservable<Mat> source)
        {
            return source.Do(value =>
            {
                if (value.Cols != 1 && value.Rows != 1 || value.Channels != 1)
                {
                    throw new InvalidOperationException("Sound waveforms must have a single channel.");
                }

                var soundWaveform = new byte[sizeof(int) * value.Cols * value.Rows];
                using (var waveformHeader = Mat.CreateMatHeader(soundWaveform, value.Rows, value.Cols, Depth.S32, channels: 1))
                {
                    CV.Convert(value, waveformHeader);
                }

                UpdateWaveform(DeviceIndex, SoundIndex, SampleRate, SampleType.Int32, soundWaveform, SoundName);
            });
        }

        static void UpdateWaveform(
            int? deviceIndex,
            int soundIndex,
            SampleRate sampleRate,
            SampleType sampleType,
            byte[] soundWaveform,
            string soundName = null)
        {
            var errorCode = WaveformHelper.WriteSoundWaveform(deviceIndex, soundIndex, sampleRate, sampleType, soundWaveform, soundName);
            if (errorCode != SoundCardErrorCode.Ok)
            {
                SoundCardErrorHelper.ThrowExceptionForErrorCode(errorCode);
            }
        }
    }
}
