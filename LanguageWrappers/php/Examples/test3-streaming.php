<?php

error_reporting(E_ALL);
ini_set('display_errors', '1');

date_default_timezone_set('America/Los_Angeles');

require_once("../report.php");


$rpt = new MyFyiReporting\Report("C:\Users\peter\Desktop\My-FyiReporting-master\Examples\SqliteExamples\SimpleTest1.rdl");
$data = $rpt->export_to_memory("pdf");


header("Content-type: application/octet-stream"); 
header("Content-disposition: attachment; filename=YourFileName2.pdf");
header('Expires: 0');
header('Cache-Control: must-revalidate, post-check=0, pre-check=0');
ob_clean();
flush();

echo $data;




?>