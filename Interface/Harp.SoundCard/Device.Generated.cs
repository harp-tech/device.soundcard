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
            { 38, typeof(Reserved0) },
            { 39, typeof(Reserved1) },
            { 40, typeof(InputState) },
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
            { 62, typeof(Reserved2) },
            { 63, typeof(Reserved3) },
            { 64, typeof(Reserved4) },
            { 65, typeof(ConfigureDO0) },
            { 66, typeof(ConfigureDO1) },
            { 67, typeof(ConfigureDO2) },
            { 68, typeof(PulseDO0) },
            { 69, typeof(PulseDO1) },
            { 70, typeof(PulseDO2) },
            { 71, typeof(Reserved5) },
            { 72, typeof(Reserved6) },
            { 73, typeof(Reserved7) },
            { 74, typeof(OutputSet) },
            { 75, typeof(OutputClear) },
            { 76, typeof(OutputToggle) },
            { 77, typeof(OutputState) },
            { 78, typeof(Reserved8) },
            { 79, typeof(Reserved9) },
            { 80, typeof(ConfigureAdc) },
            { 81, typeof(AnalogData) },
            { 82, typeof(Commands) },
            { 83, typeof(Reserved10) },
            { 84, typeof(Reserved11) },
            { 85, typeof(Reserved12) },
            { 86, typeof(EnableEvents) }
        };

        /// <summary>
        /// Gets the contents of the metadata file describing the <see cref="SoundCard"/>
        /// device registers.
        /// </summary>
        public static readonly string Metadata = GetDeviceMetadata();

        static string GetDeviceMetadata()
        {
            var deviceType = typeof(Device);
            using var metadataStream = deviceType.Assembly.GetManifestResourceStream($"{deviceType.Namespace}.device.yml");
            using var streamReader = new System.IO.StreamReader(metadataStream);
            return streamReader.ReadToEnd();
        }
    }

    /// <summary>
    /// Represents an operator that returns the contents of the metadata file
    /// describing the <see cref="SoundCard"/> device registers.
    /// </summary>
    [Description("Returns the contents of the metadata file describing the SoundCard device registers.")]
    public partial class GetMetadata : Source<string>
    {
        /// <summary>
        /// Returns an observable sequence with the contents of the metadata file
        /// describing the <see cref="SoundCard"/> device registers.
        /// </summary>
        /// <returns>
        /// A sequence with a single <see cref="string"/> object representing the
        /// contents of the metadata file.
        /// </returns>
        public override IObservable<string> Generate()
        {
            return Observable.Return(Device.Metadata);
        }
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
    /// <seealso cref="InputState"/>
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
    /// <seealso cref="AnalogData"/>
    /// <seealso cref="Commands"/>
    /// <seealso cref="EnableEvents"/>
    [XmlInclude(typeof(PlaySoundOrFrequency))]
    [XmlInclude(typeof(Stop))]
    [XmlInclude(typeof(AttenuationLeft))]
    [XmlInclude(typeof(AttenuationRight))]
    [XmlInclude(typeof(AttenuationBoth))]
    [XmlInclude(typeof(AttenuationAndPlaySoundOrFreq))]
    [XmlInclude(typeof(InputState))]
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
    [XmlInclude(typeof(AnalogData))]
    [XmlInclude(typeof(Commands))]
    [XmlInclude(typeof(EnableEvents))]
    [Description("Filters register-specific messages reported by the SoundCard device.")]
    public class FilterRegister : FilterRegisterBuilder, INamedElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterRegister"/> class.
        /// </summary>
        public FilterRegister()
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
    /// <seealso cref="InputState"/>
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
    /// <seealso cref="AnalogData"/>
    /// <seealso cref="Commands"/>
    /// <seealso cref="EnableEvents"/>
    [XmlInclude(typeof(PlaySoundOrFrequency))]
    [XmlInclude(typeof(Stop))]
    [XmlInclude(typeof(AttenuationLeft))]
    [XmlInclude(typeof(AttenuationRight))]
    [XmlInclude(typeof(AttenuationBoth))]
    [XmlInclude(typeof(AttenuationAndPlaySoundOrFreq))]
    [XmlInclude(typeof(InputState))]
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
    [XmlInclude(typeof(AnalogData))]
    [XmlInclude(typeof(Commands))]
    [XmlInclude(typeof(EnableEvents))]
    [XmlInclude(typeof(TimestampedPlaySoundOrFrequency))]
    [XmlInclude(typeof(TimestampedStop))]
    [XmlInclude(typeof(TimestampedAttenuationLeft))]
    [XmlInclude(typeof(TimestampedAttenuationRight))]
    [XmlInclude(typeof(TimestampedAttenuationBoth))]
    [XmlInclude(typeof(TimestampedAttenuationAndPlaySoundOrFreq))]
    [XmlInclude(typeof(TimestampedInputState))]
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
    [XmlInclude(typeof(TimestampedAnalogData))]
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
    /// <seealso cref="InputState"/>
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
    /// <seealso cref="AnalogData"/>
    /// <seealso cref="Commands"/>
    /// <seealso cref="EnableEvents"/>
    [XmlInclude(typeof(PlaySoundOrFrequency))]
    [XmlInclude(typeof(Stop))]
    [XmlInclude(typeof(AttenuationLeft))]
    [XmlInclude(typeof(AttenuationRight))]
    [XmlInclude(typeof(AttenuationBoth))]
    [XmlInclude(typeof(AttenuationAndPlaySoundOrFreq))]
    [XmlInclude(typeof(InputState))]
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
    [XmlInclude(typeof(AnalogData))]
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
    /// Represents a register that starts the sound index (if less than 32) or frequency (if greater or equal than 32).
    /// </summary>
    [Description("Starts the sound index (if less than 32) or frequency (if greater or equal than 32)")]
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
    /// Represents a register that reserved for future use.
    /// </summary>
    [Description("Reserved for future use")]
    internal partial class Reserved0
    {
        /// <summary>
        /// Represents the address of the <see cref="Reserved0"/> register. This field is constant.
        /// </summary>
        public const int Address = 38;

        /// <summary>
        /// Represents the payload type of the <see cref="Reserved0"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="Reserved0"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;
    }

    /// <summary>
    /// Represents a register that reserved for future use.
    /// </summary>
    [Description("Reserved for future use")]
    internal partial class Reserved1
    {
        /// <summary>
        /// Represents the address of the <see cref="Reserved1"/> register. This field is constant.
        /// </summary>
        public const int Address = 39;

        /// <summary>
        /// Represents the payload type of the <see cref="Reserved1"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="Reserved1"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;
    }

    /// <summary>
    /// Represents a register that state of the digital inputs.
    /// </summary>
    [Description("State of the digital inputs")]
    public partial class InputState
    {
        /// <summary>
        /// Represents the address of the <see cref="InputState"/> register. This field is constant.
        /// </summary>
        public const int Address = 40;

        /// <summary>
        /// Represents the payload type of the <see cref="InputState"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="InputState"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="InputState"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static DigitalInputs GetPayload(HarpMessage message)
        {
            return (DigitalInputs)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="InputState"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DigitalInputs> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((DigitalInputs)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="InputState"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="InputState"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, DigitalInputs value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="InputState"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="InputState"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, DigitalInputs value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// InputState register.
    /// </summary>
    /// <seealso cref="InputState"/>
    [Description("Filters and selects timestamped messages from the InputState register.")]
    public partial class TimestampedInputState
    {
        /// <summary>
        /// Represents the address of the <see cref="InputState"/> register. This field is constant.
        /// </summary>
        public const int Address = InputState.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="InputState"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DigitalInputs> GetPayload(HarpMessage message)
        {
            return InputState.GetTimestampedPayload(message);
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
        public static DigitalInputConfiguration GetPayload(HarpMessage message)
        {
            return (DigitalInputConfiguration)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="ConfigureDI0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DigitalInputConfiguration> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((DigitalInputConfiguration)payload.Value, payload.Seconds);
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
        public static HarpMessage FromPayload(MessageType messageType, DigitalInputConfiguration value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
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
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, DigitalInputConfiguration value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
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
        public static Timestamped<DigitalInputConfiguration> GetPayload(HarpMessage message)
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
        public static DigitalInputConfiguration GetPayload(HarpMessage message)
        {
            return (DigitalInputConfiguration)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="ConfigureDI1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DigitalInputConfiguration> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((DigitalInputConfiguration)payload.Value, payload.Seconds);
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
        public static HarpMessage FromPayload(MessageType messageType, DigitalInputConfiguration value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
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
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, DigitalInputConfiguration value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
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
        public static Timestamped<DigitalInputConfiguration> GetPayload(HarpMessage message)
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
        public static DigitalInputConfiguration GetPayload(HarpMessage message)
        {
            return (DigitalInputConfiguration)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="ConfigureDI2"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DigitalInputConfiguration> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((DigitalInputConfiguration)payload.Value, payload.Seconds);
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
        public static HarpMessage FromPayload(MessageType messageType, DigitalInputConfiguration value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
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
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, DigitalInputConfiguration value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
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
        public static Timestamped<DigitalInputConfiguration> GetPayload(HarpMessage message)
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
    /// Represents a register that reserved for future use.
    /// </summary>
    [Description("Reserved for future use")]
    internal partial class Reserved2
    {
        /// <summary>
        /// Represents the address of the <see cref="Reserved2"/> register. This field is constant.
        /// </summary>
        public const int Address = 62;

        /// <summary>
        /// Represents the payload type of the <see cref="Reserved2"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="Reserved2"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;
    }

    /// <summary>
    /// Represents a register that reserved for future use.
    /// </summary>
    [Description("Reserved for future use")]
    internal partial class Reserved3
    {
        /// <summary>
        /// Represents the address of the <see cref="Reserved3"/> register. This field is constant.
        /// </summary>
        public const int Address = 63;

        /// <summary>
        /// Represents the payload type of the <see cref="Reserved3"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="Reserved3"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;
    }

    /// <summary>
    /// Represents a register that reserved for future use.
    /// </summary>
    [Description("Reserved for future use")]
    internal partial class Reserved4
    {
        /// <summary>
        /// Represents the address of the <see cref="Reserved4"/> register. This field is constant.
        /// </summary>
        public const int Address = 64;

        /// <summary>
        /// Represents the payload type of the <see cref="Reserved4"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="Reserved4"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;
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
        public static DigitalOutputConfiguration GetPayload(HarpMessage message)
        {
            return (DigitalOutputConfiguration)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="ConfigureDO0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DigitalOutputConfiguration> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((DigitalOutputConfiguration)payload.Value, payload.Seconds);
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
        public static HarpMessage FromPayload(MessageType messageType, DigitalOutputConfiguration value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
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
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, DigitalOutputConfiguration value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
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
        public static Timestamped<DigitalOutputConfiguration> GetPayload(HarpMessage message)
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
        public static DigitalOutputConfiguration GetPayload(HarpMessage message)
        {
            return (DigitalOutputConfiguration)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="ConfigureDO1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DigitalOutputConfiguration> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((DigitalOutputConfiguration)payload.Value, payload.Seconds);
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
        public static HarpMessage FromPayload(MessageType messageType, DigitalOutputConfiguration value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
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
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, DigitalOutputConfiguration value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
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
        public static Timestamped<DigitalOutputConfiguration> GetPayload(HarpMessage message)
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
        public static DigitalOutputConfiguration GetPayload(HarpMessage message)
        {
            return (DigitalOutputConfiguration)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="ConfigureDO2"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DigitalOutputConfiguration> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((DigitalOutputConfiguration)payload.Value, payload.Seconds);
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
        public static HarpMessage FromPayload(MessageType messageType, DigitalOutputConfiguration value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
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
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, DigitalOutputConfiguration value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
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
        public static Timestamped<DigitalOutputConfiguration> GetPayload(HarpMessage message)
        {
            return ConfigureDO2.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that pulse for the digital output 0 (DO0).
    /// </summary>
    [Description("Pulse for the digital output 0 (DO0)")]
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
    /// Represents a register that pulse for the digital output 1 (DO1).
    /// </summary>
    [Description("Pulse for the digital output 1 (DO1)")]
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
    /// Represents a register that pulse for the digital output 2 (DO2).
    /// </summary>
    [Description("Pulse for the digital output 2 (DO2)")]
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
    /// Represents a register that reserved for future use.
    /// </summary>
    [Description("Reserved for future use")]
    internal partial class Reserved5
    {
        /// <summary>
        /// Represents the address of the <see cref="Reserved5"/> register. This field is constant.
        /// </summary>
        public const int Address = 71;

        /// <summary>
        /// Represents the payload type of the <see cref="Reserved5"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="Reserved5"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;
    }

    /// <summary>
    /// Represents a register that reserved for future use.
    /// </summary>
    [Description("Reserved for future use")]
    internal partial class Reserved6
    {
        /// <summary>
        /// Represents the address of the <see cref="Reserved6"/> register. This field is constant.
        /// </summary>
        public const int Address = 72;

        /// <summary>
        /// Represents the payload type of the <see cref="Reserved6"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="Reserved6"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;
    }

    /// <summary>
    /// Represents a register that reserved for future use.
    /// </summary>
    [Description("Reserved for future use")]
    internal partial class Reserved7
    {
        /// <summary>
        /// Represents the address of the <see cref="Reserved7"/> register. This field is constant.
        /// </summary>
        public const int Address = 73;

        /// <summary>
        /// Represents the payload type of the <see cref="Reserved7"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="Reserved7"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;
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
        public static DigitalOutputs GetPayload(HarpMessage message)
        {
            return (DigitalOutputs)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="OutputSet"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DigitalOutputs> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((DigitalOutputs)payload.Value, payload.Seconds);
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
        public static HarpMessage FromPayload(MessageType messageType, DigitalOutputs value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
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
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, DigitalOutputs value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
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
        public static Timestamped<DigitalOutputs> GetPayload(HarpMessage message)
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
        public static DigitalOutputs GetPayload(HarpMessage message)
        {
            return (DigitalOutputs)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="OutputClear"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DigitalOutputs> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((DigitalOutputs)payload.Value, payload.Seconds);
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
        public static HarpMessage FromPayload(MessageType messageType, DigitalOutputs value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
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
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, DigitalOutputs value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
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
        public static Timestamped<DigitalOutputs> GetPayload(HarpMessage message)
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
        public static DigitalOutputs GetPayload(HarpMessage message)
        {
            return (DigitalOutputs)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="OutputToggle"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DigitalOutputs> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((DigitalOutputs)payload.Value, payload.Seconds);
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
        public static HarpMessage FromPayload(MessageType messageType, DigitalOutputs value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
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
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, DigitalOutputs value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
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
        public static Timestamped<DigitalOutputs> GetPayload(HarpMessage message)
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
        public static DigitalOutputs GetPayload(HarpMessage message)
        {
            return (DigitalOutputs)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="OutputState"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<DigitalOutputs> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((DigitalOutputs)payload.Value, payload.Seconds);
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
        public static HarpMessage FromPayload(MessageType messageType, DigitalOutputs value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
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
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, DigitalOutputs value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
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
        public static Timestamped<DigitalOutputs> GetPayload(HarpMessage message)
        {
            return OutputState.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that reserved for future use.
    /// </summary>
    [Description("Reserved for future use")]
    internal partial class Reserved8
    {
        /// <summary>
        /// Represents the address of the <see cref="Reserved8"/> register. This field is constant.
        /// </summary>
        public const int Address = 78;

        /// <summary>
        /// Represents the payload type of the <see cref="Reserved8"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="Reserved8"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;
    }

    /// <summary>
    /// Represents a register that reserved for future use.
    /// </summary>
    [Description("Reserved for future use")]
    internal partial class Reserved9
    {
        /// <summary>
        /// Represents the address of the <see cref="Reserved9"/> register. This field is constant.
        /// </summary>
        public const int Address = 79;

        /// <summary>
        /// Represents the payload type of the <see cref="Reserved9"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="Reserved9"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;
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
        public static AdcConfiguration GetPayload(HarpMessage message)
        {
            return (AdcConfiguration)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="ConfigureAdc"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<AdcConfiguration> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((AdcConfiguration)payload.Value, payload.Seconds);
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
        public static HarpMessage FromPayload(MessageType messageType, AdcConfiguration value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
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
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, AdcConfiguration value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
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
        public static Timestamped<AdcConfiguration> GetPayload(HarpMessage message)
        {
            return ConfigureAdc.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that contains sampled analog input data or dynamic sound parameters controlled by the ADC channels. Values are zero if not used.
    /// </summary>
    [Description("Contains sampled analog input data or dynamic sound parameters controlled by the ADC channels. Values are zero if not used.")]
    public partial class AnalogData
    {
        /// <summary>
        /// Represents the address of the <see cref="AnalogData"/> register. This field is constant.
        /// </summary>
        public const int Address = 81;

        /// <summary>
        /// Represents the payload type of the <see cref="AnalogData"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="AnalogData"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 5;

        static AnalogDataPayload ParsePayload(ushort[] payload)
        {
            AnalogDataPayload result;
            result.Adc0 = payload[0];
            result.Adc1 = payload[1];
            result.AttenuationLeft = payload[2];
            result.AttenuationRight = payload[3];
            result.Frequency = payload[4];
            return result;
        }

        static ushort[] FormatPayload(AnalogDataPayload value)
        {
            ushort[] result;
            result = new ushort[5];
            result[0] = value.Adc0;
            result[1] = value.Adc1;
            result[2] = value.AttenuationLeft;
            result[3] = value.AttenuationRight;
            result[4] = value.Frequency;
            return result;
        }

        /// <summary>
        /// Returns the payload data for <see cref="AnalogData"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static AnalogDataPayload GetPayload(HarpMessage message)
        {
            return ParsePayload(message.GetPayloadArray<ushort>());
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="AnalogData"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<AnalogDataPayload> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadArray<ushort>();
            return Timestamped.Create(ParsePayload(payload.Value), payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="AnalogData"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="AnalogData"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, AnalogDataPayload value)
        {
            return HarpMessage.FromUInt16(Address, messageType, FormatPayload(value));
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="AnalogData"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="AnalogData"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, AnalogDataPayload value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, FormatPayload(value));
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// AnalogData register.
    /// </summary>
    /// <seealso cref="AnalogData"/>
    [Description("Filters and selects timestamped messages from the AnalogData register.")]
    public partial class TimestampedAnalogData
    {
        /// <summary>
        /// Represents the address of the <see cref="AnalogData"/> register. This field is constant.
        /// </summary>
        public const int Address = AnalogData.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="AnalogData"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<AnalogDataPayload> GetPayload(HarpMessage message)
        {
            return AnalogData.GetTimestampedPayload(message);
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
        public static ControllerCommand GetPayload(HarpMessage message)
        {
            return (ControllerCommand)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Commands"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ControllerCommand> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((ControllerCommand)payload.Value, payload.Seconds);
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
        public static HarpMessage FromPayload(MessageType messageType, ControllerCommand value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
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
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ControllerCommand value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
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
        public static Timestamped<ControllerCommand> GetPayload(HarpMessage message)
        {
            return Commands.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that reserved for future use.
    /// </summary>
    [Description("Reserved for future use")]
    internal partial class Reserved10
    {
        /// <summary>
        /// Represents the address of the <see cref="Reserved10"/> register. This field is constant.
        /// </summary>
        public const int Address = 83;

        /// <summary>
        /// Represents the payload type of the <see cref="Reserved10"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="Reserved10"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;
    }

    /// <summary>
    /// Represents a register that reserved for future use.
    /// </summary>
    [Description("Reserved for future use")]
    internal partial class Reserved11
    {
        /// <summary>
        /// Represents the address of the <see cref="Reserved11"/> register. This field is constant.
        /// </summary>
        public const int Address = 84;

        /// <summary>
        /// Represents the payload type of the <see cref="Reserved11"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="Reserved11"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;
    }

    /// <summary>
    /// Represents a register that reserved for future use.
    /// </summary>
    [Description("Reserved for future use")]
    internal partial class Reserved12
    {
        /// <summary>
        /// Represents the address of the <see cref="Reserved12"/> register. This field is constant.
        /// </summary>
        public const int Address = 85;

        /// <summary>
        /// Represents the payload type of the <see cref="Reserved12"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="Reserved12"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;
    }

    /// <summary>
    /// Represents a register that specifies the active events in the SoundCard device.
    /// </summary>
    [Description("Specifies the active events in the SoundCard device")]
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
        public static SoundCardEvents GetPayload(HarpMessage message)
        {
            return (SoundCardEvents)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="EnableEvents"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<SoundCardEvents> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((SoundCardEvents)payload.Value, payload.Seconds);
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
        public static HarpMessage FromPayload(MessageType messageType, SoundCardEvents value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
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
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, SoundCardEvents value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
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
        public static Timestamped<SoundCardEvents> GetPayload(HarpMessage message)
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
    /// <seealso cref="CreateInputStatePayload"/>
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
    /// <seealso cref="CreateAnalogDataPayload"/>
    /// <seealso cref="CreateCommandsPayload"/>
    /// <seealso cref="CreateEnableEventsPayload"/>
    [XmlInclude(typeof(CreatePlaySoundOrFrequencyPayload))]
    [XmlInclude(typeof(CreateStopPayload))]
    [XmlInclude(typeof(CreateAttenuationLeftPayload))]
    [XmlInclude(typeof(CreateAttenuationRightPayload))]
    [XmlInclude(typeof(CreateAttenuationBothPayload))]
    [XmlInclude(typeof(CreateAttenuationAndPlaySoundOrFreqPayload))]
    [XmlInclude(typeof(CreateInputStatePayload))]
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
    [XmlInclude(typeof(CreateAnalogDataPayload))]
    [XmlInclude(typeof(CreateCommandsPayload))]
    [XmlInclude(typeof(CreateEnableEventsPayload))]
    [XmlInclude(typeof(CreateTimestampedPlaySoundOrFrequencyPayload))]
    [XmlInclude(typeof(CreateTimestampedStopPayload))]
    [XmlInclude(typeof(CreateTimestampedAttenuationLeftPayload))]
    [XmlInclude(typeof(CreateTimestampedAttenuationRightPayload))]
    [XmlInclude(typeof(CreateTimestampedAttenuationBothPayload))]
    [XmlInclude(typeof(CreateTimestampedAttenuationAndPlaySoundOrFreqPayload))]
    [XmlInclude(typeof(CreateTimestampedInputStatePayload))]
    [XmlInclude(typeof(CreateTimestampedConfigureDI0Payload))]
    [XmlInclude(typeof(CreateTimestampedConfigureDI1Payload))]
    [XmlInclude(typeof(CreateTimestampedConfigureDI2Payload))]
    [XmlInclude(typeof(CreateTimestampedSoundIndexDI0Payload))]
    [XmlInclude(typeof(CreateTimestampedSoundIndexDI1Payload))]
    [XmlInclude(typeof(CreateTimestampedSoundIndexDI2Payload))]
    [XmlInclude(typeof(CreateTimestampedFrequencyDI0Payload))]
    [XmlInclude(typeof(CreateTimestampedFrequencyDI1Payload))]
    [XmlInclude(typeof(CreateTimestampedFrequencyDI2Payload))]
    [XmlInclude(typeof(CreateTimestampedAttenuationLeftDI0Payload))]
    [XmlInclude(typeof(CreateTimestampedAttenuationLeftDI1Payload))]
    [XmlInclude(typeof(CreateTimestampedAttenuationLeftDI2Payload))]
    [XmlInclude(typeof(CreateTimestampedAttenuationRightDI0Payload))]
    [XmlInclude(typeof(CreateTimestampedAttenuationRightDI1Payload))]
    [XmlInclude(typeof(CreateTimestampedAttenuationRightDI2Payload))]
    [XmlInclude(typeof(CreateTimestampedAttenuationAndSoundIndexDI0Payload))]
    [XmlInclude(typeof(CreateTimestampedAttenuationAndSoundIndexDI1Payload))]
    [XmlInclude(typeof(CreateTimestampedAttenuationAndSoundIndexDI2Payload))]
    [XmlInclude(typeof(CreateTimestampedAttenuationAndFrequencyDI0Payload))]
    [XmlInclude(typeof(CreateTimestampedAttenuationAndFrequencyDI1Payload))]
    [XmlInclude(typeof(CreateTimestampedAttenuationAndFrequencyDI2Payload))]
    [XmlInclude(typeof(CreateTimestampedConfigureDO0Payload))]
    [XmlInclude(typeof(CreateTimestampedConfigureDO1Payload))]
    [XmlInclude(typeof(CreateTimestampedConfigureDO2Payload))]
    [XmlInclude(typeof(CreateTimestampedPulseDO0Payload))]
    [XmlInclude(typeof(CreateTimestampedPulseDO1Payload))]
    [XmlInclude(typeof(CreateTimestampedPulseDO2Payload))]
    [XmlInclude(typeof(CreateTimestampedOutputSetPayload))]
    [XmlInclude(typeof(CreateTimestampedOutputClearPayload))]
    [XmlInclude(typeof(CreateTimestampedOutputTogglePayload))]
    [XmlInclude(typeof(CreateTimestampedOutputStatePayload))]
    [XmlInclude(typeof(CreateTimestampedConfigureAdcPayload))]
    [XmlInclude(typeof(CreateTimestampedAnalogDataPayload))]
    [XmlInclude(typeof(CreateTimestampedCommandsPayload))]
    [XmlInclude(typeof(CreateTimestampedEnableEventsPayload))]
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
    /// Represents an operator that creates a message payload
    /// that starts the sound index (if less than 32) or frequency (if greater or equal than 32).
    /// </summary>
    [DisplayName("PlaySoundOrFrequencyPayload")]
    [Description("Creates a message payload that starts the sound index (if less than 32) or frequency (if greater or equal than 32).")]
    public partial class CreatePlaySoundOrFrequencyPayload
    {
        /// <summary>
        /// Gets or sets the value that starts the sound index (if less than 32) or frequency (if greater or equal than 32).
        /// </summary>
        [Description("The value that starts the sound index (if less than 32) or frequency (if greater or equal than 32).")]
        public ushort PlaySoundOrFrequency { get; set; }

        /// <summary>
        /// Creates a message payload for the PlaySoundOrFrequency register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort GetPayload()
        {
            return PlaySoundOrFrequency;
        }

        /// <summary>
        /// Creates a message that starts the sound index (if less than 32) or frequency (if greater or equal than 32).
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the PlaySoundOrFrequency register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SoundCard.PlaySoundOrFrequency.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that starts the sound index (if less than 32) or frequency (if greater or equal than 32).
    /// </summary>
    [DisplayName("TimestampedPlaySoundOrFrequencyPayload")]
    [Description("Creates a timestamped message payload that starts the sound index (if less than 32) or frequency (if greater or equal than 32).")]
    public partial class CreateTimestampedPlaySoundOrFrequencyPayload : CreatePlaySoundOrFrequencyPayload
    {
        /// <summary>
        /// Creates a timestamped message that starts the sound index (if less than 32) or frequency (if greater or equal than 32).
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the PlaySoundOrFrequency register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SoundCard.PlaySoundOrFrequency.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that any value will stop the current sound.
    /// </summary>
    [DisplayName("StopPayload")]
    [Description("Creates a message payload that any value will stop the current sound.")]
    public partial class CreateStopPayload
    {
        /// <summary>
        /// Gets or sets the value that any value will stop the current sound.
        /// </summary>
        [Description("The value that any value will stop the current sound.")]
        public byte Stop { get; set; }

        /// <summary>
        /// Creates a message payload for the Stop register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public byte GetPayload()
        {
            return Stop;
        }

        /// <summary>
        /// Creates a message that any value will stop the current sound.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Stop register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SoundCard.Stop.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that any value will stop the current sound.
    /// </summary>
    [DisplayName("TimestampedStopPayload")]
    [Description("Creates a timestamped message payload that any value will stop the current sound.")]
    public partial class CreateTimestampedStopPayload : CreateStopPayload
    {
        /// <summary>
        /// Creates a timestamped message that any value will stop the current sound.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Stop register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SoundCard.Stop.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that configure left channel's attenuation (1 LSB is 0.1dB).
    /// </summary>
    [DisplayName("AttenuationLeftPayload")]
    [Description("Creates a message payload that configure left channel's attenuation (1 LSB is 0.1dB).")]
    public partial class CreateAttenuationLeftPayload
    {
        /// <summary>
        /// Gets or sets the value that configure left channel's attenuation (1 LSB is 0.1dB).
        /// </summary>
        [Description("The value that configure left channel's attenuation (1 LSB is 0.1dB).")]
        public ushort AttenuationLeft { get; set; }

        /// <summary>
        /// Creates a message payload for the AttenuationLeft register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort GetPayload()
        {
            return AttenuationLeft;
        }

        /// <summary>
        /// Creates a message that configure left channel's attenuation (1 LSB is 0.1dB).
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the AttenuationLeft register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SoundCard.AttenuationLeft.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that configure left channel's attenuation (1 LSB is 0.1dB).
    /// </summary>
    [DisplayName("TimestampedAttenuationLeftPayload")]
    [Description("Creates a timestamped message payload that configure left channel's attenuation (1 LSB is 0.1dB).")]
    public partial class CreateTimestampedAttenuationLeftPayload : CreateAttenuationLeftPayload
    {
        /// <summary>
        /// Creates a timestamped message that configure left channel's attenuation (1 LSB is 0.1dB).
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the AttenuationLeft register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SoundCard.AttenuationLeft.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that configure right channel's attenuation (1 LSB is 0.1dB).
    /// </summary>
    [DisplayName("AttenuationRightPayload")]
    [Description("Creates a message payload that configure right channel's attenuation (1 LSB is 0.1dB).")]
    public partial class CreateAttenuationRightPayload
    {
        /// <summary>
        /// Gets or sets the value that configure right channel's attenuation (1 LSB is 0.1dB).
        /// </summary>
        [Description("The value that configure right channel's attenuation (1 LSB is 0.1dB).")]
        public ushort AttenuationRight { get; set; }

        /// <summary>
        /// Creates a message payload for the AttenuationRight register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort GetPayload()
        {
            return AttenuationRight;
        }

        /// <summary>
        /// Creates a message that configure right channel's attenuation (1 LSB is 0.1dB).
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the AttenuationRight register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SoundCard.AttenuationRight.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that configure right channel's attenuation (1 LSB is 0.1dB).
    /// </summary>
    [DisplayName("TimestampedAttenuationRightPayload")]
    [Description("Creates a timestamped message payload that configure right channel's attenuation (1 LSB is 0.1dB).")]
    public partial class CreateTimestampedAttenuationRightPayload : CreateAttenuationRightPayload
    {
        /// <summary>
        /// Creates a timestamped message that configure right channel's attenuation (1 LSB is 0.1dB).
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the AttenuationRight register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SoundCard.AttenuationRight.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that configures both attenuation on right and left channels [Att R] [Att L].
    /// </summary>
    [DisplayName("AttenuationBothPayload")]
    [Description("Creates a message payload that configures both attenuation on right and left channels [Att R] [Att L].")]
    public partial class CreateAttenuationBothPayload
    {
        /// <summary>
        /// Gets or sets the value that configures both attenuation on right and left channels [Att R] [Att L].
        /// </summary>
        [Description("The value that configures both attenuation on right and left channels [Att R] [Att L].")]
        public ushort[] AttenuationBoth { get; set; }

        /// <summary>
        /// Creates a message payload for the AttenuationBoth register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort[] GetPayload()
        {
            return AttenuationBoth;
        }

        /// <summary>
        /// Creates a message that configures both attenuation on right and left channels [Att R] [Att L].
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the AttenuationBoth register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SoundCard.AttenuationBoth.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that configures both attenuation on right and left channels [Att R] [Att L].
    /// </summary>
    [DisplayName("TimestampedAttenuationBothPayload")]
    [Description("Creates a timestamped message payload that configures both attenuation on right and left channels [Att R] [Att L].")]
    public partial class CreateTimestampedAttenuationBothPayload : CreateAttenuationBothPayload
    {
        /// <summary>
        /// Creates a timestamped message that configures both attenuation on right and left channels [Att R] [Att L].
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the AttenuationBoth register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SoundCard.AttenuationBoth.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that configures attenuation and plays sound index [Att R] [Att L] [Index].
    /// </summary>
    [DisplayName("AttenuationAndPlaySoundOrFreqPayload")]
    [Description("Creates a message payload that configures attenuation and plays sound index [Att R] [Att L] [Index].")]
    public partial class CreateAttenuationAndPlaySoundOrFreqPayload
    {
        /// <summary>
        /// Gets or sets the value that configures attenuation and plays sound index [Att R] [Att L] [Index].
        /// </summary>
        [Description("The value that configures attenuation and plays sound index [Att R] [Att L] [Index].")]
        public ushort[] AttenuationAndPlaySoundOrFreq { get; set; }

        /// <summary>
        /// Creates a message payload for the AttenuationAndPlaySoundOrFreq register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort[] GetPayload()
        {
            return AttenuationAndPlaySoundOrFreq;
        }

        /// <summary>
        /// Creates a message that configures attenuation and plays sound index [Att R] [Att L] [Index].
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the AttenuationAndPlaySoundOrFreq register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SoundCard.AttenuationAndPlaySoundOrFreq.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that configures attenuation and plays sound index [Att R] [Att L] [Index].
    /// </summary>
    [DisplayName("TimestampedAttenuationAndPlaySoundOrFreqPayload")]
    [Description("Creates a timestamped message payload that configures attenuation and plays sound index [Att R] [Att L] [Index].")]
    public partial class CreateTimestampedAttenuationAndPlaySoundOrFreqPayload : CreateAttenuationAndPlaySoundOrFreqPayload
    {
        /// <summary>
        /// Creates a timestamped message that configures attenuation and plays sound index [Att R] [Att L] [Index].
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the AttenuationAndPlaySoundOrFreq register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SoundCard.AttenuationAndPlaySoundOrFreq.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that state of the digital inputs.
    /// </summary>
    [DisplayName("InputStatePayload")]
    [Description("Creates a message payload that state of the digital inputs.")]
    public partial class CreateInputStatePayload
    {
        /// <summary>
        /// Gets or sets the value that state of the digital inputs.
        /// </summary>
        [Description("The value that state of the digital inputs.")]
        public DigitalInputs InputState { get; set; }

        /// <summary>
        /// Creates a message payload for the InputState register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public DigitalInputs GetPayload()
        {
            return InputState;
        }

        /// <summary>
        /// Creates a message that state of the digital inputs.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the InputState register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SoundCard.InputState.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that state of the digital inputs.
    /// </summary>
    [DisplayName("TimestampedInputStatePayload")]
    [Description("Creates a timestamped message payload that state of the digital inputs.")]
    public partial class CreateTimestampedInputStatePayload : CreateInputStatePayload
    {
        /// <summary>
        /// Creates a timestamped message that state of the digital inputs.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the InputState register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SoundCard.InputState.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that configuration of the digital input 0 (DI0).
    /// </summary>
    [DisplayName("ConfigureDI0Payload")]
    [Description("Creates a message payload that configuration of the digital input 0 (DI0).")]
    public partial class CreateConfigureDI0Payload
    {
        /// <summary>
        /// Gets or sets the value that configuration of the digital input 0 (DI0).
        /// </summary>
        [Description("The value that configuration of the digital input 0 (DI0).")]
        public DigitalInputConfiguration ConfigureDI0 { get; set; }

        /// <summary>
        /// Creates a message payload for the ConfigureDI0 register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public DigitalInputConfiguration GetPayload()
        {
            return ConfigureDI0;
        }

        /// <summary>
        /// Creates a message that configuration of the digital input 0 (DI0).
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the ConfigureDI0 register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SoundCard.ConfigureDI0.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that configuration of the digital input 0 (DI0).
    /// </summary>
    [DisplayName("TimestampedConfigureDI0Payload")]
    [Description("Creates a timestamped message payload that configuration of the digital input 0 (DI0).")]
    public partial class CreateTimestampedConfigureDI0Payload : CreateConfigureDI0Payload
    {
        /// <summary>
        /// Creates a timestamped message that configuration of the digital input 0 (DI0).
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the ConfigureDI0 register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SoundCard.ConfigureDI0.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that configuration of the digital input 1 (DI1).
    /// </summary>
    [DisplayName("ConfigureDI1Payload")]
    [Description("Creates a message payload that configuration of the digital input 1 (DI1).")]
    public partial class CreateConfigureDI1Payload
    {
        /// <summary>
        /// Gets or sets the value that configuration of the digital input 1 (DI1).
        /// </summary>
        [Description("The value that configuration of the digital input 1 (DI1).")]
        public DigitalInputConfiguration ConfigureDI1 { get; set; }

        /// <summary>
        /// Creates a message payload for the ConfigureDI1 register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public DigitalInputConfiguration GetPayload()
        {
            return ConfigureDI1;
        }

        /// <summary>
        /// Creates a message that configuration of the digital input 1 (DI1).
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the ConfigureDI1 register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SoundCard.ConfigureDI1.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that configuration of the digital input 1 (DI1).
    /// </summary>
    [DisplayName("TimestampedConfigureDI1Payload")]
    [Description("Creates a timestamped message payload that configuration of the digital input 1 (DI1).")]
    public partial class CreateTimestampedConfigureDI1Payload : CreateConfigureDI1Payload
    {
        /// <summary>
        /// Creates a timestamped message that configuration of the digital input 1 (DI1).
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the ConfigureDI1 register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SoundCard.ConfigureDI1.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that configuration of the digital input 2 (DI2).
    /// </summary>
    [DisplayName("ConfigureDI2Payload")]
    [Description("Creates a message payload that configuration of the digital input 2 (DI2).")]
    public partial class CreateConfigureDI2Payload
    {
        /// <summary>
        /// Gets or sets the value that configuration of the digital input 2 (DI2).
        /// </summary>
        [Description("The value that configuration of the digital input 2 (DI2).")]
        public DigitalInputConfiguration ConfigureDI2 { get; set; }

        /// <summary>
        /// Creates a message payload for the ConfigureDI2 register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public DigitalInputConfiguration GetPayload()
        {
            return ConfigureDI2;
        }

        /// <summary>
        /// Creates a message that configuration of the digital input 2 (DI2).
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the ConfigureDI2 register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SoundCard.ConfigureDI2.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that configuration of the digital input 2 (DI2).
    /// </summary>
    [DisplayName("TimestampedConfigureDI2Payload")]
    [Description("Creates a timestamped message payload that configuration of the digital input 2 (DI2).")]
    public partial class CreateTimestampedConfigureDI2Payload : CreateConfigureDI2Payload
    {
        /// <summary>
        /// Creates a timestamped message that configuration of the digital input 2 (DI2).
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the ConfigureDI2 register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SoundCard.ConfigureDI2.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that specifies the sound index to be played when triggering DI0.
    /// </summary>
    [DisplayName("SoundIndexDI0Payload")]
    [Description("Creates a message payload that specifies the sound index to be played when triggering DI0.")]
    public partial class CreateSoundIndexDI0Payload
    {
        /// <summary>
        /// Gets or sets the value that specifies the sound index to be played when triggering DI0.
        /// </summary>
        [Description("The value that specifies the sound index to be played when triggering DI0.")]
        public byte SoundIndexDI0 { get; set; }

        /// <summary>
        /// Creates a message payload for the SoundIndexDI0 register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public byte GetPayload()
        {
            return SoundIndexDI0;
        }

        /// <summary>
        /// Creates a message that specifies the sound index to be played when triggering DI0.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the SoundIndexDI0 register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SoundCard.SoundIndexDI0.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that specifies the sound index to be played when triggering DI0.
    /// </summary>
    [DisplayName("TimestampedSoundIndexDI0Payload")]
    [Description("Creates a timestamped message payload that specifies the sound index to be played when triggering DI0.")]
    public partial class CreateTimestampedSoundIndexDI0Payload : CreateSoundIndexDI0Payload
    {
        /// <summary>
        /// Creates a timestamped message that specifies the sound index to be played when triggering DI0.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the SoundIndexDI0 register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SoundCard.SoundIndexDI0.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that specifies the sound index to be played when triggering DI1.
    /// </summary>
    [DisplayName("SoundIndexDI1Payload")]
    [Description("Creates a message payload that specifies the sound index to be played when triggering DI1.")]
    public partial class CreateSoundIndexDI1Payload
    {
        /// <summary>
        /// Gets or sets the value that specifies the sound index to be played when triggering DI1.
        /// </summary>
        [Description("The value that specifies the sound index to be played when triggering DI1.")]
        public byte SoundIndexDI1 { get; set; }

        /// <summary>
        /// Creates a message payload for the SoundIndexDI1 register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public byte GetPayload()
        {
            return SoundIndexDI1;
        }

        /// <summary>
        /// Creates a message that specifies the sound index to be played when triggering DI1.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the SoundIndexDI1 register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SoundCard.SoundIndexDI1.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that specifies the sound index to be played when triggering DI1.
    /// </summary>
    [DisplayName("TimestampedSoundIndexDI1Payload")]
    [Description("Creates a timestamped message payload that specifies the sound index to be played when triggering DI1.")]
    public partial class CreateTimestampedSoundIndexDI1Payload : CreateSoundIndexDI1Payload
    {
        /// <summary>
        /// Creates a timestamped message that specifies the sound index to be played when triggering DI1.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the SoundIndexDI1 register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SoundCard.SoundIndexDI1.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that specifies the sound index to be played when triggering DI2.
    /// </summary>
    [DisplayName("SoundIndexDI2Payload")]
    [Description("Creates a message payload that specifies the sound index to be played when triggering DI2.")]
    public partial class CreateSoundIndexDI2Payload
    {
        /// <summary>
        /// Gets or sets the value that specifies the sound index to be played when triggering DI2.
        /// </summary>
        [Description("The value that specifies the sound index to be played when triggering DI2.")]
        public byte SoundIndexDI2 { get; set; }

        /// <summary>
        /// Creates a message payload for the SoundIndexDI2 register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public byte GetPayload()
        {
            return SoundIndexDI2;
        }

        /// <summary>
        /// Creates a message that specifies the sound index to be played when triggering DI2.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the SoundIndexDI2 register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SoundCard.SoundIndexDI2.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that specifies the sound index to be played when triggering DI2.
    /// </summary>
    [DisplayName("TimestampedSoundIndexDI2Payload")]
    [Description("Creates a timestamped message payload that specifies the sound index to be played when triggering DI2.")]
    public partial class CreateTimestampedSoundIndexDI2Payload : CreateSoundIndexDI2Payload
    {
        /// <summary>
        /// Creates a timestamped message that specifies the sound index to be played when triggering DI2.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the SoundIndexDI2 register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SoundCard.SoundIndexDI2.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that specifies the sound frequency to be played when triggering DI0.
    /// </summary>
    [DisplayName("FrequencyDI0Payload")]
    [Description("Creates a message payload that specifies the sound frequency to be played when triggering DI0.")]
    public partial class CreateFrequencyDI0Payload
    {
        /// <summary>
        /// Gets or sets the value that specifies the sound frequency to be played when triggering DI0.
        /// </summary>
        [Description("The value that specifies the sound frequency to be played when triggering DI0.")]
        public ushort FrequencyDI0 { get; set; }

        /// <summary>
        /// Creates a message payload for the FrequencyDI0 register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort GetPayload()
        {
            return FrequencyDI0;
        }

        /// <summary>
        /// Creates a message that specifies the sound frequency to be played when triggering DI0.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the FrequencyDI0 register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SoundCard.FrequencyDI0.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that specifies the sound frequency to be played when triggering DI0.
    /// </summary>
    [DisplayName("TimestampedFrequencyDI0Payload")]
    [Description("Creates a timestamped message payload that specifies the sound frequency to be played when triggering DI0.")]
    public partial class CreateTimestampedFrequencyDI0Payload : CreateFrequencyDI0Payload
    {
        /// <summary>
        /// Creates a timestamped message that specifies the sound frequency to be played when triggering DI0.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the FrequencyDI0 register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SoundCard.FrequencyDI0.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that specifies the sound frequency to be played when triggering DI1.
    /// </summary>
    [DisplayName("FrequencyDI1Payload")]
    [Description("Creates a message payload that specifies the sound frequency to be played when triggering DI1.")]
    public partial class CreateFrequencyDI1Payload
    {
        /// <summary>
        /// Gets or sets the value that specifies the sound frequency to be played when triggering DI1.
        /// </summary>
        [Description("The value that specifies the sound frequency to be played when triggering DI1.")]
        public ushort FrequencyDI1 { get; set; }

        /// <summary>
        /// Creates a message payload for the FrequencyDI1 register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort GetPayload()
        {
            return FrequencyDI1;
        }

        /// <summary>
        /// Creates a message that specifies the sound frequency to be played when triggering DI1.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the FrequencyDI1 register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SoundCard.FrequencyDI1.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that specifies the sound frequency to be played when triggering DI1.
    /// </summary>
    [DisplayName("TimestampedFrequencyDI1Payload")]
    [Description("Creates a timestamped message payload that specifies the sound frequency to be played when triggering DI1.")]
    public partial class CreateTimestampedFrequencyDI1Payload : CreateFrequencyDI1Payload
    {
        /// <summary>
        /// Creates a timestamped message that specifies the sound frequency to be played when triggering DI1.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the FrequencyDI1 register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SoundCard.FrequencyDI1.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that specifies the sound frequency to be played when triggering DI2.
    /// </summary>
    [DisplayName("FrequencyDI2Payload")]
    [Description("Creates a message payload that specifies the sound frequency to be played when triggering DI2.")]
    public partial class CreateFrequencyDI2Payload
    {
        /// <summary>
        /// Gets or sets the value that specifies the sound frequency to be played when triggering DI2.
        /// </summary>
        [Description("The value that specifies the sound frequency to be played when triggering DI2.")]
        public ushort FrequencyDI2 { get; set; }

        /// <summary>
        /// Creates a message payload for the FrequencyDI2 register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort GetPayload()
        {
            return FrequencyDI2;
        }

        /// <summary>
        /// Creates a message that specifies the sound frequency to be played when triggering DI2.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the FrequencyDI2 register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SoundCard.FrequencyDI2.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that specifies the sound frequency to be played when triggering DI2.
    /// </summary>
    [DisplayName("TimestampedFrequencyDI2Payload")]
    [Description("Creates a timestamped message payload that specifies the sound frequency to be played when triggering DI2.")]
    public partial class CreateTimestampedFrequencyDI2Payload : CreateFrequencyDI2Payload
    {
        /// <summary>
        /// Creates a timestamped message that specifies the sound frequency to be played when triggering DI2.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the FrequencyDI2 register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SoundCard.FrequencyDI2.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that left channel's attenuation (1 LSB is 0.5dB) when triggering DI0.
    /// </summary>
    [DisplayName("AttenuationLeftDI0Payload")]
    [Description("Creates a message payload that left channel's attenuation (1 LSB is 0.5dB) when triggering DI0.")]
    public partial class CreateAttenuationLeftDI0Payload
    {
        /// <summary>
        /// Gets or sets the value that left channel's attenuation (1 LSB is 0.5dB) when triggering DI0.
        /// </summary>
        [Description("The value that left channel's attenuation (1 LSB is 0.5dB) when triggering DI0.")]
        public ushort AttenuationLeftDI0 { get; set; }

        /// <summary>
        /// Creates a message payload for the AttenuationLeftDI0 register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort GetPayload()
        {
            return AttenuationLeftDI0;
        }

        /// <summary>
        /// Creates a message that left channel's attenuation (1 LSB is 0.5dB) when triggering DI0.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the AttenuationLeftDI0 register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SoundCard.AttenuationLeftDI0.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that left channel's attenuation (1 LSB is 0.5dB) when triggering DI0.
    /// </summary>
    [DisplayName("TimestampedAttenuationLeftDI0Payload")]
    [Description("Creates a timestamped message payload that left channel's attenuation (1 LSB is 0.5dB) when triggering DI0.")]
    public partial class CreateTimestampedAttenuationLeftDI0Payload : CreateAttenuationLeftDI0Payload
    {
        /// <summary>
        /// Creates a timestamped message that left channel's attenuation (1 LSB is 0.5dB) when triggering DI0.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the AttenuationLeftDI0 register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SoundCard.AttenuationLeftDI0.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that left channel's attenuation (1 LSB is 0.5dB) when triggering DI1.
    /// </summary>
    [DisplayName("AttenuationLeftDI1Payload")]
    [Description("Creates a message payload that left channel's attenuation (1 LSB is 0.5dB) when triggering DI1.")]
    public partial class CreateAttenuationLeftDI1Payload
    {
        /// <summary>
        /// Gets or sets the value that left channel's attenuation (1 LSB is 0.5dB) when triggering DI1.
        /// </summary>
        [Description("The value that left channel's attenuation (1 LSB is 0.5dB) when triggering DI1.")]
        public ushort AttenuationLeftDI1 { get; set; }

        /// <summary>
        /// Creates a message payload for the AttenuationLeftDI1 register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort GetPayload()
        {
            return AttenuationLeftDI1;
        }

        /// <summary>
        /// Creates a message that left channel's attenuation (1 LSB is 0.5dB) when triggering DI1.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the AttenuationLeftDI1 register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SoundCard.AttenuationLeftDI1.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that left channel's attenuation (1 LSB is 0.5dB) when triggering DI1.
    /// </summary>
    [DisplayName("TimestampedAttenuationLeftDI1Payload")]
    [Description("Creates a timestamped message payload that left channel's attenuation (1 LSB is 0.5dB) when triggering DI1.")]
    public partial class CreateTimestampedAttenuationLeftDI1Payload : CreateAttenuationLeftDI1Payload
    {
        /// <summary>
        /// Creates a timestamped message that left channel's attenuation (1 LSB is 0.5dB) when triggering DI1.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the AttenuationLeftDI1 register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SoundCard.AttenuationLeftDI1.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that left channel's attenuation (1 LSB is 0.5dB) when triggering DI2.
    /// </summary>
    [DisplayName("AttenuationLeftDI2Payload")]
    [Description("Creates a message payload that left channel's attenuation (1 LSB is 0.5dB) when triggering DI2.")]
    public partial class CreateAttenuationLeftDI2Payload
    {
        /// <summary>
        /// Gets or sets the value that left channel's attenuation (1 LSB is 0.5dB) when triggering DI2.
        /// </summary>
        [Description("The value that left channel's attenuation (1 LSB is 0.5dB) when triggering DI2.")]
        public ushort AttenuationLeftDI2 { get; set; }

        /// <summary>
        /// Creates a message payload for the AttenuationLeftDI2 register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort GetPayload()
        {
            return AttenuationLeftDI2;
        }

        /// <summary>
        /// Creates a message that left channel's attenuation (1 LSB is 0.5dB) when triggering DI2.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the AttenuationLeftDI2 register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SoundCard.AttenuationLeftDI2.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that left channel's attenuation (1 LSB is 0.5dB) when triggering DI2.
    /// </summary>
    [DisplayName("TimestampedAttenuationLeftDI2Payload")]
    [Description("Creates a timestamped message payload that left channel's attenuation (1 LSB is 0.5dB) when triggering DI2.")]
    public partial class CreateTimestampedAttenuationLeftDI2Payload : CreateAttenuationLeftDI2Payload
    {
        /// <summary>
        /// Creates a timestamped message that left channel's attenuation (1 LSB is 0.5dB) when triggering DI2.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the AttenuationLeftDI2 register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SoundCard.AttenuationLeftDI2.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that right channel's attenuation (1 LSB is 0.5dB) when triggering DI0.
    /// </summary>
    [DisplayName("AttenuationRightDI0Payload")]
    [Description("Creates a message payload that right channel's attenuation (1 LSB is 0.5dB) when triggering DI0.")]
    public partial class CreateAttenuationRightDI0Payload
    {
        /// <summary>
        /// Gets or sets the value that right channel's attenuation (1 LSB is 0.5dB) when triggering DI0.
        /// </summary>
        [Description("The value that right channel's attenuation (1 LSB is 0.5dB) when triggering DI0.")]
        public ushort AttenuationRightDI0 { get; set; }

        /// <summary>
        /// Creates a message payload for the AttenuationRightDI0 register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort GetPayload()
        {
            return AttenuationRightDI0;
        }

        /// <summary>
        /// Creates a message that right channel's attenuation (1 LSB is 0.5dB) when triggering DI0.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the AttenuationRightDI0 register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SoundCard.AttenuationRightDI0.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that right channel's attenuation (1 LSB is 0.5dB) when triggering DI0.
    /// </summary>
    [DisplayName("TimestampedAttenuationRightDI0Payload")]
    [Description("Creates a timestamped message payload that right channel's attenuation (1 LSB is 0.5dB) when triggering DI0.")]
    public partial class CreateTimestampedAttenuationRightDI0Payload : CreateAttenuationRightDI0Payload
    {
        /// <summary>
        /// Creates a timestamped message that right channel's attenuation (1 LSB is 0.5dB) when triggering DI0.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the AttenuationRightDI0 register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SoundCard.AttenuationRightDI0.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that right channel's attenuation (1 LSB is 0.5dB) when triggering DI1.
    /// </summary>
    [DisplayName("AttenuationRightDI1Payload")]
    [Description("Creates a message payload that right channel's attenuation (1 LSB is 0.5dB) when triggering DI1.")]
    public partial class CreateAttenuationRightDI1Payload
    {
        /// <summary>
        /// Gets or sets the value that right channel's attenuation (1 LSB is 0.5dB) when triggering DI1.
        /// </summary>
        [Description("The value that right channel's attenuation (1 LSB is 0.5dB) when triggering DI1.")]
        public ushort AttenuationRightDI1 { get; set; }

        /// <summary>
        /// Creates a message payload for the AttenuationRightDI1 register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort GetPayload()
        {
            return AttenuationRightDI1;
        }

        /// <summary>
        /// Creates a message that right channel's attenuation (1 LSB is 0.5dB) when triggering DI1.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the AttenuationRightDI1 register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SoundCard.AttenuationRightDI1.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that right channel's attenuation (1 LSB is 0.5dB) when triggering DI1.
    /// </summary>
    [DisplayName("TimestampedAttenuationRightDI1Payload")]
    [Description("Creates a timestamped message payload that right channel's attenuation (1 LSB is 0.5dB) when triggering DI1.")]
    public partial class CreateTimestampedAttenuationRightDI1Payload : CreateAttenuationRightDI1Payload
    {
        /// <summary>
        /// Creates a timestamped message that right channel's attenuation (1 LSB is 0.5dB) when triggering DI1.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the AttenuationRightDI1 register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SoundCard.AttenuationRightDI1.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that right channel's attenuation (1 LSB is 0.5dB) when triggering DI2.
    /// </summary>
    [DisplayName("AttenuationRightDI2Payload")]
    [Description("Creates a message payload that right channel's attenuation (1 LSB is 0.5dB) when triggering DI2.")]
    public partial class CreateAttenuationRightDI2Payload
    {
        /// <summary>
        /// Gets or sets the value that right channel's attenuation (1 LSB is 0.5dB) when triggering DI2.
        /// </summary>
        [Description("The value that right channel's attenuation (1 LSB is 0.5dB) when triggering DI2.")]
        public ushort AttenuationRightDI2 { get; set; }

        /// <summary>
        /// Creates a message payload for the AttenuationRightDI2 register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort GetPayload()
        {
            return AttenuationRightDI2;
        }

        /// <summary>
        /// Creates a message that right channel's attenuation (1 LSB is 0.5dB) when triggering DI2.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the AttenuationRightDI2 register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SoundCard.AttenuationRightDI2.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that right channel's attenuation (1 LSB is 0.5dB) when triggering DI2.
    /// </summary>
    [DisplayName("TimestampedAttenuationRightDI2Payload")]
    [Description("Creates a timestamped message payload that right channel's attenuation (1 LSB is 0.5dB) when triggering DI2.")]
    public partial class CreateTimestampedAttenuationRightDI2Payload : CreateAttenuationRightDI2Payload
    {
        /// <summary>
        /// Creates a timestamped message that right channel's attenuation (1 LSB is 0.5dB) when triggering DI2.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the AttenuationRightDI2 register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SoundCard.AttenuationRightDI2.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that sound index and attenuation to be played when triggering DI0 [Att R] [Att L] [Index].
    /// </summary>
    [DisplayName("AttenuationAndSoundIndexDI0Payload")]
    [Description("Creates a message payload that sound index and attenuation to be played when triggering DI0 [Att R] [Att L] [Index].")]
    public partial class CreateAttenuationAndSoundIndexDI0Payload
    {
        /// <summary>
        /// Gets or sets the value that sound index and attenuation to be played when triggering DI0 [Att R] [Att L] [Index].
        /// </summary>
        [Description("The value that sound index and attenuation to be played when triggering DI0 [Att R] [Att L] [Index].")]
        public ushort[] AttenuationAndSoundIndexDI0 { get; set; }

        /// <summary>
        /// Creates a message payload for the AttenuationAndSoundIndexDI0 register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort[] GetPayload()
        {
            return AttenuationAndSoundIndexDI0;
        }

        /// <summary>
        /// Creates a message that sound index and attenuation to be played when triggering DI0 [Att R] [Att L] [Index].
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the AttenuationAndSoundIndexDI0 register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SoundCard.AttenuationAndSoundIndexDI0.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that sound index and attenuation to be played when triggering DI0 [Att R] [Att L] [Index].
    /// </summary>
    [DisplayName("TimestampedAttenuationAndSoundIndexDI0Payload")]
    [Description("Creates a timestamped message payload that sound index and attenuation to be played when triggering DI0 [Att R] [Att L] [Index].")]
    public partial class CreateTimestampedAttenuationAndSoundIndexDI0Payload : CreateAttenuationAndSoundIndexDI0Payload
    {
        /// <summary>
        /// Creates a timestamped message that sound index and attenuation to be played when triggering DI0 [Att R] [Att L] [Index].
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the AttenuationAndSoundIndexDI0 register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SoundCard.AttenuationAndSoundIndexDI0.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that sound index and attenuation to be played when triggering DI1 [Att R] [Att L] [Index].
    /// </summary>
    [DisplayName("AttenuationAndSoundIndexDI1Payload")]
    [Description("Creates a message payload that sound index and attenuation to be played when triggering DI1 [Att R] [Att L] [Index].")]
    public partial class CreateAttenuationAndSoundIndexDI1Payload
    {
        /// <summary>
        /// Gets or sets the value that sound index and attenuation to be played when triggering DI1 [Att R] [Att L] [Index].
        /// </summary>
        [Description("The value that sound index and attenuation to be played when triggering DI1 [Att R] [Att L] [Index].")]
        public ushort[] AttenuationAndSoundIndexDI1 { get; set; }

        /// <summary>
        /// Creates a message payload for the AttenuationAndSoundIndexDI1 register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort[] GetPayload()
        {
            return AttenuationAndSoundIndexDI1;
        }

        /// <summary>
        /// Creates a message that sound index and attenuation to be played when triggering DI1 [Att R] [Att L] [Index].
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the AttenuationAndSoundIndexDI1 register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SoundCard.AttenuationAndSoundIndexDI1.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that sound index and attenuation to be played when triggering DI1 [Att R] [Att L] [Index].
    /// </summary>
    [DisplayName("TimestampedAttenuationAndSoundIndexDI1Payload")]
    [Description("Creates a timestamped message payload that sound index and attenuation to be played when triggering DI1 [Att R] [Att L] [Index].")]
    public partial class CreateTimestampedAttenuationAndSoundIndexDI1Payload : CreateAttenuationAndSoundIndexDI1Payload
    {
        /// <summary>
        /// Creates a timestamped message that sound index and attenuation to be played when triggering DI1 [Att R] [Att L] [Index].
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the AttenuationAndSoundIndexDI1 register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SoundCard.AttenuationAndSoundIndexDI1.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that sound index and attenuation to be played when triggering DI2 [Att R] [Att L] [Index].
    /// </summary>
    [DisplayName("AttenuationAndSoundIndexDI2Payload")]
    [Description("Creates a message payload that sound index and attenuation to be played when triggering DI2 [Att R] [Att L] [Index].")]
    public partial class CreateAttenuationAndSoundIndexDI2Payload
    {
        /// <summary>
        /// Gets or sets the value that sound index and attenuation to be played when triggering DI2 [Att R] [Att L] [Index].
        /// </summary>
        [Description("The value that sound index and attenuation to be played when triggering DI2 [Att R] [Att L] [Index].")]
        public ushort[] AttenuationAndSoundIndexDI2 { get; set; }

        /// <summary>
        /// Creates a message payload for the AttenuationAndSoundIndexDI2 register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort[] GetPayload()
        {
            return AttenuationAndSoundIndexDI2;
        }

        /// <summary>
        /// Creates a message that sound index and attenuation to be played when triggering DI2 [Att R] [Att L] [Index].
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the AttenuationAndSoundIndexDI2 register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SoundCard.AttenuationAndSoundIndexDI2.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that sound index and attenuation to be played when triggering DI2 [Att R] [Att L] [Index].
    /// </summary>
    [DisplayName("TimestampedAttenuationAndSoundIndexDI2Payload")]
    [Description("Creates a timestamped message payload that sound index and attenuation to be played when triggering DI2 [Att R] [Att L] [Index].")]
    public partial class CreateTimestampedAttenuationAndSoundIndexDI2Payload : CreateAttenuationAndSoundIndexDI2Payload
    {
        /// <summary>
        /// Creates a timestamped message that sound index and attenuation to be played when triggering DI2 [Att R] [Att L] [Index].
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the AttenuationAndSoundIndexDI2 register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SoundCard.AttenuationAndSoundIndexDI2.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that sound index and attenuation to be played when triggering DI0 [Att BOTH] [Frequency].
    /// </summary>
    [DisplayName("AttenuationAndFrequencyDI0Payload")]
    [Description("Creates a message payload that sound index and attenuation to be played when triggering DI0 [Att BOTH] [Frequency].")]
    public partial class CreateAttenuationAndFrequencyDI0Payload
    {
        /// <summary>
        /// Gets or sets the value that sound index and attenuation to be played when triggering DI0 [Att BOTH] [Frequency].
        /// </summary>
        [Description("The value that sound index and attenuation to be played when triggering DI0 [Att BOTH] [Frequency].")]
        public ushort[] AttenuationAndFrequencyDI0 { get; set; }

        /// <summary>
        /// Creates a message payload for the AttenuationAndFrequencyDI0 register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort[] GetPayload()
        {
            return AttenuationAndFrequencyDI0;
        }

        /// <summary>
        /// Creates a message that sound index and attenuation to be played when triggering DI0 [Att BOTH] [Frequency].
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the AttenuationAndFrequencyDI0 register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SoundCard.AttenuationAndFrequencyDI0.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that sound index and attenuation to be played when triggering DI0 [Att BOTH] [Frequency].
    /// </summary>
    [DisplayName("TimestampedAttenuationAndFrequencyDI0Payload")]
    [Description("Creates a timestamped message payload that sound index and attenuation to be played when triggering DI0 [Att BOTH] [Frequency].")]
    public partial class CreateTimestampedAttenuationAndFrequencyDI0Payload : CreateAttenuationAndFrequencyDI0Payload
    {
        /// <summary>
        /// Creates a timestamped message that sound index and attenuation to be played when triggering DI0 [Att BOTH] [Frequency].
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the AttenuationAndFrequencyDI0 register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SoundCard.AttenuationAndFrequencyDI0.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that sound index and attenuation to be played when triggering DI1 [Att BOTH] [Frequency].
    /// </summary>
    [DisplayName("AttenuationAndFrequencyDI1Payload")]
    [Description("Creates a message payload that sound index and attenuation to be played when triggering DI1 [Att BOTH] [Frequency].")]
    public partial class CreateAttenuationAndFrequencyDI1Payload
    {
        /// <summary>
        /// Gets or sets the value that sound index and attenuation to be played when triggering DI1 [Att BOTH] [Frequency].
        /// </summary>
        [Description("The value that sound index and attenuation to be played when triggering DI1 [Att BOTH] [Frequency].")]
        public ushort[] AttenuationAndFrequencyDI1 { get; set; }

        /// <summary>
        /// Creates a message payload for the AttenuationAndFrequencyDI1 register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort[] GetPayload()
        {
            return AttenuationAndFrequencyDI1;
        }

        /// <summary>
        /// Creates a message that sound index and attenuation to be played when triggering DI1 [Att BOTH] [Frequency].
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the AttenuationAndFrequencyDI1 register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SoundCard.AttenuationAndFrequencyDI1.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that sound index and attenuation to be played when triggering DI1 [Att BOTH] [Frequency].
    /// </summary>
    [DisplayName("TimestampedAttenuationAndFrequencyDI1Payload")]
    [Description("Creates a timestamped message payload that sound index and attenuation to be played when triggering DI1 [Att BOTH] [Frequency].")]
    public partial class CreateTimestampedAttenuationAndFrequencyDI1Payload : CreateAttenuationAndFrequencyDI1Payload
    {
        /// <summary>
        /// Creates a timestamped message that sound index and attenuation to be played when triggering DI1 [Att BOTH] [Frequency].
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the AttenuationAndFrequencyDI1 register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SoundCard.AttenuationAndFrequencyDI1.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that sound index and attenuation to be played when triggering DI2 [Att BOTH] [Frequency].
    /// </summary>
    [DisplayName("AttenuationAndFrequencyDI2Payload")]
    [Description("Creates a message payload that sound index and attenuation to be played when triggering DI2 [Att BOTH] [Frequency].")]
    public partial class CreateAttenuationAndFrequencyDI2Payload
    {
        /// <summary>
        /// Gets or sets the value that sound index and attenuation to be played when triggering DI2 [Att BOTH] [Frequency].
        /// </summary>
        [Description("The value that sound index and attenuation to be played when triggering DI2 [Att BOTH] [Frequency].")]
        public ushort[] AttenuationAndFrequencyDI2 { get; set; }

        /// <summary>
        /// Creates a message payload for the AttenuationAndFrequencyDI2 register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort[] GetPayload()
        {
            return AttenuationAndFrequencyDI2;
        }

        /// <summary>
        /// Creates a message that sound index and attenuation to be played when triggering DI2 [Att BOTH] [Frequency].
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the AttenuationAndFrequencyDI2 register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SoundCard.AttenuationAndFrequencyDI2.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that sound index and attenuation to be played when triggering DI2 [Att BOTH] [Frequency].
    /// </summary>
    [DisplayName("TimestampedAttenuationAndFrequencyDI2Payload")]
    [Description("Creates a timestamped message payload that sound index and attenuation to be played when triggering DI2 [Att BOTH] [Frequency].")]
    public partial class CreateTimestampedAttenuationAndFrequencyDI2Payload : CreateAttenuationAndFrequencyDI2Payload
    {
        /// <summary>
        /// Creates a timestamped message that sound index and attenuation to be played when triggering DI2 [Att BOTH] [Frequency].
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the AttenuationAndFrequencyDI2 register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SoundCard.AttenuationAndFrequencyDI2.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that configuration of the digital output 0 (DO0).
    /// </summary>
    [DisplayName("ConfigureDO0Payload")]
    [Description("Creates a message payload that configuration of the digital output 0 (DO0).")]
    public partial class CreateConfigureDO0Payload
    {
        /// <summary>
        /// Gets or sets the value that configuration of the digital output 0 (DO0).
        /// </summary>
        [Description("The value that configuration of the digital output 0 (DO0).")]
        public DigitalOutputConfiguration ConfigureDO0 { get; set; }

        /// <summary>
        /// Creates a message payload for the ConfigureDO0 register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public DigitalOutputConfiguration GetPayload()
        {
            return ConfigureDO0;
        }

        /// <summary>
        /// Creates a message that configuration of the digital output 0 (DO0).
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the ConfigureDO0 register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SoundCard.ConfigureDO0.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that configuration of the digital output 0 (DO0).
    /// </summary>
    [DisplayName("TimestampedConfigureDO0Payload")]
    [Description("Creates a timestamped message payload that configuration of the digital output 0 (DO0).")]
    public partial class CreateTimestampedConfigureDO0Payload : CreateConfigureDO0Payload
    {
        /// <summary>
        /// Creates a timestamped message that configuration of the digital output 0 (DO0).
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the ConfigureDO0 register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SoundCard.ConfigureDO0.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that configuration of the digital output 1 (DO1).
    /// </summary>
    [DisplayName("ConfigureDO1Payload")]
    [Description("Creates a message payload that configuration of the digital output 1 (DO1).")]
    public partial class CreateConfigureDO1Payload
    {
        /// <summary>
        /// Gets or sets the value that configuration of the digital output 1 (DO1).
        /// </summary>
        [Description("The value that configuration of the digital output 1 (DO1).")]
        public DigitalOutputConfiguration ConfigureDO1 { get; set; }

        /// <summary>
        /// Creates a message payload for the ConfigureDO1 register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public DigitalOutputConfiguration GetPayload()
        {
            return ConfigureDO1;
        }

        /// <summary>
        /// Creates a message that configuration of the digital output 1 (DO1).
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the ConfigureDO1 register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SoundCard.ConfigureDO1.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that configuration of the digital output 1 (DO1).
    /// </summary>
    [DisplayName("TimestampedConfigureDO1Payload")]
    [Description("Creates a timestamped message payload that configuration of the digital output 1 (DO1).")]
    public partial class CreateTimestampedConfigureDO1Payload : CreateConfigureDO1Payload
    {
        /// <summary>
        /// Creates a timestamped message that configuration of the digital output 1 (DO1).
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the ConfigureDO1 register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SoundCard.ConfigureDO1.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that configuration of the digital output 2 (DO2.
    /// </summary>
    [DisplayName("ConfigureDO2Payload")]
    [Description("Creates a message payload that configuration of the digital output 2 (DO2.")]
    public partial class CreateConfigureDO2Payload
    {
        /// <summary>
        /// Gets or sets the value that configuration of the digital output 2 (DO2.
        /// </summary>
        [Description("The value that configuration of the digital output 2 (DO2.")]
        public DigitalOutputConfiguration ConfigureDO2 { get; set; }

        /// <summary>
        /// Creates a message payload for the ConfigureDO2 register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public DigitalOutputConfiguration GetPayload()
        {
            return ConfigureDO2;
        }

        /// <summary>
        /// Creates a message that configuration of the digital output 2 (DO2.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the ConfigureDO2 register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SoundCard.ConfigureDO2.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that configuration of the digital output 2 (DO2.
    /// </summary>
    [DisplayName("TimestampedConfigureDO2Payload")]
    [Description("Creates a timestamped message payload that configuration of the digital output 2 (DO2.")]
    public partial class CreateTimestampedConfigureDO2Payload : CreateConfigureDO2Payload
    {
        /// <summary>
        /// Creates a timestamped message that configuration of the digital output 2 (DO2.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the ConfigureDO2 register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SoundCard.ConfigureDO2.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that pulse for the digital output 0 (DO0).
    /// </summary>
    [DisplayName("PulseDO0Payload")]
    [Description("Creates a message payload that pulse for the digital output 0 (DO0).")]
    public partial class CreatePulseDO0Payload
    {
        /// <summary>
        /// Gets or sets the value that pulse for the digital output 0 (DO0).
        /// </summary>
        [Range(min: 1, max: 255)]
        [Editor(DesignTypes.NumericUpDownEditor, DesignTypes.UITypeEditor)]
        [Description("The value that pulse for the digital output 0 (DO0).")]
        public byte PulseDO0 { get; set; } = 1;

        /// <summary>
        /// Creates a message payload for the PulseDO0 register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public byte GetPayload()
        {
            return PulseDO0;
        }

        /// <summary>
        /// Creates a message that pulse for the digital output 0 (DO0).
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the PulseDO0 register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SoundCard.PulseDO0.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that pulse for the digital output 0 (DO0).
    /// </summary>
    [DisplayName("TimestampedPulseDO0Payload")]
    [Description("Creates a timestamped message payload that pulse for the digital output 0 (DO0).")]
    public partial class CreateTimestampedPulseDO0Payload : CreatePulseDO0Payload
    {
        /// <summary>
        /// Creates a timestamped message that pulse for the digital output 0 (DO0).
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the PulseDO0 register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SoundCard.PulseDO0.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that pulse for the digital output 1 (DO1).
    /// </summary>
    [DisplayName("PulseDO1Payload")]
    [Description("Creates a message payload that pulse for the digital output 1 (DO1).")]
    public partial class CreatePulseDO1Payload
    {
        /// <summary>
        /// Gets or sets the value that pulse for the digital output 1 (DO1).
        /// </summary>
        [Range(min: 1, max: 255)]
        [Editor(DesignTypes.NumericUpDownEditor, DesignTypes.UITypeEditor)]
        [Description("The value that pulse for the digital output 1 (DO1).")]
        public byte PulseDO1 { get; set; } = 1;

        /// <summary>
        /// Creates a message payload for the PulseDO1 register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public byte GetPayload()
        {
            return PulseDO1;
        }

        /// <summary>
        /// Creates a message that pulse for the digital output 1 (DO1).
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the PulseDO1 register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SoundCard.PulseDO1.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that pulse for the digital output 1 (DO1).
    /// </summary>
    [DisplayName("TimestampedPulseDO1Payload")]
    [Description("Creates a timestamped message payload that pulse for the digital output 1 (DO1).")]
    public partial class CreateTimestampedPulseDO1Payload : CreatePulseDO1Payload
    {
        /// <summary>
        /// Creates a timestamped message that pulse for the digital output 1 (DO1).
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the PulseDO1 register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SoundCard.PulseDO1.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that pulse for the digital output 2 (DO2).
    /// </summary>
    [DisplayName("PulseDO2Payload")]
    [Description("Creates a message payload that pulse for the digital output 2 (DO2).")]
    public partial class CreatePulseDO2Payload
    {
        /// <summary>
        /// Gets or sets the value that pulse for the digital output 2 (DO2).
        /// </summary>
        [Range(min: 1, max: 255)]
        [Editor(DesignTypes.NumericUpDownEditor, DesignTypes.UITypeEditor)]
        [Description("The value that pulse for the digital output 2 (DO2).")]
        public byte PulseDO2 { get; set; } = 1;

        /// <summary>
        /// Creates a message payload for the PulseDO2 register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public byte GetPayload()
        {
            return PulseDO2;
        }

        /// <summary>
        /// Creates a message that pulse for the digital output 2 (DO2).
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the PulseDO2 register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SoundCard.PulseDO2.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that pulse for the digital output 2 (DO2).
    /// </summary>
    [DisplayName("TimestampedPulseDO2Payload")]
    [Description("Creates a timestamped message payload that pulse for the digital output 2 (DO2).")]
    public partial class CreateTimestampedPulseDO2Payload : CreatePulseDO2Payload
    {
        /// <summary>
        /// Creates a timestamped message that pulse for the digital output 2 (DO2).
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the PulseDO2 register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SoundCard.PulseDO2.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that set the specified digital output lines.
    /// </summary>
    [DisplayName("OutputSetPayload")]
    [Description("Creates a message payload that set the specified digital output lines.")]
    public partial class CreateOutputSetPayload
    {
        /// <summary>
        /// Gets or sets the value that set the specified digital output lines.
        /// </summary>
        [Description("The value that set the specified digital output lines.")]
        public DigitalOutputs OutputSet { get; set; }

        /// <summary>
        /// Creates a message payload for the OutputSet register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public DigitalOutputs GetPayload()
        {
            return OutputSet;
        }

        /// <summary>
        /// Creates a message that set the specified digital output lines.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the OutputSet register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SoundCard.OutputSet.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that set the specified digital output lines.
    /// </summary>
    [DisplayName("TimestampedOutputSetPayload")]
    [Description("Creates a timestamped message payload that set the specified digital output lines.")]
    public partial class CreateTimestampedOutputSetPayload : CreateOutputSetPayload
    {
        /// <summary>
        /// Creates a timestamped message that set the specified digital output lines.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the OutputSet register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SoundCard.OutputSet.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that clear the specified digital output lines.
    /// </summary>
    [DisplayName("OutputClearPayload")]
    [Description("Creates a message payload that clear the specified digital output lines.")]
    public partial class CreateOutputClearPayload
    {
        /// <summary>
        /// Gets or sets the value that clear the specified digital output lines.
        /// </summary>
        [Description("The value that clear the specified digital output lines.")]
        public DigitalOutputs OutputClear { get; set; }

        /// <summary>
        /// Creates a message payload for the OutputClear register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public DigitalOutputs GetPayload()
        {
            return OutputClear;
        }

        /// <summary>
        /// Creates a message that clear the specified digital output lines.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the OutputClear register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SoundCard.OutputClear.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that clear the specified digital output lines.
    /// </summary>
    [DisplayName("TimestampedOutputClearPayload")]
    [Description("Creates a timestamped message payload that clear the specified digital output lines.")]
    public partial class CreateTimestampedOutputClearPayload : CreateOutputClearPayload
    {
        /// <summary>
        /// Creates a timestamped message that clear the specified digital output lines.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the OutputClear register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SoundCard.OutputClear.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that toggle the specified digital output lines.
    /// </summary>
    [DisplayName("OutputTogglePayload")]
    [Description("Creates a message payload that toggle the specified digital output lines.")]
    public partial class CreateOutputTogglePayload
    {
        /// <summary>
        /// Gets or sets the value that toggle the specified digital output lines.
        /// </summary>
        [Description("The value that toggle the specified digital output lines.")]
        public DigitalOutputs OutputToggle { get; set; }

        /// <summary>
        /// Creates a message payload for the OutputToggle register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public DigitalOutputs GetPayload()
        {
            return OutputToggle;
        }

        /// <summary>
        /// Creates a message that toggle the specified digital output lines.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the OutputToggle register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SoundCard.OutputToggle.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that toggle the specified digital output lines.
    /// </summary>
    [DisplayName("TimestampedOutputTogglePayload")]
    [Description("Creates a timestamped message payload that toggle the specified digital output lines.")]
    public partial class CreateTimestampedOutputTogglePayload : CreateOutputTogglePayload
    {
        /// <summary>
        /// Creates a timestamped message that toggle the specified digital output lines.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the OutputToggle register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SoundCard.OutputToggle.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that write the state of all digital output lines.
    /// </summary>
    [DisplayName("OutputStatePayload")]
    [Description("Creates a message payload that write the state of all digital output lines.")]
    public partial class CreateOutputStatePayload
    {
        /// <summary>
        /// Gets or sets the value that write the state of all digital output lines.
        /// </summary>
        [Description("The value that write the state of all digital output lines.")]
        public DigitalOutputs OutputState { get; set; }

        /// <summary>
        /// Creates a message payload for the OutputState register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public DigitalOutputs GetPayload()
        {
            return OutputState;
        }

        /// <summary>
        /// Creates a message that write the state of all digital output lines.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the OutputState register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SoundCard.OutputState.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that write the state of all digital output lines.
    /// </summary>
    [DisplayName("TimestampedOutputStatePayload")]
    [Description("Creates a timestamped message payload that write the state of all digital output lines.")]
    public partial class CreateTimestampedOutputStatePayload : CreateOutputStatePayload
    {
        /// <summary>
        /// Creates a timestamped message that write the state of all digital output lines.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the OutputState register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SoundCard.OutputState.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that configuration of Analog Inputs.
    /// </summary>
    [DisplayName("ConfigureAdcPayload")]
    [Description("Creates a message payload that configuration of Analog Inputs.")]
    public partial class CreateConfigureAdcPayload
    {
        /// <summary>
        /// Gets or sets the value that configuration of Analog Inputs.
        /// </summary>
        [Description("The value that configuration of Analog Inputs.")]
        public AdcConfiguration ConfigureAdc { get; set; }

        /// <summary>
        /// Creates a message payload for the ConfigureAdc register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public AdcConfiguration GetPayload()
        {
            return ConfigureAdc;
        }

        /// <summary>
        /// Creates a message that configuration of Analog Inputs.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the ConfigureAdc register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SoundCard.ConfigureAdc.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that configuration of Analog Inputs.
    /// </summary>
    [DisplayName("TimestampedConfigureAdcPayload")]
    [Description("Creates a timestamped message payload that configuration of Analog Inputs.")]
    public partial class CreateTimestampedConfigureAdcPayload : CreateConfigureAdcPayload
    {
        /// <summary>
        /// Creates a timestamped message that configuration of Analog Inputs.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the ConfigureAdc register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SoundCard.ConfigureAdc.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that contains sampled analog input data or dynamic sound parameters controlled by the ADC channels. Values are zero if not used.
    /// </summary>
    [DisplayName("AnalogDataPayload")]
    [Description("Creates a message payload that contains sampled analog input data or dynamic sound parameters controlled by the ADC channels. Values are zero if not used.")]
    public partial class CreateAnalogDataPayload
    {
        /// <summary>
        /// Gets or sets a value that the sampled analog input value on ADC0.
        /// </summary>
        [Description("The sampled analog input value on ADC0.")]
        public ushort Adc0 { get; set; }

        /// <summary>
        /// Gets or sets a value that the sampled analog input value on ADC1.
        /// </summary>
        [Description("The sampled analog input value on ADC1.")]
        public ushort Adc1 { get; set; }

        /// <summary>
        /// Gets or sets a value that the amplitude of the left channel controlled by ADC0.
        /// </summary>
        [Description("The amplitude of the left channel controlled by ADC0.")]
        public ushort AttenuationLeft { get; set; }

        /// <summary>
        /// Gets or sets a value that the amplitude of the right channel controlled by ADC0.
        /// </summary>
        [Description("The amplitude of the right channel controlled by ADC0.")]
        public ushort AttenuationRight { get; set; }

        /// <summary>
        /// Gets or sets a value that the output frequency controlled by ADC1.
        /// </summary>
        [Description("The output frequency controlled by ADC1.")]
        public ushort Frequency { get; set; }

        /// <summary>
        /// Creates a message payload for the AnalogData register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public AnalogDataPayload GetPayload()
        {
            AnalogDataPayload value;
            value.Adc0 = Adc0;
            value.Adc1 = Adc1;
            value.AttenuationLeft = AttenuationLeft;
            value.AttenuationRight = AttenuationRight;
            value.Frequency = Frequency;
            return value;
        }

        /// <summary>
        /// Creates a message that contains sampled analog input data or dynamic sound parameters controlled by the ADC channels. Values are zero if not used.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the AnalogData register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SoundCard.AnalogData.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that contains sampled analog input data or dynamic sound parameters controlled by the ADC channels. Values are zero if not used.
    /// </summary>
    [DisplayName("TimestampedAnalogDataPayload")]
    [Description("Creates a timestamped message payload that contains sampled analog input data or dynamic sound parameters controlled by the ADC channels. Values are zero if not used.")]
    public partial class CreateTimestampedAnalogDataPayload : CreateAnalogDataPayload
    {
        /// <summary>
        /// Creates a timestamped message that contains sampled analog input data or dynamic sound parameters controlled by the ADC channels. Values are zero if not used.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the AnalogData register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SoundCard.AnalogData.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that send commands to PIC32 micro-controller.
    /// </summary>
    [DisplayName("CommandsPayload")]
    [Description("Creates a message payload that send commands to PIC32 micro-controller.")]
    public partial class CreateCommandsPayload
    {
        /// <summary>
        /// Gets or sets the value that send commands to PIC32 micro-controller.
        /// </summary>
        [Description("The value that send commands to PIC32 micro-controller.")]
        public ControllerCommand Commands { get; set; }

        /// <summary>
        /// Creates a message payload for the Commands register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ControllerCommand GetPayload()
        {
            return Commands;
        }

        /// <summary>
        /// Creates a message that send commands to PIC32 micro-controller.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Commands register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SoundCard.Commands.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that send commands to PIC32 micro-controller.
    /// </summary>
    [DisplayName("TimestampedCommandsPayload")]
    [Description("Creates a timestamped message payload that send commands to PIC32 micro-controller.")]
    public partial class CreateTimestampedCommandsPayload : CreateCommandsPayload
    {
        /// <summary>
        /// Creates a timestamped message that send commands to PIC32 micro-controller.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Commands register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SoundCard.Commands.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that specifies the active events in the SoundCard device.
    /// </summary>
    [DisplayName("EnableEventsPayload")]
    [Description("Creates a message payload that specifies the active events in the SoundCard device.")]
    public partial class CreateEnableEventsPayload
    {
        /// <summary>
        /// Gets or sets the value that specifies the active events in the SoundCard device.
        /// </summary>
        [Description("The value that specifies the active events in the SoundCard device.")]
        public SoundCardEvents EnableEvents { get; set; }

        /// <summary>
        /// Creates a message payload for the EnableEvents register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public SoundCardEvents GetPayload()
        {
            return EnableEvents;
        }

        /// <summary>
        /// Creates a message that specifies the active events in the SoundCard device.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the EnableEvents register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SoundCard.EnableEvents.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that specifies the active events in the SoundCard device.
    /// </summary>
    [DisplayName("TimestampedEnableEventsPayload")]
    [Description("Creates a timestamped message payload that specifies the active events in the SoundCard device.")]
    public partial class CreateTimestampedEnableEventsPayload : CreateEnableEventsPayload
    {
        /// <summary>
        /// Creates a timestamped message that specifies the active events in the SoundCard device.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the EnableEvents register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SoundCard.EnableEvents.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents the payload of the AnalogData register.
    /// </summary>
    public struct AnalogDataPayload
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnalogDataPayload"/> structure.
        /// </summary>
        /// <param name="adc0">The sampled analog input value on ADC0.</param>
        /// <param name="adc1">The sampled analog input value on ADC1.</param>
        /// <param name="attenuationLeft">The amplitude of the left channel controlled by ADC0.</param>
        /// <param name="attenuationRight">The amplitude of the right channel controlled by ADC0.</param>
        /// <param name="frequency">The output frequency controlled by ADC1.</param>
        public AnalogDataPayload(
            ushort adc0,
            ushort adc1,
            ushort attenuationLeft,
            ushort attenuationRight,
            ushort frequency)
        {
            Adc0 = adc0;
            Adc1 = adc1;
            AttenuationLeft = attenuationLeft;
            AttenuationRight = attenuationRight;
            Frequency = frequency;
        }

        /// <summary>
        /// The sampled analog input value on ADC0.
        /// </summary>
        public ushort Adc0;

        /// <summary>
        /// The sampled analog input value on ADC1.
        /// </summary>
        public ushort Adc1;

        /// <summary>
        /// The amplitude of the left channel controlled by ADC0.
        /// </summary>
        public ushort AttenuationLeft;

        /// <summary>
        /// The amplitude of the right channel controlled by ADC0.
        /// </summary>
        public ushort AttenuationRight;

        /// <summary>
        /// The output frequency controlled by ADC1.
        /// </summary>
        public ushort Frequency;

        /// <summary>
        /// Returns a <see cref="string"/> that represents the payload of
        /// the AnalogData register.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> that represents the payload of the
        /// AnalogData register.
        /// </returns>
        public override string ToString()
        {
            return "AnalogDataPayload { " +
                "Adc0 = " + Adc0 + ", " +
                "Adc1 = " + Adc1 + ", " +
                "AttenuationLeft = " + AttenuationLeft + ", " +
                "AttenuationRight = " + AttenuationRight + ", " +
                "Frequency = " + Frequency + " " +
            "}";
        }
    }

    /// <summary>
    /// Specifies the state of the digital input lines.
    /// </summary>
    [Flags]
    public enum DigitalInputs : byte
    {
        None = 0x0,
        DI0 = 0x1
    }

    /// <summary>
    /// Specifies the state of the digital output lines.
    /// </summary>
    [Flags]
    public enum DigitalOutputs : byte
    {
        None = 0x0,
        DO0 = 0x1,
        DO1 = 0x2,
        DO2 = 0x3
    }

    /// <summary>
    /// Specifies the active events in the SoundCard.
    /// </summary>
    [Flags]
    public enum SoundCardEvents : byte
    {
        None = 0x0,
        PlaySoundOrFrequency = 0x1,
        Stop = 0x2,
        DigitalInputs = 0x4,
        AdcValues = 0x8
    }

    /// <summary>
    /// Specifies the operation mode of the digital input.
    /// </summary>
    public enum DigitalInputConfiguration : byte
    {
        /// <summary>
        /// Used as a pure digital input.
        /// </summary>
        Digital = 0,

        /// <summary>
        /// Starts sound when rising edge and stop when falling edge.
        /// </summary>
        StartAndStopSound = 1,

        /// <summary>
        /// Starts sound when rising edge.
        /// </summary>
        StartSound = 2,

        /// <summary>
        /// Stops sound or frequency when rising edge.
        /// </summary>
        Stop = 3,

        /// <summary>
        /// Starts frequency when rising edge and stop when falling edge.
        /// </summary>
        StartAndStopFrequency = 4,

        /// <summary>
        /// Starts frequency when rising edge.
        /// </summary>
        StartFrequency = 5
    }

    /// <summary>
    /// Specifies the operation mode of the digital output.
    /// </summary>
    public enum DigitalOutputConfiguration : byte
    {
        /// <summary>
        /// Used as a pure digital output.
        /// </summary>
        Digital = 0,

        /// <summary>
        /// The digital output will be high during a period specified by register DOxPulse.
        /// </summary>
        Pulse = 1,

        /// <summary>
        /// High when the sound is being played.
        /// </summary>
        HighWhenSound = 2,

        /// <summary>
        /// High when sound starts during 1 ms.
        /// </summary>
        Pulse1MsWhenStart = 3,

        /// <summary>
        /// High when sound starts during 10 ms.
        /// </summary>
        Pulse10MsWhenStart = 4,

        /// <summary>
        /// High when sound starts during 100 ms.
        /// </summary>
        Pulse100MsWhenStart = 5,

        /// <summary>
        /// High when sound stops during 1 ms.
        /// </summary>
        Pulse1MsWhenStop = 6,

        /// <summary>
        /// High when sound stops during 10 ms.
        /// </summary>
        Pulse10MsWhenStop = 7,

        /// <summary>
        /// High when sound starts during 100 ms.
        /// </summary>
        Pulse100MsWhenStop = 8
    }

    /// <summary>
    /// Specifies commands to send to the PIC32 micro-controller
    /// </summary>
    public enum ControllerCommand : byte
    {
        DisableBootloader = 0,
        EnableBootloader = 1,
        DeleteAllSounds = 255
    }

    /// <summary>
    /// Specifies the operation mode of the analog inputs.
    /// </summary>
    public enum AdcConfiguration : byte
    {
        NotUsed = 0,
        AdcAdc = 1,
        AmplitudeBothAdc = 2,
        AmplitudeLeftAdc = 3,
        AmplitudeRightAdc = 4,
        AmplitudeLeftAmplitudeRight = 5,
        AmplitudeBothFrequency = 6
    }
}
