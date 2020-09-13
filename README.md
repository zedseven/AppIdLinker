# AppIdLinker
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

A KeePass plugin for including Android app IDs in the description of entries based on their URLs. This increases the searchability of relevant password entries for Android apps like Keepass2Android.

## Better Explanation
Every Android app has a package name (app ID), unique to itself.

Certain KeePass mobile apps, such as [Keepass2Android](https://github.com/PhilippC/keepass2android), automatically search for that app ID when looking for the password entry to use when signing into an app.

What this plugin does is automatically and unobtrusively add those IDs to the Notes section of any entries it knows the relevant app ID for, which allows Keepass2Android to find it directly so you don't have to search for it yourself.

## Example
Here's a normal password entry before running the plugin on it:

![A screenshot of an entry before adding any IDs.](/Media/Before.png "Before adding any IDs.")

And here's after - note that this is an extreme number of IDs, because it's a Google account:

![A screenshot of an entry after adding a bunch of IDs.](/Media/After.png "After adding a bunch of IDs.")

And if you decide you don't like or need the IDs on an entry anymore, the plugin offers an easy way to remove them as well:

![A screenshot of the context (right-click) menu, showing two options provided by the plugin: Add and Remove IDs from selected entries.](/Media/ContextMenu.png "You can add or remove IDs from as many entries as you like whenever you want.")

## App IDs I Missed
I'm certain I've missed a bunch of common apps, but the database I compiled was made mostly from my own usage, so there was only so much I could do.

The database is actually [it's own GitHub repository](https://github.com/zedseven/android-application-id-url-db), and you can find more details about contributing [over there](https://github.com/zedseven/android-application-id-url-db/blob/master/CONTRIBUTING.md).
