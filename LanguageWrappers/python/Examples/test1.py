import sys
sys.path.append("..")
import report
import os

current_directory = os.path.dirname(os.path.abspath(__file__))
report_path = os.path.join(current_directory, '..', '..', '..', 'Examples', 'SqliteExamples', 'SimpleTest1.rdl')
report_path = os.path.abspath(report_path)
rpt = report.Report(report_path)
rpt.export("pdf", os.path.join(current_directory, 'output', 'SimpleTest1.pdf'))
