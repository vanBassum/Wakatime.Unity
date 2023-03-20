# Wakatime for Unity

A [WakaTime](https://wakatime.com/) plugin for [Unity](https://unity.com/).

![image](https://user-images.githubusercontent.com/38683014/219443681-8f05fb81-13ae-49e4-8ce8-76eb5ecf931f.png)

## About

I tried this [existing plugin](https://github.com/vladfaust/unity-wakatime) from @vladfaust but soon found some problems with it. I tried to fix those problems and create PRs, but failed. So, I took it upon myself to create something new. Where possible, I've used the same settings as @vladfaust's plugin, so your settings should still work here.

## Getting started

- Open your Unity project and navigate to "Window -> Package Manager".
- Click on the little `+` sign and choose "Add package from git URL...".
- Enter the following URL: "https://github.com/vanBassum/Wakatime.Unity.git"
- After installation, a new menu button will be added under "Services -> vanbassum -> Wakatime".
- Open this menu, click "Enable WakaTime" and enter the API key.
- Also ensure that "WakaTime CLI" path is set to the correct absolute path of your WakaTime CLI executable ([download here](https://github.com/wakatime/wakatime-cli/releases))
- Click "Save preferences" and enjoy working with this plugin.

## A list of things to do

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
