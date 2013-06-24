import sys
sys.path.append("..")
import report


rpt =  report.Report("C:/Users/peter/Desktop/My-FyiReporting-master/Examples/SqliteExamples/SimpleTest1.rdl")
data = rpt.export_to_memory("pdf")

print data

# This is where you output data on your site using wsgi, cgi, or whatever python framework/library you are using

