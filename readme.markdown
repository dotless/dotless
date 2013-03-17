What's a "Source map"?
======================

Some of you might have heard about the new cool upcomming feature of source maps that is gaining some momentum. But what are sourcemaps anyway? 
Sourcemaps (more correctly to say [https://docs.google.com/document/d/1U1RGAehQwRypUTovF1KRlpiOFze0b-_2gc6fAH0KY0k/edit?pli=1](Source Maps V3)) are a way to bind your source to the delivered to the browser.
The basic conecept is to provide an additional file (.map) with your generated js or css so that your brower can use to find the point of origin of a chunk of code.
If you want to readup about the topic here are some links that might interesst you [http://www.html5rocks.com/en/tutorials/developertools/sourcemaps/?redirect_from_locale=de](html 5 rocks)

At the point of writing the support for sorcemaps is kind of limited. As far as I know there are no implementations of sourcemaps targeting any css frameworks (Sass has it's own way to deal with sources). There only implementations for js-libs and compressors.
Up to this point I found two implementations of the V3-Sourcmap standard. The one by [https://github.com/mozilla/source-map](Mozilla) and the one used in [https://code.google.com/p/chromium/codesearch#chromium/src/third_party/WebKit/Source/WebCore/inspector/front-end/SourceMap.js&q=sourcemap&sq=package:chromium&l=32](Chromium). Both of them are (who'd guessed that) written is js.

Goal of this fork is to implement the sourcemap support in c#.

Note: Currently the only browser supporting sourcemaps is Chrome Canary with sourcemap enabled in the Inspector-Settings AND the developer-experiments enabled.


So why to use it with dotless?
------------------------------

One of the disadvantages of less is that it's hard to trace bugs or unintented bahaviour back to the responsible rule. The browser it self doesn't have a clue about your "source"-code. This problem even gets worse when you compress your resulting css. A sourcemap can ease this problem to some exten, by providing a link back to rule that was responsible for generating the line in question.


Sounds cool how to use?
-----------------------

Right now, the sourcemap support is a very early stage. I am still in the process of "easing" the implementation for the normal every day usage.
So please read up bevor jumping into it.

What does the fork do right now?
--------------------------------

Up to this point I've implemented some parts necessary, but not all of them.

- [http://en.wikipedia.org/wiki/Variable-length_quantity]VLQ-Integers needed for Sourcemaps
- V3-Sourcemap data format wrapped in json
- A easy to use object to aggregate information to be used in the sourcemap
- The sourcemap generation from the added fragments
- I've modified dotless to uses the sourcemap implementation
- I've created the necessary file-type-http-handlers to deliver the map and source files to the browser

Known issues and things to do
-----------------------------

- The http-handlers for the filetypes are at this point client side and need to be moved into the lib it self
- To generate the sourcemap from within dotless, I had to know some things like: The line and the column the source was generated to. Due to the recursive nature of the generation process I had a hard time to actually keep track about the code. The workaround I used wasn't very elegant, but works. I had to add markers (in form of css-comments) during the time of generation, which are tracked and remove by a post-processing routine. This might be a bottle-neck that speed things down.
- The lib is basicly untested!
- I've no idea how to handle mixins.
- The library currently only works with Chrome Canary, with sourcemaps enabled in the development settings AND the developer experiments swtiched on!

So far - 

cheers Corelgott
