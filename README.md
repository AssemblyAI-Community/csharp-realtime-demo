# AssemblyAI Real-Time C# Demo

# Setting up the Project

This project is designed to be cross-platform, but does have a few dependencies regardless of which OS you're using. For starters, you'll need to install [SoX](https://sourceforge.net/projects/sox/), the audio processing program we'll use for recording our microphone. 

### Installing and Setting Up SoX
To install SoX for Windows, navigate to the "Files" tab on SourceForge, then select "sox" and then the latest version's folder. For Windows, download and run the .exe file to install SoX and enable using it from CMD or PowerShell.

For Linux and MacOS, install instructions can be found [here](https://pysox.readthedocs.io/en/latest/).

Verify that your installation is working correctly by recording your microphone to file with the command `sox -d out.wav`. This will make a new file `out.wav` in your current directory and write your microphone recording to it. Please note that when using `sox -d`, SoX will use the default audio input device that you've configured in your OS, so verify that this is the one you're expecting.

### Installing and Setting Up .NET

Now you'll need to set up your .NET environment to make sure you can configure and run this project. Instructions for all operating systems are maintained by Microsoft and can be found [here](https://learn.microsoft.com/en-us/dotnet/core/install/).

Once it's successfully installed, you'll now have access to the .NET Command Line Interface (CLI) which will allow you to build and run this project.

# Running the Project

With the .NET CLI, navigate to the root folder of this project. Then run `dotnet build` to verify that the build completes successfully and that you've got all necessasry dependencies on your system. No third-party libraries besides SoX are needed for this project, so if your .NET installation went smoothly, you should have all of the .dll files you'll need to use built-in .NET packages.

Then, add your AssemblyAI API key on line 29 to replace the `"API_KEY_HERE"` placeholder. Your API key can be found on your [account dashboard](https://www.assemblyai.com/app/account), and if you don't have an account yet, you can sign up [here](https://www.assemblyai.com/dashboard/signup). Please note that you'll need to have an upgraded AssemblyAI account with funds deposited in order to use real-time.

Now you can use `dotnet run` to start the program and start receiving Partial and FinalTranscripts of your microphone's input. We hope this gets you started with using AssemblyAI's real-time API in your C# projects, and please feel free to contact support@assemblyai.com if you have any questions!
