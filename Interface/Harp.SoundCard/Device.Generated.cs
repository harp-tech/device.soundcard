using Bonsai;
using Bonsai.Harp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Xml.Serialization;

namespace Harp.SoundCard
{
    /// <summary>
    /// Generates events and processes commands for the SoundCard device connected
    /// at the specified serial port.
    /// </summary>
    [Combinator(MethodName = nameof(Generate))]
    [WorkflowElementCategory(ElementCategory.Source)]
    [Description("Generates events and processes commands for the SoundCard device.")]
    public partial class Device : Bonsai.Harp.Device, INamedElement
    {
        /// <summary>
        /// Represents the unique identity class of the <see cref="SoundCard"/> device.
        /// This field is constant.
        /// </summary>
        public const int WhoAmI = 1280;

        /// <summary>
        /// Initializes a new instance of the <see cref="Device"/> class.
        /// </summary>
        public Device() : base(WhoAmI) { }

        string INamedElement.Name => nameof(SoundCard);

        /// <summary>
        /// Gets a read-only mapping from address to register type.
        /// </summary>
        public static new IReadOnlyDictionary<int, Type> RegisterMap { get; } = new Dictionary<int, Type>
            (Bonsai.Harp.Device.RegisterMap.ToDictionary(entry => entry.Key, entry => entry.Value))
        {
            { 32, typeof(PlaySoundOrFrequency) },
            { 33, typeof(Stop) },
            { 34, typeof(AttenuationLeft) },
            { 35, typeof(AttenuationRight) },
            { 36, typeof(AttenuationBoth) },
            { 37, typeof(AttenuationAndPlaySoundOrFreq) },
            { 40, typeof(DigitalInputs) },
            { 41, typeof(ConfigureDI0) },
            { 42, typeof(ConfigureDI1) },
            { 43, typeof(ConfigureDI2) },
            { 44, typeof(SoundIndexDI0) },
            { 45, typeof(SoundIndexDI1) },
            { 46, typeof(SoundIndexDI2) },
            { 47, typeof(FrequencyDI0) },
            { 48, typeof(FrequencyDI1) },
            { 49, typeof(FrequencyDI2) },
            { 50, typeof(AttenuationLeftDI0) },
            { 51, typeof(AttenuationLeftDI1) },
            { 52, typeof(AttenuationLeftDI2) },
            { 53, typeof(AttenuationRightDI0) },
            { 54, typeof(AttenuationRightDI1) },
            { 55, typeof(AttenuationRightDI2) },
            { 56, typeof(AttenuationAndSoundIndexDI0) },
            { 57, typeof(AttenuationAndSoundIndexDI1) },
            { 58, typeof(AttenuationAndSoundIndexDI2) },
            { 59, typeof(AttenuationAndFrequencyDI0) },
            { 60, typeof(AttenuationAndFrequencyDI1) },
            { 61, typeof(AttenuationAndFrequencyDI2) },
            { 65, typeof(ConfigureDO0) },
            { 66, typeof(ConfigureDO1) },
            { 67, typeof(ConfigureDO2) },
            { 68, typeof(PulseDO0) },
            { 69, typeof(PulseDO1) },
            { 70, typeof(PulseDO2) },
            { 74, typeof(OutputSet) },
            { 75, typeof(OutputClear) },
            { 76, typeof(OutputToggle) },
            { 77, typeof(OutputState) },
            { 80, typeof(ConfigureAdc) },
            { 81, typeof(AnalogInputs) },
            { 82, typeof(Commands) },
            { 86, typeof(EnableEvents) }
        };
    }

    /// <summary>
    /// Represents an operator that groups the sequence of <see cref="SoundCard"/>" messages by register type.
    /// </summary>
    [Description("Groups the sequence of SoundCard messages by register type.")]
    public partial class GroupByRegister : Combinator<HarpMessage, IGroupedObservable<Type, HarpMessage>>
    {
        /// <summary>
        /// Groups an observable sequence of <see cref="SoundCard"/> messages
        /// by register type.
        /// </summary>
        /// <param name="source">The sequence of Harp device messages.</param>
        /// <returns>
        /// A sequence of observable groups, each of which corresponds to a unique
        /// <see cref="SoundCard"/> register.
        /// </returns>
        public override IObservable<IGroupedObservable<Type, HarpMessage>> Process(IObservable<HarpMessage> source)
        {
            return source.GroupBy(message => Device.RegisterMap[message.Address]);
        }
    }

    /// <summary>
    /// Represents an operator that filters register-specific messages
    /// reported by the <see cref="SoundCard"/> device.
    /// </summary>
    /// <seealso cref="PlaySoundOrFrequency"/>
    /// <seealso cref="Stop"/>
    /// <seealso cref="AttenuationLeft"/>
    /// <seealso cref="AttenuationRight"/>
    /// <seealso cref="AttenuationBoth"/>
    /// <seealso cref="AttenuationAndPlaySoundOrFreq"/>
    /// <seealso cref="DigitalInputs"/>
    /// <seealso cref="ConfigureDI0"/>
    /// <seealso cref="ConfigureDI1"/>
    /// <seealso cref="ConfigureDI2"/>
    /// <seealso cref="SoundIndexDI0"/>
    /// <seealso cref="SoundIndexDI1"/>
    /// <seealso cref="SoundIndexDI2"/>
    /// <seealso cref="FrequencyDI0"/>
    /// <seealso cref="FrequencyDI1"/>
    /// <seealso cref="FrequencyDI2"/>
    /// <seealso cref="AttenuationLeftDI0"/>
    /// <seealso cref="AttenuationLeftDI1"/>
    /// <seealso cref="AttenuationLeftDI2"/>
    /// <seealso cref="AttenuationRightDI0"/>
    /// <seealso cref="AttenuationRightDI1"/>
    /// <seealso cref="AttenuationRightDI2"/>
    /// <seealso cref="AttenuationAndSoundIndexDI0"/>
    /// <seealso cref="AttenuationAndSoundIndexDI1"/>
    /// <seealso cref="AttenuationAndSoundIndexDI2"/>
    /// <seealso cref="AttenuationAndFrequencyDI0"/>
    /// <seealso cref="AttenuationAndFrequencyDI1"/>
    /// <seealso cref="AttenuationAndFrequencyDI2"/>
    /// <seealso cref="ConfigureDO0"/>
    /// <seealso cref="ConfigureDO1"/>
    /// <seealso cref="ConfigureDO2"/>
    /// <seealso cref="PulseDO0"/>
    /// <seealso cref="PulseDO1"/>
    /// <seealso cref="PulseDO2"/>
    /// <seealso cref="OutputSet"/>
    /// <seealso cref="OutputClear"/>
    /// <seealso cref="OutputToggle"/>
    /// <seealso cref="OutputState"/>
    /// <seealso cref="ConfigureAdc"/>
    /// <seealso cref="AnalogInputs"/>
    /// <seealso cref="Commands"/>
    /// <seealso cref="EnableEvents"/>
    [XmlInclude(typeof(PlaySoundOrFrequency))]
    [XmlInclude(typeof(Stop))]
    [XmlInclude(typeof(AttenuationLeft))]
    [XmlInclude(typeof(AttenuationRight))]
    [XmlInclude(typeof(AttenuationBoth))]
    [XmlInclude(typeof(AttenuationAndPlaySoundOrFreq))]
    [XmlInclude(typeof(DigitalInputs))]
    [XmlInclude(typeof(ConfigureDI0))]
    [XmlInclude(typeof(ConfigureDI1))]
    [XmlInclude(typeof(ConfigureDI2))]
    [XmlInclude(typeof(SoundIndexDI0))]
    [XmlInclude(typeof(SoundIndexDI1))]
    [XmlInclude(typeof(SoundIndexDI2))]
    [XmlInclude(typeof(FrequencyDI0))]
    [XmlInclude(typeof(FrequencyDI1))]
    [XmlInclude(typeof(FrequencyDI2))]
    [XmlInclude(typeof(AttenuationLeftDI0))]
    [XmlInclude(typeof(AttenuationLeftDI1))]
    [XmlInclude(typeof(AttenuationLeftDI2))]
    [XmlInclude(typeof(AttenuationRightDI0))]
    [XmlInclude(typeof(AttenuationRightDI1))]
    [XmlInclude(typeof(AttenuationRightDI2))]
    [XmlInclude(typeof(AttenuationAndSoundIndexDI0))]
    [XmlInclude(typeof(AttenuationAndSoundIndexDI1))]
    [XmlInclude(typeof(AttenuationAndSoundIndexDI2))]
    [XmlInclude(typeof(AttenuationAndFrequencyDI0))]
    [XmlInclude(typeof(AttenuationAndFrequencyDI1))]
    [XmlInclude(typeof(AttenuationAndFrequencyDI2))]
    [XmlInclude(typeof(ConfigureDO0))]
    [XmlInclude(typeof(ConfigureDO1))]
    [XmlInclude(typeof(ConfigureDO2))]
    [XmlInclude(typeof(PulseDO0))]
    [XmlInclude(typeof(PulseDO1))]
    [XmlInclude(typeof(PulseDO2))]
    [XmlInclude(typeof(OutputSet))]
    [XmlInclude(typeof(OutputClear))]
    [XmlInclude(typeof(OutputToggle))]
    [XmlInclude(typeof(OutputState))]
    [XmlInclude(typeof(ConfigureAdc))]
    [XmlInclude(typeof(AnalogInputs))]
    [XmlInclude(typeof(Commands))]
    [XmlInclude(typeof(EnableEvents))]
    [Description("Filters register-specific messages reported by the SoundCard device.")]
    public class FilterMessage : FilterMessageBuilder, INamedElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterMessage"/> class.
        /// </summary>
        public FilterMessage()
        {
            Register = new PlaySoundOrFrequency();
        }

        string INamedElement.Name
        {
            get => $"{nameof(SoundCard)}.{GetElementDisplayName(Register)}";
        }
    }

    /// <summary>
    /// Represents an operator which filters and selects specific messages
    /// reported by the SoundCard device.
    /// </summary>
    /// <seealso cref="PlaySoundOrFrequency"/>
    /// <seealso cref="Stop"/>
    /// <seealso cref="AttenuationLeft"/>
    /// <seealso cref="AttenuationRight"/>
    /// <seealso cref="AttenuationBoth"/>
    /// <seealso cref="AttenuationAndPlaySoundOrFreq"/>
    /// <seealso cref="DigitalInputs"/>
    /// <seealso cref="ConfigureDI0"/>
    /// <seealso cref="ConfigureDI1"/>
    /// <seealso cref="ConfigureDI2"/>
    /// <seealso cref="SoundIndexDI0"/>
    /// <seealso cref="SoundIndexDI1"/>
    /// <seealso cref="SoundIndexDI2"/>
    /// <seealso cref="FrequencyDI0"/>
    /// <seealso cref="FrequencyDI1"/>
    /// <seealso cref="FrequencyDI2"/>
    /// <seealso cref="AttenuationLeftDI0"/>
    /// <seealso cref="AttenuationLeftDI1"/>
    /// <seealso cref="AttenuationLeftDI2"/>
    /// <seealso cref="AttenuationRightDI0"/>
    /// <seealso cref="AttenuationRightDI1"/>
    /// <seealso cref="AttenuationRightDI2"/>
    /// <seealso cref="AttenuationAndSoundIndexDI0"/>
    /// <seealso cref="AttenuationAndSoundIndexDI1"/>
    /// <seealso cref="AttenuationAndSoundIndexDI2"/>
    /// <seealso cref="AttenuationAndFrequencyDI0"/>
    /// <seealso cref="AttenuationAndFrequencyDI1"/>
    /// <seealso cref="AttenuationAndFrequencyDI2"/>
    /// <seealso cref="ConfigureDO0"/>
    /// <seealso cref="ConfigureDO1"/>
    /// <seealso cref="ConfigureDO2"/>
    /// <seealso cref="PulseDO0"/>
    /// <seealso cref="PulseDO1"/>
    /// <seealso cref="PulseDO2"/>
    /// <seealso cref="OutputSet"/>
    /// <seealso cref="OutputClear"/>
    /// <seealso cref="OutputToggle"/>
    /// <seealso cref="OutputState"/>
    /// <seealso cref="ConfigureAdc"/>
    /// <seealso cref="AnalogInputs"/>
    /// <seealso cref="Commands"/>
    /// <seealso cref="EnableEvents"/>
    [XmlInclude(typeof(PlaySoundOrFrequency))]
    [XmlInclude(typeof(Stop))]
    [XmlInclude(typeof(AttenuationLeft))]
    [XmlInclude(typeof(AttenuationRight))]
    [XmlInclude(typeof(AttenuationBoth))]
    [XmlInclude(typeof(AttenuationAndPlaySoundOrFreq))]
    [XmlInclude(typeof(DigitalInputs))]
    [XmlInclude(typeof(ConfigureDI0))]
    [XmlInclude(typeof(ConfigureDI1))]
    [XmlInclude(typeof(ConfigureDI2))]
    [XmlInclude(typeof(SoundIndexDI0))]
    [XmlInclude(typeof(SoundIndexDI1))]
    [XmlInclude(typeof(SoundIndexDI2))]
    [XmlInclude(typeof(FrequencyDI0))]
    [XmlInclude(typeof(FrequencyDI1))]
    [XmlInclude(typeof(FrequencyDI2))]
    [XmlInclude(typeof(AttenuationLeftDI0))]
    [XmlInclude(typeof(AttenuationLeftDI1))]
    [XmlInclude(typeof(AttenuationLeftDI2))]
    [XmlInclude(typeof(AttenuationRightDI0))]
    [XmlInclude(typeof(AttenuationRightDI1))]
    [XmlInclude(typeof(AttenuationRightDI2))]
    [XmlInclude(typeof(AttenuationAndSoundIndexDI0))]
    [XmlInclude(typeof(AttenuationAndSoundIndexDI1))]
    [XmlInclude(typeof(AttenuationAndSoundIndexDI2))]
    [XmlInclude(typeof(AttenuationAndFrequencyDI0))]
    [XmlInclude(typeof(AttenuationAndFrequencyDI1))]
    [XmlInclude(typeof(AttenuationAndFrequencyDI2))]
    [XmlInclude(typeof(ConfigureDO0))]
    [XmlInclude(typeof(ConfigureDO1))]
    [XmlInclude(typeof(ConfigureDO2))]
    [XmlInclude(typeof(PulseDO0))]
    [XmlInclude(typeof(PulseDO1))]
    [XmlInclude(typeof(PulseDO2))]
    [XmlInclude(typeof(OutputSet))]
    [XmlInclude(typeof(OutputClear))]
    [XmlInclude(typeof(OutputToggle))]
    [XmlInclude(typeof(OutputState))]
    [XmlInclude(typeof(ConfigureAdc))]
    [XmlInclude(typeof(AnalogInputs))]
    [XmlInclude(typeof(Commands))]
    [XmlInclude(typeof(EnableEvents))]
    [XmlInclude(typeof(TimestampedPlaySoundOrFrequency))]
    [XmlInclude(typeof(TimestampedStop))]
    [XmlInclude(typeof(TimestampedAttenuationLeft))]
    [XmlInclude(typeof(TimestampedAttenuationRight))]
    [XmlInclude(typeof(TimestampedAttenuationBoth))]
    [XmlInclude(typeof(TimestampedAttenuationAndPlaySoundOrFreq))]
    [XmlInclude(typeof(TimestampedDigitalInputs))]
    [XmlInclude(typeof(TimestampedConfigureDI0))]
    [XmlInclude(typeof(TimestampedConfigureDI1))]
    [XmlInclude(typeof(TimestampedConfigureDI2))]
    [XmlInclude(typeof(TimestampedSoundIndexDI0))]
    [XmlInclude(typeof(TimestampedSoundIndexDI1))]
    [XmlInclude(typeof(TimestampedSoundIndexDI2))]
    [XmlInclude(typeof(TimestampedFrequencyDI0))]
    [XmlInclude(typeof(TimestampedFrequencyDI1))]
    [XmlInclude(typeof(TimestampedFrequencyDI2))]
    [XmlInclude(typeof(TimestampedAttenuationLeftDI0))]
    [XmlInclude(typeof(TimestampedAttenuationLeftDI1))]
    [XmlInclude(typeof(TimestampedAttenuationLeftDI2))]
    [XmlInclude(typeof(TimestampedAttenuationRightDI0))]
    [XmlInclude(typeof(TimestampedAttenuationRightDI1))]
    [XmlInclude(typeof(TimestampedAttenuationRightDI2))]
    [XmlInclude(typeof(TimestampedAttenuationAndSoundIndexDI0))]
    [XmlInclude(typeof(TimestampedAttenuationAndSoundIndexDI1))]
    [XmlInclude(typeof(TimestampedAttenuationAndSoundIndexDI2))]
    [XmlInclude(typeof(TimestampedAttenuationAndFrequencyDI0))]
    [XmlInclude(typeof(TimestampedAttenuationAndFrequencyDI1))]
    [XmlInclude(typeof(TimestampedAttenuationAndFrequencyDI2))]
    [XmlInclude(typeof(TimestampedConfigureDO0))]
    [XmlInclude(typeof(TimestampedConfigureDO1))]
    [XmlInclude(typeof(TimestampedConfigureDO2))]
    [XmlInclude(typeof(TimestampedPulseDO0))]
    [XmlInclude(typeof(TimestampedPulseDO1))]
    [XmlInclude(typeof(TimestampedPulseDO2))]
    [XmlInclude(typeof(TimestampedOutputSet))]
    [XmlInclude(typeof(TimestampedOutputClear))]
    [XmlInclude(typeof(TimestampedOutputToggle))]
    [XmlInclude(typeof(TimestampedOutputState))]
    [XmlInclude(typeof(TimestampedConfigureAdc))]
    [XmlInclude(typeof(TimestampedAnalogInputs))]
    [XmlInclude(typeof(TimestampedCommands))]
    [XmlInclude(typeof(TimestampedEnableEvents))]
    [Description("Filters and selects specific messages reported by the SoundCard device.")]
    public partial class Parse : ParseBuilder, INamedElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Parse"/> class.
        /// </summary>
        public Parse()
        {
            Register = new PlaySoundOrFrequency();
        }

        string INamedElement.Name => $"{nameof(SoundCard)}.{GetElementDisplayName(Register)}";
    }

    /// <summary>
    /// Represents an operator which formats a sequence of values as specific
    /// SoundCard register messages.
    /// </summary>
    /// <seealso cref="PlaySoundOrFrequency"/>
    /// <seealso cref="Stop"/>
    /// <seealso cref="AttenuationLeft"/>
    /// <seealso cref="AttenuationRight"/>
    /// <seealso cref="AttenuationBoth"/>
    /// <seealso cref="AttenuationAndPlaySoundOrFreq"/>
    /// <seealso cref="DigitalInputs"/>
    /// <seealso cref="ConfigureDI0"/>
    /// <seealso cref="ConfigureDI1"/>
    /// <seealso cref="ConfigureDI2"/>
    /// <seealso cref="SoundIndexDI0"/>
    /// <seealso cref="SoundIndexDI1"/>
    /// <seealso cref="SoundIndexDI2"/>
    /// <seealso cref="FrequencyDI0"/>
    /// <seealso cref="FrequencyDI1"/>
    /// <seealso cref="FrequencyDI2"/>
    /// <seealso cref="AttenuationLeftDI0"/>
    /// <seealso cref="AttenuationLeftDI1"/>
    /// <seealso cref="AttenuationLeftDI2"/>
    /// <seealso cref="AttenuationRightDI0"/>
    /// <seealso cref="AttenuationRightDI1"/>
    /// <seealso cref="AttenuationRightDI2"/>
    /// <seealso cref="AttenuationAndSoundIndexDI0"/>
    /// <seealso cref="AttenuationAndSoundIndexDI1"/>
    /// <seealso cref="AttenuationAndSoundIndexDI2"/>
    /// <seealso cref="AttenuationAndFrequencyDI0"/>
    /// <seealso cref="AttenuationAndFrequencyDI1"/>
    /// <seealso cref="AttenuationAndFrequencyDI2"/>
    /// <seealso cref="ConfigureDO0"/>
    /// <seealso cref="ConfigureDO1"/>
    /// <seealso cref="ConfigureDO2"/>
    /// <seealso cref="PulseDO0"/>
    /// <seealso cref="PulseDO1"/>
    /// <seealso cref="PulseDO2"/>
    /// <seealso cref="OutputSet"/>
    /// <seealso cref="OutputClear"/>
    /// <seealso cref="OutputToggle"/>
    /// <seealso cref="OutputState"/>
    /// <seealso cref="ConfigureAdc"/>
    /// <seealso cref="AnalogInputs"/>
    /// <seealso cref="Commands"/>
    /// <seealso cref="EnableEvents"/>
    [XmlInclude(typeof(PlaySoundOrFrequency))]
    [XmlInclude(typeof(Stop))]
    [XmlInclude(typeof(AttenuationLeft))]
    [XmlInclude(typeof(AttenuationRight))]
    [XmlInclude(typeof(AttenuationBoth))]
    [XmlInclude(typeof(AttenuationAndPlaySoundOrFreq))]
    [XmlInclude(typeof(DigitalInputs))]
    [XmlInclude(typeof(ConfigureDI0))]
    [XmlInclude(typeof(ConfigureDI1))]
    [XmlInclude(typeof(ConfigureDI2))]
    [XmlInclude(typeof(SoundIndexDI0))]
    [XmlInclude(typeof(SoundIndexDI1))]
    [XmlInclude(typeof(SoundIndexDI2))]
    [XmlInclude(typeof(FrequencyDI0))]
    [XmlInclude(typeof(FrequencyDI1))]
    [XmlInclude(typeof(FrequencyDI2))]
    [XmlInclude(typeof(AttenuationLeftDI0))]
    [XmlInclude(typeof(AttenuationLeftDI1))]
    [XmlInclude(typeof(AttenuationLeftDI2))]
    [XmlInclude(typeof(AttenuationRightDI0))]
    [XmlInclude(typeof(AttenuationRightDI1))]
    [XmlInclude(typeof(AttenuationRightDI2))]
    [XmlInclude(typeof(AttenuationAndSoundIndexDI0))]
    [XmlInclude(typeof(AttenuationAndSoundIndexDI1))]
    [XmlInclude(typeof(AttenuationAndSoundIndexDI2))]
    [XmlInclude(typeof(AttenuationAndFrequencyDI0))]
    [XmlInclude(typeof(AttenuationAndFrequencyDI1))]
    [XmlInclude(typeof(AttenuationAndFrequencyDI2))]
    [XmlInclude(typeof(ConfigureDO0))]
    [XmlInclude(typeof(ConfigureDO1))]
    [XmlInclude(typeof(ConfigureDO2))]
    [XmlInclude(typeof(PulseDO0))]
    [XmlInclude(typeof(PulseDO1))]
    [XmlInclude(typeof(PulseDO2))]
    [XmlInclude(typeof(OutputSet))]
    [XmlInclude(typeof(OutputClear))]
    [XmlInclude(typeof(OutputToggle))]
    [XmlInclude(typeof(OutputState))]
    [XmlInclude(typeof(ConfigureAdc))]
    [XmlInclude(typeof(AnalogInputs))]
    [XmlInclude(typeof(Commands))]
    [XmlInclude(typeof(EnableEvents))]
    [Description("Formats a sequence of values as specific SoundCard register messages.")]
    public partial class Format : FormatBuilder, INamedElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Format"/> class.
        /// </summary>
        public Format()
        {
            Register = new PlaySoundOrFrequency();
        }

        string INamedElement.Name => $"{nameof(SoundCard)}.{GetElementDisplayName(Register)}";
    }

    /// <summary>
    /// Represents a register that starts the sound index (if < 32) or frequency (if >= 32).
    /// </summary>
    [Description("Starts the sound index (if < 32) or frequency (if >= 32)")]
    public partial class PlaySoundOrFrequency
    {
        /// <summary>
        /// Represents the address of the <see cref="PlaySoundOrFrequency"/> register. This field is constant.
        /// </summary>
        public const int Address = 32;

        /// <summary>
        /// Represents the payload type of the <see cref="PlaySoundOrFrequency"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="PlaySoundOrFrequency"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="PlaySoundOrFrequency"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort GetPayload(HarpMessage message)
        {
            return message.GetPayloadUInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="PlaySoundOrFrequency"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadUInt16();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="PlaySoundOrFrequency"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="PlaySoundOrFrequency"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="PlaySoundOrFrequency"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="PlaySoundOrFrequency"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// PlaySoundOrFrequency register.
    /// </summary>
    /// <seealso cref="PlaySoundOrFrequency"/>
    [Description("Filters and selects timestamped messages from the PlaySoundOrFrequency register.")]
    public partial class TimestampedPlaySoundOrFrequency
    {
        /// <summary>
        /// Represents the address of the <see cref="PlaySoundOrFrequency"/> register. This field is constant.
        /// </summary>
        public const int Address = PlaySoundOrFrequency.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="PlaySoundOrFrequency"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetPayload(HarpMessage message)
        {
            return PlaySoundOrFrequency.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that any value will stop the current sound.
    /// </summary>
    [Description("Any value will stop the current sound")]
    public partial class Stop
    {
        /// <summary>
        /// Represents the address of the <see cref="Stop"/> register. This field is constant.
        /// </summary>
        public const int Address = 33;

        /// <summary>
        /// Represents the payload type of the <see cref="Stop"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="Stop"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Stop"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static byte GetPayload(HarpMessage message)
        {
            return message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Stop"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadByte();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Stop"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Stop"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Stop"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Stop"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Stop register.
    /// </summary>
    /// <seealso cref="Stop"/>
    [Description("Filters and selects timestamped messages from the Stop register.")]
    public partial class TimestampedStop
    {
        /// <summary>
        /// Represents the address of the <see cref="Stop"/> register. This field is constant.
        /// </summary>
        public const int Address = Stop.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Stop"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetPayload(HarpMessage message)
        {
            return Stop.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that configure left channel's attenuation (1 LSB is 0.1dB).
    /// </summary>
    [Description("Configure left channel's attenuation (1 LSB is 0.1dB)")]
    public partial class AttenuationLeft
    {
        /// <summary>
        /// Represents the address of the <see cref="AttenuationLeft"/> register. This field is constant.
        /// </summary>
        public const int Address = 34;

        /// <summary>
        /// Represents the payload type of the <see cref="AttenuationLeft"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="AttenuationLeft"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="AttenuationLeft"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort GetPayload(HarpMessage message)
        {
            return message.GetPayloadUInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="AttenuationLeft"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadUInt16();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="AttenuationLeft"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="AttenuationLeft"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="AttenuationLeft"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="AttenuationLeft"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// AttenuationLeft register.
    /// </summary>
    /// <seealso cref="AttenuationLeft"/>
    [Description("Filters and selects timestamped messages from the AttenuationLeft register.")]
    public partial class TimestampedAttenuationLeft
    {
        /// <summary>
        /// Represents the address of the <see cref="AttenuationLeft"/> register. This field is constant.
        /// </summary>
        public const int Address = AttenuationLeft.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="AttenuationLeft"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetPayload(HarpMessage message)
        {
            return AttenuationLeft.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that configure right channel's attenuation (1 LSB is 0.1dB).
    /// </summary>
    [Description("Configure right channel's attenuation (1 LSB is 0.1dB)")]
    public partial class AttenuationRight
    {
        /// <summary>
        /// Represents the address of the <see cref="AttenuationRight"/> register. This field is constant.
        /// </summary>
        public const int Address = 35;

        /// <summary>
        /// Represents the payload type of the <see cref="AttenuationRight"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="AttenuationRight"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="AttenuationRight"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort GetPayload(HarpMessage message)
        {
            return message.GetPayloadUInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="AttenuationRight"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadUInt16();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="AttenuationRight"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="AttenuationRight"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="AttenuationRight"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="AttenuationRight"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// AttenuationRight register.
    /// </summary>
    /// <seealso cref="AttenuationRight"/>
    [Description("Filters and selects timestamped messages from the AttenuationRight register.")]
    public partial class TimestampedAttenuationRight
    {
        /// <summary>
        /// Represents the address of the <see cref="AttenuationRight"/> register. This field is constant.
        /// </summary>
        public const int Address = AttenuationRight.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="AttenuationRight"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetPayload(HarpMessage message)
        {
            return AttenuationRight.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that configures both attenuation on right and left channels [Att R] [Att L].
    /// </summary>
    [Description("Configures both attenuation on right and left channels [Att R] [Att L]")]
    public partial class AttenuationBoth
    {
        /// <summary>
        /// Represents the address of the <see cref="AttenuationBoth"/> register. This field is constant.
        /// </summary>
        public const int Address = 36;

        /// <summary>
        /// Represents the payload type of the <see cref="AttenuationBoth"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="AttenuationBoth"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 2;

        /// <summary>
        /// Returns the payload data for <see cref="AttenuationBoth"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort[] GetPayload(HarpMessage message)
        {
            return message.GetPayloadArray<ushort>();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="AttenuationBoth"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort[]> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadArray<ushort>();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="AttenuationBoth"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="AttenuationBoth"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort[] value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="AttenuationBoth"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="AttenuationBoth"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort[] value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// AttenuationBoth register.
    /// </summary>
    /// <seealso cref="AttenuationBoth"/>
    [Description("Filters and selects timestamped messages from the AttenuationBoth register.")]
    public partial class TimestampedAttenuationBoth
    {
        /// <summary>
        /// Represents the address of the <see cref="AttenuationBoth"/> register. This field is constant.
        /// </summary>
        public const int Address = AttenuationBoth.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="AttenuationBoth"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort[]> GetPayload(HarpMessage message)
        {
            return AttenuationBoth.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that configures attenuation and plays sound index [Att R] [Att L] [Index].
    /// </summary>
    [Description("Configures attenuation and plays sound index [Att R] [Att L] [Index]")]
    public partial class AttenuationAndPlaySoundOrFreq
    {
        /// <summary>
        /// Represents the address of the <see cref="AttenuationAndPlaySoundOrFreq"/> register. This field is constant.
        /// </summary>
        public const int Address = 37;

        /// <summary>
        /// Represents the payload type of the <see cref="AttenuationAndPlaySoundOrFreq"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="AttenuationAndPlaySoundOrFreq"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 3;

        /// <summary>
        /// Returns the payload data for <see cref="AttenuationAndPlaySoundOrFreq"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort[] GetPayload(HarpMessage message)
        {
            return message.GetPayloadArray<ushort>();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="AttenuationAndPlaySoundOrFreq"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort[]> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadArray<ushort>();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="AttenuationAndPlaySoundOrFreq"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="AttenuationAndPlaySoundOrFreq"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort[] value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="AttenuationAndPlaySoundOrFreq"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="AttenuationAndPlaySoundOrFreq"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort[] value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// AttenuationAndPlaySoundOrFreq register.
    /// </summary>
    /// <seealso cref="AttenuationAndPlaySoundOrFreq"/>
    [Description("Filters and selects timestamped messages from the AttenuationAndPlaySoundOrFreq register.")]
    public partial class TimestampedAttenuationAndPlaySoundOrFreq
    {
        /// <summary>
        /// Represents the address of the <see cref="AttenuationAndPlaySoundOrFreq"/> register. This field is constant.
        /// </summary>
        public const int Address = AttenuationAndPlaySoundOrFreq.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="AttenuationAndPlaySoundOrFreq"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort[]> GetPayload(HarpMessage message)
        {
            return AttenuationAndPlaySoundOrFreq.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that state of the digital inputs.
    /// </summary>
    [Description("State of the digital inputs")]
    public partial class DigitalInputs
    {
        /// <summary>
        /// Represents the address of the <see cref="DigitalInputs"/> register. This field is constant.
        /// </summary>
        public const int Address = 40;

        /// <summary>
        /// Represents the payload type of the <see cref="DigitalInputs"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="DigitalInputs"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="DigitalInputs"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static byte GetPayload(HarpMessage message)
        {
            return message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="DigitalInputs"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadByte();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="DigitalInputs"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="DigitalInputs"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="DigitalInputs"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="DigitalInputs"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// DigitalInputs register.
    /// </summary>
    /// <seealso cref="DigitalInputs"/>
    [Description("Filters and selects timestamped messages from the DigitalInputs register.")]
    public partial class TimestampedDigitalInputs
    {
        /// <summary>
        /// Represents the address of the <see cref="DigitalInputs"/> register. This field is constant.
        /// </summary>
        public const int Address = DigitalInputs.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="DigitalInputs"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetPayload(HarpMessage message)
        {
            return DigitalInputs.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that configuration of the digital input 0 (DI0).
    /// </summary>
    [Description("Configuration of the digital input 0 (DI0)")]
    public partial class ConfigureDI0
    {
        /// <summary>
        /// Represents the address of the <see cref="ConfigureDI0"/> register. This field is constant.
        /// </summary>
        public const int Address = 41;

        /// <summary>
        /// Represents the payload type of the <see cref="ConfigureDI0"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="ConfigureDI0"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="ConfigureDI0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static byte GetPayload(HarpMessage message)
        {
            return message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="ConfigureDI0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadByte();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="ConfigureDI0"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="ConfigureDI0"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="ConfigureDI0"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="ConfigureDI0"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// ConfigureDI0 register.
    /// </summary>
    /// <seealso cref="ConfigureDI0"/>
    [Description("Filters and selects timestamped messages from the ConfigureDI0 register.")]
    public partial class TimestampedConfigureDI0
    {
        /// <summary>
        /// Represents the address of the <see cref="ConfigureDI0"/> register. This field is constant.
        /// </summary>
        public const int Address = ConfigureDI0.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="ConfigureDI0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetPayload(HarpMessage message)
        {
            return ConfigureDI0.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that configuration of the digital input 1 (DI1).
    /// </summary>
    [Description("Configuration of the digital input 1 (DI1)")]
    public partial class ConfigureDI1
    {
        /// <summary>
        /// Represents the address of the <see cref="ConfigureDI1"/> register. This field is constant.
        /// </summary>
        public const int Address = 42;

        /// <summary>
        /// Represents the payload type of the <see cref="ConfigureDI1"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="ConfigureDI1"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="ConfigureDI1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static byte GetPayload(HarpMessage message)
        {
            return message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="ConfigureDI1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadByte();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="ConfigureDI1"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="ConfigureDI1"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="ConfigureDI1"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="ConfigureDI1"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// ConfigureDI1 register.
    /// </summary>
    /// <seealso cref="ConfigureDI1"/>
    [Description("Filters and selects timestamped messages from the ConfigureDI1 register.")]
    public partial class TimestampedConfigureDI1
    {
        /// <summary>
        /// Represents the address of the <see cref="ConfigureDI1"/> register. This field is constant.
        /// </summary>
        public const int Address = ConfigureDI1.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="ConfigureDI1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetPayload(HarpMessage message)
        {
            return ConfigureDI1.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that configuration of the digital input 2 (DI2).
    /// </summary>
    [Description("Configuration of the digital input 2 (DI2)")]
    public partial class ConfigureDI2
    {
        /// <summary>
        /// Represents the address of the <see cref="ConfigureDI2"/> register. This field is constant.
        /// </summary>
        public const int Address = 43;

        /// <summary>
        /// Represents the payload type of the <see cref="ConfigureDI2"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="ConfigureDI2"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="ConfigureDI2"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static byte GetPayload(HarpMessage message)
        {
            return message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="ConfigureDI2"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadByte();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="ConfigureDI2"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="ConfigureDI2"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="ConfigureDI2"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="ConfigureDI2"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// ConfigureDI2 register.
    /// </summary>
    /// <seealso cref="ConfigureDI2"/>
    [Description("Filters and selects timestamped messages from the ConfigureDI2 register.")]
    public partial class TimestampedConfigureDI2
    {
        /// <summary>
        /// Represents the address of the <see cref="ConfigureDI2"/> register. This field is constant.
        /// </summary>
        public const int Address = ConfigureDI2.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="ConfigureDI2"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetPayload(HarpMessage message)
        {
            return ConfigureDI2.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that specifies the sound index to be played when triggering DI0.
    /// </summary>
    [Description("Specifies the sound index to be played when triggering DI0")]
    public partial class SoundIndexDI0
    {
        /// <summary>
        /// Represents the address of the <see cref="SoundIndexDI0"/> register. This field is constant.
        /// </summary>
        public const int Address = 44;

        /// <summary>
        /// Represents the payload type of the <see cref="SoundIndexDI0"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="SoundIndexDI0"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="SoundIndexDI0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static byte GetPayload(HarpMessage message)
        {
            return message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="SoundIndexDI0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadByte();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="SoundIndexDI0"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="SoundIndexDI0"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="SoundIndexDI0"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="SoundIndexDI0"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// SoundIndexDI0 register.
    /// </summary>
    /// <seealso cref="SoundIndexDI0"/>
    [Description("Filters and selects timestamped messages from the SoundIndexDI0 register.")]
    public partial class TimestampedSoundIndexDI0
    {
        /// <summary>
        /// Represents the address of the <see cref="SoundIndexDI0"/> register. This field is constant.
        /// </summary>
        public const int Address = SoundIndexDI0.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="SoundIndexDI0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetPayload(HarpMessage message)
        {
            return SoundIndexDI0.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that specifies the sound index to be played when triggering DI1.
    /// </summary>
    [Description("Specifies the sound index to be played when triggering DI1")]
    public partial class SoundIndexDI1
    {
        /// <summary>
        /// Represents the address of the <see cref="SoundIndexDI1"/> register. This field is constant.
        /// </summary>
        public const int Address = 45;

        /// <summary>
        /// Represents the payload type of the <see cref="SoundIndexDI1"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="SoundIndexDI1"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="SoundIndexDI1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static byte GetPayload(HarpMessage message)
        {
            return message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="SoundIndexDI1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadByte();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="SoundIndexDI1"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="SoundIndexDI1"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="SoundIndexDI1"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="SoundIndexDI1"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// SoundIndexDI1 register.
    /// </summary>
    /// <seealso cref="SoundIndexDI1"/>
    [Description("Filters and selects timestamped messages from the SoundIndexDI1 register.")]
    public partial class TimestampedSoundIndexDI1
    {
        /// <summary>
        /// Represents the address of the <see cref="SoundIndexDI1"/> register. This field is constant.
        /// </summary>
        public const int Address = SoundIndexDI1.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="SoundIndexDI1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetPayload(HarpMessage message)
        {
            return SoundIndexDI1.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that specifies the sound index to be played when triggering DI2.
    /// </summary>
    [Description("Specifies the sound index to be played when triggering DI2")]
    public partial class SoundIndexDI2
    {
        /// <summary>
        /// Represents the address of the <see cref="SoundIndexDI2"/> register. This field is constant.
        /// </summary>
        public const int Address = 46;

        /// <summary>
        /// Represents the payload type of the <see cref="SoundIndexDI2"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="SoundIndexDI2"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="SoundIndexDI2"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static byte GetPayload(HarpMessage message)
        {
            return message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="SoundIndexDI2"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadByte();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="SoundIndexDI2"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="SoundIndexDI2"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="SoundIndexDI2"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="SoundIndexDI2"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// SoundIndexDI2 register.
    /// </summary>
    /// <seealso cref="SoundIndexDI2"/>
    [Description("Filters and selects timestamped messages from the SoundIndexDI2 register.")]
    public partial class TimestampedSoundIndexDI2
    {
        /// <summary>
        /// Represents the address of the <see cref="SoundIndexDI2"/> register. This field is constant.
        /// </summary>
        public const int Address = SoundIndexDI2.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="SoundIndexDI2"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetPayload(HarpMessage message)
        {
            return SoundIndexDI2.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that specifies the sound frequency to be played when triggering DI0.
    /// </summary>
    [Description("Specifies the sound frequency to be played when triggering DI0")]
    public partial class FrequencyDI0
    {
        /// <summary>
        /// Represents the address of the <see cref="FrequencyDI0"/> register. This field is constant.
        /// </summary>
        public const int Address = 47;

        /// <summary>
        /// Represents the payload type of the <see cref="FrequencyDI0"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="FrequencyDI0"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="FrequencyDI0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort GetPayload(HarpMessage message)
        {
            return message.GetPayloadUInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="FrequencyDI0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadUInt16();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="FrequencyDI0"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="FrequencyDI0"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="FrequencyDI0"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="FrequencyDI0"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// FrequencyDI0 register.
    /// </summary>
    /// <seealso cref="FrequencyDI0"/>
    [Description("Filters and selects timestamped messages from the FrequencyDI0 register.")]
    public partial class TimestampedFrequencyDI0
    {
        /// <summary>
        /// Represents the address of the <see cref="FrequencyDI0"/> register. This field is constant.
        /// </summary>
        public const int Address = FrequencyDI0.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="FrequencyDI0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetPayload(HarpMessage message)
        {
            return FrequencyDI0.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that specifies the sound frequency to be played when triggering DI1.
    /// </summary>
    [Description("Specifies the sound frequency to be played when triggering DI1")]
    public partial class FrequencyDI1
    {
        /// <summary>
        /// Represents the address of the <see cref="FrequencyDI1"/> register. This field is constant.
        /// </summary>
        public const int Address = 48;

        /// <summary>
        /// Represents the payload type of the <see cref="FrequencyDI1"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="FrequencyDI1"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="FrequencyDI1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort GetPayload(HarpMessage message)
        {
            return message.GetPayloadUInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="FrequencyDI1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadUInt16();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="FrequencyDI1"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="FrequencyDI1"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="FrequencyDI1"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="FrequencyDI1"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// FrequencyDI1 register.
    /// </summary>
    /// <seealso cref="FrequencyDI1"/>
    [Description("Filters and selects timestamped messages from the FrequencyDI1 register.")]
    public partial class TimestampedFrequencyDI1
    {
        /// <summary>
        /// Represents the address of the <see cref="FrequencyDI1"/> register. This field is constant.
        /// </summary>
        public const int Address = FrequencyDI1.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="FrequencyDI1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetPayload(HarpMessage message)
        {
            return FrequencyDI1.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that specifies the sound frequency to be played when triggering DI2.
    /// </summary>
    [Description("Specifies the sound frequency to be played when triggering DI2")]
    public partial class FrequencyDI2
    {
        /// <summary>
        /// Represents the address of the <see cref="FrequencyDI2"/> register. This field is constant.
        /// </summary>
        public const int Address = 49;

        /// <summary>
        /// Represents the payload type of the <see cref="FrequencyDI2"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="FrequencyDI2"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="FrequencyDI2"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort GetPayload(HarpMessage message)
        {
            return message.GetPayloadUInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="FrequencyDI2"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadUInt16();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="FrequencyDI2"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="FrequencyDI2"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="FrequencyDI2"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="FrequencyDI2"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// FrequencyDI2 register.
    /// </summary>
    /// <seealso cref="FrequencyDI2"/>
    [Description("Filters and selects timestamped messages from the FrequencyDI2 register.")]
    public partial class TimestampedFrequencyDI2
    {
        /// <summary>
        /// Represents the address of the <see cref="FrequencyDI2"/> register. This field is constant.
        /// </summary>
        public const int Address = FrequencyDI2.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="FrequencyDI2"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetPayload(HarpMessage message)
        {
            return FrequencyDI2.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that left channel's attenuation (1 LSB is 0.5dB) when triggering DI0.
    /// </summary>
    [Description("Left channel's attenuation (1 LSB is 0.5dB) when triggering DI0")]
    public partial class AttenuationLeftDI0
    {
        /// <summary>
        /// Represents the address of the <see cref="AttenuationLeftDI0"/> register. This field is constant.
        /// </summary>
        public const int Address = 50;

        /// <summary>
        /// Represents the payload type of the <see cref="AttenuationLeftDI0"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="AttenuationLeftDI0"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="AttenuationLeftDI0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort GetPayload(HarpMessage message)
        {
            return message.GetPayloadUInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="AttenuationLeftDI0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadUInt16();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="AttenuationLeftDI0"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="AttenuationLeftDI0"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="AttenuationLeftDI0"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="AttenuationLeftDI0"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// AttenuationLeftDI0 register.
    /// </summary>
    /// <seealso cref="AttenuationLeftDI0"/>
    [Description("Filters and selects timestamped messages from the AttenuationLeftDI0 register.")]
    public partial class TimestampedAttenuationLeftDI0
    {
        /// <summary>
        /// Represents the address of the <see cref="AttenuationLeftDI0"/> register. This field is constant.
        /// </summary>
        public const int Address = AttenuationLeftDI0.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="AttenuationLeftDI0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetPayload(HarpMessage message)
        {
            return AttenuationLeftDI0.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that left channel's attenuation (1 LSB is 0.5dB) when triggering DI1.
    /// </summary>
    [Description("Left channel's attenuation (1 LSB is 0.5dB) when triggering DI1")]
    public partial class AttenuationLeftDI1
    {
        /// <summary>
        /// Represents the address of the <see cref="AttenuationLeftDI1"/> register. This field is constant.
        /// </summary>
        public const int Address = 51;

        /// <summary>
        /// Represents the payload type of the <see cref="AttenuationLeftDI1"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="AttenuationLeftDI1"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="AttenuationLeftDI1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort GetPayload(HarpMessage message)
        {
            return message.GetPayloadUInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="AttenuationLeftDI1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadUInt16();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="AttenuationLeftDI1"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="AttenuationLeftDI1"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="AttenuationLeftDI1"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="AttenuationLeftDI1"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// AttenuationLeftDI1 register.
    /// </summary>
    /// <seealso cref="AttenuationLeftDI1"/>
    [Description("Filters and selects timestamped messages from the AttenuationLeftDI1 register.")]
    public partial class TimestampedAttenuationLeftDI1
    {
        /// <summary>
        /// Represents the address of the <see cref="AttenuationLeftDI1"/> register. This field is constant.
        /// </summary>
        public const int Address = AttenuationLeftDI1.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="AttenuationLeftDI1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetPayload(HarpMessage message)
        {
            return AttenuationLeftDI1.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that left channel's attenuation (1 LSB is 0.5dB) when triggering DI2.
    /// </summary>
    [Description("Left channel's attenuation (1 LSB is 0.5dB) when triggering DI2")]
    public partial class AttenuationLeftDI2
    {
        /// <summary>
        /// Represents the address of the <see cref="AttenuationLeftDI2"/> register. This field is constant.
        /// </summary>
        public const int Address = 52;

        /// <summary>
        /// Represents the payload type of the <see cref="AttenuationLeftDI2"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="AttenuationLeftDI2"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="AttenuationLeftDI2"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort GetPayload(HarpMessage message)
        {
            return message.GetPayloadUInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="AttenuationLeftDI2"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadUInt16();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="AttenuationLeftDI2"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="AttenuationLeftDI2"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="AttenuationLeftDI2"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="AttenuationLeftDI2"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// AttenuationLeftDI2 register.
    /// </summary>
    /// <seealso cref="AttenuationLeftDI2"/>
    [Description("Filters and selects timestamped messages from the AttenuationLeftDI2 register.")]
    public partial class TimestampedAttenuationLeftDI2
    {
        /// <summary>
        /// Represents the address of the <see cref="AttenuationLeftDI2"/> register. This field is constant.
        /// </summary>
        public const int Address = AttenuationLeftDI2.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="AttenuationLeftDI2"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetPayload(HarpMessage message)
        {
            return AttenuationLeftDI2.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that right channel's attenuation (1 LSB is 0.5dB) when triggering DI0.
    /// </summary>
    [Description("Right channel's attenuation (1 LSB is 0.5dB) when triggering DI0")]
    public partial class AttenuationRightDI0
    {
        /// <summary>
        /// Represents the address of the <see cref="AttenuationRightDI0"/> register. This field is constant.
        /// </summary>
        public const int Address = 53;

        /// <summary>
        /// Represents the payload type of the <see cref="AttenuationRightDI0"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="AttenuationRightDI0"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="AttenuationRightDI0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort GetPayload(HarpMessage message)
        {
            return message.GetPayloadUInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="AttenuationRightDI0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadUInt16();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="AttenuationRightDI0"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="AttenuationRightDI0"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="AttenuationRightDI0"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="AttenuationRightDI0"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// AttenuationRightDI0 register.
    /// </summary>
    /// <seealso cref="AttenuationRightDI0"/>
    [Description("Filters and selects timestamped messages from the AttenuationRightDI0 register.")]
    public partial class TimestampedAttenuationRightDI0
    {
        /// <summary>
        /// Represents the address of the <see cref="AttenuationRightDI0"/> register. This field is constant.
        /// </summary>
        public const int Address = AttenuationRightDI0.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="AttenuationRightDI0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetPayload(HarpMessage message)
        {
            return AttenuationRightDI0.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that right channel's attenuation (1 LSB is 0.5dB) when triggering DI1.
    /// </summary>
    [Description("Right channel's attenuation (1 LSB is 0.5dB) when triggering DI1")]
    public partial class AttenuationRightDI1
    {
        /// <summary>
        /// Represents the address of the <see cref="AttenuationRightDI1"/> register. This field is constant.
        /// </summary>
        public const int Address = 54;

        /// <summary>
        /// Represents the payload type of the <see cref="AttenuationRightDI1"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="AttenuationRightDI1"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="AttenuationRightDI1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort GetPayload(HarpMessage message)
        {
            return message.GetPayloadUInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="AttenuationRightDI1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadUInt16();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="AttenuationRightDI1"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="AttenuationRightDI1"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="AttenuationRightDI1"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="AttenuationRightDI1"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// AttenuationRightDI1 register.
    /// </summary>
    /// <seealso cref="AttenuationRightDI1"/>
    [Description("Filters and selects timestamped messages from the AttenuationRightDI1 register.")]
    public partial class TimestampedAttenuationRightDI1
    {
        /// <summary>
        /// Represents the address of the <see cref="AttenuationRightDI1"/> register. This field is constant.
        /// </summary>
        public const int Address = AttenuationRightDI1.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="AttenuationRightDI1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetPayload(HarpMessage message)
        {
            return AttenuationRightDI1.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that right channel's attenuation (1 LSB is 0.5dB) when triggering DI2.
    /// </summary>
    [Description("Right channel's attenuation (1 LSB is 0.5dB) when triggering DI2")]
    public partial class AttenuationRightDI2
    {
        /// <summary>
        /// Represents the address of the <see cref="AttenuationRightDI2"/> register. This field is constant.
        /// </summary>
        public const int Address = 55;

        /// <summary>
        /// Represents the payload type of the <see cref="AttenuationRightDI2"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="AttenuationRightDI2"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="AttenuationRightDI2"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort GetPayload(HarpMessage message)
        {
            return message.GetPayloadUInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="AttenuationRightDI2"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadUInt16();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="AttenuationRightDI2"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="AttenuationRightDI2"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="AttenuationRightDI2"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="AttenuationRightDI2"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// AttenuationRightDI2 register.
    /// </summary>
    /// <seealso cref="AttenuationRightDI2"/>
    [Description("Filters and selects timestamped messages from the AttenuationRightDI2 register.")]
    public partial class TimestampedAttenuationRightDI2
    {
        /// <summary>
        /// Represents the address of the <see cref="AttenuationRightDI2"/> register. This field is constant.
        /// </summary>
        public const int Address = AttenuationRightDI2.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="AttenuationRightDI2"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetPayload(HarpMessage message)
        {
            return AttenuationRightDI2.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that sound index and attenuation to be played when triggering DI0 [Att R] [Att L] [Index].
    /// </summary>
    [Description("Sound index and attenuation to be played when triggering DI0 [Att R] [Att L] [Index]")]
    public partial class AttenuationAndSoundIndexDI0
    {
        /// <summary>
        /// Represents the address of the <see cref="AttenuationAndSoundIndexDI0"/> register. This field is constant.
        /// </summary>
        public const int Address = 56;

        /// <summary>
        /// Represents the payload type of the <see cref="AttenuationAndSoundIndexDI0"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="AttenuationAndSoundIndexDI0"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 3;

        /// <summary>
        /// Returns the payload data for <see cref="AttenuationAndSoundIndexDI0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort[] GetPayload(HarpMessage message)
        {
            return message.GetPayloadArray<ushort>();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="AttenuationAndSoundIndexDI0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort[]> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadArray<ushort>();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="AttenuationAndSoundIndexDI0"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="AttenuationAndSoundIndexDI0"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort[] value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="AttenuationAndSoundIndexDI0"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="AttenuationAndSoundIndexDI0"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort[] value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// AttenuationAndSoundIndexDI0 register.
    /// </summary>
    /// <seealso cref="AttenuationAndSoundIndexDI0"/>
    [Description("Filters and selects timestamped messages from the AttenuationAndSoundIndexDI0 register.")]
    public partial class TimestampedAttenuationAndSoundIndexDI0
    {
        /// <summary>
        /// Represents the address of the <see cref="AttenuationAndSoundIndexDI0"/> register. This field is constant.
        /// </summary>
        public const int Address = AttenuationAndSoundIndexDI0.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="AttenuationAndSoundIndexDI0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort[]> GetPayload(HarpMessage message)
        {
            return AttenuationAndSoundIndexDI0.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that sound index and attenuation to be played when triggering DI1 [Att R] [Att L] [Index].
    /// </summary>
    [Description("Sound index and attenuation to be played when triggering DI1 [Att R] [Att L] [Index]")]
    public partial class AttenuationAndSoundIndexDI1
    {
        /// <summary>
        /// Represents the address of the <see cref="AttenuationAndSoundIndexDI1"/> register. This field is constant.
        /// </summary>
        public const int Address = 57;

        /// <summary>
        /// Represents the payload type of the <see cref="AttenuationAndSoundIndexDI1"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="AttenuationAndSoundIndexDI1"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 3;

        /// <summary>
        /// Returns the payload data for <see cref="AttenuationAndSoundIndexDI1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort[] GetPayload(HarpMessage message)
        {
            return message.GetPayloadArray<ushort>();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="AttenuationAndSoundIndexDI1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort[]> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadArray<ushort>();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="AttenuationAndSoundIndexDI1"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="AttenuationAndSoundIndexDI1"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort[] value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="AttenuationAndSoundIndexDI1"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="AttenuationAndSoundIndexDI1"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort[] value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// AttenuationAndSoundIndexDI1 register.
    /// </summary>
    /// <seealso cref="AttenuationAndSoundIndexDI1"/>
    [Description("Filters and selects timestamped messages from the AttenuationAndSoundIndexDI1 register.")]
    public partial class TimestampedAttenuationAndSoundIndexDI1
    {
        /// <summary>
        /// Represents the address of the <see cref="AttenuationAndSoundIndexDI1"/> register. This field is constant.
        /// </summary>
        public const int Address = AttenuationAndSoundIndexDI1.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="AttenuationAndSoundIndexDI1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort[]> GetPayload(HarpMessage message)
        {
            return AttenuationAndSoundIndexDI1.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that sound index and attenuation to be played when triggering DI2 [Att R] [Att L] [Index].
    /// </summary>
    [Description("Sound index and attenuation to be played when triggering DI2 [Att R] [Att L] [Index]")]
    public partial class AttenuationAndSoundIndexDI2
    {
        /// <summary>
        /// Represents the address of the <see cref="AttenuationAndSoundIndexDI2"/> register. This field is constant.
        /// </summary>
        public const int Address = 58;

        /// <summary>
        /// Represents the payload type of the <see cref="AttenuationAndSoundIndexDI2"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="AttenuationAndSoundIndexDI2"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 3;

        /// <summary>
        /// Returns the payload data for <see cref="AttenuationAndSoundIndexDI2"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort[] GetPayload(HarpMessage message)
        {
            return message.GetPayloadArray<ushort>();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="AttenuationAndSoundIndexDI2"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort[]> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadArray<ushort>();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="AttenuationAndSoundIndexDI2"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="AttenuationAndSoundIndexDI2"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort[] value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="AttenuationAndSoundIndexDI2"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="AttenuationAndSoundIndexDI2"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort[] value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// AttenuationAndSoundIndexDI2 register.
    /// </summary>
    /// <seealso cref="AttenuationAndSoundIndexDI2"/>
    [Description("Filters and selects timestamped messages from the AttenuationAndSoundIndexDI2 register.")]
    public partial class TimestampedAttenuationAndSoundIndexDI2
    {
        /// <summary>
        /// Represents the address of the <see cref="AttenuationAndSoundIndexDI2"/> register. This field is constant.
        /// </summary>
        public const int Address = AttenuationAndSoundIndexDI2.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="AttenuationAndSoundIndexDI2"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort[]> GetPayload(HarpMessage message)
        {
            return AttenuationAndSoundIndexDI2.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that sound index and attenuation to be played when triggering DI0 [Att BOTH] [Frequency].
    /// </summary>
    [Description("Sound index and attenuation to be played when triggering DI0 [Att BOTH] [Frequency]")]
    public partial class AttenuationAndFrequencyDI0
    {
        /// <summary>
        /// Represents the address of the <see cref="AttenuationAndFrequencyDI0"/> register. This field is constant.
        /// </summary>
        public const int Address = 59;

        /// <summary>
        /// Represents the payload type of the <see cref="AttenuationAndFrequencyDI0"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="AttenuationAndFrequencyDI0"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 2;

        /// <summary>
        /// Returns the payload data for <see cref="AttenuationAndFrequencyDI0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort[] GetPayload(HarpMessage message)
        {
            return message.GetPayloadArray<ushort>();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="AttenuationAndFrequencyDI0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort[]> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadArray<ushort>();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="AttenuationAndFrequencyDI0"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="AttenuationAndFrequencyDI0"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort[] value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="AttenuationAndFrequencyDI0"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="AttenuationAndFrequencyDI0"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort[] value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// AttenuationAndFrequencyDI0 register.
    /// </summary>
    /// <seealso cref="AttenuationAndFrequencyDI0"/>
    [Description("Filters and selects timestamped messages from the AttenuationAndFrequencyDI0 register.")]
    public partial class TimestampedAttenuationAndFrequencyDI0
    {
        /// <summary>
        /// Represents the address of the <see cref="AttenuationAndFrequencyDI0"/> register. This field is constant.
        /// </summary>
        public const int Address = AttenuationAndFrequencyDI0.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="AttenuationAndFrequencyDI0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort[]> GetPayload(HarpMessage message)
        {
            return AttenuationAndFrequencyDI0.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that sound index and attenuation to be played when triggering DI1 [Att BOTH] [Frequency].
    /// </summary>
    [Description("Sound index and attenuation to be played when triggering DI1 [Att BOTH] [Frequency]")]
    public partial class AttenuationAndFrequencyDI1
    {
        /// <summary>
        /// Represents the address of the <see cref="AttenuationAndFrequencyDI1"/> register. This field is constant.
        /// </summary>
        public const int Address = 60;

        /// <summary>
        /// Represents the payload type of the <see cref="AttenuationAndFrequencyDI1"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="AttenuationAndFrequencyDI1"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 2;

        /// <summary>
        /// Returns the payload data for <see cref="AttenuationAndFrequencyDI1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort[] GetPayload(HarpMessage message)
        {
            return message.GetPayloadArray<ushort>();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="AttenuationAndFrequencyDI1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort[]> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadArray<ushort>();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="AttenuationAndFrequencyDI1"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="AttenuationAndFrequencyDI1"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort[] value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="AttenuationAndFrequencyDI1"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="AttenuationAndFrequencyDI1"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort[] value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// AttenuationAndFrequencyDI1 register.
    /// </summary>
    /// <seealso cref="AttenuationAndFrequencyDI1"/>
    [Description("Filters and selects timestamped messages from the AttenuationAndFrequencyDI1 register.")]
    public partial class TimestampedAttenuationAndFrequencyDI1
    {
        /// <summary>
        /// Represents the address of the <see cref="AttenuationAndFrequencyDI1"/> register. This field is constant.
        /// </summary>
        public const int Address = AttenuationAndFrequencyDI1.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="AttenuationAndFrequencyDI1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort[]> GetPayload(HarpMessage message)
        {
            return AttenuationAndFrequencyDI1.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that sound index and attenuation to be played when triggering DI2 [Att BOTH] [Frequency].
    /// </summary>
    [Description("Sound index and attenuation to be played when triggering DI2 [Att BOTH] [Frequency]")]
    public partial class AttenuationAndFrequencyDI2
    {
        /// <summary>
        /// Represents the address of the <see cref="AttenuationAndFrequencyDI2"/> register. This field is constant.
        /// </summary>
        public const int Address = 61;

        /// <summary>
        /// Represents the payload type of the <see cref="AttenuationAndFrequencyDI2"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="AttenuationAndFrequencyDI2"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 2;

        /// <summary>
        /// Returns the payload data for <see cref="AttenuationAndFrequencyDI2"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort[] GetPayload(HarpMessage message)
        {
            return message.GetPayloadArray<ushort>();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="AttenuationAndFrequencyDI2"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort[]> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadArray<ushort>();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="AttenuationAndFrequencyDI2"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="AttenuationAndFrequencyDI2"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort[] value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="AttenuationAndFrequencyDI2"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="AttenuationAndFrequencyDI2"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort[] value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// AttenuationAndFrequencyDI2 register.
    /// </summary>
    /// <seealso cref="AttenuationAndFrequencyDI2"/>
    [Description("Filters and selects timestamped messages from the AttenuationAndFrequencyDI2 register.")]
    public partial class TimestampedAttenuationAndFrequencyDI2
    {
        /// <summary>
        /// Represents the address of the <see cref="AttenuationAndFrequencyDI2"/> register. This field is constant.
        /// </summary>
        public const int Address = AttenuationAndFrequencyDI2.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="AttenuationAndFrequencyDI2"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort[]> GetPayload(HarpMessage message)
        {
            return AttenuationAndFrequencyDI2.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that configuration of the digital output 0 (DO0).
    /// </summary>
    [Description("Configuration of the digital output 0 (DO0)")]
    public partial class ConfigureDO0
    {
        /// <summary>
        /// Represents the address of the <see cref="ConfigureDO0"/> register. This field is constant.
        /// </summary>
        public const int Address = 65;

        /// <summary>
        /// Represents the payload type of the <see cref="ConfigureDO0"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="ConfigureDO0"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="ConfigureDO0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static byte GetPayload(HarpMessage message)
        {
            return message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="ConfigureDO0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadByte();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="ConfigureDO0"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="ConfigureDO0"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="ConfigureDO0"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="ConfigureDO0"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// ConfigureDO0 register.
    /// </summary>
    /// <seealso cref="ConfigureDO0"/>
    [Description("Filters and selects timestamped messages from the ConfigureDO0 register.")]
    public partial class TimestampedConfigureDO0
    {
        /// <summary>
        /// Represents the address of the <see cref="ConfigureDO0"/> register. This field is constant.
        /// </summary>
        public const int Address = ConfigureDO0.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="ConfigureDO0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetPayload(HarpMessage message)
        {
            return ConfigureDO0.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that configuration of the digital output 1 (DO1).
    /// </summary>
    [Description("Configuration of the digital output 1 (DO1)")]
    public partial class ConfigureDO1
    {
        /// <summary>
        /// Represents the address of the <see cref="ConfigureDO1"/> register. This field is constant.
        /// </summary>
        public const int Address = 66;

        /// <summary>
        /// Represents the payload type of the <see cref="ConfigureDO1"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="ConfigureDO1"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="ConfigureDO1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static byte GetPayload(HarpMessage message)
        {
            return message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="ConfigureDO1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadByte();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="ConfigureDO1"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="ConfigureDO1"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="ConfigureDO1"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="ConfigureDO1"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// ConfigureDO1 register.
    /// </summary>
    /// <seealso cref="ConfigureDO1"/>
    [Description("Filters and selects timestamped messages from the ConfigureDO1 register.")]
    public partial class TimestampedConfigureDO1
    {
        /// <summary>
        /// Represents the address of the <see cref="ConfigureDO1"/> register. This field is constant.
        /// </summary>
        public const int Address = ConfigureDO1.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="ConfigureDO1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetPayload(HarpMessage message)
        {
            return ConfigureDO1.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that configuration of the digital output 2 (DO2.
    /// </summary>
    [Description("Configuration of the digital output 2 (DO2")]
    public partial class ConfigureDO2
    {
        /// <summary>
        /// Represents the address of the <see cref="ConfigureDO2"/> register. This field is constant.
        /// </summary>
        public const int Address = 67;

        /// <summary>
        /// Represents the payload type of the <see cref="ConfigureDO2"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="ConfigureDO2"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="ConfigureDO2"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static byte GetPayload(HarpMessage message)
        {
            return message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="ConfigureDO2"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadByte();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="ConfigureDO2"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="ConfigureDO2"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="ConfigureDO2"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="ConfigureDO2"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// ConfigureDO2 register.
    /// </summary>
    /// <seealso cref="ConfigureDO2"/>
    [Description("Filters and selects timestamped messages from the ConfigureDO2 register.")]
    public partial class TimestampedConfigureDO2
    {
        /// <summary>
        /// Represents the address of the <see cref="ConfigureDO2"/> register. This field is constant.
        /// </summary>
        public const int Address = ConfigureDO2.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="ConfigureDO2"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetPayload(HarpMessage message)
        {
            return ConfigureDO2.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that pulse for the digital output 0 (DO0) [1:255].
    /// </summary>
    [Description("Pulse for the digital output 0 (DO0) [1:255]")]
    public partial class PulseDO0
    {
        /// <summary>
        /// Represents the address of the <see cref="PulseDO0"/> register. This field is constant.
        /// </summary>
        public const int Address = 68;

        /// <summary>
        /// Represents the payload type of the <see cref="PulseDO0"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="PulseDO0"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="PulseDO0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static byte GetPayload(HarpMessage message)
        {
            return message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="PulseDO0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadByte();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="PulseDO0"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="PulseDO0"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="PulseDO0"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="PulseDO0"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// PulseDO0 register.
    /// </summary>
    /// <seealso cref="PulseDO0"/>
    [Description("Filters and selects timestamped messages from the PulseDO0 register.")]
    public partial class TimestampedPulseDO0
    {
        /// <summary>
        /// Represents the address of the <see cref="PulseDO0"/> register. This field is constant.
        /// </summary>
        public const int Address = PulseDO0.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="PulseDO0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetPayload(HarpMessage message)
        {
            return PulseDO0.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that pulse for the digital output 1 (DO1) [1:255].
    /// </summary>
    [Description("Pulse for the digital output 1 (DO1) [1:255]")]
    public partial class PulseDO1
    {
        /// <summary>
        /// Represents the address of the <see cref="PulseDO1"/> register. This field is constant.
        /// </summary>
        public const int Address = 69;

        /// <summary>
        /// Represents the payload type of the <see cref="PulseDO1"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="PulseDO1"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="PulseDO1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static byte GetPayload(HarpMessage message)
        {
            return message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="PulseDO1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadByte();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="PulseDO1"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="PulseDO1"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="PulseDO1"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="PulseDO1"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// PulseDO1 register.
    /// </summary>
    /// <seealso cref="PulseDO1"/>
    [Description("Filters and selects timestamped messages from the PulseDO1 register.")]
    public partial class TimestampedPulseDO1
    {
        /// <summary>
        /// Represents the address of the <see cref="PulseDO1"/> register. This field is constant.
        /// </summary>
        public const int Address = PulseDO1.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="PulseDO1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetPayload(HarpMessage message)
        {
            return PulseDO1.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that pulse for the digital output 2 (DO2) [1:255].
    /// </summary>
    [Description("Pulse for the digital output 2 (DO2) [1:255]")]
    public partial class PulseDO2
    {
        /// <summary>
        /// Represents the address of the <see cref="PulseDO2"/> register. This field is constant.
        /// </summary>
        public const int Address = 70;

        /// <summary>
        /// Represents the payload type of the <see cref="PulseDO2"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="PulseDO2"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="PulseDO2"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static byte GetPayload(HarpMessage message)
        {
            return message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="PulseDO2"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadByte();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="PulseDO2"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="PulseDO2"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="PulseDO2"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="PulseDO2"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// PulseDO2 register.
    /// </summary>
    /// <seealso cref="PulseDO2"/>
    [Description("Filters and selects timestamped messages from the PulseDO2 register.")]
    public partial class TimestampedPulseDO2
    {
        /// <summary>
        /// Represents the address of the <see cref="PulseDO2"/> register. This field is constant.
        /// </summary>
        public const int Address = PulseDO2.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="PulseDO2"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetPayload(HarpMessage message)
        {
            return PulseDO2.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that set the specified digital output lines.
    /// </summary>
    [Description("Set the specified digital output lines")]
    public partial class OutputSet
    {
        /// <summary>
        /// Represents the address of the <see cref="OutputSet"/> register. This field is constant.
        /// </summary>
        public const int Address = 74;

        /// <summary>
        /// Represents the payload type of the <see cref="OutputSet"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="OutputSet"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="OutputSet"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static byte GetPayload(HarpMessage message)
        {
            return message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="OutputSet"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadByte();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="OutputSet"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="OutputSet"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="OutputSet"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="OutputSet"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// OutputSet register.
    /// </summary>
    /// <seealso cref="OutputSet"/>
    [Description("Filters and selects timestamped messages from the OutputSet register.")]
    public partial class TimestampedOutputSet
    {
        /// <summary>
        /// Represents the address of the <see cref="OutputSet"/> register. This field is constant.
        /// </summary>
        public const int Address = OutputSet.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="OutputSet"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetPayload(HarpMessage message)
        {
            return OutputSet.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that clear the specified digital output lines.
    /// </summary>
    [Description("Clear the specified digital output lines")]
    public partial class OutputClear
    {
        /// <summary>
        /// Represents the address of the <see cref="OutputClear"/> register. This field is constant.
        /// </summary>
        public const int Address = 75;

        /// <summary>
        /// Represents the payload type of the <see cref="OutputClear"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="OutputClear"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="OutputClear"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static byte GetPayload(HarpMessage message)
        {
            return message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="OutputClear"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadByte();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="OutputClear"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="OutputClear"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="OutputClear"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="OutputClear"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// OutputClear register.
    /// </summary>
    /// <seealso cref="OutputClear"/>
    [Description("Filters and selects timestamped messages from the OutputClear register.")]
    public partial class TimestampedOutputClear
    {
        /// <summary>
        /// Represents the address of the <see cref="OutputClear"/> register. This field is constant.
        /// </summary>
        public const int Address = OutputClear.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="OutputClear"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetPayload(HarpMessage message)
        {
            return OutputClear.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that toggle the specified digital output lines.
    /// </summary>
    [Description("Toggle the specified digital output lines")]
    public partial class OutputToggle
    {
        /// <summary>
        /// Represents the address of the <see cref="OutputToggle"/> register. This field is constant.
        /// </summary>
        public const int Address = 76;

        /// <summary>
        /// Represents the payload type of the <see cref="OutputToggle"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="OutputToggle"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="OutputToggle"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static byte GetPayload(HarpMessage message)
        {
            return message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="OutputToggle"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadByte();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="OutputToggle"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="OutputToggle"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="OutputToggle"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="OutputToggle"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// OutputToggle register.
    /// </summary>
    /// <seealso cref="OutputToggle"/>
    [Description("Filters and selects timestamped messages from the OutputToggle register.")]
    public partial class TimestampedOutputToggle
    {
        /// <summary>
        /// Represents the address of the <see cref="OutputToggle"/> register. This field is constant.
        /// </summary>
        public const int Address = OutputToggle.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="OutputToggle"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetPayload(HarpMessage message)
        {
            return OutputToggle.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that write the state of all digital output lines.
    /// </summary>
    [Description("Write the state of all digital output lines")]
    public partial class OutputState
    {
        /// <summary>
        /// Represents the address of the <see cref="OutputState"/> register. This field is constant.
        /// </summary>
        public const int Address = 77;

        /// <summary>
        /// Represents the payload type of the <see cref="OutputState"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="OutputState"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="OutputState"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static byte GetPayload(HarpMessage message)
        {
            return message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="OutputState"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadByte();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="OutputState"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="OutputState"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="OutputState"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="OutputState"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// OutputState register.
    /// </summary>
    /// <seealso cref="OutputState"/>
    [Description("Filters and selects timestamped messages from the OutputState register.")]
    public partial class TimestampedOutputState
    {
        /// <summary>
        /// Represents the address of the <see cref="OutputState"/> register. This field is constant.
        /// </summary>
        public const int Address = OutputState.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="OutputState"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetPayload(HarpMessage message)
        {
            return OutputState.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that configuration of Analog Inputs.
    /// </summary>
    [Description("Configuration of Analog Inputs")]
    public partial class ConfigureAdc
    {
        /// <summary>
        /// Represents the address of the <see cref="ConfigureAdc"/> register. This field is constant.
        /// </summary>
        public const int Address = 80;

        /// <summary>
        /// Represents the payload type of the <see cref="ConfigureAdc"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="ConfigureAdc"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="ConfigureAdc"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static byte GetPayload(HarpMessage message)
        {
            return message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="ConfigureAdc"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadByte();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="ConfigureAdc"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="ConfigureAdc"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="ConfigureAdc"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="ConfigureAdc"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// ConfigureAdc register.
    /// </summary>
    /// <seealso cref="ConfigureAdc"/>
    [Description("Filters and selects timestamped messages from the ConfigureAdc register.")]
    public partial class TimestampedConfigureAdc
    {
        /// <summary>
        /// Represents the address of the <see cref="ConfigureAdc"/> register. This field is constant.
        /// </summary>
        public const int Address = ConfigureAdc.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="ConfigureAdc"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetPayload(HarpMessage message)
        {
            return ConfigureAdc.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that [ADC0]   [ADC1]   [ATT LEFT]   [ATT RIGHT]   [FREQUENCY]   Values are 0 if not used.
    /// </summary>
    [Description("[ADC0]   [ADC1]   [ATT LEFT]   [ATT RIGHT]   [FREQUENCY]   Values are 0 if not used")]
    public partial class AnalogInputs
    {
        /// <summary>
        /// Represents the address of the <see cref="AnalogInputs"/> register. This field is constant.
        /// </summary>
        public const int Address = 81;

        /// <summary>
        /// Represents the payload type of the <see cref="AnalogInputs"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="AnalogInputs"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 5;

        /// <summary>
        /// Returns the payload data for <see cref="AnalogInputs"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort[] GetPayload(HarpMessage message)
        {
            return message.GetPayloadArray<ushort>();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="AnalogInputs"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort[]> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadArray<ushort>();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="AnalogInputs"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="AnalogInputs"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort[] value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="AnalogInputs"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="AnalogInputs"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort[] value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// AnalogInputs register.
    /// </summary>
    /// <seealso cref="AnalogInputs"/>
    [Description("Filters and selects timestamped messages from the AnalogInputs register.")]
    public partial class TimestampedAnalogInputs
    {
        /// <summary>
        /// Represents the address of the <see cref="AnalogInputs"/> register. This field is constant.
        /// </summary>
        public const int Address = AnalogInputs.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="AnalogInputs"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort[]> GetPayload(HarpMessage message)
        {
            return AnalogInputs.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that send commands to PIC32 micro-controller.
    /// </summary>
    [Description("Send commands to PIC32 micro-controller")]
    public partial class Commands
    {
        /// <summary>
        /// Represents the address of the <see cref="Commands"/> register. This field is constant.
        /// </summary>
        public const int Address = 82;

        /// <summary>
        /// Represents the payload type of the <see cref="Commands"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="Commands"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Commands"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static byte GetPayload(HarpMessage message)
        {
            return message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Commands"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadByte();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Commands"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Commands"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Commands"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Commands"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Commands register.
    /// </summary>
    /// <seealso cref="Commands"/>
    [Description("Filters and selects timestamped messages from the Commands register.")]
    public partial class TimestampedCommands
    {
        /// <summary>
        /// Represents the address of the <see cref="Commands"/> register. This field is constant.
        /// </summary>
        public const int Address = Commands.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Commands"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetPayload(HarpMessage message)
        {
            return Commands.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that enable the Events.
    /// </summary>
    [Description("Enable the Events")]
    public partial class EnableEvents
    {
        /// <summary>
        /// Represents the address of the <see cref="EnableEvents"/> register. This field is constant.
        /// </summary>
        public const int Address = 86;

        /// <summary>
        /// Represents the payload type of the <see cref="EnableEvents"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="EnableEvents"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="EnableEvents"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static byte GetPayload(HarpMessage message)
        {
            return message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="EnableEvents"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadByte();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="EnableEvents"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="EnableEvents"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="EnableEvents"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="EnableEvents"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// EnableEvents register.
    /// </summary>
    /// <seealso cref="EnableEvents"/>
    [Description("Filters and selects timestamped messages from the EnableEvents register.")]
    public partial class TimestampedEnableEvents
    {
        /// <summary>
        /// Represents the address of the <see cref="EnableEvents"/> register. This field is constant.
        /// </summary>
        public const int Address = EnableEvents.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="EnableEvents"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetPayload(HarpMessage message)
        {
            return EnableEvents.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents an operator which creates standard message payloads for the
    /// SoundCard device.
    /// </summary>
    /// <seealso cref="CreatePlaySoundOrFrequencyPayload"/>
    /// <seealso cref="CreateStopPayload"/>
    /// <seealso cref="CreateAttenuationLeftPayload"/>
    /// <seealso cref="CreateAttenuationRightPayload"/>
    /// <seealso cref="CreateAttenuationBothPayload"/>
    /// <seealso cref="CreateAttenuationAndPlaySoundOrFreqPayload"/>
    /// <seealso cref="CreateDigitalInputsPayload"/>
    /// <seealso cref="CreateConfigureDI0Payload"/>
    /// <seealso cref="CreateConfigureDI1Payload"/>
    /// <seealso cref="CreateConfigureDI2Payload"/>
    /// <seealso cref="CreateSoundIndexDI0Payload"/>
    /// <seealso cref="CreateSoundIndexDI1Payload"/>
    /// <seealso cref="CreateSoundIndexDI2Payload"/>
    /// <seealso cref="CreateFrequencyDI0Payload"/>
    /// <seealso cref="CreateFrequencyDI1Payload"/>
    /// <seealso cref="CreateFrequencyDI2Payload"/>
    /// <seealso cref="CreateAttenuationLeftDI0Payload"/>
    /// <seealso cref="CreateAttenuationLeftDI1Payload"/>
    /// <seealso cref="CreateAttenuationLeftDI2Payload"/>
    /// <seealso cref="CreateAttenuationRightDI0Payload"/>
    /// <seealso cref="CreateAttenuationRightDI1Payload"/>
    /// <seealso cref="CreateAttenuationRightDI2Payload"/>
    /// <seealso cref="CreateAttenuationAndSoundIndexDI0Payload"/>
    /// <seealso cref="CreateAttenuationAndSoundIndexDI1Payload"/>
    /// <seealso cref="CreateAttenuationAndSoundIndexDI2Payload"/>
    /// <seealso cref="CreateAttenuationAndFrequencyDI0Payload"/>
    /// <seealso cref="CreateAttenuationAndFrequencyDI1Payload"/>
    /// <seealso cref="CreateAttenuationAndFrequencyDI2Payload"/>
    /// <seealso cref="CreateConfigureDO0Payload"/>
    /// <seealso cref="CreateConfigureDO1Payload"/>
    /// <seealso cref="CreateConfigureDO2Payload"/>
    /// <seealso cref="CreatePulseDO0Payload"/>
    /// <seealso cref="CreatePulseDO1Payload"/>
    /// <seealso cref="CreatePulseDO2Payload"/>
    /// <seealso cref="CreateOutputSetPayload"/>
    /// <seealso cref="CreateOutputClearPayload"/>
    /// <seealso cref="CreateOutputTogglePayload"/>
    /// <seealso cref="CreateOutputStatePayload"/>
    /// <seealso cref="CreateConfigureAdcPayload"/>
    /// <seealso cref="CreateAnalogInputsPayload"/>
    /// <seealso cref="CreateCommandsPayload"/>
    /// <seealso cref="CreateEnableEventsPayload"/>
    [XmlInclude(typeof(CreatePlaySoundOrFrequencyPayload))]
    [XmlInclude(typeof(CreateStopPayload))]
    [XmlInclude(typeof(CreateAttenuationLeftPayload))]
    [XmlInclude(typeof(CreateAttenuationRightPayload))]
    [XmlInclude(typeof(CreateAttenuationBothPayload))]
    [XmlInclude(typeof(CreateAttenuationAndPlaySoundOrFreqPayload))]
    [XmlInclude(typeof(CreateDigitalInputsPayload))]
    [XmlInclude(typeof(CreateConfigureDI0Payload))]
    [XmlInclude(typeof(CreateConfigureDI1Payload))]
    [XmlInclude(typeof(CreateConfigureDI2Payload))]
    [XmlInclude(typeof(CreateSoundIndexDI0Payload))]
    [XmlInclude(typeof(CreateSoundIndexDI1Payload))]
    [XmlInclude(typeof(CreateSoundIndexDI2Payload))]
    [XmlInclude(typeof(CreateFrequencyDI0Payload))]
    [XmlInclude(typeof(CreateFrequencyDI1Payload))]
    [XmlInclude(typeof(CreateFrequencyDI2Payload))]
    [XmlInclude(typeof(CreateAttenuationLeftDI0Payload))]
    [XmlInclude(typeof(CreateAttenuationLeftDI1Payload))]
    [XmlInclude(typeof(CreateAttenuationLeftDI2Payload))]
    [XmlInclude(typeof(CreateAttenuationRightDI0Payload))]
    [XmlInclude(typeof(CreateAttenuationRightDI1Payload))]
    [XmlInclude(typeof(CreateAttenuationRightDI2Payload))]
    [XmlInclude(typeof(CreateAttenuationAndSoundIndexDI0Payload))]
    [XmlInclude(typeof(CreateAttenuationAndSoundIndexDI1Payload))]
    [XmlInclude(typeof(CreateAttenuationAndSoundIndexDI2Payload))]
    [XmlInclude(typeof(CreateAttenuationAndFrequencyDI0Payload))]
    [XmlInclude(typeof(CreateAttenuationAndFrequencyDI1Payload))]
    [XmlInclude(typeof(CreateAttenuationAndFrequencyDI2Payload))]
    [XmlInclude(typeof(CreateConfigureDO0Payload))]
    [XmlInclude(typeof(CreateConfigureDO1Payload))]
    [XmlInclude(typeof(CreateConfigureDO2Payload))]
    [XmlInclude(typeof(CreatePulseDO0Payload))]
    [XmlInclude(typeof(CreatePulseDO1Payload))]
    [XmlInclude(typeof(CreatePulseDO2Payload))]
    [XmlInclude(typeof(CreateOutputSetPayload))]
    [XmlInclude(typeof(CreateOutputClearPayload))]
    [XmlInclude(typeof(CreateOutputTogglePayload))]
    [XmlInclude(typeof(CreateOutputStatePayload))]
    [XmlInclude(typeof(CreateConfigureAdcPayload))]
    [XmlInclude(typeof(CreateAnalogInputsPayload))]
    [XmlInclude(typeof(CreateCommandsPayload))]
    [XmlInclude(typeof(CreateEnableEventsPayload))]
    [Description("Creates standard message payloads for the SoundCard device.")]
    public partial class CreateMessage : CreateMessageBuilder, INamedElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateMessage"/> class.
        /// </summary>
        public CreateMessage()
        {
            Payload = new CreatePlaySoundOrFrequencyPayload();
        }

        string INamedElement.Name => $"{nameof(SoundCard)}.{GetElementDisplayName(Payload)}";
    }

    /// <summary>
    /// Represents an operator that creates a sequence of message payloads
    /// that starts the sound index (if < 32) or frequency (if >= 32).
    /// </summary>
    [DisplayName("PlaySoundOrFrequencyPayload")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Creates a sequence of message payloads that starts the sound index (if < 32) or frequency (if >= 32).")]
    public partial class CreatePlaySoundOrFrequencyPayload : HarpCombinator
    {
        /// <summary>
        /// Gets or sets the value that starts the sound index (if < 32) or frequency (if >= 32).
        /// </summary>
        [Description("The value that starts the sound index (if < 32) or frequency (if >= 32).")]
        public ushort Value { get; set; }

        /// <summary>
        /// Creates an observable sequence that contains a single message
        /// that starts the sound index (if < 32) or frequency (if >= 32).
        /// </summary>
        /// <returns>
        /// A sequence containing a single <see cref="HarpMessage"/> object
        /// representing the created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process()
        {
            return Process(Observable.Return(System.Reactive.Unit.Default));
        }

        /// <summary>
        /// Creates an observable sequence of message payloads
        /// that starts the sound index (if < 32) or frequency (if >= 32).
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting message payloads.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing each
        /// created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => PlaySoundOrFrequency.FromPayload(MessageType, Value));
        }
    }

    /// <summary>
    /// Represents an operator that creates a sequence of message payloads
    /// that any value will stop the current sound.
    /// </summary>
    [DisplayName("StopPayload")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Creates a sequence of message payloads that any value will stop the current sound.")]
    public partial class CreateStopPayload : HarpCombinator
    {
        /// <summary>
        /// Gets or sets the value that any value will stop the current sound.
        /// </summary>
        [Description("The value that any value will stop the current sound.")]
        public byte Value { get; set; }

        /// <summary>
        /// Creates an observable sequence that contains a single message
        /// that any value will stop the current sound.
        /// </summary>
        /// <returns>
        /// A sequence containing a single <see cref="HarpMessage"/> object
        /// representing the created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process()
        {
            return Process(Observable.Return(System.Reactive.Unit.Default));
        }

        /// <summary>
        /// Creates an observable sequence of message payloads
        /// that any value will stop the current sound.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting message payloads.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing each
        /// created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => Stop.FromPayload(MessageType, Value));
        }
    }

    /// <summary>
    /// Represents an operator that creates a sequence of message payloads
    /// that configure left channel's attenuation (1 LSB is 0.1dB).
    /// </summary>
    [DisplayName("AttenuationLeftPayload")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Creates a sequence of message payloads that configure left channel's attenuation (1 LSB is 0.1dB).")]
    public partial class CreateAttenuationLeftPayload : HarpCombinator
    {
        /// <summary>
        /// Gets or sets the value that configure left channel's attenuation (1 LSB is 0.1dB).
        /// </summary>
        [Description("The value that configure left channel's attenuation (1 LSB is 0.1dB).")]
        public ushort Value { get; set; }

        /// <summary>
        /// Creates an observable sequence that contains a single message
        /// that configure left channel's attenuation (1 LSB is 0.1dB).
        /// </summary>
        /// <returns>
        /// A sequence containing a single <see cref="HarpMessage"/> object
        /// representing the created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process()
        {
            return Process(Observable.Return(System.Reactive.Unit.Default));
        }

        /// <summary>
        /// Creates an observable sequence of message payloads
        /// that configure left channel's attenuation (1 LSB is 0.1dB).
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting message payloads.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing each
        /// created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => AttenuationLeft.FromPayload(MessageType, Value));
        }
    }

    /// <summary>
    /// Represents an operator that creates a sequence of message payloads
    /// that configure right channel's attenuation (1 LSB is 0.1dB).
    /// </summary>
    [DisplayName("AttenuationRightPayload")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Creates a sequence of message payloads that configure right channel's attenuation (1 LSB is 0.1dB).")]
    public partial class CreateAttenuationRightPayload : HarpCombinator
    {
        /// <summary>
        /// Gets or sets the value that configure right channel's attenuation (1 LSB is 0.1dB).
        /// </summary>
        [Description("The value that configure right channel's attenuation (1 LSB is 0.1dB).")]
        public ushort Value { get; set; }

        /// <summary>
        /// Creates an observable sequence that contains a single message
        /// that configure right channel's attenuation (1 LSB is 0.1dB).
        /// </summary>
        /// <returns>
        /// A sequence containing a single <see cref="HarpMessage"/> object
        /// representing the created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process()
        {
            return Process(Observable.Return(System.Reactive.Unit.Default));
        }

        /// <summary>
        /// Creates an observable sequence of message payloads
        /// that configure right channel's attenuation (1 LSB is 0.1dB).
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting message payloads.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing each
        /// created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => AttenuationRight.FromPayload(MessageType, Value));
        }
    }

    /// <summary>
    /// Represents an operator that creates a sequence of message payloads
    /// that configures both attenuation on right and left channels [Att R] [Att L].
    /// </summary>
    [DisplayName("AttenuationBothPayload")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Creates a sequence of message payloads that configures both attenuation on right and left channels [Att R] [Att L].")]
    public partial class CreateAttenuationBothPayload : HarpCombinator
    {
        /// <summary>
        /// Gets or sets the value that configures both attenuation on right and left channels [Att R] [Att L].
        /// </summary>
        [Description("The value that configures both attenuation on right and left channels [Att R] [Att L].")]
        public ushort[] Value { get; set; }

        /// <summary>
        /// Creates an observable sequence that contains a single message
        /// that configures both attenuation on right and left channels [Att R] [Att L].
        /// </summary>
        /// <returns>
        /// A sequence containing a single <see cref="HarpMessage"/> object
        /// representing the created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process()
        {
            return Process(Observable.Return(System.Reactive.Unit.Default));
        }

        /// <summary>
        /// Creates an observable sequence of message payloads
        /// that configures both attenuation on right and left channels [Att R] [Att L].
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting message payloads.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing each
        /// created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => AttenuationBoth.FromPayload(MessageType, Value));
        }
    }

    /// <summary>
    /// Represents an operator that creates a sequence of message payloads
    /// that configures attenuation and plays sound index [Att R] [Att L] [Index].
    /// </summary>
    [DisplayName("AttenuationAndPlaySoundOrFreqPayload")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Creates a sequence of message payloads that configures attenuation and plays sound index [Att R] [Att L] [Index].")]
    public partial class CreateAttenuationAndPlaySoundOrFreqPayload : HarpCombinator
    {
        /// <summary>
        /// Gets or sets the value that configures attenuation and plays sound index [Att R] [Att L] [Index].
        /// </summary>
        [Description("The value that configures attenuation and plays sound index [Att R] [Att L] [Index].")]
        public ushort[] Value { get; set; }

        /// <summary>
        /// Creates an observable sequence that contains a single message
        /// that configures attenuation and plays sound index [Att R] [Att L] [Index].
        /// </summary>
        /// <returns>
        /// A sequence containing a single <see cref="HarpMessage"/> object
        /// representing the created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process()
        {
            return Process(Observable.Return(System.Reactive.Unit.Default));
        }

        /// <summary>
        /// Creates an observable sequence of message payloads
        /// that configures attenuation and plays sound index [Att R] [Att L] [Index].
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting message payloads.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing each
        /// created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => AttenuationAndPlaySoundOrFreq.FromPayload(MessageType, Value));
        }
    }

    /// <summary>
    /// Represents an operator that creates a sequence of message payloads
    /// that state of the digital inputs.
    /// </summary>
    [DisplayName("DigitalInputsPayload")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Creates a sequence of message payloads that state of the digital inputs.")]
    public partial class CreateDigitalInputsPayload : HarpCombinator
    {
        /// <summary>
        /// Gets or sets the value that state of the digital inputs.
        /// </summary>
        [Description("The value that state of the digital inputs.")]
        public byte Value { get; set; }

        /// <summary>
        /// Creates an observable sequence that contains a single message
        /// that state of the digital inputs.
        /// </summary>
        /// <returns>
        /// A sequence containing a single <see cref="HarpMessage"/> object
        /// representing the created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process()
        {
            return Process(Observable.Return(System.Reactive.Unit.Default));
        }

        /// <summary>
        /// Creates an observable sequence of message payloads
        /// that state of the digital inputs.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting message payloads.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing each
        /// created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => DigitalInputs.FromPayload(MessageType, Value));
        }
    }

    /// <summary>
    /// Represents an operator that creates a sequence of message payloads
    /// that configuration of the digital input 0 (DI0).
    /// </summary>
    [DisplayName("ConfigureDI0Payload")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Creates a sequence of message payloads that configuration of the digital input 0 (DI0).")]
    public partial class CreateConfigureDI0Payload : HarpCombinator
    {
        /// <summary>
        /// Gets or sets the value that configuration of the digital input 0 (DI0).
        /// </summary>
        [Description("The value that configuration of the digital input 0 (DI0).")]
        public byte Value { get; set; }

        /// <summary>
        /// Creates an observable sequence that contains a single message
        /// that configuration of the digital input 0 (DI0).
        /// </summary>
        /// <returns>
        /// A sequence containing a single <see cref="HarpMessage"/> object
        /// representing the created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process()
        {
            return Process(Observable.Return(System.Reactive.Unit.Default));
        }

        /// <summary>
        /// Creates an observable sequence of message payloads
        /// that configuration of the digital input 0 (DI0).
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting message payloads.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing each
        /// created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => ConfigureDI0.FromPayload(MessageType, Value));
        }
    }

    /// <summary>
    /// Represents an operator that creates a sequence of message payloads
    /// that configuration of the digital input 1 (DI1).
    /// </summary>
    [DisplayName("ConfigureDI1Payload")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Creates a sequence of message payloads that configuration of the digital input 1 (DI1).")]
    public partial class CreateConfigureDI1Payload : HarpCombinator
    {
        /// <summary>
        /// Gets or sets the value that configuration of the digital input 1 (DI1).
        /// </summary>
        [Description("The value that configuration of the digital input 1 (DI1).")]
        public byte Value { get; set; }

        /// <summary>
        /// Creates an observable sequence that contains a single message
        /// that configuration of the digital input 1 (DI1).
        /// </summary>
        /// <returns>
        /// A sequence containing a single <see cref="HarpMessage"/> object
        /// representing the created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process()
        {
            return Process(Observable.Return(System.Reactive.Unit.Default));
        }

        /// <summary>
        /// Creates an observable sequence of message payloads
        /// that configuration of the digital input 1 (DI1).
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting message payloads.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing each
        /// created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => ConfigureDI1.FromPayload(MessageType, Value));
        }
    }

    /// <summary>
    /// Represents an operator that creates a sequence of message payloads
    /// that configuration of the digital input 2 (DI2).
    /// </summary>
    [DisplayName("ConfigureDI2Payload")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Creates a sequence of message payloads that configuration of the digital input 2 (DI2).")]
    public partial class CreateConfigureDI2Payload : HarpCombinator
    {
        /// <summary>
        /// Gets or sets the value that configuration of the digital input 2 (DI2).
        /// </summary>
        [Description("The value that configuration of the digital input 2 (DI2).")]
        public byte Value { get; set; }

        /// <summary>
        /// Creates an observable sequence that contains a single message
        /// that configuration of the digital input 2 (DI2).
        /// </summary>
        /// <returns>
        /// A sequence containing a single <see cref="HarpMessage"/> object
        /// representing the created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process()
        {
            return Process(Observable.Return(System.Reactive.Unit.Default));
        }

        /// <summary>
        /// Creates an observable sequence of message payloads
        /// that configuration of the digital input 2 (DI2).
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting message payloads.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing each
        /// created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => ConfigureDI2.FromPayload(MessageType, Value));
        }
    }

    /// <summary>
    /// Represents an operator that creates a sequence of message payloads
    /// that specifies the sound index to be played when triggering DI0.
    /// </summary>
    [DisplayName("SoundIndexDI0Payload")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Creates a sequence of message payloads that specifies the sound index to be played when triggering DI0.")]
    public partial class CreateSoundIndexDI0Payload : HarpCombinator
    {
        /// <summary>
        /// Gets or sets the value that specifies the sound index to be played when triggering DI0.
        /// </summary>
        [Description("The value that specifies the sound index to be played when triggering DI0.")]
        public byte Value { get; set; }

        /// <summary>
        /// Creates an observable sequence that contains a single message
        /// that specifies the sound index to be played when triggering DI0.
        /// </summary>
        /// <returns>
        /// A sequence containing a single <see cref="HarpMessage"/> object
        /// representing the created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process()
        {
            return Process(Observable.Return(System.Reactive.Unit.Default));
        }

        /// <summary>
        /// Creates an observable sequence of message payloads
        /// that specifies the sound index to be played when triggering DI0.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting message payloads.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing each
        /// created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => SoundIndexDI0.FromPayload(MessageType, Value));
        }
    }

    /// <summary>
    /// Represents an operator that creates a sequence of message payloads
    /// that specifies the sound index to be played when triggering DI1.
    /// </summary>
    [DisplayName("SoundIndexDI1Payload")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Creates a sequence of message payloads that specifies the sound index to be played when triggering DI1.")]
    public partial class CreateSoundIndexDI1Payload : HarpCombinator
    {
        /// <summary>
        /// Gets or sets the value that specifies the sound index to be played when triggering DI1.
        /// </summary>
        [Description("The value that specifies the sound index to be played when triggering DI1.")]
        public byte Value { get; set; }

        /// <summary>
        /// Creates an observable sequence that contains a single message
        /// that specifies the sound index to be played when triggering DI1.
        /// </summary>
        /// <returns>
        /// A sequence containing a single <see cref="HarpMessage"/> object
        /// representing the created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process()
        {
            return Process(Observable.Return(System.Reactive.Unit.Default));
        }

        /// <summary>
        /// Creates an observable sequence of message payloads
        /// that specifies the sound index to be played when triggering DI1.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting message payloads.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing each
        /// created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => SoundIndexDI1.FromPayload(MessageType, Value));
        }
    }

    /// <summary>
    /// Represents an operator that creates a sequence of message payloads
    /// that specifies the sound index to be played when triggering DI2.
    /// </summary>
    [DisplayName("SoundIndexDI2Payload")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Creates a sequence of message payloads that specifies the sound index to be played when triggering DI2.")]
    public partial class CreateSoundIndexDI2Payload : HarpCombinator
    {
        /// <summary>
        /// Gets or sets the value that specifies the sound index to be played when triggering DI2.
        /// </summary>
        [Description("The value that specifies the sound index to be played when triggering DI2.")]
        public byte Value { get; set; }

        /// <summary>
        /// Creates an observable sequence that contains a single message
        /// that specifies the sound index to be played when triggering DI2.
        /// </summary>
        /// <returns>
        /// A sequence containing a single <see cref="HarpMessage"/> object
        /// representing the created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process()
        {
            return Process(Observable.Return(System.Reactive.Unit.Default));
        }

        /// <summary>
        /// Creates an observable sequence of message payloads
        /// that specifies the sound index to be played when triggering DI2.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting message payloads.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing each
        /// created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => SoundIndexDI2.FromPayload(MessageType, Value));
        }
    }

    /// <summary>
    /// Represents an operator that creates a sequence of message payloads
    /// that specifies the sound frequency to be played when triggering DI0.
    /// </summary>
    [DisplayName("FrequencyDI0Payload")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Creates a sequence of message payloads that specifies the sound frequency to be played when triggering DI0.")]
    public partial class CreateFrequencyDI0Payload : HarpCombinator
    {
        /// <summary>
        /// Gets or sets the value that specifies the sound frequency to be played when triggering DI0.
        /// </summary>
        [Description("The value that specifies the sound frequency to be played when triggering DI0.")]
        public ushort Value { get; set; }

        /// <summary>
        /// Creates an observable sequence that contains a single message
        /// that specifies the sound frequency to be played when triggering DI0.
        /// </summary>
        /// <returns>
        /// A sequence containing a single <see cref="HarpMessage"/> object
        /// representing the created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process()
        {
            return Process(Observable.Return(System.Reactive.Unit.Default));
        }

        /// <summary>
        /// Creates an observable sequence of message payloads
        /// that specifies the sound frequency to be played when triggering DI0.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting message payloads.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing each
        /// created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => FrequencyDI0.FromPayload(MessageType, Value));
        }
    }

    /// <summary>
    /// Represents an operator that creates a sequence of message payloads
    /// that specifies the sound frequency to be played when triggering DI1.
    /// </summary>
    [DisplayName("FrequencyDI1Payload")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Creates a sequence of message payloads that specifies the sound frequency to be played when triggering DI1.")]
    public partial class CreateFrequencyDI1Payload : HarpCombinator
    {
        /// <summary>
        /// Gets or sets the value that specifies the sound frequency to be played when triggering DI1.
        /// </summary>
        [Description("The value that specifies the sound frequency to be played when triggering DI1.")]
        public ushort Value { get; set; }

        /// <summary>
        /// Creates an observable sequence that contains a single message
        /// that specifies the sound frequency to be played when triggering DI1.
        /// </summary>
        /// <returns>
        /// A sequence containing a single <see cref="HarpMessage"/> object
        /// representing the created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process()
        {
            return Process(Observable.Return(System.Reactive.Unit.Default));
        }

        /// <summary>
        /// Creates an observable sequence of message payloads
        /// that specifies the sound frequency to be played when triggering DI1.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting message payloads.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing each
        /// created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => FrequencyDI1.FromPayload(MessageType, Value));
        }
    }

    /// <summary>
    /// Represents an operator that creates a sequence of message payloads
    /// that specifies the sound frequency to be played when triggering DI2.
    /// </summary>
    [DisplayName("FrequencyDI2Payload")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Creates a sequence of message payloads that specifies the sound frequency to be played when triggering DI2.")]
    public partial class CreateFrequencyDI2Payload : HarpCombinator
    {
        /// <summary>
        /// Gets or sets the value that specifies the sound frequency to be played when triggering DI2.
        /// </summary>
        [Description("The value that specifies the sound frequency to be played when triggering DI2.")]
        public ushort Value { get; set; }

        /// <summary>
        /// Creates an observable sequence that contains a single message
        /// that specifies the sound frequency to be played when triggering DI2.
        /// </summary>
        /// <returns>
        /// A sequence containing a single <see cref="HarpMessage"/> object
        /// representing the created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process()
        {
            return Process(Observable.Return(System.Reactive.Unit.Default));
        }

        /// <summary>
        /// Creates an observable sequence of message payloads
        /// that specifies the sound frequency to be played when triggering DI2.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting message payloads.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing each
        /// created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => FrequencyDI2.FromPayload(MessageType, Value));
        }
    }

    /// <summary>
    /// Represents an operator that creates a sequence of message payloads
    /// that left channel's attenuation (1 LSB is 0.5dB) when triggering DI0.
    /// </summary>
    [DisplayName("AttenuationLeftDI0Payload")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Creates a sequence of message payloads that left channel's attenuation (1 LSB is 0.5dB) when triggering DI0.")]
    public partial class CreateAttenuationLeftDI0Payload : HarpCombinator
    {
        /// <summary>
        /// Gets or sets the value that left channel's attenuation (1 LSB is 0.5dB) when triggering DI0.
        /// </summary>
        [Description("The value that left channel's attenuation (1 LSB is 0.5dB) when triggering DI0.")]
        public ushort Value { get; set; }

        /// <summary>
        /// Creates an observable sequence that contains a single message
        /// that left channel's attenuation (1 LSB is 0.5dB) when triggering DI0.
        /// </summary>
        /// <returns>
        /// A sequence containing a single <see cref="HarpMessage"/> object
        /// representing the created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process()
        {
            return Process(Observable.Return(System.Reactive.Unit.Default));
        }

        /// <summary>
        /// Creates an observable sequence of message payloads
        /// that left channel's attenuation (1 LSB is 0.5dB) when triggering DI0.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting message payloads.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing each
        /// created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => AttenuationLeftDI0.FromPayload(MessageType, Value));
        }
    }

    /// <summary>
    /// Represents an operator that creates a sequence of message payloads
    /// that left channel's attenuation (1 LSB is 0.5dB) when triggering DI1.
    /// </summary>
    [DisplayName("AttenuationLeftDI1Payload")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Creates a sequence of message payloads that left channel's attenuation (1 LSB is 0.5dB) when triggering DI1.")]
    public partial class CreateAttenuationLeftDI1Payload : HarpCombinator
    {
        /// <summary>
        /// Gets or sets the value that left channel's attenuation (1 LSB is 0.5dB) when triggering DI1.
        /// </summary>
        [Description("The value that left channel's attenuation (1 LSB is 0.5dB) when triggering DI1.")]
        public ushort Value { get; set; }

        /// <summary>
        /// Creates an observable sequence that contains a single message
        /// that left channel's attenuation (1 LSB is 0.5dB) when triggering DI1.
        /// </summary>
        /// <returns>
        /// A sequence containing a single <see cref="HarpMessage"/> object
        /// representing the created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process()
        {
            return Process(Observable.Return(System.Reactive.Unit.Default));
        }

        /// <summary>
        /// Creates an observable sequence of message payloads
        /// that left channel's attenuation (1 LSB is 0.5dB) when triggering DI1.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting message payloads.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing each
        /// created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => AttenuationLeftDI1.FromPayload(MessageType, Value));
        }
    }

    /// <summary>
    /// Represents an operator that creates a sequence of message payloads
    /// that left channel's attenuation (1 LSB is 0.5dB) when triggering DI2.
    /// </summary>
    [DisplayName("AttenuationLeftDI2Payload")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Creates a sequence of message payloads that left channel's attenuation (1 LSB is 0.5dB) when triggering DI2.")]
    public partial class CreateAttenuationLeftDI2Payload : HarpCombinator
    {
        /// <summary>
        /// Gets or sets the value that left channel's attenuation (1 LSB is 0.5dB) when triggering DI2.
        /// </summary>
        [Description("The value that left channel's attenuation (1 LSB is 0.5dB) when triggering DI2.")]
        public ushort Value { get; set; }

        /// <summary>
        /// Creates an observable sequence that contains a single message
        /// that left channel's attenuation (1 LSB is 0.5dB) when triggering DI2.
        /// </summary>
        /// <returns>
        /// A sequence containing a single <see cref="HarpMessage"/> object
        /// representing the created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process()
        {
            return Process(Observable.Return(System.Reactive.Unit.Default));
        }

        /// <summary>
        /// Creates an observable sequence of message payloads
        /// that left channel's attenuation (1 LSB is 0.5dB) when triggering DI2.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting message payloads.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing each
        /// created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => AttenuationLeftDI2.FromPayload(MessageType, Value));
        }
    }

    /// <summary>
    /// Represents an operator that creates a sequence of message payloads
    /// that right channel's attenuation (1 LSB is 0.5dB) when triggering DI0.
    /// </summary>
    [DisplayName("AttenuationRightDI0Payload")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Creates a sequence of message payloads that right channel's attenuation (1 LSB is 0.5dB) when triggering DI0.")]
    public partial class CreateAttenuationRightDI0Payload : HarpCombinator
    {
        /// <summary>
        /// Gets or sets the value that right channel's attenuation (1 LSB is 0.5dB) when triggering DI0.
        /// </summary>
        [Description("The value that right channel's attenuation (1 LSB is 0.5dB) when triggering DI0.")]
        public ushort Value { get; set; }

        /// <summary>
        /// Creates an observable sequence that contains a single message
        /// that right channel's attenuation (1 LSB is 0.5dB) when triggering DI0.
        /// </summary>
        /// <returns>
        /// A sequence containing a single <see cref="HarpMessage"/> object
        /// representing the created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process()
        {
            return Process(Observable.Return(System.Reactive.Unit.Default));
        }

        /// <summary>
        /// Creates an observable sequence of message payloads
        /// that right channel's attenuation (1 LSB is 0.5dB) when triggering DI0.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting message payloads.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing each
        /// created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => AttenuationRightDI0.FromPayload(MessageType, Value));
        }
    }

    /// <summary>
    /// Represents an operator that creates a sequence of message payloads
    /// that right channel's attenuation (1 LSB is 0.5dB) when triggering DI1.
    /// </summary>
    [DisplayName("AttenuationRightDI1Payload")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Creates a sequence of message payloads that right channel's attenuation (1 LSB is 0.5dB) when triggering DI1.")]
    public partial class CreateAttenuationRightDI1Payload : HarpCombinator
    {
        /// <summary>
        /// Gets or sets the value that right channel's attenuation (1 LSB is 0.5dB) when triggering DI1.
        /// </summary>
        [Description("The value that right channel's attenuation (1 LSB is 0.5dB) when triggering DI1.")]
        public ushort Value { get; set; }

        /// <summary>
        /// Creates an observable sequence that contains a single message
        /// that right channel's attenuation (1 LSB is 0.5dB) when triggering DI1.
        /// </summary>
        /// <returns>
        /// A sequence containing a single <see cref="HarpMessage"/> object
        /// representing the created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process()
        {
            return Process(Observable.Return(System.Reactive.Unit.Default));
        }

        /// <summary>
        /// Creates an observable sequence of message payloads
        /// that right channel's attenuation (1 LSB is 0.5dB) when triggering DI1.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting message payloads.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing each
        /// created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => AttenuationRightDI1.FromPayload(MessageType, Value));
        }
    }

    /// <summary>
    /// Represents an operator that creates a sequence of message payloads
    /// that right channel's attenuation (1 LSB is 0.5dB) when triggering DI2.
    /// </summary>
    [DisplayName("AttenuationRightDI2Payload")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Creates a sequence of message payloads that right channel's attenuation (1 LSB is 0.5dB) when triggering DI2.")]
    public partial class CreateAttenuationRightDI2Payload : HarpCombinator
    {
        /// <summary>
        /// Gets or sets the value that right channel's attenuation (1 LSB is 0.5dB) when triggering DI2.
        /// </summary>
        [Description("The value that right channel's attenuation (1 LSB is 0.5dB) when triggering DI2.")]
        public ushort Value { get; set; }

        /// <summary>
        /// Creates an observable sequence that contains a single message
        /// that right channel's attenuation (1 LSB is 0.5dB) when triggering DI2.
        /// </summary>
        /// <returns>
        /// A sequence containing a single <see cref="HarpMessage"/> object
        /// representing the created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process()
        {
            return Process(Observable.Return(System.Reactive.Unit.Default));
        }

        /// <summary>
        /// Creates an observable sequence of message payloads
        /// that right channel's attenuation (1 LSB is 0.5dB) when triggering DI2.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting message payloads.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing each
        /// created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => AttenuationRightDI2.FromPayload(MessageType, Value));
        }
    }

    /// <summary>
    /// Represents an operator that creates a sequence of message payloads
    /// that sound index and attenuation to be played when triggering DI0 [Att R] [Att L] [Index].
    /// </summary>
    [DisplayName("AttenuationAndSoundIndexDI0Payload")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Creates a sequence of message payloads that sound index and attenuation to be played when triggering DI0 [Att R] [Att L] [Index].")]
    public partial class CreateAttenuationAndSoundIndexDI0Payload : HarpCombinator
    {
        /// <summary>
        /// Gets or sets the value that sound index and attenuation to be played when triggering DI0 [Att R] [Att L] [Index].
        /// </summary>
        [Description("The value that sound index and attenuation to be played when triggering DI0 [Att R] [Att L] [Index].")]
        public ushort[] Value { get; set; }

        /// <summary>
        /// Creates an observable sequence that contains a single message
        /// that sound index and attenuation to be played when triggering DI0 [Att R] [Att L] [Index].
        /// </summary>
        /// <returns>
        /// A sequence containing a single <see cref="HarpMessage"/> object
        /// representing the created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process()
        {
            return Process(Observable.Return(System.Reactive.Unit.Default));
        }

        /// <summary>
        /// Creates an observable sequence of message payloads
        /// that sound index and attenuation to be played when triggering DI0 [Att R] [Att L] [Index].
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting message payloads.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing each
        /// created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => AttenuationAndSoundIndexDI0.FromPayload(MessageType, Value));
        }
    }

    /// <summary>
    /// Represents an operator that creates a sequence of message payloads
    /// that sound index and attenuation to be played when triggering DI1 [Att R] [Att L] [Index].
    /// </summary>
    [DisplayName("AttenuationAndSoundIndexDI1Payload")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Creates a sequence of message payloads that sound index and attenuation to be played when triggering DI1 [Att R] [Att L] [Index].")]
    public partial class CreateAttenuationAndSoundIndexDI1Payload : HarpCombinator
    {
        /// <summary>
        /// Gets or sets the value that sound index and attenuation to be played when triggering DI1 [Att R] [Att L] [Index].
        /// </summary>
        [Description("The value that sound index and attenuation to be played when triggering DI1 [Att R] [Att L] [Index].")]
        public ushort[] Value { get; set; }

        /// <summary>
        /// Creates an observable sequence that contains a single message
        /// that sound index and attenuation to be played when triggering DI1 [Att R] [Att L] [Index].
        /// </summary>
        /// <returns>
        /// A sequence containing a single <see cref="HarpMessage"/> object
        /// representing the created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process()
        {
            return Process(Observable.Return(System.Reactive.Unit.Default));
        }

        /// <summary>
        /// Creates an observable sequence of message payloads
        /// that sound index and attenuation to be played when triggering DI1 [Att R] [Att L] [Index].
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting message payloads.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing each
        /// created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => AttenuationAndSoundIndexDI1.FromPayload(MessageType, Value));
        }
    }

    /// <summary>
    /// Represents an operator that creates a sequence of message payloads
    /// that sound index and attenuation to be played when triggering DI2 [Att R] [Att L] [Index].
    /// </summary>
    [DisplayName("AttenuationAndSoundIndexDI2Payload")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Creates a sequence of message payloads that sound index and attenuation to be played when triggering DI2 [Att R] [Att L] [Index].")]
    public partial class CreateAttenuationAndSoundIndexDI2Payload : HarpCombinator
    {
        /// <summary>
        /// Gets or sets the value that sound index and attenuation to be played when triggering DI2 [Att R] [Att L] [Index].
        /// </summary>
        [Description("The value that sound index and attenuation to be played when triggering DI2 [Att R] [Att L] [Index].")]
        public ushort[] Value { get; set; }

        /// <summary>
        /// Creates an observable sequence that contains a single message
        /// that sound index and attenuation to be played when triggering DI2 [Att R] [Att L] [Index].
        /// </summary>
        /// <returns>
        /// A sequence containing a single <see cref="HarpMessage"/> object
        /// representing the created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process()
        {
            return Process(Observable.Return(System.Reactive.Unit.Default));
        }

        /// <summary>
        /// Creates an observable sequence of message payloads
        /// that sound index and attenuation to be played when triggering DI2 [Att R] [Att L] [Index].
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting message payloads.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing each
        /// created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => AttenuationAndSoundIndexDI2.FromPayload(MessageType, Value));
        }
    }

    /// <summary>
    /// Represents an operator that creates a sequence of message payloads
    /// that sound index and attenuation to be played when triggering DI0 [Att BOTH] [Frequency].
    /// </summary>
    [DisplayName("AttenuationAndFrequencyDI0Payload")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Creates a sequence of message payloads that sound index and attenuation to be played when triggering DI0 [Att BOTH] [Frequency].")]
    public partial class CreateAttenuationAndFrequencyDI0Payload : HarpCombinator
    {
        /// <summary>
        /// Gets or sets the value that sound index and attenuation to be played when triggering DI0 [Att BOTH] [Frequency].
        /// </summary>
        [Description("The value that sound index and attenuation to be played when triggering DI0 [Att BOTH] [Frequency].")]
        public ushort[] Value { get; set; }

        /// <summary>
        /// Creates an observable sequence that contains a single message
        /// that sound index and attenuation to be played when triggering DI0 [Att BOTH] [Frequency].
        /// </summary>
        /// <returns>
        /// A sequence containing a single <see cref="HarpMessage"/> object
        /// representing the created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process()
        {
            return Process(Observable.Return(System.Reactive.Unit.Default));
        }

        /// <summary>
        /// Creates an observable sequence of message payloads
        /// that sound index and attenuation to be played when triggering DI0 [Att BOTH] [Frequency].
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting message payloads.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing each
        /// created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => AttenuationAndFrequencyDI0.FromPayload(MessageType, Value));
        }
    }

    /// <summary>
    /// Represents an operator that creates a sequence of message payloads
    /// that sound index and attenuation to be played when triggering DI1 [Att BOTH] [Frequency].
    /// </summary>
    [DisplayName("AttenuationAndFrequencyDI1Payload")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Creates a sequence of message payloads that sound index and attenuation to be played when triggering DI1 [Att BOTH] [Frequency].")]
    public partial class CreateAttenuationAndFrequencyDI1Payload : HarpCombinator
    {
        /// <summary>
        /// Gets or sets the value that sound index and attenuation to be played when triggering DI1 [Att BOTH] [Frequency].
        /// </summary>
        [Description("The value that sound index and attenuation to be played when triggering DI1 [Att BOTH] [Frequency].")]
        public ushort[] Value { get; set; }

        /// <summary>
        /// Creates an observable sequence that contains a single message
        /// that sound index and attenuation to be played when triggering DI1 [Att BOTH] [Frequency].
        /// </summary>
        /// <returns>
        /// A sequence containing a single <see cref="HarpMessage"/> object
        /// representing the created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process()
        {
            return Process(Observable.Return(System.Reactive.Unit.Default));
        }

        /// <summary>
        /// Creates an observable sequence of message payloads
        /// that sound index and attenuation to be played when triggering DI1 [Att BOTH] [Frequency].
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting message payloads.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing each
        /// created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => AttenuationAndFrequencyDI1.FromPayload(MessageType, Value));
        }
    }

    /// <summary>
    /// Represents an operator that creates a sequence of message payloads
    /// that sound index and attenuation to be played when triggering DI2 [Att BOTH] [Frequency].
    /// </summary>
    [DisplayName("AttenuationAndFrequencyDI2Payload")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Creates a sequence of message payloads that sound index and attenuation to be played when triggering DI2 [Att BOTH] [Frequency].")]
    public partial class CreateAttenuationAndFrequencyDI2Payload : HarpCombinator
    {
        /// <summary>
        /// Gets or sets the value that sound index and attenuation to be played when triggering DI2 [Att BOTH] [Frequency].
        /// </summary>
        [Description("The value that sound index and attenuation to be played when triggering DI2 [Att BOTH] [Frequency].")]
        public ushort[] Value { get; set; }

        /// <summary>
        /// Creates an observable sequence that contains a single message
        /// that sound index and attenuation to be played when triggering DI2 [Att BOTH] [Frequency].
        /// </summary>
        /// <returns>
        /// A sequence containing a single <see cref="HarpMessage"/> object
        /// representing the created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process()
        {
            return Process(Observable.Return(System.Reactive.Unit.Default));
        }

        /// <summary>
        /// Creates an observable sequence of message payloads
        /// that sound index and attenuation to be played when triggering DI2 [Att BOTH] [Frequency].
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting message payloads.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing each
        /// created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => AttenuationAndFrequencyDI2.FromPayload(MessageType, Value));
        }
    }

    /// <summary>
    /// Represents an operator that creates a sequence of message payloads
    /// that configuration of the digital output 0 (DO0).
    /// </summary>
    [DisplayName("ConfigureDO0Payload")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Creates a sequence of message payloads that configuration of the digital output 0 (DO0).")]
    public partial class CreateConfigureDO0Payload : HarpCombinator
    {
        /// <summary>
        /// Gets or sets the value that configuration of the digital output 0 (DO0).
        /// </summary>
        [Description("The value that configuration of the digital output 0 (DO0).")]
        public byte Value { get; set; }

        /// <summary>
        /// Creates an observable sequence that contains a single message
        /// that configuration of the digital output 0 (DO0).
        /// </summary>
        /// <returns>
        /// A sequence containing a single <see cref="HarpMessage"/> object
        /// representing the created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process()
        {
            return Process(Observable.Return(System.Reactive.Unit.Default));
        }

        /// <summary>
        /// Creates an observable sequence of message payloads
        /// that configuration of the digital output 0 (DO0).
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting message payloads.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing each
        /// created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => ConfigureDO0.FromPayload(MessageType, Value));
        }
    }

    /// <summary>
    /// Represents an operator that creates a sequence of message payloads
    /// that configuration of the digital output 1 (DO1).
    /// </summary>
    [DisplayName("ConfigureDO1Payload")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Creates a sequence of message payloads that configuration of the digital output 1 (DO1).")]
    public partial class CreateConfigureDO1Payload : HarpCombinator
    {
        /// <summary>
        /// Gets or sets the value that configuration of the digital output 1 (DO1).
        /// </summary>
        [Description("The value that configuration of the digital output 1 (DO1).")]
        public byte Value { get; set; }

        /// <summary>
        /// Creates an observable sequence that contains a single message
        /// that configuration of the digital output 1 (DO1).
        /// </summary>
        /// <returns>
        /// A sequence containing a single <see cref="HarpMessage"/> object
        /// representing the created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process()
        {
            return Process(Observable.Return(System.Reactive.Unit.Default));
        }

        /// <summary>
        /// Creates an observable sequence of message payloads
        /// that configuration of the digital output 1 (DO1).
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting message payloads.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing each
        /// created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => ConfigureDO1.FromPayload(MessageType, Value));
        }
    }

    /// <summary>
    /// Represents an operator that creates a sequence of message payloads
    /// that configuration of the digital output 2 (DO2.
    /// </summary>
    [DisplayName("ConfigureDO2Payload")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Creates a sequence of message payloads that configuration of the digital output 2 (DO2.")]
    public partial class CreateConfigureDO2Payload : HarpCombinator
    {
        /// <summary>
        /// Gets or sets the value that configuration of the digital output 2 (DO2.
        /// </summary>
        [Description("The value that configuration of the digital output 2 (DO2.")]
        public byte Value { get; set; }

        /// <summary>
        /// Creates an observable sequence that contains a single message
        /// that configuration of the digital output 2 (DO2.
        /// </summary>
        /// <returns>
        /// A sequence containing a single <see cref="HarpMessage"/> object
        /// representing the created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process()
        {
            return Process(Observable.Return(System.Reactive.Unit.Default));
        }

        /// <summary>
        /// Creates an observable sequence of message payloads
        /// that configuration of the digital output 2 (DO2.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting message payloads.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing each
        /// created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => ConfigureDO2.FromPayload(MessageType, Value));
        }
    }

    /// <summary>
    /// Represents an operator that creates a sequence of message payloads
    /// that pulse for the digital output 0 (DO0) [1:255].
    /// </summary>
    [DisplayName("PulseDO0Payload")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Creates a sequence of message payloads that pulse for the digital output 0 (DO0) [1:255].")]
    public partial class CreatePulseDO0Payload : HarpCombinator
    {
        /// <summary>
        /// Gets or sets the value that pulse for the digital output 0 (DO0) [1:255].
        /// </summary>
        [Description("The value that pulse for the digital output 0 (DO0) [1:255].")]
        public byte Value { get; set; }

        /// <summary>
        /// Creates an observable sequence that contains a single message
        /// that pulse for the digital output 0 (DO0) [1:255].
        /// </summary>
        /// <returns>
        /// A sequence containing a single <see cref="HarpMessage"/> object
        /// representing the created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process()
        {
            return Process(Observable.Return(System.Reactive.Unit.Default));
        }

        /// <summary>
        /// Creates an observable sequence of message payloads
        /// that pulse for the digital output 0 (DO0) [1:255].
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting message payloads.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing each
        /// created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => PulseDO0.FromPayload(MessageType, Value));
        }
    }

    /// <summary>
    /// Represents an operator that creates a sequence of message payloads
    /// that pulse for the digital output 1 (DO1) [1:255].
    /// </summary>
    [DisplayName("PulseDO1Payload")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Creates a sequence of message payloads that pulse for the digital output 1 (DO1) [1:255].")]
    public partial class CreatePulseDO1Payload : HarpCombinator
    {
        /// <summary>
        /// Gets or sets the value that pulse for the digital output 1 (DO1) [1:255].
        /// </summary>
        [Description("The value that pulse for the digital output 1 (DO1) [1:255].")]
        public byte Value { get; set; }

        /// <summary>
        /// Creates an observable sequence that contains a single message
        /// that pulse for the digital output 1 (DO1) [1:255].
        /// </summary>
        /// <returns>
        /// A sequence containing a single <see cref="HarpMessage"/> object
        /// representing the created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process()
        {
            return Process(Observable.Return(System.Reactive.Unit.Default));
        }

        /// <summary>
        /// Creates an observable sequence of message payloads
        /// that pulse for the digital output 1 (DO1) [1:255].
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting message payloads.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing each
        /// created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => PulseDO1.FromPayload(MessageType, Value));
        }
    }

    /// <summary>
    /// Represents an operator that creates a sequence of message payloads
    /// that pulse for the digital output 2 (DO2) [1:255].
    /// </summary>
    [DisplayName("PulseDO2Payload")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Creates a sequence of message payloads that pulse for the digital output 2 (DO2) [1:255].")]
    public partial class CreatePulseDO2Payload : HarpCombinator
    {
        /// <summary>
        /// Gets or sets the value that pulse for the digital output 2 (DO2) [1:255].
        /// </summary>
        [Description("The value that pulse for the digital output 2 (DO2) [1:255].")]
        public byte Value { get; set; }

        /// <summary>
        /// Creates an observable sequence that contains a single message
        /// that pulse for the digital output 2 (DO2) [1:255].
        /// </summary>
        /// <returns>
        /// A sequence containing a single <see cref="HarpMessage"/> object
        /// representing the created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process()
        {
            return Process(Observable.Return(System.Reactive.Unit.Default));
        }

        /// <summary>
        /// Creates an observable sequence of message payloads
        /// that pulse for the digital output 2 (DO2) [1:255].
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting message payloads.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing each
        /// created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => PulseDO2.FromPayload(MessageType, Value));
        }
    }

    /// <summary>
    /// Represents an operator that creates a sequence of message payloads
    /// that set the specified digital output lines.
    /// </summary>
    [DisplayName("OutputSetPayload")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Creates a sequence of message payloads that set the specified digital output lines.")]
    public partial class CreateOutputSetPayload : HarpCombinator
    {
        /// <summary>
        /// Gets or sets the value that set the specified digital output lines.
        /// </summary>
        [Description("The value that set the specified digital output lines.")]
        public byte Value { get; set; }

        /// <summary>
        /// Creates an observable sequence that contains a single message
        /// that set the specified digital output lines.
        /// </summary>
        /// <returns>
        /// A sequence containing a single <see cref="HarpMessage"/> object
        /// representing the created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process()
        {
            return Process(Observable.Return(System.Reactive.Unit.Default));
        }

        /// <summary>
        /// Creates an observable sequence of message payloads
        /// that set the specified digital output lines.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting message payloads.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing each
        /// created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => OutputSet.FromPayload(MessageType, Value));
        }
    }

    /// <summary>
    /// Represents an operator that creates a sequence of message payloads
    /// that clear the specified digital output lines.
    /// </summary>
    [DisplayName("OutputClearPayload")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Creates a sequence of message payloads that clear the specified digital output lines.")]
    public partial class CreateOutputClearPayload : HarpCombinator
    {
        /// <summary>
        /// Gets or sets the value that clear the specified digital output lines.
        /// </summary>
        [Description("The value that clear the specified digital output lines.")]
        public byte Value { get; set; }

        /// <summary>
        /// Creates an observable sequence that contains a single message
        /// that clear the specified digital output lines.
        /// </summary>
        /// <returns>
        /// A sequence containing a single <see cref="HarpMessage"/> object
        /// representing the created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process()
        {
            return Process(Observable.Return(System.Reactive.Unit.Default));
        }

        /// <summary>
        /// Creates an observable sequence of message payloads
        /// that clear the specified digital output lines.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting message payloads.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing each
        /// created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => OutputClear.FromPayload(MessageType, Value));
        }
    }

    /// <summary>
    /// Represents an operator that creates a sequence of message payloads
    /// that toggle the specified digital output lines.
    /// </summary>
    [DisplayName("OutputTogglePayload")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Creates a sequence of message payloads that toggle the specified digital output lines.")]
    public partial class CreateOutputTogglePayload : HarpCombinator
    {
        /// <summary>
        /// Gets or sets the value that toggle the specified digital output lines.
        /// </summary>
        [Description("The value that toggle the specified digital output lines.")]
        public byte Value { get; set; }

        /// <summary>
        /// Creates an observable sequence that contains a single message
        /// that toggle the specified digital output lines.
        /// </summary>
        /// <returns>
        /// A sequence containing a single <see cref="HarpMessage"/> object
        /// representing the created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process()
        {
            return Process(Observable.Return(System.Reactive.Unit.Default));
        }

        /// <summary>
        /// Creates an observable sequence of message payloads
        /// that toggle the specified digital output lines.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting message payloads.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing each
        /// created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => OutputToggle.FromPayload(MessageType, Value));
        }
    }

    /// <summary>
    /// Represents an operator that creates a sequence of message payloads
    /// that write the state of all digital output lines.
    /// </summary>
    [DisplayName("OutputStatePayload")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Creates a sequence of message payloads that write the state of all digital output lines.")]
    public partial class CreateOutputStatePayload : HarpCombinator
    {
        /// <summary>
        /// Gets or sets the value that write the state of all digital output lines.
        /// </summary>
        [Description("The value that write the state of all digital output lines.")]
        public byte Value { get; set; }

        /// <summary>
        /// Creates an observable sequence that contains a single message
        /// that write the state of all digital output lines.
        /// </summary>
        /// <returns>
        /// A sequence containing a single <see cref="HarpMessage"/> object
        /// representing the created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process()
        {
            return Process(Observable.Return(System.Reactive.Unit.Default));
        }

        /// <summary>
        /// Creates an observable sequence of message payloads
        /// that write the state of all digital output lines.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting message payloads.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing each
        /// created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => OutputState.FromPayload(MessageType, Value));
        }
    }

    /// <summary>
    /// Represents an operator that creates a sequence of message payloads
    /// that configuration of Analog Inputs.
    /// </summary>
    [DisplayName("ConfigureAdcPayload")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Creates a sequence of message payloads that configuration of Analog Inputs.")]
    public partial class CreateConfigureAdcPayload : HarpCombinator
    {
        /// <summary>
        /// Gets or sets the value that configuration of Analog Inputs.
        /// </summary>
        [Description("The value that configuration of Analog Inputs.")]
        public byte Value { get; set; }

        /// <summary>
        /// Creates an observable sequence that contains a single message
        /// that configuration of Analog Inputs.
        /// </summary>
        /// <returns>
        /// A sequence containing a single <see cref="HarpMessage"/> object
        /// representing the created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process()
        {
            return Process(Observable.Return(System.Reactive.Unit.Default));
        }

        /// <summary>
        /// Creates an observable sequence of message payloads
        /// that configuration of Analog Inputs.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting message payloads.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing each
        /// created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => ConfigureAdc.FromPayload(MessageType, Value));
        }
    }

    /// <summary>
    /// Represents an operator that creates a sequence of message payloads
    /// that [ADC0]   [ADC1]   [ATT LEFT]   [ATT RIGHT]   [FREQUENCY]   Values are 0 if not used.
    /// </summary>
    [DisplayName("AnalogInputsPayload")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Creates a sequence of message payloads that [ADC0]   [ADC1]   [ATT LEFT]   [ATT RIGHT]   [FREQUENCY]   Values are 0 if not used.")]
    public partial class CreateAnalogInputsPayload : HarpCombinator
    {
        /// <summary>
        /// Gets or sets the value that [ADC0]   [ADC1]   [ATT LEFT]   [ATT RIGHT]   [FREQUENCY]   Values are 0 if not used.
        /// </summary>
        [Description("The value that [ADC0]   [ADC1]   [ATT LEFT]   [ATT RIGHT]   [FREQUENCY]   Values are 0 if not used.")]
        public ushort[] Value { get; set; }

        /// <summary>
        /// Creates an observable sequence that contains a single message
        /// that [ADC0]   [ADC1]   [ATT LEFT]   [ATT RIGHT]   [FREQUENCY]   Values are 0 if not used.
        /// </summary>
        /// <returns>
        /// A sequence containing a single <see cref="HarpMessage"/> object
        /// representing the created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process()
        {
            return Process(Observable.Return(System.Reactive.Unit.Default));
        }

        /// <summary>
        /// Creates an observable sequence of message payloads
        /// that [ADC0]   [ADC1]   [ATT LEFT]   [ATT RIGHT]   [FREQUENCY]   Values are 0 if not used.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting message payloads.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing each
        /// created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => AnalogInputs.FromPayload(MessageType, Value));
        }
    }

    /// <summary>
    /// Represents an operator that creates a sequence of message payloads
    /// that send commands to PIC32 micro-controller.
    /// </summary>
    [DisplayName("CommandsPayload")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Creates a sequence of message payloads that send commands to PIC32 micro-controller.")]
    public partial class CreateCommandsPayload : HarpCombinator
    {
        /// <summary>
        /// Gets or sets the value that send commands to PIC32 micro-controller.
        /// </summary>
        [Description("The value that send commands to PIC32 micro-controller.")]
        public byte Value { get; set; }

        /// <summary>
        /// Creates an observable sequence that contains a single message
        /// that send commands to PIC32 micro-controller.
        /// </summary>
        /// <returns>
        /// A sequence containing a single <see cref="HarpMessage"/> object
        /// representing the created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process()
        {
            return Process(Observable.Return(System.Reactive.Unit.Default));
        }

        /// <summary>
        /// Creates an observable sequence of message payloads
        /// that send commands to PIC32 micro-controller.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting message payloads.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing each
        /// created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => Commands.FromPayload(MessageType, Value));
        }
    }

    /// <summary>
    /// Represents an operator that creates a sequence of message payloads
    /// that enable the Events.
    /// </summary>
    [DisplayName("EnableEventsPayload")]
    [WorkflowElementCategory(ElementCategory.Transform)]
    [Description("Creates a sequence of message payloads that enable the Events.")]
    public partial class CreateEnableEventsPayload : HarpCombinator
    {
        /// <summary>
        /// Gets or sets the value that enable the Events.
        /// </summary>
        [Description("The value that enable the Events.")]
        public byte Value { get; set; }

        /// <summary>
        /// Creates an observable sequence that contains a single message
        /// that enable the Events.
        /// </summary>
        /// <returns>
        /// A sequence containing a single <see cref="HarpMessage"/> object
        /// representing the created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process()
        {
            return Process(Observable.Return(System.Reactive.Unit.Default));
        }

        /// <summary>
        /// Creates an observable sequence of message payloads
        /// that enable the Events.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of the elements in the <paramref name="source"/> sequence.
        /// </typeparam>
        /// <param name="source">
        /// The sequence containing the notifications used for emitting message payloads.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="HarpMessage"/> objects representing each
        /// created message payload.
        /// </returns>
        public IObservable<HarpMessage> Process<TSource>(IObservable<TSource> source)
        {
            return source.Select(_ => EnableEvents.FromPayload(MessageType, Value));
        }
    }
}
