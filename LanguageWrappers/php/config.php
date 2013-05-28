<?php
namespace MyFyiReporting;

error_reporting(E_ALL);

$self_hosting_rdlcmd = false;

// Should be the full path to RdlCmd.exe
// for example it could be /home/yourname/RdlCmd.exe or if self hosting /home/yourname/RdlCmd
$path_to_rdl = "RdlCmd.exe";  

// This is the full path to mono
// Is only used if $self_hosting_rdlcmd and $is_running_on_windows are both false
$path_to_mono = "mono";

// If running on windows you can set this to true and as long as .net 4 is installed it should work.
// If this is true $path_to_mono is not used
$is_running_on_windows = false;



?>
