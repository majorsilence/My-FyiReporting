import sys
sys.path.append("..")
import report
import os

current_directory = os.path.dirname(os.path.abspath(__file__))

db_path = os.path.join(current_directory, '..', '..', '..', 'Examples', 'northwindEF.db')
report_path = os.path.join(current_directory, '..', '..', '..', 'Examples', 'SqliteExamples', 'SimpleTestConnectionString.rdl')
rpt = report.Report(report_path)
rpt.set_parameter("ConnectionString", 'Data Source=' + db_path)
rpt.export("pdf", os.path.join(current_directory, 'output', 'SimpleTestConnectionString.pdf'))

