<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="dir_playground_drag_and_drop_Default" %>

<html lang="en" dir="ltr" itemscope itemtype="http://schema.org/Article">
<head>
  <meta charset="utf-8">
  <meta property="twitter:account_id" content="1593210261" />
  <!-- Copyright (c) 2012 Google Inc.
   *
   * Licensed under the Apache License, Version 2.0 (the "License");
   * you may not use this file except in compliance with the License.
   * You may obtain a copy of the License at
   *
   *     http://www.apache.org/licenses/LICENSE-2.0
   *
   * Unless required by applicable law or agreed to in writing, software
   * distributed under the License is distributed on an "AS IS" BASIS,
   * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   * See the License for the specific language governing permissions and
   * limitations under the License.
   *
   * Author: Eric Bidelman - e.bidelman@google.com
   *
   * 
   *
  -->
  <title>Native HTML5 Drag and Drop - HTML5 Rocks</title>
  <meta name="description" content="Drag-and-drop is yet another first class citizen in HTML5! This article explains how to enhance your web applications by adding native DnD functionality.">
  <meta name="keywords" content="html5,html 5,html5 demos,html5 examples,javascript,css3,notifications,geolocation,web workers,apppcache,file api,filereader,indexeddb,offline,audio,video,drag and drop,chrome,sse,mobile">
  <meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=1.0">
  <meta name="theme-color" content="#f04530">
  <link rel="shortcut icon" href="/favicon.ico">
  <link rel="alternate" type="application/rss+xml" title="HTML5 Rocks RSS" href="http://feeds.feedburner.com/html5rocks">
  <meta name="google-site-verification" content="E1HOIDkksrWY5npenL8FeQhKn4Ujctd75iO2lfufSyA" />
  <meta itemprop="name" content="Native HTML5 Drag and Drop - HTML5 Rocks">
  <meta itemprop="description" content="Drag-and-drop is yet another first class citizen in HTML5! This article explains how to enhance your web applications by adding native DnD functionality.">
  
  <meta itemprop="image" content="http://www.html5rocks.com/static/images/html5rocks-logo-wings.png">
  

  
  <meta name="twitter:card" content="summary">
  <meta name="twitter:site" content="@ChromiumDev">
  <meta name="twitter:creator" content="@ebidel">

  <meta property="og:type" content="article">
  <meta property="og:title" content="Native HTML5 Drag and Drop - HTML5 Rocks">
  <meta property="og:url" content="http://www.html5rocks.com/en/tutorials/dnd/basics/">
  <meta property="og:description" content="Drag-and-drop is yet another first class citizen in HTML5! This article explains how to enhance your web applications by adding native DnD functionality.">
  <meta property="og:image" content="http://www.html5rocks.com/static/images/profiles/ericbidelman.png">
  <meta property="og:site_name" content="HTML5 Rocks - A resource for open web HTML5 developers">
  

  
  <link rel="author" href="https://www.google.com/profiles/118075919496626375791">
  
  
  <link rel="publisher" href="https://plus.google.com/+GoogleChromeDevelopers">

  
  
  <link rel="alternate" hreflang="pt" href="http://www.html5rocks.com/pt/tutorials/dnd/basics/">
  
  <link rel="alternate" hreflang="ru" href="http://www.html5rocks.com/ru/tutorials/dnd/basics/">
  
  <link rel="alternate" hreflang="zh" href="http://www.html5rocks.com/zh/tutorials/dnd/basics/">
  
  <link rel="alternate" hreflang="de" href="http://www.html5rocks.com/de/tutorials/dnd/basics/">
  
  <link rel="alternate" hreflang="es" href="http://www.html5rocks.com/es/tutorials/dnd/basics/">
  
  <link rel="alternate" hreflang="ja" href="http://www.html5rocks.com/ja/tutorials/dnd/basics/">
  
  

  
    
    <link rel="stylesheet" media="all" href="/static/css/v2-combined.min.css?20131111">
    

    
  

  <link href="//fonts.googleapis.com/css?family=Open+Sans:300,400,400italic,600,800|Source+Code+Pro" rel="stylesheet">

  <link rel="apple-touch-icon" href="/static/images/identity/HTML5_Badge_64.png">
  <link rel="apple-touch-icon-precomposed" href="/static/images/identity/HTML5_Badge_64.png">

  <script src="/static/js/modernizr.custom.82437.js"></script>

  <!--[if lt IE 9]>
  <script src="http://html5shim.googlecode.com/svn/trunk/html5-els.js"></script>
  <![endif]-->

  
<style>
figure img { border: 1px solid #ccc; }
h1,h2,h3,h4 { clear: both; }
/* Prevent the contents of draggable elements from being selectable. */
[draggable] {
  -moz-user-select: none;
  -khtml-user-select: none;
  -webkit-user-select: none;
  user-select: none;
  /* Required to make elements draggable in old WebKit */
  -khtml-user-drag: element;
  -webkit-user-drag: element;
}
dd {
  padding: 5px 0;
}
.column {
  height: 150px;
  width: 150px;
  float: left;
  border: 2px solid #666666;
  background-color: #ccc;
  margin-right: 5px;
  -webkit-border-radius: 10px;
  -moz-border-radius: 10px;
  -o-border-radius: 10px;
  -ms-border-radius: 10px;
  border-radius: 10px;
  -webkit-box-shadow: inset 0 0 3px #000;
  -moz-box-shadow: inset 0 0 3px #000;
  -ms-box-shadow: inset 0 0 3px #000;
  -o-box-shadow: inset 0 0 3px #000;
  box-shadow: inset 0 0 3px #000;
  text-align: center;
  cursor: move;
  margin-bottom: 30px;
}
.column header {
  color: #fff;
  text-shadow: #000 0 1px;
  box-shadow: 5px;
  padding: 5px;
  background: -moz-linear-gradient(left center, rgb(0,0,0), rgb(79,79,79), rgb(21,21,21));
  background: -webkit-gradient(linear, left top, right top,
                               color-stop(0, rgb(0,0,0)),
                               color-stop(0.50, rgb(79,79,79)),
                               color-stop(1, rgb(21,21,21)));
  background: -webkit-linear-gradient(left center, rgb(0,0,0), rgb(79,79,79), rgb(21,21,21));
  background: -ms-linear-gradient(left center, rgb(0,0,0), rgb(79,79,79), rgb(21,21,21));
  background: -o-linear-gradient(left center, rgb(0,0,0), rgb(79,79,79), rgb(21,21,21));
  border-bottom: 1px solid #ddd;
  -webkit-border-top-left-radius: 10px;
  -moz-border-radius-topleft: 10px;
  -ms-border-radius-topleft: 10px;
  -o-border-radius-topleft: 10px;
  border-top-left-radius: 10px;
  -webkit-border-top-right-radius: 10px;
  -moz-border-radius-topright: 10px;
  -ms-border-radius-topright: 10px;
  -o-border-radius-topright: 10px;
  border-top-right-radius: 10px;
}
#columns-full .column {
  -webkit-transition: -webkit-transform 0.2s ease-out;
  -moz-transition: -moz-transform 0.2s ease-out;
  -o-transition: -o-transform 0.2s ease-out;
  -ms-transition: -ms-transform 0.2s ease-out;
}
#columns-full .column.over,
#columns-dragOver .column.over,
#columns-dragEnd .column.over,
#columns-almostFinal .column.over {
  border: 2px dashed #000;
}
#columns-full .column.moving {
  opacity: 0.25;
  -webkit-transform: scale(0.8);
  -moz-transform: scale(0.8);
  -ms-transform: scale(0.8);
  -o-transform: scale(0.8);
}
#columns-full .column .count {
  padding-top: 15px;
  font-weight: bold;
  text-shadow: #fff 0 1px;
}
</style>

</head>
<body data-href="tutorials-dnd-basics" onload="" class="article tutorial">

  <header class="main" id="siteheader">
    <h1 id="title">
      <a href="/en/" title="HTML5 Rocks">HTML5 Rocks</a>
    </h1>
    <a href="#sitenav" id="navtoggle">Show navigation</a>

    
    <a id="toctoggle" href="#toc">Table of Contents</a>
    

    <nav id="sitenav">
      <ul>
        
        <li id="home_menu"><a href="/en/" class="home">Home</a></li>
        
        <li id="tutorials_menu"><a href="/en/tutorials/?page=1" class="tutorials">Tutorials</a></li>
        <li id="updates_menu"><a href="http://updates.html5rocks.com/" class="updates">Updates</a></li>
      </ul>
    </nav>

    
    <nav class="toc" id="toc">
      <h1>Table of Contents</h1>

      <ul><li><a href='#toc-introduction'>Introduction</a></li><li><a href='#toc-creating-dnd-content'>Creating draggable content</a></li><li><a href='#toc-dragging-events'>Listening for Dragging Events</a></li><li><a href='#toc-dataTransfer'>The DataTransfer object</a></li><li><a href='#toc-dnd-files'>Dragging Files</a></li><li><a href='#toc-examples'>Examples</a></li><li><a href='#toc-conclusion'>Conclusion</a></li><li><a href='#toc-references'>References</a></li></ul>

      <h1 class="visible-title">Localizations:</h1>
      <ul>
        
          
          <li><a href="/pt/tutorials/dnd/basics/">Português (Brasil)</a></li>
          
          <li><a href="/ru/tutorials/dnd/basics/">Pусский</a></li>
          
          <li><a href="/zh/tutorials/dnd/basics/">中文 (简体)</a></li>
          
          <li><a href="/de/tutorials/dnd/basics/">Deutsch</a></li>
          
          <li><a href="/es/tutorials/dnd/basics/">Español</a></li>
          
          <li><a href="/ja/tutorials/dnd/basics/">日本語</a></li>
          
          <li><a href="https://github.com/html5rocks/www.html5rocks.com/blob/master/CONTRIBUTING.md">Contribute another</a></li>
        
      </ul>
    </nav>
    
  </header>

  <div class="body-content">
    

  <section class="title">

    

    <section class="title-text container">
      
      <h1>Native HTML5 Drag and Drop</h1>
      
      

      <a href="/en/" class="watermark">HTML5 Rocks</a>
    </section>
  </section>

  <article class="content-wrapper">

    <section class="container">

      

      <div class="article-meta" id="article-meta">
        <nav class="toc">
          <h1>Table of Contents</h1>

          <ul><li><a href='#toc-introduction'>Introduction</a></li><li><a href='#toc-creating-dnd-content'>Creating draggable content</a></li><li><a href='#toc-dragging-events'>Listening for Dragging Events</a></li><li><a href='#toc-dataTransfer'>The DataTransfer object</a></li><li><a href='#toc-dnd-files'>Dragging Files</a></li><li><a href='#toc-examples'>Examples</a></li><li><a href='#toc-conclusion'>Conclusion</a></li><li><a href='#toc-references'>References</a></li></ul>
        </nav>

        <aside class="localizations">
          <h1>Localizations</h1>
          <ul>
            
              
              <li><a href="/pt/tutorials/dnd/basics/">Português (Brasil)</a></li>
              
              <li><a href="/ru/tutorials/dnd/basics/">Pусский</a></li>
              
              <li><a href="/zh/tutorials/dnd/basics/">中文 (简体)</a></li>
              
              <li><a href="/de/tutorials/dnd/basics/">Deutsch</a></li>
              
              <li><a href="/es/tutorials/dnd/basics/">Español</a></li>
              
              <li><a href="/ja/tutorials/dnd/basics/">日本語</a></li>
              
              <li><a href="https://github.com/html5rocks/www.html5rocks.com/blob/master/CONTRIBUTING.md">Contribute another</a></li>
            
          </ul>
        </aside>
      </div>
      

      <div class="content" id="article-content">

        <section class="byline">

          <div class="byline-content">
            
            <section class="author-images">
              <a href="/profiles/#ericbidelman">
                <img src="/static/images/profiles/ericbidelman.png" itemprop="photo" alt="Eric Bidelman" title="Eric Bidelman">
              </a>

              
            </section>

            <section class="meta">
              <div class="authors">
                <strong>By</strong> <a href="/profiles/#ericbidelman">Eric Bidelman</a>
                
              </div>

              

              <div class="date">
                <time pubdate><strong>Published:</strong> September 30th, 2010</time>
                
                <span><strong>Comments:</strong> <a href="#disqus_thread" class="load-comments" data-disqus-identifier="http://www.html5rocks.com/tutorials/dnd/basics/">0</a></span>
              </div>

              <div id="notcompatible" class="hidden">
                Your browser may not support the functionality in this article.
              </div>
            </section>
            <div class="clear"></div>

            
          </div>
        </section>

        

  <h2 id="toc-introduction">Introduction</h2>

  <p>For years, we've been using libraries like JQuery and Dojo to simplify
  complex UI elements like animations, rounded corners, and drag and drop.
  There's no doubt, eye-candy is important for making rich, immersive experiences on the web.
  But why should a library be required for common tasks that all developers are using?</p>

  <p><a href="http://www.whatwg.org/specs/web-apps/current-work/multipage/dnd.html#dnd">Drag and drop</a>
  (DnD) is a first class citizen in HTML5! The spec defines an event-based mechanism, JavaScript API,
  and additional markup for declaring that just about any type of element be <code>draggable</code> on a page.
  I don't think anyone can argue against native browser support for a particular feature.
  Native browser DnD means faster, more responsive web apps.</p>

  <h3 id="toc-feature-detection">Feature Detection</h3>

  <p>Many apps that utilize DnD would have a poor experience without it. For example, imagine
  a chess game in which the pieces don't move. Oops! Although browser support is fairly complete,
  determining if a browser implements DnD (or any HTML5 feature for that matter) is important
  for providing a solution that degrades nicely. When/where DnD isn't available, fire up that library fallback
  to maintain a working app.</p>

  <p>If you need to rely on an API, always use feature detection rather than sniffing the browser's User-Agent.
  One of the better libraries for feature detection is <a href="http://www.modernizr.com/">Modernizr</a>.
  Modernizr sets a boolean property for each feature it tests. Thus, checking for DnD is a one-liner:</p>
  <pre class="prettyprint">
if (Modernizr.draganddrop) {
  // Browser supports HTML5 DnD.
} else {
  // Fallback to a library solution.
}
</pre>

  <h2 id="toc-creating-dnd-content">Creating draggable content</h2>

  <p>Making an object draggable is simple. Set the <code>draggable=true</code> attribute
  on the element you want to make moveable. Just about anything can be drag-enabled,
  including images, links, files, or other DOM nodes.</p>

  <p>As an example, let's start creating rearrangeable columns. The basic markup may look something like this:</p>

  <pre class="prettyprint">
&lt;div id="columns"&gt;
  &lt;div class="column" draggable="true"&gt;&lt;header&gt;A&lt;/header&gt;&lt;/div&gt;
  &lt;div class="column" draggable="true"&gt;&lt;header&gt;B&lt;/header&gt;&lt;/div&gt;
  &lt;div class="column" draggable="true"&gt;&lt;header&gt;C&lt;/header&gt;&lt;/div&gt;
&lt;/div&gt;
</pre>

  <p>It's worth noting that in most browsers, text selections, img elements, and anchor
  elements with an <code>href</code> attribute are draggable by default. For example, dragging
  the logo on google.com produces a ghost image:</p>

  <figure class="center">
    <img src="/static/images/screenshots/dnd/img_drag.png" width="500" height="269" title="Dragging an image in the browser" alt="Dragging an image in the browser" />
    <figcaption>Most browsers support dragging an image by default.</figcaption>
  </figure>

  <p>which can be dropped in the address bar, a <code>&lt;input type="file" /&gt;</code> element, or even
  the desktop. If you want to enable other types of content to be draggable, you'll need to leverage the HTML5
  DnD APIs.</p>

  <p>Using a little CSS3 magic we can spruce up our markup to look like columns.
  Adding <code>cursor: move</code> gives users a visual indicator that something is moveable:</p>

<pre class="prettyprint">
&lt;style&gt;
/* Prevent the text contents of draggable elements from being selectable. */
[draggable] {
  -moz-user-select: none;
  -khtml-user-select: none;
  -webkit-user-select: none;
  user-select: none;
  /* Required to make elements draggable in old WebKit */
  -khtml-user-drag: element;
  -webkit-user-drag: element;
}
.column {
  height: 150px;
  width: 150px;
  float: left;
  border: 2px solid #666666;
  background-color: #ccc;
  margin-right: 5px;
  -webkit-border-radius: 10px;
  -ms-border-radius: 10px;
  -moz-border-radius: 10px;
  border-radius: 10px;
  -webkit-box-shadow: inset 0 0 3px #000;
  -ms-box-shadow: inset 0 0 3px #000;
  box-shadow: inset 0 0 3px #000;
  text-align: center;
  cursor: move;
}
.column header {
  color: #fff;
  text-shadow: #000 0 1px;
  box-shadow: 5px;
  padding: 5px;
  background: -moz-linear-gradient(left center, rgb(0,0,0), rgb(79,79,79), rgb(21,21,21));
  background: -webkit-gradient(linear, left top, right top,
                               color-stop(0, rgb(0,0,0)),
                               color-stop(0.50, rgb(79,79,79)),
                               color-stop(1, rgb(21,21,21)));
  background: -webkit-linear-gradient(left center, rgb(0,0,0), rgb(79,79,79), rgb(21,21,21));
  background: -ms-linear-gradient(left center, rgb(0,0,0), rgb(79,79,79), rgb(21,21,21));
  border-bottom: 1px solid #ddd;
  -webkit-border-top-left-radius: 10px;
  -moz-border-radius-topleft: 10px;
  -ms-border-radius-topleft: 10px;
  border-top-left-radius: 10px;
  -webkit-border-top-right-radius: 10px;
  -ms-border-top-right-radius: 10px;
  -moz-border-radius-topright: 10px;
  border-top-right-radius: 10px;
}
&lt;/style&gt;
</pre>

  <p>Result (draggable but won't do anything):</p>
  <div id="columns">
    <div class="column" draggable="true"><header>A</header></div>
    <div class="column" draggable="true"><header>B</header></div>
    <div class="column" draggable="true"><header>C</header></div>
  </div>

  <p style="padding-top:10px;clear:both">In the example above, most browsers will
  create a ghost image of the content being dragged. Others (FF in particular) will require
  that some <a href="#toc-dataTransfer">data be sent</a> in the drag operation.
  In the next section, we'll start to make our column
  example more interesting by adding listeners to process the drag/drop event model.</p>

  <h2 id="toc-dragging-events">Listening for Dragging Events</h2>

  <p>There are a number of different events to attach to for monitoring the entire drag and drop process:</p>
  <ul>
    <li><code>dragstart</code></li>
    <li><code>drag</code></li>
    <li><code>dragenter</code></li>
    <li><code>dragleave</code></li>
    <li><code>dragover</code> </li>
    <li><code>drop</code></li>
    <li><code>dragend</code></li>
  </ul>

  <p>To handle the DnD flow, we need the notion of a source element (where the drag originates), the data payload
  (what we're trying to drop), and a target (an area to catch the drop). The source element can be an image, list,
  link, file object, block of HTML...you name it. The target is the drop zone (or set of drop zones) that accepts
  the data the user is trying to drop. Keep in mind that not all elements can be targets (e.g. images).</p>

  <h3 id="toc-dragstart">1. Starting a Drag</h3>

  <p>Once you have <code>draggable="true"</code> attributes defined on your content, attach
  <code>dragstart</code> event handlers to kick off the DnD sequence for each column.</p>

  <p>This code will set the column's opacity to 40% when the user begins dragging it:</p>

<pre class="prettyprint">
function handleDragStart(e) {
  this.style.opacity = '0.4';  // this / e.target is the source node.
}

var cols = document.querySelectorAll('#columns .column');
[].forEach.call(cols, function(col) {
  col.addEventListener('dragstart', handleDragStart, false);
});
</pre>

  <p>Result:</p>
  <div id="columns-dragStart">
    <div class="column"><header>A</header></div>
    <div class="column"><header>B</header></div>
    <div class="column"><header>C</header></div>
  </div>

  <p style="clear:both">Since the <code>dragstart</code> event's target is our source element,
  setting <code>this.style.opacity</code> to 40% gives the user visual feedback that the element
  is the current selection being moved. One thing that we still need to do is return the columns opacity
  to 100% once the drag is done. An obvious place to handle that is the <code>dragend</code> event.
  More on this later.</p>

  <h3 id="toc-dragover-dragleave">2. dragenter, dragover, and dragleave</h3>

  <p><code>dragenter</code>, <code>dragover</code>, and <code>dragleave</code> event handlers can be used to
  provide additional visual cues during the drag process. For example, when a column is hovered over during a drag,
  its border could become dashed. This will let users know the columns are also drop targets.</p>

<pre class="prettyprint">
&lt;style&gt;
.column.over {
  border: 2px dashed #000;
}
&lt;/style&gt;
</pre>

<pre class="prettyprint">
function handleDragStart(e) {
  this.style.opacity = '0.4';  // this / e.target is the source node.
}

<span class="new">function handleDragOver(e) {
  if (e.preventDefault) {
    e.preventDefault(); // Necessary. Allows us to drop.
  }

  e.dataTransfer.dropEffect = 'move';  // See the section on the DataTransfer object.

  return false;
}</span>

<span class="new">function handleDragEnter(e) {
  // this / e.target is the current hover target.
  this.classList.add('over');
}</span>

<span class="new">function handleDragLeave(e) {
  this.classList.remove('over');  // this / e.target is previous target element.
}</span>

var cols = document.querySelectorAll('#columns .column');
[].forEach.call(cols, function(col) {
  col.addEventListener('dragstart', handleDragStart, false);
  <span class="new">col.addEventListener('dragenter', handleDragEnter, false);
  col.addEventListener('dragover', handleDragOver, false);
  col.addEventListener('dragleave', handleDragLeave, false);</span>
});
</pre>

  <p>There are a couple of points worth covering in this code:</p>

  <ul>
    <li>The <code>this</code>/<code>e.target</code> changes for each
    type of event, depending on where we are in the DnD event model.</li>
    <li>In the case of dragging something like a link, we need to prevent the browser's
    default behavior, which is to navigate to that link. To do this, call <code>e.preventDefault()</code>
    in the <code>dragover</code> event. Another good practice is to <code>return false</code> in that same handler.
    Browsers are somewhat inconsistent about needing these, but they don't hurt to add.</li>
    <li><code>dragenter</code> is used to toggle the 'over' class instead
    of the <code>dragover</code>. If we were to use <code>dragover</code>,
    our CSS class would be toggled many times as the event <code>dragover</code> continued
    to fire on a column hover. Ultimately, that would cause the browser's renderer to do a
    large amount of unnecessary work. Keeping redraws to a minimum is always a good idea.</li>
  </ul>

  <h3 id="toc-dragend">3. Completing a Drag</h3>

  <p>To process the actual drop, add an event listener for the <code>drop</code>
  and <code>dragend</code> events. In this handler, you'll need to prevent
  the browser's default behavior for drops, which is typically some sort of annoying redirect.
  You can prevent the event from bubbling up the DOM by calling <code>e.stopPropagation()</code>.</p>

  <p style="padding-top:10px;clear:both">Our column example won't do much without the
  <code>drop</code> event in place, but before we do that, an immediate improvement
  is to use <code>dragend</code> to remove the 'over' class from each column:</p>

<pre class="prettyprint">
...

<span class="new">function handleDrop(e) {
  // this / e.target is current target element.

  if (e.stopPropagation) {
    e.stopPropagation(); // stops the browser from redirecting.
  }

  // See the section on the DataTransfer object.

  return false;
}</span>

<span class="new">function handleDragEnd(e) {
  // this/e.target is the source node.

  [].forEach.call(cols, function (col) {
    col.classList.remove('over');
  });
}</span>

var cols = document.querySelectorAll('#columns .column');
[].forEach.call(cols, function(col) {
  col.addEventListener('dragstart', handleDragStart, false);
  col.addEventListener('dragenter', handleDragEnter, false)
  col.addEventListener('dragover', handleDragOver, false);
  col.addEventListener('dragleave', handleDragLeave, false);
  <span class="new">col.addEventListener('drop', handleDrop, false);
  col.addEventListener('dragend', handleDragEnd, false);</span>
});
</pre>

  <p>Result:</p>
  <div id="columns-dragEnd">
    <div class="column"><header>A</header></div>
    <div class="column"><header>B</header></div>
    <div class="column"><header>C</header></div>
  </div>

  <p style="padding-top:10px;clear:both">If you've been following closely up until now,
  you may notice that our example still doesn't drop the column as expected.
  Enter the <code>DataTransfer</code> object.</p>

  <h2 id="toc-dataTransfer">The DataTransfer object</h2>

  <p>The <code>dataTransfer</code> property is where all the DnD magic happens.
  It holds the piece of data sent in a drag action. <code>dataTransfer</code> is
  set in the <code>dragstart</code> event and read/handled in the <code>drop</code> event.
  Calling <code>e.dataTransfer.setData(format, data)</code>
  will set the object's content to the mimetype and data payload passed as arguments.</p>

  <p>In our example, the data payload is set to the actual HTML of the source column:</p>

<pre class="prettyprint">
var dragSrcEl = null;

function handleDragStart(e) {
  // Target (this) element is the source node.
  this.style.opacity = '0.4';

  dragSrcEl = this;

  <span class="new">e.dataTransfer.effectAllowed = 'move';
  e.dataTransfer.setData('text/html', this.innerHTML);</span>
}
</pre>

  <p>Conveniently, <code>dataTransfer</code> also has a <code>getData(format)</code>
  for fetching the drag data by mimetype. Here is the modification to process the column drop:</p>

<pre class="prettyprint">
function handleDrop(e) {
  // this/e.target is current target element.

  if (e.stopPropagation) {
    e.stopPropagation(); // Stops some browsers from redirecting.
  }

  <span class="new">// Don't do anything if dropping the same column we're dragging.
  if (dragSrcEl != this) {
    // Set the source column's HTML to the HTML of the column we dropped on.
    dragSrcEl.innerHTML = this.innerHTML;
    this.innerHTML = e.dataTransfer.getData('text/html');
  }</span>

  return false;
}
</pre>

  <p>I've added a global var named <code>dragSrcEl</code> as a convenience to facilitate the column swap.
  In <code>handleDragStart()</code>, the <code>innerHTML</code> of the source column is stored in that variable and later
  read in <code>handleDrop()</code> to swap the source column and target column's HTML.</p>

  <p>Result:</p>
  <div id="columns-almostFinal">
    <div class="column"><header>A</header></div>
    <div class="column"><header>B</header></div>
    <div class="column"><header>C</header></div>
  </div>

  <h3 id="toc-drag-properties">Dragging properties</h3>

  <p>The <code>dataTransfer</code> object exposes properties to provide
  visual feedback to the user during the drag process. These properties can also
  be used to control how each drop target responds to a particular data type.</p>

  <dl>
    <dt><code>dataTransfer.effectAllowed</code></dt>
    <dd>Restricts what 'type of drag' the user can perform on the element. It is used in the
    drag-and-drop processing model to initialize the <code>dropEffect</code> during the
    <code>dragenter</code> and <code>dragover</code> events. The property can be set to the
    following values: <code>none</code>, <code>copy</code>, <code>copyLink</code>, <code>copyMove</code>,
    <code>link</code>, <code>linkMove</code>, <code>move</code>, <code>all</code>, and <code>uninitialized</code>.
    </dd>
    <dt><code>dataTransfer.dropEffect</code></dt>
    <dd>Controls the feedback that the user is given during the <code>dragenter</code> and
    <code>dragover</code> events. When the user hovers over a target element, the browser's cursor will
    indicate what type of operation is going to take place (e.g. a copy, a move, etc.). The effect can
    take on one of the following values: <code>none</code>, <code>copy</code>, <code>link</code>, <code>move</code>.
    </dd>
    <dt><code>e.dataTransfer.setDragImage(imgElement, x, y)</code></dt>
    <dd>Instead of using the browser's default 'ghost image' feedback, you can optionally set a drag
    icon
<pre class="prettyprint">
var dragIcon = document.createElement('img');
dragIcon.src = 'logo.png';
dragIcon.width = 100;
e.dataTransfer.setDragImage(dragIcon, -10, -10);</pre>
    <p>Result (you should see the Google logo when dragging these columns):</p>
    <div id="columns-dragIcon">
      <div class="column"><header>A</header></div>
      <div class="column"><header>B</header></div>
      <div class="column"><header>C</header></div>
    </div>
    </dd>
  </dl>

  <h2 id="toc-dnd-files">Dragging Files</h2>

  <p>With the DnD APIs, it is possible to drag files from the desktop to your web app
  in the browser window. As an extension to this idea, Google Chrome supports the ability to drag file
  objects out from the browser to the desktop.</p>

  <h3 id="toc-desktop-dnd-into">Drag-in: dragging from the desktop to the browser</h3>

  <p>Dragging a file from the desktop is achieved by using the DnD events as other types of content.
  The main difference is in your <code>drop</code> handler. Instead of using
  <code>dataTransfer.getData()</code> to access the files, their data will be contained
  in the <code>dataTransfer.files</code> property:</p>

<pre class="prettyprint">
function handleDrop(e) {
  e.stopPropagation(); // Stops some browsers from redirecting.
  e.preventDefault();

  var files = e.dataTransfer.files;
  for (var i = 0, f; f = files[i]; i++) {
    // Read the File objects in this FileList.
  }
}
</pre>

  <p>For a complete guide to dragging files from desktop to the browser, see
  <a href="/tutorials/file/dndfiles/#toc-selecting-files-dnd">Using drag and drop for selecting</a>
  in <a href="/tutorials/file/dndfiles/">Reading local files in JavaScript</a>.</p>

  <h3 id="toc-desktop-dnd-out">Drag-out: dragging from the browser to the desktop</h3>

  <p>For a complete guide to dragging files from the browser to the desktop, see
  <a href="http://www.thecssninja.com/javascript/gmail-dragout">Drag out files like Gmail</a> from the CSS Ninja.</p>

  <h2 id="toc-examples">Examples</h2>

  <p>Here is the final product with a little more polish and a counter for each move:</p>
  <div id="columns-full">
    <div class="column"><header>A</header><div class="count" data-col-moves="0"></div></div>
    <div class="column"><header>B</header><div class="count" data-col-moves="0"></div></div>
    <div class="column"><header>C</header><div class="count" data-col-moves="0"></div></div>
    <div class="column"><header>D</header><div class="count" data-col-moves="0"></div></div>
  </div>

  <p style="padding-top:10px;clear:both">The interesting thing about the column sample is that
  the columns are both a drag source and a drop target. A more common scenario is for the source
  and target elements to be different. See <a href="http://html5demos.com/drag">html5demos.com/drag</a> for a demo.</p>

<!--
  other examples:
  <li>Rearrange list</li>
  <li>Creating an image gallery</li>
  <li>Exporting a canvas image</li>
-->

  <h2 id="toc-conclusion">Conclusion</h2>

  <p>No one will argue that HTML5's DnD model is complicated compared to other solutions like JQuery UI.
  However, any time you can take advantage of the browser's native APIs, do so!
  After all, that's the whole point of HTML5...which is to standardize and make available
  a rich set of APIs that are native to the browser. Hopefully popular libraries that
  implement DnD functionality will eventually include native HTML5 support by default,
  and fallback to a custom JS solution as needed.</p>

  <h2 id="toc-references">References</h2>
  <ul>
    <li><a href="http://www.whatwg.org/specs/web-apps/current-work/multipage/dnd.html#dnd">Drag and Drop</a> specification</li>
    <li><a href="https://developer.mozilla.org/En/DragDrop/Drag_Operations">Drag Operations</a> from MDC</li>
    <li><a href="http://html5doctor.com/native-drag-and-drop/">Native Drag and Drop</a> article from html5doctor</li>
    <li><a href="http://www.thecssninja.com/javascript/gmail-dragout">Drag out files like Gmail</a> from the CSS Ninja</li>
  </ul>

<script>
// Using this polyfill for safety.
Element.prototype.hasClassName = function(name) {
  return new RegExp("(?:^|\\s+)" + name + "(?:\\s+|$)").test(this.className);
};

Element.prototype.addClassName = function(name) {
  if (!this.hasClassName(name)) {
    this.className = this.className ? [this.className, name].join(' ') : name;
  }
};

Element.prototype.removeClassName = function(name) {
  if (this.hasClassName(name)) {
    var c = this.className;
    this.className = c.replace(new RegExp("(?:^|\\s+)" + name + "(?:\\s+|$)", "g"), "");
  }
};


var samples = samples || {};

// dragStart
(function() {
  var id_ = 'columns-dragStart';
  var cols_ = document.querySelectorAll('#' + id_ + ' .column');

  this.handleDragStart = function(e) {
    e.dataTransfer.effectAllowed = 'move';
    e.dataTransfer.setData('text/html', 'blah'); // needed for FF.

    // Target element (this) is the source node.
    this.style.opacity = '0.4';
  };

  [].forEach.call(cols_, function (col) {
    // Enable columns to be draggable.
    col.setAttribute('draggable', 'true');
    col.addEventListener('dragstart', this.handleDragStart, false);
  });

})();

// dragEnd
(function() {
  var id_ = 'columns-dragEnd';
  var cols_ = document.querySelectorAll('#' + id_ + ' .column');

  this.handleDragStart = function(e) {
    e.dataTransfer.effectAllowed = 'move';
    e.dataTransfer.setData('text/html', this.innerHTML); // needed for FF.

    // Target element (this) is the source node.
    this.style.opacity = '0.4';
  };

  this.handleDragOver = function(e) {
    if (e.preventDefault) {
      e.preventDefault(); // Allows us to drop.
    }

    e.dataTransfer.dropEffect = 'move';

    return false;
  };

  this.handleDragEnter = function(e) {
    this.addClassName('over');
  };

  this.handleDragLeave = function(e) {
    // this/e.target is previous target element.
    this.removeClassName('over');
  };

  this.handleDragEnd = function(e) {
    [].forEach.call(cols_, function (col) {
      col.removeClassName('over');
    });

    // target element (this) is the source node.
    this.style.opacity = '1';
  };

  [].forEach.call(cols_, function (col) {
    // Enable columns to be draggable.
    col.setAttribute('draggable', 'true');
    col.addEventListener('dragstart', this.handleDragStart, false);
    col.addEventListener('dragenter', this.handleDragEnter, false);
    col.addEventListener('dragover', this.handleDragOver, false);
    col.addEventListener('dragleave', this.handleDragLeave, false);
    col.addEventListener('dragend', this.handleDragEnd, false);
  });

})();

// dragIcon
(function() {
  var id_ = 'columns-dragIcon';
  var cols_ = document.querySelectorAll('#' + id_ + ' .column');

  this.handleDragStart = function(e) {
    e.dataTransfer.effectAllowed = 'move';
    e.dataTransfer.setData('text/html', this.innerHTML);

    var dragIcon = document.createElement('img');
    dragIcon.src = '/static/images/google_logo_small.png';
    e.dataTransfer.setDragImage(dragIcon, -10, -10);

    // Target element (this) is the source node.
    this.style.opacity = '0.4';
  };

  this.handleDragLeave = function(e) {
    // this/e.target is previous target element.

    this.removeClassName('over');
  };

  this.handleDragEnd = function(e) {
    // this/e.target is the source node.

    this.style.opacity = '1';

    [].forEach.call(cols_, function (col) {
      col.removeClassName('over');
    });
  };

  [].forEach.call(cols_, function (col) {
    // Enable columns to be draggable.
    col.setAttribute('draggable', 'true');
    col.addEventListener('dragstart', this.handleDragStart, false);
    col.addEventListener('dragend', this.handleDragEnd, false);
    col.addEventListener('dragleave', this.handleDragLeave, false);
  });

})();

// Almost final example
(function() {
  var id_ = 'columns-almostFinal';
  var cols_ = document.querySelectorAll('#' + id_ + ' .column');
  var dragSrcEl_ = null;

  this.handleDragStart = function(e) {
    e.dataTransfer.effectAllowed = 'move';
    e.dataTransfer.setData('text/html', this.innerHTML);

    dragSrcEl_ = this;

    this.style.opacity = '0.4';

    // this/e.target is the source node.
    this.addClassName('moving');
  };

  this.handleDragOver = function(e) {
    if (e.preventDefault) {
      e.preventDefault(); // Allows us to drop.
    }

    e.dataTransfer.dropEffect = 'move';

    return false;
  };

  this.handleDragEnter = function(e) {
    this.addClassName('over');
  };

  this.handleDragLeave = function(e) {
    // this/e.target is previous target element.

    this.removeClassName('over');
  };

  this.handleDrop = function(e) {
    // this/e.target is current target element.

    if (e.stopPropagation) {
      e.stopPropagation(); // stops the browser from redirecting.
    }

    // Don't do anything if we're dropping on the same column we're dragging.
    if (dragSrcEl_ != this) {
      dragSrcEl_.innerHTML = this.innerHTML;
      this.innerHTML = e.dataTransfer.getData('text/html');
    }

    return false;
  };

  this.handleDragEnd = function(e) {
    // this/e.target is the source node.
    this.style.opacity = '1';

    [].forEach.call(cols_, function (col) {
      col.removeClassName('over');
      col.removeClassName('moving');
    });
  };

  [].forEach.call(cols_, function (col) {
    col.setAttribute('draggable', 'true');  // Enable columns to be draggable.
    col.addEventListener('dragstart', this.handleDragStart, false);
    col.addEventListener('dragenter', this.handleDragEnter, false);
    col.addEventListener('dragover', this.handleDragOver, false);
    col.addEventListener('dragleave', this.handleDragLeave, false);
    col.addEventListener('drop', this.handleDrop, false);
    col.addEventListener('dragend', this.handleDragEnd, false);
  });
})();

// Full example
(function() {
  var id_ = 'columns-full';
  var cols_ = document.querySelectorAll('#' + id_ + ' .column');
  var dragSrcEl_ = null;

  this.handleDragStart = function(e) {
    e.dataTransfer.effectAllowed = 'move';
    e.dataTransfer.setData('text/html', this.innerHTML);

    dragSrcEl_ = this;

    // this/e.target is the source node.
    this.addClassName('moving');
  };

  this.handleDragOver = function(e) {
    if (e.preventDefault) {
      e.preventDefault(); // Allows us to drop.
    }

    e.dataTransfer.dropEffect = 'move';

    return false;
  };

  this.handleDragEnter = function(e) {
    this.addClassName('over');
  };

  this.handleDragLeave = function(e) {
    // this/e.target is previous target element.
    this.removeClassName('over');
  };

  this.handleDrop = function(e) {
    // this/e.target is current target element.

    if (e.stopPropagation) {
      e.stopPropagation(); // stops the browser from redirecting.
    }

    // Don't do anything if we're dropping on the same column we're dragging.
    if (dragSrcEl_ != this) {
      dragSrcEl_.innerHTML = this.innerHTML;
      this.innerHTML = e.dataTransfer.getData('text/html');

      // Set number of times the column has been moved.
      var count = this.querySelector('.count');
      var newCount = parseInt(count.getAttribute('data-col-moves')) + 1;
      count.setAttribute('data-col-moves', newCount);
      count.textContent = 'moves: ' + newCount;
    }

    return false;
  };

  this.handleDragEnd = function(e) {
    // this/e.target is the source node.
    [].forEach.call(cols_, function (col) {
      col.removeClassName('over');
      col.removeClassName('moving');
    });
  };

  [].forEach.call(cols_, function (col) {
    col.setAttribute('draggable', 'true');  // Enable columns to be draggable.
    col.addEventListener('dragstart', this.handleDragStart, false);
    col.addEventListener('dragenter', this.handleDragEnter, false);
    col.addEventListener('dragover', this.handleDragOver, false);
    col.addEventListener('dragleave', this.handleDragLeave, false);
    col.addEventListener('drop', this.handleDrop, false);
    col.addEventListener('dragend', this.handleDragEnd, false);
  });
})();
</script>


      </div>
    </section>
  </article>

  
  <section class="disqus pattern-bg-lighter">

    <div id="disqus" class="container">

      <h2>Comments</h2>

      <div id="disqus_thread">

        <a href="#disqus_thread" class="load-comments" data-disqus-identifier="http://www.html5rocks.com/tutorials/dnd/basics/">0</a>

      </div>
    </div>

    <noscript>
      <p class="center">
        <strong>
          <a href="http://disqus.com/?ref_noscript">Please enable JavaScript to view the comments powered by Disqus.</a>
        </strong>
      </p>
    </noscript>

    <script>

      var disqus_shortname = 'html5rocks';
      var disqus_identifier = 'http://www.html5rocks.com/tutorials/dnd/basics/';
      var disqus_url = 'http://www.html5rocks.com/tutorials/dnd/basics/';
      var disqus_developer = 0;

      var disqus_config = function () {
        var funky_language_code_mapping = {
          'de': 'de_inf',
          'es': 'es_ES',
          'pt': 'pt_EU',
          'sr': 'sr_CYRL',
          'sv': 'sv_SE',
          'zh': 'zh_HANT'
        };
        this.language = funky_language_code_mapping['en'] ||
                        'en';

        this.callbacks.onReady = [ function () {
                                      try {
                                        ga('send', 'event', 'View comments');
                                      } catch(err){}
                                   } ];
        this.callbacks.onNewComment = [ function (comment) {
                                          try {
                                            ga('send', 'event', 'Commented');
                                          } catch(err){}
                                        } ];
      };

      window.addEventListener('load', function(e) {

        var c = document.createElement('script');
        c.type = 'text/javascript';
        c.src = 'http://' + disqus_shortname + '.disqus.com/count.js';
        c.async = true;

        var s = document.getElementsByTagName('script')[0], sp = s.parentNode;
        sp.insertBefore(c, s);

        if (window.location.hash === '#disqus_thread')
          loadComments();

      }, false);

      var disqus_loaded = false;
      function loadComments() {

        if (disqus_loaded)
          return;

        disqus_loaded = true;

        ga('send', 'event', 'Interactions', 'Comments', 'Comments Loaded');

        var s = document.getElementsByTagName('script')[0], sp = s.parentNode;
        var dsq = document.createElement('script');
        dsq.type = 'text/javascript';
        dsq.async = true;

        var disqusContainer = document.getElementById('disqus');
        disqusContainer.classList.add('active');

        dsq.src = 'http://' + disqus_shortname + '.disqus.com/embed.js';
        sp.insertBefore(dsq, s);
      }

      function outgoing(url) {
        try {
          ga('send', 'event', 'Outbound Links' , url);
        } catch(err){}
      }
      // Open external links (also that don't have a target defined) in a new tab.
      var externLinks = document.querySelectorAll('article.tutorial a[href^="http"]:not([target])');
      for(var i = 0, a; a = externLinks[i]; ++i) {
        a.target = '_blank';
        a.addEventListener('click', new Function('outgoing(' + '"' + a.href.replace(/.*?:\/\//g, "") + '"' + ');'));
      }

      var loadCommentsButtons = document.querySelectorAll('.load-comments');
      for(var l = 0; l < loadCommentsButtons.length; l++)
        loadCommentsButtons[l].addEventListener('click', loadComments);

    </script>
  </section>
  

  <footer>
    <div class="container">

      
        <h1>Next steps</h2>

        

        <aside class="panel share">
          <h2>Share</h2>

            <a href="https://twitter.com/share?url=http://www.html5rocks.com/tutorials/dnd/basics/&text=Native HTML5 Drag and Drop&lang=en&via=ChromiumDev&related=ChromiumDev" class="twitter" target="_blank">Twitter</a>

            <a href="https://www.facebook.com/sharer/sharer.php?u=http://www.html5rocks.com/tutorials/dnd/basics/" class="facebook" target="_blank">Facebook</a>

            <a href="https://plus.google.com/share?url=http://www.html5rocks.com/tutorials/dnd/basics/" class="gplus" onclick="javascript:window.open(this.href, '', 'menubar=no,toolbar=no,resizable=yes,scrollbars=yes,height=600,width=600');return false;">Google+</a>

        </aside>

        <aside class="panel rss">
          <h2>Subscribe</h2>
          <p>Enjoyed this article? Grab the <a href="http://feeds.feedburner.com/html5rocks">RSS feed</a> and stay up-to-date.</p>
        </aside>

      

      <p class="licensing">
      
        Except as otherwise <a href="http://code.google.com/policies.html#restrictions">noted</a>, the content of this page is licensed under the <a href="http://creativecommons.org/licenses/by/3.0/">Creative Commons Attribution 3.0 License</a>, and code samples are licensed under the <a href="http://www.apache.org/licenses/LICENSE-2.0">Apache 2.0 License</a>.
      
      </p>

    </div>
  </footer>

  <script>
    window.isCompatible = function() {
      
  return Modernizr.draganddrop;

    };

    if (isCompatible() === false) {
      document.getElementById('notcompatible').className = '';
    }

    function _prettyPrint() {
      if (typeof customPrettyPrintLanguage != 'undefined') {
        customPrettyPrintLanguage();
      }
      prettyPrint();
    }
  </script>
  <script async src="/static/js/prettify.min.js" onload="_prettyPrint()"></script>
  <!-- Google Tag Manager -->
<noscript><iframe src="//www.googletagmanager.com/ns.html?id=GTM-XXXX"
height="0" width="0" style="display:none;visibility:hidden"></iframe></noscript>
<script>(function(w,d,s,l,i){w[l]=w[l]||[];w[l].push({'gtm.start':
new Date().getTime(),event:'gtm.js'});var f=d.getElementsByTagName(s)[0],
j=d.createElement(s),dl=l!='dataLayer'?'&l='+l:'';j.async=true;j.src=
'//www.googletagmanager.com/gtm.js?id='+i+dl;f.parentNode.insertBefore(j,f);
})(window,document,'script','dataLayer','GTM-MB3LRF');</script>
<!-- End Google Tag Manager -->


  </div>

  <script>
  (function() {

    // Kill feedburner and marketing tracking arguments, but let them register
    // before we do it.
    setTimeout(function() {
      if (/^\?utm_/.test(document.location.search) &&
          window.history.replaceState) {
        window.history.replaceState(
            {}, '', document.location.href.replace(/\?utm_.*/, ''));
      }
    }, 2000);

    var siteHeader = document.getElementById('siteheader');
    var navToggle = document.getElementById('navtoggle');
    var siteNav = document.getElementById('sitenav');

    function toggle(target, forceActive) {

      if (typeof toc !== 'undefined') {
        // Switch off whichever one is not the
        // current target
        if (target === toc)
          siteNav.classList.remove('active');
        else
          toc.classList.remove('active');
      }

      // Toggle if no force parameter is set
      if (typeof forceActive === 'undefined') {
        target.classList.toggle('active');
      } else {
        if (forceActive)
          target.classList.add('active');
        else
          target.classList.remove('active');
      }

      // now find out what the set state ended up being
      var isActive = target.classList.contains('active');

      if (isActive)
        siteHeader.classList.add('expanded');
      else
        siteHeader.classList.remove('expanded');

    }

    navToggle.addEventListener('click', function(e) {
      toggle(siteNav);
      e.preventDefault();
    });

    

    var tocToggle = document.getElementById('toctoggle');
    var toc = document.getElementById('toc');
    var articleMeta = document.getElementById('article-meta');
    var articleContent = document.getElementById('article-content');
    var articleMetaHeight = 0;
    var articleMetaMaxY = 0;
    var articleMetaMinY = 0;
    var articleContentPadding = 200;

    var tocLinks = document.querySelectorAll('.toc a');
    for (var t = 0; t < tocLinks.length; t++)
      tocLinks[t].addEventListener('click', onTocLinkClick);

    tocToggle.addEventListener('click', function(e) {
      toggle(toc);
      e.preventDefault();
    });

    toc.addEventListener('click', function(e) {
      if (e.target !== siteNav)
        toggle(toc, false);
    });

    function onTocLinkClick() {
      ga('send', 'event', 'Interactions', 'TOC', 'TOC Clicked');
    }

    function setMinScrollYFromMetaY() {
      var scrollPosition = window.scrollY;

      var articleMetaBounds = articleMeta.getBoundingClientRect();
      var articleMetaTop = Math.max(352,
          articleMetaBounds.top - 20 + scrollPosition);

      articleMetaHeight = articleMetaBounds.bottom - articleMetaBounds.top;
      articleMetaMinY = articleMetaTop;
    }

    function setMaxScrollYFromContentHeight() {

      var scrollPosition = window.scrollY;

      var articleContentBounds = articleContent.getBoundingClientRect();
      var articleContentTop = articleContentBounds.top + scrollPosition;
      var articleContentHeight = articleContentBounds.bottom - articleContentBounds.top;

      articleMetaMaxY = articleContentTop +
          articleContentHeight -
          articleMetaHeight -
          articleContentPadding;

    }

    function onScroll(e) {

      if (window.scrollY >= articleMetaMinY) {

        articleMeta.classList.add('sticky');

        var articleMetaTop = 22 - Math.max(0, window.scrollY - articleMetaMaxY);
        articleMeta.style.top = articleMetaTop + 'px';

      } else {
        articleMeta.classList.remove('sticky');
        articleMeta.style.top = 'auto';
      }
    }

    if (articleMeta.getBoundingClientRect) {
      setMinScrollYFromMetaY();
      setMaxScrollYFromContentHeight();
      document.addEventListener('scroll', onScroll);
      window.addEventListener('load', setMaxScrollYFromContentHeight, false);
    }

    
  })();
  </script>
  <script>
  (function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){
  (i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),
  m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)
  })(window,document,'script','//www.google-analytics.com/analytics.js','ga');

  ga('create', 'UA-15028909-1', 'auto');
  ga('create', 'UA-49880327-4', 'auto', {'name': 'html5rocks'});

  ga('send', 'pageview');
  ga('html5rocks.send', 'pageview');

  </script>
   <!-- Google Tag Manager -->
<noscript><iframe src="//www.googletagmanager.com/ns.html?id=GTM-XXXX"
height="0" width="0" style="display:none;visibility:hidden"></iframe></noscript>
<script>(function(w,d,s,l,i){w[l]=w[l]||[];w[l].push({'gtm.start':
new Date().getTime(),event:'gtm.js'});var f=d.getElementsByTagName(s)[0],
j=d.createElement(s),dl=l!='dataLayer'?'&l='+l:'';j.async=true;j.src=
'//www.googletagmanager.com/gtm.js?id='+i+dl;f.parentNode.insertBefore(j,f);
})(window,document,'script','dataLayer','GTM-MB3LRF');</script>
<!-- End Google Tag Manager -->
</body>
</html>