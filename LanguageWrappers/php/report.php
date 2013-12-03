<?php
namespace MyFyiReporting;
require_once("config.php");


class Report {

	private $report_path="";
	private $parameters = array();
	private $rdlcmd_dir = "";
	
	public function __construct($report_path){
		$this->report_path = $report_path;
		
		global $path_to_rdlcmd;
		
		if (file_exists($path_to_rdlcmd) == false)
		{
			throw new \Exception("RdlCmd.exe not found.  Set path to RdlCmd.exe in config.php");
		}
		
		$this->rdlcmd_dir = dirname($path_to_rdlcmd);
	}

	/**
	* Set a parameters values
	* @param string $name - the report parameter name
	* @param string $value - the value of the parameter
	*/
	public function set_parameter($name, $value){
	
		$this->parameters[$name] = $value;
	}
	
	/**
	* Export report to a file on the server
	* @param string $type - Export type "pdf", "csv", "xslx", "xml", "rtf", "tif", "html".  If type does not match it will default to PDF.
	* @param string $export_type - path on server to export file
	*/
	public function export($type, $export_path){
		if ($type != "pdf" && $type != "csv" && $type != "xslx" && $type != "xml" && $type != "rtf" && $type != "tif" && $type != "html"){
			$type = "pdf";
		}
		
	
		global $self_hosting_rdlcmd, $path_to_rdlcmd, $path_to_mono, $is_running_on_windows, $override_tmp_folder;
		
		
		$cmd = "";
		if($self_hosting_rdlcmd == true || $is_running_on_windows == true){
			// if self hosted or on windows we do not need to set the path to mono, rdlcmd can be run directly
			$cmd = '"' . $path_to_rdlcmd . '" ';
		}
		else{
			// mono is required to run rdlcmd
			$cmd = '"' . $path_to_mono . '" "' . $path_to_rdlcmd . '" ';
		}
		
		$temp_folder = "";
		if ($override_tmp_folder == "")
		{
			$temp_folder = sys_get_temp_dir();
		}
		else
		{
			$temp_folder = $override_tmp_folder;
		}
			
		$temp_name = tempnam($temp_folder, "majorsilencereporting");
		copy($this->report_path, $temp_name);
		
		// add path to rdl file
		$cmd = $cmd . '"/f' . $temp_name . '';
		
		// Add all parameters to report
		$count=0;
		foreach($this->parameters as $key => $value){
			if ($count == 0){
				$cmd = $cmd . '?' . $key . '=' . $value;
			}
			else {
				$cmd = $cmd . '&' . $key . '=' . $value;
			}
		
			$count = $count + 1;
		}
		$cmd = $cmd . '" ';
	
		
		// set the export type
		$cmd = $cmd . '"/t' . $type . '" ';
		
		//set the folder that the file will be exported
		$cmd = $cmd . '"/o' . $temp_folder . '" ';
		
		$cdir = getcwd();
		chdir ($this->rdlcmd_dir);
		$shell_output = shell_exec($cmd);
		chdir ($cdir);

		$temp_pdf = $temp_folder;
		
		if ($this->endsWith($temp_pdf, DIRECTORY_SEPARATOR) == false)
		{
			$temp_pdf = $temp_pdf . DIRECTORY_SEPARATOR;
		}
		$temp_pdf = $temp_pdf . basename($temp_name, ".tmp") . "." . $type;
		$final_pdf = $export_path;
		//echo($cmd);
		copy($temp_pdf, $final_pdf);
		unlink($temp_name);
		unlink($temp_pdf);
	}

	/**
	* Export report to memory for direct display on page
	* @param string $type - Export type "pdf", "csv", "xslx", "xml", "rtf", "tif", "html".  If type does not match it will default to PDF.
	*/
	public function export_to_memory($type){
		if ($type != "pdf" && $type != "csv" && $type != "xslx" && $type != "xml" && $type != "rtf" && $type != "tif" && $type != "html"){
			$type = "pdf";
		}
	
		$temp_folder = "";
		if ($override_tmp_folder == "")
		{
			$temp_folder = sys_get_temp_dir();
		}
		else
		{
			$temp_folder = $override_tmp_folder;
		}
		$temp_name = tempnam($temp_folder, "majorsilencereporting");
	
		$this->export($type, $temp_name);
		$data = file_get_contents($temp_name);
	
		unlink($temp_name);
	
		return $data;
	}
	
	private function endsWith($haystack, $needle)
	{
		return $needle === "" || substr($haystack, -strlen($needle)) === $needle;
	}
	
}




?>



