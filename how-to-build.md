# HOW TO BUILD DOTLESS


dotless uses [Cake build](https://cakebuild.net/) to build the project and packages.
> Cake (C# Make) is a cross platform build automation system with a C# DSL to do things like compiling code, copy files/folders, running unit tests, compress files and build NuGet packages.

To build simply execute build.ps1 (or build.sh on Linux).

Possible build targets:
* Clean = Removes all build artefacts
* Build = Builds the main dotless solution
* Test = (Default) Builds the solution and runs Unit tests using NUnit
* Publish = Executes Build, Test and publishes the Nuget packages.

To run another build targert simply run:
PS> .\build.ps1 -Target <target>


### Note:
You may need to execute the following command before being able to execute the script:
Set-ExecutionPolicy remotesigned
(Make sure you run this as administrator and inside PowerShell)

Please read the [PowerShell Security Topic](https://cakebuild.net/docs/tutorials/powershell-security) of cake for further help.

