import sys
sys.path.append("..")
import report


rpt = report.Report('C:/Users/peter/Desktop/My-FyiReporting-master/Examples/SqliteExamples/SimpleTest1.rdl')
rpt.export("pdf", "C:/Users/peter/Desktop/test/hello2.pdf")


