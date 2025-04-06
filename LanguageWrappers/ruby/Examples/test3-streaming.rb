#!/usr/bin/env ruby

$LOAD_PATH << '../'
require 'report'

# SETUP
current_directory = File.dirname(File.expand_path(__FILE__))
db_path = File.expand_path(File.join(current_directory, '..', '..', '..', 'Examples', 'northwindEF.db'))
report_path = File.expand_path(File.join(current_directory, '..', '..', '..', 'Examples', 'SqliteExamples', 'SimpleTest1.rdl'))

if Gem.win_platform?
    # If on Windows, we do not need to set the path to dotnet, RdlCmd can be run directly
    path_to_dotnet = nil
    path_to_rdlcmd = File.expand_path(File.join(current_directory, '..', '..', '..', 'RdlCmd', 'bin', 'Debug', 'net8.0', 'RdlCmd.exe'))
else
    # dotnet is required to run RdlCmd
    path_to_dotnet = 'dotnet'
    path_to_rdlcmd = File.expand_path(File.join(current_directory, '..', '..', '..', 'RdlCmd', 'bin', 'Debug', 'net8.0', 'RdlCmd.dll'))
end

output_directory = File.join(current_directory, 'output')
Dir.mkdir(output_directory) unless Dir.exist?(output_directory)

# REPORT EXAMPLE

rpt = Report.new(report_path, path_to_rdlcmd, path_to_dotnet)
rpt.set_connection_string('Data Source=' + db_path)
data = rpt.export_to_memory("pdf")

# show the data in the console
print(data)

# or save it to a file
File.open(File.join(output_directory, 'test3-streaming.pdf'), 'wb') do |file| 
  file.write(data)
end



# This is where you output data on your site using wsgi, cgi, or whatever python framework/library you are using
