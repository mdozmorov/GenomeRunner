* GUI Version
By default, building in Visual Studio will compile and run the GUI version.

* Command-line version, Windows
1. Click on My Project, change Application Type to "Console Application"
2. Change the Startup Object to "Sub Main"
3. In the Solution Explorer (file browser, right pane), select all the *.vb files in the src/GenomeRunner/ui/graphical folder and change the "Build Action" property from "Compile" to "None".

* Command-line version, POSIX
IN VISUAL STUDIO:
1. Click on My Project, change Application Type to "Class Library".
2. In the Solution Explorer (file browser, right pane), select all the *.vb files in the src/GenomeRunner/ui/graphical folder and change the "Build Action" property from "Compile" to "None".

3. Transfer the folder to a Linux system with Mono installed or open a Cygwin shell with Mono on the path.  For example in Ubuntu, "sudo apt-get install mono-complete" will prepare your system.
4. Run the Makefile ("make"). The (statically-linked) binary will be output to "genome-runner".
