# SICVM
A SIC Virtual Machine, Emulator Assembler, loader and Linker


## Features

* SIC Virtual Machine. Full memory, Registers, CC and Status Words
* Device Connections with read and write bytes shown
* Full Assembler for SIC Source (Pass 1 and Pass 2)
* Step through memory one instruction at a time
* Symbol Display
* Asbolute and Relocating Loader for SIC Object Files
* SIC Linker


## Releases

Release Date	|	Version		| Change(s)
----------------|-----------------------|------------------------------------------------------------------------------------
08 Nov 2021 | 21.11.08.1  |  Fixed Issue with About Dialog where columns were formatted incorrectly
05 Nov 2020	|	20.11.05.1	|  Bugfixed issue with exception being thrown when stepping through high memory addresses
04 Nov 2020	|	20.11.04.1	|  Bugfixed microcode for JEQ opcode. Bugfixes To Assembler. Improved Memory Display
02 Nov 2020	|	20.11.02.1	|  Bugfixed microcode for JSUB opcode. Bugfixed Device Output display.
02 Nov 2020	|	20.11.02.0	|  Bugfixed to Assembler END directive handler. Added handler for Set PC OPtion in GUI.
31 Oct 2020	|	20.10.31.0	|  Pass 1 and Pass 2 of Assembler Added. Object file loaded on successful assembly.
30 Oct 2020	|	20.10.30.0	|	Initial Release. Load object files, step through them. see memory and registers


## Contributors
Name             | Contribution(s) / Lead 
--------------   | ---------------------------------------------
Scott Piersall   | Chief Architect & Development Lead
Riley Strickland | Pass 1 & 2 of SIC Assembler
Ellis Levine     | Pass 1 & 2 of SIC Assembler
Kris Wieben      | GUI & VM Testing
Brandon Woodrum | Absolute & Relocating SIC Loader
