# KeyPressStats
A tool that tracks your keypresses and stores them locally in a JSON file. 

![image](https://github.com/ziptieentity/Keypress-Stats/assets/48808194/4a5e49b7-999e-44b9-8318-cd921888d475)

## How to use
- Head over to [releases](https://github.com/ziptieentity/Keypress-Stats/releases) and download the latest version
- Launch **KeyPressStats.exe**
- In your system tray, you should see Keypress Stats is now open 

![image](https://github.com/ziptieentity/Keypress-Stats/assets/48808194/af5d2796-ee3f-4337-8638-e27eb6e93a69)
- Right click on it and it will show you your most and least used keys and when it next updates. You can also click the **Open Keypresses File** to launch the **Keypresses.json** file which contains all of your keypress stats.
- You can add Keypress Stats to your startup apps. On windows, press Win + R and type **shell:startup** and then create a shortcut of **KeyPressStats.exe** inside of the startup folder.

## How does it work?
Every time you press a key, it stores that keypress in a list and every 30 seconds, all of the keys inside of that list will be stored in the **Keypresses.json** file and sorted.

### Notes
Requires [.NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
