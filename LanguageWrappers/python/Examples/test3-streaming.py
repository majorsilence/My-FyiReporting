#! /usr/bin/env python

import sys
sys.path.append("..")
import report
import os

current_directory = os.path.dirname(os.path.abspath(__file__))

report_path = os.path.join(current_directory, '..', '..', '..', 'Examples', 'SqliteExamples', 'SimpleTest1.rdl')
rpt = report.Report(report_path)
data = rpt.export_to_memory("pdf")

print(data)

# This is where you output data on your site using wsgi, cgi, or whatever python framework/library you are using
