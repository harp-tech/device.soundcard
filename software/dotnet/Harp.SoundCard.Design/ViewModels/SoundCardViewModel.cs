using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
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
using DynamicData;
using Harp.SoundCard.Design.Views;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Harp.SoundCard.Design.ViewModels;


public class SoundCardViewModel : ViewModelBase
{
    const int ConnectTimeoutMilliseconds = 500;
    const int BootloaderFallThroughMilliseconds = 3500;

    public string AppVersion { get; set; } = string.Empty;
    public ReactiveCommand<Unit, Unit> LoadDeviceInformation { get; }

    #region Connection Information

    [Reactive] public ObservableCollection<string> Ports { get; set; }
    [Reactive] public string SelectedPort { get; set; } = string.Empty;
    [Reactive] public bool CanSelectPort { get; set; } = true;
    [Reactive] public bool Connected { get; set; }
    [Reactive] public string ConnectButtonText { get; set; } = "Connect";
    [Reactive] public string PortLabelText { get; set; } = string.Empty;
    [Reactive] public string WindowTitle { get; set; } = string.Empty;
    public ReactiveCommand<Unit, Unit> ConnectAndGetBaseInfoCommand { get; }

    #endregion

    #region Operations

    public ReactiveCommand<bool, Unit> SaveConfigurationCommand { get; }
    public ReactiveCommand<Unit, Unit> ResetConfigurationCommand { get; }

    #endregion

    #region Device basic information

    [Reactive] public int DeviceID { get; set; }
    [Reactive] public string DeviceName { get; set; } = string.Empty;
    [Reactive] public HarpVersion? HardwareVersion { get; set; }
    [Reactive] public HarpVersion? FirmwareVersion { get; set; }
    [Reactive] public int SerialNumber { get; set; }

    #endregion

    #region Registers

    [Reactive] public ushort Start { get; set; }
    [Reactive] public byte Stop { get; set; }
    [Reactive] public ushort AttenuationLeft { get; set; }
    [Reactive] public ushort AttenuationRight { get; set; }
    [Reactive] public ushort[] AttenuationBoth { get; set; } = new ushort[2];
    [Reactive] public ushort[] AttenuationStart { get; set; } = new ushort[3];
    [Reactive] public DigitalInputs InputState { get; set; }
    [Reactive] public DIMode ConfigureDI0 { get; set; }
    [Reactive] public DIMode ConfigureDI1 { get; set; }
    [Reactive] public ushort StartOnDI0 { get; set; }
    [Reactive] public ushort StartOnDI1 { get; set; }
    [Reactive] public ushort AttenuationLeftDI0 { get; set; }
    [Reactive] public ushort AttenuationLeftDI1 { get; set; }
    [Reactive] public ushort AttenuationRightDI0 { get; set; }
    [Reactive] public ushort AttenuationRightDI1 { get; set; }
    [Reactive] public DOMode ConfigureDO0 { get; set; }
    [Reactive] public DOMode ConfigureDO1 { get; set; }
    [Reactive] public DOMode ConfigureDO2 { get; set; }
    [Reactive] public DigitalOutputs OutputSet { get; set; }
    [Reactive] public DigitalOutputs OutputClear { get; set; }
    [Reactive] public DigitalOutputs OutputToggle { get; set; }
    [Reactive] public DigitalOutputs OutputState { get; set; }
    [Reactive] public EnableFlag EnableAdcControlState { get; set; }
    [Reactive] public AdcControlStatePayload AdcControlState { get; set; }

    #endregion

    #region Array collections

    [Reactive] public ObservableCollection<ArrayItemWrapper<ushort>> AttenuationBothCollection { get; private set; } = new();
    [Reactive] public ObservableCollection<ArrayItemWrapper<ushort>> AttenuationStartCollection { get; private set; } = new();
    #endregion

    #region Polled Registers

    [Reactive] public bool IsStartEnabled { get; set; } = true;
    [Reactive] public bool IsStopEnabled { get; set; } = true;
    [Reactive] public bool IsDigitalInputsEnabled { get; set; } = true;
    [Reactive] public bool IsAdcControlStateEnabled { get; set; } = true;

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
    [Reactive] public ObservableCollection<string> SentMessages { get; set; } = new();

    public ReactiveCommand<Unit, Unit> ShowAboutCommand { get; private set; }
    public ReactiveCommand<Unit, Unit> ClearMessagesCommand { get; private set; }
    public ReactiveCommand<Unit, Unit> ShowMessagesCommand { get; private set; }
    public SoundGenerationViewModel SoundGenerationViewModel { get; } = new();
    
    // play sound index
    [Reactive] public int PlaySoundIndex { get; set; } = 2;
    public ReactiveCommand<Unit, Unit> PlaySoundIndexCommand { get; private set; }
    public ReactiveCommand<Unit, Unit> StopSoundCommand { get; private set; }

    #endregion

    private Harp.SoundCard.AsyncDevice? _device;
    private IObservable<string>? _deviceEventsObservable;
    private IDisposable? _deviceEventsSubscription;

    public SoundCardViewModel()
    {
        var assembly = typeof(SoundCardViewModel).Assembly;
        var informationVersion = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;
        if (informationVersion != null)
            // remove everything after the + if it exists
            AppVersion = $"v{informationVersion.Split("+")[0]}";

        Ports = new ObservableCollection<string>();

        ClearMessagesCommand = ReactiveCommand.Create(() => { SentMessages.Clear(); });
        ShowMessagesCommand = ReactiveCommand.Create(() => { ShowWriteMessages = !ShowWriteMessages; });

        PlaySoundIndexCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            if (_device == null)
                return;

            try
            {
                await _device.WriteStartAsync((ushort)PlaySoundIndex);
                SentMessages.Add($"Wrote Start with value: {PlaySoundIndex}");
            }
            catch (HarpException ex)
            {
                Console.WriteLine($"Error writing Start with exception: {ex.Message}");
                //Log.Error(ex, "Error writing Start with exception: {Exception}", ex));
            }
        });

        StopSoundCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            if (_device == null)
                return;

            try
            {
                await _device.WriteStopAsync(1);
                SentMessages.Add("Wrote Stop with value: 1");
            }
            catch (HarpException ex)
            {
                Console.WriteLine($"Error writing Stop with exception: {ex.Message}");
            }
        });


        LoadDeviceInformation = ReactiveCommand.CreateFromObservable(LoadUsbInformation);
        LoadDeviceInformation.IsExecuting.ToPropertyEx(this, x => x.IsLoadingPorts);
        LoadDeviceInformation.ThrownExceptions.Subscribe(ex =>
            Console.WriteLine($"Error loading device information with exception: {ex.Message}"));
        //Log.Error(ex, "Error loading device information with exception: {Exception}", ex));

        // can connect if there is a selection and also if the new selection is different than the old one
        var canConnect = this.WhenAnyValue(x => x.SelectedPort)
            .Select(selectedPort => !string.IsNullOrEmpty(selectedPort));

        ShowAboutCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var mainWindow = (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
            if (mainWindow !=
                null)
            {
                await new About { DataContext = new AboutViewModel() }.ShowDialog(mainWindow);
            }
        });

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

        this.WhenAnyValue(x => x.CanSelectPort, x => x.SelectedPort)
            .Subscribe(x =>
            {
                PortLabelText = x.Item1 ? "Select a device" : "Selected port";
                WindowTitle = x.Item1 ? "Harp SoundCard" : $"Harp SoundCard ({x.Item2})";
            });

        // handle the events from the device
        // When Connected changes subscribe/unsubscribe the device events.
        this.WhenAnyValue(x => x.Connected)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(isConnected =>
            {
                if (isConnected && _deviceEventsObservable is not null)
                {
                    SubscribeToEvents(_deviceEventsObservable);
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
            var sortedPorts = devices;
            
            if (OperatingSystem.IsMacOS())
            {
                sortedPorts = sortedPorts.Where(port =>
                        !port.Contains("cu.") && !port.Contains("Bluetooth") && !port.Contains("debug"))
                    .OrderBy(port => int.Parse(port.Substring(port.LastIndexOf("tty.", StringComparison.Ordinal) + 4)))
                    .ToArray();
            }
            else if (OperatingSystem.IsWindows())
            {
                sortedPorts = sortedPorts.OrderBy(port => int.Parse(port.Substring(3))).ToArray();
            }

            Ports.Clear();
            Ports.AddRange(sortedPorts);
            Console.WriteLine("Loaded USB information");
            //Log.Information("Loaded USB information");
        });
    }

    public void ConnectToPort(string portName)
    {
        Ports = new ObservableCollection<string> { portName };
        SelectedPort = portName;
        CanSelectPort = false;
        ConnectAndGetBaseInfoCommand.Execute().Catch(Observable.Empty<Unit>()).Subscribe();
    }

    private static async Task<Harp.SoundCard.AsyncDevice> CreateDeviceAsync(string portName)
    {
        try
        {
            using var cts = new CancellationTokenSource(ConnectTimeoutMilliseconds);
            return await Harp.SoundCard.Device.CreateAsync(portName, cts.Token);
        }
        catch (OperationCanceledException)
        {
            await Task.Delay(BootloaderFallThroughMilliseconds);
            using var cts = new CancellationTokenSource(ConnectTimeoutMilliseconds);
            return await Harp.SoundCard.Device.CreateAsync(portName, cts.Token);
        }
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
            _device = await CreateDeviceAsync(SelectedPort);
        }
        catch (OperationCanceledException ex)
        {
            Console.WriteLine($"Error connecting to device with error: {ex}");
            //Log.Error(ex, "Error connecting to device with error: {Exception}", ex);
            var messageBoxStandardWindow = MessageBoxManager
                .GetMessageBoxStandard("Device not responding",
                    $"The device on port {SelectedPort} did not respond. If it was just connected or restarted, " +
                    "wait a few seconds and press Connect again. Otherwise, it may not be a Harp device.",
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
        catch (Exception ex) when (ex is IOException or ArgumentException)
        {
            Console.WriteLine($"Error opening port {SelectedPort} with error: {ex}");
            var messageBoxStandardWindow = MessageBoxManager
                .GetMessageBoxStandard("Port not available",
                    $"The port {SelectedPort} could not be opened. Check that the device is connected to this port." +
                    $"{Environment.NewLine}Specific error: {ex.Message}",
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

        Start = await _device.ReadStartAsync();
        Stop = await _device.ReadStopAsync();
        AttenuationLeft = await _device.ReadAttenuationLeftAsync();
        AttenuationRight = await _device.ReadAttenuationRightAsync();
        AttenuationBoth = await _device.ReadAttenuationBothAsync();
        AttenuationStart = await _device.ReadAttenuationStartAsync();
        InputState = await _device.ReadInputStateAsync();
        ConfigureDI0 = await _device.ReadConfigureDI0Async();
        ConfigureDI1 = await _device.ReadConfigureDI1Async();
        StartOnDI0 = await _device.ReadStartOnDI0Async();
        StartOnDI1 = await _device.ReadStartOnDI1Async();
        AttenuationLeftDI0 = await _device.ReadAttenuationLeftDI0Async();
        AttenuationLeftDI1 = await _device.ReadAttenuationLeftDI1Async();
        AttenuationRightDI0 = await _device.ReadAttenuationRightDI0Async();
        AttenuationRightDI1 = await _device.ReadAttenuationRightDI1Async();
        ConfigureDO0 = await _device.ReadConfigureDO0Async();
        ConfigureDO1 = await _device.ReadConfigureDO1Async();
        ConfigureDO2 = await _device.ReadConfigureDO2Async();
        OutputSet = await _device.ReadOutputSetAsync();
        OutputClear = await _device.ReadOutputClearAsync();
        OutputToggle = await _device.ReadOutputToggleAsync();
        OutputState = await _device.ReadOutputStateAsync();
        EnableAdcControlState = await _device.ReadEnableAdcControlStateAsync();
        AdcControlState = await _device.ReadAdcControlStateAsync();

        UpdateAttenuationCollection(AttenuationBoth, AttenuationBothCollection);
        UpdateAttenuationCollection(AttenuationStart, AttenuationStartCollection);

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
                    // Check if Start event is enabled
                    if (IsStartEnabled)
                    {
                        var result = await device.ReadStartAsync(cancellationToken);
                        Start = result;
                        observer.OnNext($"Start: {result}");
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

                    // Check if AdcControlState event is enabled
                    if (IsAdcControlStateEnabled)
                    {
                        var result = await device.ReadAdcControlStateAsync(cancellationToken);
                        AdcControlState = result;
                        observer.OnNext($"AdcControlState: {result}");
                    }

                    // Wait a short while before polling again. Adjust delay as necessary.
                    await Task.Delay(TimeSpan.FromMilliseconds(10), cancellationToken);
                }
                catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
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
                value => _device.WriteStartAsync(value),
                Start,
                "Start");
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
                value => _device.WriteAttenuationStartAsync(value),
                AttenuationStart,
                "AttenuationStart");
            await WriteAndLogAsync(
                value => _device.WriteConfigureDI0Async(value),
                ConfigureDI0,
                "ConfigureDI0");
            await WriteAndLogAsync(
                value => _device.WriteConfigureDI1Async(value),
                ConfigureDI1,
                "ConfigureDI1");
            await WriteAndLogAsync(
                value => _device.WriteStartOnDI0Async(value),
                StartOnDI0,
                "StartOnDI0");
            await WriteAndLogAsync(
                value => _device.WriteStartOnDI1Async(value),
                StartOnDI1,
                "StartOnDI1");
            await WriteAndLogAsync(
                value => _device.WriteAttenuationLeftDI0Async(value),
                AttenuationLeftDI0,
                "AttenuationLeftDI0");
            await WriteAndLogAsync(
                value => _device.WriteAttenuationLeftDI1Async(value),
                AttenuationLeftDI1,
                "AttenuationLeftDI1");
            await WriteAndLogAsync(
                value => _device.WriteAttenuationRightDI0Async(value),
                AttenuationRightDI0,
                "AttenuationRightDI0");
            await WriteAndLogAsync(
                value => _device.WriteAttenuationRightDI1Async(value),
                AttenuationRightDI1,
                "AttenuationRightDI1");
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
                value => _device.WriteEnableAdcControlStateAsync(value),
                EnableAdcControlState,
                "EnableAdcControlState");

            // Save the configuration to the device permanently
            if (savePermanently && _deviceEventsObservable is not null)
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
                SubscribeToEvents(_deviceEventsObservable);
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

    private void SubscribeToEvents(IObservable<string> deviceEvents)
    {
        _deviceEventsSubscription = deviceEvents
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(
                _ => { },
                ex =>
                {
                    Debug.WriteLine($"Error in device events: {ex}");
                    if (ex is not HarpException)
                        OnConnectionLost(ex);
                }
            );
    }

    private void OnConnectionLost(Exception error)
    {
        if (!Connected)
            return;

        Console.WriteLine($"Connection to port {SelectedPort} lost with error: {error}");
        try
        {
            _device?.Dispose();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error closing port {SelectedPort} with error: {ex}");
        }

        _device = null;
        Connected = false;
        SentMessages.Clear();

        var messageBoxStandardWindow = MessageBoxManager
            .GetMessageBoxStandard("Connection lost",
                $"The connection to the device on port {SelectedPort} was lost. Check that the device is connected, then press Connect.",
                icon: Icon.Error);
        _ = messageBoxStandardWindow.ShowAsync();
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
