<?php

error_reporting(E_ALL);
ini_set('display_errors', '1');

date_default_timezone_set('America/Los_Angeles');

require_once("../report.php");


$rpt = new MyFyiReporting\Report('C:\Users\peter\Desktop\My-FyiReporting-master\Examples\SqliteExamples\SimpleTestConnectionString.rdl');
$rpt->set_parameter("ConnectionString", 'Data Source=C:\Users\peter\Desktop\My-FyiReporting-master\Examples\northwindEF.db;Version=3;Pooling=True;Max Pool Size=100;');
$rpt->export("pdf", "C:\\Users\\peter\\Desktop\\test\hello3.pdf");


?>