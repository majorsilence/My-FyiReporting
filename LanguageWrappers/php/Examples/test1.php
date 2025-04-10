<?php

// HOWTO run from command line:
// php.exe -f test1.php
// php.exe -f test1.php > test1.log 2>&1
// php.exe -f "C:\PHP test1.php" -- -arg1 -arg2 -arg3

error_reporting(E_ALL);
ini_set('display_errors', '1');

date_default_timezone_set('America/Los_Angeles');

require_once("../report.php");

# SETUP
$current_directory = dirname(__FILE__);
$base_directory = realpath($current_directory . '/../../../');

$db_path = realpath($current_directory . '/../../../Examples/northwindEF.db');
$report_path = realpath($current_directory . '/../../../Examples/SqliteExamples/SimpleTest1.rdl');

if (strtoupper(substr(PHP_OS, 0, 3)) === 'WIN') {
    // If self-hosted or on Windows, we do not need to set the path to dotnet, rdlcmd can be run directly
    $path_to_dotnet = null;
    $path_to_rdlcmd = realpath($base_directory . '/RdlCmd/bin/Release/net8.0/win-x64/publish/RdlCmd.exe');
} else {
    // dotnet is required to run rdlcmd
    // if a self contained build is used, the path to dotnet is not needed and the call should be to RdlCmd instead of RdlCmd.dll directly
    // if a self contained build is not used, the path to dotnet is needed
    $path_to_dotnet = 'dotnet';
    $path_to_rdlcmd = realpath($base_directory . '/RdlCmd/bin/Debug/net8.0/RdlCmd.dll');
}

$output_directory = $current_directory . '/output';
if (!file_exists($output_directory)) {
    mkdir($output_directory, 0777, true);
}


# EXAMPLE REPORT

$rpt = new MajorsilenceReporting\Report($report_path, $path_to_rdlcmd, $path_to_dotnet);
$rpt->set_connection_string('Data Source=' . $db_path);
$rpt->export("pdf", $output_directory . '/test1.pdf');


?>