import config
import tempfile
import os
import shutil
import subprocess
import platform

class Report :

	__report_path=""
	__parameters = {}
	
	def __init__(self, report_path):
		self.__report_path = report_path
	
	def set_parameter(self, name, value):
		'''
		Set a parameters values
		 name - string - the report parameter name
		 value - string - the value of the parameter
		'''
		
		self.__parameters.update({name:value})
		
	

	def export(self, type, export_path):
		'''
		 Export report to a file on the server
		 type - string - Export type "pdf", "csv", "xslx", "xml", "rtf", "tif", "html".  If type does not match it will default to PDF.
		 export_type - string - path on server to export file
		'''
		if (type != "pdf" and type != "csv" and type != "xslx" and type != "xml" and type != "rtf" and type != "tif" and type != "html"):
			type = "pdf"
		
		
		cmd = []
		if platform.system() == 'Windows':
			# if self hosted or on windows we do not need to set the path to mono, rdlcmd can be run directly
			cmd.append(config.path_to_rdlcmd)
		else:
			# mono is required to run rdlcmd
			cmd.append(config.path_to_dotnet)
			cmd.append(config.path_to_rdlcmd)
		
		
		fd, temp_name = tempfile.mkstemp()
		os.close(fd)
		temp_folder = os.path.dirname(temp_name)
		shutil.copyfile(self.__report_path, temp_name)
		
		#add path to rdl file
		rdl_path = "/f" + temp_name
		
		# Add all parameters to report
		count=0
		print(self.__parameters)
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
		
		subprocess.call(cmd)

		temp_pdf = temp_folder + os.sep + os.path.basename(temp_name) + "." + type
		final_pdf = export_path
		#echo(cmd)
		shutil.copyfile(temp_pdf, final_pdf)
		
		os.remove(temp_name)
		os.remove(temp_pdf)
	


	def export_to_memory(self, type):
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
	
	

