require 'rake'

task :build do
	sh "powershell psake.ps1"
end

task :default => :build
