# [SmartResource] (https://visualstudiogallery.msdn.microsoft.com/54030b1d-da06-495d-af96-e153403a2594) - Smart Resource VS Extension


##What is this?
 This is a small helper extension that helps you manage resources a lot quicker and easier. 

 It was written for SharePoint, but can be generally used, if you modify the replace patters appropriately. 


##The way it works:
 1. You select some text that you want to move to the resource file
 2. Hit ALT+R, ALT+R (or whatever shortcut you bind to the "Smart Resource Insert" command in Tools
 3. Type the name of a new resource key or choose an existing
 4. Done. Your text is replaced and the resx file updated :-)

 The .resx file is located by looking for 1) one named the same as your assembly, 2) then one named "Resources.resx" and 3) finally just a .resx file. It will fail if there is more than one suitable candidate.


Read more ([here](http://soerennielsen.wordpress.com/2014/02/20/announcing-smart-resource-vs2013-extension/)) 
 

##Remarks
I made this extension to mostly cater for SharePoint specific resources. It's equally applicable for other development scenarios, but the method call in the default patterns (change to your needs!) is:
```
        /// <summary>
        /// Fetch resource string from a resource file in the \Resources folder given by the named key
        ///
        /// 28/6-2012 SLN
        /// </summary>
        /// <param name="key">The key of the string to lookup, e.g. $Resources:Delegate.Sample,Feature_sample_Title.
        /// If "$Resources" is not found it will just return the key</param>
        /// <returns>Lookup resource value.</returns>
        public static string SPGetLocalizedString(string key)
        {
            if (!key.StartsWith("$Resources:"))
            {
                return key;
            }
            var split = key.TrimEnd(';').Remove(0, "$Resources:".Length).Split(',');

            if (split.Length != 2 || string.IsNullOrEmpty(split[1]))
            {
                return key;
            }

            return SPUtility.GetLocalizedString(key, split[0], (uint)System.Globalization.CultureInfo.CurrentUICulture.LCID);
        }
```
You'll need to add it somewhere in your code. Note that it is dependent on the SPUtility class from SharePoint, so if you're not in SharePoint mode, you'll need to modify it or use another replace pattern (see in options).


##Contributing
You are welcome to contribute and I'll gladly accept pull requests.

Do NOT publish this as your own extension at the visual studio gallery - instead create pull requests so that we can maintain a single source for the published extension.


##Change log

1.0.0.1
- Bug fixes. Quotes were manhandled in C#/VB files, they are treated better now 
- Feature: Another replace pattern option, that allows you to use std. .Net resource patterns if you do not want to roll your own method. Note: You have to update your settings in the Options pane if you modified the defaults in the previous version. 

1.0
- Initial public release. Fully featured. 



