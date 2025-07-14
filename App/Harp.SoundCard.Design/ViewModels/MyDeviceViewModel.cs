using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Bonsai.Harp;
using Harp.SoundCard.Design.Views;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Harp.SoundCard.Design.ViewModels;


public class SoundCardViewModel : ViewModelBase
{
    public string AppVersion { get; set; }
    public ReactiveCommand<Unit, Unit> LoadDeviceInformation { get; }

    #region Connection Information

    [Reactive] public ObservableCollection<string> Ports { get; set; }
    [Reactive] public string SelectedPort { get; set; }
    [Reactive] public bool Connected { get; set; }
    [Reactive] public string ConnectButtonText { get; set; } = "Connect";
    public ReactiveCommand<Unit, Unit> ConnectAndGetBaseInfoCommand { get; }

    #endregion

    #region Operations

    public ReactiveCommand<bool, Unit> SaveConfigurationCommand { get; }
    public ReactiveCommand<Unit, Unit> ResetConfigurationCommand { get; }

    #endregion

    #region Device basic information

    [Reactive] public int DeviceID { get; set; }
    [Reactive] public string DeviceName { get; set; }
    [Reactive] public HarpVersion HardwareVersion { get; set; }
    [Reactive] public HarpVersion FirmwareVersion { get; set; }
    [Reactive] public int SerialNumber { get; set; }

    #endregion

    #region Registers

    [Reactive] public ushort PlaySoundOrFrequency { get; set; }
    [Reactive] public byte Stop { get; set; }
    [Reactive] public ushort AttenuationLeft { get; set; }
    [Reactive] public ushort AttenuationRight { get; set; }
    [Reactive] public ushort[] AttenuationBoth { get; set; }
    [Reactive] public ushort[] AttenuationAndPlaySoundOrFreq { get; set; }
    [Reactive] public DigitalInputs InputState { get; set; }
    [Reactive] public DigitalInputConfiguration ConfigureDI0 { get; set; }
    [Reactive] public DigitalInputConfiguration ConfigureDI1 { get; set; }
    [Reactive] public DigitalInputConfiguration ConfigureDI2 { get; set; }
    [Reactive] public byte SoundIndexDI0 { get; set; }
    [Reactive] public byte SoundIndexDI1 { get; set; }
    [Reactive] public byte SoundIndexDI2 { get; set; }
    [Reactive] public ushort FrequencyDI0 { get; set; }
    [Reactive] public ushort FrequencyDI1 { get; set; }
    [Reactive] public ushort FrequencyDI2 { get; set; }
    [Reactive] public ushort AttenuationLeftDI0 { get; set; }
    [Reactive] public ushort AttenuationLeftDI1 { get; set; }
    [Reactive] public ushort AttenuationLeftDI2 { get; set; }
    [Reactive] public ushort AttenuationRightDI0 { get; set; }
    [Reactive] public ushort AttenuationRightDI1 { get; set; }
    [Reactive] public ushort AttenuationRightDI2 { get; set; }
    [Reactive] public ushort[] AttenuationAndSoundIndexDI0 { get; set; }
    [Reactive] public ushort[] AttenuationAndSoundIndexDI1 { get; set; }
    [Reactive] public ushort[] AttenuationAndSoundIndexDI2 { get; set; }
    [Reactive] public ushort[] AttenuationAndFrequencyDI0 { get; set; }
    [Reactive] public ushort[] AttenuationAndFrequencyDI1 { get; set; }
    [Reactive] public ushort[] AttenuationAndFrequencyDI2 { get; set; }
    [Reactive] public DigitalOutputConfiguration ConfigureDO0 { get; set; }
    [Reactive] public DigitalOutputConfiguration ConfigureDO1 { get; set; }
    [Reactive] public DigitalOutputConfiguration ConfigureDO2 { get; set; }
    [Reactive] public byte PulseDO0 { get; set; }
    [Reactive] public byte PulseDO1 { get; set; }
    [Reactive] public byte PulseDO2 { get; set; }
    [Reactive] public DigitalOutputs OutputSet { get; set; }
    [Reactive] public DigitalOutputs OutputClear { get; set; }
    [Reactive] public DigitalOutputs OutputToggle { get; set; }
    [Reactive] public DigitalOutputs OutputState { get; set; }
    [Reactive] public AdcConfiguration ConfigureAdc { get; set; }
    [Reactive] public AnalogDataPayload AnalogData { get; set; }
    [Reactive] public ControllerCommand Commands { get; set; }
    [Reactive] public SoundCardEvents EnableEvents { get; set; }

    #endregion

    #region Array collections

    [Reactive] public ObservableCollection<ArrayItemWrapper<ushort>> AttenuationBothCollection { get; private set; } = new();
    [Reactive] public ObservableCollection<ArrayItemWrapper<ushort>> AttenuationAndPlaySoundOrFreqCollection { get; private set; } = new();
    [Reactive] public ObservableCollection<ArrayItemWrapper<ushort>> AttenuationAndSoundIndexDI0Collection { get; private set; } = new();
    [Reactive] public ObservableCollection<ArrayItemWrapper<ushort>> AttenuationAndSoundIndexDI1Collection { get; private set; } = new();
    [Reactive] public ObservableCollection<ArrayItemWrapper<ushort>> AttenuationAndSoundIndexDI2Collection { get; private set; } = new();
    [Reactive] public ObservableCollection<ArrayItemWrapper<ushort>> AttenuationAndFrequencyDI0Collection { get; private set; } = new();
    [Reactive] public ObservableCollection<ArrayItemWrapper<ushort>> AttenuationAndFrequencyDI1Collection { get; private set; } = new();
    [Reactive] public ObservableCollection<ArrayItemWrapper<ushort>> AttenuationAndFrequencyDI2Collection { get; private set; } = new();
    #endregion

    #region Events Flags

    public bool IsPlaySoundOrFrequencyEnabled
    {
        get
        {
            return EnableEvents.HasFlag(SoundCardEvents.PlaySoundOrFrequency);
        }
        set
        {
            if (value)
            {
                EnableEvents |= SoundCardEvents.PlaySoundOrFrequency;
            }
            else
            {
                EnableEvents &= ~SoundCardEvents.PlaySoundOrFrequency;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsPlaySoundOrFrequencyEnabled));
            this.RaisePropertyChanged(nameof(EnableEvents));
        }
    }

    public bool IsStopEnabled
    {
        get
        {
            return EnableEvents.HasFlag(SoundCardEvents.Stop);
        }
        set
        {
            if (value)
            {
                EnableEvents |= SoundCardEvents.Stop;
            }
            else
            {
                EnableEvents &= ~SoundCardEvents.Stop;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsStopEnabled));
            this.RaisePropertyChanged(nameof(EnableEvents));
        }
    }

    public bool IsDigitalInputsEnabled
    {
        get
        {
            return EnableEvents.HasFlag(SoundCardEvents.DigitalInputs);
        }
        set
        {
            if (value)
            {
                EnableEvents |= SoundCardEvents.DigitalInputs;
            }
            else
            {
                EnableEvents &= ~SoundCardEvents.DigitalInputs;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsDigitalInputsEnabled));
            this.RaisePropertyChanged(nameof(EnableEvents));
        }
    }

    public bool IsAdcValuesEnabled
    {
        get
        {
            return EnableEvents.HasFlag(SoundCardEvents.AdcValues);
        }
        set
        {
            if (value)
            {
                EnableEvents |= SoundCardEvents.AdcValues;
            }
            else
            {
                EnableEvents &= ~SoundCardEvents.AdcValues;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsAdcValuesEnabled));
            this.RaisePropertyChanged(nameof(EnableEvents));
        }
    }

    #endregion

    #region DigitalInputs_InputState Flags

    public bool IsDI0Enabled_InputState
    {
        get
        {
            return InputState.HasFlag(DigitalInputs.DI0);
        }
        set
        {
            if (value)
            {
                InputState |= DigitalInputs.DI0;
            }
            else
            {
                InputState &= ~DigitalInputs.DI0;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsDI0Enabled_InputState));
            this.RaisePropertyChanged(nameof(InputState));
        }
    }

    #endregion

    #region DigitalOutputs_OutputSet Flags

    public bool IsDO0Enabled_OutputSet
    {
        get
        {
            return OutputSet.HasFlag(DigitalOutputs.DO0);
        }
        set
        {
            if (value)
            {
                OutputSet |= DigitalOutputs.DO0;
            }
            else
            {
                OutputSet &= ~DigitalOutputs.DO0;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsDO0Enabled_OutputSet));
            this.RaisePropertyChanged(nameof(OutputSet));
        }
    }

    public bool IsDO1Enabled_OutputSet
    {
        get
        {
            return OutputSet.HasFlag(DigitalOutputs.DO1);
        }
        set
        {
            if (value)
            {
                OutputSet |= DigitalOutputs.DO1;
            }
            else
            {
                OutputSet &= ~DigitalOutputs.DO1;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsDO1Enabled_OutputSet));
            this.RaisePropertyChanged(nameof(OutputSet));
        }
    }

    public bool IsDO2Enabled_OutputSet
    {
        get
        {
            return OutputSet.HasFlag(DigitalOutputs.DO2);
        }
        set
        {
            if (value)
            {
                OutputSet |= DigitalOutputs.DO2;
            }
            else
            {
                OutputSet &= ~DigitalOutputs.DO2;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsDO2Enabled_OutputSet));
            this.RaisePropertyChanged(nameof(OutputSet));
        }
    }

    #endregion

    #region DigitalOutputs_OutputClear Flags

    public bool IsDO0Enabled_OutputClear
    {
        get
        {
            return OutputClear.HasFlag(DigitalOutputs.DO0);
        }
        set
        {
            if (value)
            {
                OutputClear |= DigitalOutputs.DO0;
            }
            else
            {
                OutputClear &= ~DigitalOutputs.DO0;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsDO0Enabled_OutputClear));
            this.RaisePropertyChanged(nameof(OutputClear));
        }
    }

    public bool IsDO1Enabled_OutputClear
    {
        get
        {
            return OutputClear.HasFlag(DigitalOutputs.DO1);
        }
        set
        {
            if (value)
            {
                OutputClear |= DigitalOutputs.DO1;
            }
            else
            {
                OutputClear &= ~DigitalOutputs.DO1;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsDO1Enabled_OutputClear));
            this.RaisePropertyChanged(nameof(OutputClear));
        }
    }

    public bool IsDO2Enabled_OutputClear
    {
        get
        {
            return OutputClear.HasFlag(DigitalOutputs.DO2);
        }
        set
        {
            if (value)
            {
                OutputClear |= DigitalOutputs.DO2;
            }
            else
            {
                OutputClear &= ~DigitalOutputs.DO2;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsDO2Enabled_OutputClear));
            this.RaisePropertyChanged(nameof(OutputClear));
        }
    }

    #endregion

    #region DigitalOutputs_OutputToggle Flags

    public bool IsDO0Enabled_OutputToggle
    {
        get
        {
            return OutputToggle.HasFlag(DigitalOutputs.DO0);
        }
        set
        {
            if (value)
            {
                OutputToggle |= DigitalOutputs.DO0;
            }
            else
            {
                OutputToggle &= ~DigitalOutputs.DO0;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsDO0Enabled_OutputToggle));
            this.RaisePropertyChanged(nameof(OutputToggle));
        }
    }

    public bool IsDO1Enabled_OutputToggle
    {
        get
        {
            return OutputToggle.HasFlag(DigitalOutputs.DO1);
        }
        set
        {
            if (value)
            {
                OutputToggle |= DigitalOutputs.DO1;
            }
            else
            {
                OutputToggle &= ~DigitalOutputs.DO1;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsDO1Enabled_OutputToggle));
            this.RaisePropertyChanged(nameof(OutputToggle));
        }
    }

    public bool IsDO2Enabled_OutputToggle
    {
        get
        {
            return OutputToggle.HasFlag(DigitalOutputs.DO2);
        }
        set
        {
            if (value)
            {
                OutputToggle |= DigitalOutputs.DO2;
            }
            else
            {
                OutputToggle &= ~DigitalOutputs.DO2;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsDO2Enabled_OutputToggle));
            this.RaisePropertyChanged(nameof(OutputToggle));
        }
    }

    #endregion

    #region DigitalOutputs_OutputState Flags

    public bool IsDO0Enabled_OutputState
    {
        get
        {
            return OutputState.HasFlag(DigitalOutputs.DO0);
        }
        set
        {
            if (value)
            {
                OutputState |= DigitalOutputs.DO0;
            }
            else
            {
                OutputState &= ~DigitalOutputs.DO0;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsDO0Enabled_OutputState));
            this.RaisePropertyChanged(nameof(OutputState));
        }
    }

    public bool IsDO1Enabled_OutputState
    {
        get
        {
            return OutputState.HasFlag(DigitalOutputs.DO1);
        }
        set
        {
            if (value)
            {
                OutputState |= DigitalOutputs.DO1;
            }
            else
            {
                OutputState &= ~DigitalOutputs.DO1;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsDO1Enabled_OutputState));
            this.RaisePropertyChanged(nameof(OutputState));
        }
    }

    public bool IsDO2Enabled_OutputState
    {
        get
        {
            return OutputState.HasFlag(DigitalOutputs.DO2);
        }
        set
        {
            if (value)
            {
                OutputState |= DigitalOutputs.DO2;
            }
            else
            {
                OutputState &= ~DigitalOutputs.DO2;
            }

            // Notify the UI about the change
            this.RaisePropertyChanged(nameof(IsDO2Enabled_OutputState));
            this.RaisePropertyChanged(nameof(OutputState));
        }
    }

    #endregion

    #region Application State

    [ObservableAsProperty] public bool IsLoadingPorts { get; }
    [ObservableAsProperty] public bool IsConnecting { get; }
    [ObservableAsProperty] public bool IsResetting { get; }
    [ObservableAsProperty] public bool IsSaving { get; }

    [Reactive] public bool ShowWriteMessages { get; set; }
    [Reactive] public ObservableCollection<string> HarpEvents { get; set; } = new ObservableCollection<string>();
    [Reactive] public ObservableCollection<string> SentMessages { get; set; } = new ObservableCollection<string>();

    public ReactiveCommand<Unit, Unit> ShowAboutCommand { get; private set; }
    public ReactiveCommand<Unit, Unit> ClearMessagesCommand { get; private set; }
    public ReactiveCommand<Unit, Unit> ShowMessagesCommand { get; private set; }

    #endregion

    private Harp.SoundCard.AsyncDevice? _device;
    private IObservable<string> _deviceEventsObservable;
    private IDisposable? _deviceEventsSubscription;

    public SoundCardViewModel()
    {
        var assembly = typeof(SoundCardViewModel).Assembly;
        var informationVersion = assembly.GetName().Version;
        if (informationVersion != null)
            AppVersion = $"v{informationVersion.Major}.{informationVersion.Minor}.{informationVersion.Build}";

        Ports = new ObservableCollection<string>();

        ClearMessagesCommand = ReactiveCommand.Create(() => { SentMessages.Clear(); });
        ShowMessagesCommand = ReactiveCommand.Create(() => { ShowWriteMessages = !ShowWriteMessages; });


        LoadDeviceInformation = ReactiveCommand.CreateFromObservable(LoadUsbInformation);
        LoadDeviceInformation.IsExecuting.ToPropertyEx(this, x => x.IsLoadingPorts);
        LoadDeviceInformation.ThrownExceptions.Subscribe(ex =>
            Console.WriteLine($"Error loading device information with exception: {ex.Message}"));
        //Log.Error(ex, "Error loading device information with exception: {Exception}", ex));

        // can connect if there is a selection and also if the new selection is different than the old one
        var canConnect = this.WhenAnyValue(x => x.SelectedPort)
            .Select(selectedPort => !string.IsNullOrEmpty(selectedPort));

        ShowAboutCommand = ReactiveCommand.CreateFromTask(async () =>
                await new About() { DataContext = new AboutViewModel() }.ShowDialog(
                    (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow));

        ConnectAndGetBaseInfoCommand = ReactiveCommand.CreateFromTask(ConnectAndGetBaseInfo, canConnect);
        ConnectAndGetBaseInfoCommand.IsExecuting.ToPropertyEx(this, x => x.IsConnecting);
        ConnectAndGetBaseInfoCommand.ThrownExceptions.Subscribe(ex =>
            //Log.Error(ex, "Error connecting to device with error: {Exception}", ex));
            Console.WriteLine($"Error connecting to device with error: {ex}"));

        var canChangeConfig = this.WhenAnyValue(x => x.Connected).Select(connected => connected);
        // Handle Save and Reset
        SaveConfigurationCommand =
            ReactiveCommand.CreateFromObservable<bool, Unit>(SaveConfiguration, canChangeConfig);
        SaveConfigurationCommand.IsExecuting.ToPropertyEx(this, x => x.IsSaving);
        SaveConfigurationCommand.ThrownExceptions.Subscribe(ex =>
            //Log.Error(ex, "Error saving configuration with error: {Exception}", ex));
            Console.WriteLine($"Error saving configuration with error: {ex}"));

        ResetConfigurationCommand = ReactiveCommand.CreateFromObservable(ResetConfiguration, canChangeConfig);
        ResetConfigurationCommand.IsExecuting.ToPropertyEx(this, x => x.IsResetting);
        ResetConfigurationCommand.ThrownExceptions.Subscribe(ex =>
            //Log.Error(ex, "Error resetting device configuration with error: {Exception}", ex));
            Console.WriteLine($"Error resetting device configuration with error: {ex}"));

        this.WhenAnyValue(x => x.Connected)
            .Subscribe(x => { ConnectButtonText = x ? "Disconnect" : "Connect"; });

        this.WhenAnyValue(x => x.EnableEvents)
            .Subscribe(x =>
            {
                IsPlaySoundOrFrequencyEnabled = x.HasFlag(SoundCardEvents.PlaySoundOrFrequency);
                IsStopEnabled = x.HasFlag(SoundCardEvents.Stop);
                IsDigitalInputsEnabled = x.HasFlag(SoundCardEvents.DigitalInputs);
                IsAdcValuesEnabled = x.HasFlag(SoundCardEvents.AdcValues);
            });


        // handle the events from the device
        // When Connected changes subscribe/unsubscribe the device events.
        this.WhenAnyValue(x => x.Connected)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(isConnected =>
            {
                if (isConnected && _deviceEventsObservable != null)
                {
                    // Subscribe on the UI thread so that the HarpEvents collection can be updated safely.
                    SubscribeToEvents();
                }
                else
                {
                    // Dispose subscription and clear messages.
                    _deviceEventsSubscription?.Dispose();
                    _deviceEventsSubscription = null;
                }
            });

        this.WhenAnyValue(x => x.InputState)
            .Subscribe(x =>
            {
                IsDI0Enabled_InputState = x.HasFlag(DigitalInputs.DI0);
            });

        this.WhenAnyValue(x => x.OutputSet)
            .Subscribe(x =>
            {
                IsDO0Enabled_OutputSet = x.HasFlag(DigitalOutputs.DO0);
                IsDO1Enabled_OutputSet = x.HasFlag(DigitalOutputs.DO1);
                IsDO2Enabled_OutputSet = x.HasFlag(DigitalOutputs.DO2);
            });

        this.WhenAnyValue(x => x.OutputClear)
            .Subscribe(x =>
            {
                IsDO0Enabled_OutputClear = x.HasFlag(DigitalOutputs.DO0);
                IsDO1Enabled_OutputClear = x.HasFlag(DigitalOutputs.DO1);
                IsDO2Enabled_OutputClear = x.HasFlag(DigitalOutputs.DO2);
            });

        this.WhenAnyValue(x => x.OutputToggle)
            .Subscribe(x =>
            {
                IsDO0Enabled_OutputToggle = x.HasFlag(DigitalOutputs.DO0);
                IsDO1Enabled_OutputToggle = x.HasFlag(DigitalOutputs.DO1);
                IsDO2Enabled_OutputToggle = x.HasFlag(DigitalOutputs.DO2);
            });

        this.WhenAnyValue(x => x.OutputState)
            .Subscribe(x =>
            {
                IsDO0Enabled_OutputState = x.HasFlag(DigitalOutputs.DO0);
                IsDO1Enabled_OutputState = x.HasFlag(DigitalOutputs.DO1);
                IsDO2Enabled_OutputState = x.HasFlag(DigitalOutputs.DO2);
            });

        // force initial population of currently connected ports
        LoadUsbInformation();
    }

    private IObservable<Unit> LoadUsbInformation()
    {
        return Observable.Start(() =>
        {
            var devices = SerialPort.GetPortNames();

            if (OperatingSystem.IsMacOS())
                // except with Bluetooth in the name
                Ports = new ObservableCollection<string>(devices.Where(d => d.Contains("cu.")).Except(devices.Where(d => d.Contains("Bluetooth"))));
            else
                Ports = new ObservableCollection<string>(devices);

            Console.WriteLine("Loaded USB information");
            //Log.Information("Loaded USB information");
        });
    }

    private async Task ConnectAndGetBaseInfo()
    {
        if (string.IsNullOrEmpty(SelectedPort))
            throw new Exception("invalid parameter");

        if (Connected)
        {
            _device?.Dispose();
            _device = null;
            Connected = false;
            SentMessages.Clear();
            return;
        }

        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(500));
            _device = await Harp.SoundCard.Device.CreateAsync(SelectedPort, cts.Token);
        }
        catch (OperationCanceledException ex)
        {
            Console.WriteLine($"Error connecting to device with error: {ex}");
            //Log.Error(ex, "Error connecting to device with error: {Exception}", ex);
            var messageBoxStandardWindow = MessageBoxManager
                .GetMessageBoxStandard("Unexpected device found",
                    "Timeout when trying to connect to a device. Most likely not an Harp device.",
                    icon: Icon.Error);
            await messageBoxStandardWindow.ShowAsync();
            _device?.Dispose();
            _device = null;
            return;

        }
        catch (HarpException ex)
        {
            Console.WriteLine($"Error connecting to device with error: {ex}");
            //Log.Error(ex, "Error connecting to device with error: {Exception}", ex);

            var messageBoxStandardWindow = MessageBoxManager
                .GetMessageBoxStandard("Unexpected device found",
                    ex.Message,
                    icon: Icon.Error);
            await messageBoxStandardWindow.ShowAsync();

            _device?.Dispose();
            _device = null;
            return;
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"COM port still in use and most likely not the expected Harp device");
            var messageBoxStandardWindow = MessageBoxManager
                .GetMessageBoxStandard("Unexpected device found",
                    $"COM port still in use and most likely not the expected Harp device.{Environment.NewLine}Specific error: {ex.Message}",
                    icon: Icon.Error);
            await messageBoxStandardWindow.ShowAsync();

            _device?.Dispose();
            _device = null;
            return;
        }

        // Clear the sent messages list
        SentMessages.Clear();

        //Log.Information("Attempting connection to port \'{SelectedPort}\'", SelectedPort);
        Console.WriteLine($"Attempting connection to port \'{SelectedPort}\'");

        DeviceID = await _device.ReadWhoAmIAsync();
        DeviceName = await _device.ReadDeviceNameAsync();
        HardwareVersion = await _device.ReadHardwareVersionAsync();
        FirmwareVersion = await _device.ReadFirmwareVersionAsync();
        try
        {
            // some devices may not have a serial number
            SerialNumber = await _device.ReadSerialNumberAsync();
        }
        catch (HarpException)
        {
            // Device does not have a serial number, simply continue by ignoring the exception
        }

        /*****************************************************************
        * TODO: Please REVIEW all these registers and update the values
        * ****************************************************************/
        PlaySoundOrFrequency = await _device.ReadPlaySoundOrFrequencyAsync();
        Stop = await _device.ReadStopAsync();
        AttenuationLeft = await _device.ReadAttenuationLeftAsync();
        AttenuationRight = await _device.ReadAttenuationRightAsync();
        AttenuationBoth = await _device.ReadAttenuationBothAsync();
        AttenuationAndPlaySoundOrFreq = await _device.ReadAttenuationAndPlaySoundOrFreqAsync();
        InputState = await _device.ReadInputStateAsync();
        ConfigureDI0 = await _device.ReadConfigureDI0Async();
        ConfigureDI1 = await _device.ReadConfigureDI1Async();
        ConfigureDI2 = await _device.ReadConfigureDI2Async();
        SoundIndexDI0 = await _device.ReadSoundIndexDI0Async();
        SoundIndexDI1 = await _device.ReadSoundIndexDI1Async();
        SoundIndexDI2 = await _device.ReadSoundIndexDI2Async();
        FrequencyDI0 = await _device.ReadFrequencyDI0Async();
        FrequencyDI1 = await _device.ReadFrequencyDI1Async();
        FrequencyDI2 = await _device.ReadFrequencyDI2Async();
        AttenuationLeftDI0 = await _device.ReadAttenuationLeftDI0Async();
        AttenuationLeftDI1 = await _device.ReadAttenuationLeftDI1Async();
        AttenuationLeftDI2 = await _device.ReadAttenuationLeftDI2Async();
        AttenuationRightDI0 = await _device.ReadAttenuationRightDI0Async();
        AttenuationRightDI1 = await _device.ReadAttenuationRightDI1Async();
        AttenuationRightDI2 = await _device.ReadAttenuationRightDI2Async();
        AttenuationAndSoundIndexDI0 = await _device.ReadAttenuationAndSoundIndexDI0Async();
        AttenuationAndSoundIndexDI1 = await _device.ReadAttenuationAndSoundIndexDI1Async();
        AttenuationAndSoundIndexDI2 = await _device.ReadAttenuationAndSoundIndexDI2Async();
        AttenuationAndFrequencyDI0 = await _device.ReadAttenuationAndFrequencyDI0Async();
        AttenuationAndFrequencyDI1 = await _device.ReadAttenuationAndFrequencyDI1Async();
        AttenuationAndFrequencyDI2 = await _device.ReadAttenuationAndFrequencyDI2Async();
        ConfigureDO0 = await _device.ReadConfigureDO0Async();
        ConfigureDO1 = await _device.ReadConfigureDO1Async();
        ConfigureDO2 = await _device.ReadConfigureDO2Async();
        PulseDO0 = await _device.ReadPulseDO0Async();
        PulseDO1 = await _device.ReadPulseDO1Async();
        PulseDO2 = await _device.ReadPulseDO2Async();
        OutputSet = await _device.ReadOutputSetAsync();
        OutputClear = await _device.ReadOutputClearAsync();
        OutputToggle = await _device.ReadOutputToggleAsync();
        OutputState = await _device.ReadOutputStateAsync();
        ConfigureAdc = await _device.ReadConfigureAdcAsync();
        AnalogData = await _device.ReadAnalogDataAsync();
        Commands = await _device.ReadCommandsAsync();
        EnableEvents = await _device.ReadEnableEventsAsync();

        UpdateAttenuationCollection(AttenuationBoth, AttenuationBothCollection);
        UpdateAttenuationCollection(AttenuationAndPlaySoundOrFreq, AttenuationAndPlaySoundOrFreqCollection);
        UpdateAttenuationCollection(AttenuationAndSoundIndexDI0, AttenuationAndSoundIndexDI0Collection);
        UpdateAttenuationCollection(AttenuationAndSoundIndexDI1, AttenuationAndSoundIndexDI1Collection);
        UpdateAttenuationCollection(AttenuationAndSoundIndexDI2, AttenuationAndSoundIndexDI2Collection);
        UpdateAttenuationCollection(AttenuationAndFrequencyDI0, AttenuationAndFrequencyDI0Collection);
        UpdateAttenuationCollection(AttenuationAndFrequencyDI1, AttenuationAndFrequencyDI1Collection);
        UpdateAttenuationCollection(AttenuationAndFrequencyDI2, AttenuationAndFrequencyDI2Collection);

        // generate observable for the _deviceSync
        _deviceEventsObservable = GenerateEventMessages();

        Connected = true;

        //Log.Information("Connected to device");
        Console.WriteLine("Connected to device");
    }

    public IObservable<string> GenerateEventMessages()
    {
        return Observable.Create<string>(async (observer, cancellationToken) =>
        {
            // Loop until cancellation is requested or the device is no longer available.
            while (!cancellationToken.IsCancellationRequested && _device != null)
            {
                // Capture local reference and check for null.
                var device = _device;
                if (device == null)
                {
                    observer.OnCompleted();
                    break;
                }

                try
                {
                    // Check if PlaySoundOrFrequency event is enabled
                    if (IsPlaySoundOrFrequencyEnabled)
                    {
                        var result = await device.ReadPlaySoundOrFrequencyAsync(cancellationToken);
                        PlaySoundOrFrequency = result;
                        observer.OnNext($"PlaySoundOrFrequency: {result}");
                    }

                    // Check if Stop event is enabled
                    if (IsStopEnabled)
                    {
                        var result = await device.ReadStopAsync(cancellationToken);
                        Stop = result;
                        observer.OnNext($"Stop: {result}");
                    }

                    // Check if DigitalInputs event is enabled
                    if (IsDigitalInputsEnabled)
                    {
                        var result = await device.ReadInputStateAsync(cancellationToken);
                        InputState = result;
                        observer.OnNext($"InputState: {result}");
                    }

                    // Check if AdcValues event is enabled
                    if (IsAdcValuesEnabled)
                    {
                        var result = await device.ReadAnalogDataAsync(cancellationToken);
                        AnalogData = result;
                        observer.OnNext($"AnalogData: {result}");
                    }

                    // Wait a short while before polling again. Adjust delay as necessary.
                    await Task.Delay(TimeSpan.FromMilliseconds(10), cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    observer.OnError(ex);
                    break;
                }
            }
            observer.OnCompleted();
            return Disposable.Empty;
        });
    }

    private IObservable<Unit> SaveConfiguration(bool savePermanently)
    {
        return Observable.StartAsync(async () =>
        {
            if (_device == null)
                throw new Exception("You need to connect to the device first");

            /*****************************************************************
            * TODO: Please REVIEW all these registers and update the values
            * ****************************************************************/
            await WriteAndLogAsync(
                value => _device.WritePlaySoundOrFrequencyAsync(value),
                PlaySoundOrFrequency,
                "PlaySoundOrFrequency");
            await WriteAndLogAsync(
                value => _device.WriteStopAsync(value),
                Stop,
                "Stop");
            await WriteAndLogAsync(
                value => _device.WriteAttenuationLeftAsync(value),
                AttenuationLeft,
                "AttenuationLeft");
            await WriteAndLogAsync(
                value => _device.WriteAttenuationRightAsync(value),
                AttenuationRight,
                "AttenuationRight");
            await WriteAndLogAsync(
                value => _device.WriteAttenuationBothAsync(value),
                AttenuationBoth,
                "AttenuationBoth");
            await WriteAndLogAsync(
                value => _device.WriteAttenuationAndPlaySoundOrFreqAsync(value),
                AttenuationAndPlaySoundOrFreq,
                "AttenuationAndPlaySoundOrFreq");
            await WriteAndLogAsync(
                value => _device.WriteConfigureDI0Async(value),
                ConfigureDI0,
                "ConfigureDI0");
            await WriteAndLogAsync(
                value => _device.WriteConfigureDI1Async(value),
                ConfigureDI1,
                "ConfigureDI1");
            await WriteAndLogAsync(
                value => _device.WriteConfigureDI2Async(value),
                ConfigureDI2,
                "ConfigureDI2");
            await WriteAndLogAsync(
                value => _device.WriteSoundIndexDI0Async(value),
                SoundIndexDI0,
                "SoundIndexDI0");
            await WriteAndLogAsync(
                value => _device.WriteSoundIndexDI1Async(value),
                SoundIndexDI1,
                "SoundIndexDI1");
            await WriteAndLogAsync(
                value => _device.WriteSoundIndexDI2Async(value),
                SoundIndexDI2,
                "SoundIndexDI2");
            await WriteAndLogAsync(
                value => _device.WriteFrequencyDI0Async(value),
                FrequencyDI0,
                "FrequencyDI0");
            await WriteAndLogAsync(
                value => _device.WriteFrequencyDI1Async(value),
                FrequencyDI1,
                "FrequencyDI1");
            await WriteAndLogAsync(
                value => _device.WriteFrequencyDI2Async(value),
                FrequencyDI2,
                "FrequencyDI2");
            await WriteAndLogAsync(
                value => _device.WriteAttenuationLeftDI0Async(value),
                AttenuationLeftDI0,
                "AttenuationLeftDI0");
            await WriteAndLogAsync(
                value => _device.WriteAttenuationLeftDI1Async(value),
                AttenuationLeftDI1,
                "AttenuationLeftDI1");
            await WriteAndLogAsync(
                value => _device.WriteAttenuationLeftDI2Async(value),
                AttenuationLeftDI2,
                "AttenuationLeftDI2");
            await WriteAndLogAsync(
                value => _device.WriteAttenuationRightDI0Async(value),
                AttenuationRightDI0,
                "AttenuationRightDI0");
            await WriteAndLogAsync(
                value => _device.WriteAttenuationRightDI1Async(value),
                AttenuationRightDI1,
                "AttenuationRightDI1");
            await WriteAndLogAsync(
                value => _device.WriteAttenuationRightDI2Async(value),
                AttenuationRightDI2,
                "AttenuationRightDI2");
            await WriteAndLogAsync(
                value => _device.WriteAttenuationAndSoundIndexDI0Async(value),
                AttenuationAndSoundIndexDI0,
                "AttenuationAndSoundIndexDI0");
            await WriteAndLogAsync(
                value => _device.WriteAttenuationAndSoundIndexDI1Async(value),
                AttenuationAndSoundIndexDI1,
                "AttenuationAndSoundIndexDI1");
            await WriteAndLogAsync(
                value => _device.WriteAttenuationAndSoundIndexDI2Async(value),
                AttenuationAndSoundIndexDI2,
                "AttenuationAndSoundIndexDI2");
            await WriteAndLogAsync(
                value => _device.WriteAttenuationAndFrequencyDI0Async(value),
                AttenuationAndFrequencyDI0,
                "AttenuationAndFrequencyDI0");
            await WriteAndLogAsync(
                value => _device.WriteAttenuationAndFrequencyDI1Async(value),
                AttenuationAndFrequencyDI1,
                "AttenuationAndFrequencyDI1");
            await WriteAndLogAsync(
                value => _device.WriteAttenuationAndFrequencyDI2Async(value),
                AttenuationAndFrequencyDI2,
                "AttenuationAndFrequencyDI2");
            await WriteAndLogAsync(
                value => _device.WriteConfigureDO0Async(value),
                ConfigureDO0,
                "ConfigureDO0");
            await WriteAndLogAsync(
                value => _device.WriteConfigureDO1Async(value),
                ConfigureDO1,
                "ConfigureDO1");
            await WriteAndLogAsync(
                value => _device.WriteConfigureDO2Async(value),
                ConfigureDO2,
                "ConfigureDO2");
            await WriteAndLogAsync(
                value => _device.WritePulseDO0Async(value),
                PulseDO0,
                "PulseDO0");
            await WriteAndLogAsync(
                value => _device.WritePulseDO1Async(value),
                PulseDO1,
                "PulseDO1");
            await WriteAndLogAsync(
                value => _device.WritePulseDO2Async(value),
                PulseDO2,
                "PulseDO2");
            await WriteAndLogAsync(
                value => _device.WriteOutputSetAsync(value),
                OutputSet,
                "OutputSet");
            await WriteAndLogAsync(
                value => _device.WriteOutputClearAsync(value),
                OutputClear,
                "OutputClear");
            await WriteAndLogAsync(
                value => _device.WriteOutputToggleAsync(value),
                OutputToggle,
                "OutputToggle");
            await WriteAndLogAsync(
                value => _device.WriteOutputStateAsync(value),
                OutputState,
                "OutputState");
            await WriteAndLogAsync(
                value => _device.WriteConfigureAdcAsync(value),
                ConfigureAdc,
                "ConfigureAdc");
            await WriteAndLogAsync(
                value => _device.WriteCommandsAsync(value),
                Commands,
                "Commands");
            await WriteAndLogAsync(
                value => _device.WriteEnableEventsAsync(value),
                EnableEvents,
                "EnableEvents");

            // Save the configuration to the device permanently
            if (savePermanently)
            {
                // To prevent multiple calls to the device while it is resetting
                _deviceEventsSubscription?.Dispose();
                _deviceEventsSubscription = null;

                await WriteAndLogAsync(
                    value => _device.WriteResetDeviceAsync(value),
                    ResetFlags.Save,
                    "SavePermanently");

                // Wait to ensure the device is ready after the reset
                await Task.Delay(4000);

                // Re-subscribe to the device events observable
                SubscribeToEvents();
            }
        });
    }

    private IObservable<Unit> ResetConfiguration()
    {
        return Observable.StartAsync(async () =>
        {
            if (_device != null)
            {
                await WriteAndLogAsync(
                    value => _device.WriteResetDeviceAsync(value),
                    ResetFlags.RestoreDefault,
                    "ResetDevice");
            }
        });
    }

    private async Task WriteAndLogAsync<T>(Func<T, Task> writeFunc, T value, string registerName)
    {
        if (_device == null)
            throw new Exception("Device is not connected");

        await writeFunc(value);

        // Log the message to the SentMessages collection on the UI thread
        RxApp.MainThreadScheduler.Schedule(() =>
        {
            SentMessages.Add($"{DateTime.Now:HH:mm:ss.fff} - Write {registerName}: {value}");
        });
    }

    private void SubscribeToEvents()
    {
        _deviceEventsSubscription = _deviceEventsObservable
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(
                msg => HarpEvents.Add(msg.ToString()),
                ex => Debug.WriteLine($"Error in device events: {ex}")
            );
    }
    
    private static void UpdateAttenuationCollection(ushort[]? array, ObservableCollection<ArrayItemWrapper<ushort>> collection)
    {
        if (array == null) return;
        
        RxApp.MainThreadScheduler.Schedule(() => 
        {
            collection.Clear();
            for (int i = 0; i < array.Length; i++)
            {
                collection.Add(new ArrayItemWrapper<ushort>(i, array[i]));
            }
        });
    }


    public class ArrayItemWrapper<T> : ReactiveObject
    {
        public int Index { get; }

        [Reactive]
        public T Value { get; set; }

        public ArrayItemWrapper(int index, T value)
        {
            Index = index;
            Value = value;
        }
    }
}
