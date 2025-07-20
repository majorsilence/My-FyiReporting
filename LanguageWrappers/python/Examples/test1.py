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
report_path = os.path.abspath(report_path)

if platform.system() == 'Windows':
    # if self hosted or on windows we do not need to set the path to dotnet, rdlcmd can be run directly
    path_to_dotnet = None
    path_to_rdlcmd = os.path.join(base_directory, "RdlCmd/bin/Release/net8.0/win-x64/publish/RdlCmd.exe") 
else:
    # dotnet is required to run rdlcmd
    # if a self contained build is used, the path to dotnet is not needed and the call should be to RdlCmd instead of RdlCmd.dll directly
    # if a self contained build is not used, the path to dotnet is needed
    path_to_dotnet= "dotnet"
    path_to_rdlcmd = os.path.join(base_directory, "RdlCmd/bin/Debug/net8.0/RdlCmd.dll") 

path_to_rdlcmd = os.path.abspath(path_to_rdlcmd)

output_directory = os.path.join(current_directory, 'output')
if not os.path.exists(output_directory):
    os.makedirs(output_directory)

# REPORT EXAMPLE
rpt = report.Report(report_path, path_to_rdlcmd, path_to_dotnet)
rpt.set_connection_string('Data Source=' + db_path)
rpt.export("pdf", os.path.join(output_directory, 'test1.pdf'))
