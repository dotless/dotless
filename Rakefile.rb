require 'rake'

task :build do
	sh "powershell .\src\packages\psake.4.3.2\tools\psake.ps1 .\default.ps1"
end

task :default => :build
