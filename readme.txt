Requirement:
=============================
- Windows XP or later.
- .Net Framework 4.0
- log4net 1.2.11 (newkey)

For linux/Mac, you can try to compile the source code using:
- Mono (http://www.mono-project.com/Main_Page)
- SharpDevelop (http://www.icsharpcode.net/OpenSource/SD/Download/)

The latest source code can be pulled from https://github.com/Nandaka/DanbooruDownloader.
The compiled binary can be downloaded at http://nandaka.wordpress.com/tag/danbooru-batch-download/
The previous version of compiled binary can be downloaded at http://www.mediafire.com/?tglaujm3ylc88

Changelog:
=============================
- DanbooruDownloader201205xx
  - Add option to backup the tags.xml before updating.
  - Add option to merge the tags.xml after updating (slow!).
  - Clear the page number text box if provider selection is changed.
  - Add logging using log4net.
  - Fix hard limit and default limit bug, now always use the hard limit if given limit is over the hard limit.
  - Some UI updates.
  - Change Add Batch Download behaviour, each provider as separate job.

- DanbooruDownloader20120505
  - Fix SubString error in MakeFilename if there is no '/' in the filename format or save folder.
  - Add blacklist tag support.
    - The blacklisted post is still shown in the Main Tab, but with different background color, 
      the color can be configured in Settings Tab.
    - The blacklisted post is skipped in the Full Batch mode, see the Log tab for more details.
    - Separate each blacklisted tag with space.
  - Add option to replace empty filename format.
  - Add check if the file_url is empty.
  - Fix query url generator.
  - Add application icon.

- DanbooruDownloader20120427
  - Fix absolute path in download batch.
  - Add form for updating tags.xml.
  - Update json parser.
  - Fix unhandled 'root element missing' exception when downloading list.
  - Fix remove .!tmp when download success (20120428)

- DanbooruDownloader20120421
  - Properly decode JSON-encoded unicode string.
  - Add .!tmp for currently downloaded file.
  - Add %originalFilename% tag for filename.
  - Update parser to process server response message.

- DanbooruDownloader20120413
  - Fix shimmie2 downloader as reported by Spidey.
  - Add tags auto complete in tags text box.
  - Add timeout retry in batch download.

- DanbooruDownloader20120405
  - Add preprocessor for Pixiv referer column (use the proper url if clicked).
  - Add new order by criteria.
  - Fix thumbnail not loaded if clicking get list when the current thumbnail loading is not completed.
  - Fix Gelbooru referer url.
  - Fix Shimmie2 paging for batch download.
  - Add tags.xml for tags type detection, you can get the latest from:
    - http://danbooru.donmai.us/tag/index.xml?limit=0 or 
    - https://yande.re/tag/index.xml?limit=0 or from other *booru.
    Just save the xml as tags.xml in the same folder as the application.
  - Add %artist%, %copyright%, %character%, %circle%, and %faults% tag for filename.
  - Add colored tags column in Main Tag (enable in Settings tab).
    - Depend on tags.xml file, you can download it from http://danbooru.donmai.us/tag/index.xml?limit=0.
  - Fix invalid auth info for batch download.

- DanbooruDownloader20120330
  - Update DanbooruProvider.xml
  - Add basic Shimmie2 RSS XML parsing.
  - Fix start page detection for batch download if mixed board type as reported by Erin.
  - Disable clear/remove job if batch job still running.
  - Add Danbooru Provider Editor.
  - Make Referer column clickable and auto resize Preview column.

- DanbooruDownloader20120316
  - Add Clear All Batch Job as requested by Xemnarth.
  - Fix Start Batch Job button state if abort on error is checked.
  - Add background color for batch job state (red == error, green == completed).
  - Retain last entered job information (cleared when closing the application).
  - Add context menu to delete job in batch job grid.

- DanbooruDownloader20120309
  - Fix batch download limit detection as reported by Q.
  - Uncheck Auto Load Next Page if get list is failed.
  - Fix xml parser for relative url as reported by m00kz.
  - Fix list loading.
  - Fix batch download as reported by saviorz.
  - Increase max filename length to 255 characters.
  - Add Clear Completed Batch Job button as requested by Xemnarth.
  - Add progress bar for batch job download (per provider).

- DanbooruDownloader20120229
  - Add abort on error option for batch download as requested by Xemnarth.
  - Fix batch download stop button.
  - Add Pause/Resume batch download button.
  - Change Target Framework to .NET Framework 4.
  - Update DanbooruProviderList.xml.

- DanbooruDownloader20120203
  - Change rating to expanded mode for compatibility with gelbooru.
  - Fix bug when add batch job but no provider is selected.
  - Update batch job message.
  - Fix batch job limit detection.

- DanbooruDownloader20120120
  - Add danbooru login information using the DanbooruProviderList.xml.
  - fix preview image parsing if using relative path.
  - fix Gelbooru auto forward page hang (20120122).
  - Update DanbooruProviderList.xml to add board type (20120122).

- DanbooruDownloader20120108
  - Add proxy username/password support. 
  - Add select all provider button in add batch job dialog.
  - Updating progress message for batch download.
  - Fix proxy login support (20120110).

DanbooruProvider.xml
=============================
This file will be parsed when you run the applications. You can modify the xml to add new provider.
The contents structure are:
- Name	: The name of the *booru.
- Url	: The root path of the *booru url, ie:http://danbooru.donmai.us
- QueryStringJson : For danbooru based, use:/post/index.json?%_query%
		    For gelbooru, not supported/no API provided.
- QueryStringXml  : For danbooru based, use:/post/index.xml?%_query%
             	    For gelbooru based, use:/index.php?page=dapi&amp;s=post&amp;q=index&amp;%_query%
- Preferred   : Xml/Json.
- DefaultLimit: Number of limit if not given.
- HardLimit   : Hard limit from server for each request.
- PasswordSalt: choujin-steiner--%PASSWORD%-- for danbooru.donmai.us.
- UserName    : Your username.
- Password    : Your password, in plain text.
- UseAuth     : true/false. Use authentication/login.


Filename Format
=============================
- %provider% 	= provider Name
- %id% 		= Image ID
- %tags% 	= Image Tags
- %rating% 	= Image Rating
- %md5% 	= MD5 Hash
- %artist% 	= Artist Tag
- %copyright% 	= Copyright Tag
- %character% 	= Character Tag
- %circle% 	= Circle Tag, yande.re extension
- %faults% 	= Faults Tag, yande.re extension
- %originalFilename% = Original Filename
- %searchtag% 	= Search tag

If the given tags is not available, it will replaced with empty string.

FAQ
=============================
Q1: I cannot download from Danbooru (403 Forbidden)!
A1: Please read http://danbooru.donmai.us/forum/show/72300.
    You need to supply login information in the DanbooruProvider.xml and set UseAuth to true.

Q2: I cannot download/can only download 1 image from 3DBooru!
A2: On Settings tab, check Pad User Agent.

Q3: I cannot use space in batch download, the program always converting it to underscores!
A3: Use '+' for space.

Q4: I got 503 error from Danbooru!
A4: See A1. If still have the error, see this: http://danbooru.donmai.us/forum/show/24011.

Q5: I got 'ERROR_MESSAGE_HERE'!
A5: Sent me a message in the comment with the details, such as:
    - The Provider you are using.
    - The query.
    - The settings (screen shot is fine, please upload it to http://imgur.com/)
    - The error message (screen shot also fine)

Q6: I got a lot of skipped files when do batch download!
A6: It is caused of the target filename is already exists. Add %md5% in your filename format.

Supported Board
=============================
Currently only supporting Danbooru-type, Gelbooru-type with Danbooru API,
and Shimmie2 with RSS enabled.

- danbooru (http://danbooru.donmai.us)				NSFW
- Sankaku Complex (http://chan.sankakucomplex.com)		NSFW
- Sankaku Complex (Idol) (http://idol.sankakucomplex.com)	NSFW
- Konachan (http://konachan.com)				NSFW
- oreno.imouto.org (http://oreno.imouto.org)			NSFW
- gelbooru.com (http://gelbooru.com)				NSFW
- nekobooru.net (http://nekobooru.net)				NSFW
- safebooru (http://safebooru.org)
- ichijou (ichijou.org)
- TheDoujin.com (http://thedoujin.com)				NSFW
- 3dbooru (http://behoimi.org)
- e621 (http://e621.net)					NSFW, Furry
- rule34 (http://rule34.xxx)					NSFW
- dollbooru
- 4chanhouse
- fairygarden

License Agreement
=============================
Copyright (c) 2012, Nandaka
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, 
are permitted provided that the following conditions are met:

  - Redistributions of source code must retain the above copyright notice, this 
    list of conditions and the following disclaimer.
  - Redistributions in binary form must reproduce the above copyright notice, 
    this list of conditions and the following disclaimer in the documentation 
    and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE 
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR 
ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES 
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS 
OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY 
THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING 
NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN 
IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
