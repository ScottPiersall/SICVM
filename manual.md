# SIC Virtual Machine Manual 

## Preface

Due to the nature of modern microprocessors having subtle yet complex functions for efficiency purposes, teaching/learning systems programming can prove to be a difficult task. This problem is solved with the SIC by getting rid of these complex behaviors to have an architecture that is much more simplified for people who want to start learning systems software. This virtual machine was created to assist new systems programmers in their crafting and testing of SIC code. This manual will serve as a guide on installing the software and testing your SIC programs.

## Installation Instructions (.Exe File)

The easiest way to install the SICvm is to download the compiled binaries in the releases tab of this repository. If you are Windows, running the SICvm is as simple as installing the exe file and double clicking the file when downloaded. The releases for this project can be found here: https://github.com/ScottPiersall/SICVM/releases

## Installation Instructions (Visual Studio)

1. If you do not already have it, download the latest version of Visual Studio and make sure the .NET Core framework and C# are selected during the install process. The community edition is free and will suffice for this project.

2. Open up the Visual Studio application and select the "Clone a repository" button on the get started menu

3. On the next screen it will prompt you for a Repository location. For this field simply copy and paste the link to this github page, https://github.com/ScottPiersall/SICVM. If you want you can change the local path which is the location on your computer where the files will be stored. After that is done click the button on the bottom of the screen titled "Clone".

4. You should now see the Visual Studio IDE screen. To start the SICvm simply click the "Start" button on the top bar next to the green arrow and you're good to go!

![startimg](https://github.com/EllisLevine/imagesForSICmanual/blob/main/start.PNG)


## Using SICvm

If the installation instructions were done correctly you should be greeted with a GUI displaying the five SIC CPU registers, the next instruction, and three tabs on the bottom. The three tabs on the bottom will show the memory once loaded in, the devices and the microsteps the processor is doing. These areas will become populated after your SIC source or object file is loaded in.

### Loading in your SIC program

There are three ways you can load programs into memory in the SICvm and they can all be found in the File tab in the top left. The most common one we expect students to use is "Load and Assemble Sic Source File" which takes a .sic file, assembles it, and loads it into memory. If you have an object file you can also load those files by having their extension be .sic.obj. Finally, you can also load a SIC Machine state that you have saved previously with the "Load Saved SIC Machine State" option.

![loaded](https://github.com/EllisLevine/imagesForSICmanual/blob/main/loaded%20program.PNG)

An example of the SICvm after a SIC file has been loaded and assembled.

### The Machine Tab

The machine tab in the top left of the SIC machine provides many useful features before and after code has been loaded into the machine. In this section I will give a quick summary of these options.

* Zero All Memory: This option simply allows the user to make all the memory back to its original state of all zeros.

* Randomize Memory: This option is great to test if your SIC program allocates memory correctly. I recommend randomizing all of the memory first and then loading in your sic program.

* Set Program Counter To: Allows you to change the (PC) register which changes where the SICvm is reading your program. Setting this value to all zeros will let you loop through your program again.

* Set Memory Byte/Word: This option will allow you to set a byte or word anywhere you specify in the program. This can be done before or after you load the program in

* Reset SIC Virtual Machine: This option is perhaps one of the most useful as it completely clears any input, registers or memory the SICvm is occupying allowing you to input your file again without closing the program.


## Testing SIC Programs

After a .sic file is loaded into memory you can step through the instructions to see what is happening in each space in memory and what exactly the values in the register are. In this part of the manual I will took about basic Input/Output for the virtual machine so you can test your programs with certain values.

### Input

While you can always hard code values in your sic program for testing. As a systems programmer you might receive a problem where you have to get a value that is wired to a certain word in memory and you have to output it to a specified device. This can be tested on the SICvm by using the Set Memory options in the Machine tab. Select the  Set Memory Word in the machine tab and input the address in hex of the symbol you want to store, then input the value you want to store in the next section and click Ok.

![lastword](https://github.com/EllisLevine/imagesForSICmanual/blob/main/lastword.PNG)

In this example, the program length is 84 and we are trying to put the value of "5" into the last word in memory which is program length minus three.

### Output 

Once your program is loaded in and you have set an input word in memory to the appropriate location you can step through the program and see what is doing with the memory and registers. If you wrote your SIC program to print to devices correctly (using the WD opcode) you should see your output appear on the Devices tab next to the specified device ID you are writing to. This will happen after stepping through the program enough to get to the address of the WD instruction.

![output](https://github.com/EllisLevine/imagesForSICmanual/blob/main/output.PNG)

An example output of a program that converts seconds past midnight to 12 hour time and writes that output to device number 64.

### The "Run" Button

The process of stepping through the memory to obtain results in devices can be completely avoided with the "Run" button. However, for this to work, the program must stop gracefully. Adding the following assembly code to your program before jumping to the end and ending the program will allow the SIC code to stop gracefully.

```
ZERO WORD 0
     LDL ZERO
     RSUB
```
If done correctly the SICvm should display the text similar to the image below and your output should be available to whatever device you wrote to in the devices tab.

![halted](https://github.com/EllisLevine/imagesForSICmanual/blob/main/halted.PNG)





