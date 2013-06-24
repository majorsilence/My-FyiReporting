require 'config'
require 'tempfile'

class Report


	def set_rdl_path(report_path)
		@report_path = report_path
		@parameters = {}
		
	end
	
	def set_parameter(name, value)
		#Set a parameters values
		# name - string - the report parameter name
		# value - string - the value of the parameter
		
		@parameters[name] = value
		
	end

	def export(type, export_path)
		# Export report to a file on the server
		# type - string - Export type "pdf", "csv", "xslx", "xml", "rtf", "tif", "html".  If type does not match it will default to PDF.
		# export_type - string - path on server to export file

		if (type != "pdf" and type != "csv" and type != "xslx" and type != "xml" and type != "rtf" and type != "tif" and type != "html")
			type = "pdf"
		end
		
		
		cmd = []
		if($self_hosting_rdlcmd == true or $is_running_on_windows == true)
			# if self hosted or on windows we do not need to set the path to mono, rdlcmd can be run directly
			cmd.push($path_to_rdlcmd)
		else
			# mono is required to run rdlcmd
			cmd.push($path_to_mono)
			cmd.push($path_to_rdlcmd)
		end
		
		temp_file_to_close = Tempfile.new('maj')
		temp_name = temp_file_to_close.path
		temp_file_to_close.close
		temp_folder = File.dirname(temp_name)
		FileUtils.cp(@report_path, temp_name)
		
		#add path to rdl file
		rdl_path = "/f" + temp_name
		
		# Add all parameters to report
		count=0
		
		@parameters.each do |key, value|
			if (count == 0)
				rdl_path = rdl_path + '?' + key + '=' + value
			else
				rdl_path = rdl_path + '&' + key + '=' + value
			end
		
			count = count + 1
		end
		
		cmd.push(rdl_path)
		
		# set the export type
		cmd.push('/t' + type)
		
		#set the folder that the file will be exported
		cmd.push('/o' + temp_folder)
		
		IO.popen(cmd) do |io|
			 # wait until process finished
			io.readlines
		end 
		
		temp_pdf = temp_folder + File::SEPARATOR + File.basename(temp_name) + "." + type
		final_pdf = export_path
		#echo(cmd)
		FileUtils.cp(temp_pdf, final_pdf)
		
		File.delete(temp_name)
		File.delete(temp_pdf)
	
	end


	
end

