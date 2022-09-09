# Wakatime for unity
A [WakaTime](https://wakatime.com/) plugin for [Unity](https://unity.com/).

## About
I've tried this existing solution (https://github.com/vladfaust/unity-wakatime) but soon found some problems with this repo. I've tried to fix those problems and create PR's, but failed. So I took it upon myself to create something new. Where possible, I used the same settings as the repo from vladfaust. So your settings will work in this plugin.

## A list of things todo

 - [ ] Add setup instructions
	 - [ ] Support Unity package manager
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
 - [ ] Add git info to heartbeat
