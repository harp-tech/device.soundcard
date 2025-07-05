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
            { 32, typeof(Start) },
            { 33, typeof(Stop) },
            { 34, typeof(AttenuationLeft) },
            { 35, typeof(AttenuationRight) },
            { 36, typeof(AttenuationBoth) },
            { 37, typeof(AttenuationStart) },
            { 38, typeof(Reserved0) },
            { 39, typeof(Reserved1) },
            { 40, typeof(InputState) },
            { 41, typeof(ConfigureDI0) },
            { 42, typeof(ConfigureDI1) },
            { 43, typeof(StartDI0) },
            { 44, typeof(StartDI1) },
            { 45, typeof(AttenuationLeftDI0) },
            { 46, typeof(AttenuationLeftDI1) },
            { 47, typeof(AttenuationRightDI0) },
            { 48, typeof(AttenuationRightDI1) },
            { 49, typeof(Reserved2) },
            { 50, typeof(Reserved3) },
            { 51, typeof(Reserved4) },
            { 52, typeof(ConfigureDO0) },
            { 53, typeof(ConfigureDO1) },
            { 54, typeof(ConfigureDO2) },
            { 55, typeof(Reserved5) },
            { 56, typeof(Reserved6) },
            { 57, typeof(Reserved7) },
            { 58, typeof(OutputSet) },
            { 59, typeof(OutputClear) },
            { 60, typeof(OutputToggle) },
            { 61, typeof(OutputState) },
            { 62, typeof(Reserved8) },
            { 63, typeof(Reserved9) },
            { 64, typeof(EnableAdcControlState) },
            { 65, typeof(AdcControlState) },
            { 66, typeof(ConfigureAdc0) },
            { 67, typeof(ConfigureAdc1) },
            { 68, typeof(Reserved10) },
            { 69, typeof(Reserved11) },
            { 70, typeof(Reserved12) },
            { 71, typeof(Reserved13) },
            { 72, typeof(Reserved14) },
            { 73, typeof(Reserved15) },
            { 74, typeof(Reserved16) },
            { 75, typeof(Reserved17) },
            { 76, typeof(Reserved18) },
            { 77, typeof(Reserved19) },
            { 78, typeof(Reserved20) },
            { 79, typeof(Reserved21) },
            { 80, typeof(Reserved22) },
            { 81, typeof(Reserved23) },
            { 82, typeof(Pic32Commands) }
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
    public partial class GetDeviceMetadata : Source<string>
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
    /// Represents an operator that writes the sequence of <see cref="SoundCard"/>" messages
    /// to the standard Harp storage format.
    /// </summary>
    [Description("Writes the sequence of SoundCard messages to the standard Harp storage format.")]
    public partial class DeviceDataWriter : Sink<HarpMessage>, INamedElement
    {
        const string BinaryExtension = ".bin";
        const string MetadataFileName = "device.yml";
        readonly Bonsai.Harp.MessageWriter writer = new();

        string INamedElement.Name => nameof(SoundCard) + "DataWriter";

        /// <summary>
        /// Gets or sets the relative or absolute path on which to save the message data.
        /// </summary>
        [Description("The relative or absolute path of the directory on which to save the message data.")]
        [Editor("Bonsai.Design.SaveFileNameEditor, Bonsai.Design", DesignTypes.UITypeEditor)]
        public string Path
        {
            get => System.IO.Path.GetDirectoryName(writer.FileName);
            set => writer.FileName = System.IO.Path.Combine(value, nameof(SoundCard) + BinaryExtension);
        }

        /// <summary>
        /// Gets or sets a value indicating whether element writing should be buffered. If <see langword="true"/>,
        /// the write commands will be queued in memory as fast as possible and will be processed
        /// by the writer in a different thread. Otherwise, writing will be done in the same
        /// thread in which notifications arrive.
        /// </summary>
        [Description("Indicates whether writing should be buffered.")]
        public bool Buffered
        {
            get => writer.Buffered;
            set => writer.Buffered = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to overwrite the output file if it already exists.
        /// </summary>
        [Description("Indicates whether to overwrite the output file if it already exists.")]
        public bool Overwrite
        {
            get => writer.Overwrite;
            set => writer.Overwrite = value;
        }

        /// <summary>
        /// Gets or sets a value specifying how the message filter will use the matching criteria.
        /// </summary>
        [Description("Specifies how the message filter will use the matching criteria.")]
        public FilterType FilterType
        {
            get => writer.FilterType;
            set => writer.FilterType = value;
        }

        /// <summary>
        /// Gets or sets a value specifying the expected message type. If no value is
        /// specified, all messages will be accepted.
        /// </summary>
        [Description("Specifies the expected message type. If no value is specified, all messages will be accepted.")]
        public MessageType? MessageType
        {
            get => writer.MessageType;
            set => writer.MessageType = value;
        }

        private IObservable<TSource> WriteDeviceMetadata<TSource>(IObservable<TSource> source)
        {
            var basePath = Path;
            if (string.IsNullOrEmpty(basePath))
                return source;

            var metadataPath = System.IO.Path.Combine(basePath, MetadataFileName);
            return Observable.Create<TSource>(observer =>
            {
                Bonsai.IO.PathHelper.EnsureDirectory(metadataPath);
                if (System.IO.File.Exists(metadataPath) && !Overwrite)
                {
                    throw new System.IO.IOException(string.Format("The file '{0}' already exists.", metadataPath));
                }

                System.IO.File.WriteAllText(metadataPath, Device.Metadata);
                return source.SubscribeSafe(observer);
            });
        }

        /// <summary>
        /// Writes each Harp message in the sequence to the specified binary file, and the
        /// contents of the device metadata file to a separate text file.
        /// </summary>
        /// <param name="source">The sequence of messages to write to the file.</param>
        /// <returns>
        /// An observable sequence that is identical to the <paramref name="source"/>
        /// sequence but where there is an additional side effect of writing the
        /// messages to a raw binary file, and the contents of the device metadata file
        /// to a separate text file.
        /// </returns>
        public override IObservable<HarpMessage> Process(IObservable<HarpMessage> source)
        {
            return source.Publish(ps => ps.Merge(
                WriteDeviceMetadata(writer.Process(ps.GroupBy(message => message.Address)))
                .IgnoreElements()
                .Cast<HarpMessage>()));
        }

        /// <summary>
        /// Writes each Harp message in the sequence of observable groups to the
        /// corresponding binary file, where the name of each file is generated from
        /// the common group register address. The contents of the device metadata file are
        /// written to a separate text file.
        /// </summary>
        /// <param name="source">
        /// A sequence of observable groups, each of which corresponds to a unique register
        /// address.
        /// </param>
        /// <returns>
        /// An observable sequence that is identical to the <paramref name="source"/>
        /// sequence but where there is an additional side effect of writing the Harp
        /// messages in each group to the corresponding file, and the contents of the device
        /// metadata file to a separate text file.
        /// </returns>
        public IObservable<IGroupedObservable<int, HarpMessage>> Process(IObservable<IGroupedObservable<int, HarpMessage>> source)
        {
            return WriteDeviceMetadata(writer.Process(source));
        }

        /// <summary>
        /// Writes each Harp message in the sequence of observable groups to the
        /// corresponding binary file, where the name of each file is generated from
        /// the common group register name. The contents of the device metadata file are
        /// written to a separate text file.
        /// </summary>
        /// <param name="source">
        /// A sequence of observable groups, each of which corresponds to a unique register
        /// type.
        /// </param>
        /// <returns>
        /// An observable sequence that is identical to the <paramref name="source"/>
        /// sequence but where there is an additional side effect of writing the Harp
        /// messages in each group to the corresponding file, and the contents of the device
        /// metadata file to a separate text file.
        /// </returns>
        public IObservable<IGroupedObservable<Type, HarpMessage>> Process(IObservable<IGroupedObservable<Type, HarpMessage>> source)
        {
            return WriteDeviceMetadata(writer.Process(source));
        }
    }

    /// <summary>
    /// Represents an operator that filters register-specific messages
    /// reported by the <see cref="SoundCard"/> device.
    /// </summary>
    /// <seealso cref="Start"/>
    /// <seealso cref="Stop"/>
    /// <seealso cref="AttenuationLeft"/>
    /// <seealso cref="AttenuationRight"/>
    /// <seealso cref="AttenuationBoth"/>
    /// <seealso cref="AttenuationStart"/>
    /// <seealso cref="InputState"/>
    /// <seealso cref="ConfigureDI0"/>
    /// <seealso cref="ConfigureDI1"/>
    /// <seealso cref="StartDI0"/>
    /// <seealso cref="StartDI1"/>
    /// <seealso cref="AttenuationLeftDI0"/>
    /// <seealso cref="AttenuationLeftDI1"/>
    /// <seealso cref="AttenuationRightDI0"/>
    /// <seealso cref="AttenuationRightDI1"/>
    /// <seealso cref="ConfigureDO0"/>
    /// <seealso cref="ConfigureDO1"/>
    /// <seealso cref="ConfigureDO2"/>
    /// <seealso cref="OutputSet"/>
    /// <seealso cref="OutputClear"/>
    /// <seealso cref="OutputToggle"/>
    /// <seealso cref="OutputState"/>
    /// <seealso cref="EnableAdcControlState"/>
    /// <seealso cref="AdcControlState"/>
    /// <seealso cref="Pic32Commands"/>
    [XmlInclude(typeof(Start))]
    [XmlInclude(typeof(Stop))]
    [XmlInclude(typeof(AttenuationLeft))]
    [XmlInclude(typeof(AttenuationRight))]
    [XmlInclude(typeof(AttenuationBoth))]
    [XmlInclude(typeof(AttenuationStart))]
    [XmlInclude(typeof(InputState))]
    [XmlInclude(typeof(ConfigureDI0))]
    [XmlInclude(typeof(ConfigureDI1))]
    [XmlInclude(typeof(StartDI0))]
    [XmlInclude(typeof(StartDI1))]
    [XmlInclude(typeof(AttenuationLeftDI0))]
    [XmlInclude(typeof(AttenuationLeftDI1))]
    [XmlInclude(typeof(AttenuationRightDI0))]
    [XmlInclude(typeof(AttenuationRightDI1))]
    [XmlInclude(typeof(ConfigureDO0))]
    [XmlInclude(typeof(ConfigureDO1))]
    [XmlInclude(typeof(ConfigureDO2))]
    [XmlInclude(typeof(OutputSet))]
    [XmlInclude(typeof(OutputClear))]
    [XmlInclude(typeof(OutputToggle))]
    [XmlInclude(typeof(OutputState))]
    [XmlInclude(typeof(EnableAdcControlState))]
    [XmlInclude(typeof(AdcControlState))]
    [XmlInclude(typeof(Pic32Commands))]
    [Description("Filters register-specific messages reported by the SoundCard device.")]
    public class FilterRegister : FilterRegisterBuilder, INamedElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterRegister"/> class.
        /// </summary>
        public FilterRegister()
        {
            Register = new Start();
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
    /// <seealso cref="Start"/>
    /// <seealso cref="Stop"/>
    /// <seealso cref="AttenuationLeft"/>
    /// <seealso cref="AttenuationRight"/>
    /// <seealso cref="AttenuationBoth"/>
    /// <seealso cref="AttenuationStart"/>
    /// <seealso cref="InputState"/>
    /// <seealso cref="ConfigureDI0"/>
    /// <seealso cref="ConfigureDI1"/>
    /// <seealso cref="StartDI0"/>
    /// <seealso cref="StartDI1"/>
    /// <seealso cref="AttenuationLeftDI0"/>
    /// <seealso cref="AttenuationLeftDI1"/>
    /// <seealso cref="AttenuationRightDI0"/>
    /// <seealso cref="AttenuationRightDI1"/>
    /// <seealso cref="ConfigureDO0"/>
    /// <seealso cref="ConfigureDO1"/>
    /// <seealso cref="ConfigureDO2"/>
    /// <seealso cref="OutputSet"/>
    /// <seealso cref="OutputClear"/>
    /// <seealso cref="OutputToggle"/>
    /// <seealso cref="OutputState"/>
    /// <seealso cref="EnableAdcControlState"/>
    /// <seealso cref="AdcControlState"/>
    /// <seealso cref="Pic32Commands"/>
    [XmlInclude(typeof(Start))]
    [XmlInclude(typeof(Stop))]
    [XmlInclude(typeof(AttenuationLeft))]
    [XmlInclude(typeof(AttenuationRight))]
    [XmlInclude(typeof(AttenuationBoth))]
    [XmlInclude(typeof(AttenuationStart))]
    [XmlInclude(typeof(InputState))]
    [XmlInclude(typeof(ConfigureDI0))]
    [XmlInclude(typeof(ConfigureDI1))]
    [XmlInclude(typeof(StartDI0))]
    [XmlInclude(typeof(StartDI1))]
    [XmlInclude(typeof(AttenuationLeftDI0))]
    [XmlInclude(typeof(AttenuationLeftDI1))]
    [XmlInclude(typeof(AttenuationRightDI0))]
    [XmlInclude(typeof(AttenuationRightDI1))]
    [XmlInclude(typeof(ConfigureDO0))]
    [XmlInclude(typeof(ConfigureDO1))]
    [XmlInclude(typeof(ConfigureDO2))]
    [XmlInclude(typeof(OutputSet))]
    [XmlInclude(typeof(OutputClear))]
    [XmlInclude(typeof(OutputToggle))]
    [XmlInclude(typeof(OutputState))]
    [XmlInclude(typeof(EnableAdcControlState))]
    [XmlInclude(typeof(AdcControlState))]
    [XmlInclude(typeof(Pic32Commands))]
    [XmlInclude(typeof(TimestampedStart))]
    [XmlInclude(typeof(TimestampedStop))]
    [XmlInclude(typeof(TimestampedAttenuationLeft))]
    [XmlInclude(typeof(TimestampedAttenuationRight))]
    [XmlInclude(typeof(TimestampedAttenuationBoth))]
    [XmlInclude(typeof(TimestampedAttenuationStart))]
    [XmlInclude(typeof(TimestampedInputState))]
    [XmlInclude(typeof(TimestampedConfigureDI0))]
    [XmlInclude(typeof(TimestampedConfigureDI1))]
    [XmlInclude(typeof(TimestampedStartDI0))]
    [XmlInclude(typeof(TimestampedStartDI1))]
    [XmlInclude(typeof(TimestampedAttenuationLeftDI0))]
    [XmlInclude(typeof(TimestampedAttenuationLeftDI1))]
    [XmlInclude(typeof(TimestampedAttenuationRightDI0))]
    [XmlInclude(typeof(TimestampedAttenuationRightDI1))]
    [XmlInclude(typeof(TimestampedConfigureDO0))]
    [XmlInclude(typeof(TimestampedConfigureDO1))]
    [XmlInclude(typeof(TimestampedConfigureDO2))]
    [XmlInclude(typeof(TimestampedOutputSet))]
    [XmlInclude(typeof(TimestampedOutputClear))]
    [XmlInclude(typeof(TimestampedOutputToggle))]
    [XmlInclude(typeof(TimestampedOutputState))]
    [XmlInclude(typeof(TimestampedEnableAdcControlState))]
    [XmlInclude(typeof(TimestampedAdcControlState))]
    [XmlInclude(typeof(TimestampedPic32Commands))]
    [Description("Filters and selects specific messages reported by the SoundCard device.")]
    public partial class Parse : ParseBuilder, INamedElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Parse"/> class.
        /// </summary>
        public Parse()
        {
            Register = new Start();
        }

        string INamedElement.Name => $"{nameof(SoundCard)}.{GetElementDisplayName(Register)}";
    }

    /// <summary>
    /// Represents an operator which formats a sequence of values as specific
    /// SoundCard register messages.
    /// </summary>
    /// <seealso cref="Start"/>
    /// <seealso cref="Stop"/>
    /// <seealso cref="AttenuationLeft"/>
    /// <seealso cref="AttenuationRight"/>
    /// <seealso cref="AttenuationBoth"/>
    /// <seealso cref="AttenuationStart"/>
    /// <seealso cref="InputState"/>
    /// <seealso cref="ConfigureDI0"/>
    /// <seealso cref="ConfigureDI1"/>
    /// <seealso cref="StartDI0"/>
    /// <seealso cref="StartDI1"/>
    /// <seealso cref="AttenuationLeftDI0"/>
    /// <seealso cref="AttenuationLeftDI1"/>
    /// <seealso cref="AttenuationRightDI0"/>
    /// <seealso cref="AttenuationRightDI1"/>
    /// <seealso cref="ConfigureDO0"/>
    /// <seealso cref="ConfigureDO1"/>
    /// <seealso cref="ConfigureDO2"/>
    /// <seealso cref="OutputSet"/>
    /// <seealso cref="OutputClear"/>
    /// <seealso cref="OutputToggle"/>
    /// <seealso cref="OutputState"/>
    /// <seealso cref="EnableAdcControlState"/>
    /// <seealso cref="AdcControlState"/>
    /// <seealso cref="Pic32Commands"/>
    [XmlInclude(typeof(Start))]
    [XmlInclude(typeof(Stop))]
    [XmlInclude(typeof(AttenuationLeft))]
    [XmlInclude(typeof(AttenuationRight))]
    [XmlInclude(typeof(AttenuationBoth))]
    [XmlInclude(typeof(AttenuationStart))]
    [XmlInclude(typeof(InputState))]
    [XmlInclude(typeof(ConfigureDI0))]
    [XmlInclude(typeof(ConfigureDI1))]
    [XmlInclude(typeof(StartDI0))]
    [XmlInclude(typeof(StartDI1))]
    [XmlInclude(typeof(AttenuationLeftDI0))]
    [XmlInclude(typeof(AttenuationLeftDI1))]
    [XmlInclude(typeof(AttenuationRightDI0))]
    [XmlInclude(typeof(AttenuationRightDI1))]
    [XmlInclude(typeof(ConfigureDO0))]
    [XmlInclude(typeof(ConfigureDO1))]
    [XmlInclude(typeof(ConfigureDO2))]
    [XmlInclude(typeof(OutputSet))]
    [XmlInclude(typeof(OutputClear))]
    [XmlInclude(typeof(OutputToggle))]
    [XmlInclude(typeof(OutputState))]
    [XmlInclude(typeof(EnableAdcControlState))]
    [XmlInclude(typeof(AdcControlState))]
    [XmlInclude(typeof(Pic32Commands))]
    [Description("Formats a sequence of values as specific SoundCard register messages.")]
    public partial class Format : FormatBuilder, INamedElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Format"/> class.
        /// </summary>
        public Format()
        {
            Register = new Start();
        }

        string INamedElement.Name => $"{nameof(SoundCard)}.{GetElementDisplayName(Register)}";
    }

    /// <summary>
    /// Represents a register that starts the sound index (if less than 32) or waveform generator (if greater or equal than 32).
    /// </summary>
    [Description("Starts the sound index (if less than 32) or waveform generator (if greater or equal than 32).")]
    public partial class Start
    {
        /// <summary>
        /// Represents the address of the <see cref="Start"/> register. This field is constant.
        /// </summary>
        public const int Address = 32;

        /// <summary>
        /// Represents the payload type of the <see cref="Start"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="Start"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Start"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort GetPayload(HarpMessage message)
        {
            return message.GetPayloadUInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Start"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadUInt16();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Start"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Start"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Start"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Start"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Start register.
    /// </summary>
    /// <seealso cref="Start"/>
    [Description("Filters and selects timestamped messages from the Start register.")]
    public partial class TimestampedStart
    {
        /// <summary>
        /// Represents the address of the <see cref="Start"/> register. This field is constant.
        /// </summary>
        public const int Address = Start.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Start"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetPayload(HarpMessage message)
        {
            return Start.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that stops the frequency generator process with any input value (not implemented for a sound index). The time precision of this event is frequency dependent: +/- 2500 us at 500 Hz, +/- 750 us at 1000 Hz, +/- 250 us at 2000 Hz, and under +/- 210 us for any frequency above 5000 Hz.
    /// </summary>
    [Description("Stops the frequency generator process with any input value (not implemented for a sound index). The time precision of this event is frequency dependent: +/- 2500 us at 500 Hz, +/- 750 us at 1000 Hz, +/- 250 us at 2000 Hz, and under +/- 210 us for any frequency above 5000 Hz.")]
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
    /// Represents a register that specifies the attenuation for the left channel (1 LSB is 0.1dB).
    /// </summary>
    [Description("Specifies the attenuation for the left channel (1 LSB is 0.1dB).")]
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
    /// Represents a register that specifies the attenuation for the right channel (1 LSB is 0.1dB).
    /// </summary>
    [Description("Specifies the attenuation for the right channel (1 LSB is 0.1dB).")]
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
    /// Represents a register that specifies the attenuation for the right and left channels simultaneously [Att R] [Att L] (1 LSB is 0.1dB).
    /// </summary>
    [Description("Specifies the attenuation for the right and left channels simultaneously [Att R] [Att L] (1 LSB is 0.1dB).")]
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
    /// Represents a register that specifies attenuation and simultaneously starts the sound index or frequency [Att R] [Att L] [Index].
    /// </summary>
    [Description("Specifies attenuation and simultaneously starts the sound index or frequency [Att R] [Att L] [Index].")]
    public partial class AttenuationStart
    {
        /// <summary>
        /// Represents the address of the <see cref="AttenuationStart"/> register. This field is constant.
        /// </summary>
        public const int Address = 37;

        /// <summary>
        /// Represents the payload type of the <see cref="AttenuationStart"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="AttenuationStart"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 3;

        /// <summary>
        /// Returns the payload data for <see cref="AttenuationStart"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort[] GetPayload(HarpMessage message)
        {
            return message.GetPayloadArray<ushort>();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="AttenuationStart"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort[]> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadArray<ushort>();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="AttenuationStart"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="AttenuationStart"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort[] value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="AttenuationStart"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="AttenuationStart"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort[] value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// AttenuationStart register.
    /// </summary>
    /// <seealso cref="AttenuationStart"/>
    [Description("Filters and selects timestamped messages from the AttenuationStart register.")]
    public partial class TimestampedAttenuationStart
    {
        /// <summary>
        /// Represents the address of the <see cref="AttenuationStart"/> register. This field is constant.
        /// </summary>
        public const int Address = AttenuationStart.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="AttenuationStart"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort[]> GetPayload(HarpMessage message)
        {
            return AttenuationStart.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that reserved for future use.
    /// </summary>
    [Description("Reserved for future use.")]
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
    [Description("Reserved for future use.")]
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
    /// Represents a register that reports the state of the digital inputs.
    /// </summary>
    [Description("Reports the state of the digital inputs.")]
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
    /// Represents a register that specifies the configuration for the digital input 0 (DI0).
    /// </summary>
    [Description("Specifies the configuration for the digital input 0 (DI0).")]
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
    /// Represents a register that specifies the configuration for the digital input 1 (DI1).
    /// </summary>
    [Description("Specifies the configuration for the digital input 1 (DI1).")]
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
    /// Represents a register that specifies the sound index to be played when triggering DI0.
    /// </summary>
    [Description("Specifies the sound index to be played when triggering DI0.")]
    public partial class StartDI0
    {
        /// <summary>
        /// Represents the address of the <see cref="StartDI0"/> register. This field is constant.
        /// </summary>
        public const int Address = 43;

        /// <summary>
        /// Represents the payload type of the <see cref="StartDI0"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="StartDI0"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="StartDI0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort GetPayload(HarpMessage message)
        {
            return message.GetPayloadUInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="StartDI0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadUInt16();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="StartDI0"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="StartDI0"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="StartDI0"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="StartDI0"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// StartDI0 register.
    /// </summary>
    /// <seealso cref="StartDI0"/>
    [Description("Filters and selects timestamped messages from the StartDI0 register.")]
    public partial class TimestampedStartDI0
    {
        /// <summary>
        /// Represents the address of the <see cref="StartDI0"/> register. This field is constant.
        /// </summary>
        public const int Address = StartDI0.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="StartDI0"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetPayload(HarpMessage message)
        {
            return StartDI0.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that specifies the sound index to be played when triggering DI1.
    /// </summary>
    [Description("Specifies the sound index to be played when triggering DI1.")]
    public partial class StartDI1
    {
        /// <summary>
        /// Represents the address of the <see cref="StartDI1"/> register. This field is constant.
        /// </summary>
        public const int Address = 44;

        /// <summary>
        /// Represents the payload type of the <see cref="StartDI1"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="StartDI1"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="StartDI1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort GetPayload(HarpMessage message)
        {
            return message.GetPayloadUInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="StartDI1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadUInt16();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="StartDI1"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="StartDI1"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="StartDI1"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="StartDI1"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// StartDI1 register.
    /// </summary>
    /// <seealso cref="StartDI1"/>
    [Description("Filters and selects timestamped messages from the StartDI1 register.")]
    public partial class TimestampedStartDI1
    {
        /// <summary>
        /// Represents the address of the <see cref="StartDI1"/> register. This field is constant.
        /// </summary>
        public const int Address = StartDI1.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="StartDI1"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetPayload(HarpMessage message)
        {
            return StartDI1.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that specifies the attenuation to set on the left channel (1 LSB is 0.1dB) when triggering DI0.
    /// </summary>
    [Description("Specifies the attenuation to set on the left channel (1 LSB is 0.1dB) when triggering DI0.")]
    public partial class AttenuationLeftDI0
    {
        /// <summary>
        /// Represents the address of the <see cref="AttenuationLeftDI0"/> register. This field is constant.
        /// </summary>
        public const int Address = 45;

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
    /// Represents a register that specifies the attenuation to set on the left channel (1 LSB is 0.1dB) when triggering DI1.
    /// </summary>
    [Description("Specifies the attenuation to set on the left channel (1 LSB is 0.1dB) when triggering DI1.")]
    public partial class AttenuationLeftDI1
    {
        /// <summary>
        /// Represents the address of the <see cref="AttenuationLeftDI1"/> register. This field is constant.
        /// </summary>
        public const int Address = 46;

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
    /// Represents a register that specifies the attenuation to set on the right channel (1 LSB is 0.1dB) when triggering DI0.
    /// </summary>
    [Description("Specifies the attenuation to set on the right channel (1 LSB is 0.1dB) when triggering DI0.")]
    public partial class AttenuationRightDI0
    {
        /// <summary>
        /// Represents the address of the <see cref="AttenuationRightDI0"/> register. This field is constant.
        /// </summary>
        public const int Address = 47;

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
    /// Represents a register that specifies the attenuation to set on the right channel (1 LSB is 0.1dB) when triggering DI1.
    /// </summary>
    [Description("Specifies the attenuation to set on the right channel (1 LSB is 0.1dB) when triggering DI1.")]
    public partial class AttenuationRightDI1
    {
        /// <summary>
        /// Represents the address of the <see cref="AttenuationRightDI1"/> register. This field is constant.
        /// </summary>
        public const int Address = 48;

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
    /// Represents a register that reserved for future use.
    /// </summary>
    [Description("Reserved for future use.")]
    internal partial class Reserved2
    {
        /// <summary>
        /// Represents the address of the <see cref="Reserved2"/> register. This field is constant.
        /// </summary>
        public const int Address = 49;

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
    [Description("Reserved for future use.")]
    internal partial class Reserved3
    {
        /// <summary>
        /// Represents the address of the <see cref="Reserved3"/> register. This field is constant.
        /// </summary>
        public const int Address = 50;

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
    [Description("Reserved for future use.")]
    internal partial class Reserved4
    {
        /// <summary>
        /// Represents the address of the <see cref="Reserved4"/> register. This field is constant.
        /// </summary>
        public const int Address = 51;

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
    /// Represents a register that specifies the configuration of the digital output 0 (DO0).
    /// </summary>
    [Description("Specifies the configuration of the digital output 0 (DO0).")]
    public partial class ConfigureDO0
    {
        /// <summary>
        /// Represents the address of the <see cref="ConfigureDO0"/> register. This field is constant.
        /// </summary>
        public const int Address = 52;

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
    /// Represents a register that specifies the configuration of the digital output 1 (DO1).
    /// </summary>
    [Description("Specifies the configuration of the digital output 1 (DO1).")]
    public partial class ConfigureDO1
    {
        /// <summary>
        /// Represents the address of the <see cref="ConfigureDO1"/> register. This field is constant.
        /// </summary>
        public const int Address = 53;

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
    /// Represents a register that specifies the configuration of the digital output 2 (DO2).
    /// </summary>
    [Description("Specifies the configuration of the digital output 2 (DO2).")]
    public partial class ConfigureDO2
    {
        /// <summary>
        /// Represents the address of the <see cref="ConfigureDO2"/> register. This field is constant.
        /// </summary>
        public const int Address = 54;

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
    /// Represents a register that reserved for future use.
    /// </summary>
    [Description("Reserved for future use.")]
    internal partial class Reserved5
    {
        /// <summary>
        /// Represents the address of the <see cref="Reserved5"/> register. This field is constant.
        /// </summary>
        public const int Address = 55;

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
    [Description("Reserved for future use.")]
    internal partial class Reserved6
    {
        /// <summary>
        /// Represents the address of the <see cref="Reserved6"/> register. This field is constant.
        /// </summary>
        public const int Address = 56;

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
    [Description("Reserved for future use.")]
    internal partial class Reserved7
    {
        /// <summary>
        /// Represents the address of the <see cref="Reserved7"/> register. This field is constant.
        /// </summary>
        public const int Address = 57;

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
    /// Represents a register that sets the specified digital output lines.
    /// </summary>
    [Description("Sets the specified digital output lines.")]
    public partial class OutputSet
    {
        /// <summary>
        /// Represents the address of the <see cref="OutputSet"/> register. This field is constant.
        /// </summary>
        public const int Address = 58;

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
    /// Represents a register that clears the specified digital output lines.
    /// </summary>
    [Description("Clears the specified digital output lines.")]
    public partial class OutputClear
    {
        /// <summary>
        /// Represents the address of the <see cref="OutputClear"/> register. This field is constant.
        /// </summary>
        public const int Address = 59;

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
    /// Represents a register that toggles the specified digital output lines.
    /// </summary>
    [Description("Toggles the specified digital output lines.")]
    public partial class OutputToggle
    {
        /// <summary>
        /// Represents the address of the <see cref="OutputToggle"/> register. This field is constant.
        /// </summary>
        public const int Address = 60;

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
    /// Represents a register that writes the state of all digital output lines.
    /// </summary>
    [Description("Writes the state of all digital output lines.")]
    public partial class OutputState
    {
        /// <summary>
        /// Represents the address of the <see cref="OutputState"/> register. This field is constant.
        /// </summary>
        public const int Address = 61;

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
    [Description("Reserved for future use.")]
    internal partial class Reserved8
    {
        /// <summary>
        /// Represents the address of the <see cref="Reserved8"/> register. This field is constant.
        /// </summary>
        public const int Address = 62;

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
    [Description("Reserved for future use.")]
    internal partial class Reserved9
    {
        /// <summary>
        /// Represents the address of the <see cref="Reserved9"/> register. This field is constant.
        /// </summary>
        public const int Address = 63;

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
    /// Represents a register that specifies the configuration of the ADC control stream.
    /// </summary>
    [Description("Specifies the configuration of the ADC control stream.")]
    public partial class EnableAdcControlState
    {
        /// <summary>
        /// Represents the address of the <see cref="EnableAdcControlState"/> register. This field is constant.
        /// </summary>
        public const int Address = 64;

        /// <summary>
        /// Represents the payload type of the <see cref="EnableAdcControlState"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="EnableAdcControlState"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="EnableAdcControlState"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static AdcControlStateConfiguration GetPayload(HarpMessage message)
        {
            return (AdcControlStateConfiguration)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="EnableAdcControlState"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<AdcControlStateConfiguration> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((AdcControlStateConfiguration)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="EnableAdcControlState"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="EnableAdcControlState"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, AdcControlStateConfiguration value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="EnableAdcControlState"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="EnableAdcControlState"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, AdcControlStateConfiguration value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// EnableAdcControlState register.
    /// </summary>
    /// <seealso cref="EnableAdcControlState"/>
    [Description("Filters and selects timestamped messages from the EnableAdcControlState register.")]
    public partial class TimestampedEnableAdcControlState
    {
        /// <summary>
        /// Represents the address of the <see cref="EnableAdcControlState"/> register. This field is constant.
        /// </summary>
        public const int Address = EnableAdcControlState.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="EnableAdcControlState"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<AdcControlStateConfiguration> GetPayload(HarpMessage message)
        {
            return EnableAdcControlState.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that reports sampled analog input data and current sound parameters.
    /// </summary>
    [Description("Reports sampled analog input data and current sound parameters.")]
    public partial class AdcControlState
    {
        /// <summary>
        /// Represents the address of the <see cref="AdcControlState"/> register. This field is constant.
        /// </summary>
        public const int Address = 65;

        /// <summary>
        /// Represents the payload type of the <see cref="AdcControlState"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="AdcControlState"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 5;

        static AdcControlStatePayload ParsePayload(ushort[] payload)
        {
            AdcControlStatePayload result;
            result.Adc0 = payload[0];
            result.Adc1 = payload[1];
            result.AttenuationLeft = payload[2];
            result.AttenuationRight = payload[3];
            result.Frequency = payload[4];
            return result;
        }

        static ushort[] FormatPayload(AdcControlStatePayload value)
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
        /// Returns the payload data for <see cref="AdcControlState"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static AdcControlStatePayload GetPayload(HarpMessage message)
        {
            return ParsePayload(message.GetPayloadArray<ushort>());
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="AdcControlState"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<AdcControlStatePayload> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadArray<ushort>();
            return Timestamped.Create(ParsePayload(payload.Value), payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="AdcControlState"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="AdcControlState"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, AdcControlStatePayload value)
        {
            return HarpMessage.FromUInt16(Address, messageType, FormatPayload(value));
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="AdcControlState"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="AdcControlState"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, AdcControlStatePayload value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, FormatPayload(value));
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// AdcControlState register.
    /// </summary>
    /// <seealso cref="AdcControlState"/>
    [Description("Filters and selects timestamped messages from the AdcControlState register.")]
    public partial class TimestampedAdcControlState
    {
        /// <summary>
        /// Represents the address of the <see cref="AdcControlState"/> register. This field is constant.
        /// </summary>
        public const int Address = AdcControlState.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="AdcControlState"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<AdcControlStatePayload> GetPayload(HarpMessage message)
        {
            return AdcControlState.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that specifies the configuration of analog input ADC0 (not implemented yet).
    /// </summary>
    [Description("Specifies the configuration of analog input ADC0 (not implemented yet).")]
    internal partial class ConfigureAdc0
    {
        /// <summary>
        /// Represents the address of the <see cref="ConfigureAdc0"/> register. This field is constant.
        /// </summary>
        public const int Address = 66;

        /// <summary>
        /// Represents the payload type of the <see cref="ConfigureAdc0"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="ConfigureAdc0"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;
    }

    /// <summary>
    /// Represents a register that specifies the configuration of analog input ADC1 (not implemented yet).
    /// </summary>
    [Description("Specifies the configuration of analog input ADC1 (not implemented yet).")]
    internal partial class ConfigureAdc1
    {
        /// <summary>
        /// Represents the address of the <see cref="ConfigureAdc1"/> register. This field is constant.
        /// </summary>
        public const int Address = 67;

        /// <summary>
        /// Represents the payload type of the <see cref="ConfigureAdc1"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="ConfigureAdc1"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;
    }

    /// <summary>
    /// Represents a register that reserved for future use.
    /// </summary>
    [Description("Reserved for future use.")]
    internal partial class Reserved10
    {
        /// <summary>
        /// Represents the address of the <see cref="Reserved10"/> register. This field is constant.
        /// </summary>
        public const int Address = 68;

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
    [Description("Reserved for future use.")]
    internal partial class Reserved11
    {
        /// <summary>
        /// Represents the address of the <see cref="Reserved11"/> register. This field is constant.
        /// </summary>
        public const int Address = 69;

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
    [Description("Reserved for future use.")]
    internal partial class Reserved12
    {
        /// <summary>
        /// Represents the address of the <see cref="Reserved12"/> register. This field is constant.
        /// </summary>
        public const int Address = 70;

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
    /// Represents a register that reserved for future use.
    /// </summary>
    [Description("Reserved for future use.")]
    internal partial class Reserved13
    {
        /// <summary>
        /// Represents the address of the <see cref="Reserved13"/> register. This field is constant.
        /// </summary>
        public const int Address = 71;

        /// <summary>
        /// Represents the payload type of the <see cref="Reserved13"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="Reserved13"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;
    }

    /// <summary>
    /// Represents a register that reserved for future use.
    /// </summary>
    [Description("Reserved for future use.")]
    internal partial class Reserved14
    {
        /// <summary>
        /// Represents the address of the <see cref="Reserved14"/> register. This field is constant.
        /// </summary>
        public const int Address = 72;

        /// <summary>
        /// Represents the payload type of the <see cref="Reserved14"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="Reserved14"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;
    }

    /// <summary>
    /// Represents a register that reserved for future use.
    /// </summary>
    [Description("Reserved for future use.")]
    internal partial class Reserved15
    {
        /// <summary>
        /// Represents the address of the <see cref="Reserved15"/> register. This field is constant.
        /// </summary>
        public const int Address = 73;

        /// <summary>
        /// Represents the payload type of the <see cref="Reserved15"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="Reserved15"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;
    }

    /// <summary>
    /// Represents a register that reserved for future use.
    /// </summary>
    [Description("Reserved for future use.")]
    internal partial class Reserved16
    {
        /// <summary>
        /// Represents the address of the <see cref="Reserved16"/> register. This field is constant.
        /// </summary>
        public const int Address = 74;

        /// <summary>
        /// Represents the payload type of the <see cref="Reserved16"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="Reserved16"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;
    }

    /// <summary>
    /// Represents a register that reserved for future use.
    /// </summary>
    [Description("Reserved for future use.")]
    internal partial class Reserved17
    {
        /// <summary>
        /// Represents the address of the <see cref="Reserved17"/> register. This field is constant.
        /// </summary>
        public const int Address = 75;

        /// <summary>
        /// Represents the payload type of the <see cref="Reserved17"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="Reserved17"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;
    }

    /// <summary>
    /// Represents a register that reserved for future use.
    /// </summary>
    [Description("Reserved for future use.")]
    internal partial class Reserved18
    {
        /// <summary>
        /// Represents the address of the <see cref="Reserved18"/> register. This field is constant.
        /// </summary>
        public const int Address = 76;

        /// <summary>
        /// Represents the payload type of the <see cref="Reserved18"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="Reserved18"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;
    }

    /// <summary>
    /// Represents a register that reserved for future use.
    /// </summary>
    [Description("Reserved for future use.")]
    internal partial class Reserved19
    {
        /// <summary>
        /// Represents the address of the <see cref="Reserved19"/> register. This field is constant.
        /// </summary>
        public const int Address = 77;

        /// <summary>
        /// Represents the payload type of the <see cref="Reserved19"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="Reserved19"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;
    }

    /// <summary>
    /// Represents a register that reserved for future use.
    /// </summary>
    [Description("Reserved for future use.")]
    internal partial class Reserved20
    {
        /// <summary>
        /// Represents the address of the <see cref="Reserved20"/> register. This field is constant.
        /// </summary>
        public const int Address = 78;

        /// <summary>
        /// Represents the payload type of the <see cref="Reserved20"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="Reserved20"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;
    }

    /// <summary>
    /// Represents a register that reserved for future use.
    /// </summary>
    [Description("Reserved for future use.")]
    internal partial class Reserved21
    {
        /// <summary>
        /// Represents the address of the <see cref="Reserved21"/> register. This field is constant.
        /// </summary>
        public const int Address = 79;

        /// <summary>
        /// Represents the payload type of the <see cref="Reserved21"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="Reserved21"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;
    }

    /// <summary>
    /// Represents a register that reserved for future use.
    /// </summary>
    [Description("Reserved for future use.")]
    internal partial class Reserved22
    {
        /// <summary>
        /// Represents the address of the <see cref="Reserved22"/> register. This field is constant.
        /// </summary>
        public const int Address = 80;

        /// <summary>
        /// Represents the payload type of the <see cref="Reserved22"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="Reserved22"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;
    }

    /// <summary>
    /// Represents a register that reserved for future use.
    /// </summary>
    [Description("Reserved for future use.")]
    internal partial class Reserved23
    {
        /// <summary>
        /// Represents the address of the <see cref="Reserved23"/> register. This field is constant.
        /// </summary>
        public const int Address = 81;

        /// <summary>
        /// Represents the payload type of the <see cref="Reserved23"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="Reserved23"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;
    }

    /// <summary>
    /// Represents a register that sends commands to the PIC32 micro-controller.
    /// </summary>
    [Description("Sends commands to the PIC32 micro-controller")]
    public partial class Pic32Commands
    {
        /// <summary>
        /// Represents the address of the <see cref="Pic32Commands"/> register. This field is constant.
        /// </summary>
        public const int Address = 82;

        /// <summary>
        /// Represents the payload type of the <see cref="Pic32Commands"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="Pic32Commands"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Pic32Commands"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static Pic32Command GetPayload(HarpMessage message)
        {
            return (Pic32Command)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Pic32Commands"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<Pic32Command> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((Pic32Command)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Pic32Commands"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Pic32Commands"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, Pic32Command value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Pic32Commands"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Pic32Commands"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, Pic32Command value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Pic32Commands register.
    /// </summary>
    /// <seealso cref="Pic32Commands"/>
    [Description("Filters and selects timestamped messages from the Pic32Commands register.")]
    public partial class TimestampedPic32Commands
    {
        /// <summary>
        /// Represents the address of the <see cref="Pic32Commands"/> register. This field is constant.
        /// </summary>
        public const int Address = Pic32Commands.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Pic32Commands"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<Pic32Command> GetPayload(HarpMessage message)
        {
            return Pic32Commands.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents an operator which creates standard message payloads for the
    /// SoundCard device.
    /// </summary>
    /// <seealso cref="CreateStartPayload"/>
    /// <seealso cref="CreateStopPayload"/>
    /// <seealso cref="CreateAttenuationLeftPayload"/>
    /// <seealso cref="CreateAttenuationRightPayload"/>
    /// <seealso cref="CreateAttenuationBothPayload"/>
    /// <seealso cref="CreateAttenuationStartPayload"/>
    /// <seealso cref="CreateInputStatePayload"/>
    /// <seealso cref="CreateConfigureDI0Payload"/>
    /// <seealso cref="CreateConfigureDI1Payload"/>
    /// <seealso cref="CreateStartDI0Payload"/>
    /// <seealso cref="CreateStartDI1Payload"/>
    /// <seealso cref="CreateAttenuationLeftDI0Payload"/>
    /// <seealso cref="CreateAttenuationLeftDI1Payload"/>
    /// <seealso cref="CreateAttenuationRightDI0Payload"/>
    /// <seealso cref="CreateAttenuationRightDI1Payload"/>
    /// <seealso cref="CreateConfigureDO0Payload"/>
    /// <seealso cref="CreateConfigureDO1Payload"/>
    /// <seealso cref="CreateConfigureDO2Payload"/>
    /// <seealso cref="CreateOutputSetPayload"/>
    /// <seealso cref="CreateOutputClearPayload"/>
    /// <seealso cref="CreateOutputTogglePayload"/>
    /// <seealso cref="CreateOutputStatePayload"/>
    /// <seealso cref="CreateEnableAdcControlStatePayload"/>
    /// <seealso cref="CreateAdcControlStatePayload"/>
    /// <seealso cref="CreatePic32CommandsPayload"/>
    [XmlInclude(typeof(CreateStartPayload))]
    [XmlInclude(typeof(CreateStopPayload))]
    [XmlInclude(typeof(CreateAttenuationLeftPayload))]
    [XmlInclude(typeof(CreateAttenuationRightPayload))]
    [XmlInclude(typeof(CreateAttenuationBothPayload))]
    [XmlInclude(typeof(CreateAttenuationStartPayload))]
    [XmlInclude(typeof(CreateInputStatePayload))]
    [XmlInclude(typeof(CreateConfigureDI0Payload))]
    [XmlInclude(typeof(CreateConfigureDI1Payload))]
    [XmlInclude(typeof(CreateStartDI0Payload))]
    [XmlInclude(typeof(CreateStartDI1Payload))]
    [XmlInclude(typeof(CreateAttenuationLeftDI0Payload))]
    [XmlInclude(typeof(CreateAttenuationLeftDI1Payload))]
    [XmlInclude(typeof(CreateAttenuationRightDI0Payload))]
    [XmlInclude(typeof(CreateAttenuationRightDI1Payload))]
    [XmlInclude(typeof(CreateConfigureDO0Payload))]
    [XmlInclude(typeof(CreateConfigureDO1Payload))]
    [XmlInclude(typeof(CreateConfigureDO2Payload))]
    [XmlInclude(typeof(CreateOutputSetPayload))]
    [XmlInclude(typeof(CreateOutputClearPayload))]
    [XmlInclude(typeof(CreateOutputTogglePayload))]
    [XmlInclude(typeof(CreateOutputStatePayload))]
    [XmlInclude(typeof(CreateEnableAdcControlStatePayload))]
    [XmlInclude(typeof(CreateAdcControlStatePayload))]
    [XmlInclude(typeof(CreatePic32CommandsPayload))]
    [XmlInclude(typeof(CreateTimestampedStartPayload))]
    [XmlInclude(typeof(CreateTimestampedStopPayload))]
    [XmlInclude(typeof(CreateTimestampedAttenuationLeftPayload))]
    [XmlInclude(typeof(CreateTimestampedAttenuationRightPayload))]
    [XmlInclude(typeof(CreateTimestampedAttenuationBothPayload))]
    [XmlInclude(typeof(CreateTimestampedAttenuationStartPayload))]
    [XmlInclude(typeof(CreateTimestampedInputStatePayload))]
    [XmlInclude(typeof(CreateTimestampedConfigureDI0Payload))]
    [XmlInclude(typeof(CreateTimestampedConfigureDI1Payload))]
    [XmlInclude(typeof(CreateTimestampedStartDI0Payload))]
    [XmlInclude(typeof(CreateTimestampedStartDI1Payload))]
    [XmlInclude(typeof(CreateTimestampedAttenuationLeftDI0Payload))]
    [XmlInclude(typeof(CreateTimestampedAttenuationLeftDI1Payload))]
    [XmlInclude(typeof(CreateTimestampedAttenuationRightDI0Payload))]
    [XmlInclude(typeof(CreateTimestampedAttenuationRightDI1Payload))]
    [XmlInclude(typeof(CreateTimestampedConfigureDO0Payload))]
    [XmlInclude(typeof(CreateTimestampedConfigureDO1Payload))]
    [XmlInclude(typeof(CreateTimestampedConfigureDO2Payload))]
    [XmlInclude(typeof(CreateTimestampedOutputSetPayload))]
    [XmlInclude(typeof(CreateTimestampedOutputClearPayload))]
    [XmlInclude(typeof(CreateTimestampedOutputTogglePayload))]
    [XmlInclude(typeof(CreateTimestampedOutputStatePayload))]
    [XmlInclude(typeof(CreateTimestampedEnableAdcControlStatePayload))]
    [XmlInclude(typeof(CreateTimestampedAdcControlStatePayload))]
    [XmlInclude(typeof(CreateTimestampedPic32CommandsPayload))]
    [Description("Creates standard message payloads for the SoundCard device.")]
    public partial class CreateMessage : CreateMessageBuilder, INamedElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateMessage"/> class.
        /// </summary>
        public CreateMessage()
        {
            Payload = new CreateStartPayload();
        }

        string INamedElement.Name => $"{nameof(SoundCard)}.{GetElementDisplayName(Payload)}";
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that starts the sound index (if less than 32) or waveform generator (if greater or equal than 32).
    /// </summary>
    [DisplayName("StartPayload")]
    [Description("Creates a message payload that starts the sound index (if less than 32) or waveform generator (if greater or equal than 32).")]
    public partial class CreateStartPayload
    {
        /// <summary>
        /// Gets or sets the value that starts the sound index (if less than 32) or waveform generator (if greater or equal than 32).
        /// </summary>
        [Description("The value that starts the sound index (if less than 32) or waveform generator (if greater or equal than 32).")]
        public ushort Start { get; set; }

        /// <summary>
        /// Creates a message payload for the Start register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort GetPayload()
        {
            return Start;
        }

        /// <summary>
        /// Creates a message that starts the sound index (if less than 32) or waveform generator (if greater or equal than 32).
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Start register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SoundCard.Start.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that starts the sound index (if less than 32) or waveform generator (if greater or equal than 32).
    /// </summary>
    [DisplayName("TimestampedStartPayload")]
    [Description("Creates a timestamped message payload that starts the sound index (if less than 32) or waveform generator (if greater or equal than 32).")]
    public partial class CreateTimestampedStartPayload : CreateStartPayload
    {
        /// <summary>
        /// Creates a timestamped message that starts the sound index (if less than 32) or waveform generator (if greater or equal than 32).
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Start register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SoundCard.Start.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that stops the frequency generator process with any input value (not implemented for a sound index). The time precision of this event is frequency dependent: +/- 2500 us at 500 Hz, +/- 750 us at 1000 Hz, +/- 250 us at 2000 Hz, and under +/- 210 us for any frequency above 5000 Hz.
    /// </summary>
    [DisplayName("StopPayload")]
    [Description("Creates a message payload that stops the frequency generator process with any input value (not implemented for a sound index). The time precision of this event is frequency dependent: +/- 2500 us at 500 Hz, +/- 750 us at 1000 Hz, +/- 250 us at 2000 Hz, and under +/- 210 us for any frequency above 5000 Hz.")]
    public partial class CreateStopPayload
    {
        /// <summary>
        /// Gets or sets the value that stops the frequency generator process with any input value (not implemented for a sound index). The time precision of this event is frequency dependent: +/- 2500 us at 500 Hz, +/- 750 us at 1000 Hz, +/- 250 us at 2000 Hz, and under +/- 210 us for any frequency above 5000 Hz.
        /// </summary>
        [Description("The value that stops the frequency generator process with any input value (not implemented for a sound index). The time precision of this event is frequency dependent: +/- 2500 us at 500 Hz, +/- 750 us at 1000 Hz, +/- 250 us at 2000 Hz, and under +/- 210 us for any frequency above 5000 Hz.")]
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
        /// Creates a message that stops the frequency generator process with any input value (not implemented for a sound index). The time precision of this event is frequency dependent: +/- 2500 us at 500 Hz, +/- 750 us at 1000 Hz, +/- 250 us at 2000 Hz, and under +/- 210 us for any frequency above 5000 Hz.
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
    /// that stops the frequency generator process with any input value (not implemented for a sound index). The time precision of this event is frequency dependent: +/- 2500 us at 500 Hz, +/- 750 us at 1000 Hz, +/- 250 us at 2000 Hz, and under +/- 210 us for any frequency above 5000 Hz.
    /// </summary>
    [DisplayName("TimestampedStopPayload")]
    [Description("Creates a timestamped message payload that stops the frequency generator process with any input value (not implemented for a sound index). The time precision of this event is frequency dependent: +/- 2500 us at 500 Hz, +/- 750 us at 1000 Hz, +/- 250 us at 2000 Hz, and under +/- 210 us for any frequency above 5000 Hz.")]
    public partial class CreateTimestampedStopPayload : CreateStopPayload
    {
        /// <summary>
        /// Creates a timestamped message that stops the frequency generator process with any input value (not implemented for a sound index). The time precision of this event is frequency dependent: +/- 2500 us at 500 Hz, +/- 750 us at 1000 Hz, +/- 250 us at 2000 Hz, and under +/- 210 us for any frequency above 5000 Hz.
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
    /// that specifies the attenuation for the left channel (1 LSB is 0.1dB).
    /// </summary>
    [DisplayName("AttenuationLeftPayload")]
    [Description("Creates a message payload that specifies the attenuation for the left channel (1 LSB is 0.1dB).")]
    public partial class CreateAttenuationLeftPayload
    {
        /// <summary>
        /// Gets or sets the value that specifies the attenuation for the left channel (1 LSB is 0.1dB).
        /// </summary>
        [Description("The value that specifies the attenuation for the left channel (1 LSB is 0.1dB).")]
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
        /// Creates a message that specifies the attenuation for the left channel (1 LSB is 0.1dB).
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
    /// that specifies the attenuation for the left channel (1 LSB is 0.1dB).
    /// </summary>
    [DisplayName("TimestampedAttenuationLeftPayload")]
    [Description("Creates a timestamped message payload that specifies the attenuation for the left channel (1 LSB is 0.1dB).")]
    public partial class CreateTimestampedAttenuationLeftPayload : CreateAttenuationLeftPayload
    {
        /// <summary>
        /// Creates a timestamped message that specifies the attenuation for the left channel (1 LSB is 0.1dB).
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
    /// that specifies the attenuation for the right channel (1 LSB is 0.1dB).
    /// </summary>
    [DisplayName("AttenuationRightPayload")]
    [Description("Creates a message payload that specifies the attenuation for the right channel (1 LSB is 0.1dB).")]
    public partial class CreateAttenuationRightPayload
    {
        /// <summary>
        /// Gets or sets the value that specifies the attenuation for the right channel (1 LSB is 0.1dB).
        /// </summary>
        [Description("The value that specifies the attenuation for the right channel (1 LSB is 0.1dB).")]
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
        /// Creates a message that specifies the attenuation for the right channel (1 LSB is 0.1dB).
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
    /// that specifies the attenuation for the right channel (1 LSB is 0.1dB).
    /// </summary>
    [DisplayName("TimestampedAttenuationRightPayload")]
    [Description("Creates a timestamped message payload that specifies the attenuation for the right channel (1 LSB is 0.1dB).")]
    public partial class CreateTimestampedAttenuationRightPayload : CreateAttenuationRightPayload
    {
        /// <summary>
        /// Creates a timestamped message that specifies the attenuation for the right channel (1 LSB is 0.1dB).
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
    /// that specifies the attenuation for the right and left channels simultaneously [Att R] [Att L] (1 LSB is 0.1dB).
    /// </summary>
    [DisplayName("AttenuationBothPayload")]
    [Description("Creates a message payload that specifies the attenuation for the right and left channels simultaneously [Att R] [Att L] (1 LSB is 0.1dB).")]
    public partial class CreateAttenuationBothPayload
    {
        /// <summary>
        /// Gets or sets the value that specifies the attenuation for the right and left channels simultaneously [Att R] [Att L] (1 LSB is 0.1dB).
        /// </summary>
        [Description("The value that specifies the attenuation for the right and left channels simultaneously [Att R] [Att L] (1 LSB is 0.1dB).")]
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
        /// Creates a message that specifies the attenuation for the right and left channels simultaneously [Att R] [Att L] (1 LSB is 0.1dB).
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
    /// that specifies the attenuation for the right and left channels simultaneously [Att R] [Att L] (1 LSB is 0.1dB).
    /// </summary>
    [DisplayName("TimestampedAttenuationBothPayload")]
    [Description("Creates a timestamped message payload that specifies the attenuation for the right and left channels simultaneously [Att R] [Att L] (1 LSB is 0.1dB).")]
    public partial class CreateTimestampedAttenuationBothPayload : CreateAttenuationBothPayload
    {
        /// <summary>
        /// Creates a timestamped message that specifies the attenuation for the right and left channels simultaneously [Att R] [Att L] (1 LSB is 0.1dB).
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
    /// that specifies attenuation and simultaneously starts the sound index or frequency [Att R] [Att L] [Index].
    /// </summary>
    [DisplayName("AttenuationStartPayload")]
    [Description("Creates a message payload that specifies attenuation and simultaneously starts the sound index or frequency [Att R] [Att L] [Index].")]
    public partial class CreateAttenuationStartPayload
    {
        /// <summary>
        /// Gets or sets the value that specifies attenuation and simultaneously starts the sound index or frequency [Att R] [Att L] [Index].
        /// </summary>
        [Description("The value that specifies attenuation and simultaneously starts the sound index or frequency [Att R] [Att L] [Index].")]
        public ushort[] AttenuationStart { get; set; }

        /// <summary>
        /// Creates a message payload for the AttenuationStart register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort[] GetPayload()
        {
            return AttenuationStart;
        }

        /// <summary>
        /// Creates a message that specifies attenuation and simultaneously starts the sound index or frequency [Att R] [Att L] [Index].
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the AttenuationStart register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SoundCard.AttenuationStart.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that specifies attenuation and simultaneously starts the sound index or frequency [Att R] [Att L] [Index].
    /// </summary>
    [DisplayName("TimestampedAttenuationStartPayload")]
    [Description("Creates a timestamped message payload that specifies attenuation and simultaneously starts the sound index or frequency [Att R] [Att L] [Index].")]
    public partial class CreateTimestampedAttenuationStartPayload : CreateAttenuationStartPayload
    {
        /// <summary>
        /// Creates a timestamped message that specifies attenuation and simultaneously starts the sound index or frequency [Att R] [Att L] [Index].
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the AttenuationStart register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SoundCard.AttenuationStart.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that reports the state of the digital inputs.
    /// </summary>
    [DisplayName("InputStatePayload")]
    [Description("Creates a message payload that reports the state of the digital inputs.")]
    public partial class CreateInputStatePayload
    {
        /// <summary>
        /// Gets or sets the value that reports the state of the digital inputs.
        /// </summary>
        [Description("The value that reports the state of the digital inputs.")]
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
        /// Creates a message that reports the state of the digital inputs.
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
    /// that reports the state of the digital inputs.
    /// </summary>
    [DisplayName("TimestampedInputStatePayload")]
    [Description("Creates a timestamped message payload that reports the state of the digital inputs.")]
    public partial class CreateTimestampedInputStatePayload : CreateInputStatePayload
    {
        /// <summary>
        /// Creates a timestamped message that reports the state of the digital inputs.
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
    /// that specifies the configuration for the digital input 0 (DI0).
    /// </summary>
    [DisplayName("ConfigureDI0Payload")]
    [Description("Creates a message payload that specifies the configuration for the digital input 0 (DI0).")]
    public partial class CreateConfigureDI0Payload
    {
        /// <summary>
        /// Gets or sets the value that specifies the configuration for the digital input 0 (DI0).
        /// </summary>
        [Description("The value that specifies the configuration for the digital input 0 (DI0).")]
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
        /// Creates a message that specifies the configuration for the digital input 0 (DI0).
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
    /// that specifies the configuration for the digital input 0 (DI0).
    /// </summary>
    [DisplayName("TimestampedConfigureDI0Payload")]
    [Description("Creates a timestamped message payload that specifies the configuration for the digital input 0 (DI0).")]
    public partial class CreateTimestampedConfigureDI0Payload : CreateConfigureDI0Payload
    {
        /// <summary>
        /// Creates a timestamped message that specifies the configuration for the digital input 0 (DI0).
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
    /// that specifies the configuration for the digital input 1 (DI1).
    /// </summary>
    [DisplayName("ConfigureDI1Payload")]
    [Description("Creates a message payload that specifies the configuration for the digital input 1 (DI1).")]
    public partial class CreateConfigureDI1Payload
    {
        /// <summary>
        /// Gets or sets the value that specifies the configuration for the digital input 1 (DI1).
        /// </summary>
        [Description("The value that specifies the configuration for the digital input 1 (DI1).")]
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
        /// Creates a message that specifies the configuration for the digital input 1 (DI1).
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
    /// that specifies the configuration for the digital input 1 (DI1).
    /// </summary>
    [DisplayName("TimestampedConfigureDI1Payload")]
    [Description("Creates a timestamped message payload that specifies the configuration for the digital input 1 (DI1).")]
    public partial class CreateTimestampedConfigureDI1Payload : CreateConfigureDI1Payload
    {
        /// <summary>
        /// Creates a timestamped message that specifies the configuration for the digital input 1 (DI1).
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
    /// that specifies the sound index to be played when triggering DI0.
    /// </summary>
    [DisplayName("StartDI0Payload")]
    [Description("Creates a message payload that specifies the sound index to be played when triggering DI0.")]
    public partial class CreateStartDI0Payload
    {
        /// <summary>
        /// Gets or sets the value that specifies the sound index to be played when triggering DI0.
        /// </summary>
        [Description("The value that specifies the sound index to be played when triggering DI0.")]
        public ushort StartDI0 { get; set; }

        /// <summary>
        /// Creates a message payload for the StartDI0 register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort GetPayload()
        {
            return StartDI0;
        }

        /// <summary>
        /// Creates a message that specifies the sound index to be played when triggering DI0.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the StartDI0 register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SoundCard.StartDI0.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that specifies the sound index to be played when triggering DI0.
    /// </summary>
    [DisplayName("TimestampedStartDI0Payload")]
    [Description("Creates a timestamped message payload that specifies the sound index to be played when triggering DI0.")]
    public partial class CreateTimestampedStartDI0Payload : CreateStartDI0Payload
    {
        /// <summary>
        /// Creates a timestamped message that specifies the sound index to be played when triggering DI0.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the StartDI0 register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SoundCard.StartDI0.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that specifies the sound index to be played when triggering DI1.
    /// </summary>
    [DisplayName("StartDI1Payload")]
    [Description("Creates a message payload that specifies the sound index to be played when triggering DI1.")]
    public partial class CreateStartDI1Payload
    {
        /// <summary>
        /// Gets or sets the value that specifies the sound index to be played when triggering DI1.
        /// </summary>
        [Description("The value that specifies the sound index to be played when triggering DI1.")]
        public ushort StartDI1 { get; set; }

        /// <summary>
        /// Creates a message payload for the StartDI1 register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort GetPayload()
        {
            return StartDI1;
        }

        /// <summary>
        /// Creates a message that specifies the sound index to be played when triggering DI1.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the StartDI1 register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SoundCard.StartDI1.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that specifies the sound index to be played when triggering DI1.
    /// </summary>
    [DisplayName("TimestampedStartDI1Payload")]
    [Description("Creates a timestamped message payload that specifies the sound index to be played when triggering DI1.")]
    public partial class CreateTimestampedStartDI1Payload : CreateStartDI1Payload
    {
        /// <summary>
        /// Creates a timestamped message that specifies the sound index to be played when triggering DI1.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the StartDI1 register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SoundCard.StartDI1.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that specifies the attenuation to set on the left channel (1 LSB is 0.1dB) when triggering DI0.
    /// </summary>
    [DisplayName("AttenuationLeftDI0Payload")]
    [Description("Creates a message payload that specifies the attenuation to set on the left channel (1 LSB is 0.1dB) when triggering DI0.")]
    public partial class CreateAttenuationLeftDI0Payload
    {
        /// <summary>
        /// Gets or sets the value that specifies the attenuation to set on the left channel (1 LSB is 0.1dB) when triggering DI0.
        /// </summary>
        [Description("The value that specifies the attenuation to set on the left channel (1 LSB is 0.1dB) when triggering DI0.")]
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
        /// Creates a message that specifies the attenuation to set on the left channel (1 LSB is 0.1dB) when triggering DI0.
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
    /// that specifies the attenuation to set on the left channel (1 LSB is 0.1dB) when triggering DI0.
    /// </summary>
    [DisplayName("TimestampedAttenuationLeftDI0Payload")]
    [Description("Creates a timestamped message payload that specifies the attenuation to set on the left channel (1 LSB is 0.1dB) when triggering DI0.")]
    public partial class CreateTimestampedAttenuationLeftDI0Payload : CreateAttenuationLeftDI0Payload
    {
        /// <summary>
        /// Creates a timestamped message that specifies the attenuation to set on the left channel (1 LSB is 0.1dB) when triggering DI0.
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
    /// that specifies the attenuation to set on the left channel (1 LSB is 0.1dB) when triggering DI1.
    /// </summary>
    [DisplayName("AttenuationLeftDI1Payload")]
    [Description("Creates a message payload that specifies the attenuation to set on the left channel (1 LSB is 0.1dB) when triggering DI1.")]
    public partial class CreateAttenuationLeftDI1Payload
    {
        /// <summary>
        /// Gets or sets the value that specifies the attenuation to set on the left channel (1 LSB is 0.1dB) when triggering DI1.
        /// </summary>
        [Description("The value that specifies the attenuation to set on the left channel (1 LSB is 0.1dB) when triggering DI1.")]
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
        /// Creates a message that specifies the attenuation to set on the left channel (1 LSB is 0.1dB) when triggering DI1.
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
    /// that specifies the attenuation to set on the left channel (1 LSB is 0.1dB) when triggering DI1.
    /// </summary>
    [DisplayName("TimestampedAttenuationLeftDI1Payload")]
    [Description("Creates a timestamped message payload that specifies the attenuation to set on the left channel (1 LSB is 0.1dB) when triggering DI1.")]
    public partial class CreateTimestampedAttenuationLeftDI1Payload : CreateAttenuationLeftDI1Payload
    {
        /// <summary>
        /// Creates a timestamped message that specifies the attenuation to set on the left channel (1 LSB is 0.1dB) when triggering DI1.
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
    /// that specifies the attenuation to set on the right channel (1 LSB is 0.1dB) when triggering DI0.
    /// </summary>
    [DisplayName("AttenuationRightDI0Payload")]
    [Description("Creates a message payload that specifies the attenuation to set on the right channel (1 LSB is 0.1dB) when triggering DI0.")]
    public partial class CreateAttenuationRightDI0Payload
    {
        /// <summary>
        /// Gets or sets the value that specifies the attenuation to set on the right channel (1 LSB is 0.1dB) when triggering DI0.
        /// </summary>
        [Description("The value that specifies the attenuation to set on the right channel (1 LSB is 0.1dB) when triggering DI0.")]
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
        /// Creates a message that specifies the attenuation to set on the right channel (1 LSB is 0.1dB) when triggering DI0.
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
    /// that specifies the attenuation to set on the right channel (1 LSB is 0.1dB) when triggering DI0.
    /// </summary>
    [DisplayName("TimestampedAttenuationRightDI0Payload")]
    [Description("Creates a timestamped message payload that specifies the attenuation to set on the right channel (1 LSB is 0.1dB) when triggering DI0.")]
    public partial class CreateTimestampedAttenuationRightDI0Payload : CreateAttenuationRightDI0Payload
    {
        /// <summary>
        /// Creates a timestamped message that specifies the attenuation to set on the right channel (1 LSB is 0.1dB) when triggering DI0.
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
    /// that specifies the attenuation to set on the right channel (1 LSB is 0.1dB) when triggering DI1.
    /// </summary>
    [DisplayName("AttenuationRightDI1Payload")]
    [Description("Creates a message payload that specifies the attenuation to set on the right channel (1 LSB is 0.1dB) when triggering DI1.")]
    public partial class CreateAttenuationRightDI1Payload
    {
        /// <summary>
        /// Gets or sets the value that specifies the attenuation to set on the right channel (1 LSB is 0.1dB) when triggering DI1.
        /// </summary>
        [Description("The value that specifies the attenuation to set on the right channel (1 LSB is 0.1dB) when triggering DI1.")]
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
        /// Creates a message that specifies the attenuation to set on the right channel (1 LSB is 0.1dB) when triggering DI1.
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
    /// that specifies the attenuation to set on the right channel (1 LSB is 0.1dB) when triggering DI1.
    /// </summary>
    [DisplayName("TimestampedAttenuationRightDI1Payload")]
    [Description("Creates a timestamped message payload that specifies the attenuation to set on the right channel (1 LSB is 0.1dB) when triggering DI1.")]
    public partial class CreateTimestampedAttenuationRightDI1Payload : CreateAttenuationRightDI1Payload
    {
        /// <summary>
        /// Creates a timestamped message that specifies the attenuation to set on the right channel (1 LSB is 0.1dB) when triggering DI1.
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
    /// that specifies the configuration of the digital output 0 (DO0).
    /// </summary>
    [DisplayName("ConfigureDO0Payload")]
    [Description("Creates a message payload that specifies the configuration of the digital output 0 (DO0).")]
    public partial class CreateConfigureDO0Payload
    {
        /// <summary>
        /// Gets or sets the value that specifies the configuration of the digital output 0 (DO0).
        /// </summary>
        [Description("The value that specifies the configuration of the digital output 0 (DO0).")]
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
        /// Creates a message that specifies the configuration of the digital output 0 (DO0).
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
    /// that specifies the configuration of the digital output 0 (DO0).
    /// </summary>
    [DisplayName("TimestampedConfigureDO0Payload")]
    [Description("Creates a timestamped message payload that specifies the configuration of the digital output 0 (DO0).")]
    public partial class CreateTimestampedConfigureDO0Payload : CreateConfigureDO0Payload
    {
        /// <summary>
        /// Creates a timestamped message that specifies the configuration of the digital output 0 (DO0).
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
    /// that specifies the configuration of the digital output 1 (DO1).
    /// </summary>
    [DisplayName("ConfigureDO1Payload")]
    [Description("Creates a message payload that specifies the configuration of the digital output 1 (DO1).")]
    public partial class CreateConfigureDO1Payload
    {
        /// <summary>
        /// Gets or sets the value that specifies the configuration of the digital output 1 (DO1).
        /// </summary>
        [Description("The value that specifies the configuration of the digital output 1 (DO1).")]
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
        /// Creates a message that specifies the configuration of the digital output 1 (DO1).
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
    /// that specifies the configuration of the digital output 1 (DO1).
    /// </summary>
    [DisplayName("TimestampedConfigureDO1Payload")]
    [Description("Creates a timestamped message payload that specifies the configuration of the digital output 1 (DO1).")]
    public partial class CreateTimestampedConfigureDO1Payload : CreateConfigureDO1Payload
    {
        /// <summary>
        /// Creates a timestamped message that specifies the configuration of the digital output 1 (DO1).
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
    /// that specifies the configuration of the digital output 2 (DO2).
    /// </summary>
    [DisplayName("ConfigureDO2Payload")]
    [Description("Creates a message payload that specifies the configuration of the digital output 2 (DO2).")]
    public partial class CreateConfigureDO2Payload
    {
        /// <summary>
        /// Gets or sets the value that specifies the configuration of the digital output 2 (DO2).
        /// </summary>
        [Description("The value that specifies the configuration of the digital output 2 (DO2).")]
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
        /// Creates a message that specifies the configuration of the digital output 2 (DO2).
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
    /// that specifies the configuration of the digital output 2 (DO2).
    /// </summary>
    [DisplayName("TimestampedConfigureDO2Payload")]
    [Description("Creates a timestamped message payload that specifies the configuration of the digital output 2 (DO2).")]
    public partial class CreateTimestampedConfigureDO2Payload : CreateConfigureDO2Payload
    {
        /// <summary>
        /// Creates a timestamped message that specifies the configuration of the digital output 2 (DO2).
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
    /// that sets the specified digital output lines.
    /// </summary>
    [DisplayName("OutputSetPayload")]
    [Description("Creates a message payload that sets the specified digital output lines.")]
    public partial class CreateOutputSetPayload
    {
        /// <summary>
        /// Gets or sets the value that sets the specified digital output lines.
        /// </summary>
        [Description("The value that sets the specified digital output lines.")]
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
        /// Creates a message that sets the specified digital output lines.
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
    /// that sets the specified digital output lines.
    /// </summary>
    [DisplayName("TimestampedOutputSetPayload")]
    [Description("Creates a timestamped message payload that sets the specified digital output lines.")]
    public partial class CreateTimestampedOutputSetPayload : CreateOutputSetPayload
    {
        /// <summary>
        /// Creates a timestamped message that sets the specified digital output lines.
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
    /// that clears the specified digital output lines.
    /// </summary>
    [DisplayName("OutputClearPayload")]
    [Description("Creates a message payload that clears the specified digital output lines.")]
    public partial class CreateOutputClearPayload
    {
        /// <summary>
        /// Gets or sets the value that clears the specified digital output lines.
        /// </summary>
        [Description("The value that clears the specified digital output lines.")]
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
        /// Creates a message that clears the specified digital output lines.
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
    /// that clears the specified digital output lines.
    /// </summary>
    [DisplayName("TimestampedOutputClearPayload")]
    [Description("Creates a timestamped message payload that clears the specified digital output lines.")]
    public partial class CreateTimestampedOutputClearPayload : CreateOutputClearPayload
    {
        /// <summary>
        /// Creates a timestamped message that clears the specified digital output lines.
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
    /// that toggles the specified digital output lines.
    /// </summary>
    [DisplayName("OutputTogglePayload")]
    [Description("Creates a message payload that toggles the specified digital output lines.")]
    public partial class CreateOutputTogglePayload
    {
        /// <summary>
        /// Gets or sets the value that toggles the specified digital output lines.
        /// </summary>
        [Description("The value that toggles the specified digital output lines.")]
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
        /// Creates a message that toggles the specified digital output lines.
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
    /// that toggles the specified digital output lines.
    /// </summary>
    [DisplayName("TimestampedOutputTogglePayload")]
    [Description("Creates a timestamped message payload that toggles the specified digital output lines.")]
    public partial class CreateTimestampedOutputTogglePayload : CreateOutputTogglePayload
    {
        /// <summary>
        /// Creates a timestamped message that toggles the specified digital output lines.
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
    /// that writes the state of all digital output lines.
    /// </summary>
    [DisplayName("OutputStatePayload")]
    [Description("Creates a message payload that writes the state of all digital output lines.")]
    public partial class CreateOutputStatePayload
    {
        /// <summary>
        /// Gets or sets the value that writes the state of all digital output lines.
        /// </summary>
        [Description("The value that writes the state of all digital output lines.")]
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
        /// Creates a message that writes the state of all digital output lines.
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
    /// that writes the state of all digital output lines.
    /// </summary>
    [DisplayName("TimestampedOutputStatePayload")]
    [Description("Creates a timestamped message payload that writes the state of all digital output lines.")]
    public partial class CreateTimestampedOutputStatePayload : CreateOutputStatePayload
    {
        /// <summary>
        /// Creates a timestamped message that writes the state of all digital output lines.
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
    /// that specifies the configuration of the ADC control stream.
    /// </summary>
    [DisplayName("EnableAdcControlStatePayload")]
    [Description("Creates a message payload that specifies the configuration of the ADC control stream.")]
    public partial class CreateEnableAdcControlStatePayload
    {
        /// <summary>
        /// Gets or sets the value that specifies the configuration of the ADC control stream.
        /// </summary>
        [Description("The value that specifies the configuration of the ADC control stream.")]
        public AdcControlStateConfiguration EnableAdcControlState { get; set; }

        /// <summary>
        /// Creates a message payload for the EnableAdcControlState register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public AdcControlStateConfiguration GetPayload()
        {
            return EnableAdcControlState;
        }

        /// <summary>
        /// Creates a message that specifies the configuration of the ADC control stream.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the EnableAdcControlState register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SoundCard.EnableAdcControlState.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that specifies the configuration of the ADC control stream.
    /// </summary>
    [DisplayName("TimestampedEnableAdcControlStatePayload")]
    [Description("Creates a timestamped message payload that specifies the configuration of the ADC control stream.")]
    public partial class CreateTimestampedEnableAdcControlStatePayload : CreateEnableAdcControlStatePayload
    {
        /// <summary>
        /// Creates a timestamped message that specifies the configuration of the ADC control stream.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the EnableAdcControlState register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SoundCard.EnableAdcControlState.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that reports sampled analog input data and current sound parameters.
    /// </summary>
    [DisplayName("AdcControlStatePayload")]
    [Description("Creates a message payload that reports sampled analog input data and current sound parameters.")]
    public partial class CreateAdcControlStatePayload
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
        /// Gets or sets a value that the current amplitude of the left channel.
        /// </summary>
        [Description("The current amplitude of the left channel.")]
        public ushort AttenuationLeft { get; set; }

        /// <summary>
        /// Gets or sets a value that the current amplitude of the right channel.
        /// </summary>
        [Description("The current amplitude of the right channel.")]
        public ushort AttenuationRight { get; set; }

        /// <summary>
        /// Gets or sets a value that the output sound index (if less than 32) or frequency (if greater or equal than 32) being played.
        /// </summary>
        [Description("The output sound index (if less than 32) or frequency (if greater or equal than 32) being played.")]
        public ushort Frequency { get; set; }

        /// <summary>
        /// Creates a message payload for the AdcControlState register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public AdcControlStatePayload GetPayload()
        {
            AdcControlStatePayload value;
            value.Adc0 = Adc0;
            value.Adc1 = Adc1;
            value.AttenuationLeft = AttenuationLeft;
            value.AttenuationRight = AttenuationRight;
            value.Frequency = Frequency;
            return value;
        }

        /// <summary>
        /// Creates a message that reports sampled analog input data and current sound parameters.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the AdcControlState register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SoundCard.AdcControlState.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that reports sampled analog input data and current sound parameters.
    /// </summary>
    [DisplayName("TimestampedAdcControlStatePayload")]
    [Description("Creates a timestamped message payload that reports sampled analog input data and current sound parameters.")]
    public partial class CreateTimestampedAdcControlStatePayload : CreateAdcControlStatePayload
    {
        /// <summary>
        /// Creates a timestamped message that reports sampled analog input data and current sound parameters.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the AdcControlState register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SoundCard.AdcControlState.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that sends commands to the PIC32 micro-controller.
    /// </summary>
    [DisplayName("Pic32CommandsPayload")]
    [Description("Creates a message payload that sends commands to the PIC32 micro-controller.")]
    public partial class CreatePic32CommandsPayload
    {
        /// <summary>
        /// Gets or sets the value that sends commands to the PIC32 micro-controller.
        /// </summary>
        [Description("The value that sends commands to the PIC32 micro-controller.")]
        public Pic32Command Pic32Commands { get; set; }

        /// <summary>
        /// Creates a message payload for the Pic32Commands register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public Pic32Command GetPayload()
        {
            return Pic32Commands;
        }

        /// <summary>
        /// Creates a message that sends commands to the PIC32 micro-controller.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Pic32Commands register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.SoundCard.Pic32Commands.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that sends commands to the PIC32 micro-controller.
    /// </summary>
    [DisplayName("TimestampedPic32CommandsPayload")]
    [Description("Creates a timestamped message payload that sends commands to the PIC32 micro-controller.")]
    public partial class CreateTimestampedPic32CommandsPayload : CreatePic32CommandsPayload
    {
        /// <summary>
        /// Creates a timestamped message that sends commands to the PIC32 micro-controller.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Pic32Commands register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.SoundCard.Pic32Commands.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents the payload of the AdcControlState register.
    /// </summary>
    public struct AdcControlStatePayload
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdcControlStatePayload"/> structure.
        /// </summary>
        /// <param name="adc0">The sampled analog input value on ADC0.</param>
        /// <param name="adc1">The sampled analog input value on ADC1.</param>
        /// <param name="attenuationLeft">The current amplitude of the left channel.</param>
        /// <param name="attenuationRight">The current amplitude of the right channel.</param>
        /// <param name="frequency">The output sound index (if less than 32) or frequency (if greater or equal than 32) being played.</param>
        public AdcControlStatePayload(
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
        /// The current amplitude of the left channel.
        /// </summary>
        public ushort AttenuationLeft;

        /// <summary>
        /// The current amplitude of the right channel.
        /// </summary>
        public ushort AttenuationRight;

        /// <summary>
        /// The output sound index (if less than 32) or frequency (if greater or equal than 32) being played.
        /// </summary>
        public ushort Frequency;

        /// <summary>
        /// Returns a <see cref="string"/> that represents the payload of
        /// the AdcControlState register.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> that represents the payload of the
        /// AdcControlState register.
        /// </returns>
        public override string ToString()
        {
            return "AdcControlStatePayload { " +
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
        /// <summary>
        /// Specifies that no flags are defined.
        /// </summary>
        None = 0x0,

        /// <summary>
        /// Digital input 0.
        /// </summary>
        [Description("Digital input 0.")]
        DI0 = 0x1,

        /// <summary>
        /// Digital input 1.
        /// </summary>
        [Description("Digital input 1.")]
        DI1 = 0x2,

        /// <summary>
        /// Digital input 2 (no associated event).
        /// </summary>
        [Description("Digital input 2 (no associated event).")]
        DI2 = 0x4
    }

    /// <summary>
    /// Specifies the state of the digital output lines.
    /// </summary>
    [Flags]
    public enum DigitalOutputs : byte
    {
        /// <summary>
        /// Specifies that no flags are defined.
        /// </summary>
        None = 0x0,

        /// <summary>
        /// Digital output 0.
        /// </summary>
        [Description("Digital output 0.")]
        DO0 = 0x1,

        /// <summary>
        /// Digital output 1.
        /// </summary>
        [Description("Digital output 1.")]
        DO1 = 0x2,

        /// <summary>
        /// Digital output 2.
        /// </summary>
        [Description("Digital output 2.")]
        DO2 = 0x4
    }

    /// <summary>
    /// Specifies the operation mode of the digital input.
    /// </summary>
    public enum DigitalInputConfiguration : byte
    {
        /// <summary>
        /// Used as a pure digital input.
        /// </summary>
        [Description("Used as a pure digital input.")]
        Digital = 0,

        /// <summary>
        /// Starts sound index (if less than 32) or frequency (if greater or equal than 32) when rising edge and stop when falling edge (frequency only).
        /// </summary>
        [Description("Starts sound index (if less than 32) or frequency (if greater or equal than 32) when rising edge and stop when falling edge (frequency only).")]
        StartAndStop = 1,

        /// <summary>
        /// Starts sound index (if less than 32) or frequency (if greater or equal than 32) when rising edge.
        /// </summary>
        [Description("Starts sound index (if less than 32) or frequency (if greater or equal than 32) when rising edge.")]
        Start = 2,

        /// <summary>
        /// Stops frequency when rising edge.
        /// </summary>
        [Description("Stops frequency when rising edge.")]
        Stop = 3
    }

    /// <summary>
    /// Specifies the operation mode of the digital output.
    /// </summary>
    public enum DigitalOutputConfiguration : byte
    {
        /// <summary>
        /// Used as a pure digital output.
        /// </summary>
        [Description("Used as a pure digital output.")]
        Digital = 0,

        /// <summary>
        /// Positive 500us pulse when sound starts or frequency changes.
        /// </summary>
        [Description("Positive 500us pulse when sound starts or frequency changes.")]
        PulseOnStart = 1
    }

    /// <summary>
    /// Specifies the operation mode of the ADC control stream.
    /// </summary>
    public enum AdcControlStateConfiguration : byte
    {
        /// <summary>
        /// The ADC control state register is disabled.
        /// </summary>
        [Description("The ADC control state register is disabled.")]
        Disabled = 0,

        /// <summary>
        /// The ADC control state will be sampled at 1 kHz.
        /// </summary>
        [Description("The ADC control state will be sampled at 1 kHz.")]
        SampleRate1000Hz = 1
    }

    /// <summary>
    /// Specifies the operation mode of the analog input ADC0.
    /// </summary>
    public enum Adc0Configuration : byte
    {
        /// <summary>
        /// Used as a pure analog input.
        /// </summary>
        [Description("Used as a pure analog input.")]
        AnalogInputOnly = 0,

        /// <summary>
        /// Controls the attenuation of the left channel.
        /// </summary>
        [Description("Controls the attenuation of the left channel.")]
        ControlAttenuationLeft = 1,

        /// <summary>
        /// Controls the attenuation of the right channel.
        /// </summary>
        [Description("Controls the attenuation of the right channel.")]
        ControlAttenuationRight = 2
    }

    /// <summary>
    /// Specifies the operation mode of the analog input ADC1.
    /// </summary>
    public enum Adc1Configuration : byte
    {
        /// <summary>
        /// Used as a pure analog input.
        /// </summary>
        [Description("Used as a pure analog input.")]
        AnalogInputOnly = 0,

        /// <summary>
        /// Controls the attenuation of the right channel.
        /// </summary>
        [Description("Controls the attenuation of the right channel.")]
        ControlAttenuationRight = 1,

        /// <summary>
        /// Controls the attenuation of both channels.
        /// </summary>
        [Description("Controls the attenuation of both channels.")]
        ControlAttenuationBoth = 2,

        /// <summary>
        /// Controls the frequency of the waveform generator.
        /// </summary>
        [Description("Controls the frequency of the waveform generator.")]
        ControlFrequencyGenerator = 3
    }

    /// <summary>
    /// Specifies commands to send to the PIC32 micro-controller
    /// </summary>
    public enum Pic32Command : byte
    {
        /// <summary>
        /// Disable the PIC32 bootloader mode.
        /// </summary>
        [Description("Disable the PIC32 bootloader mode.")]
        DisableBootloader = 0,

        /// <summary>
        /// Enable the PIC32 bootloader mode.
        /// </summary>
        [Description("Enable the PIC32 bootloader mode.")]
        EnableBootloader = 1,

        /// <summary>
        /// Delete all sounds stored in the PIC32 micro-controller.
        /// </summary>
        [Description("Delete all sounds stored in the PIC32 micro-controller.")]
        DeleteAllSounds = 255
    }
}
