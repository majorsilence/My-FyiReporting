copy "C:\fyiReporting\RDL Project 41\src\RdlAsp\bin\Debug\*.*" "C:\fyiReporting\RDL Project 41\src\ReportServer\bin\*.*" 
copy "C:\fyiReporting\RDL Project 41\src\RdlEngine\bin\Debug\*.*" "C:\fyiReporting\RDL Project 41\src\ReportServer\bin\*.*" 
copy "C:\fyiReporting\RDL Project 41\src\RdlEngine\RdlEngineConfig.xml" "C:\fyiReporting\RDL Project 41\src\ReportServer\bin\*.*" 
copy "C:\fyiReporting\RDL Project 41\src\RdlEngine\usa_map.xml" "C:\fyiReporting\RDL Project 41\src\ReportServer\bin\*.*" 
copy "C:\fyiReporting\RDL Project 41\src\RdlEngine\world_map.xml" "C:\fyiReporting\RDL Project 41\src\ReportServer\bin\*.*" 

"C:\Windows\Microsoft.NET\Framework\v2.0.50727\WebDev.WebServer.EXE" /port:8080 /path:"C:\fyiReporting\RDL Project 41\src\ReportServer"