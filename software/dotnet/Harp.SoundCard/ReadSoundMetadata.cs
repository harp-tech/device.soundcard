using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Linq;
using Bonsai;

namespace Harp.SoundCard
{
    /// <summary>
    /// Represents an operator that reads the metadata of the sounds stored in the SoundCard device.
    /// </summary>
    [Description("Reads the metadata of the sounds stored in the SoundCard device.")]
    public class ReadSoundMetadata : Source<SoundMetadata>
    {
        /// <summary>
        /// Gets or sets the index of the SoundCard device to read. If no index
        /// is specified, the first SoundCard will be used.
        /// </summary>
        [Description("The index of the SoundCard device to read. If no index is specified, the first SoundCard will be used.")]
        public int? DeviceIndex { get; set; }

        /// <summary>
        /// Gets or sets the index of the sound to read. If no index is specified,
        /// every stored sound is read.
        /// </summary>
        [Range(2, 31)]
        [Description("The index of the sound to read. If no index is specified, every stored sound is read.")]
        public int? SoundIndex { get; set; }

        /// <summary>
        /// Generates an observable sequence of the metadata of the sounds stored in the SoundCard device.
        /// </summary>
        /// <returns>
        /// A sequence of <see cref="SoundMetadata"/> objects, one for each stored sound,
        /// which completes after the last sound is read.
        /// </returns>
        public override IObservable<SoundMetadata> Generate()
        {
            return Observable.Defer(() =>
            {
                var sounds = new List<SoundMetadata>();
                var errorCode = WaveformHelper.ReadSoundMetadata(DeviceIndex, SoundIndex, sounds);
                if (errorCode != SoundCardErrorCode.Ok)
                {
                    SoundCardErrorHelper.ThrowExceptionForErrorCode(errorCode);
                }

                return sounds.ToObservable();
            });
        }
    }
}
