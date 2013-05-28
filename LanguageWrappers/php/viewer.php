<?php
namespace MyFyiReporting;

require_once("report.php");

class Viewer {

	private $rpt;
	
	/**
	* Pass in Report object that will be viewed
	*/
	public function __construct($rpt){
	
		$this->rpt = $rpt;
	}

	/**
	* Display a pdf.js viewer with the report
	*/
	public function show(){
	
	}
	
	
}



?>
