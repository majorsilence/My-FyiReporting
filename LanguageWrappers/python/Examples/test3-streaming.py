#! /usr/bin/env python

import sys
sys.path.append("..")
import report
import os
import platform

# SETUP
current_directory = os.path.dirname(os.path.abspath(__file__))
base_directory = os.path.join(current_directory, '..', '..', '..')
base_directory = os.path.abspath(base_directory)
db_path = os.path.join(current_directory, '..', '..', '..', 'Examples', 'northwindEF.db')
db_path = os.path.abspath(db_path)

report_path = os.path.join(current_directory, '..', '..', '..', 'Examples', 'SqliteExamples', 'SimpleTest1.rdl')

if platform.system() == 'Windows':
    # if self hosted or on windows we do not need to set the path to dotnet, rdlcmd can be run directly
    path_to_dotnet = None
    path_to_rdlcmd = os.path.join(base_directory, "RdlCmd\\bin\\Debug\\net8.0\\RdlCmd.exe") 
else:
    # dotnet is required to run rdlcmd
    path_to_dotnet= "dotnet"
    path_to_rdlcmd = os.path.join(base_directory, "RdlCmd/bin/Debug/net8.0/RdlCmd.dll") 

path_to_rdlcmd = os.path.abspath(path_to_rdlcmd)

# REPORT EXAMPLE

rpt = report.Report(report_path, path_to_rdlcmd, path_to_dotnet)
rpt.set_connection_string('Data Source=' + db_path)
data = rpt.export_to_memory("pdf")

print(data)

# This is where you output data on your site using wsgi, cgi, or whatever python framework/library you are using
