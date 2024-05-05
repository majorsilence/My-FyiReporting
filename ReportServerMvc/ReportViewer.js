/*
 * Copyright 2007-2008 fyiReporting Software, LLC  (www.fyireporting.com)
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *         http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 *
*/

var _pages;
var _pageCurrent;
var _pageWidth;
var _pageHeight;
var _ScrollingCanvas;

/*
* Function parses comma separated name=value argument pairs
* from the query string of the URL.  It stores the name=value pairs
* in properties of an object and returns the object.
*/
function createRdlSilverlightPlugin(uniqueId)
{      
    Silverlight.createObject( 
        "rexaml.xaml",                  // Source property value.
        parentElement,                  // DOM reference to hosting DIV tag.
        uniqueId,                       // Unique plug-in ID value.
        {                               // Per-instance properties.
            width:'500',                // Width of rectangular region of 
                                        // plug-in area in pixels.
            height:'350',               // Height of rectangular region of 
                                        // plug-in area in pixels.
            inplaceInstallPrompt:false, // Determines whether to display 
                                        // in-place install prompt if 
                                        // invalid version detected.
            background:'#FFFFFF',       // Background color of plug-in.
            isWindowless:'false',       // Determines whether to display plug-in 
                                        // in Windowless mode.
            framerate:'24',             // MaxFrameRate property value.
            version:'1.0'               // Silverlight version to use.
        },
        {
            onError:null,               // OnError property value -- 
                                        // event handler function name.
            onLoad:onRvLoad             // OnLoad property value -- 
                                        // event handler function name.
        },
        null);                          // Context value -- event handler function name.
}

/*
* Function parses comma separated name=value argument pairs
* It stores the name=value pairs
* in properties of an object and returns the object.
*/
function GetArgs(parms)
{
    var args = new Object();
 //   var query = location.search.substring(1);   // get query string
    var pairs = parms.split("&");
    for(var i=0; i < pairs.length; i++)         // loop thru all 
    {
        var pos = pairs[i].indexOf('=');            // look for name=value
        if (pos == -1) continue;                    // no value, skip
        var argname = pairs[i].substring(0, pos);   // obtain argument name
        var value = pairs[i].substring(pos+1);      // obtain value
        if (args[argname])
            args[argname] += ("," + value);         // add value into list
        else
            args[argname] = value;                  // store as property
    }
    return args;    // return the object
}

function onRvLoad(control, context, rootElement)
{
    control.Content.OnFullScreenChange = OnFSChange;
}

function OnFSChange(rootElement)
{
    if (!_ScrollingCanvas)
        return;
    var control = document.getElementById("RSSilverlightPlugin");
    var height = control.Content.ActualHeight;
    var width = control.Content.ActualWidth;
    
    _ScrollingCanvas.resize(width, height-22);
}

function setReport(report) {
    var control = document.getElementById("RSSilverlightPlugin");

    var dler = control.CreateObject("downloader");
    dler.AddEventListener("DownloadProgressChanged", onDownloadProgress);
    dler.AddEventListener("Completed", onCompleted);
    _pages = null;
// clear out old report for progress bar
    var root = control.Content.Root;
    root.Children.Clear();

    var width = control.width;
    var pb_text = "<Canvas xmlns=\"http://schemas.microsoft.com/client/2007\">" + 
        "<Rectangle Fill=\"LightGray\" Width=\"" + width + "\" Height=\"22\"/>" +
        "<Rectangle Name=\"progressBar\" Fill=\"Lime\" Width=\"0\" Height=\"22\" />" +
        "<TextBlock Name=\"progressBarText\">0%</TextBlock>" +
        "</Canvas>";
    var pb = control.Content.CreateFromXaml(pb_text);
    root.Children.Add(pb); 

    dler.Open("GET", document.location.href + "?rs:url=" + report  + "&rs:zip=yes");
//    dler.Open("GET", "DrilldownXaml.zip"); 
    dler.Send();
}

function onDownloadProgress(sender, eventArgs)
{
    var pb = sender.FindName("progressBar");
    if (pb == null)         // if no progress bar then we're probably done (ie it was deleted)
        return;
    var pc = sender.DownloadProgress * 100;
    pb.Width = pc;
    sender.FindName("progressBarText").Text = Math.floor(pc) + "%";
}
 
function onCompleted(sender, e)
{
//    var xaml = sender.ResponseText;
    var control = sender.GetHost();
    
    // create a scrolling canvas to hold the report
//    var pg = control.Content.CreateFromXamlDownloader(sender, "pg_1.xaml");
    var meta = sender.getResponseText("meta.txt");
    var args = GetArgs(meta);
    var pc = args.pagecount;
    
    _pages = new Array();
    _pagewidth = args.pagewidth;
    _pageheight = args.pageheight;
//        alert("pagewidth="+_pagewidth + " pageheight="+_pageheight);   // debug
    var j;
    for (j=0; j < pc; j++)
    {
        var pgs = sender.getResponseText("pg_" + (j+1) + ".xaml");
        var pg = control.Content.CreateFromXaml(pgs); 
        bindImages(sender, pg);
        _pages[j] = pg;
    }
    
    var root = control.Content.Root;
    root.Children.Clear();
    
    _pageCurrent = 0;
    var shell = buildShell(control, _pages[0]);
   
    root.Children.Add(shell);
}

function bindImages(sender, parent)
{
    // Enumerate the children of the Canvas object.
    var i;
    for (i = 0; i < parent.children.count; i++)
    {
        var child = parent.children.getItem(i);

        var type = child.toString();
        if (type == "Image")
        {
            child.setSource(sender, child.Source);
        }
        else if (type == "Canvas")
            bindImages(sender, child);
    }
    return;
}

function buildShell(control, pg)
{
 //   var x = document.yyy.asdf;  // forces break

    var width = control.width;
    var tbHeight = 22;
    var height = control.height;

    var pgHeight = 1056;    // 11 * 96
    if (_pageheight > 0)
        pgHeight = _pageheight;
    var pgWidth = 816;      // 8.5 * 96=816
    if (_pagewidth > 0)
        pgWidth = _pagewidth;
// create a toolbar and canvas to hold the report
    var shell_text = "<Canvas xmlns=\"http://schemas.microsoft.com/client/2007\">" + 
        '  <Canvas Name="Toolbar">' +
        "   <Rectangle Fill=\"LightGray\" Width=\"" + width + "\" Height=\"" + tbHeight + "\"/>" +
        '   <Image Source="SLViewer/fullscrn.png" MouseLeftButtonDown="tbFullScreenClick" Cursor="Hand"/>' +
        '   <Image Name="ZoomIn" Source="SLViewer/zoomout.png" MouseLeftButtonDown="tbZoomClick" Canvas.Left="22" Cursor="Hand"/>' +
        '   <Image Name="ZoomOut" Source="SLViewer/zoomin.png" MouseLeftButtonDown="tbZoomClick" Canvas.Left="44" Cursor="Hand"/>' +
        '   <Image Name="PageBack" Source="SLViewer/pgback.png" MouseLeftButtonDown="tbPageBack" Canvas.Left="66" Cursor="Hand"/>' +
        '   <Image Name="PageNext" Source="SLViewer/pgnext.png" MouseLeftButtonDown="tbPageNext" Canvas.Left="88" Cursor="Hand"/>' +
        '  </Canvas>' +
        '  <Canvas Name="ReportBody" Canvas.Top="' + tbHeight + '" Width="' + pgWidth + '" Height="' + pgHeight + '">' +
        '    <Canvas.RenderTransform><ScaleTransform Name="reportTransform" ScaleX="1.0" ScaleY="1.0" /></Canvas.RenderTransform>' +
        '  </Canvas>' +
        "</Canvas>";

    var shell = control.Content.CreateFromXaml(shell_text);

    var tb = shell.FindName("Toolbar");
    var rb = shell.FindName("ReportBody");
    rb.Children.Add(pg);

    shell.Children.Clear();

    var sc = new ScrollingCanvas(rb);       // put report page in a scrolling canvas

    shell.Children.Add(tb);
    shell.Children.Add(sc.canvas);
    
    // Set the height    
    sc.resize(width, height - tbHeight);
    _ScrollingCanvas = sc;
    
    return shell;
}
function tbFullScreenClick(sender, mouseEventArgs)
{
    var content = sender.GetHost().Content;
    content.FullScreen = !content.FullScreen;
}
function tbPageBack(sender, mouseEventArgs)
{
    if (_pages == null)
        return;

    if (_pageCurrent == 0)
    {
 //       alert("You are already on the first page.");   // this takes you out of full page mode
        return;
    }        
    _pageCurrent--;
    
    var control = sender.GetHost();
    var root = control.Content.Root;
    
    SetPage(root, control);
}
function tbPageNext(sender, mouseEventArgs)
{
    if (_pages == null)
        return;

    if (_pageCurrent >= _pages.length - 1)
    {
//        alert("You are already on the last page.");   // this takes you out of full screen mode
        return;
    }        
    _pageCurrent++;
    
    var control = sender.GetHost();
    var root = control.Content.Root;
    
    SetPage(root, control);
}
function SetPage(root, control)
{
    // first find the existing scale
    var scaleTransform = root.findName("reportTransform");
    var z =scaleTransform.ScaleX;

    root.Children.Clear();
    var parent =_pages[_pageCurrent].GetParent();
    if (parent != null)
        parent.Children.Clear();
    var shell = buildShell(control, _pages[_pageCurrent]);
   
    root.Children.Add(shell);

    var scaleTransform = root.findName("reportTransform");
    scaleTransform.ScaleX = z;
    scaleTransform.ScaleY = z;

    // set the size
    var height = control.Content.ActualHeight;
    var width = control.Content.ActualWidth;
    
    _ScrollingCanvas.resize(width, height-22);
}
function tbZoomClick(sender, mouseEventArgs)
{
   var scaleTransform = sender.findName("reportTransform");

    var z =scaleTransform.ScaleX * 100;
    
    var inc = sender.Name == "ZoomOut"? 25: -25;       
    if (z + inc >= 800 || z + inc <= 25)
        return;
 
    z = (z + inc) / 100;
        
    scaleTransform.ScaleX = z;
    scaleTransform.ScaleY = z;

    // zooming affects the sizing (especially of scroll bars)
    var content = sender.GetHost().Content;
    var height = content.ActualHeight;
    var width = content.ActualWidth;
    
    _ScrollingCanvas.resize(width, height-22);
}

/*
ScrollingCanvas is adapted from Silverlight 1.0 Unleashed by Adam Nathan
*/
// Constructor for ScrollingCanvas
function ScrollingCanvas(content)
{
  // Used for scrollbar Width, upArrow/downArrow Width and Height, and the
  // delta for position and time when perfoming continuous scrolling:
  this.SMALLVALUE = 16;

  // Build up the XAML for the ScrollingCanvas, including the scrollbar
  var xaml = '<Canvas>';
  xaml += '  <Canvas.Clip><RectangleGeometry Name="clip"/></Canvas.Clip>';
  xaml += '</Canvas>';

  // Create the elements and add the passed-in content as a child of the root
  var hostContent = content.GetHost().Content;
  this.canvas = hostContent.CreateFromXaml(xaml);
  this.canvas.Children.Add(content);        // add in the content
  
  // now build the scrollbars - last so it shows on top
  // first the vertical scroll bar
  xaml = '  <Canvas Name="scrollBar" Width="' + this.SMALLVALUE
          + '" Background="WhiteSmoke">';
  xaml += '    <Canvas Name="upArrow" Width="' + this.SMALLVALUE
          + '" Height="' + this.SMALLVALUE + '" Background="LightGray">';
  xaml += '      <Line X1="8" X2="8" Y1="11" Y2="11.1" Stroke="Black"'
          + '      StrokeThickness="12" StrokeStartLineCap="Triangle"/>';
  xaml += '    </Canvas>';
  xaml += '    <Canvas Name="downArrow" Width="' + this.SMALLVALUE + '" Height="'
          + this.SMALLVALUE + '" Background="LightGray">';
  xaml += '      <Line X1="8" X2="8" Y1="6" Y2="6.1" Stroke="Black"'
          + '      StrokeThickness="12" StrokeEndLineCap="Triangle"/>';
  xaml += '    </Canvas>';
  xaml += '    <Canvas Name="thumb" Width="' + this.SMALLVALUE
          + '" Background="DarkGray">' 

          + '<Rectangle Name="vthumbRect" Canvas.Left="0" Canvas.Top="0" Stroke="Black" StrokeThickness="0">'
          + ' <Rectangle.Fill>'
		  +	'		<LinearGradientBrush EndPoint="1,0">'
		  +	'			<GradientStop Color="Gray" Offset="0.0" />'
		  +	'			<GradientStop Color="LightGray" Offset="0.5" />'
		  +	'			<GradientStop Color="Gray" Offset="1.0" />'
		  +	'		</LinearGradientBrush>'
		  +	'	</Rectangle.Fill></Rectangle>';  


  xaml += '      <Ellipse Name="thumbCircle" Width="12" Height="12"'
          + '      Canvas.Left="2" Fill="LightGray"/>';
  xaml += '    </Canvas>';
  xaml += '  </Canvas>';
  this.canvas.Children.Add(content.GetHost().Content.CreateFromXaml(xaml));

  // construct the horizontal scrollbar 
  xaml = '  <Canvas Name="hscrollBar" Canvas.Left="0" Height="' + this.SMALLVALUE
          + '" Background="WhiteSmoke">';
  xaml += '    <Canvas Name="rightArrow" Canvas.Left="0" Width="' + this.SMALLVALUE
          + '" Height="' + this.SMALLVALUE + '" Background="LightGray">';
  xaml += '      <Line X1="11" X2="11.1" Y1="8" Y2="8" Stroke="Black"'
          + '      StrokeThickness="12" StrokeStartLineCap="Triangle"/>';
  xaml += '    </Canvas>';
  xaml += '    <Canvas Name="leftArrow" Width="' + this.SMALLVALUE + '" Height="'
          + this.SMALLVALUE + '" Background="LightGray">';
  xaml += '      <Line X1="6" X2="6.1" Y1="8" Y2="8" Stroke="Black"'
          + '      StrokeThickness="12" StrokeEndLineCap="Triangle"/>';
  xaml += '    </Canvas>';
  xaml += '    <Canvas Name="hthumb" Height="' + this.SMALLVALUE + '" >'
          + '<Rectangle Name="hthumbRect" Canvas.Left="0" Canvas.Top="0" Stroke="Black" StrokeThickness="0">'
          + ' <Rectangle.Fill>'
		  +	'		<LinearGradientBrush EndPoint="0,1">'
		  +	'			<GradientStop Color="Gray" Offset="0.0" />'
		  +	'			<GradientStop Color="LightGray" Offset="0.5" />'
		  +	'			<GradientStop Color="Gray" Offset="1.0" />'
		  +	'		</LinearGradientBrush>'
		  +	'	</Rectangle.Fill></Rectangle>';  
   xaml += '      <Ellipse Name="hthumbCircle" Width="12" Height="12"'
          + '      Canvas.Top="2" Fill="LightGray"/>';
  xaml += '    </Canvas>';
  xaml += '  </Canvas>';
  
  this.canvas.Children.Add(content.GetHost().Content.CreateFromXaml(xaml));

  // construct the small box in the lower right rectangle  
  xaml = '    <Canvas Name="fillBox" Height="' + this.SMALLVALUE + '" Width="' + this.SMALLVALUE
          + '" Background="WhiteSmoke">';
  xaml += '    </Canvas>';
  this.canvas.Children.Add(content.GetHost().Content.CreateFromXaml(xaml));

    
  // Store the important elements in member variables
  this.clip = this.canvas.FindName("clip");
  this.scrollBar = this.canvas.FindName("scrollBar");
  this.upArrow = this.canvas.FindName("upArrow");
  this.downArrow = this.canvas.FindName("downArrow");
  this.thumb = this.canvas.FindName("thumb");
  this.vthumbRect = this.canvas.FindName("vthumbRect");
  this.thumbCircle = this.canvas.FindName("thumbCircle");
  this.content = content;

  // Horizontal Store the important elements in member variables
  this.hscrollBar = this.canvas.FindName("hscrollBar");
  this.leftArrow = this.canvas.FindName("leftArrow");
  this.rightArrow = this.canvas.FindName("rightArrow");
  this.hthumb = this.canvas.FindName("hthumb");
  this.hthumbRect = this.canvas.FindName("hthumbRect");
  this.hthumbCircle = this.canvas.FindName("hthumbCircle");
  this.fillBox = this.canvas.FindName("fillBox");

  // Move any Canvas.Left and Canvas.Top setting from the content
  this.canvas["Canvas.Left"] = content["Canvas.Left"];
  this.canvas["Canvas.Top"] = content["Canvas.Top"];
  content["Canvas.Left"] = 0;
  content["Canvas.Top"] = 0;

  // Attach event handlers to the thumb
  this.thumb.AddEventListener("MouseLeftButtonDown",
    delegate(this, this.onThumbMouseLeftButtonDown));
  this.thumb.AddEventListener("MouseMove", delegate(this, this.onThumbMouseMove));
  this.thumb.AddEventListener("MouseLeftButtonUp",
    delegate(this, this.onThumbMouseLeftButtonUp));

  // Attach event handlers to the up and down arrows
  this.upArrow.AddEventListener("MouseLeftButtonDown",
    delegate(this, this.onArrowMouseLeftButtonDown));
  this.upArrow.AddEventListener("MouseLeftButtonUp",
    delegate(this, this.onArrowMouseUpOrLeave));
  this.upArrow.AddEventListener("MouseLeave",
    delegate(this, this.onArrowMouseUpOrLeave));
  this.downArrow.AddEventListener("MouseLeftButtonDown",
    delegate(this, this.onArrowMouseLeftButtonDown));
  this.downArrow.AddEventListener("MouseLeftButtonUp",
    delegate(this, this.onArrowMouseUpOrLeave));
  this.downArrow.AddEventListener("MouseLeave",
    delegate(this, this.onArrowMouseUpOrLeave));

  // Attach event handlers to the thumb
  this.hthumb.AddEventListener("MouseLeftButtonDown",
    delegate(this, this.onHThumbMouseLeftButtonDown));
  this.hthumb.AddEventListener("MouseMove", delegate(this, this.onHThumbMouseMove));
  this.hthumb.AddEventListener("MouseLeftButtonUp",
    delegate(this, this.onHThumbMouseLeftButtonUp));

  // Attach event handlers to the left and right arrows
  this.leftArrow.AddEventListener("MouseLeftButtonDown",
    delegate(this, this.onHArrowMouseLeftButtonDown));
  this.leftArrow.AddEventListener("MouseLeftButtonUp",
    delegate(this, this.onHArrowMouseUpOrLeave));
  this.leftArrow.AddEventListener("MouseLeave",
    delegate(this, this.onHArrowMouseUpOrLeave));
  this.rightArrow.AddEventListener("MouseLeftButtonDown",
    delegate(this, this.onHArrowMouseLeftButtonDown));
  this.rightArrow.AddEventListener("MouseLeftButtonUp",
    delegate(this, this.onHArrowMouseUpOrLeave));
  this.rightArrow.AddEventListener("MouseLeave",
    delegate(this, this.onHArrowMouseUpOrLeave));

  // By default, set the root Canvas height to match
  // the content height (which means no scrolling)
  this.resize(hostContent.ActualWidth, hostContent.ActualHeight);
}

// Resize to the content's width and desired height
ScrollingCanvas.prototype.resize = function(width, height)
{
  // determine the zoom  
  var scaleTransform = this.content.findName("reportTransform");
  var zoom = scaleTransform.ScaleX;     // X and Y scales are always kept the same
    
  // Resize the canvas and its clipping rectangle 
  // (leaving room for the scrollbar)
  width = width - this.SMALLVALUE;
  height = height - this.SMALLVALUE; 

  this.canvas.Width = width + this.SMALLVALUE;
  this.canvas.Height = height + this.SMALLVALUE;
  this.clip.Rect = "0,0," + this.canvas.Width + "," + this.canvas.Height;
  
  // Show, position and resize the scrollbar
  this.scrollBar["Canvas.Left"] = width;
  this.scrollBar.Height = height;
  this.downArrow["Canvas.Top"] = height - this.SMALLVALUE;
  this.thumb.Height = Math.max(this.SMALLVALUE,
    (height - 2 * this.SMALLVALUE) * height / (this.content.Height * zoom) );
  this.vthumbRect.Width = this.thumb.Width;
  this.vthumbRect.Height = this.thumb.Height;
  this.thumbCircle["Canvas.Top"] = this.thumb.Height / 2
                                 - this.thumbCircle.Height / 2;
  this.maxThumbPosition = this.canvas.Height - this.SMALLVALUE - this.SMALLVALUE
                        - this.thumb.Height;

  // Calculate the ratio of content scrolling distance to thumb scrolling distance
  this.ratio = (this.content.Height * zoom - height) /
               (height - 2 * this.SMALLVALUE - this.thumb.Height);
  if (isNaN(this.ratio))
      this.ratio = 0;  
  // Don't show the scrollbar thumb if the content isn't taller than the canvas
  if (this.content.Height * zoom <= height)
  {
      this.ratio = 0;  
      this.thumb.Visibility = "Collapsed";
  }
  else  
      this.thumb.Visibility = "Visible";

  // Show, position and resize the horz scrollbar
  this.hscrollBar["Canvas.Top"] = height;
  this.hscrollBar.Width = width;
  this.leftArrow["Canvas.Left"] = width - this.SMALLVALUE;
  this.hthumb.Width = Math.max(this.SMALLVALUE,
    (width - 2 * this.SMALLVALUE) * width / (this.content.Width * zoom) );
  this.hthumbRect.Width = this.hthumb.Width;
  this.hthumbRect.Height = this.hthumb.Height; 
  this.hthumbCircle["Canvas.Left"] = this.hthumb.Width / 2
                                 - this.hthumbCircle.Width / 2;
  this.hmaxThumbPosition = this.canvas.Width - this.SMALLVALUE - this.SMALLVALUE
                        - this.hthumb.Width;

  // Calculate the ratio of content scrolling distance to thumb scrolling distance
  this.hratio = (this.content.Width * zoom - width) /
               (width - 2 * this.SMALLVALUE - this.hthumb.Width);
  if (isNaN(this.hratio))
      this.hratio = 0;  

  // Don't show the scrollbar thumb if the content isn't wideer than the canvas
  if (this.content.Width * zoom <= width)
  {
    this.hratio = 0;  
    this.hthumb.Visibility = "Collapsed";
  }
  else  
      this.hthumb.Visibility = "Visible";

    // position the fill box (bottom right corner)
    this.fillBox["Canvas.Top"] = this.canvas.Height - this.SMALLVALUE;
    this.fillBox["Canvas.Left"] = this.canvas.Width - this.SMALLVALUE;

  // Reset the scrollbar
  this.scrollTo(0);
  this.hscrollTo(0);
};

// Capture the mouse when pressing the thumb
ScrollingCanvas.prototype.onThumbMouseLeftButtonDown =
function(sender, mouseEventArgs)
{
  this.thumb.CaptureMouse();
  this.lastThumbPoint = mouseEventArgs.GetPosition(null);
  this.thumbDragging = true;
};

// Capture the mouse when pressing the thumb
ScrollingCanvas.prototype.onHThumbMouseLeftButtonDown =
function(sender, mouseEventArgs)
{
  this.hthumb.CaptureMouse();
  this.hlastThumbPoint = mouseEventArgs.GetPosition(null);
  this.hthumbDragging = true;
};

// If pressed, move the thumb along with the mouse
ScrollingCanvas.prototype.onThumbMouseMove = function(sender, mouseEventArgs)
{
  if (this.thumbDragging)
  {
    var point = mouseEventArgs.GetPosition(null);
    this.scrollTo(this.thumb["Canvas.Top"] + point.Y - this.lastThumbPoint.Y);
    this.lastThumbPoint = point;
  }
};

// If pressed, move the thumb along with the mouse
ScrollingCanvas.prototype.onHThumbMouseMove = function(sender, mouseEventArgs)
{
  if (this.hthumbDragging)
  {
    var point = mouseEventArgs.GetPosition(null);
    this.hscrollTo(this.hthumb["Canvas.Left"] + point.X - this.hlastThumbPoint.X);
    this.hlastThumbPoint = point;
  }
};

// Release mouse capture when releasing the thumb
ScrollingCanvas.prototype.onThumbMouseLeftButtonUp =
function(sender, mouseEventArgs)
{
  this.thumb.ReleaseMouseCapture();
  this.thumbDragging = false;
};

// Release mouse capture when releasing the thumb
ScrollingCanvas.prototype.onHThumbMouseLeftButtonUp =
function(sender, mouseEventArgs)
{
  this.hthumb.ReleaseMouseCapture();
  this.hthumbDragging = false;
};

// Move the content and thumb to the specified vertical position
ScrollingCanvas.prototype.scrollTo = function(thumbPosition)
{
  // Constrain the position to the bounds of the scrollbar
  thumbPosition = Math.max(thumbPosition, this.SMALLVALUE);
  thumbPosition = Math.min(thumbPosition, this.maxThumbPosition);

  if (this.thumb["Canvas.Top"] == thumbPosition)
  {
    // We're already at the desired position.
    // Just in case this is from a continuous scroll:
    this.stopContinuousScrolling();
  }
  else
  {
    // Move the thumb to the desired position
    this.thumb["Canvas.Top"] = thumbPosition;

    // Move the content to the corresponding position
    this.content["Canvas.Top"] = (this.SMALLVALUE - thumbPosition) * this.ratio;
  }
};

// Move the content and thumb to the specified vertical position
ScrollingCanvas.prototype.hscrollTo = function(thumbPosition)
{
  // Constrain the position to the bounds of the scrollbar
  thumbPosition = Math.max(thumbPosition, this.SMALLVALUE);
  thumbPosition = Math.min(thumbPosition, this.hmaxThumbPosition);

  if (this.hthumb["Canvas.Left"] == thumbPosition)
  {
    // We're already at the desired position.
    // Just in case this is from a continuous scroll:
    this.hstopContinuousScrolling();
  }
  else
  {
    // Move the thumb to the desired position
    this.hthumb["Canvas.Left"] = thumbPosition;

    // Move the content to the corresponding position
    this.content["Canvas.Left"] = (this.SMALLVALUE - thumbPosition) * this.hratio;
  }
};


// Scroll continuously when pressing the up or down arrow
ScrollingCanvas.prototype.onArrowMouseLeftButtonDown =
function(sender, mouseEventArgs)
{
  this.startContinuousScrolling(sender.Name == "upArrow");
};

// Scroll continuously when pressing the up or down arrow
ScrollingCanvas.prototype.onHArrowMouseLeftButtonDown =
function(sender, mouseEventArgs)
{
  this.hstartContinuousScrolling(sender.Name == "rightArrow");
};

// Stop scrolling continuously when releasing the up or down arrow
ScrollingCanvas.prototype.onArrowMouseUpOrLeave = function(sender, mouseEventArgs)
{
  this.stopContinuousScrolling();
};

// Stop scrolling continuously when releasing the up or down arrow
ScrollingCanvas.prototype.onHArrowMouseUpOrLeave = function(sender, mouseEventArgs)
{
  this.hstopContinuousScrolling();
};


// Begin continuous scrolling
ScrollingCanvas.prototype.startContinuousScrolling = function(up)
{
  var delta = this.SMALLVALUE;
  if (up)
    delta *= -1;

  // Call scroll every couple of milliseconds, adding the delta
  var scrollTo = delegate(this, this.scrollTo);
  var thumb = this.thumb;
  var callback = function() { scrollTo(thumb["Canvas.Top"] + delta); }
  this.handle = setInterval(callback, this.SMALLVALUE);
};
// Begin continuous scrolling
ScrollingCanvas.prototype.hstartContinuousScrolling = function(up)
{
  var delta = this.SMALLVALUE;
  if (up)
    delta *= -1;

  // Call scroll every couple of milliseconds, adding the delta
  var hscrollTo = delegate(this, this.hscrollTo);
  var thumb = this.hthumb;
  var callback = function() { hscrollTo(thumb["Canvas.Left"] + delta); }
  this.hhandle = setInterval(callback, this.SMALLVALUE);
};

// End the continuous scrolling, if it is happening
ScrollingCanvas.prototype.stopContinuousScrolling = function()
{
  clearInterval(this.handle);
};

// End the continuous scrolling, if it is happening
ScrollingCanvas.prototype.hstopContinuousScrolling = function()
{
  clearInterval(this.hhandle);
};

// Helper for attaching events to instance functions
function delegate(target, callback) {
  return function() { callback.apply(target, arguments); };
}


