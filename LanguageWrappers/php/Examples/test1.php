<?php

error_reporting(E_ALL);
ini_set('display_errors', '1');

date_default_timezone_set('America/Los_Angeles');

require_once("../report.php");


$rpt = new MyFyiReporting\Report("C:\Users\peter\Desktop\My-FyiReporting-master\Examples\SqliteExamples\SimpleTest1.rdl");
$rpt->export("pdf", "C:\\Users\\peter\\Desktop\\test\hello2.pdf");


?>