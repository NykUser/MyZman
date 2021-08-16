# ZmanCode
A small desktop program to calculate different Jewish Zmanim And Dates. With options for exporting and setting reminders.

[This link](https://downgit.github.io/#/home?url=https://github.com/NykUser/MyZman/tree/master/MyZmanPortable) can be used to download the portable .exe, 
and this [is a link](https://github.com/NykUser/MyZman/blob/master/MyZmanPortable/eng.mp4) to a clip with example uses of the program

The Code is released under the LGPL 2.1 license.

# Credits:
Core Zmanim calculations based on Eliyahu Hershfeld work [KosherJava](https://kosherjava.com/) - this is its [GitHub Home](https://github.com/KosherJava/zmanim)

Project uses the NET port [from here](https://github.com/Yitzchok/Zmanim)

Project uses the GeoTimeZone library [from here](https://github.com/mattjohnsonpint/GeoTimeZone) to look up time zones according to Geolocations.

IANA time zone IDs are passed to the TimeZoneConverter library [from here](https://github.com/mattjohnsonpint/TimeZoneConverter)

# Info:
Calculations use the NOAA algorithm by default, with option to use USNO.

The NET port (4/28/21 download) is part of the solution, as an C# project called Zmanim, it compiles itself into a Dll into MyZman\Resources\.

The vb zman project then compiles the DLL into itself, all the is needed to run thr propgrem is a stand alone exe.

# Notable points:
Project uses Embedded fonts "ArialUnicodeCompact" & "VarelaRound-Regular", ArialUnicodeCompact is Arial Unicode MS with only Eng & Heb characters.

MessageBox are centered to mouse Location with Centered_MessageBox class

Round Button's are used for some of the controls

Status Strip Messages are Trimmed and ellipsis are added 

User settings are Serialized and saved in the exe folder

Variables are mostly Globalized 

# Disclaimer:
Please double check before relying on these zmanim for halacha lemaaseh.

Get Current Location relies on windows GeoCoordinateWatcher, the results should be checked.

# Screenshots.
![1](https://user-images.githubusercontent.com/83419922/129582704-c70581a7-2ead-467a-a055-553da29555fe.jpg)
![2](https://user-images.githubusercontent.com/83419922/129582744-d270cc55-60b1-4867-a61c-532982cedd1a.jpg)

