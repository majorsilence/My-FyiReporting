import sys
sys.path.append("..")
import report

rpt = report.Report('C:/Users/peter/Desktop/My-FyiReporting-master/Examples/SqliteExamples/SimpleTestConnectionString.rdl')
rpt.set_parameter("ConnectionString", 'Data Source=C:/Users/peter/Desktop/My-FyiReporting-master/Examples/northwindEF.db;Version=3;Pooling=True;Max Pool Size=100;')
rpt.export("pdf", "C:/Users/peter/Desktop/test/hello3.pdf")

