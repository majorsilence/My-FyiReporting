<?php
namespace MyFyiReporting;
require_once("config.php");


class Report {

	private $report_path="";
	private $parameters = array("");
	
	public function __construct($report_path){
		$this->report_path = $report_path;
	}

	/**
	* Set a parameters values
	* @param string $name - the report parameter name
	* @param string $value - the value of the parameter
	*/
	public function set_parameter($name, $value){
	
	}
	
	/**
	* Export report to a file on the server
	* @param string $type - Export type "PDF", "CSV", "EXCEL".  If type does not match it will default to PDF.
	* @param string $export_type - path on server to export file
	*/
	public function export($type, $export_path){
	
	
	
	
		//shell_exec();
	}

	/**
	* Export report to memory for direct display on page
	* @param string $type - Export type "PDF", "CSV", "EXCEL".  If type does not match it will default to PDF.
	*/
	public function export_to_memory($type){
	
		//shell_exec();
	}
	
	
}




?>



