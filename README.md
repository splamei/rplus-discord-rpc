
# Rhythm Plus DiscordRPC
This is an app and extension that allows you to play the [Rhythm Plus music game](https://rhythm-plus.com) in your web browser and show others that you're playing the game within Discord via Rich Prescence and optionally showing some stats about your game!

Rhythm Plus is a web-based vertical scrolling rhythm game (VSRG), you can make, play, and share any songs from and with anyone! The game can also be played on PC and other platforms with a working web-browser.
## What does it  do?

 This comes in two parts, the client extensions and the server app. The client extension is a Chromium extension that gets information about your game before sending that to the server app which is installed on your device. This is then what takes that data, and displays it through a Discord Rich Prescence for your friends to see!
 
 TL;DR The extensions gets the data about the game and send it to the server app to display as a DiscordRPC

## Compatibility
- A 64-bit version of Windows 10 or 11
- .NET Framework 4.7.2 (Should be preinstalled)
- Be able to access Rhythm Plus in a Chromium Web Browser

## Installation
Note: You must have the Discord app installed as it will not work on the website version
1. Download the server app and extension through the [releases tabs](https://github.com/splamei/rplus-discord-rpc/releases)
2. Extract the download zip file via your fave tool
3. Go to the extension page on your web browser and enable developer mode (toggle usually in the top-right corner)
4. Press 'Load Unpacked' and open the folder with the extension (from the zip you extracted). It should be named ``Extension``.
5. You may see an 'Errors' button show by the extension on the extensions page. This is normal and can be ignored.
6. Open the file named ``Server App Installer.exe`` to install the server app on your device. You can choose to allow it to run at start-up
7. When starting the server app. Windows may ask about the apps connections. It's best to press allow though you should be able to press deny and not suffer much issues.
8. When visiting the Rhythm Plus website, you should now see a Discord Rich Prescence show
## How to configure
To configure the app and extension, simply press the extension icon in the address bars extension menu or press the apps icon in the taskbar system tray. You'll see options to configure the following:
| Setting name | Default Value | Description |
|--|--|--|
| Server Port (Extension + Server) | 55256 | The port that the server app and extension use to communicate. Only change this if it conflicts with other apps |
| Update Interval in seconds (Extension) | 2 | How often to send the data to the server app. Must be smaller then the timeout time
| Update Timeout (Server) | 3 | How long to wait before hiding the Rich Presence. Must be higher then the update interval to prevent hiding the RPC then showing
| Show your current stats when playing a chart (Server) | On | Shows the current stats like score and accuracy when your playing a chart
| Show the current chart being played (Server) | On | Shows the song name, author, subtitle and charter of the song being played

## Using this for other websites
If you want to use this to have DiscordRPC on other websites, you can though please do the following:
 - Change the port used by the extension and server app
 - Change the namespace, names and other information
 - If it's not for Rhythm Plus, you should change the icon and target site
 - Because this client is currently under the MIT Licence, you **must** acknowledge the copyright and MIT Licence
## Contributing
I would love for people to help the extension and server app's development so any little contribution would go a long way!
You could contribute by:
 - Reporting issues / feature requests on the [issue page](https://github.com/splamei/rplus-discord-rpc/issues)
 - Making forks of the repo
 - Making pull request to add code or fix bugs
 - Staring or watching the repo
 - Sharing the repo!

## FAQ
### Q) Does this support the Splamei client?
There is no need for this on the Splamei PC client since it already has Rich Prescence build it. (It may even break the client)
### Q) Can this run on mobile?
No. Most mobile browsers do not support extensions so checking if your playing Rhythm Plus and getting the game stats would be almost impossible plus Rich Presence only works on the Desktop version of Discord
### Q) Can older Windows versions run this?
If you can get the .NET Framework 4.7.2 working, get Discord installed and working and a modern Chromium browser that supports Manifest V3, then yes, it should work.
### Q) Can it run on ARM64 or 32-bit devices
No. The server app is build for x64 CPUs only at the moment so you will have to build it yourself.
### Q) Can I use this for other websites?
Yes! Just make sure you follow everything stated in the 'Using this for other websites' section
### Q) Is this client official?
no
## Socials
[YouTube](https://youtube.com/@splamei)
[Twitch](https://twitch.tv/splamei)
[Twitter](https://twitter.com/splamei)
[BlueSky](http://splamei.bsky.social/)
[Discord](https://discord.gg/g2KTP5X9At)
## Built with ❤️ by Splamei
