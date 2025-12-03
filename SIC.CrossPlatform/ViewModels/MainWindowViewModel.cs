#nullable enable
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SIC_Simulator;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using SIC.Avalonia.Views.Dialogs;

namespace SIC.Avalonia.ViewModels
{
public partial class MainWindowViewModel : ViewModelBase
{
        private static int _instanceCounter = 0;
        private readonly int _instanceId;
        
        private SIC_CPU _cpu;
        private Window? _mainWindow;
        private string _currentFilePath = string.Empty;
        private string _lastLoadedFileName = string.Empty;
        private int _lastValidPC = 0; // Track last valid PC for memory display when program ends
        private string _objectCodeText = string.Empty;
        private string _modRecordsText = string.Empty;
        
        // Register properties - Hex
        [ObservableProperty]
        private string _registerA_Hex = "000000";
        
        [ObservableProperty]
        private string _registerL_Hex = "000000";
        
        [ObservableProperty]
        private string _registerX_Hex = "000000";
        
        [ObservableProperty]
        private string _registerPC_Hex = "000000";
        
        [ObservableProperty]
        private string _registerSW_Hex = "000000";

        // Register properties - Binary MSB
        [ObservableProperty]
        private string _registerA_BIN_MSB = "00000000";
        
        [ObservableProperty]
        private string _registerL_BIN_MSB = "00000000";
        
        [ObservableProperty]
        private string _registerX_BIN_MSB = "00000000";
        
        [ObservableProperty]
        private string _registerPC_BIN_MSB = "00000000";
        
        [ObservableProperty]
        private string _registerSW_BIN_MSB = "00000000";

        // Register properties - Binary MIB
        [ObservableProperty]
        private string _registerA_BIN_MIB = "00000000";
        
        [ObservableProperty]
        private string _registerL_BIN_MIB = "00000000";
        
        [ObservableProperty]
        private string _registerX_BIN_MIB = "00000000";
        
        [ObservableProperty]
        private string _registerPC_BIN_MIB = "00000000";
        
        [ObservableProperty]
        private string _registerSW_BIN_MIB = "00000000";

        // Register properties - Binary LSB
        [ObservableProperty]
        private string _registerA_BIN_LSB = "00000000";
        
        [ObservableProperty]
        private string _registerL_BIN_LSB = "00000000";
        
        [ObservableProperty]
        private string _registerX_BIN_LSB = "00000000";
        
        [ObservableProperty]
        private string _registerPC_BIN_LSB = "00000000";
        
        [ObservableProperty]
        private string _registerSW_BIN_LSB = "00000000";

        // Register properties - Decimal
        [ObservableProperty]
        private string _registerA_Dec = "0";
        
        [ObservableProperty]
        private string _registerL_Dec = "0";
        
        [ObservableProperty]
        private string _registerX_Dec = "0";
        
        [ObservableProperty]
        private string _registerPC_Dec = "0";
        
        [ObservableProperty]
        private string _registerSW_Dec = "0";

        // CC and Comparison
        [ObservableProperty]
        private string _registerSW_CC = "00";
        
        [ObservableProperty]
        private string _comparisonResult = "xx";

        // Current Instruction
        [ObservableProperty]
        private string _currentInstruction = "xxxxxx";
        
        [ObservableProperty]
        private string _currentInstructionDescription = "xxxxxx";
        
        [ObservableProperty]
        private string _currentInstructionEffect = "xxxxxx";

        // Next Instruction
        [ObservableProperty]
        private string _nextInstruction = "xxxxxx";
        
        [ObservableProperty]
        private string _nextInstructionDescription = "xxxxxx";
        
        [ObservableProperty]
        private string _nextInstructionEffect = "xxxxxx";

        // Memory Display
        [ObservableProperty]
        private string _memoryDisplay = "";
        
        [ObservableProperty]
        private string _memoryHeader = "=== Memory View ===";
        
        [ObservableProperty]
        private ObservableCollection<MemoryLineViewModel> _memoryLines = new();
        
        [ObservableProperty]
        private bool _memoryDisplayBinary = false;
        
        [ObservableProperty]
        private bool _memoryDisplayHex = true;
        
        [ObservableProperty]
        private bool _memoryDisplayDecimal = false;
        
        [ObservableProperty]
        private bool _memoryDisplayAscii = false;

        // Devices
        [ObservableProperty]
        private ObservableCollection<DeviceViewModel> _devices = new();

        // Microsteps
        [ObservableProperty]
        private string _microstepsDisplay = "";

        // Code Editor and Tabs
        [ObservableProperty]
        private string _codeEditorText = "";
        
        [ObservableProperty]
        private string _symbolTableDisplay = "";
        
        [ObservableProperty]
        private ObservableCollection<SymbolTableRowViewModel> _symbolTableRows = new();
        
        // Store the raw instruction data for highlighting
        private List<SymbolTableRowViewModel> _allSymbolTableRows = new();
        
        [ObservableProperty]
        private string _objectCodeDisplay = "";
        
        [ObservableProperty]
        private string _modificationRecordsDisplay = "";
        
        [ObservableProperty]
        private string _relocatedObjectCodeDisplay = "";

        public MainWindowViewModel()
        {
            _instanceId = ++_instanceCounter;
            _cpu = new SIC_CPU(true);
            
            RefreshCPUDisplays();
            RefreshDeviceDisplay();
        }

        public void SetMainWindow(Window window)
        {
            _mainWindow = window;
        }
        
        private Window? GetMainWindow()
        {
            // Try stored reference first
            if (_mainWindow != null) return _mainWindow;
            
            // Try to find window from Application
            if (global::Avalonia.Application.Current?.ApplicationLifetime is global::Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop)
            {
                return desktop.MainWindow;
            }
            
            return null;
        }

        private async void Step()
        {
            if (_cpu.PC == -1)
            {
                await ShowMessageBox("Program Stepping Halted. L=0, RSUB, PC = -1", "Program Halted");
                return;
            }
            
            try
            {
                _cpu.PerformStep();
            }
            catch (Exception ex)
            {
                await ShowExceptionDialog(ex);
                return;
            }
            RefreshCPUDisplays();
            RefreshDeviceDisplay();
        }

        private async void ThreeStep()
        {
            int i = 0;
            
            try
            {
                while (_cpu.PC >= 0 && _cpu.PC <= 32767 && i < 3)
                {
                    _cpu.PerformStep();
                    RefreshCPUDisplays();
                    i++;
                }
            }
            catch (Exception ex)
            {
                await ShowExceptionDialog(ex);
                return;
            }
            RefreshDeviceDisplay();
        }

        private async void Run()
        {
            try
            {
                while (_cpu.PC != -1)
                {
                    _cpu.PerformStep();
                    RefreshCPUDisplays();
                    RefreshDeviceDisplay();
                    await Task.Delay(250);
                }
            }
            catch (Exception ex)
            {
                await ShowExceptionDialog(ex);
                return;
            }
            RefreshCPUDisplays();
        }
        
        private async Task ShowMessageBox(string message, string title)
        {
            var window = GetMainWindow();
            if (window == null) return;
            
            var msgBox = new Window
            {
                Title = title,
                Width = 400,
                Height = 150,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                CanResize = false
            };
            
            var panel = new StackPanel { Margin = new Thickness(20), VerticalAlignment = VerticalAlignment.Center };
            panel.Children.Add(new TextBlock { Text = message, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 20) });
            var btn = new Button { Content = "OK", Width = 75, HorizontalAlignment = HorizontalAlignment.Center };
            btn.Click += (s, e) => msgBox.Close();
            panel.Children.Add(btn);
            msgBox.Content = panel;
            
            await msgBox.ShowDialog(window);
        }
        
        private async Task ShowExceptionDialog(Exception ex)
        {
            var window = GetMainWindow();
            if (window == null) return;
            
            var dialog = new SIC.Avalonia.Views.Dialogs.ExceptionDialog(ex);
            await dialog.ShowDialog(window);
        }

        private void Restart()
        {
            if (!string.IsNullOrWhiteSpace(_objectCodeText))
            {
                LoadObjectFile(_objectCodeText.Split('\n'));
            }
            else
            {
                // Just reset PC if no object code
                _cpu.PC = _cpu.CurrentProgramStartAddress;
            }
            RefreshCPUDisplays();
            RefreshDeviceDisplay();
        }

        private void ZeroAllMemory()
        {
            _cpu.ZeroizeMemory();
            RefreshCPUDisplays();
        }

        private void RandomizeAllMemory()
        {
            _cpu.RandomizeMemory();
            RefreshCPUDisplays();
        }

        private void ResetSICVirtualMachine()
        {
            _cpu.ResetVM();
            RefreshCPUDisplays();
            RefreshDeviceDisplay();
        }

        private void ResetSICDevices()
        {
            for (int i = 0; i < SIC_CPU.NumDevices; i++)
            {
                _cpu.Devices[i].reset();
            }
            RefreshDeviceDisplay();
        }

        // Menu command properties - lazy initialization
        private ICommand? _loadSICSourceFileCommand;
        public ICommand LoadSICSourceFileCommand => _loadSICSourceFileCommand ??= new AsyncRelayCommand(LoadSICSourceFileAsync);
        
        private ICommand? _saveSICSourceFileCommand;
        public ICommand SaveSICSourceFileCommand => _saveSICSourceFileCommand ??= new AsyncRelayCommand(SaveSICSourceFileAsync);
        
        private ICommand? _saveNewSICSourceFileCommand;
        public ICommand SaveNewSICSourceFileCommand => _saveNewSICSourceFileCommand ??= new AsyncRelayCommand(SaveNewSICSourceFileAsync);
        
        private ICommand? _assembleCurrentFileCommand;
        public ICommand AssembleCurrentFileCommand => _assembleCurrentFileCommand ??= new AsyncRelayCommand(AssembleCurrentFileAsync);
        
        private ICommand? _openSICObjectFileCommand;
        public ICommand OpenSICObjectFileCommand => _openSICObjectFileCommand ??= new AsyncRelayCommand(OpenSICObjectFileAsync);
        
        private ICommand? _loadSavedSICMachineStateCommand;
        public ICommand LoadSavedSICMachineStateCommand => _loadSavedSICMachineStateCommand ??= new AsyncRelayCommand(LoadSavedSICMachineStateAsync);
        
        private ICommand? _saveMachineStateCommand;
        public ICommand SaveMachineStateCommand => _saveMachineStateCommand ??= new AsyncRelayCommand(SaveMachineStateAsync);
        
        private ICommand? _relocateCurrentProgramCommand;
        public ICommand RelocateCurrentProgramCommand => _relocateCurrentProgramCommand ??= new AsyncRelayCommand(RelocateCurrentProgramAsync);
        
        private ICommand? _exitCommand;
        public ICommand ExitCommand => _exitCommand ??= new RelayCommand(Exit);
        
        private ICommand? _checkForUpdatesCommand;
        public ICommand CheckForUpdatesCommand => _checkForUpdatesCommand ??= new AsyncRelayCommand(CheckForUpdatesAsync);
        
        private ICommand? _aboutCommand;
        public ICommand AboutCommand => _aboutCommand ??= new AsyncRelayCommand(AboutAsync);
        
        // Machine menu commands
        private ICommand? _setProgramCounterToCommand;
        public ICommand SetProgramCounterToCommand => _setProgramCounterToCommand ??= new AsyncRelayCommand(SetProgramCounterToAsync);
        
        private ICommand? _stepSingleInstructionCommand;
        public ICommand StepSingleInstructionCommand => _stepSingleInstructionCommand ??= new RelayCommand(Step);
        
        private ICommand? _setMemoryByteCommand;
        public ICommand SetMemoryByteCommand => _setMemoryByteCommand ??= new AsyncRelayCommand(SetMemoryByteAsync);
        
        private ICommand? _setMemoryWordCommand;
        public ICommand SetMemoryWordCommand => _setMemoryWordCommand ??= new AsyncRelayCommand(SetMemoryWordAsync);
        
        // Bottom button commands
        private ICommand? _stepCommand;
        public ICommand StepCommand => _stepCommand ??= new RelayCommand(Step);
        
        private ICommand? _threeStepCommand;
        public ICommand ThreeStepCommand => _threeStepCommand ??= new RelayCommand(ThreeStep);
        
        private ICommand? _runCommand;
        public ICommand RunCommand => _runCommand ??= new RelayCommand(Run);
        
        private ICommand? _restartCommand;
        public ICommand RestartCommand => _restartCommand ??= new RelayCommand(Restart);
        
        // Machine menu commands (additional)
        private ICommand? _zeroAllMemoryCommand;
        public ICommand ZeroAllMemoryCommand => _zeroAllMemoryCommand ??= new RelayCommand(ZeroAllMemory);
        
        private ICommand? _randomizeAllMemoryCommand;
        public ICommand RandomizeAllMemoryCommand => _randomizeAllMemoryCommand ??= new RelayCommand(RandomizeAllMemory);
        
        private ICommand? _resetSICVirtualMachineCommand;
        public ICommand ResetSICVirtualMachineCommand => _resetSICVirtualMachineCommand ??= new RelayCommand(ResetSICVirtualMachine);
        
        private ICommand? _resetSICDevicesCommand;
        public ICommand ResetSICDevicesCommand => _resetSICDevicesCommand ??= new RelayCommand(ResetSICDevices);

        // Menu command implementations
        private async Task LoadSICSourceFileAsync()
        {
            var window = GetMainWindow();
            if (window == null) return;
            
            var storage = window.StorageProvider;
            var files = await storage.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Load SIC Source File",
                AllowMultiple = false,
                FileTypeFilter = new[] { new FilePickerFileType("SIC Source Files") { Patterns = new[] { "*.sic" } } }
            });
            
            if (files.Count > 0)
            {
                var file = files[0];
                using var stream = await file.OpenReadAsync();
                using var reader = new StreamReader(stream);
                CodeEditorText = await reader.ReadToEndAsync();
                _lastLoadedFileName = file.Path.LocalPath;
            }
        }
        
        private async Task SaveSICSourceFileAsync()
        {
            var window = GetMainWindow();
            if (window == null) return;
            
            if (string.IsNullOrEmpty(_lastLoadedFileName))
            {
                await SaveNewSICSourceFileAsync();
                return;
            }
            
            try
            {
                await File.WriteAllTextAsync(_lastLoadedFileName, CodeEditorText);
                await ShowMessageDialog("Success", "File saved successfully.");
            }
            catch (Exception ex)
            {
                await ShowMessageDialog("Error", $"Failed to save file: {ex.Message}");
            }
        }
        
        private async Task SaveNewSICSourceFileAsync()
        {
            var window = GetMainWindow();
            if (window == null) return;
            
            var storage = window.StorageProvider;
            var file = await storage.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                Title = "Save SIC Source File",
                SuggestedFileName = "program.sic",
                FileTypeChoices = new[] { new FilePickerFileType("SIC Source Files") { Patterns = new[] { "*.sic" } } }
            });
            
            if (file != null)
            {
                try
                {
                    using var stream = await file.OpenWriteAsync();
                    using var writer = new StreamWriter(stream);
                    await writer.WriteAsync(CodeEditorText);
                    _lastLoadedFileName = file.Path.LocalPath;
                    await ShowMessageDialog("Success", "File saved successfully.");
                }
                catch (Exception ex)
                {
                    await ShowMessageDialog("Error", $"Failed to save file: {ex.Message}");
                }
            }
        }
        
        private async Task AssembleCurrentFileAsync()
        {
            var window = GetMainWindow();
            if (window == null) return;
            
            if (string.IsNullOrWhiteSpace(CodeEditorText))
            {
                await ShowMessageDialog("Error", "The code editor is empty. Please write valid SIC assembly code.");
                return;
            }
            
            // Save current file if necessary
            if (string.IsNullOrWhiteSpace(_lastLoadedFileName))
            {
                var storage = window.StorageProvider;
                var file = await storage.SaveFilePickerAsync(new FilePickerSaveOptions
                {
                    Title = "Save SIC Source File",
                    SuggestedFileName = "program.sic",
                    FileTypeChoices = new[] { new FilePickerFileType("SIC Assembly Files") { Patterns = new[] { "*.sic" } } }
                });
                
                if (file == null)
                {
                    await ShowMessageDialog("Error", "File save cancelled. Assembly cannot proceed.");
                    return;
                }
                
                _lastLoadedFileName = file.Path.LocalPath;
            }
            
            // Write current code editor contents to the file
            try
            {
                await File.WriteAllTextAsync(_lastLoadedFileName, CodeEditorText);
                
                // Assemble the file
                await AssembleSICFileAsync(_lastLoadedFileName);
            }
            catch (Exception ex)
            {
                await ShowMessageDialog("Error", $"An error occurred: {ex.Message}");
            }
        }
        
        private async Task OpenSICObjectFileAsync()
        {
            var window = GetMainWindow();
            if (window == null) return;
            
            var storage = window.StorageProvider;
            var files = await storage.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Open SIC Object File",
                AllowMultiple = false,
                FileTypeFilter = new[] { new FilePickerFileType("SIC Object Files") { Patterns = new[] { "*.sic.obj" } } }
            });
            
            if (files.Count > 0)
            {
                var file = files[0];
                using var stream = await file.OpenReadAsync();
                using var reader = new StreamReader(stream);
                string fileText = await reader.ReadToEndAsync();
                
                var fullText = fileText.Split('\n');
                var lines = fullText.Where(line => !string.IsNullOrWhiteSpace(line) && (line[0] == 'H' || line[0] == 'E' || line[0] == 'T')).ToArray();
                var mods = fullText.Where(line => !string.IsNullOrWhiteSpace(line) && line[0] == 'M').ToArray();
                
                _objectCodeText = string.Join("\n", lines);
                _modRecordsText = string.Join("\n", mods);
                ObjectCodeDisplay = _objectCodeText;
                
                // Ask if user wants to relocate
                var relocateDialog = new RelocatePromptDialog();
                await relocateDialog.ShowDialog(window);
                
                if (relocateDialog.DialogResult && relocateDialog.UseRelocatingLoader)
                {
                    var relocateObjDialog = new RelocateObjectFileDialog(lines, mods);
                    await relocateObjDialog.ShowDialog(window);
                    
                    if (relocateObjDialog.DialogResult)
                    {
                        int startAddr = relocateObjDialog.RelocatedToAddress;
                        RelocateLoadObjectFile(startAddr, lines, mods);
                    }
                    else
                    {
                        LoadObjectFile(lines);
                    }
                }
                else
                {
                    LoadObjectFile(lines);
                }
                
                RefreshCPUDisplays();
            }
        }
        
        private async Task LoadSavedSICMachineStateAsync()
        {
            var window = GetMainWindow();
            if (window == null) return;
            
            await ShowMessageDialog("Feature Not Available", 
                "Machine state save/load requires serialization support.\nThis feature will be added in a future update.");
        }
        
        private async Task SaveMachineStateAsync()
        {
            var window = GetMainWindow();
            if (window == null) return;
            
            await ShowMessageDialog("Feature Not Available", 
                "Machine state save/load requires serialization support.\nThis feature will be added in a future update.");
        }
        
        private async Task RelocateCurrentProgramAsync()
        {
            var window = GetMainWindow();
            if (window == null) return;
            
            if (string.IsNullOrWhiteSpace(_objectCodeText))
            {
                await ShowMessageDialog("Error", "No object code loaded. Please load or assemble a program first.");
                return;
            }
            
            string[] lines = _objectCodeText.Split('\n');
            string[] mods = _modRecordsText.Split('\n');
            
            // Warn user about relocation
            var warnDialog = new RelocationWarningDialog();
            await warnDialog.ShowDialog(window);
            
            if (warnDialog.DialogResult)
            {
                // Open relocation dialog
                var relocateDialog = new RelocateObjectFileDialog(lines, mods);
                await relocateDialog.ShowDialog(window);
                
                if (relocateDialog.DialogResult)
                {
                    int startingValue = relocateDialog.RelocatedToAddress;
                    RelocateLoadObjectFile(startingValue, lines, mods);
                    RefreshCPUDisplays();
                }
            }
        }
        
        private void Exit()
        {
            Environment.Exit(0);
        }
        
        
        private async Task CheckForUpdatesAsync()
        {
            var window = GetMainWindow();
            if (window == null) return;
            
            var msgBox = new Window
            {
                Title = "Check for Updates",
                Width = 400,
                Height = 150,
                Content = new TextBlock
                {
                    Text = "You are using the latest version of SIC Virtual Machine.",
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(20)
                }
            };
            await msgBox.ShowDialog(window);
        }
        
        private async Task SetProgramCounterToAsync()
        {
            var window = GetMainWindow();
            if (window == null) return;
            
            var dialog = new SetRegisterWordDialog();
            await dialog.ShowDialog(window);
            
            if (dialog.DialogResult && dialog.Register == "PC")
            {
                _cpu.PC = dialog.Value;
                RefreshCPUDisplays();
            }
        }
        
        private async Task SetMemoryByteAsync()
        {
            var window = GetMainWindow();
            if (window == null) return;
            
            var dialog = new SetMemoryByteDialog();
            await dialog.ShowDialog(window);
            
            if (dialog.DialogResult)
            {
                _cpu.StoreByte(dialog.Address, dialog.Value);
                RefreshCPUDisplays();
            }
        }
        
        private async Task SetMemoryWordAsync()
        {
            var window = GetMainWindow();
            if (window == null) return;
            
            var dialog = new SetMemoryWordDialog();
            await dialog.ShowDialog(window);
            
            if (dialog.DialogResult)
            {
                _cpu.StoreWord(dialog.Address, dialog.Value);
                RefreshCPUDisplays();
            }
        }
        
        private async Task AboutAsync()
        {
            var window = GetMainWindow();
            if (window == null) return;
            
            var dialog = new AboutDialog();
            await dialog.ShowDialog(window);
        }

        private void RefreshCPUDisplays()
        {
            // Update register hex values
            RegisterA_Hex = _cpu.A.ToString("X6");
            RegisterL_Hex = _cpu.L.ToString("X6");
            RegisterX_Hex = _cpu.X.ToString("X6");
            RegisterPC_Hex = _cpu.PC.ToString("X6");
            RegisterSW_Hex = _cpu.SW.ToString("X6");

            // Update register decimal values
            RegisterA_Dec = _cpu.A.ToString();
            RegisterL_Dec = _cpu.L.ToString();
            RegisterX_Dec = _cpu.X.ToString();
            RegisterPC_Dec = _cpu.PC.ToString();
            RegisterSW_Dec = _cpu.SW.ToString();

            // Update binary values for A
            int valA = _cpu.A;
            RegisterA_BIN_MSB = Convert.ToString((valA >> 16) & 0xFF, 2).PadLeft(8, '0');
            RegisterA_BIN_MIB = Convert.ToString((valA >> 8) & 0xFF, 2).PadLeft(8, '0');
            RegisterA_BIN_LSB = Convert.ToString(valA & 0xFF, 2).PadLeft(8, '0');

            // Update binary values for L
            int valL = _cpu.L;
            RegisterL_BIN_MSB = Convert.ToString((valL >> 16) & 0xFF, 2).PadLeft(8, '0');
            RegisterL_BIN_MIB = Convert.ToString((valL >> 8) & 0xFF, 2).PadLeft(8, '0');
            RegisterL_BIN_LSB = Convert.ToString(valL & 0xFF, 2).PadLeft(8, '0');

            // Update binary values for X
            int valX = _cpu.X;
            RegisterX_BIN_MSB = Convert.ToString((valX >> 16) & 0xFF, 2).PadLeft(8, '0');
            RegisterX_BIN_MIB = Convert.ToString((valX >> 8) & 0xFF, 2).PadLeft(8, '0');
            RegisterX_BIN_LSB = Convert.ToString(valX & 0xFF, 2).PadLeft(8, '0');

            // Update binary values for PC
            int valPC = _cpu.PC;
            RegisterPC_BIN_MSB = Convert.ToString((valPC >> 16) & 0xFF, 2).PadLeft(8, '0');
            RegisterPC_BIN_MIB = Convert.ToString((valPC >> 8) & 0xFF, 2).PadLeft(8, '0');
            RegisterPC_BIN_LSB = Convert.ToString(valPC & 0xFF, 2).PadLeft(8, '0');

            // Update binary values for SW
            int valSW = _cpu.SW;
            RegisterSW_BIN_MSB = Convert.ToString((valSW >> 16) & 0xFF, 2).PadLeft(8, '0');
            RegisterSW_BIN_MIB = Convert.ToString((valSW >> 8) & 0xFF, 2).PadLeft(8, '0');
            RegisterSW_BIN_LSB = Convert.ToString(valSW & 0xFF, 2).PadLeft(8, '0');

            // Update CC
            int cc = (_cpu.SW & 0xC0) >> 6;
            RegisterSW_CC = cc.ToString("D2");
            
            ComparisonResult = cc switch
            {
                0 => "Equal",
                1 => "Less Than",
                2 => "Greater Than",
                _ => "xx"
            };

            // Update instruction info
            if (_cpu.PC >= 0 && _cpu.PC < 32768)
            {
                string instrInfo = _cpu.GetInstructionDescription(_cpu.PC);
                string[] parts = instrInfo.Split('|');
                if (parts.Length >= 3)
                {
                    CurrentInstruction = parts[0];
                    CurrentInstructionDescription = parts[1];
                    CurrentInstructionEffect = parts[2];
                }
            }

            // Update next instruction
            if (_cpu.PC >= 0 && _cpu.PC + 3 < 32768)
            {
                string nextInstrInfo = _cpu.GetInstructionDescription(_cpu.PC + 3);
                string[] nextParts = nextInstrInfo.Split('|');
                if (nextParts.Length >= 3)
                {
                    NextInstruction = nextParts[0];
                    NextInstructionDescription = nextParts[1];
                    NextInstructionEffect = nextParts[2];
                }
            }

            // Update memory display
            RefreshMemoryDisplay();

            // Update microsteps
            MicrostepsDisplay = _cpu.MicrocodeSteps;
            
            // Update symbol table highlighting
            UpdateSymbolTableHighlight();
        }

        private string FormatSymbolTable(string instructionSource)
        {
            if (string.IsNullOrEmpty(instructionSource))
                return "";
            
            var lines = instructionSource.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            var sb = new System.Text.StringBuilder();
            
            // Clear and populate the rows for DataGrid
            _allSymbolTableRows.Clear();
            SymbolTableRows.Clear();
            
            bool isHeader = true;
            foreach (var line in lines)
            {
                var parts = line.Split('\t');
                if (parts.Length >= 5)
                {
                    string col1 = parts[0].PadRight(6);   // Line
                    string col2 = parts[1].PadRight(8);   // Address
                    string col3 = parts[2].PadRight(10);  // Symbol
                    string col4 = parts[3].PadRight(8);   // OpCode
                    string col5 = parts[4].PadRight(12);  // Operand
                    string col6 = parts.Length > 5 ? parts[5] : "";  // CurrentValue
                    
                    sb.AppendLine($"{col1}{col2}{col3}{col4}{col5}{col6}");
                    
                    if (!isHeader)
                    {
                        var row = new SymbolTableRowViewModel
                        {
                            Line = parts[0].Trim(),
                            Address = parts[1].Trim(),
                            Symbol = parts[2].Trim(),
                            OpCode = parts[3].Trim(),
                            Operand = parts[4].Trim(),
                            CurrentValue = parts.Length > 5 ? parts[5].Trim() : "",
                            RowBackground = global::Avalonia.Media.Brushes.White
                        };
                        
                        // Parse address for highlighting lookup
                        if (int.TryParse(parts[1].Trim(), System.Globalization.NumberStyles.HexNumber, null, out int addr))
                        {
                            row.AddressValue = addr;
                        }
                        
                        _allSymbolTableRows.Add(row);
                        SymbolTableRows.Add(row);
                    }
                    isHeader = false;
                }
                else if (parts.Length > 0)
                {
                    sb.AppendLine(line);
                    isHeader = false;
                }
            }
            
            return sb.ToString();
        }
        
        private void UpdateSymbolTableHighlight()
        {
            if (_allSymbolTableRows.Count == 0) return;
            
            int pc = _cpu.PC;
            
            // Reset all row backgrounds
            foreach (var row in _allSymbolTableRows)
            {
                row.RowBackground = global::Avalonia.Media.Brushes.White;
            }
            
            // Find and highlight the current PC row (green)
            SymbolTableRowViewModel? currentRow = null;
            foreach (var row in _allSymbolTableRows)
            {
                if (row.AddressValue == pc)
                {
                    row.RowBackground = global::Avalonia.Media.Brushes.LightGreen;
                    currentRow = row;
                    break;
                }
            }
            
            // Find and highlight the referenced symbol (light blue)
            if (currentRow != null && !string.IsNullOrEmpty(currentRow.Operand))
            {
                // Get the operand (might have ,X suffix for indexed addressing)
                string operand = currentRow.Operand.Split(',')[0].Trim();
                
                foreach (var row in _allSymbolTableRows)
                {
                    if (row.Symbol == operand && row != currentRow)
                    {
                        row.RowBackground = global::Avalonia.Media.Brushes.LightBlue;
                        break;
                    }
                }
            }
            
            // Force refresh of the collection to update UI
            var temp = new List<SymbolTableRowViewModel>(_allSymbolTableRows);
            SymbolTableRows.Clear();
            foreach (var row in temp)
            {
                SymbolTableRows.Add(row);
            }
        }

        private void RefreshMemoryDisplay()
        {
            int pc = _cpu.PC;
            
            // Track last valid PC for when program ends (PC = -1)
            if (pc >= 0 && pc < 32768)
            {
                _lastValidPC = pc;
            }
            
            // Use last valid PC if current PC is invalid (program ended)
            int displayPC = (pc >= 0 && pc < 32768) ? pc : _lastValidPC;
            
            // Calculate start address - show memory starting a few lines before PC
            int pcLine = (displayPC / 16) * 16;
            int startAddr = Math.Max(0, pcLine - 64); // Start 4 lines before PC
            int endAddr = Math.Min(32768, startAddr + 1024); // Show ~64 lines
            
            // Update header - show actual PC value (including -1 for halted)
            string pcDisplay = (pc == -1) ? "HALTED" : pc.ToString("X4");
            if (MemoryDisplayHex)
                MemoryHeader = $"Memory View (PC = {pcDisplay})";
            else if (MemoryDisplayBinary)
                MemoryHeader = $"Memory View (PC = {(pc == -1 ? "HALTED" : Convert.ToString(pc, 2).PadLeft(16, '0'))})";
            else if (MemoryDisplayDecimal)
                MemoryHeader = $"Memory View (PC = {(pc == -1 ? "HALTED" : pc.ToString())})";
            else
                MemoryHeader = $"Memory View (PC = {pcDisplay})";
            
            // Clear and rebuild memory lines
            MemoryLines.Clear();
            
            // Highlight color for current instruction bytes
            var highlightBg = global::Avalonia.Media.Brushes.Yellow;
            var normalBg = global::Avalonia.Media.Brushes.Transparent;
            var normalFg = global::Avalonia.Media.Brushes.Black;
            
            if (MemoryDisplayHex)
            {
                for (int i = startAddr; i < endAddr; i += 16)
                {
                    var line = new MemoryLineViewModel
                    {
                        AddressText = i.ToString("X4") + ": "
                    };
                    
                    for (int j = 0; j < 16; j++)
                    {
                        int addr = i + j;
                        if (addr < 32768)
                        {
                            // Only highlight if PC is valid (not halted)
                            bool isPC = (pc >= 0) && (addr >= displayPC && addr < displayPC + 3);
                            line.Bytes.Add(new MemoryByteViewModel
                            {
                                Text = _cpu.MemoryBytes[addr].ToString("X2"),
                                Background = isPC ? highlightBg : normalBg,
                                Foreground = normalFg
                            });
                        }
                    }
                    
                    MemoryLines.Add(line);
                }
            }
            else if (MemoryDisplayBinary)
            {
                int binPcLine = (displayPC / 6) * 6;
                int binStartAddr = Math.Max(0, binPcLine - 24);
                int binEndAddr = Math.Min(32768, binStartAddr + 360);
                
                for (int i = binStartAddr; i < binEndAddr; i += 6)
                {
                    var line = new MemoryLineViewModel
                    {
                        AddressText = Convert.ToString(i, 2).PadLeft(16, '0') + ": "
                    };
                    
                    for (int j = 0; j < 6; j++)
                    {
                        int addr = i + j;
                        if (addr < 32768)
                        {
                            bool isPC = (pc >= 0) && (addr >= displayPC && addr < displayPC + 3);
                            string bits = Convert.ToString(_cpu.MemoryBytes[addr], 2).PadLeft(8, '0');
                            line.Bytes.Add(new MemoryByteViewModel
                            {
                                Text = bits,
                                Background = isPC ? highlightBg : normalBg,
                                Foreground = normalFg
                            });
                        }
                    }
                    
                    MemoryLines.Add(line);
                }
            }
            else if (MemoryDisplayDecimal)
            {
                for (int i = startAddr; i < endAddr; i += 16)
                {
                    var line = new MemoryLineViewModel
                    {
                        AddressText = i.ToString("D5") + ": "
                    };
                    
                    for (int j = 0; j < 16; j++)
                    {
                        int addr = i + j;
                        if (addr < 32768)
                        {
                            bool isPC = (pc >= 0) && (addr >= displayPC && addr < displayPC + 3);
                            string val = _cpu.MemoryBytes[addr].ToString().PadLeft(3, ' ');
                            line.Bytes.Add(new MemoryByteViewModel
                            {
                                Text = val,
                                Background = isPC ? highlightBg : normalBg,
                                Foreground = normalFg
                            });
                        }
                    }
                    
                    MemoryLines.Add(line);
                }
            }
            else if (MemoryDisplayAscii)
            {
                for (int i = startAddr; i < endAddr; i += 16)
                {
                    var line = new MemoryLineViewModel
                    {
                        AddressText = i.ToString("X4") + ": "
                    };
                    
                    for (int j = 0; j < 16; j++)
                    {
                        int addr = i + j;
                        if (addr < 32768)
                        {
                            byte b = _cpu.MemoryBytes[addr];
                            char c = (b >= 32 && b < 127) ? (char)b : '.';
                            bool isPC = (pc >= 0) && (addr >= displayPC && addr < displayPC + 3);
                            line.Bytes.Add(new MemoryByteViewModel
                            {
                                Text = c.ToString(),
                                Background = isPC ? highlightBg : normalBg,
                                Foreground = normalFg
                            });
                        }
                    }
                    
                    MemoryLines.Add(line);
                }
            }
        }

        private void RefreshDeviceDisplay()
        {
            Devices.Clear();
            for (int i = 0; i < SIC_CPU.NumDevices; i++)
            {
                Devices.Add(new DeviceViewModel
                {
                    DeviceID = i.ToString("D2"), // Zero-padded two digits like "00", "01", etc.
                    ASCIIOutput = _cpu.Devices[i].GetASCIIStringWrites(),
                    HexOutput = _cpu.Devices[i].GetHEXStringWrites()
                });
            }
        }

        partial void OnMemoryDisplayBinaryChanged(bool value) { if (value) RefreshMemoryDisplay(); }
        partial void OnMemoryDisplayHexChanged(bool value) { if (value) RefreshMemoryDisplay(); }
        partial void OnMemoryDisplayDecimalChanged(bool value) { if (value) RefreshMemoryDisplay(); }
        partial void OnMemoryDisplayAsciiChanged(bool value) { if (value) RefreshMemoryDisplay(); }
        
        private async Task ShowMessageDialog(string title, string message)
        {
            var window = GetMainWindow();
            if (window == null) return;
            
            var dialog = new Window
            {
                Title = title,
                Width = 400,
                Height = 150,
                Content = new TextBlock
                {
                    Text = message,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(20)
                }
            };
            await dialog.ShowDialog(window);
        }
        
        private async Task AssembleSICFileAsync(string filename)
        {
            Assembler assembler = new Assembler(filename);
            _cpu.getSICSource(assembler);
            
            if (!string.IsNullOrEmpty(assembler.ObjectCode))
            {
                SymbolTableDisplay = FormatSymbolTable(assembler.InstructionSource);
                ObjectCodeDisplay = assembler.ObjectCode;
                ModificationRecordsDisplay = assembler.ModRecords;
                _objectCodeText = assembler.ObjectCode;
                _modRecordsText = assembler.ModRecords;
                
                string[] lines = assembler.ObjectCode.Split('\n');
                string[] mods = assembler.ModRecords.Split('\n');
                
                // Show the Relocate Prompt Dialog
                var window = GetMainWindow();
                if (window != null)
                {
                    var relocatePrompt = new SIC.Avalonia.Views.Dialogs.RelocatePromptDialog();
                    await relocatePrompt.ShowDialog(window);
                    
                    if (relocatePrompt.DialogResult)
                    {
                        if (relocatePrompt.UseRelocatingLoader)
                        {
                            var relocateDialog = new SIC.Avalonia.Views.Dialogs.RelocateObjectFileDialog(lines, mods);
                            await relocateDialog.ShowDialog(window);
                            
                            if (relocateDialog.DialogResult)
                            {
                                int startAddr = relocateDialog.RelocatedToAddress;
                                RelocateLoadObjectFile(startAddr, lines, mods);
                            }
                            else
                            {
                                LoadObjectFile(lines);
                            }
                        }
                        else
                        {
                            LoadObjectFile(lines);
                        }
                    }
                }
                else
                {
                    LoadObjectFile(lines);
                }
            }
            RefreshCPUDisplays();
        }
        
        private void LoadObjectFile(string[] lines)
        {
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                
                if (line[0] == 'H')
                {
                    var firstAddress = line.Substring(7, 6);
                    var programSize = line.Substring(13, 6);
                    _cpu.CurrentProgramEndAddress = int.Parse(firstAddress, System.Globalization.NumberStyles.HexNumber) + 
                                                     int.Parse(programSize, System.Globalization.NumberStyles.HexNumber);
                }
                else if (line[0] == 'T')
                {
                    int recordStartAddress = 0;
                    int recordLength = 0;
                    ReadTextRecord(line, ref recordStartAddress, ref recordLength);
                    _cpu.LoadToMemory(line, recordStartAddress, recordLength);
                }
                else if (line[0] == 'E')
                {
                    int addressOfFirstInstruction = 0;
                    ReadEndRecord(line, ref addressOfFirstInstruction);
                    _cpu.PC = addressOfFirstInstruction;
                    _cpu.CurrentProgramStartAddress = addressOfFirstInstruction;
                }
            }
        }
        
        private void ReadTextRecord(string line, ref int recordStartAdd, ref int recordLength)
        {
            int i = 1, num = 0;
            while (i < 7)
            {
                char ch = line[i++];
                if (ch >= 'A') ch -= (char)7;
                ch -= (char)48;
                num += (int)ch;
                num = num << 4;
            }
            num = num >> 4;
            recordStartAdd = num;
            num = 0;
            while (i < 9)
            {
                char ch = line[i++];
                if (ch >= 'A') ch -= (char)7;
                ch -= (char)48;
                num += (int)ch;
                num = num << 4;
            }
            num = num >> 4;
            recordLength = num;
        }
        
        private void ReadEndRecord(string line, ref int firstExecIns)
        {
            int i = 1, num = 0;
            while (i < 7)
            {
                char ch = line[i++];
                if (ch >= 'A') ch -= (char)7;
                ch -= (char)48;
                num += (int)ch;
                num = num << 4;
            }
            firstExecIns = num >> 4;
        }
        
        private void RelocateLoadObjectFile(int newAddress, string[] unmodified, string[] Mrec)
        {
            int lineNum = 0;
            int linePos = 0;
            int curMemoryAddress = 0;
            
            // Get the original starting address from H-record
            int oldAddress = int.Parse(unmodified[lineNum].Substring(9, 4), System.Globalization.NumberStyles.HexNumber);
            int offset = newAddress - oldAddress;
            
            string[] lines = new string[unmodified.Length];
            unmodified.CopyTo(lines, 0);
            
            // Modify H-Record - update starting address
            lines[lineNum] = lines[lineNum].Substring(0, 9) + newAddress.ToString("X4") + lines[lineNum].Substring(13);
            
            // Modify E-Record - update first executable instruction address
            int firstInstruction = int.Parse(lines[unmodified.Length - 1].Substring(3, 4), System.Globalization.NumberStyles.HexNumber);
            firstInstruction += offset;
            lines[lines.Length - 1] = "E00" + firstInstruction.ToString("X4") + "\n";
            
            // Modify T-Records
            lineNum++;
            linePos = TRecordOverhead(lineNum, ref lines, offset, ref curMemoryAddress);
            
            // Loop through M-records and adjust addresses
            foreach (string rec in Mrec)
            {
                if (string.IsNullOrWhiteSpace(rec)) continue;
                
                // Get the address that needs to be modified from the M-record
                string addressSubstring = rec.Substring(3, 4);
                int targetAddress = int.Parse(addressSubstring, System.Globalization.NumberStyles.HexNumber);
                
                // Adjust the address
                AdjustString(ref lines, ref lineNum, ref linePos, ref curMemoryAddress, targetAddress, offset);
            }
            
            // Update the relocated object code display
            RelocatedObjectCodeDisplay = string.Join("\n", lines);
            
            // Call the absolute loader on the relocated object code
            LoadObjectFile(lines);
        }
        
        private int TRecordOverhead(int lineNum, ref string[] lines, int offset, ref int address)
        {
            // Get position from T-record (characters 3-6 are the starting address)
            int pos = int.Parse(lines[lineNum].Substring(3, 4), System.Globalization.NumberStyles.HexNumber);
            address = pos;
            pos += offset;
            
            // Update the T-record with new starting address
            string replacement = pos.ToString("X4");
            lines[lineNum] = lines[lineNum].Substring(0, 3) + replacement + lines[lineNum].Substring(7);
            
            return 9; // Return position after the header portion (T + 4 addr + 2 length = 9)
        }
        
        private bool AdjustString(ref string[] lines, ref int lineNum, ref int linePos, ref int address, int targetAddress, int offset)
        {
            // Move through string until target address is hit
            while (lineNum < lines.Length - 1)
            {
                // Check if we've reached the target address and have enough characters
                if (address == targetAddress && linePos <= lines[lineNum].Length - 4)
                {
                    // Get old address and add offset
                    int oldpos = int.Parse(lines[lineNum].Substring(linePos, 4), System.Globalization.NumberStyles.HexNumber);
                    oldpos += offset;
                    string replacement = oldpos.ToString("X4");
                    
                    // Update the T-record
                    lines[lineNum] = lines[lineNum].Substring(0, linePos) + replacement + lines[lineNum].Substring(linePos + 4);
                    
                    // Move 4 characters (2 bytes)
                    linePos += 4;
                    address += 2;
                    return true;
                }
                
                // If we've gone past the target or reached end of line
                if (address > targetAddress)
                {
                    return false;
                }
                
                // Move to next byte in current line
                if (linePos < lines[lineNum].Length - 2)
                {
                    linePos += 2;
                    address += 1;
                }
                else
                {
                    // Move to next T-record
                    lineNum++;
                    if (lineNum < lines.Length - 1 && lines[lineNum].Length > 0 && lines[lineNum][0] == 'T')
                    {
                        linePos = TRecordOverhead(lineNum, ref lines, offset, ref address);
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false;
        }
    }

    public class DeviceViewModel
    {
        public string DeviceID { get; set; } = "";
        public string ASCIIOutput { get; set; } = "";
        public string HexOutput { get; set; } = "";
    }
    
    public class MemoryLineViewModel
    {
        public string Text { get; set; } = "";
        public string AddressText { get; set; } = "";
        public global::Avalonia.Media.IBrush Background { get; set; } = global::Avalonia.Media.Brushes.Transparent;
        public global::Avalonia.Media.IBrush Foreground { get; set; } = global::Avalonia.Media.Brushes.Black;
        public ObservableCollection<MemoryByteViewModel> Bytes { get; set; } = new();
    }
    
    public class MemoryByteViewModel
    {
        public string Text { get; set; } = "";
        public global::Avalonia.Media.IBrush Background { get; set; } = global::Avalonia.Media.Brushes.Transparent;
        public global::Avalonia.Media.IBrush Foreground { get; set; } = global::Avalonia.Media.Brushes.Black;
    }
    
    public class SymbolTableRowViewModel : ObservableObject
    {
        public string Line { get; set; } = "";
        public string Address { get; set; } = "";
        public string Symbol { get; set; } = "";
        public string OpCode { get; set; } = "";
        public string Operand { get; set; } = "";
        public string CurrentValue { get; set; } = "";
        public int AddressValue { get; set; } = -1;
        
        private global::Avalonia.Media.IBrush _rowBackground = global::Avalonia.Media.Brushes.White;
        public global::Avalonia.Media.IBrush RowBackground 
        { 
            get => _rowBackground;
            set => SetProperty(ref _rowBackground, value);
        }
    }
}
