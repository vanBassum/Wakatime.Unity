# Wakatime for unity
A [WakaTime](https://wakatime.com/) plugin for [Unity](https://unity.com/).

## About
I've tried this existing solution (https://github.com/vladfaust/unity-wakatime) but soon found some problems with this repo. I've tried to fix those problems and create PR's, but failed. So I took it upon myself to create something new. Where possible, I used the same settings as the repo from vladfaust. So your settings will work in this plugin.

## A list of things todo

 - [ ] Add setup instructions
	 - [ ] Support Unity package manager
 - [ ] Add basic features list
 - [ ] Add console logging
 - [ ] Catch events and create heartbeats
 - [ ] Send heartbeats async to wakatime
	 - [ ] Implement heartbeat call
	 - [ ] Implement heartbeat bulk call
	 - [ ] Implement cooldown technique
	 - [ ] Handle http status codes
 - [ ] Improve settings UI
	 - [ ] Add setting, logging niveau
	 - [ ] Add button to open dashboard
	 - [ ] Add button to open ApiKey page
 - [ ] Add git info to heartbeat
