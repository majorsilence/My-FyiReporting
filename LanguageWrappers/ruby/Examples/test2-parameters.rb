#! /usr/bin/env ruby
#sys.path.append("..")
$LOAD_PATH << '../'
require 'report'

current_directory = File.dirname(File.expand_path(__FILE__))
db_path = File.expand_path(File.join(current_directory, '..', '..', '..', 'Examples', 'northwindEF.db'))
report_path = File.expand_path(File.join(current_directory, '..', '..', '..', 'Examples', 'SqliteExamples', 'SimpleTest3WithParameters.rdl'))
rpt = Report.new
rpt.set_rdl_path(report_path)
rpt.set_parameter("TestParam1", 'I am a parameter value.')
rpt.set_parameter("TestParam2", 'The second parameter.')
rpt.set_connection_string('Data Source=' + db_path)

output_directory = File.join(current_directory, 'output')
Dir.mkdir(output_directory) unless Dir.exist?(output_directory)
rpt.export("pdf", File.join(output_directory, 'test2-parameters.pdf'))
