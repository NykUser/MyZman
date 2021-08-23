# MyZman
A compact windows program for calculating Jewish Zmanim.

And converting between Hebrew & Gregorian Dates [including Parsha, Holiday’s, Daf Yomi].

The program also includes options for exporting and setting reminders.

[This link](https://downgit.github.io/#/home?url=https://github.com/NykUser/MyZman/tree/master/MyZmanPortable) can be used to download the portable .exe.

The Code is released under the LGPL 2.1 license.

# Instructions
### Dates
* Gregorian dates are selected via the date picker.
* Hebrew dates are typed into the Hebrew Date textbox.
### Locations
* Included with the program the is a list of locations (in a file named LocationsList.xml, this file needs to be in the same folder as MyZman.exe).
Please be aware that this list was gathered from online sources, and its accuracy has not been checked.
Most of the locations are missing elevation info.
* When a location is selected the zmanim automatically updates according to the location’s values.
* The Time shown on the program’s title bar is the current system time converted to the time zone selected in the time zone dropdown box.
* The user can also manually enter an Latitude & Longitude with time zone info, and the zmanim will automatically update to the changed location.
* Changes can be saved, locations added or removed, the default startup locations set, via the three-dot menu (kebab menu).
* All Changes are saved to the user settings file (MyZman.exe.UserSettings.xml, in the same folder as the .exe), and not to the default locations list.
* In addition, the is an option to retrieve the user’s current location, this relies on windows geocoordinatewatcher, accuracy depends on several factors, and hence the results should be checked.
### Zmanim
* The program has a preset list of zmanim that can be changed to the users preference.
* Pressing delete will remove the selected zman.
* Double clicking on the zman’s name will allow editing its name
* Double clicking on the zman’s time will open a dropdown list with all available zmanim. Explanation of the varies zmanim can be [found here](https://kosherjava.com/zmanim-project/).
* The order of the zmanim can be rearranged simply by dragging and dropping.
* The are additional options available via an right click context menu, when clicking somewhere in the zmanim box.
* When a Zman is selected it will show up on the status bar, with the amount of time left\past selected zman (according to the time-zone of the selected location).
* When the option Color for Zmanim is selected, the zmanim that have already past will be shown in red and upcoming zmanim in green (shaah zmanis stay black).
* All changes are saved to the user settings file and can be reset via the right click context menu.
### Tools
#### The tools menu includes:
* Settings menu with additional settings, such as hebrew menus and messages, showing the parsha and holiday’s as in israel, time format, setting the programs opacity\transparency  when not active. And more.
* Tool for exporting the selected list of zmanim to Excel (as an CSV comma-separated values file that can be used by additional software) or .VCS used by several popular Calendar programs. 
* Tool for compering zmanim.
* Tool for setting reminders according to preset time or zman. Reminders run on windows task scheduler and will run even when MyZman is closed.
* User can set a message to display as well as a audio file to play when reminder is shown.
* Zmanim reminders are shown according to the location selected on the schedule window (not the one on the main program window).
 
 this [is a link](https://github.com/NykUser/MyZman/blob/master/MyZmanPortable/eng.mp4) to a clip with example uses of the program

# Calculations Info and Credits
* Core Zmanim calculations are based on Eliyahu Hershfeld’s work [Kosher Java](https://kosherjava.com/) available on its [GitHub Home](https://github.com/KosherJava/zmanim).
* Project uses the above’s NET port developed by Yitzchok Gottlieb [Found here](https://github.com/Yitzchok/Zmanim) (Release 1.5.0)
* The Net Port’s Function GetParsha has [an issue](https://github.com/Yitzchok/Zmanim/issues/28) Custom Function GetParshaNew used instead.
* By default, all zmanim calculations use the NOAA algorithm (developed by Jean Meeus).
* By default, all zmanim calculations and are based on sea level.
* The is an options to use the older USNO algorithm (more on the differences see below) or to use the locations elevation via the right click context menu in the zmanim box (as mentioned above the saved locations elevation is incomplete).
* Project uses the GeoTimeZone library [from here](https://github.com/mattjohnsonpint/GeoTimeZone) (Release 4.1.0) to look up time zones according to Geolocations.
* IANA time zone IDs are passed to the TimeZoneConverter library [from here](https://github.com/mattjohnsonpint/TimeZoneConverter) (Release 3.5.0)

# Disclaimer
#### Below is a note on Kosher Java’s website this applies to MyZman as well:
Due to atmospheric conditions (pressure, humidity and other conditions), calculating zmanim accurately is very complex. The calculation of zmanim is dependent on [Atmospheric refraction](https://en.wikipedia.org/wiki/Atmospheric_refraction) (refraction of sunlight through the atmosphere), and zmanim can be off by up to 2 minutes based on atmospheric conditions. Inaccuracy is increased by elevation. It is not the intent of this API to provide any guarantee of accuracy. See [Using a Digital Terrain Model to Calculate Visual Sunrise and Sunset Times](http://www.chaitables.com/webpub/DblHallpaperpub.pdf) for additional information on the subject.

**While I did my best to get accurate results, please double check before relying on these zmanim for halacha lemaaseh.**

# Screenshots
![1](https://user-images.githubusercontent.com/83419922/129582704-c70581a7-2ead-467a-a055-553da29555fe.jpg)
![2](https://user-images.githubusercontent.com/83419922/129582744-d270cc55-60b1-4867-a61c-532982cedd1a.jpg)

# Notable points about the Programs Code
* The NET port is part of the solution, as an C# project called Zmanim, it compiles itself into a Dll into MyZman\Resources\.
* The VB zman project compiles the DLL into itself, all that is needed to run the propgrem is a stand alone exe & an xml file with locations info.
* Project uses Embedded fonts "ArialUnicodeCompact" & "VarelaRound-Regular", ArialUnicodeCompact is Arial Unicode MS with only English & Hebrew characters.
* Message Boxes are centered to mouse position with Centered_MessageBox class
* Round Buttons are used for some of the controls
* Status Strip Messages are Trimmed and ellipsis are added 
* User settings are Serialized and saved in the exe folder
* Variables are mostly Globalized 

# More info on USNO vs NOAA from Kosher Java
The USNO algorithm was used by many poskim over the years. It does not differentiate between leap and non-leap years. It is now a drop out of date and is not considered as accurate.
Changes to earth’s rotation since the time they were published and other factors contribute to this. Since they do not differentiate between leap and non-leap years, this alone can mean that the data is a day off (something that corrects itself every 4 years).
The USNO algorithm was also designed to be accurate from about the year 1980 to 2050 with lower accuracy at the edges. It was never considered more accurate than one minute.
You can contrast that with the [NOAA algorithm](https://www.esrl.noaa.gov/gmd/grad/solcalc/calcdetails.html) that is based on newer algorithms by Jean Meeus that are considered more accurate.
That said, you will find very small differences between the two. See the old post[ Zmanim Bug Report from the Land of the Midnight Sun](https://kosherjava.com/2008/05/08/zmanim-bug-report-from-the-land-of-the-midnight-sun/) that references the two. You can see the [extract on a study comparing them](https://ui.adsabs.harvard.edu/abs/2018AAS...23115003P/abstract) to have an idea.
That said, none are really accurate due to atmospheric conditions. See The [Novaya Zemlya Effect’s Impact on Zmanim](https://kosherjava.com/2018/08/14/the-novaya-zemlya-effect-impact-on-zmanim/) article that covers this.

