import tempfile
import os
import shutil
import subprocess



class Report :
	"""
	Report Class
	This class provides functionality to generate and export reports using the RDL (Report Definition Language) command-line tool. 
	It supports exporting reports in various formats such as PDF, CSV, XLSX, XML, RTF, TIF, and HTML.
	Usage:
	1. Copy and paste the `report.py` file into your Python project.
	2. Import the `Report` class into your script:
		```python
		import report
		```
	3. Create an instance of the `Report` class:
		```python
		rpt = report.Report(report_path="path_to_report.rdl", rdl_cmd_path="path_to_rdl_cmd.exe", path_to_dotnet="path_to_dotnet_executable")
		```
		- `report_path`: Path to the RDL file.
		- `rdl_cmd_path`: Path to the RDL command-line executable.
		- `path_to_dotnet`: (Optional) Path to the .NET executable, if required.
	4. Set report parameters (if any):
		```python
		rpt.set_parameter("ParameterName", "ParameterValue")
		```
	5. Set the connection string (if required):
		```python
		rpt.set_connection_string("your_connection_string")
		```
	6. Export the report to a file:
		```python
		rpt.export(type="pdf", export_path="output_path.pdf")
		```
		- `type`: The export format. Supported formats are "pdf", "csv", "xslx", "xml", "rtf", "tif", "html". Defaults to "pdf".
		- `export_path`: The path where the exported file will be saved.
	7. Export the report to memory (for direct display):
		```python
		data = rpt.export_to_memory(type="pdf")
		```
		- `type`: The export format. Supported formats are "pdf", "csv", "xslx", "xml", "rtf", "tif", "html". Defaults to "pdf".
		- Returns the exported report data as a string or binary, depending on the format.
	"""
	__report_path=""
	__parameters = {}
	__connection_string = None
	__rdl_cmd_path=""
	__path_to_dotnet = None
	
	def __init__(self, report_path: str, rdl_cmd_path: str, path_to_dotnet :str =None):
		self.__report_path = report_path
		self.__rdl_cmd_path = rdl_cmd_path
		self.__path_to_dotnet = path_to_dotnet
	
	def set_parameter(self, name, value):
		'''
		Set a parameters values
		 name - string - the report parameter name
		 value - string - the value of the parameter
		'''
		
		self.__parameters.update({name:value})
		
	def set_connection_string(self, connection_string: str):
		'''
		Set the connection string for the report
		 connection_string - string - the connection string to use for the report
		'''
		
		self.__connection_string = connection_string


	def export(self, type : str, export_path : str):
		'''
		 Export report to a file on the server
		 type - string - Export type "pdf", "csv", "xslx", "xml", "rtf", "tif", "html".  If type does not match it will default to PDF.
		 export_type - string - path on server to export file
		'''
		if (type != "pdf" and type != "csv" and type != "xslx" and type != "xml" and type != "rtf" and type != "tif" and type != "html"):
			type = "pdf"
			
		cmd = []
		if (self.__path_to_dotnet != None):
			cmd.append(self.__path_to_dotnet)

		cmd.append(self.__rdl_cmd_path)
	
		fd, temp_name = tempfile.mkstemp()
		os.close(fd)
		temp_folder = os.path.dirname(temp_name)
		shutil.copyfile(self.__report_path, temp_name)
		
		#add path to rdl file
		rdl_path = "/f" + temp_name
		
		# Add all parameters to report
		count=0
		for key in self.__parameters:
			if (count == 0):
				rdl_path = rdl_path + '?' + key + '=' + self.__parameters[key]
			else:
				rdl_path = rdl_path + '&' + key + '=' + self.__parameters[key]
		
			count = count + 1
		
		cmd.append(rdl_path)
		
		# set the export type
		cmd.append('/t' + type)
		
		#set the folder that the file will be exported
		cmd.append('/o' + temp_folder)

		if self.__connection_string:
			cmd.append('/c' + self.__connection_string+'')
		
		#print("Executing command:", cmd)

		subprocess.run(cmd)

		temp_pdf = temp_folder + os.sep + os.path.basename(temp_name) + "." + type
		final_pdf = export_path

		shutil.copyfile(temp_pdf, final_pdf)
		os.remove(temp_name)
		os.remove(temp_pdf)
	


	def export_to_memory(self, type : str) -> bytes | str :
		'''
		Export report to memory for direct display on page
		type - string - Export type "pdf", "csv", "xslx", "xml", "rtf", "tif", "html".  If type does not match it will default to PDF.
		'''
		if (type != "pdf" and type != "csv" and type != "xslx" and type != "xml" and type != "rtf" and type != "tif" and type != "html"):
			type = "pdf"
		
		fd, temp_name = tempfile.mkstemp()
		os.close(fd);
		self.export(type, temp_name)
		if type == "pdf" or type == "tif" or type == "rtf" or type == "xslx":
			with open(temp_name, 'rb') as binary_file:
				data = binary_file.read()
		else:
			with open(temp_name, 'r+') as text_file:
				data = text_file.read()

		os.remove(temp_name)
	
		return data
	
	

