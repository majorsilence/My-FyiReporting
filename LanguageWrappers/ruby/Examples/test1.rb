#sys.path.append("..")
$LOAD_PATH << '../'
require 'report'

rpt = Report.new
rpt.set_rdl_path('C:/Users/pgill/Desktop/My-FyiReporting-master/Examples/SqliteExamples/SimpleTest1.rdl')
rpt.export("pdf", "C:/Users/pgill/Desktop/test/hello2.pdf")

