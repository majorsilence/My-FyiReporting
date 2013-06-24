#sys.path.append("..")
$LOAD_PATH << '../'
require 'report'

rpt = Report.new
rpt.set_rdl_path('C:/Users/pgill/Desktop/My-FyiReporting-master/Examples/SqliteExamples/SimpleTestConnectionString.rdl')
rpt.set_parameter("ConnectionString", 'Data Source=C:/Users/pgill/Desktop/My-FyiReporting-master/Examples/northwindEF.db;Version=3;Pooling=True;Max Pool Size=100;')
rpt.export("pdf", "C:/Users/pgill/Desktop/test/hello3.pdf")


