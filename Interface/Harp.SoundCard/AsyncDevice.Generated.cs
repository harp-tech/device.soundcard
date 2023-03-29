using Bonsai.Harp;
using System.Threading.Tasks;

namespace Harp.SoundCard
{
    /// <inheritdoc/>
    public partial class Device
    {
        /// <summary>
        /// Initializes a new instance of the asynchronous API to configure and interface
        /// with SoundCard devices on the specified serial port.
        /// </summary>
        /// <param name="portName">
        /// The name of the serial port used to communicate with the Harp device.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous initialization operation. The value of
        /// the <see cref="Task{TResult}.Result"/> parameter contains a new instance of
        /// the <see cref="AsyncDevice"/> class.
        /// </returns>
        public static async Task<AsyncDevice> CreateAsync(string portName)
        {
            var device = new AsyncDevice(portName);
            var whoAmI = await device.ReadWhoAmIAsync();
            if (whoAmI != Device.WhoAmI)
            {
                var errorMessage = string.Format(
                    "The device ID {1} on {0} was unexpected. Check whether a SoundCard device is connected to the specified serial port.",
                    portName, whoAmI);
                throw new HarpException(errorMessage);
            }

            return device;
        }
    }

    /// <summary>
    /// Represents an asynchronous API to configure and interface with SoundCard devices.
    /// </summary>
    public partial class AsyncDevice : Bonsai.Harp.AsyncDevice
    {
        internal AsyncDevice(string portName)
            : base(portName)
        {
        }

        /// <summary>
        /// Asynchronously reads the contents of the PlaySoundOrFrequency register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<ushort> ReadPlaySoundOrFrequencyAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(PlaySoundOrFrequency.Address));
            return PlaySoundOrFrequency.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the PlaySoundOrFrequency register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<ushort>> ReadTimestampedPlaySoundOrFrequencyAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(PlaySoundOrFrequency.Address));
            return PlaySoundOrFrequency.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the PlaySoundOrFrequency register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WritePlaySoundOrFrequencyAsync(ushort value)
        {
            var request = PlaySoundOrFrequency.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the Stop register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<byte> ReadStopAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(Stop.Address));
            return Stop.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the Stop register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<byte>> ReadTimestampedStopAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(Stop.Address));
            return Stop.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the Stop register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteStopAsync(byte value)
        {
            var request = Stop.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the AttenuationLeft register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<ushort> ReadAttenuationLeftAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(AttenuationLeft.Address));
            return AttenuationLeft.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the AttenuationLeft register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<ushort>> ReadTimestampedAttenuationLeftAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(AttenuationLeft.Address));
            return AttenuationLeft.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the AttenuationLeft register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteAttenuationLeftAsync(ushort value)
        {
            var request = AttenuationLeft.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the AttenuationRight register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<ushort> ReadAttenuationRightAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(AttenuationRight.Address));
            return AttenuationRight.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the AttenuationRight register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<ushort>> ReadTimestampedAttenuationRightAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(AttenuationRight.Address));
            return AttenuationRight.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the AttenuationRight register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteAttenuationRightAsync(ushort value)
        {
            var request = AttenuationRight.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the AttenuationBoth register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<ushort[]> ReadAttenuationBothAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(AttenuationBoth.Address));
            return AttenuationBoth.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the AttenuationBoth register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<ushort[]>> ReadTimestampedAttenuationBothAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(AttenuationBoth.Address));
            return AttenuationBoth.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the AttenuationBoth register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteAttenuationBothAsync(ushort[] value)
        {
            var request = AttenuationBoth.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the AttenuationAndPlaySoundOrFreq register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<ushort[]> ReadAttenuationAndPlaySoundOrFreqAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(AttenuationAndPlaySoundOrFreq.Address));
            return AttenuationAndPlaySoundOrFreq.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the AttenuationAndPlaySoundOrFreq register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<ushort[]>> ReadTimestampedAttenuationAndPlaySoundOrFreqAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(AttenuationAndPlaySoundOrFreq.Address));
            return AttenuationAndPlaySoundOrFreq.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the AttenuationAndPlaySoundOrFreq register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteAttenuationAndPlaySoundOrFreqAsync(ushort[] value)
        {
            var request = AttenuationAndPlaySoundOrFreq.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the InputState register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<DigitalInputs> ReadInputStateAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(InputState.Address));
            return InputState.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the InputState register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<DigitalInputs>> ReadTimestampedInputStateAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(InputState.Address));
            return InputState.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the contents of the ConfigureDI0 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<DigitalInputConfiguration> ReadConfigureDI0Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(ConfigureDI0.Address));
            return ConfigureDI0.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the ConfigureDI0 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<DigitalInputConfiguration>> ReadTimestampedConfigureDI0Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(ConfigureDI0.Address));
            return ConfigureDI0.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the ConfigureDI0 register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteConfigureDI0Async(DigitalInputConfiguration value)
        {
            var request = ConfigureDI0.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the ConfigureDI1 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<DigitalInputConfiguration> ReadConfigureDI1Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(ConfigureDI1.Address));
            return ConfigureDI1.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the ConfigureDI1 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<DigitalInputConfiguration>> ReadTimestampedConfigureDI1Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(ConfigureDI1.Address));
            return ConfigureDI1.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the ConfigureDI1 register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteConfigureDI1Async(DigitalInputConfiguration value)
        {
            var request = ConfigureDI1.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the ConfigureDI2 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<DigitalInputConfiguration> ReadConfigureDI2Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(ConfigureDI2.Address));
            return ConfigureDI2.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the ConfigureDI2 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<DigitalInputConfiguration>> ReadTimestampedConfigureDI2Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(ConfigureDI2.Address));
            return ConfigureDI2.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the ConfigureDI2 register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteConfigureDI2Async(DigitalInputConfiguration value)
        {
            var request = ConfigureDI2.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the SoundIndexDI0 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<byte> ReadSoundIndexDI0Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(SoundIndexDI0.Address));
            return SoundIndexDI0.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the SoundIndexDI0 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<byte>> ReadTimestampedSoundIndexDI0Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(SoundIndexDI0.Address));
            return SoundIndexDI0.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the SoundIndexDI0 register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteSoundIndexDI0Async(byte value)
        {
            var request = SoundIndexDI0.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the SoundIndexDI1 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<byte> ReadSoundIndexDI1Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(SoundIndexDI1.Address));
            return SoundIndexDI1.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the SoundIndexDI1 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<byte>> ReadTimestampedSoundIndexDI1Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(SoundIndexDI1.Address));
            return SoundIndexDI1.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the SoundIndexDI1 register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteSoundIndexDI1Async(byte value)
        {
            var request = SoundIndexDI1.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the SoundIndexDI2 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<byte> ReadSoundIndexDI2Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(SoundIndexDI2.Address));
            return SoundIndexDI2.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the SoundIndexDI2 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<byte>> ReadTimestampedSoundIndexDI2Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(SoundIndexDI2.Address));
            return SoundIndexDI2.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the SoundIndexDI2 register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteSoundIndexDI2Async(byte value)
        {
            var request = SoundIndexDI2.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the FrequencyDI0 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<ushort> ReadFrequencyDI0Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(FrequencyDI0.Address));
            return FrequencyDI0.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the FrequencyDI0 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<ushort>> ReadTimestampedFrequencyDI0Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(FrequencyDI0.Address));
            return FrequencyDI0.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the FrequencyDI0 register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteFrequencyDI0Async(ushort value)
        {
            var request = FrequencyDI0.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the FrequencyDI1 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<ushort> ReadFrequencyDI1Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(FrequencyDI1.Address));
            return FrequencyDI1.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the FrequencyDI1 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<ushort>> ReadTimestampedFrequencyDI1Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(FrequencyDI1.Address));
            return FrequencyDI1.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the FrequencyDI1 register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteFrequencyDI1Async(ushort value)
        {
            var request = FrequencyDI1.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the FrequencyDI2 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<ushort> ReadFrequencyDI2Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(FrequencyDI2.Address));
            return FrequencyDI2.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the FrequencyDI2 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<ushort>> ReadTimestampedFrequencyDI2Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(FrequencyDI2.Address));
            return FrequencyDI2.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the FrequencyDI2 register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteFrequencyDI2Async(ushort value)
        {
            var request = FrequencyDI2.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the AttenuationLeftDI0 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<ushort> ReadAttenuationLeftDI0Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(AttenuationLeftDI0.Address));
            return AttenuationLeftDI0.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the AttenuationLeftDI0 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<ushort>> ReadTimestampedAttenuationLeftDI0Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(AttenuationLeftDI0.Address));
            return AttenuationLeftDI0.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the AttenuationLeftDI0 register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteAttenuationLeftDI0Async(ushort value)
        {
            var request = AttenuationLeftDI0.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the AttenuationLeftDI1 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<ushort> ReadAttenuationLeftDI1Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(AttenuationLeftDI1.Address));
            return AttenuationLeftDI1.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the AttenuationLeftDI1 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<ushort>> ReadTimestampedAttenuationLeftDI1Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(AttenuationLeftDI1.Address));
            return AttenuationLeftDI1.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the AttenuationLeftDI1 register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteAttenuationLeftDI1Async(ushort value)
        {
            var request = AttenuationLeftDI1.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the AttenuationLeftDI2 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<ushort> ReadAttenuationLeftDI2Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(AttenuationLeftDI2.Address));
            return AttenuationLeftDI2.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the AttenuationLeftDI2 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<ushort>> ReadTimestampedAttenuationLeftDI2Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(AttenuationLeftDI2.Address));
            return AttenuationLeftDI2.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the AttenuationLeftDI2 register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteAttenuationLeftDI2Async(ushort value)
        {
            var request = AttenuationLeftDI2.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the AttenuationRightDI0 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<ushort> ReadAttenuationRightDI0Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(AttenuationRightDI0.Address));
            return AttenuationRightDI0.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the AttenuationRightDI0 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<ushort>> ReadTimestampedAttenuationRightDI0Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(AttenuationRightDI0.Address));
            return AttenuationRightDI0.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the AttenuationRightDI0 register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteAttenuationRightDI0Async(ushort value)
        {
            var request = AttenuationRightDI0.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the AttenuationRightDI1 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<ushort> ReadAttenuationRightDI1Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(AttenuationRightDI1.Address));
            return AttenuationRightDI1.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the AttenuationRightDI1 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<ushort>> ReadTimestampedAttenuationRightDI1Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(AttenuationRightDI1.Address));
            return AttenuationRightDI1.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the AttenuationRightDI1 register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteAttenuationRightDI1Async(ushort value)
        {
            var request = AttenuationRightDI1.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the AttenuationRightDI2 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<ushort> ReadAttenuationRightDI2Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(AttenuationRightDI2.Address));
            return AttenuationRightDI2.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the AttenuationRightDI2 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<ushort>> ReadTimestampedAttenuationRightDI2Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(AttenuationRightDI2.Address));
            return AttenuationRightDI2.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the AttenuationRightDI2 register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteAttenuationRightDI2Async(ushort value)
        {
            var request = AttenuationRightDI2.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the AttenuationAndSoundIndexDI0 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<ushort[]> ReadAttenuationAndSoundIndexDI0Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(AttenuationAndSoundIndexDI0.Address));
            return AttenuationAndSoundIndexDI0.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the AttenuationAndSoundIndexDI0 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<ushort[]>> ReadTimestampedAttenuationAndSoundIndexDI0Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(AttenuationAndSoundIndexDI0.Address));
            return AttenuationAndSoundIndexDI0.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the AttenuationAndSoundIndexDI0 register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteAttenuationAndSoundIndexDI0Async(ushort[] value)
        {
            var request = AttenuationAndSoundIndexDI0.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the AttenuationAndSoundIndexDI1 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<ushort[]> ReadAttenuationAndSoundIndexDI1Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(AttenuationAndSoundIndexDI1.Address));
            return AttenuationAndSoundIndexDI1.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the AttenuationAndSoundIndexDI1 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<ushort[]>> ReadTimestampedAttenuationAndSoundIndexDI1Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(AttenuationAndSoundIndexDI1.Address));
            return AttenuationAndSoundIndexDI1.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the AttenuationAndSoundIndexDI1 register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteAttenuationAndSoundIndexDI1Async(ushort[] value)
        {
            var request = AttenuationAndSoundIndexDI1.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the AttenuationAndSoundIndexDI2 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<ushort[]> ReadAttenuationAndSoundIndexDI2Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(AttenuationAndSoundIndexDI2.Address));
            return AttenuationAndSoundIndexDI2.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the AttenuationAndSoundIndexDI2 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<ushort[]>> ReadTimestampedAttenuationAndSoundIndexDI2Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(AttenuationAndSoundIndexDI2.Address));
            return AttenuationAndSoundIndexDI2.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the AttenuationAndSoundIndexDI2 register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteAttenuationAndSoundIndexDI2Async(ushort[] value)
        {
            var request = AttenuationAndSoundIndexDI2.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the AttenuationAndFrequencyDI0 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<ushort[]> ReadAttenuationAndFrequencyDI0Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(AttenuationAndFrequencyDI0.Address));
            return AttenuationAndFrequencyDI0.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the AttenuationAndFrequencyDI0 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<ushort[]>> ReadTimestampedAttenuationAndFrequencyDI0Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(AttenuationAndFrequencyDI0.Address));
            return AttenuationAndFrequencyDI0.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the AttenuationAndFrequencyDI0 register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteAttenuationAndFrequencyDI0Async(ushort[] value)
        {
            var request = AttenuationAndFrequencyDI0.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the AttenuationAndFrequencyDI1 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<ushort[]> ReadAttenuationAndFrequencyDI1Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(AttenuationAndFrequencyDI1.Address));
            return AttenuationAndFrequencyDI1.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the AttenuationAndFrequencyDI1 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<ushort[]>> ReadTimestampedAttenuationAndFrequencyDI1Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(AttenuationAndFrequencyDI1.Address));
            return AttenuationAndFrequencyDI1.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the AttenuationAndFrequencyDI1 register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteAttenuationAndFrequencyDI1Async(ushort[] value)
        {
            var request = AttenuationAndFrequencyDI1.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the AttenuationAndFrequencyDI2 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<ushort[]> ReadAttenuationAndFrequencyDI2Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(AttenuationAndFrequencyDI2.Address));
            return AttenuationAndFrequencyDI2.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the AttenuationAndFrequencyDI2 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<ushort[]>> ReadTimestampedAttenuationAndFrequencyDI2Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(AttenuationAndFrequencyDI2.Address));
            return AttenuationAndFrequencyDI2.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the AttenuationAndFrequencyDI2 register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteAttenuationAndFrequencyDI2Async(ushort[] value)
        {
            var request = AttenuationAndFrequencyDI2.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the ConfigureDO0 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<DigitalOutputConfiguration> ReadConfigureDO0Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(ConfigureDO0.Address));
            return ConfigureDO0.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the ConfigureDO0 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<DigitalOutputConfiguration>> ReadTimestampedConfigureDO0Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(ConfigureDO0.Address));
            return ConfigureDO0.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the ConfigureDO0 register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteConfigureDO0Async(DigitalOutputConfiguration value)
        {
            var request = ConfigureDO0.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the ConfigureDO1 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<DigitalOutputConfiguration> ReadConfigureDO1Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(ConfigureDO1.Address));
            return ConfigureDO1.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the ConfigureDO1 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<DigitalOutputConfiguration>> ReadTimestampedConfigureDO1Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(ConfigureDO1.Address));
            return ConfigureDO1.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the ConfigureDO1 register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteConfigureDO1Async(DigitalOutputConfiguration value)
        {
            var request = ConfigureDO1.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the ConfigureDO2 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<DigitalOutputConfiguration> ReadConfigureDO2Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(ConfigureDO2.Address));
            return ConfigureDO2.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the ConfigureDO2 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<DigitalOutputConfiguration>> ReadTimestampedConfigureDO2Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(ConfigureDO2.Address));
            return ConfigureDO2.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the ConfigureDO2 register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteConfigureDO2Async(DigitalOutputConfiguration value)
        {
            var request = ConfigureDO2.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the PulseDO0 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<byte> ReadPulseDO0Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(PulseDO0.Address));
            return PulseDO0.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the PulseDO0 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<byte>> ReadTimestampedPulseDO0Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(PulseDO0.Address));
            return PulseDO0.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the PulseDO0 register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WritePulseDO0Async(byte value)
        {
            var request = PulseDO0.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the PulseDO1 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<byte> ReadPulseDO1Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(PulseDO1.Address));
            return PulseDO1.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the PulseDO1 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<byte>> ReadTimestampedPulseDO1Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(PulseDO1.Address));
            return PulseDO1.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the PulseDO1 register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WritePulseDO1Async(byte value)
        {
            var request = PulseDO1.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the PulseDO2 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<byte> ReadPulseDO2Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(PulseDO2.Address));
            return PulseDO2.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the PulseDO2 register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<byte>> ReadTimestampedPulseDO2Async()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(PulseDO2.Address));
            return PulseDO2.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the PulseDO2 register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WritePulseDO2Async(byte value)
        {
            var request = PulseDO2.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the OutputSet register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<DigitalOutputs> ReadOutputSetAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(OutputSet.Address));
            return OutputSet.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the OutputSet register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<DigitalOutputs>> ReadTimestampedOutputSetAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(OutputSet.Address));
            return OutputSet.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the OutputSet register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteOutputSetAsync(DigitalOutputs value)
        {
            var request = OutputSet.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the OutputClear register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<DigitalOutputs> ReadOutputClearAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(OutputClear.Address));
            return OutputClear.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the OutputClear register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<DigitalOutputs>> ReadTimestampedOutputClearAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(OutputClear.Address));
            return OutputClear.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the OutputClear register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteOutputClearAsync(DigitalOutputs value)
        {
            var request = OutputClear.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the OutputToggle register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<DigitalOutputs> ReadOutputToggleAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(OutputToggle.Address));
            return OutputToggle.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the OutputToggle register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<DigitalOutputs>> ReadTimestampedOutputToggleAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(OutputToggle.Address));
            return OutputToggle.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the OutputToggle register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteOutputToggleAsync(DigitalOutputs value)
        {
            var request = OutputToggle.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the OutputState register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<DigitalOutputs> ReadOutputStateAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(OutputState.Address));
            return OutputState.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the OutputState register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<DigitalOutputs>> ReadTimestampedOutputStateAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(OutputState.Address));
            return OutputState.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the OutputState register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteOutputStateAsync(DigitalOutputs value)
        {
            var request = OutputState.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the ConfigureAdc register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<AdcConfiguration> ReadConfigureAdcAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(ConfigureAdc.Address));
            return ConfigureAdc.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the ConfigureAdc register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<AdcConfiguration>> ReadTimestampedConfigureAdcAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(ConfigureAdc.Address));
            return ConfigureAdc.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the ConfigureAdc register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteConfigureAdcAsync(AdcConfiguration value)
        {
            var request = ConfigureAdc.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the AnalogData register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<AnalogDataPayload> ReadAnalogDataAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(AnalogData.Address));
            return AnalogData.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the AnalogData register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<AnalogDataPayload>> ReadTimestampedAnalogDataAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(AnalogData.Address));
            return AnalogData.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the contents of the Commands register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<ControllerCommand> ReadCommandsAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(Commands.Address));
            return Commands.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the Commands register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<ControllerCommand>> ReadTimestampedCommandsAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(Commands.Address));
            return Commands.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the Commands register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteCommandsAsync(ControllerCommand value)
        {
            var request = Commands.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }

        /// <summary>
        /// Asynchronously reads the contents of the EnableEvents register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<SoundCardEvents> ReadEnableEventsAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(EnableEvents.Address));
            return EnableEvents.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the EnableEvents register.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<SoundCardEvents>> ReadTimestampedEnableEventsAsync()
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(EnableEvents.Address));
            return EnableEvents.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the EnableEvents register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteEnableEventsAsync(SoundCardEvents value)
        {
            var request = EnableEvents.FromPayload(MessageType.Write, value);
            await CommandAsync(request);
        }
    }
}
