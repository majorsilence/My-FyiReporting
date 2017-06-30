The 4.1.0 maintenance build includes code for the following features and bug fixes: 

[list][b]RdlMapFile[/b] Added reduce Polygon Points menu item under Edit.  Uses the ... technique of reducing size of polygon.[/list]
[list][b]RdlMapFile[/b] Select By Key dialog lets user specify polygon keys to determine selection.  For example, the key USA could match polygons for the continental US, Alaska, Hawaii, ...[/list]
[list][b]Silverlight[/b]Adjust scrolling based on reports page height and width (was using fixed page size)[/list]
[list][b]designer [/b]New option to control showing the Rendering Wait dialog when previewing a report.   Default is on.[/list]
[list][b]CustomReportItem Performance [/b] Added Aulofee performance improvement to RdlEngineConfig.[/list]
[list][b]RdlViewer Performance [/b] Autofee change to RdlViewer Rebuild() method.[/list] 
[list][b]Expressions [/b] vb function DateAdd support[/list]
[list][b]bug [/b] CustomReportItem doesn't render correctly in pdf[/list]
[list][b]bug [/b] Designer Width, PageWidth properties don't update the scrollable region of the design surface[/list]
[list][b]designer [/b]Improved error message when CustomReportItem implementation of GetCustomReportItemXml is incorrect.  See http://fyireporting.com/forum/viewtopic.php?p=2522#2522[/list]
[list][b]bug [/b]CustomReportItem applying changes even when Cancel button hit.  http://fyireporting.com/forum/viewtopic.php?p=2521#2521[/list]
[list][b]bug [/b]=Count() > 0   causes index out of bounds during parse.[/list]
[list][b]bug [/b]scrolling via down arrow (change from scroll via mousewheel); when zoomed doesn't scroll to end[/list]
[list][b]bug [/b]Fixes mht renderer.  See http://fyireporting.com/forum/viewtopic.php?p=2523[/list]
[list][b]bug [/b]Exception when closing RdlViewer window when height or width of window < 0 see http://fyireporting.com/forum/viewtopic.php?t=820[/list]
-2017-
