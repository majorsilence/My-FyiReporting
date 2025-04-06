require 'tempfile'

class Report

	def initialize(report_path, rdl_cmd_path, path_to_dotnet = nil)
		@@report_path = report_path
		@@path_to_rdlcmd = rdl_cmd_path
		@@path_to_dotnet = path_to_dotnet
		@@parameters = {}
	end
	
	def set_parameter(name, value)
		#Set a parameters values
		# name - string - the report parameter name
		# value - string - the value of the parameter
		
		@@parameters[name] = value
		
	end

	def set_connection_string(connection_string)
		#Set the connection string for the report
		# connection_string - string - the connection string to use for the report
		
		@@connection_string = connection_string
	end

	def export(type, export_path)
		# Export report to a file on the server
		# type - string - Export type "pdf", "csv", "xslx", "xml", "rtf", "tif", "html".  If type does not match it will default to PDF.
		# export_type - string - path on server to export file

		if (type != "pdf" and type != "csv" and type != "xslx" and type != "xml" and type != "rtf" and type != "tif" and type != "html")
			type = "pdf"
		end
			
		cmd = []
		if @@path_to_dotnet
			cmd.push(@@path_to_dotnet)
		end

		cmd.push(@@path_to_rdlcmd)
		
		temp_file_to_close = Tempfile.new('maj')
		temp_name = temp_file_to_close.path
		temp_file_to_close.close
		temp_folder = File.dirname(temp_name)
		FileUtils.cp(@@report_path, temp_name)
		
		#add path to rdl file
		rdl_path = "/f" + temp_name
		
		# Add all parameters to report
		count=0
		
		@@parameters.each do |key, value|
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

		if @@connection_string
			cmd.push('/c' + @@connection_string)
		end

		#puts cmd.join(" ")
		
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


	def export_to_memory(type)
		# Export report to memory for direct display on page
		# type - string - Export type "pdf", "csv", "xslx", "xml", "rtf", "tif", "html". If type does not match it will default to PDF.

		if !["pdf", "csv", "xslx", "xml", "rtf", "tif", "html"].include?(type)
			type = "pdf"
		end

		temp_file = Tempfile.new('export')
		temp_name = temp_file.path
		temp_file.close
		temp_file.unlink

		export(type, temp_name)

		data = nil
		if ["pdf", "tif", "rtf", "xslx"].include?(type)
			File.open(temp_name, 'rb') do |binary_file|
				data = binary_file.read
			end
		else
			File.open(temp_name, 'r') do |text_file|
				data = text_file.read
			end
		end

		File.delete(temp_name) if File.exist?(temp_name)

		data
	end
	
end

