#! /usr/bin/env python

import sys
sys.path.append("..")
import report
import os

current_directory = os.path.dirname(os.path.abspath(__file__))
db_path = os.path.join(current_directory, '..', '..', '..', 'Examples', 'northwindEF.db')
db_path = os.path.abspath(db_path)
report_path = os.path.join(current_directory, '..', '..', '..', 'Examples', 'SqliteExamples', 'SimpleTest1.rdl')
report_path = os.path.abspath(report_path)
rpt = report.Report(report_path)

output_directory = os.path.join(current_directory, 'output')
if not os.path.exists(output_directory):
    os.makedirs(output_directory)

rpt.set_connection_string('Data Source=' + db_path)
rpt.export("pdf", os.path.join(output_directory, 'test1.pdf'))
