# Wakatime for unity
A [WakaTime](https://wakatime.com/) plugin for [Unity](https://unity.com/).

![image](https://user-images.githubusercontent.com/38683014/219443681-8f05fb81-13ae-49e4-8ce8-76eb5ecf931f.png)

## About
I've tried this existing solution (https://github.com/vladfaust/unity-wakatime) but soon found some problems with this repo. I've tried to fix those problems and create PR's, but failed. So I took it upon myself to create something new. Where possible, I used the same settings as the repo from vladfaust. So your settings will work in this plugin.

## Getting started
- Open your unity project and navigate to Window -> Package Manager. 
- Click on the little + sign and choose "add package from GIT url". 
- Enter the following url: "https://github.com/vanBassum/Wakatime.Unity.git" 
- After installation a new menu button will be added under Services -> vanbassum -> Wakatime. 
- OPen this menu, enable wakatime and enter the api key.
- Also ensure that the wakatime cli path is set to the wakatime cli. 
- This can be downloaded here: https://github.com/wakatime/wakatime-cli/releases
- Click save preferences and enjoy working with this plugin.


## A list of things todo
 - [ ] Add an auto update feature that downloads the cli automatically
 - [x] Add setup instructions
	 - [x] Support Unity package manager
	 - [x] Add getting started
 - [x] Add console logging
 - [x] Catch events and create heartbeats
 - [x] Send heartbeats async to wakatime
	 - [x] Implement heartbeat call
	 - [x] Implement heartbeat bulk call
	 - [x] Implement cooldown technique
	 - [x] Handle http status codes
 - [x] Improve settings UI
	 - [x] Add setting, logging niveau
	 - [x] Add button to open dashboard
	 - [x] Add button to open ApiKey page
 - [x] Add info to heartbeat
	 - [x] Git branching info
	 - [x] Current OS
	 - [x] Editor information.
