#About
An example program of using a commandline parser. 
This is usefule when you have a console program that should be run with different arguments eg:

    whatisnew.exe -o "c:\oldfile.txt" --k1,2 -n "c:\newfile.txt" --k3,4 

It uses [commandline version 1.9](https://github.com/gsscoder/commandline/tree/stable-1.9.71.2 "Link to github repo commandline") which has a [getopts](https://en.wikipedia.org/wiki/Getopts "Link to wiki article about unix tool getopts") style commandline parser. It is here for future note for me and also for blog article reference. 

##What does it do?
The program WhatIsNew takes two csv files and see what is new in one of them. It is very similar to [zetcmd](https://github.com/patriklindstrom/ZetCmd "Link to github zetcmd repo") but it focuses on the commandline part.
##Roadmap
There will be a branch for the commandline 2.0 version when it is stable as well. 